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
	public class ChatRoomInfo {
		// 聊天室Id。
		[JsonProperty]
		String id;
		// 聊天室名称。
		[JsonProperty]
		String name;
		
		public ChatRoomInfo(String id, String name) {
			this.id = id;
			this.name = name;
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
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
