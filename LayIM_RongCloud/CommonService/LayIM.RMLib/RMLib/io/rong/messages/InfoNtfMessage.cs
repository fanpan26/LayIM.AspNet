using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 提示条（小灰条）通知消息。此类型消息没有 Push 通知。
	 *
	 */
	public class InfoNtfMessage  {
		[JsonProperty]
		private String message = "";
		[JsonProperty]
		private String extra = "";
		private  static  String TYPE = "RC:InfoNtf";
		
		public InfoNtfMessage() {

		}
		
		public InfoNtfMessage(String message, String extra) {
			this.message = message;
			this.extra = extra;
		}
		
		public String getType() {
			return TYPE;
		}
		
		/**
		 * 获取提示条消息内容。
		 *
		 * @returnString
		 */
		public String getMessage() {
			return message;
		}
		
		/**
		 * 设置提示条消息内容。
		 *
		 * @return
		 */
		public void setMessage(String message) {
			this.message = message;
		}  
		
		/**
		 * 获取附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @returnString
		 */
		public String getExtra() {
			return extra;
		}
		
		/**
		 * 设置附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @return
		 */
		public void setExtra(String extra) {
			this.extra = extra;
		}  
		
		public string toString() {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this);
        }
	}
}

