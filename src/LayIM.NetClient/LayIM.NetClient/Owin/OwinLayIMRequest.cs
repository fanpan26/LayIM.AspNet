using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LayIM.NetClient
{
   internal sealed class OwinLayimRequest : LayimRequest
    {

        private readonly IOwinContext _context;

        public OwinLayimRequest(IDictionary<string, object> environment)
        {
            Error.ThrowIfNull(environment, nameof(environment));

            _context = new OwinContext(environment);
        }

        public override string Method => _context.Request.Method;

        public override string Path => _context.Request.Path.Value;

        public override string PathBase => _context.Request.PathBase.Value;

        public override string LocalIpAddress => _context.Request.LocalIpAddress;

        public override string RemoteIpAddress => _context.Request.RemoteIpAddress;

        public override RequestCookieCollection Cookies => _context.Request.Cookies;

        public override IReadableStringCollection Query => _context.Request.Query;

        public override IHeaderDictionary Header => _context.Request.Headers;

        public override Stream Body => _context.Request.Body;


        public override async Task<IList<string>> GetFormValuesAsync(string key)
        {
            var form = await _context.ReadFormSafeAsync();
            return form.GetValues(key) ?? new List<string>();
        }

        public override async Task<IDictionary<string, string>> GetFormKeyValuesAsync(params string[] key)
        {
            if (key == null) { return null; }
            var form = await _context.ReadFormSafeAsync();
            var dict = new Dictionary<string, string>();
            foreach (var k in key) {
                if (k == null) { continue; }
                dict.Add(k, form.Get(k));
            }
            return dict;
        }
        public override string GetQuery(string key) => _context.Request.Query[key];
        
    }
}
