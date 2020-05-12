using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.FastFood.Data;
using FastFoodServer.FastFood.Enums;

namespace FastFoodServer.Net.API.Incoming
{
    class UpdateSettingsIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            APIConnection api = connection as APIConnection;
            if (api != null && api.PrivateAPIUsable)
            {
                Dictionary<string, string> texts = new Dictionary<string, string>();
                byte textsCount = packet.ReadByte();
                for(byte i = 0; i < textsCount; i++)
                {
                    texts.Add(packet.ReadString(), packet.ReadString());
                }

                List<GamePowerup> gamePowerups = new List<GamePowerup>();
                byte gamePowerupsCount = packet.ReadByte();
                for(byte i = 0; i < gamePowerupsCount; i++)
                {
                    string packageName = packet.ReadString();
                    GamePowerupType type = (GamePowerupType)packet.ReadByte();
                    int amount = packet.ReadInt32();
                    int cost = packet.ReadInt32();

                    gamePowerups.Add(new GamePowerup(packageName, type, amount, cost));
                }

                api.Hotel.Settings = new HotelSettings(texts, gamePowerups);
            }
        }
    }
}
