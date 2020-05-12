using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    public class OutgoingHeaders
    {
        public const short ServerVersion = 88;
        public const short PrivateAPIAuthenicationResponse = 14;
        public const short AuthenicateUserResponse = 65;
        public const short GameLoadDetails = 7;
        public const short PurchasePowerupPackage = 17;
        public const short FastFoodGameFinished = 51;
        public const short UserLeftGame = 32;
    }
}
