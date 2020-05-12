using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Interfaces
{
    public interface OutgoingPacket
    {
        /// <summary>
        /// Builds the packet
        /// </summary>
        /// <returns>Packet as bytes</returns>
        byte[] GetBytes();
    }
}
