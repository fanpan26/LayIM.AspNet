using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * queryBlacklistUser返回结果
	 */
	public class QueryBlacklistUserReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 黑名单用户列表。
		[JsonProperty]
		String[] users;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public QueryBlacklistUserReslut(int code, String[] users, String errorMessage) {
			this.code = code;
			this.users = users;
			this.errorMessage = errorMessage;
		}
		
		/**
		 * 设置code
		 *
		 */	
		public void setCode(int code) {
			this.code = code;
		}
		
		/**
		 * 获取code
		 *
		 * @return Integer
		 */
		public int getCode() {
			return code;
		}
		
		/**
		 * 设置users
		 *
		 */	
		public void setUsers(String[] users) {
			this.users = users;
		}
		
		/**
		 * 获取users
		 *
		 * @return String[]
		 */
		public String[] getUsers() {
			return users;
		}
		
		/**
		 * 设置errorMessage
		 *
		 */	
		public void setErrorMessage(String errorMessage) {
			this.errorMessage = errorMessage;
		}
		
		/**
		 * 获取errorMessage
		 *
		 * @return String
		 */
		public String getErrorMessage() {
			return errorMessage;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
