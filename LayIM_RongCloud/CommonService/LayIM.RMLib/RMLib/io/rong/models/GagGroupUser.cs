using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 群组用户信息。
	 */
	public class GagGroupUser {
		// 解禁时间。
		[JsonProperty]
		String time;
		// 群成员 Id。
		[JsonProperty]
		String userId;
		
		public GagGroupUser(String time, String userId) {
			this.time = time;
			this.userId = userId;
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
		
		/**
		 * 设置userId
		 *
		 */	
		public void setUserId(String userId) {
			this.userId = userId;
		}
		
		/**
		 * 获取userId
		 *
		 * @return String
		 */
		public String getUserId() {
			return userId;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
