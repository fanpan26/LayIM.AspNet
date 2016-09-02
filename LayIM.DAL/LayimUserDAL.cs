using LayIM.DAL.DB;
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
    public class LayimUserDAL : BaseDB
    {
        #region  获取用户基本信息，个人信息，群组信息，好友组信息等
        /// <summary>
        /// 获取用户基本信息，个人信息，群组信息，好友组信息等
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataSet GetChatRoomBaseInfo(int userid)
        {
            const string procedureName = "Proc_LayIM_GetUserInitInfo";
            var parameters = new SqlParameter[] { MakeParameterInt("userid", userid) };
            return ExecuteDataSetStoreProcedure(procedureName, parameters);
        }
        #endregion

        #region 获取用户的群组的群员信息
        public DataSet GetGroupMembers(int groupid)
        {
            const string procedureName = "Proc_LayIM_GetGroupMembers";
            var parameters = new SqlParameter[] { MakeParameterInt("groupid", groupid) };
            return ExecuteDataSetStoreProcedure(procedureName, parameters);
        }
        #endregion

        #region 用户登录或者注册流程
        /// <summary>
        /// 用户登陆或者注册，返回用户id如果为 0 说明密码错误
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public DataSet UserLoginOrRegister(string loginName, string loginPwd,string nickName="")
        {
            const string procedureName = "Proc_LayIM_UserLoginOrRegister";
            var parameters = new SqlParameter[] {
                MakeParameterVarChar("loginname",loginName ),
                MakeParameterVarChar("loginpwd",loginPwd),
                MakeParameterVarChar("nickname",nickName)
            };
            var ds = ExecuteDataSetStoreProcedure(procedureName, parameters);
            return ds;
        }
        #endregion

        #region 用户创建群
        public DataTable CreateGroup(string groupName, string groupDesc, int userid)
        {
            const string procedureName = "Proc_LayIM_CreateGroup";
            var parameters = new SqlParameter[] {
                MakeParameterVarChar("groupname",groupName ),
                MakeParameterVarChar("groupdesc",groupDesc),
                MakeParameterInt("userid",userid)
            };
            return ExecuteDataTableStoreProcedure(procedureName, parameters);
        }
        #endregion

        #region 获取与用户有关的消息
        public DataTable GetUserApplyMessage(int userid)
        {
            string procedureName = "Proc_LayIM_GetUserApplyRecord";
            var parameters = new SqlParameter[] { MakeParameterInt("userid", userid) };
            return ExecuteDataTableStoreProcedure(procedureName, parameters);
        }
        #endregion

        #region 获取某个用户的好友列表
        /// <summary>
        /// 获取某个用户的好友列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns>返回格式如下 ""或者 "10001,10002,10003"</returns>
        public string GetUserFriends(int userid)
        {
            string sql = "SELECT CONVERT(VARCHAR(20),friendid)+',' FROM dbo.v_layim_friend_group_detail_info WHERE userid=@userid FOR XML PATH('');  SELECT headphoto AS avatar FROM dbo.layim_user WHERE id=@userid ";
            var parameters = new SqlParameter[] {
                MakeParameterInt("userid",userid)
            };
            var ds = ExecuteDataSetSQL(sql, parameters);
            var dt = ds.Tables[0];
            string friends = "";
            if (dt.Rows.Count > 0)
            {
                var ids = dt.Rows[0][0].ToString();
                if (ids.Length > 0)
                {
                    //去掉最后一个逗号
                    friends = ids.Substring(0, ids.Length - 1);
                }
            }
            var avatar = ds.Tables[1].Rows[0][0].ToString();
            return avatar + "$LAYIM$" + friends;
           
        }
        #endregion

        #region 获取用户所有加的群
        public string[] GetUserAllGroups(int userId)
        {
            string sql = "SELECT gid FROM dbo.layim_group_detail WHERE [uid]=" + userId;
            var dt = ExecuteDateTableSQL(sql);
            return dt.Rows.Cast<DataRow>().Select(x => x["gid"].ToString()).ToArray();
        }
        #endregion
    }
}
