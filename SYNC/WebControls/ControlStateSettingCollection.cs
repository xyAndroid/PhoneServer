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
    /// ���ÿؼ�״̬�ļ����ࡣ
    /// </summary>
    public sealed class ControlStateSettingCollection : List<ControlStateSetting>
    {
    }

    /// <summary>
    /// �ؼ�״̬��һ���ǿؼ��� bool ���͵����ԡ�
    /// </summary>
    public enum ControlState
    {
        /// <summary>
        /// ʹ��״̬��
        /// </summary>
        Enabled,
        /// <summary>
        /// �ɼ�״̬��
        /// </summary>
        Visible,
    }

    /// <summary>
    /// �ؼ�״̬Ϊ���������
    /// </summary>
    public enum ControlStateForTrueCriteria
    {
        /// <summary>
        /// �������ֻ��һ�б�ѡ�С�
        /// </summary>
        OnlyOneRowSelected,
        /// <summary>
        /// �������������һ�б�ѡ�С�
        /// </summary>
        AtLeastOneRowSelected,
        /// <summary>
        /// ���з���Ȩ�ޡ�
        /// </summary>
        HasPermission,
    }

    /// <summary>
    /// �ؼ�״̬�����
    /// </summary>
    [TypeConverter(typeof(ControlStateSettingConverter))]
    [ControlBuilder(typeof(ControlStateSettingBuilder))]
    public class ControlStateSetting : IStateManager
    {
        #region Properties

        private string _controlId = string.Empty;
        /// <summary>
        /// ��Ҫ����״̬�Ŀؼ�ID��
        /// </summary>
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("��Ҫ����״̬�Ŀؼ�ID��")]
        [NotifyParentProperty(true)]
        public string ControlID
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private ControlState _controlState = ControlState.Enabled;
        /// <summary>
        /// ��Ҫ���ƵĿؼ�״̬��
        /// </summary>
        [Category("��Ϊ")]
        [DefaultValue(ControlState.Enabled)]
        [Description("��Ҫ���ƵĿؼ�״̬��")]
        [NotifyParentProperty(true)]
        public ControlState ControlState
        {
            get { return _controlState; }
            set { _controlState = value; }
        }

        private ControlStateForTrueCriteria _controlStateForTrueCriteria = ControlStateForTrueCriteria.AtLeastOneRowSelected;
        /// <summary>
        /// �ؼ�״̬Ϊ���������
        /// </summary>
        [Category("��Ϊ")]
        [DefaultValue(ControlStateForTrueCriteria.AtLeastOneRowSelected)]
        [Description("�ؼ�״̬Ϊ���������")]
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
    /// �ؼ�״̬���ϱ༭����
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
    /// �ؼ�״̬��������
    /// </summary>
    public class ControlStateSettingBuilder : ControlBuilder
    {
        public ControlStateSettingBuilder()
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

    /// <summary>
    /// �ؼ�״̬ת������
    /// </summary>
    public class ControlStateSettingConverter : ExpandableObjectConverter
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
