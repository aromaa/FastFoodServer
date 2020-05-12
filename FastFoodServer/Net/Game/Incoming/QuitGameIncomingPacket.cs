using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.API;
using FastFoodServer.Net.API.Outgoing;

namespace FastFoodServer.Net.Game.Incoming
{
    class QuitGameIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            //GameConnection game = connection as GameConnection;
            //if (game?.LoggedIn ?? false)
            //{
            //    game.User.Hotel.APIConnection?.SendPacket(new UserLeftGameOutgoingPacket(game.User.UserID));
            //}
        }
    }
}
