using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    public class OutgoingHeaders
    {
        public const short Maintenance = 1;
        public const short GameStarting = 3;
        public const short UpdateState = 4;
        public const short FoodHitTable = 5;
        public const short GameEnd = 6;
        public const short OpenBigParachute = 9;
        public const short LaunchMissile = 10;
        public const short AuthenicationOK = 11;
        public const short SetShield = 12;
        public const short Texts = 13;
        public const short UserPowerups = 14;
        public const short GamePowerups = 15;
        public const short Credits = 16;
        public const short UnableToFriend = 17;
        public const short PowerupGained = 18;
        public const short Playtime = 19;
        public const short Scores = 20;
    }
}
