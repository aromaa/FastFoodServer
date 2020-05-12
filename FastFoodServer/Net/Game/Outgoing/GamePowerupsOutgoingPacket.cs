using FastFoodServer.FastFood.Data;
using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class GamePowerupsOutgoingPacket : OutgoingPacket
    {
        private List<GamePowerup> GamePowerups;

        public GamePowerupsOutgoingPacket(List<GamePowerup> gamePowerups)
        {
            this.GamePowerups = gamePowerups;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.GamePowerups);

                    if (this.GamePowerups != null)
                    {
                        binaryWriter.Write(this.GamePowerups.Count);

                        foreach(GamePowerup powerup in this.GamePowerups)
                        {
                            binaryWriter.Write(powerup.PackageName);
                            binaryWriter.Write(powerup.TypeString);
                            binaryWriter.Write(powerup.Amount);
                            binaryWriter.Write(powerup.Cost);
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
