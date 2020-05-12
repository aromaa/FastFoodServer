using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Utils
{
    public class AuthenicationUtils
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int Lenght = 50;

        public static string GenerateSessionToken()
        {
            Random random = RandomUtils.GetRandom();

            return "FastFood-" + new string((new string(Enumerable.Repeat(AuthenicationUtils.Chars, AuthenicationUtils.Lenght).Select(s => s[random.Next(s.Length)]).ToArray()) + TimeUtils.GetUnixTimestamp().ToString()).OrderBy(c => random.Next(int.MinValue, int.MaxValue)).ToArray());
        }
    }
}
