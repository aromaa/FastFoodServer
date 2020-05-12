using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Messages;

namespace FastFoodServer.Net.Game.Outgoing
{
    class MaintenanceOutgoingPacket : OutgoingPacket
    {
        private bool FastFoodServerMaintenance;

        public MaintenanceOutgoingPacket(bool fastFoodServerMaintenance)
        {
            this.FastFoodServerMaintenance = fastFoodServerMaintenance;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.Maintenance);
                    binaryWriter.Write(false); //unused
                    binaryWriter.Write(this.FastFoodServerMaintenance);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
