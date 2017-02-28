using LayIM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.ViewModel;

namespace LayIM.LogicLayer
{
    public class ElasticSearchService : ISearchService
    {
        public PageView<LayIMGroup> SearchGroup(string name = null, int pageindex = 1, int pagesize = 20)
        {
            throw new NotImplementedException();
        }

        public PageView<LayIMUser> SearchUser(string name = null, int pageindex = 1, int pagesize = 20)
        {
            throw new NotImplementedException();
        }
    }
}
