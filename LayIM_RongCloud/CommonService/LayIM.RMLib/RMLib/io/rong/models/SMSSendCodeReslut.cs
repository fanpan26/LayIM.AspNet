using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 *  SMSSendCodeReslut 成功返回结果
	 */
	public class SMSSendCodeReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 短信验证码唯一标识。
		[JsonProperty]
		String sessionId;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public SMSSendCodeReslut(int code, String sessionId, String errorMessage) {
			this.code = code;
			this.sessionId = sessionId;
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
		 * 设置sessionId
		 *
		 */	
		public void setSessionId(String sessionId) {
			this.sessionId = sessionId;
		}
		
		/**
		 * 获取sessionId
		 *
		 * @return String
		 */
		public String getSessionId() {
			return sessionId;
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
