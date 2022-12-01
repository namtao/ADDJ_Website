using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using System.Data;
using AIVietNam.GQKN.Impl;
using Aspose.Cells;
using System.Drawing;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.Admin;
using AIVietNam.GQKN.Entity;
using System.IO;
using System.Globalization;
using Website.AppCode.Controller;
using System.Text;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for ExportExcel
    /// </summary>
    /// 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportExcel : IHttpHandler, IReadOnlySessionState
    {

        KhieuNaiImpl _KhieuNaiImpl = new KhieuNaiImpl();
        private const int PageSizeExcel = 50;

        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request.QueryString["key"].ToString();
            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";

            if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            {
                AdminInfo infoUser = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
                if (infoUser != null)
                {
                    context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
                }
            }
        }

        private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        {
            string strValue = "";
            switch (key)
            {
                //case "1": //Khieu Nai Cho Xu Ly
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string trangThai = context.Request.QueryString["trangThai"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        //string soThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        //string nguoiXuly = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        //string nguoiTiepnhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
                //        //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
                //        //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
                //        //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiChuaXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //case "2": //Khieu Nai Chuyen Bo Phan Khac
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string trangThai = context.Request.QueryString["trangThai"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                //        string phongBanXuLy = context.Request.QueryString["phongBanXuLy"].ToString();
                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiChuyenBoPhanKhac(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                //            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //case "3": //Khieu Nai Bo phan khac chuyen ve
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string trangThai = context.Request.QueryString["trangThai"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiBoPhanKhacChuyenVe(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //case "4": //Khieu Nai Sap Qua Han
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX


                //        strValue = ExportKhieuNaiSapQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //case "5": //Khieu Nai Qua Han
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                case "6": //Khieu Nai phan viec
                    string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = "-1";
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = ExportKhieuNaiPhanViec(typeKhieuNai, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                    }

                    break;

                case "7": //Khieu Nai so theo doi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string select = context.Request.QueryString["select"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();////???????
                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                        string ThoiGianTiepNhanTu = context.Request.QueryString["ThoiGianTiepNhanTu"].ToString();
                        string ThoiGianTiepNhanDen = context.Request.QueryString["ThoiGianTiepNhanDen"].ToString();

                        string PhongBanXuLy = context.Request.QueryString["PhongBanXuLy"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        bool IsTatCaKN = false;

                        string PhongBanId = infoUser.PhongBanId.ToString();
                        string NguoiXuLy = "";
                        bool isPermission = false;
                        switch (typeSearch)
                        {
                            case "-2":

                                IsTatCaKN = true;
                                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                                break;
                            case "-1":
                                NguoiXuLy = infoUser.Username;
                                break;

                            default:
                                PhongBanId = typeSearch;
                                break;
                        }

                        strValue = ExportKhieuNaiSoTheoDoi(select, PhongBanId, SoThueBao, NguoiXuLy, NguoiTiepNhan, ThoiGianTiepNhanTu,
                            ThoiGianTiepNhanDen, IsTatCaKN, infoUser.DoiTacId, isPermission, startPageIndex, pageSize);
                    }

                    break;
                //case "8": //Khieu Nai Cho Xu Ly Tong hop
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string trangThai = context.Request.QueryString["trangThai"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiTongHopChoXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //#region Export Excel KhieuNai Da Phan Hoi
                //case "9": //Khieu Nai Chuyen Bo Phan Khac
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                //    {
                //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                //        string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
                //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                //        string trangThai = context.Request.QueryString["trangThai"].ToString();
                //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                //        //LONGLX
                //        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                //        string NguoiXuLy_Default = "-1";
                //        bool IsTatCaKN = false;
                //        int KNHangLoat = 0;
                //        string PhongBanId = infoUser.PhongBanId.ToString();
                //        bool isPermission = false;
                //        if (ShowNguoiXuLy.Equals("1"))
                //        {
                //            NguoiXuLy_Default = "";
                //        }

                //        switch (typeSearch)
                //        {
                //            case "-2":
                //                KNHangLoat = -1;
                //                IsTatCaKN = true;
                //                isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
                //                break;
                //            case "-1":
                //                KNHangLoat = -1;
                //                NguoiXuLy_Default = infoUser.Username;
                //                break;
                //            case "0":
                //                KNHangLoat = 1;
                //                break;

                //            default:
                //                PhongBanId = typeSearch;
                //                break;
                //        }

                //        if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
                //        {
                //            contentSeach = string.Empty;
                //        }

                //        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                //        int nSoThueBao = -1;
                //        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                //        {
                //            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                //        }

                //        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
                //        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                //        {
                //            NguoiTiepNhan = string.Empty;
                //        }
                //        string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
                //        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                //        {
                //            NguoiXuLy = string.Empty;
                //        }

                //        string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
                //        int nNgayTiepNhan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
                //        int nNgayTiepNhan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
                //        int nNgayQuaHan_From = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }

                //        string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
                //        int nNgayQuaHan_To = -1;
                //        if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
                //        {
                //            try
                //            {
                //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                //            }
                //            catch { }
                //        }
                //        //END LONGLX

                //        strValue = ExportKhieuNaiDaPhanHoi(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                //            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                //            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                //            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
                //            startPageIndex, pageSize);
                //    }
                //    break;
                //#endregion
            }
            return strValue;
        }



        #region Process

        private string GetListFileDinhKem(string id, int Status)
        {
            try
            {
                string strValues = "";
                KhieuNai_FileDinhKemImpl _KhieuNai_FileDinhKemImpl = new KhieuNai_FileDinhKemImpl();
                List<KhieuNai_FileDinhKemInfo> list = _KhieuNai_FileDinhKemImpl.GetListByKhieuNaiId(Convert.ToInt32(id));
                if (list.Count > 0)
                {
                    string domainDownload = AIVietNam.Core.Config.DomainDownload;
                    if (Status == (int)FileDinhKem_Status.File_KH_Gửi)
                    {
                        var results = list.Where(s => s.Status == (int)FileDinhKem_Status.File_KH_Gửi).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var info in results)
                            {
                                strValues += info.TenFile + ",";
                            }
                        }
                    }
                    else
                    {
                        var results = list.Where(s => s.Status == (int)FileDinhKem_Status.File_GQKN_Gửi).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var info in results)
                            {
                                strValues += info.TenFile + ",";
                            }
                        }
                    }
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private Aspose.Cells.Style StyleCell()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cell cell = worksheet.Cells["A1"];
            Aspose.Cells.Style style = cell.GetStyle();

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Black;
            return style;
        }

        private int AddContentToSheetSoTheoDoi(DataTable tab, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (DataRow row in tab.Rows)
                {
                    sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 3].PutValue(row["NguoiTiepNhan"]);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 4].PutValue(row["SoThueBao"]);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 5].PutValue(row["HoTenLienHe"]);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 6].PutValue(row["NoiDungPA"]);
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(GetListFileDinhKem(row["ID"].ToString(), (int)FileDinhKem_Status.File_KH_Gửi));
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 8].PutValue(row["NoiDungXuLy"]);
                    sheet.Cells[RowIndex, 8].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 9].PutValue(ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(row["PhongBanXuLyId"])).Name);
                    sheet.Cells[RowIndex, 9].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 10].PutValue(Convert.ToDateTime(row["NgayTraLoiKN"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 10].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 11].PutValue(row["KetQuaXuLy"]);
                    sheet.Cells[RowIndex, 11].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 12].PutValue(row["GhiChu"]);
                    sheet.Cells[RowIndex, 12].SetStyle(StyleCell());


                    RowIndex++;


                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        private int AddContentToSheet(DataTable tab, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (DataRow row in tab.Rows)
                {
                    sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 1].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(row["TrangThai"])).Replace("_", " "));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 2].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 3].PutValue(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(row["DoUuTien"])).Replace("_", " "));
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 4].PutValue(row["SoThueBao"]);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 5].PutValue(row["LoaiKhieuNai"]);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 6].PutValue(row["LinhVucChung"]);
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(row["LinhVucCon"]);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 8].PutValue(row["NoiDungPA"]);
                    sheet.Cells[RowIndex, 8].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 9].PutValue(ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(row["PhongBanXuLyId"])).Name);
                    sheet.Cells[RowIndex, 9].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 10].PutValue(row["NguoiTiepNhan"]);
                    sheet.Cells[RowIndex, 10].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 11].PutValue(row["NguoiXuLy"]);
                    sheet.Cells[RowIndex, 11].SetStyle(StyleCell());

                    //sheet.Cells[RowIndex, 12].PutValue(row["NguoiTienXuLyCap1"]);
                    //sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 12].PutValue(Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 13].PutValue(Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 13].SetStyle(StyleCell());

                    //sheet.Cells[RowIndex, 14].PutValue(Convert.ToDateTime(row["NgayQuaHan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    //sheet.Cells[RowIndex, 14].SetStyle(StyleCell());

                    RowIndex++;


                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        private int AddContentToSheet_QuaHan(DataTable tab, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (DataRow row in tab.Rows)
                {
                    sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 1].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(row["TrangThai"])).Replace("_", " "));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 2].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 3].PutValue(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(row["DoUuTien"])).Replace("_", " "));
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 4].PutValue(row["SoThueBao"]);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 5].PutValue(row["LoaiKhieuNai"]);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 6].PutValue(row["LinhVucChung"]);
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(row["LinhVucCon"]);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 8].PutValue(row["NoiDungPA"]);
                    sheet.Cells[RowIndex, 8].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 9].PutValue(ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(row["PhongBanXuLyId"])).Name);
                    sheet.Cells[RowIndex, 9].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 10].PutValue(row["NguoiTiepNhan"]);
                    sheet.Cells[RowIndex, 10].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 11].PutValue(row["NguoiXuLy"]);
                    sheet.Cells[RowIndex, 11].SetStyle(StyleCell());

                    //sheet.Cells[RowIndex, 12].PutValue(row["NguoiTienXuLyCap1"]);
                    //sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 12].PutValue(Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 12].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 13].PutValue(Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 13].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 14].PutValue(Convert.ToDateTime(row["NgayQuaHan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 14].SetStyle(StyleCell());

                    RowIndex++;


                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

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
        #endregion

        #region KN Cho Xu Ly Tong Hop

        private string ExportKhieuNaiChuaXuLyTongHop(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyTongHop_GetAllWithPadding_TotalRecords(contentSeach,
                Convert.ToInt32(LoaiKhieuNaiId),
                Convert.ToInt32(LinhVucChungId),
                Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId),
                Convert.ToInt32(DoUuTien),
                Convert.ToInt32(trangThai),
                NguoiXuLy,
                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                Convert.ToInt32(startPageIndex),
                Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " chờ xử lý");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyTongHop_GetAllWithPadding(contentSeach,
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLy,
                                                                                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                                                                                i,
                                                                                                Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_ChuaXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }

        }

        #endregion

        #region KN Cho Xu Ly

        private string ExportKhieuNaiChuaXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLy_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                Convert.ToInt32(LoaiKhieuNaiId),
                Convert.ToInt32(LinhVucChungId),
                Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId),
                Convert.ToInt32(DoUuTien),
                Convert.ToInt32(trangThai),
                NguoiXuLy,
                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                Convert.ToInt32(startPageIndex),
                Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " chờ xử lý");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLy_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLy,
                                                                                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                                                                                i,
                                                                                                Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_ChuaXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }

        }

        #endregion

        #region KN ChuyenBoPhanKhac

        private string ExportKhieuNaiChuyenBoPhanKhac(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
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

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " chuyển bộ phận khác");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        //DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(phongBanXuLy),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        Convert.ToInt32(trangThai),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i,
                        //                                                                        Convert.ToInt32(pageSize));
                        //RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_ChuyenBoPhanKhac" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }

        }

        #endregion

        #region KN BoPhanKhacChuyenVe

        private string ExportKhieuNaiBoPhanKhacChuyenVe(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                Convert.ToInt32(LoaiKhieuNaiId),
                Convert.ToInt32(LinhVucChungId),
                Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId),
                Convert.ToInt32(DoUuTien),
                Convert.ToInt32(trangThai),
                NguoiXuLy,
                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                Convert.ToInt32(startPageIndex),
                Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " bộ phận khác chuyển về");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLy,
                                                                                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                                                                                i, Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_BoPhanKhacChuyenVe" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN SapQuaHan

        private string ExportKhieuNaiSapQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = 0;
                if (int.Parse(LinhVucChungId) > 0)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
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
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch),
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
                }


                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " sắp quá hạn");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable lstResult = new DataTable();
                        if (int.Parse(LinhVucChungId) > 0)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding1(contentSeach, Convert.ToInt32(TypeSearch),
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

                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHan_GetAllWithPadding2(contentSeach, Convert.ToInt32(TypeSearch),
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

                        }
                        RowIndex = AddContentToSheet(lstResult, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_SapQuaHan" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN QuaHan

        private string ExportKhieuNaiQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai_QuaHan.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiQuaHan_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                Convert.ToInt32(LoaiKhieuNaiId),
                Convert.ToInt32(LinhVucChungId),
                Convert.ToInt32(LinhVucConId),
                Convert.ToInt32(PhongBanId),
                Convert.ToInt32(DoUuTien),
                NguoiXuLy,
                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                Convert.ToInt32(startPageIndex),
                Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " quá hạn");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiQuaHan_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                NguoiXuLy,
                                                                                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                                                                                i,
                                                                                                Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet_QuaHan(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_QuaHan" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN DaPhanHoi

        private string ExportKhieuNaiDaPhanHoi(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = 0;
                if (int.Parse(LinhVucChungId) > 0)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        Convert.ToInt32(LinhVucConId),
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLy,
                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        LinhVucConId,
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLy,
                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }

                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " chuyển bộ phận khác");
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        DataTable tab = new DataTable();
                        if (int.Parse(LinhVucChungId) > 0)
                        {
                            tab = _KhieuNaiImpl.QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding1(contentSeach,
                                Convert.ToInt32(TypeSearch),
                                Convert.ToInt32(LoaiKhieuNaiId),
                                Convert.ToInt32(LinhVucChungId),
                                Convert.ToInt32(LinhVucConId),
                                Convert.ToInt32(PhongBanId),
                                Convert.ToInt32(DoUuTien),
                                Convert.ToInt32(trangThai),
                                NguoiXuLy,
                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                                NgayQuaHan_From, NgayQuaHan_To,
                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                i,
                                Convert.ToInt32(pageSize));
                        }
                        else
                        {
                            tab = _KhieuNaiImpl.QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding2(contentSeach,
                                Convert.ToInt32(TypeSearch),
                                Convert.ToInt32(LoaiKhieuNaiId),
                                Convert.ToInt32(LinhVucChungId),
                                LinhVucConId,
                                Convert.ToInt32(PhongBanId),
                                Convert.ToInt32(DoUuTien),
                                Convert.ToInt32(trangThai),
                                NguoiXuLy,
                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To,
                                NgayQuaHan_From, NgayQuaHan_To,
                                KNHangLoat, GetAllKN, DoiTacId, isPermission,
                                i,
                                Convert.ToInt32(pageSize));

                        }
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_DaPhanHoi" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN PhanViec

        private string ExportKhieuNaiPhanViec(string typeKhieuNai, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string TrangThai, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNai.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5; int TotalRecords = 0;
                string title = "";
                DataTable tab = new DataTable();

                //Save the Excel file.

                switch (typeKhieuNai)
                {
                    case "1":
                        title = "chờ xử lý";
                        TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords
                        (Convert.ToInt32(TypeSearch),
                        Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        Convert.ToInt32(LinhVucConId),
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(TrangThai),
                        NguoiXuLy,
                        Convert.ToInt32(startPageIndex),
                        Convert.ToInt32(pageSize));
                        break;
                    case "2":
                        title = "bộ phận khác chuyển về";
                        TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding_TotalRecords
                             (Convert.ToInt32(TypeSearch),
                             Convert.ToInt32(LoaiKhieuNaiId),
                             Convert.ToInt32(LinhVucChungId),
                             Convert.ToInt32(LinhVucConId),
                             Convert.ToInt32(PhongBanId),
                             Convert.ToInt32(DoUuTien),
                             Convert.ToInt32(TrangThai),
                             NguoiXuLy,
                             Convert.ToInt32(startPageIndex),
                             Convert.ToInt32(pageSize));
                        break;
                    case "3":
                        title = "sắp quá hạn";
                        TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding_TotalRecords
                             (Convert.ToInt32(TypeSearch),
                             Convert.ToInt32(LoaiKhieuNaiId),
                             Convert.ToInt32(LinhVucChungId),
                             Convert.ToInt32(LinhVucConId),
                             Convert.ToInt32(PhongBanId),
                             Convert.ToInt32(DoUuTien),
                             NguoiXuLy,
                             Convert.ToInt32(startPageIndex),
                             Convert.ToInt32(pageSize));
                        break;
                    case "4":
                        title = "quá hạn";
                        TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding_TotalRecords
                              (Convert.ToInt32(TypeSearch),
                              Convert.ToInt32(LoaiKhieuNaiId),
                              Convert.ToInt32(LinhVucChungId),
                              Convert.ToInt32(LinhVucConId),
                              Convert.ToInt32(PhongBanId),
                              Convert.ToInt32(DoUuTien),
                              NguoiXuLy,
                              Convert.ToInt32(startPageIndex),
                              Convert.ToInt32(pageSize));
                        break;
                }


                if (TotalRecords > 0)
                {
                    if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                    {
                        sheet.Cells[0, 0].PutValue("Danh sách phân việc khiếu nại " + title);
                        Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                        style.IsTextWrapped = true;
                        sheet.Cells[0, 0].SetStyle(style);
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        switch (typeKhieuNai)
                        {
                            case "1":

                                tab = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding
                                (Convert.ToInt32(TypeSearch),
                                Convert.ToInt32(LoaiKhieuNaiId),
                                Convert.ToInt32(LinhVucChungId),
                                Convert.ToInt32(LinhVucConId),
                                Convert.ToInt32(PhongBanId),
                                Convert.ToInt32(DoUuTien),
                                Convert.ToInt32(TrangThai),
                                NguoiXuLy,
                                i,
                                Convert.ToInt32(pageSize));
                                break;
                            case "2":

                                tab = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding
                                     (Convert.ToInt32(TypeSearch),
                                     Convert.ToInt32(LoaiKhieuNaiId),
                                     Convert.ToInt32(LinhVucChungId),
                                     Convert.ToInt32(LinhVucConId),
                                     Convert.ToInt32(PhongBanId),
                                     Convert.ToInt32(DoUuTien),
                                     Convert.ToInt32(TrangThai),
                                     NguoiXuLy,
                                     i,
                                     Convert.ToInt32(pageSize));
                                break;
                            case "3":

                                tab = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding
                                     (Convert.ToInt32(TypeSearch),
                                     Convert.ToInt32(LoaiKhieuNaiId),
                                     Convert.ToInt32(LinhVucChungId),
                                     Convert.ToInt32(LinhVucConId),
                                     Convert.ToInt32(PhongBanId),
                                     Convert.ToInt32(DoUuTien),
                                     NguoiXuLy,
                                     i,
                                     Convert.ToInt32(pageSize));
                                break;
                            case "4":

                                tab = _KhieuNaiImpl.QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding
                                      (Convert.ToInt32(TypeSearch),
                                      Convert.ToInt32(LoaiKhieuNaiId),
                                      Convert.ToInt32(LinhVucChungId),
                                      Convert.ToInt32(LinhVucConId),
                                      Convert.ToInt32(PhongBanId),
                                      Convert.ToInt32(DoUuTien),
                                      NguoiXuLy,
                                      i,
                                      Convert.ToInt32(pageSize));
                                break;
                        }
                        RowIndex = AddContentToSheet(tab, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNai_PhanViec" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
            }
            catch
            {
                return "";
            }
            return strValue;
        }

        #endregion

        #region KN SoTheoDoi

        private string ExportKhieuNaiSoTheoDoi(string Select, string PhongBanId, string SoThueBao, string NguoiXuLy, string NguoiTiepNhan, string ThoiGianTiepNhanTu, string ThoiGianTiepNhanDen,
            bool GetAllKN, int DoiTacId, bool IsPermission, string startPageIndex, string pageSize)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                Workbook workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNaiSotheoDoi.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int thoiGianTu = -1;
                int thoiGianDem = -1;
                if (!string.IsNullOrEmpty(ThoiGianTiepNhanTu))
                {
                    var listItemDate = ThoiGianTiepNhanTu.Split('/');
                    DateTime ngayTiepNhan = Convert.ToDateTime(listItemDate[1] + "/" + listItemDate[0] + "/" + listItemDate[2]);

                    thoiGianTu = Convert.ToInt32(ngayTiepNhan.ToString("yyyyMMdd"));
                }
                if (!string.IsNullOrEmpty(ThoiGianTiepNhanDen))
                {
                    var listItemDate = ThoiGianTiepNhanDen.Split('/');
                    DateTime ngayTiepNhan = Convert.ToDateTime(listItemDate[1] + "/" + listItemDate[0] + "/" + listItemDate[2]);
                    thoiGianDem = Convert.ToInt32(ngayTiepNhan.ToString("yyyyMMdd"));
                }
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding_TotalRecords(Convert.ToInt32(Select),
                Convert.ToInt32(PhongBanId),
                Convert.ToInt64(SoThueBao),
                NguoiXuLy,
                NguoiTiepNhan,
                thoiGianTu,
                thoiGianDem, GetAllKN, DoiTacId, IsPermission,
                Convert.ToInt32(startPageIndex),
                Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    if (TotalRecords > 60000)
                    {
                        return "-1";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                        {
                            sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + " từ ngày " + ThoiGianTiepNhanTu + " đến ngày " + ThoiGianTiepNhanDen);
                            Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                            style.IsTextWrapped = true;
                            sheet.Cells[0, 0].SetStyle(style);
                        }
                        int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                        for (int i = 1; i <= totalPage; i++)
                        {
                            DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding(Convert.ToInt32(Select),
                            Convert.ToInt32(PhongBanId),
                            Convert.ToInt64(SoThueBao),
                            NguoiXuLy,
                            NguoiTiepNhan,
                            thoiGianTu,
                            thoiGianDem, GetAllKN, DoiTacId, IsPermission,
                            i,
                            Convert.ToInt32(pageSize));
                            RowIndex = AddContentToSheetSoTheoDoi(tab, RowIndex, sheet);
                        }
                    }

                }

                string fileName = "DanhSachKhieuNai_SoTheoDoi" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString();
                    pathChild += "/" + DateTime.Now.Year.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString();
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString();
                    pathChild += "/" + DateTime.Now.Day.ToString();
                }

                pathSave += "\\" + fileName;
                workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

                #endregion
                strValue = pathChild + "/" + fileName;
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Longlx Get HTML Excel

        #region KN Cho Xu Ly Tong Hop

//        private string ExportKhieuNaiTongHopChoXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
//            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
//            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string startPageIndex, string pageSize)
//        {

//            StringBuilder sb = new StringBuilder();
//            try
//            {
//                sb.Append(@"<table cellspacing='0' cellpadding='0' style='border-collapse: collapse;'>
//                            <tr><th colspan='14'><h1>DANH SÁCH KHIẾU NẠI TỔNG HỢP CHỜ XỬ LÝ</h1></th></tr>
//                            <tr><th colspan='14'>&nbsp;</th></tr>
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
//                            </tr>");

//                int TotalRecords = _KhieuNaiImpl.CountTongSoKhieuNai_WithPage(contentSeach, Convert.ToInt32(LoaiKhieuNaiId),
//                Convert.ToInt32(LinhVucChungId),
//                Convert.ToInt32(LinhVucConId),
//                Convert.ToInt32(PhongBanId),
//                Convert.ToInt32(DoUuTien),
//                Convert.ToInt32(trangThai),
//                NguoiXuLy,
//                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                KNHangLoat, GetAllKN, DoiTacId, isPermission,
//                Convert.ToInt32(startPageIndex),
//                Convert.ToInt32(pageSize));

//                if (TotalRecords > 0)
//                {
//                    var admin = LoginAdmin.AdminLogin();
//                    int totalPage = GetTotalPage(TotalRecords, PageSizeExcel);
//                    for (int i = 1; i <= totalPage; i++)
//                    {
//                        var lstResult = _KhieuNaiImpl.GetTongSoKhieuNai_WithPage(contentSeach,
//                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
//                                                                                                Convert.ToInt32(LinhVucChungId),
//                                                                                                Convert.ToInt32(LinhVucConId),
//                                                                                                Convert.ToInt32(PhongBanId),
//                                                                                                Convert.ToInt32(DoUuTien),
//                                                                                                Convert.ToInt32(trangThai),
//                                                                                                NguoiXuLy,
//                                                                                                SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
//                                                                                                KNHangLoat, GetAllKN, DoiTacId, isPermission,"LDate",
//                                                                                                i,
//                                                                                                PageSizeExcel);

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
//                                        sb.Append("<td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row.Id + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</a></td>");
//                                    }
//                                    else
//                                    {
//                                        sb.Append("        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</td>");
//                                    }
//                                }
//                                else
//                                {
//                                    sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:CheckXuLyKhieuNai(" + row.Id + ",'/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row.Id + "&ReturnUrl=" + strReturnURL + "&Mode=Process')\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, row.Id, 10) + "</a></td>");
//                                }
//                                sb.Append("        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row.DoUuTien).Replace("_", " ") + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoupChiTietKN('" + row.Id + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row.SoThueBao + "</a></td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.NoiDungPA + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucChung + "</td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"left\">" + row.LinhVucCon + "</td>");

//                                sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row.NguoiTiepNhan + "'>" + row.NguoiTiepNhan + "</a></td>");
//                                sb.Append("        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row.NguoiXuLy + "'>" + row.NguoiXuLy + "</a></td>");
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

//                                sb.Append(" </tr>");
//                            }

//                        }
//                    }

//                }

//                sb.Append("</table>");
//                return sb.ToString();
//            }
//            catch (Exception ex)
//            {
//                Utility.LogEvent(ex);
//                return "-1";
//            }

//        }

        #endregion
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