using FastFoodServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game
{
    public class GameListener
    {
        private SocketManager SocketManager;

        private AsyncCallback AcceptCallback;
        private Socket Socket;
        private volatile bool Listening = false;

        public GameListener(SocketManager socketManager, string ip, int port)
        {
            this.SocketManager = socketManager;
            this.AcceptCallback = new AsyncCallback(this.AcceptConnection);

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            this.Socket.Listen(1000);
            this.Socket.NoDelay = true;

            Logging.WriteLine("Listening for game on port: " + port);
        }

        public void Start()
        {
            if (!this.Listening)
            {
                this.Listening = true;

                this.StartListening();
            }
        }

        public void Stop()
        {
            if (this.Listening)
            {
                this.Listening = false;

                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                    this.Socket.Close();
                    this.Socket.Dispose();
                }
                catch
                {

                }

                this.AcceptCallback = null;
            }
        }

        private void StartListening()
        {
            if (this.Listening)
            {
                try
                {
                    this.Socket.BeginAccept(this.AcceptConnection, this.Socket);
                }
                catch
                {

                }
            }
        }

        private void AcceptConnection(IAsyncResult ar)
        {
            try
            {
                Socket socket = ((Socket)ar.AsyncState).EndAccept(ar);
                socket.NoDelay = true;

                this.SocketManager.Connection(socket);
            }
            catch (Exception ex)
            {
                Logging.WriteLine(ex.ToString());
            }
            finally
            {
                this.StartListening();
            }
        }
    }
}
