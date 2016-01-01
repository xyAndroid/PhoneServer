using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    [DefaultProperty("")]
    [ParseChildren(true, "Parameters")]
    [ToolboxData("<{0}:Attaching runat=server></{0}:Attaching>")]
    public class Attaching : CompositeControl
    {
        #region Fields

        /// <summary>
        /// 执行开关附件控件的外部控件。
        /// </summary>
        Control _displayOrHideCtrl;

        /// <summary>
        /// 获取上载附件的控件。
        /// </summary>
        FileUpload _fileUpload;

        /// <summary>
        /// 执行上载的控件。
        /// </summary>
        Button _btnUpload;

        /// <summary>
        /// 显示已上载附件的列表控件。
        /// </summary>
        Table _tblUploadedFiles;

        /// <summary>
        /// 获取控件附件上载到数据源中的外部控件ID。
        /// </summary>
        System.Web.UI.DataSourceControl _dsCtrl;

        #endregion

        #region Properties

        private string _displayOrHideControlID = string.Empty;
        /// <summary>
        /// 显示/隐藏附件控件的外部控件ID。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("行为")]
        [Description("显示/隐藏附件控件的外部控件ID。")]
        public string DisplayOrHideControlID
        {
            get { return _displayOrHideControlID; }
            set { _displayOrHideControlID = value; }
        }

        /// <summary>
        /// 显示/隐藏附件控件的外部控件的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("附件")]
        [Category("行为")]
        [Description("显示/隐藏附件控件的外部控件的显示文本。")]
        public string DisplayCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "DisplayCommandText", "附件");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "DisplayCommandText", value);
            }
        }

        /// <summary>
        /// 显示/隐藏附件控件的外部控件的隐藏文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("隐藏")]
        [Category("行为")]
        [Description("显示/隐藏附件控件的外部控件的隐藏文本。")]
        public string HideCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "HideCommandText", "隐藏");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "HideCommandText", value);
            }
        }

        /// <summary>
        /// 执行上载命令的控件文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("上载")]
        [Category("行为")]
        [Description("执行上载命令的控件文本。")]
        public string UploadCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UploadCommandText", "上载");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UploadCommandText", value);
            }
        }

        private ParameterCollection _params;
        /// <summary>
        /// 参数集合。
        /// </summary>
        [Category("数据")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("参数集合。")]
        public virtual ParameterCollection Parameters
        {
            get
            {
                if (_params == null)
                {
                    _params = new ParameterCollection();
                }
                return _params;
            }
        }


        private string _dataSourceID = string.Empty;
        /// <summary>
        /// 获取控件附件上载到数据源中的外部控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("获取控件附件上载到数据源中的外部控件ID。")]
        public string DataSourceID
        {
            get { return _dataSourceID; }
            set { _dataSourceID = value; }
        }

        #endregion

        #region Operates

        private void InitControl()
        {
            if (!this.DesignMode)
            {
                InitDisplayOrHideControl();
            }
        }

        private void InitDisplayOrHideControl()
        {
            if (!string.IsNullOrEmpty(_displayOrHideControlID))
            {
                _displayOrHideCtrl = Utility.FindControl(this, _displayOrHideControlID);
                if (_displayOrHideCtrl is Button)
                {
                    ((Button)_displayOrHideCtrl).UseSubmitBehavior = false;
                }
                if (_displayOrHideCtrl is IButtonControl)
                {
                    ((IButtonControl)_displayOrHideCtrl).Command += new CommandEventHandler(Attaching_Command);
                }
            }
        }

        private void InitDataSourceControl()
        {
            if (!string.IsNullOrEmpty(_dataSourceID))
            {
                _dsCtrl = (DataSourceControl)Utility.FindControl(this, _dataSourceID);
                if (_dsCtrl is SqlDataSource)
                {
                }
                else if (_dsCtrl is ObjectDataSource)
                {
                }
                else
                {
                    throw new Exception("目前附件控件只能支持 ObjectDataSource / SqlDataSource 控件作为数据源，或者请检查 DataSourceID 是否正确。");
                }
            }
        }

        private void ConstructCommandControls()
        {
            _btnUpload = new Button();
            _btnUpload.ID = "btnUpload";
            _btnUpload.UseSubmitBehavior = false;
            _btnUpload.Text = this.UploadCommandText;
            _btnUpload.Command += new CommandEventHandler(Attaching_Command);

            this.Controls.Add(_btnUpload);
        }

        private void ConstructUploadFilesTableControl()
        {
            _tblUploadedFiles = new Table();
            _tblUploadedFiles.ID = "tblUploadFiles";

            this.Controls.Add(_tblUploadedFiles);
        }

        void Attaching_Command(object sender, CommandEventArgs e)
        {
            WebControl aCtrl = (WebControl)sender;

            if (_btnUpload != null && aCtrl.UniqueID.Equals(_btnUpload.UniqueID))
            {
                this.DoUpload();
            }
        }

        private void DoUpload()
        {
            if (_dsCtrl != null)
            {
                if (_dsCtrl is SqlDataSource)
                {
                    SqlDataSource sqlDs = (SqlDataSource)_dsCtrl;

                }
                if (_dsCtrl is ObjectDataSource)
                {
                }
            }
        }

        public override void DataBind()
        {
            if (_dsCtrl != null)
            {
                System.Collections.IEnumerator ie = null;

                if (_dsCtrl is SqlDataSource)
                {
                    SqlDataSource sqlDs = (SqlDataSource)_dsCtrl;

                    ie = sqlDs.Select(new DataSourceSelectArguments()).GetEnumerator();
                }
                if (_dsCtrl is ObjectDataSource)
                {
                    ObjectDataSource objDs = (ObjectDataSource)_dsCtrl;

                    ie = objDs.Select().GetEnumerator();
                }

                if (ie != null && _tblUploadedFiles != null && _params != null)
                {
                    while (ie.MoveNext())
                    {
                        if (ie.Current is System.Data.DataRowView)
                        {
                            System.Data.DataRow aDr = ((System.Data.DataRowView)ie.Current).Row;

                            TableRow aTblRow = new TableRow();

                            foreach (Parameter aParam in _params)
                            {
                                TableCell aTblCell = new TableCell();

                                aTblCell.Text = aDr[aParam.Name].ToString();

                                aTblRow.Cells.Add(aTblCell);
                            }

                            _tblUploadedFiles.Rows.Add(aTblRow);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
