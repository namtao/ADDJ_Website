using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using Website.AppCode;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNLichSuPhanViec : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var obj_admin = LoginAdmin.AdminLogin();
                int phongBanId = obj_admin.PhongBanId;
                txtPhongBan.Text = ServiceFactory.GetInstancePhongBan().GetInfo(phongBanId).Name;
            }
        }
    }
}