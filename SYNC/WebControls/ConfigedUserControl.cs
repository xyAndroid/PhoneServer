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
    /// 为用户提供的页面视图模式。
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// 提供列表的模式。
        /// </summary>
        AllItems,
        /// <summary>
        /// 提供只读表单的模式。
        /// </summary>
        DispForm,
        /// <summary>
        /// 提供插入表单的模式。
        /// </summary>
        NewForm,
        /// <summary>
        /// 提供编辑表单的模式。
        /// </summary>
        EditForm,
    }

    /// <summary>
    /// 数据通信的方式。该枚举主要用于 SendData 和 RecieveData。
    /// </summary>
    public enum CommunicationMode
    {
        /// <summary>
        /// 不存在通信。
        /// </summary>
        None,
        /// <summary>
        /// 地址栏参数方式。
        /// </summary>
        QueryString,
        /// <summary>
        /// 会话方式。
        /// </summary>
        Session,
    }

    /// <summary>
    /// 页请求有效性。
    /// </summary>
    public enum PageEffective
    {
        /// <summary>
        /// 始终有效。
        /// </summary>
        Always,
        /// <summary>
        /// 初始页请求。
        /// </summary>
        Initial,
        /// <summary>
        /// 回发页请求。
        /// </summary>
        PostBack,
    }

    /// <summary>
    /// 用于控件之间进行通信，传递的数据对象集合。
    /// </summary>
    public class CommunicationObjectCollection : Dictionary<string, object>
    {
        /// <summary>
        /// 添加一批数据对象。
        /// </summary>
        /// <param name="commObjs">数据对象集合。</param>
        public void AddRange(IEnumerable<KeyValuePair<string, object>> commObjs)
        {
            foreach (KeyValuePair<string, object> keyValue in commObjs)
            {
                if (!this.ContainsKey(keyValue.Key))
                {
                    this.Add(keyValue.Key, keyValue.Value);
                }
            }
        }

        /// <summary>
        /// 添加一个数据对象到集合中。
        /// </summary>
        /// <param name="keyValue">数据对象。</param>
        public void Add(KeyValuePair<string, object> keyValue)
        {
            if (!this.ContainsKey(keyValue.Key))
            {
                this.Add(keyValue.Key, keyValue.Value);
            }
        }

        /// <summary>
        /// 通过通信数据的名称键值查找通信数据对象。
        /// </summary>
        /// <param name="key">通信数据的名称键值。</param>
        /// <returns>如果存在指定名称键值的通信数据对象则返回该数据对象，否则返回空值。</returns>
        public object FindByKey(string key)
        {
            if (this.ContainsKey(key))
            {
                return base[key];
            }
            return null;
        }

        /// <summary>
        /// 通过通信数据的名称键值查找通信数据对象。
        /// </summary>
        /// <param name="key">通信数据的名称键值。</param>
        /// <returns>如果存在指定名称键值的通信数据对象则返回该数据对象，否则返回空值。</returns>
        public new object this[string key]
        {
            get
            {
                return this.FindByKey(key);
            }
            set
            {
                base[key] = value;
            }
        }

        /// <summary>
        /// 如果通信数据对象是一组数据，并被表示成“,”分割的字符串，通过通信数据的名称键值查找并转换第一个通信数据对象。
        /// </summary>
        /// <param name="key">通信数据的名称键值。</param>
        /// <param name="index">指示返回指定索引号的通信数据对象。</param>
        /// <returns>如果存在该通信数据对象，并且被表示成“,”分割的字符串，则返回该数据通信对象的第一个，否则返回空值。</returns>
        public object this[string key, int index]
        {
            get
            {
                object ret = this[key];

                if (ret is List<object>)
                {
                    List<object> list = (List<object>)ret;

                    if (index >= 0 && index < list.Count)
                    {
                        return list[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                return this[key];
            }
        }

        /// <summary>
        /// 将通信数据复制到新数组中。
        /// </summary>
        /// <returns>包含通信数据的数组。</returns>
        public KeyValuePair<string, object>[] ToArray()
        {
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();

            foreach (KeyValuePair<string, object> aKeyValue in this)
            {
                list.Add(aKeyValue);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获得某个通信对象的列表数据。如果某个通信对象为一组数据，那么将会被表示成以“,”分割的字符串，通过该方法可以将其还原为列表形式的一组数据。
        /// </summary>
        /// <param name="keyName">通信对象的键名。</param>
        /// <param name="type">通信对象的类型。</param>
        /// <returns>如果通信对象是以“,”分割的字符串，则返回还原后的数据列表，否则返回一个空的列表。</returns>
        internal List<object> GetCommunicationObjectList(string keyName, Type type)
        {
            List<object> list = new List<object>();

            object keyValue = this[keyName];

            string strCommObj = null;

            if (keyValue is string)
            {
                strCommObj = (string)keyValue;
            }

            if (!string.IsNullOrEmpty(strCommObj))
            {
                string[] strCommObjArr =
                    strCommObj.Split(
                    System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string aStrCommObj in strCommObjArr)
                {
                    list.Add(
                        Convert.ChangeType(aStrCommObj, type));
                }
            }

            return list;
        }
    }

    /// <summary>
    /// 数据通信的方向。
    /// </summary>
    public enum CommunicationDirection
    {
        /// <summary>
        /// 发送方向。
        /// </summary>
        Send,
        /// <summary>
        /// 接收方向。
        /// </summary>
        Receive,
        /// <summary>
        /// 双向，既接收数据又发送数据。
        /// </summary>
        Bidirect,
    }

    /// <summary>
    /// 页为返回后需要恢复状态保留下来的特征枚举。
    /// </summary>
    public enum BaseSavedPageFeature
    {
        /// <summary>
        /// 当前分页索引。
        /// </summary>
        PageIndex,
        /// <summary>
        /// 当前每页行数。
        /// </summary>
        PageSize,
        /// <summary>
        /// 排序表达式。
        /// </summary>
        Sort,
        /// <summary>
        /// 排序方向。
        /// </summary>
        SortDir,
        /// <summary>
        /// 筛选表达式。
        /// </summary>
        Filter,
    }

    /// <summary>
    /// 使能模式。
    /// </summary>
    public enum EnableMode
    {
        /// <summary>
        /// 按照可见性使能。
        /// </summary>
        Visible,
        /// <summary>
        /// 按照使能性使能。
        /// </summary>
        Enabled,
    }

    /// <summary>
    /// 行选择模式。
    /// </summary>
    public enum RowSelectMode
    {
        /// <summary>
        /// 没有做任何行选择模式。
        /// </summary>
        None,
        /// <summary>
        /// 单行选择。
        /// </summary>
        Single,
        /// <summary>
        /// 多行选择。
        /// </summary>
        Multiple,
    }

    /// <summary>
    /// 权限检查点。
    /// </summary>
    public enum CheckPoint
    {
        /// <summary>
        /// 用户控件加载的时候。
        /// </summary>
        UserControlLoad,
        /// <summary>
        /// 执行控件命令的时候。
        /// </summary>
        ExecuteControlCommand,
        /// <summary>
        /// 控件呈现的时候。
        /// </summary>
        ControlRender,
    }

    /// <summary>
    /// 权限检查失败的时候所采取的处理模式。
    /// </summary>
    public enum CheckFailureMode
    {
        /// <summary>
        /// 系统不采取任何处理。
        /// </summary>
        None,
        /// <summary>
        /// 系统抛出安全异常。
        /// </summary>
        ThrowException,
        /// <summary>
        /// 系统自动隐藏控件。
        /// </summary>
        HideControl,
        /// <summary>
        /// 系统自动使控件不可用。
        /// </summary>
        DisableControl,
    }

    /// <summary>
    /// 权限检查规则。
    /// </summary>
    public class CheckPermissionRule
    {
        /// <summary>
        /// 获取检查对象。
        /// </summary>
        public Object CheckObject
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取检查点。
        /// </summary>
        public CheckPoint CheckPoint
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取与设置检查失败所采取的处理模式。
        /// </summary>
        public CheckFailureMode CheckFailureMode
        {
            get;
            set;
        }
        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="checkPoint">检查点。</param>
        /// <param name="mode">检查失败处理模式。</param>
        /// <param name="checkObj">检查对象。</param>
        public CheckPermissionRule(CheckPoint checkPoint, CheckFailureMode mode, Object checkObj)
        {
            CheckPoint = checkPoint;
            CheckFailureMode = mode;
            CheckObject = checkObj;
        }
    }

    /// <summary>
    /// 表示数据键值集合。该集合实际上为一张二维表，集合中的每个元素对象相当于一条记录，其类型为 DataKey。
    /// 如果要访问一条记录中的某个字段，通过二维数组的方式可以获得。
    /// 如：dataKeys[0][1]表示第一条记录的第二个字段的数据，或者 dataKeys[0]["Name"]，其中“Name”为字段名。
    /// 该类主要用于定义 ConfigedUserControl 的 DataKeys 属性。
    /// </summary>
    public class DataKeyCollection : List<DataKey>
    {
        /// <summary>
        /// 以若干键值对作为一条记录添加到数据键值集合中。
        /// </summary>
        /// <param name="dataKey">若干键值对。键表示字段名，值表示该字段的值。</param>
        public void Add(params KeyValuePair<string, object>[] dataKey)
        {
            System.Collections.Specialized.OrderedDictionary dict =
                new System.Collections.Specialized.OrderedDictionary();

            foreach (KeyValuePair<string, object> keyValue in dataKey)
            {
                dict[keyValue.Key] = keyValue.Value;
            }

            this.Add(new DataKey(dict));
        }

        /// <summary>
        /// 将指定的键值添加到数据键值集合中。如果集合中存在多条记录，则添加的键值将作为一系列的同样的数据形成为二维表的一个新列。
        /// </summary>
        /// <param name="key">键名。即字段名。</param>
        /// <param name="data">键值。即字段值。</param>
        public void Add(string key, object data)
        {
            DataKeyCollection dataKeys = new DataKeyCollection();

            if (this.Count == 0)
            {
                dataKeys.Add(
                    new KeyValuePair<string, object>(key, data));
            }
            else
            {
                foreach (DataKey aDataKey in this)
                {
                    System.Collections.Specialized.OrderedDictionary dict =
                        new System.Collections.Specialized.OrderedDictionary();

                    foreach (string aKey in aDataKey.Values.Keys)
                    {
                        dict[aKey] = aDataKey[aKey];
                    }
                    dict[key] = data;

                    dataKeys.Add(new DataKey(dict));
                }
            }

            this.Clear();
            this.AddRange(dataKeys);
        }
    }

    /// <summary>
    /// 提供 Web 用户控件的基础类。
    /// </summary>
    public abstract class ConfigedUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// 获取与设置配置文件名。通过设置该属性，Web 用户控件获取配置文件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        public string ConfigFile
        {
            get
            {
                return this.GetViewStatePropertyValue<string>("ConfigFile", string.Empty);
            }
            set
            {
                this.SetViewStatePropertyValue<string>("ConfigFile", value);
            }
        }

        /// <summary>
        /// 获取与设置页面配置文件。通过该设置属性，Web 用户寻找页面配置文件。
        /// </summary>
        [Browsable(true)]
        [DefaultValue("~/Configs/PageConfig.aspx")]
        public string PageConfigFile
        {
            get
            {
                return this.GetViewStatePropertyValue<string>("PageConfigFile", 
                    "~/Configs/PageConfig.aspx");
            }
            set
            {
                this.SetViewStatePropertyValue<string>("PageConfigFile", value);
            }
        }

        /// <summary>
        /// 获得当前登录的角色。
        /// </summary>
        [Browsable(false)]
        public string LoginRole
        {
            get
            {
                return Utility.GetCurrentLoginRole();
            }
        }

        /// <summary>
        /// 获得当前登录的用户名。
        /// </summary>
        [Browsable(false)]
        public string LoginUser
        {
            get
            {
                return Utility.GetCurrentLoginUser();
            }
        }

        /// <summary>
        /// 获取与设置一个指示忽略保存的页特性的标志。如果该标志被设置成真，则基础用户控件将不再按照加载的页特性进行恢复操作。默认情况下为 false。
        /// </summary>
        [Browsable(false)]
        protected bool IsIgnoreSavedPageFeatures
        {
            get
            {
                return this.GetViewStatePropertyValue<bool>("IsIgnoreSavedPageFeatures", false);
            }
            set
            {
                this.SetViewStatePropertyValue<bool>("IsIgnoreSavedPageFeatures", value);
            }
        }

        /// <summary>
        /// 获取与设置是否执行默认的返回处理。
        /// </summary>
        [Browsable(false)]
        protected bool IsDefaultRedirect
        {
            get
            {
                return this.GetViewStatePropertyValue<bool>("IsDefaultRedirect", true);
            }
            set
            {
                this.SetViewStatePropertyValue<bool>("IsDefaultRedirect", value);
            }
        }

        #endregion

        #region Fields

        private UserControlConfig _xmlConfig;
        /// <summary>
        /// 获得用户控件的配置信息，该信息从<see cref="ConfigFile"/>属性指定的配置文件中获得。
        /// </summary>
        [Browsable(false)]
        protected UserControlConfig Config
        {
            get
            {
                if (_xmlConfig == null && !string.IsNullOrEmpty(this.ConfigFile))
                {
                    _xmlConfig = new UserControlConfig(this.ConfigFile, this.ID);
                }
                return _xmlConfig;
            }
        }

        /// <summary>
        /// 记录检索数据的参数集合。
        /// </summary>
        private object _selectParameters = null;

        private const string RestoreSavedPageFeaturesSessionName = "__RestoreSavedPageFeatures__";

        private const string CommunicationObjectsSessionName = "__CommunicationObjects__";

        private string communicationObjectsKey;

        private bool _isCommObjSessionFirstAccess = true;

        /// <summary>
        /// 获得用于用户控件之间通信的数据集合。无论是通过 Session 方式还是 QueryString 方式都通过该集合属性获得通信的数据。
        /// </summary>
        protected CommunicationObjectCollection CommunicationObjects
        {
            get
            {
                if (_isCommObjSessionFirstAccess)
                {
                    _isCommObjSessionFirstAccess = false;
                    if (!this.IsPostBack) this.Session.Remove(this.communicationObjectsKey);
                }

                object obj = this.Session[this.communicationObjectsKey];

                if (obj == null)
                {
                    obj = new CommunicationObjectCollection();

                    this.Session[this.communicationObjectsKey] = obj;
                }

                return (CommunicationObjectCollection)obj;
            }
        }

        private string dataKeysKey;

        private bool _isDataKeysFirstAccess = true;

        /// <summary>
        /// 表示数据关键字段。
        /// </summary>
        [Browsable(false)]
        public DataKeyCollection DataKeys
        {
            get
            {
                if (_isDataKeysFirstAccess)
                {
                    _isDataKeysFirstAccess = false;
                    if (!this.IsPostBack) this.Session.Remove(this.dataKeysKey);
                }

                object obj = this.Session[this.dataKeysKey];

                if (obj == null)
                {
                    obj = new DataKeyCollection();

                    this.Session[this.dataKeysKey] = obj;
                }

                return (DataKeyCollection)obj;
            }
        }

        private List<Control> _exposedControls;
        /// <summary>
        /// 表示命令型控件集合。
        /// </summary>
        [Browsable(false)]
        public List<Control> ExposedControls
        {
            get
            {
                if (_exposedControls == null)
                {
                    _exposedControls = new List<Control>();
                }
                return _exposedControls;
            }
        }

        /// <summary>
        /// 数据控件。
        /// </summary>
        [Browsable(false)]
        protected abstract Control DataControl
        {
            get;
        }

        /// <summary>
        /// 数据源控件。
        /// </summary>
        [Browsable(false)]
        protected abstract DataSourceControl DataSourceControl
        {
            get;
        }

        /// <summary>
        /// 数据项。
        /// </summary>
        [Browsable(false)]
        protected abstract IEnumerable DataColumns
        {
            get;
        }

        private string savedPageFeaturesKey;
        /// <summary>
        /// 被页面作为返回后需要保留的特征数据存储。
        /// </summary>
        [Browsable(false)]
        protected System.Collections.Specialized.NameValueCollection SavedPageFeatures
        {
            get
            {
                return this.GetViewStatePropertyValue<System.Collections.Specialized.NameValueCollection>(
                    "SavedPageFeatures", new System.Collections.Specialized.NameValueCollection());
            }
            private set
            {
                this.SetViewStatePropertyValue<System.Collections.Specialized.NameValueCollection>(
                    "SavedPageFeatures", value);
            }
        }

        #endregion

        #region Operates

        private bool CalEnableRoles(string enableRoles)
        {
            bool ret = false;

            string[] enableRolesArray = enableRoles.Split(
                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray());

            string everyone = string.Empty;

            foreach (string aRole in enableRolesArray)
            {
                if (aRole.Equals("*"))
                {
                    everyone = aRole;
                    break;
                }
            }

            if (string.IsNullOrEmpty(everyone))
            {
                string enableRole = string.Empty;

                foreach (string aRole in enableRolesArray)
                {
                    if (!string.IsNullOrEmpty(this.LoginRole))
                    {
                        if (aRole.Equals(this.LoginRole))
                        {
                            enableRole = aRole;
                            break;
                        }
                    }
                    else
                    {
                        if (this.Context.User.IsInRole(aRole))
                        {
                            enableRole = aRole;
                            break;
                        }
                    }
                }

                ret = (string.IsNullOrEmpty(enableRole) ? false : true);
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        private void ChangeViewMode()
        {
            if (this.DataControl is DetailsView)
            {
                this.ChangeViewMode((DetailsView)DataControl);
            }
            else if (this.DataControl is FormView)
            {
                this.ChangeViewMode((FormView)DataControl);
            }
        }

        private void ChangeViewMode(DetailsView dvCtrl)
        {
            if (this.Config != null)
            {
                switch (this.Config.ViewMode)
                {
                    case ViewMode.DispForm:
                        dvCtrl.ChangeMode(DetailsViewMode.ReadOnly);
                        break;
                    case ViewMode.EditForm:
                        dvCtrl.ChangeMode(DetailsViewMode.Edit);
                        break;
                    case ViewMode.NewForm:
                        dvCtrl.ChangeMode(DetailsViewMode.Insert);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ChangeViewMode(FormView fvCtrl)
        {
            if (this.Config != null)
            {
                switch (this.Config.ViewMode)
                {
                    case ViewMode.DispForm:
                        fvCtrl.ChangeMode(FormViewMode.ReadOnly);
                        break;
                    case ViewMode.EditForm:
                        fvCtrl.ChangeMode(FormViewMode.Edit);
                        break;
                    case ViewMode.NewForm:
                        fvCtrl.ChangeMode(FormViewMode.Insert);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetConfigedRowSelectMode()
        {
            if (this.Config != null && this.Config.RowSelectMode != RowSelectMode.None)
            {
                this.SetRowSelectMode(this.Config.RowSelectMode);
            }
        }

        private void SubscribeDataControlEvents()
        {
            if (this.DataControl is GridView)
            {
                GridView gvCtrl = (GridView)this.DataControl;
                gvCtrl.DataBinding += new EventHandler(DataControl_DataBinding);
                gvCtrl.DataBound += new EventHandler(DataControl_DataBound);
                gvCtrl.RowCommand += new GridViewCommandEventHandler(DataControl_Command);
            }
            else if (this.DataControl is DetailsView)
            {
                DetailsView dView = (DetailsView)this.DataControl;
                dView.DataBinding += new EventHandler(DataControl_DataBinding);
                dView.DataBound += new EventHandler(DataControl_DataBound);
                dView.ItemCommand += new DetailsViewCommandEventHandler(DataControl_Command);
            }
            else if (this.DataControl is FormView)
            {
                FormView fView = (FormView)this.DataControl;
                fView.DataBinding += new EventHandler(DataControl_DataBinding);
                fView.DataBound += new EventHandler(DataControl_DataBound);
                fView.ItemCommand += new FormViewCommandEventHandler(DataControl_Command);
            }
        }

        private void SubscribeDataSourceControlEvents()
        {
            if (this.DataSourceControl is ObjectDataSource)
            {
                ObjectDataSource objDsCtrl = (ObjectDataSource)this.DataSourceControl;
                objDsCtrl.Selecting += new ObjectDataSourceSelectingEventHandler(DataSourceControl_Selecting);
                objDsCtrl.Selected += new ObjectDataSourceStatusEventHandler(DataSourceControl_Selected);
                objDsCtrl.Inserting += new ObjectDataSourceMethodEventHandler(DataSourceControl_Inserting);
                objDsCtrl.Inserted += new ObjectDataSourceStatusEventHandler(DataSourceControl_Inserted);
                objDsCtrl.Updating += new ObjectDataSourceMethodEventHandler(DataSourceControl_Updating);
                objDsCtrl.Updated += new ObjectDataSourceStatusEventHandler(DataSourceControl_Updated);
                objDsCtrl.Deleting += new ObjectDataSourceMethodEventHandler(DataSourceControl_Deleting);
                objDsCtrl.Deleted += new ObjectDataSourceStatusEventHandler(DataSourceControl_Deleted);
            }
            else if (this.DataSourceControl is SqlDataSource)
            {
                SqlDataSource sqlDsCtrl = (SqlDataSource)this.DataSourceControl;
                sqlDsCtrl.Selecting += new SqlDataSourceSelectingEventHandler(DataSourceControl_Selecting);
                sqlDsCtrl.Selected += new SqlDataSourceStatusEventHandler(DataSourceControl_Selected);
                sqlDsCtrl.Inserting += new SqlDataSourceCommandEventHandler(DataSourceControl_Inserting);
                sqlDsCtrl.Inserted += new SqlDataSourceStatusEventHandler(DataSourceControl_Inserted);
                sqlDsCtrl.Updating += new SqlDataSourceCommandEventHandler(DataSourceControl_Updating);
                sqlDsCtrl.Updated += new SqlDataSourceStatusEventHandler(DataSourceControl_Updated);
                sqlDsCtrl.Deleting += new SqlDataSourceCommandEventHandler(DataSourceControl_Deleting);
                sqlDsCtrl.Deleted += new SqlDataSourceStatusEventHandler(DataSourceControl_Deleted);
            }
            else if (this.DataSourceControl is LinqDataSource)
            {
                LinqDataSource lnqDs = (LinqDataSource)this.DataSourceControl;
                lnqDs.Selecting += new EventHandler<LinqDataSourceSelectEventArgs>(DataSourceControl_Selecting);
                lnqDs.Selected += new EventHandler<LinqDataSourceStatusEventArgs>(DataSourceControl_Selected);
                lnqDs.Inserting += new EventHandler<LinqDataSourceInsertEventArgs>(DataSourceControl_Inserting);
                lnqDs.Inserted += new EventHandler<LinqDataSourceStatusEventArgs>(DataSourceControl_Inserted);
                lnqDs.Updating += new EventHandler<LinqDataSourceUpdateEventArgs>(DataSourceControl_Updating);
                lnqDs.Updated += new EventHandler<LinqDataSourceStatusEventArgs>(DataSourceControl_Updated);
                lnqDs.Deleting += new EventHandler<LinqDataSourceDeleteEventArgs>(DataSourceControl_Deleting);
                lnqDs.Deleted += new EventHandler<LinqDataSourceStatusEventArgs>(DataSourceControl_Deleted);
            }
        }

        private void SubscribeConfigControlEvents()
        {
            if (this.Config != null)
            {
                foreach (UserControlConfig.Control aConfigCtrl in this.Config.Controls)
                {
                    Control foundCtrl = this.FindControl(aConfigCtrl.Name);

                    if (foundCtrl != null)
                    {
                        foundCtrl.Load += new EventHandler(Control_Load);
                        foundCtrl.PreRender += new EventHandler(Control_PreRender);
                        if (foundCtrl is IButtonControl)
                        {
                            ((System.Web.UI.WebControls.IButtonControl)foundCtrl).Command += new CommandEventHandler(Control_Command);
                        }
                        foundCtrl.DataBinding += new EventHandler(Control_DataBinding);
                    }
                }
            }
        }

        private string GenerateReturnUrl()
        {
            string returnUrl = string.Empty;

            List<KeyValuePair<string, object>> requestList = new List<KeyValuePair<string, object>>();

            foreach (string key in this.SavedPageFeatures.AllKeys)
            {
                requestList.Add(new KeyValuePair<string, object>(key, Utility.ConvertToUnicodeStr(this.SavedPageFeatures[key])));
            }

            returnUrl = Utility.GetUrlString(this.Request.Url.LocalPath, requestList.ToArray());

            return returnUrl;
        }

        /// <summary>
        /// 恢复被保存的控件分页的页特征。该方法在控件的 DataBinding 事件中调用。
        /// </summary>
        /// <param name="ctrl">控件。</param>
        protected void RestorePagingFeatures(GridView ctrl)
        {
            string pageIndex = this.SavedPageFeatures[BaseSavedPageFeature.PageIndex.ToString()];
            string pageSize = this.SavedPageFeatures[BaseSavedPageFeature.PageSize.ToString()];

            int iPageIdx = 0;
            int iPageSize = 10;

            if (int.TryParse(pageIndex, out iPageIdx))
            {
                ctrl.PageIndex = iPageIdx;
            }
            if (int.TryParse(pageSize, out iPageSize))
            {
                ctrl.PageSize = iPageSize;
            }
        }
        /// <summary>
        /// 恢复排序页特征。该方法建议在 Page_PreRender 事件中调用。
        /// </summary>
        /// <param name="ctrl">具有排序的控件。</param>
        protected void RestoreSortingFeatures(GridView ctrl)
        {
            string sort = this.SavedPageFeatures[BaseSavedPageFeature.Sort.ToString()];
            string sortDir = this.SavedPageFeatures[BaseSavedPageFeature.SortDir.ToString()];

            if (!string.IsNullOrEmpty(sort))
            {
                ctrl.Sort(sort,
                    (SortDirection)Enum.Parse(typeof(SortDirection), sortDir));
            }
        }

        /// <summary>
        /// 将被保存的筛选条件页特征恢复到数据源控件。该方法在数据源控件的 Selecting 调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询参数。</param>
        protected void RestoreFilteringFeatures(ObjectDataSource ds, ObjectDataSourceSelectingEventArgs e)
        {
            // Original criteria is 'this.DataControl is GridView && '
            if (!this.IsIgnoreSavedPageFeatures)
            {
                if (!string.IsNullOrEmpty(this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()]))
                {
                    ds.FilterExpression = this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()];
                }
            }
        }
        /// <summary>
        /// 将被保存的筛选条件页特征恢复到数据源控件。该方法在数据源控件的 Selecting 调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询参数。</param>
        protected void RestoreFilteringFeatures(SqlDataSource ds, SqlDataSourceSelectingEventArgs e)
        {
            // Original criteria is 'this.DataControl is GridView && '
            if (!this.IsIgnoreSavedPageFeatures)
            {
                if (!string.IsNullOrEmpty(this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()]))
                {
                    ds.FilterExpression = this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()];
                }
            }
        }
        /// <summary>
        /// 将被保存的筛选条件页特征恢复到数据源控件。该方法在数据源控件的 Selecting 调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询参数。</param>
        protected void RestoreFilteringFeatures(LinqDataSource ds, LinqDataSourceSelectEventArgs e)
        {
            // Original criteria is 'this.DataControl is GridView && '
            if (!this.IsIgnoreSavedPageFeatures)
            {
                if (!string.IsNullOrEmpty(this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()]))
                {
                    ds.Where = this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()];
                }
            }
        }

        private void SavePagingFeatures()
        {
            if (this.DataControl is GridView)
            {
                GridView gvCtrl = (GridView)this.DataControl;

                this.SavedPageFeatures[BaseSavedPageFeature.PageIndex.ToString()] = gvCtrl.PageIndex.ToString();
                this.SavedPageFeatures[BaseSavedPageFeature.PageSize.ToString()] = gvCtrl.PageSize.ToString();
            }
        }

        private void SaveSortingFeatures()
        {
            if (this.DataControl is GridView)
            {
                GridView gvCtrl = (GridView)this.DataControl;

                this.SavedPageFeatures[BaseSavedPageFeature.Sort.ToString()] = gvCtrl.SortExpression;
                this.SavedPageFeatures[BaseSavedPageFeature.SortDir.ToString()] = gvCtrl.SortDirection.ToString();
            }
        }

        private void SaveFilteringFeatures()
        {
            if (this.DataSourceControl is LinqDataSource)
            {
                LinqDataSource lnqDsCtrl = (LinqDataSource)this.DataSourceControl;

                this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()] = lnqDsCtrl.Where;
            }
            else if (this.DataSourceControl is SqlDataSource)
            {
                SqlDataSource sqlDsCtrl = (SqlDataSource)this.DataSourceControl;

                this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()] = sqlDsCtrl.FilterExpression;
            }
            else if (this.DataSourceControl is ObjectDataSource)
            {
                ObjectDataSource objDsCtrl = (ObjectDataSource)this.DataSourceControl;

                this.SavedPageFeatures[BaseSavedPageFeature.Filter.ToString()] = objDsCtrl.FilterExpression;
            }
        }

        private void SetDataColumnsState()
        {
            if (this.Config != null && this.DataColumns is DataControlFieldCollection)
            {
                foreach (DataControlField aField in this.DataColumns)
                {
                    if (string.IsNullOrEmpty(aField.HeaderText)) continue;

                    UserControlConfig.DataColumn aDataColumn =
                        this.Config.DataColumns.FirstOrDefault(p => p.Name.Equals(aField.HeaderText));

                    if (aDataColumn != null)
                    {
                        bool enabled = this.CalEnableRoles(aDataColumn.EnableRoles);

                        if (!enabled)
                        {
                            aField.Visible = false;
                        }
                        else
                        {
                            if (!this.IsPostBack)
                            {
                                aField.Visible = aDataColumn.DefaultVisible;
                            }
                        }
                    }
                }
            }
        }

        private void SetInitSelectData()
        {
            if (this.Config != null && this.DataControl != null && this.DataSourceControl != null)
            {
                System.Reflection.PropertyInfo aDsIdPro =
                    this.DataControl.GetType().GetProperty("DataSourceID");

                if (aDsIdPro != null)
                {
                    if (this.Config.IsInitSelectData)
                    {
                        aDsIdPro.SetValue(this.DataControl, this.DataSourceControl.ID, null);
                    }
                    else
                    {
                        aDsIdPro.SetValue(this.DataControl, null, null);
                    }
                }
            }
        }

        private void SetPropertyValues()
        {
            if (this.Config != null)
            {
                foreach (UserControlConfig.Property aConfigedProp in this.Config.Properties)
                {
                    System.Reflection.PropertyInfo aPropInfo = null;
                    object aPropObj = null;

                    string[] nameArr = aConfigedProp.Name.Split('.');

                    if (nameArr.Length > 1)
                    {
                        Control aCtrl = this.FindControl(nameArr[0]);

                        if (aCtrl != null)
                        {
                            aPropInfo =
                                aCtrl.GetType().GetProperty(nameArr[1]);
                            aPropObj = aCtrl;
                        }
                    }
                    else
                    {
                        aPropInfo =
                            this.GetType().GetProperty(aConfigedProp.Name, 
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                        aPropObj = this;
                    }

                    if (aPropInfo != null)
                    {
                        if (aConfigedProp.PageEffective == PageEffective.Initial && !this.IsPostBack ||
                            aConfigedProp.PageEffective == PageEffective.PostBack && this.IsPostBack ||
                            aConfigedProp.PageEffective == PageEffective.Always)
                        {
                            aPropInfo.SetValue(aPropObj, aConfigedProp.Value, null);
                        }
                    }
                }
            }
        }

        private void SetDataColumnsOrder()
        {
            if (this.Config != null)
            {
                if (this.DataControl is DataGridView)
                {
                    Dictionary<string, int> orders =
                        new Dictionary<string, int>();

                    foreach (var col in this.Config.DataColumns)
                    {
                        orders[col.Name] = col.Order;
                    }

                    ((DataGridView)this.DataControl).SetColumnsOrder(orders);
                }
            }
        }

        private bool HandleCheckCommandPermission(string functionName, CheckPermissionRule rule)
        {
            bool r = false;

            if (!(r = this.CheckCommandPermission(functionName)))
            {
                throw new System.Security.SecurityException();
            }
            if (!(r = this.CheckCommandPermission(functionName, rule)))
            {
                switch (rule.CheckFailureMode)
                {
                    case CheckFailureMode.ThrowException:
                        throw new System.Security.SecurityException();
                    case CheckFailureMode.HideControl:
                        if (rule.CheckObject is Control)
                        {
                            ((Control)rule.CheckObject).Visible = false;
                        }
                        break;
                    case CheckFailureMode.DisableControl:
                        if (rule.CheckObject is WebControl)
                        {
                            ((WebControl)rule.CheckObject).Enabled = false;
                        }
                        break;
                    default:
                        break;
                }
            }

            return r;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 将数据控件的数据导出到Excel。
        /// </summary>
        /// <param name="exportTitle">导出标题。</param>
        /// <param name="fileName">导出文件名。</param>
        protected virtual void ExportToExcel(string exportTitle, string fileName)
        {
            if (this.DataControl is GridView)
            {
                GridView gv = this.DataControl as GridView;

                List<int> excludeCols = new List<int>();

                if (this.Config != null)
                {
                    foreach (DataControlField aCol in gv.Columns)
                    {
                        UserControlConfig.DataColumn aConfigedDataCol =
                            this.Config.DataColumns[aCol.HeaderText];

                        bool isExclude = !aCol.Visible;

                        if (aConfigedDataCol != null)
                        {
                            bool enableForRoles = this.CalEnableRoles(aConfigedDataCol.EnableRoles);

                            if (!(
                                (!aCol.Visible && aConfigedDataCol.EnableExport != null && aConfigedDataCol.EnableExport.Value && enableForRoles) ||
                                (aCol.Visible && aConfigedDataCol.EnableExport == null && enableForRoles) ||
                                (aCol.Visible && aConfigedDataCol.EnableExport != null && aConfigedDataCol.EnableExport.Value && enableForRoles)))
                            {
                                isExclude = true;
                            }
                            else
                            {
                                isExclude = false;
                            }
                        }

                        if (isExclude) excludeCols.Add(gv.Columns.IndexOf(aCol));
                    }
                }

                Utility.ExportToExcel(gv,
                    excludeCols.ToArray(), exportTitle, fileName);
            }
        }

        /// <summary>
        /// 获得作为存放在视图状态中的属性值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="defaultValue">属性的默认值。</param>
        /// <returns></returns>
        protected T GetViewStatePropertyValue<T>(string propertyName, T defaultValue)
        {
            return Utility.GetViewStatePropertyValue<T>(this.ViewState, propertyName, defaultValue);
        }

        /// <summary>
        /// 设置存放在视图状态中的属性值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性名称。</param>
        /// <param name="value">属性值。</param>
        protected void SetViewStatePropertyValue<T>(string propertyName, T value)
        {
            Utility.SetViewStatePropertyValue<T>(this.ViewState, propertyName, value);
        }

        /// <summary>
        /// 检测命令执行权限。该方法需要派生类重写。
        /// </summary>
        /// <param name="functionName">功能名称。</param>
        /// <returns>如果检测到的执行权限为Allow，则返回true，否则返回false。</returns>
        protected virtual bool CheckCommandPermission(string functionName)
        {
            return true;
        }

        protected virtual bool CheckCommandPermission(string functionName, CheckPermissionRule rule)
        {
            return true;
        }

        /// <summary>
        /// 设置行选择模式。该方法需要由派生类重写。
        /// </summary>
        /// <param name="rowSelectMode">行选择模式。</param>
        protected virtual void SetRowSelectMode(RowSelectMode rowSelectMode)
        {
            return;
        }

        /// <summary>
        /// 接收通信数据。
        /// </summary>
        /// <remarks>
        /// 该方法可以被派生类覆写，在覆写的方法中最好在第一句话中调用基类方法以获得通信的数据集合，该数据集合存放在 CommunicationObjects 属性中。
        /// </remarks>
        protected virtual void ReceiveData()
        {
            if (this.Config != null &&
                (this.Config.CommunicationDirection == CommunicationDirection.Bidirect ||
                this.Config.CommunicationDirection == CommunicationDirection.Receive))
            {
                //  Get the Communication Objects in session.
                CommunicationObjectCollection commObjs =
                    this.Session[CommunicationObjectsSessionName] as CommunicationObjectCollection;

                //  If the Communication Objects in session are existing,
                //  put them into the CommunicationObjects property of current WebUserControl,
                //  else get the Communication Objects in QueryString and put them into it.
                if (commObjs != null)
                {
                    foreach (UserControlConfig.CommunicationObject aConfigCommObj in this.Config.CommunicationObjects)
                    {
                        if (commObjs.ContainsKey(aConfigCommObj.Name))
                        {
                            this.CommunicationObjects[aConfigCommObj.Name] =
                                commObjs.GetCommunicationObjectList(aConfigCommObj.Name, aConfigCommObj.Type);
                        }
                    }
                }
                else
                {
                    foreach (UserControlConfig.CommunicationObject aConfigCommObj in this.Config.CommunicationObjects)
                    {
                        string commObjValue = this.Request.QueryString[aConfigCommObj.Name];

                        if (!string.IsNullOrEmpty(commObjValue))
                        {
                            this.CommunicationObjects[aConfigCommObj.Name] = commObjValue;
                            this.CommunicationObjects[aConfigCommObj.Name] =
                                this.CommunicationObjects.GetCommunicationObjectList(aConfigCommObj.Name, aConfigCommObj.Type);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 向在同一页面中的其他用户控件发送通信数据。
        /// </summary>
        /// <param name="destUserCtrl">目标用户控件，即接受方用户控件。</param>
        /// <param name="commObjs">发送用于通信的数据。</param>
        public void SendData(ConfigedUserControl destUserCtrl, CommunicationObjectCollection commObjs)
        {
            if (commObjs != null)
            {
                destUserCtrl.CommunicationObjects.AddRange(commObjs);
            }
            destUserCtrl.ReceiveData();
        }

        /// <summary>
        /// 执行返回页请求。
        /// </summary>
        /// <param name="aUrl">需要返回的Url。</param>
        protected virtual void RedirectReturn(UserControlConfig.Url aUrl)
        {
            if (aUrl == null && this.Config != null)
            {
                aUrl = this.Config.ReturnUrl;
            }

            this.Session[RestoreSavedPageFeaturesSessionName] = true;

            string returnUrl = this.Request.QueryString["ReturnUrl"];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (aUrl == null)
                {
                    this.Session.Remove(CommunicationObjectsSessionName);
                    this.Session.Remove(this.communicationObjectsKey);
                    this.Session.Remove(this.dataKeysKey);
                    this.Session.Remove(this.savedPageFeaturesKey);

                    this.Response.Redirect(this.Server.UrlDecode(returnUrl));
                }
                else
                {
                    aUrl.Path = this.Server.UrlDecode(returnUrl);
                    this.Redirect(aUrl);
                }
            }
            else
            {
                if (aUrl != null)
                {
                    this.Redirect(aUrl);
                }
                else
                {
                    this.Session.Remove(CommunicationObjectsSessionName);
                    this.Session.Remove(this.communicationObjectsKey);
                    this.Session.Remove(this.dataKeysKey);
                    this.Session.Remove(this.savedPageFeaturesKey);

                    this.Response.Redirect(this.Request.UrlReferrer.AbsoluteUri);
                }
            }
        }

        /// <summary>
        /// 执行返回页请求。
        /// </summary>
        protected virtual void RedirectReturn()
        {
            this.RedirectReturn(null);
        }

        /// <summary>
        /// 执行完成跳转。
        /// </summary>
        /// <param name="aUrl">跳转的Url。</param>
        protected virtual void RedirectCompleted(UserControlConfig.Url aUrl)
        {
            if (aUrl == null && this.Config != null)
            {
                aUrl = this.Config.CompletedUrl;
            }

            this.RedirectReturn(aUrl);
        }

        /// <summary>
        /// 执行完成跳转。
        /// </summary>
        protected virtual void RedirectCompleted()
        {
            this.RedirectCompleted(null);
        }

        /// <summary>
        /// 按照配置文件中配置的 Url 进行页面跳转。
        /// </summary>
        /// <param name="aUrl"></param>
        protected void Redirect(UserControlConfig.Url aUrl)
        {
            if (aUrl == null) return;

            CommunicationObjectCollection commObjs = new CommunicationObjectCollection();

            foreach (UserControlConfig.CommunicationObject aConfigCommObj in aUrl.CommunicationObjects)
            {
                System.Collections.Specialized.NameValueCollection keyValues =
                    new System.Collections.Specialized.NameValueCollection();

                foreach (DataKey aDk in this.DataKeys)
                {
                    if (aDk[aConfigCommObj.DataKeyName] != null)
                    {
                        keyValues.Add(aConfigCommObj.Name, aDk[aConfigCommObj.DataKeyName].ToString());
                    }
                }

                if (keyValues.Count == 0)
                {
                    if (string.IsNullOrEmpty(aConfigCommObj.ControlProperty))
                    {
                        aConfigCommObj.ControlProperty = string.Empty;
                    }
                    string[] ctrlProps = aConfigCommObj.ControlProperty.Split(new char[] { '.' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (ctrlProps.Length >= 2)
                    {
                        Control foundCtrl = this.FindControl(ctrlProps[0]);
                        if (foundCtrl != null)
                        {
                            System.Reflection.PropertyInfo aPropInfo = foundCtrl.GetType().GetProperty(ctrlProps[1]);
                            if (aPropInfo != null)
                            {
                                object propValue = aPropInfo.GetValue(foundCtrl, null);

                                if (propValue != null)
                                {
                                    keyValues.Add(aConfigCommObj.Name, propValue.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.CommunicationObjects.ContainsKey(aConfigCommObj.Name))
                        {
                            List<object> list = this.CommunicationObjects[aConfigCommObj.Name] as List<object>;

                            if (list != null)
                            {
                                foreach (object anValue in list)
                                {
                                    keyValues.Add(aConfigCommObj.Name, 
                                        (anValue == null ? null : anValue.ToString()));
                                }
                            }
                        }
                        else
                        {
                            keyValues.Add(aConfigCommObj.Name,
                                (aConfigCommObj.DefaultValue == null ? null : aConfigCommObj.DefaultValue.ToString()));
                        }
                    }
                }

                commObjs[aConfigCommObj.Name] = keyValues[aConfigCommObj.Name];

            }

            this.Session.Remove(CommunicationObjectsSessionName);
            this.Session.Remove(this.communicationObjectsKey);
            this.Session.Remove(this.dataKeysKey);

            if (!(this.Config != null && this.Config.ReturnUrl != null && this.Config.ReturnUrl.Name.Equals(aUrl.Name)))
            {
                this.Session[this.savedPageFeaturesKey] = this.SavedPageFeatures;
            }

            if (aUrl.CommunicationMode == CommunicationMode.None)
            {
                commObjs.Clear();
                if (aUrl.GenerateReturnUrl)
                {
                    commObjs.Add("ReturnUrl", this.Server.UrlEncode(this.Request.RawUrl));
                }
                this.Response.Redirect(Utility.GetUrlString(aUrl.Path, commObjs.ToArray()));
            }
            else if (aUrl.CommunicationMode == CommunicationMode.QueryString)
            {
                if (aUrl.GenerateReturnUrl)
                {
                    commObjs.Add("ReturnUrl", this.Server.UrlEncode(this.Request.RawUrl));
                }
                this.Response.Redirect(Utility.GetUrlString(aUrl.Path, commObjs.ToArray()));
            }
            else if (aUrl.CommunicationMode == CommunicationMode.Session)
            {
                this.Session[CommunicationObjectsSessionName] = commObjs;

                if (aUrl.GenerateReturnUrl)
                {
                    this.Response.Redirect(Utility.GetUrlString(aUrl.Path,
                        new KeyValuePair<string, object>("ReturnUrl", this.Server.UrlEncode(this.Request.RawUrl))));
                }
                else
                {
                    this.Response.Redirect(aUrl.Path);
                }
            }
        }

        /// <summary>
        /// 加载被保存的页特性。
        /// </summary>
        protected virtual void LoadSavedPageFeatures()
        {
            this.SavedPageFeatures = new System.Collections.Specialized.NameValueCollection();

            string[] keys = Enum.GetNames(typeof(BaseSavedPageFeature));
            foreach (string key in keys)
            {
                if (string.IsNullOrEmpty(this.SavedPageFeatures[key]))
                {
                    this.SavedPageFeatures.Add(key, this.GetSavedPageFeatures(key));
                }
            }

            if (this.DataSourceControl is SqlDataSource)
            {
                SqlDataSource sqlDsCtrl = (SqlDataSource)this.DataSourceControl;

                foreach (Parameter aParam in sqlDsCtrl.SelectParameters)
                {
                    string aFeature = this.GetSavedPageFeatures("@" + aParam.Name);
                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        this.SavedPageFeatures["@" + aParam.Name] = aFeature;
                    }
                }
            }
            else if (this.DataSourceControl is ObjectDataSource)
            {
                ObjectDataSource objDsCtrl = (ObjectDataSource)this.DataSourceControl;

                foreach (Parameter aParam in objDsCtrl.SelectParameters)
                {
                    string aFeature = this.GetSavedPageFeatures(aParam.Name);
                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        this.SavedPageFeatures[aParam.Name] = aFeature;
                    }
                }
            }
            else if (this.DataSourceControl is LinqDataSource)
            {
                LinqDataSource lnqDsCtrl = (LinqDataSource)this.DataSourceControl;

                foreach (Parameter aParam in lnqDsCtrl.WhereParameters)
                {
                    string aFeature = this.GetSavedPageFeatures(aParam.Name);
                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        this.SavedPageFeatures[aParam.Name] = aFeature;
                    }
                }
            }
        }

        /// <summary>
        /// 将需要暴露的控件添加到集合中。
        /// </summary>
        /// <param name="exposedCtrls">需要暴露的控件集合。</param>
        protected virtual void AddExposedControls(List<Control> exposedCtrls)
        {
            if (this.Config != null)
            {
                foreach (UserControlConfig.Control aConfigCtrl in this.Config.Controls)
                {
                    Control ctrl = this.FindControl(aConfigCtrl.Name);

                    if (ctrl != null && aConfigCtrl.IsExposed && !exposedCtrls.Contains(ctrl))
                    {
                        exposedCtrls.Add(ctrl);
                    }
                }
            }
        }

        /// <summary>
        /// 页面配置控件访问角色的应用设置名。
        /// </summary>
        public const string PageConfigControlAccessRoleAppSettingName = "PageConfigControlAccessRole";
        private string _pageConfigCtrlAccessRole;
        /// <summary>
        /// 获取页面配置控件访问角色。来自 Web.config 中的 appSetings 的 PageConfigControlAccessRole 配置节。
        /// </summary>
        public string PageConfigControlAccessRole
        {
            get
            {
                if (string.IsNullOrEmpty(_pageConfigCtrlAccessRole))
                {
                    _pageConfigCtrlAccessRole =
                        System.Web.Configuration.WebConfigurationManager.AppSettings[
                        PageConfigControlAccessRoleAppSettingName];
                }
                return _pageConfigCtrlAccessRole;
            }
        }

        /// <summary>
        /// 为用户控件添加页面配置控件。
        /// </summary>
        protected virtual void AddPageConfigControl()
        {
            if (string.IsNullOrEmpty(
                this.PageConfigControlAccessRole)) return;

            if (!System.Web.Security.Roles.Enabled)
                return;

            if (string.IsNullOrEmpty(this.LoginRole))
            {
                if (!System.Web.Security.Roles.GetRolesForUser().Contains(
                    this.PageConfigControlAccessRole)) return;
            }
            else
            {
                if (!this.LoginRole.Equals(
                    this.PageConfigControlAccessRole)) return;
            }

            HyperLink hlnkConfig = new HyperLink();

            hlnkConfig.ID = "hlnkConfig";

            hlnkConfig.Text =
                Properties.Resources.PageConfigControlText;

            hlnkConfig.NavigateUrl =
                string.Format("{0}?ConfigFile={1}&Name={2}&ReturnUrl={3}",
                this.PageConfigFile,
                this.ConfigFile,
                this.ID,
                this.Request.RawUrl);

            this.Controls.Add(new LiteralControl("<hr/>"));

            this.Controls.Add(new LiteralControl(
                string.Format("<span>{0}</span>", Properties.Resources.PageConfigControlTipText)));

            this.Controls.Add(hlnkConfig);

            this.Controls.Add(new LiteralControl("<hr/>"));
        }

        /// <summary>
        /// 通过给定的被保存的页特性名称，获得特性字符串。
        /// </summary>
        /// <param name="featureName">页特性名称。</param>
        /// <returns>特性字符串。</returns>
        protected string GetSavedPageFeatures(string featureName)
        {
            string feature = this.Request.QueryString[featureName];

            if (string.IsNullOrEmpty(feature))
            {
                bool? restore = this.Session[RestoreSavedPageFeaturesSessionName] as bool?;

                System.Collections.Specialized.NameValueCollection nameValues =
                    this.Session[this.savedPageFeaturesKey]
                    as System.Collections.Specialized.NameValueCollection;

                if (nameValues != null && restore != null && restore.Value)
                {
                    feature = nameValues[featureName];
                }
            }

            return feature;
        }

        #endregion

        /// <summary>
        /// 已经被覆写。用户控件在初始化的时候执行的操作。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.communicationObjectsKey = string.Format("{0}${1}$CommunicationObjects", this.Page.GetType().Name, this.UniqueID);
            this.dataKeysKey = string.Format("{0}${1}$DataKeys", this.Page.GetType().Name, this.UniqueID);
            this.savedPageFeaturesKey = string.Format("{0}{1}SavedPageFeatures", this.Page.GetType().Name, this.UniqueID);

            this.Page.PreLoad += new EventHandler(Page_PreLoad);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);

            this.SubscribeDataControlEvents();
            this.SubscribeDataSourceControlEvents();
            this.SubscribeConfigControlEvents();
        }

        /// <summary>
        /// 主要执行改变视图模式、接收数据、加载被保存页特性、添加将被暴露控件到集合中等操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_PreLoad(object sender, EventArgs e)
        {
            this.ChangeViewMode();

            if (!this.IsPostBack)
            {
                if (this.Config != null)
                {
                    this.HandleCheckCommandPermission(this.Config.FunctionName,
                        new CheckPermissionRule(CheckPoint.UserControlLoad,
                            CheckFailureMode.ThrowException, this));
                }

                this.ReceiveData();
                this.LoadSavedPageFeatures();

                this.SetInitSelectData();
            }

            this.SetPropertyValues();

            this.SetDataColumnsOrder();

            this.AddPageConfigControl();

            this.AddExposedControls(this.ExposedControls);
        }

        /// <summary>
        /// 已经被覆写。恢复排序页特征，设置数据列状态。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.IsPostBack)
            {
                if (this.DataControl is GridView)
                    this.RestoreSortingFeatures((GridView)this.DataControl);
            }
            this.SetDataColumnsState();
            this.SetConfigedRowSelectMode();
        }

        protected virtual void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.Session.Remove(CommunicationObjectsSessionName);
            this.Session.Remove(RestoreSavedPageFeaturesSessionName);
            this.Session.Remove(this.savedPageFeaturesKey);
        }

        protected virtual void DataControl_DataBound(object sender, EventArgs e)
        {
            this.SavePagingFeatures();

            this.SaveSortingFeatures();
        }

        protected virtual void DataControl_DataBinding(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.DataControl is GridView)
                    this.RestorePagingFeatures((GridView)this.DataControl);
            }
        }

        protected virtual void DataControl_Command(object sender, GridViewCommandEventArgs e)
        {

        }

        protected virtual void DataControl_Command(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName.Equals(CommandName.Cancel.ToString()))
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectReturn();
                }
            }
        }

        protected virtual void DataControl_Command(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals(CommandName.Cancel.ToString()))
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectReturn();
                }
            }
        }

        /// <summary>
        /// 将数据通信对象存入数据源查询参数中。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreCommObjectsForSelectParamaters(LinqDataSource ds, LinqDataSourceSelectEventArgs e)
        {
            if (this.Config == null) return;

            foreach (KeyValuePair<string, object> aCommObj in this.CommunicationObjects)
            {
                UserControlConfig.CommunicationObject aConfigedCommObj = 
                    this.Config.CommunicationObjects[aCommObj.Key];

                if (aConfigedCommObj != null && e.WhereParameters.Keys.Contains(aConfigedCommObj.DataKeyName))
                {
                    List<object> list = aCommObj.Value as List<object>;

                    if (list != null && list.Count > 0)
                    {
                        if (list.Count == 1)
                        {
                            e.WhereParameters[aConfigedCommObj.DataKeyName] = list[0];
                        }
                        else
                        {
                            e.WhereParameters[aConfigedCommObj.DataKeyName] =
                                Utility.ConvertCollectionToString(list,
                                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 将被保存的页特征恢复到数据源控件的查询参数。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreSelectParameterFeatures(LinqDataSource ds, LinqDataSourceSelectEventArgs e)
        {
            //  Restore the SavedPageFeatures to the Parameters.
            if (!this.IsIgnoreSavedPageFeatures)
            {
                foreach (Parameter aParam in ds.WhereParameters)
                {
                    string aFeature = this.SavedPageFeatures[aParam.Name];

                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        e.WhereParameters[aParam.Name] =
                            Convert.ChangeType(aFeature, aParam.Type);
                    }
                }
            }
        }
        /// <summary>
        /// 被覆写。数据源控件的 Selecting 事件。
        /// </summary>
        /// <param name="sender">数据源控件。</param>
        /// <param name="e">事件参数。</param>
        protected virtual void DataSourceControl_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            LinqDataSource lnqDsCtrl = (LinqDataSource)this.DataSourceControl;

            //  Restore the CommunicationObjects to the Parameters.
            this.RestoreCommObjectsForSelectParamaters(lnqDsCtrl, e);

            //  Restore the SavedPageFeatures to the Parameters.
            this.RestoreSelectParameterFeatures(lnqDsCtrl, e);

            //  Restore Filerting Features.
            this.RestoreFilteringFeatures(lnqDsCtrl, e);

            _selectParameters = e.WhereParameters;
        }

        protected virtual void DataSourceControl_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            LinqDataSource lnqDsCtrl = (LinqDataSource)this.DataSourceControl;

            //  Save the Parameters into the SavedPageFeatures.
            if (_selectParameters is IDictionary<string, object>)
            {
                IDictionary<string, object> dictParams = (IDictionary<string, object>)_selectParameters;

                foreach (string key in dictParams.Keys)
                {
                    object value = dictParams[key];

                    if (value != null)
                    {
                        this.SavedPageFeatures[key] = value.ToString();
                    }
                }
            }
            
            //  Save the Filter into the SavedPageFeatures.
            this.SaveFilteringFeatures();
        }

        protected virtual void DataSourceControl_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
        {
            
        }

        protected virtual void DataSourceControl_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {

        }

        protected virtual void DataSourceControl_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            
        }

        /// <summary>
        /// 将数据通信对象存入数据源查询参数中。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreCommObjectsForSelectParamaters(SqlDataSource ds, SqlDataSourceSelectingEventArgs e)
        {
            if (this.Config == null) return;

            foreach (KeyValuePair<string, object> aCommObj in this.CommunicationObjects)
            {
                UserControlConfig.CommunicationObject aConfigedCommObj = this.Config.CommunicationObjects[aCommObj.Key];

                if (aConfigedCommObj != null)
                {
                    string dataKeyName = string.Format("@{0}", aConfigedCommObj.DataKeyName);

                    if (e.Command.Parameters.Contains(dataKeyName))
                    {
                        List<object> list = aCommObj.Value as List<object>;

                        if (list != null && list.Count > 0)
                        {
                            if (list.Count == 1)
                            {
                                e.Command.Parameters[dataKeyName].Value = list[0];
                            }
                            else
                            {
                                e.Command.Parameters[dataKeyName].Value =
                                    Utility.ConvertCollectionToString(list,
                                    System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 将被保存的页特征恢复到数据源控件的查询参数。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreSelectParameterFeatures(SqlDataSource ds, SqlDataSourceSelectingEventArgs e)
        {
            //  Restore the SavedPageFeatures to the Parameters.
            if (!this.IsIgnoreSavedPageFeatures)
            {
                foreach (System.Data.Common.DbParameter aDbParam in e.Command.Parameters)
                {
                    string aFeature = this.SavedPageFeatures[aDbParam.ParameterName];

                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        aDbParam.Value =
                            Convert.ChangeType(
                            aFeature, ds.SelectParameters[aDbParam.ParameterName.TrimStart('@')].Type);
                    }
                }
            }
        }
        /// <summary>
        /// 被覆写。数据源控件的 Selecting 事件。
        /// </summary>
        /// <param name="sender">数据源控件。</param>
        /// <param name="e">事件参数。</param>
        protected virtual void DataSourceControl_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            SqlDataSource sqlDsCtrl = (SqlDataSource)this.DataSourceControl;

            //  Restore the CommunicationObjects to the Parameters.
            //  The proccess is mainly used for Form ViewMode.
            this.RestoreCommObjectsForSelectParamaters(sqlDsCtrl, e);

            //  Restore the SavedPageFeatures to the Parameters.
            this.RestoreSelectParameterFeatures(sqlDsCtrl, e);

            //  Restore Filerting Features.
            this.RestoreFilteringFeatures(sqlDsCtrl, e);

            _selectParameters = e.Command.Parameters;
        }

        protected virtual void DataSourceControl_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            //  Save the Parameters into SavedPageFeatures.
            if (_selectParameters is System.Data.Common.DbParameterCollection)
            {
                System.Data.Common.DbParameterCollection dbparams = (System.Data.Common.DbParameterCollection)_selectParameters;

                foreach (System.Data.Common.DbParameter aDbParam in dbparams)
                {
                    if (aDbParam.Value != null)
                    {
                        this.SavedPageFeatures[aDbParam.ParameterName] = aDbParam.Value.ToString();
                    }
                }
            }

            //  Save the Filtering Features.
            this.SaveFilteringFeatures();
        }

        protected virtual void DataSourceControl_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            
        }

        protected virtual void DataSourceControl_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            SqlDataSource sqlDsCtrl = (SqlDataSource)this.DataSourceControl;

            if (this.Config != null)
            {
                foreach (UserControlConfig.CommunicationObject aConfigCommObj in this.Config.CommunicationObjects)
                {
                    System.Data.Common.DbParameter aDbParam = e.Command.Parameters[
                        string.Format("@" + sqlDsCtrl.OldValuesParameterFormatString, aConfigCommObj.DataKeyName)];

                    if (aDbParam != null)
                    {
                        aDbParam.Value = this.CommunicationObjects[aConfigCommObj.Name];
                    }
                }
            }
        }

        protected virtual void DataSourceControl_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            
        }

        protected virtual void DataSourceControl_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        /// <summary>
        /// 将数据通信对象存入数据源查询参数中。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreCommObjectsForSelectParamaters(ObjectDataSource ds, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.Config == null) return;

            foreach (KeyValuePair<string, object> aCommObj in this.CommunicationObjects)
            {
                UserControlConfig.CommunicationObject aConfigedCommObj = this.Config.CommunicationObjects[aCommObj.Key];

                if (aConfigedCommObj != null && e.InputParameters.Contains(aConfigedCommObj.DataKeyName))
                {
                    List<object> list = aCommObj.Value as List<object>;

                    if (list != null && list.Count > 0)
                    {
                        if (list.Count == 1)
                        {
                            e.InputParameters[aConfigedCommObj.DataKeyName] = list[0];
                        }
                        else
                        {
                            e.InputParameters[aConfigedCommObj.DataKeyName] =
                                Utility.ConvertCollectionToString(list,
                                System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 将被保存的页特征恢复到数据源控件的查询参数。该方法在数据源控件的 Selecting 事件中被调用。
        /// </summary>
        /// <param name="ds">数据源控件。</param>
        /// <param name="e">数据源查询事件参数。</param>
        protected void RestoreSelectParameterFeatures(ObjectDataSource ds, ObjectDataSourceSelectingEventArgs e)
        {
            //  Restore the CommunicationObjects to the Parameters.
            if (!this.IsIgnoreSavedPageFeatures)
            {
                foreach (Parameter aParam in ds.SelectParameters)
                {
                    string aFeature = this.SavedPageFeatures[aParam.Name];

                    if (!string.IsNullOrEmpty(aFeature))
                    {
                        e.InputParameters[aParam.Name] =
                            Convert.ChangeType(aFeature, ds.SelectParameters[aParam.Name].Type);
                    }
                }
            }
        }
        /// <summary>
        /// 被覆写。数据源控件的 Selecting 事件。
        /// </summary>
        /// <param name="sender">数据源控件。</param>
        /// <param name="e">事件参数。</param>
        protected virtual void DataSourceControl_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            ObjectDataSource objDsCtrl = (ObjectDataSource)this.DataSourceControl;

            //  Restore the SavedPageFeatures to the Parameters.
            this.RestoreCommObjectsForSelectParamaters(objDsCtrl, e);

            //  Restore the SavedPageFeatures to the Parameters.
            this.RestoreSelectParameterFeatures(objDsCtrl, e);

            //  Restore Filerting Features.
            this.RestoreFilteringFeatures(objDsCtrl, e);

            _selectParameters = e.InputParameters;
        }

        protected virtual void DataSourceControl_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            //  Save the Parameters into the SavedPageFeature.
            if (_selectParameters is System.Collections.Specialized.IOrderedDictionary)
            {
                System.Collections.Specialized.IOrderedDictionary dictParams = (System.Collections.Specialized.IOrderedDictionary)_selectParameters;

                foreach (string key in dictParams.Keys)
                {
                    object value = dictParams[key];

                    if (value != null)
                    {
                        this.SavedPageFeatures[key] = value.ToString();
                    }
                }
            }

            //  Save the Filering features.
            this.SaveFilteringFeatures();
        }

        protected virtual void DataSourceControl_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {

        }

        protected virtual void DataSourceControl_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ObjectDataSource objDsCtrl = (ObjectDataSource)this.DataSourceControl;

            if (this.Config != null)
            {
                foreach (UserControlConfig.CommunicationObject aConfigCommObj in this.Config.CommunicationObjects)
                {
                    //e.InputParameters[aConfigCommObj.DataKeyName] =
                    //    this.CommunicationObjects[aConfigCommObj.Name];

                    List<object> list = this.CommunicationObjects[aConfigCommObj.Name] as List<object>;

                    if (list != null && list.Count > 0)
                    {
                        e.InputParameters[aConfigCommObj.DataKeyName] = list[0];
                    }
                }
            }            
        }

        protected virtual void DataSourceControl_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (this.IsDefaultRedirect)
                {
                    this.RedirectCompleted();
                }
            }
        }

        protected virtual void DataSourceControl_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            
        }

        /// <summary>
        /// 该方法被配置文件中 Controls 节中的控件自动订阅。主要用于将控件添加到被暴露的集合中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Control_Load(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            if (this.Config != null)
            {
                UserControlConfig.Control aConfigCtrl = this.Config.Controls[ctrl.ID];

                if (aConfigCtrl != null && aConfigCtrl.IsExposed)
                {
                    if (!this.ExposedControls.Contains(ctrl))
                    {
                        this.ExposedControls.Add(ctrl);
                    }
                }
                if (aConfigCtrl != null && aConfigCtrl.IsExportControl)
                {
                    ScriptManager scriptMgr = ScriptManager.GetCurrent(this.Page);
                    if (scriptMgr != null)
                    {
                        scriptMgr.RegisterPostBackControl((Control)sender);
                    }
                }
            }
        }

        private void ConfigControlRedirect(UserControlConfig.Control aCtrl)
        {
            UserControlConfig.Url aUrl = null;

            if (!string.IsNullOrEmpty(this.LoginRole))
            {
                UserControlConfig.UrlRoleMapping aUrlRole =
                    aCtrl.UrlRoleMappings.FirstOrDefault(p => p.Role.Equals(this.LoginRole));

                if (aUrlRole != null) aUrl = aUrlRole.Url;
            }
            else
            {
                if (System.Web.Security.Roles.Enabled)
                {
                    var roles =
                        System.Web.Security.Roles.GetRolesForUser().Intersect(
                        aCtrl.UrlRoleMappings.Select(p => p.Role));

                    UserControlConfig.UrlRoleMapping aUrlRole =
                        aCtrl.UrlRoleMappings.FirstOrDefault(p => roles.Contains(p.Role));

                    if (aUrlRole != null) aUrl = aUrlRole.Url;
                }
            }

            if (aUrl == null)
            {
                aUrl = aCtrl.Url;
            }

            if (aCtrl.IsReturnControl)
            {
                this.RedirectReturn(aUrl);
            }
            if (aCtrl.IsCompleteControl)
            {
                this.RedirectCompleted(aUrl);
            }
            this.Redirect(aUrl);
        }

        /// <summary>
        /// 该方法被配置文件中 Controls 节中的控件自动订阅。主要用于控件执行命令。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Control_Command(object sender, CommandEventArgs e)
        {
            if (this.Config != null)
            {
                UserControlConfig.Control aConfigCtrl =
                    this.Config.Controls[((Control)sender).ID];

                if (aConfigCtrl != null)
                {
                    this.HandleCheckCommandPermission(aConfigCtrl.FunctionName,
                        new CheckPermissionRule(CheckPoint.ExecuteControlCommand,
                            CheckFailureMode.ThrowException, sender));

                    if (aConfigCtrl.IsExportControl)
                    {
                        this.ExportToExcel(aConfigCtrl.ExportTitle, aConfigCtrl.ExportFileName);
                    }
                    this.ConfigControlRedirect(aConfigCtrl);
                }
            }
        }

        /// <summary>
        /// 该方法被配置文件中 Controls 节中的控件自动订阅。主要用于控件属性设置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Control_PreRender(object sender, EventArgs e)
        {
            if (this.Config != null)
            {
                UserControlConfig.Control aControl =
                    this.Config.Controls[((Control)sender).ID];

                if (aControl != null)
                {
                    Control aCtrl = (Control)sender;

                    bool enabled = this.CalEnableRoles(aControl.EnableRoles) &&
                        this.HandleCheckCommandPermission(aControl.FunctionName,
                        new CheckPermissionRule(CheckPoint.ControlRender,
                            CheckFailureMode.HideControl, aCtrl));

                    if (aControl.EnableMode == EnableMode.Visible)
                    {
                        aCtrl.Visible = enabled;
                    }
                    else
                    {
                        System.Reflection.PropertyInfo aProp =
                            aCtrl.GetType().GetProperty("Enabled");

                        if (aProp != null)
                        {
                            aProp.SetValue(aCtrl, enabled, null);
                        }
                    }

                    if (!string.IsNullOrEmpty(aControl.Text))
                    {
                        System.Reflection.PropertyInfo aProp =
                            aCtrl.GetType().GetProperty("Text");

                        if (aProp != null)
                        {
                            aProp.SetValue(aCtrl, aControl.Text, null);
                        }
                    }

                    if (!string.IsNullOrEmpty(aControl.ToolTip))
                    {
                        System.Reflection.PropertyInfo aProp =
                            aCtrl.GetType().GetProperty("ToolTip");

                        if (aProp != null)
                        {
                            aProp.SetValue(aCtrl, aControl.ToolTip, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 该方法被配置文件中 Controls 节中的控件自动订阅。主要用于控件在执行数据绑定前执行的动作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Control_DataBinding(object sender, EventArgs e)
        {
            if (this.Config != null)
            {
                UserControlConfig.Control aControl =
                    this.Config.Controls[((Control)sender).ID];

                if (sender is HyperLink && 
                    aControl != null && aControl.Url != null && aControl.Url.CommunicationMode == CommunicationMode.QueryString)
                {
                    HyperLink hyperLnkCtrl = (HyperLink)sender;

                    if (hyperLnkCtrl.NamingContainer != null)
                    {
                        System.Reflection.PropertyInfo aDataItemProp =
                            hyperLnkCtrl.NamingContainer.GetType().GetProperty("DataItem");

                        if (aDataItemProp != null)
                        {
                            object aDataItem = aDataItemProp.GetValue(hyperLnkCtrl.NamingContainer, null);

                            CommunicationObjectCollection commObjs = new CommunicationObjectCollection();

                            if (aDataItem is DataRowView)
                            {
                                DataRowView drv = (DataRowView)aDataItem;

                                foreach (UserControlConfig.CommunicationObject aConfigCommObj in aControl.Url.CommunicationObjects)
                                {
                                    commObjs.Add(aConfigCommObj.Name, drv[aConfigCommObj.DataKeyName]);
                                }
                            }
                            else if (aDataItem is DataRow)
                            {
                                DataRow dr = (DataRow)aDataItem;

                                foreach (UserControlConfig.CommunicationObject aConfigCommObj in aControl.Url.CommunicationObjects)
                                {
                                    commObjs.Add(aConfigCommObj.Name, dr[aConfigCommObj.DataKeyName]);
                                }
                            }
                            else if (aDataItem != null)
                            {
                                foreach (UserControlConfig.CommunicationObject aConfigCommObj in aControl.Url.CommunicationObjects)
                                {
                                    System.Reflection.PropertyInfo aDataKeyProp =
                                        aDataItem.GetType().GetProperty(aConfigCommObj.DataKeyName);

                                    if (aDataKeyProp != null)
                                    {
                                        commObjs.Add(aConfigCommObj.Name, aDataKeyProp.GetValue(aDataItem, null));
                                    }
                                }
                            }
                            hyperLnkCtrl.NavigateUrl =
                                Utility.GetUrlString(aControl.Url.Path, commObjs.ToArray());
                        }
                    }
                }
            }
        }
    }
}