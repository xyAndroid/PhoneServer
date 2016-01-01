using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /*
注意：首先要配置DatePicker文件夹的路径，默认是在网站根目录，也可以自定义
		例如将DatePicker文件夹放在网站根目录下的Common文件夹下，请将控件的ResourcePath属性设置为Common/
		
	如果要把DatePicker嵌套在数据呈现控件(GridView或DetailsView)时，需要手动绑定Date属性
		<cc1:DatePicker ID="DatePicker1" runat="server" Date='<%# Bind("startTime") %>'/>
		
	如果要把DatePicker放入UpdatePanel中，请在ScriptManager中注册JS文件,例如(注意WdatePicker.js文件路径)
	<asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="Common/DatePicker/WdatePicker.js" />
        </Scripts>
    </asp:ScriptManager>

属性说明：

custom属性设置(多个custom属性用,隔开)：
	使用positon属性指定,弹出日期的坐标为{left:100,top:50}   
		position:{left:100,top:50} 
	多语言支持   
		繁体中文 lang:'zh-tw'  英文 lang:'en'  简体中文 lang:'zh-cn'
	确认清空   
		onclearing:function(){if(!confirm('日期框的值为:'+this.value+', 确实要清空吗?'))return true;}
		oncleared:function(dp){alert('当前日期所选择的月份为:'+$dp.cal.date.M);}
	确认覆盖   
		onpicking:function(dp){if(!confirm('日期框原来的值为: '+dp.cal.getDateStr()+', 要覆盖吗?')) return true;}
	
日期范围限制：
	限制日期的范围是 2006-09-10到2008-12-20   
		minDate:2006-09-10  maxDate:2008-12-20
	限制日期的范围是 2008-3-8 11:30:00 到 2008-3-10 20:59:30   
		dateFmt:yyyy-MM-dd HH:mm:ss  minDate:2008-03-08 11:30:00  maxDate:2008-03-10 20:59:30
	限制日期的范围是 2008年2月 到 2008年10月   
		dateFmt:yyyy年M月  minDate:2008-2  maxDate:2008-10
	限制日期的范围是 8:00:00 到 11:30:00   
		dateFmt:H:mm:ss  minDate:8:00:00  maxDate:11:30:00
	只能选择今天以前的日期(包括今天)   
		maxDate:%y-%M-%d
	只能选择今天以后的日期(不包括今天)   
		minDate:%y-%M-#{%d+1}
	只能选择本月的日期1号至本月最后一天   
		minDate:%y-%M-01  maxDate:%y-%M-%ld
	只能选择今天7:00:00至明天21:00:00的日期   
		dateFmt:yyyy-M-d H:mm:ss  minDate:%y-%M-%d 7:00:00  maxDate:%y-%M-#{%d+1} 21:00:00
	只能选择 20小时前 至 30小时后 的日期   
		dateFmt:yyyy-MM-dd HH:mm  minDate:%y-%M-%d #{%H-20}:%m:%s  maxDate:%y-%M-%d #{%H+30}:%m:%s
	
无效天和无效日期功能：
	同时禁用 周六和周日 所对应的日期   
		disabledDays:[0,6]
	无效日期    如果你熟悉正则表达式,会很容易理解下面的匹配用法
				如果不熟悉,可以参考下面的常用示例 
				['2008-02-01','2008-02-29'] 表示禁用 2008-02-01 和 2008-02-29
				['2008-..-01','2008-02-29'] 表示禁用 2008-所有月份-01 和 2008-02-29
				['200[0-8]]-02-01','2008-02-29'] 表示禁用 [2000至2008]-02-01 和 2008-02-29
				['^2006'] 表示禁用 2006年的所有日期 

				此外,您还可以使用 %y %M %d %H %m %s 等变量, 用法同动态日期限制 注意:%ld不能使用
				['....-..-01','%y-%M-%d'] 表示禁用 所有年份和所有月份的第一天和今天 
				['%y-%M-#{%d-1}','%y-%M-#{%d+1}'] 表示禁用 昨天和明天

				当然,除了可以限制日期以外,您还可以限制时间
				['....-..-.. 10\:00\:00'] 表示禁用 每天10点 (注意 : 需要 使用 \: )
	配合min/maxDate使用,可以把可选择的日期分隔成多段 本示例本月可用日期分隔成五段 分别是: 1-3 8-10 16-24 26,27 29-月末
		minDate:%y-%M-01  maxDate:%y-%M-%ld  disabledDates:['0[4-7]$','1[1-5]$','2[58]$']
	min/maxDate disabledDays disabledDates 三者配合使用 即使在要求非常苛刻的情况下也能满足需求
		minDate:%y-%M-01  maxDate:%y-%M-%ld  disabledDates:['0[4-7]$','1[1-5]$','2[58]$']  disabledDays:[1,3,6]
	禁用前一个小时和后一个小时内所有时间 使用 %y %M %d %H %m %s 等变量
		dateFmt:yyyy-MM-dd HH:mm:ss  disabledDates:['%y-%M-%d #{%H-1}\:..\:..','%y-%M-%d #{%H+1}\:..\:..']
		鼠标点击 小时输入框时,你会发现当然时间对应的前一个小时和后一个小时是灰色的
     */

    public enum DatePickerSkin
    {
        Blue = 1, Default = 2, Simple = 3, WhyGreen = 4, Ext = 5, YcloudRed = 6,
    }


    [DefaultProperty("ResourcePath")]
    [DefaultEvent("TextChanged")]
    [System.Drawing.ToolboxBitmap(typeof(DatePicker), "DatePicker.bmp")]
    [ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
    public class DatePicker : CompositeControl
    {
        private TextBox _txtDate;

        #region properties

        [Browsable(true)]
        [Category("扩展属性")]
        [Description("是否执行验证。")]
        [Localizable(true)]
        [DefaultValue(false)]
        public bool CausesValidation
        {
            get
            {
                EnsureChildControls();
                return _txtDate.CausesValidation;
            }
            set
            {
                EnsureChildControls();
                _txtDate.CausesValidation = Convert.ToBoolean(value);
            }
        }

        private string _resourcePath = "";
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置DatePicker文件夹路径。(默认在网站根目录下)")]
        [Localizable(true)]
        public string ResourcePath
        {
            get
            {
                EnsureChildControls();
                return _resourcePath;
            }
            set
            {
                EnsureChildControls();
                if (value == null)
                {
                    _resourcePath = "";
                }
                else if (!value.EndsWith("/"))
                {
                    _resourcePath = value.Trim() + "/";
                }
                else
                {
                    _resourcePath = value.Trim();
                }
            }
        }

        private string _startDate = "%y-%M-%d 00:00:00";
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得开始日期。")]
        [Localizable(true)]
        [DefaultValue("%y-%M-%d 00:00:00")]
        public string StartDate
        {
            get
            {
                EnsureChildControls();
                return _startDate;
            }
            set
            {
                EnsureChildControls();
                _startDate = value as string;
            }
        }

        private bool _alwaysUseStartDate = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("是否启用开始日期。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool AlwaysUseStartDate
        {
            get
            {
                EnsureChildControls();
                return _alwaysUseStartDate;
            }
            set
            {
                EnsureChildControls();
                _alwaysUseStartDate = Convert.ToBoolean(value);
            }
        }

        private string _dateFmt = "yyyy-MM-dd HH:mm:ss";
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得日期格式。")]
        [Localizable(true)]
        [DefaultValue("yyyy-MM-dd HH:mm:ss")]
        public string DateFmt
        {
            get
            {
                EnsureChildControls();
                return _dateFmt;
            }
            set
            {
                EnsureChildControls();
                _dateFmt = value as string;
            }
        }

        private bool _isShowWeek = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("启用支持周显示。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool IsShowWeek
        {
            get
            {
                EnsureChildControls();
                return _isShowWeek;
            }
            set
            {
                EnsureChildControls();
                _isShowWeek = Convert.ToBoolean(value);
            }
        }

        //private string _skin = "whyGreen";
        //[Browsable(true)]
        //[Category("扩展属性")]
        //[Description("设置或获得弹出日期的皮肤。")]
        //[Localizable(true)]
        //[DefaultValue("whyGreen")]
        //public string Skin
        //{
        //    get
        //    {
        //        EnsureChildControls();
        //        return _skin;
        //    }
        //    set
        //    {
        //        EnsureChildControls();
        //        _skin = value as string;
        //    }
        //}

        private string _skin = "default";
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得弹出日期的皮肤。")]
        [Localizable(true)]
        [DefaultValue("whyGreen")]
        public DatePickerSkin Skin
        {
            get
            {
                EnsureChildControls();
                switch (_skin)
                {
                    case "default": { return DatePickerSkin.Default; }
                    case "blue": { return DatePickerSkin.Blue; }
                    case "ext": { return DatePickerSkin.Ext; }
                    case "simple": { return DatePickerSkin.Simple; }
                    case "whyGreen": { return DatePickerSkin.WhyGreen; }
                    case "YcloudRed": { return DatePickerSkin.YcloudRed; }
                    default: { return DatePickerSkin.Default; }
                }
            }
            set
            {
                EnsureChildControls();
                switch ((DatePickerSkin)value)
                {
                    case DatePickerSkin.Default: { _skin = "default"; break; }
                    case DatePickerSkin.Blue: { _skin = "blue"; break; }
                    case DatePickerSkin.Ext: { _skin = "ext"; break; }
                    case DatePickerSkin.Simple: { _skin = "simple"; break; }
                    case DatePickerSkin.WhyGreen: { _skin = "whyGreen"; break; }
                    case DatePickerSkin.YcloudRed: { _skin = "YcloudRed"; break; }
                }
            }
        }

        private bool _isShowOthers = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("是否显示其他月的日期。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool IsShowOthers
        {
            get
            {
                EnsureChildControls();
                return _isShowOthers;
            }
            set
            {
                EnsureChildControls();
                _isShowOthers = Convert.ToBoolean(value);
            }
        }

        private bool _readOnly = false;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("指定日期框是否只读。")]
        [Localizable(true)]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                EnsureChildControls();
                return _readOnly;
            }
            set
            {
                EnsureChildControls();
                _readOnly = Convert.ToBoolean(value);
            }
        }

        private bool _highLineWeekDay = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("指定是否高亮周末。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool IsHighLineWeekDay
        {
            get
            {
                EnsureChildControls();
                return _highLineWeekDay;
            }
            set
            {
                EnsureChildControls();
                _highLineWeekDay = Convert.ToBoolean(value);
            }
        }

        private bool _isShowClear = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("指定是否显示清空按钮。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool IsShowClear
        {
            get
            {
                EnsureChildControls();
                return _isShowClear;
            }
            set
            {
                EnsureChildControls();
                _isShowClear = Convert.ToBoolean(value);
            }
        }

        private bool _isShowToday = true;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("指定是否显示今天按钮。")]
        [Localizable(true)]
        [DefaultValue(true)]
        public bool IsShowToday
        {
            get
            {
                EnsureChildControls();
                return _isShowToday;
            }
            set
            {
                EnsureChildControls();
                _isShowToday = Convert.ToBoolean(value);
            }
        }

        private DateTime _maxDate = DateTime.MinValue;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得最大日期。")]
        [Localizable(true)]
        [DefaultValue("")]
        public DateTime MaxDate
        {
            get
            {
                EnsureChildControls();
                return _maxDate;
            }
            set
            {
                EnsureChildControls();
                _maxDate = Convert.ToDateTime(value);
            }
        }

        private DateTime _minDate = DateTime.MinValue;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得最小日期。")]
        [Localizable(true)]
        [DefaultValue("")]
        public DateTime MinDate
        {
            get
            {
                EnsureChildControls();
                return _minDate;
            }
            set
            {
                EnsureChildControls();
                _minDate = Convert.ToDateTime(value);
            }
        }

        private string _disabledDays = string.Empty;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得无效星期。")]
        [Localizable(true)]
        [DefaultValue("")]
        public string DisabledDays
        {
            get
            {
                EnsureChildControls();
                return _disabledDays;
            }
            set
            {
                EnsureChildControls();
                _disabledDays = value.ToString();
            }
        }

        private string _disabledDates = string.Empty;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得无效日期。")]
        [Localizable(true)]
        [DefaultValue("")]
        public string DisabledDates
        {
            get
            {
                EnsureChildControls();
                return _disabledDates;
            }
            set
            {
                EnsureChildControls();
                _disabledDates = value.ToString();
            }
        }

        private string _custom = string.Empty;
        [Browsable(true)]
        [Category("扩展属性")]
        [Description("设置或获得自定义规则。")]
        [Localizable(true)]
        [DefaultValue("")]
        public string Custom
        {
            get
            {
                EnsureChildControls();
                return _custom;
            }
            set
            {
                EnsureChildControls();
                _custom = value.ToString();
            }
        }

        [Browsable(true)]
        [Category("扩展属性")]
        [Description("在文本修改之后，自动回发到服务器。")]
        [Localizable(true)]
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                EnsureChildControls();
                return _txtDate.AutoPostBack;
            }
            set
            {
                EnsureChildControls();
                _txtDate.AutoPostBack = Convert.ToBoolean(value);
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        [Category("扩展属性")]
        [Description("设置或获得显示日期。")]
        [Localizable(true)]
        [DefaultValue("")]
        public DateTime Date
        {
            get
            {
                EnsureChildControls();
                if (string.IsNullOrEmpty(this._txtDate.Text))
                {
                    return DateTime.Now;
                }
                else
                {
                    return Convert.ToDateTime(this._txtDate.Text);
                }
            }
            set
            {
                EnsureChildControls();
                this._txtDate.Text = value.ToString().Trim();
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        [Category("扩展属性")]
        [Description("设置或获得显示文本。")]
        [Localizable(true)]
        [DefaultValue("")]
        public string Text
        {
            get
            {
                EnsureChildControls();
                return this._txtDate.Text;
            }
            set
            {
                EnsureChildControls();
                this._txtDate.Text = value.ToString();
            }
        }

        #endregion

        #region Events

        private static readonly object EventTextChangedKey = new object();

        private void _text_changed(object sender, EventArgs e)
        {
            OnTextChanged(EventArgs.Empty);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler textChangedHandler = (EventHandler)Events[EventTextChangedKey];
            if (textChangedHandler != null)
            {
                textChangedHandler(this, e);
            }
        }

        [Browsable(true)]
        [Category("扩展事件")]
        [Description("在更改文本属性后激发。")]
        public event EventHandler TextChanged
        {
            add
            {
                Events.AddHandler(EventTextChangedKey, value);
            }
            remove
            {
                Events.RemoveHandler(EventTextChangedKey, value);
            }
        }

        #endregion

        #region method

        private string getProperties()
        {
            StringBuilder property = new StringBuilder("startDate:'" + _startDate + "'");
            property.Append(",alwaysUseStartDate:" + _alwaysUseStartDate.ToString().ToLower());
            property.Append(",dateFmt:'" + _dateFmt + "'");
            property.Append(",isShowWeek:" + _isShowWeek.ToString().ToLower());
            property.Append(",skin:'" + _skin + "'");
            property.Append(",isShowOthers:" + _isShowOthers.ToString().ToLower());
            property.Append(",readOnly:" + _readOnly.ToString().ToLower());
            property.Append(",highLineWeekDay:" + _highLineWeekDay.ToString().ToLower());
            property.Append(",isShowClear:" + _isShowClear.ToString().ToLower());
            property.Append(",isShowToday:" + _isShowToday.ToString().ToLower());

            if (_maxDate != DateTime.MinValue)
            {
                property.Append(",maxDate:'" + _maxDate.ToString() + "'");
            }
            if (_minDate != DateTime.MinValue)
            {
                property.Append(",minDate:'" + _minDate.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(_disabledDays))
            {
                property.Append(",disabledDays:" + _disabledDays);
            }
            if (!string.IsNullOrEmpty(_disabledDates))
            {
                property.Append(",disabledDates:" + _disabledDates);
            }
            if (!string.IsNullOrEmpty(_custom))
            {
                property.Append("," + _custom);
            }

            return property.ToString();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            _txtDate = new TextBox();
            _txtDate.Width = base.Width;
            _txtDate.ID = "txtDate";
            _txtDate.CausesValidation = false;
            _txtDate.TextChanged += new EventHandler(_text_changed);

            this.Controls.Add(_txtDate);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<script type=\"text/javascript\"src=\"" +
               _resourcePath + "DatePicker/WdatePicker.js\"></script>");

            _txtDate.Attributes["class"] = "Wdate";
            _txtDate.Attributes["onFocus"] = "WdatePicker({" + getProperties() + "})";

            this.AddAttributesToRender(writer);
            _txtDate.RenderControl(writer);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        #endregion
    }
}
