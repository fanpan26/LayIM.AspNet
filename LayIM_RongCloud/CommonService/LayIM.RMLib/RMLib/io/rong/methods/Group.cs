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

    public class Group {
    	
        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;
        
		public Group(String appKey, String appSecret) {
			this.appKey = appKey;
			this.appSecret = appSecret;
	
		}

        /**
	 	 * 创建群组方法（创建群组，并将用户加入该群组，用户将可以收到该群的消息，同一用户最多可加入 500 个群，每个群最大至 3000 人，App 内的群组数量没有限制.注：其实本方法是加入群组方法 /group/join 的别名。） 
	 	 * 
	 	 * @param  userId:要加入群的用户 Id。（必传）
	 	 * @param  groupId:创建群组 Id。（必传）
	 	 * @param  groupName:群组 Id 对应的名称。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut create(String[] userId, String groupId, String groupName) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
			if(groupName == null) {
				throw new ArgumentNullException("Paramer 'groupName' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr += "groupName=" + HttpUtility.UrlEncode(groupName == null ? "" : groupName,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/create.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 同步用户所属群组方法（当第一次连接融云服务器时，需要向融云服务器提交 userId 对应的用户当前所加入的所有群组，此接口主要为防止应用中用户群信息同融云已知的用户所属群信息不同步。） 
	 	 * 
	 	 * @param  userId:被同步群信息的用户 Id。（必传）
	 	 * @param  groupInfo:该用户的群信息，如群 Id 已经存在，则不会刷新对应群组名称，如果想刷新群组名称请调用刷新群组信息方法。
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut sync(String userId, GroupInfo[] groupInfo) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupInfo == null) {
				throw new ArgumentNullException("Paramer 'groupInfo' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
			if(groupInfo != null){
				for (int i = 0; i < groupInfo.Length; i++){
	               String id = HttpUtility.UrlEncode(groupInfo[i].getId(), Encoding.UTF8);
	               String name = HttpUtility.UrlEncode(groupInfo[i].getName(), Encoding.UTF8);
	               postStr += "group[" + id + "]=" + name + "&";
	            }
	        }
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/sync.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 刷新群组信息方法 
	 	 * 
	 	 * @param  groupId:群组 Id。（必传）
	 	 * @param  groupName:群名称。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut refresh(String groupId, String groupName) {

			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
			if(groupName == null) {
				throw new ArgumentNullException("Paramer 'groupName' is required");
			}
			
	    	String postStr = "";
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr += "groupName=" + HttpUtility.UrlEncode(groupName == null ? "" : groupName,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/refresh.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 将用户加入指定群组，用户将可以收到该群的消息，同一用户最多可加入 500 个群，每个群最大至 3000 人。 
	 	 * 
	 	 * @param  userId:要加入群的用户 Id，可提交多个，最多不超过 1000 个。（必传）
	 	 * @param  groupId:要加入的群 Id。（必传）
	 	 * @param  groupName:要加入的群 Id 对应的名称。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut join(String[] userId, String groupId, String groupName) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
			if(groupName == null) {
				throw new ArgumentNullException("Paramer 'groupName' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr += "groupName=" + HttpUtility.UrlEncode(groupName == null ? "" : groupName,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/join.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询群成员方法 
	 	 * 
	 	 * @param  groupId:群组Id。（必传）
		 *
	 	 * @return GroupUserQueryReslut
	 	 **/
		public  GroupUserQueryReslut queryUser(String groupId) {

			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (GroupUserQueryReslut) RongJsonUtil.JsonStringToObj<GroupUserQueryReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/user/query.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 退出群组方法（将用户从群中移除，不再接收该群组的消息.） 
	 	 * 
	 	 * @param  userId:要退出群的用户 Id.（必传）
	 	 * @param  groupId:要退出的群 Id.（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut quit(String[] userId, String groupId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/quit.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 添加禁言群成员方法（在 App 中如果不想让某一用户在群中发言时，可将此用户在群组中禁言，被禁言用户可以接收查看群组中用户聊天信息，但不能发送消息。） 
	 	 * 
	 	 * @param  userId:用户 Id。（必传）
	 	 * @param  groupId:群组 Id。（必传）
	 	 * @param  minute:禁言时长，以分钟为单位，最大值为43200分钟。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut addGagUser(String userId, String groupId, String minute) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
			if(minute == null) {
				throw new ArgumentNullException("Paramer 'minute' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr += "minute=" + HttpUtility.UrlEncode(minute == null ? "" : minute,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/user/gag/add.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询被禁言群成员方法 
	 	 * 
	 	 * @param  groupId:群组Id。（必传）
		 *
	 	 * @return ListGagGroupUserReslut
	 	 **/
		public  ListGagGroupUserReslut lisGagUser(String groupId) {

			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (ListGagGroupUserReslut) RongJsonUtil.JsonStringToObj<ListGagGroupUserReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/user/gag/list.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 移除禁言群成员方法 
	 	 * 
	 	 * @param  userId:用户Id。支持同时移除多个群成员（必传）
	 	 * @param  groupId:群组Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut rollBackGagUser(String[] userId, String groupId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
	    	String postStr = "";
	    	for(int i = 0 ; i< userId.Length; i++){
			String child  = userId[i];
			postStr += "userId=" + HttpUtility.UrlEncode(child, Encoding.UTF8) + "&";
			}
			
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/user/gag/rollback.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 解散群组方法。（将该群解散，所有用户都无法再接收该群的消息。） 
	 	 * 
	 	 * @param  userId:操作解散群的用户 Id。（必传）
	 	 * @param  groupId:要解散的群 Id。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut dismiss(String userId, String groupId) {

			if(userId == null) {
				throw new ArgumentNullException("Paramer 'userId' is required");
			}
			
			if(groupId == null) {
				throw new ArgumentNullException("Paramer 'groupId' is required");
			}
			
	    	String postStr = "";
	    	postStr += "userId=" + HttpUtility.UrlEncode(userId == null ? "" : userId,Encoding.UTF8) + "&";
	    	postStr += "groupId=" + HttpUtility.UrlEncode(groupId == null ? "" : groupId,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/group/dismiss.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
	}
       
}