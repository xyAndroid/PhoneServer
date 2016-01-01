using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Linq;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 和多数据控件提供许多内置功能，这些功能使用户可以对控件中的项进行更新、删除、插入和分页。
    /// 这些数据控件绑定到数据源控件时，可以利用该数据源控件的功能并提供自动更新、删除、插入和分页功能。
    /// 这些数据控件如：FormView、DetailView、GridView。
    /// 该枚举列示了目前支持的命令名称。
    /// </summary>
    public enum CommandName
    {
        /// <summary>
        /// 在更新或插入操作中用于取消操作和放弃用户输入的值。
        /// </summary>
        Cancel,
        /// <summary>
        /// 在删除操作中用于从数据源中删除显示的记录。
        /// </summary>
        Delete,
        /// <summary>
        /// 在更新操作中用于使数据控件处于编辑模式。在 EditItemTemplate 属性中指定的内容是为数据行显示的。
        /// </summary>
        Edit,
        /// <summary>
        /// 在插入操作中用于尝试使用用户提供的值在数据源中插入新记录。引发 ItemInserting 和 ItemInserted 事件。
        /// </summary>
        Insert,
        /// <summary>
        /// 在插入操作中用于使数据控件处于插入模式。在 InsertItemTemplate 属性中指定的内容是为数据行显示的。
        /// </summary>
        New,
        /// <summary>
        /// 在分页操作中用于表示页导航行中执行分页的按钮。若要指定分页操作，请将该按钮的 CommandArgument 属性设置为“Next”、“Prev”、“First”、“Last”或要导航至的目标页的索引。引发 PageIndexChanging 和 PageIndexChanged 事件。
        /// </summary>
        Page,
        /// <summary>
        /// 在更新操作中用于尝试使用用户提供的值更新数据源中所显示的记录。引发 ItemUpdating 和 ItemUpdated 事件。
        /// </summary>
        Update,
    }

    /// <summary>
    /// 设置使能状态的控件及控制属性集合。
    /// </summary>
    public class SetEnableStateControlCollection : List<SetEnableStateControl>
    {
        /// <summary>
        /// 增加添加项目的控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddItemNewControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemNew"));
        }

        /// <summary>
        /// 增加删除项目的控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddItemDeleteControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemDelete"));
        }

        /// <summary>
        /// 增加编辑项目的控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddItemEditControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemEdit"));
        }

        /// <summary>
        /// 增加启用/停用项目的控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddSetUsedFlagControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSetUsedFlag"));
        }

        /// <summary>
        /// 增加导出项目的控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddExportControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableExport"));
        }

        /// <summary>
        /// 增加工具栏控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddToolBarControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableToolBar"));
        }

        /// <summary>
        /// 增加添加附件控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddAttachingControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableAttaching"));
        }

        /// <summary>
        /// 增加审核控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddAuditingControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableAuditing"));
        }

        /// <summary>
        /// 增加返回控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddReturnControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableReturn"));
        }

        /// <summary>
        /// 增加查询控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddSeekControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSeek"));
        }

        /// <summary>
        /// 增加搜索控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddSearchControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSearch"));
        }

        /// <summary>
        /// 增加打印控件。
        /// </summary>
        /// <param name="ctrlId">控件ID</param>
        public void AddPrintControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnablePrint"));
        }
    }

    /// <summary>
    /// 设置使能状态的控件及控制属性。
    /// </summary>
    public class SetEnableStateControl
    {
        /// <summary>
        /// 按照给定的控件和控制属性构造对象。
        /// </summary>
        /// <param name="ctrlId">给定的控件</param>
        /// <param name="propertyName">控制属性</param>
        public SetEnableStateControl(string ctrlId, string propertyName)
        {
            _ctrlId = ctrlId;
            _propName = propertyName;
        }

        private string _ctrlId;
        /// <summary>
        /// 需要设置使能状态的控件。
        /// </summary>
        public string ControlID
        {
            get { return _ctrlId; }
            set { _ctrlId = value; }
        }

        private string _propName;
        /// <summary>
        /// 控制使能状态的属性。
        /// </summary>
        public string PropertyName
        {
            get { return _propName; }
            set { _propName = value; }
        }
    }

    /// <summary>
    /// 提供用户控件的基类。
    /// </summary>
    [Obsolete]
    public partial class BaseUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// 能否新建项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否新建项目。")]
        public bool EnableItemNew
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemNew", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemNew", value);
            }
        }

        /// <summary>
        /// 能否删除项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否删除项目。")]
        public bool EnableItemDelete
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDelete", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDelete", value);
            }
        }

        /// <summary>
        /// 能否修改项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否修改项目。")]
        public bool EnableItemEdit
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemEdit", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemEdit", value);
            }
        }

        /// <summary>
        /// 能否显示项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否显示项目。")]
        public bool EnableItemDisp
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDisp", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDisp", value);
            }
        }

        /// <summary>
        /// 能否显示项目详情。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否显示项目详情。")]
        public bool EnableItemDetail
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDetail", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableItemDetail", value);
            }
        }

        /// <summary>
        /// 能否执行返回。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("能否执行返回。")]
        public bool EnableReturn
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableReturn", true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableReturn", value);
            }
        }
        
        /// <summary>
        /// 能否执行查询项目详情。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("能否执行查询项目详情。")]
        public bool EnableSeek
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSeek", true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSeek", value);
            }
        }

        /// <summary>
        /// 能否执行搜索项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("能否执行搜索项目。")]
        public bool EnableSearch
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSearch", true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSearch", value);
            }
        }

        /// <summary>
        /// 能否执行打印。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("能否执行打印。")]
        public bool EnablePrint
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnablePrint", true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnablePrint", value);
            }
        }

        /// <summary>
        /// 能否启用/停用项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否启用/停用项目。")]
        public bool EnableSetUsedFlag
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSetUsedFlag", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableSetUsedFlag", value);
            }
        }

        /// <summary>
        /// 能否导出项目。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否导出项目。")]
        public bool EnableExport
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableExport", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableExport", value);
            }
        }

        /// <summary>
        /// 能否添加附件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否添加附件。")]
        public bool EnableAttaching
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableAttaching", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableAttaching", value);
            }
        }

        /// <summary>
        /// 能否执行审核。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("行为")]
        [Description("能否执行审核。")]
        public bool EnableAuditing
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableAuditing", false);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableAuditing", value);
            }
        }

        /// <summary>
        /// 能否执行工具栏。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("行为")]
        [Description("能否执行工具栏。")]
        public bool EnableToolBar
        {
            get
            {
                return Utility.GetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableToolBar", true);
            }
            set
            {
                Utility.SetViewStatePropertyValue<bool>(this.ViewState,
                    "EnableToolBar", value);
            }
        }

        /// <summary>
        /// 页面展现的视图模式。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(ViewMode.AllItems)]
        [Category("行为")]
        [Description("页面展现的视图模式。")]
        public ViewMode ViewMode
        {
            get
            {
                return Utility.GetViewStatePropertyValue<ViewMode>(this.ViewState,
                    "ViewMode", ViewMode.AllItems);
            }
            set
            {
                Utility.SetViewStatePropertyValue<ViewMode>(this.ViewState,
                    "ViewMode", value);
            }
        }

        /// <summary>
        /// 以数据表方式展现时行选择模式。
        /// </summary>
        [Browsable(true)]
        [DefaultValue(GridViewRowSelectMode.Multiple)]
        [Category("行为")]
        [Description("以数据表方式展现时行选择模式。")]
        public GridViewRowSelectMode RowSelectMode
        {
            get
            {
                return Utility.GetViewStatePropertyValue<GridViewRowSelectMode>(this.ViewState,
                    "RowSelectMode", GridViewRowSelectMode.Multiple);
            }
            set
            {
                Utility.SetViewStatePropertyValue<GridViewRowSelectMode>(this.ViewState,
                    "RowSelectMode", value);
            }
        }

        /// <summary>
        /// 新建项目的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("新建项目的链接。")]
        public string UrlOfItemNew
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ImageUrlOfItemNew", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ImageUrlOfItemNew", value);
            }
        }

        /// <summary>
        /// 修改项目的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("修改项目的链接。")]
        public string UrlOfItemEdit
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfItemEdit", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfItemEdit", value);
            }
        }

        /// <summary>
        /// 显示项目的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("显示项目的链接。")]
        public string UrlOfItemDisp
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfItemDisp", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfItemDisp", value);
            }
        }

        /// <summary>
        /// 启用/停用项目的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("启用/停用项目的链接。")]
        public string UrlOfSetUsedFlag
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSetUsedFlag", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSetUsedFlag", value);
            }
        }

        /// <summary>
        /// 添加附件的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("添加附件的链接。")]
        public string UrlOfAttaching
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfAttaching", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfAttaching", value);
            }
        }

        /// <summary>
        /// 执行审核的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("执行审核的链接。")]
        public string UrlOfAuditing
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfAuditing", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfAuditing", value);
            }
        }

        /// <summary>
        /// 查看详情的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("查看详情的链接。")]
        public string UrlOfDetail
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfDetail", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfDetail", value);
            }
        }

        /// <summary>
        /// 查询页面的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("查询页面的链接。")]
        public string UrlOfSeek
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSeek", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSeek", value);
            }
        }

        /// <summary>
        /// 搜索页面的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("搜索页面的链接。")]
        public string UrlOfSearch
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSearch", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfSearch", value);
            }
        }

        /// <summary>
        /// 打印页面的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("打印页面的链接。")]
        public string UrlOfPrint
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfPrint", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfPrint", value);
            }
        }

        /// <summary>
        /// 返回的链接。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("链接")]
        [Description("返回的链接。")]
        public string UrlOfReturn
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfReturn", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "UrlOfReturn", value);
            }
        }

        /// <summary>
        /// 接收返回链接的地址栏请求参数名。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ReturnUrl")]
        [Category("行为")]
        [Description("接收返回链接的地址栏请求参数名。")]
        public string ReturnUrlRequestParameterName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ReturnUrlRequestParameterName", "ReturnUrl");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ReturnUrlRequestParameterName", value);
            }
        }

        /// <summary>
        /// 表示主码ID的地址栏请求参数名。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ID")]
        [Category("行为")]
        [Description("表示主码ID的地址栏请求参数名。")]
        public string KeyIDRequestParameterName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "KeyIDRequestParameterName", "ID");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "KeyIDRequestParameterName", value);
            }
        }

        /// <summary>
        /// 导出项目的标题。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("导出")]
        [Description("启用/导出项目的标题。")]
        public string ExportTitle
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ExportTitle", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ExportTitle", value);
            }
        }

        /// <summary>
        /// 导出项目的保存的文件名。
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("导出")]
        [Description("导出项目的保存的文件名。")]
        public string ExportFile
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ExportFile", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ExportFile", value);
            }
        }

        /// <summary>
        /// 保存主码键值。
        /// </summary>
        [Browsable(false)]
        [Category("数据")]
        [Description("保存主码键值。")]
        public object KeyValue
        {
            get
            {
                return Utility.GetViewStatePropertyValue<object>(this.ViewState,
                    "KeyValue", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<object>(this.ViewState,
                    "KeyValue", value);
            }
        }

        private KeyValuePair<string, object> _communicationObj;
        /// <summary>
        /// 用于控件之间通信时交换的数据对象，其中 Key 用于标识通信数据的名称，以区分出不同的通信数据内容，Value 则是具体的通信数据对象。
        /// 该属性主要用于 SendData 发送数据对象，RecieveData 接收数据对象，并做相关的处理。该属性只能用于传递一个通信数据对象，并且会放入 CommunicationObjects 通信数据对象集合的第一个元素。
        /// </summary>
        protected KeyValuePair<string, object> CommunicationObject
        {
            get
            {
                return _communicationObj;
            }
            set
            {
                _communicationObj = value;
            }
        }

        private CommunicationObjectCollection _commObjs;
        /// <summary>
        /// 用于控件之间通信时交换的数据对象，并且是一组对象集合，每个对象为一个 KeyValuePair 的键值对。
        /// </summary>
        protected CommunicationObjectCollection CommunicationObjects
        {
            get
            {
                if (_commObjs == null)
                {
                    _commObjs = new CommunicationObjectCollection();
                }
                return _commObjs;
            }
        }

        private CommunicationMode _commMode = CommunicationMode.QueryString;
        /// <summary>
        /// 获得数据通信的方式。
        /// </summary>
        protected CommunicationMode CommunicationMode
        {
            get
            {
                return _commMode;
            }
            private set
            {
                _commMode = value;
            }
        }

        private SetEnableStateControlCollection _setEnableStateControls;
        /// <summary>
        /// 获得需要设置使能状态的控件集合，可以通过向该集合中添加需要进行使能控制的控件，由基础用户控件来控制这些控件的使能状态。
        /// </summary>
        /// <summary>
        /// 属性映射集合。
        /// </summary>
        [Category("数据")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("属性映射集合。")]
        public SetEnableStateControlCollection SetEnableStateControls
        {
            get
            {
                if (_setEnableStateControls == null)
                {
                    _setEnableStateControls = new SetEnableStateControlCollection();
                }
                return _setEnableStateControls;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 从当前用户控件的视图状态中获得属性值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="defaultValue">属性默认值。</param>
        /// <returns>如果存在属性值则返回该值，否则返回默认值。</returns>
        protected T GetViewStatePropertyValue<T>(string propertyName, T defaultValue)
        {
            return Utility.GetViewStatePropertyValue<T>(this.ViewState, propertyName, defaultValue);
        }

        /// <summary>
        /// 将指定的属性值放入当前用户控件的视图状态中。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="value">属性值。</param>
        protected void SetViewStatePropertyValue<T>(string propertyName, T value)
        {
            Utility.SetViewStatePropertyValue<T>(this.ViewState, propertyName, value);
        }

        /// <summary>
        /// 将项目导出。        
        /// </summary>
        /// <param name="gv">需要导出的 GridView 控件。</param>
        /// <param name="needToHideColumnIndexes">需要在导出的时候隐藏的列索引数组。</param>
        protected virtual void ExportItems(GridView gv, int[] needToHideColumnIndexes)
        {
            Utility.ExportToExcel(gv, needToHideColumnIndexes,
                this.ExportTitle, this.ExportFile);
        }

        /// <summary>
        /// 改变当前的页面展现模式。
        /// </summary>
        /// <param name="formView">FormView控件。</param>
        protected virtual void ChangeViewMode(FormView formView)
        {
            switch (this.ViewMode)
            {
                case eSchool.Web.UI.WebControls.ViewMode.EditForm:
                    formView.ChangeMode(FormViewMode.Edit);
                    break;
                case eSchool.Web.UI.WebControls.ViewMode.NewForm:
                    formView.ChangeMode(FormViewMode.Insert);
                    break;
                case eSchool.Web.UI.WebControls.ViewMode.DispForm:
                    formView.ChangeMode(FormViewMode.ReadOnly);
                    break;
                default:
                    throw new Exception("页面展现模式错误。");
            }
        }

        /// <summary>
        /// 改变当前的页面展现模式。
        /// </summary>
        /// <param name="detailView">DetailView控件。</param>
        protected virtual void ChangeViewMode(DetailsView detailView)
        {
            switch (this.ViewMode)
            {
                case eSchool.Web.UI.WebControls.ViewMode.EditForm:
                    detailView.ChangeMode(DetailsViewMode.Edit);
                    break;
                case eSchool.Web.UI.WebControls.ViewMode.NewForm:
                    detailView.ChangeMode(DetailsViewMode.Insert);
                    break;
                case eSchool.Web.UI.WebControls.ViewMode.DispForm:
                    detailView.ChangeMode(DetailsViewMode.ReadOnly);
                    break;
                default:
                    throw new Exception("页面展现模式错误。");
            }
        }

        /// <summary>
        /// 按照提供的数据项执行命令进行页面跳转。一般在数据控件的“ItemCommand”事件中调用该方法，并且该方法提供一种默认跳转机制，派生类可以根据具体需要重写该方法以实现自己的跳转过程。
        /// </summary>
        /// <param name="cmdName">数据项执行的命令。</param>
        protected virtual void ResponseItemCommand(CommandName cmdName)
        {
            if (cmdName == CommandName.Cancel)
            {
                this.ResponseReturn();
            }
            if (cmdName == CommandName.Edit)
            {
                this.ResponseEditForm();
            }
            if (cmdName == CommandName.New)
            {
                this.ResponseNewForm();
            }
        }

        /// <summary>
        /// 返回请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果地址栏内有<see cref="ReturnUrlRequestParameterName"/>属性指定的参数，则按此参数的地址返回；
        /// （2）如果设置了<see cref="UrlOfReturn"/>属性，则按此属性值的地址返回；
        /// （3）如果上述情况不存在，则按请求页地址返回。
        /// </summary>
        protected virtual void ResponseReturn()
        {
            string returnUrl = this.Request.Params[this.ReturnUrlRequestParameterName];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                this.Response.Redirect(returnUrl, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(this.UrlOfReturn))
                {
                    this.Response.Redirect(this.UrlOfReturn, true);
                }
                else
                {
                    this.Response.Redirect(this.Request.UrlReferrer.AbsoluteUri, true);
                }
            }
        }

        /// <summary>
        /// 新建项目页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfItemNew"/>属性，则按此属性值的地址跳转；
        /// （2）如果上述情况不存在，则按请求页地址跳转。
        /// </summary>
        protected virtual void ResponseNewForm()
        {
            if (!string.IsNullOrEmpty(this.UrlOfItemNew))
            {
                this.Response.Redirect(this.UrlOfItemNew, true);
            }
            else
            {
                this.Response.Redirect(this.Request.UrlReferrer.AbsoluteUri, true);
            }
        }

        /// <summary>
        /// 编辑项目页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfItemEdit"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“SomeEditPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseEditForm()
        {
            if (!string.IsNullOrEmpty(this.UrlOfItemEdit))
            {
                this.ResponseRedirect(this.UrlOfItemEdit,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 显示项目页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfItemDisp"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“SomeDispPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseDispForm()
        {
            if (!string.IsNullOrEmpty(this.UrlOfItemDisp))
            {
                this.ResponseRedirect(this.UrlOfItemDisp,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 启用/停用页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfSetUsedFlag"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“SetUsedFlagPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseSetUsedFlagForm()
        {
            if (!string.IsNullOrEmpty(this.UrlOfSetUsedFlag))
            {
                this.ResponseRedirect(this.UrlOfSetUsedFlag, 
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 详情页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfDetail"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“DetailPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseDetail()
        {
            if (!string.IsNullOrEmpty(this.UrlOfDetail))
            {
                this.ResponseRedirect(this.UrlOfDetail,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 查询页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfSeek"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“SeekPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseSeek()
        {
            if (!string.IsNullOrEmpty(this.UrlOfSeek))
            {
                this.ResponseRedirect(this.UrlOfSeek,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 搜索页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfSearch"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“SearchPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseSearch()
        {
            if (!string.IsNullOrEmpty(this.UrlOfSearch))
            {
                this.ResponseRedirect(this.UrlOfSearch,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 添加附件页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfAttaching"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“AttachingPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseAttaching()
        {
            if (!string.IsNullOrEmpty(this.UrlOfAttaching))
            {
                this.ResponseRedirect(this.UrlOfAttaching,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 审核页请求。
        /// 按照以下顺序优先执行返回：
        /// （1）如果设置了<see cref="UrlOfAuditing"/>属性，则按此属性值的地址跳转；
        /// 在跳转的时候会将<see cref="KeyIDRequestParameterName"/>属性和<see cref="KeyValue"/>属性作为依据，即在地址栏内形成如同“AuditPage.aspx?ID=1”的请求。
        /// </summary>
        protected virtual void ResponseAuditing()
        {
            if (!string.IsNullOrEmpty(this.UrlOfAuditing))
            {
                this.ResponseRedirect(this.UrlOfAuditing,
                    new System.Collections.Generic.KeyValuePair<string, object>(this.KeyIDRequestParameterName, this.KeyValue));
            }
        }

        /// <summary>
        /// 请求页。
        /// </summary>
        /// <param name="url">原请求页地址。</param>
        /// <param name="requestParams">请求参数列表。</param>
        protected virtual void ResponseRedirect(string url, params System.Collections.Generic.KeyValuePair<string, object>[] requestParams)
        {
            if (requestParams == null)
            {
                this.Response.Redirect(url, true);
            }
            else
            {
                this.Response.Redirect(Utility.GetUrlString(url, requestParams) , true);
            }
        }

        /// <summary>
        /// 向其他的用户控件发送数据。
        /// </summary>
        /// <param name="reciever">接受数据的用户控件。</param>
        /// <param name="data">发送的数据。</param>
        public void SendData(BaseUserControl reciever, object data)
        {
            reciever.RecieveData(data);
        }

        private const string RecieverIDSessionName = "__RecieverID__";
        private const string CommunicationObjectsSessionName = "__CommunicationObjects__";
        /// <summary>
        /// 在不同页面之间传递数据进行通信。
        /// </summary>
        /// <param name="url">跳转页面的Url。</param>
        /// <param name="recieverId">接受方对象的ID。</param>
        /// <param name="data">需要发送的数据对象。</param>
        /// <remarks>该方法将自动跳转页面，并且将传递的数据放入会话中，同时 CommunicationMode 被标识为 Session，并在跳转后的页面的 PreLoad 事件中调用 RecieveData 方法接受数据。接受对象需要重写 ReceiveData ，以接受并处理数据。</remarks>
        public void SendData(string url, string recieverId, object data)
        {
            this.Session[RecieverIDSessionName] = recieverId;
            this.Session[CommunicationObjectsSessionName] = data;
            this.ResponseRedirect(url);
        }

        /// <summary>
        /// 接受其他用户发送的数据，并且处理数据。这需要派生类去重写该方法实现具体的处理。
        /// </summary>
        /// <param name="data">接受的数据。</param>
        protected virtual void RecieveData(object data)
        {
            if (data is CommunicationObjectCollection)
            {
                this.CommunicationObjects.AddRange((CommunicationObjectCollection)data);
            }
        }

        /// <summary>
        /// 主要提供给派生用户控件重写，按照 SetEnableStateControls 中指定的控件使能设置集合，设置控件的使能状态。
        /// 该方法会被页面的 PreRender 事件方法调用。
        /// </summary>
        protected virtual void SetControlEnableState()
        {
            foreach (SetEnableStateControl setEnableStateCtrl in this.SetEnableStateControls)
            {
                if (setEnableStateCtrl == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(setEnableStateCtrl.ControlID) || string.IsNullOrEmpty(setEnableStateCtrl.PropertyName))
                {
                    continue;
                }

                Control aCtrl = this.FindControl(setEnableStateCtrl.ControlID);

                if (aCtrl == null)
                {
                    continue;
                }

                System.Reflection.PropertyInfo aPropInfo = this.GetType().GetProperty(setEnableStateCtrl.PropertyName);

                if (aPropInfo == null || !aPropInfo.CanRead || !aPropInfo.PropertyType.Equals(typeof(bool)))
                {
                    continue;
                }

                aCtrl.Visible = (bool)aPropInfo.GetValue(this, null);
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Page.PreLoad += new EventHandler(Page_PreLoad);
            this.Page.PreRender += new EventHandler(Page_PreRender);

        }

        void Page_PreLoad(object sender, EventArgs e)
        {
            string recieverId = (string)this.Session[RecieverIDSessionName];
            if (!string.IsNullOrEmpty(recieverId) && recieverId.Equals(this.ID))
            {
                this.CommunicationMode = CommunicationMode.Session;
                this.RecieveData(this.Session[CommunicationObjectsSessionName]);
                this.Session.Remove(CommunicationObjectsSessionName);
                this.Session.Remove(RecieverIDSessionName);
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            this.SetControlEnableState();
        }
    }
}
