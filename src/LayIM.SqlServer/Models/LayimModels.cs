using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.SqlServer.Models
{

    #region 主要列表数据
    public class BaseModel
    {
        public long id { get; set; }
    }
    public class AvatarModel : BaseModel
    {
        public string avatar { get; set; }
    }
    public class UserModel : AvatarModel
    {
        public string sign { get; set; }
        public string username { get; set; }
    }

    public class GroupUserModel : UserModel
    {
        public int gid { get; set; }
    }

    public class FriendGroupModel : BaseModel
    {
        public string groupname { get; set; }
        public IEnumerable<UserModel> list { get; set; }
    }

    public class BigGroupModel : AvatarModel
    {
        public string groupname { get; set; }
    }

    /// <summary>
    /// 好友列表，群组列表，好友分组等信息
    /// </summary>
    public class BaseListResult
    {
        public UserModel mine { get; set; }

        public IEnumerable<BigGroupModel> group { get; set; }

        public IEnumerable<FriendGroupModel> friend { get; set; }
    }

    /// <summary>
    /// 群组获取群员列表
    /// </summary>
    public class GroupMemberResult
    {
        public UserModel owner { get; set; }
        public int members
        {
            get
            {
                if (list == null) { return 0; }
                return list.Count();
            }
        }
        public IEnumerable<UserModel> list { get; set; }
    }
    #endregion

    #region 审核申请
    internal class ApplyInfo
    {
        public int Type { get; set; }
        public long GroupId { get; set; }
        public long UserId { get; set; }
    }
    #endregion
}
