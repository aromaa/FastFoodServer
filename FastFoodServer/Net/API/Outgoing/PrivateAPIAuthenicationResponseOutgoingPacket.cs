using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    class PrivateAPIAuthenicationResponseOutgoingPacket : OutgoingPacket
    {
        private bool Result;

        public PrivateAPIAuthenicationResponseOutgoingPacket(bool result)
        {
            this.Result = result;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.PrivateAPIAuthenicationResponse);
                    binaryWriter.Write(this.Result);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
