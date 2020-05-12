using FastFoodServer.FastFood.Enums;
using FastFoodServer.FastFood.Games;
using FastFoodServer.FastFood.User;
using FastFoodServer.Net.Game.Outgoing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Data
{
    public class FastFoodUserPlate
    {
        public GameLobby GameLobby { get; private set; }
        public FastFoodGameUser User { get; private set; }
        public FastFoodGamePlate Plate { get; private set; }
        public GamePlateState State { get; private set; }

        public double Speed { get; private set; }
        public double Distance { get; private set; }
        public bool BigParachuteActive { get; private set; }
        public bool Failed { get; private set; }
        public bool Shield { get; private set; }

        public FastFoodUserPlate(GameLobby game, FastFoodGameUser user, FastFoodGamePlate plate, bool failed)
        {
            this.GameLobby = game;
            this.User = user;
            this.Plate = plate;
            this.Failed = failed;

            this.Speed = 0;
            this.Distance = 1;

            this.UpdateState(GamePlateState.Falling);
        }

        public void Cycle(double timePast)
        {
            if (this.State == GamePlateState.Falling || this.State == GamePlateState.Parachute)
            {
                this.Speed += (timePast / 1000d) * this.GetSpeedMultiplayer();
                if (this.State == GamePlateState.Parachute)
                {
                    this.Speed = Math.Max(this.Speed, this.GetParachuteSpeed());
                }

                this.Distance -= (timePast / 1000d) * this.Speed;
                if (this.Distance < 0)
                {
                    this.HitTableLogic();
                }
            }
        }

        public void HitTableLogic()
        {
            this.GameLobby.RemovePlate(this);

            if (this.User.PlateTimer == null)
            {
                this.User.PlateTimer = Stopwatch.StartNew();
            }

            if (this.Failed)
            {
                this.User.PlateTimer.Start();
            }
            else
            {
                this.User.PlateTimer.Restart();
            }
            
            if (!this.Failed && (this.Shield || this.Speed <= 0.5))
            {
                this.User.PrepareNextPlate(true);

                if (this.User.Stars != 6)
                {
                    this.UpdateState(GamePlateState.Done);
                }
            }
            else
            {
                this.User.PrepareNextPlate(false);
                this.UpdateState(GamePlateState.Failed);
            }
        }

        public double GetSpeedMultiplayer()
        {
            switch (this.State)
            {
                case GamePlateState.Falling:
                    return this.Plate.FallMultiplayer;
                case GamePlateState.Parachute:
                    return this.BigParachuteActive ? (this.Plate.ParachuteMultiplayer * 10) : this.Plate.ParachuteMultiplayer;
                default:
                    return 0;
            }
        }

        public double GetParachuteSpeed()
        {
            return this.BigParachuteActive ? this.Plate.BigParachuteSpeed : this.Plate.ParachuteSpeed;
        }

        public void UpdateState(GamePlateState state)
        {
            this.State = state;

            if (this.State == GamePlateState.Falling || this.State == GamePlateState.Parachute)
            {
                this.GameLobby.SendToAll(new UpdateStateOutgoingPacket(this.Plate.ID, this.User.GameUserID, this.Distance, this.Speed, this.State, this.Failed));
            }
            else
            {
                this.GameLobby.SendToAll(new FoodHitTableOutgoingPacket(this.User.Stars, this.User.GameUserID, this.State, this.User.NextPlateId));
            }
        }

        public bool OpenParachute()
        {
            if (!this.Failed && this.State == GamePlateState.Falling)
            {
                this.UpdateState(GamePlateState.Parachute);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OpenBigParachute()
        {
            if (this.OpenParachute())
            {
                this.GameLobby.SendToAll(new OpenBigParachuteOutgoingPacket(this.Plate.ID, this.User.GameUserID));

                this.BigParachuteActive = true;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActivateShield()
        {
            if (!this.Failed & !this.Shield)
            {
                this.SetShield(true);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetShield(bool activated)
        {
            this.GameLobby.SendToAll(new SetShieldOutgoingPacket(this.Plate.ID, this.User.GameUserID, activated));

            this.Shield = activated;
        }

        public bool TryExplodePlate()
        {
            if (this.Shield)
            {
                this.SetShield(false);

                return false;
            }
            else
            {
                if (this.User.PlateTimer == null)
                {
                    this.User.PlateTimer = Stopwatch.StartNew();
                }
                else
                {
                    this.User.PlateTimer.Restart();
                }

                this.User.PrepareNextPlate(false);
                this.UpdateState(GamePlateState.Exploded);

                return true;
            }
        }
    }
}
