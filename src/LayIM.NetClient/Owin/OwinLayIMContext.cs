using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public sealed class OwinLayimContext : LayimContext
    {
        public OwinLayimContext(LayimStorage storage, LayimOptions options, IDictionary<string, object> environment) : base(storage, options)
        {
            Error.ThrowIfNull(environment, nameof(environment));

            Request = new OwinLayimRequest(environment);
            Response = new OwinLayimResponse(environment);

        }

        public IDictionary<string, object> Environment { get; }
    }
}
