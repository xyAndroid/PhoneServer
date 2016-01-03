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
    /// 提供基于WebControls的实用方法的类。
    /// </summary>
    public class Utility
    {
        private Utility()
        {
        }

        /// <summary>
        /// 获得当前登录角色名。
        /// </summary>
        /// <returns>如果存在当前登录的角色，则返回角色名，否则返回null。</returns>
        public static string GetCurrentLoginRole()
        {
            return System.Web.HttpContext.Current.Session["loginRole"] as string;
        }

        /// <summary>
        /// 获得当前登录的用户名，该用户名不带域前缀。
        /// </summary>
        /// <returns>不带域前缀的用户名。</returns>
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
        /// 将输入的字符串以“$”分割成按照Unicode编码的串。
        /// </summary>
        /// <param name="input">输入的字符串。</param>
        /// <returns>以“$”分割成按照Unicode编码的串。</returns>
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
        /// 将以“$”分割成按照Unicode编码的串转换为原始字符串。
        /// </summary>
        /// <param name="input">以“$”分割成按照Unicode编码的串。</param>
        /// <returns>原始字符串。</returns>
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
        /// 将一个集合按照指定的分割符串行化为一个字符串。
        /// </summary>
        /// <param name="collection">需要串行化的集合。</param>
        /// <param name="seperator">分割符。</param>
        /// <returns>串行化为一个字符串。</returns>
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
        /// 将行选择模式枚举转换成 GridView 行选择模式枚举。
        /// </summary>
        /// <param name="rowSelectMode">行选择枚举。</param>
        /// <returns>转换之后的 GridView 行选择模式枚举。</returns>
        public static GridViewRowSelectMode ConvertToGridViewRowSelectMode(RowSelectMode rowSelectMode)
        {
            return (GridViewRowSelectMode)
                Enum.Parse(typeof(GridViewRowSelectMode), rowSelectMode.ToString());
        }

        /// <summary>
        /// 将错误消息显示在弹出对话框内。
        /// </summary>
        /// <param name="invokeObject">引发错误的对象</param>
        /// <param name="errorMessage">错误消息</param>
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
        /// 在一个控件的命名容器中寻找指定ID的一个控件。
        /// </summary>
        /// <param name="ctrl">需要在其命名容器中查找指定ID控件的控件。</param>
        /// <param name="ctrlId">待寻找控件的ID。</param>
        /// <returns>找到的指定ID的控件，如果没有找到则返回null。</returns>
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
        /// 给定一个控件，在期所在页面以及命名容器中寻找指定ID的控件。
        /// </summary>
        /// <param name="ctrl">给定的一个控件对象。</param>
        /// <param name="ctrlId">需要寻找的控件ID</param>
        /// <returns>如果找到返回该控件对象实例，否则返回null。</returns>
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
        /// 在一个控件中寻找一个属性的值，并在控件或属性不存在的情况下指定一个默认值。
        /// </summary>
        /// <typeparam name="T">控件属性的类型。</typeparam>
        /// <param name="ctrl">控件。</param>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="defaultValue">在控件或属性不存在的情况下指定一个默认值。</param>
        /// <returns>如果找到则返回属性值，否则返回指定的默认值。</returns>
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
        /// 为一个控件的属性赋值，如果控件或属性不存在则不执行任何操作。
        /// </summary>
        /// <typeparam name="T">控件属性的类型。</typeparam>
        /// <param name="ctrl">控件。</param>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="value">需要赋值的属性值。</param>
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
        /// 为某个控件的属性从视图状态中获取属性值。
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="viewState">控件的视图状态</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>获得的属性值，如果视图状态中没有保存该属性值则返回默认值。</returns>
        public static T GetViewStatePropertyValue<T>(StateBag viewState, string propertyName, T defaultValue)
        {
            if(viewState[propertyName] != null)
            {
                return (T)viewState[propertyName];
            }
            return defaultValue;
        }

        /// <summary>
        /// 为某个控件的属性将其属性值保存到视图状态中。
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="viewState">控件的视图状态</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        public static void SetViewStatePropertyValue<T>(StateBag viewState, string propertyName, T propertyValue)
        {
            viewState[propertyName] = propertyValue;
        }

        /// <summary>
        /// 提供无界面的控件设计时呈现。
        /// </summary>
        /// <param name="writer"></param>
        public static void RenderUnUIControlDesign(Control ctrl, HtmlTextWriter writer)
        {
            writer.Write(@"<table cellspacing=""0"" style=""height:25px"">");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.Write(@"<td style=""border-right: 1px ridge; border-top: 1px ridge; border-left: 1px ridge; border-bottom: 1px ridge; background-color: buttonface"">");
            writer.Write(@"<span style=""font-family: 宋体; font-size: 9pt""><strong>" + ctrl.GetType().Name + @"</strong> - " + ctrl.ID);
            writer.Write("</span></td>");
            writer.RenderEndTag();
            writer.Write("</table>");
        }

        /// <summary>
        /// 获得当前页面请求传递的参数值。
        /// </summary>
        /// <param name="thePage">当前页面</param>
        /// <param name="paramName">参数名</param>
        /// <returns>参数值，如果不存在指定的参数名，则返回null。</returns>
        public static string GetRequestParameterValue(Page thePage, string paramName)
        {
            return thePage.Request.Params[paramName];
        }

        /// <summary>
        /// 从一组数据中找出最大数。
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="array">待查找的数组</param>
        /// <param name="defaultMax">默认的最大数</param>
        /// <returns>最大数</returns>
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
        /// 从一组数据中找出最小数。
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="array">待查找的数组</param>
        /// <param name="defaultMin">默认的最小数</param>
        /// <returns>最小数</returns>
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
        /// 将GridView数据导出到Excel表格中。
        /// </summary>
        /// <param name="gvCtrl">要导出数据的GridView控件。</param>
        /// <param name="needToHideColumnIndexes">需要隐藏的GridView的列索引数组</param>
        /// <param name="exportTitle">导出标题</param>
        /// <param name="fileName">导出文件名</param>
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

            //  记录GridView原始信息。
            bool isSorting = gvCtrl.AllowSorting;
            int pageIndex = gvCtrl.PageIndex;
            int pageSize = gvCtrl.PageSize;

            //  设置GridView状态信息。
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

            //  绑定GridView。
            gvCtrl.DataBind();

            //  处理输出流。
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

            //  还原GridView状态信息。
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
        /// 获得带参数列表的Url。
        /// </summary>
        /// <param name="url">原请求页地址。</param>
        /// <param name="requestParams">请求参数列表。</param>
        /// <returns>获得带参数列表的Url</returns>
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
        /// 获得控件的名称。控件名称来自于控件ID，但是不包括命名前缀。
        /// </summary>
        /// <param name="ctrl">控件</param>
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
