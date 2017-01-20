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
        }
    }
}
