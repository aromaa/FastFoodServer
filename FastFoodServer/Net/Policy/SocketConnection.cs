using FastFoodServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Policy
{
    public class SocketConnection : IDisposable
    {
        private static readonly byte[] XmlPolicy = Encoding.ASCII.GetBytes("<?xml version=\"1.0\"?>\r\n<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n<cross-domain-policy>\r\n<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n</cross-domain-policy>\0");

        private Socket Socket;
        private byte[] Buffer;
        private AsyncCallback ReceiveCallback;
        private AsyncCallback SendCallback;
        private bool Disconnected = false;
        private bool ClosePending = false; //Mono socket fix, If any data is sended forces the connecting to be closed

        public SocketConnection(Socket socket)
        {
            this.Socket = socket;
        }

        ~SocketConnection()
        {
            this.Dispose();
        }

        public void Start()
        {
            this.Buffer = new byte[64];
            this.ReceiveCallback = new AsyncCallback(this.ClientReceiveData);
            this.SendCallback = new AsyncCallback(this.ClientSendedData);

            this.AcceptData();
        }

        public void AcceptData()
        {
            if (!this.Disconnected)
            {
                try
                {
                    this.Socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, this.ReceiveCallback, this);
                }
                catch
                {
                    this.Dispose();
                }
            }
        }

        public void ClientReceiveData(IAsyncResult ar)
        {
            if (!this.Disconnected)
            {
                try
                {
                    int num = 0;
                    try
                    {
                        num = this.Socket.EndReceive(ar);
                    }
                    catch
                    {
                    }

                    if (num > 0)
                    {
                        if (!this.ClosePending)
                        {
                            byte[] packet = new byte[num];
                            Array.Copy(this.Buffer, packet, num);

                            string packet_ = Encoding.ASCII.GetString(packet);
                            if (packet_ == "<policy-file-request/>\0")
                            {
                                this.SendData(SocketConnection.XmlPolicy);

                                if (!MonoUtils.IsMonoRunning)
                                {
                                    this.Dispose(); //we done
                                }
                                else
                                {
                                    this.ClosePending = true;
                                }
                            }
                            else //we no accept other
                            {
                                this.Dispose();
                            }
                        }
                        else
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        this.Dispose();
                    }
                }
                catch
                {
                    this.Dispose();
                }
            }
        }

        public void SendData(byte[] data)
        {
            if (!this.Disconnected)
            {
                try
                {
                    this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, this.SendCallback, this);
                }
                catch
                {
                    this.Dispose();
                }
            }
        }

        public void ClientSendedData(IAsyncResult ar)
        {
            if (!this.Disconnected)
            {
                try
                {
                    this.Socket.EndSend(ar);
                }
                catch
                {
                    this.Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (!this.Disconnected)
            {
                this.Disconnected = true;

                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                    this.Socket.Close();
                    this.Socket.Dispose();
                }
                catch
                {

                }

                if (this.Buffer != null)
                {
                    Array.Clear(this.Buffer, 0, this.Buffer.Length);
                }

                this.Buffer = null;
                this.ReceiveCallback = null;
                this.SendCallback = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
