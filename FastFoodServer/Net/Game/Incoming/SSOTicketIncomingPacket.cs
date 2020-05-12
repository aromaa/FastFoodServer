using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Core;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.Net.Game.Outgoing;

namespace FastFoodServer.Net.Game.Incoming
{
    class SSOTicketIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game != null)
            {
                string sso = packet.ReadString();

                KeyValuePair<Hotel, uint> data = Core.Server.GetGame().GetSessionManager().GetSSOTicket(sso);
                if (data.Key != null && data.Value > 0)
                {
                    (game.User = data.Key.GetUser(data.Value)).GameConnection = game;

                    connection.SendPacket(new AuthenicationOKOutgoingPacket());
                }
            }
        }
    }
}
