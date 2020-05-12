using FastFoodServer.FastFood.Hotels;
using FastFoodServer.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Session
{
    public class SessionManager
    {
        private ConcurrentDictionary<string, KeyValuePair<Hotel, uint>> ActiveSSOTickets;

        public SessionManager()
        {
            this.ActiveSSOTickets = new ConcurrentDictionary<string, KeyValuePair<Hotel, uint>>();
        }

        public string CreateSSOTicket(Hotel hotel, uint userId)
        {
            KeyValuePair<Hotel, uint> data = new KeyValuePair<Hotel, uint>(hotel, userId);

            while (true)
            {
                string sso = AuthenicationUtils.GenerateSessionToken();
                if (this.ActiveSSOTickets.TryAdd(sso, data))
                {
                    return sso;
                }
            }
        }

        public KeyValuePair<Hotel, uint> GetSSOTicket(string ssoTicket)
        {
            KeyValuePair<Hotel, uint> value;
            this.ActiveSSOTickets.TryRemove(ssoTicket, out value);
            return value;
        }
    }
}
