using FastFoodServer.FastFood.Hotels;
using FastFoodServer.Net.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.User
{
    public class FastFoodUser
    {
        public Hotel Hotel { get; private set; }

        public uint UserID { get; private set; }
        public string Username { get; private set; }
        public string Figure { get; private set; }
        public string Gender { get; private set; }
        public List<string> Badges { get; private set; }

        public int Missiles { get; set; }
        public int Parachutes { get; set; }
        public int Shields { get; set; }
        public int Credits { get; set; }

        private GameConnection _GameConnection;
        public GameConnection GameConnection
        {
            get
            {
                return this._GameConnection;
            }
            set
            {
                if (this._GameConnection != null)
                {
                    this._GameConnection.DisconnectedEvent -= this.GameConnectionDisconnected;
                    this._GameConnection.Disconnect("New connection");
                }

                value.DisconnectedEvent += this.GameConnectionDisconnected;
                this._GameConnection = value;
            }
        }

        private void GameConnectionDisconnected()
        {
            this._GameConnection = null;
        }

        public FastFoodUser(Hotel hotel, uint userId, string username, string figure, string gender, List<string> badges, int missiles, int parachutes, int shilds, int credits)
        {
            this.Hotel = hotel;
            this.UserID = userId;
            this.Username = username;
            this.Figure = figure;
            this.Gender = gender;
            this.Badges = badges;

            this.Missiles = missiles;
            this.Parachutes = parachutes;
            this.Shields = shilds;
            this.Credits = credits;
        }
    }
}
