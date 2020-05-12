using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.API.Outgoing;
using FastFoodServer.Net.Game.Outgoing;
using FastFoodServer.FastFood.Enums;

namespace FastFoodServer.Net.Game.Incoming
{
    class PurchasePowerupPackageIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            GameConnection game = connection as GameConnection;
            if (game?.LoggedIn ?? false)
            {
                string packageName = packet.ReadString();
                
                game.User.Hotel.APIConnection.SendPacket(new PurchasePowerupPackageOutgoingPacket(game.User.UserID, packageName));
            }
        }
    }
}
