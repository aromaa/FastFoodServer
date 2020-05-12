using FastFoodServerAPI.Enums;
using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    public class UpdateUserPowerupCountOutgoingPacket : OutgoingPacket
    {
        private uint UserID;
        private GamePowerupType PowerupType;
        private int Amount;

        public UpdateUserPowerupCountOutgoingPacket(uint userId, GamePowerupType powerupType, int amount)
        {
            this.UserID = userId;
            this.PowerupType = powerupType;
            this.Amount = amount;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UpdateUserPowerupCount);
                    binaryWriter.Write(this.UserID);
                    binaryWriter.Write((byte)this.PowerupType);
                    binaryWriter.Write(this.Amount);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
