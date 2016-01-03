using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SYNC.Web.Common
{
    public class SYNCRoleName
    {
        //1、系统管理员
        public const string Administrator = "Administrator";

        //2、普通用户
        public const string User = "User";

        /// <summary>
        /// 允许登陆的角色
        /// </summary>
        private static string[] _allowLoginRoles;
        public static string[] AllowLoginRoles
        {
            get
            {
                if (_allowLoginRoles == null || _allowLoginRoles.Length == 0)
                    _allowLoginRoles = new string[]{
                    Administrator,
                    User
                };
                return _allowLoginRoles;
            }
        }
    }
}