using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SYNC;

namespace Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lblContactName_PreRender(object sender, EventArgs e)
        {
            Label lblContactName = (Label)sender;
            lblContactName.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            string CurrentUserName = lblContactName.Text;

            SYNCDataContext dc = new SYNCDataContext();
            string a=dc.Connection.ConnectionString;
            string tempRoleName = System.Web.Security.Roles.GetRolesForUser(CurrentUserName).FirstOrDefault();
            SYNC.aspnet_Roles aRole = dc.aspnet_Roles.FirstOrDefault(p => p.RoleName.Equals(tempRoleName));
            if (aRole != null)
            {
                lblContactName.ToolTip = aRole.Description;
                lblContactName.Text = lblContactName.Text + "(" + aRole.Description + ")";
            }
           
        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            if (e.Exception.InnerException != null)
            {
                this.ScriptManager1.AsyncPostBackErrorMessage = e.Exception.InnerException.Message;
            }
            else
            {
                this.ScriptManager1.AsyncPostBackErrorMessage = e.Exception.Message;
            }
        }

        protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
        {
            if (Session.Count > 0)
                this.Session.RemoveAll();
        }
    }
}
