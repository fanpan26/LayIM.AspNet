using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * historyMessage返回结果
	 */
	public class HistoryMessageReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 历史消息下载地址。
		[JsonProperty]
		String url;
		// 历史记录时间。（yyyymmddhh）
		[JsonProperty]
		String date;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public HistoryMessageReslut(int code, String url, String date, String errorMessage) {
			this.code = code;
			this.url = url;
			this.date = date;
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
		 * 设置url
		 *
		 */	
		public void setUrl(String url) {
			this.url = url;
		}
		
		/**
		 * 获取url
		 *
		 * @return String
		 */
		public String getUrl() {
			return url;
		}
		
		/**
		 * 设置date
		 *
		 */	
		public void setDate(String date) {
			this.date = date;
		}
		
		/**
		 * 获取date
		 *
		 * @return String
		 */
		public String getDate() {
			return date;
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
