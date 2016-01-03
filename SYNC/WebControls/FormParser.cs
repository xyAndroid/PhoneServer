using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 表单解析器。
    /// </summary>
    [Config]
    public class Form : ConfigNode
    {
        private System.Xml.XmlDocument _xmlDoc;
        private System.Xml.XmlNode _xmlNode;
        /// <summary>
        /// 获取模板文件中的配置节点。
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
        /// 构造方法。
        /// </summary>
        /// <param name="xmlTemplateFileName">模板文件名。</param>
        /// <param name="name">表单名称。</param>
        public Form(string xmlTemplateFileName, string name)
            : base(null, "Forms/Form", name)
        {
            this.Config = this;

            this.TemplateFile = xmlTemplateFileName;

            string fullFileName = System.Web.HttpContext.Current.Server.MapPath(xmlTemplateFileName);

            _xmlDoc = new System.Xml.XmlDocument();
            _xmlDoc.Load(fullFileName);

            if (this.XmlNode != null)
            {
                this.CheckConfig();

                this.AssignFromXmlNode();
            }
        }
        public Form()
            : base(null, "Forms/Form", null)
        {
            this.Config = this;
        }
        public void LoadXml(string xmlText, string name)
        {
            this.Name = name;

            _xmlDoc = new System.Xml.XmlDocument();
            _xmlDoc.LoadXml(xmlText);

            if (this.XmlNode != null)
            {
                this.CheckConfig();

                this.AssignFromXmlNode();
            }
        }

        /// <summary>
        /// 更新 Xml 节点数据。
        /// </summary>
        public void UpdateXmlNodes()
        {
            this.Fields.UpdateXmlNodes();
        }

        private string _templFileName = string.Empty;
        /// <summary>
        /// 模板文件名称。
        /// </summary>
        public string TemplateFile
        {
            get { return _templFileName; }
            protected set { _templFileName = value; }
        }

        public const string FieldPrefixAttributeName = "FieldPrefix";
        private string _fieldPrefix;
        /// <summary>
        /// 字段名前缀。
        /// </summary>
        [ConfigNodeAttribute(IsNeeded = true)]
        public string FieldPrefix
        {
            get { return _fieldPrefix; }
            protected set { _fieldPrefix = value; }
        }

        private FieldCollection _fields;
        /// <summary>
        /// 获得所有字段。
        /// </summary>
        public FieldCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new FieldCollection(this);

                    if (this.XmlNode != null)
                    {
                        System.Xml.XmlNode aNode =
                            this.XmlNode.SelectSingleNode(_fields.XmlNodeName);

                        if (aNode == null) return _fields;

                        System.Xml.XmlNodeList nodes = aNode.ChildNodes;

                        foreach (System.Xml.XmlNode aXmlNode in nodes)
                        {
                            if (aXmlNode.NodeType != System.Xml.XmlNodeType.Element) continue;

                            System.Xml.XmlAttribute attr = aXmlNode.Attributes[Field.NameAttributeName];
                            if (attr == null)
                            {
                                throw new ConfigException(
                                    string.Format(
                                    WebControls.Properties.Resources.ConfigAttributeNotFound,
                                    Field.NameAttributeName, typeof(Field).GetType().Name));
                            }

                            Field aFld = new Field(this, attr.Value);

                            aFld.CheckConfig();

                            aFld.AssignFromXmlNode();

                            _fields.Add(aFld);
                        }
                    }
                }
                return _fields;
            }
        }

        /// <summary>
        /// 表单。
        /// </summary>
        public class Field : ConfigNode
        {
            internal Field(Form form, string name)
                : base(form, "Fields/Field", name)
            {
            }

            public const string TypeAttributeName = "Type";
            private Type _type;
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false)]
            public Type Type
            {
                get { return _type; }
                set { _type = value; }
            }

            public const string ValueAttributeName = "Value";
            private object _value;
            /// <summary>
            /// 字段值。
            /// </summary>
            [ConfigNodeAttribute(IsNeeded = true, CanConvertFromXml = false)]
            public Object Value
            {
                get { return _value; }
                set { _value = value; }
            }

            public const string MinValueAttributeName = "MinValue";
            private object _minValue;
            /// <summary>
            /// 最小值。
            /// </summary>
            [ConfigNodeAttribute(CanConvertFromXml = false)]
            public Object MinValue
            {
                get { return _minValue; }
                set { _minValue = value; }
            }

            public const string MaxValueAttributeName = "MaxValue";
            private object _maxValue;
            /// <summary>
            /// 最大值。
            /// </summary>
            [ConfigNodeAttribute(CanConvertFromXml = false)]
            public Object MaxValue
            {
                get { return _maxValue; }
                set { _maxValue = value; }
            }

            public const string DescriptionAttributeName = "Description";
            private string _description;
            /// <summary>
            /// 字段值。
            /// </summary>
            [ConfigNodeAttribute(IsNeeded = false)]
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            internal override void AssignFromXmlNode()
            {
                base.AssignFromXmlNode();

                System.Xml.XmlAttribute attr =
                    this.XmlNode.Attributes[Field.TypeAttributeName];

                if (attr != null)
                {
                    this.Type = Type.GetType(attr.Value);
                }

                attr =
                    this.XmlNode.Attributes[Field.ValueAttributeName];

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

                attr =
                    this.XmlNode.Attributes[Field.MinValueAttributeName];

                if (attr != null)
                {
                    if (this.Type.IsEnum)
                    {
                        this.MinValue =
                            Enum.Parse(this.Type, attr.Value);
                    }
                    else
                    {
                        this.MinValue =
                            Convert.ChangeType(attr.Value, this.Type);
                    }
                }

                attr =
                    this.XmlNode.Attributes[Field.MaxValueAttributeName];

                if (attr != null)
                {
                    if (this.Type.IsEnum)
                    {
                        this.MaxValue =
                            Enum.Parse(this.Type, attr.Value);
                    }
                    else
                    {
                        this.MaxValue =
                            Convert.ChangeType(attr.Value, this.Type);
                    }
                }
            }

            internal override void AssignToXmlNode()
            {
                base.AssignToXmlNode();
            }
        }
        /// <summary>
        /// 表单集合。
        /// </summary>
        public class FieldCollection : ConfigNodeCollection<Field>
        {
            internal FieldCollection(Form form)
                : base(form, "Fields")
            {
            }
            /// <summary>
            /// 创建表单。
            /// </summary>
            /// <param name="name">表单名称。</param>
            /// <returns>表单。</returns>
            public Field CreateForm(string name)
            {
                return new Field((Form)this.Config, name);
            }
        }
    }

    /// <summary>
    /// 表单解析控件。
    /// </summary>
    [ToolboxData("<{0}:FormParser runat=server></{0}:FormParser>")]
    public class FormParser : System.Web.UI.WebControls.Xml
    {
        private Form _form;
        /// <summary>
        /// 被解析的表单对象。
        /// </summary>
        [Browsable(false)]
        public Form Form
        {
            get
            {
                if (_form == null)
                {
                    if (!string.IsNullOrEmpty(this.DocumentSource))
                    {
                        _form = new Form(this.DocumentSource, this.FormName);
                    }
                    else
                    {
                        _form = new Form();
                        _form.LoadXml(this.DocumentContent, this.FormName);
                    }
                }
                return _form;
            }
        }
        /// <summary>
        /// 表单名称。
        /// </summary>
        [Description("表单名称。")]
        [Category("数据")]
        [Browsable(true)]
        public string FormName
        {
            get
            {
                return this.ViewState["FormName"] as string;
            }
            set
            {
                this.ViewState["FormName"] = value;
            }
        }
        /// <summary>
        /// Xml 文本。
        /// </summary>
        [Description("Xml 文本。")]
        [Category("行为")]
        [Browsable(true)]
        [Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]
        public new string DocumentContent
        {
            get
            {
                return this.ViewState["DocumentContent"] as string;
            }
            set
            {
                this.ViewState["DocumentContent"] = value;
            }
        }

        /// <summary>
        /// 引发 FormParser 的 Load 事件。
        /// </summary>
        /// <param name="e">包含事件数据的参数对象。</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnInit(e);

            if (this.Page.IsPostBack &&
                (!string.IsNullOrEmpty(this.DocumentContent) || !string.IsNullOrEmpty(this.DocumentSource)))
            {
                UpdateFormXmlContent();
            }
        }
        /// <summary>
        /// 引发 FormParser 的 PreRender 事件。
        /// </summary>
        /// <param name="e">包含事件数据的参数对象。</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!string.IsNullOrEmpty(this.DocumentContent))
            {
                base.DocumentContent = this.DocumentContent;
            }
        }

        private void UpdateFormXmlContent()
        {
            foreach (string fName in this.Page.Request.Form.AllKeys)
            {
                if (string.IsNullOrEmpty(fName) ||
                    !fName.StartsWith(this.Form.FieldPrefix)) continue;

                string fValue = this.Page.Request.Form[fName];

                if (!string.IsNullOrEmpty(fValue))
                {
                    foreach (Form.Field f in this.Form.Fields)
                    {
                        if (fName.Equals(this.Form.FieldPrefix + f.Name))
                        {
                            f.Value = Convert.ChangeType(fValue, f.Type);
                            break;
                        }
                    }
                }
            }
            this.Form.UpdateXmlNodes();
            this.DocumentContent = this.Form.OwnerDocumentOuterXml;
        }
    }
}
