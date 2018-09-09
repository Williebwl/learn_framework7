using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BIStudio.Framework
{
    /// <summary>
    /// 页面信息
    /// </summary>
    public struct ContextPage : IContextPage
    {
        private Uri pageurl;
        public ContextPage(
            Uri pageurl = default(Uri))
            : this()
        {
            this.pageurl = pageurl;
        }

        #region 解析服务器资源
        /// <summary>
        /// 获得完整的绝对路径
        /// </summary>
        /// <param name="relativeUrl">与 System.Web.UI.Control.TemplateSourceDirectory 属性相关联的 URL。</param>
        /// <returns>转换后的 URL。</returns>
        public string ResolveFullUrl(string relativeUrl)
        {
            if ((relativeUrl ?? "").StartsWith("http://"))
                return relativeUrl;
            return pageurl.GetLeftPart(UriPartial.Authority) + HttpUtility.UrlDecode(ResolveUrl(relativeUrl));
        }
        /// <summary>
        /// 获得网站的绝对路径
        /// </summary>
        /// <param name="relativeUrl">与 System.Web.UI.Control.TemplateSourceDirectory 属性相关联的 URL。</param>
        /// <returns>转换后的 URL。</returns>
        public string ResolveUrl(string relativeUrl)
        {
            if (relativeUrl == null) return null;

            if (relativeUrl.Length == 0 || relativeUrl[0] == '/' ||
                relativeUrl[0] == '\\') return relativeUrl;

            int idxOfScheme =
              relativeUrl.IndexOf(@"://", StringComparison.Ordinal);
            if (idxOfScheme != -1)
            {
                int idxOfQM = relativeUrl.IndexOf('?');
                if (idxOfQM == -1 || idxOfQM > idxOfScheme) return relativeUrl;
            }

            StringBuilder sbUrl = new StringBuilder();
            sbUrl.Append(HttpRuntime.AppDomainAppVirtualPath);
            if (sbUrl.Length == 0 || sbUrl[sbUrl.Length - 1] != '/') sbUrl.Append('/');

            // found question mark already? query string, do not touch!
            bool foundQM = false;
            bool foundSlash; // the latest char was a slash?
            if (relativeUrl.Length > 1
                && relativeUrl[0] == '~'
                && (relativeUrl[1] == '/' || relativeUrl[1] == '\\'))
            {
                relativeUrl = relativeUrl.Substring(2);
                foundSlash = true;
            }
            else foundSlash = false;
            foreach (char c in relativeUrl)
            {
                if (!foundQM)
                {
                    if (c == '?') foundQM = true;
                    else
                    {
                        if (c == '/' || c == '\\')
                        {
                            if (foundSlash) continue;
                            else
                            {
                                sbUrl.Append('/');
                                foundSlash = true;
                                continue;
                            }
                        }
                        else if (foundSlash) foundSlash = false;
                    }
                }
                sbUrl.Append(c);
            }

            return sbUrl.ToString();
        }
        /// <summary>
        /// 获得项目的绝对路径
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public string ResolveSiteUrl(string relativeUrl)
        {
            string currentUrl = ResolveUrl(relativeUrl);
            string rootUrl = ResolveUrl("~/");

            if (currentUrl.StartsWith(rootUrl))
                return currentUrl.Substring(rootUrl.Length - 1);
            else
                return relativeUrl;
        }

        #endregion

    }
}
