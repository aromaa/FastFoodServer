using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Incoming
{
    public class IncomingHeaders
    {
        public const short SSOTicket = 1;
        public const short UseButton = 3;
        public const short UnUsed = 4; //another option for "join game", unused
        public const short QuitGame = 5;
        public const short JoinGame = 6;
        public const short RequestTexts = 7;
        public const short GetUserPowerups = 8;
        public const short GetGamePowerups = 9;
        public const short PurchasePowerupPackage = 10;
        public const short RequestPlaytime = 11;
        public const short RequestScoreView = 12;
        public const short FriendRequest = 13; //TODO
        public const short GetCredits = 14;
        public const short GetGlobalLeaderboard = 15; //TODO
    }
}
