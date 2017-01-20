using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 语音消息。
	 *
	 */
	public class VoiceMessage  {
		[JsonProperty]
		private String content = "";
		[JsonProperty]
		private String extra = "";
		[JsonProperty]
		private long duration = 0L;
		private  static  String TYPE = "RC:VcMsg";
		
		public VoiceMessage() {

		}
		
		public VoiceMessage(String content, String extra, long duration) {
			this.content = content;
			this.extra = extra;
			this.duration = duration;
		}
		
		public String getType() {
			return TYPE;
		}
		
		/**
		 * 获取表示语音内容，格式为 AMR，以 Base64 进行 Encode 后需要将所有 \r\n 和 \r 和 \n 替换成空，大小不超过 60k，duration 表示语音长度，最长为 60 秒。
		 *
		 * @returnString
		 */
		public String getContent() {
			return content;
		}
		
		/**
		 * 设置表示语音内容，格式为 AMR，以 Base64 进行 Encode 后需要将所有 \r\n 和 \r 和 \n 替换成空，大小不超过 60k，duration 表示语音长度，最长为 60 秒。
		 *
		 * @return
		 */
		public void setContent(String content) {
			this.content = content;
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
		 * 获取持续时间。
		 *
		 * @returnlong
		 */
		public long getDuration() {
			return duration;
		}
		
		/**
		 * 设置持续时间。
		 *
		 * @return
		 */
		public void setDuration(long duration) {
			this.duration = duration;
		}  
		
		public string toString() {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this);
        }
	}
}

