using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class CreditsOutgoingPacket : OutgoingPacket
    {
        private int Credits;

        public CreditsOutgoingPacket(int credits)
        {
            this.Credits = credits;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.Credits);
                    binaryWriter.Write(this.Credits);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
