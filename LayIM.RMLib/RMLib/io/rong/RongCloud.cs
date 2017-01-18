/**
 * 融云 Server API donet 客户端
 * create by kitName
 * create datetime : 2016-09-06 
 * 
 * v2.0.1
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web;
using donet.io.rong.models;
using donet.io.rong.util;
using donet.io.rong.methods;
using System.Collections.Generic;

namespace donet.io.rong {
    public class RongCloud {
    
    	private static Dictionary<String, RongCloud> rongCloud = new Dictionary<String, RongCloud>();
		public static String RONGCLOUDURI = "http://api.cn.ronghub.com";
        public static String RONGCLOUDSMSURI = "http://api.sms.ronghub.com";
		//确保线程同步
        private static readonly object locker = new object();
        
		public User user;
		public Message message;
		public Wordfilter wordfilter;
		public Group group;
		public Chatroom chatroom;
		public Push push;
		public SMS sms;

		private RongCloud(String appKey, String appSecret) {
			user = new User(appKey, appSecret);
			message = new Message(appKey, appSecret);
			wordfilter = new Wordfilter(appKey, appSecret);
			group = new Group(appKey, appSecret);
			chatroom = new Chatroom(appKey, appSecret);
			push = new Push(appKey, appSecret);
			sms = new SMS(appKey, appSecret);
	
		}
	
		public static RongCloud getInstance(String appKey, String appSecret) {
			lock (locker) {
                if (!rongCloud.ContainsKey(appKey)) {
                    rongCloud.Add(appKey, new RongCloud(appKey, appSecret));
                }
            }
            return rongCloud[appKey];
		}
    }
}