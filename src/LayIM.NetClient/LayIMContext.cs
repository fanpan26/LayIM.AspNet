using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public abstract class LayimContext
    {
        protected LayimContext(LayimStorage storage,LayimOptions options)
        {
            Error.ThrowIfNull(storage, nameof(storage));
            Error.ThrowIfNull(options, nameof(options));

            Storage = storage;
            Options = options;
        }

        public LayimStorage Storage { get; set; }

        public LayimOptions Options { get; set; }

        public Match UriMatch { get; set; }

        public LayimRequest Request { get; protected set; }
        public LayimResponse Response { get; protected set; }

    }
}
