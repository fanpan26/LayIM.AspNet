using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class CombinedResourceDispatcher : EmbeddedResourceDispatcher
    {
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly string[] _resourceNames;
        public CombinedResourceDispatcher(string contentType, Assembly assembly, string baseNamespace,params string[] resourceNames) : base(contentType, assembly, null)
        {
            _assembly = assembly;
            _baseNamespace = baseNamespace;
            _resourceNames = resourceNames;
        }

        protected override void WriteResponse(LayimResponse response)
        {
            foreach (var resourceName in _resourceNames) {
                WriteResource(response, _assembly, $"{_baseNamespace}.{resourceName}");
            }
        }
    }
}
