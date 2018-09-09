using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI
{
    /// <summary>
    /// 视图模型拓展
    /// </summary>
    public static class ViewModelExt
    {
        /// <summary>
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <typeparam name="T">当前对象类型</typeparam>
        /// <param name="vm">需要创建浅表副本的对象</param>
        /// <returns>当前对象的浅表副本。</returns>
        public static T Clone<T>(this T vm) where T : ViewModel
        {
            return vm.Clone<T>();
        }
    }
}
