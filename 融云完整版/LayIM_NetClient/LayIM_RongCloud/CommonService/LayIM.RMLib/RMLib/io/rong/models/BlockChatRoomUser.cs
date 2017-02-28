using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 聊天室被封禁用户信息。
	 */
	public class BlockChatRoomUser {
		// 聊天室用户Id。
		[JsonProperty]
		String id;
		// 加入聊天室时间。
		[JsonProperty]
		String time;
		
		public BlockChatRoomUser(String id, String time) {
			this.id = id;
			this.time = time;
		}
		
		/**
		 * 设置id
		 *
		 */	
		public void setId(String id) {
			this.id = id;
		}
		
		/**
		 * 获取id
		 *
		 * @return String
		 */
		public String getId() {
			return id;
		}
		
		/**
		 * 设置time
		 *
		 */	
		public void setTime(String time) {
			this.time = time;
		}
		
		/**
		 * 获取time
		 *
		 * @return String
		 */
		public String getTime() {
			return time;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
