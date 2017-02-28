using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 自定义消息
	 *
	 */
	public class CustomTxtMessage  {
		[JsonProperty]
		private String content = "";
		private  static  String TYPE = "RC:TxtMsg";
		
		public CustomTxtMessage() {

		}
		
		public CustomTxtMessage(String content) {
			this.content = content;
		}
		
		public String getType() {
			return TYPE;
		}
		
		/**
		 * 获取自定义消息内容。
		 *
		 * @returnString
		 */
		public String getContent() {
			return content;
		}
		
		/**
		 * 设置自定义消息内容。
		 *
		 * @return
		 */
		public void setContent(String content) {
			this.content = content;
		}  
		
		public string toString() {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this);
        }
	}
}

