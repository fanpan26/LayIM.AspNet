using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Lib
{
    public static class TokenExtension
    {
        const string SecretKey = "LAYIM_RONG_CLOUD";

        /// <summary>
        /// 转换成token
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToToken(this object value)
        {
            return JWT.JsonWebToken.Encode(value, SecretKey, JWT.JwtHashAlgorithm.HS384);
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string DecodeToken(string token)
        {
            if (string.IsNullOrEmpty(token)) {
                return "";
            }
            string obj = JWT.JsonWebToken.Decode(token, SecretKey);
            return obj;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string token)
        {
            string obj = DecodeToken(token);
            return JWT.JsonWebToken.JsonSerializer.Deserialize<T>(obj);
        }

        public static string ToMD5(this string value)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(value);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            return OutString.ToLower();
        }
    }
}
