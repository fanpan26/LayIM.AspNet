using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 添加联系人消息。
	 *
	 */
	public class ContactNtfMessage  {
		[JsonProperty]
		private String operation = "";
		[JsonProperty]
		private String extra = "";
		[JsonProperty]
		private String sourceUserId = "";
		[JsonProperty]
		private String targetUserId = "";
		[JsonProperty]
		private String message = "";
		private  static  String TYPE = "RC:ContactNtf";
		
		public ContactNtfMessage() {

		}
		
		public ContactNtfMessage(String operation, String extra, String sourceUserId, String targetUserId, String message) {
			this.operation = operation;
			this.extra = extra;
			this.sourceUserId = sourceUserId;
			this.targetUserId = targetUserId;
			this.message = message;
		}
		
		public String getType() {
			return TYPE;
		}
		
		/**
		 * 获取操作名。
		 *
		 * @returnString
		 */
		public String getOperation() {
			return operation;
		}
		
		/**
		 * 设置操作名。
		 *
		 * @return
		 */
		public void setOperation(String operation) {
			this.operation = operation;
		}  
		
		/**
		 * 获取为附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @returnString
		 */
		public String getExtra() {
			return extra;
		}
		
		/**
		 * 设置为附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @return
		 */
		public void setExtra(String extra) {
			this.extra = extra;
		}  
		
		/**
		 * 获取请求者或者响应者的 UserId。
		 *
		 * @returnString
		 */
		public String getSourceUserId() {
			return sourceUserId;
		}
		
		/**
		 * 设置请求者或者响应者的 UserId。
		 *
		 * @return
		 */
		public void setSourceUserId(String sourceUserId) {
			this.sourceUserId = sourceUserId;
		}  
		
		/**
		 * 获取被请求者或者被响应者的 UserId。
		 *
		 * @returnString
		 */
		public String getTargetUserId() {
			return targetUserId;
		}
		
		/**
		 * 设置被请求者或者被响应者的 UserId。
		 *
		 * @return
		 */
		public void setTargetUserId(String targetUserId) {
			this.targetUserId = targetUserId;
		}  
		
		/**
		 * 获取请求或者响应消息。
		 *
		 * @returnString
		 */
		public String getMessage() {
			return message;
		}
		
		/**
		 * 设置请求或者响应消息。
		 *
		 * @return
		 */
		public void setMessage(String message) {
			this.message = message;
		}  
		
		public string toString() {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this);
        }
	}
}

