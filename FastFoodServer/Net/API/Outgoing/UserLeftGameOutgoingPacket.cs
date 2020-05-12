using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    class UserLeftGameOutgoingPacket : OutgoingPacket
    {
        private uint UserID;

        public UserLeftGameOutgoingPacket(uint userId)
        {
            this.UserID = userId;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UserLeftGame);
                    binaryWriter.Write(this.UserID);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
