using FastFoodServerAPI.Data;
using FastFoodServerAPI.Enums;
using FastFoodServerAPI.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Interfaces
{
    public interface APIConnection
    {
        /// <summary>
        /// Connect to API server async
        /// </summary>
        /// <param name="ip">The API server IP</param>
        /// <param name="port">The API server port</param>
        /// <param name="completed">Method that will be run with one param as ConnectionResult enum presenting the completion status. If null nothing will be run</param>
        void ConnectAsync(string ip, int port, Action<ConnectionResult> completed = null);

        /// <summary>
        /// Sends packet to API server
        /// </summary>
        /// <param name="packet">The packet to be send</param>
        /// <param name="completed">Method that will be run with one param as boolean presenting the completion status. If null nothing will be run</param>
        void SendPacket(OutgoingPacket packet, Action<bool> completed = null);

        /// <summary>
        /// Tries to get access to private API async
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="signature">Signature</param>
        /// <param name="completed">Method that will be run with one param as boolean presenting the completion status. If null nothing will be run</param>
        void AuthenicatePrivateAPIAsync(string key, string signature, Action<bool> completed);

        /// <summary>
        /// Authenicates user to FastFood API Server and fills their session token if success
        /// </summary>
        /// <param name="user">The user to authenicate</param>
        /// <param name="completed">Method that will be run with one param as boolean presenting the completion status. If null nothing will be run</param>
        void AuthenicateUserAsync(FastFoodUser user, Action<bool> completed);

        /// <summary>
        /// Requests default game load details async
        /// </summary>
        /// <param name="completed">Method that will be run with one param as GameLoadDetails or null if failed that contains the default values. If null nothing will be run</param>
        void RequestDefaultGameLoadDetailsAsync(Action<GameLoadDetails> completed);

        /// <summary>
        /// Updates hotel settings async
        /// </summary>
        /// <param name="settings">Method that will be run with one param as boolean presenting the completion status. If null nothing will be run</param>
        void UpdateHotelSettingsAsync(HotelSettings settings, Action<bool> completed);
        
        /// <summary>
        /// Registers packet listener
        /// </summary>
        /// <param name="listener">The listener</param>
        void RegisterPacketListener(ReceivedPacket listener);

        /// <summary>
        /// Unregister packet listneer
        /// </summary>
        /// <param name="listener">The listener</param>
        void UnregisterPacketListener(ReceivedPacket listener);

        event Disconnected DisconnectedEvent;
    }
}
