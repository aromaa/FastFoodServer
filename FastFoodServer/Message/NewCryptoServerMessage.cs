using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Messages
{
    public class NewCryptoServerMessage : BinaryWriter
    {
        public NewCryptoServerMessage(Stream output) : base(output)
        {
        }

        public override void Write(short value)
        {
            base.Write((byte)(value >> 8));
            base.Write((byte)value);
        }

        public override void Write(int value)
        {
            base.Write((byte)(value >> 24));
            base.Write((byte)(value >> 16));
            base.Write((byte)(value >> 8));
            base.Write((byte)value);
        }

        public override void Write(uint value)
        {
            base.Write((byte)(value >> 24));
            base.Write((byte)(value >> 16));
            base.Write((byte)(value >> 8));
            base.Write((byte)value);
        }

        public override void Write(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);

            this.Write((short)bytes.Length);
            base.Write(bytes);
        }
    }
}
