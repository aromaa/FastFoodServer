using FastFoodServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Policy
{
    public class PolicyListener
    {
        private Socket Server;
        private AsyncCallback AcceptCallback;
        private volatile bool Listening = false;

        public PolicyListener(string ip, int port)
        {
            this.AcceptCallback = new AsyncCallback(this.AcceptConnection);
            this.Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Server.NoDelay = true;
            this.Server.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            this.Server.Listen(1000);

            Logging.WriteLine("Listening for policy on port: " + port);
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
                    this.Server.Shutdown(SocketShutdown.Both);
                    this.Server.Close();
                    this.Server.Dispose();
                }
                catch
                {

                }

                this.AcceptCallback = null;
            }
        }

        public void StartListening()
        {
            if (this.Listening)
            {
                try
                {
                    this.Server.BeginAccept(this.AcceptCallback, this.Server);
                }
                catch
                {

                }
            }
        }

        public void AcceptConnection(IAsyncResult ar)
        {
            if (this.Listening)
            {
                try
                {
                    Socket socket = ((Socket)ar.AsyncState).EndAccept(ar);
                    socket.NoDelay = true;

                    new SocketConnection(socket).Start();
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
}
