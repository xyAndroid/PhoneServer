using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// �ϳ� SiteMapProvider �ķ�ʽ��
    /// </summary>
    public enum ProduceSiteMapProviderManner
    {
        /// <summary>
        /// �������κκϳɡ�
        /// </summary>
        None,
        /// <summary>
        /// SiteMapProviderǰ׺ + SiteMapProvider + Roleǰ׺ + Role��
        /// </summary>
        PrefixAddSiteMapProviderANDPrefixAddRole,
        /// <summary>
        /// SiteMapProvider + Roleǰ׺ + Role��
        /// </summary>
        SiteMapProviderANDPrefixAddRole,
        /// <summary>
        /// SiteMapProviderǰ׺ + SiteMapProvider + Role��
        /// </summary>
        PrefixAddSiteMapProviderANDRole,
        /// <summary>
        /// SiteMapProvider + Role��
        /// </summary>
        SiteMapProviderANDRole,
    }

    /// <summary>
    /// ��ǩ�ؼ�����Ҫ����ҳ��֮���л���
    /// </summary>
    [ToolboxData("<{0}:Tab runat=server></{0}:Tab>")]
    public class Tab : CompositeControl
    {
        #region Fields

        Menu _mnuTabs;
        SiteMapDataSource _smDsCtrl;

        Style _selectedTabItemStyle;
        Style _tabItemStyle;
        Style _tabStyle;

        #endregion
        #region Properties

        /// <summary>
        /// ѡ�б�ǩ�����ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("ѡ�б�ǩ�����ʽ��")]
        public virtual Style SelectedTabItemStyle
        {
            get
            {
                if (_selectedTabItemStyle == null)
                {
                    _selectedTabItemStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_selectedTabItemStyle).TrackViewState();
                    }
                }
                return _selectedTabItemStyle;
            }
        }

        /// <summary>
        /// ��ǩ�����ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("��ǩ�����ʽ��")]
        public virtual Style TabItemStyle
        {
            get
            {
                if (_tabItemStyle == null)
                {
                    _tabItemStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_tabItemStyle).TrackViewState();
                    }
                }
                return _tabItemStyle;
            }
        }

        /// <summary>
        /// ��ǩ����ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��ʽ")]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("��ǩ����ʽ��")]
        public virtual Style TabStyle
        {
            get
            {
                if (_tabStyle == null)
                {
                    _tabStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_tabStyle).TrackViewState();
                    }
                }
                return _tabStyle;
            }
        }

        /// <summary>
        /// ��ǩ�������Դ�ṩ�ߡ�
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [DefaultValue("")]
        [Description("��ǩ�������Դ�ṩ�ߡ�")]
        public string SiteMapProvider
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "SiteMapProvider", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "SiteMapProvider", value);
            }
        }

        /// <summary>
        /// ��ǩ�������Դ�ṩ������ǰ׺��ProduceSiteMapProviderManner ��Ϊ None ʱ���������������ǰ׺�ϳ� SiteMapProvider��
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("��ǩ�������Դ�ṩ������ǰ׺��ProduceSiteMapProviderManner ��Ϊ None ʱ���������������ǰ׺�ϳ� SiteMapProvider��")]
        public string SiteMapProviderPrefix
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "SiteMapProviderPrefix", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "SiteMapProviderPrefix", value);
            }
        }

        /// <summary>
        /// ��ɫ��ǰ׺���� ProduceSiteMapProviderManner ��Ϊ None ʱ���������������ǰ׺�ϳ� SiteMapProvider��
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue("")]
        [Description("��ɫ��ǰ׺���� ProduceSiteMapProviderManner ��Ϊ None ʱ���������������ǰ׺�ϳ� SiteMapProvider��")]
        public string RolePrefix
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "RolePrefix", string.Empty);
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "RolePrefix", value);
            }
        }

        /// <summary>
        /// SiteMapProvider�ϳɷ�ʽ��
        /// </summary>
        [Browsable(true)]
        [Category("��Ϊ")]
        [DefaultValue(ProduceSiteMapProviderManner.None)]
        [Description("SiteMapProvider�ϳɷ�ʽ��")]
        public ProduceSiteMapProviderManner ProduceSiteMapProviderManner
        {
            get
            {
                return Utility.GetViewStatePropertyValue<ProduceSiteMapProviderManner>(this.ViewState,
                    "ProduceSiteMapProviderManner", ProduceSiteMapProviderManner.None);
            }
            set
            {
                Utility.SetViewStatePropertyValue<ProduceSiteMapProviderManner>(this.ViewState,
                    "ProduceSiteMapProviderManner", value);
            }
        }

        #endregion

        #region Operates

        private void ConstructTabControls()
        {
            _mnuTabs = new Menu();
            _mnuTabs.ID = "tabMenu";
            _mnuTabs.DataSourceID = "siteMapDsMenu";
            _mnuTabs.Orientation = Orientation.Horizontal;
            _mnuTabs.StaticEnableDefaultPopOutImage = false;

            this.Controls.Add(_mnuTabs);
        }

        private void ConstructSiteMapDataSourceControl()
        {
            _smDsCtrl = new SiteMapDataSource();
            _smDsCtrl.ID = "siteMapDsMenu";
            _smDsCtrl.ShowStartingNode = false;

            if (!this.DesignMode)
            {
                string role = Utility.GetCurrentLoginRole();
                role = (role == null ? string.Empty : role);

                string nonPrefixRole = (string.IsNullOrEmpty(this.RolePrefix) ? role : role.Replace(this.RolePrefix, string.Empty));
                string nonPrefixSiteMapProvider = (string.IsNullOrEmpty(this.SiteMapProviderPrefix) ? this.SiteMapProvider : this.SiteMapProvider.Replace(this.SiteMapProviderPrefix, string.Empty));

                switch (this.ProduceSiteMapProviderManner)
                {
                    case ProduceSiteMapProviderManner.PrefixAddSiteMapProviderANDPrefixAddRole:
                        this.SiteMapProvider =
                            string.Format("{0}{1}{2}{3}", this.SiteMapProviderPrefix, this.SiteMapProvider, this.RolePrefix, nonPrefixRole);
                        break;
                    case ProduceSiteMapProviderManner.PrefixAddSiteMapProviderANDRole:
                        this.SiteMapProvider =
                            string.Format("{0}{1}{2}", this.SiteMapProviderPrefix, this.SiteMapProvider, nonPrefixRole);
                        break;
                    case ProduceSiteMapProviderManner.SiteMapProviderANDPrefixAddRole:
                        this.SiteMapProvider =
                            string.Format("{0}{1}{2}", nonPrefixSiteMapProvider, this.RolePrefix, nonPrefixRole);
                        break;
                    case ProduceSiteMapProviderManner.SiteMapProviderANDRole:
                        this.SiteMapProvider =
                            string.Format("{0}{1}", nonPrefixSiteMapProvider, nonPrefixRole);
                        break;
                    default:
                        break;
                }
            }
            _smDsCtrl.SiteMapProvider = this.SiteMapProvider;

            this.Controls.Add(_smDsCtrl);
        }

        private void ApplyStyle()
        {
            if (_selectedTabItemStyle != null)
            {
                _mnuTabs.StaticSelectedStyle.CopyFrom(_selectedTabItemStyle);

                _mnuTabs.Font.Bold = false;
                _mnuTabs.Font.Underline = true;
            }
            if (_tabItemStyle != null)
            {
                _mnuTabs.StaticMenuItemStyle.CopyFrom(_tabItemStyle);
            }
            if (_tabStyle != null)
            {
                _mnuTabs.StaticMenuStyle.CopyFrom(_tabStyle);
            }
        }

        #endregion

        protected override object SaveViewState()
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();

            list.Add(base.SaveViewState());

            if (_selectedTabItemStyle != null)
            {
                list.Add(
                    ((IStateManager)_selectedTabItemStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_tabItemStyle != null)
            {
                list.Add(
                    ((IStateManager)_tabItemStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            if (_tabItemStyle != null)
            {
                list.Add(
                    ((IStateManager)_tabItemStyle).SaveViewState());
            }
            else
            {
                list.Add(null);
            }
            return list;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState is System.Collections.ArrayList)
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)savedState;

                if (list.Count >= 4)
                {
                    base.LoadViewState(list[0]);

                    if (list[1] != null)
                    {
                        ((IStateManager)SelectedTabItemStyle).LoadViewState(list[1]);
                    }
                    if (list[2] != null)
                    {
                        ((IStateManager)TabItemStyle).LoadViewState(list[2]);
                    }
                    if (list[3] != null)
                    {
                        ((IStateManager)TabStyle).LoadViewState(list[3]);
                    }
                }
            }
            else
            {
                base.LoadViewState(savedState);
            }
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_selectedTabItemStyle != null)
            {
                ((IStateManager)_selectedTabItemStyle).TrackViewState();
            }
            if (_tabItemStyle != null)
            {
                ((IStateManager)_tabItemStyle).TrackViewState();
            }
            if (_tabStyle != null)
            {
                ((IStateManager)_tabStyle).TrackViewState();
            }
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.ConstructSiteMapDataSourceControl();
            this.ConstructTabControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                this.ApplyStyle();

                _mnuTabs.RenderControl(writer);
            }
            else
            {
                Utility.RenderUnUIControlDesign(this, writer);
            }
        }
    }
}
