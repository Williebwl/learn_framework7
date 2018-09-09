using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace BIStudio.Framework.File
{
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Domain;
    using Dapper;

    /// <summary>
    /// 系统附件
    /// </summary>
    [Table("SYSAttach")]
    public class SYSAttach : Entity
    {
        /// <summary>
        /// 自定义类型(适用多表)
        /// </summary>
        public string BindTableName { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        public long? BindTableID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int? FileSize { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 存储模式（0-数据库存储；1-硬盘存储）
        /// </summary>
        public int? Mode { get; set; }

        /// <summary>
        /// 文件相对路径（文件存储在硬盘时）
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 自定义类型，用于扩展
        /// </summary>
        public int? CustomType { get; set; }

        /// <summary>
        /// 上传用户编号
        /// </summary>
        public long? InputerID { get; set; }

        /// <summary>
        /// 上传用户名称
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime? InputTime { get; set; }

        /// <summary>
        /// 附件内容
        /// </summary>
        [Column(IsExtend = true)]
        public Stream FileStream { get; set; }

        /// <summary>
        /// 附件读取
        /// </summary>
        /// <param name="id">附件id</param>
        /// <param name="action">文件流操作</param>
        /// <param name="unitOfWork">操作单元</param>
        public void ReadFile(long? id, Action<Stream> action, IUnitOfWork unitOfWork = null)
        {
            if (!id.HasValue) id = this.ID;

            if (!(id > 0) || action == null) return;

            var builder = DBBuilder.Define(@"SELECT FileContent.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() FROM STDAttach Where ID=@ID", new { ID = id });

            (unitOfWork ?? this.UnitOfWork).ToReader(builder, reader =>
            {
                using (var sqlReader = (reader as IWrappedDataReader).Reader as SqlDataReader)
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            using (var stream = new SqlFileStream(sqlReader.GetString(0), sqlReader.GetSqlBytes(1).Buffer, FileAccess.Read))
                            {
                                action(stream);
                            }
                        }
                    }
                }
            });
        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="outputStream">需要保存的文件</param>
        /// <param name="id">附件id</param>
        /// <param name="unitOfWork">操作单元</param>
        public void WriteFile(Stream outputStream, long? id = null)
        {
            if (!id.HasValue) id = this.ID;

            if (!(id > 0) || outputStream == null) return;

            var builder = DBBuilder.Define(@"UPDATE STDAttach SET FileContent=CAST('' as varbinary(max)) WHERE ID=@ID; SELECT FileContent.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() FROM STDAttach Where ID=@ID", new { ID = id });

            this.UnitOfWork.ToReader(builder, reader =>
            {
                using (var sqlReader = (reader as IWrappedDataReader).Reader as SqlDataReader)
                {
                    while (sqlReader.Read())
                    {
                        using (var stream = new SqlFileStream(sqlReader.GetString(0), sqlReader.GetSqlBytes(1).Buffer, FileAccess.Write))
                        {
                            outputStream.CopyTo(stream);
                        }
                    }
                }
            });
        }

    }
}
