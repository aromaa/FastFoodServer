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
    class RequestTextsIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game != null && game.LoggedIn)
            {
                connection.SendPacket(new TextsOutgoingPacket(game.User.Hotel.Settings?.Texts));
            }
        }
    }
}
