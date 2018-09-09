using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public struct ContextServer : IContextServer
    {
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public string UrlEncode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (var c in input)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c == '_'))
                    sb.Append(c);
                else
                {
                    var buffer = Encoding.Unicode.GetBytes(new char[] { c });
                    sb.AppendFormat("%u{1:x2}{0:x2}", buffer[0], buffer[1]);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="query">需要编码的请求参数</param>
        /// <returns></returns>
        public string UrlEncode(NameValueCollection query)
        {
            return UrlEncode(query, null);
        }
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="query">需要编码的请求参数</param>
        /// <param name="args">需要替换的请求参数</param>
        /// <returns></returns>
        public string UrlEncode(NameValueCollection query, Dictionary<string, string> args)
        {
            query = query ?? new NameValueCollection();
            args = args ?? new Dictionary<string, string>();

            StringBuilder sb = new StringBuilder();
            foreach (var kv in query.AllKeys.Except(args.Keys))
                sb.AppendFormat("{0}{1}={2}", (sb.Length > 0 ? "&" : "?"), UrlEncode(kv), UrlEncode(query[kv]));
            foreach (var kv in args)
                sb.AppendFormat("{0}{1}={2}", (sb.Length > 0 ? "&" : "?"), UrlEncode(kv.Key), UrlEncode(kv.Value));
            return sb.ToString();
        }
        /// <summary>
        /// 检索虚拟路径（绝对的或相对的）或应用程序相关的路径映射到的物理路径。
        /// </summary>
        /// <param name="virtualPath">表示虚拟路径的 System.String。</param>
        /// <returns>与虚拟路径或应用程序相关的路径关联的物理路径。</returns>
        public string MapPath(string virtualPath)
        {
            return MapPath(virtualPath, AppDomain.CurrentDomain.BaseDirectory);
        }
        public string MapPath(string virtualPath, string basePath)
        {
            if (!virtualPath.StartsWith("~") && !virtualPath.StartsWith("/"))
                return virtualPath;

            virtualPath = virtualPath.TrimStart('~', '/', '\\');
            basePath = basePath.TrimEnd('\\');
            return basePath + "\\" + virtualPath.Replace('/', '\\');
        }
    }
}
