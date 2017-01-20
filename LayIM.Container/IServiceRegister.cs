using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Container
{
    /// <summary>
    /// 服务注册接口
    /// </summary>
    public interface IServiceRegister
    {
        /// <summary>
        /// 注册一个服务工厂
        /// </summary>
        /// <typeparam name="TService">工厂类</typeparam>
        /// <param name="serviceCreator">创建一个新的接口实例</param>
        /// <returns>返回当前注册工厂,以便调用其他的register方法</returns>
        IServiceRegister Register<TService>(Func<IServiceProvider, TService> serviceCreator) where TService : class;

        /// <summary>
        /// 注册服务类型和服务实例
        /// </summary>
        /// <typeparam name="TService">接口实例</typeparam>
        /// <typeparam name="TImplementation">类型继承自TService</typeparam>
        /// <returns>返回当前注册工厂,以便调用其他的register方法</returns>
        IServiceRegister Register<TService, TImplementation>()
            where TService : class
            //TImplementation 继承自 TService
            where TImplementation : class, TService;
    }
}
