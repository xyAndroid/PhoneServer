using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// �ṩ����WebControls��ʵ�÷������ࡣ
    /// </summary>
    public class Utility
    {
        private Utility()
        {
        }

        /// <summary>
        /// ��õ�ǰ��¼��ɫ����
        /// </summary>
        /// <returns>������ڵ�ǰ��¼�Ľ�ɫ���򷵻ؽ�ɫ�������򷵻�null��</returns>
        public static string GetCurrentLoginRole()
        {
            return System.Web.HttpContext.Current.Session["loginRole"] as string;
        }

        /// <summary>
        /// ��õ�ǰ��¼���û��������û���������ǰ׺��
        /// </summary>
        /// <returns>������ǰ׺���û�����</returns>
        public static string GetCurrentLoginUser()
        {
            string userName = string.Empty;

            if (System.Web.HttpContext.Current.User != null &&
                System.Web.HttpContext.Current.User.Identity != null &&
                System.Web.HttpContext.Current.User.Identity.Name != null)
            {
                string[] userFullNames =
                    System.Web.HttpContext.Current.User.Identity.Name.Split(
                    new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                if (userFullNames.Length > 1)
                {
                    userName = userFullNames[1];
                }
                else if (userFullNames.Length == 1)
                {
                    userName = userFullNames[0];
                }
            }

            return userName;
        }

        /// <summary>
        /// ��������ַ����ԡ�$���ָ�ɰ���Unicode����Ĵ���
        /// </summary>
        /// <param name="input">������ַ�����</param>
        /// <returns>�ԡ�$���ָ�ɰ���Unicode����Ĵ���</returns>
        public static string ConvertToUnicodeStr(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            StringBuilder sBuilder = new StringBuilder();

            char[] inputCharArr = input.ToCharArray();

            foreach (char c in inputCharArr)
            {
                sBuilder.AppendFormat("${0}", Convert.ToByte(c));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// ���ԡ�$���ָ�ɰ���Unicode����Ĵ�ת��Ϊԭʼ�ַ�����
        /// </summary>
        /// <param name="input">�ԡ�$���ָ�ɰ���Unicode����Ĵ���</param>
        /// <returns>ԭʼ�ַ�����</returns>
        public static string ConvertFromUnicodeStr(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            StringBuilder sBuilder = new StringBuilder();

            string[] inputStrArr = input.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                foreach (string aStr in inputStrArr)
                {
                    sBuilder.Append(Convert.ToChar(Convert.ToByte(aStr)));
                }
            }
            catch (Exception)
            {
                return input;
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// ��һ�����ϰ���ָ���ķָ�����л�Ϊһ���ַ�����
        /// </summary>
        /// <param name="collection">��Ҫ���л��ļ��ϡ�</param>
        /// <param name="seperator">�ָ����</param>
        /// <returns>���л�Ϊһ���ַ�����</returns>
        public static string ConvertCollectionToString(System.Collections.ICollection collection, string seperator)
        {
            string str = string.Empty;
            foreach (object obj in collection)
            {
                str = string.Format("{0},{1}", str, obj);
            }
            str = str.TrimStart(seperator.ToCharArray());

            return str;
        }

        /// <summary>
        /// ����ѡ��ģʽö��ת���� GridView ��ѡ��ģʽö�١�
        /// </summary>
        /// <param name="rowSelectMode">��ѡ��ö�١�</param>
        /// <returns>ת��֮��� GridView ��ѡ��ģʽö�١�</returns>
        public static GridViewRowSelectMode ConvertToGridViewRowSelectMode(RowSelectMode rowSelectMode)
        {
            return (GridViewRowSelectMode)
                Enum.Parse(typeof(GridViewRowSelectMode), rowSelectMode.ToString());
        }

        /// <summary>
        /// ��������Ϣ��ʾ�ڵ����Ի����ڡ�
        /// </summary>
        /// <param name="invokeObject">��������Ķ���</param>
        /// <param name="errorMessage">������Ϣ</param>
        public static void PopupErrorBox(object invokeObject, string errorMessage)
        {
            if (!(invokeObject is Control))
            {
                invokeObject = new Control();
            }
            ScriptManager.RegisterStartupScript((Control)invokeObject, typeof(Control), "Error",
                string.Format("alert('{0}')", errorMessage.Replace("\r", "\\r").Replace("\n", "\\n")), true);
        }

        /// <summary>
        /// ��һ���ؼ�������������Ѱ��ָ��ID��һ���ؼ���
        /// </summary>
        /// <param name="ctrl">��Ҫ�������������в���ָ��ID�ؼ��Ŀؼ���</param>
        /// <param name="ctrlId">��Ѱ�ҿؼ���ID��</param>
        /// <returns>�ҵ���ָ��ID�Ŀؼ������û���ҵ��򷵻�null��</returns>
        public static Control FindControlInNamingContainer(Control ctrl, string ctrlId)
        {
            if (ctrl.NamingContainer != null)
            {
                if (!string.IsNullOrEmpty(ctrl.NamingContainer.ID) &&
                    ctrl.NamingContainer.ID.Equals(ctrlId))
                {
                    return ctrl.NamingContainer;
                }
                else
                {
                    Control aCtrl = ctrl.NamingContainer.FindControl(ctrlId);
                    if (aCtrl != null)
                    {
                        return aCtrl;
                    }
                    return FindControlInNamingContainer(ctrl.NamingContainer, ctrlId);
                }
            }
            return null;
        }

        /// <summary>
        /// ����һ���ؼ�����������ҳ���Լ�����������Ѱ��ָ��ID�Ŀؼ���
        /// </summary>
        /// <param name="ctrl">������һ���ؼ�����</param>
        /// <param name="ctrlId">��ҪѰ�ҵĿؼ�ID</param>
        /// <returns>����ҵ����ظÿؼ�����ʵ�������򷵻�null��</returns>
        public static Control FindControl(Control ctrl, string ctrlId)
        {
            Control foundCtrl = ctrl.Page.FindControl(ctrlId);
            if (foundCtrl == null)
            {
                foundCtrl = FindControlInNamingContainer(ctrl, ctrlId);
            }
            return foundCtrl;
        }

        /// <summary>
        /// ��һ���ؼ���Ѱ��һ�����Ե�ֵ�����ڿؼ������Բ����ڵ������ָ��һ��Ĭ��ֵ��
        /// </summary>
        /// <typeparam name="T">�ؼ����Ե����͡�</typeparam>
        /// <param name="ctrl">�ؼ���</param>
        /// <param name="propertyName">�������ơ�</param>
        /// <param name="defaultValue">�ڿؼ������Բ����ڵ������ָ��һ��Ĭ��ֵ��</param>
        /// <returns>����ҵ��򷵻�����ֵ�����򷵻�ָ����Ĭ��ֵ��</returns>
        public static T GetControlPropertyValue<T>(Control ctrl, string propertyName, T defaultValue)
        {
            if (ctrl == null)
            {
                return defaultValue;
            }

            System.Reflection.PropertyInfo prop = ctrl.GetType().GetProperty(propertyName);

            if (prop == null)
            {
                return defaultValue;
            }

            return (T)prop.GetValue(ctrl, null);
        }

        /// <summary>
        /// Ϊһ���ؼ������Ը�ֵ������ؼ������Բ�������ִ���κβ�����
        /// </summary>
        /// <typeparam name="T">�ؼ����Ե����͡�</typeparam>
        /// <param name="ctrl">�ؼ���</param>
        /// <param name="propertyName">�������ơ�</param>
        /// <param name="value">��Ҫ��ֵ������ֵ��</param>
        public static void SetControlPropertyValue<T>(Control ctrl, string propertyName, T value)
        {
            if (ctrl == null)
            {
                return;
            }

            System.Reflection.PropertyInfo prop = ctrl.GetType().GetProperty(propertyName);

            if (prop == null)
            {
                return;
            }
            prop.SetValue(ctrl, value, null);
        }

        /// <summary>
        /// Ϊĳ���ؼ������Դ���ͼ״̬�л�ȡ����ֵ��
        /// </summary>
        /// <typeparam name="T">���Ե�����</typeparam>
        /// <param name="viewState">�ؼ�����ͼ״̬</param>
        /// <param name="propertyName">��������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>��õ�����ֵ�������ͼ״̬��û�б��������ֵ�򷵻�Ĭ��ֵ��</returns>
        public static T GetViewStatePropertyValue<T>(StateBag viewState, string propertyName, T defaultValue)
        {
            if(viewState[propertyName] != null)
            {
                return (T)viewState[propertyName];
            }
            return defaultValue;
        }

        /// <summary>
        /// Ϊĳ���ؼ������Խ�������ֵ���浽��ͼ״̬�С�
        /// </summary>
        /// <typeparam name="T">���Ե�����</typeparam>
        /// <param name="viewState">�ؼ�����ͼ״̬</param>
        /// <param name="propertyName">��������</param>
        /// <param name="propertyValue">����ֵ</param>
        public static void SetViewStatePropertyValue<T>(StateBag viewState, string propertyName, T propertyValue)
        {
            viewState[propertyName] = propertyValue;
        }

        /// <summary>
        /// �ṩ�޽���Ŀؼ����ʱ���֡�
        /// </summary>
        /// <param name="writer"></param>
        public static void RenderUnUIControlDesign(Control ctrl, HtmlTextWriter writer)
        {
            writer.Write(@"<table cellspacing=""0"" style=""height:25px"">");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.Write(@"<td style=""border-right: 1px ridge; border-top: 1px ridge; border-left: 1px ridge; border-bottom: 1px ridge; background-color: buttonface"">");
            writer.Write(@"<span style=""font-family: ����; font-size: 9pt""><strong>" + ctrl.GetType().Name + @"</strong> - " + ctrl.ID);
            writer.Write("</span></td>");
            writer.RenderEndTag();
            writer.Write("</table>");
        }

        /// <summary>
        /// ��õ�ǰҳ�����󴫵ݵĲ���ֵ��
        /// </summary>
        /// <param name="thePage">��ǰҳ��</param>
        /// <param name="paramName">������</param>
        /// <returns>����ֵ�����������ָ���Ĳ��������򷵻�null��</returns>
        public static string GetRequestParameterValue(Page thePage, string paramName)
        {
            return thePage.Request.Params[paramName];
        }

        /// <summary>
        /// ��һ���������ҳ��������
        /// </summary>
        /// <typeparam name="T">���Ͳ���</typeparam>
        /// <param name="array">�����ҵ�����</param>
        /// <param name="defaultMax">Ĭ�ϵ������</param>
        /// <returns>�����</returns>
        public static T GetMaxValue<T>(T[] array, T defaultMax)
        {
            if (array.Length == 0)
            {
                return defaultMax;
            }
            T[] destArr = new T[array.Length];
            Array.Copy(array, destArr, array.Length);
            Array.Sort<T>(destArr);
            return destArr[destArr.Length - 1];
        }

        /// <summary>
        /// ��һ���������ҳ���С����
        /// </summary>
        /// <typeparam name="T">���Ͳ���</typeparam>
        /// <param name="array">�����ҵ�����</param>
        /// <param name="defaultMin">Ĭ�ϵ���С��</param>
        /// <returns>��С��</returns>
        public static T GetMinValue<T>(T[] array, T defaultMin)
        {
            if (array.Length == 0)
            {
                return defaultMin;
            }
            T[] destArr = new T[array.Length];
            Array.Copy(array, destArr, array.Length);
            Array.Sort<T>(destArr);
            return destArr[0];
        }

        /// <summary>
        /// ��GridView���ݵ�����Excel����С�
        /// </summary>
        /// <param name="gvCtrl">Ҫ�������ݵ�GridView�ؼ���</param>
        /// <param name="needToHideColumnIndexes">��Ҫ���ص�GridView������������</param>
        /// <param name="exportTitle">��������</param>
        /// <param name="fileName">�����ļ���</param>
        static public void ExportToExcel(System.Web.UI.WebControls.GridView gvCtrl,
            int[] needToHideColumnIndexes, string exportTitle, string fileName)
        {
            if (string.IsNullOrEmpty(exportTitle))
            {
                exportTitle = Properties.Resources.ExportTitle;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Properties.Resources.ExportFile;
            }

            //  ��¼GridViewԭʼ��Ϣ��
            bool isSorting = gvCtrl.AllowSorting;
            int pageIndex = gvCtrl.PageIndex;
            int pageSize = gvCtrl.PageSize;

            //  ����GridView״̬��Ϣ��
            Dictionary<int, bool> visibleStates = new Dictionary<int, bool>();
            for (int i = 0; i < gvCtrl.Columns.Count; i++)
            {
                DataControlField aCol = gvCtrl.Columns[i];
                visibleStates[i] = aCol.Visible;

                bool bContains = false;
                foreach (int x in needToHideColumnIndexes)
                {
                    if (x == i)
                    {
                        bContains = true;
                        break;
                    }
                }

                aCol.Visible = (needToHideColumnIndexes == null || !bContains);
            }

            //gvCtrl.AllowPaging = false;
            gvCtrl.AllowSorting = false;

            gvCtrl.PageSize = (gvCtrl.PageSize * gvCtrl.PageCount <= 0 ? 1 : gvCtrl.PageSize * gvCtrl.PageCount);
            gvCtrl.PageIndex = 0;

            //  ��GridView��
            gvCtrl.DataBind();

            //  �����������
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            context.Response.Clear();

            context.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            context.Response.ContentType = "application/vnd.ms-excel";
            
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.Charset = "GB2312";

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

            gvCtrl.RenderControl(htw);

            context.Response.Write(exportTitle);
            context.Response.Write(sw.ToString());
            context.Response.End();

            //  ��ԭGridView״̬��Ϣ��
            for (int i = 0; i < gvCtrl.Columns.Count; i++)
            {
                DataControlField aCol = gvCtrl.Columns[i];
                aCol.Visible = visibleStates[i];
            }

            gvCtrl.AllowSorting = isSorting;

            gvCtrl.PageSize = pageSize;
            gvCtrl.PageIndex = pageIndex;
        }

        /// <summary>
        /// ��ô������б��Url��
        /// </summary>
        /// <param name="url">ԭ����ҳ��ַ��</param>
        /// <param name="requestParams">��������б�</param>
        /// <returns>��ô������б��Url</returns>
        public static string GetUrlString(string url, params System.Collections.Generic.KeyValuePair<string, object>[] requestParams)
        {
            StringBuilder sBuilder = new StringBuilder();

            List<KeyValuePair<string, object>> finalParams = new List<KeyValuePair<string, object>>(requestParams);

            string[] urls = url.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (urls.Length == 1)
            {
                sBuilder.Append(urls[0]);
            }
            else if (urls.Length == 2)
            {
                sBuilder.Append(urls[0]);

                string[] urlParams = urls[1].Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string aUrlParam in urlParams)
                {
                    string[] urlParamPair = aUrlParam.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if (urlParamPair.Length == 2)
                    {
                        KeyValuePair<string, object> foundParam = finalParams.FirstOrDefault(p => p.Key.Equals(urlParamPair[0]));

                        if (string.IsNullOrEmpty(foundParam.Key))
                        {
                            finalParams.Add(new KeyValuePair<string, object>(urlParamPair[0], urlParamPair[1]));
                        }
                    }
                }
            }

            if (finalParams.Count > 0) sBuilder.Append('?');

            foreach (System.Collections.Generic.KeyValuePair<string, object> KeyValuePair in finalParams)
            {
                if (!string.IsNullOrEmpty(KeyValuePair.Key))
                {
                    sBuilder.Append(KeyValuePair.Key);
                    sBuilder.Append('=');
                    sBuilder.Append(KeyValuePair.Value.ToString());
                    sBuilder.Append('&');
                }
            }
            string finalUrl = sBuilder.ToString();

            finalUrl = finalUrl.Trim();
            finalUrl = finalUrl.TrimEnd('?');
            finalUrl = finalUrl.TrimEnd('&');

            return finalUrl;
        }

        /// <summary>
        /// ��ÿؼ������ơ��ؼ����������ڿؼ�ID�����ǲ���������ǰ׺��
        /// </summary>
        /// <param name="ctrl">�ؼ�</param>
        /// <returns></returns>
        public static string GetControlName(Control ctrl)
        {
            if (ctrl == null || string.IsNullOrEmpty(ctrl.ID)) return string.Empty;

            string prefix = string.Empty;
            if (ctrl is Panel)
            {
                prefix = "pnl";
            }
            else if (ctrl is Image)
            {
                prefix = "img";
            }
            else if (ctrl is LinkButton)
            {
                prefix = "lnkBtn";
            }
            else if (ctrl is UserControl)
            {
                prefix = "wuc";
            }
            else if (ctrl is GridView)
            {
                prefix = "gv";
            }
            else if (ctrl is GridViewExtender)
            {
                prefix = "gvExt";
            }
            else if (ctrl is DetailsView)
            {
                prefix = "dv";
            }
            else if (ctrl is FormView)
            {
                prefix = "fv";
            }
            else
            {
                prefix = "_unknow";
            }
            return ctrl.ID.Replace(prefix, string.Empty);
        }
    }
}
