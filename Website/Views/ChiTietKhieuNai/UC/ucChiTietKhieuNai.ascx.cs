using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucChiTietKhieuNai : System.Web.UI.UserControl
    {
        public int KhieuNaiId { get; set; }
        public bool IsTraSau { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            if (!IsPostBack)
                UcCacBuocXuLy.KhieuNaiId = KhieuNaiId;
            ////UcFileDinhKem.KhieuNaiId = KhieuNaiId;
            //UcKieuNaiCuocDichVu.KhieuNaiId = KhieuNaiId;
            //UcTimKiemGiaiPhap.KhieuNaiId = KhieuNaiId;
            //UcKetQuaGiaiQuyetKN.KhieuNaiId = KhieuNaiId;
            //UcLuuVetThayDoi.KhieuNaiId = KhieuNaiId;
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabContainer1.ActiveTabIndex == 0)
            {
                UcCacBuocXuLy.KhieuNaiId = KhieuNaiId;
                UcCacBuocXuLy.FillBuocXuLyGrid();
            }
            else if (TabContainer1.ActiveTabIndex == 1)
            {
                UcFileDinhKem.KhieuNaiId = KhieuNaiId;
                UcFileDinhKem.FillFileDinhKemGrid();
            }
            else if (TabContainer1.ActiveTabIndex == 2)
            {
                UcKieuNaiCuocDichVu.KhieuNaiId = KhieuNaiId;
                UcKieuNaiCuocDichVu.FillKieuNaiCuocDichVuGrid();
                UcKieuNaiCuocDichVu.IsTraSau = IsTraSau;
            }
            else if (TabContainer1.ActiveTabIndex == 3)
            {
                UcKetQuaGiaiQuyetKN.KhieuNaiId = KhieuNaiId;
                UcKetQuaGiaiQuyetKN.FillKetQuaGiaiQuyetKNGrid();
            }
            else if (TabContainer1.ActiveTabIndex == 4)
            {
                UcLuuVetThayDoi.KhieuNaiId = KhieuNaiId;
                UcLuuVetThayDoi.FillLuuVetThayDoiGrid();
            }
            else if (TabContainer1.ActiveTabIndex == 5)
            {
                UcQuaTrinhKhieuNai.KhieuNaiId = KhieuNaiId;
                UcQuaTrinhKhieuNai.FillQuaTrinhKhieuNaiGrid();
            }
            return;
        }
    }
}