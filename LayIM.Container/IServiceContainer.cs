using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Container
{
    /// <summary>
    /// 对外接口Container，继承自IServiceProvider,IServiceRegister
    /// </summary>
    public interface IServiceContainer : IServiceProvider, IServiceRegister { }
}
