using LayIM.Model;
using LayIM.Model.Message.DB;
using LayIM.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Interface
{
    public interface IUser
    {
        BaseListResult GetUserBase(int userId);
        JsonResultModel LoginOrRegister(string loginName, string passWord);
        PageView<LayIMUser> SearchUser(string name = null, int pageindex = 1, int pagesize = 20);
        PageView<LayIMGroup> SearchGroup(string name = null, int pageindex = 1, int pagesize = 20);
        JsonResultModel ApplyToFriend(int applyUserId, int targetId, string other = null);
        JsonResultModel ApplyToGroup(int applyUserId, int targetId,int gid, string other = null);
        JsonResultModel AgreeOrDenyFriend(int userId, int friendId, int groupId = 0);
        IEnumerable<NotifyMessage> GetApplyList(int userId);
    }
}
