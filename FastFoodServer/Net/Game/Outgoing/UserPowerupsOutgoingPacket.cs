using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class UserPowerupsOutgoingPacket : OutgoingPacket
    {
        public const int Parachute = 0;
        public const int Missile = 1;
        public const int Shield = 2;

        public Dictionary<int, int> Powerups;

        public UserPowerupsOutgoingPacket(Dictionary<int, int> powerups)
        {
            this.Powerups = powerups;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UserPowerups);

                    if (this.Powerups != null)
                    {
                        binaryWriter.Write(this.Powerups.Count);

                        foreach(KeyValuePair<int, int> powerup in this.Powerups)
                        {
                            binaryWriter.Write(powerup.Key);
                            binaryWriter.Write(powerup.Value);
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
