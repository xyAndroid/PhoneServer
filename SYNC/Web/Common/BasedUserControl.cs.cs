using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eIvy.Web.UI.ConfigedControls;

namespace SYNC.Web.Common
{
    public class BasedUserControl : BasedConfigedControl
    {
        private SYNCDataContext _dataContext;
        /// <summary>
        /// 用于使用dbml来来处理数据的上下文对象
        /// </summary>
        protected SYNCDataContext DataContext
        {
            get
            {
                _dataContext = _dataContext ?? new SYNCDataContext();
                return _dataContext;
            }
        }


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


        private string _currentRoleName;
        /// <summary>
        /// 当前登录用户的角色名称;
        /// </summary>
        public string CurrentRoleName
        {
            get
            {
                if (!string.IsNullOrEmpty(CurrentUserName))
                {
                    string[] r = System.Web.Security.Roles.GetRolesForUser(CurrentUserName);
                    try
                    {
                        switch (r.Length)
                        {
                            case 1: _currentRoleName = r[0]; break;
                            default: _currentRoleName = r.First(); break;
                        }
                    }
                    catch (Exception)
                    { Response.Redirect("~/Login.aspx"); }
                }
                return _currentRoleName;
            }
        }


        /// <summary>
        /// 当前登录用户的ID;
        /// </summary>
        protected int CurrentEntityID
        {
            get
            {
                if (this.UserMapping == null) return 0;
                return Convert.ToInt32(this.UserMapping.EntityID);
            }
        }

        private UserMappings _usrMap;
        /// <summary>
        /// 该属性用来获取当前登录用户影射的实体信息。
        /// </summary>
        public UserMappings UserMapping
        {
            get
            {
                if (_usrMap == null)
                {
                    SYNCDataContext dc = new SYNCDataContext();

                    _usrMap =
                        dc.UserMappings.FirstOrDefault(
                        p => p.UserName.Equals(this.Page.User.Identity.Name.Trim()));
                }
                return _usrMap;
            }
        }
    }
}