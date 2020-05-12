using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class OpenBigParachuteOutgoingPacket : OutgoingPacket
    {
        private int PlateId;
        private int GameUserID;

        public OpenBigParachuteOutgoingPacket(int plateId, int gameUserId)
        {
            this.PlateId = plateId;
            this.GameUserID = gameUserId;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.OpenBigParachute);
                    binaryWriter.Write(this.PlateId);
                    binaryWriter.Write(this.GameUserID);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
