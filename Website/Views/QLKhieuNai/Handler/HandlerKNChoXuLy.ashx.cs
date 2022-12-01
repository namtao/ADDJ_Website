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
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerKNChoXuLy
    /// </summary>
    public class HandlerKNChoXuLy : IHttpHandler, IReadOnlySessionState
    {
        class DateKhieuNai
        {
            public int page { get; set; }
            public int total { get; set; }
            public List<DataItem> rows { get; set; }
        }

        class DataItem
        {
            public Int64 STT { get; set; }
            public string CheckAll { get; set; }
            public string Id { get; set; }
            public string DoUuTien { get; set; }
            public string TrangThai { get; set; }
            public string SoThueBao { get; set; }

            public string NoiDungPA { get; set; }
            public string LoaiKhieuNai { get; set; }
            public string LinhVucChung { get; set; }
            public string LinhVucCon { get; set; }

            public string PhongBanTiepNhan { get; set; }
            public string NguoiTiepNhan { get; set; }

            public string NguoiXuLyTruoc { get; set; }

            public string PhongBanXuLy { get; set; }
            public string NguoiXuLy { get; set; }
            public string NguoiDuocPhanHoi { get; set; }

            public string NguoiTienXuLyCap1 { get; set; }
            public string NguoiTienXuLyCap2 { get; set; }
            public string NguoiTienXuLyCap3 { get; set; }

            public string NgayTiepNhanSort { get; set; }
            public string NgayQuaHanSort { get; set; }
            public string NgayQuaHanPhongBanXuLySort { get; set; }

            public string IsKNGiamTru { get; set; }
            public int CallCount { get; set; }

            public string IsPhanViec { get; set; }
            public string LDate { get; set; }

        }

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
            if (!string.IsNullOrEmpty(context.Request.Form["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.Form["pageSize"]))
            {
                string typeSearch = context.Request.Form["typeSearch"];
                string doUuTien = context.Request.Form["doUuTien"];
                string trangThai = context.Request.Form["trangThai"];
                string loaiKhieuNai = context.Request.Form["loaiKhieuNai"];
                string linhVucChung = context.Request.Form["linhVucChung"];
                string tenLinhVucCon = context.Request.Form["linhVucCon"] ?? context.Request.QueryString["linhVucCon"];
                //if(tenLinhVucCon == "0")
                //{
                //    tenLinhVucCon = "";
                //}
                string phongBanXuLy = context.Request.Form["phongBanXuLy"].ToString();

                //LONGLX
                string ShowNguoiXuLy = context.Request.Form["ShowNguoiXuLy"];

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
                //END LONGLX

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

                if (tenLinhVucCon == "-1")
                {
                    tenLinhVucCon = "";
                }
                //string ListLinhVucConID = "";
                //if (tenLinhVucCon != "0" && tenLinhVucCon != "-1")
                //{
                //    // HaiPH 10/07/2014 : Khi chọn lĩnh vực con thì lọc theo loại khiếu nại và lĩnh vực chung đã chọn
                //    string whereClause = string.Empty;
                //    if(loaiKhieuNai != "-1")
                //    {
                //        whereClause = string.Format(" AND ParentLoaiKhieuNaiId={0}", loaiKhieuNai);
                //    }
                //    if(linhVucChung != "-1")
                //    {
                //        whereClause = string.Format("{0} AND ParentId={1}", whereClause, linhVucChung);
                //    }

                //    whereClause = string.Format("Name LIKE N'{0}' {1}", tenLinhVucCon, whereClause);

                //    var ListLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id", whereClause, "");
                //    for (int i = 0; i < ListLinhVucCon.Count; i++)
                //    {
                //        if (i < ListLinhVucCon.Count - 1)
                //        {
                //            ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id + ",";
                //        }
                //        else
                //        {
                //            ListLinhVucConID = ListLinhVucConID + ListLinhVucCon[i].Id;
                //        }
                //    }
                //}
                //else
                //{
                //    ListLinhVucConID = tenLinhVucCon;
                //}
                string sortName = context.Request.Form["sortname"];
                string sortOrder = context.Request.Form["sortorder"];

                string startPageIndex = context.Request.Form["startPageIndex"].ToString();
                string pageSize = context.Request.Form["pageSize"].ToString();

                switch (type)
                {
                    case "56":
                        //strReturn = GetTongSoKhieuNai_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        //    ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetTongSoKhieuNai_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            tenLinhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);

                        break;
                    case "4":
                        //strReturn = GetKhieuNai_ChoXuLy_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                        //    ListLinhVucConID, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetKhieuNai_ChoXuLy_WithPage(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            tenLinhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;
                    case "7":
                        //strReturn = GetHtmlKhieuNaiChuyenBoPhanKhac(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID, phongBanXuLy,
                        //    PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetHtmlKhieuNaiChuyenBoPhanKhac(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, tenLinhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;
                    case "12":
                        //strReturn = GetHtmlKhieuNaiBoPhanKhacChuyenVe(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        //    PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetHtmlKhieuNaiBoPhanKhacChuyenVe(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, tenLinhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;
                    case "15":
                        //strReturn = GetHtmlKhieuNaiSapQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        //    PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetHtmlKhieuNaiSapQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, tenLinhVucCon,
                            PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;
                    case "18":
                        //strReturn = GetHtmlKhieuNaiQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        //    PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                        //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetHtmlKhieuNaiQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, tenLinhVucCon,
                            PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;

                    case "61":
                        //strReturn = GetHtmlKhieuNaiDaPhanHoi(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, ListLinhVucConID,
                        //    PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
                        //    nSoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                        //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                        //    startPageIndex, pageSize, infoUser);
                        strReturn = GetHtmlKhieuNaiDaPhanHoi(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, tenLinhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                        break;
                }
            }

            context.Response.Write(strReturn);
        }
        #region Tất cả KN

        private string GetTongSoKhieuNai_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
           string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
          int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
          int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
          string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetTongSoKhieuNai_WithPage3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
                            tenLinhVucCon, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// Todo : Tìm kiếm theo tên lĩnh vực con
        /// </summary>

        private string GetTongSoKhieuNai_WithPage3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
           string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage3(contentSeach,
                                    Convert.ToInt32(LoaiKhieuNaiId),
                                    Convert.ToInt32(LinhVucChungId),
                                    tenLinhVucCon,
                                    Convert.ToInt32(PhongBanId),
                                    Convert.ToInt32(DoUuTien),
                                    Convert.ToInt32(trangThai),
                                    NguoiXuLyId,
                                    NguoiTienXuLyId,
                                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                                    Convert.ToInt32(startPageIndex),
                                    Convert.ToInt32(pageSize));

                //string linhVucConIdTemp = (LinhVucConId.Trim().Length > 0 && LinhVucConId.Split(',').Length > 0) ? LinhVucConId.Split(',')[0] : LinhVucConId;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab6-KNTongHopChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        //private string GetTongSoKhieuNai_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
        //    string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        //   int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //   int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetTongSoKhieuNai_WithPage1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
        //                    LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetTongSoKhieuNai_WithPage2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId,
        //                    LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return string.Empty;
        //    }
        //}
        // Lĩnh vực chung đầu vào là xác định -> xác định được lĩnh vực con
        private string GetTongSoKhieuNai_WithPage1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
           string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage1(contentSeach,
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));


                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab6-KNTongHopChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }
        // Lĩnh vực chung đầu vào là không xác định -> xác định được một list lĩnh vực con
        private string GetTongSoKhieuNai_WithPage2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage2(contentSeach,
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                //string linhVucConIdTemp = (LinhVucConId.Trim().Length > 0 && LinhVucConId.Split(',').Length > 0) ? LinhVucConId.Split(',')[0] : LinhVucConId;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab6-KNTongHopChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }
        #endregion

        #region KN chờ xử lý

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// Todo : Tìm kiếm theo tên lĩnh vực con
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="LinhVucConId"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetKhieuNai_ChoXuLy_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetKhieuNai_ChoXuLy_WithPage3(context, contentSeach, TypeSearch, LoaiKhieuNaiId,
                       LinhVucChungId,
                       tenLinhVucCon, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                       SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                       NgayQuaHan_From, NgayQuaHan_To, NgayQuaHanPB_From, NgayQuaHanPB_To,
                       KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                       startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// Todo : Tìm kiếm theo tên lĩnh vực con
        /// </summary>
        /// <param name="context"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetKhieuNai_ChoXuLy_WithPage3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    tenLinhVucCon,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        //private string GetKhieuNai_ChoXuLy_WithPage(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
        //    string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        //   int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //   int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetKhieuNai_ChoXuLy_WithPage1(context, contentSeach, TypeSearch, LoaiKhieuNaiId,
        //                LinhVucChungId,
        //                LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
        //                NgayQuaHan_From, NgayQuaHan_To,
        //                KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetKhieuNai_ChoXuLy_WithPage2(context, contentSeach, TypeSearch, LoaiKhieuNaiId,
        //                LinhVucChungId,
        //                LinhVucConId, PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
        //                NgayQuaHan_From, NgayQuaHan_To,
        //                KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                startPageIndex, pageSize, infoUser);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return string.Empty;
        //    }
        //}

        private string GetKhieuNai_ChoXuLy_WithPage1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetKhieuNai_ChoXuLy_WithPage2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
           string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNChoXuLy" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN chuyển bộ phận khác

        private string GetHtmlKhieuNaiChuyenBoPhanKhac(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetHtmlKhieuNaiChuyenBoPhanKhac3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, phongBanXuLy,
                            PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        private string GetHtmlKhieuNaiChuyenBoPhanKhac3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   tenLinhVucCon,
                   Convert.ToInt32(phongBanXuLy),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   NguoiTienXuLyId,
                   SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, NgayQuaHanPB_From, NgayQuaHanPB_To,
                   KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNChuyenBoPhanKhac" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        //private string GetHtmlKhieuNaiChuyenBoPhanKhac(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
        //    string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetHtmlKhieuNaiChuyenBoPhanKhac1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, phongBanXuLy,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetHtmlKhieuNaiChuyenBoPhanKhac2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, phongBanXuLy,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "";
        //    }
        //}

        private string GetHtmlKhieuNaiChuyenBoPhanKhac1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
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
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNChuyenBoPhanKhac" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        private string GetHtmlKhieuNaiChuyenBoPhanKhac2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
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
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNChuyenBoPhanKhac" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        #endregion

        #region KN Bộ phận khác chuyển về

        private string GetHtmlKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string contentSeach, string TypeSearch,
            string LoaiKhieuNaiId, string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetHtmlKhieuNaiBoPhanKhacChuyenVe3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon,
                            PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        private string GetHtmlKhieuNaiBoPhanKhacChuyenVe3(HttpContext context, string contentSeach, string TypeSearch,
           string LoaiKhieuNaiId, string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
           string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   tenLinhVucCon,
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLyId,
                   NguoiTienXuLyId,
                   SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab3-KNBoPhanKhacChuyenVe" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        //private string GetHtmlKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string contentSeach, string TypeSearch,
        //    string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetHtmlKhieuNaiBoPhanKhacChuyenVe1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetHtmlKhieuNaiBoPhanKhacChuyenVe2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "";
        //    }
        //}

        private string GetHtmlKhieuNaiBoPhanKhacChuyenVe1(HttpContext context, string contentSeach, string TypeSearch,
            string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   Convert.ToInt32(LinhVucConId),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLyId,
                   NguoiTienXuLyId,
                   SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab3-KNBoPhanKhacChuyenVe" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        private string GetHtmlKhieuNaiBoPhanKhacChuyenVe2(HttpContext context, string contentSeach, string TypeSearch,
            string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   LinhVucConId,
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLyId,
                   NguoiTienXuLyId,
                   SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab3-KNBoPhanKhacChuyenVe" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        #endregion

        #region KN sắp quá hạn

        private string GetHtmlKhieuNaiSapQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetHtmlKhieuNaiSapQuaHan3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon,
                            PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtmlKhieuNaiSapQuaHan3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
           string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
           string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var dtResult = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding3(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    tenLinhVucCon,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                var lstResult = ServiceFactory.GetInstanceKhieuNai().ToCollection(dtResult);

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab4-KNSapQuaHan" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        //private string GetHtmlKhieuNaiSapQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
        //    string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetHtmlKhieuNaiSapQuaHan1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetHtmlKhieuNaiSapQuaHan2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "-1";
        //    }
        //}

        private string GetHtmlKhieuNaiSapQuaHan1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding1(contentSeach, Convert.ToInt32(TypeSearch),
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

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab4-KNSapQuaHan" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                //result.total = lstResult.Count;
                //result.page = Convert.ToInt32(startPageIndex);
                //result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtmlKhieuNaiSapQuaHan2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding2(contentSeach, Convert.ToInt32(TypeSearch),
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

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab4-KNSapQuaHan" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                //result.total = lstResult.Count;
                //result.page = Convert.ToInt32(startPageIndex);
                //result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion

        #region KN quá hạn

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetHtmlKhieuNaiQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetHtmlKhieuNaiQuaHan3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon,
                            PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetHtmlKhieuNaiQuaHan3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string tenLinhVucCon, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                List<KhieuNaiInfo> lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    tenLinhVucCon,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab5-KNCanDong" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        //private string GetHtmlKhieuNaiQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
        //    string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetHtmlKhieuNaiQuaHan1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetHtmlKhieuNaiQuaHan2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, NguoiXuLyId, NguoiTienXuLyId,
        //                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "-1";
        //    }
        //}

        private string GetHtmlKhieuNaiQuaHan1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab5-KNCanDong" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtmlKhieuNaiQuaHan2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    LinhVucConId,
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLyId,
                    NguoiTienXuLyId,
                    SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));

                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab5-KNCanDong" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion

        #region KN đã phản hồi

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="NguoiTienXuLy"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetHtmlKhieuNaiDaPhanHoi(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                return GetHtmlKhieuNaiDaPhanHoi3(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon,
                            PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
                            SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                            NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="NguoiTienXuLy"></param>
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
        /// <param name="ShowNguoiXuLy"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="infoUser"></param>
        /// <returns></returns>
        private string GetHtmlKhieuNaiDaPhanHoi3(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string tenLinhVucCon, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage3(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   tenLinhVucCon,
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                int temp = 0;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, tenLinhVucCon, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNDaPhanHoi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        //private string GetHtmlKhieuNaiDaPhanHoi(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
        //    string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
        //    int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //{
        //    try
        //    {
        //        int dem = LinhVucConId.Split(',').Count();
        //        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
        //        {
        //            return GetHtmlKhieuNaiDaPhanHoi1(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
        //                    SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //        else
        //        {
        //            return GetHtmlKhieuNaiDaPhanHoi2(context, contentSeach, TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId,
        //                    PhongBanId, DoUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
        //                    SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
        //                    KNHangLoat, GetAllKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
        //                    startPageIndex, pageSize, infoUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "-1";
        //    }
        //}

        private string GetHtmlKhieuNaiDaPhanHoi1(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   Convert.ToInt32(LinhVucConId),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                int temp = 0;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNDaPhanHoi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtmlKhieuNaiDaPhanHoi2(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy,
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            try
            {
                var lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   LinhVucConId,
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, sortName, sortOrder,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));

                int temp = 0;
                string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}&QHPBTu={17}&QHPBDen={18}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy, NgayQuaHanPB_From, NgayQuaHanPB_To);
                strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNDaPhanHoi" + strReturnURL;
                strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                DateKhieuNai result = new DateKhieuNai();
                result.total = lstResult.Count;
                result.page = Convert.ToInt32(startPageIndex);
                result.rows = BindResultToDataItem(lstResult, infoUser, strReturnURL);

                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Bind Data

        private List<DataItem> BindResultToDataItem(List<KhieuNaiInfo> lstResult, AdminInfo infoUser, string strReturnURL)
        {
            List<DataItem> lstRow = new List<DataItem>();

            if (lstResult != null && lstResult.Count > 0)
            {
                foreach (var item in lstResult)
                {
                    DataItem itemKN = new DataItem();

                    itemKN.STT = item.STT;

                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.PhongBanXuLyId != infoUser.PhongBanId || item.NguoiXuLy != infoUser.Username)
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" type =\"checkbox\" disabled=\"disabled\" />";
                        }
                        else
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" value=\"" + item.Id + "\" id =\"checkbox" + item.Id + "\" type =\"checkbox\" />";
                        }
                    }
                    else
                    {
                        if (item.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" value=\"" + item.Id + "\" id =\"checkbox" + item.Id + "\" type =\"checkbox\" />";
                        }
                        else
                        {
                            itemKN.CheckAll = "<input class=\"checkbox-item\" name=\"item\" type =\"checkbox\" disabled=\"disabled\" />";
                        }
                    }

                    itemKN.TrangThai = BindTinhTrangXuLy(item.TrangThai, item.IsPhanHoi, item.NgayQuaHanPhongBanXuLy);

                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.PhongBanXuLyId != infoUser.PhongBanId || item.NguoiXuLy != infoUser.Username)
                        {
                            itemKN.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            itemKN.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }
                    else
                    {
                        if (item.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            itemKN.Id = "<a href=\"javascript:CheckXuLyKhieuNai(" + item.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&Mode=Process','" + strReturnURL + "')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                        else
                        {
                            itemKN.Id = "<a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, item.Id, 10) + "</a>";
                        }
                    }

                    itemKN.DoUuTien = Enum.GetName(typeof(KhieuNai_DoUuTien_Type), item.DoUuTien).Replace("_", " ");
                    itemKN.SoThueBao = "<a class='ShowChiTiet_" + item.Id + "' href=\"javascript:ShowPoupChiTietKN('" + item.Id + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + item.SoThueBao + "</a>";
                    itemKN.LoaiKhieuNai = item.LoaiKhieuNai;
                    itemKN.LinhVucChung = item.LinhVucChung;
                    itemKN.LinhVucCon = item.LinhVucCon;
                    itemKN.NoiDungPA = item.NoiDungPA;
                    itemKN.PhongBanTiepNhan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanTiepNhanId);
                    itemKN.PhongBanXuLy = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                    itemKN.NguoiTienXuLyCap1 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap1 + "'>" + item.NguoiTienXuLyCap1 + "</a>";
                    itemKN.NguoiTienXuLyCap2 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap2 + "'>" + item.NguoiTienXuLyCap2 + "</a>";
                    itemKN.NguoiTienXuLyCap3 = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTienXuLyCap3 + "'>" + item.NguoiTienXuLyCap3 + "</a>";
                    itemKN.NguoiTiepNhan = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiTiepNhan + "'>" + item.NguoiTiepNhan + "</a>";
                    itemKN.NguoiXuLy = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiXuLy + "'>" + item.NguoiXuLy + "</a>";
                    if (string.IsNullOrEmpty(item.NguoiDuocPhanHoi))
                        itemKN.NguoiDuocPhanHoi = string.Empty;
                    else
                        itemKN.NguoiDuocPhanHoi = "<a href=\"#\" class=\"normalTip exampleTip\" title='" + item.NguoiDuocPhanHoi + "'>" + item.NguoiDuocPhanHoi + "</a>";
                    if (item.IsPhanViec)
                    {
                        itemKN.IsPhanViec = "<input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\">";
                    }
                    else
                    {
                        itemKN.IsPhanViec = "";
                    }
                    itemKN.NgayTiepNhanSort = item.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.NgayQuaHanSort = item.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.NgayQuaHanPhongBanXuLySort = item.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.CallCount = item.CallCount;
                    itemKN.LDate = item.LDate.ToString("dd/MM/yyyy HH:mm:ss");
                    itemKN.IsKNGiamTru = "<input type=\"checkbox\" disabled=\"disabled\" " + (item.IsKNGiamTru ? "checked =\"yes\"" : "") + ">";
                    lstRow.Add(itemKN);
                }
            }
            return lstRow;
        }

        protected string BindTinhTrangXuLy(object obj, bool isPhanHoi, DateTime NgayQuaHanPhongBan)
        {
            if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Đóng)
                return string.Format("<span style='border: 1pt solid #CCC; background: green; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
            else
            {
                if (isPhanHoi)
                {
                    return string.Format("<span style='border: 1pt solid #CCC; background: #FF8000; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
                else if (NgayQuaHanPhongBan <= DateTime.Now)
                {
                    return string.Format("<span style='border: 1pt solid #CCC; background: #999; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
                else
                {
                    if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
                        return string.Format("<span style='border: 1pt solid #CCC; background: red; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                    else if (Convert.ToByte(obj) == (byte)KhieuNai_TrangThai_Type.Chờ_đóng)
                        return string.Format("<span style='border: 1pt solid #CCC; background: #0095CC; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                    else
                        return string.Format("<span style='border: 1pt solid #CCC; background: yellow; width: 15px; height: 10px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>", "");
                }
            }
        }

        protected string BindMaKN(object obj)
        {
            return string.Format("<a href=\"javascript:ShowPoupChiTietKN('{0}');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">{1}</a>", obj, GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, obj, 10));
        }

        protected string BindNgayDong(object trangthai, object ngaydong)
        {
            if ((int)KhieuNai_TrangThai_Type.Đóng == Convert.ToInt32(trangthai))
            {
                return ngaydong.ToString();
            }
            return string.Empty;
        }

        protected string BindDoUuTien(object obj)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(obj)).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        protected string BindHTTiepNhan(object obj)
        {
            try
            {
                return Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), Convert.ToByte(obj)).Replace("_", " ");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return string.Empty;
            }
        }

        protected string BindBooleanToCheckbox(string id, bool flag, bool readOnly)
        {
            return string.Format("<input id='{0}' type='checkbox' {1} {2}/>", id, flag ? "checked='checked'" : "", readOnly ? "disabled='disabled'" : "");
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}