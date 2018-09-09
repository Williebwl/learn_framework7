using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.UI
{
    using BIStudio.Framework.Domain;
    using Models;

    /// <summary>
    /// TreeHelp
    /// </summary>
    public static class TreeHelp
    {
        /// <summary>
        /// 将数据转换为树结构
        /// </summary>
        /// <typeparam name="T">要转换的数据类型</typeparam>
        /// <param name="infos">需要转换的数据集</param>
        /// <returns>数据树结构</returns>
        public static IList<T> GetTree<T>(this IList<T> infos) where T : ITreeVM
        {
            if (infos == null) return infos;

            var tree = new List<T>();

            T pInfo = default(T);

            foreach (var info in infos)
            {
                if (info.ParentID.GetValueOrDefault(0) == 0 || pInfo == null) tree.Add(pInfo = info);
                else
                {
                    if (pInfo.ID == info.ParentID || (pInfo = infos.FirstOrDefault(d => d.ID == info.ParentID)) != null)
                    {
                        if (pInfo.Children == null) pInfo.Children = new List<ITreeVM>();

                        pInfo.Children.Add(info);
                    }
                    else tree.Add(pInfo = info);
                }
            }

            return tree;
        }
    }
}
