using FastFoodServer.Core;
using FastFoodServer.FastFood.Hotels;
using FastFoodServer.FastFood.User;
using FastFoodServer.Messages;
using FastFoodServer.Net.Base;
using FastFoodServer.Net.Game.Incoming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game
{
    public class GameConnection : BaseConnection
    {
        private static readonly Dictionary<short, IncomingPacket> Packets = new Dictionary<short, IncomingPacket>();
        static GameConnection()
        {
            GameConnection.Packets.Add(IncomingHeaders.SSOTicket, new SSOTicketIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.RequestTexts, new RequestTextsIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.GetUserPowerups, new GetUserPowerupsIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.RequestPlaytime, new RequestPlaytimeIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.GetGamePowerups, new GetGamePowerupsIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.GetCredits, new GetCreditsIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.PurchasePowerupPackage, new PurchasePowerupPackageIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.JoinGame, new JoinGameIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.UseButton, new UseButtonIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.RequestScoreView, new RequestScoreViewIncomingPacket());
            GameConnection.Packets.Add(IncomingHeaders.QuitGame, new QuitGameIncomingPacket());
        }

        public readonly long ID;
        public delegate void ReceivedPacket(BinaryReader data);
        
        private ReceivedPacket ReceivedPacketDelegate;

        public FastFoodUser User { get; set; }
        public bool LoggedIn
        {
            get
            {
                return this.User != null;
            }
        }

        public FastFoodGameUser GameUser { get; set; }
        public bool InGame
        {
            get
            {
                return this.GameUser != null;
            }
        }

        public FastFoodGameUser LastGame { get; set; }

        public GameConnection(long id, Socket socket) : base(socket)
        {
            this.ID = id;

            this.DisconnectedEvent += this.OnDisconnect;
        }

        private void OnDisconnect()
        {
            Server.GetSocketManager().Disconnect(this.ID);

            this.GameUser?.GameLobby?.Leave(this);
        }

        public void SetPacketHandler(ReceivedPacket delegate_)
        {
            this.ReceivedPacketDelegate = delegate_;
        }

        public override void HandleData()
        {
            while (this.Data.Count >= 4 || (this.Data.Count > 0 && this.CurrentPacket != null))
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
                    this.CurrentPacket = new NewCryptoClientMessage(new MemoryStream((this.Data.Dequeue() << 0x18 | this.Data.Dequeue() << 0x10 | this.Data.Dequeue() << 8 | this.Data.Dequeue())), Encoding.Default, false);
                }
            }
        }

        private void HandlePacket()
        {
            if (this.CurrentPacket.PeekChar() != -1)
            {
                short header = this.CurrentPacket.ReadInt16();

                IncomingPacket packet;
                if (GameConnection.Packets.TryGetValue(header, out packet))
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

            byte[] data_ = new byte[data.Length + 4];
            data_[0] = (byte)(data.Length >> 0x18);
            data_[1] = (byte)(data.Length >> 0x10);
            data_[2] = (byte)(data.Length >> 8);
            data_[3] = (byte)data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                data_[i + 4] = data[i];
            }

            this.SendData(data_);
        }
    }
}
