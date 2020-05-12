using FastFoodServer.FastFood.Enums;
using FastFoodServer.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Net.Game.Outgoing
{
    class UpdateStateOutgoingPacket : OutgoingPacket
    {
        public int PlateID;
        public int GameUserID;
        public double Distance;
        public double Speed;
        public GamePlateState State;
        public bool Failed;

        public UpdateStateOutgoingPacket(int plateId, int gameUserId, double distance, double speed, GamePlateState state, bool failed)
        {
            this.PlateID = plateId;
            this.GameUserID = gameUserId;
            this.Distance = distance;
            this.Speed = speed;
            this.State = state;
            this.Failed = failed;
        }

        public byte[] GetBytes()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (NewCryptoServerMessage binaryWriter = new NewCryptoServerMessage(memoryStream))
                {
                    binaryWriter.Write(OutgoingHeaders.UpdateState);
                    binaryWriter.Write(this.PlateID);
                    binaryWriter.Write(this.GameUserID);
                    binaryWriter.Write(this.Distance.ToString());
                    binaryWriter.Write(this.Speed.ToString());
                    binaryWriter.Write((int)this.State);
                    binaryWriter.Write(this.Failed);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
