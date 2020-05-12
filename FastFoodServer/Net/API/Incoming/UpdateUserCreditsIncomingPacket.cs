using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.FastFood.User;
using FastFoodServer.Net.Game.Outgoing;

namespace FastFoodServer.Net.API.Incoming
{
    class UpdateUserCreditsIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            APIConnection api = connection as APIConnection;
            if (api?.PrivateAPIUsable ?? false)
            {
                uint userId = packet.ReadUInt32();
                int credits = packet.ReadInt32();

                FastFoodUser user = api.Hotel.GetUser(userId);
                if (user != null)
                {
                    user.Credits = credits;

                    user.GameConnection?.SendPacket(new CreditsOutgoingPacket(credits));
                }
            }
        }
    }
}
