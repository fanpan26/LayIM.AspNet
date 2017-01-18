using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
		
	/**
	 * 按操作系统类型推送消息内容，如 platform 中设置了给 ios 和 android 系统推送消息，而在 notification 中只设置了 ios 的推送内容，则 android 的推送内容为最初 alert 设置的内容。（非必传）
	 */
	public class Notification {
		// 默认推送消息内容，如填写了 ios 或 android 下的 alert 时，则推送内容以对应平台系统的 alert 为准。（必传）
		[JsonProperty]
		String alert;
		// 设置 iOS 平台下的推送及附加信息。
		[JsonProperty]
		PlatformNotification ios;
		// 设置 Android 平台下的推送及附加信息。
		[JsonProperty]
		PlatformNotification android;
		
		public Notification(String alert, PlatformNotification ios, PlatformNotification android) {
			this.alert = alert;
			this.ios = ios;
			this.android = android;
		}
		
		/**
		 * 设置alert
		 *
		 */	
		public void setAlert(String alert) {
			this.alert = alert;
		}
		
		/**
		 * 获取alert
		 *
		 * @return String
		 */
		public String getAlert() {
			return alert;
		}
		
		/**
		 * 设置ios
		 *
		 */	
		public void setIos(PlatformNotification ios) {
			this.ios = ios;
		}
		
		/**
		 * 获取ios
		 *
		 * @return PlatformNotification
		 */
		public PlatformNotification getIos() {
			return ios;
		}
		
		/**
		 * 设置android
		 *
		 */	
		public void setAndroid(PlatformNotification android) {
			this.android = android;
		}
		
		/**
		 * 获取android
		 *
		 * @return PlatformNotification
		 */
		public PlatformNotification getAndroid() {
			return android;
		}
		
		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
