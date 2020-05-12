using FastFoodServer.Core;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.FastFood.User;
using FastFoodServer.Net.API;
using FastFoodServer.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Hotels
{
    public class Hotel
    {
        private ConcurrentDictionary<uint, FastFoodUser> Users;

        public HotelSettings Settings { get; set; } = new HotelSettings();

        private APIConnection _APIConnection;
        public APIConnection APIConnection
        {
            get
            {
                return this._APIConnection;
            }
            set
            {
                if (this._APIConnection != null)
                {
                    this._APIConnection.DisconnectedEvent -= this.APIConnectionDisconnected;
                    this._APIConnection.Disconnect("New connection");
                }

                value.DisconnectedEvent += this.APIConnectionDisconnected;
                this._APIConnection = value;
            }
        }

        private void APIConnectionDisconnected()
        {
            this._APIConnection = null;
        }

        public Hotel()
        {
            this.Users = new ConcurrentDictionary<uint, FastFoodUser>();
        }

        public string AuthenicateUser(FastFoodUser user)
        {
            this.Users.AddOrUpdate(user.UserID, user, (key, value) => user);

            return Server.GetGame().GetSessionManager().CreateSSOTicket(this, user.UserID);
        }

        public FastFoodUser GetUser(uint userId)
        {
            FastFoodUser user;
            this.Users.TryGetValue(userId, out user);
            return user;
        }
    }
}
