using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.Game.Outgoing;

namespace FastFoodServer.Net.Game.Incoming
{
    class GetUserPowerupsIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game != null && game.LoggedIn)
            {
                Dictionary<int, int> powerups = new Dictionary<int, int>();
                if (game.User.Missiles > 0)
                {
                    powerups.Add(UserPowerupsOutgoingPacket.Missile, game.User.Missiles);
                }

                if (game.User.Parachutes > 0)
                {
                    powerups.Add(UserPowerupsOutgoingPacket.Parachute, game.User.Parachutes);
                }

                if (game.User.Shields > 0)
                {
                    powerups.Add(UserPowerupsOutgoingPacket.Shield, game.User.Shields);
                }

                connection.SendPacket(new UserPowerupsOutgoingPacket(powerups));
            }
        }
    }
}
