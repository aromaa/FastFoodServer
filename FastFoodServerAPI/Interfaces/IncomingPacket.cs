using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServerAPI.Net;

namespace FastFoodServerAPI.Interfaces
{
    public interface IncomingPacket
    {
        void Handle(APIConnection apiConnection, BinaryReader currentPacket);
    }
}
