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

    public class Wordfilter {
    	
        private static String UTF8 = "UTF-8";
        private String appKey;
        private String appSecret;
        
		public Wordfilter(String appKey, String appSecret) {
			this.appKey = appKey;
			this.appSecret = appSecret;
	
		}

        /**
	 	 * 添加敏感词方法（设置敏感词后，App 中用户不会收到含有敏感词的消息内容，默认最多设置 50 个敏感词。） 
	 	 * 
	 	 * @param  word:敏感词，最长不超过 32 个字符。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut add(String word) {

			if(word == null) {
				throw new ArgumentNullException("Paramer 'word' is required");
			}
			
	    	String postStr = "";
	    	postStr += "word=" + HttpUtility.UrlEncode(word == null ? "" : word,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/wordfilter/add.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 查询敏感词列表方法 
	 	 * 
		 *
	 	 * @return ListWordfilterReslut
	 	 **/
		public  ListWordfilterReslut getList() {

	    	String postStr = "";
	    	
          	return (ListWordfilterReslut) RongJsonUtil.JsonStringToObj<ListWordfilterReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/wordfilter/list.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
        /**
	 	 * 移除敏感词方法（从敏感词列表中，移除某一敏感词。） 
	 	 * 
	 	 * @param  word:敏感词，最长不超过 32 个字符。（必传）
		 *
	 	 * @return CodeSuccessReslut
	 	 **/
		public  CodeSuccessReslut delete(String word) {

			if(word == null) {
				throw new ArgumentNullException("Paramer 'word' is required");
			}
			
	    	String postStr = "";
	    	postStr += "word=" + HttpUtility.UrlEncode(word == null ? "" : word,Encoding.UTF8) + "&";
	    	postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
	    	
          	return (CodeSuccessReslut) RongJsonUtil.JsonStringToObj<CodeSuccessReslut>(RongHttpClient.ExecutePost(appKey, appSecret, RongCloud.RONGCLOUDURI+"/wordfilter/delete.json", postStr, "application/x-www-form-urlencoded" ));
		}
            
	}
       
}