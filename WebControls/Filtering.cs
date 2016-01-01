using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// ɸѡ�ؼ���
    /// </summary>
    /// <remarks>
    /// ɸѡ�ؼ�Ҫ�ܹ�����ִ����Ҫ�������µ����ù�����
    /// ��1����һ�������ɸѡ���̵�ִ���� DataSet �� FilterExpression ������ɡ�
    ///     a)  ���� ObjectDataSource / SqlDataSource������ͨ���ù���һ����
    ///     b)  ���� GridView������ͨ���ù���һ����
    ///     c)  �������ԴΪ SqlDataSource���� GridView ��ҪΪÿ�� Column ���á�AccessibleHeaderText�����ԣ�
    ///         ���磺��AccessibleHeaderText������Ϊ��StudentNo,N������ʾ�� Column �ṩ��ɸѡ���ʽ������Ϊ�����ͣ�����μ��ڶ��������
    /// ��2���ڶ��������ɸѡ���̵�ִ���ɺ�̨ SQL ������ɡ�
    ///     a)  ���� ObjectDataSource������ EnablePaging������ȷ���á�SelectCountMethod������StartRowIndexParameterName������MaximumRowsParameterName���������ԣ�
    ///     b)  ���� GridView������ Paging����Ϊÿ�� Column ��ȷ���á�AccessibleHeaderText������HeaderText���������ԣ�
    ///         ���С�AccessibleHeaderText��Ϊɸѡ���HeaderText��Ϊɸѡ����ɸѡ�ؼ���ɸѡ���б�����ʾ���Ѻ��ı���
    ///         ���ڡ�AccessibleHeaderText���������ֹ��ɣ�ɸѡ����ʽ�ͱ��ʽ���ͣ�
    ///         ���ʽ����ָ��S������N������D�����֣��ֱ����ɸѡ����ʽΪ���ı��͡����������͡���������ʱ���͡������С�S������ʡ�ԣ�
    ///         ���磺��Student.Name,S����
    ///         һ��ɸѡ����ʽΪ SQL �ֶα��ʽ���� SQL �� From �Ӿ�������ű��ֶ�ǰһ��Ҫ���ϱ���ǰ׺��
    ///     c)  ���� Filtering���������ԡ�FilterParameterName��Ϊ����Դ�ؼ����õ� SelectMethod �еĿ��Խ��ܵ�ɸѡ���ʽ������
    ///         ���磺SelectMethod ��ԭ�� ��public DSStudent.StudentDataTable GetStudents(string filterExp, string sortExp, int? startRowIndex, int? maximumRows)����
    ///         ���еġ�filterExp��Ϊ��FilterParameterName�������Ǹò����Ƿ����ִ��ɸѡ���̣������� SelectMethod �����߾����ġ�
    /// ��3�������������ɸѡ���̵�ִ���� Linq ��ɡ�
    ///     a)  ���� LinqDataSource������ͨ���ù���һ����
    ///     b)  ���� GridView������ Paging����Ϊÿ�� Column ��ȷ���á�AccessibleHeaderText������HeaderText���������ԣ�
    ///         ���С�AccessibleHeaderText��Ϊɸѡ���HeaderText��Ϊɸѡ����ɸѡ�ؼ���ɸѡ���б�����ʾ���Ѻ��ı���
    ///         ���ڡ�AccessibleHeaderText���������ֹ��ɣ�ɸѡ����ʽ�ͱ��ʽ���ͣ�
    ///         ���ʽ����ָ��S������N������D�����֣��ֱ����ɸѡ����ʽΪ���ı��͡����������͡���������ʱ���͡������С�S������ʡ�ԣ�
    ///         ���磺��Student.Name{0}{1},S������Students.Count(Name{0}{1}) > 0��
    ///         һ��ɸѡ����ʽΪ Linq where �Ӿ�֧�ֵ���ͨ���Ա��ʽ��������ҪΪɸѡֵ��������ռλ������һ����ʾɸѡ�Ƚ���������ڶ�����ʾɸѡֵ��
    /// ��4����������ӳ�䡣
    ///     ����ӳ����ָ������Դ����һЩ�ֶ�����һЩ�����ʾ���磺AuditFlag �ֶ��á�0���͡�1�������ܾ����롰ͨ������
    ///     �����û�������ɸѡ������ɸѡֵ��ʱ�򲢲�֪�����������ʾ������ֱ����������Ѻ����ݵ��ı����磺���ܾ����롰ͨ������
    ///     ��ʱ��ɸѡ�ؼ��ṩ���Ѻ��ı��ʹ���֮����ж�Ӧת�������������û�������Ѻ��ı��������Ӧ�������ϣ��磺��0���͡�1����
    ///     ����ӳ������ã�
    ///     a)  ���� PropertyMappingDataSourceID ���ԣ�ָ��һ��Ϊ����ӳ��ת��������Դ�ؼ�ID��
    ///     b)  ���� PropertyMappings �������ԣ�ָ����Ҫִ������ӳ��ת�����ֶΣ�ͨ��ָ��ÿ������ӳ��ġ�PropertyName���͡�FilterItem�����Խ�ת����Ӧ������
    ///         ��PropertyName����ָ������ӳ������Դ�����Ե����ƣ��磺��AuditFlag����
    ///         ��FilterItem����ָ��ɸѡ���б�����Ҫ��������ӳ��ת����ɸѡ����ɸѡִ������Ϊ��̨ SQL ������Ϊ AccessibleHeaderText �е�ɸѡ���ʽ��
    ///         ��������ɸѡ���ʽ���ͣ��磺��AccessibleHeaderText��Ϊ��Audit.AuditFlag,N������FilterItem��Ӧ���óɡ�Audit.AuditFlag����
    ///     c)  ���� PropertyMappingSetting �������ԣ�ָ��������ӳ������Դ�����������������ӳ�����ݵ��ֶ����ƣ�
    ///         ��RefPropertyName��ָ������ӳ������Դ������ӳ������
    ///         ��RefPropertyValue��ָ������ӳ������Դ������ӳ��ֵ��
    ///         ��RefPropertyMeaning��ָ������ӳ������Դ�����Զ�Ӧ������Ѻ��ı���
    /// </remarks>
    [DefaultProperty("ControlID")]
    [ParseChildren(true, "PropertyMappings")]
    [System.Drawing.ToolboxBitmap(typeof(Filtering), "Filtering.bmp")]
    [ToolboxData("<{0}:Filtering runat=server></{0}:Filtering>")]
    public class Filtering : CompositeControl
    {
        #region Fields
        /// <summary>
        /// ��־���� (��ȡ��̬������Դ)
        /// </summary>
        private DataView _dv;

        /// <summary>
        /// ��Ҫɸѡ���ݵĿؼ���Ŀǰֻ֧�� GridView��
        /// </summary>
        private GridView _filterCtrl;
        /// <summary>
        /// ��Ϊɸѡ������Դ������Դ�ؼ���Ŀǰֻ֧�� ObjectDataSource��
        /// </summary>
        private DataSourceControl _dsCtrl;
        /// <summary>
        /// ��ʾ��νṹ���ݵ�����Դ�ؼ�����������ִ������ӳ��ɸѡ�ġ�
        /// </summary>
        private DataSourceControl _propertyMappingDsCtrl;
        /// <summary>
        /// ��õ�����ӳ������
        /// </summary>
        private IEnumerable _propertyMappingResult;
        /// <summary>
        /// ɸѡ��ļ��ϣ�ÿ��ɸѡ����һ�� ListItem ����
        /// ���� ListItem.Text ���Լ�¼��ʾ��ɸѡ���б��е��Ѻ��ı���һ���������� GridView ��ÿ�� Column �� HeaderText �е��ı���
        /// ���� ListItem.Value ���Լ�¼���¼��ɸѡ��Ĺ����ı���
        ///     ��1���������Դ�ؼ������˷�ҳ SQL ������������������� GridView ��ÿ�� Column �� AccessibleHeaderText �е��ı���
        ///          �����������ֹ��ɣ���ʾɸѡ��ı��ʽ�ı������ʽ���������ͣ���Ϊ��S������N������D�� ���֡�
        ///          "S"��ʾ���Ϊ�ı����ͣ�����ʡ�ԣ�
        ///          "N"��ʾ���Ϊ�������ͣ�
        ///          "D"��ʾ���Ϊ�������͡�
        ///          Sample: 
        ///             ListItem("����", "Student.Name,S")��������������Ϊ�Ѻ��ı���ʾ��ɸѡ���б��У���������Ϊɸѡ������������ı���
        ///     ��2���������Դ�ؼ�û�����÷�ҳ SQL ������ɸѡ���̷����� DataSet �С�
        /// </summary>
        private System.Collections.Generic.List<ListItem> _filterItems;

        /// <summary>
        /// ����ɸѡ�ؼ���ʾ/���ص��ⲿ�ؼ���
        /// </summary>
        private Control _displayOrHideCtrl;

        /// <summary>
        /// ִ�в�ѯ���ⲿ�ؼ���
        /// </summary>
        private Control _doQueryingCtrl;

        private Style _commandStyle;
        private Style _criteriaTableStyle;
        private Style _filterColumnsStyle;

        #endregion

        #region Controls

        Table _tblCriteria;

        Button _btnFilter;
        Button _btnReset;
        Button _btnAddCriteria;
        //Button _btnInsertCriteria;
        Button _btnDeleteCriteria;
        Button _btnDisplayFilterColumns;

        Panel _pnlFilterColumns;

        CheckBoxList _chkBoxLstFilterColumns;
        Button _btnFilterColumns;
        Button _btnCancelFilterColumns;

        #endregion

        #region Properties

        private string _lblbasicuserid = string.Empty;
        /// <summary>
        /// ���ܲ���ֵ_BID�Ŀؼ�ID
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("���ý���BasicUser����ID������Label��ID��")]
        public string LableBasicUserID
        {
            get { return _lblbasicuserid; }
            set { _lblbasicuserid = value; }
        }

        private string _lbluserentityid = string.Empty;
        /// <summary>
        /// ���ܲ���_UEID�Ŀؼ�ID
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("���ý���User_Entity����ID�Ĳ�����Label��ID")]
        public string LabelUserEntityID
        {
            get { return _lbluserentityid; }
            set { _lbluserentityid = value; }
        }


        private string _controlID = string.Empty;
        /// <summary>
        /// ����ִ��ɸѡ�Ŀؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("����ִ��ɸѡ�Ŀؼ�ID��")]
        public string ControlID
        {
            get { return _controlID; }
            set { _controlID = value; }
        }

        private string _dataSourceID = string.Empty;
        /// <summary>
        /// ����ɸѡ������Դ������Դ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("����ɸѡ������Դ������Դ�ؼ�ID��")]
        public string DataSourceID
        {
            get { return _dataSourceID; }
            set { _dataSourceID = value; }
        }

        private string _propertyMappingDataSourceID = string.Empty;
        /// <summary>
        /// ���ڻ������ӳ�������Դ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("����")]
        [Description("���ڻ������ӳ�������Դ�ؼ�ID��")]
        public string PropertyMappingDataSourceID
        {
            get { return _propertyMappingDataSourceID; }
            set { _propertyMappingDataSourceID = value; }
        }

        private string _displayOrHideControlId = string.Empty;
        /// <summary>
        /// ��ʾ����������ؼ����ⲿ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("��ʾ������ɸѡ�ؼ����ⲿ�ؼ�ID��")]
        public string DisplayOrHideControlID
        {
            get { return _displayOrHideControlId; }
            set { _displayOrHideControlId = value; }
        }

        private string _doQueryingControlId = string.Empty;
        /// <summary>
        /// ִ�в�ѯ���ⲿ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("��Ϊ")]
        [Description("ִ�в�ѯ���ⲿ�ؼ�ID��")]
        public string DoQueryingControlID
        {
            get { return _doQueryingControlId; }
            set { _doQueryingControlId = value; }
        }

        private const string m_DoQueryingControlTextViewStateName = "DoQueryingControlText";
        /// <summary>
        /// ִ�в�ѯ���ⲿ�ؼ���ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("��ѯ")]
        [Category("���")]
        [Description("ִ�в�ѯ���ⲿ�ؼ���ʾ�ı���")]
        public string DoQueryingControlText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DoQueryingControlTextViewStateName, "��ѯ");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DoQueryingControlTextViewStateName, value);
            }
        }

        private const string m_IsHidingDoQueryingControlViewStateName = "IsHidingDoQueryingControl";
        /// <summary>
        /// ������ɸѡ�ؼ���ʱ���Ƿ�����ִ�в�ѯ���ⲿ�ؼ���
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("������ɸѡ�ؼ���ʱ���Ƿ�����ִ�в�ѯ���ⲿ�ؼ���")]
        public bool IsHidingDoQueryingControl
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsHidingDoQueryingControlViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_IsHidingDoQueryingControlViewStateName, value);
            }
        }

        private const string m_FilterCommandTextViewStateName = "FilterCommandText";
        /// <summary>
        /// ɸѡ�������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("��ѯ")]
        [Category("����")]
        [Description("ɸѡ�������ʾ�ı���")]
        public string FilterCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterCommandTextViewStateName, "��ѯ");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterCommandTextViewStateName, value);
            }
        }

        private const string m_ResetCommandTextViewStateName = "ResetCommandText";
        /// <summary>
        /// �����������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("����")]
        [Category("����")]
        [Description("�����������ʾ�ı���")]
        public string ResetCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_ResetCommandTextViewStateName, "����");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_ResetCommandTextViewStateName, value);
            }
        }

        private const string m_AddCriteriaCommandTextViewStateName = "AddCriteriaCommandText";
        /// <summary>
        /// ��������������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("�������")]
        [Category("����")]
        [Description("��������������ʾ�ı���")]
        public string AddCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_AddCriteriaCommandTextViewStateName, "�������");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_AddCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_InsertCriteriaCommandTextViewStateName = "InsertCriteriaCommandText";
        /// <summary>
        /// ���������������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("��������")]
        [Category("����")]
        [Description("���������������ʾ�ı���")]
        public string InsertCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_InsertCriteriaCommandTextViewStateName, "��������");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_InsertCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_DeleteCriteriaCommandTextViewStateName = "DeleteCriteriaCommandText";
        /// <summary>
        /// ɾ�������������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ɾ������")]
        [Category("����")]
        [Description("ɾ�������������ʾ�ı���")]
        public string DeleteCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DeleteCriteriaCommandTextViewStateName, "ɾ������");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DeleteCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_DisplayFilterColumnsCommandTextViewStateName = "DisplayFilterColumnsCommandText";
        /// <summary>
        /// ��ʾɸѡ���������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ɸѡ��")]
        [Category("����")]
        [Description("��ʾɸѡ���������ʾ�ı���")]
        public string DisplayFilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DisplayFilterColumnsCommandTextViewStateName, "ɸѡ��");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DisplayFilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_FilterColumnsCommandTextViewStateName = "FilterColumnsCommandText";
        /// <summary>
        /// ����ɸѡ���������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ȷ��")]
        [Category("����")]
        [Description("����ɸѡ���������ʾ�ı���")]
        public string FilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterColumnsCommandTextViewStateName, "ȷ��");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_CancelFilterColumnsCommandTextViewStateName = "CancelFilterColumnsCommandText";
        /// <summary>
        /// ȡ��ɸѡ���������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ȡ��")]
        [Category("����")]
        [Description("ȡ��ɸѡ���������ʾ�ı���")]
        public string CancelFilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_CancelFilterColumnsCommandTextViewStateName, "ȡ��");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_CancelFilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_DisplayCommandTextViewStateName = "DisplayCommandText";
        /// <summary>
        /// ��ʾ/�����������ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ɸѡ")]
        [Category("����")]
        [Description("��ʾ/�����������ʾ�ı���")]
        public string DisplayCommandText
        {
            get
            {
                if (this.ViewState[m_DisplayCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_DisplayCommandTextViewStateName];
                }
                return "ɸѡ";
            }
            set
            {
                this.ViewState[m_DisplayCommandTextViewStateName] = value;
            }
        }

        private const string m_HideCommandTextViewStateName = "HideCommandText";
        /// <summary>
        /// ��ʾ/��������������ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("����")]
        [Category("����")]
        [Description("��ʾ/��������������ı���")]
        public string HideCommandText
        {
            get
            {
                if (this.ViewState[m_HideCommandTextViewStateName] != null)
                {
                    return (string)this.ViewState[m_HideCommandTextViewStateName];
                }
                return "����";
            }
            set
            {
                this.ViewState[m_HideCommandTextViewStateName] = value;
            }
        }

        private const string m_ShowHeaderViewStateName = "ShowHeader";
        /// <summary>
        /// �Ƿ���ʾɸѡ����ͷ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("���")]
        [Description("�Ƿ���ʾɸѡ����ͷ��")]
        public bool ShowHeader
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_ShowHeaderViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_ShowHeaderViewStateName, value);
            }
        }

        private const string m_BlankFilterItemTextViewStateName = "BlankFilterItemText";
        /// <summary>
        /// �հ�ɸѡ�����ʾ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("--��ѡ��ɸѡ��--")]
        [Category("���")]
        [Description("�հ�ɸѡ�����ʾ�ı���")]
        public string BlankFilterItemText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemTextViewStateName, "--��ѡ��ɸѡ��--");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemTextViewStateName, value);
            }
        }

        private const string m_BlankFilterItemValueViewStateName = "BlankFilterItemValue";
        /// <summary>
        /// �հ�ɸѡ��Ĳ�ѯ�ı���
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("���")]
        [Description("�հ�ɸѡ��Ĳ�ѯ�ı���")]
        public string BlankFilterItemValue
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemValueViewStateName, "");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemValueViewStateName, value);
            }
        }

        private const string m_EnableFilterColumnsViewStateName = "EnableFilterColumns";
        /// <summary>
        /// �����ܷ�ɸѡ�С�
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�����ܷ�ɸѡ�С�")]
        public bool EnableFilterColumns
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_EnableFilterColumnsViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_EnableFilterColumnsViewStateName, value);
            }
        }

        private const string m_FilterParameterNameViewStateName = "FilterParameterName";
        /// <summary>
        /// ִ��ɸѡ�ķ�����������һ��������Դ�ؼ������õ�Select�����Ĳ�������
        /// </summary>
        [Browsable(true)]
        [DefaultValue("filterExp")]
        [Category("��Ϊ")]
        [Description("ִ��ɸѡ�ķ�����������")]
        public string FilterParameterName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterParameterNameViewStateName, "filterExp");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterParameterNameViewStateName, value);
            }
        }

        /// <summary>
        /// ������ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("����ؼ�����ʽ��")]
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
        /// ɸѡ����������ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("ɸѡ����������ʽ��")]
        public virtual Style CriteriaTableStyle
        {
            get
            {
                if (_criteriaTableStyle == null)
                {
                    _criteriaTableStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_criteriaTableStyle).TrackViewState();
                    }
                }
                return _criteriaTableStyle;
            }
        }

        /// <summary>
        /// ɸѡ�пؼ�����ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("ɸѡ�пؼ�����ʽ��")]
        public virtual Style FilterColumnsStyle
        {
            get
            {
                if (_filterColumnsStyle == null)
                {
                    _filterColumnsStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_filterColumnsStyle).TrackViewState();
                    }
                }
                return _filterColumnsStyle;
            }
        }

        private const string m_ResetCriteriaWithHidingViewStateName = "ResetCriteriaWithHiding";
        /// <summary>
        /// ������ɸѡ�ؼ���ͬʱ�Ƿ�����ɸѡ������
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("������ɸѡ�ؼ���ͬʱ�Ƿ�����ɸѡ������")]
        public bool ResetCriteriaWithHiding
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_ResetCriteriaWithHidingViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_ResetCriteriaWithHidingViewStateName, value);
            }
        }

        private const string m_InitSelectDataViewStateName = "InitSelectData";
        /// <summary>
        /// �Ƿ��ڳ�ʼ��ʱ��������ݡ�
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�Ƿ��ڳ�ʼ��ʱ��������ݡ�")]
        public bool InitSelectData
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    m_InitSelectDataViewStateName, true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    m_InitSelectDataViewStateName, value);
            }
        }

        private PropertyMappingCollection _propertyMappings;
        /// <summary>
        /// ����ӳ�伯�ϡ�
        /// </summary>
        [Category("����")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("����ӳ�伯�ϡ�")]
        public virtual PropertyMappingCollection PropertyMappings
        {
            get
            {
                if (_propertyMappings == null)
                {
                    _propertyMappings = new PropertyMappingCollection();
                }
                return _propertyMappings;
            }
        }

        private PropertyMappingSettings _propertyMappingSettings;
        /// <summary>
        /// ����ӳ�����á�
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("����ӳ�����á�")]
        public virtual PropertyMappingSettings PropertyMappingSettings
        {
            get
            {
                if (_propertyMappingSettings == null)
                {
                    _propertyMappingSettings = new PropertyMappingSettings();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_propertyMappingSettings).TrackViewState();
                    }
                }
                return _propertyMappingSettings;
            }
        }

        
        private const string m_CriteriaNoListViewStateName = "CriteriaNoList";
        /// <summary>
        /// Ϊÿһ��ɸѡ��������һ��Ψһ�ı�ţ��Ա��ڻط������ʱ���ع�ɸѡ������
        /// </summary>
        [Browsable(false)]
        [Description("��¼������š�")]
        protected int[] CriteriaNoList
        {
            get
            {
                return Utility.GetViewStatePropertyValue<int[]>(this.ViewState,
                    m_CriteriaNoListViewStateName, new int[] { 0 });
            }
            set
            {
                Utility.SetViewStatePropertyValue<int[]>(this.ViewState,
                    m_CriteriaNoListViewStateName, value);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// ��ִ��ɸѡ��ʱ�������¼���
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        public event CommandEventHandler Command;

        #endregion

        #region Operates


        void Filtering_Command(object sender, CommandEventArgs e)
        {
            if (_btnAddCriteria != null && ((WebControl)sender).UniqueID.Equals(_btnAddCriteria.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("AddCriteria", null));
                }
                this.AddCriteriaTableRow();
            }
            if (_btnReset != null && ((WebControl)sender).UniqueID.Equals(_btnReset.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("ResetCriteria", null));
                }
                this.ResetCrieriaTableRow();
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("DoFilter", null));
                }
                this.DoFilter();
            }
            if (_btnDeleteCriteria != null && ((WebControl)sender).UniqueID.Equals(_btnDeleteCriteria.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("DeleteCriteria", null));
                }
                this.DeleteCriteriaTableRow();
                if (_tblCriteria.Rows.Count == 1)
                {
                    this.AddCriteriaTableRow();
                }
            }
            //if (_btnInsertCriteria != null && ((WebControl)sender).UniqueID.Equals(_btnInsertCriteria.UniqueID))
            //{
            //    if (this.Command != null)
            //    {
            //        this.Command(this, new CommandEventArgs("InsertCriteria", null));
            //    }
            //    this.InsertCriteriaTableRow();
            //}
            if (_btnDisplayFilterColumns != null && ((WebControl)sender).UniqueID.Equals(_btnDisplayFilterColumns.UniqueID))
            {
                _pnlFilterColumns.Visible = true;
            }
            if (_btnFilterColumns != null && ((WebControl)sender).UniqueID.Equals(_btnFilterColumns.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("DoFilterColumns", null));
                }
                this.DoFilterColumns();
                _pnlFilterColumns.Visible = false;
            }
            if (_btnCancelFilterColumns != null && ((WebControl)sender).UniqueID.Equals(_btnCancelFilterColumns.UniqueID))
            {
                _pnlFilterColumns.Visible = false;
            }
            if (_btnFilter != null && ((WebControl)sender).UniqueID.Equals(_btnFilter.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("DoFilter", null));
                }
                this.DoFilter();
            }
            if (_displayOrHideCtrl != null && ((WebControl)sender).UniqueID.Equals(_displayOrHideCtrl.UniqueID))
            {
                this.Visible = !this.Visible;
                if (!this.Visible && this.ResetCriteriaWithHiding)
                {
                    if (this.Command != null)
                    {
                        this.Command(this, new CommandEventArgs("ResetCriteria", null));
                    }
                    this.ResetCrieriaTableRow();
                    if (this.Command != null)
                    {
                        this.Command(this, new CommandEventArgs("DoFilter", null));
                    }
                    this.DoFilter();
                }
            }
            if (_doQueryingCtrl != null && ((WebControl)sender).UniqueID.Equals(_doQueryingCtrl.UniqueID))
            {
                if (this.Command != null)
                {
                    this.Command(this, new CommandEventArgs("DoFilter", null));
                }
                this.DoFilter();
            }
        }

        /// <summary>
        /// ������ʾ/�����ⲿ�ؼ���״̬����Ҫ�ǽ�������ؼ���ʾ�ı���
        /// </summary>
        private void SetDisplayOrHideControlState()
        {
            if (_displayOrHideCtrl != null && _displayOrHideCtrl is IButtonControl)
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

        private void SetDoQueryingControlState()
        {
            if (_doQueryingCtrl != null)
            {
                if (this.Visible && this.IsHidingDoQueryingControl)
                {
                    _doQueryingCtrl.Visible = false;
                }
                else
                {
                    _doQueryingCtrl.Visible = true;
                }
            }
        }

        #region ����Դɸѡ�¼�

        /// <summary>
        /// ���㲢����ɸѡ���ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinqDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            LinqDataSourceView lnqDsCtrl = (LinqDataSourceView)sender;

            if (!string.IsNullOrEmpty(lnqDsCtrl.Where))
            {
                string filter = this.CalFilterString();

                if (!string.IsNullOrEmpty(filter) && !lnqDsCtrl.Where.Contains(filter))
                {
                    lnqDsCtrl.Where = string.Format("{0} AND {1}",
                        lnqDsCtrl.Where, filter);
                }
            }
            else
            {
                lnqDsCtrl.Where = this.CalFilterString();
            }
        }

        /// <summary>
        /// ���㲢����ɸѡ���ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SqlDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            SqlDataSourceView sqlDsCtrl = (SqlDataSourceView)sender;

            if (!string.IsNullOrEmpty(sqlDsCtrl.FilterExpression))
            {
                string filter = this.CalFilterString();

                if (!string.IsNullOrEmpty(filter) && !sqlDsCtrl.FilterExpression.Contains(filter))
                {
                    sqlDsCtrl.FilterExpression = string.Format("{0} AND {1}",
                        sqlDsCtrl.FilterExpression, filter);
                }
            }
            else
            {
                sqlDsCtrl.FilterExpression = this.CalFilterString();
            }
        }

        private DataTable _dataTable;
        /// <summary>
        /// ���㲢����ɸѡ���ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ObjectDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            ObjectDataSource objDs = (ObjectDataSource)_dsCtrl;
            if (objDs.EnablePaging)
            {
                return;
            }

            if (e.ReturnValue != null)
            {
                if (e.ReturnValue is DataSet)
                {
                    _dataTable = ((DataSet)e.ReturnValue).Tables[0];
                }
                if (e.ReturnValue is DataTable)
                {
                    _dataTable = (DataTable)e.ReturnValue;
                }
                if (e.ReturnValue is DataView)
                {
                    _dataTable = ((DataView)e.ReturnValue).Table;
                }
            }

            objDs.FilterExpression = this.CalFilterString();
        }

        /// <summary>
        /// ���㲢����ɸѡ���ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ObjectDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            ObjectDataSource objDs = (ObjectDataSource)_dsCtrl;
            if (!objDs.EnablePaging)
            {
                return;
            }
            string filterExp = (string)e.InputParameters[this.FilterParameterName];
            if (!string.IsNullOrEmpty(filterExp))
            {
                string filterStr = CalFilterString();
                if (!string.IsNullOrEmpty(filterStr))
                {
                    e.InputParameters[this.FilterParameterName] =
                        string.Format("{0} {1} {2}",
                        filterExp, Constants.Logic.And,
                        filterStr);
                }
            }
            else
            {
                e.InputParameters[this.FilterParameterName] = CalFilterString();
            }
        }

        #endregion


        /// <summary>
        /// ����ɸѡ�������
        /// </summary>
        private void ConstructCriteriaTable()
        {
            _tblCriteria = new Table();
            _tblCriteria.ID = "tblCriteria";
            ConstructCriteriaTableHeader();
            for (int i = 0; i < this.CriteriaNoList.Length; i++)
            {
                this.AddCriteriaTableRow();
            }
            this.Controls.Add(_tblCriteria);
        }

        /// <summary>
        /// ����ɸѡ�������ı�ͷ��
        /// </summary>
        private void ConstructCriteriaTableHeader()
        {
            TableHeaderRow tblHeader = new TableHeaderRow();

            TableHeaderCell tblHeaderCell = new TableHeaderCell();
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "��ѯ��";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "�Ƚ�";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "ֵ";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "�߼�";
            tblHeader.Cells.Add(tblHeaderCell);

            _tblCriteria.Rows.Add(tblHeader);
        }

        /// <summary>
        /// ��¼�ڵ�ǰҳ���������д���������б��лָ������ı�Ŷ�Ӧ���б��±ꡣ
        /// </summary>
        private int iCriteriaNo = 0;

        /// <summary>
        /// ����հ׵�ɸѡ�����С�
        /// </summary>
        /// <returns></returns>
        private TableRow ConstructBlankCriteriaTableRow()
        {
            int iCount = (iCriteriaNo > this.CriteriaNoList.Length - 1) ? Utility.GetMaxValue<int>(this.CriteriaNoList, 0) + 1 : this.CriteriaNoList[iCriteriaNo];

            TableRow tblRow = new TableRow();

            TableCell tblCell = new TableCell();
            CheckBox chkBoxChooseOne = new CheckBox();
            chkBoxChooseOne.ID = "chkBoxChooseOne" + iCount.ToString();
            tblCell.Controls.Add(chkBoxChooseOne);
            tblRow.Cells.Add(tblCell);

            tblCell = new TableCell();
            DropDownList ddlstFilterItems = new DropDownList();
            ddlstFilterItems.ID = "ddlstFilterItems" + iCount.ToString();
            ddlstFilterItems.Items.Add(new ListItem(this.BlankFilterItemText, this.BlankFilterItemValue));
            if (!this.DesignMode && this._filterItems != null)
            {
                foreach (ListItem aItem in this._filterItems)
                {
                    ddlstFilterItems.Items.Add(new ListItem(aItem.Text, aItem.Value));
                }
            }
            tblCell.Controls.Add(ddlstFilterItems);
            tblRow.Cells.Add(tblCell);

            tblCell = new TableCell();
            DropDownList ddlstComparer = new DropDownList();
            ddlstComparer.ID = "ddlstComparer" + iCount.ToString();
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.Equal), Constants.Comparer.Equal));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.NotEqual), Constants.Comparer.NotEqual));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.GreatThan), Constants.Comparer.GreatThan));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.GreatEqualThan), Constants.Comparer.GreatEqualThan));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.LessThan), Constants.Comparer.LessThan));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.LessEqualThan), Constants.Comparer.LessEqualThan));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.Include), Constants.Comparer.Include));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.NotInclude), Constants.Comparer.NotInclude));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.IsNull), Constants.Comparer.IsNull));
            ddlstComparer.Items.Add(new ListItem(Constants.Comparer.GetText(Constants.Comparer.IsNotNull), Constants.Comparer.IsNotNull));
            tblCell.Controls.Add(ddlstComparer);
            tblRow.Cells.Add(tblCell);

            tblCell = new TableCell();
            TextBox txtValue = new TextBox();
            txtValue.ID = "txtValue" + iCount.ToString();
            txtValue.Text = string.Empty;
            tblCell.Controls.Add(txtValue);
            tblRow.Cells.Add(tblCell);

            tblCell = new TableCell();
            DropDownList ddlstLogic = new DropDownList();
            ddlstLogic.ID = "ddlstLogic" + iCount.ToString();
            ddlstLogic.Items.Add(new ListItem(Constants.Logic.GetText(Constants.Logic.And), Constants.Logic.And));
            ddlstLogic.Items.Add(new ListItem(Constants.Logic.GetText(Constants.Logic.Or), Constants.Logic.Or));
            tblCell.Controls.Add(ddlstLogic);
            tblRow.Cells.Add(tblCell);

            iCriteriaNo++;

            return tblRow;
        }


        /// <summary>
        /// ��������ؼ���
        /// </summary>
        private void ConstructCommandControl()
        {
            _btnFilter = new Button();
            _btnFilter.ID = "btnFilter";
            _btnFilter.Text = this.FilterCommandText;
            _btnFilter.UseSubmitBehavior = false;
            _btnFilter.Command += new CommandEventHandler(Filtering_Command);

            _btnReset = new Button();
            _btnReset.ID = "btnReset";
            _btnReset.Text = this.ResetCommandText;
            _btnReset.UseSubmitBehavior = false;
            _btnReset.Command += new CommandEventHandler(Filtering_Command);

            _btnAddCriteria = new Button();
            _btnAddCriteria.ID = "btnAddCriteria";
            _btnAddCriteria.Text = this.AddCriteriaCommandText;
            _btnAddCriteria.UseSubmitBehavior = false;
            _btnAddCriteria.Command += new CommandEventHandler(Filtering_Command);

            //_btnInsertCriteria = new Button();
            //_btnInsertCriteria.ID = "btnInsertCriteria";
            //_btnInsertCriteria.Text = this.InsertCriteriaCommandText;
            //_btnInsertCriteria.UseSubmitBehavior = false;
            //_btnInsertCriteria.Command += new CommandEventHandler(Filtering_Command);

            _btnDeleteCriteria = new Button();
            _btnDeleteCriteria.ID = "btnDeleteCriteria";
            _btnDeleteCriteria.Text = this.DeleteCriteriaCommandText;
            _btnDeleteCriteria.UseSubmitBehavior = false;
            _btnDeleteCriteria.Command += new CommandEventHandler(Filtering_Command);

            _btnDisplayFilterColumns = new Button();
            _btnDisplayFilterColumns.ID = "btnFilterColumns";
            _btnDisplayFilterColumns.Text = this.DisplayFilterColumnsCommandText;
            _btnDisplayFilterColumns.UseSubmitBehavior = false;
            _btnDisplayFilterColumns.Command += new CommandEventHandler(Filtering_Command);

            this.Controls.Add(_btnFilter);
            this.Controls.Add(_btnReset);
            this.Controls.Add(_btnAddCriteria);
            //this.Controls.Add(_btnInsertCriteria);
            this.Controls.Add(_btnDeleteCriteria);
            this.Controls.Add(_btnDisplayFilterColumns);
        }

        /// <summary>
        /// ����ɸѡ�пؼ���
        /// </summary>
        private void ConstructFilterColumns()
        {
            _pnlFilterColumns = new Panel();
            _pnlFilterColumns.ID = "pnlFilterColumns";

            _chkBoxLstFilterColumns = new CheckBoxList();
            _chkBoxLstFilterColumns.ID = "chkBoxLstFilterColumns";
            _chkBoxLstFilterColumns.CausesValidation = false;
            _chkBoxLstFilterColumns.RepeatDirection = RepeatDirection.Horizontal;
            _chkBoxLstFilterColumns.RepeatColumns = 5;
            _chkBoxLstFilterColumns.RepeatLayout = RepeatLayout.Table;

            if (!this.DesignMode && _filterCtrl != null)
            {
                foreach (DataControlField aField in _filterCtrl.Columns)
                {
                    if (!string.IsNullOrEmpty(aField.HeaderText))
                    {
                        ListItem aItem = new ListItem(aField.HeaderText);
                        aItem.Selected = aField.Visible;
                        _chkBoxLstFilterColumns.Items.Add(aItem);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= _chkBoxLstFilterColumns.RepeatColumns; i++)
                {
                    _chkBoxLstFilterColumns.Items.Add(new ListItem("ɸѡ��" + i.ToString()));
                }
            }

            _btnFilterColumns = new Button();
            _btnFilterColumns.ID = "btnSetFilterColumns";
            _btnFilterColumns.Text = this.FilterColumnsCommandText;
            _btnFilterColumns.UseSubmitBehavior = false;
            _btnFilterColumns.Command += new CommandEventHandler(Filtering_Command);

            _btnCancelFilterColumns = new Button();
            _btnCancelFilterColumns.ID = "btnCancelFilterColumns";
            _btnCancelFilterColumns.Text = this.CancelFilterColumnsCommandText;
            _btnCancelFilterColumns.UseSubmitBehavior = false;
            _btnCancelFilterColumns.Command += new CommandEventHandler(Filtering_Command);

            _pnlFilterColumns.Controls.Add(_chkBoxLstFilterColumns);
            _pnlFilterColumns.Controls.Add(_btnFilterColumns);
            _pnlFilterColumns.Controls.Add(_btnCancelFilterColumns);

            if (!this.DesignMode)
            {
                _pnlFilterColumns.Visible = false;
            }

            this.Controls.Add(_pnlFilterColumns);
        }

        /// <summary>
        /// ����ɸѡ�����С�
        /// </summary>
        private void ResetCrieriaTableRow()
        {
            for (int i = 0; i < _tblCriteria.Rows.Count; i++)
            {
                if (_tblCriteria.Rows[i] is TableHeaderRow || _tblCriteria.Rows[i] is TableFooterRow)
                {
                    continue;
                }
                _tblCriteria.Rows.Remove(_tblCriteria.Rows[i]);
                i--;
            }
            this.AddCriteriaTableRow();
        }

        /// <summary>
        /// ���ɸѡ�����С�
        /// </summary>
        private void AddCriteriaTableRow()
        {
            _tblCriteria.Rows.Add(ConstructBlankCriteriaTableRow());
        }

        /// <summary>
        /// ����ɸѡ�����С�
        /// </summary>
        private void InsertCriteriaTableRow()
        {
            for (int i = 0; i < _tblCriteria.Rows.Count; i++)
            {
                if (_tblCriteria.Rows[i] is TableHeaderRow || _tblCriteria.Rows[i] is TableFooterRow)
                {
                    continue;
                }
                if (_tblCriteria.Rows[i].Cells.Count > 0 && _tblCriteria.Rows[i].Cells[0].Controls.Count > 0)
                {
                    Control ctrl = _tblCriteria.Rows[i].Cells[0].Controls[0];
                    if (ctrl != null && ctrl is CheckBox)
                    {
                        CheckBox chkBoxChooseOne = (CheckBox)ctrl;
                        if (chkBoxChooseOne.Checked)
                        {
                            _tblCriteria.Rows.AddAt(_tblCriteria.Rows.GetRowIndex(_tblCriteria.Rows[i]),
                                ConstructBlankCriteriaTableRow());
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ɾ��ɸѡ�����С�
        /// </summary>
        private void DeleteCriteriaTableRow()
        {
            for (int i = 0; i < _tblCriteria.Rows.Count; i++)
            {
                if (_tblCriteria.Rows[i] is TableHeaderRow || _tblCriteria.Rows[i] is TableFooterRow)
                {
                    continue;
                }
                if (_tblCriteria.Rows[i].Cells.Count > 0 && _tblCriteria.Rows[i].Cells[0].Controls.Count > 0)
                {
                    Control ctrl = _tblCriteria.Rows[i].Cells[0].Controls[0];
                    if (ctrl != null && ctrl is CheckBox)
                    {
                        CheckBox chkBoxChooseOne = (CheckBox)ctrl;
                        if (chkBoxChooseOne.Checked)
                        {
                            _tblCriteria.Rows.Remove(_tblCriteria.Rows[i]);
                            i--;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// ����ɸѡ����ؿؼ�״̬��
        /// </summary>
        private void SetFilterColumnState()
        {
            if (_btnDisplayFilterColumns != null && _pnlFilterColumns != null)
            {
                if (this.EnableFilterColumns)
                {
                    _btnDisplayFilterColumns.Visible = true;
                }
                else
                {
                    _btnDisplayFilterColumns.Visible = false;
                    _pnlFilterColumns.Visible = false;
                }
            }
        }

        /// <summary>
        /// ִ��ɸѡ�С�
        /// </summary>
        private void DoFilterColumns()
        {
            if (_filterCtrl != null && _pnlFilterColumns != null && _pnlFilterColumns.Controls.Count > 0)
            {
                Control ctrl = _pnlFilterColumns.Controls[0];
                if (ctrl != null && ctrl is CheckBoxList)
                {
                    CheckBoxList chkBoxLstFilterColumns = (CheckBoxList)ctrl;

                    foreach (DataControlField aField in _filterCtrl.Columns)
                    {
                        ListItem theItem = chkBoxLstFilterColumns.Items.FindByText(aField.HeaderText);
                        if (theItem != null)
                        {
                            aField.Visible = theItem.Selected;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ִ��ɸѡ��
        /// </summary>
        private void DoFilter()
        {
            if (_filterCtrl != null)
            {
                if (_dsCtrl != null)
                {
                    if (_dsCtrl is SqlDataSource)
                        _filterCtrl.DataSource = ((SqlDataSource)_dsCtrl).Select(DataSourceSelectArguments.Empty);
                }
                _filterCtrl.PageIndex = 0;
                _filterCtrl.DataBind();
            }
        }

        /// <summary>
        /// ����ɸѡ���ʽ��
        /// ���㲽�裺
        /// ��1��ͨ������ɸѡ��������е�ÿһ�У��õ���ɸѡ������Ƚ������������ɸѡֵ������ɸѡ�߼�����
        /// ��2��Ȼ�������õ�������ӳ�һ���Ϸ���ɸѡ���ʽ�����磺��Student.Name LIKE '%��%' AND����
        /// ��3�����Ž������������д��ӵ�ɸѡ���ʽȫ�����ӳ����յĺϷ�ɸѡ���ʽ��
        ///       ���磺��Student.Name LIKE '%��%' AND Student.Gender = '��'����
        /// ��Ҫע�⣺
        /// ���ձ��ʽ�Ƿ�Ϸ�����Ҫ����ִ��ɸѡ���ʽ���������Ͷ� GridView ������Ҫ����ɸѡ���̵��ⲿ�ؼ������������
        /// ���磺ִ��ɸѡ���ʽ��������������DataSet��FilterExpression���͡�SQL����
        /// ���磺GridView �� Column �� HeaderText��AccessibleHeaderText �����������Ƿ���ȷ��
        /// </summary>
        /// <returns>����õ���ɸѡ���ʽ��</returns>
        private string CalFilterString()
        {
            string filterExp = string.Empty;

            if (_tblCriteria != null)
            {
                foreach (TableRow aRow in _tblCriteria.Rows)
                {
                    if (aRow is TableHeaderRow || aRow is TableFooterRow)
                    {
                        continue;
                    }

                    //  ���ɸѡ��ı��ʽ�����͡�
                    string fieldName = string.Empty;
                    string fieldType = Constants.FieldType.Text;

                    DropDownList ddlstFilterItems = (DropDownList)aRow.Cells[1].Controls[0];
                    if (!string.IsNullOrEmpty(ddlstFilterItems.SelectedValue))
                    {
                        string[] fieldStrArray =
                            ddlstFilterItems.SelectedValue.Split(
                            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries);

                        if (fieldStrArray.Length <= 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (fieldStrArray.Length == 1)
                            {
                                fieldName = fieldStrArray[0];
                                if (_dataTable != null)
                                {
                                    DataColumn col = _dataTable.Columns[fieldName];
                                    if (col != null)
                                    {
                                        fieldType = Constants.FieldType.GetFieldType(col.DataType);
                                    }
                                }
                            }
                            else
                            {
                                fieldName = fieldStrArray[0];
                                fieldType = fieldStrArray[1];
                            }
                        }
                    }

                    //  ��ñȽ��������
                    string comparer = string.Empty;

                    DropDownList ddlstComparer = (DropDownList)aRow.Cells[2].Controls[0];
                    if (!string.IsNullOrEmpty(ddlstComparer.SelectedValue))
                    {
                        comparer = ddlstComparer.SelectedValue;
                    }

                    //  ���ɸѡֵ��
                    string filterValue = string.Empty;

                    TextBox txtValue = (TextBox)aRow.Cells[3].Controls[0];
                    if (!string.IsNullOrEmpty(txtValue.Text))
                    {
                        filterValue = txtValue.Text.Trim();
                    }

                    //  ���ɸѡ�߼���
                    string logic = Constants.Logic.And;

                    DropDownList ddlstLogic = (DropDownList)aRow.Cells[4].Controls[0];
                    if (!string.IsNullOrEmpty(ddlstLogic.SelectedValue))
                    {
                        logic = ddlstLogic.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        //  �����������ӳ��������ɸѡֵ��
                        filterValue = GetPropertyFilterValue(fieldName, filterValue);

                        //  ��֤������ɸѡֵ��
                        filterValue = Constants.FieldType.Validate(filterValue, fieldType);

                        //  ���ӱ��ʽ��

                        if (_dsCtrl is LinqDataSource)
                        {
                            if (fieldType.Equals(Constants.FieldType.Text))
                            {
                                filterValue = "\"" + filterValue + "\"";
                            }
                            else if (fieldType.Equals(Constants.FieldType.DateTime))
                            {
                                filterValue =
                                    string.Format(@"DateTime.Parse(""{0}"")", filterValue);
                            }
                            if (comparer.Equals(Constants.Comparer.Equal))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "==", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.GreatEqualThan))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    ">=", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.GreatThan))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    ">", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.Include) && fieldType.Equals(Constants.FieldType.Text))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    ".Contains(", filterValue + ")", logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.IsNotNull))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "!=", "null", logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.IsNull))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "==", "null", logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.LessEqualThan))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "<=", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.LessThan))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "<", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.NotEqual))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    "!=", filterValue, logic);
                            }
                            else if (comparer.Equals(Constants.Comparer.NotInclude) && fieldType.Equals(Constants.FieldType.Text))
                            {
                                filterExp +=
                                    string.Format(fieldName + " {2} ",
                                    ".IndexOf(", filterValue + ")<0", logic);
                            }
                        }
                        else
                        {
                            if (comparer.Equals(Constants.Comparer.IsNull) || comparer.Equals(Constants.Comparer.IsNotNull))
                            {
                                filterExp += string.Format("{0} {1} {2} ", fieldName, comparer, logic);
                            }
                            else
                            {
                                if (fieldType.Equals(Constants.FieldType.Text))
                                {
                                    if (comparer.Equals(Constants.Comparer.Include) || comparer.Equals(Constants.Comparer.NotInclude))
                                    {
                                        filterExp += string.Format("{0} {1} '%{2}%' {3} ", fieldName, comparer, filterValue, logic);
                                    }
                                    else
                                    {
                                        filterExp += string.Format("{0} {1} '{2:n}' {3} ", fieldName, comparer, filterValue, logic);
                                    }
                                }
                                else
                                {
                                    if (comparer.Equals(Constants.Comparer.Include))
                                    {
                                        comparer = Constants.Comparer.Equal;
                                    }
                                    if (comparer.Equals(Constants.Comparer.NotInclude))
                                    {
                                        comparer = Constants.Comparer.NotEqual;
                                    }
                                    if (fieldType.Equals(Constants.FieldType.Numeric))
                                    {
                                        filterExp += string.Format("{0} {1} {2} {3} ", fieldName, comparer, filterValue, logic);
                                    }
                                    if (fieldType.Equals(Constants.FieldType.DateTime))
                                    {
                                        filterExp += string.Format("{0} {1} '{2}' {3} ", fieldName, comparer, filterValue, logic);
                                    }
                                }
                            }
                        }
                    }
                }
                filterExp = filterExp.Trim();
                filterExp = filterExp.TrimEnd(Constants.Logic.And.ToCharArray());
                filterExp = filterExp.TrimEnd(Constants.Logic.Or.ToCharArray());
            }
            return filterExp;
        }

        /// <summary>
        /// Ϊָ����ɸѡ��ֶΣ�ת������ӳ���Ӧ������ֵ��
        /// </summary>
        /// <param name="fieldName">ָ����ɸѡ��ֶΣ�</param>
        /// <param name="filterValue">��ת������ӳ���Դֵ��һ�����û�ͨ��ɸѡ�ؼ�������Ѻ��ı�</param>
        /// <returns>�õ�ת���������ӳ���Ӧ������ֵ�����û��ת���ɹ����򷵻�ԭʼֵ��</returns>
        private string GetPropertyFilterValue(string fieldName, string filterValue)
        {
            if (_propertyMappings != null)
            {
                PropertyMapping aPropMap = _propertyMappings.FindByFilterItem(fieldName);
                if (aPropMap != null && _propertyMappingResult != null && _propertyMappingSettings != null)
                {
                    IEnumerator ie = _propertyMappingResult.GetEnumerator();
                    while(ie.MoveNext())
                    {
                        object obj = ie.Current;
                        if (obj is DataRowView)
                        {
                            DataRowView drv = (DataRowView)obj;
                            object propertyName = drv.Row[_propertyMappingSettings.RefPropertyName];
                            if (propertyName != null && propertyName.Equals(aPropMap.PropertyName))
                            {
                                object propertyMeaning = drv.Row[_propertyMappingSettings.RefPropertyMeaning];
                                if (propertyMeaning != null && propertyMeaning.Equals(filterValue))
                                {
                                    object propertyValue = drv.Row[_propertyMappingSettings.RefPropertyValue];
                                    if (propertyValue != null)
                                    {
                                        filterValue = propertyValue.ToString();
                                        return filterValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return filterValue;
        }

        /// <summary>
        /// ����ɸѡ�������״̬��
        /// </summary>
        private void SetCriteriaTableState()
        {
            if (_tblCriteria != null && _tblCriteria.Rows.Count > 0 && _tblCriteria.Rows[0] is TableHeaderRow)
            {
                _tblCriteria.Rows[0].Visible = this.ShowHeader;
            }
        }

        /// <summary>
        /// ���ü�¼ɸѡ��������б����ԡ�
        /// </summary>
        private void SetCriteriaNoListProperty()
        {
            if (_tblCriteria != null)
            {
                List<int> list = new List<int>();
                foreach (TableRow aRow in _tblCriteria.Rows)
                {
                    if (aRow is TableHeaderRow || aRow is TableFooterRow)
                    {
                        continue;
                    }
                    list.Add(Convert.ToInt32(aRow.Cells[0].Controls[0].ID.Substring("chkBoxChooseOne".Length)));
                }
                this.CriteriaNoList = list.ToArray();
            }
        }

        private void SetControlState()
        {
            if (!this.DesignMode)
            {
                this.SetFilterColumnState();

                this.SetDisplayOrHideControlState();

                this.SetDoQueryingControlState();

                this.SetCriteriaTableState();

                this.SetCriteriaNoListProperty();
            }
        }

        private void ApplyStyle()
        {
            if (_commandStyle != null)
            {
                _btnFilter.ApplyStyle(CommandStyle);
                _btnReset.ApplyStyle(CommandStyle);
                _btnAddCriteria.ApplyStyle(CommandStyle);
                //_btnInsertCriteria.ApplyStyle(CommandStyle);
                _btnDeleteCriteria.ApplyStyle(CommandStyle);
                _btnDisplayFilterColumns.ApplyStyle(CommandStyle);
                _btnFilterColumns.ApplyStyle(CommandStyle);
                _btnCancelFilterColumns.ApplyStyle(CommandStyle);
            }

            if (_criteriaTableStyle != null)
            {
                _tblCriteria.ApplyStyle(CriteriaTableStyle);
            }

            if (_filterColumnsStyle != null)
            {
                _pnlFilterColumns.ApplyStyle(FilterColumnsStyle);
            }
        }
        
        #endregion

        /// <summary>
        /// ����Page_Init�¼�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this); //ע��ؼ��ɽ��пؼ�״̬�ı���ͻָ�
            InitControl();
        }
        #region ��ʼ��
        /// <summary>
        /// ��ʼ���ÿؼ���
        /// </summary>
        private void InitControl()
        {
            if (!this.DesignMode)
            {
                InitLableText();
                InitDataView();

                InitDataSourceControl();
                InitFilterObjectControl();
                InitPropertyMappingDataSourceControl();
                InitDoQueryingControl();
                InitDisplayOrHideConstrol();
                //InitFilterItems();
                FillDataIntoFilterControl();
                this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
            }
        }

        /// <summary>
        /// ����Label��ֵ
        /// </summary>
        private void InitLableText()
        {
            Label lblBasicUserIDControl = (Label)Utility.FindControl(this, LableBasicUserID);
            Label lblUserEntityIDControl = (Label)Utility.FindControl(this, LabelUserEntityID);
            if (lblBasicUserIDControl != null && lblUserEntityIDControl != null)
            {
                if (Page.Request.QueryString.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Page.Request.QueryString["_BID"])
                        && !string.IsNullOrEmpty(Page.Request.QueryString["_UEID"]))
                    {
                        lblBasicUserIDControl.Text = Page.Request.QueryString["_BID"];
                        lblUserEntityIDControl.Text = Page.Request.QueryString["_UEID"];
                    }
                }
            }
        }

        /// <summary>
        /// ��־����
        /// </summary>
        private void InitDataView()
        {
            if (!string.IsNullOrEmpty(this._dataSourceID))
            {
                Control ctrl = this.Page.FindControl(this._dataSourceID);
                if (ctrl == null)
                {
                    ctrl = Utility.FindControlInNamingContainer(this, this._dataSourceID);
                }
                if (ctrl is ObjectDataSource)
                {
                    _dsCtrl = (ObjectDataSource)ctrl;
                    _dv = (DataView)((ObjectDataSource)_dsCtrl).Select();
                }
                else if (ctrl is SqlDataSource)
                {
                    _dsCtrl = (DataSourceControl)ctrl;
                    DataSourceSelectArguments dsSelectArg = new DataSourceSelectArguments();
                    _dv = (DataView)((SqlDataSource)_dsCtrl).Select(dsSelectArg);
                }
            }
        }

        /// <summary>
        /// ��־����(����ѯ���е�DropDownList)
        /// </summary>
        private void FillDataIntoFilterControl()
        {
            if (this._filterCtrl == null || this._dv == null)
                return;
            ListItem lst = null;
            this._filterItems = new List<ListItem>();
            foreach (DataColumn clm in this._dv.Table.Columns)
            {
                lst = new ListItem(clm.ColumnName, clm.ColumnName);
                this._filterItems.Add(lst);
            }
        }

        /// <summary>
        /// ��ʼ��ִ��ɸѡ������Դ�ⲿ�ؼ���
        /// </summary>
        private void InitDataSourceControl()
        {
            if (!string.IsNullOrEmpty(this._dataSourceID))
            {
                Control ctrl = this.Page.FindControl(this._dataSourceID);
                if (ctrl == null)
                {
                    ctrl = Utility.FindControlInNamingContainer(this, this._dataSourceID);
                }
                if (ctrl is System.Web.UI.WebControls.ObjectDataSource)
                {
                    _dsCtrl = (DataSourceControl)ctrl;
                    ((ObjectDataSource)_dsCtrl).Selecting += new ObjectDataSourceSelectingEventHandler(ObjectDataSource_Selecting);
                    ((ObjectDataSource)_dsCtrl).Selected += new ObjectDataSourceStatusEventHandler(ObjectDataSource_Selected);
                }
                else if (ctrl is System.Web.UI.WebControls.SqlDataSource)
                {
                    _dsCtrl = (DataSourceControl)ctrl;
                    ((SqlDataSource)_dsCtrl).Selecting += new SqlDataSourceSelectingEventHandler(SqlDataSource_Selecting);
                }
                else if (ctrl is System.Web.UI.WebControls.LinqDataSource)
                {
                    _dsCtrl = (DataSourceControl)ctrl;
                    ((LinqDataSource)_dsCtrl).Selecting += new EventHandler<LinqDataSourceSelectEventArgs>(LinqDataSource_Selecting);
                }
                else
                {
                    throw new Exception("Ŀǰɸѡ�ؼ�ֻ��֧�� ObjectDataSource / SqlDataSource / LinqDataSource �ؼ����������� DataSourceID �Ƿ���ȷ��");
                }
            }
        }

        /// <summary>
        /// ��ʼ����Ҫ����ɸѡ���ⲿ�ؼ���
        /// </summary>
        private void InitFilterObjectControl()
        {
            if (!string.IsNullOrEmpty(this._controlID))
            {
                Control ctrl = this.Page.FindControl(this._controlID);
                if (ctrl == null)
                {
                    ctrl = Utility.FindControlInNamingContainer(this, this._controlID);
                }
                if (ctrl is System.Web.UI.WebControls.GridView)
                {
                    _filterCtrl = (GridView)ctrl;
                    if (!this.Page.IsPostBack && !this.InitSelectData)
                    {
                        _filterCtrl.DataSourceID = string.Empty;
                    }
                    //��־���޸�
                    else
                    {
                        //if (_dv.Count != 0)
                        //{
                        //    _filterCtrl.DataSource = _dv;
                        //}
                    }
                }
                else
                {
                    throw new Exception("Ŀǰɸѡ�ؼ�ֻ��֧�� GridView �ؼ����������� ControlID �Ƿ���ȷ��");
                }
            }
        }

        /// <summary>
        /// ��ʼ��ִ�в�ѯ���ⲿ�ؼ���
        /// </summary>
        private void InitDoQueryingControl()
        {
            if (!string.IsNullOrEmpty(this._doQueryingControlId))
            {
                _doQueryingCtrl = this.Page.FindControl(this._doQueryingControlId);
                if (_doQueryingCtrl == null)
                {
                    _doQueryingCtrl = Utility.FindControlInNamingContainer(this, this._doQueryingControlId);
                }
                if (_doQueryingCtrl is IButtonControl)
                {
                    if (_doQueryingCtrl is Button)
                    {
                        ((Button)_doQueryingCtrl).UseSubmitBehavior = false;
                    }

                    ((IButtonControl)_doQueryingCtrl).Command += new CommandEventHandler(Filtering_Command);
                }
            }
        }

        /// <summary>
        /// ��ʼ����ʾ/�����ⲿ�ؼ���
        /// </summary>
        private void InitDisplayOrHideConstrol()
        {
            _displayOrHideCtrl = (WebControl)this.Page.FindControl(_displayOrHideControlId);
            if (_displayOrHideCtrl == null)
            {
                _displayOrHideCtrl = (WebControl)Utility.FindControlInNamingContainer(this, _displayOrHideControlId);
            }
            if (_displayOrHideCtrl is IButtonControl)
            {
                if (_displayOrHideCtrl is Button)
                {
                    ((Button)_displayOrHideCtrl).UseSubmitBehavior = false;
                }

                ((IButtonControl)_displayOrHideCtrl).Command += new CommandEventHandler(Filtering_Command);
            }
        }

        /// <summary>
        /// ��ʼ���������ӳ�������Դ�ⲿ�ؼ���
        /// </summary>
        private void InitPropertyMappingDataSourceControl()
        {
            if (!string.IsNullOrEmpty(this._propertyMappingDataSourceID))
            {
                Control ctrl = this.Page.FindControl(this._propertyMappingDataSourceID);
                if (ctrl == null)
                {
                    ctrl = Utility.FindControlInNamingContainer(this, this._propertyMappingDataSourceID);
                }
                if (ctrl is ObjectDataSource)
                {
                    _propertyMappingDsCtrl = (DataSourceControl)ctrl;
                    _propertyMappingResult = ((ObjectDataSource)_propertyMappingDsCtrl).Select();
                }
                else if (ctrl is SqlDataSource)
                {
                    _propertyMappingDsCtrl = (DataSourceControl)ctrl;
                    DataSourceSelectArguments dsSelectArg = new DataSourceSelectArguments();
                    _propertyMappingResult = ((SqlDataSource)_propertyMappingDsCtrl).Select(dsSelectArg);
                }
                else
                {
                    throw new Exception("Ŀǰɸѡ�ؼ�ֻ��֧�� ObjectDataSource / SqlDataSource �ؼ���Ϊ����ӳ�������Դ���������� PropertyMappingDataSourceID �Ƿ���ȷ��");
                }
            }
        }

        /// <summary>
        /// ��ʼ��ɸѡ�
        /// </summary>
        private void InitFilterItems()
        {
            if (this._filterCtrl == null || this._dsCtrl == null)
            {
                return;
            }

            this._filterItems = new List<ListItem>();
            foreach (DataControlField aField in this._filterCtrl.Columns)
            {
                ListItem aItem = null;

                if (this._dsCtrl is ObjectDataSource)
                {
                    ObjectDataSource objDs = (ObjectDataSource)_dsCtrl;

                    if (objDs.EnablePaging)
                    {
                        if (!string.IsNullOrEmpty(aField.HeaderText) && !string.IsNullOrEmpty(aField.AccessibleHeaderText))
                        {
                            aItem = new ListItem(aField.HeaderText, aField.AccessibleHeaderText);
                        }
                    }
                    else
                    {
                        System.Reflection.PropertyInfo dataFieldProp = aField.GetType().GetProperty("DataField");
                        if (dataFieldProp != null)
                        {
                            string dataFieldName = (string)dataFieldProp.GetValue(aField, null);
                            if (!string.IsNullOrEmpty(aField.HeaderText) && !string.IsNullOrEmpty(dataFieldName))
                            {
                                aItem = new ListItem(aField.HeaderText, dataFieldName);
                            }
                        }
                    }
                }
                if (this._dsCtrl is SqlDataSource)
                {
                    if (!string.IsNullOrEmpty(aField.HeaderText) && !string.IsNullOrEmpty(aField.AccessibleHeaderText))
                    {
                        aItem = new ListItem(aField.HeaderText, aField.AccessibleHeaderText);
                    }
                }
                if (this._dsCtrl is LinqDataSource)
                {
                    if (!string.IsNullOrEmpty(aField.HeaderText) && !string.IsNullOrEmpty(aField.AccessibleHeaderText))
                    {
                        aItem = new ListItem(aField.HeaderText, aField.AccessibleHeaderText);
                    }
                }

                if (aItem != null)
                {
                    this._filterItems.Add(aItem);
                }
            }
        }

        #endregion

        #region �ؼ�״̬
        /// <summary>
        /// ��ҳ�������ڽ���ʱ�����¿ؼ���״̬
        /// </summary>
        /// <returns></returns>
        protected override object SaveControlState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveControlState());
            list.Add(_lblbasicuserid);
            list.Add(_lbluserentityid);
            list.Add(_controlID);
            list.Add(_dataSourceID);
            list.Add(_propertyMappingDataSourceID);
            list.Add(_displayOrHideControlId);
            list.Add(_doQueryingControlId);

            return list;
        }

        /// <summary>
        /// ��ҳ�������ڵĿ�ʼ����֮ǰ(��Page_Load֮ǰ)���пؼ�״̬�Ļָ�
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadControlState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)savedState;

                if (list.Count >= 8)
                {
                    base.LoadControlState(list[0]);
                    _lblbasicuserid = (string)list[1];
                    _lbluserentityid = (string)list[2];
                    _controlID = (string)list[3];
                    _dataSourceID = (string)list[4];
                    _propertyMappingDataSourceID = (string)list[5];
                    _displayOrHideControlId = (string)list[6];
                    _doQueryingControlId = (string)list[7];
                }
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }
        #endregion 

        #region ��ͼ״̬
        /// <summary>
        ///  ��ҳ�������ڽ���ʱ��������ͼ��״̬(��SaveControlState֮��)
        /// </summary>
        /// <returns></returns>
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
            if (_criteriaTableStyle != null)
            {
                list.Add(
                    ((IStateManager)_criteriaTableStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_filterColumnsStyle != null)
            {
                list.Add(
                    ((IStateManager)_filterColumnsStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_propertyMappingSettings != null)
            {
                list.Add(
                    ((IStateManager)_propertyMappingSettings).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            return list;
        }

         /// <summary>
         /// ��ҳ�������ڵĿ�ʼ����֮ǰ(��Page_Load֮ǰ)���пؼ�״̬�Ļָ�(��LoadControlState֮��)
         /// </summary>
         /// <param name="savedState"></param>
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
                        ((IStateManager)CriteriaTableStyle).LoadViewState(list[2]);
                    }
                    if (list[3] != null)
                    {
                        ((IStateManager)FilterColumnsStyle).LoadViewState(list[3]);
                    }
                    if (list[4] != null)
                    {
                        ((IStateManager)PropertyMappingSettings).LoadViewState(list[4]);
                    }
                }
            }
            else
            {
                base.LoadViewState(savedState);
            }
        }

        /// <summary>
        /// ��ҳ�����֮ǰ������ͼ�ĸ���
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_commandStyle != null)
            {
                ((IStateManager)_commandStyle).TrackViewState();
            }
            if (_criteriaTableStyle != null)
            {
                ((IStateManager)_criteriaTableStyle).TrackViewState();
            }
            if (_filterColumnsStyle != null)
            {
                ((IStateManager)_filterColumnsStyle).TrackViewState();
            }
            if (_propertyMappingSettings != null)
            {
                ((IStateManager)_propertyMappingSettings).TrackViewState();
            }
        }
        #endregion

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.ConstructCommandControl();
            this.ConstructCriteriaTable();
            this.ConstructFilterColumns();

            this.ClearChildViewState();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.ApplyStyle();

            this.AddAttributesToRender(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _tblCriteria.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _btnFilter.RenderControl(writer);
            _btnReset.RenderControl(writer);
            _btnAddCriteria.RenderControl(writer);
            //_btnInsertCriteria.RenderControl(writer);
            _btnDeleteCriteria.RenderControl(writer);
            _btnDisplayFilterColumns.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            _pnlFilterColumns.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.SetControlState();
        }
    }
}
