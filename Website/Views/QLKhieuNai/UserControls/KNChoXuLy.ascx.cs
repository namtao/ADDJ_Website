using System;
using AIVietNam.GQKN.Entity;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNChoXuLy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại))
            {
                btnDongKN.Visible = false;
            }

            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xử_lý_khiếu_nại))
            {
                btnUpdateKNHangLoat.Visible = false;
            }

            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
            {
                btnChuyenNgangHang.Visible = false;
                btnChuyenPhanHoi.Visible = false;
                btnChuyenXuLy.Visible = false;
            }
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_khiếu_nại))
            {
                btnTiepNhan.Visible = false;
                btnChuyenNgangHang.Visible = false;
            }
        }
    }
}