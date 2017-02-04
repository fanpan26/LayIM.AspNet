using Dapper;
using LayIM.DataAccessLayer.Helper;
using LayIM.DataAccessLayer.Interface;
using LayIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DataAccessLayer.Classes
{
    public class User : IUser
    {
        private IMultipleHandler<BaseListResult> _handler;
        public User(IMultipleHandler<BaseListResult> handler)
        {
            _handler = handler;
        }
        /// <summary>
        /// 获取基本用户数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BaseListResult GetUserBase(int userId)
        {
            const string spName = "[Proc_LayIM_GetUserInitInfo_RC]";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userid", userId, DbType.Int32);
            if (_handler == null)
            {
                throw new ArgumentOutOfRangeException("IMultipleHandler<BaseListResult> 服务未注册");
            }
            return DBHelper.QueryMultiple(spName, parameter, CommandType.StoredProcedure, _handler);

            #region 使用Func回调返回
            //return DBHelper.QueryMultiple(spName, parameter, CommandType.StoredProcedure, reader =>
            //{
            //    var result = new BaseListResult();
            //    //用户本人数据
            //    result.mine = reader.ReadFirstOrDefault<UserEntity>();
            //    //处理friend逻辑 start
            //    var friend = reader.Read<FriendGroupEntity>();
            //    var groupUsers = reader.Read<GroupUserEntity>();
            //    friend.ToList().ForEach(f =>
            //    {
            //        //每一组的人分配给各个组
            //        f.list = groupUsers?.Where(x => x.groupid == f.id);
            //    });
            //    result.friend = friend;
            //    //处理friend逻辑 end
            //    //读取用户所在群
            //    result.group = reader.Read<GroupEntity>();
            //    return result;
            //});
            #endregion
        }
    }
}
