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
    
    using BIStudio.Framework.Domain;
    using BIStudio.Framework.Data;
    using BIStudio.Framework.Utils;

    /// <summary>
    /// 附件管理
    /// </summary>
    public class STDAttachBO : Repository<SYSAttach>, IBiAttach
    {
        #region 保存

        /// <summary>
        /// 保存附件信息
        /// </summary>
        /// <param name="delIDs">需要删除的附件ids</param>
        /// <param name="infos">附件信息</param>
        /// <returns>是否保存成功</returns>
        public virtual bool Save(string delIDs, params SYSAttach[] infos)
        {
            var result = false;

            delIDs = (delIDs ?? string.Empty).Trim(',');

            if (infos == null || !infos.Any()) goto End;

            using (var dbContext = BoundedContext.Create())
            {
                var bo = dbContext.Resolve<STDAttachBO>();//dbContext.Repository<STDAttachInfo>();

                result = true;

                var arr = ALConvert.ToList<long>(delIDs);
                if (arr.Any()) result = bo.Remove(item => arr.Contains(item.ID.Value));

                if (result)
                {
                    foreach (var info in infos)
                    {
                        if (info == null || !(info.ID > 0 ? bo.Modify(info) : bo.Add(info))) { result = false; break; }

                        SaveFile(dbContext, info);
                    }
                }

                if (result) dbContext.Commit();
            }

        End: return result;
        }

        protected virtual void SaveFile(IBoundedContext dbContext, SYSAttach info)
        {
            if (info.Mode == (int)ModeEnum.数据库存储 && info.FileStream != null)
            {
                info.DependOn(dbContext);
                info.WriteFile(info.FileStream, info.ID.Value);

                info.FileStream.Close();
                info.FileStream.Dispose();
                info.FileStream = null;
            }
        }

        #endregion 保存

        #region 删除

        public virtual bool Remove(string tableName, long tableID, int customType = 0)
        {
            return Remove(GetInfos(tableName, tableID, customType).ToArray());
        }

        public virtual bool Remove(params SYSAttach[] infos)
        {
            if (infos == null) throw new ArgumentNullException("infos");

            var result = true;

            if (!infos.Any()) goto End;

            using (var dbContext = BoundedContext.Create())
            {
                foreach (var info in infos) { if (!dbContext.Remove(info)) { result = false; break; } }

                if (result) dbContext.Commit();
                else dbContext.Rollback();
            }

        End: return result;
        }

        public virtual bool Remove(string ids)
        {
            ids = (ids ?? string.Empty).Trim();

            var arr = ALConvert.ToList<long>(ids);
            if (arr.Count==0) return false;

            return base.Remove(item => arr.Contains(item.ID.Value));
        }

        #endregion 删除

        #region 查询

        public virtual IList<SYSAttach> GetInfos(string[] attachKeys)
        {
            var q = from d in this.Entities
                    where attachKeys.Contains(d.BindTableName)
                    select d;

            return q.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableID"></param>
        /// <param name="customType"></param>
        /// <returns></returns>
        public virtual List<SYSAttach> GetInfos(string tableName, long tableID, int customType = 0)
        {
            var q = from d in this.Entities
                    where d.BindTableName == tableName && d.BindTableID == tableID && d.CustomType == customType
                    select d;

            return q.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual List<SYSAttach> GetInfos(string ids)
        {
            ids = (ids ?? string.Empty).Trim(',');

            var arr = ALConvert.ToList<long>(ids);
            if (arr.Count==0) return null;

            return this.GetAll(item => arr.Contains(item.ID.Value)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SYSAttach GetInfo(long id)
        {
            return this.Get(id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="infos"></param>
        public virtual void ReadFile(Action<Stream> action, params SYSAttach[] infos)
        {
            if (infos == null || !infos.Any()) return;

            using (var dbContext = BoundedContext.Create())
            {
                foreach (var info in infos)
                {
                    if (info.Mode == (int)ModeEnum.数据库存储 && info.ID > 0)
                    {
                        info.ReadFile(info.ID.Value, action, dbContext.UnitOfWork);
                    }
                }

                dbContext.Commit();
            }
        }

        #endregion 查询
    }
}
