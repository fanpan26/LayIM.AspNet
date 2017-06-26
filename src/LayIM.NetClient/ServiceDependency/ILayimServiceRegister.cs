using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.ServiceDependency
{
   public interface ILayimServiceRegister
    {
        ILayimServiceRegister Register<TService>(Func<ILayimServiceProvider, TService> serviceCreator) where TService : class;

        ILayimServiceRegister Register<TService, ITmplementation>()
            where TService : class
            where ITmplementation : class, TService; 
    }
}
