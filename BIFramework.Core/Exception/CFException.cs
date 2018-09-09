using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    /// <summary>
    /// 创建一个可预见的异常
    /// </summary>
    public sealed class CFException
    {
        /// <summary>
        /// 使用指定的错误枚举创建异常
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public static DefinedException Create(OperateResult errorCode, Exception innerException = null)
        {
            return Create<OperateResult>(errorCode, innerException);
        }
        /// <summary>
        /// 使用指定的错误枚举创建异常
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public static DefinedException Create<TEnum>(TEnum errorCode, Exception innerException = null) where TEnum : struct
        {
            var st = new StackTrace(true);
            var stackTrace = new StringBuilder();
            for (int i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                stackTrace.AppendFormat("  在 {0}", frame.GetMethod());
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                    stackTrace.AppendFormat(" 位置 {0}", frame.GetFileName());
                if (frame.GetFileLineNumber() > 0)
                    stackTrace.AppendFormat(":行号 {0}", frame.GetFileLineNumber());
                stackTrace.Append("\r\n");
            }

            var source = errorCode.GetType().FullName + "." + errorCode.ToString();
            var message = GetDescription(errorCode);

            var ex = new DefinedException(message, innerException, stackTrace.ToString());
            ex.Source = source;
            ex.HelpLink = "http://help.bitech.cn/" + ex.Source + "/";
            return ex;
        }
        
        /// <summary>
        /// 使用指定的错误代码创建异常
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public static DefinedException Create(OperateResult errorCode, string message, Exception innerException = null)
        {
            return Create<OperateResult>(errorCode, message, innerException);
        }
        /// <summary>
        /// 使用指定的错误代码创建异常
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public static DefinedException Create<TEnum>(TEnum errorCode, string message, Exception innerException = null) where TEnum : struct
        {
            var st = new StackTrace(true);
            var stackTrace = new StringBuilder();
            for (int i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);
                stackTrace.AppendFormat("  在 {0}", frame.GetMethod());
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                    stackTrace.AppendFormat(" 位置 {0}", frame.GetFileName());
                if (frame.GetFileLineNumber() > 0)
                    stackTrace.AppendFormat(":行号 {0}", frame.GetFileLineNumber());
                stackTrace.Append("\r\n");
            }

            var source = errorCode.GetType().FullName + "." + errorCode.ToString();

            var ex = new DefinedException(message, innerException, stackTrace.ToString());
            ex.Source = errorCode.ToString();
            ex.HelpLink = "http://help.bitech.cn/" + ex.Source + "/";
            return ex;
        }

        /// <summary>
        /// 获得描述信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetDescription(object obj)
        {
            string str = obj.ToString();
            FieldInfo field = obj.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }
    }
}
