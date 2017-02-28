using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Container
{
    /// <summary>
    /// 全局服务容器
    /// </summary>
    public sealed class LayIMGlobalServiceContainer : ServiceProvider
    {
        public static IServiceContainer GlobalContainer => LayIMContainerBuilder<LayIMGlobalServiceContainer>.GlobalContainer;
    }

    /// <summary>
    /// 数据服务容器
    /// </summary>
    public sealed class LayIMDataAccessLayerContainer : ServiceProvider
    {
        public static IServiceContainer GlobalContainer => LayIMContainerBuilder<LayIMDataAccessLayerContainer>.GlobalContainer;
    }


    /// <summary>
    /// 公共服务容器构造类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LayIMContainerBuilder<T> where T : IServiceContainer,new()
    {
        private static T _container;
        private static object _lock = new object();
        public static IServiceContainer GlobalContainer
        {
            get
            {
                if (_container == null)
                {
                    lock (_lock)
                    {
                        if (_container == null)
                        {
                            _container = new T();
                        }
                    }
                }
                return _container;
            }
        }
    }
}
