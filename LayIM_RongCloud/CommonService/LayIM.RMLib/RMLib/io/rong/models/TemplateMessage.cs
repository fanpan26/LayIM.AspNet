using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 模版消息对象。
	 */
	public class TemplateMessage {
		// 发送人用户 Id。（必传）
		[JsonProperty]
		String fromUserId;
		// 接收用户 Id，提供多个本参数可以实现向多人发送消息，上限为 1000 人。（必传）
		[JsonProperty]
		String[] toUserId;
		// 发送消息内容，内容中定义标识通过 values 中设置的标识位内容进行替换，参考融云消息类型表.示例说明；如果 objectName 为自定义消息类型，该参数可自定义格式。（必传）
		[JsonProperty]
		String content;
		// 消息内容中，标识位对应内容。（必传）
		[JsonProperty]
		List<Dictionary<String, String>> values;
		// 接收用户 Id，提供多个本参数可以实现向多人发送消息，上限为 1000 人。（必传）
		[JsonProperty]
		String objectName;
		// 定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息。如果为自定义消息，定义显示的 Push 内容，内容中定义标识通过 values 中设置的标识位内容进行替换。如消息类型为自定义不需要 Push 通知，则对应数组传空值即可。（必传）
		[JsonProperty]
		String[] pushContent;
		// 针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。如不需要 Push 功能对应数组传空值即可。（可选）
		[JsonProperty]
		String[] pushData;
		// 是否过滤发送人黑名单列表，0 为不过滤、 1 为过滤，默认为 0 不过滤。（可选）
		[JsonProperty]
		int verifyBlacklist;
		
		public TemplateMessage(String fromUserId, String[] toUserId, String content, List<Dictionary<String, String>> values, String objectName, String[] pushContent, String[] pushData, int verifyBlacklist) {
			this.fromUserId = fromUserId;
			this.toUserId = toUserId;
			this.content = content;
			this.values = values;
			this.objectName = objectName;
			this.pushContent = pushContent;
			this.pushData = pushData;
			this.verifyBlacklist = verifyBlacklist;
		}
		
		/**
		 * 设置fromUserId
		 *
		 */	
		public void setFromUserId(String fromUserId) {
			this.fromUserId = fromUserId;
		}
		
		/**
		 * 获取fromUserId
		 *
		 * @return String
		 */
		public String getFromUserId() {
			return fromUserId;
		}
		
		/**
		 * 设置toUserId
		 *
		 */	
		public void setToUserId(String[] toUserId) {
			this.toUserId = toUserId;
		}
		
		/**
		 * 获取toUserId
		 *
		 * @return String[]
		 */
		public String[] getToUserId() {
			return toUserId;
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
		 * 设置values
		 *
		 */	
		public void setValues(List<Dictionary<String, String>> values) {
			this.values = values;
		}
		
		/**
		 * 获取values
		 *
		 * @return List<Map<String, String>>
		 */
		public List<Dictionary<String, String>> getValues() {
			return values;
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
		
		/**
		 * 设置pushContent
		 *
		 */	
		public void setPushContent(String[] pushContent) {
			this.pushContent = pushContent;
		}
		
		/**
		 * 获取pushContent
		 *
		 * @return String[]
		 */
		public String[] getPushContent() {
			return pushContent;
		}
		
		/**
		 * 设置pushData
		 *
		 */	
		public void setPushData(String[] pushData) {
			this.pushData = pushData;
		}
		
		/**
		 * 获取pushData
		 *
		 * @return String[]
		 */
		public String[] getPushData() {
			return pushData;
		}
		
		/**
		 * 设置verifyBlacklist
		 *
		 */	
		public void setVerifyBlacklist(int verifyBlacklist) {
			this.verifyBlacklist = verifyBlacklist;
		}
		
		/**
		 * 获取verifyBlacklist
		 *
		 * @return Integer
		 */
		public int getVerifyBlacklist() {
			return verifyBlacklist;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
