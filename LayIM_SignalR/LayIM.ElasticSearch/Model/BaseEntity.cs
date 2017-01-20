using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.ElasticSearch.Model
{
    [Serializable]
    public class BaseEntity
    {
        public BaseEntity() { }
        public string id { get; set; }

        public override string ToString()
        {
            return new JsonNetSerializer().Serialize(this);

        }
    }
}
