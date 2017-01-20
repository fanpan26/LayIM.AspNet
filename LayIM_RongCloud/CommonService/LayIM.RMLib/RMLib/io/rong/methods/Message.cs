using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web;

using donet.io.rong.models;
using donet.io.rong.util;
using donet.io.rong.messages;
using Newtonsoft.Json;

namespace donet.io.rong.methods
{

    public class Message
    {

        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;

        public Message(String appKey, String appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;

        }

        /**
	 	 * 发送单聊消息方法（一个用户向另外一个用户发送消息，单条消息最大 128k。每分钟最多发送 6000 条信息，每次发送用户上限为 1000 人，如：一次发送 1000 人时，示为 1000 条消息。） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id。（必传）
	 	 * @param  toUserId:接收用户 Id，可以实现向多人发送消息，每次上限为 1000 人。（必传）
	 	 * @param  voiceMessage:消息。
	 	 * @param  pushContent:定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息。如果为自定义消息，则 pushContent 为自定义消息显示的 Push 内容，如果不传则用户不会收到 Push 通知。（可选）
	 	 * @param  pushData:针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。（可选）
	 	 * @param  count:针对 iOS 平台，Push 时用来控制未读消息显示数，只有在 toUserId 为一个用户 Id 的时候有效。（可选）
	 	 * @param  verifyBlacklist:是否过滤发送人黑名单列表，0 表示为不过滤、 1 表示为过滤，默认为 0 不过滤。（可选）
	 	 * @param  isPersisted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行存储，0 表示为不存储、 1 表示为存储，默认为 1 存储消息。（可选）
	 	 * @param  isCounted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行未读消息计数，0 表示为不计数、 1 表示为计数，默认为 1 计数，未读消息数增加 1。（可选）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishPrivate(String fromUserId, String[] toUserId, VoiceMessage message, String pushContent, String pushData, String count, int verifyBlacklist, int isPersisted, int isCounted)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (toUserId == null)
            {
                throw new ArgumentNullException("Paramer 'toUserId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            for (int i = 0; i < toUserId.Length; i++)
            {
                String child = toUserId[i];
                postStr += "toUserId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
            }

            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr += "pushContent=" + HttpUtility.UrlEncode(pushContent == null ? "" : pushContent, Encoding.UTF8) + "&";
            postStr += "pushData=" + HttpUtility.UrlEncode(pushData == null ? "" : pushData, Encoding.UTF8) + "&";
            postStr += "count=" + HttpUtility.UrlEncode(count == null ? "" : count, Encoding.UTF8) + "&";
            postStr += "verifyBlacklist=" + HttpUtility.UrlEncode(Convert.ToString(verifyBlacklist) == null ? "" : Convert.ToString(verifyBlacklist), Encoding.UTF8) + "&";
            postStr += "isPersisted=" + HttpUtility.UrlEncode(Convert.ToString(isPersisted) == null ? "" : Convert.ToString(isPersisted), Encoding.UTF8) + "&";
            postStr += "isCounted=" + HttpUtility.UrlEncode(Convert.ToString(isCounted) == null ? "" : Convert.ToString(isCounted), Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/private/publish.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 发送单聊模板消息方法（一个用户向多个用户发送不同消息内容，单条消息最大 128k。每分钟最多发送 6000 条信息，每次发送用户上限为 1000 人。） 
	 	 * 
	 	 * @param  templateMessage:单聊模版消息。
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishTemplate(TemplateMessage templateMessage)
        {

            if (templateMessage == null)
            {
                throw new ArgumentNullException("Paramer 'templateMessage' is required");
            }

            String postStr = "";
            postStr = JsonConvert.SerializeObject(templateMessage);
            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/private/publish_template.json", postStr, "application/json"));
        }

        /**
	 	 * 发送系统消息方法（一个用户向一个或多个用户发送系统消息，单条消息最大 128k，会话类型为 SYSTEM。每秒钟最多发送 100 条消息，每次最多同时向 100 人发送，如：一次发送 100 人时，示为 100 条消息。） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id。（必传）
	 	 * @param  toUserId:接收用户 Id，提供多个本参数可以实现向多人发送消息，上限为 1000 人。（必传）
	 	 * @param  txtMessage:发送消息内容（必传）
	 	 * @param  pushContent:如果为自定义消息，定义显示的 Push 内容，内容中定义标识通过 values 中设置的标识位内容进行替换.如消息类型为自定义不需要 Push 通知，则对应数组传空值即可。（可选）
	 	 * @param  pushData:针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。如不需要 Push 功能对应数组传空值即可。（可选）
	 	 * @param  isPersisted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行存储，0 表示为不存储、 1 表示为存储，默认为 1 存储消息。（可选）
	 	 * @param  isCounted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行未读消息计数，0 表示为不计数、 1 表示为计数，默认为 1 计数，未读消息数增加 1。（可选）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut PublishSystem(String fromUserId, String[] toUserId, TxtMessage message, String pushContent, String pushData, int isPersisted, int isCounted)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (toUserId == null)
            {
                throw new ArgumentNullException("Paramer 'toUserId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            for (int i = 0; i < toUserId.Length; i++)
            {
                String child = toUserId[i];
                postStr += "toUserId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
            }

            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr += "pushContent=" + HttpUtility.UrlEncode(pushContent == null ? "" : pushContent, Encoding.UTF8) + "&";
            postStr += "pushData=" + HttpUtility.UrlEncode(pushData == null ? "" : pushData, Encoding.UTF8) + "&";
            postStr += "isPersisted=" + HttpUtility.UrlEncode(Convert.ToString(isPersisted) == null ? "" : Convert.ToString(isPersisted), Encoding.UTF8) + "&";
            postStr += "isCounted=" + HttpUtility.UrlEncode(Convert.ToString(isCounted) == null ? "" : Convert.ToString(isCounted), Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/system/publish.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 发送系统模板消息方法（一个用户向一个或多个用户发送系统消息，单条消息最大 128k，会话类型为 SYSTEM.每秒钟最多发送 100 条消息，每次最多同时向 100 人发送，如：一次发送 100 人时，示为 100 条消息。） 
	 	 * 
	 	 * @param  templateMessage:系统模版消息。
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishSystemTemplate(TemplateMessage templateMessage)
        {

            if (templateMessage == null)
            {
                throw new ArgumentNullException("Paramer 'templateMessage' is required");
            }

            String postStr = "";
            postStr = JsonConvert.SerializeObject(templateMessage);
            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/system/publish_template.json", postStr, "application/json"));
        }

        /**
	 	 * 发送群组消息方法（以一个用户身份向群组发送消息，单条消息最大 128k.每秒钟最多发送 20 条消息，每次最多向 3 个群组发送，如：一次向 3 个群组发送消息，示为 3 条消息。） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id 。（必传）
	 	 * @param  toGroupId:接收群Id，提供多个本参数可以实现向多群发送消息，最多不超过 3 个群组。（必传）
	 	 * @param  txtMessage:发送消息内容（必传）
	 	 * @param  pushContent:定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息. 如果为自定义消息，则 pushContent 为自定义消息显示的 Push 内容，如果不传则用户不会收到 Push 通知。（可选）
	 	 * @param  pushData:针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。（可选）
	 	 * @param  isPersisted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行存储，0 表示为不存储、 1 表示为存储，默认为 1 存储消息。（可选）
	 	 * @param  isCounted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行未读消息计数，0 表示为不计数、 1 表示为计数，默认为 1 计数，未读消息数增加 1。（可选）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishGroup(String fromUserId, String[] toGroupId, TxtMessage message, String pushContent, String pushData, int isPersisted, int isCounted)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (toGroupId == null)
            {
                throw new ArgumentNullException("Paramer 'toGroupId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            for (int i = 0; i < toGroupId.Length; i++)
            {
                String child = toGroupId[i];
                postStr += "toGroupId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
            }

            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr += "pushContent=" + HttpUtility.UrlEncode(pushContent == null ? "" : pushContent, Encoding.UTF8) + "&";
            postStr += "pushData=" + HttpUtility.UrlEncode(pushData == null ? "" : pushData, Encoding.UTF8) + "&";
            postStr += "isPersisted=" + HttpUtility.UrlEncode(Convert.ToString(isPersisted) == null ? "" : Convert.ToString(isPersisted), Encoding.UTF8) + "&";
            postStr += "isCounted=" + HttpUtility.UrlEncode(Convert.ToString(isCounted) == null ? "" : Convert.ToString(isCounted), Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/group/publish.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 发送讨论组消息方法（以一个用户身份向讨论组发送消息，单条消息最大 128k，每秒钟最多发送 20 条消息.） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id。（必传）
	 	 * @param  toDiscussionId:接收讨论组 Id。（必传）
	 	 * @param  txtMessage:发送消息内容（必传）
	 	 * @param  pushContent:定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息. 如果为自定义消息，则 pushContent 为自定义消息显示的 Push 内容，如果不传则用户不会收到 Push 通知。（可选）
	 	 * @param  pushData:针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData.（可选）
	 	 * @param  isPersisted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行存储，0 表示为不存储、 1 表示为存储，默认为 1 存储消息.（可选）
	 	 * @param  isCounted:当前版本有新的自定义消息，而老版本没有该自定义消息时，老版本客户端收到消息后是否进行未读消息计数，0 表示为不计数、 1 表示为计数，默认为 1 计数，未读消息数增加 1。（可选）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishDiscussion(String fromUserId, String toDiscussionId, TxtMessage message, String pushContent, String pushData, int isPersisted, int isCounted)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (toDiscussionId == null)
            {
                throw new ArgumentNullException("Paramer 'toDiscussionId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            postStr += "toDiscussionId=" + HttpUtility.UrlEncode(toDiscussionId == null ? "" : toDiscussionId, Encoding.UTF8) + "&";
            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr += "pushContent=" + HttpUtility.UrlEncode(pushContent == null ? "" : pushContent, Encoding.UTF8) + "&";
            postStr += "pushData=" + HttpUtility.UrlEncode(pushData == null ? "" : pushData, Encoding.UTF8) + "&";
            postStr += "isPersisted=" + HttpUtility.UrlEncode(Convert.ToString(isPersisted) == null ? "" : Convert.ToString(isPersisted), Encoding.UTF8) + "&";
            postStr += "isCounted=" + HttpUtility.UrlEncode(Convert.ToString(isCounted) == null ? "" : Convert.ToString(isCounted), Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/discussion/publish.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 发送聊天室消息方法（一个用户向聊天室发送消息，单条消息最大 128k。每秒钟限 100 次。） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id。（必传）
	 	 * @param  toChatroomId:接收聊天室Id，提供多个本参数可以实现向多个聊天室发送消息。（必传）
	 	 * @param  txtMessage:发送消息内容（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut publishChatroom(String fromUserId, String[] toChatroomId, TxtMessage message)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (toChatroomId == null)
            {
                throw new ArgumentNullException("Paramer 'toChatroomId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            for (int i = 0; i < toChatroomId.Length; i++)
            {
                String child = toChatroomId[i];
                postStr += "toChatroomId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
            }

            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/chatroom/publish.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 发送广播消息方法（发送消息给一个应用下的所有注册用户，如用户未在线会对满足条件（绑定手机终端）的用户发送 Push 信息，单条消息最大 128k，会话类型为 SYSTEM。每小时只能发送 1 次，每天最多发送 3 次。） 
	 	 * 
	 	 * @param  fromUserId:发送人用户 Id。（必传）
	 	 * @param  txtMessage:文本消息。
	 	 * @param  pushContent:定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息. 如果为自定义消息，则 pushContent 为自定义消息显示的 Push 内容，如果不传则用户不会收到 Push 通知.（可选）
	 	 * @param  pushData:针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。（可选）
	 	 * @param  os:针对操作系统发送 Push，值为 iOS 表示对 iOS 手机用户发送 Push ,为 Android 时表示对 Android 手机用户发送 Push ，如对所有用户发送 Push 信息，则不需要传 os 参数。（可选）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut broadcast(String fromUserId, TxtMessage message, String pushContent, String pushData, String os)
        {

            if (fromUserId == null)
            {
                throw new ArgumentNullException("Paramer 'fromUserId' is required");
            }

            if (message.getType() == null)
            {
                throw new ArgumentNullException("Paramer 'ObjectName' is required");
            }

            if (message.toString() == null)
            {
                throw new ArgumentNullException("Paramer 'Content' is required");
            }

            String postStr = "";
            postStr += "fromUserId=" + HttpUtility.UrlEncode(fromUserId == null ? "" : fromUserId, Encoding.UTF8) + "&";
            postStr += "objectName=" + HttpUtility.UrlEncode(message.getType(), Encoding.UTF8) + "&";
            postStr += "content=" + HttpUtility.UrlEncode(message.toString(), Encoding.UTF8) + "&";
            postStr += "pushContent=" + HttpUtility.UrlEncode(pushContent == null ? "" : pushContent, Encoding.UTF8) + "&";
            postStr += "pushData=" + HttpUtility.UrlEncode(pushData == null ? "" : pushData, Encoding.UTF8) + "&";
            postStr += "os=" + HttpUtility.UrlEncode(os == null ? "" : os, Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/broadcast.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 消息历史记录下载地址获取 方法消息历史记录下载地址获取方法。获取 APP 内指定某天某小时内的所有会话消息记录的下载地址。（目前支持二人会话、讨论组、群组、聊天室、客服、系统通知消息历史记录下载） 
	 	 * 
	 	 * @param  date:指定北京时间某天某小时，格式为2014010101,表示：2014年1月1日凌晨1点。（必传）
		 *
	 	 * @return HistoryMessageReslut
	 	 **/
        public HistoryMessageReslut getHistory(String date)
        {

            if (date == null)
            {
                throw new ArgumentNullException("Paramer 'date' is required");
            }

            String postStr = "";
            postStr += "date=" + HttpUtility.UrlEncode(date == null ? "" : date, Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (HistoryMessageReslut)RongJsonUtil.JsonStringToObj<HistoryMessageReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/history.json", postStr, "application/x-www-form-urlencoded"));
        }

        /**
	 	 * 消息历史记录删除方法（删除 APP 内指定某天某小时内的所有会话消息记录。调用该接口返回成功后，date参数指定的某小时的消息记录文件将在随后的5-10分钟内被永久删除。） 
	 	 * 
	 	 * @param  date:指定北京时间某天某小时，格式为2014010101,表示：2014年1月1日凌晨1点。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
        public CodeSuccessReslut deleteMessage(String date)
        {

            if (date == null)
            {
                throw new ArgumentNullException("Paramer 'date' is required");
            }

            String postStr = "";
            postStr += "date=" + HttpUtility.UrlEncode(date == null ? "" : date, Encoding.UTF8) + "&";
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            return (CodeSuccessReslut)RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI + "/message/history/delete.json", postStr, "application/x-www-form-urlencoded"));
        }

    }

}