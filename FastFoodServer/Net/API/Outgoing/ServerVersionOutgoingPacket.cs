using FastFoodServer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    public class ServerVersionOutgoingPacket : OutgoingPacket
    {
        private static byte[] MAGIC = { 0x10, 0x7, 0x56, 0x1, 0x8, 0xef, 0xe, 0xf, 0xE, 0x4, 0x8, 0xf, 0x54, 0x78, 0x1, 0x1 };

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.ServerVersion);
                    binaryWriter.Write(ServerVersionOutgoingPacket.MAGIC);
                    binaryWriter.Write(Server.Version);
                    binaryWriter.Write(Server.BuildNumber);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
