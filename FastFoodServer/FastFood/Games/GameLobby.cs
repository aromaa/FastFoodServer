using FastFoodServer.FastFood.Enums;
using FastFoodServer.FastFood.User;
using FastFoodServer.Net.Game;
using FastFoodServer.Net.Game.Outgoing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastFoodServer.FastFood.Data;
using FastFoodServer.Net;
using System.Diagnostics;
using FastFoodServer.Core;
using FastFoodServer.Net.API.Outgoing;

namespace FastFoodServer.FastFood.Games
{
    public class GameLobby
    {
        private static int NextGameID;

        private const int MaxPlayersPerGame = 3;
        private const int MaxPowerups = 3;

        private ConcurrentDictionary<int, GameConnection> Players;
        private List<FastFoodGameUser> Participants;
        private List<FastFoodUserPlate> Plates;
        private List<FastFoodMissile> Missiles;

        public long ID { get; private set; }
        public GameStatus Status { get; private set; }

        public double Countdown;
        public Task CycleTask { get; set; }
        private Stopwatch LastCycle;

        public GameLobby(long id)
        {
            this.Players = new ConcurrentDictionary<int, GameConnection>(GameLobby.MaxPlayersPerGame, GameLobby.MaxPlayersPerGame);
            this.Participants = new List<FastFoodGameUser>(GameLobby.MaxPlayersPerGame);
            this.Plates = new List<FastFoodUserPlate>();
            this.Missiles = new List<FastFoodMissile>();

            this.ID = id;
            this.Status = GameStatus.WaitingForPlayers;
            this.Countdown = 3.0;

            this.LastCycle = Stopwatch.StartNew();
        }

