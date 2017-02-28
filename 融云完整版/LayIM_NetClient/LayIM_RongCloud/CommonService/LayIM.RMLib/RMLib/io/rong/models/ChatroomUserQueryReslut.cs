using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 *  chatroomUserQuery 返回结果
	 */
	public class ChatroomUserQueryReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 聊天室中用户数。
		[JsonProperty]
		int total;
		// 聊天室成员列表。
		[JsonProperty]
		List<ChatRoomUser> users;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public ChatroomUserQueryReslut(int code, int total, List<ChatRoomUser> users, String errorMessage) {
			this.code = code;
			this.total = total;
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
		 * 设置total
		 *
		 */	
		public void setTotal(int total) {
			this.total = total;
		}
		
		/**
		 * 获取total
		 *
		 * @return Integer
		 */
		public int getTotal() {
			return total;
		}
		
		/**
		 * 设置users
		 *
		 */	
		public void setUsers(List<ChatRoomUser> users) {
			this.users = users;
		}
		
		/**
		 * 获取users
		 *
		 * @return List<ChatRoomUser>
		 */
		public List<ChatRoomUser> getUsers() {
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
