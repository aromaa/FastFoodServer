using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Hotels
{
    public class HotelManager
    {
        private ConcurrentDictionary<string, Hotel> Hotels;

        public HotelManager()
        {
            this.Hotels = new ConcurrentDictionary<string, Hotel>();
        }

        public Hotel PrivateAPIAuthenication(string key, string sign)
        {
            string combined = key + "-" + sign;

            return this.Hotels.GetOrAdd(combined, new Hotel());
        }
    }
}
