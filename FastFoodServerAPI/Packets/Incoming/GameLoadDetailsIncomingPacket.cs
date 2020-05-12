using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FastFoodServerAPI.Data;

namespace FastFoodServerAPI.Packets.Incoming
{
    internal class GameLoadDetailsIncomingPacket : IncomingPacket
    {
        public GameLoadDetails Details;

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            string gameSWFUrl = currentPacket.ReadString();
            string quality = currentPacket.ReadString();
            string scale = currentPacket.ReadString();
            int fps = currentPacket.ReadInt16();
            int flashMajorVersion = currentPacket.ReadByte();
            int flashMinorVersion = currentPacket.ReadByte();

            Dictionary<string, string> params_ = new Dictionary<string, string>();
            byte paramsCount = currentPacket.ReadByte();
            for (byte i = 0; i < paramsCount; i++)
            {
                params_.Add(currentPacket.ReadString(), currentPacket.ReadString());
            }

            this.Details = new GameLoadDetails(gameSWFUrl, quality, scale, fps, flashMajorVersion, flashMinorVersion, params_);
        }
    }
}
