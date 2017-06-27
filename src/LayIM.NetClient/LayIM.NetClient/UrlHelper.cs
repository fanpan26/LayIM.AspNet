using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public class UrlHelper
    {
        private readonly LayimContext _context;

        public UrlHelper(LayimContext context)
        {
            Error.ThrowIfNull(context, nameof(context));

            _context = context;
        }

        public string To(string relativePath)
        {
            return _context.Request.PathBase + relativePath;
        }

        public string HistoryMessage()
        {
            return To("/history");
        }
    }
}
