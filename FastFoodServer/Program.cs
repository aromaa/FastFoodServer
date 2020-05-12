using FastFoodServer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer
{
    class Program
    {
        private enum CtrlType
        {
            CTRL_BREAK_EVENT = 1,
            CTRL_C_EVENT = 0,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool LicenceError;

        //only windows support these
        private delegate bool EventHandler(CtrlType sig);
        private static EventHandler ConsoleCtrlEventHandler;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(Program.EventHandler handler, bool add);

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        static void Main(string[] args)
        {
            Console.Title = "FastFood Server";

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.UnhandledException);

            Program.ConsoleCtrlEventHandler = (Program.EventHandler)Delegate.Combine(Program.ConsoleCtrlEventHandler, new Program.EventHandler(Program.ConsoleCtrlHandler));
            Program.SetConsoleCtrlHandler(Program.ConsoleCtrlEventHandler, true);

            SystemEvents.SessionEnded += Program.SessionEnded;
            SystemEvents.PowerModeChanged += Program.PowerModeChanged;

            new Server().Init();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Program.Destroy();
                }
            }
        }

        private static void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Program.Destroy();
        }

        private static void SessionEnded(object sender, SessionEndedEventArgs e)
        {
            Program.Destroy();
        }

        public static void LicenceFailure()
        {
            if (!Program.LicenceError)
            {
                Program.LicenceError = true;

                Task task = new Task(Program.LicenceFail);
                task.Start();
            }
        }

        private static void LicenceFail()
        {
            Program.LicenceFail2();
        }

        private static void LicenceFail2()
        {
            Program.LicenceFail();
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                Program.Destroy(false);

                Logging.WriteLine("SERVER TERMINATED BECAUSE UNHANDLED RUNTIME EXCEPTION!", ConsoleColor.Red);
                Logging.WriteLine(e.ExceptionObject.ToString(), ConsoleColor.Red);
            }
        }

        private static bool ConsoleCtrlHandler(CtrlType ctrlType)
        {
            Program.Destroy();

            return true;
        }

        public static void Destroy(bool close = true)
        {
            SystemEvents.SessionEnded -= SessionEnded;
            SystemEvents.PowerModeChanged -= PowerModeChanged;

            Logging.Clear();
            Logging.WriteLine("The server is saving data. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!", ConsoleColor.Red);

            Server.Destroy(close);
        }
    }
}
