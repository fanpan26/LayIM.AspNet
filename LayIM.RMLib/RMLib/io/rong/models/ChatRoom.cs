using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 聊天室信息。
	 */
	public class ChatRoom {
		// 聊天室 ID。
		[JsonProperty]
		String chrmId;
		// 聊天室名称。
		[JsonProperty]
		String name;
		// 聊天室创建时间。
		[JsonProperty]
		String time;
		
		public ChatRoom(String chrmId, String name, String time) {
			this.chrmId = chrmId;
			this.name = name;
			this.time = time;
		}
		
		/**
		 * 设置chrmId
		 *
		 */	
		public void setChrmId(String chrmId) {
			this.chrmId = chrmId;
		}
		
		/**
		 * 获取chrmId
		 *
		 * @return String
		 */
		public String getChrmId() {
			return chrmId;
		}
		
		/**
		 * 设置name
		 *
		 */	
		public void setName(String name) {
			this.name = name;
		}
		
		/**
		 * 获取name
		 *
		 * @return String
		 */
		public String getName() {
			return name;
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
