using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServerAPI.Enums
{
    public enum ConnectionResult
    {
        ConnectionFailed,
        FailedToVerify,
        NotAPIServer,
        Connected
    }
}
