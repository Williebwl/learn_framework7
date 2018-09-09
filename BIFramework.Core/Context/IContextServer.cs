using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace BIStudio.Framework
{
    public interface IContextServer
    {
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        string UrlEncode(NameValueCollection query);
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="query">需要编码的请求参数</param>
        /// <returns></returns>
        string UrlEncode(NameValueCollection query, Dictionary<string, string> args);
        /// <summary>
        /// 对链接地址进行编码
        /// </summary>
        /// <param name="query">需要编码的请求参数</param>
        /// <param name="args">需要替换的请求参数</param>
        /// <returns></returns>
        string UrlEncode(string input);
        /// <summary>
        /// 检索虚拟路径（绝对的或相对的）或应用程序相关的路径映射到的物理路径。
        /// </summary>
        /// <param name="virtualPath">表示虚拟路径的 System.String。</param>
        /// <returns>与虚拟路径或应用程序相关的路径关联的物理路径。</returns>
        string MapPath(string virtualPath);
        /// <summary>
        /// 检索虚拟路径（绝对的或相对的）或应用程序相关的路径映射到的物理路径。
        /// </summary>
        /// <param name="virtualPath">表示虚拟路径的 System.String。</param>
        /// <returns>与虚拟路径或应用程序相关的路径关联的物理路径。</returns>
        string MapPath(string virtualPath, string basePath);
    }
}
