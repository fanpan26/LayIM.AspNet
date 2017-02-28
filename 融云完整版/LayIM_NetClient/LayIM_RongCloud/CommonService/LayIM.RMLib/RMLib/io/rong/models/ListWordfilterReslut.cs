using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * listWordfilter返回结果
	 */
	public class ListWordfilterReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 敏感词内容。
		[JsonProperty]
		String word;
		// 错误信息。
		[JsonProperty]
		String errorMessage;
		
		public ListWordfilterReslut(int code, String word, String errorMessage) {
			this.code = code;
			this.word = word;
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
		 * 设置word
		 *
		 */	
		public void setWord(String word) {
			this.word = word;
		}
		
		/**
		 * 获取word
		 *
		 * @return String
		 */
		public String getWord() {
			return word;
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
