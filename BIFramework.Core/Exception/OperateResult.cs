using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public enum OperateResult
    {
        /// <summary>
        /// 请求的信息未找到
        /// </summary>
        [Description("请求的信息未找到")]
        NotFound,
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Fail,
    }
}
