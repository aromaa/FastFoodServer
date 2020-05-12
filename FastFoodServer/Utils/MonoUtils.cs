using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Utils
{
    public class MonoUtils
    {
        public static readonly bool IsMonoRunning = Type.GetType("Mono.Runtime") != null;
    }
}
