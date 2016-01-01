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
    /// 表示空白的控件，如用在 <typeparamref name="GridView"/> 的 EmptyTemplate 中。
    /// </summary>
    [DefaultProperty("Text")]
    [ParseChildren(true, "Text")]
    [System.Drawing.ToolboxBitmap(typeof(Empty), "Empty.bmp")]
    [ToolboxData("<{0}:Empty runat=server></{0}:Empty>")]
    public class Empty : CompositeControl
    {
        #region Controls

        Image _imgEmpty;
        Label _lblEmpty;

        #endregion
        #region Properties

        private const string m_TextViewStateName = "Text";
        /// <summary>
        /// 空白显示的文本。
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [Category("外观")]
        [DefaultValue("")]
        [Description("空白显示的文本。")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_TextViewStateName, "");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_TextViewStateName, value);
            }
        }

        private const string m_ImageUrlViewStateName = "ImageUrl";
        [Browsable(true)]
        [Bindable(true)]
        [Category("外观")]
        [DefaultValue("")]
        [Description("空白显示的图片链接。")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [UrlProperty("AllowedTypes=UrlTypes.Absolute|UrlTypes.RootRelative|UrlTypes.AppRelative")]
        public string ImageUrl
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_ImageUrlViewStateName, "");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_ImageUrlViewStateName, value);
            }
        }

        #endregion

        #region Operates

        private void ConstructEmptyImageControl()
        {
            _imgEmpty = new Image();
            _imgEmpty.ID = "imgEmpty";
            _imgEmpty.ImageUrl = this.ImageUrl;

            this.Controls.Add(_imgEmpty);
        }

        private void ConstructEmptyTextControl()
        {
            _lblEmpty = new Label();
            _lblEmpty.ID = "lblEmpty";
            _lblEmpty.Text = this.Text;

            this.Controls.Add(_lblEmpty);
        }

        #endregion

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.ConstructEmptyImageControl();
            this.ConstructEmptyTextControl();

            this.ClearChildViewState();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _imgEmpty.RenderControl(writer);

            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _lblEmpty.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}
