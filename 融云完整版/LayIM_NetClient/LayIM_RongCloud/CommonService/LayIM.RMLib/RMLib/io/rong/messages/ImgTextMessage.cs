using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 图文消息。
	 *
	 */
	public class ImgTextMessage  {
		[JsonProperty]
		private String content = "";
		[JsonProperty]
		private String extra = "";
		[JsonProperty]
		private String title = "";
		[JsonProperty]
		private String imageUri = "";
		[JsonProperty]
		private String url = "";
		private  static  String TYPE = "RC:ImgTextMsg";
		
		public ImgTextMessage() {

		}
		
		public ImgTextMessage(String content, String extra, String title, String imageUri, String url) {
			this.content = content;
			this.extra = extra;
			this.title = title;
			this.imageUri = imageUri;
			this.url = url;
		}
		
		public String getType() {
			return TYPE;
		}
		
		/**
		 * 获取消息文本内容。
		 *
		 * @returnString
		 */
		public String getContent() {
			return content;
		}
		
		/**
		 * 设置消息文本内容。
		 *
		 * @return
		 */
		public void setContent(String content) {
			this.content = content;
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
		
		/**
		 * 获取消息标题。
		 *
		 * @returnString
		 */
		public String getTitle() {
			return title;
		}
		
		/**
		 * 设置消息标题。
		 *
		 * @return
		 */
		public void setTitle(String title) {
			this.title = title;
		}  
		
		/**
		 * 获取图片地址。
		 *
		 * @returnString
		 */
		public String getImageUri() {
			return imageUri;
		}
		
		/**
		 * 设置图片地址。
		 *
		 * @return
		 */
		public void setImageUri(String imageUri) {
			this.imageUri = imageUri;
		}  
		
		/**
		 * 获取 url 跳转地址。
		 *
		 * @returnString
		 */
		public String getUrl() {
			return url;
		}
		
		/**
		 * 设置 url 跳转地址。
		 *
		 * @return
		 */
		public void setUrl(String url) {
			this.url = url;
		}  
		
		public string toString() {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this);
        }
	}
}

