using System;
using System.Web.UI;
using ADDJ.Core;
using ADDJ.Admin;
using ADDJ.Log.Impl;
using Website.AppCode;
using ADDJ.Core.Provider;
using Website.HTHTKT;

public partial class Thoat : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AdminInfo info = LoginAdmin.AdminLogin();
        if (info != null)
        {
            //ServiceFactory.GetInstanceNguoiSuDung().UpdateDynamic("IsLogin = 0", "Id = " + info.Id);
            using (var ctx = new ADDJContext())
            {
                var strSqlFileTemp = string.Format(@"update HT_NGUOIDUNG SET IsLogin = 0 where Id={0}", info.Id);
                ctx.Database.ExecuteSqlCommand(strSqlFileTemp);
            }
            LogImpl.Log(ADDJ.Log.Entity.ObjTypeLog.System, ADDJ.Log.Entity.ActionLog.Logout);
        }

        // Hủy phiên là việc
        Session.Abandon();

        // xóa cache, refresh lại menu
        CacheProvider.ClearCache();


        Response.Redirect("loginai.aspx", false);


        //// Chuyển hướng url
        //if (Request.IsLocal) Response.Redirect("~/");
        //else if (AppSetting.IsLoginLocal) Response.Redirect("~/");
        //else
        //{
        //    if (Config.IsLocal) Response.Redirect("~/");
        //    else Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["UrlLogout"]);
        //}
    }
}
