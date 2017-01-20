using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 封禁用户信息
	 */
	public class BlockUsers {
		// 被封禁用户 ID。
		[JsonProperty]
		String userId;
		// 封禁结束时间。
		[JsonProperty]
		String blockEndTime;
		
		public BlockUsers(String userId, String blockEndTime) {
			this.userId = userId;
			this.blockEndTime = blockEndTime;
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
		
		/**
		 * 设置blockEndTime
		 *
		 */	
		public void setBlockEndTime(String blockEndTime) {
			this.blockEndTime = blockEndTime;
		}
		
		/**
		 * 获取blockEndTime
		 *
		 * @return String
		 */
		public String getBlockEndTime() {
			return blockEndTime;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
