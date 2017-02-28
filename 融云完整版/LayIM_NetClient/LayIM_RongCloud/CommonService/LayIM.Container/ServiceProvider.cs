using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Container
{
    /// <summary>
    /// 服务提供类的实现,类似服务工厂
    /// </summary>
    public class ServiceProvider : IServiceContainer
    {
        /// <summary>
        /// 锁
        /// </summary>
        private readonly object syncLock = new object();
        /// <summary>
        /// 存储单例工厂
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> factories = new ConcurrentDictionary<Type, object>();
        /// <summary>
        /// 存储注册类型
        /// </summary>
        private readonly ConcurrentDictionary<Type, Type> registrations = new ConcurrentDictionary<Type, Type>();
        /// <summary>
        /// 存储实例
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();


        #region 私有方法
        /// <summary>
        /// 判断服务是否已经被注册过
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private bool ServiceIsRegistered(Type serviceType)
        {
            //判断是否在工厂或者注册库内已经注册过该类型
            return factories.ContainsKey(serviceType) || registrations.ContainsKey(serviceType);
        }
        #endregion

        /// <summary>
        /// 注册工厂
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceCreator"></param>
        /// <returns></returns>
        public IServiceRegister Register<TService>(Func<IServiceProvider, TService> serviceCreator) where TService : class
        {
            lock (syncLock) {
                var serviceType = typeof(TService);
                if (ServiceIsRegistered(serviceType)) { return this; }

                //将服务添加到存储器中
                factories.TryAdd(serviceType, serviceCreator);
                return this;
            }
        }

        /// <summary>
        /// 注册实例类
        /// </summary>
        /// <typeparam name="TService">IService 必须实现一个构造器</typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public IServiceRegister Register<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            lock (syncLock)
            {
                var serviceType = typeof(TService);
                var implType = typeof(TImplementation);

                if (ServiceIsRegistered(serviceType)) { return this; }

                //如果注册的类不是继承自TService接口，抛出异常
                if (!serviceType.IsAssignableFrom(implType))
                {
                    throw new Exception(string.Format("类型 {0} 不继承接口 {1}", implType.Name, serviceType.Name));
                }
                //获取构造方法，必须只有一个构造方法（why？）
                var constructors = implType.GetConstructors();
                if (constructors.Length != 1)
                {
                    throw new Exception(string.Format("服务实例必须有且只有一个构造方法.当前实例 {0} 拥有 {1} 个", implType.Name, constructors.Length.ToString()));
                }
                //添加
                registrations.TryAdd(serviceType, implType);
                return this;
            }
        }

        /// <summary>
        /// 返回一个实例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public virtual TService Resolve<TService>() where TService : class
        {
            var serviceType = typeof(TService);
            object service;
            //如果实例存储器中已经存在该实例，直接返回
            if (instances.TryGetValue(serviceType, out service))
                return (TService)service;
            lock (syncLock)
            {
                //加锁，再次判断
                if (instances.TryGetValue(serviceType, out service))
                { 
                    return (TService)service;
                }

                //如果注册器中存在该类型，创建该实例，并加入到实例存储器中
                if (registrations.ContainsKey(serviceType))
                {
                    var implementationType = registrations[serviceType];
                    service = CreateServiceInstance(implementationType);
                    instances.TryAdd(serviceType, service);
                }
                else if (factories.ContainsKey(serviceType))
                {
                    service = ((Func<IServiceProvider, TService>)factories[serviceType])(this);
                    instances.TryAdd(serviceType, service);
                }
                else
                {
                    throw new Exception(string.Format("服务类型 {0} 未注册", serviceType.Name));
                }
                return (TService)service;
            }
        }

        private object Resolve(Type serviceType)
        {
            return GetType()
                .GetMethod("Resolve", new Type[0])
                .MakeGenericMethod(serviceType)
                .Invoke(this, new object[0]);
        }

        private object CreateServiceInstance(Type implementationType)
        {
            //获取构造器
            var constructors = implementationType.GetConstructors();
            //遍历构造器中的参数类型，获取参数
            var parameters = constructors[0]
                .GetParameters()
                .Select(parameterInfo => Resolve(parameterInfo.ParameterType))
                .ToArray();

            //创建实例方法
            return constructors[0].Invoke(parameters);
        }
    }
}
