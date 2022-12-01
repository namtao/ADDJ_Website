using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai
{
    public partial class Excel : System.Web.UI.Page
    {
        private const int PageSizeExcel = 50;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();

            if (!IsPostBack)
            {
                Process();
            }
        }

        private void Process()
        {
            var admin = LoginAdmin.AdminLogin();
            int key = ConvertUtility.ToInt32(Request.QueryString["key"].ToString());
            string typeSearch = Request.QueryString["typeSearch"].ToString();
            string contentSeach = Request.Form["contentSeach"] ?? Request.QueryString["contentSeach"];
            string doUuTien = Request.QueryString["doUuTien"].ToString();
            string trangThai = Request.QueryString["trangThai"].ToString();
            string loaiKhieuNai = Request.QueryString["loaiKhieuNai"].ToString();
            string linhVucChung = Request.QueryString["linhVucChung"].ToString();
            string linhVucCon = Request.QueryString["linhVucCon"].ToString();
            var sortName = Request.QueryString["sortname"];
            var sortOrder = Request.QueryString["sortorder"];
            if (string.IsNullOrEmpty(sortName))
                sortName = "LDate";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "DESC";
            if(linhVucCon == "-1")
            {
                linhVucCon = "";
            }

            string phongBanXuLy = Request.QueryString["phongBanXuLy"].ToString();
            //LONGLX
            string ShowNguoiXuLy = Request.QueryString["ShowNguoiXuLy"].ToString();

            string NguoiXuLy_Default = "-1";
            bool IsTatCaKN = false;
            int KNHangLoat = 0;
            string PhongBanId = admin.PhongBanId.ToString();
            bool isPermission = false;
            if (ShowNguoiXuLy.Equals("1"))
            {
                NguoiXuLy_Default = "";
            }

            switch (typeSearch)
            {
                case "-2":
                    KNHangLoat = -1;
                    IsTatCaKN = true;
                    isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                    break;
                case "-1":
                    KNHangLoat = -1;
                    NguoiXuLy_Default = admin.Username;
                    break;
                case "0":
                    KNHangLoat = 1;
                    break;

                default:
                    PhongBanId = typeSearch;
                    break;
            }

            if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
            {
                contentSeach = string.Empty;
            }

            string SoThueBao = Request.Form["SoThueBao"] ?? Request.QueryString["SoThueBao"];
            int nSoThueBao = -1;
            if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
            {
                nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
            }

            //string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
            string NguoiTiepNhan = Request.Form["NguoiTiepNhan"] ?? Request.QueryString["NguoiTiepNhan"];
            int NguoiTiepNhanId = -1;
            if (!string.IsNullOrEmpty(NguoiTiepNhan) && !NguoiTiepNhan.Equals("Người tiếp nhận..."))
            {
                NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTiepNhan);
                if (NguoiTiepNhanId == 0)
                    NguoiTiepNhanId = -1;
            }
            else
            {
                NguoiTiepNhanId = -1;
                NguoiTiepNhan = "";
            }
            // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
            string NguoiTienXuLy = Request.Form["NguoiTienXuLy"] ?? Request.QueryString["NguoiTienXuLy"];
            int NguoiTienXuLyId = -1;
            if (!string.IsNullOrEmpty(NguoiTienXuLy) && !NguoiTienXuLy.Equals("Người tiền xử lý..."))
            {
                NguoiTienXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTienXuLy);
                if (NguoiTienXuLyId == 0)
                    NguoiTienXuLyId = -1;
            }
            else
            {
                NguoiTienXuLyId = -1;
                NguoiTienXuLy = "";
            }
            //string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
            string NguoiXuLy = Request.Form["NguoiXuLy"] ?? Request.QueryString["NguoiXuLy"];
            int NguoiXuLyId = -1;
            if (typeSearch != "-1" && !string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
            {
                NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiXuLy);
                if (NguoiXuLyId == 0)
                    NguoiXuLyId = -1;
            }
            else
            {
                NguoiXuLyId = -1;
                NguoiXuLy = "";
            }

            string NgayTiepNhan_From = Request.Form["NgayTiepNhan_From"] ?? Request.QueryString["NgayTiepNhan_From"];
            int nNgayTiepNhan_From = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayTiepNhan_To = Request.Form["NgayTiepNhan_To"] ?? Request.QueryString["NgayTiepNhan_To"];
            int nNgayTiepNhan_To = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayQuaHan_From = Request.Form["NgayQuaHan_From"] ?? Request.QueryString["NgayQuaHan_From"];
            int nNgayQuaHan_From = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayQuaHan_To = Request.Form["NgayQuaHan_To"] ?? Request.QueryString["NgayQuaHan_To"];
            int nNgayQuaHan_To = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayQuaHanPB_From = Request.Form["NgayQuaHanPB_From"] ?? Request.QueryString["NgayQuaHanPB_From"];
            int nNgayQuaHanPB_From = -1;
            if (!string.IsNullOrEmpty(NgayQuaHanPB_From) && !NgayQuaHanPB_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayQuaHanPB_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHanPB_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayQuaHanPB_To = Request.Form["NgayQuaHanPB_To"] ?? Request.QueryString["NgayQuaHanPB_To"];
            int nNgayQuaHanPB_To = -1;
            if (!string.IsNullOrEmpty(NgayQuaHanPB_To) && !NgayQuaHanPB_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayQuaHanPB_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHanPB_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            switch (key)
            {
                case 1: //Khiếu nại chờ xử lý
                    ExportKhieuNaiChoXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                                    PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                                    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                                    nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiChoXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                    //                PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                    //                nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //                KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission);
                    break;
                case 2:
                    ExportKhieuNaiChuyenBoPhanKhac(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiChuyenBoPhanKhac(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID, phongBanXuLy,
                    //        PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                    //        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //        KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission);
                    break;
                case 3:
                    ExportKhieuNaiBoPhanKhacChuyenVe(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiBoPhanKhacChuyenVe(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID, phongBanXuLy,
                    //        PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                    //        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //        KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission);
                    break;
                case 5:
                    ExportKhieuNaiQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID, phongBanXuLy,
                    //        PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId, 
                    //        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //        KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission);
                    break;
                case 9:
                    ExportKhieuNaiDaPhanHoi(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiDaPhanHoi(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID, phongBanXuLy,
                    //        PhongBanId, doUuTien, trangThai, NguoiXuLy,
                    //        nSoThueBao, NguoiTiepNhan, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //        KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission);
                    break;
                case 8: //Khiếu nại tổng hợp chờ xử lý
                    ExportKhieuNaiTongHopChoXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                                   PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                                   nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                                   nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, sortName, sortOrder);
                    //ExportKhieuNaiTongHopChoXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                    //                PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                    //                nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                    //                KNHangLoat, IsTatCaKN, admin.DoiTacId, isPermission, "Ldate", "DESC");
                    break;
                default:
                    export("hizhiz", "test");
                    break;
            }
        }

        #region KN Cho Xu Ly Tong Hop

        private void ExportKhieuNaiTongHopChoXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
           string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI TỔNG HỢP CHỜ XỬ LÝ</h1></th></tr>
                            <tr><th colspan='15'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
                                </th>                                
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>  
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
                                </th>	
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
                                </th>								                              
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai()
                        .CountTongSoKhieuNai_WithPage3(contentSeach, Convert.ToInt32(LoaiKhieuNaiId),
                            Convert.ToInt32(LinhVucChungId),
                            tenLinhVucCon,
                            Convert.ToInt32(PhongBanId),
                            Convert.ToInt32(DoUuTien),
                            Convert.ToInt32(trangThai),
                            NguoiXuLyId, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                            NgayQuaHan_From, NgayQuaHan_To,
                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);

                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage3(contentSeach,
                                                                                                ConvertUtility.ToInt32(LoaiKhieuNaiId),
                                                                                                ConvertUtility.ToInt32(LinhVucChungId),
                                                                                                tenLinhVucCon,
                                                                                                ConvertUtility.ToInt32(PhongBanId),
                                                                                                ConvertUtility.ToInt32(DoUuTien),
                                                                                                ConvertUtility.ToInt32(trangThai),
                                                                                                NguoiXuLyId,
                                                                                                NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                                                                                i,
                                                                                                PageSizeExcel);

                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
                                if (row.IsPhanViec)
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
                                }

                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");

                                // Phải để style định dạng thì khi mở file excel mới định dạng đúng dd/MM/yyyy, nếu không thì excel tự động định dạng m/d/yyyy hh:mm
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayTiepNhan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHanPhongBanXuLy.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNTongHopChoXuLy_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiTongHopChoXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
//            string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
//            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
//        {
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI TỔNG HỢP CHỜ XỬ LÝ</h1></th></tr>
//                            <tr><th colspan='15'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
//                                </th>                                
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>  
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
//                                </th>	
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
//                                </th>								                              
//                            </tr>");

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai()
//                        .CountTongSoKhieuNai_WithPage1(contentSeach, Convert.ToInt32(LoaiKhieuNaiId),
//                            Convert.ToInt32(LinhVucChungId),
//                            Convert.ToInt32(LinhVucConId),
//                            Convert.ToInt32(PhongBanId),
//                            Convert.ToInt32(DoUuTien),
//                            Convert.ToInt32(trangThai),
//                            NguoiXuLyId, NguoiTienXuLyId,
//                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                            NgayQuaHan_From, NgayQuaHan_To,
//                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {

//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai()
//                        .CountTongSoKhieuNai_WithPage2(contentSeach, Convert.ToInt32(LoaiKhieuNaiId),
//                            Convert.ToInt32(LinhVucChungId),
//                            LinhVucConId,
//                            Convert.ToInt32(PhongBanId),
//                            Convert.ToInt32(DoUuTien),
//                            Convert.ToInt32(trangThai),
//                            NguoiXuLyId, NguoiTienXuLyId,
//                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                            NgayQuaHan_From, NgayQuaHan_To,
//                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {
//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage1(contentSeach,
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                Convert.ToInt32(LinhVucConId),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLyId,
//                                                                                                NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage2(contentSeach,
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                LinhVucConId,
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLyId,
//                                                                                                NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }
                        

//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
//                                if (row.IsPhanViec)
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNTongHopChoXuLy_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region KN Cho Xu Ly

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        private void ExportKhieuNaiChoXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI CHỜ XỬ LÝ</h1></th></tr>
                            <tr><th colspan='15'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
                                </th>                                
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
                                </th>        
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
                                </th>                         
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai()
                        .CountKhieuNai_ChoXuLy_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                            Convert.ToInt32(LoaiKhieuNaiId),
                            Convert.ToInt32(LinhVucChungId),
                            tenLinhVucCon,
                            Convert.ToInt32(PhongBanId),
                            Convert.ToInt32(DoUuTien),
                            Convert.ToInt32(trangThai),
                            NguoiXuLyId,
                            NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                            NgayQuaHan_From, NgayQuaHan_To,
                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);

                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage3(contentSeach,
                                Convert.ToInt32(TypeSearch),
                                Convert.ToInt32(LoaiKhieuNaiId),
                                Convert.ToInt32(LinhVucChungId),
                                tenLinhVucCon,
                                Convert.ToInt32(PhongBanId),
                                Convert.ToInt32(DoUuTien),
                                Convert.ToInt32(trangThai),
                                NguoiXuLyId,
                                NguoiTienXuLyId,
                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                                NgayQuaHan_From, NgayQuaHan_To, NgayQuaHanPB_From, NgayQuaHanPB_To,
                                KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                i,
                                PageSizeExcel);

                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
                                if (row.IsPhanViec)
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
                                }

                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");

                                // Phải để style định dạng thì khi mở file excel mới định dạng đúng dd/MM/yyyy, nếu không thì excel tự động định dạng m/d/yyyy hh:mm
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayTiepNhan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHanPhongBanXuLy.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNChoXuLy_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiChoXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
//            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission)
//        {
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI CHỜ XỬ LÝ</h1></th></tr>
//                            <tr><th colspan='15'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
//                                </th>                                
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
//                                </th>        
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
//                                </th>                         
//                            </tr>");

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai()
//                        .CountKhieuNai_ChoXuLy_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
//                            Convert.ToInt32(LoaiKhieuNaiId),
//                            Convert.ToInt32(LinhVucChungId),
//                            Convert.ToInt32(LinhVucConId),
//                            Convert.ToInt32(PhongBanId),
//                            Convert.ToInt32(DoUuTien),
//                            Convert.ToInt32(trangThai),
//                            NguoiXuLyId,
//                            NguoiTienXuLyId,
//                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                            NgayQuaHan_From, NgayQuaHan_To,
//                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai()
//                        .CountKhieuNai_ChoXuLy_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
//                            Convert.ToInt32(LoaiKhieuNaiId),
//                            Convert.ToInt32(LinhVucChungId),
//                            LinhVucConId,
//                            Convert.ToInt32(PhongBanId),
//                            Convert.ToInt32(DoUuTien),
//                            Convert.ToInt32(trangThai),
//                            NguoiXuLyId,
//                            NguoiTienXuLyId,
//                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                            NgayQuaHan_From, NgayQuaHan_To,
//                            KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {
//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage1(contentSeach,
//                                Convert.ToInt32(TypeSearch),
//                                Convert.ToInt32(LoaiKhieuNaiId),
//                                Convert.ToInt32(LinhVucChungId),
//                                Convert.ToInt32(LinhVucConId),
//                                Convert.ToInt32(PhongBanId),
//                                Convert.ToInt32(DoUuTien),
//                                Convert.ToInt32(trangThai),
//                                NguoiXuLyId,
//                                NguoiTienXuLyId,
//                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                                NgayQuaHan_From, NgayQuaHan_To,
//                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                i,
//                                PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage2(contentSeach,
//                                Convert.ToInt32(TypeSearch),
//                                Convert.ToInt32(LoaiKhieuNaiId),
//                                Convert.ToInt32(LinhVucChungId),
//                                LinhVucConId,
//                                Convert.ToInt32(PhongBanId),
//                                Convert.ToInt32(DoUuTien),
//                                Convert.ToInt32(trangThai),
//                                NguoiXuLyId,
//                                NguoiTienXuLyId,
//                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
//                                NgayQuaHan_From, NgayQuaHan_To,
//                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                i,
//                                PageSizeExcel);
//                        }
//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
//                                if (row.IsPhanViec)
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>",row.HoTenLienHe);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNChoXuLy_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region KN Chuyen Bo Phan Khac

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="phongBanXuLy"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        private void ExportKhieuNaiChuyenBoPhanKhac(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetList();
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU CHUYỂN BỘ PHẬN KHÁC</h1></th></tr>
                            <tr><th colspan='15'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
                                </th>                                
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>        
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phòng ban xử lý
                                </th>                                
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
                                </th>                            
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiChuyenBoPhanKhac3(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        tenLinhVucCon,
                        Convert.ToInt32(phongBanXuLy),
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLy,
                        NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);


                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {

                        List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage3(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                tenLinhVucCon,
                                                                                                Convert.ToInt32(phongBanXuLy),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLy,
                                                                                                NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                                                                                i,
                                                                                                PageSizeExcel);

                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
                                if (row.IsPhanViec)
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");

                                PhongBanInfo objPhongBan = ServiceFactory.GetInstancePhongBan().GetPhongBanByIdFromList(listPhongBan, row.PhongBanXuLyId);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", objPhongBan != null ? objPhongBan.Name : string.Empty);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap2);
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNChuyenBoPhanKhac_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiChuyenBoPhanKhac(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
//            string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
//            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission)
//        {
//            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetList();
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU CHUYỂN BỘ PHẬN KHÁC</h1></th></tr>
//                            <tr><th colspan='15'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
//                                </th>                                
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>        
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phòng ban xử lý
//                                </th>                                
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
//                                </th>                            
//                            </tr>");

//                // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
//                //string NguoiTienXuLy = Request.Form["NguoiTienXuLy"] ?? Request.QueryString["NguoiTienXuLy"];
//                //if (NguoiTienXuLy != null && !NguoiTienXuLy.Equals("Người tiền xử lý..."))
//                //{
//                //    NguoiXuLy = NguoiTienXuLy;
//                //}

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiChuyenBoPhanKhac1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        Convert.ToInt32(LinhVucConId),
//                        Convert.ToInt32(phongBanXuLy),
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLy,
//                        NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiChuyenBoPhanKhac2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        LinhVucConId,
//                        Convert.ToInt32(phongBanXuLy),
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLy,
//                        NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }


//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {

//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage1(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                Convert.ToInt32(LinhVucConId),
//                                                                                                Convert.ToInt32(phongBanXuLy),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLy,
//                                                                                                NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage2(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                LinhVucConId,
//                                                                                                Convert.ToInt32(phongBanXuLy),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLy,
//                                                                                                NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }

//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
//                                if (row.IsPhanViec)
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");

//                                PhongBanInfo objPhongBan = ServiceFactory.GetInstancePhongBan().GetPhongBanByIdFromList(listPhongBan, row.PhongBanXuLyId);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", objPhongBan != null ? objPhongBan.Name : string.Empty);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap2);
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNChuyenBoPhanKhac_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region KN Bộ Phận Khác Chuyển Về

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="phongBanXuLy"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        private void ExportKhieuNaiBoPhanKhacChuyenVe(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='17'><h1>DANH SÁCH KHIẾU NẠI BỘ PHẬN KHÁC CHUYỂN VỀ</h1></th></tr>
                            <tr><th colspan='17'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người được phản hồi
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
                                </th>                                
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>    
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
                                </th>		                            
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe3(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        tenLinhVucCon,
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);



                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage3(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                tenLinhVucCon,
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                                                                                i,
                                                                                                PageSizeExcel);

                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</a></td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLyTruoc + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiDuocPhanHoi + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
                                if (row.IsPhanViec)
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNBoPhanKhacChuyenVe_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiBoPhanKhacChuyenVe(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
//            string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
//            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission)
//        {
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='17'><h1>DANH SÁCH KHIẾU NẠI BỘ PHẬN KHÁC CHUYỂN VỀ</h1></th></tr>
//                            <tr><th colspan='17'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người được phản hồi
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
//                                </th>                                
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>    
//<th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
//                                </th>
//<th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
//                                </th>
//<th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
//                                </th>
//<th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
//                                </th>
//<th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
//                                </th>
//<th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
//                                </th>		                            
//                            </tr>");

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        Convert.ToInt32(LinhVucConId),
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLyId, NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        LinhVucConId,
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLyId, NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }

                

//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {
//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage1(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                Convert.ToInt32(LinhVucConId),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLyId, NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage2(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                LinhVucConId,
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLyId, NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }


//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</a></td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLyTruoc + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiDuocPhanHoi + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
//                                if (row.IsPhanViec)
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNBoPhanKhacChuyenVe_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region KN Quá Hạn

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="phongBanXuLy"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        private void ExportKhieuNaiQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI QUÁ HẠN</h1></th></tr>
                            <tr><th colspan='15'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>                                
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
                                </th>                                
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>    
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
                                </th>
                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
                                </th>                                						                            
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan3(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                                        Convert.ToInt32(LinhVucChungId),
                                        tenLinhVucCon,
                                        Convert.ToInt32(PhongBanId),
                                        Convert.ToInt32(DoUuTien),
                                        NguoiXuLyId, NguoiTienXuLyId,
                                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);

                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage3(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                tenLinhVucCon,
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                                                                                i,
                                                                                                PageSizeExcel);


                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
                                if (row.IsPhanViec)
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
                                }

                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                //sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");

                                // Phải để style định dạng thì khi mở file excel mới định dạng đúng dd/MM/yyyy, nếu không thì excel tự động định dạng m/d/yyyy hh:mm
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayTiepNhan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHanPhongBanXuLy.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\" style=\"mso-number-format:'dd/mm/yyyy hh:mm:ss'\">" + row.NgayQuaHan.ToString("MM/dd/yyyy HH:mm:ss") + "</td>");

                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNQuaHan_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
//            string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
//            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission)
//        {
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='15'><h1>DANH SÁCH KHIẾU NẠI QUÁ HẠN</h1></th></tr>
//                            <tr><th colspan='15'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>                                
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phân việc
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PB
//                                </th>                                
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>    
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Họ tên liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa chỉ liên hệ
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Thời gian xảy ra
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã quận sự cố
//                                </th>
//                                <th class='thead-colunm' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã tỉnh sự cố 
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Địa điểm sự cố
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền XL phòng ban
//                                </th>                                						                            
//                            </tr>");

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        Convert.ToInt32(LinhVucConId),
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        NguoiXuLyId, NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        LinhVucConId,
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        NguoiXuLyId, NguoiTienXuLyId,
//                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }

//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {
//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage1(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                Convert.ToInt32(LinhVucConId),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                NguoiXuLyId, NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage2(contentSeach,
//                                                                                                Convert.ToInt32(TypeSearch),
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                LinhVucConId,
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                NguoiXuLyId, NguoiTienXuLyId,
//                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                                                                                                i,
//                                                                                                PageSizeExcel);
//                        }


//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");
//                                if (row.IsPhanViec)
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">True</td>");
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">False</td>");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.HoTenLienHe);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaChiLienHe.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.ThoiGianXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaQuan);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.MaTinh);
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.DiaDiemXayRa.Replace("<", "&lt;").Replace(">", "&gt;"));
//                                sb.AppendFormat("<td class =\"nowrap\" align=\"center\">{0}</td>", row.NguoiTienXuLyCap1);
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNQuaHan_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region KN Đã phản hồi

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="phongBanXuLy"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhan"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        private void ExportKhieuNaiDaPhanHoi(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
                            <tr><th colspan='16'><h1>DANH SÁCH KHIẾU NẠI ĐÃ PHẢN HỒI</h1></th></tr>
                            <tr><th colspan='16'>&nbsp;</th></tr>
                            <thead class='grid-data-thead'>
                                <tr>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
                                </th>
                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
                                </th>                                
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phòng ban xử lý
                                </th>
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
                                </th>                                
                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
                                </th>
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PBXL
                                </th>                                
                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
                                </th>                                
                            </tr>");

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords3(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        tenLinhVucCon,
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLy,
                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                
                if (TotalRecords > 0)
                {
                    var admin = LoginAdmin.AdminLogin();
                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
                    for (int i = 1; i <= totalPage; i++)
                    {

                        List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                               Convert.ToInt32(LoaiKhieuNaiId),
                               Convert.ToInt32(LinhVucChungId),
                               tenLinhVucCon,
                               Convert.ToInt32(PhongBanId),
                               Convert.ToInt32(DoUuTien),
                               Convert.ToInt32(trangThai),
                               NguoiXuLy,
                               SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                               NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                               i,
                               PageSizeExcel);

                        if (lstResult != null && lstResult.Count > 0)
                        {
                            int temp = 0;

                            string strReturnURL = string.Empty;


                            foreach (KhieuNaiInfo row in lstResult)
                            {
                                if (temp % 2 == 0)
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
                                }
                                else
                                {
                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
                                }

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
                                {
                                    if (row.NguoiXuLy != admin.Username)
                                    {
                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
                                }
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA.Replace("<", "&lt;").Replace(">", "&gt;") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLyTruoc + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(row.PhongBanXuLyId) + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");

                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
                                sb.Append(" </tr>");
                            }

                        }
                    }

                }

                sb.Append("</table>");

                export(sb.ToString(), "KNDaPhanHoi_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

//        private void ExportKhieuNaiDaPhanHoi(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
//            string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
//            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission)
//        {
//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='16'><h1>DANH SÁCH KHIẾU NẠI ĐÃ PHẢN HỒI</h1></th></tr>
//                            <tr><th colspan='16'>&nbsp;</th></tr>
//                            <thead class='grid-data-thead'>
//                                <tr>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>STT
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Trạng thái
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Mã PA/KN
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Độ ưu tiên
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px; width: 100px;background: #4D709A; color: #fff; border: 1pt solid;'>Số thuê bao
//                                </th>
//                                <th class='thead-colunm' style='padding-left: 5px;background: #4D709A; color: #fff; border: 1pt solid;width: 600px:'>Nội dung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Loại khiếu nại
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực chung
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Lĩnh vực con
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiếp nhận
//                                </th>                                
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người tiền xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Phòng ban xử lý
//                                </th>
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Người xử lý
//                                </th>                                
//                                <th class='thead-colunm' style='white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày tiếp nhận
//                                </th>
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn PBXL
//                                </th>                                
//                                <th class='thead-colunm-end' style='color: #fff; white-space: nowrap; padding-left: 5px; padding-right: 5px;background: #4D709A; color: #fff; border: 1pt solid;'>Ngày quá hạn TT
//                                </th>                                
//                            </tr>");

//                int TotalRecords = 0;
//                if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        Convert.ToInt32(LinhVucConId),
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLy,
//                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }
//                else
//                {
//                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                        Convert.ToInt32(LinhVucChungId),
//                        LinhVucConId,
//                        Convert.ToInt32(PhongBanId),
//                        Convert.ToInt32(DoUuTien),
//                        Convert.ToInt32(trangThai),
//                        NguoiXuLy,
//                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                        KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
//                }


//                //int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiDaPhanHoi(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
//                //Convert.ToInt32(LinhVucChungId),
//                //Convert.ToInt32(LinhVucConId),
//                //Convert.ToInt32(PhongBanId),
//                //Convert.ToInt32(DoUuTien),
//                //Convert.ToInt32(trangThai),
//                //NguoiXuLy,
//                //SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                //KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);

//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {

//                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
//                        if (int.Parse(LinhVucChungId) > 0 && LinhVucConId.Split(',').Count() <= 1)
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
//                               Convert.ToInt32(LoaiKhieuNaiId),
//                               Convert.ToInt32(LinhVucChungId),
//                               Convert.ToInt32(LinhVucConId),
//                               Convert.ToInt32(PhongBanId),
//                               Convert.ToInt32(DoUuTien),
//                               Convert.ToInt32(trangThai),
//                               NguoiXuLy,
//                               SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                               KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                               i,
//                               PageSizeExcel);
//                        }
//                        else
//                        {
//                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
//                               Convert.ToInt32(LoaiKhieuNaiId),
//                               Convert.ToInt32(LinhVucChungId),
//                               LinhVucConId,
//                               Convert.ToInt32(PhongBanId),
//                               Convert.ToInt32(DoUuTien),
//                               Convert.ToInt32(trangThai),
//                               NguoiXuLy,
//                               SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                               KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
//                               i,
//                               PageSizeExcel);
//                        }

//                        if (lstResult != null && lstResult.Count > 0)
//                        {
//                            int temp = 0;

//                            string strReturnURL = string.Empty;


//                            foreach (KhieuNaiInfo row in lstResult)
//                            {
//                                if (temp % 2 == 0)
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowA\">");
//                                }
//                                else
//                                {
//                                    sb.Append("<tr id =\"row-" + row.Id + "\" class=\"rowB\">");
//                                }

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.STT + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetColorTrangThaiXuLy(row.TrangThai) + "</td>");

