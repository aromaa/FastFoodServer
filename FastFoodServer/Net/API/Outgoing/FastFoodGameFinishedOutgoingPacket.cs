using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    class FastFoodGameFinishedOutgoingPacket : OutgoingPacket
    {
        private uint UserID;
        private bool Won;
        private int BigParachutesUsed;
        private int MissilesUsed;
        private int ShildsUsed;

        public FastFoodGameFinishedOutgoingPacket(uint userId, bool won, int bigParachutesUsed, int missilesUsed, int shiledsUsed)
        {
            this.UserID = userId;
            this.Won = won;
            this.BigParachutesUsed = bigParachutesUsed;
            this.MissilesUsed = missilesUsed;
            this.ShildsUsed = shiledsUsed;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.FastFoodGameFinished);
                    binaryWriter.Write(this.UserID);
                    binaryWriter.Write(this.Won);
                    binaryWriter.Write(this.BigParachutesUsed);
                    binaryWriter.Write(this.MissilesUsed);
                    binaryWriter.Write(this.ShildsUsed);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
