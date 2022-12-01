using ADDJ.Admin;
using ADDJ.Core.Provider;
using ADDJ.Log.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website
{
    public partial class exit : System.Web.UI.Page
    {
        public string CASHOST = ConfigurationManager.AppSettings["cashost"];
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



            //string CASHOST = ConfigurationManager.AppSettings["cashost"];
            //String certPath = ConfigurationManager.AppSettings["casCert"];
            //string casLogout = "";
            //string casLogin = "";
            //Utils utils = new Utils();
            //string netid = "";

            //String servicePath = Request.Url.GetLeftPart(UriPartial.Path);
            //casLogout = CASHOST + "logout?service=" + servicePath;
            //casLogin = CASHOST + "login?service=" + servicePath;


            //Response.Redirect(casLogout.Replace("exit","otp"), false);

        }
    }
}