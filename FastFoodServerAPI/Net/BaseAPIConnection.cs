using FastFoodServerAPI.Data;
using FastFoodServerAPI.Enums;
using FastFoodServerAPI.Interfaces;
using FastFoodServerAPI.Packets;
using FastFoodServerAPI.Packets.Incoming;
using FastFoodServerAPI.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Net
{
    public delegate void ReceivedPacket(IncomingPacket packet);
    public delegate void Disconnected();

    internal class BaseAPIConnection : APIConnection, IDisposable
    {
        private static readonly Dictionary<short, IncomingPacket> Packets = new Dictionary<short, IncomingPacket>();
        static BaseAPIConnection()
        {
            BaseAPIConnection.Packets.Add(IncomingHeaders.ServerVersion, new ServerVersionIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.PrivateAPIAuthenicationResponse, new PrivateAPIAuthenicationResponseIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.AuthenicateUserResponse, new AuthenicateUserResponseIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.GameLoadDetails, new GameLoadDetailsIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.PurchasePowerupPackage, new PurchasePowerupPackageIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.FastFoodGameFinished, new FastFoodGameFinishedIncomingPacket());
            BaseAPIConnection.Packets.Add(IncomingHeaders.UserLeftGame, new UserLeftGameIncomingPacket());
        }

        private Socket Socket;
        private AsyncCallback SendCallback;
        private AsyncCallback ReceiveCallback;
        private volatile bool Disposed;
        private byte[] Buffer;
        private Queue<byte> Data;
        private BinaryReader CurrentPacket;
        private event ReceivedPacket ReceivedPacketEvent;
        public event Disconnected DisconnectedEvent;

        public BaseAPIConnection()
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Buffer = new byte[1024];
            this.Data = new Queue<byte>();

            this.SendCallback = new AsyncCallback(this.SendDataCallback);
            this.ReceiveCallback = new AsyncCallback(this.ReceiveDataCallback);
        }

        public void ConnectAsync(string ip, int port, Action<ConnectionResult> completed = null)
        {
            this.Socket.BeginConnect(ip, port, new AsyncCallback(this.ConnectAsyncResult), completed);
        }

        public void SendPacket(OutgoingPacket packet, Action<bool> completed = null)
        {
            byte[] data = packet.GetBytes();

            byte[] data_ = new byte[data.Length + 2];
            data_[0] = (byte)data.Length;
            data_[1] = (byte)(data.Length >> 8);
            for (int i = 0; i < data.Length; i++)
            {
                data_[i + 2] = data[i];
            }

            this.SendData(data_, completed);
        }

        public void AuthenicatePrivateAPIAsync(string key, string signature, Action<bool> completed)
        {
            this.SendPacket(new RequestPrivateAPIAccessOutgoingPacket(key, signature), (c) =>
            {
                if (c)
                {
                    ReceivedPacket delegate_ = null;
                    delegate_ = delegate (IncomingPacket packet)
                    {
                        completed?.Invoke((packet as PrivateAPIAuthenicationResponseIncomingPacket)?.Result ?? false);

                        this.UnregisterPacketListenerUnsafe(delegate_);
                    };

                    this.RegisterPacketListenerUnsafe(delegate_);
                }
                else
                {
                    completed?.Invoke(false);
                }
            });
        }

        public void AuthenicateUserAsync(FastFoodUser user, Action<bool> completed)
        {
            this.SendPacket(new AuthenicateUserOutgoingPacket(user), (c) =>
            {
                if (c)
                {
                    ReceivedPacket delegate_ = null;
                    delegate_ = delegate (IncomingPacket packet)
                    {
                        AuthenicateUserResponseIncomingPacket auth = packet as AuthenicateUserResponseIncomingPacket;
                        if (auth?.Result ?? false)
                        {
                            user.SetSessionToken(auth.SessionToken);

                            completed?.Invoke(true);
                        }
                        else
                        {
                            completed?.Invoke(false);
                        }

                        this.UnregisterPacketListenerUnsafe(delegate_);
                    };

                    this.RegisterPacketListenerUnsafe(delegate_);
                }
                else
                {
                    completed?.Invoke(false);
                }
            });
        }

        public void RequestDefaultGameLoadDetailsAsync(Action<GameLoadDetails> completed)
        {
            this.SendPacket(new RequestGameLoadDetailsOutgoingPacket(), (c) =>
            {
                if (c)
                {
                    ReceivedPacket delegate_ = null;
                    delegate_ = delegate (IncomingPacket packet)
                    {
                        GameLoadDetailsIncomingPacket auth = packet as GameLoadDetailsIncomingPacket;
                        
                        completed?.Invoke(auth.Details);

                        this.UnregisterPacketListenerUnsafe(delegate_);
                    };

                    this.RegisterPacketListenerUnsafe(delegate_);
                }
                else
                {
                    completed?.Invoke(null);
                }
            });
        }

        public void UpdateHotelSettingsAsync(HotelSettings settings, Action<bool> completed)
        {
            this.SendPacket(new UpdateSettingsOutgoingPacket(settings), completed);
        }

        public void RegisterPacketListener(ReceivedPacket listener)
        {
            this.RegisterPacketListenerUnsafe(listener);
        }

        public void UnregisterPacketListener(ReceivedPacket listener)
        {
            this.UnregisterPacketListener(listener);
        }

        private void ConnectAsyncResult(IAsyncResult ar)
        {
            Action<ConnectionResult> completed = (Action<ConnectionResult>)ar.AsyncState;

            try
            {
                this.Socket.EndConnect(ar);

                this.SendPacket(new RequestServerVersionOutgoingPacket(), (c) =>
                {
                    if (c)
                    {
                        ReceivedPacket delegate_ = null;
                        delegate_ = delegate (IncomingPacket packet)
                        {
                            ServerVersionIncomingPacket serverVersion = packet as ServerVersionIncomingPacket;
                            if (serverVersion?.MagicMatches ?? false)
                            {
                                completed?.Invoke(ConnectionResult.Connected);
                            }
                            else
                            {
                                completed?.Invoke(ConnectionResult.NotAPIServer);
                            }

                            this.UnregisterPacketListenerUnsafe(delegate_);
                        };

                        this.RegisterPacketListenerUnsafe(delegate_);
                    }
                    else
                    {
                        completed?.Invoke(ConnectionResult.FailedToVerify);
                    }
                });

                this.StartListening();
            }
            catch
            {
                completed?.Invoke(ConnectionResult.ConnectionFailed);
            }
        }

        private void SendData(byte[] data, Action<bool> completed = null)
        {
            this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, this.SendCallback, completed);
        }

        private void SendDataCallback(IAsyncResult ar)
        {
            Action<bool> completed = (Action<bool>)ar.AsyncState;

            try
            {
                this.Socket.EndSend(ar);

                completed?.Invoke(true);
            }
            catch
            {
                completed?.Invoke(false);
            }
        }

        private void ReceiveDataCallback(IAsyncResult ar)
        {
            if (!this.Disposed)
            {
                try
                {
                    int bytes = this.Socket.EndReceive(ar);

                    if (bytes > 0)
                    {
                        for (int i = 0; i < bytes; i++)
                        {
                            this.Data.Enqueue(this.Buffer[i]);
                        }

                        this.HandleData();
                    }
                }
                catch
                {
                    this.Disconnect("Failed to receive data");
                }
                finally
                {
                    this.StartListening();
                }
            }
        }

        public void HandleData()
        {
            while (this.Data.Count >= 2 || (this.Data.Count > 0  && this.CurrentPacket != null))
            {
                if (this.CurrentPacket != null)
                {
                    if (this.CurrentPacket.BaseStream.Length != ((MemoryStream)this.CurrentPacket.BaseStream).Capacity)
                    {
                        this.CurrentPacket.BaseStream.WriteByte(this.Data.Dequeue());
                    }

                    if (this.CurrentPacket.BaseStream.Length == ((MemoryStream)this.CurrentPacket.BaseStream).Capacity)
                    {
                        this.CurrentPacket.BaseStream.Position = 0;
                        this.HandlePacket();
                        this.CurrentPacket = null;
                    }
                }
                else
                {
                    this.CurrentPacket = new BinaryReader(new MemoryStream((this.Data.Dequeue() | this.Data.Dequeue() << 8)), Encoding.Default, false);
                }
            }
        }

        private void HandlePacket()
        {
            short header = this.CurrentPacket.ReadInt16();

            IncomingPacket packet;
            if (BaseAPIConnection.Packets.TryGetValue(header, out packet))
            {
                packet.Handle(this, this.CurrentPacket);

                this.ReceivedPacketEvent?.Invoke(packet);
            }
        }

        private void HandleData(byte[] data)
        {

        }

        private void StartListening()
        {
            if (!this.Disposed)
            {
                try
                {
                    this.Socket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, this.ReceiveCallback, this);
                }
                catch
                {
                    this.Disconnect("Failed to start receiving data");
                }
            }
        }

        private void RegisterPacketListenerUnsafe(ReceivedPacket receivedPacket)
        {
            this.ReceivedPacketEvent += receivedPacket;
        }

        private void UnregisterPacketListenerUnsafe(ReceivedPacket receivedPacket)
        {
            this.ReceivedPacketEvent -= receivedPacket;
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
                finally
                {
                    this.Socket = null;
                }

                if (this.Buffer != null)
                {
                    Array.Clear(this.Buffer, 0, this.Buffer.Length);
                }

                this.Buffer = null;
                this.SendCallback = null;
                this.ReceiveCallback = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}
