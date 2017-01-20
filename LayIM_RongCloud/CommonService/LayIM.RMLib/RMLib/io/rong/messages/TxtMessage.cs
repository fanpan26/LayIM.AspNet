using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.messages {

	/**
	 *
	 * 文本消息。
	 *
	 */
	//public class TxtMessage  {
	//	[JsonProperty]
	//	private String content = "";
	//	[JsonProperty]
	//	private String extra = "";
 //       //private  static  String TYPE = "RC:TxtMsg";
 //       private static String TYPE = "s:screentext";
      
 //       public TxtMessage() {

	//	}
		
	//	public TxtMessage(String content, String extra) {
	//		this.content = content;
	//		this.extra = extra;
	//	}
		
	//	public String getType() {
	//		return TYPE;
	//	}
		
	//	/**
	//	 * 获取消息内容。
	//	 *
	//	 * @returnString
	//	 */
	//	public String getContent() {
	//		return content;
	//	}
		
	//	/**
	//	 * 设置消息内容。
	//	 *
	//	 * @return
	//	 */
	//	public void setContent(String content) {
	//		this.content = content;
	//	}  
		
	//	/**
	//	 * 获取附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
	//	 *
	//	 * @returnString
	//	 */
	//	public String getExtra() {
	//		return extra;
	//	}
		
	//	/**
	//	 * 设置附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
	//	 *
	//	 * @return
	//	 */
	//	public void setExtra(String extra) {
	//		this.extra = extra;
	//	}  
		
	//	public string toString() {
 //           JsonSerializerSettings jsetting = new JsonSerializerSettings();
 //           jsetting.NullValueHandling = NullValueHandling.Ignore;
 //           var str= JsonConvert.SerializeObject(this);
 //           return   "{\"cv\":\"131276\",\"name\":\"panzi\",\"txt\":\"我是内容\",\"extra\":\"helloExtra\"}";
 //       }
	//}

    public class TxtMessage
    {
        [JsonProperty]
        private String content = "";
        [JsonProperty]
        private String extra = "";
        //private  static  String TYPE = "RC:TxtMsg";
        private static String TYPE = "A:OtherMessage";

        public TxtMessage()
        {

        }

        public TxtMessage(String content, String type= "A:OtherMessage")
        {
            this.content = content;
            TYPE = type;
        }

        public String getType()
        {
            return TYPE;
        }

        /**
		 * 获取消息内容。
		 *
		 * @returnString
		 */
        public String getContent()
        {
            return content;
        }

        /**
		 * 设置消息内容。
		 *
		 * @return
		 */
        public void setContent(String content)
        {
            this.content = content;
        }

        /**
		 * 获取附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @returnString
		 */
        public String getExtra()
        {
            return extra;
        }

        /**
		 * 设置附加信息(如果开发者自己需要，可以自己在 App 端进行解析)。
		 *
		 * @return
		 */
        public void setExtra(String extra)
        {
            this.extra = extra;
        }

        public string toString()
        {
            return content;
        }
    }
}

