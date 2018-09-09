using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIStudio.Framework.UI;

namespace WebApi.Controllers.Attach
{

    public class AttachVM : ViewModel
    {
        /// <summary>
        /// 父编号
        /// </summary>
        public long? TableID { get; set; }

        /// <summary>
        /// 自定义类型(适用多表)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 存储模式（0-数据库存储；1-硬盘存储）
        /// </summary>
        public int? Mode { get; set; }

        /// <summary>
        /// 文件是否覆盖
        /// </summary>
        public int? CoverAttach { get; set; }

        /// <summary>
        /// 自定义类型，用于扩展
        /// </summary>
        public int? CustomType { get; set; }

        /// <summary>
        /// 文件上传标示
        /// </summary>
        public string AttachKey { get; set; }

        /// <summary>
        /// 文件标示
        /// </summary>
        public IList<long> FileIDs { get; set; }
    }
}