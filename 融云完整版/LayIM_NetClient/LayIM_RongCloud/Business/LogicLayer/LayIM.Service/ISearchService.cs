using LayIM.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Service
{
    /// <summary>
    /// 查询接口
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// 搜索已经注册的用户
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        PageView<LayIMUser> SearchUser(string name = null, int pageindex = 1, int pagesize = 20);
        PageView<LayIMGroup> SearchGroup(string name = null, int pageindex = 1, int pagesize = 20);
    }
}
