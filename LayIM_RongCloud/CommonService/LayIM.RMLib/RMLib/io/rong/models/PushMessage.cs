using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 不落地 push 消息体。
	 */
	public class PushMessage {
		// 目标操作系统。（iOS、Android）。（必传）
		[JsonProperty]
		String[] platform;
		// 发送人用户 Id。（必传）
		[JsonProperty]
		String fromuserid;
		// 推送条件，包括： tag 、 userid 、 is_to_all。（必传）
		[JsonProperty]
		TagObj audience;
		// true为全部，忽略上面的tag、userId。
		[JsonProperty]
		MsgObj message;
		// 按操作系统类型推送消息内容，如 platform 中设置了给 ios 和 android 系统推送消息，而在 notification 中只设置了 ios 的推送内容，则 android 的推送内容为最初 alert 设置的内容。
		[JsonProperty]
		Notification notification;
		
		public PushMessage(String[] platform, String fromuserid, TagObj audience, MsgObj message, Notification notification) {
			this.platform = platform;
			this.fromuserid = fromuserid;
			this.audience = audience;
			this.message = message;
			this.notification = notification;
		}
		
		/**
		 * 设置platform
		 *
		 */	
		public void setPlatform(String[] platform) {
			this.platform = platform;
		}
		
		/**
		 * 获取platform
		 *
		 * @return String[]
		 */
		public String[] getPlatform() {
			return platform;
		}
		
		/**
		 * 设置fromuserid
		 *
		 */	
		public void setFromuserid(String fromuserid) {
			this.fromuserid = fromuserid;
		}
		
		/**
		 * 获取fromuserid
		 *
		 * @return String
		 */
		public String getFromuserid() {
			return fromuserid;
		}
		
		/**
		 * 设置audience
		 *
		 */	
		public void setAudience(TagObj audience) {
			this.audience = audience;
		}
		
		/**
		 * 获取audience
		 *
		 * @return TagObj
		 */
		public TagObj getAudience() {
			return audience;
		}
		
		/**
		 * 设置message
		 *
		 */	
		public void setMessage(MsgObj message) {
			this.message = message;
		}
		
		/**
		 * 获取message
		 *
		 * @return MsgObj
		 */
		public MsgObj getMessage() {
			return message;
		}
		
		/**
		 * 设置notification
		 *
		 */	
		public void setNotification(Notification notification) {
			this.notification = notification;
		}
		
		/**
		 * 获取notification
		 *
		 * @return Notification
		 */
		public Notification getNotification() {
			return notification;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
