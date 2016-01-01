using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eSchool.Web.UI.WebControls
{
    /// <summary>
    /// ����/ͣ�ñ�ǡ�
    /// </summary>
    public enum UsedFlag
    {
        /// <summary>
        /// ���á�
        /// </summary>
        BeingUsed = 1,
        /// <summary>
        /// ͣ�á�
        /// </summary>
        Stopped = 0,
    }

    /// <summary>
    /// �ṩ��������/ͣ�õĿؼ���
    /// </summary>
    [DefaultProperty("Text")]
    [DefaultEvent("Command")]
    [RefreshProperties(RefreshProperties.Repaint)]
    [ToolboxData("<{0}:SetUsedFlag runat=server></{0}:SetUsedFlag>")]
    public class SetUsedFlag : CompositeControl
    {
        #region Controls

        private RadioButtonList _rdoBtnLstUsedFlag;

        private Button _btnOK;
        private Button _btnCancel;

        private Control _setUsedFlagOuterCtrl;

        #endregion

        #region Events

        private readonly static object EventCommandKey = new object();
        /// <summary>
        /// ִ������󼤷����¼���
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [Description("ִ������󼤷����¼���")]
        public event CommandEventHandler Command
        {
            add
            {
                Events.AddHandler(EventCommandKey, value);
            }
            remove
            {
                Events.RemoveHandler(EventCommandKey, value);
            }
        }

        /// <summary>
        /// ִ������󼤷����¼���
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler handler =
                (CommandEventHandler)Events[EventCommandKey];

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Properties

        private string _setUsedFlagControlID = string.Empty;
        /// <summary>
        /// ������ر���������/ͣ�ÿؼ����ⲿ�ؼ�ID��
        /// </summary>
        [Browsable(true)]
        [IDReferenceProperty]
        [TypeConverter(typeof(ControlIDConverter))]
        [Category("��Ϊ")]
        [Description("������ر���������/ͣ�ÿؼ����ⲿ�ؼ�ID��")]
        public string SetUsedFlagControlID
        {
            get { return _setUsedFlagControlID; }
            set { _setUsedFlagControlID = value; }
        }

        /// <summary>
        /// ��ȡ����������/ͣ�ñ�־��
        /// </summary>
        [Browsable(true)]
        [Bindable(true)]
        [Category("����")]
        [DefaultValue(UsedFlag.BeingUsed)]
        [Description("��ȡ����������/ͣ�ñ�־��")]
        public UsedFlag UsedFlag
        {
            get
            {
                return Utility.GetViewStatePropertyValue<UsedFlag>(this.ViewState,
                    "UsedFlag", UsedFlag.BeingUsed);
            }
            set
            {
                Utility.SetViewStatePropertyValue<UsedFlag>(this.ViewState,
                    "UsedFlag", value);
            }
        }

        /// <summary>
        /// �ظ����ֵķ���
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [NotifyParentProperty(true)]
        [DefaultValue(RepeatDirection.Horizontal)]
        [Description("�ظ����ֵķ���")]
        public RepeatDirection RepeatDirection
        {
            get
            {
                return Utility.GetViewStatePropertyValue<RepeatDirection>(this.ViewState,
                    "RepeatDirection", RepeatDirection.Horizontal);
            }
            set
            {
                Utility.SetViewStatePropertyValue<RepeatDirection>(this.ViewState,
                    "RepeatDirection", value);
            }
        }

        /// <summary>
        /// ȷ����������ơ�
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [NotifyParentProperty(true)]
        [DefaultValue("Confirm")]
        [Description("ȷ����������ơ�")]
        public string ConfirmCommandName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "ConfirmCommandName", "Confirm");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "ConfirmCommandName", value);
            }
        }

        /// <summary>
        /// ȡ����������ơ�
        /// </summary>
        [Browsable(true)]
        [Category("����")]
        [NotifyParentProperty(true)]
        [DefaultValue("Cancel")]
        [Description("ȡ����������ơ�")]
        public string CancelCommandName
        {
            get
            {
                return Utility.GetViewStatePropertyValue<string>(this.ViewState,
                    "CancelCommandName", "Cancel");
            }
            set
            {
                Utility.SetViewStatePropertyValue<string>(this.ViewState,
                    "CancelCommandName", value);
            }
        }

        #endregion

        #region Operates

        private void InitControl()
        {
            if (!this.DesignMode)
            {
                InitSetUsedFlagOuterControl();
                InitControlVisible();
            }
        }

        private void InitSetUsedFlagOuterControl()
        {
            if (string.IsNullOrEmpty(_setUsedFlagControlID))
            {
                return;
            }

            _setUsedFlagOuterCtrl = this.Page.FindControl(_setUsedFlagControlID);
            if (_setUsedFlagOuterCtrl == null)
            {
                _setUsedFlagOuterCtrl = Utility.FindControlInNamingContainer(this, _setUsedFlagControlID);
            }

            if (_setUsedFlagOuterCtrl is Button)
            {
                ((Button)_setUsedFlagOuterCtrl).UseSubmitBehavior = false;
            }
        }

        private void InitControlVisible()
        {
            string __eventTarget = Utility.GetRequestParameterValue(this.Page, Constants.RequestParameter.EventTarget);
            if (!string.IsNullOrEmpty(__eventTarget))
            {
                if (_setUsedFlagOuterCtrl != null && __eventTarget.Equals(_setUsedFlagOuterCtrl.UniqueID))
                {
                    this.Visible = !this.Visible;
                }
            }
        }

        private void ConstructUsedFlagControl()
        {
            _rdoBtnLstUsedFlag = new RadioButtonList();
            _rdoBtnLstUsedFlag.ID = "rdoBtnLstUsedFlag";
            _rdoBtnLstUsedFlag.RepeatDirection = this.RepeatDirection;
            _rdoBtnLstUsedFlag.Items.Add(new ListItem("����", UsedFlag.BeingUsed.ToString()));
            _rdoBtnLstUsedFlag.Items.Add(new ListItem("ͣ��", UsedFlag.Stopped.ToString()));
            _rdoBtnLstUsedFlag.SelectedIndex =
                _rdoBtnLstUsedFlag.Items.IndexOf(
                _rdoBtnLstUsedFlag.Items.FindByValue(UsedFlag.ToString()));

            this.Controls.Add(_rdoBtnLstUsedFlag);
        }

        private void ConstructCommandButtonControls()
        {
            _btnOK = new Button();
            _btnOK.ID = "btnOK";
            _btnOK.Text = "ȷ��";
            _btnOK.CommandName = "Confirm";
            _btnOK.UseSubmitBehavior = false;
            _btnOK.Command += new CommandEventHandler(_btnOK_Command);

            _btnCancel = new Button();
            _btnCancel.ID = "btnCancel";
            _btnCancel.Text = "ȡ��";
            _btnCancel.CommandName = "Cancel";
            _btnCancel.UseSubmitBehavior = false;
            _btnCancel.CausesValidation = false;
            _btnCancel.Command += new CommandEventHandler(_btnCancel_Command);

            this.Controls.Add(_btnOK);
            this.Controls.Add(_btnCancel);
        }

        void _btnCancel_Command(object sender, CommandEventArgs e)
        {
            this.Visible = false;
            OnCommand(e);
        }

        void _btnOK_Command(object sender, CommandEventArgs e)
        {
            this.Visible = false;
            OnCommand(e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// ��õ�ǰ���õ�����/ͣ�ñ�־��
        /// </summary>
        /// <returns></returns>
        public UsedFlag GetSetUsedFlag()
        {
            if (_rdoBtnLstUsedFlag != null)
            {
                if (!string.IsNullOrEmpty(_rdoBtnLstUsedFlag.SelectedValue))
                {
                    if (_rdoBtnLstUsedFlag.SelectedValue.Equals(UsedFlag.BeingUsed.ToString()))
                    {
                        return UsedFlag.BeingUsed;
                    }
                    if (_rdoBtnLstUsedFlag.SelectedValue.Equals(UsedFlag.Stopped.ToString()))
                    {
                        return UsedFlag.Stopped;
                    }
                }
                else
                {
                    return this.UsedFlag;
                }
            }
            return this.UsedFlag;
        }

        #endregion

        protected override object SaveControlState()
        {
            ArrayList list = new ArrayList();
            list.Add(base.SaveControlState());

            list.Add(_setUsedFlagControlID);

            return list;
        }

        protected override void LoadControlState(object savedState)
        {
            if (savedState is ArrayList)
            {
                ArrayList list = (ArrayList)savedState;

                if (list.Count >= 2)
                {
                    base.LoadControlState(list[0]);

                    _setUsedFlagControlID = (string)list[1];
                }
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Load += new EventHandler(Page_Load);
            this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);
        }

        void Page_Load(object sender, EventArgs e)
        {
            this.InitControl();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            this.ConstructUsedFlagControl();

            this.ConstructCommandButtonControls();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.AddAttributesToRender(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _rdoBtnLstUsedFlag.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            _btnOK.RenderControl(writer);
            _btnCancel.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }
    }
}
