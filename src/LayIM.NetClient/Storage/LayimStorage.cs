using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    /// <summary>
    /// 存储抽象类
    /// </summary>
    public abstract class LayimStorage
    {
        private static readonly object storageLock = new object();
        private static LayimStorage _current;

        public static LayimStorage Current
        {
            get
            {
                lock (storageLock)
                {
                    if (_current == null)
                    {
                        throw new InvalidOperationException("LayimStorage 属性尚未初始化");
                    }
                    return _current;
                }
            }
            set
            {
                lock (storageLock)
                {
                    _current = value;
                }
            }
        }

        public abstract IStorageConnection GetConnection();
    }
}
