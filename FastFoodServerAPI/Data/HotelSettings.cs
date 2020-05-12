using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Data
{
    public class HotelSettings
    {
        public Dictionary<string, string> Texts { get; set; }
        public List<GamePowerup> GamePowerups { get; set; }

        public HotelSettings(Dictionary<string, string> texts = null, List<GamePowerup> gamePowerups = null)
        {
            this.Texts = texts;
            this.GamePowerups = gamePowerups;
        }
    }
}
