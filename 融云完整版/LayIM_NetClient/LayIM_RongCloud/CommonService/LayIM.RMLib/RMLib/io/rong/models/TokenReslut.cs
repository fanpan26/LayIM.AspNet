using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * getToken 返回结果
	 */
	public class TokenReslut {
		// 返回码，200 为正常.如果您正在使用开发环境的 AppKey，您的应用只能注册 100 名用户，达到上限后，将返回错误码 2007.如果您需要更多的测试账户数量，您需要在应用配置中申请“增加测试人数”。
		[JsonProperty]
		int code;
		// 用户 Token，可以保存应用内，长度在 256 字节以内.用户 Token，可以保存应用内，长度在 256 字节以内。
		[JsonProperty]
		String token;
		// 用户 Id，与输入的用户 Id 相同.用户 Id，与输入的用户 Id 相同。
		[JsonProperty]
		String userId;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public TokenReslut(int code, String token, String userId, String errorMessage) {
			this.code = code;
			this.token = token;
			this.userId = userId;
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
		 * 设置token
		 *
		 */	
		public void setToken(String token) {
			this.token = token;
		}
		
		/**
		 * 获取token
		 *
		 * @return String
		 */
		public String getToken() {
			return token;
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
