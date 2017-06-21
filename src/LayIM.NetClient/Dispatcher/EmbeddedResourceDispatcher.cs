using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class EmbeddedResourceDispatcher : ILayimDispatcher
    {

        private readonly Assembly _assembly;
        private readonly string _resourceName;
        private readonly string _contentType;


        public EmbeddedResourceDispatcher(string contentType, Assembly assembly, string resourceName)
        {
            Error.ThrowIfNull(contentType, nameof(contentType));
            Error.ThrowIfNull(assembly, nameof(assembly));

            _assembly = assembly;
            _resourceName = resourceName;
            _contentType = contentType;
        }

        public Task Dispatch(LayimContext context)
        {
            context.Response.ContentType = _contentType;
            context.Response.SetExpire(DateTimeOffset.Now.AddYears(1));

            WriteResponse(context.Response);

            return Task.FromResult(true);
        }

        protected virtual void WriteResponse(LayimResponse response)
        {
            WriteResource(response, _assembly, _resourceName);
        }

        protected void WriteResource(LayimResponse response, Assembly assembly, string resourceName)
        {
            using (var inputStream = assembly.GetManifestResourceStream(resourceName)) {
                if (inputStream == null) {
                    throw new ArgumentException($@"在{assembly}中{resourceName} 资源未找到.");
                }
                inputStream.CopyTo(response.Body);
            }
        }
    }
}
