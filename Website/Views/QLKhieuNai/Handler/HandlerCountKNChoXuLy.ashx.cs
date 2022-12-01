using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Aspose.Cells;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerCountKNChoXuLy
    /// </summary>
    public class HandlerCountKNChoXuLy : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string strReturn = string.Empty;
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["key"] ?? context.Request.QueryString["key"];
            AdminInfo infoUser = LoginAdmin.AdminLogin();
            if (infoUser == null)
            {
                context.Response.Write(strReturn);
                return;
            }
            
            string typeSearch = context.Request.Form["typeSearch"] ?? context.Request.QueryString["typeSearch"];
            string doUuTien = context.Request.Form["doUuTien"] ?? context.Request.QueryString["doUuTien"];
            string trangThai = context.Request.Form["trangThai"] ?? context.Request.QueryString["trangThai"];
            string loaiKhieuNai = context.Request.Form["loaiKhieuNai"] ?? context.Request.QueryString["loaiKhieuNai"];
            string linhVucChung = context.Request.Form["linhVucChung"] ?? context.Request.QueryString["linhVucChung"];
            string linhVucCon = context.Request.Form["linhVucCon"] ?? context.Request.QueryString["linhVucCon"];
            string phongBanXuLy = context.Request.Form["phongBanXuLy"] ?? context.Request.QueryString["phongBanXuLy"];

            //LONGLX
            string ShowNguoiXuLy = context.Request.Form["ShowNguoiXuLy"] ?? context.Request.QueryString["ShowNguoiXuLy"];

            string NguoiXuLy_Filter = "-1";
            bool IsTatCaKN = false;
            int KNHangLoat = 0;
            string PhongBanId = infoUser.PhongBanId.ToString();
            bool isPermission = false;

            if (ShowNguoiXuLy.Equals("1"))
            {
                NguoiXuLy_Filter = "";
            }

            switch (typeSearch)
            {
                case "-2": //Tất cả
                    KNHangLoat = -1;
                    IsTatCaKN = true;
                    isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                    break;
                case "-1": //Cá nhân
                    KNHangLoat = -1;
                    NguoiXuLy_Filter = infoUser.Username;
                    break;
                case "0": //Hàng loạt
                    KNHangLoat = 1;
                    break;
                default: //Phòng ban
                    PhongBanId = typeSearch;
                    break;
            }

            //string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
            string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
            if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
            {
                contentSeach = string.Empty;
            }

            //string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
            string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
            int nSoThueBao = -1;
            if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
            {
                nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
            }

            //string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
            string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
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
            string NguoiTienXuLy = context.Request.Form["NguoiTienXuLy"] ?? context.Request.QueryString["NguoiTienXuLy"];
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
            string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
            int NguoiXuLyId = -1;
            if (typeSearch != "-1" &&  !string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
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

            //string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
            string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
            int nNgayTiepNhan_From = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
            string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
            int nNgayTiepNhan_To = -1;
            if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
            string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
            int nNgayQuaHan_From = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
            string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
            int nNgayQuaHan_To = -1;
            if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string NgayQuaHanPB_From = context.Request.Form["NgayQuaHanPB_From"] ?? context.Request.QueryString["NgayQuaHanPB_From"];
            int nNgayQuaHanPB_From = -1;
            if (!string.IsNullOrEmpty(NgayQuaHanPB_From) && !NgayQuaHanPB_From.Equals("Từ ngày..."))
            {
                try
                {
                    nNgayQuaHanPB_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHanPB_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            //string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
            string NgayQuaHanPB_To = context.Request.Form["NgayQuaHanPB_To"] ?? context.Request.QueryString["NgayQuaHanPB_To"];
            int nNgayQuaHanPB_To = -1;
            if (!string.IsNullOrEmpty(NgayQuaHanPB_To) && !NgayQuaHanPB_To.Equals("Đến ngày..."))
            {
                try
                {
                    nNgayQuaHanPB_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHanPB_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                }
                catch { }
            }

            string ListLinhVucConID = "";
            if (linhVucCon != "0" && linhVucCon != "-1")
            {
                var ListLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id", "Name=" + "N'" + linhVucCon + "'", "");
                for (int i = 0; i < ListLinhVucCon.Count; i++)
                {
                    if (i < ListLinhVucCon.Count -1)
                    {
                        ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id + ",";
                    }
                    else
                    {
                        ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id;
                    }
                }
            }
            else
            {
                ListLinhVucConID = linhVucCon;
            }
            string startPageIndex = "1";
            string pageSize = "1";
            
            switch (type)
            {
                case "3":
                    strReturn = CountKhieuNai_ChoXuLy_WithPage(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "6":
                    strReturn = GetKhieuNaiChuyenBoPhanKhac_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        ListLinhVucConID, phongBanXuLy, PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "11":
                    strReturn = Count_KhieuNaiBoPhanKhacChuyenVe(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "14":
                    strReturn = GetKhieuNaiSapQuaHan_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "17":
                    strReturn = Count_KhieuNaiQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "55":
                    strReturn = CountTongSoKhieuNai_WithPage(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
                case "60":
                    strReturn = GetKhieuNaiDaPhanHoi_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLy,
                        nSoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                    break;
            }
            //}

            context.Response.Write(strReturn);
        }

        private string CountTongSoKhieuNai_WithPage(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai,
            int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To,
            int NgayQuaHan_From, int NgayQuaHan_To, int NgayQuaHanPB_From, int NgayQuaHanPB_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex,
            string pageSize)
        {
            int dem = LinhVucConId.Split(',').Count();
            if (int.Parse(LinhVucChungId) < 1 && dem < 2)
            {
                return CountTongSoKhieuNai_WithPage1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
            else
            {
                return CountTongSoKhieuNai_WithPage2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
        }

        // Lĩnh vực chung đầu vào là xác định -> xác định được lĩnh vực con
        private string CountTongSoKhieuNai_WithPage1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_WithPage1(contentSeach,
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        // Lĩnh vực chung đầu vào là không xác định -> xác định được một list lĩnh vực con
        private string CountTongSoKhieuNai_WithPage2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_WithPage2(contentSeach,
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId, NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiChuyenBoPhanKhac_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            int dem = LinhVucConId.Split(',').Count();
            if (int.Parse(LinhVucChungId) < 1 && dem < 2)
            {
                return GetKhieuNaiChuyenBoPhanKhac_TotalRecords1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, phongBanXuLy, PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
            else
            {
                return GetKhieuNaiChuyenBoPhanKhac_TotalRecords2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, phongBanXuLy, PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
        }


        private string GetKhieuNaiChuyenBoPhanKhac_TotalRecords1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
    string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
    int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(phongBanXuLy),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiChuyenBoPhanKhac_TotalRecords2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
    string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
    int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(phongBanXuLy),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string Count_KhieuNaiBoPhanKhacChuyenVe(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            int dem = LinhVucConId.Split(',').Count();
            if (int.Parse(LinhVucChungId) < 1 && dem < 2)
            {
                return Count_KhieuNaiBoPhanKhacChuyenVe1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
            else
            {
                return Count_KhieuNaiBoPhanKhacChuyenVe2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
        }

        private string Count_KhieuNaiBoPhanKhacChuyenVe1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string Count_KhieuNaiBoPhanKhacChuyenVe2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_ChoXuLy_WithPage(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
                    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
                     int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            int dem = LinhVucConId.Split(',').Count();
            if (int.Parse(LinhVucChungId) < 1 && dem < 2)
            {
                return CountKhieuNai_ChoXuLy_WithPage1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
            else
            {
                return CountKhieuNai_ChoXuLy_WithPage2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
            }
        }


        private string CountKhieuNai_ChoXuLy_WithPage1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_ChoXuLy_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));                
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string CountKhieuNai_ChoXuLy_WithPage2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_ChoXuLy_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));                
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiSapQuaHan_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return GetKhieuNaiSapQuaHan_TotalRecords1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
                else
                {
                    return GetKhieuNaiSapQuaHan_TotalRecords2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiSapQuaHan_TotalRecords1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiSapQuaHan_TotalRecords2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string Count_KhieuNaiQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return Count_KhieuNaiQuaHan1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
                else
                {
                    return Count_KhieuNaiQuaHan2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
                        PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string Count_KhieuNaiQuaHan1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string Count_KhieuNaiQuaHan2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiDaPhanHoi_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    return GetKhieuNaiDaPhanHoi_TotalRecords1(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLy,
                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
                else
                {
                    return GetKhieuNaiDaPhanHoi_TotalRecords2(contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                        LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLy,
                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, ShowNguoiXuLy,
                        startPageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiDaPhanHoi_TotalRecords1(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNaiDaPhanHoi_TotalRecords2(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}