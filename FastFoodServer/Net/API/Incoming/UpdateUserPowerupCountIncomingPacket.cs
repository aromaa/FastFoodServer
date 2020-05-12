using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.FastFood.Enums;
using FastFoodServer.FastFood.User;
using FastFoodServer.Net.Game.Outgoing;

namespace FastFoodServer.Net.API.Incoming
{
    class UpdateUserPowerupCountIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            APIConnection api = connection as APIConnection;
            if (api?.PrivateAPIUsable ?? false)
            {
                uint userId = packet.ReadUInt32();
                GamePowerupType powerupType = (GamePowerupType)packet.ReadByte();
                int amount = packet.ReadInt32();

                FastFoodUser user = api.Hotel.GetUser(userId);
                if (user != null)
                {
                    switch(powerupType)
                    {
                        case GamePowerupType.Missile:
                            user.Missiles = amount;
                            break;
                        case GamePowerupType.Parachute:
                            user.Parachutes = amount;
                            break;
                        case GamePowerupType.Shield:
                            user.Shields = amount;
                            break;
                    }

                    Dictionary<int, int> powerups = new Dictionary<int, int>();
                    if (user.Missiles > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Missile, user.Missiles);
                    }

                    if (user.Parachutes > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Parachute, user.Parachutes);
                    }

                    if (user.Shields > 0)
                    {
                        powerups.Add(UserPowerupsOutgoingPacket.Shield, user.Shields);
                    }

                    user.GameConnection?.SendPacket(new UserPowerupsOutgoingPacket(powerups));
                }
            }
        }
    }
}
