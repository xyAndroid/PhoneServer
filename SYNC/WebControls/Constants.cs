using System;
using System.Collections.Generic;
using System.Text;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 表示常量集合。
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 与页面请求参数相关的常量。
        /// </summary>
        public class RequestParameter
        {
            /// <summary>
            /// 表示从页面回发的请求目标，一般为某个能够引发回发请求控件的ID。
            /// </summary>
            public const string EventTarget = "__EVENTTARGET";
            /// <summary>
            /// 表示从页面回发的请求参数，一般为某个能够引发回发请求控件指定的参数值，
            /// 具体指控件的客户端事件函数 __doPostBack 的第二个参数。
            /// </summary>
            public const string EventArgument = "__EVENTARGUMENT";
        }

        /// <summary>
        /// 数据源类型。
        /// </summary>
        public class DataSourceType
        {
            /// <summary>
            /// 以 ADO.Net 作为数据源。
            /// </summary>
            public const string ADONet = "ADONet";
            /// <summary>
            /// 以 SQL 作为数据源。
            /// </summary>
            public const string SQL = "SQL";
            /// <summary>
            /// 以 Linq 作为数据源。
            /// </summary>
            public const string Linq = "Linq";
        }

        /// <summary>
        /// 表示比较关系运算的常量。
        /// </summary>
        public class Comparer
        {
            /// <summary>
            /// 等于
            /// </summary>
            public const string Equal = "=";
            /// <summary>
            /// 不等于
            /// </summary>
            public const string NotEqual = "<>";
            /// <summary>
            /// 大于
            /// </summary>
            public const string GreatThan = ">";
            /// <summary>
            /// 大于等于
            /// </summary>
            public const string GreatEqualThan = ">=";
            /// <summary>
            /// 小于
            /// </summary>
            public const string LessThan = "<";
            /// <summary>
            /// 小于等于
            /// </summary>
            public const string LessEqualThan = "<=";
            /// <summary>
            /// 包含
            /// </summary>
            public const string Include = "LIKE";
            /// <summary>
            /// 不包含
            /// </summary>
            public const string NotInclude = "NOT LIKE";
            /// <summary>
            /// 为空
            /// </summary>
            public const string IsNull = "IS NULL";
            /// <summary>
            /// 不为空
            /// </summary>
            public const string IsNotNull = "IS NOT NULL";

            /// <summary>
            /// 根据指定的比较运算符号得到一个友好的文本。
            /// </summary>
            /// <param name="comparer">比较运算符号</param>
            /// <returns>与比较运算符号对应的友好文本。</returns>
            public static string GetText(string comparer)
            {
                return GetText(comparer, System.Globalization.CultureInfo.CurrentCulture);
            }

            /// <summary>
            /// 根据指定的比较运算符号得到一个友好的文本。
            /// </summary>
            /// <param name="comparer">比较运算符号</param>
            /// <param name="culture">指定的区域性</param>
            /// <returns>与比较运算符号对应的友好文本。</returns>
            public static string GetText(string comparer, System.Globalization.CultureInfo culture)
            {
                string text = string.Empty;
                switch (comparer)
                {
                    case Comparer.Equal:
                        text = "等于";
                        break;
                    case Comparer.NotEqual:
                        text = "不等于";
                        break;
                    case Comparer.GreatThan:
                        text = "大于";
                        break;
                    case Comparer.GreatEqualThan:
                        text = "大于等于";
                        break;
                    case Comparer.LessThan:
                        text = "小于";
                        break;
                    case Comparer.LessEqualThan:
                        text = "小于等于";
                        break;
                    case Comparer.Include:
                        text = "包含";
                        break;
                    case Comparer.NotInclude:
                        text = "不包含";
                        break;
                    case Comparer.IsNull:
                        text = "为空";
                        break;
                    case Comparer.IsNotNull:
                        text = "不为空";
                        break;
                    default:
                        break;
                }
                return text;
            }
        }

        /// <summary>
        /// 表示逻辑运算的常量。
        /// </summary>
        public class Logic
        {
            /// <summary>
            /// 逻辑与
            /// </summary>
            public const string And = "AND";
            /// <summary>
            /// 逻辑或
            /// </summary>
            public const string Or = "OR";

            /// <summary>
            /// 代表CSharp语言中的逻辑与
            /// </summary>
            public const string CSharpAnd = "&&";
            /// <summary>
            /// 代表CSharp语言中的逻辑或
            /// </summary>
            public const string CSharpOr = "||";

            /// <summary>
            /// 获得与逻辑运算符号对应的一个友好文本。
            /// </summary>
            /// <param name="logic">逻辑运算符号</param>
            /// <returns>与指定的逻辑运算符号对应的友好文本。</returns>
            public static string GetText(string logic)
            {
                return GetText(logic, System.Globalization.CultureInfo.CurrentCulture);
            }

            /// <summary>
            /// 获得与逻辑运算符号对应的一个友好文本。
            /// </summary>
            /// <param name="logic">逻辑运算符号</param>
            /// <param name="culture">指定的区域性</param>
            /// <returns>与指定的逻辑运算符号对应的友好文本。</returns>
            public static string GetText(string logic, System.Globalization.CultureInfo culture)
            {
                string text = string.Empty;
                switch (logic)
                {
                    case Logic.And:
                        text = "与";
                        break;
                    case Logic.CSharpAnd:
                        text = "与";
                        break;
                    case Logic.Or:
                        text = "或";
                        break;
                    case Logic.CSharpOr:
                        text = "或";
                        break;
                    default:
                        break;
                }
                return text;
            }
        }

        /// <summary>
        /// 字段类型。
        /// </summary>
        public class FieldType
        {
            private static readonly Type[] _textTypes = new Type[]{
                typeof(System.String), typeof(System.Char) };

            private static readonly Type[] _numericTypes = new Type[]{
                typeof(System.Int16), typeof(System.Int32), typeof(System.Int64),
                typeof(System.UInt16), typeof(System.UInt32), typeof(System.UInt64),
                typeof(System.Single), typeof(System.Double),
                typeof(System.Decimal) };

            private static readonly Type[] _dateTimeTypes = new Type[] { typeof(DateTime) };
            /// <summary>
            /// 文本类型。
            /// </summary>
            public const string Text = "S";
            /// <summary>
            /// 数字类型。
            /// </summary>
            public const string Numeric = "N";
            /// <summary>
            /// 日期时间类型。
            /// </summary>
            public const string DateTime = "D";
            /// <summary>
            /// 将类型转换成字段类型符号串。
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static string GetFieldType(Type type)
            {
                foreach (Type aType in _textTypes)
                {
                    if (type.Name.Equals(aType.Name))
                    {
                        return FieldType.Text;
                    }
                }
                foreach (Type aType in _numericTypes)
                {
                    if (type.Name.Equals(aType.Name))
                    {
                        return FieldType.Numeric;
                    }
                }
                foreach (Type aType in _dateTimeTypes)
                {
                    if (type.Name.Equals(aType.Name))
                    {
                        return FieldType.DateTime;
                    }
                }
                throw new Exception("Invalid Type.");
            }

            /// <summary>
            /// 验证字段值是否符合字段类型格式，如果不符合则返回一个无效的默认值。
            /// </summary>
            /// <param name="fieldValue"></param>
            /// <param name="fieldType"></param>
            /// <returns></returns>
            public static string Validate(string fieldValue, string fieldType)
            {
                if (fieldType.Equals(FieldType.Numeric))
                {
                    int retValue;
                    if (int.TryParse(fieldValue, out retValue))
                    {
                        return fieldValue;
                    }
                    return int.MinValue.ToString();
                }
                else if (fieldType.Equals(FieldType.DateTime))
                {
                    System.DateTime retValue;
                    if (System.DateTime.TryParse(fieldValue, out retValue))
                    {
                        return fieldValue;
                    }
                    return System.DateTime.MinValue.ToString();
                }
                return fieldValue;
            }
        }

        /// <summary>
        /// 与分页相关的常量。
        /// </summary>
        public class Paging
        {
            /// <summary>
            /// 分页大小属性名。
            /// </summary>
            public const string PageSizePopertyName = "PageSize";
            /// <summary>
            /// 当前页索引属性名。
            /// </summary>
            public const string PageIndexPropertyName = "PageIndex";
            /// <summary>
            /// 分页数属性名。
            /// </summary>
            public const string PageCountPropertyName = "PageCount";
            /// <summary>
            /// 分页设置属性名。
            /// </summary>
            public const string PagerSettingsPropertyName = "PagerSettings";
        }

        /// <summary>
        /// 与控件属性相关的常量。
        /// </summary>
        public class ControlProperties
        {
            /// <summary>
            /// Checked 的属性名。
            /// </summary>
            public const string CheckedPropertyName = "Checked";
            /// <summary>
            /// Selected 的属性名。
            /// </summary>
            public const string SelectedPropertyName = "Selected";
            /// <summary>
            /// Text 的属性名。
            /// </summary>
            public const string TextPropertyName = "Text";
            /// <summary>
            /// AutoPostBack 的属性名。
            /// </summary>
            public const string AutoPostBackPropertyName = "AutoPostBack";
        }

        /// <summary>
        /// 字符串常量。
        /// </summary>
        public class StringInfo
        {
            /// <summary>
            /// 空格常量。
            /// </summary>
            public const string Space = " ";
        }

        /// <summary>
        /// 文字常量。
        /// </summary>
        public class Literals
        {
            /// <summary>
            /// 在导出数据时候的文字提示。
            /// </summary>
            public const string ExportTips = "如果您的数据没有经过检索，或者数据量很大，导出数据的过程将会持续较长时间，您是否继续执行导出操作？";
        }
    }
}
