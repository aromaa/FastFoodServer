using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    public class AuthenicateUserResponseOutgoingPacket : OutgoingPacket
    {
        private bool Result;
        private string SessionToken;

        public AuthenicateUserResponseOutgoingPacket(bool result, string sessionToken)
        {
            this.Result = result;
            this.SessionToken = sessionToken;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.AuthenicateUserResponse);
                    binaryWriter.Write(this.Result);

                    if (this.Result)
                    {
                        binaryWriter.Write(this.SessionToken);
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
