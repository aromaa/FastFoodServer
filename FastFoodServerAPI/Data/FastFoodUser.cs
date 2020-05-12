using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Data
{
    /// <summary>
    /// Represents FastFood user, may be offline, all virtual fields should be overriden with automatic update logic
    /// </summary>
    public class FastFoodUser
    {
        public uint UserID { get; private set; }
        public virtual string Username { get; private set; }
        public virtual string Figure { get; set; }
        public virtual string Gender { get; set; }
        public virtual List<string> Badges { get; set; }

        public virtual int Missiles { get; set; }
        public virtual int Parachutes { get; set; }
        public virtual int Shields { get; set; }
        public virtual int Credits { get; set; }

        /// <summary>
        /// This is automatically filled when APIConnection.AuthenicateUserAsync is ran succefully
        /// </summary>
        public string SessionToken { get; private set; }

        public FastFoodUser(uint userId, string username, string figure, string gender, List<string> badges, int missiles, int parachutes, int shilds, int credits)
        {
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

        internal void SetSessionToken(string token)
        {
            this.SessionToken = token;
        }
    }
}
