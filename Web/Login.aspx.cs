using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SyncServer.Web.Common;

namespace SCMS.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void MyLogin_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), DateTime.Now.Ticks.ToString(),
            //    "<script>alert('" + aReturn.Value + " ')</script>", false);
            //e.Cancel = true;
            #region 验证码匹配
            TextBox txtValidCode = (TextBox)this.MyLogin.FindControl("VerificationCode");
            if (txtValidCode != null)
            {
                if (Session["ValidateCode"] != null)
                {
                    string validCode = Session["ValidateCode"].ToString();
                    string tempValidCode = txtValidCode.Text;
                    Session.Remove("ValidateCode");
                    txtValidCode.Text = string.Empty;
                    if (!string.IsNullOrEmpty(tempValidCode) && !string.Equals(validCode, tempValidCode.Trim()))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.Ticks.ToString(), "<script>alert('验证码输入错误！')</script>");
                        e.Cancel = true;
                        return;
                    }
                }
            }
            #endregion

            #region 用户验证
            MembershipUser aUser = Membership.GetUser(MyLogin.UserName);
            if (aUser != null)
            {
                if (aUser.IsLockedOut == true)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.Ticks.ToString(), "<script>alert('此帐号已锁定或失效！')</script>");
                    e.Cancel = true;
                    return;
                }
            }

            if (!Membership.ValidateUser(MyLogin.UserName, MyLogin.Password))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.Ticks.ToString(), "<script>alert('您输入的用户名或密码错误！请检查！')</script>");
                e.Cancel = true;
                return;
            }

            //string[] CurrentRoles = System.Web.Security.Roles.GetRolesForUser(MyLogin.UserName);

            //int count = 0;
            //foreach (string s in CurrentRoles)
            //{
            //    if (SCMSRoleName.AllowLoginRoles.Length != 0 && SCMSRoleName.AllowLoginRoles.Contains(s))
            //    {
            //        count++;
            //    }
            //}
            //if (count == 0)
            //{
            //    e.Cancel = true;
            //    return;
            //}
            #endregion
        }

        protected void MyLogin_LoggedIn(object sender, EventArgs e)
        {
            Session[BasedUserControl.LoginRoleSessionKey] = System.Web.Security.Roles.GetRolesForUser(MyLogin.UserName).FirstOrDefault();
            if (!string.IsNullOrEmpty(Request.Url.Query))
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}