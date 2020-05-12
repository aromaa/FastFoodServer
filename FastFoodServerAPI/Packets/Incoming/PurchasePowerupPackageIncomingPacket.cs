using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastFoodServerAPI.Packets.Incoming
{
    public class PurchasePowerupPackageIncomingPacket : IncomingPacket
    {
        public uint UserID { get; private set; }
        public string PackageName { get; private set; }

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            this.UserID = currentPacket.ReadUInt32();
            this.PackageName = currentPacket.ReadString();
        }
    }
}
