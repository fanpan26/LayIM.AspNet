using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal static class OwinRequestExtensions
    {
        private const string FormCollectionKey = "Microsoft.Owin.Form#collection";

        public static async Task<IFormCollection> ReadFormSafeAsync(this IOwinContext context)
        {
            object previousForm = null;
            if (context.Environment.ContainsKey(FormCollectionKey))
            {
                previousForm = context.Environment[FormCollectionKey];
                context.Environment.Remove(FormCollectionKey);
            }

            try
            {
                if (context.Request.Body.CanSeek)
                {
                    context.Request.Body.Seek(0L, System.IO.SeekOrigin.Begin);
                }
                return await context.Request.ReadFormAsync();
            }
            finally
            {
                if (previousForm != null)
                {
                    context.Environment[FormCollectionKey] = previousForm;
                }
            }
        }
     
    }
}
