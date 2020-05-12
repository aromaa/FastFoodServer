using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class SetShieldOutgoingPacket : OutgoingPacket
    {
        private int PlateId;
        private int GameUserID;
        private bool Active;

        public SetShieldOutgoingPacket(int plateId, int gameUserId, bool active)
        {
            this.PlateId = plateId;
            this.GameUserID = gameUserId;
            this.Active = active;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.SetShield);
                    binaryWriter.Write(this.PlateId);
                    binaryWriter.Write(this.GameUserID);
                    binaryWriter.Write(this.Active);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
