using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * groupUserQuery返回结果
	 */
	public class GroupUserQueryReslut {
		// 返回码，200 为正常。
		[JsonProperty]
		int code;
		// 群成员用户Id。
		[JsonProperty]
		String id;
		// 群成员列表。
		[JsonProperty]
		List<GroupUser> users;
		
		public GroupUserQueryReslut(int code, String id, List<GroupUser> users) {
			this.code = code;
			this.id = id;
			this.users = users;
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
		 * 设置users
		 *
		 */	
		public void setUsers(List<GroupUser> users) {
			this.users = users;
		}
		
		/**
		 * 获取users
		 *
		 * @return List<GroupUser>
		 */
		public List<GroupUser> getUsers() {
			return users;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
