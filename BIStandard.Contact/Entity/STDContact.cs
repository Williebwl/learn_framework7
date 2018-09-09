using BIStudio.Framework.Data;
using BIStudio.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Standard.Contact
{
    /// <summary>
    /// 联系人
    /// </summary>
    [Table("STDContact")]
    public class STDContact : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public long? UserID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 用户简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 联系人所有者
        /// </summary>
        public string BindTableName { get; set; }

        /// <summary>
        /// 联系人所有者编号
        /// </summary>
        public long? BindTableID { get; set; }

        /// <summary>
        /// 联系人所有者名称
        /// </summary>
        public string BindTableIDValue { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否默认联系人
        /// </summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// 联系人类型（多选）
        /// </summary>
        public string ContactType { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 个人简历
        /// </summary>
        public object ContentXML { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? UnitID { get; set; }
    }

}
