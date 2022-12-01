using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;
using AIVietNam.Admin;
using System.Text;
using AIVietNam.GQKN.Entity;
using AIVietNam.Core;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNSoTheoDoi : System.Web.UI.UserControl
    {
        protected string strFilterKhieuNai = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var admin = LoginAdmin.AdminLogin();
                    if (admin != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<option value=\"-2\">--Tất cả khiếu nại--</option>");
                        sb.AppendLine("<option value=\"-1\" selected=\"selected\">Cá nhân</option>");
                        sb.AppendFormat("<option value=\"{0}\">Phòng ban</option>", admin.PhongBanId);                        
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
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }
    }
}