using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// �ȴ��ؼ���
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Waiting runat=server></{0}:Waiting>")]
    public class Waiting : CompositeControl
    {
        #region Controls

        Image _imgWaiting;

        #endregion
        #region Properties

        /// <summary>
        /// ���õȴ��ı���
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [Category("���")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("���õȴ��ı���")]
        public string Text
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "Text", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "Text", value);
            }
        }

        /// <summary>
        /// ���õȴ�ͼƬ��
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [Category("���")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [UrlProperty("AllowedTypes=UrlTypes.Absolute|UrlTypes.RootRelative|UrlTypes.AppRelative")]
        [Description("���õȴ�ͼƬ��")]
        public string ImageUrl
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ImageUrl", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ImageUrl", value);
            }
        }
        #endregion

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            _imgWaiting = new Image();
            _imgWaiting.ID = "imgWaiting";
            _imgWaiting.ImageUrl = ImageUrl;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _imgWaiting.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.Write(Text);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}
