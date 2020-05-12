using FastFoodServer.FastFood.Enums;
using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    public class PowerupGainedOutgoingPacket : OutgoingPacket
    {
        private GamePowerupType Type;
        private int Amount;

        public PowerupGainedOutgoingPacket(GamePowerupType type, int amount)
        {
            this.Type = type;
            this.Amount = amount;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.PowerupGained);
                    binaryWriter.Write((int)this.Type);
                    binaryWriter.Write(this.Amount);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
