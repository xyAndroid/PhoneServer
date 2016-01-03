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
    /// �Ͷ����ݿؼ��ṩ������ù��ܣ���Щ����ʹ�û����ԶԿؼ��е�����и��¡�ɾ��������ͷ�ҳ��
    /// ��Щ���ݿؼ��󶨵�����Դ�ؼ�ʱ���������ø�����Դ�ؼ��Ĺ��ܲ��ṩ�Զ����¡�ɾ��������ͷ�ҳ���ܡ�
    /// ��Щ���ݿؼ��磺FormView��DetailView��GridView��
    /// ��ö����ʾ��Ŀǰ֧�ֵ��������ơ�
    /// </summary>
    public enum CommandName
    {
        /// <summary>
        /// �ڸ��»�������������ȡ�������ͷ����û������ֵ��
        /// </summary>
        Cancel,
        /// <summary>
        /// ��ɾ�����������ڴ�����Դ��ɾ����ʾ�ļ�¼��
        /// </summary>
        Delete,
        /// <summary>
        /// �ڸ��²���������ʹ���ݿؼ����ڱ༭ģʽ���� EditItemTemplate ������ָ����������Ϊ��������ʾ�ġ�
        /// </summary>
        Edit,
        /// <summary>
        /// �ڲ�����������ڳ���ʹ���û��ṩ��ֵ������Դ�в����¼�¼������ ItemInserting �� ItemInserted �¼���
        /// </summary>
        Insert,
        /// <summary>
        /// �ڲ������������ʹ���ݿؼ����ڲ���ģʽ���� InsertItemTemplate ������ָ����������Ϊ��������ʾ�ġ�
        /// </summary>
        New,
        /// <summary>
        /// �ڷ�ҳ���������ڱ�ʾҳ��������ִ�з�ҳ�İ�ť����Ҫָ����ҳ�������뽫�ð�ť�� CommandArgument ��������Ϊ��Next������Prev������First������Last����Ҫ��������Ŀ��ҳ������������ PageIndexChanging �� PageIndexChanged �¼���
        /// </summary>
        Page,
        /// <summary>
        /// �ڸ��²��������ڳ���ʹ���û��ṩ��ֵ��������Դ������ʾ�ļ�¼������ ItemUpdating �� ItemUpdated �¼���
        /// </summary>
        Update,
    }

    /// <summary>
    /// ����ʹ��״̬�Ŀؼ����������Լ��ϡ�
    /// </summary>
    public class SetEnableStateControlCollection : List<SetEnableStateControl>
    {
        /// <summary>
        /// ���������Ŀ�Ŀؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddItemNewControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemNew"));
        }

        /// <summary>
        /// ����ɾ����Ŀ�Ŀؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddItemDeleteControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemDelete"));
        }

        /// <summary>
        /// ���ӱ༭��Ŀ�Ŀؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddItemEditControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableItemEdit"));
        }

        /// <summary>
        /// ��������/ͣ����Ŀ�Ŀؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddSetUsedFlagControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSetUsedFlag"));
        }

        /// <summary>
        /// ���ӵ�����Ŀ�Ŀؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddExportControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableExport"));
        }

        /// <summary>
        /// ���ӹ������ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddToolBarControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableToolBar"));
        }

        /// <summary>
        /// ������Ӹ����ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddAttachingControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableAttaching"));
        }

        /// <summary>
        /// ������˿ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddAuditingControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableAuditing"));
        }

        /// <summary>
        /// ���ӷ��ؿؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddReturnControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableReturn"));
        }

        /// <summary>
        /// ���Ӳ�ѯ�ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddSeekControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSeek"));
        }

        /// <summary>
        /// ���������ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddSearchControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnableSearch"));
        }

        /// <summary>
        /// ���Ӵ�ӡ�ؼ���
        /// </summary>
        /// <param name="ctrlId">�ؼ�ID</param>
        public void AddPrintControl(string ctrlId)
        {
            this.Add(new SetEnableStateControl(ctrlId, "EnablePrint"));
        }
    }

    /// <summary>
    /// ����ʹ��״̬�Ŀؼ����������ԡ�
    /// </summary>
    public class SetEnableStateControl
    {
        /// <summary>
        /// ���ո����Ŀؼ��Ϳ������Թ������
        /// </summary>
        /// <param name="ctrlId">�����Ŀؼ�</param>
        /// <param name="propertyName">��������</param>
        public SetEnableStateControl(string ctrlId, string propertyName)
        {
            _ctrlId = ctrlId;
            _propName = propertyName;
        }

        private string _ctrlId;
        /// <summary>
        /// ��Ҫ����ʹ��״̬�Ŀؼ���
        /// </summary>
        public string ControlID
        {
            get { return _ctrlId; }
            set { _ctrlId = value; }
        }

        private string _propName;
        /// <summary>
        /// ����ʹ��״̬�����ԡ�
        /// </summary>
        public string PropertyName
        {
            get { return _propName; }
            set { _propName = value; }
        }
    }

    /// <summary>
    /// �ṩ�û��ؼ��Ļ��ࡣ
    /// </summary>
    [Obsolete]
    public partial class BaseUserControl : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// �ܷ��½���Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ��½���Ŀ��")]
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
        /// �ܷ�ɾ����Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ�ɾ����Ŀ��")]
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
        /// �ܷ��޸���Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ��޸���Ŀ��")]
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
        /// �ܷ���ʾ��Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ���ʾ��Ŀ��")]
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
        /// �ܷ���ʾ��Ŀ���顣
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ���ʾ��Ŀ���顣")]
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
        /// �ܷ�ִ�з��ء�
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ�з��ء�")]
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
        /// �ܷ�ִ�в�ѯ��Ŀ���顣
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ�в�ѯ��Ŀ���顣")]
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
        /// �ܷ�ִ��������Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ��������Ŀ��")]
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
        /// �ܷ�ִ�д�ӡ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ�д�ӡ��")]
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
        /// �ܷ�����/ͣ����Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ�����/ͣ����Ŀ��")]
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
        /// �ܷ񵼳���Ŀ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ񵼳���Ŀ��")]
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
        /// �ܷ���Ӹ�����
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ���Ӹ�����")]
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
        /// �ܷ�ִ����ˡ�
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ����ˡ�")]
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
        /// �ܷ�ִ�й�������
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("��Ϊ")]
        [Description("�ܷ�ִ�й�������")]
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
        /// ҳ��չ�ֵ���ͼģʽ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(ViewMode.AllItems)]
        [Category("��Ϊ")]
        [Description("ҳ��չ�ֵ���ͼģʽ��")]
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
        /// �����ݱ�ʽչ��ʱ��ѡ��ģʽ��
        /// </summary>
        [Browsable(true)]
        [DefaultValue(GridViewRowSelectMode.Multiple)]
        [Category("��Ϊ")]
        [Description("�����ݱ�ʽչ��ʱ��ѡ��ģʽ��")]
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
        /// �½���Ŀ�����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("�½���Ŀ�����ӡ�")]
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
        /// �޸���Ŀ�����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("�޸���Ŀ�����ӡ�")]
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
        /// ��ʾ��Ŀ�����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("��ʾ��Ŀ�����ӡ�")]
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
        /// ����/ͣ����Ŀ�����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("����/ͣ����Ŀ�����ӡ�")]
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
        /// ��Ӹ��������ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("��Ӹ��������ӡ�")]
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
        /// ִ����˵����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("ִ����˵����ӡ�")]
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
        /// �鿴��������ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("�鿴��������ӡ�")]
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
        /// ��ѯҳ������ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("��ѯҳ������ӡ�")]
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
        /// ����ҳ������ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("����ҳ������ӡ�")]
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
        /// ��ӡҳ������ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("��ӡҳ������ӡ�")]
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
        /// ���ص����ӡ�
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("���ص����ӡ�")]
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
        /// ���շ������ӵĵ�ַ�������������
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ReturnUrl")]
        [Category("��Ϊ")]
        [Description("���շ������ӵĵ�ַ�������������")]
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
        /// ��ʾ����ID�ĵ�ַ�������������
        /// </summary>
        [Browsable(true)]
        [DefaultValue("ID")]
        [Category("��Ϊ")]
        [Description("��ʾ����ID�ĵ�ַ�������������")]
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
        /// ������Ŀ�ı��⡣
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("����")]
        [Description("����/������Ŀ�ı��⡣")]
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
        /// ������Ŀ�ı�����ļ�����
        /// </summary>
        [Browsable(true)]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("")]
        [Category("����")]
        [Description("������Ŀ�ı�����ļ�����")]
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
        /// ���������ֵ��
        /// </summary>
        [Browsable(false)]
        [Category("����")]
        [Description("���������ֵ��")]
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
        /// ���ڿؼ�֮��ͨ��ʱ���������ݶ������� Key ���ڱ�ʶͨ�����ݵ����ƣ������ֳ���ͬ��ͨ���������ݣ�Value ���Ǿ����ͨ�����ݶ���
        /// ��������Ҫ���� SendData �������ݶ���RecieveData �������ݶ��󣬲�����صĴ���������ֻ�����ڴ���һ��ͨ�����ݶ��󣬲��һ���� CommunicationObjects ͨ�����ݶ��󼯺ϵĵ�һ��Ԫ�ء�
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
        /// ���ڿؼ�֮��ͨ��ʱ���������ݶ��󣬲�����һ����󼯺ϣ�ÿ������Ϊһ�� KeyValuePair �ļ�ֵ�ԡ�
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
        /// �������ͨ�ŵķ�ʽ��
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
        /// �����Ҫ����ʹ��״̬�Ŀؼ����ϣ�����ͨ����ü����������Ҫ����ʹ�ܿ��ƵĿؼ����ɻ����û��ؼ���������Щ�ؼ���ʹ��״̬��
        /// </summary>
        /// <summary>
        /// ����ӳ�伯�ϡ�
        /// </summary>
        [Category("����")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(PropertyMappingCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Description("����ӳ�伯�ϡ�")]
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
        /// �ӵ�ǰ�û��ؼ�����ͼ״̬�л������ֵ��
        /// </summary>
        /// <typeparam name="T">���Ե����͡�</typeparam>
        /// <param name="propertyName">�������ơ�</param>
        /// <param name="defaultValue">����Ĭ��ֵ��</param>
        /// <returns>�����������ֵ�򷵻ظ�ֵ�����򷵻�Ĭ��ֵ��</returns>
        protected T GetViewStatePropertyValue<T>(string propertyName, T defaultValue)
        {
            return Utility.GetViewStatePropertyValue<T>(this.ViewState, propertyName, defaultValue);
        }

        /// <summary>
        /// ��ָ��������ֵ���뵱ǰ�û��ؼ�����ͼ״̬�С�
        /// </summary>
        /// <typeparam name="T">���Ե����͡�</typeparam>
        /// <param name="propertyName">�������ơ�</param>
        /// <param name="value">����ֵ��</param>
        protected void SetViewStatePropertyValue<T>(string propertyName, T value)
        {
            Utility.SetViewStatePropertyValue<T>(this.ViewState, propertyName, value);
        }

        /// <summary>
        /// ����Ŀ������        
        /// </summary>
        /// <param name="gv">��Ҫ������ GridView �ؼ���</param>
        /// <param name="needToHideColumnIndexes">��Ҫ�ڵ�����ʱ�����ص����������顣</param>
        protected virtual void ExportItems(GridView gv, int[] needToHideColumnIndexes)
        {
            Utility.ExportToExcel(gv, needToHideColumnIndexes,
                this.ExportTitle, this.ExportFile);
        }

        /// <summary>
        /// �ı䵱ǰ��ҳ��չ��ģʽ��
        /// </summary>
        /// <param name="formView">FormView�ؼ���</param>
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
                    throw new Exception("ҳ��չ��ģʽ����");
            }
        }

        /// <summary>
        /// �ı䵱ǰ��ҳ��չ��ģʽ��
        /// </summary>
        /// <param name="detailView">DetailView�ؼ���</param>
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
                    throw new Exception("ҳ��չ��ģʽ����");
            }
        }

        /// <summary>
        /// �����ṩ��������ִ���������ҳ����ת��һ�������ݿؼ��ġ�ItemCommand���¼��е��ø÷��������Ҹ÷����ṩһ��Ĭ����ת���ƣ���������Ը��ݾ�����Ҫ��д�÷�����ʵ���Լ�����ת���̡�
        /// </summary>
        /// <param name="cmdName">������ִ�е����</param>
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
        /// ��������
        /// ��������˳������ִ�з��أ�
        /// ��1�������ַ������<see cref="ReturnUrlRequestParameterName"/>����ָ���Ĳ������򰴴˲����ĵ�ַ���أ�
        /// ��2�����������<see cref="UrlOfReturn"/>���ԣ��򰴴�����ֵ�ĵ�ַ���أ�
        /// ��3�����������������ڣ�������ҳ��ַ���ء�
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
        /// �½���Ŀҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfItemNew"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ��2�����������������ڣ�������ҳ��ַ��ת��
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
        /// �༭��Ŀҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfItemEdit"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��SomeEditPage.aspx?ID=1��������
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
        /// ��ʾ��Ŀҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfItemDisp"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��SomeDispPage.aspx?ID=1��������
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
        /// ����/ͣ��ҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfSetUsedFlag"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��SetUsedFlagPage.aspx?ID=1��������
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
        /// ����ҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfDetail"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��DetailPage.aspx?ID=1��������
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
        /// ��ѯҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfSeek"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��SeekPage.aspx?ID=1��������
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
        /// ����ҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfSearch"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��SearchPage.aspx?ID=1��������
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
        /// ��Ӹ���ҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfAttaching"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��AttachingPage.aspx?ID=1��������
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
        /// ���ҳ����
        /// ��������˳������ִ�з��أ�
        /// ��1�����������<see cref="UrlOfAuditing"/>���ԣ��򰴴�����ֵ�ĵ�ַ��ת��
        /// ����ת��ʱ��Ὣ<see cref="KeyIDRequestParameterName"/>���Ժ�<see cref="KeyValue"/>������Ϊ���ݣ����ڵ�ַ�����γ���ͬ��AuditPage.aspx?ID=1��������
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
        /// ����ҳ��
        /// </summary>
        /// <param name="url">ԭ����ҳ��ַ��</param>
        /// <param name="requestParams">��������б�</param>
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
        /// ���������û��ؼ��������ݡ�
        /// </summary>
        /// <param name="reciever">�������ݵ��û��ؼ���</param>
        /// <param name="data">���͵����ݡ�</param>
        public void SendData(BaseUserControl reciever, object data)
        {
            reciever.RecieveData(data);
        }

        private const string RecieverIDSessionName = "__RecieverID__";
        private const string CommunicationObjectsSessionName = "__CommunicationObjects__";
        /// <summary>
        /// �ڲ�ͬҳ��֮�䴫�����ݽ���ͨ�š�
        /// </summary>
        /// <param name="url">��תҳ���Url��</param>
        /// <param name="recieverId">���ܷ������ID��</param>
        /// <param name="data">��Ҫ���͵����ݶ���</param>
        /// <remarks>�÷������Զ���תҳ�棬���ҽ����ݵ����ݷ���Ự�У�ͬʱ CommunicationMode ����ʶΪ Session��������ת���ҳ��� PreLoad �¼��е��� RecieveData �����������ݡ����ܶ�����Ҫ��д ReceiveData ���Խ��ܲ��������ݡ�</remarks>
        public void SendData(string url, string recieverId, object data)
        {
            this.Session[RecieverIDSessionName] = recieverId;
            this.Session[CommunicationObjectsSessionName] = data;
            this.ResponseRedirect(url);
        }

        /// <summary>
        /// ���������û����͵����ݣ����Ҵ������ݡ�����Ҫ������ȥ��д�÷���ʵ�־���Ĵ���
        /// </summary>
        /// <param name="data">���ܵ����ݡ�</param>
        protected virtual void RecieveData(object data)
        {
            if (data is CommunicationObjectCollection)
            {
                this.CommunicationObjects.AddRange((CommunicationObjectCollection)data);
            }
        }

        /// <summary>
        /// ��Ҫ�ṩ�������û��ؼ���д������ SetEnableStateControls ��ָ���Ŀؼ�ʹ�����ü��ϣ����ÿؼ���ʹ��״̬��
        /// �÷����ᱻҳ��� PreRender �¼��������á�
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
