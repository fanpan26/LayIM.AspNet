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

    public class Push {
    	
        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;
        
		public Push(String appKey, String appSecret) {
			this.appKey = appKey;
			this.appSecret = appSecret;
	
		}

        /**
	 	 * 添加 Push 标签方法 
	 	 * 
	 	 * @param  userTag:用户标签。
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut setUserPushTag(UserTag userTag) {

			if(userTag == null) {
				throw new ArgumentNullException("Paramer 'userTag' is required");
			}
			
	    	String postStr = "";
	        postStr = JsonConvert.SerializeObject(userTag);
	        return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/user/tag/set.json", postStr, "application/json" ));
		}
            
        /**
	 	 * 广播消息方法（fromuserid 和 message为null即为不落地的push） 
	 	 * 
	 	 * @param  pushMessage:json数据
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut broadcastPush(PushMessage pushMessage) {

			if(pushMessage == null) {
				throw new ArgumentNullException("Paramer 'pushMessage' is required");
			}
			
	    	String postStr = "";
	        postStr = JsonConvert.SerializeObject(pushMessage);
	        return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/push.json", postStr, "application/json" ));
		}
            
	}
       
}