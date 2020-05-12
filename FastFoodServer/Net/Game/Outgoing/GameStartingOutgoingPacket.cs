using FastFoodServer.FastFood.Data;
using FastFoodServer.FastFood.User;
using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class GameStartingOutgoingPacket : OutgoingPacket
    {
        public const int MaxPowerups = 3;

        private int GameUserID;
        private Dictionary<int, int> Powerups;
        private List<FastFoodGameUser> Users;

        public GameStartingOutgoingPacket(int gameUserId, Dictionary<int, int> powerups, List<FastFoodGameUser> users)
        {
            this.GameUserID = gameUserId;
            this.Powerups = powerups;
            this.Users = users;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.GameStarting);
                    binaryWriter.Write(this.GameUserID);
                    binaryWriter.Write(true);
                    binaryWriter.Write(true);
                    binaryWriter.Write(true);
                    binaryWriter.Write(0);

                    if (FastFoodGamePlate.Plates != null)
                    {
                        binaryWriter.Write(FastFoodGamePlate.Plates.Count);

                        foreach(FastFoodGamePlate plate in FastFoodGamePlate.Plates)
                        {
                            binaryWriter.Write(plate.ID);
                            binaryWriter.Write(plate.FallMultiplayer.ToString());
                            binaryWriter.Write(plate.ParachuteMultiplayer.ToString());
                            binaryWriter.Write(plate.ParachuteSpeed.ToString());
                            binaryWriter.Write(plate.BigParachuteSpeed.ToString());
                            binaryWriter.Write(plate.PlateTimer);
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0);
                    }

                    if (this.Powerups != null)
                    {
                        binaryWriter.Write(this.Powerups.Count);
                        
                        foreach(KeyValuePair<int, int> powerup in this.Powerups)
                        {
                            binaryWriter.Write(powerup.Key); //id
                            binaryWriter.Write(powerup.Value); //count
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0); //powerup count
                    }

                    if (this.Users != null)
                    {
                        binaryWriter.Write(this.Users.Count);

                        foreach(FastFoodGameUser user in this.Users)
                        {
                            binaryWriter.Write(user.GameUserID);
                            binaryWriter.Write(user.User.Username);
                            binaryWriter.Write(""); //Figure URL
                            binaryWriter.Write(user.User.Gender);
                            binaryWriter.Write("hhfi"); //hotel

                            if (user.User.Badges != null)
                            {
                                binaryWriter.Write(user.User.Badges.Count);

                                for (int i = 0; i < user.User.Badges.Count; i++)
                                {
                                    binaryWriter.Write(user.User.Badges[i]);
                                    binaryWriter.Write(i);
                                    binaryWriter.Write(""); //URL
                                }
                            }
                            else
                            {
                                binaryWriter.Write(0); //badges count
                            }
                        }
                    }
                    else
                    {
                        binaryWriter.Write(0); //players count
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
