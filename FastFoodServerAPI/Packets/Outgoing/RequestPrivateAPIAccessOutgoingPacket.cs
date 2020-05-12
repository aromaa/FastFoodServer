using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    internal class RequestPrivateAPIAccessOutgoingPacket : OutgoingPacket
    {
        private string Key;
        private string Sign;

        public RequestPrivateAPIAccessOutgoingPacket(string key, string sign)
        {
            this.Key = key;
            this.Sign = sign;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.RequestPrivateAPIAccess);
                    binaryWriter.Write(this.Key);
                    binaryWriter.Write(this.Sign);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
