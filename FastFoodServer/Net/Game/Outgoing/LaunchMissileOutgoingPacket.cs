using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class LaunchMissileOutgoingPacket : OutgoingPacket
    {
        private int TargetGameUserID;
        private int TargetPlateID;
        private int SenderGameUserID;

        public LaunchMissileOutgoingPacket(int targetGameUserId, int targetPlateId, int senderGameuserId)
        {
            this.TargetGameUserID = targetGameUserId;
            this.TargetPlateID = targetPlateId;
            this.SenderGameUserID = senderGameuserId;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.LaunchMissile);
                    binaryWriter.Write(this.TargetPlateID);
                    binaryWriter.Write(this.SenderGameUserID);
                    binaryWriter.Write(this.TargetGameUserID);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
