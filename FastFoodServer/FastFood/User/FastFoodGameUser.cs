using FastFoodServer.FastFood.Data;
using FastFoodServer.FastFood.Games;
using FastFoodServer.Net.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.User
{
    public class FastFoodGameUser
    {
        private const int MaxPowerups = 3;

        public GameLobby GameLobby { get; private set; }
        public FastFoodUser User;
        public int GameUserID { get; private set; }
        public int Index { get; private set; }

        public int Stars { get; set; }
        public int NextPlateId { get; set; }
        public FastFoodUserPlate Plate { get; set; }
        public Stopwatch PlateTimer { get; set; }

        public int UsedParachutes { get; set; }
        public int UsedMissiles { get; set; }
        public int UsedShields { get; set; }

        public FastFoodGameUser(GameLobby gameLobby, FastFoodUser user, int gameUserId, int index)
        {
            this.GameLobby = gameLobby;
            this.User = user;
            this.GameUserID = gameUserId;
            this.Index = index;

            this.PlateTimer = null;
        }

        public void TryReleaseFood()
        {
            if (this.Plate == null)
            {
                this.Plate = this.GameLobby.ReleaseFood(this);
            }
        }

        public void PrepareNextPlate(bool progress)
        {
            this.Plate = null;

            if (progress)
            {
                this.NextPlateId++;
                this.Stars++;

                if (this.Stars >= 6)
                {
                    this.GameLobby.EndGame(this.GameUserID);
                }
            }
        }

        public void OpenParachute()
        {
            this.Plate?.OpenParachute();
        }

        public void OpenBigParachute()
        {
            if (this.User.Parachutes > 0 && this.UsedParachutes < FastFoodGameUser.MaxPowerups && (this.Plate?.OpenBigParachute() ?? false))
            {
                this.UsedParachutes++;
                this.User.Parachutes--;
            }
        }

        public void LaunchMissile()
        {
            if (this.User.Missiles > 0 && this.UsedMissiles < FastFoodGameUser.MaxPowerups && (this.GameLobby?.LaunchMissile(this.GameUserID) ?? false))
            {
                this.UsedMissiles++;
                this.User.Missiles--;
            }
        }

        public void ActivateShield()
        {
            if (this.User.Shields > 0 && this.UsedShields < FastFoodGameUser.MaxPowerups && (this.Plate?.ActivateShield() ?? false))
            {
                this.UsedShields++;
                this.User.Shields--;
            }
        }
    }
}
