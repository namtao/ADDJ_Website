using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.HeThongHoTro.Report
{
    public partial class BC_DanhMucBaoCao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void linkbtnBC_TongHopSoLieuNguoiDung_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/HeThongHoTro/Report/BC_TongHopSoLieu_YCHT_TheoNguoiDung.aspx");
        }

        protected void linkbtnBC_TongHopSoLieuPhongBan_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/HeThongHoTro/Report/BC_BaoCaoTongHopSoLieu_YCHT_TheoPhongBan.aspx");
        }
    }
}