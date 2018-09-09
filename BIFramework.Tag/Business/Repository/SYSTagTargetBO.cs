using BIStudio.Framework.Domain;
using BIStudio.Framework.Tag.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Tag
{
    internal class SYSTagTargetBO : SYSTagBase<SYSTagTarget>
    {
        private static Dictionary<string, SYSTagTarget> data = new Dictionary<string, SYSTagTarget>();
        private static object lockHelper = new object();
        /// <summary>
        /// 返回指定表名的贴入对象
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal SYSTagTarget this[string tableName]
        {
            get
            {
                if (data.Keys.Contains(tableName))
                    return data[tableName];
                lock (lockHelper)
                {
                    SYSTagTarget info = this.Get(new SYSTagTarget { TargetCode = tableName }.AsSpec());
                    if (info == null)
                    {
                        //throw new ArgumentOutOfRangeException("tableName", "在TagTarget表中未找到设置项" + tableName + "");
                        //创建新标签项
                        info = new SYSTagTarget
                        {
                            TargetName = tableName,
                            TargetCode = tableName
                        };
                        Add(info);
                    }
                    if (!data.Keys.Contains(tableName))
                        data.Add(tableName, info);
                    return info;
                }
            }
        }

    }
}
