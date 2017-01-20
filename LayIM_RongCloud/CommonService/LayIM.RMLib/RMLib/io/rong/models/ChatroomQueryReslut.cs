using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 *  chatroomQuery 返回结果
	 */
	public class ChatroomQueryReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 聊天室信息数组。
		[JsonProperty]
		List<ChatRoom> chatRooms;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public ChatroomQueryReslut(int code, List<ChatRoom> chatRooms, String errorMessage) {
			this.code = code;
			this.chatRooms = chatRooms;
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
		 * 设置chatRooms
		 *
		 */	
		public void setChatRooms(List<ChatRoom> chatRooms) {
			this.chatRooms = chatRooms;
		}
		
		/**
		 * 获取chatRooms
		 *
		 * @return List<ChatRoom>
		 */
		public List<ChatRoom> getChatRooms() {
			return chatRooms;
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
