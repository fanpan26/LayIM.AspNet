using LayIM.DAL;
using LayIM.Model;
using LayIM.Model.Enum;
using LayIM.Model.Message;
using LayIM.Utils.Consts;
using LayIM.Utils.Extension;
using LayIM.Utils.JsonResult;
using LayIM.Utils.Serialize;
using LayIM.Utils.Single;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.BLL
{
    /// <summary>
    /// 用户添加逻辑 加好友，加群
    /// </summary>
    public class LayIMUserJoinBLL
    {
        public static LayIMUserJoinBLL Instance
        {
            get { return SingleHelper<LayIMUserJoinBLL>.Instance; }
        }

        private LayIMUserJoinDAL _dal = new LayIMUserJoinDAL();

        #region 添加好友申请
        public JsonResultModel AddApply(int userid, int targetid, string other, bool isfriend = true)
        {
            string[] result = _dal.AddApply(userid, targetid, other, isfriend ? ApplyType.Friend : ApplyType.UserJoinGroup);
            return JsonResultHelper.CreateJson(result, true);
        }
        #endregion

        #region 获取需要我处理的好友请求
        public JsonResultModel GetUserNeedHandleApply(int userid)
        {
            var result = _dal.GetUserNeedHandleApply(userid);
            return JsonResultHelper.CreateJson(new { msgcount = result.Rows[0][0].ToInt() });
        }
        #endregion

        #region 处理好友请求
        public JsonResultModel HandleApply(int applyid, int userid, short result, string reason)
        {
            var ds = _dal.HandleApply(applyid, userid, result, reason);
            //转换成相应的推送model id,applytype,targetid,userid
            ApplyHandledMessgae message = ds.Tables[0].Rows.Cast<DataRow>().Select(x => new ApplyHandledMessgae
            {
                id = x["id"].ToInt(),
                result = x["result"].ToShort(),
                applyuid = x["userid"].ToInt(),
                targetid = x["targetid"].ToInt(),
                applytype=x["applytype"].ToShort()
            }).FirstOrDefault();
            if (message.result == 1)
            {
                #region 返回加好友的信息
                if (message.applytype ==ApplyType.Friend.GetHashCode())
                {
                    //好友信息
                    //发送给申请方
                    message.friend = ds.Tables[1].Rows.Cast<DataRow>().Select(x => new UserFriendAddedMessage
                    {
                        groupid = x["groupid"].ToInt(),
                        avatar = x["friendavatar"].ToString(),
                        id = x["friendid"].ToInt(),
                        sign = x["friendremark"].ToString(),
                        type = LayIMConst.LayIMFriendType,
                        username = x["friendname"].ToString()
                    }).FirstOrDefault();
                    //发送给当前操作人
                    message.mine = ds.Tables[2].Rows.Cast<DataRow>().Select(x => new UserFriendAddedMessage
                    {
                        groupid = x["groupid"].ToInt(),
                        avatar = x["friendavatar"].ToString(),
                        id = x["friendid"].ToInt(),
                        sign = x["friendremark"].ToString(),
                        type = LayIMConst.LayIMFriendType,
                        username = x["friendname"].ToString()
                    }).FirstOrDefault();
                }
                #endregion

                #region 返回群信息
                if (message.applytype == ApplyType.UserJoinGroup.GetHashCode())
                {
                    message.group = ds.Tables[1].Rows.Cast<DataRow>().Select(x => new UserGroupCreatedMessage
                    {
                        id = x["groupid"].ToInt(),
                        groupname = x["groupname"].ToString(),
                        avatar = x["avatar"].ToString(),
                        memebers = x["members"].ToInt(),
                        type = LayIMConst.LayIMGroupType
                    }).FirstOrDefault();
                    //增加申请人的姓名
                    message.friend = new UserFriendAddedMessage
                    {
                        username = ds.Tables[1].Rows[0]["applyusername"].ToString()
                    };
                }
                #endregion
            }
            return JsonResultHelper.CreateJson(message, true);
        }
        #endregion
    }
}
