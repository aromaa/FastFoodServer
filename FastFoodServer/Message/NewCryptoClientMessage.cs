using FastFoodServer.Utilies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Messages
{
    public class NewCryptoClientMessage : BinaryReader
    {
        public NewCryptoClientMessage(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override short ReadInt16()
        {
            return (short)HabboEncoding.DecodeInt16(this.ReadBytes(2));
        }

        public override string ReadString()
        {
            return Encoding.Default.GetString(this.ReadBytes(this.ReadInt16())).Replace(Convert.ToChar(1), ' ');
        }

        public override int ReadInt32()
        {
            return HabboEncoding.DecodeInt32(this.ReadBytes(4));
        }
    }
}
