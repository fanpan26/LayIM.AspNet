using LayIM.DAL.DB;
using LayIM.Model.Enum;
using LayIM.Utils.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.DAL
{
    public class LayIMUserJoinDAL : BaseDB
    {
        #region 添加好友申请或者群组申请
        public string[] AddApply(int userid, int targetid, string other, ApplyType applytype)
        {
            const string procedureName = "Proc_LayIM_AddApply";
            var parameters = new SqlParameter[] { MakeParameterInt("applyuid", userid), MakeParameterInt("targetid", targetid), MakeParameterInt("applytype", applytype.GetHashCode()), MakeParameterVarChar("other", other) };
            var result = ExecuteDataTableStoreProcedure(procedureName, parameters);
            if (result.Rows.Count > 0) {
                return result.Rows.Cast<DataRow>().Select(x => x[0].ToString()).ToArray();
            }
            return new string[] { "" };
        }
        #endregion

        #region 获取需要我处理的好友请求
        public DataTable GetUserNeedHandleApply(int userid)
        {
            string procedureName = "Proc_LayIM_GetUserIfHasMessage";
            var parameters = new SqlParameter[] { MakeParameterInt("userid", userid) };
            return ExecuteDataTableStoreProcedure(procedureName, parameters);
        }
        #endregion

        #region 同意或者拒绝或者忽略好友请求
        public DataSet HandleApply(int applyid,int userid,short result,string reason)
        {
            string procedureName = "Proc_LayIM_HandleApply";
            var parameters = new SqlParameter[] {
                MakeParameterInt("applyid", applyid),
                MakeParameterInt("userid", userid),
                MakeParameterTinyInt("result", result),
                MakeParameterVarChar("reason", reason)
            };
            return ExecuteDataSetStoreProcedure(procedureName, parameters);
        }
        #endregion

    }
}
