using FastFoodServerAPI.Data;
using FastFoodServerAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Packets.Outgoing
{
    internal class AuthenicateUserOutgoingPacket : OutgoingPacket
    {
        private FastFoodUser User;

        public AuthenicateUserOutgoingPacket(FastFoodUser user)
        {
            this.User = user;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.AuthenicateUser);
                    binaryWriter.Write(this.User.UserID);
                    binaryWriter.Write(this.User.Username);
                    binaryWriter.Write(this.User.Figure);
                    binaryWriter.Write(this.User.Gender);

                    if (this.User.Badges != null)
                    {
                        binaryWriter.Write((byte)this.User.Badges.Count);
                        foreach (string string_ in this.User.Badges)
                        {
                            binaryWriter.Write(string_);
                        }
                    }
                    else
                    {
                        binaryWriter.Write((byte)0);
                    }

                    binaryWriter.Write(this.User.Missiles);
                    binaryWriter.Write(this.User.Parachutes);
                    binaryWriter.Write(this.User.Shields);
                    binaryWriter.Write(this.User.Credits);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
