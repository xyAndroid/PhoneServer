using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 用于构造邮件地址的控件。
    /// </summary>
    [DefaultProperty("Text")]
    [ValidationProperty("EMail")]
    [ParseChildren(true, "Text")]
    [System.Drawing.ToolboxBitmap(typeof(MailLink), "MailLink.bmp")]
    [ToolboxData("<{0}:MailLink runat=\"server\"></{0}:MailLink>")]
    public class MailLink : WebControl
    {
        private const string _defaultText = "邮件地址";

        /// <summary>
        /// 邮件地址的显示文本。
        /// </summary>
        [Bindable(true)]
        [Category("外观")]
        [DefaultValue(_defaultText)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Localizable(true)]
        [Description("显示为文本。")]
        public virtual string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? _defaultText : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// 邮件地址，以“mailto:”开始。
        /// </summary>
        [Bindable(true)]
        [Category("数据")]
        [DefaultValue("exapmle@server.com")]
        [Localizable(true)]
        [Description("E-Mail链接地址。")]
        public virtual string EMail
        {
            get
            {
                String s = (String)ViewState["EMail"];
                return ((s == null) ? "exapmle@server.com" : s);
            }

            set
            {
                ViewState["EMail"] = value;
            }
        }

        /// <summary>
        /// 重写属性。
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.A;
            }
        }

        /// <summary>
        /// 重写方法。
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href,
                "mailto:" + EMail);
        }

        /// <summary>
        /// 重写方法。
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (String.IsNullOrEmpty(Text))
            {
                Text = EMail;
            }
            output.Write(Text);
        }
    }
}
