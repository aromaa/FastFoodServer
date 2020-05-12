using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FastFoodServerAPI.Packets.Incoming
{
    internal class AuthenicateUserResponseIncomingPacket : IncomingPacket
    {
        public bool Result { get; private set; }
        public string SessionToken { get; private set; }

        public void Handle(APIConnection apiConnection, BinaryReader currentPacket)
        {
            if (this.Result = currentPacket.ReadBoolean())
            {
                this.SessionToken = currentPacket.ReadString();
            }
        }
    }
}
