using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 分页控件呈现模式。
    /// </summary>
    public enum PagingRenderMode
    {
        /// <summary>
        /// 按照LinkButton方式呈现。
        /// </summary>
        LinkButton = 1,
        /// <summary>
        /// 按照ImageButton方式呈现。
        /// </summary>
        ImageButton = 2,
    }

    /// <summary>
    /// 分页大小改变之后发生的事件参数。
    /// </summary>
    public class PageSizeChangedEventArgs : EventArgs
    {
        private int m_SourcePageSize;
        /// <summary>
        /// 原分页大小。
        /// </summary>
        public int SourcePageSize
        {
            get { return m_SourcePageSize; }
            internal set { m_SourcePageSize = value; }
        }

        private int m_CurrentPageSize;
        /// <summary>
        /// 当前分页大小。
        /// </summary>
        public int CurrentPageSize
        {
            get { return m_CurrentPageSize; }
            internal set { m_CurrentPageSize = value; }
        }
    }

    /// <summary>
    /// 分页大小改变之后发生的事件委托。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PageSizeChangedEventHandler(object sender, PageSizeChangedEventArgs e);

    /// <summary>
    /// 对包含分页功能的控件进行分页处理的控件。
    /// </summary>
    /// <remarks>
    /// 分页控件可以放在 GridView 的 PagerTemplate 中也可以放在 GridView 之外。
    /// 分页控件支持两种分页方式：
    /// （1）由数据源控件支持的分页，如：SqlDataSource、LinqDataSource 自带的分页功能；
    /// （2）由数据源支持的分页，如：构造了分页存储过程，并绑定在 SqlDataSource 上。
    /// 注意：如果由数据源支持的分页，需要提供给数据源 PageIndex 和 PageSize 两个传入参数，并且要
    /// 获得一个 PageCount 传出参数。对于分页控件来说需要通过 PageIndexParameterName 和
    /// PageCountParameterName 两个属性来配置 PageIndex 和 PageCount 两个参数名，但是 PageSize 
    /// 则不用配置，这个可以由 GridView 这样的数据绑定控件获得。
    /// 注意：如果由数据源支持的分页，需要将分页控件放置在像 GridView 这样的数据绑定控件的 PagerTemplate 之外。
    /// </remarks>
    [Designer(typeof(System.Web.UI.Design.WebControls.CompositeControlDesigner))]
    [DefaultProperty("ControlID")]
    [DefaultEvent("PageSizeChanged")]
    [System.Drawing.ToolboxBitmap(typeof(Paging), "Paging.bmp")]
    [ToolboxData("<{0}:Paging runat=server></{0}:Paging>")]
    public class Paging : CompositeControl
    {
        #region Constants

        private const string _defaultFirstPageText = "【首页】";
        private const string _defaultPreviousPageText = "【上一页】";
        private const string _defaultNextPageText = "【下一页】";
        private const string _defaultLastPageText = "【末页】";

        private const string _defaultGoToPageText = "【GO】";

        private const string _defaultGoToPageBeforeText = "转第";
        private const string _defaultGoToPageAfterText = "页";

        private const string _defaultPageSizeBeforeText = "每页";
        private const string _defaultPageSizeAfterText = "条";

        #endregion

        #region Fields

        private Style _navigateControlStyle;
        private Style _goToPageControlStyle;
        private Style _pageSizeControlStyle;

        #endregion

        #region Controls

        LinkButton _lnkBtnFirst;
        LinkButton _lnkBtnPrev;
        LinkButton _lnkBtnNext;
        LinkButton _lnkBtnLast;

        ImageButton _imgBtnFirst;
        ImageButton _imgBtnPrev;
        ImageButton _imgBtnNext;
        ImageButton _imgBtnLast;

        Label _lblGoToPageBefore;
        TextBox _txtGoToPage;
        Label _lblGoToPageAfter;
        LinkButton _lnkBtnGoTo;
        ImageButton _imgBtnGoTo;

        Label _lblPageSizeBefore;
        DropDownList _ddlstPageSize;
        Label _lblPageSizeAfter;

        WebControl _ctrl;
        DataSourceControl _dsCtrl;

        #endregion

        #region Properties

        private string _controlId = string.Empty;
        /// <summary>
        /// 控制分页的控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("控制分页的控件ID。")]
        public string ControlID
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private const string m_DataSourcePagingViewStateName = "DataSourcePaging";
        /// <summary>
        /// 指示数据源是否支持分页查询。
        /// </summary>
        [Browsable(false)]
        public bool DataSourcePaging
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_DataSourcePagingViewStateName, false);
            }
            private set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_DataSourcePagingViewStateName, value);
            }
        }

        private const string m_PageIndexViewStateName = "PageIndex";
        /// <summary>
        /// 如果数据源支持分页查询，获得与设置当前页索引。
        /// </summary>
        [Browsable(false)]
        public int PageIndex
        {
            get
            {
                return Utility.GetViewStatePropertyValue<int>(this.ViewState,
                    m_PageIndexViewStateName, 0);
            }
            set
            {
                Utility.SetViewStatePropertyValue<int>(this.ViewState,
                    m_PageIndexViewStateName, value);
            }
        }

        private const string m_PageCountViewStateName = "PageCount";
        /// <summary>
        /// 如果数据源支持分页查询，获得分页页数。
        /// </summary>
        [Browsable(false)]
        public int PageCount
        {
            get
            {
                return Utility.GetViewStatePropertyValue<int>(this.ViewState,
                    m_PageCountViewStateName, 0);
            }
            private set
            {
                Utility.SetViewStatePropertyValue<int>(this.ViewState,
                    m_PageCountViewStateName, value);
            }
        }

        private const string m_PageIndexParameterNameViewStateName = "PageIndexParameterName";
        /// <summary>
        /// 如果数据源支持分页查询，传递给数据源的页索引参数名。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("@PageIndex")]
        [Category("数据")]
        [Description("如果数据源支持分页查询，传递给数据源的页索引参数名。")]
        public string PageIndexParameterName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_PageIndexParameterNameViewStateName, "@PageIndex");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_PageIndexParameterNameViewStateName, value);
            }
        }

        private const string m_PageCountParameterNameViewStateName = "PageCountParameterName";
        /// <summary>
        /// 如果数据源支持分页查询，传递得到页数的参数名。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("@PageCount")]
        [Category("数据")]
        [Description("如果数据源支持分页查询，传递得到页数的参数名。")]
        public string PageCountParameterName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_PageCountParameterNameViewStateName, "@PageCount");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_PageCountParameterNameViewStateName, value);
            }
        }

        private const string m_IsVisibleWithNoDataViewStateName = "IsVisibleWithNoData";
        /// <summary>
        /// 当呈现数据的控件没有任何呈现的数据时，该分页控件是否可见。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("当呈现数据的控件没有任何呈现的数据时，该分页控件是否可见。")]
        public bool IsVisibleWithNoData
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsVisibleWithNoDataViewStateName, false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsVisibleWithNoDataViewStateName, value);
            }
        }

        private PagingRenderMode _pagingRenderMode = PagingRenderMode.LinkButton;
        /// <summary>
        /// 指明以“LinkButton”或“ImageButton”方式呈现导航内容。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(PagingRenderMode.LinkButton)]
        [Category("导航")]
        [Description("指明以“LinkButton”或“ImageButton”方式呈现导航内容。")]
        public PagingRenderMode PagingRenderMode
        {
            get { return _pagingRenderMode; }
            set { _pagingRenderMode = value; }
        }

        private const string m_PageSizesViewStateName = "PageSizes";
        /// <summary>
        /// 分页大小设置。
        /// </summary>
        [Browsable(false)]
        [Category("导航")]
        [Description("分页大小设置。")]
        public int[] PageSizes
        {
            get
            {
                if (this.ViewState[m_PageSizesViewStateName] != null)
                {
                    return (int[])this.ViewState[m_PageSizesViewStateName];
                }
                return new int[] { 5, 10, 20, 30, 50, 100 };
            }
            set
            {
                this.ViewState[m_PageSizesViewStateName] = value;
            }
        }

        private const string m_FirstPageTextViewStateName = "FirstPageText";
        /// <summary>
        /// 导航到第一页的文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultFirstPageText)]
        [Category("导航")]
        [Description("导航到第一页的文本。")]
        public virtual string FirstPageText
        {
            get
            {
                if (this.ViewState[m_FirstPageTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_FirstPageTextViewStateName];
                }
                return _defaultFirstPageText;
            }
            set
            {
                this.ViewState[m_FirstPageTextViewStateName] = value;
            }
        }

        private const string m_PreviousPageTextViewStateName = "PreviousPageText";
        /// <summary>
        /// 导航到上一页的文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultPreviousPageText)]
        [Category("导航")]
        [Description("导航到上一页的文本。")]
        public virtual string PreviousPageText
        {
            get
            {
                if (this.ViewState[m_PreviousPageTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_PreviousPageTextViewStateName];
                }
                return _defaultPreviousPageText;
            }
            set
            {
                this.ViewState[m_PreviousPageTextViewStateName] = value;
            }
        }

        private const string m_NextPageTextViewStateName = "NextPageText";
        /// <summary>
        /// 导航到下一页的文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultNextPageText)]
        [Category("导航")]
        [Description("导航到下一页的文本。")]
        public virtual string NextPageText
        {
            get
            {
                if (this.ViewState[m_NextPageTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_NextPageTextViewStateName];
                }
                return _defaultNextPageText;
            }
            set
            {
                this.ViewState[m_NextPageTextViewStateName] = value;
            }
        }

        private const string m_LastPageTextViewStateName = "LastPageText";
        /// <summary>
        /// 导航到最后一页的文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultLastPageText)]
        [Category("导航")]
        [Description("导航到最后一页的文本。")]
        public virtual string LastPageText
        {
            get
            {
                if (this.ViewState[m_LastPageTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_LastPageTextViewStateName];
                }
                return _defaultLastPageText;
            }
            set
            {
                this.ViewState[m_LastPageTextViewStateName] = value;
            }
        }

        private const string m_GoToPageTextViewStateName = "GoToPageText";
        /// <summary>
        /// 跳转导航的文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultGoToPageText)]
        [Category("导航")]
        [Description("跳转导航的文本。")]
        public virtual string GoToPageText
        {
            get
            {
                if (this.ViewState[m_GoToPageTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_GoToPageTextViewStateName];
                }
                return _defaultGoToPageText;
            }
            set
            {
                this.ViewState[m_GoToPageTextViewStateName] = value;
            }
        }

        private const string m_FirstPageImageUrlViewStateName = "FirstPageImageUrl";
        /// <summary>
        /// 导航到第一页的图片链接地址。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("导航")]
        [Description("导航到第一页的图片链接地址。")]
        public virtual string FirstPageImageUrl
        {
            get
            {
                if (this.ViewState[m_FirstPageImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_FirstPageImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_FirstPageImageUrlViewStateName] = value;
            }
        }

        private const string m_PreviousPageImageUrlViewStateName = "PreviousPageImageUrl";
        /// <summary>
        /// 导航到上一页的图片链接地址。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("导航")]
        [Description("导航到上一页的图片链接地址。")]
        public virtual string PreviousPageImageUrl
        {
            get
            {
                if (this.ViewState[m_PreviousPageImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_PreviousPageImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_PreviousPageImageUrlViewStateName] = value;
            }
        }

        private const string m_NextPageImageUrlViewStateName = "NextPageImageUrl";
        /// <summary>
        /// 导航到下一页的图片链接地址。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("导航")]
        [Description("导航到下一页的图片链接地址。")]
        public virtual string NextPageImageUrl
        {
            get
            {
                if (this.ViewState[m_NextPageImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_NextPageImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_NextPageImageUrlViewStateName] = value;
            }
        }

        private const string m_LastPageImageUrlViewStateName = "LastPageImageUrl";
        /// <summary>
        /// 导航到最后一页的图片链接地址。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("导航")]
        [Description("导航到最后一页的图片链接地址。")]
        public virtual string LastPageImageUrl
        {
            get
            {
                if (this.ViewState[m_LastPageImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_LastPageImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_LastPageImageUrlViewStateName] = value;
            }
        }

        private const string m_GoToPageImageUrlViewStateName = "GoToPageImageUrl";
        /// <summary>
        /// 跳转导航的图片链接地址。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("导航")]
        [Description("跳转导航的图片链接地址。")]
        public virtual string GoToPageImageUrl
        {
            get
            {
                if (this.ViewState[m_GoToPageImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_GoToPageImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_GoToPageImageUrlViewStateName] = value;
            }
        }

        private const string m_GoToPageBeforeTextViewStateName = "GoToPageBeforeText";
        /// <summary>
        /// 跳转导航前置文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultGoToPageBeforeText)]
        [Category("导航")]
        [Description("跳转导航前置文本。")]
        public virtual string GoToPageBeforeText
        {
            get
            {
                if (this.ViewState[m_GoToPageBeforeTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_GoToPageBeforeTextViewStateName];
                }
                return _defaultGoToPageBeforeText;
            }
            set
            {
                this.ViewState[m_GoToPageBeforeTextViewStateName] = value;
            }
        }

        private const string m_GoToPageAfterTextViewStateName = "GoToPageAfterText";
        /// <summary>
        /// 跳转导航后置文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultGoToPageAfterText)]
        [Category("导航")]
        [Description("跳转导航后置文本。")]
        public virtual string GoToPageAfterText
        {
            get
            {
                if (this.ViewState[m_GoToPageAfterTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_GoToPageAfterTextViewStateName];
                }
                return _defaultGoToPageAfterText;
            }
            set
            {
                this.ViewState[m_GoToPageAfterTextViewStateName] = value;
            }
        }

        private const string m_PageIndexPageCountSeperatorViewStateName = "PageIndexPageCountSeperator";
        /// <summary>
        /// 页索引与页数之间的分隔符。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("/")]
        [Category("导航")]
        [Description("页索引与页数之间的分隔符。")]
        public virtual string PageIndexPageCountSeperator
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_PageIndexPageCountSeperatorViewStateName, "/");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_PageIndexPageCountSeperatorViewStateName, value);
            }
        }

        private const string m_PageSizeBeforeTextViewStateName = "PageSizeBeforeText";
        /// <summary>
        /// 每页条数前置文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultPageSizeBeforeText)]
        [Category("导航")]
        [Description("每页条数前置文本。")]
        public virtual string PageSizeBeforeText
        {
            get
            {
                if (this.ViewState[m_PageSizeBeforeTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_PageSizeBeforeTextViewStateName];
                }
                return _defaultPageSizeBeforeText;
            }
            set
            {
                this.ViewState[m_PageSizeBeforeTextViewStateName] = value;
            }
        }

        private const string m_PageSizeAfterTextViewStateName = "PageSizeAfterText";
        /// <summary>
        /// 每页条数后置文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(_defaultPageSizeAfterText)]
        [Category("导航")]
        [Description("每页条数后置文本。")]
        public virtual string PageSizeAfterText
        {
            get
            {
                if (this.ViewState[m_PageSizeAfterTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_PageSizeAfterTextViewStateName];
                }
                return _defaultPageSizeAfterText;
            }
            set
            {
                this.ViewState[m_PageSizeAfterTextViewStateName] = value;
            }
        }

        /// <summary>
        /// 导航控件的样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("导航控件的样式。")]
        public virtual Style NavigateControlStyle
        {
            get
            {
                if (_navigateControlStyle == null)
                {
                    _navigateControlStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_navigateControlStyle).TrackViewState();
                    }
                }
                return _navigateControlStyle;
            }
        }

        /// <summary>
        /// 跳转控件的样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("跳转控件的样式。")]
        public virtual Style GoToPageControlStyle
        {
            get
            {
                if (_goToPageControlStyle == null)
                {
                    _goToPageControlStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_goToPageControlStyle).TrackViewState();
                    }
                }
                return _goToPageControlStyle;
            }
        }

        /// <summary>
        /// 页大小控件的样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("页大小控件的样式。")]
        public virtual Style PageSizeControlStyle
        {
            get
            {
                if (_pageSizeControlStyle == null)
                {
                    _pageSizeControlStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_pageSizeControlStyle).TrackViewState();
                    }
                }
                return _pageSizeControlStyle;
            }
        }

        private const string m_LeftPaddingWidthViewStateName = "LeftCellingWidth";
        /// <summary>
        /// 左侧空白宽度。
        /// </summary>
        [Browsable(true)]
        [Category("布局")]
        [DefaultValue("10px")]
        [Description("左侧空白宽度。")]
        public virtual Unit LeftPaddingWidth
        {
            get
            {
                if (this.ViewState[m_LeftPaddingWidthViewStateName] != null)
                {
                    return (Unit)this.ViewState[m_LeftPaddingWidthViewStateName];
                }
                return new Unit("10px");
            }
            set
            {
                this.ViewState[m_LeftPaddingWidthViewStateName] = value;
            }
        }

        private const string m_RightPaddingWidthViewStateName = "RightCellingWidth";
        /// <summary>
        /// 右侧空白宽度。
        /// </summary>
        [Browsable(true)]
        [Category("布局")]
        [DefaultValue("10px")]
        [Description("右侧空白宽度。")]
        public virtual Unit RightPaddingWidth
        {
            get
            {
                if (this.ViewState[m_RightPaddingWidthViewStateName] != null)
                {
                    return (Unit)this.ViewState[m_RightPaddingWidthViewStateName];
                }
                return new Unit("10px");
            }
            set
            {
                this.ViewState[m_RightPaddingWidthViewStateName] = value;
            }
        }

        #endregion

        #region Operates

        private void InitControl()
        {
            if (!this.DesignMode)
            {
                InitPagingControl();
            }
        }

        private void InitPagingControl()
        {
            _ctrl = (WebControl)Utility.FindControl(this, this.ControlID);

            if (_ctrl is DataBoundControl)
            {
                DataBoundControl dbc = _ctrl as DataBoundControl;

                _dsCtrl = (DataSourceControl)Utility.FindControl(this, dbc.DataSourceID);

                if (_dsCtrl is SqlDataSource)
                {
                    SqlDataSource sqlDs = _dsCtrl as SqlDataSource;

                    foreach (Parameter p in sqlDs.SelectParameters)
                    {
                        if (p.Name.Equals(this.PageCountParameterName.TrimStart('@')))
                        {
                            this.DataSourcePaging = true;
                            break;
                        }
                    }

                    sqlDs.Selecting += new SqlDataSourceSelectingEventHandler(SqlDataSource_Selecting);
                    sqlDs.Selected += new SqlDataSourceStatusEventHandler(SqlDataSource_Selected);
                }
            }
        }

        void SqlDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (e.Command.Parameters.Contains(this.PageIndexParameterName))
            {
                e.Command.Parameters[this.PageIndexParameterName].Value = this.PageIndex;
            }
        }

        void SqlDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Command.Parameters.Contains(this.PageCountParameterName) 
                && !string.IsNullOrEmpty(e.Command.Parameters[this.PageCountParameterName].Value.ToString()))
            {
                this.PageCount = (int)e.Command.Parameters[this.PageCountParameterName].Value;
            }
        }

        private PagerSettings GetPagerSettings(WebControl ctrl)
        {
            return Utility.GetControlPropertyValue<PagerSettings>(ctrl, Constants.Paging.PagerSettingsPropertyName, null);
        }

        private int GetPageIndex(WebControl ctrl)
        {
            if (this.DataSourcePaging)
            {
                return this.PageIndex;
            }
            return Utility.GetControlPropertyValue<int>(ctrl, Constants.Paging.PageIndexPropertyName, 0);
        }

        private int GetPageCount(WebControl ctrl)
        {
            if (this.DataSourcePaging)
            {
                return this.PageCount;
            }
            return Utility.GetControlPropertyValue<int>(ctrl, Constants.Paging.PageCountPropertyName, 1);
        }

        private int GetPageSize(WebControl ctrl)
        {
            return Utility.GetControlPropertyValue<int>(ctrl, Constants.Paging.PageSizePopertyName, 1);
        }

        private void SetPageIndex(WebControl ctrl, int pageIndex)
        {
            if (this.DataSourcePaging)
            {
                this.PageIndex = pageIndex;
            }
            Utility.SetControlPropertyValue<int>(ctrl, Constants.Paging.PageIndexPropertyName, pageIndex);

            this.OnPageIndexChanged(new EventArgs());
        }

        private void SetPageSize(WebControl ctrl, int pageSize)
        {
            Utility.SetControlPropertyValue<int>(ctrl, Constants.Paging.PageSizePopertyName, pageSize);
        }

        private void DataBindSource()
        {
            if (this.DataSourcePaging)
            {
                if (_ctrl is DataBoundControl)
                {
                    ((DataBoundControl)_ctrl).DataBind();
                }
            }
        }

        private void SetControlState()
        {
            if (this.DesignMode)
            {
                return;
            }

            if (_pagingRenderMode == PagingRenderMode.LinkButton)
            {
                _lnkBtnFirst.Enabled = true;
                _lnkBtnPrev.Enabled = true;
                _lnkBtnNext.Enabled = true;
                _lnkBtnLast.Enabled = true;
            }
            else if (_pagingRenderMode == PagingRenderMode.ImageButton)
            {
                _imgBtnFirst.Enabled = true;
                _imgBtnPrev.Enabled = true;
                _imgBtnNext.Enabled = true;
                _imgBtnLast.Enabled = true;
            }

            if (_ctrl != null)
            {
                if (this.GetPageCount(_ctrl) > 0)
                {
                    if (this.GetPageIndex(_ctrl) == 0)
                    {
                        if (_pagingRenderMode == PagingRenderMode.LinkButton)
                        {
                            _lnkBtnFirst.Enabled = false;
                            _lnkBtnPrev.Enabled = false;
                        }
                        else if (_pagingRenderMode == PagingRenderMode.ImageButton)
                        {
                            _imgBtnFirst.Enabled = false;
                            _imgBtnPrev.Enabled = false;
                        }
                    }
                    if (this.GetPageIndex(_ctrl) == this.GetPageCount(_ctrl) - 1)
                    {
                        if (_pagingRenderMode == PagingRenderMode.LinkButton)
                        {
                            _lnkBtnNext.Enabled = false;
                            _lnkBtnLast.Enabled = false;
                        }
                        else if (_pagingRenderMode == PagingRenderMode.ImageButton)
                        {
                            _imgBtnNext.Enabled = false;
                            _imgBtnLast.Enabled = false;
                        }
                    }

                    _txtGoToPage.Text = (this.GetPageIndex(_ctrl) + 1).ToString();
                }
                else
                {
                    if (_pagingRenderMode == PagingRenderMode.LinkButton)
                    {
                        _lnkBtnFirst.Enabled = false;
                        _lnkBtnPrev.Enabled = false;
                        _lnkBtnNext.Enabled = false;
                        _lnkBtnLast.Enabled = false;
                    }
                    else if (_pagingRenderMode == PagingRenderMode.ImageButton)
                    {
                        _imgBtnFirst.Enabled = false;
                        _imgBtnPrev.Enabled = false;
                        _imgBtnNext.Enabled = false;
                        _imgBtnLast.Enabled = false;
                    }
                    _txtGoToPage.Text = this.GetPageCount(_ctrl).ToString();
                }

                _ddlstPageSize.SelectedIndex =
                    _ddlstPageSize.Items.IndexOf(
                    _ddlstPageSize.Items.FindByValue(this.GetPageSize(_ctrl).ToString()));
            }
        }

        private void ApplyStyle()
        {
            if (_pagingRenderMode == PagingRenderMode.LinkButton)
            {
                if (_navigateControlStyle != null)
                {
                    _lnkBtnFirst.ApplyStyle(NavigateControlStyle);
                    _lnkBtnPrev.ApplyStyle(NavigateControlStyle);
                    _lnkBtnNext.ApplyStyle(NavigateControlStyle);
                    _lnkBtnLast.ApplyStyle(NavigateControlStyle);

                    _lnkBtnGoTo.ApplyStyle(NavigateControlStyle);
                }
            }
            else if (_pagingRenderMode == PagingRenderMode.ImageButton)
            {
                if (_navigateControlStyle != null)
                {
                    _imgBtnFirst.ApplyStyle(NavigateControlStyle);
                    _imgBtnPrev.ApplyStyle(NavigateControlStyle);
                    _imgBtnNext.ApplyStyle(NavigateControlStyle);
                    _imgBtnLast.ApplyStyle(NavigateControlStyle);

                    _imgBtnGoTo.ApplyStyle(NavigateControlStyle);
                }
            }

            if (_goToPageControlStyle != null)
            {
                _txtGoToPage.ApplyStyle(GoToPageControlStyle);
            }

            if (_pageSizeControlStyle != null)
            {
                _ddlstPageSize.ApplyStyle(PageSizeControlStyle);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 页大小变化之后激发。
        /// </summary>
        [Browsable(true)]
        [Category("行为")]
        [Description("页大小变化之后激发。")]
        public event PageSizeChangedEventHandler PageSizeChanged
        {
            add
            {
                Events.AddHandler("_PageSizeChanged", value);
            }
            remove
            {
                Events.RemoveHandler("_PageSizeChanged", value);
            }
        }

        /// <summary>
        /// 当页大小变化之后。
        /// </summary>
        /// <param name="e">页大小变化参数。</param>
        protected virtual void OnPageSizeChanged(PageSizeChangedEventArgs e)
        {
            PageSizeChangedEventHandler PageSizeChangedHandler =
                (PageSizeChangedEventHandler)Events["_PageSizeChanged"];

            if (PageSizeChangedHandler != null)
            {
                PageSizeChangedHandler(this, e);
            }
        }

        /// <summary>
        /// 页索引变化之后激发。
        /// </summary>
        [Browsable(true)]
        [Category("行为")]
        [Description("页索引变化之后激发。")]
        public event EventHandler PageIndexChanged
        {
            add
            {
                Events.AddHandler("_PageIndexChanged", value);
            }
            remove
            {
                Events.RemoveHandler("_PageIndexChanged", value);
            }
        }

        /// <summary>
        /// 当页索引变化之后。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnPageIndexChanged(EventArgs e)
        {
            EventHandler PageIndexChangedHandler =
                (EventHandler)Events["_PageIndexChanged"];

            if (PageIndexChangedHandler != null)
            {
                PageIndexChangedHandler(this, e);
            }
        }

        #endregion

        /// <summary>
        /// 重写OnInit方法，以响应控件的Init事件，并将当前控件注册为需要页面保存其控件状态的页面控件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
            InitControl();
        }

        /// <summary>
        /// 保存控件状态。
        /// </summary>
        /// <returns>需要保存的控件状态对象。</returns>
        protected override object SaveControlState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveControlState());

            list.Add(_controlId);
            list.Add(_pagingRenderMode);

            return list;
        }

        /// <summary>
        /// 在回发请求的时候去重新装载控件状态。
        /// </summary>
        /// <param name="savedState">需要状态的控件状态。</param>
        protected override void LoadControlState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = savedState as System.Collections.ArrayList;

                if (list.Count >= 3)
                {
                    base.LoadControlState(list[0]);

                    ControlID = (string)list[1];
                    PagingRenderMode = (PagingRenderMode)list[2];
                }
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }

        protected override object SaveViewState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveViewState());

            if (_navigateControlStyle != null)
            {
                list.Add(
                    ((IStateManager)_navigateControlStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_goToPageControlStyle != null)
            {
                list.Add(
                    ((IStateManager)_goToPageControlStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_pageSizeControlStyle != null)
            {
                list.Add(
                    ((IStateManager)_pageSizeControlStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }

            return list;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)savedState;

                if (list.Count >= 4)
                {
                    base.LoadViewState(list[0]);

                    if (list[1] != null)
                    {
                        ((IStateManager)NavigateControlStyle).LoadViewState(list[1]);
                    }
                    if (list[2] != null)
                    {
                        ((IStateManager)GoToPageControlStyle).LoadViewState(list[2]);
                    }
                    if (list[3] != null)
                    {
                        ((IStateManager)PageSizeControlStyle).LoadViewState(list[3]);
                    }
                }
            }
            else
            {
                base.LoadViewState(savedState);
            }
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_navigateControlStyle != null)
            {
                ((IStateManager)_navigateControlStyle).TrackViewState();
            }
            if (_goToPageControlStyle != null)
            {
                ((IStateManager)_goToPageControlStyle).TrackViewState();
            }
            if (_pageSizeControlStyle != null)
            {
                ((IStateManager)_pageSizeControlStyle).TrackViewState();
            }
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            _lnkBtnFirst = new LinkButton();
            _lnkBtnFirst.ID = "lnkBtnFirst";
            _lnkBtnFirst.Text = this.FirstPageText;
            _lnkBtnFirst.CommandName = "First";
            _lnkBtnFirst.CausesValidation = false;
            _lnkBtnFirst.Command += new CommandEventHandler(First_Click);

            _lnkBtnPrev = new LinkButton();
            _lnkBtnPrev.ID = "lnkBtnPrev";
            _lnkBtnPrev.Text = this.PreviousPageText;
            _lnkBtnPrev.CommandName = "Prev";
            _lnkBtnPrev.CausesValidation = false;
            _lnkBtnPrev.Command += new CommandEventHandler(Prev_Click);

            _lnkBtnNext = new LinkButton();
            _lnkBtnNext.ID = "lnkBtnNext";
            _lnkBtnNext.Text = this.NextPageText;
            _lnkBtnNext.CommandName = "Next";
            _lnkBtnNext.CausesValidation = false;
            _lnkBtnNext.Command += new CommandEventHandler(Next_Click);

            _lnkBtnLast = new LinkButton();
            _lnkBtnLast.ID = "lnkBtnLast";
            _lnkBtnLast.Text = this.LastPageText;
            _lnkBtnLast.CommandName = "Last";
            _lnkBtnLast.CausesValidation = false;
            _lnkBtnLast.Command += new CommandEventHandler(Last_Click);

            _imgBtnFirst = new ImageButton();
            _imgBtnFirst.ID = "imgBtnFirst";
            _imgBtnFirst.AlternateText = this.FirstPageText;
            _imgBtnFirst.ToolTip = _imgBtnFirst.AlternateText;
            _imgBtnFirst.ImageUrl = this.FirstPageImageUrl;
            _imgBtnFirst.CommandName = "First";
            _imgBtnFirst.CausesValidation = false;
            _imgBtnFirst.Command += new CommandEventHandler(First_Click);

            _imgBtnPrev = new ImageButton();
            _imgBtnPrev.ID = "imgBtnPrev";
            _imgBtnPrev.AlternateText = this.PreviousPageText;
            _imgBtnPrev.ToolTip = _imgBtnPrev.AlternateText;
            _imgBtnPrev.ImageUrl = this.PreviousPageImageUrl;
            _imgBtnPrev.CommandName = "Prev";
            _imgBtnPrev.CausesValidation = false;
            _imgBtnPrev.Command += new CommandEventHandler(Prev_Click);

            _imgBtnNext = new ImageButton();
            _imgBtnNext.ID = "imgBtnNext";
            _imgBtnNext.AlternateText = this.NextPageText;
            _imgBtnNext.ToolTip = _imgBtnNext.AlternateText;
            _imgBtnNext.ImageUrl = this.NextPageImageUrl;
            _imgBtnNext.CommandName = "Next";
            _imgBtnNext.CausesValidation = false;
            _imgBtnNext.Command += new CommandEventHandler(Next_Click);

            _imgBtnLast = new ImageButton();
            _imgBtnLast.ID = "imgBtnLast";
            _imgBtnLast.AlternateText = this.LastPageText;
            _imgBtnLast.ToolTip = _imgBtnLast.AlternateText;
            _imgBtnLast.ImageUrl = this.LastPageImageUrl;
            _imgBtnLast.CommandName = "Last";
            _imgBtnLast.CausesValidation = false;
            _imgBtnLast.Command += new CommandEventHandler(Last_Click);

            _lblGoToPageBefore = new Label();
            _lblGoToPageBefore.ID = "lblGoToPageBefore";
            _lblGoToPageBefore.Text = this.GoToPageBeforeText;

            _txtGoToPage = new TextBox();
            _txtGoToPage.ID = "txtGoToPage";
            _txtGoToPage.Text = "1";

            _lblGoToPageAfter = new Label();
            _lblGoToPageAfter.ID = "lblGoToPageAfter";
            _lblGoToPageAfter.Text = this.GoToPageAfterText;

            _lnkBtnGoTo = new LinkButton();
            _lnkBtnGoTo.ID = "lnkBtnGoTo";
            _lnkBtnGoTo.Text = this.GoToPageText;
            _lnkBtnGoTo.Command += new CommandEventHandler(GoTo_Click);

            _imgBtnGoTo = new ImageButton();
            _imgBtnGoTo.ID = "imgBtnGoTo";
            _imgBtnGoTo.AlternateText = this.GoToPageText;
            _imgBtnGoTo.ToolTip = _imgBtnGoTo.AlternateText;
            _imgBtnGoTo.ImageUrl = this.GoToPageImageUrl;
            _imgBtnGoTo.Command += new CommandEventHandler(GoTo_Click);

            _lblPageSizeBefore = new Label();
            _lblPageSizeBefore.ID = "lblPageSizeBefore";
            _lblPageSizeBefore.Text = this.PageSizeBeforeText;

            _ddlstPageSize = new DropDownList();
            _ddlstPageSize.ID = "ddlstPageSize";
            _ddlstPageSize.AutoPostBack = true;
            _ddlstPageSize.DataSource = this.PageSizes;
            _ddlstPageSize.DataBind();
            _ddlstPageSize.SelectedIndex = 1;
            _ddlstPageSize.SelectedIndexChanged += new EventHandler(_ddlstPageSize_SelectedIndexChanged);

            _lblPageSizeAfter = new Label();
            _lblPageSizeAfter.ID = "lblPageSizeAfter";
            _lblPageSizeAfter.Text = this.PageSizeAfterText;

            this.Controls.Add(_lnkBtnFirst);
            this.Controls.Add(_lnkBtnPrev);
            this.Controls.Add(_lnkBtnNext);
            this.Controls.Add(_lnkBtnLast);

            this.Controls.Add(_imgBtnFirst);
            this.Controls.Add(_imgBtnPrev);
            this.Controls.Add(_imgBtnNext);
            this.Controls.Add(_imgBtnLast);

            this.Controls.Add(_lblGoToPageBefore);
            this.Controls.Add(_txtGoToPage);
            this.Controls.Add(_lblGoToPageAfter);
            this.Controls.Add(_lnkBtnGoTo);
            this.Controls.Add(_imgBtnGoTo);

            this.Controls.Add(_lblPageSizeBefore);
            this.Controls.Add(_ddlstPageSize);
            this.Controls.Add(_lblPageSizeAfter);
        }

        void _ddlstPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ctrl != null && !string.IsNullOrEmpty(_ddlstPageSize.SelectedValue))
            {
                PageSizeChangedEventArgs pageSizeChangedArg = new PageSizeChangedEventArgs();
                pageSizeChangedArg.SourcePageSize = this.GetPageSize(_ctrl);

                this.SetPageSize(_ctrl, Convert.ToInt32(_ddlstPageSize.SelectedValue));
                this.SetPageIndex(_ctrl, 0);

                pageSizeChangedArg.CurrentPageSize = this.GetPageSize(_ctrl);
                OnPageSizeChanged(pageSizeChangedArg);
            }
        }

        void GoTo_Click(object sender, CommandEventArgs e)
        {
            if (_ctrl != null)
            {
                int pageIdx = 0;
                int count = this.GetPageCount(_ctrl);
                if (count <= 0)
                {
                    return;
                }
                try
                {
                    pageIdx = Convert.ToInt32(_txtGoToPage.Text) - 1;
                    if (pageIdx < 0)
                    {
                        pageIdx = 0;
                    }
                    if (pageIdx >= count)
                    {
                        pageIdx = count - 1;
                    }
                }
                catch (FormatException)
                {
                    pageIdx = 0;
                }
                this.SetPageIndex(_ctrl, pageIdx);

                this.DataBindSource();
            }
        }

        void First_Click(object sender, CommandEventArgs e)
        {
            if (_ctrl != null)
            {
                this.SetPageIndex(_ctrl, 0);

                this.DataBindSource();
            }
        }

        void Prev_Click(object sender, CommandEventArgs e)
        {
            if (_ctrl != null)
            {
                if (this.GetPageIndex(_ctrl) > 0)
                {
                    this.SetPageIndex(_ctrl, this.GetPageIndex(_ctrl) - 1);
                }

                this.DataBindSource();
            }
        }

        void Next_Click(object sender, CommandEventArgs e)
        {
            if (_ctrl != null)
            {
                if (this.GetPageIndex(_ctrl) < this.GetPageCount(_ctrl) - 1)
                {
                    this.SetPageIndex(_ctrl, this.GetPageIndex(_ctrl) + 1);
                }

                this.DataBindSource();
            }
        }

        void Last_Click(object sender, CommandEventArgs e)
        {
            if (_ctrl != null)
            {
                this.SetPageIndex(_ctrl, this.GetPageCount(_ctrl) - 1);

                this.DataBindSource();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.IsVisibleWithNoData && this.GetPageCount(_ctrl) <= 0)
            {
                return;
            }

            this.ApplyStyle();

            this.AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1", false);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Width, LeftPaddingWidth.ToString(), false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (_pagingRenderMode == PagingRenderMode.LinkButton)
            {
                _lnkBtnFirst.RenderControl(writer);
                _lnkBtnPrev.RenderControl(writer);
                _lnkBtnNext.RenderControl(writer);
                _lnkBtnLast.RenderControl(writer);
            }
            else if (_pagingRenderMode == PagingRenderMode.ImageButton)
            {
                _imgBtnFirst.RenderControl(writer);
                _imgBtnPrev.RenderControl(writer);
                _imgBtnNext.RenderControl(writer);
                _imgBtnLast.RenderControl(writer);
            }
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _lblGoToPageBefore.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _txtGoToPage.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.Write(this.PageIndexPageCountSeperator + (_ctrl == null ? "1" : this.GetPageCount(_ctrl).ToString()));

            _lblGoToPageAfter.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (_pagingRenderMode == PagingRenderMode.LinkButton)
            {
                _lnkBtnGoTo.RenderControl(writer);
            }
            else if (_pagingRenderMode == PagingRenderMode.ImageButton)
            {
                _imgBtnGoTo.RenderControl(writer);
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();

            if (this.GetPageSize(_ctrl) > 1 || this.DesignMode)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                _lblPageSizeBefore.RenderControl(writer);

                writer.RenderEndTag();
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                _ddlstPageSize.RenderControl(writer);

                writer.RenderEndTag();
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                _lblPageSizeAfter.RenderControl(writer);

                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Width, RightPaddingWidth.ToString(), false);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.SetControlState();
        }
    }
}
