using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public static class OwinLayimContextExtensions
    {
        public static IDictionary<string, object> GetOwinEnvironment(this LayimContext context)
        {
            Error.ThrowIfNull(context, nameof(context));

            var owinContext = context as OwinLayimContext;

            return owinContext.Environment;
        }
    }
}
