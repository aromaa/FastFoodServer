using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Data
{
    public class FastFoodGamePlate
    {
        public static readonly List<FastFoodGamePlate> Plates = new List<FastFoodGamePlate>()
        {
            new FastFoodGamePlate(0, 0.4, -2.0, 0.1, 0.2, 100),
            new FastFoodGamePlate(1, 0.5, -2.0, 0.1, 0.15, 150),
            new FastFoodGamePlate(2, 0.7, -1.2, 0.2, 0.2, 100),
            new FastFoodGamePlate(3, 0.9, -1.5, 0.2, 0.2, 200),
            new FastFoodGamePlate(4, 1.1, -1.5, 0.15, 0.15, 300),
            new FastFoodGamePlate(5, 1.5, -2.0, 0.15, 0.2, 200)
        };

        public int ID { get; private set; }
        public double FallMultiplayer { get; private set; }
        public double ParachuteMultiplayer { get; private set; }
        public double ParachuteSpeed { get; private set; }
        public double BigParachuteSpeed { get; private set; }
        public int PlateTimer { get; private set; }

        public FastFoodGamePlate(int id, double fallMultiplayer, double parachuteMultiplayer, double parachuteSpeed, double bigParachuteSpeed, int plateTimer)
        {
            this.ID = id;
            this.FallMultiplayer = fallMultiplayer;
            this.ParachuteMultiplayer = parachuteMultiplayer;
            this.ParachuteSpeed = parachuteSpeed;
            this.BigParachuteSpeed = bigParachuteSpeed;
            this.PlateTimer = plateTimer;
        }
    }
}
