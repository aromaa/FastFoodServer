﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Utils
{
    public class RandomUtils
    {
        public static Random GetRandom()
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[4];
            rNGCryptoServiceProvider.GetBytes(array);
            int seed = BitConverter.ToInt32(array, 0);
            return new Random(seed);
        }

        public static int GetRandom(int min, int max)
        {
            return RandomUtils.GetRandom().Next(min, max + 1);
        }
    }
}
