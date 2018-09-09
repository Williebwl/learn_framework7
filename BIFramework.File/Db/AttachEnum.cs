using System;
using System.Collections.Generic;
using System.Text;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 决定存储方式的枚举
    /// </summary>
    public enum ModeEnum : int
    {
        /// <summary>
        /// 数据库存储
        /// </summary>
        数据库存储 = 0,

        /// <summary>
        /// 硬盘存储
        /// </summary>
        硬盘存储 = 1,

        /// <summary>
        /// 虚拟目录存储
        /// </summary>
        虚拟目录存储 = 2
    }

    /// <summary>
    /// 决定上传文件是否覆盖的枚举
    /// </summary>
    public enum CoverAttachEnum : int
    {
        /// <summary>
        /// 不覆盖
        /// </summary>
        不覆盖 = 0,

        /// <summary>
        /// 覆盖并且删除数据
        /// </summary>
        覆盖并且删除数据 = 1,

        /// <summary>
        /// 覆盖但不删除数据
        /// </summary>
        覆盖但不删除数据 = 2,
    }

    /// <summary>
    /// 附件展现形式
    /// </summary>
    public enum EnumShowMode : int
    {
        /// <summary>
        /// 普通模式
        /// </summary>
        普通模式 = 0,

        /// <summary>
        /// 块状列表模式
        /// </summary>
        块状列表模式 = 1,

        /// <summary>
        /// 大图模式
        /// </summary>
        大图模式 = 2,

        /// <summary>
        /// 最简模式
        /// </summary>
        最简模式 = 3,
    }

    /// <summary>
    /// 附件保存模式
    /// </summary>
    public enum EnumSaveMode : int
    {
        /// <summary>
        /// 页面提交时存储附件操作
        /// </summary>
        页面提交时存储附件操作 = 0,

        /// <summary>
        /// 直接操作附件
        /// </summary>
        直接操作附件 = 1,
    }

}
