using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 查询条件处理类
    /// </summary>
    /// <remarks>
    /// [2012-03-11]
    /// </remarks>
    public static class ALCondition
    {
        #region 获取日期查询条件
        /// <summary>
        /// 获取日期查询条件 如：yyyy-MM-dd
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="begDate">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回查询条件</returns>
        public static string GetDateCondition(string field, DateTime? begDate, DateTime? endDate)
        {
            if (begDate.HasValue && endDate.HasValue)
            {
                return string.Format(" And {0} between '{1}' and '{2}' ", field, begDate.Value.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));
            }
            else if (begDate.HasValue)
            {
                return string.Format(" And {0} >= '{1}' ", field, begDate.Value.ToString("yyyy-MM-dd"));
            }
            else if (endDate.HasValue)
            {
                return string.Format(" And {0} <= '{1}' ", field, endDate.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取日期时间查询条件 如：yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="begDateTime">开始日期时间</param>
        /// <param name="endDateTime">结束日期时间</param>
        /// <returns>返回查询条件</returns>
        public static string GetDateTimeCondition(string field, DateTime? begDateTime, DateTime? endDateTime)
        {
            if (begDateTime.HasValue && endDateTime.HasValue)
            {
                return string.Format(" And {0} between '{1}' and '{2}' ", field, begDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), endDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (begDateTime.HasValue)
            {
                return string.Format(" And {0} >= '{1}' ", field, begDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (endDateTime.HasValue)
            {
                return string.Format(" And {0} <= '{1}' ", field, endDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 获取金额条件
        /// <summary>
        /// 获取金额条件(默认带And条件)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <returns></returns>
        public static string GetSqlCondition(string field, decimal? minMoney, decimal? maxMoney)
        {
            return GetSqlCondition(field, minMoney, maxMoney, true);
        }

        /// <summary>
        /// 获取金额条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="minMoney"></param>
        /// <param name="maxMoney"></param>
        /// <param name="hasAnd">是否包含And</param>
        /// <returns></returns>
        public static string GetSqlCondition(string field, decimal? minMoney, decimal? maxMoney, bool hasAnd)
        {
            if (minMoney.HasValue || maxMoney.HasValue)
            {
                string condition;
                if (minMoney.HasValue && maxMoney.HasValue)
                {
                    condition = string.Format(" {0} between {1} and {2} ", field, minMoney.Value.ToString(), maxMoney.Value.ToString());
                }
                else if (minMoney.HasValue)
                {
                    condition = string.Format(" {0} >= {1} ", field, minMoney.Value.ToString());
                }
                else
                {
                    condition = string.Format(" {0} <= {1} ", field, maxMoney.Value.ToString());
                }
                return " " + (hasAnd ? "and" : "") + condition;
            }
            return string.Empty;
        }
        #endregion
    }
}
