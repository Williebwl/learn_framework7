using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace BIStudio.Framework.Utils
{
    /// <summary>
    /// 把枚举值按照指定的文本显示
    /// <remarks>
    /// 一般通过枚举值的ToString()可以得到变量的文本，
    /// 但是有时候需要的到与之对应的更充分的文本，
    /// 这个类帮助达到此目的
    /// Date: 2012-1-11 
    /// </remarks>
    /// </summary>
    /// <example>
    /// [ALEnumDescription("中文数字")]
    /// enum MyEnum
    /// {
    ///		[ALEnumDescription("数字一")]
    /// 	One = 1, 
    /// 
    ///		[ALEnumDescription("数字二")]
    ///		Two, 
    /// 
    ///		[ALEnumDescription("数字三")]
    ///		Three
    /// }
    /// ALEnumDescription.GetEnumText(typeof(MyEnum));
    /// ALEnumDescription.GetFieldText(MyEnum.Two);
    /// ALEnumDescription.GetFieldTexts(typeof(MyEnum)); 
    /// 绑定到下拉框：
    /// comboBox1.DataSource = ALEnumDescription.GetFieldTexts(typeof(OrderStateEnum));
    /// comboBox1.ValueMember = "EnumValue";
    /// comboBox1.DisplayMember = "EnumDisplayText";
    /// comboBox1.SelectedValue = (int)OrderStateEnum.Finished;  //选中值
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public partial class ALEnumDescription : Attribute
    {
        #region 构造函数
        private static System.Collections.Hashtable cachedEnum = new Hashtable(); 

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public ALEnumDescription(string enumDisplayText, int enumRank)
        {
            this.enumDisplayText = enumDisplayText;
            this.enumRank = enumRank;
        }

        /// <summary>
        /// 描述枚举值，默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public ALEnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5) { } 
        #endregion

        #region 属性
        private string enumDisplayText;
        public string EnumDisplayText
        {

            get { return this.enumDisplayText; }
        }

        private int enumRank;
        public int EnumRank
        {
            get { return enumRank; }
        }

        private FieldInfo fieldIno;
        public int EnumValue
        {
            get { return (int)fieldIno.GetValue(null); }
        }

        public string FieldName
        {
            get { return fieldIno.Name; }
        } 

        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType
        {
            /// <summary>
            ///按枚举顺序默认排序
            /// </summary>
            Default,

            /// <summary>
            /// 按描述值排序
            /// </summary>
            DisplayText,

            /// <summary>
            /// 按排序熵
            /// </summary>
            Rank
        }
        #endregion

        #region 得到对枚举的描述文本
        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumText(Type enumType)
        {
            ALEnumDescription[] eds = (ALEnumDescription[])enumType.GetCustomAttributes(typeof(ALEnumDescription), false);
            if (eds.Length != 1) return string.Empty;
            return eds[0].EnumDisplayText;
        } 
        #endregion

        #region 获得指定枚举类型中，指定值的描述文本
        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本
        /// </summary>
        /// <param name="enumType">传入枚举大类型（如：GenderEnum）</param>
        /// <param name="enumItem">所指定枚举项的value值</param>
        /// <param name="isFormatStr">格式化描述文本，即去掉文本前的数字</param>
        /// <returns></returns>
        public static string GetFieldText(Type enumType, int enumItem, bool isFormatStr)
        {
            ALEnumDescription[] descriptions = GetFieldTexts(enumType, SortType.Default);
            foreach (ALEnumDescription ed in descriptions)
            {
                if (ed.EnumValue == enumItem) return isFormatStr ? ALFormater.FormatStringNumber(ed.EnumDisplayText) : ed.EnumDisplayText;
            }
            return string.Empty;
        }

        public static string GetFieldText(Type enumType, int enumItem)
        {
            return GetFieldText(enumType, enumItem, false);
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <param name="isFormatStr">格式化描述文本，即格式化掉显示字符串前边的数字以及空格 例："1 合理咯"==>"合理咯"</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue, bool isFormatStr)
        {
            ALEnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (ALEnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString()) return isFormatStr ? ALFormater.FormatStringNumber(ed.EnumDisplayText) : ed.EnumDisplayText;
            }
            return string.Empty;
        }

        public static string GetFieldText(object enumValue)
        {
            return GetFieldText(enumValue, false);
        }


        public static string GetFieldText<TEnum>(TEnum enumValue) where TEnum:struct
        {
            var attrs = typeof(TEnum).GetField(enumValue.ToString()).GetCustomAttributes<ALEnumDescription>()
                           .ToList();
            return attrs.Any() ? attrs[0].EnumDisplayText : string.Empty;
        }

        #endregion

        #region 获得指定枚举类型中，指定值对应的对象。
        /// <summary>
        /// 获得指定枚举类型中，指定值对应的对象。
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>枚举对象</returns>
        public static ALEnumDescription GetEnumText(object enumValue)
        {
            ALEnumDescription[] descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (ALEnumDescription ed in descriptions)
            {
                if (ed.fieldIno.Name == enumValue.ToString()) return ed;
            }
            return null;
        } 
        #endregion

        #region 得到枚举类型定义的所有文本
        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <param name="enumTypeName"></param>
        /// <returns></returns>
        public static ALEnumDescription[] GetFieldTexts(string enumTypeName)
        {
            return GetFieldTexts(Type.GetType(enumTypeName));
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static ALEnumDescription[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Rank);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static ALEnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            ALEnumDescription[] descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList enumDescAL = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(ALEnumDescription), false);
                    if (eds.Length != 1) continue;
                    ((ALEnumDescription)eds[0]).fieldIno = fi;
                    enumDescAL.Add(eds[0]);
                }
                if (cachedEnum.Contains(enumType.FullName) == false)
                    cachedEnum.Add(enumType.FullName, (ALEnumDescription[])enumDescAL.ToArray(typeof(ALEnumDescription)));
            }

            descriptions = (ALEnumDescription[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0) throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");

            //按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++)
            {
                //默认就不排序了
                if (sortType == SortType.Default) break;

                for (int n = m; n < descriptions.Length; n++)
                {
                    ALEnumDescription temp;
                    bool swap = false;

                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (string.Compare(descriptions[m].EnumDisplayText, descriptions[n].EnumDisplayText) > 0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank) swap = true;
                            break;
                    }

                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        } 
        #endregion

        #region 根据某个枚举项的描述文本，得到枚举的值(ID)
        /// <summary>
        /// 根据某个枚举项的描述文本，得到枚举的值(ID)
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="DisplayText"></param>
        /// <returns>
        /// 找到则返回枚举的值(ID)
        /// 没找到返回int.MinValue</returns>
        public static int GetEnumValueByDisplayText(Type enumType, string DisplayText)
        {
            ALEnumDescription[] descriptions = ALEnumDescription.GetFieldTexts(enumType);
            foreach (ALEnumDescription ed in descriptions)
            {
                if (ed.EnumDisplayText == DisplayText)
                {
                    return ed.EnumValue;
                }
            }
            return int.MinValue;
        } 
        #endregion

        #region 根据某个枚举项的描述文本，得到枚举的值(ID)
        /// <summary>
        /// 根据某个枚举项的描述文本，得到枚举的值(ID)
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="DisplayText"></param>
        /// <returns>
        /// 找到则返回枚举的值(ID)
        /// 没找到返回enumDisplayText</returns>
        public static string GetEnumValueByDBText(Type enumType, string DisplayText)
        {
            ALEnumDescription[] descriptions = ALEnumDescription.GetFieldTexts(enumType);
            foreach (ALEnumDescription ed in descriptions)
            {
                if (ed.FieldName == DisplayText)
                {
                    return ed.enumDisplayText;
                }
            }
            return "未知";
        }
        #endregion 
    }
}
