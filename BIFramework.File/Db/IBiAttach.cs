
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.File
{
    /// <summary>
    /// 附件管理
    /// </summary>
    public interface IBiAttach
    {
        /// <summary>
        /// 保存附件信息
        /// </summary>
        /// <param name="delIDs">需要删除的附件ids</param>
        /// <param name="infos">附件信息</param>
        /// <returns>是否保存成功</returns>
        bool Save(string delIDs, params SYSAttach[] infos);

        /// <summary>
        /// 删除附件信息
        /// </summary>
        /// <param name="tableName">业务表名称</param>
        /// <param name="tableID">业务表id</param>
        /// <param name="customType">拓展类型</param>
        /// <returns>是否删除成功</returns>
        bool Remove(string tableName, long tableID, int customType);

        /// <summary>
        /// 删除附件信息
        /// </summary>
        /// <param name="ids">附件ids</param>
        /// <returns>是否删除成功</returns>
        bool Remove(string ids);

        /// <summary>
        /// 查询附件信息
        /// </summary>
        /// <param name="attachKeys">业务表名称</param>
        /// <returns>附件信息</returns>
        IList<SYSAttach> GetInfos(string[] attachKeys);

        /// <summary>
        /// 查询附件信息
        /// </summary>
        /// <param name="tableName">业务表名称</param>
        /// <param name="tableID">业务表id</param>
        /// <param name="customType">拓展类型</param>
        /// <returns>附件信息</returns>
        List<SYSAttach> GetInfos(string tableName, long tableID, int customType);

        /// <summary>
        /// 查询附件信息
        /// </summary>
        /// <param name="ids">附件ids</param>
        /// <returns>附件信息</returns>
        List<SYSAttach> GetInfos(string ids);

        /// <summary>
        /// 查询附件信息
        /// </summary>
        /// <param name="id">附件id</param>
        /// <returns>附件信息</returns>
        SYSAttach GetInfo(long id);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="action">文件流操作</param>
        /// <param name="infos">文件信息</param>
        void ReadFile(Action<Stream> action, params SYSAttach[] infos);
    }
}
