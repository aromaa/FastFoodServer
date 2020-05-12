using FastFoodServerAPI.Interfaces;
using FastFoodServerAPI.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI
{
    public class FastFoodAPI
    {
        internal FastFoodAPI()
        {

        }

        /// <summary>
        /// Creates new instance of APIConnection
        /// </summary>
        /// <returns>Unconncted APIConnection</returns>
        public static APIConnection CreateAPIConnection()
        {
            return new BaseAPIConnection();
        }
    }
}
