using FastFoodServer.FastFood.Games;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Data
{
    public class FastFoodMissile
    {
        public const int ExplodeAtMS = 750;

        private GameLobby GameLobby;
        private FastFoodUserPlate Target;
        private Stopwatch LifeTime;

        public FastFoodMissile(GameLobby game, FastFoodUserPlate target)
        {
            this.GameLobby = game;
            this.Target = target;
            
            this.LifeTime = Stopwatch.StartNew();
        }

        public void Cycle(double timePast)
        {
            if (this.LifeTime.ElapsedMilliseconds >= FastFoodMissile.ExplodeAtMS)
            {
                this.GameLobby.TryExplodePlate(this, this.Target);
            }
        }
    }
}
