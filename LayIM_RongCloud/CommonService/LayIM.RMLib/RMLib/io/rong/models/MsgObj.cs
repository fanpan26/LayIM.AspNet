using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 用于Push中的message。
	 */
	public class MsgObj {
		// push 消息中的消息体。
		[JsonProperty]
		String content;
		// 聊天室名称。
		[JsonProperty]
		String objectName;
		
		public MsgObj(String content, String objectName) {
			this.content = content;
			this.objectName = objectName;
		}
		
		/**
		 * 设置content
		 *
		 */	
		public void setContent(String content) {
			this.content = content;
		}
		
		/**
		 * 获取content
		 *
		 * @return String
		 */
		public String getContent() {
			return content;
		}
		
		/**
		 * 设置objectName
		 *
		 */	
		public void setObjectName(String objectName) {
			this.objectName = objectName;
		}
		
		/**
		 * 获取objectName
		 *
		 * @return String
		 */
		public String getObjectName() {
			return objectName;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
