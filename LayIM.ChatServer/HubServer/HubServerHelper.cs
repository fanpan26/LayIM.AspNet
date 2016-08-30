using LayIM.BLL;
using LayIM.ChatServer.Hubs;
using LayIM.Model.Enum;
using LayIM.Model.Message;
using LayIM.Utils.Extension;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LayIM.ChatServer.HubServer
{
    public class HubServerHelper
    {
        //获取当前的Hub对象
       static IHubContext hub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<LayIMHub>();
            }
        }
        public static void SendMessage(object message, string userId, ChatToClientType type, bool moreUser = false)
        {
            //构造消息体
            ToClientMessageResult result = new ToClientMessageResult
            {
                msg = message,
                msgtype = type,
                other = null
            };
           
            // 给多个用户发送消息
            if (moreUser)
            {
                hub.Clients.Users(userId.Split(',').ToList()).receiveMessage(result);
            }
            else
            {
                //给单个用户发送消息
                hub.Clients.User(userId).receiveMessage(result);
            }
        }

        /// <summary>
        /// 发送请求被处理的消息
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(ApplyHandledMessgae message)
        {
            short agreeFlag = 1;
            short refuseFlag = 2;
            //只有有消息，并且同意
            if (message.id > 0)
            {
                var userid = message.applyuid.ToString();
                if (message.result == refuseFlag)
                {
                    //如果是被拒绝，只需要发送一条提示消息即可
                  
                    var msg = new ApplySendedMessage
                    {
                        msg = "您的" + (message.applytype == 0 ? "好友" : "加群") + "请求已经被处理，请点击查看详情"
                    };
                    SendMessage(msg, userid, ChatToClientType.ApplyHandledToClient);

                }
                else if (message.result == agreeFlag)
                {
                    //如果同意了，判断如果是加群，需要给群发送消息：某某某已经加入群，如果是加好友，发送一条消息，我们已经成为好友了，开始聊天吧。
                    var msg = "您的" + (message.applytype == 0 ? "好友" : "加群") + "请求已经被处理，请点击查看详情";
                    if (message.applytype == ApplyType.Friend.GetHashCode())
                    {
                        //这里的friend是为了配合 LayIM的 addList接口
                        SendMessage(new { friend = message.friend, msg =msg  }, userid, ChatToClientType.ApplyHandledToClient);
                    }
                    else {
                        //发送群信息 group也是为了配合Layim的addList接口
                        SendMessage(new { group = message.group, msg = msg }, userid, ChatToClientType.ApplyHandledToClient);
                    }

                }
            }
        }

        /// <summary>
        /// 发送用户上下线的消息
        /// </summary>
        public static void SendUserOnOffLineMessage(string userId,bool online=true)
        {
            int userid = userId.ToInt();
            //1.获取用户的所有好友

            var users = LayimUserBLL.Instance.GetUserFriends(userid);
            //没有好友，不发消息
            var friends = users.Split(new string[] { "$LAYIM$" }, StringSplitOptions.RemoveEmptyEntries);
            if (friends.Length == 2)
            {
                var avatar = friends[0];
                var notifyUsers = friends[1];


                //2.发送用户上下线通知
                UserOnOffLineMessage message = new UserOnOffLineMessage
                {
                    avatar = avatar,
                    online = online,
                    userid = userid
                };
                SendMessage(message, notifyUsers, ChatToClientType.UserOnOffLineToClient, true);
            }
        }
    }
}