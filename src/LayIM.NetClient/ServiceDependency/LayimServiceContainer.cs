using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient.ServiceDependency
{
    /// <summary>
    /// 一个简单版本的注入
    /// </summary>
    public class LayimServiceContainer : ILayimServiceContainer
    {

        private readonly object serviceLock = new object();
        /// <summary>
        /// 保存创建工厂的实例
        /// </summary>

        private readonly ConcurrentDictionary<Type, object> factories = new ConcurrentDictionary<Type, object>();
        /// <summary>
        /// 保存注册类型
        /// </summary>

        private readonly ConcurrentDictionary<Type, Type> registrations = new ConcurrentDictionary<Type, Type>();
        /// <summary>
        /// 保存实例
        /// </summary>

        private readonly ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();


        #region 私有方法
        private bool IsServiceRegistered(Type serviceType)
        {
            return factories.ContainsKey(serviceType) || registrations.ContainsKey(serviceType);
        }

        private object Resolve(Type serviceType)
        {
            return GetType()
                .GetMethod("Resolve", new Type[0])
                .MakeGenericMethod(serviceType)
                .Invoke(this, new object[0]);
        }

        private object CreateServiceInstance(Type type)
        {
            var constructors = type.GetConstructors();
            var parameters = constructors[0]
                .GetParameters()
                .Select(parameter => Resolve(parameter.ParameterType))
                .ToArray();

            return constructors[0].Invoke(parameters);
        }

        #endregion

        public ILayimServiceRegister Register<TService>(Func<ILayimServiceProvider, TService> serviceCreator) where TService : class
        {
            lock (serviceLock) {
                var serviceType = typeof(TService);
                if (IsServiceRegistered(serviceType)) {
                    return this;
                }

                factories.TryAdd(serviceType, serviceCreator);

                return this;
            }
        }
        public ILayimServiceRegister Register<TService, ITmplementation>()
            where TService : class
            where ITmplementation : class, TService
        {
            lock (serviceLock)
            {
                var serviceType = typeof(TService);
                var implType = typeof(ITmplementation);
                if (IsServiceRegistered(serviceType)) { return this; }

                if (!serviceType.IsAssignableFrom(implType))
                {
                    throw new Exception(string.Format("类型 {0} 不继承接口 {1}", implType.Name, serviceType.Name));
                }
                var constructors = implType.GetConstructors();
                if (constructors.Length != 1)
                {
                    throw new Exception(string.Format("服务实例必须有且只有一个构造方法.当前实例 {0} 拥有 {1} 个", implType.Name, constructors.Length.ToString()));
                }

                registrations.TryAdd(serviceType, implType);
                return this;
            }
        }

        public TService Resolve<TService>() where TService : class
        {
            var serviceType = typeof(TService);
            object service;

            if (instances.TryGetValue(serviceType, out service)) return (TService)service;

            lock (serviceLock)
            {
                if (instances.TryGetValue(serviceType, out service))
                {
                    return (TService)service;
                }

                if (registrations.ContainsKey(serviceType))
                {
                    var implType = registrations[serviceType];
                    service = CreateServiceInstance(implType);
                    instances.TryAdd(serviceType, service);
                }
                else if (factories.ContainsKey(serviceType))
                {
                    service = ((Func<ILayimServiceProvider, TService>)factories[serviceType])(this);
                    instances.TryAdd(serviceType, service);
                }
                else
                {
                    throw new Exception(string.Format("服务类型 {0} 未注册", serviceType.Name));
                }

                return (TService)service;
            }
        }

       
    }
}
