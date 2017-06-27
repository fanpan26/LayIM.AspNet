using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    internal class DefaultUserIdProvider : ILayimUserIdProvider
    {
        public string GetUserId(LayimRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
