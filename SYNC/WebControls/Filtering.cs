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
    /// 筛选控件。
    /// </summary>
    /// <remarks>
    /// 筛选控件要能够正常执行需要进行以下的配置工作：
    /// （1）第一种情况：筛选过程的执行由 DataSet 的 FilterExpression 引擎完成。
    ///     a)  配置 ObjectDataSource / SqlDataSource，与普通配置过程一样；
    ///     b)  配置 GridView，与普通配置过程一样；
    ///     c)  如果数据源为 SqlDataSource，则 GridView 需要为每个 Column 配置“AccessibleHeaderText”属性，
    ///         例如：“AccessibleHeaderText”配置为“StudentNo,N”，表示该 Column 提供的筛选表达式的类型为数字型，具体参见第二种情况。
    /// （2）第二种情况：筛选过程的执行由后台 SQL 引擎完成。
    ///     a)  配置 ObjectDataSource，启用 EnablePaging，并正确配置“SelectCountMethod”，“StartRowIndexParameterName”，“MaximumRowsParameterName”三个属性；
    ///     b)  配置 GridView，启用 Paging，并为每个 Column 正确配置“AccessibleHeaderText”，“HeaderText”两个属性，
    ///         其中“AccessibleHeaderText”为筛选项，“HeaderText”为筛选项在筛选控件中筛选项列表中显示的友好文本，
    ///         对于“AccessibleHeaderText”由两部分构成，筛选项表达式和表达式类型，
    ///         表达式类型指“S”，“N”，“D”三种，分别代表筛选项表达式为“文本型”，“数字型”，“日期时间型”，其中“S”可以省略，
    ///         例如：“Student.Name,S”，
    ///         一般筛选项表达式为 SQL 字段表达式，当 SQL 的 From 子句包含多张表，字段前一定要加上表名前缀；
    ///     c)  配置 Filtering，设置属性“FilterParameterName”为数据源控件配置的 SelectMethod 中的可以接受的筛选表达式参数，
    ///         例如：SelectMethod 的原型 “public DSStudent.StudentDataTable GetStudents(string filterExp, string sortExp, int? startRowIndex, int? maximumRows)”，
    ///         其中的“filterExp”为“FilterParameterName”，但是该参数是否参与执行筛选过程，则是由 SelectMethod 的作者决定的。
    /// （3）第三种情况：筛选过程的执行由 Linq 完成。
    ///     a)  配置 LinqDataSource，与普通配置过程一样；
    ///     b)  配置 GridView，启用 Paging，并为每个 Column 正确配置“AccessibleHeaderText”，“HeaderText”两个属性，
    ///         其中“AccessibleHeaderText”为筛选项，“HeaderText”为筛选项在筛选控件中筛选项列表中显示的友好文本，
    ///         对于“AccessibleHeaderText”由两部分构成，筛选项表达式和表达式类型，
    ///         表达式类型指“S”，“N”，“D”三种，分别代表筛选项表达式为“文本型”，“数字型”，“日期时间型”，其中“S”可以省略，
    ///         例如：“Student.Name{0}{1},S”，“Students.Count(Name{0}{1}) > 0”
    ///         一般筛选项表达式为 Linq where 子句支持的普通语言表达式，但是需要为筛选值给定两个占位符，第一个表示筛选比较运算符，第二个表示筛选值。
    /// （4）配置属性映射。
    ///     属性映射是指在数据源中有一些字段是用一些代码表示，如：AuditFlag 字段用“0”和“1”代表“拒绝”与“通过”，
    ///     但是用户在输入筛选条件的筛选值的时候并不知道上述代码表示，而是直接输入具有友好内容的文本，如：“拒绝”与“通过”，
    ///     此时，筛选控件提供在友好文本和代码之间进行对应转换处理，即根据用户输入的友好文本，将其对应到代码上，如：“0”和“1”。
    ///     属性映射的配置：
    ///     a)  配置 PropertyMappingDataSourceID 属性，指定一个为属性映射转换的数据源控件ID；
    ///     b)  配置 PropertyMappings 集合属性，指定需要执行属性映射转换的字段，通过指定每个属性映射的“PropertyName”和“FilterItem”属性将转换对应起来，
    ///         “PropertyName”是指在属性映射数据源中属性的名称，如：“AuditFlag”，
    ///         “FilterItem”是指在筛选项列表中需要进行属性映射转换的筛选项，如果筛选执行引擎为后台 SQL 则设置为 AccessibleHeaderText 中的筛选表达式，
    ///         但不包括筛选表达式类型，如：“AccessibleHeaderText”为“Audit.AuditFlag,N”，则“FilterItem”应设置成“Audit.AuditFlag”；
    ///     c)  配置 PropertyMappingSetting 复合属性，指定在属性映射数据源中用来查找相关属性映射内容的字段名称，
    ///         “RefPropertyName”指定属性映射数据源中属性映射名，
    ///         “RefPropertyValue”指定属性映射数据源中属性映射值，
    ///         “RefPropertyMeaning”指定属性映射数据源中属性对应含义的友好文本。
    /// </remarks>
    [DefaultProperty("ControlID")]
    [ParseChildren(true, "PropertyMappings")]
    [System.Drawing.ToolboxBitmap(typeof(Filtering), "Filtering.bmp")]
    [ToolboxData("<{0}:Filtering runat=server></{0}:Filtering>")]
    public class Filtering : CompositeControl
    {
        #region Fields
        /// <summary>
        /// 李志鹏增 (获取动态的数据源)
        /// </summary>
        private DataView _dv;

        /// <summary>
        /// 需要筛选数据的控件，目前只支持 GridView。
        /// </summary>
        private GridView _filterCtrl;
        /// <summary>
        /// 作为筛选数据来源的数据源控件，目前只支持 ObjectDataSource。
        /// </summary>
        private DataSourceControl _dsCtrl;
        /// <summary>
        /// 表示层次结构数据的数据源控件，这是用来执行属性映射筛选的。
        /// </summary>
        private DataSourceControl _propertyMappingDsCtrl;
        /// <summary>
        /// 获得的属性映射结果。
        /// </summary>
        private IEnumerable _propertyMappingResult;
        /// <summary>
        /// 筛选项的集合，每个筛选项是一个 ListItem 对象，
        /// 其中 ListItem.Text 属性记录显示在筛选项列表中的友好文本，一般是配置在 GridView 的每个 Column 的 HeaderText 中的文本，
        /// 而其 ListItem.Value 属性记录则记录了筛选项的构造文本，
        ///     （1）如果数据源控件启用了分页 SQL 处理，则该属性是配置在 GridView 的每个 Column 的 AccessibleHeaderText 中的文本，
        ///          并且由两部分构成：表示筛选项的表达式文本，表达式计算结果类型，分为“S”、“N”、“D” 三种。
        ///          "S"表示结果为文本类型，可以省略；
        ///          "N"表示结果为数字类型；
        ///          "D"表示结果为日期类型。
        ///          Sample: 
        ///             ListItem("姓名", "Student.Name,S")，“姓名”将作为友好文本显示在筛选项列表中，“”则作为筛选项的真正构造文本。
        ///     （2）如果数据源控件没有启用分页 SQL 处理，则筛选过程发生在 DataSet 中。
        /// </summary>
        private System.Collections.Generic.List<ListItem> _filterItems;

        /// <summary>
        /// 设置筛选控件显示/隐藏的外部控件。
        /// </summary>
        private Control _displayOrHideCtrl;

        /// <summary>
        /// 执行查询的外部控件。
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
        /// 接受参数值_BID的控件ID
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("设置接受BasicUser表中ID参数的Label的ID。")]
        public string LableBasicUserID
        {
            get { return _lblbasicuserid; }
            set { _lblbasicuserid = value; }
        }

        private string _lbluserentityid = string.Empty;
        /// <summary>
        /// 接受参数_UEID的控件ID
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("设置接受User_Entity表中ID的参数的Label的ID")]
        public string LabelUserEntityID
        {
            get { return _lbluserentityid; }
            set { _lbluserentityid = value; }
        }


        private string _controlID = string.Empty;
        /// <summary>
        /// 用于执行筛选的控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("用于执行筛选的控件ID。")]
        public string ControlID
        {
            get { return _controlID; }
            set { _controlID = value; }
        }

        private string _dataSourceID = string.Empty;
        /// <summary>
        /// 用于筛选数据来源的数据源控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("用于筛选数据来源的数据源控件ID。")]
        public string DataSourceID
        {
            get { return _dataSourceID; }
            set { _dataSourceID = value; }
        }

        private string _propertyMappingDataSourceID = string.Empty;
        /// <summary>
        /// 用于获得属性映射的数据源控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty()]
        [TypeConverter(typeof(System.Web.UI.Design.DataSourceConverter))]
        [DefaultValue("")]
        [Category("数据")]
        [Description("用于获得属性映射的数据源控件ID。")]
        public string PropertyMappingDataSourceID
        {
            get { return _propertyMappingDataSourceID; }
            set { _propertyMappingDataSourceID = value; }
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
        [Description("显示和隐藏筛选控件的外部控件ID。")]
        public string DisplayOrHideControlID
        {
            get { return _displayOrHideControlId; }
            set { _displayOrHideControlId = value; }
        }

        private string _doQueryingControlId = string.Empty;
        /// <summary>
        /// 执行查询的外部控件ID。
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue("")]
        [Category("行为")]
        [Description("执行查询的外部控件ID。")]
        public string DoQueryingControlID
        {
            get { return _doQueryingControlId; }
            set { _doQueryingControlId = value; }
        }

        private const string m_DoQueryingControlTextViewStateName = "DoQueryingControlText";
        /// <summary>
        /// 执行查询的外部控件显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("查询")]
        [Category("外观")]
        [Description("执行查询的外部控件显示文本。")]
        public string DoQueryingControlText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DoQueryingControlTextViewStateName, "查询");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DoQueryingControlTextViewStateName, value);
            }
        }

        private const string m_IsHidingDoQueryingControlViewStateName = "IsHidingDoQueryingControl";
        /// <summary>
        /// 当出现筛选控件的时候是否隐藏执行查询的外部控件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("当出现筛选控件的时候是否隐藏执行查询的外部控件。")]
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
        /// 筛选命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("查询")]
        [Category("命令")]
        [Description("筛选命令的显示文本。")]
        public string FilterCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterCommandTextViewStateName, "查询");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterCommandTextViewStateName, value);
            }
        }

        private const string m_ResetCommandTextViewStateName = "ResetCommandText";
        /// <summary>
        /// 重置命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("重置")]
        [Category("命令")]
        [Description("重置命令的显示文本。")]
        public string ResetCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_ResetCommandTextViewStateName, "重置");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_ResetCommandTextViewStateName, value);
            }
        }

        private const string m_AddCriteriaCommandTextViewStateName = "AddCriteriaCommandText";
        /// <summary>
        /// 添加条件命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("添加条件")]
        [Category("命令")]
        [Description("添加条件命令的显示文本。")]
        public string AddCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_AddCriteriaCommandTextViewStateName, "添加条件");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_AddCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_InsertCriteriaCommandTextViewStateName = "InsertCriteriaCommandText";
        /// <summary>
        /// 插入条件命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("插入条件")]
        [Category("命令")]
        [Description("插入条件命令的显示文本。")]
        public string InsertCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_InsertCriteriaCommandTextViewStateName, "插入条件");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_InsertCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_DeleteCriteriaCommandTextViewStateName = "DeleteCriteriaCommandText";
        /// <summary>
        /// 删除条件命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("删除条件")]
        [Category("命令")]
        [Description("删除条件命令的显示文本。")]
        public string DeleteCriteriaCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DeleteCriteriaCommandTextViewStateName, "删除条件");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DeleteCriteriaCommandTextViewStateName, value);
            }
        }

        private const string m_DisplayFilterColumnsCommandTextViewStateName = "DisplayFilterColumnsCommandText";
        /// <summary>
        /// 显示筛选列命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("筛选列")]
        [Category("命令")]
        [Description("显示筛选列命令的显示文本。")]
        public string DisplayFilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_DisplayFilterColumnsCommandTextViewStateName, "筛选列");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_DisplayFilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_FilterColumnsCommandTextViewStateName = "FilterColumnsCommandText";
        /// <summary>
        /// 设置筛选列命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("确定")]
        [Category("命令")]
        [Description("设置筛选列命令的显示文本。")]
        public string FilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterColumnsCommandTextViewStateName, "确定");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_FilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_CancelFilterColumnsCommandTextViewStateName = "CancelFilterColumnsCommandText";
        /// <summary>
        /// 取消筛选列命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("取消")]
        [Category("命令")]
        [Description("取消筛选列命令的显示文本。")]
        public string CancelFilterColumnsCommandText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_CancelFilterColumnsCommandTextViewStateName, "取消");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_CancelFilterColumnsCommandTextViewStateName, value);
            }
        }

        private const string m_DisplayCommandTextViewStateName = "DisplayCommandText";
        /// <summary>
        /// 显示/隐藏命令的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("筛选")]
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
                return "筛选";
            }
            set
            {
                this.ViewState[m_DisplayCommandTextViewStateName] = value;
            }
        }

        private const string m_HideCommandTextViewStateName = "HideCommandText";
        /// <summary>
        /// 显示/隐藏命令的隐藏文本。
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

        private const string m_ShowHeaderViewStateName = "ShowHeader";
        /// <summary>
        /// 是否显示筛选表格表头。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("外观")]
        [Description("是否显示筛选表格表头。")]
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
        /// 空白筛选项的显示文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("--请选择筛选项--")]
        [Category("外观")]
        [Description("空白筛选项的显示文本。")]
        public string BlankFilterItemText
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemTextViewStateName, "--请选择筛选项--");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    m_BlankFilterItemTextViewStateName, value);
            }
        }

        private const string m_BlankFilterItemValueViewStateName = "BlankFilterItemValue";
        /// <summary>
        /// 空白筛选项的查询文本。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("外观")]
        [Description("空白筛选项的查询文本。")]
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
        /// 设置能否筛选列。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("设置能否筛选列。")]
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
        /// 执行筛选的方法参数名，一般是数据源控件中配置的Select方法的参数名。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("filterExp")]
        [Category("行为")]
        [Description("执行筛选的方法参数名。")]
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
        /// 筛选条件表格的样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("筛选条件表格的样式。")]
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
        /// 筛选列控件的样式。
        /// </summary>
        [Browsable(true)]
        [Category("样式")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("筛选列控件的样式。")]
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
        /// 在隐藏筛选控件的同时是否重置筛选条件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("在隐藏筛选控件的同时是否重置筛选条件。")]
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
        /// 是否在初始的时候检索数据。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("是否在初始的时候检索数据。")]
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
        /// 属性映射集合。
        /// </summary>
        [Category("数据")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("属性映射集合。")]
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
        /// 属性映射设置。
        /// </summary>
        [Browsable(true)]
        [Category("数据")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("属性映射设置。")]
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
        /// 为每一个筛选条件设置一个唯一的编号，以便在回发请求的时候重构筛选条件。
        /// </summary>
        [Browsable(false)]
        [Description("记录条件编号。")]
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
        /// 当执行筛选的时候发生该事件。
        /// </summary>
        [Browsable(true)]
        [Category("操作")]
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
        /// 设置显示/隐藏外部控件的状态，主要是交替更换控件显示文本。
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

        #region 数据源筛选事件

        /// <summary>
        /// 计算并设置筛选表达式。
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
        /// 计算并设置筛选表达式。
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
        /// 计算并设置筛选表达式。
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
        /// 计算并设置筛选表达式。
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
        /// 构造筛选条件表格。
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
        /// 构建筛选条件表格的表头。
        /// </summary>
        private void ConstructCriteriaTableHeader()
        {
            TableHeaderRow tblHeader = new TableHeaderRow();

            TableHeaderCell tblHeaderCell = new TableHeaderCell();
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "查询项";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "比较";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "值";
            tblHeader.Cells.Add(tblHeaderCell);

            tblHeaderCell = new TableHeaderCell();
            tblHeaderCell.Text = "逻辑";
            tblHeader.Cells.Add(tblHeaderCell);

            _tblCriteria.Rows.Add(tblHeader);
        }

        /// <summary>
        /// 记录在当前页请求周期中从条件编号列表中恢复过来的编号对应的列表下标。
        /// </summary>
        private int iCriteriaNo = 0;

        /// <summary>
        /// 构造空白的筛选条件行。
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
        /// 构造命令控件。
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
        /// 构造筛选列控件。
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
                    _chkBoxLstFilterColumns.Items.Add(new ListItem("筛选列" + i.ToString()));
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
        /// 重置筛选条件行。
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
        /// 添加筛选条件行。
        /// </summary>
        private void AddCriteriaTableRow()
        {
            _tblCriteria.Rows.Add(ConstructBlankCriteriaTableRow());
        }

        /// <summary>
        /// 插入筛选条件行。
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
        /// 删除筛选条件行。
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
        /// 设置筛选列相关控件状态。
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
        /// 执行筛选列。
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
        /// 执行筛选。
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
        /// 计算筛选表达式。
        /// 计算步骤：
        /// （1）通过遍历筛选条件表格中的每一行，得到“筛选项”，“比较运算符”，“筛选值”，“筛选逻辑”。
        /// （2）然后将上述得到的四项串接成一条合法的筛选表达式，例如：“Student.Name LIKE '%张%' AND”。
        /// （3）接着将遍历到的所有串接的筛选表达式全部串接成最终的合法筛选表达式。
        ///       例如：“Student.Name LIKE '%张%' AND Student.Gender = '男'”。
        /// 需要注意：
        /// 最终表达式是否合法，需要依赖执行筛选表达式的引擎程序和对 GridView 这样需要控制筛选过程的外部控件的配置情况。
        /// 例如：执行筛选表达式的引擎程序包括“DataSet的FilterExpression”和“SQL”。
        /// 例如：GridView 的 Column 的 HeaderText，AccessibleHeaderText 等属性配置是否正确。
        /// </summary>
        /// <returns>计算得到的筛选表达式。</returns>
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

                    //  获得筛选项的表达式和类型。
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

                    //  获得比较运算符。
                    string comparer = string.Empty;

                    DropDownList ddlstComparer = (DropDownList)aRow.Cells[2].Controls[0];
                    if (!string.IsNullOrEmpty(ddlstComparer.SelectedValue))
                    {
                        comparer = ddlstComparer.SelectedValue;
                    }

                    //  获得筛选值。
                    string filterValue = string.Empty;

                    TextBox txtValue = (TextBox)aRow.Cells[3].Controls[0];
                    if (!string.IsNullOrEmpty(txtValue.Text))
                    {
                        filterValue = txtValue.Text.Trim();
                    }

                    //  获得筛选逻辑。
                    string logic = Constants.Logic.And;

                    DropDownList ddlstLogic = (DropDownList)aRow.Cells[4].Controls[0];
                    if (!string.IsNullOrEmpty(ddlstLogic.SelectedValue))
                    {
                        logic = ddlstLogic.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        //  如果存在属性映射则修正筛选值。
                        filterValue = GetPropertyFilterValue(fieldName, filterValue);

                        //  验证并修正筛选值。
                        filterValue = Constants.FieldType.Validate(filterValue, fieldType);

                        //  串接表达式。

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
        /// 为指定的筛选项（字段）转换属性映射对应的属性值。
        /// </summary>
        /// <param name="fieldName">指定的筛选项（字段）</param>
        /// <param name="filterValue">待转换属性映射的源值，一般是用户通过筛选控件输入的友好文本</param>
        /// <returns>得到转换后的属性映射对应的属性值，如果没有转换成功，则返回原始值。</returns>
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
        /// 设置筛选条件表格状态。
        /// </summary>
        private void SetCriteriaTableState()
        {
            if (_tblCriteria != null && _tblCriteria.Rows.Count > 0 && _tblCriteria.Rows[0] is TableHeaderRow)
            {
                _tblCriteria.Rows[0].Visible = this.ShowHeader;
            }
        }

        /// <summary>
        /// 设置记录筛选条件编号列表属性。
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
        /// 重载Page_Init事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this); //注册控件可进行控件状态的保存和恢复
            InitControl();
        }
        #region 初始化
        /// <summary>
        /// 初始化该控件。
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
        /// 设置Label的值
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
        /// 李志鹏增
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
        /// 李志鹏增(填充查询项中的DropDownList)
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
        /// 初始化执行筛选的数据源外部控件。
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
                    throw new Exception("目前筛选控件只能支持 ObjectDataSource / SqlDataSource / LinqDataSource 控件，或者请检查 DataSourceID 是否正确。");
                }
            }
        }

        /// <summary>
        /// 初始化需要控制筛选的外部控件。
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
                    //李志鹏修改
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
                    throw new Exception("目前筛选控件只能支持 GridView 控件，或者请检查 ControlID 是否正确。");
                }
            }
        }

        /// <summary>
        /// 初始化执行查询的外部控件。
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
        /// 初始化显示/隐藏外部控件。
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
        /// 初始化获得属性映射的数据源外部控件。
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
                    throw new Exception("目前筛选控件只能支持 ObjectDataSource / SqlDataSource 控件作为属性映射的数据源，或者请检查 PropertyMappingDataSourceID 是否正确。");
                }
            }
        }

        /// <summary>
        /// 初始化筛选项。
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

        #region 控件状态
        /// <summary>
        /// 在页声明周期结束时保存下控件的状态
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
        /// 在页生命周期的开始加载之前(即Page_Load之前)进行控件状态的恢复
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

        #region 视图状态
        /// <summary>
        ///  在页声明周期结束时保存下视图的状态(在SaveControlState之后)
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
         /// 在页生命周期的开始加载之前(即Page_Load之前)进行控件状态的恢复(在LoadControlState之后)
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
        /// 在页面加载之前进行视图的跟踪
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
