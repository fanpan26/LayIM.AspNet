using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public class GlobalConfiguration : IGlobalConfiguration
    {
        public static IGlobalConfiguration Configuration { get; } = new GlobalConfiguration();

        internal GlobalConfiguration()
        {
        }
    }
}
