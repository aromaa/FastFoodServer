using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastFoodServerAPI.Packets.Incoming
{
    internal class ServerVersionIncomingPacket : IncomingPacket
    {
        private static byte[] MAGIC = { 0x10, 0x7, 0x56, 0x1, 0x8, 0xef, 0xe, 0xf, 0xE, 0x4, 0x8, 0xf, 0x54, 0x78, 0x1, 0x1 };

        public bool MagicMatches { get; private set; }
        public string FullVersion { get; private set; }
        public int Build { get; private set; }

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            bool matches = true;
            foreach(byte byte_ in ServerVersionIncomingPacket.MAGIC)
            {
                if (byte_ != currentPacket.ReadByte())
                {
                    matches = false;
                }
            }

            this.MagicMatches = matches;
            this.FullVersion = currentPacket.ReadString();
            this.Build = currentPacket.ReadInt16();
        }
    }
}
