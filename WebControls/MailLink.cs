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
    /// ���ڹ����ʼ���ַ�Ŀؼ���
    /// </summary>
    [DefaultProperty("Text")]
    [ValidationProperty("EMail")]
    [ParseChildren(true, "Text")]
    [System.Drawing.ToolboxBitmap(typeof(MailLink), "MailLink.bmp")]
    [ToolboxData("<{0}:MailLink runat=\"server\"></{0}:MailLink>")]
    public class MailLink : WebControl
    {
        private const string _defaultText = "�ʼ���ַ";

        /// <summary>
        /// �ʼ���ַ����ʾ�ı���
        /// </summary>
        [Bindable(true)]
        [Category("���")]
        [DefaultValue(_defaultText)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Localizable(true)]
        [Description("��ʾΪ�ı���")]
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
        /// �ʼ���ַ���ԡ�mailto:����ʼ��
        /// </summary>
        [Bindable(true)]
        [Category("����")]
        [DefaultValue("exapmle@server.com")]
        [Localizable(true)]
        [Description("E-Mail���ӵ�ַ��")]
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
        /// ��д���ԡ�
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.A;
            }
        }

        /// <summary>
        /// ��д������
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href,
                "mailto:" + EMail);
        }

        /// <summary>
        /// ��д������
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
