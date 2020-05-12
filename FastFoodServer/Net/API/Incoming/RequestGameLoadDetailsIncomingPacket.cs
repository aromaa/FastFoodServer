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
    public class RequestGameLoadDetailsIncomingPacket : IncomingPacket
    {
        public void Handle(BaseConnection connection, BinaryReader packet)
        {
            connection.SendPacket(new GameLoadDetailsOutgoingPacket(Server.Config["game.swf.url"], Server.Config["game.swf.quality"], Server.Config["game.swf.scale"], int.Parse(Server.Config["game.swf.fps"]), int.Parse(Server.Config["game.swf.flash.major"]), int.Parse(Server.Config["game.swf.flash.minor"]), new Dictionary<string, string>() { { "assetUrl", Server.Config["game.swf.asset.url"] }, {"gameServerHost", Server.Config["game.tcp.ip"] }, { "gameServerPort", Server.Config["game.tcp.port"] }, { "socketPolicyPort", Server.Config["game.policy.port"] } }));
        }
    }
}
