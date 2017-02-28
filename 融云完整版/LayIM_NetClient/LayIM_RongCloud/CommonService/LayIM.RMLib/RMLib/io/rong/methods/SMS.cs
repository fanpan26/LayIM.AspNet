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

    public class SMS {
    	
        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;
        
		public SMS(String appKey, String appSecret) {
			this.appKey = appKey;
			this.appSecret = appSecret;
	
		}

        /**
	 	 * 获取图片验证码方法 
	 	 * 
	 	 * @param  appKey:应用Id
		 *
	 	 * @return SMSImageCodeReslut
	 	 **/
		public  SMSImageCodeReslut getImageCode(String appKey) {

			if(appKey == null) {
				throw new ArgumentNullException("Paramer 'appKey' is required");
			}
			
	    	String postStr = "";
			postStr = RongCloud.RONGCLOUDSMSURI +"/getImgCode.json";
			postStr = postStr + ("?appKey=") + (HttpUtility.UrlEncode(appKey == null ? "" : appKey, Encoding.UTF8));
			            
            return (SMSImageCodeReslut) RongJsonUtil.JsonStringToObj<SMSImageCodeReslut>(RongHttpClient.ExecuteGet(postStr));
		}	
            
        /**
	 	 * 发送短信验证码方法。 
	 	 * 
	 	 * @param  mobile:接收短信验证码的目标手机号，每分钟同一手机号只能发送一次短信验证码，同一手机号 1 小时内最多发送 3 次。（必传）
	 	 * @param  templateId:短信模板 Id，在开发者后台->短信服务->服务设置->短信模版中获取。（必传）
	 	 * @param  region:手机号码所属国家区号，目前只支持中图区号 86）
	 	 * @param  verifyId:图片验证标识 Id ，开启图片验证功能后此参数必传，否则可以不传。在获取图片验证码方法返回值中获取。
	 	 * @param  verifyCode:图片验证码，开启图片验证功能后此参数必传，否则可以不传。
		 *
	 	 * @return SMSSendCodeReslut
	 	 **/
		public  SMSSendCodeReslut sendCode(String mobile, String templateId, String region, String verifyId, String verifyCode) {

			if(mobile == null) {
				throw new ArgumentNullException("Paramer 'mobile' is required");
			}
			
			if(templateId == null) {
				throw new ArgumentNullException("Paramer 'templateId' is required");
			}
			
			if(region == null) {
				throw new ArgumentNullException("Paramer 'region' is required");
			}
			
	    	String postStr = "";
	    	postStr += "mobile=" + HttpUtility.UrlEncode(mobile == null ? "" : mobile,Encoding.UTF8) + "&";
	    	postStr += "templateId=" + HttpUtility.UrlEncode(templateId == null ? "" : templateId,Encoding.UTF8) + "&";
	    	postStr += "region=" + HttpUtility.UrlEncode(region == null ? "" : region,Encoding.UTF8) + "&";
	    	postStr += "verifyId=" + HttpUtility.UrlEncode(verifyId == null ? "" : verifyId,Encoding.UTF8) + "&";
	    	postStr += "verifyCode=" + HttpUtility.UrlEncode(verifyCode == null ? "" : verifyCode,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (SMSSendCodeReslut) RongJsonUtil.JsonStringToObj<SMSSendCodeReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDSMSURI+"/sendCode.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 验证码验证方法 
	 	 * 
	 	 * @param  sessionId:短信验证码唯一标识，在发送短信验证码方法，返回值中获取。（必传）
	 	 * @param  code:短信验证码内容。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut verifyCode(String sessionId, String code) {

			if(sessionId == null) {
				throw new ArgumentNullException("Paramer 'sessionId' is required");
			}
			
			if(code == null) {
				throw new ArgumentNullException("Paramer 'code' is required");
			}
			
	    	String postStr = "";
	    	postStr += "sessionId=" + HttpUtility.UrlEncode(sessionId == null ? "" : sessionId,Encoding.UTF8) + "&";
	    	postStr += "code=" + HttpUtility.UrlEncode(code == null ? "" : code,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDSMSURI+"/verifyCode.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
	}
       
}