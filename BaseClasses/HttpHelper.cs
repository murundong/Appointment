using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BaseClasses
{
    public static partial class HttpHelper
    {
        private const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private static readonly Encoding DefaultRequestEncoding = Encoding.GetEncoding("utf-8");

        /// <summary>
        /// 创建GET方式访问
        /// Author: George liu
        /// </summary>
        /// <param name="url">访问url</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="userAgent">用户代理</param>
        /// <param name="cookies">cookie信息</param>
        /// <returns></returns>
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout = null, string userAgent = null, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url)) { return null; }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent)) { request.UserAgent = userAgent; }
            if (timeout.HasValue) { request.Timeout = timeout.Value; }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 创建POST访问方式
        /// Author: George liu
        /// </summary>
        /// <param name="url">访问url</param>
        /// <param name="parameters">参数键值对</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="userAgent">用户代理</param>
        /// <param name="requestEncoding">编码类型</param>
        /// <param name="cookies">cookie信息</param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters = null, int? timeout = null, string userAgent = null,
                                                             Encoding requestEncoding = null, CookieCollection cookies = null)
        {
            if (string.IsNullOrEmpty(url)) { return null; }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent)) { request.UserAgent = userAgent; }
            if (timeout.HasValue) { request.Timeout = timeout.Value; }
            if (requestEncoding == null) { requestEncoding = DefaultRequestEncoding; }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                var keyArray = parameters.Keys.ToArray();
                for (int i = 0; i < keyArray.Length; i++)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", keyArray[i], parameters[keyArray[i]]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", keyArray[i], parameters[keyArray[i]]);
                    }
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            var clientIp = string.Empty;
            var request = HttpContext.Current.Request;
            if (request.ServerVariables["HTTP_VIA"] != null)
            {
                clientIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                clientIp = request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return clientIp;
        }
    }
}
