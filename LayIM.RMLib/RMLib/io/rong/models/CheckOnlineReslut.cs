using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * checkOnlineUser返回结果
	 */
	public class CheckOnlineReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 在线状态，1为在线，0为不在线。
		[JsonProperty]
		String status;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public CheckOnlineReslut(int code, String status, String errorMessage) {
			this.code = code;
			this.status = status;
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
		 * 设置status
		 *
		 */	
		public void setStatus(String status) {
			this.status = status;
		}
		
		/**
		 * 获取status
		 *
		 * @return String
		 */
		public String getStatus() {
			return status;
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
