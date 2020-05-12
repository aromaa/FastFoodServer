using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class GameEndOutgoingPacket : OutgoingPacket
    {
        private Dictionary<int, int> Players;

        public GameEndOutgoingPacket(Dictionary<int, int> players)
        {
            this.Players = players;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.GameEnd);

                    if (this.Players != null)
                    {
                        binaryWriter.Write(this.Players.Count);

                        foreach(KeyValuePair<int, int> player in this.Players)
                        {
                            binaryWriter.Write(player.Key);
                            binaryWriter.Write(player.Value);
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
