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
        /// ִ�п��ظ����ؼ����ⲿ�ؼ���
        /// </summary>
        Control _displayOrHideCtrl;

        /// <summary>
        /// ��ȡ���ظ����Ŀؼ���
        /// </summary>
        FileUpload _fileUpload;

        /// <summary>
        /// ִ�����صĿؼ���
        /// </summary>
        Button _btnUpload;

        /// <summary>
        /// ��ʾ�����ظ������б�ؼ���
        /// </summary>
        Table _tblUploadedFiles;

        /// <summary>
        /// ��ȡ�ؼ��������ص�����Դ�е��ⲿ�ؼ�ID��
        /// </summary>
        System.Web.UI.DataSourceControl _dsCtrl;

        #endregion

        #region Properties

        private string _displayOrHideControlID = string.Empty;
        /// <summary>
        /// ��ʾ/���ظ����ؼ����ⲿ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("��ʾ/���ظ����ؼ����ⲿ�ؼ�ID��")]
        public string DisplayOrHideControlID
        {
            get { return _displayOrHideControlID; }
            set { _displayOrHideControlID = value; }
        }

        /// <summary>
        /// ��ʾ/���ظ����ؼ����ⲿ�ؼ�����ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("����")]
        [Category("��Ϊ")]
        [Description("��ʾ/���ظ����ؼ����ⲿ�ؼ�����ʾ�ı���")]
        public string DisplayCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "DisplayCommandText", "����");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "DisplayCommandText", value);
            }
        }

        /// <summary>
        /// ��ʾ/���ظ����ؼ����ⲿ�ؼ��������ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("����")]
        [Category("��Ϊ")]
        [Description("��ʾ/���ظ����ؼ����ⲿ�ؼ��������ı���")]
        public string HideCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "HideCommandText", "����");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "HideCommandText", value);
            }
        }

        /// <summary>
        /// ִ����������Ŀؼ��ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("����")]
        [Category("��Ϊ")]
        [Description("ִ����������Ŀؼ��ı���")]
        public string UploadCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UploadCommandText", "����");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UploadCommandText", value);
            }
        }

        private ParameterCollection _params;
        /// <summary>
        /// �������ϡ�
        /// </summary>
        [Category("����")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("�������ϡ�")]
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
        /// ��ȡ�ؼ��������ص�����Դ�е��ⲿ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("��ȡ�ؼ��������ص�����Դ�е��ⲿ�ؼ�ID��")]
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
                    throw new Exception("Ŀǰ�����ؼ�ֻ��֧�� ObjectDataSource / SqlDataSource �ؼ���Ϊ����Դ���������� DataSourceID �Ƿ���ȷ��");
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
