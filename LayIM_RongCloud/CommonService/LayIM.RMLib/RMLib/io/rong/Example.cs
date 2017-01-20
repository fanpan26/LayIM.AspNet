using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

using donet.io.rong;
using donet.io.rong.util;
using donet.io.rong.models;
using donet.io.rong.messages;
using Newtonsoft.Json;

namespace donet.io.rong {
    class Program {
        static void Main(string[] args) {
            //String appKey = AppSettings.GetValue("IM_APPKEY");// "c9kqb3rdkb49j";
            //String appSecret = AppSettings.GetValue("IM_APPSECRET");//"Sa1sMSogTIL";
            //RongCloud rongcloud = RongCloud.getInstance(appKey, appSecret);
            JsonSerializer serializer = new JsonSerializer();

            //         Console.WriteLine("**************** user ****************");
            //// 获取 Token 方法 
            //TokenReslut usergetTokenResult = rongcloud.user.getToken("userId1","username","http://www.rongcloud.cn/images/logo.png");
            //Console.WriteLine("user.getToken:  "+  usergetTokenResult.toString());
            //Console.ReadKey();

            //// 刷新用户信息方法 
            //CodeSuccessReslut userrefreshResult = rongcloud.user.refresh("userId1","username","http://www.rongcloud.cn/images/logo.png");
            //Console.WriteLine("user.refresh:  "+  userrefreshResult.toString());
            //Console.ReadKey();

            //// 检查用户在线状态 方法 
            //CheckOnlineReslut usercheckOnlineResult = rongcloud.user.checkOnline("userId1");
            //Console.WriteLine("user.checkOnline:  "+  usercheckOnlineResult.toString());
            //Console.ReadKey();

            //// 封禁用户方法（每秒钟限 100 次） 
            //CodeSuccessReslut userblockResult = rongcloud.user.block("userId4",10);
            //Console.WriteLine("user.block:  "+  userblockResult.toString());
            //Console.ReadKey();

            //// 解除用户封禁方法（每秒钟限 100 次） 
            //CodeSuccessReslut userunBlockResult = rongcloud.user.unBlock("userId2");
            //Console.WriteLine("user.unBlock:  "+  userunBlockResult.toString());
            //Console.ReadKey();

            //// 获取被封禁用户方法（每秒钟限 100 次） 
            //QueryBlockUserReslut userqueryBlockResult = rongcloud.user.queryBlock();
            //Console.WriteLine("user.queryBlock:  "+  userqueryBlockResult.toString());
            //Console.ReadKey();

            //// 添加用户到黑名单方法（每秒钟限 100 次） 
            //CodeSuccessReslut useraddBlacklistResult = rongcloud.user.addBlacklist("userId1","userId2");
            //Console.WriteLine("user.addBlacklist:  "+  useraddBlacklistResult.toString());
            //Console.ReadKey();

            //// 获取某用户的黑名单列表方法（每秒钟限 100 次） 
            //QueryBlacklistUserReslut userqueryBlacklistResult = rongcloud.user.queryBlacklist("userId1");
            //Console.WriteLine("user.queryBlacklist:  "+  userqueryBlacklistResult.toString());
            //Console.ReadKey();

            //// 从黑名单中移除用户方法（每秒钟限 100 次） 
            //CodeSuccessReslut userremoveBlacklistResult = rongcloud.user.removeBlacklist("userId1","userId2");
            //Console.WriteLine("user.removeBlacklist:  "+  userremoveBlacklistResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** message ****************");
            //// 发送单聊消息方法（一个用户向另外一个用户发送消息，单条消息最大 128k。每分钟最多发送 6000 条信息，每次发送用户上限为 1000 人，如：一次发送 1000 人时，示为 1000 条消息。） 
            //String[] messagepublishPrivateToUserId = {"userId2","userid3","userId4"};
            //VoiceMessage messagepublishPrivateVoiceMessage = new VoiceMessage("hello","helloExtra",20L);
            //CodeSuccessReslut messagepublishPrivateResult = rongcloud.message.publishPrivate("userId1",messagepublishPrivateToUserId ,messagepublishPrivateVoiceMessage ,"thisisapush","{\"pushData\":\"hello\"}","4",0,0,0);
            //Console.WriteLine("message.publishPrivate:  "+  messagepublishPrivateResult.toString());
            //Console.ReadKey();

            //// 发送单聊模板消息方法（一个用户向多个用户发送不同消息内容，单条消息最大 128k。每分钟最多发送 6000 条信息，每次发送用户上限为 1000 人。） 
            //String str10 = File.ReadAllText("./TemplateMessage.json");
            //         TemplateMessage publishTemplateTemplateMessage = RongJsonUtil.JsonStringToObj<TemplateMessage>(str10);
            //         CodeSuccessReslut messagepublishTemplateResult = rongcloud.message.publishTemplate( publishTemplateTemplateMessage);
            //         Console.WriteLine("message.publishTemplate:  "+  messagepublishTemplateResult.toString());
            //Console.ReadKey();

            //// 发送系统消息方法（一个用户向一个或多个用户发送系统消息，单条消息最大 128k，会话类型为 SYSTEM。每秒钟最多发送 100 条消息，每次最多同时向 100 人发送，如：一次发送 100 人时，示为 100 条消息。） 
            //String[] messagepublishSystemToUserId = {"userId2","userid3","userId4"};
            //TxtMessage messagepublishSystemTxtMessage = new TxtMessage("hello","helloExtra");
            //CodeSuccessReslut messagepublishSystemResult = rongcloud.message.PublishSystem("userId1",messagepublishSystemToUserId ,messagepublishSystemTxtMessage ,"thisisapush","{\"pushData\":\"hello\"}",0,0);
            //Console.WriteLine("message.PublishSystem:  "+  messagepublishSystemResult.toString());
            //Console.ReadKey();

            //// 发送系统模板消息方法（一个用户向一个或多个用户发送系统消息，单条消息最大 128k，会话类型为 SYSTEM.每秒钟最多发送 100 条消息，每次最多同时向 100 人发送，如：一次发送 100 人时，示为 100 条消息。） 
            //String str12 = File.ReadAllText("./TemplateMessage.json");
            //         TemplateMessage publishSystemTemplateTemplateMessage = RongJsonUtil.JsonStringToObj<TemplateMessage>(str12);
            //         CodeSuccessReslut messagepublishSystemTemplateResult = rongcloud.message.publishSystemTemplate( publishSystemTemplateTemplateMessage);
            //         Console.WriteLine("message.publishSystemTemplate:  "+  messagepublishSystemTemplateResult.toString());
            //Console.ReadKey();

            //// 发送群组消息方法（以一个用户身份向群组发送消息，单条消息最大 128k.每秒钟最多发送 20 条消息，每次最多向 3 个群组发送，如：一次向 3 个群组发送消息，示为 3 条消息。） 
            //String[] messagepublishGroupToGroupId = {"groupId1","groupId2","groupId3"};
            //TxtMessage messagepublishGroupTxtMessage = new TxtMessage("hello","helloExtra");
            //CodeSuccessReslut messagepublishGroupResult = rongcloud.message.publishGroup("userId",messagepublishGroupToGroupId ,messagepublishGroupTxtMessage ,"thisisapush","{\"pushData\":\"hello\"}",1,1);
            //Console.WriteLine("message.publishGroup:  "+  messagepublishGroupResult.toString());
            //Console.ReadKey();

            //// 发送讨论组消息方法（以一个用户身份向讨论组发送消息，单条消息最大 128k，每秒钟最多发送 20 条消息.） 
            //TxtMessage messagepublishDiscussionTxtMessage = new TxtMessage("hello","helloExtra");
            //CodeSuccessReslut messagepublishDiscussionResult = rongcloud.message.publishDiscussion("userId1","discussionId1",messagepublishDiscussionTxtMessage ,"thisisapush","{\"pushData\":\"hello\"}",1,1);
            //Console.WriteLine("message.publishDiscussion:  "+  messagepublishDiscussionResult.toString());
            //Console.ReadKey();

            // 发送聊天室消息方法（一个用户向聊天室发送消息，单条消息最大 128k。每秒钟限 100 次。） 
          
                String[] messagepublishChatroomToChatroomId = { "10000" };
                TxtMessage messagepublishChatroomTxtMessage = new TxtMessage("\"cv\":131276,\"name\":\"范月盘\",\"txt\":\"我的弹幕\"", "helloExtra");
                //CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishChatroom("131276", messagepublishChatroomToChatroomId, messagepublishChatroomTxtMessage);

               // Console.WriteLine("message.publishChatroom:  " + messagepublishChatroomResult.toString());
           
            Console.ReadKey();

            //// 发送广播消息方法（发送消息给一个应用下的所有注册用户，如用户未在线会对满足条件（绑定手机终端）的用户发送 Push 信息，单条消息最大 128k，会话类型为 SYSTEM。每小时只能发送 1 次，每天最多发送 3 次。） 
            //TxtMessage messagebroadcastTxtMessage = new TxtMessage("hello","helloExtra");
            //CodeSuccessReslut messagebroadcastResult = rongcloud.message.broadcast("userId1",messagebroadcastTxtMessage ,"thisisapush","{\"pushData\":\"hello\"}","iOS");
            //Console.WriteLine("message.broadcast:  "+  messagebroadcastResult.toString());
            //Console.ReadKey();

            //// 消息历史记录下载地址获取 方法消息历史记录下载地址获取方法。获取 APP 内指定某天某小时内的所有会话消息记录的下载地址。（目前支持二人会话、讨论组、群组、聊天室、客服、系统通知消息历史记录下载） 
            //HistoryMessageReslut messagegetHistoryResult = rongcloud.message.getHistory("2014010101");
            //Console.WriteLine("message.getHistory:  "+  messagegetHistoryResult.toString());
            //Console.ReadKey();

            //// 消息历史记录删除方法（删除 APP 内指定某天某小时内的所有会话消息记录。调用该接口返回成功后，date参数指定的某小时的消息记录文件将在随后的5-10分钟内被永久删除。） 
            //CodeSuccessReslut messagedeleteMessageResult = rongcloud.message.deleteMessage("2014010101");
            //Console.WriteLine("message.deleteMessage:  "+  messagedeleteMessageResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** wordfilter ****************");
            //// 添加敏感词方法（设置敏感词后，App 中用户不会收到含有敏感词的消息内容，默认最多设置 50 个敏感词。） 
            //CodeSuccessReslut wordfilteraddResult = rongcloud.wordfilter.add("money");
            //Console.WriteLine("wordfilter.add:  "+  wordfilteraddResult.toString());
            //Console.ReadKey();

            //// 查询敏感词列表方法 
            //ListWordfilterReslut wordfiltergetListResult = rongcloud.wordfilter.getList();
            //Console.WriteLine("wordfilter.getList:  "+  wordfiltergetListResult.toString());
            //Console.ReadKey();

            //// 移除敏感词方法（从敏感词列表中，移除某一敏感词。） 
            //CodeSuccessReslut wordfilterdeleteResult = rongcloud.wordfilter.delete("money");
            //Console.WriteLine("wordfilter.delete:  "+  wordfilterdeleteResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** group ****************");
            //// 创建群组方法（创建群组，并将用户加入该群组，用户将可以收到该群的消息，同一用户最多可加入 500 个群，每个群最大至 3000 人，App 内的群组数量没有限制.注：其实本方法是加入群组方法 /group/join 的别名。） 
            //String[] groupcreateUserId = {"userId1","userid2","userId3"};
            //CodeSuccessReslut groupcreateResult = rongcloud.group.create(groupcreateUserId ,"groupId1","groupName1");
            //Console.WriteLine("group.create:  "+  groupcreateResult.toString());
            //Console.ReadKey();

            //// 同步用户所属群组方法（当第一次连接融云服务器时，需要向融云服务器提交 userId 对应的用户当前所加入的所有群组，此接口主要为防止应用中用户群信息同融云已知的用户所属群信息不同步。） 
            //GroupInfo[] groupsyncGroupInfo = {new GroupInfo("groupId1","groupName1" ),new GroupInfo("groupId2","groupName2" ),new GroupInfo("groupId3","groupName3" )};
            //CodeSuccessReslut groupsyncResult = rongcloud.group.sync("userId1",groupsyncGroupInfo );
            //Console.WriteLine("group.sync:  "+  groupsyncResult.toString());
            //Console.ReadKey();

            //// 刷新群组信息方法 
            //CodeSuccessReslut grouprefreshResult = rongcloud.group.refresh("groupId1","newGroupName");
            //Console.WriteLine("group.refresh:  "+  grouprefreshResult.toString());
            //Console.ReadKey();

            //// 将用户加入指定群组，用户将可以收到该群的消息，同一用户最多可加入 500 个群，每个群最大至 3000 人。 
            //String[] groupjoinUserId = {"userId2","userid3","userId4"};
            //CodeSuccessReslut groupjoinResult = rongcloud.group.join(groupjoinUserId ,"groupId1","TestGroup");
            //Console.WriteLine("group.join:  "+  groupjoinResult.toString());
            //Console.ReadKey();

            //// 查询群成员方法 
            //GroupUserQueryReslut groupqueryUserResult = rongcloud.group.queryUser("groupId1");
            //Console.WriteLine("group.queryUser:  "+  groupqueryUserResult.toString());
            //Console.ReadKey();

            //// 退出群组方法（将用户从群中移除，不再接收该群组的消息.） 
            //String[] groupquitUserId = {"userId2","userid3","userId4"};
            //CodeSuccessReslut groupquitResult = rongcloud.group.quit(groupquitUserId ,"TestGroup");
            //Console.WriteLine("group.quit:  "+  groupquitResult.toString());
            //Console.ReadKey();

            //// 添加禁言群成员方法（在 App 中如果不想让某一用户在群中发言时，可将此用户在群组中禁言，被禁言用户可以接收查看群组中用户聊天信息，但不能发送消息。） 
            //CodeSuccessReslut groupaddGagUserResult = rongcloud.group.addGagUser("userId1","groupId1","1");
            //Console.WriteLine("group.addGagUser:  "+  groupaddGagUserResult.toString());
            //Console.ReadKey();

            //// 查询被禁言群成员方法 
            //ListGagGroupUserReslut grouplisGagUserResult = rongcloud.group.lisGagUser("groupId1");
            //Console.WriteLine("group.lisGagUser:  "+  grouplisGagUserResult.toString());
            //Console.ReadKey();

            //// 移除禁言群成员方法 
            //String[] grouprollBackGagUserUserId = {"userId2","userid3","userId4"};
            //CodeSuccessReslut grouprollBackGagUserResult = rongcloud.group.rollBackGagUser(grouprollBackGagUserUserId ,"groupId1");
            //Console.WriteLine("group.rollBackGagUser:  "+  grouprollBackGagUserResult.toString());
            //Console.ReadKey();

            //// 解散群组方法。（将该群解散，所有用户都无法再接收该群的消息。） 
            //CodeSuccessReslut groupdismissResult = rongcloud.group.dismiss("userId1","groupId1");
            //Console.WriteLine("group.dismiss:  "+  groupdismissResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** chatroom ****************");
            //// 创建聊天室方法 
            //ChatRoomInfo[] chatroomcreateChatRoomInfo = {new ChatRoomInfo("chatroomId1","chatroomName1" ),new ChatRoomInfo("chatroomId2","chatroomName2" ),new ChatRoomInfo("chatroomId3","chatroomName3" )};
            //CodeSuccessReslut chatroomcreateResult = rongcloud.chatroom.create(chatroomcreateChatRoomInfo );
            //Console.WriteLine("chatroom.create:  "+  chatroomcreateResult.toString());
            //Console.ReadKey();

            //// 加入聊天室方法 
            //String[] chatroomjoinUserId = {"userId2","userid3","userId4"};
            //CodeSuccessReslut chatroomjoinResult = rongcloud.chatroom.join(chatroomjoinUserId ,"chatroomId1");
            //Console.WriteLine("chatroom.join:  "+  chatroomjoinResult.toString());
            //Console.ReadKey();

            //// 查询聊天室信息方法 
            //String[] chatroomqueryChatroomId = {"chatroomId1","chatroomId2","chatroomId3"};
            //ChatroomQueryReslut chatroomqueryResult = rongcloud.chatroom.query(chatroomqueryChatroomId );
            //Console.WriteLine("chatroom.query:  "+  chatroomqueryResult.toString());
            //Console.ReadKey();

            //// 查询聊天室内用户方法 
            //ChatroomUserQueryReslut chatroomqueryUserResult = rongcloud.chatroom.queryUser("chatroomId1","500","2");
            //Console.WriteLine("chatroom.queryUser:  "+  chatroomqueryUserResult.toString());
            //Console.ReadKey();

            //// 聊天室消息停止分发方法（可实现控制对聊天室中消息是否进行分发，停止分发后聊天室中用户发送的消息，融云服务端不会再将消息发送给聊天室中其他用户。） 
            //CodeSuccessReslut chatroomstopDistributionMessageResult = rongcloud.chatroom.stopDistributionMessage("chatroomId1");
            //Console.WriteLine("chatroom.stopDistributionMessage:  "+  chatroomstopDistributionMessageResult.toString());
            //Console.ReadKey();

            //// 聊天室消息恢复分发方法 
            //CodeSuccessReslut chatroomresumeDistributionMessageResult = rongcloud.chatroom.resumeDistributionMessage("chatroomId1");
            //Console.WriteLine("chatroom.resumeDistributionMessage:  "+  chatroomresumeDistributionMessageResult.toString());
            //Console.ReadKey();

            //// 添加禁言聊天室成员方法（在 App 中如果不想让某一用户在聊天室中发言时，可将此用户在聊天室中禁言，被禁言用户可以接收查看聊天室中用户聊天信息，但不能发送消息.） 
            //CodeSuccessReslut chatroomaddGagUserResult = rongcloud.chatroom.addGagUser("userId1","chatroomId1","1");
            //Console.WriteLine("chatroom.addGagUser:  "+  chatroomaddGagUserResult.toString());
            //Console.ReadKey();

            //// 查询被禁言聊天室成员方法 
            //ListGagChatroomUserReslut chatroomlistGagUserResult = rongcloud.chatroom.ListGagUser("chatroomId1");
            //Console.WriteLine("chatroom.ListGagUser:  "+  chatroomlistGagUserResult.toString());
            //Console.ReadKey();

            //// 移除禁言聊天室成员方法 
            //CodeSuccessReslut chatroomrollbackGagUserResult = rongcloud.chatroom.rollbackGagUser("userId1","chatroomId1");
            //Console.WriteLine("chatroom.rollbackGagUser:  "+  chatroomrollbackGagUserResult.toString());
            //Console.ReadKey();

            //// 添加封禁聊天室成员方法 
            //CodeSuccessReslut chatroomaddBlockUserResult = rongcloud.chatroom.addBlockUser("userId1","chatroomId1","1");
            //Console.WriteLine("chatroom.addBlockUser:  "+  chatroomaddBlockUserResult.toString());
            //Console.ReadKey();

            //// 查询被封禁聊天室成员方法 
            //ListBlockChatroomUserReslut chatroomgetListBlockUserResult = rongcloud.chatroom.getListBlockUser("chatroomId1");
            //Console.WriteLine("chatroom.getListBlockUser:  "+  chatroomgetListBlockUserResult.toString());
            //Console.ReadKey();

            //// 移除封禁聊天室成员方法 
            //CodeSuccessReslut chatroomrollbackBlockUserResult = rongcloud.chatroom.rollbackBlockUser("userId1","chatroomId1");
            //Console.WriteLine("chatroom.rollbackBlockUser:  "+  chatroomrollbackBlockUserResult.toString());
            //Console.ReadKey();

            //// 销毁聊天室方法 
            //String[] chatroomdestroyChatroomId = {"chatroomId","chatroomId1","chatroomId2"};
            //CodeSuccessReslut chatroomdestroyResult = rongcloud.chatroom.destroy(chatroomdestroyChatroomId );
            //Console.WriteLine("chatroom.destroy:  "+  chatroomdestroyResult.toString());
            //Console.ReadKey();

            //// 添加聊天室白名单成员方法 
            //String[] chatroomaddWhiteListUserUserId = {"userId1","userId2","userId3","userId4","userId5"};
            //CodeSuccessReslut chatroomaddWhiteListUserResult = rongcloud.chatroom.addWhiteListUser("chatroomId",chatroomaddWhiteListUserUserId );
            //Console.WriteLine("chatroom.addWhiteListUser:  "+  chatroomaddWhiteListUserResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** push ****************");
            //// 添加 Push 标签方法 
            //String str46 = File.ReadAllText("./UserTag.json");
            //         UserTag setUserPushTagUserTag = RongJsonUtil.JsonStringToObj<UserTag>(str46);
            //         CodeSuccessReslut pushsetUserPushTagResult = rongcloud.push.setUserPushTag( setUserPushTagUserTag);
            //         Console.WriteLine("push.setUserPushTag:  "+  pushsetUserPushTagResult.toString());
            //Console.ReadKey();

            //// 广播消息方法（fromuserid 和 message为null即为不落地的push） 
            //String str47 = File.ReadAllText("./PushMessage.json");
            //         PushMessage broadcastPushPushMessage = RongJsonUtil.JsonStringToObj<PushMessage>(str47);
            //         CodeSuccessReslut pushbroadcastPushResult = rongcloud.push.broadcastPush( broadcastPushPushMessage);
            //         Console.WriteLine("push.broadcastPush:  "+  pushbroadcastPushResult.toString());
            //Console.ReadKey();


            //         Console.WriteLine("**************** SMS ****************");
            //// 获取图片验证码方法 
            //SMSImageCodeReslut sMSgetImageCodeResult = rongcloud.sms.getImageCode(appKey);
            //Console.WriteLine("SMS.getImageCode:  "+  sMSgetImageCodeResult.toString());
            //Console.ReadKey();

            //// 发送短信验证码方法。 
            //SMSSendCodeReslut sMSsendCodeResult = rongcloud.sms.sendCode("13500000000","dsfdsfd","86","1408706337","1408706337");
            //Console.WriteLine("SMS.sendCode:  "+  sMSsendCodeResult.toString());
            //Console.ReadKey();

            //// 验证码验证方法 
            //CodeSuccessReslut sMSverifyCodeResult = rongcloud.sms.verifyCode("2312312","2312312");
            //Console.WriteLine("SMS.verifyCode:  "+  sMSverifyCodeResult.toString());
            //Console.ReadKey();


        }
    }
}