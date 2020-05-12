using FastFoodServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API
{
    public class APIListener
    {
        private AsyncCallback AcceptCallback;
        private Socket Socket;
        private volatile bool Listening = false;

        public APIListener(string ip, int port)
        {
            this.AcceptCallback = new AsyncCallback(this.AcceptConnection);

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            this.Socket.Listen(10);

            Logging.WriteLine("Listening for API on port: " + port);
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
                    this.Socket.Close();
                }
                catch
                {

                }
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

                new APIConnection(socket);
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
