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
    class RequestPlaytimeIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            connection.SendPacket(new PlaytimeOutgoingPacket(0, -1));
        }
    }
}
