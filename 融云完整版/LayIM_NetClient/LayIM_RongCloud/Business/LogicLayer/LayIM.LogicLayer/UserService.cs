using LayIM.Container;
using LayIM.DataAccessLayer.Interface;
using LayIM.Lib;
using LayIM.Model;
using LayIM.Model.Enum;
using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.LogicLayer
{
    public class UserService
    {
        private IUser _user => LayIMDataAccessLayerContainer.GlobalContainer.Resolve<IUser>();


        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResultModel GetUserBase(int userId)
        {
            var result = _user.GetUserBase(userId);
            if (result?.mine?.id > 0)
            {
                return new JsonResultModel
                {
                    code = JsonResultType.Success,
                    data = result
                };
            }
            return new JsonResultModel { code = JsonResultType.Failed, msg = "无此用户" };
        }

        /// <summary>
        /// 注册或者用户登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public JsonResultModel LoginOrRegister(string loginName, string passWord)
        {
           
            var result = _user.LoginOrRegister(loginName, passWord);
            if (result.code == JsonResultType.Success)
            {
                //登录成功
                var userId = result.data;
                var user = new LoginUser
                {
                    LoginName = loginName,
                    UserId = Convert.ToInt32(userId)
                };

                var token = user.ToToken();

                CookieHelper.SetCookie("LAYIM_USER_UID", userId.ToString());
                CookieHelper.SetCookie("LAYIM_USER", token);
            }
            return result;
        }

        /// <summary>
        /// 申请为好友关系
        /// </summary>
        /// <param name="applyUserId"></param>
        /// <param name="targetId"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public JsonResultModel ApplyToFriend(int applyUserId, int targetId, string other = null)
        {
            return _user.ApplyToFriend(applyUserId, targetId, other);
        }
        /// <summary>
        /// 申请加群
        /// </summary>
        /// <param name="applyUserId"></param>
        /// <param name="targetId"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public JsonResultModel ApplyToGroup(int applyUserId, int targetId,int gid, string other = null)
        {
            return _user.ApplyToGroup(applyUserId, targetId,gid, other);
        }

        /// <summary>
        /// 获取用户的申请
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResultModel GetApplyByUserId(int userId)
        {
            var result = _user.GetApplyList(userId);
            var res = result.Select(x => new Model.ViewModel.UserNotifyMessage
            {
                id = x.Id,
                avatar = x.Avatar,
                content = GetContentByType(x.Type),
                from = x.FromUserId,
                remark = x.Other,
                time = x.AddTime.ToDateTime().ToTimeText(),
                type = (int)x.Type,
                uid = userId,
                username = x.UserName,result = x.OperateResult
            }).ToList();

            return new JsonResultModel
            {
                code = 0,
                data = res
            };
        }

        public JsonResultModel AgreeOrDenyFriend(int userId, int friendId, int groupId=0)
        {
            return _user.AgreeOrDenyFriend(userId, friendId, groupId);
        }

        private string GetContentByType(NotifyType type,string gName = "")
        {
            string result = string.Empty;
            switch (type) {
                case NotifyType.AgreeToFriend:
                    result = "通过了你的加好友请求";
                    break;
                case NotifyType.AgreeToGroup:
                    result = "通过了你的加群请求";
                    break;
                case NotifyType.ApplyToFriend:
                    result = "请求加你为好友";
                    break;
                case NotifyType.ApplyToGroup:
                    result = "申请加入群 " + gName;
                    break;
                case NotifyType.DenyToFriend:
                    result = "拒绝了你的好友请求";
                    break;
                case NotifyType.DenyToGroup:
                    result = "拒绝了你的加群请求";
                    break;
            }
            return result;
        }
    }
}
