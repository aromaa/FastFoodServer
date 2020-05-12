using FastFoodServer.Net.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net
{
    public interface IncomingPacket
    {
        void Handle(BaseConnection connection, BinaryReader packet);
    }
}
