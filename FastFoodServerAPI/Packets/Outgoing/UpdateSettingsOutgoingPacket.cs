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
    public class UpdateSettingsOutgoingPacket : OutgoingPacket
    {
        private HotelSettings Settings;

        public UpdateSettingsOutgoingPacket(HotelSettings settings)
        {
            this.Settings = settings;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UpdateSettings);

                    if (this.Settings.Texts != null)
                    {
                        binaryWriter.Write((byte)this.Settings.Texts.Count);
                        foreach(KeyValuePair<string, string> text in this.Settings.Texts)
                        {
                            binaryWriter.Write(text.Key);
                            binaryWriter.Write(text.Value);
                        }
                    }
                    else
                    {
                        binaryWriter.Write((byte)0);
                    }

                    if (this.Settings.GamePowerups != null)
                    {
                        binaryWriter.Write((byte)this.Settings.GamePowerups.Count);

                        foreach (GamePowerup powerup in this.Settings.GamePowerups)
                        {
                            binaryWriter.Write(powerup.PackageName);
                            binaryWriter.Write((byte)powerup.Type);
                            binaryWriter.Write(powerup.Amount);
                            binaryWriter.Write(powerup.Cost);
                        }
                    }
                    else
                    {
                        binaryWriter.Write((byte)0);
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
