using FastFoodServer.Core;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.Net.API.Incoming;
using FastFoodServer.Net.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.API
{
    public class APIConnection : BaseConnection
    {
        private static readonly Dictionary<short, IncomingPacket> Packets = new Dictionary<short, IncomingPacket>();
        static APIConnection()
        {
            APIConnection.Packets.Add(IncomingHeaders.RequestServerVersion, new RequestServerVersionIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.RequestPrivateAPIAccess, new RequestPrivateAPIAccessIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.AuthenicateUser, new AuthenicateUserIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.RequestGameLoadDetails, new RequestGameLoadDetailsIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.UpdateSettings, new UpdateSettingsIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.UpdateUserPowerupCount, new UpdateUserPowerupCountIncomingPacket());
            APIConnection.Packets.Add(IncomingHeaders.UpdateUserCredits, new UpdateUserCreditsIncomingPacket());
        }

        public Hotel Hotel { get; set; }
        public bool PrivateAPIUsable
        {
            get
            {
                return this.Hotel != null;
            }
        }

        public APIConnection(Socket socket) : base(socket)
        {
        }

        public override void HandleData()
        {
            while (this.Data.Count >= 2 || (this.Data.Count > 0 && this.CurrentPacket != null))
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
            if (this.CurrentPacket.PeekChar() != -1)
            {
                short header = this.CurrentPacket.ReadInt16();

                IncomingPacket packet;
                if (APIConnection.Packets.TryGetValue(header, out packet))
                {
                    try
                    {
                        packet.Handle(this, this.CurrentPacket);
                    }
                    catch
                    {
                        this.Disconnect("Error handling packet");
                    }
                }
            }
            else
            {
                this.Disconnect("No header");
            }
        }

        public override void SendPacket(OutgoingPacket packet)
        {
            byte[] data = packet.GetBytes();

            byte[] data_ = new byte[data.Length + 2];
            data_[0] = (byte)data.Length;
            data_[1] = (byte)(data.Length >> 8);
            for (int i = 0; i < data.Length; i++)
            {
                data_[i + 2] = data[i];
            }

            this.SendData(data_);
        }
    }
}
