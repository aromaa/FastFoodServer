using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    public class UnableToFriendOutgoingPacket : OutgoingPacket
    {
        private List<int> Players;

        public UnableToFriendOutgoingPacket(List<int> players)
        {
            this.Players = players;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UnableToFriend);

                    if (this.Players != null)
                    {
                        binaryWriter.Write(this.Players.Count);

                        foreach(int userId in this.Players)
                        {
                            binaryWriter.Write(userId);
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
