using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Globalization;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// 配置控件状态的集合类。
    /// </summary>
    public sealed class ControlStateSettingCollection : List<ControlStateSetting>
    {
    }

    /// <summary>
    /// 控件状态，一般是控件中 bool 类型的属性。
    /// </summary>
    public enum ControlState
    {
        /// <summary>
        /// 使能状态。
        /// </summary>
        Enabled,
        /// <summary>
        /// 可见状态。
        /// </summary>
        Visible,
    }

    /// <summary>
    /// 控件状态为真的条件。
    /// </summary>
    public enum ControlStateForTrueCriteria
    {
        /// <summary>
        /// 当表格中只有一行被选中。
        /// </summary>
        OnlyOneRowSelected,
        /// <summary>
        /// 当表格中至少有一行被选中。
        /// </summary>
        AtLeastOneRowSelected,
        /// <summary>
        /// 当有访问权限。
        /// </summary>
        HasPermission,
    }

    /// <summary>
    /// 控件状态设置项。
    /// </summary>
    [TypeConverter(typeof(ControlStateSettingConverter))]
    [ControlBuilder(typeof(ControlStateSettingBuilder))]
    public class ControlStateSetting : IStateManager
    {
        #region Properties

        private string _controlId = string.Empty;
        /// <summary>
        /// 需要控制状态的控件ID。
        /// </summary>
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [Category("行为")]
        [DefaultValue("")]
        [Description("需要控制状态的控件ID。")]
        [NotifyParentProperty(true)]
        public string ControlID
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private ControlState _controlState = ControlState.Enabled;
        /// <summary>
        /// 需要控制的控件状态。
        /// </summary>
        [Category("行为")]
        [DefaultValue(ControlState.Enabled)]
        [Description("需要控制的控件状态。")]
        [NotifyParentProperty(true)]
        public ControlState ControlState
        {
            get { return _controlState; }
            set { _controlState = value; }
        }

        private ControlStateForTrueCriteria _controlStateForTrueCriteria = ControlStateForTrueCriteria.AtLeastOneRowSelected;
        /// <summary>
        /// 控件状态为真的条件。
        /// </summary>
        [Category("行为")]
        [DefaultValue(ControlStateForTrueCriteria.AtLeastOneRowSelected)]
        [Description("控件状态为真的条件。")]
        [NotifyParentProperty(true)]
        public ControlStateForTrueCriteria ControlStateForTrueCriteria
        {
            get { return _controlStateForTrueCriteria; }
            set { _controlStateForTrueCriteria = value; }
        }

        private StateBag _viewState;
        private StateBag ViewState
        {
            get
            {
                if (_viewState == null)
                {
                    _viewState = new StateBag(false);
                    if (_isTrackingViewState)
                    {
                        ((IStateManager)_viewState).TrackViewState();
                    }
                }
                return _viewState;
            }
        }
        #endregion

        #region Operates

        internal void SetDirty()
        {
            ViewState.SetDirty(true);
        }

        #endregion

        #region IStateManager 成员

        private bool _isTrackingViewState;
        bool IStateManager.IsTrackingViewState
        {
            get { return _isTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            ArrayList list = (ArrayList)state;
            if (list.Count != 1)
            {
                throw new ArgumentException("Invalid ControlStateSettings View State");
            }
            ((IStateManager)ViewState).LoadViewState(list[0]);
        }

        object IStateManager.SaveViewState()
        {
            ArrayList list = new ArrayList();

            if (_viewState != null)
            {
                list.Add(((IStateManager)_viewState).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            return list;
        }

        void IStateManager.TrackViewState()
        {
            _isTrackingViewState = true;
            if (_viewState != null)
            {
                ((IStateManager)_viewState).TrackViewState();
            }
        }

        #endregion
    }

    /// <summary>
    /// 控件状态集合编辑器。
    /// </summary>
    public class ControlStateSettingCollectionEditor : CollectionEditor
    {
        public ControlStateSettingCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(ControlStateSetting);
        }
    }

    /// <summary>
    /// 控件状态构造器。
    /// </summary>
    public class ControlStateSettingBuilder : ControlBuilder
    {
        public ControlStateSettingBuilder()
        {
        }
        public override bool AllowWhitespaceLiterals()
        {
            return false;//除去控件标签对嵌套内容首尾空白
        }

        public override bool HtmlDecodeLiterals()
        {
            return true;//删除HTML编码
        }
    }

    /// <summary>
    /// 控件状态转换器。
    /// </summary>
    public class ControlStateSettingConverter : ExpandableObjectConverter
    {
        #region 方法

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
            object value)
        {
            if (value == null)
            {
                return new ControlStateSetting();
            }
            if (value is string)
            {
                string s = (string)value;
                if (s.Length == 0)
                {
                    return new ControlStateSetting();
                }
                return "ControlStateSetting";

            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is ControlStateSetting))
                {
                    throw new ArgumentException(
                        "Invalid ControlStateSetting", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return String.Empty;
                }
                return "ControlStateSetting";
            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
        #endregion
    }
}
