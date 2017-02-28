using LayIM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.ViewModel;
using LayIM.DataAccessLayer.Interface;

namespace LayIM.LogicLayer
{
    public class SearchService : ISearchService
    {
        IUser _user = Container.LayIMDataAccessLayerContainer.GlobalContainer.Resolve<IUser>();
        public PageView<LayIMGroup> SearchGroup(string name = null, int pageindex = 1, int pagesize = 20)
        {
            return _user.SearchGroup(name, pageindex, pagesize);
        }

        public PageView<LayIMUser> SearchUser(string name = null, int pageindex = 1, int pagesize = 20)
        {
            return _user.SearchUser(name, pageindex, pagesize);
        }
    }
}
