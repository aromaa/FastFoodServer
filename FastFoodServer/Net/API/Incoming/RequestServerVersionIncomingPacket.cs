using FastFoodServer.Net.API.Outgoing;
using FastFoodServer.Net.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Incoming
{
    public class RequestServerVersionIncomingPacket : IncomingPacket
    {
        private static byte[] MAGIC = { 0x00, 0x11, 0x44, 0x58, 0x64, 0x1b, 0x58, 0x9, 0x8, 0xe, 0xE, 0xf, 0xB, 0xC, 0x0, 0x0 };

        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            foreach(byte byte_ in RequestServerVersionIncomingPacket.MAGIC)
            {
                if (byte_ != packet.ReadByte())
                {
                    return;
                }
            }

            //magic was correct
            connection.SendPacket(new ServerVersionOutgoingPacket());
        }
    }
}
