using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.API.Outgoing;
using FastFoodServer.Core;

namespace FastFoodServer.Net.API.Incoming
{
    class RequestPrivateAPIAccessIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            APIConnection api = connection as APIConnection;
            if (api != null)
            {
                if (!api.PrivateAPIUsable)
                {
                    string key = packet.ReadString();
                    string sign = packet.ReadString();
                    
                    (api.Hotel = Server.GetGame().GetHotelManager().PrivateAPIAuthenication(key, sign)).APIConnection = api;
                }

                connection.SendPacket(new PrivateAPIAuthenicationResponseOutgoingPacket(api.PrivateAPIUsable));
            }
        }
    }
}
