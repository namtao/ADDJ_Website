using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;
using ADDJ.Entity;
using System.Web;
using ADDJ.Admin;
using System;
using System.Linq;

namespace Website.AppCode.Controller
{
    public class BuildPhongBan_Permission
    {

        public static bool CheckPermission(PermissionSchemes permission)
        {
            return CheckPermission(permission.GetHashCode());
        }

        public static bool CheckPermission(int permission)
        {
            var admin = LoginAdmin.AdminLogin();
            if (admin == null)
                return false;

            var PermissionAllow = HttpContext.Current.Session[Constant.SESSION_PERMISSION_SCHEMES] as Dictionary<int, bool>;
            if (PermissionAllow == null)
            {
                PermissionAllow = ServiceFactory.GetInstancePhongBan_Permission().GetPermission(admin);
                HttpContext.Current.Session[Constant.SESSION_PERMISSION_SCHEMES] = PermissionAllow;
            }

            if (PermissionAllow == null || PermissionAllow.Count == 0)
                return false;

            if (PermissionAllow.ContainsKey(permission))
            {
                return PermissionAllow[permission];
            }
            return false;            
        }



        public static string BuildListPhongBan_Permission(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstancePhongBan_Permission().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListPhongBan_Permission()
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstancePhongBan_Permission().GetList();
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListPhongBan_PermissionByPermissionSchemeId(int _permissionSchemeId)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstancePhongBan_Permission().GetListByPermissionSchemeId(_permissionSchemeId);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListPhongBan_PermissionByPhongBanId(int _phongBanId)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstancePhongBan_Permission().GetListByPhongBanId(_phongBanId);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

    }
}

