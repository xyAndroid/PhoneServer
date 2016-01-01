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
    /// 设定排序字段的呈现方式。
    /// </summary>
    public enum SortingSetSortingFieldsRenderMode
    {
        /// <summary>
        /// 按照ListBox呈现。
        /// </summary>
        ListBox,
        /// <summary>
        /// 按照RadioButtonList呈现。
        /// </summary>
        RadioButtonList,
    }

    /// <summary>
    /// 命令呈现方式。
    /// </summary>
    public enum SortingCommandRenderMode
    {
        /// <summary>
        /// 按照Button呈现。
        /// </summary>
        Button,
        /// <summary>
        /// 按照ImageButton呈现。
        /// </summary>
        ImageButton,
    }

    /// <summary>
    /// 设置排序。
    /// </summary>
    [Designer(typeof(System.Web.UI.Design.WebControls.CompositeControlDesigner))]
    [DefaultProperty("ControlID")]
    [DefaultEvent("DataBinding")]
    [System.Drawing.ToolboxBitmap(typeof(Sorting), "Sorting.bmp")]
    [ToolboxData("<{0}:Sorting runat=server></{0}:Sorting>")]
    public class Sorting : CompositeControl
    {
        #region Fields

        Style _commandStyle;
        Style _setSortingFieldsStyle;
        Style _sortingFieldsStyle;
        Style _sortDirectionStyle;

        #endregion

        #region Controls

        DropDownList _ddlstSortingFields;
        RadioButtonList _rdoBtnLstSortDirection;
        
        ListBox _lstBoxSetSortingFields;
        RadioButtonList _rdoBtnLstSetSortingFields; 

        Button _btnAdd;
        Button _btnUp;
        Button _btnDown;
        Button _btnDelete;
        Button _btnClear;
        Button _btnOK;

        ImageButton _imgBtnAdd;
        ImageButton _imgBtnUp;
        ImageButton _imgBtnDown;
        ImageButton _imgBtnDelete;
        ImageButton _imgBtnClear;
        ImageButton _imgBtnOK;

        WebControl _ctrl;
        WebControl _displayOrHideCtrl;

        #endregion

        #region Properties

        private string _controlId = string.Empty;
        /// <summary>
        /// 需要设置排序的控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("需要设置排序的控件ID。")]
        public string ConctrolID
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private string _displayOrHideControlId = string.Empty;
        /// <summary>
        /// 显示和隐藏排序控件的外部控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("行为")]
        [Description("显示和隐藏排序控件的外部控件ID。")]
        public string DisplayOrHideControlID
        {
            get { return _displayOrHideControlId; }
            set { _displayOrHideControlId = value; }
        }

        private const string m_HideWithBeingSortedViewStateName = "HideWithBeingSorted";
        /// <summary>
        /// 在执行排序之后是否隐藏控件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("在执行排序之后是否隐藏控件。")]
        public bool HideWithBeingSorted
        {
            get
            {
                if (this.ViewState[m_HideWithBeingSortedViewStateName] != null)
                {
                    return (bool)this.ViewState[m_HideWithBeingSortedViewStateName];
                }
                return true;
            }
            set
            {
                this.ViewState[m_HideWithBeingSortedViewStateName] = value;
            }
        }

        private SortingSetSortingFieldsRenderMode _setSortingFieldsRenderMode = SortingSetSortingFieldsRenderMode.ListBox;
        /// <summary>
        /// 设定排序字段呈现方式。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(SortingSetSortingFieldsRenderMode.ListBox)]
        [Category("外观")]
        [Description("设定排序字段呈现方式。")]
        public SortingSetSortingFieldsRenderMode SetSortingFieldsRenderMode
        {
            get { return _setSortingFieldsRenderMode; }
            set { _setSortingFieldsRenderMode = value; }
        }

        private const string m_BlankSortItemTextViewStateName = "BlankSortItemText";
        /// <summary>
        /// 空白排序项的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("--请选择排序项--")]
        [Category("外观")]
        [Description("空白排序项的显示文本。")]
        public string BlankSortItemText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankSortItemTextViewStateName, "--请选择排序项--");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankSortItemTextViewStateName, value);
            }
        }

        private const string m_BlankSortItemValueViewStateName = "BlankSortItemValue";
        /// <summary>
        /// 空白排序项的查询文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("外观")]
        [Description("空白排序项的查询文本。")]
        public string BlankSortItemValue
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankSortItemValueViewStateName, "");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankSortItemValueViewStateName, value);
            }
        }
        
        private SortingCommandRenderMode _commandRenderMode = SortingCommandRenderMode.Button;
        /// <summary>
        /// 命令呈现方式。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(SortingCommandRenderMode.Button)]
        [Category("外观")]
        [Description("命令呈现方式。")]
        public SortingCommandRenderMode CommandRenderMode
        {
            get { return _commandRenderMode; }
            set { _commandRenderMode = value; }
        }

        private const string m_AddCommandTextViewStateName = "AddCommandText";
        /// <summary>
        /// 将排序字段添加到排序列表中命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("添加")]
        [Category("命令")]
        [Description("将排序字段添加到排序列表中命令文本。")]
        public string AddCommandText
        {
            get
            {
                if (this.ViewState[m_AddCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_AddCommandTextViewStateName];
                }
                return "添加";
            }
            set
            {
                this.ViewState[m_AddCommandTextViewStateName] = value;
            }
        }

        private const string m_UpCommandTextViewStateName = "UpCommandText";
        /// <summary>
        /// 在排序列表中向上重排命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("上移")]
        [Category("命令")]
        [Description("在排序列表中向上重排命令文本。")]
        public string UpCommandText
        {
            get
            {
                if (this.ViewState[m_UpCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_UpCommandTextViewStateName];
                }
                return "上移";
            }
            set
            {
                this.ViewState[m_UpCommandTextViewStateName] = value;
            }
        }

        private const string m_DownCommandTextViewStateName = "DownCommandText";
        /// <summary>
        /// 在排序列表中向下重排命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("下移")]
        [Category("命令")]
        [Description("在排序列表中向下重排命令文本。")]
        public string DownCommandText
        {
            get
            {
                if (this.ViewState[m_DownCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_DownCommandTextViewStateName];
                }
                return "下移";
            }
            set
            {
                this.ViewState[m_DownCommandTextViewStateName] = value;
            }
        }

        private const string m_DeleteCommandTextViewStateName = "DeleteCommandText";
        /// <summary>
        /// 在排序列表中删除排序项命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("删除")]
        [Category("命令")]
        [Description("在排序列表中删除排序项命令文本。")]
        public string DeleteCommandText
        {
            get
            {
                if (this.ViewState[m_DeleteCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_DeleteCommandTextViewStateName];
                }
                return "删除";
            }
            set
            {
                this.ViewState[m_DeleteCommandTextViewStateName] = value;
            }
        }

        private const string m_ClearCommandTextViewStateName = "ClearCommandText";
        /// <summary>
        /// 在排序列表中清除排序项命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("清除")]
        [Category("命令")]
        [Description("在排序列表中清除排序项命令文本。")]
        public string ClearCommandText
        {
            get
            {
                if (this.ViewState[m_ClearCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_ClearCommandTextViewStateName];
                }
                return "清除";
            }
            set
            {
                this.ViewState[m_ClearCommandTextViewStateName] = value;
            }
        }

        private const string m_OkCommandTextViewStateName = "OkCommandText";
        /// <summary>
        /// 确定排序列表中排序项命令文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("确定")]
        [Category("命令")]
        [Description("确定排序列表中排序项命令文本。")]
        public string OkCommandText
        {
            get
            {
                if (this.ViewState[m_OkCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_OkCommandTextViewStateName];
                }
                return "确定";
            }
            set
            {
                this.ViewState[m_OkCommandTextViewStateName] = value;
            }
        }

        private const string m_DisplayCommandTextViewStateName = "DisplayCommandText";
        /// <summary>
        /// 显示/隐藏命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("设置排序")]
        [Category("命令")]
        [Description("显示/隐藏命令的显示文本。")]
        public string DisplayCommandText
        {
            get
            {
                if (this.ViewState[m_DisplayCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_DisplayCommandTextViewStateName];
                }
                return "设置排序";
            }
            set
            {
                this.ViewState[m_DisplayCommandTextViewStateName] = value;
            }
        }

        private const string m_HideCommandTextViewStateName = "HideCommandText";
        /// <summary>
        /// 显示/隐藏命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("隐藏")]
        [Category("命令")]
        [Description("显示/隐藏命令的隐藏文本。")]
        public string HideCommandText
        {
            get
            {
                if (this.ViewState[m_HideCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_HideCommandTextViewStateName];
                }
                return "隐藏";
            }
            set
            {
                this.ViewState[m_HideCommandTextViewStateName] = value;
            }
        }

        private const string m_AddCommandImageUrlViewStateName = "AddCommandImageUrl";
        /// <summary>
        /// 将排序字段添加到排序列表中图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("添加")]
        [Category("命令")]
        [Description("将排序字段添加到排序列表中图片链接。")]
        public string AddCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_AddCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_AddCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_AddCommandImageUrlViewStateName] = value;
            }
        }

        private const string m_UpCommandImageUrlViewStateName = "UpCommandImageUrl";
        /// <summary>
        /// 在排序列表中向上重排图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("命令")]
        [Description("在排序列表中向上重排图片链接。")]
        public string UpCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_UpCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_UpCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_UpCommandImageUrlViewStateName] = value;
            }
        }

        private const string m_DownCommandImageUrlViewStateName = "DownCommandImageUrl";
        /// <summary>
        /// 在排序列表中向下重排图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("命令")]
        [Description("在排序列表中向下重排图片链接。")]
        public string DownCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_DownCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_DownCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_DownCommandImageUrlViewStateName] = value;
            }
        }

        private const string m_DeleteCommandImageUrlViewStateName = "DeleteCommandImageUrl";
        /// <summary>
        /// 在排序列表中删除排序项命令图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("命令")]
        [Description("在排序列表中删除排序项命令图片链接。")]
        public string DeleteCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_DeleteCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_DeleteCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_DeleteCommandImageUrlViewStateName] = value;
            }
        }

        private const string m_ClearCommandImageUrlViewStateName = "ClearCommandImageUrl";
        /// <summary>
        /// 在排序列表中清除排序项命令图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("命令")]
        [Description("在排序列表中清除排序项命令图片链接。")]
        public string ClearCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_ClearCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_ClearCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_ClearCommandImageUrlViewStateName] = value;
            }
        }

        private const string m_OkCommandImageUrlViewStateName = "OkCommandImageUrl";
        /// <summary>
        /// 确定排序列表中排序项命令图片链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("命令")]
        [Description("确定排序列表中排序项命令图片链接。")]
        public string OkCommandImageUrl
        {
            get
            {
                if (this.ViewState[m_OkCommandImageUrlViewStateName] != null)
                {
                    return (string)this.ViewState[m_OkCommandImageUrlViewStateName];
                }
                return string.Empty;
            }
            set
            {
                this.ViewState[m_OkCommandImageUrlViewStateName] = value;
            }
        }

        /// <summary>
        /// 命令样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("命令控件的样式。")]
        public virtual Style CommandStyle
        {
            get
            {
                if (_commandStyle == null)
                {
                    _commandStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_commandStyle).TrackViewState();
                    }
                }
                return _commandStyle;
            }
        }

        /// <summary>
        /// 设定排序字段样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("给定排序列表控件的样式。")]
        public virtual Style SetSortingFieldsStyle
        {
            get
            {
                if (_setSortingFieldsStyle == null)
                {
                    _setSortingFieldsStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_setSortingFieldsStyle).TrackViewState();
                    }
                }
                return _setSortingFieldsStyle;
            }
        }

        /// <summary>
        /// 排序字段样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("排序列表控件的样式。")]
        public virtual Style SortingFieldsStyle
        {
            get
            {
                if (_sortingFieldsStyle == null)
                {
                    _sortingFieldsStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_sortingFieldsStyle).TrackViewState();
                    }
                }
                return _sortingFieldsStyle;
            }
        }

        /// <summary>
        /// 排序方向样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("排序方向控件的样式。")]
        public virtual Style SortDirectionStyle
        {
            get
            {
                if (_sortDirectionStyle == null)
                {
                    _sortDirectionStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_sortDirectionStyle).TrackViewState();
                    }
                }
                return _sortDirectionStyle;
            }
        }

        #endregion

        #region Operates

        private void InitControl()
        {
            if (!this.DesignMode)
            {
                this.InitSortObjectControl();
                this.InitDisplayOrHideControl();
            }
        }

        private void InitSortObjectControl()
        {
            _ctrl = (WebControl)this.Page.FindControl(this.ConctrolID);
            if (_ctrl == null)
            {
                _ctrl = (WebControl)Utility.FindControlInNamingContainer(this, this.ConctrolID);
            }
        }

        private void InitDisplayOrHideControl()
        {
            _displayOrHideCtrl = (WebControl)this.Page.FindControl(_displayOrHideControlId);
            
            if (_displayOrHideCtrl == null)
            {
                _displayOrHideCtrl = (WebControl)Utility.FindControlInNamingContainer(this, _displayOrHideControlId);
            }

            if (_displayOrHideCtrl is IButtonControl)
            {
                IButtonControl btnCtrl = (IButtonControl)_displayOrHideCtrl;
                this.SetDisplayOrHideControlProperty();
                btnCtrl.Command += new CommandEventHandler(DisplayOrHide_Command);
            }
        }

        private void SetDisplayOrHideControlProperty()
        {
            if (_displayOrHideCtrl is IButtonControl)
            {
                IButtonControl btnCtrl = (IButtonControl)_displayOrHideCtrl;
                if (this.Visible)
                {
                    btnCtrl.Text = this.HideCommandText;
                }
                else
                {
                    btnCtrl.Text = this.DisplayCommandText;
                }
            }
        }

        private void ConstructSortingFields(DropDownList ddlstSortingFields, WebControl ctrl)
        {
            if (ctrl == null || ddlstSortingFields == null)
            {
                return;
            }

            ddlstSortingFields.Items.Clear();

            ddlstSortingFields.Items.Add(new ListItem(this.BlankSortItemText, this.BlankSortItemValue));

            if (ctrl is GridView)
            {
                GridView gv = (GridView)ctrl;

                if (!this.DesignMode)
                {
                    foreach (DataControlField aField in gv.Columns)
                    {
                        if (string.IsNullOrEmpty(aField.SortExpression))
                        {
                            continue;
                        }
                        ddlstSortingFields.Items.Add(new ListItem(
                            aField.HeaderText, aField.SortExpression));
                    }
                }
            }
        }

        private void DoCommand(string cmdName)
        {
            ListControl lstCtrl = null;
            if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.ListBox)
            {
                lstCtrl = _lstBoxSetSortingFields;
            }
            else if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.RadioButtonList)
            {
                lstCtrl = _rdoBtnLstSetSortingFields;
            }

            if (lstCtrl == null)
            {
                return;
            }

            if (cmdName.Equals("Add"))
            {
                if (string.IsNullOrEmpty(_ddlstSortingFields.SelectedValue))
                {
                    return;
                }
                string itemText = _ddlstSortingFields.SelectedItem.Text + "（" + _rdoBtnLstSortDirection.SelectedItem.Text + "）";
                string itemValue = _ddlstSortingFields.SelectedValue + "_" + _rdoBtnLstSortDirection.SelectedValue;
                if (lstCtrl.Items.FindByText(itemText) != null)
                {
                    return;
                }
                lstCtrl.Items.Add(new ListItem(itemText, itemValue));
            }
            else if (cmdName.Equals("Delete"))
            {
                for (int i = 0; i < lstCtrl.Items.Count; i++)
                {
                    if (lstCtrl.Items[i].Selected)
                    {
                        lstCtrl.Items.Remove(lstCtrl.Items[i]);
                        i--;
                    }
                }
            }
            else if(cmdName.Equals("Clear"))
            {
                lstCtrl.Items.Clear();
            }
            else if (cmdName.Equals("Up"))
            {
                for (int i = 0; i < lstCtrl.Items.Count; i++)
                {

                    if (lstCtrl.Items[i].Selected && i > 0)
                    {
                        lstCtrl.Items.Insert(i - 1, lstCtrl.Items[i]);
                        lstCtrl.Items.RemoveAt(i + 1);
                    }
                }
            }
            else if (cmdName.Equals("Down"))
            {
                for (int i = 0; i < lstCtrl.Items.Count; i++)
                {
                    if (lstCtrl.Items[i].Selected && i < lstCtrl.Items.Count - 1)
                    {
                        lstCtrl.Items.Insert(i + 2, lstCtrl.Items[i]);
                        lstCtrl.Items.RemoveAt(i);
                        i = i + 3;
                    }
                }
            }
            else if (cmdName.Equals("OK"))
            {
                if (_ctrl == null)
                {
                    return;
                }

                string sortExp = string.Empty;
                for(int i = 0; i < lstCtrl.Items.Count; i++)
                {
                    string[] itemValues = lstCtrl.Items[i].Value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                    sortExp += (itemValues[0] + " " + itemValues[1]);

                    if (i < lstCtrl.Items.Count - 1)
                    {
                        sortExp += ", ";
                    }
                }

                if (_ctrl is GridView)
                {
                    GridView gv = (GridView)_ctrl;

                    gv.Sort(sortExp, SortDirection.Ascending);

                    if (this.HideWithBeingSorted)
                    {
                        this.Visible = false;

                        this.SetDisplayOrHideControlProperty();
                    }
                }
            }

        }

        private void ApplyStyle()
        {
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                if (_commandStyle != null)
                {
                    _btnAdd.ApplyStyle(CommandStyle);
                    _btnDelete.ApplyStyle(CommandStyle);
                    _btnUp.ApplyStyle(CommandStyle);
                    _btnDown.ApplyStyle(CommandStyle);
                    _btnClear.ApplyStyle(CommandStyle);
                    _btnOK.ApplyStyle(CommandStyle);
                }
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnAdd.ApplyStyle(CommandStyle);
                _imgBtnDelete.ApplyStyle(CommandStyle);
                _imgBtnUp.ApplyStyle(CommandStyle);
                _imgBtnDown.ApplyStyle(CommandStyle);
                _imgBtnOK.ApplyStyle(CommandStyle);
            }

            if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.ListBox)
            {
                _lstBoxSetSortingFields.ApplyStyle(SetSortingFieldsStyle);
            }
            else if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.RadioButtonList)
            {
                _rdoBtnLstSetSortingFields.ApplyStyle(SetSortingFieldsStyle);
            }

            if (_sortingFieldsStyle != null)
            {
                _ddlstSortingFields.ApplyStyle(_sortingFieldsStyle);
            }

            if (_sortDirectionStyle != null)
            {
                _rdoBtnLstSortDirection.ApplyStyle(_sortDirectionStyle);
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
            InitControl();
        }

        protected override object SaveControlState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveControlState());

            list.Add(_controlId);
            list.Add(_displayOrHideControlId);
            list.Add(_setSortingFieldsRenderMode);
            list.Add(_commandRenderMode);

            return list;
        }

        protected override void LoadControlState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)savedState;

                if (list.Count >= 5)
                {
                    base.LoadControlState(list[0]);

                    _controlId = (string)list[1];
                    _displayOrHideControlId = (string)list[2];
                    _setSortingFieldsRenderMode = (SortingSetSortingFieldsRenderMode)list[3];
                    _commandRenderMode = (SortingCommandRenderMode)list[4];
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

            if (_commandStyle != null)
            {
                list.Add(
                    ((IStateManager)_commandStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_setSortingFieldsStyle != null)
            {
                list.Add(
                    ((IStateManager)_setSortingFieldsStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_sortingFieldsStyle != null)
            {
                list.Add(
                    ((IStateManager)_sortingFieldsStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_sortDirectionStyle != null)
            {
                list.Add(
                    ((IStateManager)_sortDirectionStyle).SaveViewState());
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

                if (list.Count >= 5)
                {
                    base.LoadViewState(list[0]);

                    if (list[1] != null)
                    {
                        ((IStateManager)CommandStyle).LoadViewState(list[1]);
                    }
                    if (list[2] != null)
                    {
                        ((IStateManager)SetSortingFieldsStyle).LoadViewState(list[2]);
                    }
                    if (list[3] != null)
                    {
                        ((IStateManager)SortingFieldsStyle).LoadViewState(list[3]);
                    }
                    if (list[4] != null)
                    {
                        ((IStateManager)SortDirectionStyle).LoadViewState(list[4]);
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
            if (_commandStyle != null)
            {
                ((IStateManager)_commandStyle).TrackViewState();
            }
            if (_setSortingFieldsStyle != null)
            {
                ((IStateManager)_setSortingFieldsStyle).TrackViewState();
            }
            if (_sortingFieldsStyle != null)
            {
                ((IStateManager)_sortingFieldsStyle).TrackViewState();
            }
            if (_sortDirectionStyle != null)
            {
                ((IStateManager)_sortDirectionStyle).TrackViewState();
            }
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            _ddlstSortingFields = new DropDownList();
            _ddlstSortingFields.ID = "ddlstSortingFields";
            this.ConstructSortingFields(_ddlstSortingFields, _ctrl);

            _rdoBtnLstSortDirection = new RadioButtonList();
            _rdoBtnLstSortDirection.ID = "rdoBtnLstSortingDirection";
            _rdoBtnLstSortDirection.Items.Add(new ListItem("升序", "ASC"));
            _rdoBtnLstSortDirection.Items.Add(new ListItem("降序", "DESC"));
            _rdoBtnLstSortDirection.SelectedIndex = 0;
            _rdoBtnLstSortDirection.RepeatDirection = RepeatDirection.Horizontal;

            _btnAdd = new Button();
            _btnAdd.ID = "btnAdd";
            _btnAdd.Text = this.AddCommandText;
            _btnAdd.CommandName = "Add";
            _btnAdd.Command += new CommandEventHandler(Add_Command);

            _btnDelete = new Button();
            _btnDelete.ID = "btnDelete";
            _btnDelete.Text = this.DeleteCommandText;
            _btnDelete.CommandName = "Delete";
            _btnDelete.Command += new CommandEventHandler(Delete_Command);

            _btnClear = new Button();
            _btnClear.ID = "btnClear";
            _btnClear.Text = this.ClearCommandText;
            _btnClear.CommandName = "Clear";
            _btnClear.Command += new CommandEventHandler(Clear_Command);

            _btnUp = new Button();
            _btnUp.ID = "btnUp";
            _btnUp.Text = this.UpCommandText;
            _btnUp.CommandName = "Up";
            _btnUp.Command += new CommandEventHandler(Up_Command);

            _btnDown = new Button();
            _btnDown.ID = "btnDown";
            _btnDown.Text = this.DownCommandText;
            _btnDown.CommandName = "Down";
            _btnDown.Command += new CommandEventHandler(Down_Command);

            _btnOK = new Button();
            _btnOK.ID = "btnOK";
            _btnOK.Text = this.OkCommandText;
            _btnOK.CommandName = "OK";
            _btnOK.Command += new CommandEventHandler(OK_Command);

            _imgBtnAdd = new ImageButton();
            _imgBtnAdd.ID = "imgBtnAdd";
            _imgBtnAdd.AlternateText = this.AddCommandText;
            _imgBtnAdd.ImageUrl = this.AddCommandImageUrl;
            _imgBtnAdd.CommandName = "Add";
            _imgBtnAdd.Command += new CommandEventHandler(Add_Command);

            _imgBtnDelete = new ImageButton();
            _imgBtnDelete.ID = "imgBtnDelete";
            _imgBtnDelete.AlternateText = this.DeleteCommandText;
            _imgBtnDelete.ImageUrl = this.DeleteCommandImageUrl;
            _imgBtnDelete.CommandName = "Delete";
            _imgBtnDelete.Command += new CommandEventHandler(Delete_Command);

            _imgBtnClear = new ImageButton();
            _imgBtnClear.ID = "imgBtnClear";
            _imgBtnClear.AlternateText = this.ClearCommandText;
            _imgBtnClear.ImageUrl = this.ClearCommandImageUrl;
            _imgBtnClear.CommandName = "Clear";
            _imgBtnClear.Command += new CommandEventHandler(Clear_Command);

            _imgBtnUp = new ImageButton();
            _imgBtnUp.ID = "imgBtnUp";
            _imgBtnUp.AlternateText = this.UpCommandText;
            _imgBtnUp.ImageUrl = this.UpCommandImageUrl;
            _imgBtnUp.CommandName = "Up";
            _imgBtnUp.Command += new CommandEventHandler(Up_Command);

            _imgBtnDown = new ImageButton();
            _imgBtnDown.ID = "imgBtnDown";
            _imgBtnDown.AlternateText = this.DownCommandText;
            _imgBtnDown.ImageUrl = this.DownCommandImageUrl;
            _imgBtnDown.CommandName = "Down";
            _imgBtnDown.Command += new CommandEventHandler(Down_Command);

            _imgBtnOK = new ImageButton();
            _imgBtnOK.ID = "imgBtnOK";
            _imgBtnOK.AlternateText = this.OkCommandText;
            _imgBtnOK.ImageUrl = this.OkCommandImageUrl;
            _imgBtnOK.CommandName = "OK";
            _imgBtnOK.Command += new CommandEventHandler(OK_Command);

            _lstBoxSetSortingFields = new ListBox();
            _lstBoxSetSortingFields.ID = "lstBoxSetSortingFields";
            _lstBoxSetSortingFields.Width = new Unit("100%");

            _rdoBtnLstSetSortingFields = new RadioButtonList();
            _rdoBtnLstSetSortingFields.ID = "rdoBtnLstSetSortingFields";
            _rdoBtnLstSetSortingFields.Width = new Unit("100%");

            this.Controls.Add(_ddlstSortingFields);
            this.Controls.Add(_rdoBtnLstSortDirection);

            this.Controls.Add(_btnAdd);
            this.Controls.Add(_btnDelete);
            this.Controls.Add(_btnClear);
            this.Controls.Add(_btnUp);
            this.Controls.Add(_btnDown);
            this.Controls.Add(_btnOK);

            this.Controls.Add(_imgBtnAdd);
            this.Controls.Add(_imgBtnDelete);
            this.Controls.Add(_imgBtnClear);
            this.Controls.Add(_imgBtnUp);
            this.Controls.Add(_imgBtnDown);
            this.Controls.Add(_imgBtnOK);

            this.Controls.Add(_lstBoxSetSortingFields);
            this.Controls.Add(_rdoBtnLstSetSortingFields);
        }

        void DisplayOrHide_Command(object sender, CommandEventArgs e)
        {
            this.Visible = !this.Visible;
            this.SetDisplayOrHideControlProperty();
        }
        
        void OK_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        void Down_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        void Up_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        void Delete_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        void Clear_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        void Add_Command(object sender, CommandEventArgs e)
        {
            this.DoCommand(e.CommandName);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.ApplyStyle();

            this.AddAttributesToRender(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _ddlstSortingFields.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _rdoBtnLstSortDirection.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnAdd.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnAdd.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();

            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            this.SetSortingFieldsStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.ListBox)
            {
                _lstBoxSetSortingFields.RenderControl(writer);
            }
            else if (this.SetSortingFieldsRenderMode == SortingSetSortingFieldsRenderMode.RadioButtonList)
            {
                _rdoBtnLstSetSortingFields.RenderControl(writer);
            }
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnUp.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnUp.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnDown.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnDown.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnDelete.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnDelete.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnClear.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnClear.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.CommandRenderMode == SortingCommandRenderMode.Button)
            {
                _btnOK.RenderControl(writer);
            }
            else if (this.CommandRenderMode == SortingCommandRenderMode.ImageButton)
            {
                _imgBtnOK.RenderControl(writer);
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}
