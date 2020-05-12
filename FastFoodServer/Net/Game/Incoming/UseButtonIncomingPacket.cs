using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.FastFood.Enums;

namespace FastFoodServer.Net.Game.Incoming
{
    class UseButtonIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game?.InGame ?? false && game.GameUser.GameLobby.Status == GameStatus.Running)
            {
                int buttonId = packet.ReadInt32();
                switch(buttonId)
                {
                    case 0: //release food
                        {
                            game.GameUser.TryReleaseFood();
                        }
                        break;
                    case 1: //open parachute
                        {
                            game.GameUser.OpenParachute();
                        }
                        break;
                    case 2: //open big parachute
                        {
                            game.GameUser.OpenBigParachute();
                        }
                        break;
                    case 3: //launch missile
                        {
                            game.GameUser.LaunchMissile();
                        }
                        break;
                    case 4: //activate shield
                        {
                            game.GameUser.ActivateShield();
                        }
                        break;
                }
            }
        }
    }
}
