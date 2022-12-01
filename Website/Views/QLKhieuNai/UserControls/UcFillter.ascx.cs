using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class UcFillter : System.Web.UI.UserControl
    {
        protected string strFilterKhieuNai = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //var admin = LoginAdmin.AdminLogin();
                //if (admin.Username.StartsWith("vnpttt_"))
                //{
                //    li.Visible = true;
                //    liChuyenNgangHang.Visible = false;
                //    liChuyenPhanHoi.Visible = false;
                //    liChuyenPhanHoiTrungTam.Visible = false;
                //    liChuyenXuLy.Visible = false;
                //    liDongKN.Visible = false;
                //}

                if (!IsPostBack)
                {
                    var admin = LoginAdmin.AdminLogin();
                    if (admin != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<option value=\"-2\">--Tất cả khiếu nại--</option>");
                        sb.AppendLine("<option value=\"-1\">Cá nhân</option>");
                        sb.AppendFormat("<option value=\"{0}\" selected=\"selected\">Phòng ban</option>", admin.PhongBanId);
                        sb.AppendFormat("<option value=\"{0}\">Khiếu nại hàng loạt</option>", 0);
                        if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới))
                        {
                            string whereClause = string.Format("Id != {0} and DoiTacId = {1} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {1} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {1})))", admin.PhongBanId, admin.DoiTacId);
                            var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", whereClause, "Sort");
                            foreach (var item in lstPhongBan)
                            {
                                sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
                            }

                        }
                        strFilterKhieuNai = sb.ToString();
                    }

                }
            }
            catch(Exception ex) {
                Utility.LogEvent(ex);
            }
        }
    }
}