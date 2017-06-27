using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
   internal static class RongCloudExtensions
    {
        private static Random random = new Random();

        /// <summary>
        /// 添加融云请求的通用头部信息
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <param name="appKey">融云AppKey</param>
        /// <param name="appSecret">融云AppSecret</param>
        public static void AddRongCloudHeader(this IRestRequest request,string appKey,string appSecret)
        {
            int rd_i = random.Next();

            string nonce = rd_i.ToString();
            string timestamp = Convert.ToString(DateTime.Now.ToTimestamp());
            string signature = GetHash(appSecret + nonce + timestamp).ToLowerInvariant();
            //appkey
            request.AddHeader("App-Key",appKey);
            //随机字符串
            request.AddHeader("Nonce", nonce);
            //时间戳
            request.AddHeader("Timestamp", timestamp);
            //根据字符串获取的签名
            request.AddHeader("Signature", signature);
        }
        /// <summary>
        /// 将字符串进行sha1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetHash(string input)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            UTF8Encoding enc = new UTF8Encoding();
            byte[] dataToHash = enc.GetBytes(input);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
    }
}
