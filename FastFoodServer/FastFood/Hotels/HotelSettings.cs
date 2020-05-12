using FastFoodServer.FastFood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Hotels
{
    public class HotelSettings
    {
        public Dictionary<string, string> Texts { get; private set; }
        public List<GamePowerup> GamePowerups { get; private set; }

        public HotelSettings(Dictionary<string, string> texts = null, List<GamePowerup> gamePowerups = null)
        {
            this.Texts = texts;
            this.GamePowerups = gamePowerups;
        }
    }
}
