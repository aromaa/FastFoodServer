using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Core;

namespace FastFoodServer.Net.Game.Incoming
{
    class JoinGameIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game?.LoggedIn ?? false)
            {
                Server.GetGame().GetGameLobbyManager().GetFreeGame(game);
            }
        }
    }
}
