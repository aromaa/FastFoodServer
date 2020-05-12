using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastFoodServerAPI.Packets.Incoming
{
    internal class UserLeftGameIncomingPacket : IncomingPacket
    {
        public uint UserID { get; private set; }

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            this.UserID = currentPacket.ReadUInt32();
        }
    }
}
