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

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for ExportExcelChiTietKN
    /// </summary>
    /// 
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportExcelChiTietKN : IHttpHandler, IReadOnlySessionState
    {
        private string userName = "";
        private string domain = "";
       // KhieuNaiImpl _KhieuNaiImpl = new KhieuNaiImpl();
        //KhieuNai_BuocXuLyImpl _KhieuNai_BuocXuLyImpl = new KhieuNai_BuocXuLyImpl();
        public void ProcessRequest(HttpContext context)
        {
            var strReturn = string.Empty;
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

            string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
            //LONGLX
            string ShowNguoiXuLy = context.Request.Form["ShowNguoiXuLy"] ?? context.Request.QueryString["ShowNguoiXuLy"];
              string sortName = context.Request.QueryString["sortname"];
            string sortOrder = context.Request.QueryString["sortorder"];
            if (string.IsNullOrEmpty(sortName))
                sortName = "LDate";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "DESC";
            string NguoiXuLy_Filter = "-1";
            bool IsTatCaKN = false;
            int KNHangLoat = 0;
            string PhongBanId = infoUser.PhongBanId.ToString(CultureInfo.InvariantCulture);
            bool isPermission = false;
            if (ShowNguoiXuLy!= null && ShowNguoiXuLy.Equals("1"))
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
            //END LONGLX

            

            string startPageIndex = context.Request.QueryString["startPageIndex"].ToString(CultureInfo.InvariantCulture);
            string pageSize =  context.Request.QueryString["pageSize"].ToString(CultureInfo.InvariantCulture);;
            switch (type)
            {
                case "1": //Khieu Nai Cho Xu Ly
                    strReturn = ExportKhieuNaiChuaXuLy(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "2": //Khieu Nai Chuyen Bo Phan Khac
                    strReturn = ExportKhieuNaiChuyenBoPhanKhac(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "3": //Khieu Nai Bo phan khac chuyen ve
                    strReturn = ExportKhieuNaiBoPhanKhacChuyenVe(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "4": //Khieu Nai Sap Qua Han
                    strReturn = ExportKhieuNaiSapQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "5"://Khieu Nai Qua Han
                    strReturn = ExportKhieuNaiQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "6": //Khieu Nai phan viec
                    strReturn = ExportKhieuNaiPhanViec(typeKhieuNai, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(CultureInfo.InvariantCulture), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                    break;
                case "8": //Khieu Nai Cho Xu Ly Tong hop

                    strReturn = ExportKhieuNaiChuaXuLyTongHop(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLyId, NguoiTienXuLyId,
                            nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;
                case "9": //Khieu Nai Chuyen Bo Phan Khac

                    strReturn = ExportKhieuNaiDaPhanHoi(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLy,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            nNgayQuaHanPB_From, nNgayQuaHanPB_To, KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy, sortName, sortOrder,
                            startPageIndex, pageSize, infoUser);
                    break;

            }
            var JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.Write( JSSerializer.Serialize(strReturn));
            //System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //context.Response.CacheControl = "no-cache";
            //context.Response.ContentType = "text/plain";
            //domain = "http://" + context.Request.Url.Authority;
            //if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            //{
            //    AdminInfo infoUser = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
            //    if (infoUser != null)
            //    {
            //        userName = infoUser.Username;
            //        context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
            //    }
            //}
        }

        //private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        //{
        //    string strValue = "";
        //    switch (key)
        //    {
        //        case "1": //Khieu Nai Cho Xu Ly
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = context.Request.QueryString["trangThai"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhan = string.Empty;
        //                }
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLy = string.Empty;
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                //strValue = ExportKhieuNaiChuaXuLy(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                //    PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
        //                //    nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                //    startPageIndex, pageSize);
        //            }
        //            break;
        //        case "2": //Khieu Nai Chuyen Bo Phan Khac
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = context.Request.QueryString["trangThai"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();
        //                string phongBanXuLy = context.Request.QueryString["phongBanXuLy"].ToString();
        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                //string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                int NguoiTiepNhanId = -1;
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && !NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTiepNhan);
        //                    if (NguoiTiepNhanId == 0)
        //                        NguoiTiepNhanId = -1;
        //                }
        //                else
        //                {
        //                    NguoiTiepNhanId = -1;
        //                    NguoiTiepNhan = "";
        //                }
        //                // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
        //                string NguoiTienXuLy = context.Request.Form["NguoiTienXuLy"] ?? context.Request.QueryString["NguoiTienXuLy"];
        //                int NguoiTienXuLyId = -1;
        //                if (!string.IsNullOrEmpty(NguoiTienXuLy) && !NguoiTienXuLy.Equals("Người tiền xử lý..."))
        //                {
        //                    NguoiTienXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTienXuLy);
        //                    if (NguoiTienXuLyId == 0)
        //                        NguoiTienXuLyId = -1;
        //                }
        //                else
        //                {
        //                    NguoiTienXuLyId = -1;
        //                    NguoiTienXuLy = "";
        //                }
        //                //string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                int NguoiXuLyId = -1;
        //                if (typeSearch != "-1" && !string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiXuLy);
        //                    if (NguoiXuLyId == 0)
        //                        NguoiXuLyId = -1;
        //                }
        //                else
        //                {
        //                    NguoiXuLyId = -1;
        //                    NguoiXuLy = "";
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                //strValue = ExportKhieuNaiChuyenBoPhanKhac(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
        //                //    PhongBanId, doUuTien, trangThai, NguoiXuLy, NguoiTienXuLyId,
        //                //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                //    startPageIndex, pageSize);
        //            }
        //            break;
        //        case "3": //Khieu Nai Bo phan khac chuyen ve
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = context.Request.QueryString["trangThai"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhan = string.Empty;
        //                }
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLy = string.Empty;
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                //strValue = ExportKhieuNaiBoPhanKhacChuyenVe(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                //    PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
        //                //    nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                //    startPageIndex, pageSize);
        //            }
        //            break;
        //        case "4": //Khieu Nai Sap Qua Han
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                //string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                int NguoiTiepNhanId = -1;
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && !NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhanId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTiepNhan);
        //                    if (NguoiTiepNhanId == 0)
        //                        NguoiTiepNhanId = -1;
        //                }
        //                else
        //                {
        //                    NguoiTiepNhanId = -1;
        //                    NguoiTiepNhan = "";
        //                }
        //                // neu nguoi dung gõ ten nguoi tien xu ly de tim kiem thi gan gia tri nguoixuly_default bang gia tri nhap vao
        //                string NguoiTienXuLy = context.Request.Form["NguoiTienXuLy"] ?? context.Request.QueryString["NguoiTienXuLy"];
        //                int NguoiTienXuLyId = -1;
        //                if (!string.IsNullOrEmpty(NguoiTienXuLy) && !NguoiTienXuLy.Equals("Người tiền xử lý..."))
        //                {
        //                    NguoiTienXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiTienXuLy);
        //                    if (NguoiTienXuLyId == 0)
        //                        NguoiTienXuLyId = -1;
        //                }
        //                else
        //                {
        //                    NguoiTienXuLyId = -1;
        //                    NguoiTienXuLy = "";
        //                }
        //                //string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                int NguoiXuLyId = -1;
        //                if (typeSearch != "-1" && !string.IsNullOrEmpty(NguoiXuLy) && !NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiXuLy);
        //                    if (NguoiXuLyId == 0)
        //                        NguoiXuLyId = -1;
        //                }
        //                else
        //                {
        //                    NguoiXuLyId = -1;
        //                    NguoiXuLy = "";
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX


        //                //strValue = ExportKhieuNaiSapQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                //    PhongBanId, doUuTien, NguoiXuLyId, NguoiTienXuLyId,
        //                //    nSoThueBao, NguoiTiepNhanId, NguoiXuLy_Default, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                //    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                //    startPageIndex, pageSize);
        //            }
        //            break;
        //        case "5": //Khieu Nai Qua Han
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhan = string.Empty;
        //                }
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLy = string.Empty;
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                strValue = ExportKhieuNaiQuaHan(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                    PhongBanId, doUuTien, NguoiXuLy_Default,
        //                    nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                    startPageIndex, pageSize);
        //            }
        //            break;
        //        case "6": //Khieu Nai phan viec
        //            string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = "-1";
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();
        //                strValue = ExportKhieuNaiPhanViec(typeKhieuNai, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
        //            }

        //            break;            

                    
        //        case "8": //Khieu Nai Cho Xu Ly Tong hop
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = context.Request.QueryString["trangThai"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                //string soThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                //string nguoiXuly = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                //string nguoiTiepnhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
        //                //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
        //                //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
        //                //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhan = string.Empty;
        //                }
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLy = string.Empty;
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                strValue = ExportKhieuNaiChuaXuLyTongHop(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                    PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
        //                    nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                    startPageIndex, pageSize);
        //            }
        //            break;
        //        #region Export Excel KhieuNai Da Phan Hoi
        //        case "9": //Khieu Nai Chuyen Bo Phan Khac
        //            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
        //            {
        //                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
        //                string contentSeach = context.Request.Form["contentSeach"] ?? context.Request.QueryString["contentSeach"];
        //                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
        //                string trangThai = context.Request.QueryString["trangThai"].ToString();
        //                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
        //                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
        //                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
        //                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
        //                string pageSize = context.Request.QueryString["pageSize"].ToString();

        //                //LONGLX
        //                string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

        //                string NguoiXuLy_Default = "-1";
        //                bool IsTatCaKN = false;
        //                int KNHangLoat = 0;
        //                string PhongBanId = infoUser.PhongBanId.ToString();
        //                bool isPermission = false;
        //                if (ShowNguoiXuLy.Equals("1"))
        //                {
        //                    NguoiXuLy_Default = "";
        //                }

        //                switch (typeSearch)
        //                {
        //                    case "-2":
        //                        KNHangLoat = -1;
        //                        IsTatCaKN = true;
        //                        isPermission = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xem_khiếu_nại_chờ_xử_lý_phòng_ban_cấp_dưới);
        //                        break;
        //                    case "-1":
        //                        KNHangLoat = -1;
        //                        NguoiXuLy_Default = infoUser.Username;
        //                        break;
        //                    case "0":
        //                        KNHangLoat = 1;
        //                        break;

        //                    default:
        //                        PhongBanId = typeSearch;
        //                        break;
        //                }

        //                if (!string.IsNullOrEmpty(contentSeach) && contentSeach.Equals("Nhập giá trị tìm kiếm..."))
        //                {
        //                    contentSeach = string.Empty;
        //                }

        //                string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
        //                int nSoThueBao = -1;
        //                if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
        //                {
        //                    nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
        //                }

        //                string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
        //                if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
        //                {
        //                    NguoiTiepNhan = string.Empty;
        //                }
        //                string NguoiXuLy = context.Request.Form["NguoiXuLy"] ?? context.Request.QueryString["NguoiXuLy"];
        //                if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
        //                {
        //                    NguoiXuLy = string.Empty;
        //                }

        //                string NgayTiepNhan_From = context.Request.Form["NgayTiepNhan_From"] ?? context.Request.QueryString["NgayTiepNhan_From"];
        //                int nNgayTiepNhan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayTiepNhan_To = context.Request.Form["NgayTiepNhan_To"] ?? context.Request.QueryString["NgayTiepNhan_To"];
        //                int nNgayTiepNhan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_From = context.Request.Form["NgayQuaHan_From"] ?? context.Request.QueryString["NgayQuaHan_From"];
        //                int nNgayQuaHan_From = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }

        //                string NgayQuaHan_To = context.Request.Form["NgayQuaHan_To"] ?? context.Request.QueryString["NgayQuaHan_To"];
        //                int nNgayQuaHan_To = -1;
        //                if (!string.IsNullOrEmpty(NgayQuaHan_To) && !NgayQuaHan_To.Equals("Đến ngày..."))
        //                {
        //                    try
        //                    {
        //                        nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                    }
        //                    catch { }
        //                }
        //                //END LONGLX

        //                strValue = ExportKhieuNaiDaPhanHoi(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
        //                    PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
        //                    nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
        //                    KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission,
        //                    startPageIndex, pageSize);
        //            }
        //            break;
        //        #endregion
        //    }
        //    return strValue;
        //}



        #region Process

        private string GetListFileDinhKem(string id, int Status)
        {
            try
            {
                string strValues = "";
                //KhieuNai_FileDinhKemImpl _KhieuNai_FileDinhKemImpl = new KhieuNai_FileDinhKemImpl();
                List<KhieuNai_FileDinhKemInfo> list = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListByKhieuNaiId(Convert.ToInt32(id));
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

        #region Seting Style
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

        private Aspose.Cells.Style StyleCellTextCenter()
        {
            var workbook = new Workbook();
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
            style.HorizontalAlignment = TextAlignmentType.Center;
            return style;
        }
        private Aspose.Cells.Style StyleCellMerge()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cell cell = worksheet.Cells["A1"];
            Aspose.Cells.Style style = cell.GetStyle();

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Black;           
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Black;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = Color.Aqua;            
            style.Font.IsBold = true;
            return style;
        }

        private Aspose.Cells.Style StyleCellTitleNoBg()
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
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.Font.IsBold = true;
            return style;
        }

        private Aspose.Cells.Style StyleCellTitle()
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
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = Color.Silver;   
            style.Font.IsBold = true;
            return style;
        }
        #endregion
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
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(row["STT"]);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                    {
                        if (row["NguoiXuLy"].ToString() != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(row["SoThueBao"]);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(row["HoTenLienHe"]);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(row["DiaChiLienHe"]);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    Aspose.Cells.Style style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(row["TrangThai"])).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(row["NoiDungPA"]);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    Aspose.Cells.Style style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(Convert.ToInt32(row["Id"]));
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        Aspose.Cells.Style style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
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

        #endregion

        #region KN Cho Xu Ly Tong Hop
        private int AddContentToSheet_ChuaXuLyTongHop(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    var style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    var style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        private string ExportKhieuNaiChuaXuLyTongHop(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        {
            string strValue = "";
            try
            {
                #region ExportExcel
                Workbook workbookExport = new Workbook();
                Worksheet sheetExport = workbookExport.Worksheets[0];

                var workbookTemp = new Workbook();
                Worksheet sheet = null;

                string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = 0;
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_WithPage1(contentSeach,
                      Convert.ToInt32(LoaiKhieuNaiId),
                      Convert.ToInt32(LinhVucChungId),
                      Convert.ToInt32(LinhVucConId),
                      Convert.ToInt32(PhongBanId),
                      Convert.ToInt32(DoUuTien),
                      Convert.ToInt32(trangThai),
                      NguoiXuLyId, NguoiTienXuLyId,
                      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                      NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission,
                      Convert.ToInt32(startPageIndex),
                      Convert.ToInt32(pageSize));
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_WithPage2(contentSeach,
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
                }

                //TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_WithPage(contentSeach,
                //      Convert.ToInt32(LoaiKhieuNaiId),
                //      Convert.ToInt32(LinhVucChungId),
                //      Convert.ToInt32(LinhVucConId),
                //      Convert.ToInt32(PhongBanId),
                //      Convert.ToInt32(DoUuTien),
                //      Convert.ToInt32(trangThai),
                //      NguoiXuLyId, NguoiTienXuLyId,
                //      SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                //      KNHangLoat, GetAllKN, DoiTacId, isPermission,
                //      Convert.ToInt32(startPageIndex),
                //      Convert.ToInt32(pageSize));

                if (TotalRecords > 0)
                {
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));                    
                    
                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );
                        
                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage1(contentSeach,
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
                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetTongSoKhieuNai_WithPage2(contentSeach,
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
                        }
                        
                        //DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyTongHop_GetAllWithPadding(contentSeach,
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        Convert.ToInt32(trangThai),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i,
                        //                                                                        Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet_ChuaXuLyTongHop(lstResult, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_ChuaXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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


        private int AddContentToSheet_ChoXuLy(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object) item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy ))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    Aspose.Cells.Style style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    Aspose.Cells.Style style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            Aspose.Cells.Style style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        Aspose.Cells.Style style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        private string ExportKhieuNaiChuaXuLy(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)

        //private string ExportKhieuNaiChuaXuLy(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
        //    int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string startPageIndex, string pageSize)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                //int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLy_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                //Convert.ToInt32(LoaiKhieuNaiId),
                //Convert.ToInt32(LinhVucChungId),
                //Convert.ToInt32(LinhVucConId),
                //Convert.ToInt32(PhongBanId),
                //Convert.ToInt32(DoUuTien),
                //Convert.ToInt32(trangThai),
                //NguoiXuLy,
                //SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                //KNHangLoat, GetAllKN, DoiTacId, isPermission,
                //Convert.ToInt32(startPageIndex),
                //Convert.ToInt32(pageSize));

                int TotalRecords = 0;
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_ChoXuLy_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
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
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountKhieuNai_ChoXuLy_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
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
                }


                if (TotalRecords > 0)
                {
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );

                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        //DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLy_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        Convert.ToInt32(trangThai),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i,
                        //                                                                        Convert.ToInt32(pageSize));

                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
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
                            RowIndex = AddContentToSheet_ChoXuLy(lstResult, RowIndex, sheet);
                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().GetKhieuNai_ChoXuLy_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
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
                            RowIndex = AddContentToSheet_ChoXuLy(lstResult, RowIndex, sheet);
                        }

                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_ChuaXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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
        private int AddContentToSheet_ChuyenBoPhanKhac(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    var style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    var style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        private string ExportKhieuNaiChuyenBoPhanKhac(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, 
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)

        //private string ExportKhieuNaiChuyenBoPhanKhac(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
        //    string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
        //    string startPageIndex, string pageSize)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.

                int TotalRecords = 0;
                int dem = LinhVucConId.Split(',').Count();
                if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords1(contentSeach, Convert.ToInt32(TypeSearch),
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
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords2(contentSeach, Convert.ToInt32(TypeSearch),
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
                }

                if (TotalRecords > 0)
                {
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );

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



                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) < 1 && dem < 2)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
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
                            RowIndex = AddContentToSheet_ChuyenBoPhanKhac(lstResult, RowIndex, sheet);
                        }
                        else
                        {
                            ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiChuyenBoPhanKhac_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
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
                            RowIndex = AddContentToSheet_ChuyenBoPhanKhac(lstResult, RowIndex, sheet);
                        }
                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_ChuyenBoPhanKhac" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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
        private int AddContentToSheet_BoPhanKhacChuyenVe(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    var style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    var style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        private string ExportKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string contentSeach, string TypeSearch,
            string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //private string ExportKhieuNaiBoPhanKhacChuyenVe(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
        //    string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
        //    int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
        //    string startPageIndex, string pageSize)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                //int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                //Convert.ToInt32(LoaiKhieuNaiId),
                //Convert.ToInt32(LinhVucChungId),
                //Convert.ToInt32(LinhVucConId),
                //Convert.ToInt32(PhongBanId),
                //Convert.ToInt32(DoUuTien),
                //Convert.ToInt32(trangThai),
                //NguoiXuLy,
                //SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                //KNHangLoat, GetAllKN, DoiTacId, isPermission,
                //Convert.ToInt32(startPageIndex),
                //Convert.ToInt32(pageSize));
                int TotalRecords = 0;
                if (int.Parse(LinhVucChungId) > 0)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        Convert.ToInt32(LinhVucConId),
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiBoPhanKhacChuyenVe2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        LinhVucConId,
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        Convert.ToInt32(trangThai),
                        NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }
                if (TotalRecords > 0)
                {
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );

                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        //DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        Convert.ToInt32(trangThai),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i, Convert.ToInt32(pageSize));
                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) > 0)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage1(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                                                                                                i,
                                                                                                Convert.ToInt32(pageSize));
                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiBoPhanKhacChuyenVe_WithPage2(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                LinhVucConId,
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                Convert.ToInt32(trangThai),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                                                                                                i,
                                                                                                Convert.ToInt32(pageSize));
                        }
                        RowIndex = AddContentToSheet_BoPhanKhacChuyenVe(lstResult, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_BoPhanKhacChuyenVe" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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
        private int AddContentToSheet_DaQuaHan(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    var style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    var style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list =ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        var style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        private string ExportKhieuNaiSapQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, 
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
        //private string ExportKhieuNaiSapQuaHan(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
        //    string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
        //    int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
        //    int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
        //    string startPageIndex, string pageSize)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                //int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                //Convert.ToInt32(LoaiKhieuNaiId),
                //Convert.ToInt32(LinhVucChungId),
                //Convert.ToInt32(LinhVucConId),
                //Convert.ToInt32(PhongBanId),
                //Convert.ToInt32(DoUuTien),
                //NguoiXuLyId, NguoiTienXuLyId,
                //SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                //KNHangLoat, GetAllKN, DoiTacId, isPermission,
                //Convert.ToInt32(startPageIndex),
                //Convert.ToInt32(pageSize));

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
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString(CultureInfo.InvariantCulture).Substring(6, 2) + "/" + NgayTiepNhan_From.ToString(CultureInfo.InvariantCulture).Substring(4, 2) + "/" + NgayTiepNhan_From.ToString(CultureInfo.InvariantCulture).Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString(CultureInfo.InvariantCulture).Substring(6, 2) + "/" + NgayTiepNhan_To.ToString(CultureInfo.InvariantCulture).Substring(4, 2) + "/" + NgayTiepNhan_To.ToString(CultureInfo.InvariantCulture).Substring(0, 4)
                            );

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

                string fileName = "DanhSachKhieuNaiChiTiet_SapQuaHan" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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

        private string ExportKhieuNaiQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To,  int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, 
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
                workbookTemp.Open(path);
                sheet = workbookTemp.Worksheets[0];
                sheet.Cells.DeleteRows(6, sheet.Cells.Rows.Count);
                int RowIndex = 5;
                //Save the Excel file.
                int TotalRecords = 0;
                if (int.Parse(LinhVucChungId) > 0)
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan1(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        Convert.ToInt32(LinhVucConId),
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }
                else
                {
                    TotalRecords = ServiceFactory.GetInstanceKhieuNai().Count_KhieuNaiQuaHan2(contentSeach, Convert.ToInt32(TypeSearch), Convert.ToInt32(LoaiKhieuNaiId),
                        Convert.ToInt32(LinhVucChungId),
                        LinhVucConId,
                        Convert.ToInt32(PhongBanId),
                        Convert.ToInt32(DoUuTien),
                        NguoiXuLyId, NguoiTienXuLyId,
                        SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, 1, 1);
                }

                if (TotalRecords > 0)
                {
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );

                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List < KhieuNaiInfo > lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) > 0)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage1(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                Convert.ToInt32(LinhVucConId),
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                                                                                                i,
                                                                                                Convert.ToInt32
                                                                                                            (pageSize));
                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiQuaHan_WithPage2(contentSeach,
                                                                                                Convert.ToInt32(TypeSearch),
                                                                                                Convert.ToInt32(LoaiKhieuNaiId),
                                                                                                Convert.ToInt32(LinhVucChungId),
                                                                                                LinhVucConId,
                                                                                                Convert.ToInt32(PhongBanId),
                                                                                                Convert.ToInt32(DoUuTien),
                                                                                                NguoiXuLyId, NguoiTienXuLyId,
                                                                                                SoThueBao, NguoiTiepNhanId, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                                                                                                NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                                                                                                i,
                                                                                                Convert.ToInt32
                                                                                                            (pageSize));
                        }
                        //DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiQuaHan_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i,
                        //                                                                        Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet_DaQuaHan(lstResult, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_QuaHan" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString();
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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
        private int AddContentToSheet_DaPhanHoi(List<KhieuNaiInfo> pKhieuNaiInfos, int RowIndex, Worksheet sheet)
        {
            try
            {
                foreach (var item in pKhieuNaiInfos)
                {
                    #region Title
                    sheet.Cells[RowIndex, 0].PutValue("STT");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 1].PutValue("Mã khiếu nại");
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 2].PutValue("Ngày tiếp nhận");
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 3].PutValue("Số thuê bao");
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 4].PutValue("Họ và tên");
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitle());


                    sheet.Cells[RowIndex, 5].PutValue("Địa chỉ liên hệ");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 6].PutValue("Kết quả xử lý");
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitle());

                    sheet.Cells[RowIndex, 7].PutValue("Nội dung phản ánh");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitle());
                    #endregion
                    #region Noi Dung
                    RowIndex = RowIndex + 1;
                    sheet.Cells[RowIndex, 0].PutValue(item.STT);
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 1].PutValue(GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)item.Id, 10));
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTextCenter());
                    Aspose.Cells.Style style1 = sheet.Cells[RowIndex, 1].GetStyle();
                    style1.Font.Color = Color.Blue;
                    sheet.Cells[RowIndex, 1].SetStyle(style1);
                    if (!string.IsNullOrEmpty(item.NguoiXuLy))
                    {
                        if (item.NguoiXuLy != userName)
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=View");
                        }
                        else
                        {
                            sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");

                        }
                    }
                    else
                    {
                        sheet.Hyperlinks.Add("B" + RowIndex, RowIndex, 1, domain + "/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + item.Id + "&ReturnUrl=" + "/Default.aspx" + "&Mode=Process");
                    }

                    sheet.Cells[RowIndex, 2].PutValue(Convert.ToDateTime(item.NgayTiepNhan).ToString("dd/MM/yyyy HH:mm:ss"));
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTextCenter());

                    sheet.Cells[RowIndex, 3].PutValue(item.SoThueBao);
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTextCenter());


                    sheet.Cells[RowIndex, 4].PutValue(item.HoTenLienHe);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());


                    sheet.Cells[RowIndex, 5].PutValue(item.DiaChiLienHe);
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCell());
                    Aspose.Cells.Style style5 = sheet.Cells[RowIndex, 5].GetStyle();
                    style5.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 5].SetStyle(style5);

                    sheet.Cells[RowIndex, 6].PutValue(Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(item.TrangThai)).Replace("_", " "));
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 7].PutValue(item.NoiDungPA);
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCell());
                    Aspose.Cells.Style style7 = sheet.Cells[RowIndex, 7].GetStyle();
                    style7.IsTextWrapped = true;
                    sheet.Cells[RowIndex, 7].SetStyle(style7);
                    #endregion
                    RowIndex = RowIndex + 2;

                    sheet.Cells.Merge(RowIndex, 0, 1, 8);
                    sheet.Cells[RowIndex, 0].PutValue("TÌNH TRẠNG XỬ LÝ");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellMerge());
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellMerge());
                    RowIndex = RowIndex + 1;
                    sheet.Cells.Merge(RowIndex, 0, 1, 5);
                    sheet.Cells[RowIndex, 0].PutValue("Nội dung xử lý");
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells.Merge(RowIndex, 5, 1, 2);
                    sheet.Cells[RowIndex, 5].PutValue("Ngày xử lý");
                    sheet.Cells[RowIndex, 5].SetStyle(StyleCellTitleNoBg());
                    sheet.Cells[RowIndex, 6].SetStyle(StyleCellTitleNoBg());

                    sheet.Cells[RowIndex, 7].PutValue("Người xử lý");
                    sheet.Cells[RowIndex, 7].SetStyle(StyleCellTitleNoBg());

                    RowIndex = RowIndex + 1;
                    List<KhieuNai_BuocXuLyInfo> list = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListByKhieuNaiId(item.Id);
                    if (list.Count > 0)
                    {
                        foreach (var info in list)
                        {
                            sheet.Cells.Merge(RowIndex, 0, 1, 5);
                            sheet.Cells[RowIndex, 0].PutValue(info.NoiDung);
                            sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                            sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                            Aspose.Cells.Style style0 = sheet.Cells[RowIndex, 0].GetStyle();
                            style0.IsTextWrapped = true;
                            sheet.Cells[RowIndex, 0].SetStyle(style0);

                            sheet.Cells.Merge(RowIndex, 5, 1, 2);
                            sheet.Cells[RowIndex, 5].PutValue(info.CDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                            sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                            sheet.Cells[RowIndex, 7].PutValue(info.CUser);
                            sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                            RowIndex++;
                        }
                    }
                    else
                    {
                        sheet.Cells.Merge(RowIndex, 0, 1, 5);
                        sheet.Cells[RowIndex, 0].PutValue("");
                        sheet.Cells[RowIndex, 0].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 1].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 2].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 3].SetStyle(StyleCell());
                        sheet.Cells[RowIndex, 4].SetStyle(StyleCell());
                        Aspose.Cells.Style style0 = sheet.Cells[RowIndex, 0].GetStyle();
                        style0.IsTextWrapped = true;
                        sheet.Cells[RowIndex, 0].SetStyle(style0);

                        sheet.Cells.Merge(RowIndex, 5, 1, 2);
                        sheet.Cells[RowIndex, 5].PutValue("");
                        sheet.Cells[RowIndex, 5].SetStyle(StyleCellTextCenter());
                        sheet.Cells[RowIndex, 6].SetStyle(StyleCellTextCenter());

                        sheet.Cells[RowIndex, 7].PutValue("");
                        sheet.Cells[RowIndex, 7].SetStyle(StyleCellTextCenter());

                        RowIndex++;
                    }
                    RowIndex = RowIndex + 1;
                }
                return RowIndex;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        private string ExportKhieuNaiDaPhanHoi(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string NguoiTienXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, 
            string sortName, string sortOrder, string startPageIndex, string pageSize, AdminInfo infoUser)
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
                path += @"\Template\DanhSachKhieuNaiChiTiet.xlsx";
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
                    sheet.Cells[2, 7].PutValue("," + DateTime.Now.ToString("dd/MM/yyyy"));

                    if (NgayTiepNhan_To != -1 && NgayTiepNhan_From != -1)
                    {
                        sheet.Cells[3, 2].PutValue("Từ ngày :" + NgayTiepNhan_From.ToString().Substring(6, 2) + "/" + NgayTiepNhan_From.ToString().Substring(4, 2) + "/" + NgayTiepNhan_From.ToString().Substring(0, 4)
                                + " đến ngày : " + NgayTiepNhan_To.ToString().Substring(6, 2) + "/" + NgayTiepNhan_To.ToString().Substring(4, 2) + "/" + NgayTiepNhan_To.ToString().Substring(0, 4)
                            );

                    }
                    int totalPage = GetTotalPage(TotalRecords, Convert.ToInt32(pageSize));
                    for (int i = 1; i <= totalPage; i++)
                    {
                        List<KhieuNaiInfo> lstResult = new List<KhieuNaiInfo>();
                        if (int.Parse(LinhVucChungId) > 0)
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage1(contentSeach, Convert.ToInt32(TypeSearch),
                               Convert.ToInt32(LoaiKhieuNaiId),
                               Convert.ToInt32(LinhVucChungId),
                               Convert.ToInt32(LinhVucConId),
                               Convert.ToInt32(PhongBanId),
                               Convert.ToInt32(DoUuTien),
                               Convert.ToInt32(trangThai),
                               NguoiXuLy,
                               SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                               NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                               i,
                               Convert.ToInt32(pageSize));
                        }
                        else
                        {
                            lstResult = ServiceFactory.GetInstanceKhieuNai().Get_KhieuNaiDaPhanHoi_WithPage2(contentSeach, Convert.ToInt32(TypeSearch),
                               Convert.ToInt32(LoaiKhieuNaiId),
                               Convert.ToInt32(LinhVucChungId),
                               LinhVucConId,
                               Convert.ToInt32(PhongBanId),
                               Convert.ToInt32(DoUuTien),
                               Convert.ToInt32(trangThai),
                               NguoiXuLy,
                               SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                               NgayQuaHanPB_From, NgayQuaHanPB_To, KNHangLoat, GetAllKN, DoiTacId, isPermission, "LDate", "DESC",
                               i,
                               Convert.ToInt32(pageSize));
                        }
                        //DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                        //                                                                        Convert.ToInt32(LoaiKhieuNaiId),
                        //                                                                        Convert.ToInt32(LinhVucChungId),
                        //                                                                        Convert.ToInt32(LinhVucConId),
                        //                                                                        Convert.ToInt32(PhongBanId),
                        //                                                                        Convert.ToInt32(DoUuTien),
                        //                                                                        Convert.ToInt32(trangThai),
                        //                                                                        NguoiXuLy,
                        //                                                                        SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                        //                                                                        KNHangLoat, GetAllKN, DoiTacId, isPermission,
                        //                                                                        i,
                        //                                                                        Convert.ToInt32(pageSize));
                        RowIndex = AddContentToSheet_DaPhanHoi(lstResult, RowIndex, sheet);
                    }

                }

                string fileName = "DanhSachKhieuNaiChiTiet_DaPhanHoi" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
                string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
                string pathChild = "";
                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                }

                if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture)))
                {
                    Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture));
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    pathSave += "\\" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                    pathChild += "/" + DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
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
                int RowIndex = 5; 
                int TotalRecords = 0;
                string title = "";
                DataTable tab = new DataTable();

                //Save the Excel file.

                switch (typeKhieuNai)
                {
                    case "1":
                        title = "chờ xử lý";
                        TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords
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
                        TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding_TotalRecords
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
                        TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding_TotalRecords
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
                        TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding_TotalRecords
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

                                tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding
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

                                tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding
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

                                tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding
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

                                tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding
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
                return strValue;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
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