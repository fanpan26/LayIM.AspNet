using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 群组信息。
	 */
	public class GroupInfo {
		// 群组Id。
		[JsonProperty]
		String id;
		// 群组名称。
		[JsonProperty]
		String name;
		
		public GroupInfo(String id, String name) {
			this.id = id;
			this.name = name;
		}
		
		/**
		 * 设置id
		 *
		 */	
		public void setId(String id) {
			this.id = id;
		}
		
		/**
		 * 获取id
		 *
		 * @return String
		 */
		public String getId() {
			return id;
		}
		
		/**
		 * 设置name
		 *
		 */	
		public void setName(String name) {
			this.name = name;
		}
		
		/**
		 * 获取name
		 *
		 * @return String
		 */
		public String getName() {
			return name;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
