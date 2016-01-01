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
    /// ��������ӳ��ļ����࣬���� Filtering �ؼ��� PropertyMappings ���ԡ�
    /// </summary>
    public sealed class PropertyMappingCollection : ArrayList
    {
        public PropertyMapping FindByPropertyName(string propertyName)
        {
            foreach (object obj in this)
            {
                if (obj is PropertyMapping)
                {
                    PropertyMapping propMap = (PropertyMapping)obj;
                    if (propMap.PropertyName.Equals(propertyName))
                    {
                        return propMap;
                    }
                }
            }
            return null;
        }

        public PropertyMapping FindByFilterItem(string filterItem)
        {
            foreach (object obj in this)
            {
                if (obj is PropertyMapping)
                {
                    PropertyMapping propMap = (PropertyMapping)obj;
                    if (propMap.FilterItem.Equals(filterItem))
                    {
                        return propMap;
                    }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// ��������ӳ�������ࡣ
    /// </summary>
    [TypeConverter(typeof(PropertyMappingSettingConverter))]
    public sealed class PropertyMappingSettings : IStateManager
    {
        #region Properties
        
        private string _refPropertyName = string.Empty;
        /// <summary>
        /// ���������ã����ڲ�������ӳ�䡣
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("���������ã����ڲ�������ӳ�䡣")]
        [NotifyParentProperty(true)]
        public string RefPropertyName
        {
            get { return _refPropertyName; }
            set { _refPropertyName = value; }
        }

        private string _refPropertyValue = string.Empty;
        /// <summary>
        /// ����ֵ���ã������ṩ���롣
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("����ֵ���ã������ṩ���롣")]
        [NotifyParentProperty(true)]
        public string RefPropertyValue
        {
            get { return _refPropertyValue; }
            set { _refPropertyValue = value; }
        }

        private string _refPropertyMeaning = string.Empty;
        /// <summary>
        /// ���Ժ������ã������ṩ�Ѻ��ı���
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("���Ժ������ã������ṩ�Ѻ��ı�")]
        [NotifyParentProperty(true)]
        public string RefPropertyMeaning
        {
            get { return _refPropertyMeaning; }
            set { _refPropertyMeaning = value; }
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

        #region IStateManager ��Ա

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
                throw new ArgumentException("Invalid PropertyMappingSetting View State");
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
    /// ����ӳ�䡣
    /// </summary>
    [TypeConverter(typeof(PropertyMappingConverter))]
    [ControlBuilder(typeof(PropertyMappingControlBuilder))]
    public class PropertyMapping : IStateManager
    {
        #region Properties

        private string _propertyName = string.Empty;
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("������")]
        [NotifyParentProperty(true)]
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        private string _filterItem = string.Empty;
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("��Ӧ��ɸѡ��")]
        [NotifyParentProperty(true)]
        public string FilterItem
        {
            get { return _filterItem; }
            set { _filterItem = value; }
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

        #region IStateManager ��Ա

        private bool _isTrackingViewState;
        bool IStateManager.IsTrackingViewState
        {
            get { return _isTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            ArrayList list = (ArrayList)state;
            if(list.Count != 1)
            {
                throw new ArgumentException("Invalid PropertyMapping View State");
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
            if (_viewState != null) {
                ((IStateManager)_viewState).TrackViewState();
            }
        }

        #endregion
    }

    /// <summary>
    /// ����ӳ��༭����
    /// </summary>
    public class PropertyMappingCollectionEditor : CollectionEditor
    {
        public PropertyMappingCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(PropertyMapping);
        }
    }

    /// <summary>
    /// ����ӳ������ת������
    /// </summary>
    public class PropertyMappingConverter : ExpandableObjectConverter
    {
        #region ����
        
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
                return new PropertyMapping();
            }
            if (value is string)
            {
                string s = (string)value;
                if (s.Length == 0)
                {
                    return new PropertyMapping();
                }
                return "PropertyMapping";

            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is PropertyMapping))
                {
                    throw new ArgumentException(
                        "Invalid PropertyMapping", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return String.Empty;
                }
                return "PropertyMapping";
            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
        #endregion
    }

    /// <summary>
    /// ��������ת������
    /// </summary>
    public class PropertyMappingSettingConverter : ExpandableObjectConverter
    {
        #region ����

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
                return new PropertyMappingSettings();
            }
            if (value is string)
            {
                string s = (string)value;
                if (s.Length == 0)
                {
                    return new PropertyMappingSettings();
                }
                return "PropertyMappingSetting";

            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is PropertyMapping))
                {
                    throw new ArgumentException(
                        "Invalid PropertyMapping", "value");
                }
            }

            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return String.Empty;
                }
                return "PropertyMappingSetting";
            }
            return base.ConvertTo(context, culture, value,
                destinationType);
        }
        #endregion
    }

    /// <summary>
    /// �ؼ���������
    /// </summary>
    public class PropertyMappingControlBuilder : ControlBuilder
    {
        public PropertyMappingControlBuilder()
        {
        }
        public override bool AllowWhitespaceLiterals()
        {
            return false;//��ȥ�ؼ���ǩ��Ƕ��������β�հ�
        }

        public override bool HtmlDecodeLiterals()
        {
            return true;//ɾ��HTML����
        }
    }
}
