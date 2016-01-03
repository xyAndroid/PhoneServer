using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace eSchool.Web.UI.WebControls
{
    public class ConfigException : Exception
    {
        public ConfigException(string message)
            : base(message)
        {
        }
    }

    public class ConfigAttribute : Attribute
    {

    }

    public class ConfigNodeAttributeAttribute : Attribute
    {
        private object _defaultValue;
        public object DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        private bool _isNeeded;
        public bool IsNeeded
        {
            get { return _isNeeded; }
            set { _isNeeded = value; }
        }

        private bool _canConvertFromXml = true;
        public bool CanConvertFromXml
        {
            get { return _canConvertFromXml; }
            set { _canConvertFromXml = value; }
        }

        private bool _canConvertToXml = true;
        public bool CanConvertToXml
        {
            get { return _canConvertToXml; }
            set { _canConvertToXml = value; }
        }
    }

    /// <summary>
    /// 配置节点。
    /// </summary>
    public abstract class ConfigNode
    {
        private ConfigNode _config;
        /// <summary>
        /// 节点的配置对象。一般指有具体对应对象的顶级配置节点。
        /// </summary>
        protected ConfigNode Config
        {
            get { return _config; }
            set { _config = value; }
        }

        private string _xmlNodeName;
        internal string XmlNodeName
        {
            get { return _xmlNodeName; }
            private set { _xmlNodeName = value; }
        }

        internal ConfigNode(ConfigNode config,
            string xmlNodeName, string name)
        {
            this.Config = config;
            this.XmlNodeName = xmlNodeName;
            this.Name = name;
        }

        private System.Xml.XmlNode _xmlNode;
        internal virtual System.Xml.XmlNode XmlNode
        {
            get
            {
                if (_xmlNode == null && _config != null && _config.XmlNode != null)
                {
                    ConfigAttribute attr =
                        Attribute.GetCustomAttribute(typeof(ConfigNode).Assembly, typeof(ConfigAttribute))
                        as ConfigAttribute;

                    if (attr != null)
                    {
                        _xmlNode = _config.XmlNode;
                    }
                    else
                    {
                        _xmlNode = _config.XmlNode.SelectSingleNode(
                            string.Format("{0}[@{1}='{2}']",
                            XmlNodeName, NameAttributeName, Name));
                    }
                }

                return _xmlNode;
            }
        }

        public const string NameAttributeName = "Name";
        private string _name;
        /// <summary>
        /// 节点名。
        /// </summary>
        [ConfigNodeAttribute(IsNeeded = true)]
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        internal object GetDefaultValue(string propName)
        {
            System.Reflection.PropertyInfo aProp =
                this.GetType().GetProperty(propName);

            if (aProp == null)
            {
                throw new ConfigException(
                    string.Format(
                    WebControls.Properties.Resources.ConfigNodeNotFound,
                    propName));
            }

            ConfigNodeAttributeAttribute aAttr =
                Attribute.GetCustomAttribute(aProp, typeof(ConfigNodeAttributeAttribute))
                as ConfigNodeAttributeAttribute;

            if (aAttr == null)
            {
                throw new ConfigException(
                    string.Format(
                    WebControls.Properties.Resources.ConfigNodeNotFound,
                    propName));
            }

            return aAttr.DefaultValue;
        }

        internal virtual void AssignFromXmlNode()
        {
            if (this.XmlNode == null) return;

            foreach (System.Reflection.PropertyInfo aProp in
                this.GetType().GetProperties())
            {
                ConfigNodeAttributeAttribute aAttr =
                    Attribute.GetCustomAttribute(aProp, typeof(ConfigNodeAttributeAttribute))
                    as ConfigNodeAttributeAttribute;

                if (aAttr == null) continue;

                object propValue = aProp.GetValue(this, null);

                System.Xml.XmlAttribute xmlAttr =
                    this.XmlNode.Attributes[aProp.Name];

                if (aAttr.IsNeeded && xmlAttr == null)
                {
                    throw new ConfigException(
                        string.Format(
                        WebControls.Properties.Resources.ConfigAttributeNotFound,
                        aProp.Name, this.XmlNodeName + '/' + this.Name));
                }

                if (!aAttr.CanConvertFromXml) continue;

                if (xmlAttr == null)
                {
                    aProp.SetValue(this, aAttr.DefaultValue, null);
                }
                else
                {
                    Type underlyingType =
                        Nullable.GetUnderlyingType(aProp.PropertyType);

                    if (aProp.PropertyType.IsEnum)
                    {
                        aProp.SetValue(this,
                            Enum.Parse(aProp.PropertyType, xmlAttr.Value), null);
                    }
                    else if (underlyingType != null)
                    {
                        aProp.SetValue(this,
                            Convert.ChangeType(xmlAttr.Value, underlyingType), null);
                    }
                    else
                    {
                        aProp.SetValue(this,
                            Convert.ChangeType(xmlAttr.Value, aProp.PropertyType), null);
                    }
                }
            }
        }

        internal virtual void AssignToXmlNode()
        {
            if (this.XmlNode == null)
            {
                string thisNodeName = this.XmlNodeName.Substring(
                    this.XmlNodeName.IndexOf('/') + 1);
                string parentNodeName = this.XmlNodeName.Substring(0,
                    this.XmlNodeName.IndexOf('/'));

                _xmlNode =
                    this.Config.XmlNode.OwnerDocument.CreateElement(thisNodeName);

                System.Xml.XmlNode aNode =
                    this.Config.XmlNode.SelectSingleNode(parentNodeName);

                if (aNode == null)
                {
                    aNode =
                        this.Config.XmlNode.OwnerDocument.CreateElement(parentNodeName);
                    this.Config.XmlNode.AppendChild(aNode);
                }
                aNode.AppendChild(_xmlNode);
            }

            foreach (System.Reflection.PropertyInfo aProp in
                this.GetType().GetProperties())
            {
                ConfigNodeAttributeAttribute aAttr =
                    Attribute.GetCustomAttribute(aProp, typeof(ConfigNodeAttributeAttribute))
                    as ConfigNodeAttributeAttribute;

                if (aAttr == null) continue;

                object propValue = aProp.GetValue(this, null);

                System.Xml.XmlAttribute xmlAttr =
                    this.XmlNode.Attributes[aProp.Name];

                if (aAttr.IsNeeded)
                {
                    xmlAttr =
                        this.XmlNode.OwnerDocument.CreateAttribute(
                        aProp.Name);

                    this.XmlNode.Attributes.Append(xmlAttr);
                }

                if (!aAttr.CanConvertToXml) continue;

                if (aAttr.DefaultValue != null && propValue != null)
                {
                    if (xmlAttr == null)
                    {
                        if (!aAttr.DefaultValue.Equals(propValue))
                        {
                            xmlAttr =
                                this.XmlNode.OwnerDocument.CreateAttribute(
                                aProp.Name);

                            xmlAttr.Value = propValue.ToString();

                            this.XmlNode.Attributes.Append(xmlAttr);
                        }
                    }
                    else
                    {
                        xmlAttr.Value = propValue.ToString();
                    }
                }
                if (aAttr.DefaultValue == null && propValue != null)
                {
                    if (xmlAttr == null)
                    {
                        xmlAttr =
                            this.XmlNode.OwnerDocument.CreateAttribute(
                            aProp.Name);

                        xmlAttr.Value = propValue.ToString();

                        this.XmlNode.Attributes.Append(xmlAttr);
                    }
                    else
                    {
                        if (!propValue.ToString().Equals(xmlAttr.Value))
                        {
                            xmlAttr.Value = propValue.ToString();
                        }
                    }
                }
                if (aAttr.DefaultValue != null && propValue == null)
                {
                    if (xmlAttr != null)
                    {
                        this.XmlNode.Attributes.Remove(xmlAttr);
                    }
                    else
                    {
                        xmlAttr =
                            this.XmlNode.OwnerDocument.CreateAttribute(
                            aProp.Name);

                        xmlAttr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(xmlAttr);
                    }
                }
                if (aAttr.DefaultValue == null && propValue == null)
                {
                    if (xmlAttr != null)
                    {
                        xmlAttr.Value = string.Empty;
                    }
                }
            }
        }

        internal void CheckConfig()
        {
            System.Reflection.FieldInfo[] fields =
                this.GetType().GetFields(
                System.Reflection.BindingFlags.FlattenHierarchy |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static);

            foreach (System.Xml.XmlAttribute attr in this.XmlNode.Attributes)
            {
                bool bContains = false;
                foreach (System.Reflection.FieldInfo f in fields)
                {
                    if (f.Name.EndsWith("AttributeName") &&
                        attr.Name.Equals(f.GetValue(this)))
                    {
                        bContains = true; break;
                    }
                }
                if (!bContains)
                {
                    throw new ConfigException(
                        string.Format(
                        WebControls.Properties.Resources.ConfigIllegalAttribute,
                        attr.Name, this.XmlNodeName + '/' + this.Name));
                }
            }
        }
        /// <summary>
        /// 获得节点所属 XML 文档的所有文本。
        /// </summary>
        public string OwnerDocumentOuterXml
        {
            get
            {
                return this.XmlNode.OwnerDocument.OuterXml;
            }
        }
    }

    public abstract class ConfigNodeCollection<T> : List<T>
        where T : ConfigNode
    {
        private ConfigNode _config;
        protected ConfigNode Config
        {
            get { return _config; }
            private set { _config = value; }
        }

        private string _xmlNodeName;
        internal string XmlNodeName
        {
            get { return _xmlNodeName; }
            private set { _xmlNodeName = value; }
        }

        internal ConfigNodeCollection(ConfigNode config, string xmlNodeName)
        {
            this.Config = config;
            this.XmlNodeName = xmlNodeName;
        }

        public T this[string name]
        {
            get
            {
                return this.FindByName(name);
            }
        }

        public T FindByName(string name)
        {
            foreach (T t in this)
            {
                if (t.Name.Equals(name)) return t;
            }
            return null;
        }

        public bool Remove(string name)
        {
            T t = this[name];

            if (t != null)
                return this.Remove(t);

            return false;
        }

        internal virtual void UpdateXmlNodes()
        {
            foreach (T t in this)
            {
                t.AssignToXmlNode();
            }
            System.Xml.XmlNode aNode = 
                this.Config.XmlNode.SelectSingleNode(XmlNodeName);

            if (aNode == null) return;

            System.Xml.XmlNodeList nodes = aNode.ChildNodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType != System.Xml.XmlNodeType.Element) continue;

                bool bContains = false;
                foreach (T t in this)
                {
                    System.Xml.XmlAttribute attr =
                        nodes[i].Attributes[ConfigNode.NameAttributeName];

                    if (attr == null)
                    {
                        continue;
                    }
                    if (attr.Value.Equals(t.Name))
                    {
                        bContains = true; break;
                    }
                }
                if (!bContains)
                {
                    nodes[i].ParentNode.RemoveChild(nodes[i]);
                    i--;
                }
            }
        }
    }


    /// <summary>
    /// 用户控件的页面配置的解析类，并且代表了一个用户控件的配置根节点。
    /// </summary>
    [Config]
    public class UserControlConfig : ConfigNode
    {
        #region Fields

        private System.Xml.XmlDocument _xmlDoc;

        #endregion

        #region Properties

        private string _configFile;
        /// <summary>
        /// 获取配置文件名。
        /// </summary>
        public string ConfigFile
        {
            get
            {
                return _configFile;
            }
            private set
            {
                _configFile = value;
            }
        }

        private System.Xml.XmlNode _xmlNode;
        /// <summary>
        /// 获取配置文件中的配置节点。
        /// </summary>
        internal override System.Xml.XmlNode XmlNode
        {
            get
            {
                if (_xmlNode == null)
                {
                    _xmlNode = _xmlDoc.SelectSingleNode(
                        string.Format("{0}[@{1}='{2}']",
                        XmlNodeName, NameAttributeName, Name));

                    if (_xmlNode == null)
                    {
                        throw new ConfigException(
                            string.Format(
                            WebControls.Properties.Resources.ConfigNodeNotFound,
                            XmlNodeName));
                    }
                }

                return _xmlNode;
            }
        }

        /// <summary>
        /// 视图模式的配置属性名。
        /// </summary>
        public const string ViewModeAttributeName = "ViewMode";
        private ViewMode _viewMode = ViewMode.AllItems;
        /// <summary>
        /// 获取与设置视图模式。默认为 AllItems 模式。
        /// </summary>
        [ConfigNodeAttribute(DefaultValue = ViewMode.AllItems, IsNeeded = true)]
        public ViewMode ViewMode
        {
            get { return _viewMode; }
            set { _viewMode = value; }
        }

        /// <summary>
        /// 数据通信方向的配置属性名。
        /// </summary>
        public const string CommunicationDirectionAttributeName = "CommunicationDirection";
        private CommunicationDirection _commDir = CommunicationDirection.Bidirect;
        /// <summary>
        /// 获取与设置数据通信方向。默认为 Bidirect 。
        /// </summary>
        [ConfigNodeAttribute(DefaultValue = CommunicationDirection.Bidirect)]
        public CommunicationDirection CommunicationDirection
        {
            get { return _commDir; }
            set { _commDir = value; }
        }

        /// <summary>
        /// 返回地址的配置属性名。
        /// </summary>
        public const string ReturnUrlAttributeName = "ReturnUrl";
        private Url _returnUrl;
        /// <summary>
        /// 获取与设置返回地址。默认为空。
        /// </summary>
        [ConfigNodeAttribute(CanConvertFromXml = false, CanConvertToXml = false)]
        public Url ReturnUrl
        {
            get { return _returnUrl; }
            set { _returnUrl = value; }
        }

        /// <summary>
        /// 完成地址的配置属性名。
        /// </summary>
        public const string CompletedUrlAttributeName = "CompletedUrl";
        private Url _completedUrl;
        /// <summary>
        /// 获取与设置完成地址。默认为空。
        /// </summary>
        [ConfigNodeAttribute(CanConvertFromXml = false, CanConvertToXml = false)]
        public Url CompletedUrl
        {
            get { return _completedUrl; }
            set { _completedUrl = value; }
        }

        /// <summary>
        /// 是否初始检索数据的配置属性名。
        /// </summary>
        public const string IsInitSelectDataAttributeName = "IsInitSelectData";
        private bool _isInitSelectData = true;
        /// <summary>
        /// 获取与设置是否初始检索数据。默认为 true 。
        /// </summary>
        [ConfigNodeAttribute(DefaultValue = true)]
        public bool IsInitSelectData
        {
            get { return _isInitSelectData; }
            set { _isInitSelectData = value; }
        }

        /// <summary>
        /// 行选择模式的配置属性名。
        /// </summary>
        public const string RowSelectModeAttributeName = "RowSelectMode";
        private RowSelectMode _rowSelectMode = RowSelectMode.None;
        /// <summary>
        /// 获取与设置行选择模式。默认为 None 。
        /// </summary>
        [ConfigNodeAttribute(DefaultValue = RowSelectMode.None)]
        public RowSelectMode RowSelectMode
        {
            get { return _rowSelectMode; }
            set { _rowSelectMode = value; }
        }

        /// <summary>
        /// 功能名称的配置属性名。
        /// </summary>
        public const string FunctionNameAttributeName = "FunctionName";
        private string _functionName;
        /// <summary>
        /// 获取与设置功能名称。默认为空字符串。
        /// </summary>
        [ConfigNodeAttribute]
        public string FunctionName
        {
            get { return _functionName; }
            set { _functionName = value; }
        }

        private PropertyCollection _properties;
        /// <summary>
        /// 获取所有 Property 配置节点的配置。
        /// </summary>
        public PropertyCollection Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new PropertyCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_properties.XmlNodeName);

                        if (aNode == null) return _properties;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[Property.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    Property.NameAttributeName, typeof(Property).GetType().Name));
                            }

                            Property aProp = new Property(this, attr.Value);

                            aProp.CheckConfig();

                            aProp.AssignFromXmlNode();

                            _properties.Add(aProp);
                        }
                    }
                }
                return _properties;
            }
        }

        private ControlCollection _controls;
        /// <summary>
        /// 获取所有 Control 配置节点的配置。
        /// </summary>
        public ControlCollection Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new ControlCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_controls.XmlNodeName);

                        if (aNode == null) return _controls;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[Control.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    Control.NameAttributeName, typeof(Control).GetType().Name));
                            }

                            Control aCtrl = new Control(this, attr.Value);

                            aCtrl.CheckConfig();

                            aCtrl.AssignFromXmlNode();

                            _controls.Add(aCtrl);
                        }
                    }
                }
                return _controls;
            }
        }

        private UrlCollection _urls;
        /// <summary>
        /// 获取所有 Url 配置节点的配置。
        /// </summary>
        public UrlCollection Urls
        {
            get
            {
                if (_urls == null)
                {
                    _urls = new UrlCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_urls.XmlNodeName);

                        if (aNode == null) return _urls;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[Url.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    Url.NameAttributeName, typeof(Url).GetType().Name));
                            }

                            Url aUrl = new Url(this, attr.Value);

                            aUrl.CheckConfig();

                            aUrl.AssignFromXmlNode();

                            _urls.Add(aUrl);
                        }
                    }
                }
                return _urls;
            }
        }

        private UrlRoleMappingCollection _urlRoleMappings;
        /// <summary>
        /// 获取所有 UrlRoleMapping 配置节点的配置。
        /// </summary>
        public UrlRoleMappingCollection UrlRoleMappings
        {
            get
            {
                if (_urlRoleMappings == null)
                {
                    _urlRoleMappings = new UrlRoleMappingCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_urlRoleMappings.XmlNodeName);

                        if (aNode == null) return _urlRoleMappings;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            System.Xml.XmlAttribute attr =
                                aXmlNode.Attributes[UrlRoleMapping.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    UrlRoleMapping.NameAttributeName, typeof(UrlRoleMapping).GetType().Name));
                            }

                            UrlRoleMapping aUrlRole = new UrlRoleMapping(this, attr.Value);

                            aUrlRole.CheckConfig();

                            aUrlRole.AssignFromXmlNode();

                            _urlRoleMappings.Add(aUrlRole);
                        }
                    }
                }
                return _urlRoleMappings;
            }
        }

        private CommunicationObjectCollection _commObjs;
        /// <summary>
        /// 获取所有 CommunicationObject 配置节点的配置。
        /// </summary>
        public CommunicationObjectCollection CommunicationObjects
        {
            get
            {
                if (_commObjs == null)
                {
                    _commObjs = new CommunicationObjectCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_commObjs.XmlNodeName);

                        if (aNode == null) return _commObjs;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[
                                CommunicationObject.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    CommunicationObject.NameAttributeName, typeof(CommunicationObject).GetType().Name));
                            }

                            CommunicationObject aCommObj = 
                                new CommunicationObject(this, attr.Value);

                            aCommObj.CheckConfig();

                            aCommObj.AssignFromXmlNode();

                            _commObjs.Add(aCommObj);
                        }
                    }
                }
                return _commObjs;
            }
        }

        private DataColumnCollection _dataColumns;
        /// <summary>
        /// 获取所有 DataColumn 配置节点的配置。
        /// </summary>
        public DataColumnCollection DataColumns
        {
            get
            {
                if (_dataColumns == null)
                {
                    _dataColumns = new DataColumnCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_dataColumns.XmlNodeName);

                        if (aNode == null) return _dataColumns;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[DataColumn.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    DataColumn.NameAttributeName, typeof(DataColumn).GetType().Name));
                            }

                            DataColumn aDataColumn = new DataColumn(this, attr.Value);

                            aDataColumn.CheckConfig();

                            aDataColumn.AssignFromXmlNode();

                            _dataColumns.Add(aDataColumn);
                        }
                    }
                }
                return _dataColumns;
            }
        }

        #endregion

        /// <summary>
        /// 保存对配置的修改到配置文件。
        /// </summary>
        public void Save()
        {
            this.AssignToXmlNode();

            this.CommunicationObjects.UpdateXmlNodes();
            this.Controls.UpdateXmlNodes();
            this.Properties.UpdateXmlNodes();
            this.DataColumns.UpdateXmlNodes();
            this.Urls.UpdateXmlNodes();
            this.UrlRoleMappings.UpdateXmlNodes();

            _xmlDoc.Save(System.Web.HttpContext.Current.Server.MapPath(this.ConfigFile));
        }

        internal override void AssignFromXmlNode()
        {
            base.AssignFromXmlNode();

            System.Xml.XmlAttribute attr =
                this.XmlNode.Attributes[UserControlConfig.ReturnUrlAttributeName];

            if (attr != null)
            {
                this.ReturnUrl =
                    this.Urls[attr.Value];
            }

            attr =
                this.XmlNode.Attributes[UserControlConfig.CompletedUrlAttributeName];

            if (attr != null)
            {
                this.CompletedUrl =
                    this.Urls[attr.Value];
            }
        }

        internal override void AssignToXmlNode()
        {
            base.AssignToXmlNode();

            System.Xml.XmlAttribute attr =
                this.XmlNode.Attributes[UserControlConfig.ReturnUrlAttributeName];

            if (attr != null)
            {
                attr.Value = string.Empty;
            }

            if (this.ReturnUrl != null)
            {
                if (attr == null)
                {
                    attr = this.XmlNode.OwnerDocument.CreateAttribute(
                        UserControlConfig.ReturnUrlAttributeName);

                    attr.Value = string.Empty;

                    this.XmlNode.Attributes.Append(attr);
                }

                attr.Value = this.ReturnUrl.Name;
            }


            attr =
                this.XmlNode.Attributes[UserControlConfig.CompletedUrlAttributeName];

            if (attr != null)
            {
                attr.Value = string.Empty;
            }

            if (this.CompletedUrl != null)
            {
                if (attr == null)
                {
                    attr = this.XmlNode.OwnerDocument.CreateAttribute(
                        UserControlConfig.CompletedUrlAttributeName);

                    attr.Value = string.Empty;

                    this.XmlNode.Attributes.Append(attr);
                }

                attr.Value = this.CompletedUrl.Name;
            }
        }

        /// <summary>
        /// 构造方法。
        /// </summary>
        /// <param name="xmlConfigFileName">配置文件。</param>
        /// <param name="name">节点名。</param>
        public UserControlConfig(string xmlConfigFileName, string name)
            : base(null, "UserControlConfigs/UserControlConfig", name)
        {
            this.Config = this;

            this.ConfigFile = xmlConfigFileName;

            string fullFileName = System.Web.HttpContext.Current.Server.MapPath(xmlConfigFileName);

            if (!File.Exists(fullFileName))
            {
                using (StreamWriter sw = File.CreateText(fullFileName))
                {
                    sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                    sw.WriteLine(@"<UserControlConfigs>");
                    sw.WriteLine(string.Format(@"<UserControlConfig Name=""{0}"" ViewMode=""AllItems"">", name));
                    sw.WriteLine(@"</UserControlConfig>");
                    sw.WriteLine(@"</UserControlConfigs>");
                }
            }

            _xmlDoc = new System.Xml.XmlDocument();
            _xmlDoc.Load(fullFileName);

            if (this.XmlNode != null)
            {
                this.CheckConfig();

                this.AssignFromXmlNode();
            }
        }

        /// <summary>
        /// 配置节点 Property 配置。
        /// </summary>
        public class Property : ConfigNode
        {
            #region Properties

            public const string ValueAttributeName = "Value";
            private object _value;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml= false)]
            public object Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public const string TypeAttributeName = "Type";
            private Type _type;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false)]
            public Type Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public const string PageEffectiveAttributeName = "PageEffective";
            private PageEffective _effective = PageEffective.Initial;
            [ConfigNodeAttribute(DefaultValue = PageEffective.Initial)]
            public PageEffective PageEffective
            {
                get { return _effective; }
                set { _effective = value; }
            }

            #endregion

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Property.TypeAttributeName];

                if (attr != null)
                {
                    this.Type = Type.GetType(attr.Value);
                }

                attr =
                    this.XmlNode.Attributes[Property.ValueAttributeName];

                if (attr != null)
                {
                    if (this.Type.IsEnum)
                    {
                        this.Value =
                            Enum.Parse(this.Type, attr.Value);
                    }
                    else
                    {
                        this.Value =
                            Convert.ChangeType(attr.Value, this.Type);
                    }
                }
            }

            internal Property(UserControlConfig config, string name)
                : base(config, "Properties/Property", name)
            {
            }
        }

        public class PropertyCollection : ConfigNodeCollection<Property>
        {
            internal PropertyCollection(UserControlConfig config)
                : base(config, "Properties")
            {
            }

            public Property CreateProperty(string name)
            {
                return new Property((UserControlConfig)this.Config, name);
            }
        }

        public class Control : ConfigNode
        {
            #region Properties

            public const string EnableRolesAttributeName = "EnableRoles";
            private string _enableRoles;
            [ConfigNodeAttribute(IsNeeded = true)]
            public string EnableRoles
            {
                get { return _enableRoles; }
                set { _enableRoles = value; }
            }

            public const string EnableModeAttributeName = "EnableMode";
            private EnableMode _enableMode = EnableMode.Visible;
            [ConfigNodeAttribute(DefaultValue = EnableMode.Visible)]
            public EnableMode EnableMode
            {
                get { return _enableMode; }
                set { _enableMode = value; }
            }

            public const string UrlAttributeName = "Url";
            private Url _url;
            [ConfigNodeAttribute(CanConvertFromXml = false, CanConvertToXml = false)]
            public Url Url
            {
                get { return _url; }
                set { _url = value; }
            }

            public const string IsExposedAttributeName = "IsExposed";
            private bool _isExposed = false;
            [ConfigNodeAttribute(DefaultValue = false)]
            public bool IsExposed
            {
                get { return _isExposed; }
                set { _isExposed = value; }
            }

            public const string FunctionNameAttributeName = "FunctionName";
            private string _funcName;
            [ConfigNodeAttribute]
            public string FunctionName
            {
                get { return _funcName; }
                set { _funcName = value; }
            }

            public const string IsExportControlAttributeName = "IsExportControl";
            private bool _isExportControl = false;
            [ConfigNodeAttribute(DefaultValue = false)]
            public bool IsExportControl
            {
                get { return _isExportControl; }
                set { _isExportControl = value; }
            }

            public const string ExportFileNameAttributeName = "ExportFileName";
            private string _exportFileName;
            [ConfigNodeAttribute]
            public string ExportFileName
            {
                get { return _exportFileName; }
                set { _exportFileName = value; }
            }

            public const string ExportTitleAttributeName = "ExportTitle";
            private string _exportTitle;
            [ConfigNodeAttribute]
            public string ExportTitle
            {
                get { return _exportTitle; }
                set { _exportTitle = value; }
            }

            public const string IsReturnControlAttributeName = "IsReturnControl";
            private bool _isReturnControl = false;
            [ConfigNodeAttribute(DefaultValue = false)]
            public bool IsReturnControl
            {
                get { return _isReturnControl; }
                set { _isReturnControl = value; }
            }

            public const string IsCompleteControlAttributeName = "IsCompleteControl";
            private bool _isCompleteControl = false;
            [ConfigNodeAttribute(DefaultValue = false)]
            public bool IsCompleteControl
            {
                get { return _isCompleteControl; }
                set { _isCompleteControl = value; }
            }

            public const string TextAttributeName = "Text";
            private string _text;
            [ConfigNodeAttribute]
            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            public const string ToolTipAttributeName = "ToolTip";
            private string _toolTip;
            [ConfigNodeAttribute]
            public string ToolTip
            {
                get { return _toolTip; }
                set { _toolTip = value; }
            }

            public const string UrlRoleMappingsAttributeName = "UrlRoleMappings";
            private UrlRoleMappingCollection _urlRoleMappings;
            [ConfigNodeAttribute(CanConvertFromXml = false, CanConvertToXml = false)]
            public UrlRoleMappingCollection UrlRoleMappings
            {
                get
                {
                    if (_urlRoleMappings == null)
                    {
                        _urlRoleMappings = new UrlRoleMappingCollection((UserControlConfig)this.Config);
                    }
                    return _urlRoleMappings;
                }
            }

            #endregion

            internal Control(UserControlConfig config, string name)
                : base(config, "Controls/Control", name)
            {
            }

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Control.UrlAttributeName];

                if (attr != null)
                {
                    this.Url =
                        ((UserControlConfig)this.Config).Urls[attr.Value];
                }

                attr =
                    this.XmlNode.Attributes[Control.UrlRoleMappingsAttributeName];

                if (attr != null)
                {
                    foreach (string name in
                        attr.Value.Split(
                        System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries))
                    {
                        UrlRoleMapping aUrlRole =
                            ((UserControlConfig)this.Config).UrlRoleMappings[name];

                        if (aUrlRole != null)
                        {
                            this.UrlRoleMappings.Add(aUrlRole);
                        }
                    }
                }
            }

            internal override void AssignToXmlNode()
            {
                base.AssignToXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Control.UrlAttributeName];

                if (attr != null)
                {
                    attr.Value = string.Empty;
                }

                if (this.Url != null)
                {
                    if (attr == null)
                    {
                        attr = this.XmlNode.OwnerDocument.CreateAttribute(
                            Control.UrlAttributeName);

                        attr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(attr);
                    }

                    attr.Value = this.Url.Name;
                }

                attr =
                    this.XmlNode.Attributes[Control.UrlRoleMappingsAttributeName];

                if (attr != null)
                {
                    attr.Value = string.Empty;
                }

                if (this.UrlRoleMappings.Count > 0)
                {
                    if (attr == null)
                    {
                        attr = this.XmlNode.OwnerDocument.CreateAttribute(
                            Control.UrlRoleMappingsAttributeName);

                        attr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(attr);
                    }

                    foreach (UrlRoleMapping aUrlRole in this.UrlRoleMappings)
                    {
                        attr.Value =
                            string.Format("{0}{1}{2}",
                            attr.Value,
                            (string.IsNullOrEmpty(attr.Value) ? "" : ","),
                            aUrlRole.Name);
                    }
                }
            }
        }

        public class ControlCollection : ConfigNodeCollection<Control>
        {
            internal ControlCollection(UserControlConfig config)
                : base(config, "Controls")
            {
            }

            public Control CreateControl(string name)
            {
                return new Control((UserControlConfig)this.Config, name);
            }
        }

        public class Url : ConfigNode
        {
            #region Properties

            public const string CommunicationModeAttributeName = "CommunicationMode";
            private CommunicationMode _commMode = CommunicationMode.None;
            [ConfigNodeAttribute(DefaultValue = CommunicationMode.None)]
            public CommunicationMode CommunicationMode
            {
                get { return _commMode; }
                set { _commMode = value; }
            }

            public const string PathAttributeName = "Path";
            private string _path;
            [ConfigNodeAttribute(IsNeeded = true)]
            public string Path
            {
                get { return _path; }
                set { _path = value; }
            }

            public const string GenerateReturnUrlAttributeName = "GenerateReturnUrl";
            private bool _genReturnUrl = false;
            [ConfigNodeAttribute(DefaultValue = false)]
            public bool GenerateReturnUrl
            {
                get { return _genReturnUrl; }
                set { _genReturnUrl = value; }
            }

            public const string CommunicationObjectsAttributeName = "CommunicationObjects";
            private CommunicationObjectCollection _commObjs;
            [ConfigNodeAttribute(CanConvertFromXml = false, CanConvertToXml = false)]
            public CommunicationObjectCollection CommunicationObjects
            {
                get
                {
                    if (_commObjs == null)
                    {
                        _commObjs = new CommunicationObjectCollection((UserControlConfig)this.Config);
                    }
                    return _commObjs;
                }
            }

            #endregion

            internal Url(UserControlConfig config, string name)
                : base(config, "Urls/Url", name)
            {
            }

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Url.CommunicationObjectsAttributeName];

                if (attr != null)
                {
                    string[] commObjNames = attr.Value.Split(
                        System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string aCommObjName in commObjNames)
                    {
                        CommunicationObject aCommObj = 
                            ((UserControlConfig)this.Config).CommunicationObjects[aCommObjName];

                        if (aCommObj != null)
                        {
                            this.CommunicationObjects.Add(aCommObj);
                        }
                    }
                }
            }

            internal override void AssignToXmlNode()
            {
                base.AssignToXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Url.CommunicationObjectsAttributeName];

                if (attr != null)
                {
                    attr.Value = string.Empty;
                }

                if (this.CommunicationObjects.Count > 0)
                {
                    if (attr == null)
                    {
                        attr = this.XmlNode.OwnerDocument.CreateAttribute(
                            Url.CommunicationObjectsAttributeName);

                        attr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(attr);
                    }

                    foreach (CommunicationObject aCommObj in this.CommunicationObjects)
                    {
                        attr.Value =
                            string.Format("{0}{1}{2}",
                            attr.Value,
                            (string.IsNullOrEmpty(attr.Value) ? "" : ","),
                            aCommObj.Name);
                    }
                }
            }
        }

        public class UrlCollection : ConfigNodeCollection<Url>
        {
            internal UrlCollection(UserControlConfig config)
                : base(config, "Urls")
            {
            }

            public Url CreateUrl(string name)
            {
                return new Url((UserControlConfig)this.Config, name);
            }
        }

        public class UrlRoleMapping : ConfigNode
        {
            #region Properties

            public const string UrlAttributeName = "Url";
            private Url _url;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false, CanConvertToXml = false)]
            public Url Url
            {
                get { return _url; }
                set { _url = value; }
            }

            public const string RoleAttributeName = "Role";
            private string _role;
            [ConfigNodeAttribute(IsNeeded = true)]
            public string Role
            {
                get { return _role; }
                set { _role = value; }
            }

            #endregion

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[UrlRoleMapping.UrlAttributeName];

                if (attr != null)
                {
                    this.Url =
                        ((UserControlConfig)this.Config).Urls[attr.Value];
                }
            }

            internal override void AssignToXmlNode()
            {
                base.AssignToXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[UrlRoleMapping.UrlAttributeName];

                if (attr != null)
                {
                    attr.Value = string.Empty;
                }

                if (this.Url != null)
                {
                    if (attr == null)
                    {
                        attr = this.XmlNode.OwnerDocument.CreateAttribute(
                            UrlRoleMapping.UrlAttributeName);

                        attr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(attr);
                    }

                    attr.Value = this.Url.Name;
                }
            }

            internal UrlRoleMapping(UserControlConfig config, string name)
                : base(config, "UrlRoleMappings/UrlRoleMapping", name)
            {
            }
        }

        public class UrlRoleMappingCollection : ConfigNodeCollection<UrlRoleMapping>
        {
            internal UrlRoleMappingCollection(UserControlConfig config)
                : base(config, "UrlRoleMappings")
            {
            }

            public UrlRoleMapping CreateUrlRoleMapping(string name)
            {
                return new UrlRoleMapping((UserControlConfig)this.Config, name);
            }
        }

        public class CommunicationObject : ConfigNode
        {
            #region Properties

            public const string DefaultValueAttributeName = "DefaultValue";
            private object _defaultValue;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false)]
            public object DefaultValue
            {
                get { return _defaultValue; }
                set { _defaultValue = value; }
            }

            public const string TypeAttributeName = "Type";
            private Type _type;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false)]
            public Type Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public const string DataKeyNameAttributeName = "DataKeyName";
            private string _dataKeyName;
            [ConfigNodeAttribute]
            public string DataKeyName
            {
                get { return _dataKeyName; }
                set { _dataKeyName = value; }
            }

            public const string ControlPropertyAttributeName = "ControlProperty";
            private string _ctrlProp;
            [ConfigNodeAttribute]
            public string ControlProperty
            {
                get { return _ctrlProp; }
                set { _ctrlProp = value; }
            }

            #endregion

            internal CommunicationObject(UserControlConfig config, string name)
                : base(config, "CommunicationObjects/CommunicationObject", name)
            {
            }

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[CommunicationObject.TypeAttributeName];

                if (attr != null)
                {
                    this.Type = Type.GetType(attr.Value);
                }

                attr =
                    this.XmlNode.Attributes[CommunicationObject.DefaultValueAttributeName];

                if (attr != null)
                {
                    if (this.Type.IsEnum)
                    {
                        this.DefaultValue =
                            Enum.Parse(this.Type, attr.Value);
                    }
                    else
                    {
                        this.DefaultValue =
                            Convert.ChangeType(attr.Value, this.Type);
                    }
                }
            }
        }

        public class CommunicationObjectCollection : ConfigNodeCollection<CommunicationObject>
        {
            internal CommunicationObjectCollection(UserControlConfig config)
                : base(config, "CommunicationObjects")
            {
            }

            public CommunicationObject CreateCommunicationObject(string name)
            {
                return new CommunicationObject((UserControlConfig)this.Config, name);
            }
        }

        public class DataColumn : ConfigNode
        {
            #region Properties

            public const string EnableRolesAttributeName = "EnableRoles";
            private string _enableRoles;
            [ConfigNodeAttribute(IsNeeded = true)]
            public string EnableRoles
            {
                get { return _enableRoles; }
                set { _enableRoles = value; }
            }

            public const string DefaultVisibleAttributeName = "DefaultVisible";
            private bool _defaultVisible = true;
            [ConfigNodeAttribute(DefaultValue = true)]
            public bool DefaultVisible
            {
                get { return _defaultVisible; }
                set { _defaultVisible = value; }
            }

            public const string EnableExportAttributeName = "EnableExport";
            private bool? _enableExport;
            [ConfigNodeAttribute(CanConvertToXml = false)]
            public bool? EnableExport
            {
                get { return _enableExport; }
                set { _enableExport = value; }
            }

            public const string OrderAttributeName = "Order";
            private int _order = -1;
            [ConfigNodeAttribute(DefaultValue = -1)]
            public int Order
            {
                get { return _order; }
                set { _order = value; }
            }

            #endregion

            internal override void AssignToXmlNode()
            {
                base.AssignToXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[DataColumn.EnableExportAttributeName];

                if (this.EnableExport != null)
                {
                    if (attr == null)
                    {
                        attr = this.XmlNode.OwnerDocument.CreateAttribute(
                            DataColumn.EnableExportAttributeName);

                        attr.Value = string.Empty;

                        this.XmlNode.Attributes.Append(attr);
                    }

                    attr.Value = this.EnableExport.ToString();
                }
                else
                {
                    if (attr != null)
                        this.XmlNode.Attributes.Remove(attr);
                }
            }

            internal DataColumn(UserControlConfig config, string name)
                : base(config, "DataColumns/DataColumn", name)
            {
            }
        }

        public class DataColumnCollection : ConfigNodeCollection<DataColumn>
        {
            internal DataColumnCollection(UserControlConfig config)
                : base(config, "DataColumns")
            {
            }

            public DataColumn CreateDataColumn(string name)
            {
                return new DataColumn((UserControlConfig)this.Config, name);
            }
        }
    }
}
