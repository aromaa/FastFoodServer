using FastFoodServer.FastFood.Enums;
using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class FoodHitTableOutgoingPacket : OutgoingPacket
    {
        private int Stars;
        private int GameUserID;
        private GamePlateState State;
        private int NextPlateID;

        public FoodHitTableOutgoingPacket(int stars, int gameUserId, GamePlateState state, int nextPlateId)
        {
            this.Stars = stars;
            this.GameUserID = gameUserId;
            this.State = state;
            this.NextPlateID = nextPlateId;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.FoodHitTable);
                    binaryWriter.Write(this.Stars);
                    binaryWriter.Write(this.GameUserID);
                    binaryWriter.Write((int)this.State);
                    binaryWriter.Write(this.NextPlateID);
                    binaryWriter.Write(12); //idk?
                }

                return memoryStream.ToArray();
            }
        }
    }
}
