using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.ServiceDependency
{
    public interface ILayimServiceContainer : ILayimServiceProvider, ILayimServiceRegister
    {
    }
}
