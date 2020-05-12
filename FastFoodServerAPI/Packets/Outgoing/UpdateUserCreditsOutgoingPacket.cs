using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    public class UpdateUserCreditsOutgoingPacket : OutgoingPacket
    {
        private uint UserID;
        private int Credits;

        public UpdateUserCreditsOutgoingPacket(uint userId, int credits)
        {
            this.UserID = userId;
            this.Credits = credits;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UpdateUserCredits);
                    binaryWriter.Write(this.UserID);
                    binaryWriter.Write(this.Credits);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
