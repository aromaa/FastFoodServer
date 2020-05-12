using FastFoodServer.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Base
{
    public abstract class BaseConnection
    {
        public delegate void Disconnected();

        internal Socket Socket;
        internal AsyncCallback ReceiveCallback;
        internal AsyncCallback SendCallback;
        internal byte[] Buffer;
        internal Queue<byte> Data;
        internal volatile bool Disposed;
        internal BinaryReader CurrentPacket;

        public event Disconnected DisconnectedEvent;

        public BaseConnection(Socket socket)
        {
            this.Socket = socket;
            this.ReceiveCallback = new AsyncCallback(this.ReceiveData);
            this.SendCallback = new AsyncCallback(this.ClientSendedData);
            this.Buffer = new byte[1024];
            this.Data = new Queue<byte>();

            this.Listen();
        }

        private void Listen()
        {
            try
            {
                this.Socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, this.ReceiveCallback, this.Socket);
            }
            catch //if we fail we dispose, if we dont we wait for data and after that dispose
            {
                this.Disconnect("Begin receive failed");
            }
        }

        private void ReceiveData(IAsyncResult ar)
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
                    this.Disconnect("Failed to receive data"); //failed read
                }

                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        this.Data.Enqueue(this.Buffer[i]);
                    }

                    this.HandleData();
                }
                else //send nothing aka closed connection
                {
                    this.Disconnect("Received 0 bytes");
                }
            }
            catch(Exception ex)
            {
                this.Disconnect("Failed to handle data");

                Logging.WriteLine("Failed to handle data: " + ex.ToString(), ConsoleColor.Red);
            }
            finally
            {
                this.Listen();
            }
        }

        public void ClientSendedData(IAsyncResult ar)
        {
            if (!this.Disposed)
            {
                try
                {
                    this.Socket.EndSend(ar);
                }
                catch
                {
                    this.Disconnect("Failed to send data");
                }
            }
        }

        public abstract void HandleData();

        public abstract void SendPacket(OutgoingPacket packet);

        internal void SendData(byte[] data)
        {
            if (!this.Disposed)
            {
                try
                {
                    this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, this.SendCallback, this);
                }
                catch
                {
                    this.Disconnect("Failed to begin send data");
                }
            }
        }

        public void Disconnect(string reason)
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (!this.Disposed)
            {
                this.Disposed = true;

                try
                {
                    this.DisconnectedEvent?.Invoke();
                }
                catch
                {

                }

                try
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                    this.Socket.Close();
                    this.Socket.Dispose();
                }
                catch
                {

                }

                this.Socket = null;
                Array.Clear(this.Buffer, 0, this.Buffer.Length);
                this.Buffer = null;
                this.ReceiveCallback = null;
                this.SendCallback = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
