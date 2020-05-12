using FastFoodServer.Core;
using FastFoodServer.FastFood.Games;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.FastFood.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FastFoodServer.FastFood
{
    public class Game
    {
        private HotelManager HotelManager;
        private SessionManager SessionManager;
        private GameLobbyManager GameLobbyManager;

        private Timer GameCycleTimer;

        private Stopwatch UpdateConsoleTitleTimer;
        private Task UpdateConsoleTitleTask;

        public Game()
        {
            this.HotelManager = new HotelManager();
            this.SessionManager = new SessionManager();
            this.GameLobbyManager = new GameLobbyManager();

            this.UpdateConsoleTitleTimer = Stopwatch.StartNew();

            this.GameCycleTimer = new Timer();
            this.GameCycleTimer.Elapsed += this.Cycle;
            this.GameCycleTimer.AutoReset = true;
            this.GameCycleTimer.Interval = 8;
            this.GameCycleTimer.Start();

            GC.KeepAlive(this.GameCycleTimer); //IK timer adds itself to the gc already, but just for sure ;P
        }

        private void Cycle(object sender, ElapsedEventArgs e)
        {
            if (!Server.ServerShutdown)
            {
                try
                {
                    if (this.GameLobbyManager.CycleTask == null || this.GameLobbyManager.CycleTask.IsCompleted)
                    {
                        (this.GameLobbyManager.CycleTask = new Task(new Action(this.GameLobbyManager.Cycle))).Start();
                    }

                    if (this.UpdateConsoleTitleTimer.Elapsed.TotalSeconds >= 5)
                    {
                        this.UpdateConsoleTitleTimer.Restart();

                        if (this.UpdateConsoleTitleTask == null || this.UpdateConsoleTitleTask.IsCompleted)
                        {
                            (this.UpdateConsoleTitleTask = new Task(new Action(this.UpdateConsoleTitle))).Start();
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logging.WriteLine("Error in game cycle! " + ex.ToString());
                }
            }
        }

        private void UpdateConsoleTitle()
        {
            TimeSpan uptime = Server.Uptime;
            Console.Title = "FastFood Server | Users Online: " + Server.GetSocketManager().UsersOnline + " | Games: " + Server.GetGame().GetGameLobbyManager().GamesCount + " | Uptime: " + uptime.Days + " days, " + uptime.Hours + " hours, " + uptime.Minutes + " minutes | Memory: " + GC.GetTotalMemory(false) / 1024 + " KB";
        }

        public HotelManager GetHotelManager()
        {
            return this.HotelManager;
        }

        public SessionManager GetSessionManager()
        {
            return this.SessionManager;
        }

        public GameLobbyManager GetGameLobbyManager()
        {
            return this.GameLobbyManager;
        }
    }
}
