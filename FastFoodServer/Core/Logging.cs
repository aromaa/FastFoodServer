using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Core
{
    public class Logging
    {
        public static ConsoleColor DefaultColor = ConsoleColor.White;

        static Logging()
        {
            Console.ForegroundColor = DefaultColor;
        }

        public static void WriteBlank()
        {
            WriteLine("");
        }

        public static void WriteLine(string message)
        {
            WriteLine(message, DefaultColor);
        }

        public static void WriteLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = DefaultColor;
        }

        public static void Write(string message)
        {
            Write(message, DefaultColor);
        }

        public static void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = DefaultColor;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
