using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Data
{
    public class GameLoadDetails
    {
        public string GameSWFUrl { get; private set; }
        public string Quality { get; private set; }
        public string Scale { get; private set; }
        public int FPS { get; private set; }
        public int FlashMajorVersion { get; private set; }
        public int FlashMinorVersion { get; private set; }
        public Dictionary<string, string> Params { get; private set; }

        public GameLoadDetails(string gameSWFUrl, string quality, string scale, int fps, int flashMajorVersion, int flashMinorVersion, Dictionary<string, string> params_)
        {
            this.GameSWFUrl = gameSWFUrl;
            this.Quality = quality;
            this.Scale = scale;
            this.FPS = fps;
            this.FlashMajorVersion = flashMajorVersion;
            this.FlashMinorVersion = flashMinorVersion;
            this.Params = params_;
        }
    }
}
