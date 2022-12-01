using AIVietNam.Admin;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class UcTopMyKhieuNai : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();

            if (!IsPostBack)
            {
                var admin = LoginAdmin.AdminLogin();
                ltKNDaGui.Text = ServiceFactory.GetInstanceKhieuNai_Activity().GetTotalKhieuNaiGuiDi(admin.PhongBanId, admin.Username, (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới).ToString("#,##0");
                ltKNPhanHoi.Text = ServiceFactory.GetInstanceKhieuNai_Activity().GetTotalKhieuNaiPhanHoi(admin.PhongBanId, admin.Username, (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi).ToString("#,##0");
            }
        }
    }
}