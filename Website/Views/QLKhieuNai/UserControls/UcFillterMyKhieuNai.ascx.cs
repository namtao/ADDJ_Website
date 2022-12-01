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
    public partial class UcFillterMyKhieuNai : System.Web.UI.UserControl
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
                        if (Request.QueryString["ctrl"] != null && !Request.QueryString["ctrl"].Equals(""))
                        {
                            
                            var arr = Request.QueryString["ctrl"].Split('-');

                            if (arr[1] == "KNPhanHoi")
                            {
                                
                                sb.AppendLine("<option value=\"0\">--Tất cả khiếu nại--</option>");

                                sb.AppendFormat("<option value=\"{0}\" >Phòng ban</option>", 3);
                                sb.AppendLine("<option value=\"1\" selected=\"selected\">Khiếu nại của tôi</option>");
                                strFilterKhieuNai = sb.ToString();
                                //sb.AppendFormat("<option value=\"{0}\">Khiếu nại đã chuyển</option>", 2);
                            }
                            else
                            {
                                
                                sb.AppendLine("<option value=\"0\">--Tất cả khiếu nại--</option>");

                                sb.AppendFormat("<option value=\"{0}\" >Phòng ban</option>", 3);
                                sb.AppendLine("<option value=\"1\" selected=\"selected\">Khiếu nại của tôi</option>");
                                sb.AppendFormat("<option value=\"{0}\">Khiếu nại đã chuyển</option>", 2);
                                strFilterKhieuNai = sb.ToString();
                            }
                        }
                        else
                        {
                          
                            sb.AppendLine("<option value=\"0\">--Tất cả khiếu nại--</option>");

                            sb.AppendFormat("<option value=\"{0}\" >Phòng ban</option>", 3);
                            sb.AppendLine("<option value=\"1\" selected=\"selected\">Khiếu nại của tôi</option>");
                            sb.AppendFormat("<option value=\"{0}\">Khiếu nại đã chuyển</option>", 2);
                            strFilterKhieuNai = sb.ToString();
                        }
                        
                        //if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới))
                        //{
                        //    string whereClause = string.Format("Id != {0} and DoiTacId = {1} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {1} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {1})))", admin.PhongBanId, admin.DoiTacId);
                        //    var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", whereClause, "Sort");
                        //    foreach (var item in lstPhongBan)
                        //    {
                        //        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
                        //    }

                        //}
                       
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