using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    class PurchasePowerupPackageOutgoingPacket : OutgoingPacket
    {
        private uint UserID;
        private string PackageName;

        public PurchasePowerupPackageOutgoingPacket(uint userId, string packageName)
        {
            this.UserID = userId;
            this.PackageName = packageName;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.PurchasePowerupPackage);
                    binaryWriter.Write(this.UserID);
                    binaryWriter.Write(this.PackageName);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
