using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class NhomKNChoXL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng))
            {
                btnPhanViec.Visible = false;
            }
        }
    }
}