using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class TextsOutgoingPacket : OutgoingPacket
    {
        private Dictionary<string, string> Texts;

        public TextsOutgoingPacket(Dictionary<string, string> texts)
        {
            this.Texts = texts;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.Texts);

                    if (this.Texts != null)
                    {
                        binaryWriter.Write(this.Texts.Count);
                        foreach(KeyValuePair<string, string> text in this.Texts)
                        {
                            binaryWriter.Write(text.Key);
                            binaryWriter.Write(text.Value);
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
