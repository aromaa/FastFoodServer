using FastFoodServer.FastFood;
using FastFoodServer.Net;
using FastFoodServer.Net.API;
using FastFoodServer.Net.Game;
using FastFoodServer.Net.Game.Outgoing;
using FastFoodServer.Net.Policy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastFoodServer.Core
{
    public sealed class Server
    {
        public static volatile bool ServerShutdown;
        public static readonly int BuildNumber = Assembly.GetEntryAssembly().GetName().Version.Revision;
        public static readonly string Version = "FastFood Server 1.0.0.0 (Build: " + Server.BuildNumber + ")";

        private static Stopwatch ServerStarted;
        private static ConfigurationData ConfigurationData;
        private static APIListener APIListener;
        private static PolicyListener PolicyListener;
        private static SocketManager SocketManager;
        private static Game Game;

        public void Init()
        {
            Logging.WriteLine(@".---.           .  .---.              .       .-.                          ", ConsoleColor.Yellow);
            Logging.WriteLine(@"|              _|_ |                  |      (   )                         ", ConsoleColor.Yellow);
            Logging.WriteLine(@"|---  .-.  .--. |  |---  .-.  .-.  .-.|       `-.  .-. .--..    ._ .-. .--.", ConsoleColor.Yellow);
            Logging.WriteLine(@"|    (   ) `--. |  |    (   )(   )(   |      (   )(.-' |    \  /  (.-' |   ", ConsoleColor.Yellow);
            Logging.WriteLine(@"'     `-'`-`--' `-''     `-'  `-'  `-'`-      `-'  `--''     `'    `--''   ", ConsoleColor.Yellow);
            Logging.WriteLine(Server.Version, ConsoleColor.Yellow);
            Logging.WriteLine("Made by Jonny", ConsoleColor.Yellow);
            Logging.WriteBlank();

            try
            {
                Server.ServerStarted = Stopwatch.StartNew();
                Server.ConfigurationData = new ConfigurationData("config.conf");
                Server.Game = new Game();
                
                Server.APIListener = new APIListener(Server.Config["game.tcp.ip"], int.Parse(Server.Config["game.api.port"]));
                Server.APIListener.Start();

                Server.PolicyListener = new PolicyListener(Server.Config["game.tcp.ip"], int.Parse(Server.Config["game.policy.port"]));
                Server.PolicyListener.Start();

                Server.SocketManager = new SocketManager(Server.Config["game.tcp.ip"], int.Parse(Server.Config["game.tcp.port"]));
                Server.SocketManager.Start();

                TimeSpan bootTime = Server.Uptime;
                Logging.WriteLine("READY! (" + bootTime.Seconds + " s, " + bootTime.Milliseconds + " ms)", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                Logging.WriteLine("FAILED TO BOOT! ", ConsoleColor.Red);

                Server.ExceptionShutdown(ex);
            }
        }

        public static TimeSpan Uptime
        {
            get
            {
                return Server.ServerStarted.Elapsed;
            }
        }

        public static ConfigurationData Config
        {
            get
            {
                return Server.ConfigurationData;
            }
        }

        public static Game GetGame()
        {
            return Server.Game;
        }

        public static SocketManager GetSocketManager()
        {
            return Server.SocketManager;
        }

        public static void ExceptionShutdown(Exception exception)
        {
            Logging.WriteLine(exception.ToString(), ConsoleColor.Red);
            Logging.WriteBlank();
            Logging.WriteLine("Press any key to close", ConsoleColor.Blue);

            Console.ReadKey();

            Program.Destroy();
        }

        public static void Destroy(bool close = true)
        {
            if (Server.ServerShutdown)
            {
                return;
            }

            Server.ServerShutdown = true;

            if (Server.PolicyListener != null)
            {
                Server.PolicyListener.Stop();
            }
            Server.PolicyListener = null;

            if (Server.SocketManager != null)
            {
                foreach (GameConnection connection in Server.GetSocketManager().GetClients())
                {
                    try
                    {
                        connection.SendPacket(new MaintenanceOutgoingPacket(true));
                    }
                    catch
                    {

                    }
                }

                Server.SocketManager.Stop();
            }
            Server.SocketManager = null;

            if (close)
            {
                Environment.Exit(0);
            }
        }
    }
}