        public bool TryJoin(GameConnection connection)
        {
            if (this.Status == GameStatus.WaitingForPlayers)
            {
                int count = this.Players.Count;
                if (count < GameLobby.MaxPlayersPerGame)
                {
                    if (this.Players.TryAdd(count, connection))
                    {
                        this.Participants.Add(connection.GameUser = new FastFoodGameUser(this, connection.User, Interlocked.Increment(ref GameLobby.NextGameID), count));

                        if (this.Players.Count == GameLobby.MaxPlayersPerGame)
                        {
                            this.StartGame();
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public void RemovePlate(FastFoodUserPlate fastFoodUserPlate)
        {
            this.Plates.Remove(fastFoodUserPlate);
        }

        public void Leave(GameConnection connection)
        {
            GameConnection connection_;
            if (this.Players.TryRemove(connection.GameUser.Index, out connection_))
            {
                connection_.GameUser = null;

                if (this.Status != GameStatus.WaitingForPlayers)
                {
                    if (this.Players.Count <= 0)
                    {
                        this.Status = GameStatus.Ended;

                        Server.GetGame().GetGameLobbyManager().EndGameLobby(this.ID);
                    }
                }
            }
        }

        private void StartGame()
        {
            if (this.Status == GameStatus.WaitingForPlayers)
            {
                this.Status = GameStatus.Start;

                List<FastFoodGameUser> users = this.Players.Values.Select(u => u.GameUser).ToList();
                foreach(GameConnection connection in this.Players.Values)
                {
                    Dictionary<int, int> powerups = new Dictionary<int, int>();
                    if (connection.User.Missiles > 0)
                    {
                        powerups.Add((int)GamePowerupType.Missile, Math.Min(connection.User.Missiles, GameLobby.MaxPowerups));
                    }

                    if (connection.User.Parachutes > 0)
                    {
                        powerups.Add((int)GamePowerupType.Parachute, Math.Min(connection.User.Parachutes, GameLobby.MaxPowerups));
                    }

                    if (connection.User.Shields > 0)
                    {
                        powerups.Add((int)GamePowerupType.Shield, Math.Min(connection.User.Shields, GameLobby.MaxPowerups));
                    }

                    connection.SendPacket(new GameStartingOutgoingPacket(connection.GameUser.GameUserID, powerups, users));
                }
            }
        }
        
        public FastFoodUserPlate ReleaseFood(FastFoodGameUser fastFoodGameUser)
        {
            if (this.Status == GameStatus.Running)
            {
                fastFoodGameUser.PlateTimer?.Stop();

                FastFoodUserPlate plate = new FastFoodUserPlate(this, fastFoodGameUser, FastFoodGamePlate.Plates[fastFoodGameUser.NextPlateId], (fastFoodGameUser.PlateTimer?.Elapsed.TotalMilliseconds) < (FastFoodGamePlate.Plates[fastFoodGameUser.NextPlateId].PlateTimer / 60d) * 1000d);
                this.Plates.Add(plate);
                return plate;
            }
            else
            {
                return null;
            }
        }

        public void SendToAll(OutgoingPacket outgoing)
        {
            foreach(GameConnection connection in this.Players.Values)
            {
                connection.SendPacket(outgoing);
            }
        }

        public void Cycle()
        {
            double timePast = this.LastCycle.Elapsed.TotalMilliseconds;
            this.LastCycle.Restart();
            
            if (this.Status == GameStatus.Start)
            {
                this.Countdown -= (timePast / 1000);
                if (this.Countdown <= 0)
                {
                    this.Status = GameStatus.Running;
                }
            }
            else if (this.Status == GameStatus.Running)
            {
                foreach(FastFoodUserPlate plate in this.Plates.ToList())
                {
                    plate.Cycle(timePast);
                }

                foreach(FastFoodMissile missile in this.Missiles.ToList())
                {
                    missile.Cycle(timePast);
                }
            }
            else if (this.Status == GameStatus.Ended)
            {
                Server.GetGame().GetGameLobbyManager().EndGameLobby(this.ID);
            }
        }

        public bool LaunchMissile(int gameUserId)
        {
            FastFoodUserPlate plate = this.Plates.Where(p => p.User?.GameUserID != gameUserId).OrderBy(p => p.Distance).FirstOrDefault();
            if (plate != null)
            {
                this.SendToAll(new LaunchMissileOutgoingPacket(plate.User.GameUserID, plate.Plate.ID, gameUserId));
                this.Missiles.Add(new FastFoodMissile(this, plate));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TryExplodePlate(FastFoodMissile missile, FastFoodUserPlate target)
        {
            this.Missiles.Remove(missile);
            
            if (target.TryExplodePlate())
            {
                this.RemovePlate(target);
            }
        }

        public void EndGame(int winerGamerUserId)
        {
            if (this.Status == GameStatus.Running)
            {
                this.Status = GameStatus.Ended;

                this.SendToAll(new GameEndOutgoingPacket(this.Players.Values.OrderByDescending(u => u.GameUser.Stars).ToDictionary(u => u.GameUser.GameUserID, u => u.GameUser.Stars)));

                foreach(GameConnection connection in this.Players.Values)
                {
                    connection.User.Hotel.APIConnection?.SendPacket(new FastFoodGameFinishedOutgoingPacket(connection.User.UserID, connection.GameUser.GameUserID == winerGamerUserId, connection.GameUser.UsedParachutes, connection.GameUser.UsedMissiles, connection.GameUser.UsedShields));

                    connection.LastGame = connection.GameUser;
                    connection.GameUser = null;

                    Dictionary<int, int> powerups = new Dictionary<int, int>();
                    if (connection.GameUser.UsedMissiles > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Missile, connection.User.Missiles);
                    }

                    if (connection.GameUser.UsedParachutes > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Parachute, connection.User.Parachutes);
                    }

                    if (connection.GameUser.UsedShields > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Shield, connection.User.Shields);
                    }

                    connection.SendPacket(new UserPowerupsOutgoingPacket(powerups));
                }

                Server.GetGame().GetGameLobbyManager().EndGameLobby(this.ID);
            }
        }

        public ICollection<GameConnection> GetPlayers()
        {
            return this.Players.Values;
        }

        public List<FastFoodGameUser> GetParticipants()
        {
            return this.Participants;
        }
    }
}
