using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API.Outgoing
{
    public class GameLoadDetailsOutgoingPacket : OutgoingPacket
    {
        public string GameSWFUrl;
        public string Quality;
        public string Scale;
        public int FPS;
        public int FlashMajor;
        public int FlashMinor;
        public Dictionary<string, string> Params;

        public GameLoadDetailsOutgoingPacket(string gameSWFUrl, string quality, string scale, int fps, int flashMajor, int flashMinor, Dictionary<string, string> params_)
        {
            this.GameSWFUrl = gameSWFUrl;
            this.Quality = quality;
            this.Scale = scale;
            this.FPS = fps;
            this.FlashMajor = flashMajor;
            this.FlashMinor = flashMinor;
            this.Params = params_;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.GameLoadDetails);
                    binaryWriter.Write(this.GameSWFUrl);
                    binaryWriter.Write(this.Quality);
                    binaryWriter.Write(this.Scale);
                    binaryWriter.Write((short)this.FPS);
                    binaryWriter.Write((byte)this.FlashMajor);
                    binaryWriter.Write((byte)this.FlashMinor);

                    if (this.Params != null)
                    {
                        binaryWriter.Write((byte)this.Params.Count);

                        foreach(KeyValuePair<string, string> param in this.Params)
                        {
                            binaryWriter.Write(param.Key);
                            binaryWriter.Write(param.Value);
                        }
                    }
                    else
                    {
                        binaryWriter.Write((byte)0);
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