//                                if (!string.IsNullOrEmpty(row.NguoiXuLy))
//                                {
//                                    if (row.NguoiXuLy != admin.Username)
//                                    {
//                                        sb.Append("<td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.SoThueBao + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiTiepNhan + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLyTruoc + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(row.PhongBanXuLyId) + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NguoiXuLy + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + row.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss") + "</td>");
//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");

//                export(sb.ToString(), "KNDaPhanHoi_" + DateTime.Now.ToString("ddMMyyyy_HHmmss"));
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//            }
//        }

        #endregion

        #region Utitlity

        private int GetTotalPage(int TotalRecords, int pageSize)
        {
            try
            {
                int totalRemainder = TotalRecords % pageSize;
                int totalPage = (TotalRecords - totalRemainder) / pageSize;
                if (totalRemainder > 0)
                {
                    totalPage = totalPage + 1;
                }
                return totalPage;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }

        private string GetColorTrangThaiXuLy(Int16 trangThaiXuLy)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_TrangThai_Type), trangThaiXuLy).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        public void export(string html, string name)
        {
            //XLWorkbook wb = new XLWorkbook();
            //var ws = wb.Worksheets.Add(name);
            //ws.Cell(2, 1).InsertTable(html);
            //Response.Clear();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", name.Replace(" ", "_")));

            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    wb.SaveAs(memoryStream);
            //    memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            //    memoryStream.Close();
            //}

            //Response.End();

            Response.Clear();
            Response.AddHeader("content-disposition", "inline;filename=" + name + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>" + name + "</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>" + html + "</table></body></html>");
            //Response.Write("Field\tValue\tCount\n");

            //Response.Write("Coin\tPenny\t443\n");
            //Response.Write("Coin\tNickel\t99\n"); 

            Response.End();
        }
        #endregion
    }
}