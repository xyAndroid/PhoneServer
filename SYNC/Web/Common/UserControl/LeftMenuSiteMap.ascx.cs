using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SYNC.Web.Common;

namespace SCMS.Web.Common.UserControl
{
    public partial class LeftMenuSiteMap : System.Web.UI.UserControl
    {
        private string _currentUserName;
        /// <summary>
        /// 当前登录用户的名称;
        /// </summary>
        protected string CurrentUserName
        {
            get
            {
                if (HttpContext.Current != null
                        && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity != null
                                && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    _currentUserName = HttpContext.Current.User.Identity.Name.Trim();
                return _currentUserName;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string currentRoleName = string.Empty;
            string[] r = System.Web.Security.Roles.GetRolesForUser(CurrentUserName);
            try
            {
                switch (r.Length)
                {
                    case 1: currentRoleName = r[0]; break;
                    default: currentRoleName = r.First(); break;
                }
            }
            catch (Exception)
            { Response.Redirect("~/Login.aspx"); }

            switch (currentRoleName)
            {
                case SYNCRoleName.Administrator: this.SiteMapDataSource1.SiteMapProvider = "Administrator"; break;
                case SYNCRoleName.User: this.SiteMapDataSource1.SiteMapProvider = "User"; break;
                default: this.SiteMapDataSource1.SiteMapProvider = null; break;
            }
        }

        protected void tv_PreRender(object sender, EventArgs e)
        {
            if (null != Session["SelectedNodeNo"])
            {
                int SelectedNodeNo = (int)Session["SelectedNodeNo"];
                Session.Remove("SelectedNodeNo");

                //折叠其他节点，只展开当前节点
                if (tv.Nodes[SelectedNodeNo].Parent != null)
                {
                    tv.Nodes[SelectedNodeNo].Parent.Expand();
                    tv.Nodes[SelectedNodeNo].Parent.Selected = true;
                }
                tv.Nodes[SelectedNodeNo].Expand();
                tv.Nodes[SelectedNodeNo].Selected = true;
            }
            else if (tv.SelectedNode != null)
            {
                //折叠其他节点，只展开当前节点
                if (tv.SelectedNode.Parent != null)
                {
                    tv.SelectedNode.Parent.Expand();
                    tv.SelectedNode.Expand();
                }
                else if (0 == tv.SelectedNode.Depth && tv.SelectedNode.ChildNodes.Count >= 2)
                {
                    Response.Redirect("~/Default.aspx");
                }
                tv.SelectedNode.Selected = true;
            }
        }

        protected void tv_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (tv.SelectedNode.Depth == 0)
            {   //如果是一级目录，需要展开，所以要记录下一级目录的索引
                Session["SelectedNodeNo"] = tv.Nodes.IndexOf(tv.SelectedNode) == -1 ? 0 : tv.Nodes.IndexOf(tv.SelectedNode);
            }
            if (0 == tv.SelectedNode.Depth && tv.SelectedNode.ChildNodes.Count >= 1)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Redirect(tv.SelectedNode.NavigateUrl);
            }
        }

        protected void tv_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            string urlPattern = e.Node.Value;
            if (!string.IsNullOrEmpty(urlPattern))
            {
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(urlPattern);

                if (rgx.IsMatch(this.Request.Url.AbsolutePath))
                {
                    if (e.Node.Parent != null)
                    {
                        e.Node.Parent.Expand();
                    }
                    e.Node.Selected = true;
                }
            }
        }
    }
}