using ADDJ.Admin;
using ADDJ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Website.AppCode
{
    public class PageBase : Page
    {
        public AdminInfo UserInfo { get; private set; }
        protected RightInfo Permission { get; private set; }
         protected override void OnPreLoad(EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();

            UserInfo = LoginAdmin.AdminLogin();

            Permission = UserRightImpl.CheckRightAdminnistrator_Cache();

            if (!Permission.UserRead)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }
            base.OnPreLoad(e);
        }
    }
}