using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class PlaytimeOutgoingPacket : OutgoingPacket
    {
        private int Playtime;
        private int Max;

        public PlaytimeOutgoingPacket(int playtime, int max)
        {
            this.Playtime = playtime;
            this.Max = max;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.Playtime);
                    binaryWriter.Write(this.Playtime);
                    binaryWriter.Write(this.Max);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
