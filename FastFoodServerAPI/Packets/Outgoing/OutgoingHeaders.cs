using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    internal class OutgoingHeaders
    {
        public const short RequestServerVersion = 44;
        public const short RequestPrivateAPIAccess = 24;
        public const short AuthenicateUser = 11;
        public const short RequestGameLoadDetails = 72;
        public const short UpdateSettings = 12;
        public const short UpdateUserPowerupCount = 3;
        public const short UpdateUserCredits = 9;
    }
}
