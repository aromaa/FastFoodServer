using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Utils;
using FastFoodServer.Net.API.Outgoing;
using FastFoodServer.FastFood.User;

namespace FastFoodServer.Net.API.Incoming
{
    class AuthenicateUserIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            APIConnection api = connection as APIConnection;
            if (api != null)
            {
                if (api.PrivateAPIUsable)
                {
                    uint userId = packet.ReadUInt32();
                    string username = packet.ReadString();
                    string figure = packet.ReadString();
                    string gender = packet.ReadString();

                    byte badgesCount = packet.ReadByte();
                    List<string> badges = new List<string>(badgesCount);
                    for (byte i = 0; i < badgesCount; i++)
                    {
                        badges.Add(packet.ReadString());
                    }

                    int missiles = packet.ReadInt32();
                    int parachutes = packet.ReadInt32();
                    int shilds = packet.ReadInt32();
                    int credits = packet.ReadInt32();

                    connection.SendPacket(new AuthenicateUserResponseOutgoingPacket(true, api.Hotel.AuthenicateUser(new FastFoodUser(api.Hotel, userId, username, figure, gender, badges, missiles, parachutes, shilds, credits))));
                }
                else
                {
                    connection.SendPacket(new AuthenicateUserResponseOutgoingPacket(false, null));
                }
            }
        }
    }
}
