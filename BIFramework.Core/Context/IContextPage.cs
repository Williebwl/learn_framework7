using System;
namespace BIStudio.Framework
{
    /// <summary>
    /// 页面信息
    /// </summary>
    public interface IContextPage
    {
        /// <summary>
        /// 获得完整的绝对路径
        /// </summary>
        /// <param name="relativeUrl">与 System.Web.UI.Control.TemplateSourceDirectory 属性相关联的 URL。</param>
        /// <returns>转换后的 URL。</returns>
        string ResolveFullUrl(string relativeUrl);
        /// <summary>
        /// 获得网站的绝对路径
        /// </summary>
        /// <param name="relativeUrl">与 System.Web.UI.Control.TemplateSourceDirectory 属性相关联的 URL。</param>
        /// <returns>转换后的 URL。</returns>
        string ResolveSiteUrl(string relativeUrl);
        /// <summary>
        /// 获得项目的绝对路径
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        string ResolveUrl(string relativeUrl);
    }
}
