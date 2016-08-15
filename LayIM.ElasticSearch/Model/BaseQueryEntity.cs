using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.ElasticSearch.Model
{
    public class BaseQueryEntity<T> where T :BaseEntity
    {
        /// <summary>
        /// 命中条数
        /// </summary>
        public long hits { get; set; }
        /// <summary>
        /// 花费时间（单位ms）
        /// </summary>
        public long took { get; set; }
        public IEnumerable<T> list { get; set; }
    }
}
