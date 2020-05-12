using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    internal class RequestServerVersionOutgoingPacket : OutgoingPacket
    {
        private static byte[] MAGIC = { 0x00, 0x11, 0x44, 0x58, 0x64, 0x1b, 0x58, 0x9, 0x8, 0xe, 0xE, 0xf, 0xB, 0xC, 0x0, 0x0};

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.RequestServerVersion);
                    binaryWriter.Write(RequestServerVersionOutgoingPacket.MAGIC);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
