using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using System.Data;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNPhanViec : System.Web.UI.UserControl
    {
        NguoiSuDungImpl _NguoiSuDungImpl = new NguoiSuDungImpl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BuildPhongBan_Permission.CheckPermission(AIVietNam.GQKN.Entity.PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng))
            {
                Response.Redirect("/Views/QLKhieuNai/QuanLyKhieuNai.aspx");
                return;
            }
            if (!IsPostBack)
            {                
                LoadDanhSachPhanViec();
            }
        }
        private void LoadDanhSachPhanViec()
        {
            AdminInfo infoUser = (AdminInfo)Session[Constant.SessionNameAccountAdmin];
            if (infoUser != null)
            {
                var phongBanId = infoUser.PhongBanId;
                DataTable tab = _NguoiSuDungImpl.GetListNguoiSuDungByPhanViecPhongBanId(infoUser.PhongBanId);
                //List<NguoiSuDungInfo> list = _NguoiSuDungImpl.GetListNguoiSuDungByPhongBanId(infoUser.PhongBanId);
                if (tab.Rows.Count > 0)
                {
                    rptListDataPhanViec.DataSource = tab;
                    rptListDataPhanViec.DataBind();

                }
            }

        }
    }
}