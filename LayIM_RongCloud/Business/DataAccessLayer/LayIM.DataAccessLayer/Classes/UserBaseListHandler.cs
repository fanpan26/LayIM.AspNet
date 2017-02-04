using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LayIM.DataAccessLayer.Interface;

namespace LayIM.DataAccessLayer.Classes
{
    public class UserBaseListHandler : IMultipleHandler<BaseListResult>
    {
        public BaseListResult Handle(SqlMapper.GridReader reader)
        {
            var result = new BaseListResult();
            //用户本人数据
            result.mine = reader.ReadFirstOrDefault<UserEntity>();
            //处理friend逻辑 start
            var friend = reader.Read<FriendGroupEntity>();
            var groupUsers = reader.Read<GroupUserEntity>();
            friend.ToList().ForEach(f =>
            {
                //每一组的人分配给各个组
                f.list = groupUsers?.Where(x => x.groupid == f.id);
            });
            result.friend = friend;
            //处理friend逻辑 end
            //读取用户所在群
            result.group = reader.Read<GroupEntity>();
            return result;
        }
    }
}
