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

namespace donet.io.rong.methods {

    public class Chatroom {
    	
        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;
        
		public Chatroom(String appKey, String appSecret) {
			this.appKey = appKey;
			this.appSecret = appSecret;
	
		}

        /**
	 	 * 创建聊天室方法 
	 	 * 
	 	 * @param  chatRoomInfo:id:要创建的聊天室的id；name:要创建的聊天室的name。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut create(ChatRoomInfo[] chatRoomInfo) {

			if(chatRoomInfo == null) {
				throw new ArgumentNullException("Paramer 'chatRoomInfo' is required");
			}
			
	    	String postStr = "";
            if(chatRoomInfo != null){
            	for (int i = 0; i < chatRoomInfo.Length; i++) {
		            String id = HttpUtility.UrlEncode(chatRoomInfo[i].getId(), Encoding.UTF8);
		            String name = HttpUtility.UrlEncode(chatRoomInfo[i].getName(), Encoding.UTF8);
		            postStr += "chatroom[" + id + "]=" + name + "&";
            	}
            }
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/create.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 加入聊天室方法 
	 	 * 
	 	 * @param  userId:要加入聊天室的用户 Id，可提交多个，最多不超过 50 个。（必传）
	 	 * @param  chatroomId:要加入的聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut join(String[] userId, String chatroomId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/join.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询聊天室信息方法 
	 	 * 
	 	 * @param  chatroomId:要查询的聊天室id（必传）
		 *
	 	 * @return ChatroomQueryReslut
	 	 **/
		public  ChatroomQueryReslut query(String[] chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< chatroomId.Length; i++){
			String child  = chatroomId[i];
			postStr += "chatroomId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (ChatroomQueryReslut) RongJsonUtil.JsonStringToObj<ChatroomQueryReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/query.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询聊天室内用户方法 
	 	 * 
	 	 * @param  chatroomId:要查询的聊天室 ID。（必传）
	 	 * @param  count:要获取的聊天室成员数，上限为 500 ，超过 500 时最多返回 500 个成员。（必传）
	 	 * @param  order:加入聊天室的先后顺序， 1 为加入时间正序， 2 为加入时间倒序。（必传）
		 *
	 	 * @return ChatroomUserQueryReslut
	 	 **/
		public  ChatroomUserQueryReslut queryUser(String chatroomId, String count, String order) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
			if(count == null) {
				throw new ArgumentNullException("Paramer 'count' is required");
			}
			
			if(order == null) {
				throw new ArgumentNullException("Paramer 'order' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr += "count=" + HttpUtility.UrlEncode(count == null ? "" : count,Encoding.UTF8) + "&";
	    	postStr += "order=" + HttpUtility.UrlEncode(order == null ? "" : order,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (ChatroomUserQueryReslut) RongJsonUtil.JsonStringToObj<ChatroomUserQueryReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/query.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 聊天室消息停止分发方法（可实现控制对聊天室中消息是否进行分发，停止分发后聊天室中用户发送的消息，融云服务端不会再将消息发送给聊天室中其他用户。） 
	 	 * 
	 	 * @param  chatroomId:聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut stopDistributionMessage(String chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/message/stopDistribution.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 聊天室消息恢复分发方法 
	 	 * 
	 	 * @param  chatroomId:聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut resumeDistributionMessage(String chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/message/resumeDistribution.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 添加禁言聊天室成员方法（在 App 中如果不想让某一用户在聊天室中发言时，可将此用户在聊天室中禁言，被禁言用户可以接收查看聊天室中用户聊天信息，但不能发送消息.） 
	 	 * 
	 	 * @param  userId:用户 Id。（必传）
	 	 * @param  chatroomId:聊天室 Id。（必传）
	 	 * @param  minute:禁言时长，以分钟为单位，最大值为43200分钟。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut addGagUser(String userId, String chatroomId, String minute) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
			if(minute == null) {
				throw new ArgumentNullException("Paramer 'minute' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr += "minute=" + HttpUtility.UrlEncode(minute == null ? "" : minute,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/gag/add.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询被禁言聊天室成员方法 
	 	 * 
	 	 * @param  chatroomId:聊天室 Id。（必传）
		 *
	 	 * @return ListGagChatroomUserReslut
	 	 **/
		public  ListGagChatroomUserReslut ListGagUser(String chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (ListGagChatroomUserReslut) RongJsonUtil.JsonStringToObj<ListGagChatroomUserReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/gag/list.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 移除禁言聊天室成员方法 
	 	 * 
	 	 * @param  userId:用户 Id。（必传）
	 	 * @param  chatroomId:聊天室Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut rollbackGagUser(String userId, String chatroomId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/gag/rollback.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 添加封禁聊天室成员方法 
	 	 * 
	 	 * @param  userId:用户 Id。（必传）
	 	 * @param  chatroomId:聊天室 Id。（必传）
	 	 * @param  minute:封禁时长，以分钟为单位，最大值为43200分钟。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut addBlockUser(String userId, String chatroomId, String minute) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
			if(minute == null) {
				throw new ArgumentNullException("Paramer 'minute' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr += "minute=" + HttpUtility.UrlEncode(minute == null ? "" : minute,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/block/add.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询被封禁聊天室成员方法 
	 	 * 
	 	 * @param  chatroomId:聊天室 Id。（必传）
		 *
	 	 * @return ListBlockChatroomUserReslut
	 	 **/
		public  ListBlockChatroomUserReslut getListBlockUser(String chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (ListBlockChatroomUserReslut) RongJsonUtil.JsonStringToObj<ListBlockChatroomUserReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/block/list.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 移除封禁聊天室成员方法 
	 	 * 
	 	 * @param  userId:用户 Id。（必传）
	 	 * @param  chatroomId:聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut rollbackBlockUser(String userId, String chatroomId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/block/rollback.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 销毁聊天室方法 
	 	 * 
	 	 * @param  chatroomId:要销毁的聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut destroy(String[] chatroomId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< chatroomId.Length; i++){
			String child  = chatroomId[i];
			postStr += "chatroomId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/destroy.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 添加聊天室白名单成员方法 
	 	 * 
	 	 * @param  chatroomId:聊天室中用户 Id，可提交多个，聊天室中白名单用户最多不超过 5 个。（必传）
	 	 * @param  userId:聊天室 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut addWhiteListUser(String chatroomId, String[] userId) {

			if(chatroomId == null) {
				throw new ArgumentNullException("Paramer 'chatroomId' is required");
			}
			
			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "chatroomId=" + HttpUtility.UrlEncode(chatroomId == null ? "" : chatroomId,Encoding.UTF8) + "&";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/chatroom/user/whitelist/add.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
	}
       
}