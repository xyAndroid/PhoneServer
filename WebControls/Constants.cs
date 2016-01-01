using System;
using System.Collections.Generic;
using System.Text;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// ��ʾ�������ϡ�
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// ��ҳ�����������صĳ�����
        /// </summary>
        public class RequestParameter
        {
            /// <summary>
            /// ��ʾ��ҳ��ط�������Ŀ�꣬һ��Ϊĳ���ܹ������ط�����ؼ���ID��
            /// </summary>
            public const string EventTarget = "__EVENTTARGET";
            /// <summary>
            /// ��ʾ��ҳ��ط������������һ��Ϊĳ���ܹ������ط�����ؼ�ָ���Ĳ���ֵ��
            /// ����ָ�ؼ��Ŀͻ����¼����� __doPostBack �ĵڶ���������
            /// </summary>
            public const string EventArgument = "__EVENTARGUMENT";
        }

        /// <summary>
        /// ����Դ���͡�
        /// </summary>
        public class DataSourceType
        {
            /// <summary>
            /// �� ADO.Net ��Ϊ����Դ��
            /// </summary>
            public const string ADONet = "ADONet";
            /// <summary>
            /// �� SQL ��Ϊ����Դ��
            /// </summary>
            public const string SQL = "SQL";
            /// <summary>
            /// �� Linq ��Ϊ����Դ��
            /// </summary>
            public const string Linq = "Linq";
        }

        /// <summary>
        /// ��ʾ�ȽϹ�ϵ����ĳ�����
        /// </summary>
        public class Comparer
        {
            /// <summary>
            /// ����
            /// </summary>
            public const string Equal = "=";
            /// <summary>
            /// ������
            /// </summary>
            public const string NotEqual = "<>";
            /// <summary>
            /// ����
            /// </summary>
            public const string GreatThan = ">";
            /// <summary>
            /// ���ڵ���
            /// </summary>
            public const string GreatEqualThan = ">=";
            /// <summary>
            /// С��
            /// </summary>
            public const string LessThan = "<";
            /// <summary>
            /// С�ڵ���
            /// </summary>
            public const string LessEqualThan = "<=";
            /// <summary>
            /// ����
            /// </summary>
            public const string Include = "LIKE";
            /// <summary>
            /// ������
            /// </summary>
            public const string NotInclude = "NOT LIKE";
            /// <summary>
            /// Ϊ��
            /// </summary>
            public const string IsNull = "IS NULL";
            /// <summary>
            /// ��Ϊ��
            /// </summary>
            public const string IsNotNull = "IS NOT NULL";

            /// <summary>
            /// ����ָ���ıȽ�������ŵõ�һ���Ѻõ��ı���
            /// </summary>
            /// <param name="comparer">�Ƚ��������</param>
            /// <returns>��Ƚ�������Ŷ�Ӧ���Ѻ��ı���</returns>
            public static string GetText(string comparer)
            {
                return GetText(comparer, System.Globalization.CultureInfo.CurrentCulture);
            }

            /// <summary>
            /// ����ָ���ıȽ�������ŵõ�һ���Ѻõ��ı���
            /// </summary>
            /// <param name="comparer">�Ƚ��������</param>
            /// <param name="culture">ָ����������</param>
            /// <returns>��Ƚ�������Ŷ�Ӧ���Ѻ��ı���</returns>
            public static string GetText(string comparer, System.Globalization.CultureInfo culture)
            {
                string text = string.Empty;
                switch (comparer)
                {
                    case Comparer.Equal:
                        text = "����";
                        break;
                    case Comparer.NotEqual:
                        text = "������";
                        break;
                    case Comparer.GreatThan:
                        text = "����";
                        break;
                    case Comparer.GreatEqualThan:
                        text = "���ڵ���";
                        break;
                    case Comparer.LessThan:
                        text = "С��";
                        break;
                    case Comparer.LessEqualThan:
                        text = "С�ڵ���";
                        break;
                    case Comparer.Include:
                        text = "����";
                        break;
                    case Comparer.NotInclude:
                        text = "������";
                        break;
                    case Comparer.IsNull:
                        text = "Ϊ��";
                        break;
                    case Comparer.IsNotNull:
                        text = "��Ϊ��";
                        break;
                    default:
                        break;
                }
                return text;
            }
        }

        /// <summary>
        /// ��ʾ�߼�����ĳ�����
        /// </summary>
        public class Logic
        {
            /// <summary>
            /// �߼���
            /// </summary>
            public const string And = "AND";
            /// <summary>
            /// �߼���
            /// </summary>
            public const string Or = "OR";

            /// <summary>
            /// ����CSharp�����е��߼���
            /// </summary>
            public const string CSharpAnd = "&&";
            /// <summary>
            /// ����CSharp�����е��߼���
            /// </summary>
            public const string CSharpOr = "||";

            /// <summary>
            /// ������߼�������Ŷ�Ӧ��һ���Ѻ��ı���
            /// </summary>
            /// <param name="logic">�߼��������</param>
            /// <returns>��ָ�����߼�������Ŷ�Ӧ���Ѻ��ı���</returns>
            public static string GetText(string logic)
            {
                return GetText(logic, System.Globalization.CultureInfo.CurrentCulture);
            }

            /// <summary>
            /// ������߼�������Ŷ�Ӧ��һ���Ѻ��ı���
            /// </summary>
            /// <param name="logic">�߼��������</param>
            /// <param name="culture">ָ����������</param>
            /// <returns>��ָ�����߼�������Ŷ�Ӧ���Ѻ��ı���</returns>
            public static string GetText(string logic, System.Globalization.CultureInfo culture)
            {
                string text = string.Empty;
                switch (logic)
                {
                    case Logic.And:
                        text = "��";
                        break;
                    case Logic.CSharpAnd:
                        text = "��";
                        break;
                    case Logic.Or:
                        text = "��";
                        break;
                    case Logic.CSharpOr:
                        text = "��";
                        break;
                    default:
                        break;
                }
                return text;
            }
        }

        /// <summary>
        /// �ֶ����͡�
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
            /// �ı����͡�
            /// </summary>
            public const string Text = "S";
            /// <summary>
            /// �������͡�
            /// </summary>
            public const string Numeric = "N";
            /// <summary>
            /// ����ʱ�����͡�
            /// </summary>
            public const string DateTime = "D";
            /// <summary>
            /// ������ת�����ֶ����ͷ��Ŵ���
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
            /// ��֤�ֶ�ֵ�Ƿ�����ֶ����͸�ʽ������������򷵻�һ����Ч��Ĭ��ֵ��
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
        /// ���ҳ��صĳ�����
        /// </summary>
        public class Paging
        {
            /// <summary>
            /// ��ҳ��С��������
            /// </summary>
            public const string PageSizePopertyName = "PageSize";
            /// <summary>
            /// ��ǰҳ������������
            /// </summary>
            public const string PageIndexPropertyName = "PageIndex";
            /// <summary>
            /// ��ҳ����������
            /// </summary>
            public const string PageCountPropertyName = "PageCount";
            /// <summary>
            /// ��ҳ������������
            /// </summary>
            public const string PagerSettingsPropertyName = "PagerSettings";
        }

        /// <summary>
        /// ��ؼ�������صĳ�����
        /// </summary>
        public class ControlProperties
        {
            /// <summary>
            /// Checked ����������
            /// </summary>
            public const string CheckedPropertyName = "Checked";
            /// <summary>
            /// Selected ����������
            /// </summary>
            public const string SelectedPropertyName = "Selected";
            /// <summary>
            /// Text ����������
            /// </summary>
            public const string TextPropertyName = "Text";
            /// <summary>
            /// AutoPostBack ����������
            /// </summary>
            public const string AutoPostBackPropertyName = "AutoPostBack";
        }

        /// <summary>
        /// �ַ���������
        /// </summary>
        public class StringInfo
        {
            /// <summary>
            /// �ո�����
            /// </summary>
            public const string Space = " ";
        }

        /// <summary>
        /// ���ֳ�����
        /// </summary>
        public class Literals
        {
            /// <summary>
            /// �ڵ�������ʱ���������ʾ��
            /// </summary>
            public const string ExportTips = "�����������û�о��������������������ܴ󣬵������ݵĹ��̽�������ϳ�ʱ�䣬���Ƿ����ִ�е���������";
        }
    }
}
