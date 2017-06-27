using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
   internal sealed class LayimFactory
    {
        public static ILayimUserIdProvider GetUserIdProvider()
        {
            var provider = GlobalConfiguration.ServiceContainer.Resolve<ILayimUserIdProvider>();
            if (provider == null) {
                return new DefaultUserIdProvider();
            }
            return provider;
        }
    }
}
