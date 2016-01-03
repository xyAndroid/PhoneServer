using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// GridView��ѡ��ģʽ��
    /// </summary>
    public enum GridViewRowSelectMode
    {
        /// <summary>
        /// �����ڵ���ѡ��
        /// </summary>
        None,
        /// <summary>
        /// ��ѡģʽ��
        /// </summary>
        Single,
        /// <summary>
        /// ��ѡģʽ��
        /// </summary>
        Multiple
    }

    /// <summary>
    /// GridView��ѡ�������ı�֮���������¼�������
    /// </summary>
    public class GridViewRowSelectedIndexChangedEventArgs : EventArgs
    {
        GridView _gv;
        GridViewRow _gvRow;
        Control _ctrl;
        bool _selected;
        DataKey _dataKey;

        /// <summary>
        /// GridView ��ѡ��仯���������¼�������
        /// </summary>
        /// <param name="gv">GridView ����</param>
        /// <param name="gvRow">�����¼����ж���</param>
        /// <param name="ctrl">������ѡ��Ŀؼ���</param>
        /// <param name="selected">��ѡ��״̬��</param>
        /// <param name="dataKey">�����ݼ���</param>
        public GridViewRowSelectedIndexChangedEventArgs(
            GridView gv, GridViewRow gvRow,
            Control ctrl, bool selected,
            DataKey dataKey)
        {
            _gv = gv;
            _gvRow = gvRow;
            _ctrl = ctrl;
            _selected = selected;
            _dataKey = dataKey;
        }

        /// <summary>
        /// ��ȡ�����¼��� GridViewRow ���������ȫѡ��Ϊ null������Ƕ�ѡ��Ϊ���һ�������¼����С�
        /// </summary>
        public GridViewRow Row
        {
            get { return _gvRow; }
        }

        /// <summary>
        /// ��ȡ�����¼��Ŀؼ���
        /// </summary>
        public Control Control
        {
            get { return _ctrl; }
        }

        /// <summary>
        /// ��ȡһ��ֵ���ж������¼���ѡ��״̬��
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
        }

        /// <summary>
        /// ��ȡһ��ֵ������ǰ�е����ݼ�ֵ���������ȫѡ��Ϊ null������Ƕ�ѡ��Ϊ���һ�������¼����е����ݼ�ֵ����
        /// </summary>
        public DataKey DataKey
        {
            get { return _dataKey; }
        }
    }

    /// <summary>
    /// GridView��ѡ�������ı�֮���������¼�ί�С�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GridViewRowSelectedIndexChangedEventHandler(object sender, GridViewRowSelectedIndexChangedEventArgs e);

    /// <summary>
    /// GridView��չ�ؼ�����Ҫ���ڿ�����ѡ�񣬹���������Ŀ�ѡ���Ŀ��Ƶȡ�
    /// </summary>
    [Bindable(false)]
    [ParseChildren(true, "ControlStateSettings")]
    [NonVisualControl(true)]
    [DefaultProperty("ControlID")]
    [DefaultEvent("GridViewRowSelectedIndexChanged")]
    [System.Drawing.ToolboxBitmap(typeof(GridViewExtender), "GridViewExtender.bmp")]
    [ToolboxData("<{0}:GridViewExtender runat=server></{0}:GridViewExtender>")]
    public class GridViewExtender : CompositeControl
    {
        #region Fields

        private Style _selectedRowStyle;

        #endregion

        #region Controls

        GridView _gv;

        #endregion

        #region Properties

        private string _controlId = string.Empty;
        /// <summary>
        /// ��Ҫ���Ƶ�GridViewID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [Themeable(false)]
        [DefaultValue("")]
        [Category("����")]
        [Description("��Ҫ���Ƶ�GridViewID��")]
        public string ControlID
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private GridViewRowSelectMode _rowSelectMode = GridViewRowSelectMode.Multiple;
        /// <summary>
        /// GridView��ѡ��ģʽ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(GridViewRowSelectMode.Multiple)]
        [Category("��Ϊ")]
        [Description("GridView��ѡ��ģʽ��")]
        public GridViewRowSelectMode RowSelectMode
        {
            get { return _rowSelectMode; }
            set { _rowSelectMode = value; }
        }

        private const string m_SelectSingleRowControlIDViewStateName = "SelectSingleRowControlID";

        /// <summary>
        /// ��ѡ��ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("��ѡ��ؼ�ID��")]
        public string SelectSingleRowControlID
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_SelectSingleRowControlIDViewStateName, string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_SelectSingleRowControlIDViewStateName, value);
            }
        }

        private const string m_SelectAllRowsControlIDViewStateName = "SelectAllRowsControlID";
        /// <summary>
        /// ѡ�������еĿؼ�ID��
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("ѡ�������еĿؼ�ID��")]
        public string SelectAllRowsControlID
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_SelectAllRowsControlIDViewStateName, string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_SelectAllRowsControlIDViewStateName, value);
            }
        }

        private const string m_OrderNoControlIDViewStateName = "OrderNoControlID";
        /// <summary>
        /// ��ʾ�����ŵĿؼ�ID��
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("��ʾ�����ŵĿؼ�ID��")]
        public string OrderNoControlID
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_OrderNoControlIDViewStateName, string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_OrderNoControlIDViewStateName, value);
            }
        }

        private const string m_IsRowSelectedControlPostBackViewStateName = "IsRowSelectedControlPostBack";
        /// <summary>
        /// ��ѡ��ؼ��Ƿ�ִ�лط�������
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("��ѡ��ؼ��Ƿ�ִ�лط�������")]
        public bool IsRowSelectedControlPostBack
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsRowSelectedControlPostBackViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsRowSelectedControlPostBackViewStateName, value);
            }
        }

        private ControlStateSettingCollection _controlStateSettings;
        /// <summary>
        /// �ؼ�״̬���ü��ϡ�
        /// </summary>
        [Category("��Ϊ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(ControlStateSettingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("�ؼ�״̬���ü��ϡ�")]
        public ControlStateSettingCollection ControlStateSettings
        {
            get
            {
                if (_controlStateSettings == null)
                {
                    _controlStateSettings = new ControlStateSettingCollection();
                }
                return _controlStateSettings;
            }
        }

        /// <summary>
        /// ɸѡ����������ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("ѡ���е���ʽ��")]
        public virtual Style SelectedRowStyle
        {
            get
            {
                if (_selectedRowStyle == null)
                {
                    _selectedRowStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_selectedRowStyle).TrackViewState();
                    }
                }
                return _selectedRowStyle;
            }
        }

        /// <summary>
        /// ���ѡ�е��С�
        /// </summary>
        [Browsable(false)]
        public GridViewRow[] SelectedRows
        {
            get
            {
                List<GridViewRow> selectedRows = new List<GridViewRow>();
                if (_gv != null)
                {
                    foreach (GridViewRow aRow in _gv.Rows)
                    {
                        Control ctrl = aRow.FindControl(this.SelectSingleRowControlID);
                        if (ctrl != null)
                        {
                            if (Utility.GetControlPropertyValue<bool>(ctrl,
                                Constants.ControlProperties.CheckedPropertyName, false))
                            {
                                selectedRows.Add(aRow);
                            }
                        }
                    }
                }
                return selectedRows.ToArray();
            }
        }

        private DataKeyArray _selectedDataKeys;
        /// <summary>
        /// ���ѡ�е��е����ݼ�ֵ���ϡ�
        /// </summary>
        [Browsable(false)]
        public DataKeyArray SelectedDataKeys
        {
            get
            {
                if (_selectedDataKeys == null)
                {
                    ArrayList selectedDataKeys = new ArrayList();

                    if (_gv != null && _gv.DataKeys != null && _gv.DataKeys.Count > 0)
                    {
                        foreach (GridViewRow aRow in _gv.Rows)
                        {
                            Control ctrl = aRow.FindControl(this.SelectSingleRowControlID);
                            if (ctrl != null)
                            {
                                if (Utility.GetControlPropertyValue<bool>(ctrl,
                                    Constants.ControlProperties.CheckedPropertyName, false))
                                {
                                    if (aRow.RowIndex < _gv.DataKeys.Count)
                                    {
                                        selectedDataKeys.Add(_gv.DataKeys[aRow.RowIndex]);
                                    }
                                }
                            }
                        }
                    }
                    _selectedDataKeys = new DataKeyArray(selectedDataKeys);
                }
                return _selectedDataKeys;
            }
        }

        #endregion

        #region Operates

        private void InitControl()
        {
            if (!this.DesignMode)
            {
                InitGridViewControl();
            }
        }

        void Page_Load(object sender, EventArgs e)
        {
            this.InitControl();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.SetControlState();
        }

        private void InitGridViewControl()
        {
            if (string.IsNullOrEmpty(_controlId))
            {
                return;
            }

            Control ctrl = this.Page.FindControl(_controlId);

            if (ctrl == null)
            {
                ctrl = Utility.FindControlInNamingContainer(this, _controlId);
            }

            if (ctrl is GridView)
            {
                _gv = (GridView)ctrl;
                _gv.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
                _gv.DataBound += new EventHandler(GridView_DataBound);
            }
            else
            {
                throw new Exception("Ŀǰ GridViewExtender �ؼ�ֻ��֧�� GridView �ؼ����������� ControlID �Ƿ���ȷ��");
            }

        }

        void GridView_DataBound(object sender, EventArgs e)
        {

        }

        void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
            {
                //  ����ѡ��������ѡ��ؼ��Ƿ�ִ�лط���
                Control ctrl = e.Row.FindControl(this.SelectAllRowsControlID);
                if (ctrl != null)
                {
                    Utility.SetControlPropertyValue<bool>(ctrl,
                        Constants.ControlProperties.AutoPostBackPropertyName,
                        this.IsRowSelectedControlPostBack);
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  ���ñ������š�
                Control ctrl = e.Row.FindControl(this.OrderNoControlID);
                if (ctrl != null)
                {
                    int orderNo = _gv.PageIndex * _gv.PageSize + e.Row.RowIndex + 1;
                    Utility.SetControlPropertyValue<string>(ctrl,
                        Constants.ControlProperties.TextPropertyName, orderNo.ToString());
                }
                //  ���õ�һ��ѡ��ؼ��Ƿ�ִ�лط���
                ctrl = e.Row.FindControl(this.SelectSingleRowControlID);
                if (ctrl != null)
                {
                    Utility.SetControlPropertyValue<bool>(ctrl,
                        Constants.ControlProperties.AutoPostBackPropertyName,
                        this.IsRowSelectedControlPostBack);
                }
            }
        }

        /// <summary>
        /// ���ÿؼ�������״̬��
        /// </summary>
        private void SetControlState()
        {
            if (!this.DesignMode)
            {
                if (!this.IsRowSelectedControlPostBack)
                {
                    this.SetNonRowSelectedPostBackState();
                }
                else
                {
                    this.SetSelectAllRowControlState();
                    this.SetControlStateSettingsState();
                }
            }
        }

        /// <summary>
        /// ���õ� IsRowSelectedControlPostBack Ϊ false ʱ�����ѡ��ؼ���״̬��
        /// </summary>
        private void SetNonRowSelectedPostBackState()
        {
            if (_gv == null)
            {
                return;
            }

            WebControl ctrl = null;

            if (_gv.HeaderRow != null)
            {
                ctrl = (WebControl)_gv.HeaderRow.FindControl(this.SelectAllRowsControlID);
            }
            else if (_gv.FooterRow != null)
            {
                ctrl = (WebControl)_gv.FooterRow.FindControl(this.SelectAllRowsControlID);
            }

            if (ctrl != null)
            {
                if (this.RowSelectMode == GridViewRowSelectMode.Multiple)
                {
                    ctrl.Attributes.Add("onclick",
                        string.Format("__doCheckAllStates(__chkCtrlIdArr_{0}, __ctrlSettings_{1}, __LASTSELECTED_{2}, this.id, '{3}')",
                        this.UniqueID, this.UniqueID, this.UniqueID, this.RowSelectMode.ToString()));
                    ctrl.Visible = true;
                }
                else
                {
                    ctrl.Visible = false;
                }
            }
            else
            {
                return;
            }

            string chkCtrlAllId = ctrl.ClientID;

            string chkCtrlIds = "var __chkCtrlIdArr_" + this.UniqueID + " = new Array(";

            foreach (GridViewRow aRow in _gv.Rows)
            {
                ctrl = (WebControl)aRow.FindControl(this.SelectSingleRowControlID);

                if (ctrl != null)
                {
                    chkCtrlIds += string.Format("\"{0}\",", ctrl.ClientID);
                    ctrl.Attributes.Add("onclick",
                        string.Format("__doCheckSingleState(__chkCtrlIdArr_{0}, __ctrlSettings_{1}, __LASTSELECTED_{2}, this.id, '{3}')",
                        this.UniqueID, this.UniqueID, this.UniqueID, this.RowSelectMode.ToString()));
                }
            }

            chkCtrlIds = chkCtrlIds.TrimEnd(',');
            chkCtrlIds += ");";

            string ctrlSettings = string.Empty;
            string ctrlSettingsArr = "var __ctrlSettings_" + this.UniqueID + " = new Array(";
            int iCtrlCount = 0;
            foreach (ControlStateSetting aCtrlStateSet in this.ControlStateSettings)
            {
                ctrl = (WebControl)this.Page.FindControl(aCtrlStateSet.ControlID);
                if (ctrl == null)
                {
                    ctrl = (WebControl)Utility.FindControlInNamingContainer(this, aCtrlStateSet.ControlID);
                }
                if (ctrl != null)
                {
                    ctrlSettings += string.Format(
                        @"
                        var __ctrlSetting_{0}{1} = new Object(); 
                        __ctrlSetting_{0}{1}.ControlID = {2};
                        __ctrlSetting_{0}{1}.ControlState = {3};
                        __ctrlSetting_{0}{1}.ControlStateForTrueCriteria = {4};", this.UniqueID, iCtrlCount,
                                                      "\"" + ctrl.ClientID + "\"",
                                                      "\"" + aCtrlStateSet.ControlState.ToString() + "\"",
                                                      "\"" + aCtrlStateSet.ControlStateForTrueCriteria.ToString() + "\"");

                    ctrlSettingsArr += string.Format(
                        @"__ctrlSetting_{0}{1},", this.UniqueID, iCtrlCount);

                    iCtrlCount++;
                }
            }
            ctrlSettingsArr = ctrlSettingsArr.Trim(',');
            ctrlSettingsArr += ");";
            ctrlSettings += ctrlSettingsArr;

            this.Page.ClientScript.RegisterHiddenField("__LASTSELECTED_" + this.UniqueID, string.Empty);

            string arrScript =
                @"
                    <script type=""text/javascript"">
                    <!--
                    " +
                chkCtrlIds + ctrlSettings +
                @"
                    // -->
                    </script>
                ";

            string funcScript =
                @"
                    <script type=""text/javascript"">
                    <!--
                        function __getSelectedRowsCount(chkCtrlIdArr)
                        {
                            var iCount = 0;
                            for(var i = 0; i < chkCtrlIdArr.length; i++)
                            {
                                var ctrl = document.getElementById(chkCtrlIdArr[i]);
                                if(ctrl == null) continue;
                                if(ctrl.checked)    iCount++;
                            }
                            return iCount;
                        }
                        function __doCheckSingleState(chkCtrlIdArr, ctrlSettings, lastSelectedId, id, rowSelectMode)
                        {
                            var lastSelected = lastSelectedId;
                            if(lastSelected == null) return;
                            lastSelected.value = id;
                            if(rowSelectMode == """ + GridViewRowSelectMode.Single.ToString() + @""")
                            {
                                for(var i = 0; i < chkCtrlIdArr.length; i++)
                                {
                                    var chkCtrl = document.getElementById(chkCtrlIdArr[i]);
                                    if(chkCtrl == null) continue;
                                    if(chkCtrl.id == id)
                                    {
                                        if(document.getElementById(id).checked)
                                        {
                                            chkCtrl.checked = true;
                                        }
                                    }
                                    else
                                    {
                                        chkCtrl.checked = false;
                                    }
                                }
                            }
                            var chkCtrlAll = document.getElementById(""" + chkCtrlAllId + @""");
                            if(chkCtrlAll)
                                chkCtrlAll.checked = (__getSelectedRowsCount(chkCtrlIdArr) == chkCtrlIdArr.length);
                            __doControlStateSettingsState(chkCtrlIdArr, ctrlSettings);
                        }
                        function __linkClick()
                        { 
                            return false;  
                        }
                        function __doCheckAllStates(chkCtrlIdArr, ctrlSettings, lastSelectedId, id, rowSelectMode)
                        {
                            var lastSelected = lastSelectedId;
                            if(lastSelected == null) return;
                            lastSelected.value = id;
                            var allChkCtrl = document.getElementById(id);
                            if(allChkCtrl == null) return;
                            for(var i = 0; i < chkCtrlIdArr.length; i++)
                            {
                                var chkCtrl = document.getElementById(chkCtrlIdArr[i]);
                                if(chkCtrl == null) continue;
                                chkCtrl.checked = allChkCtrl.checked;
                            }
                            __doControlStateSettingsState(chkCtrlIdArr, ctrlSettings);
                        }
                        function __doCheckStateSettingsState(chkCtrlIdArr)
                        {
                            for(var i = 0; i < chkCtrlIdArr.length; i++)
                            {
                                var chkCtrl = document.getElementById(chkCtrlIdArr[i]);
                                if(chkCtrl == null) continue;
                                var sender = chkCtrl;
                                while(sender && sender.tagName && sender.tagName.toLowerCase()!=""tr"")
                                {
                                    if(!sender.parentNode)
                                    {
                                        sender = null;
                                    }
                                    sender = sender.parentNode;
                                }
                                if(sender != null)
                                {
                                    if(chkCtrl.checked)
                                        sender.className = """ + this.SelectedRowStyle.CssClass + @""";
                                    else
                                        sender.className = """";
                                }
                            }                        
                        }
                        function __doControlStateSettingsState(chkCtrlIdArr, ctrlSettings)
                        {
                            __doCheckStateSettingsState(chkCtrlIdArr);
                            var selectedRowCount = __getSelectedRowsCount(chkCtrlIdArr);
                            for(var i = 0; i < ctrlSettings.length; i++)
                            {
                                var ctrlSetting = ctrlSettings[i];
                                var ctrl = document.getElementById(ctrlSetting.ControlID);
                                if(ctrl == null) continue;
                                if(selectedRowCount <= 0)
                                {
                                    ctrl.disabled = true;
                                }
                                if(selectedRowCount == 1)
                                {
                                    ctrl.disabled = false;
                                }
                                if(selectedRowCount > 1)
                                {
                                    if(ctrlSetting.ControlStateForTrueCriteria == """ + ControlStateForTrueCriteria.OnlyOneRowSelected.ToString() + @""")
                                    {
                                        ctrl.disabled = true;
                                    }
                                    else
                                    {
                                        ctrl.disabled = false;
                                    }
                                }
                                ctrl.detachEvent(""onclick"", __linkClick);
                                if(ctrl.disabled){
                                    ctrl.attachEvent(""onclick"", __linkClick);
                                }
                            }
                        }
                    // -->
                    </script>
                    ";

            string startScript =
                @"
                    <script type=""text/javascript"">
                    <!--
                        __doControlStateSettingsState(__chkCtrlIdArr_" + this.UniqueID + @", __ctrlSettings_" + this.UniqueID + @");
                    // -->
                    </script>
                ";

            ScriptManager scriptMgr = ScriptManager.GetCurrent(this.Page);

            if (scriptMgr != null)
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "__gridviewClientScript_arr_" + this.UniqueID, arrScript, false);
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "__gridviewClientScript", funcScript, false);
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "__gridviewClientScript_start_" + this.UniqueID, startScript, false);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "__gridviewClientScript_arr_" + this.UniqueID, arrScript, false);
                this.Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "__gridviewClientScript", funcScript, false);
                this.Page.ClientScript.RegisterStartupScript(
                    this.GetType(), "__gridviewClientScript_start_" + this.UniqueID, startScript, false);
            }
        }

        /// <summary>
        /// ���õ� IsRowSelectedControlPostBack Ϊ true ѡ�������еĿؼ���״̬��
        /// </summary>
        private void SetSelectAllRowControlState()
        {
            if (_gv == null)
            {
                return;
            }

            Control ctrl = null;

            if (_gv.HeaderRow != null)
            {
                ctrl = _gv.HeaderRow.FindControl(this.SelectAllRowsControlID);
            }
            else if (_gv.FooterRow != null)
            {
                ctrl = _gv.FooterRow.FindControl(this.SelectAllRowsControlID);
            }

            if (ctrl != null && this.RowSelectMode == GridViewRowSelectMode.Single)
            {
                ctrl.Visible = false;
            }
        }

        /// <summary>
        /// ���õ� IsRowSelectedControlPostBack Ϊ true ��Ҫ���Ƶ��ⲿ�ؼ�״̬���á�
        /// </summary>
        private void SetControlStateSettingsState()
        {
            if (_controlStateSettings == null)
            {
                return;
            }

            int selectedRowCount = this.SelectedRows.Length;

            foreach (ControlStateSetting aCtrlStateSet in _controlStateSettings)
            {
                if (string.IsNullOrEmpty(aCtrlStateSet.ControlID))
                {
                    continue;
                }
                Control ctrl = this.Page.FindControl(aCtrlStateSet.ControlID);
                if (ctrl == null)
                {
                    ctrl = Utility.FindControlInNamingContainer(this, aCtrlStateSet.ControlID);
                }
                if (ctrl != null)
                {
                    if (selectedRowCount <= 0)
                    {
                        if (aCtrlStateSet.ControlStateForTrueCriteria == ControlStateForTrueCriteria.OnlyOneRowSelected)
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), false);
                        }
                        else
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), false);
                        }
                    }
                    else if (selectedRowCount == 1)
                    {
                        if (aCtrlStateSet.ControlStateForTrueCriteria == ControlStateForTrueCriteria.OnlyOneRowSelected)
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), true);
                        }
                        else
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), true);
                        }
                    }
                    else
                    {
                        if (aCtrlStateSet.ControlStateForTrueCriteria == ControlStateForTrueCriteria.OnlyOneRowSelected)
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), false);
                        }
                        else
                        {
                            Utility.SetControlPropertyValue<bool>(ctrl,
                                aCtrlStateSet.ControlState.ToString(), true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ִ�е���ѡ��ؼ��Ļط��¼���
        /// </summary>
        private void DoSelectSingleRow()
        {
            if (_gv == null)
            {
                return;
            }

            //  ���������ִ�ж�����Ŀ��ؼ�UniqueID��
            string __eventTarget = Utility.GetRequestParameterValue(this.Page,
                Constants.RequestParameter.EventTarget);

            if (string.IsNullOrEmpty(__eventTarget))
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.SelectSingleRowControlID))
            {
                foreach (GridViewRow aRow in _gv.Rows)
                {
                    Control ctrl = aRow.FindControl(this.SelectSingleRowControlID);
                    if (ctrl != null)
                    {

                        //  ���Ϊ��ǰ���С�
                        if (__eventTarget.Equals(ctrl.UniqueID))
                        {
                            //  ��������ѡ��ؼ�����ѡ�������ı��¼���
                            GridViewRowSelectedIndexChangedEventArgs arg =
                                new GridViewRowSelectedIndexChangedEventArgs(_gv, aRow, ctrl,
                                Utility.GetControlPropertyValue<bool>(ctrl,
                                Constants.ControlProperties.CheckedPropertyName, false),
                                aRow.RowIndex < _gv.DataKeys.Count ? _gv.DataKeys[aRow.RowIndex] : null);
                            this.OnGridViewRowSelectedIndexChanged(arg);
                        }
                        else
                        {
                            if (this.RowSelectMode == GridViewRowSelectMode.Single)
                            {
                                //  ���÷ǵ�ǰ��Ϊ��ѡ��״̬��
                                Utility.SetControlPropertyValue<bool>(ctrl,
                                    Constants.ControlProperties.CheckedPropertyName, false);

                                //  ����������ʽ��
                                aRow.ControlStyle.Reset();
                            }
                        }

                        //  ���ݵ�ǰ�е�ѡ��״̬����ѡ����ʽ��ȡ��ѡ����ʽ��
                        if (_selectedRowStyle != null)
                        {
                            if (Utility.GetControlPropertyValue<bool>(ctrl,
                                Constants.ControlProperties.CheckedPropertyName, false))
                            {
                                aRow.ApplyStyle(_selectedRowStyle);
                            }
                            else
                            {
                                aRow.ControlStyle.Reset();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ִ�ж���ѡ��ؼ��Ļط��¼���
        /// </summary>
        private void DoSelectAllRows()
        {
            if (_gv == null)
            {
                return;
            }

            Control ctrl = null;

            if (_gv.HeaderRow != null)
            {
                ctrl = _gv.HeaderRow.FindControl(this.SelectAllRowsControlID);
            }
            else if (_gv.FooterRow != null)
            {
                ctrl = _gv.FooterRow.FindControl(this.SelectAllRowsControlID);
            }

            if (ctrl != null)
            {
                //  �����ǰ�Ƕ���ѡ��ģʽ������Ҫ���ݶ���ѡ��ؼ��� Checked ״̬ȥ�ı䵥��ѡ��ؼ��� Checked ״̬��
                if (this.RowSelectMode == GridViewRowSelectMode.Multiple)
                {
                    foreach (GridViewRow aRow in _gv.Rows)
                    {
                        Control ctrlSelectSingleRow = aRow.FindControl(this.SelectSingleRowControlID);

                        if (ctrlSelectSingleRow != null)
                        {
                            Utility.SetControlPropertyValue<bool>(ctrlSelectSingleRow,
                                Constants.ControlProperties.CheckedPropertyName,
                                Utility.GetControlPropertyValue<bool>(ctrl, Constants.ControlProperties.CheckedPropertyName, false));

                            //  �����ǰ�����¼��ĵ���ѡ��ؼ��� Checked ״̬Ϊ true����Ӧ����ѡ����ʽ������ȥ����ʽ��
                            if (_selectedRowStyle != null)
                            {
                                if (Utility.GetControlPropertyValue<bool>(ctrlSelectSingleRow,
                                    Constants.ControlProperties.CheckedPropertyName, false))
                                {
                                    aRow.ApplyStyle(_selectedRowStyle);
                                }
                                else
                                {
                                    aRow.ControlStyle.Reset();
                                }
                            }
                        }
                    }
                }
                //  ����������ѡ�е��¼���
                GridViewRowSelectedIndexChangedEventArgs arg =
                    new GridViewRowSelectedIndexChangedEventArgs(_gv, null, ctrl,
                    Utility.GetControlPropertyValue<bool>(ctrl,
                    Constants.ControlProperties.CheckedPropertyName, false),
                    null);
                this.OnGridViewRowSelectedIndexChanged(arg);
            }
        }

        /// <summary>
        /// ִ����ѡ��仯���¼�����ִ�з���ֻ�� IsRowSelectControlPostBack Ϊ true ��ʱ��ִ�С�
        /// </summary>
        private void DoGridViewRowSelectedIndexChanged()
        {
            if (_gv == null || string.IsNullOrEmpty(this.Page.Request.Params["__LASTSELECTED_" + this.UniqueID]))
            {
                return;
            }

            Control ctrl = null;

            if (_gv.HeaderRow != null)
            {
                ctrl = _gv.HeaderRow.FindControl(this.SelectAllRowsControlID);
            }
            else if (_gv.FooterRow != null)
            {
                ctrl = _gv.FooterRow.FindControl(this.SelectAllRowsControlID);
            }

            if (ctrl != null)
            {
                if (this.Page.Request.Params["__LASTSELECTED_" + this.UniqueID] == ctrl.UniqueID ||
                    this.Page.Request.Params["__LASTSELECTED_" + this.UniqueID] == ctrl.ClientID)
                {
                    //  ����������ѡ�е��¼���
                    GridViewRowSelectedIndexChangedEventArgs arg =
                        new GridViewRowSelectedIndexChangedEventArgs(_gv, null, ctrl,
                        Utility.GetControlPropertyValue<bool>(ctrl,
                        Constants.ControlProperties.CheckedPropertyName, false),
                        null);
                    this.OnGridViewRowSelectedIndexChanged(arg);

                    //  �������֮�󽫲�����������ѡ��ʵ��
                    return;
                }
            }

            foreach (GridViewRow aRow in _gv.Rows)
            {
                ctrl = aRow.FindControl(this.SelectSingleRowControlID);

                if (ctrl != null)
                {
                    if (this.Page.Request.Params["__LASTSELECTED_" + this.UniqueID] == ctrl.UniqueID ||
                        this.Page.Request.Params["__LASTSELECTED_" + this.UniqueID] == ctrl.ClientID)
                    {
                        //  ��������ѡ��ؼ�����ѡ�������ı��¼���
                        GridViewRowSelectedIndexChangedEventArgs arg =
                            new GridViewRowSelectedIndexChangedEventArgs(_gv, aRow, ctrl,
                            Utility.GetControlPropertyValue<bool>(ctrl,
                            Constants.ControlProperties.CheckedPropertyName, false),
                            aRow.RowIndex < _gv.DataKeys.Count ? _gv.DataKeys[aRow.RowIndex] : null);
                        this.OnGridViewRowSelectedIndexChanged(arg);
                    }
                }
            }
        }

        private void DoEvents()
        {
            if (this.IsRowSelectedControlPostBack)
            {
                string __eventTarget = Utility.GetRequestParameterValue(this.Page, Constants.RequestParameter.EventTarget);
                if (string.IsNullOrEmpty(__eventTarget))
                {
                    return;
                }

                if (_gv != null && __eventTarget.Contains(_gv.ID) && __eventTarget.EndsWith(this.SelectSingleRowControlID))
                {
                    DoSelectSingleRow();
                }
                if (_gv != null && __eventTarget.Contains(_gv.ID) && __eventTarget.EndsWith(this.SelectAllRowsControlID))
                {
                    DoSelectAllRows();
                }
            }
            else
            {
                DoGridViewRowSelectedIndexChanged();
            }
        }

        #endregion

        #region Events

        private static readonly object EventGridViewRowSelectedIndexChangedKey = new object();

        /// <summary>
        /// GridView��ѡ�������ı�֮������
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [Description("GridView��ѡ�������ı�֮������")]
        public event GridViewRowSelectedIndexChangedEventHandler GridViewRowSelectedIndexChanged
        {
            add
            {
                Events.AddHandler(EventGridViewRowSelectedIndexChangedKey, value);
            }
            remove
            {
                Events.RemoveHandler(EventGridViewRowSelectedIndexChangedKey, value);
            }
        }

        /// <summary>
        /// GridView��ѡ�������ı�֮������
        /// </summary>
        protected virtual void OnGridViewRowSelectedIndexChanged(GridViewRowSelectedIndexChangedEventArgs e)
        {
            GridViewRowSelectedIndexChangedEventHandler handler =
                (GridViewRowSelectedIndexChangedEventHandler)
                Events[EventGridViewRowSelectedIndexChangedKey];

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        protected override object SaveControlState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveControlState());

            list.Add(_controlId);
            list.Add(_rowSelectMode);

            return list;
        }

        protected override void LoadControlState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)savedState;

                if (list.Count >= 3)
                {
                    base.LoadControlState(list[0]);

                    _controlId = (string)list[1];
                    _rowSelectMode = (GridViewRowSelectMode)list[2];
                }
            }
        }

        protected override object SaveViewState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveViewState());

            if (_selectedRowStyle != null)
            {
                list.Add(
                    ((IStateManager)_selectedRowStyle).SaveViewState());
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

                if (list.Count >= 2)
                {
                    base.LoadViewState(list[0]);

                    if (list[1] != null)
                    {
                        ((IStateManager)SelectedRowStyle).LoadViewState(list[1]);
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
            if (_selectedRowStyle != null)
            {
                ((IStateManager)_selectedRowStyle).TrackViewState();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Load += new EventHandler(Page_Load);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoEvents();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                Utility.RenderUnUIControlDesign(this, writer);
            }
        }
    }
}
