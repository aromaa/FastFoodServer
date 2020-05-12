using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastFoodServerAPI.Packets.Incoming
{
    public class FastFoodGameFinishedIncomingPacket : IncomingPacket
    {
        public uint UserID { get; private set; }
        public bool Won { get; private set; }
        public int BigParachutesUsed { get; private set; }
        public int MissilesUsed { get; private set; }
        public int ShildsUsed { get; private set; }

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            this.UserID = currentPacket.ReadUInt32();
            this.Won = currentPacket.ReadBoolean();
            this.BigParachutesUsed = currentPacket.ReadInt32();
            this.MissilesUsed = currentPacket.ReadInt32();
            this.ShildsUsed = currentPacket.ReadInt32();
        }
    }
}
