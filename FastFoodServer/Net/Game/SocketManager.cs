using FastFoodServer.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game
{
    public class SocketManager
    {
        private GameListener GameListener;

        private ConcurrentDictionary<long, GameConnection> Connections;
        private int NextID = 0;

        public int UsersOnline
        {
            get
            {
                return this.Connections.Values.Count(c => c.LoggedIn);
            }
        }

        public SocketManager(string ip, int port)
        {
            this.Connections = new ConcurrentDictionary<long, GameConnection>();

            this.GameListener = new GameListener(this, ip, port);
        }

        public void Start()
        {
            this.GameListener.Start();
        }

        public long GetNextID()
        {
            return Interlocked.Increment(ref this.NextID);
        }

        public void Connection(Socket socket)
        {
            long id = this.GetNextID(); //every socket have their own unique id
            GameConnection connection = new GameConnection(id, socket);
            if (this.Connections.TryAdd(id, connection))
            {
            }
            else
            {
                connection.Disconnect("Connection TryAdd failed");
            }
        }

        public void Disconnect(long id)
        {
            GameConnection connection;
            if (this.Connections.TryRemove(id, out connection))
            {
            }
        }

        public ICollection<GameConnection> GetClients()
        {
            return this.Connections.Values;
        }

        public void Stop()
        {
            if (this.GameListener != null)
            {
                this.GameListener.Stop();
            }
            this.GameListener = null;

            foreach(GameConnection connection in this.Connections.Values)
            {
                connection.Disconnect("Server shutdown");
            }
        }
    }
}
