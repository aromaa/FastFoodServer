using FastFoodServer.Net.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastFoodServer.FastFood.Games
{
    public class GameLobbyManager
    {
        private ConcurrentDictionary<long, GameLobby> Games;

        public int GamesCount
        {
            get
            {
                return this.Games.Count;
            }
        }

        public Task CycleTask { get; set; }

        private long NextGameID;

        public GameLobbyManager()
        {
            this.Games = new ConcurrentDictionary<long, GameLobby>();
        }

        private long GetNextGameID()
        {
            return Interlocked.Increment(ref this.NextGameID);
        }

        public GameLobby GetFreeGame(GameConnection connection)
        {
            while (true)
            {
                foreach (GameLobby game in this.Games.Values)
                {
                    if (game.TryJoin(connection))
                    {
                        return game;
                    }
                }

                GameLobby newGame = this.CreateGameLobby();
                if (newGame.TryJoin(connection))
                {
                    return newGame;
                }
            }
        }

        public GameLobby CreateGameLobby()
        {
            while (true)
            {
                long id = this.GetNextGameID();

                GameLobby game = new GameLobby(id);
                if (this.Games.TryAdd(id, game))
                {
                    return game;
                }
            }
        }

        public void Cycle()
        {
            foreach(GameLobby game in this.Games.Values)
            {
                if (game.CycleTask == null || game.CycleTask.IsCompleted)
                {
                    (game.CycleTask = new Task(new Action(() => game.Cycle()))).Start();
                }
            }
        }

        public void EndGameLobby(long id)
        {
            GameLobby lobby;
            this.Games.TryRemove(id, out lobby);
        }
    }
}
