using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.Game.Outgoing;

namespace FastFoodServer.Net.Game.Incoming
{
    class RequestScoreViewIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game?.LastGame != null)
            {
                game.SendPacket(new UnableToFriendOutgoingPacket(game.LastGame.GameLobby.GetParticipants().Select(u => u.GameUserID).ToList()));
                //game.SendPacket(new PowerupGainedOutgoingPacket(GamePowerupType.Missile, 9999999)); //RANDOM LOOT, TODO

                game.LastGame = null;
            }
        }
    }
}
