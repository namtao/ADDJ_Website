using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using AIVietNam.GQKN.Impl;
using System.Data;
using AIVietNam.GQKN.Entity;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using Website.AppCode;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Globalization;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    /// 

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler_NghiNV : IHttpHandler, IReadOnlySessionState
    {
        LoaiKhieuNaiImpl _LoaiKhieuNaiImpl = ServiceFactory.GetInstanceLoaiKhieuNai();
        KhieuNaiImpl _KhieuNaiImpl = ServiceFactory.GetInstanceKhieuNai();
        PhongBanImpl _PhongBanImpl = ServiceFactory.GetInstancePhongBan();
        PhongBan2PhongBanImpl _PhongBan2PhongBanImpl = ServiceFactory.GetInstancePhongBan2PhongBan();
        KhieuNai_ActivityImpl _KhieuNai_ActivityImpl = new KhieuNai_ActivityImpl();
        NguoiSuDungImpl _NguoiSuDungImpl = new NguoiSuDungImpl();
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
                    //context.Response.Write(JSSerializer.Serialize(context.Request.QueryString["key"]));
                    if (context.Request.QueryString["key"] == "5")
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                        {
                            string id = context.Request.QueryString["id"].ToString();
                            string view = context.Request.QueryString["view"].ToString();
                            context.Response.Write(JSSerializer.Serialize(GetInfoKhieuNaiByID(id, view)));
                        }
                        
                    }
                    else
                    {
                        context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"], infoUser, context)));
                    }
                    
                }
            }
        }
        private string ProcessData(string key, AdminInfo infoUser, HttpContext context)
        {
            string strValue = "";
            switch (key)
            {
                #region Load Ghi Chu Trang Thai Xu Ly
                case "30":
                    strValue = LoadGhiChuTrangThaiXuLy();
                    break;
                #endregion
                #region LoadDropList
                case "25":
                    strValue = GetItemDropListLoaiKhieuNai(infoUser.PhongBanId);
                    break;
                case "26":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["loaiKhieuNaiId"]))
                    {
                        strValue = GetItemDropListLinhVucChung(context.Request.QueryString["loaiKhieuNaiId"]);
                    }
                    break;
                case "27":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["linhVucChungId"]))
                    {
                        strValue = GetItemDropListLinhVucCon(context.Request.QueryString["linhVucChungId"]);
                    }
                    break;
                case "28":
                    strValue = GetItemDropListDoUuTien();
                    break;
                case "35":
                    //Trang Thai Cho Xu Ly
                    strValue = GetItemDropListTrangThai(((int)KhieuNai_TrangThai_Type.Chờ_xử_lý).ToString() + "#" + ((int)KhieuNai_TrangThai_Type.Đang_xử_lý).ToString() + "#" + ((int)KhieuNai_TrangThai_Type.Chờ_đóng).ToString());
                    break;


                case "33":
                    strValue = LoadPhongBanChuyenDen(infoUser);
                    break;
                case "34":
                    strValue = GetDanhSachUser();
                    break;
                case "63":
                    strValue = GetItemDropListPhongBanXuLy();
                    break;
                #endregion
                #region LoaiKhieuNai
                case "1":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetTotalRecords(infoUser.PhongBanId, startPageIndex, pageSize);
                    }
                    break;
                case "2":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetHtmlLoaiKhieuNai(infoUser.PhongBanId, startPageIndex, pageSize);
                    }
                    break;
                case "10":
                    string catid = context.Request.QueryString["catid"].ToString();
                    strValue = GetNameLoaiKhieuNai(catid);
                    break;
                case "39":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetQLKN_LoaiKhieuNai_TongSoKhieuNai(infoUser.PhongBanId, startPageIndex, pageSize);
                    }
                    break;
                #endregion
                #region KhieuNaiChuaXuLyTongHop
                case "55":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //string soThueBao = context.Request.QueryString["soThueBao"].ToString();
                        //string nguoiXuly = context.Request.QueryString["nguoiXuly"].ToString();
                        //string nguoiTiepnhan = context.Request.QueryString["nguoiTiepnhan"].ToString();
                        //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
                        //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
                        //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
                        //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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
                        strValue = GetKhieuNaiChuaXuLyTongHop_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "56":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //string soThueBao = context.Request.QueryString["soThueBao"].ToString();
                        //string nguoiXuly = context.Request.QueryString["nguoiXuly"].ToString();
                        //string nguoiTiepnhan = context.Request.QueryString["nguoiTiepnhan"].ToString();
                        //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
                        //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
                        //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
                        //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetHtmlKhieuNaiChuaXuLyTongHop(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "57":
                    strValue = QLKN_GetTotalKhieuNaiChuaXuLyTongHop(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiChuaXuLy
                case "3":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //string soThueBao = context.Request.QueryString["soThueBao"].ToString();
                        //string nguoiXuly = context.Request.QueryString["nguoiXuly"].ToString();
                        //string nguoiTiepnhan = context.Request.QueryString["nguoiTiepnhan"].ToString();
                        //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
                        //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
                        //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
                        //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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
                        strValue = GetKhieuNaiChuaXuLy_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "4":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //string soThueBao = context.Request.QueryString["soThueBao"].ToString();
                        //string nguoiXuly = context.Request.QueryString["nguoiXuly"].ToString();
                        //string nguoiTiepnhan = context.Request.QueryString["nguoiTiepnhan"].ToString();
                        //string ngayTiepNhanTu = context.Request.QueryString["ngayTiepNhanTu"].ToString();
                        //string ngayTiepNhanDen = context.Request.QueryString["ngayTiepNhanDen"].ToString();
                        //string ngayQuaHanTu = context.Request.QueryString["ngayQuaHanTu"].ToString();
                        //string ngayQuaHanDen = context.Request.QueryString["ngayQuaHanDen"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetHtmlKhieuNaiChuaXuLy(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;

                //case "5":
                //    if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                //    {
                //        string id = context.Request.QueryString["id"].ToString();
                //        string view = context.Request.QueryString["view"].ToString();
                //        strValue = GetInfoKhieuNaiByID(id, view);
                //    }
                //    break;
                case "8":
                    strValue = QLKN_GetTotalKhieuNaiChuaXuLy(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiChuyenBoPhanKhac
                case "6":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();

                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string phongBanXuLy = context.Request.QueryString["phongBanXuLy"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetKhieuNaiChuyenBoPhanKhac_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, phongBanXuLy,PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "7":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        string phongBanXuLy = context.Request.QueryString["phongBanXuLy"].ToString();
                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetHtmlKhieuNaiChuyenBoPhanKhac(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, phongBanXuLy,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "9":
                    strValue = QLKN_GetTotalKhieuNaiChuyenBoPhanKhac(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiBoPhanKhacChuyenVe
                case "11":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "12":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetHtmlKhieuNaiBoPhanKhacChuyenVe(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "13":
                    strValue = QLKN_GetTotalKhieuNaiBoPhanKhacChuyenVe(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiSapQuaHan
                case "14":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetKhieuNaiSapQuaHan_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "15":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetHtmlKhieuNaiSapQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "16":
                    strValue = QLKN_GetTotalKhieuNaiSapQuaHan(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiQuaHan
                case "17":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetKhieuNaiQuaHan_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "18":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetHtmlKhieuNaiQuaHan(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "19":
                    strValue = QLKN_GetTotalKhieuNaiQuaHan(infoUser.PhongBanId);
                    break;
                #endregion
                #region KhieuNaiDaPhanHoi
                case "60":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();

                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();


                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetKhieuNaiDaPhanHoi_TotalRecords(contentSeach, typeSearch, loaiKhieuNai, linhVucChung,
                            linhVucCon, PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "61":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string contentSeach = context.Request.QueryString["contentSeach"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        string trangThai = context.Request.QueryString["trangThai"].ToString();
                        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //LONGLX
                        string ShowNguoiXuLy = context.Request.QueryString["ShowNguoiXuLy"].ToString();

                        string NguoiXuLy_Default = "-1";
                        bool IsTatCaKN = false;
                        int KNHangLoat = 0;
                        string PhongBanId = infoUser.PhongBanId.ToString();
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
                                NguoiXuLy_Default = infoUser.Username;
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

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        int nSoThueBao = -1;
                        if (!string.IsNullOrEmpty(SoThueBao) && !SoThueBao.Equals("Số thuê bao..."))
                        {
                            nSoThueBao = ConvertUtility.ToInt32(SoThueBao);
                        }

                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        if (!string.IsNullOrEmpty(NguoiTiepNhan) && NguoiTiepNhan.Equals("Người tiếp nhận..."))
                        {
                            NguoiTiepNhan = string.Empty;
                        }
                        string NguoiXuLy = context.Request.QueryString["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(NguoiXuLy) && NguoiXuLy.Equals("Người xử lý..."))
                        {
                            NguoiXuLy = string.Empty;
                        }

                        string NgayTiepNhan_From = context.Request.QueryString["NgayTiepNhan_From"].ToString();
                        int nNgayTiepNhan_From = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_From) && !NgayTiepNhan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayTiepNhan_To = context.Request.QueryString["NgayTiepNhan_To"].ToString();
                        int nNgayTiepNhan_To = -1;
                        if (!string.IsNullOrEmpty(NgayTiepNhan_To) && !NgayTiepNhan_To.Equals("Đến ngày..."))
                        {
                            try
                            {
                                nNgayTiepNhan_To = Convert.ToInt32(Convert.ToDateTime(NgayTiepNhan_To, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_From = context.Request.QueryString["NgayQuaHan_From"].ToString();
                        int nNgayQuaHan_From = -1;
                        if (!string.IsNullOrEmpty(NgayQuaHan_From) && !NgayQuaHan_From.Equals("Từ ngày..."))
                        {
                            try
                            {
                                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(NgayQuaHan_From, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                            }
                            catch { }
                        }

                        string NgayQuaHan_To = context.Request.QueryString["NgayQuaHan_To"].ToString();
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

                        strValue = GetHtmlKhieuNaiDaPhanHoi(context, contentSeach, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon,
                            PhongBanId, doUuTien, trangThai, NguoiXuLy_Default,
                            nSoThueBao, NguoiTiepNhan, NguoiXuLy, nNgayTiepNhan_From, nNgayTiepNhan_To, nNgayQuaHan_From, nNgayQuaHan_To,
                            KNHangLoat, IsTatCaKN, infoUser.DoiTacId, isPermission, ShowNguoiXuLy,
                            startPageIndex, pageSize);
                    }
                    break;
                case "62":
                    strValue = QLKN_GetTotalKhieuNaiDaPhanHoi(infoUser.PhongBanId);
                    break;
                #endregion
                #region ChuyenKhieuNai
                case "21"://Chuyen Xu Ly
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]) && !string.IsNullOrEmpty(context.Request.QueryString["phongban"]))
                    {
                        string listID = context.Request.QueryString["listID"];
                        string phongban = context.Request.QueryString["phongban"];
                        string dataNote = context.Request.Form["data"];
                        string username = context.Request.QueryString["Username"];
                        if (username == "-1" || username == "undefined")
                        {
                            username = string.Empty;
                        }
                        strValue = QLKN_ChuyenXuLy(listID, Convert.ToInt32(phongban),username, dataNote);
                    }
                    break;
                case "52"://Chuyen Phan Hoi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                    {
                        string listID = context.Request.QueryString["listID"];

                        string dataNote = context.Request.Form["data"];
                        strValue = QLKN_ChuyenPhanHoi(listID, dataNote);
                    }
                    break;
                case "53"://Chuyen Ngang Hang
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                    {
                        string listID = context.Request.QueryString["listID"];

                        string dataNote = context.Request.Form["data"];
                        strValue = QLKN_ChuyenNgangHang(listID, dataNote);
                    }
                    break;

                case "24":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                    {
                        string dataNote = context.Request.Form["data"];
                        string listID = context.Request.QueryString["listID"];
                        strValue = QLKN_DongKhieuNai(listID, dataNote);
                    }
                    break;
                #endregion
                #region SoTheoDoiKhieuNai
                case "31"://Tính tổng số bản ghi tren so theo dõi

                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string select = context.Request.QueryString["select"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();

                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        string ThoiGianTiepNhanTu = context.Request.QueryString["ThoiGianTiepNhanTu"].ToString();
                        string ThoiGianTiepNhanDen = context.Request.QueryString["ThoiGianTiepNhanDen"].ToString();

                        
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

                        strValue = GetKhieuNaiSoTheoDoi_TotalRecords(select, PhongBanId, SoThueBao, NguoiXuLy, NguoiTiepNhan, ThoiGianTiepNhanTu, 
                            ThoiGianTiepNhanDen, IsTatCaKN, infoUser.DoiTacId, isPermission, startPageIndex, pageSize);
                       
                    }
                    break;
                case "32"://Lấy danh sách so theo dõi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string select = context.Request.QueryString["select"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        string SoThueBao = context.Request.QueryString["SoThueBao"].ToString();
                        string NguoiTiepNhan = context.Request.QueryString["NguoiTiepNhan"].ToString();
                        string ThoiGianTiepNhanTu = context.Request.QueryString["ThoiGianTiepNhanTu"].ToString();
                        string ThoiGianTiepNhanDen = context.Request.QueryString["ThoiGianTiepNhanDen"].ToString();                        
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

                        strValue = GetHtmlKhieuNaiSoTheoDoi(infoUser.Username, select, PhongBanId, SoThueBao, NguoiXuLy, NguoiTiepNhan, ThoiGianTiepNhanTu,
                            ThoiGianTiepNhanDen, IsTatCaKN, infoUser.DoiTacId, isPermission, startPageIndex, pageSize);
                    }
                    break;
                case "36"://Cap nhật thêm moi vào sổ theo dõi
                    if (!string.IsNullOrEmpty(context.Request.QueryString["maKhieuNai"]))
                    {
                        string maKhieuNai = context.Request.QueryString["maKhieuNai"].ToString();

                        string HoTenLienHe = context.Request.Form["HoTenLienHe"].ToString();
                        string DiaChiLienHe = context.Request.Form["DiaChiLienHe"].ToString();
                        string NoiDungPA = context.Request.Form["NoiDungPA"].ToString();
                        string NoiDungXuLy = context.Request.Form["NoiDungXuLy"].ToString();
                        string NgayTiepNhan = context.Request.Form["NgayTiepNhan"].ToString();
                        string NgayTraLoiKN = context.Request.Form["NgayTraLoiKN"].ToString();
                        string KetQuaXuLy = context.Request.Form["KetQuaXuLy"].ToString();
                        string GhiChu = context.Request.Form["GhiChu"].ToString();
                        string soThueBao = context.Request.Form["soThueBao"];
                        string LoaiKhieuNai = context.Request.Form["LoaiKhieuNai"];
                        string LinhVucChung = context.Request.Form["LinhVucChung"];
                        string LinhVucCon = context.Request.Form["LinhVucCon"];

                        strValue = CapNhatSoTheoDoi(maKhieuNai, infoUser, soThueBao, HoTenLienHe, DiaChiLienHe, NoiDungPA,
                            NoiDungXuLy, NgayTiepNhan, NgayTraLoiKN, KetQuaXuLy, GhiChu, LoaiKhieuNai, LinhVucChung, LinhVucCon);
                    }
                    break;
                case "37"://Bind Thong tin Loai Khieu Nai vao DropList
                    strValue = GetItemDropListLoaiKhieuNai(0);
                    break;
                case "38"://Bind Thong tin cua so thuê bao
                    string dauSo = context.Request.QueryString["dauSo"];
                    string thueBao = context.Request.QueryString["soThueBao"];
                    strValue = BindThongTinThueBao(dauSo, thueBao);
                    break;
                case "40"://Kiem tra quyền thêm mới du liệu vao sổ theo dõi                    
                    strValue = CheckQuyenThemMoi();
                    break;
                case "41"://Kiem tra quyền cập nhật du liệu vao sổ theo dõi                    
                    strValue = CheckQuyenCapNhat();
                    break;
                case "42"://Kiem tra quyền cập nhật du liệu vao sổ theo dõi                    
                    strValue = CheckQuyenXoa();
                    break;
                #endregion
                #region PhanViec
                case "45":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                    {
                        string user = context.Request.QueryString["user"];
                        string listID = context.Request.QueryString["listID"];
                        strValue = QLKN_KhieuNai_PhanViec(listID, user, infoUser);
                    }
                    break;
                case "46": //Check quyen phan viec cho nguoi dung
                    try
                    {
                        bool value = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng);
                        if (value)
                        {
                            strValue = "1";
                        }
                        else
                        {
                            strValue = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        strValue = ex.Message;
                    }

                    break;
                case "47":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["typeKhieuNai"]))
                    {
                        string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
                        if (typeKhieuNai == "1") //Load danh sách khiếu nại chờ xử lý
                        {
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
                                strValue = PhanViec_GetKhieuNaiChuaXuLy_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "2")//Load danh sách khiếu nại bộ phận khác chuyển về
                        {
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
                                strValue = PhanViec_GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "3")//Load danh sách khiếu nại sắp quá hạn
                        {
                            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                            {
                                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                                string pageSize = context.Request.QueryString["pageSize"].ToString();
                                strValue = PhanViec_GetKhieuNaiSapQuaHan_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "4")//Load danh sách khiếu nại quá hạn
                        {
                            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                            {
                                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                                string pageSize = context.Request.QueryString["pageSize"].ToString();
                                strValue = PhanViec_GetKhieuNaiQuaHan_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                    }
                    break;
                case "48":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["typeKhieuNai"]))
                    {
                        string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
                        if (typeKhieuNai == "1") //Load danh sách khiếu nại chờ xử lý
                        {
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
                                strValue = PhanViec_GetHtmlKhieuNaiChuaXuLy(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "2")//Load danh sách khiếu nại bộ phận khác chuyển về
                        {
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

                                strValue = PhanViec_GetHtmlKhieuNaiBoPhanKhacChuyenVe(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "3")//Load danh sách khiếu nại sắp quá hạn
                        {
                            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                            {
                                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                                string pageSize = context.Request.QueryString["pageSize"].ToString();

                                strValue = PhanViec_GetHtmlKhieuNaiSapQuaHan(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                        else if (typeKhieuNai == "4")//Load danh sách khiếu nại quá hạn
                        {
                            if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                            {
                                string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                                string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                                string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                                string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                                string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                                string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                                string pageSize = context.Request.QueryString["pageSize"].ToString();

                                strValue = PhanViec_GetHtmlKhieuNaiQuaHan(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize);
                            }
                        }
                    }
                    break;
                #endregion
                #region Khiếu Nại Cảnh báo
                case "49":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        strValue = GetKhieuNaiCanhBao_TotalRecords(infoUser.Username, startPageIndex, pageSize);
                    }
                    break;
                case "50":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        strValue = GetHtmlKhieuNaiCanhBao(infoUser.Username, startPageIndex, pageSize);
                    }
                    break;
                #endregion
                #region Lấy thông tin tai khoản theo ten dang nhap
                case "54":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["tenTruyCap"]))
                    {
                        strValue = GetInfoUser(context.Request.QueryString["tenTruyCap"]);
                    }
                    break;
                #endregion
                #region Cap Nhật thành KN Hàng Loạt
                case "59":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                    {                        
                        string listID = context.Request.QueryString["listID"];
                        strValue = QLKN_UpdateKhieuNaiToHangLoat(listID);
                    }
                    break;
                #endregion

            }
            return strValue;
        }
        #region Dung Chung
        private string GetInfoUser(string tenTruyCap)
        {
            try
            {
                string strValues = "";
                List<NguoiSuDungInfo> list = _NguoiSuDungImpl.NguoiSuDung_GetInfoNguoiSuDungByTenTruyCap(tenTruyCap);
                if (list.Count > 0)
                {
                    foreach (NguoiSuDungInfo info in list)
                    {

                        strValues += "<table cellpadding=\"0\" cellspacing=\"0\" width=\"300px\" style=\"white-space: nowrap; font-size: 12px;\">";
                        strValues += "            <tr style=\"line-height: 25px;\">";
                        strValues += "                <td colspan=\"2\">";
                        strValues += "                    <div style=\"font: 14px; font-weight: bold; color: #4D709A;border-bottom: 1pt solid #B2D4E6;margin-bottom:10px;\">Thông tin " + tenTruyCap + "</div>";
                        strValues += "                </td>";
                        strValues += "            </tr>";
                        strValues += "            <tr>";
                        strValues += "                <td>";
                        strValues += "                    <strong>Tên đầy đủ:</strong>";
                        strValues += "                </td>";
                        strValues += "                <td>";
                        strValues += "                    <span class=\"info\">" + info.TenDayDu + "</span>";
                        strValues += "                </td>";
                        strValues += "            </tr>";
                        strValues += "            <tr>";
                        strValues += "                <td>";
                        strValues += "                    <strong>Tên đối tác:</strong>";
                        strValues += "                </td>";
                        strValues += "                <td>";
                        strValues += "                    <span class=\"info\">" + info.TenDoiTac.TrimStart() + "</span>";
                        strValues += "                </td>";
                        strValues += "            </tr>";
                        strValues += "            <tr>";
                        strValues += "				<td>";
                        strValues += "                    <strong>Di động:</strong>";
                        strValues += "                </td>";
                        strValues += "                <td>";
                        strValues += "                    <span class=\"info\">" + info.DiDong + "</span>";
                        strValues += "                </td>";
                        strValues += "            </tr>";
                        strValues += "            <tr>";
                        strValues += "				<td>";
                        strValues += "                    <strong>Email:</strong>";
                        strValues += "                </td>";
                        strValues += "                <td>";
                        strValues += "                    <span class=\"info\">" + info.Email + "</span>";
                        strValues += "                </td>";
                        strValues += "            </tr>";
                        strValues += "        </table>";
                    }
                }
                else
                {
                    strValues = "Chưa có thông tin";
                }

                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetDanhSachUser()
        {
            string strVales = "";
            strVales += "{";

            strVales += "}";
            return strVales;
        }
        private string LoadPhongBanChuyenDen(AdminInfo infoUser)
        {
            try
            {
                string strValues = "";
                if (infoUser != null)
                {
                    string strValuePhongBan = "";
                    List<PhongBanInfo> lst = _PhongBanImpl.QLKN_PhongBanGetAll();
                    List<PhongBanInfo> listLoadPhongBan = new List<PhongBanInfo>();
                    List<PhongBan2PhongBanInfo> listPhongBan = _PhongBan2PhongBanImpl.GetListByPhongBanId(infoUser.PhongBanId);
                    if (listPhongBan.Count > 0)
                    {
                        foreach (PhongBan2PhongBanInfo item in listPhongBan)
                        {
                            strValuePhongBan = item.PhongBanDen;
                        }
                    }
                    if (strValuePhongBan != "")
                    {
                        string[] words = null;
                        if (strValuePhongBan.IndexOf(",") != -1)
                        {
                            words = strValuePhongBan.Replace("[", "").Replace("]", "").Split(',');
                            if (words.Length > 0)
                            {
                                foreach (string word in words)
                                {
                                    if (!string.IsNullOrEmpty(word))
                                    {
                                        var results = lst.Where(s => s.Id == Convert.ToInt32(word)).ToList();
                                        listLoadPhongBan.AddRange(results);
                                    }

                                }
                            }
                        }
                        else
                        {
                            var results = lst.Where(s => s.Id == Convert.ToInt32(strValuePhongBan.Replace("[", "").Replace("]", ""))).ToList();
                            listLoadPhongBan.AddRange(results);
                        }

                    }
                    if (listLoadPhongBan.Count > 0)
                    {

                        strValues += "<option value=\"-1\">--Chuyển bộ phận khác--</option>";
                        foreach (var item in listLoadPhongBan)
                        {
                            strValues += "<option value=\"" + item.Id + "\">" + item.Name + "</option>";

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
        private string LoadGhiChuTrangThaiXuLy()
        {
            try
            {
                string strValues = "";
                strValues += "<p style=\"border: 1pt solid #CCC; background:" + GetColorTrangThaiXuLy((int)KhieuNai_TrangThai_Type.Chờ_xử_lý) + "; width: 22px; height: 13px;float:left;\"></p>";
                strValues += "<span style=\"color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;\">Chờ xử lý</span>";

                strValues += "<p style=\"border: 1pt solid #CCC; background:" + GetColorTrangThaiXuLy((int)KhieuNai_TrangThai_Type.Đang_xử_lý) + "; width: 22px; height: 13px;float:left;\"></p>";
                strValues += "<span style=\"color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;\">Đang xử lý</span>";

                strValues += "<p style=\"border: 1pt solid #CCC; background:" + GetColorTrangThaiXuLy((int)KhieuNai_TrangThai_Type.Chờ_đóng) + "; width: 22px; height: 13px;float:left;\"></p>";
                strValues += "<span style=\"color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;\">Chờ đóng</span>";

                strValues += "<p style=\"border: 1pt solid #CCC; background:" + GetColorTrangThaiXuLy((int)KhieuNai_TrangThai_Type.Đóng) + "; width: 22px; height: 13px;float:left;\"></p>";
                strValues += "<span style=\"color: #4D709A; font-size: 12px; font-weight: bold;float:left;padding-left:5px;padding-right:5px;\">KN đã đóng</span>";
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetColorTrangThaiXuLy(int trangThaiXuLy)
        {
            try
            {
                string strValues = "";
                switch (trangThaiXuLy)
                {
                    case (int)KhieuNai_TrangThai_Type.Chờ_xử_lý:
                        strValues = "#FF0000";
                        break;
                    case (int)KhieuNai_TrangThai_Type.Đang_xử_lý:
                        strValues = "#FFFF00";
                        break;

                    case (int)KhieuNai_TrangThai_Type.Chờ_đóng:
                        strValues = "#0095CC";
                        break;

                    case (int)KhieuNai_TrangThai_Type.Đóng:
                        strValues = "#088A08";
                        break;
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetItemDropListDoUuTien()
        {
            try
            {
                string strValues = "";
                strValues += "<option value=\"-1\">--Độ ưu tiên--</option>";
                foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
                {
                    strValues += "<option value=\"" + i.ToString() + "\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ") + "</option>";
                }

                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetItemDropListTrangThai(string lstValues)
        {
            try
            {
                string strValues = "";
                string[] words = lstValues.Split('#');
                strValues += "<option value=\"-1\">--Trạng thái--</option>";
                foreach (string word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        strValues += "<option value=\"" + word + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), Convert.ToInt32(word)).Replace("_", " ") + "</option>";
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
        private string GetItemDropListLoaiKhieuNai(int PhongBanId)
        {
            try
            {
                string strValues = "";

                if (PhongBanId != 0)
                {
                    DataTable tab = _LoaiKhieuNaiImpl.QLKN_LoaiKhieuNai_ByPhongBanId(PhongBanId);
                    strValues += "<option value=\"-1\">--Loại khiếu nại--</option>";
                    if (tab.Rows.Count > 0)
                    {
                        foreach (DataRow info in tab.Rows)
                        {
                            strValues += "<option value=\"" + info["LoaiKhieuNaiId"] + "\">" + info["LoaiKhieuNai"] + "</option>";
                        }
                    }
                }
                else
                {
                    List<LoaiKhieuNaiInfo> lstLoaiKN = new List<LoaiKhieuNaiInfo>();
                    lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
                    strValues += "<option value=\"-1\">--Loại khiếu nại--</option>";
                    if (lstLoaiKN.Count > 0)
                    {
                        foreach (LoaiKhieuNaiInfo info in lstLoaiKN)
                        {
                            strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
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
        private string GetItemDropListLinhVucChung(string loaiKhieuNaiID)
        {
            try
            {
                string strValues = "";
                var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + loaiKhieuNaiID, "Sort");
                strValues += "<option value=\"-1\">--Lĩnh vực chung--</option>";
                if (lstLinhVucChung.Count > 0)
                {
                    foreach (LoaiKhieuNaiInfo info in lstLinhVucChung)
                    {
                        strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
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
        private string GetItemDropListLinhVucCon(string lihVucChungId)
        {
            try
            {
                string strValues = "";
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + lihVucChungId, "Sort");
                strValues += "<option value=\"-1\">--Lĩnh vực con--</option>";
                if (lstLinhVucCon.Count > 0)
                {
                    foreach (LoaiKhieuNaiInfo info in lstLinhVucCon)
                    {
                        strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
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
        private string GetItemDropListPhongBanXuLy()
        {
            try
            {
                string strValues = "";

                List<PhongBanInfo> lst = new List<PhongBanInfo>();
                lst = ServiceFactory.GetInstancePhongBan().GetList();
                var newList = lst.OrderBy(x => x.Name).ToList();
                strValues += "<option value=\"-1\">--Phòng ban xử lý--</option>";
                if (newList.Count > 0)
                {
                    foreach (PhongBanInfo info in newList)
                    {
                        strValues += "<option value=\"" + info.Id + "\">" + info.Name + "</option>";
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
        private string GetListFileDinhKem(string id, int Status, int view)
        {
            try
            {
                string strValues = "";
                KhieuNai_FileDinhKemImpl _KhieuNai_FileDinhKemImpl = new KhieuNai_FileDinhKemImpl();
                List<KhieuNai_FileDinhKemInfo> list = _KhieuNai_FileDinhKemImpl.GetListByKhieuNaiId(Convert.ToInt32(id));
                if (list.Count > 0)
                {
                    //string pathUrlFile = ConfigurationManager.AppSettings["PathUrlFile"];
                    string domainDownload = AIVietNam.Core.Config.DomainDownload;
                    if (Status == (int)FileDinhKem_Status.File_KH_Gửi)
                    {
                        var results = list.Where(s => s.Status == (int)FileDinhKem_Status.File_KH_Gửi).ToList();
                        if (results.Count > 0)
                        {
                            foreach (var info in results)
                            {
                                if (view != 0)
                                {
                                    strValues += "<span id='fileId-" + info.Id + "'><a class='delete-file' style='color: Red;padding-right:5px;' href='javascript:DeleteFile(" + info.Id + ")'><img src ='/images/icons/del_file.png' /></a><a href ='" + domainDownload + info.URLFile + "'>" + info.TenFile + "</a> <br /></span>";
                                }
                                else
                                {
                                    strValues += "<a href ='" + domainDownload + info.URLFile + "'>" + info.TenFile + "</a> <br />";
                                }
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
                                if (view != 0)
                                {
                                    strValues += "<span id='fileId-" + info.Id + "'><a class='delete-file' style='color: Red;padding-right:5px;' href='javascript:DeleteFile(" + info.Id + ")'><img src ='/images/icons/del_file.png' /></a><a href ='" + domainDownload + info.URLFile + "'>" + info.TenFile + "</a> <br /></span>";
                                }
                                else
                                {
                                    strValues += "<a href ='" + domainDownload + info.URLFile + "'>" + info.TenFile + "</a> <br />";
                                }
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
        private KhieuNaiChiTietInfo GetInfoKhieuNaiByID(string id, string view)
        {
            try
            {
                KhieuNaiChiTietInfo KNChiTietInfo = new KhieuNaiChiTietInfo();
                //string strData = "";
                KhieuNaiInfo info = _KhieuNaiImpl.QLKN_KhieuNaigetByID(Convert.ToInt32(id));
                if (info != null)
                {
                    KNChiTietInfo.Id = info.Id.ToString();
                    KNChiTietInfo.MaKhieuNai = GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)info.Id, 10);
                    KNChiTietInfo.KhuVucId = info.KhuVucId.ToString();
                    KNChiTietInfo.DoiTacId = info.DoiTacId.ToString();
                    KNChiTietInfo.PhongBanTiepNhanId = info.PhongBanTiepNhanId.ToString();
                    KNChiTietInfo.PhongBanXuLyId = info.PhongBanXuLyId.ToString();
                    KNChiTietInfo.LoaiKhieuNaiId = info.LoaiKhieuNaiId.ToString();
                    KNChiTietInfo.LinhVucChungId = info.LinhVucChungId.ToString();
                    KNChiTietInfo.LinhVucConId = info.LinhVucConId.ToString();
                    KNChiTietInfo.LoaiKhieuNai = info.LoaiKhieuNai;
                    KNChiTietInfo.LinhVucChung = info.LinhVucChung;
                    KNChiTietInfo.LinhVucCon = info.LinhVucCon;
                    if (!string.IsNullOrEmpty(info.DoUuTien.ToString()) && info.DoUuTien != 0)
                    {
                        KNChiTietInfo.DoUuTien = Enum.GetName(typeof(KhieuNai_DoUuTien_Type), info.DoUuTien).Replace("_", " ");
                    }
                    KNChiTietInfo.SoThueBao = info.SoThueBao.ToString();
                    KNChiTietInfo.FileDinhKemKH = GetListFileDinhKem(id, (int)FileDinhKem_Status.File_KH_Gửi, Convert.ToInt32(view));
                    KNChiTietInfo.FileDinhKemGQKN = GetListFileDinhKem(id, (int)FileDinhKem_Status.File_GQKN_Gửi, Convert.ToInt32(view));
                    KNChiTietInfo.MaTinh = info.MaTinh;
                    KNChiTietInfo.HoTenLienHe = info.HoTenLienHe;
                    KNChiTietInfo.DiaChiLienHe = info.DiaChiLienHe;
                    KNChiTietInfo.SDTLienHe = info.SDTLienHe;
                    KNChiTietInfo.DiaDiemXayRa = info.DiaDiemXayRa;
                    KNChiTietInfo.ThoiGianXayRa = info.ThoiGianXayRa;
                    KNChiTietInfo.NoiDungPA = info.NoiDungPA;

                    KNChiTietInfo.NoiDungCanHoTro = info.NoiDungCanHoTro;
                    if (info.TrangThai == (int)KhieuNai_TrangThai_Type.Chờ_xử_lý || info.TrangThai == (int)KhieuNai_TrangThai_Type.Đang_xử_lý)
                    {
                        KNChiTietInfo.TrangThai = "Chờ xử lý";
                    }
                    else if (info.TrangThai == (int)KhieuNai_TrangThai_Type.Chờ_đóng)
                    {
                        KNChiTietInfo.TrangThai = "Đang xử lý";
                    }
                    else if (info.TrangThai == (int)KhieuNai_TrangThai_Type.Đóng)
                    {
                        KNChiTietInfo.TrangThai = "Đóng";
                    }
                    KNChiTietInfo.IsChuyenBoPhan = info.IsChuyenBoPhan.ToString();
                    KNChiTietInfo.NguoiTiepNhan = info.NguoiTiepNhan.ToString();
                    KNChiTietInfo.HTTiepNhan = info.HTTiepNhan.ToString();
                    if (info.NgayTiepNhan != null)
                    {
                        KNChiTietInfo.NgayTiepNhan = info.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayTiepNhan = "";
                    }

                    KNChiTietInfo.NguoiTienXuLyCap1 = info.NguoiTienXuLyCap1;
                    KNChiTietInfo.NguoiTienXuLyCap2 = info.NguoiTienXuLyCap2;
                    KNChiTietInfo.NguoiTienXuLyCap3 = info.NguoiTienXuLyCap3;
                    KNChiTietInfo.NguoiXuLy = info.NguoiXuLy;
                    if (info.NgayQuaHan != null)
                    {
                        KNChiTietInfo.NgayQuaHan = info.NgayQuaHan.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayQuaHan = "";
                    }

                    if (info.NgayCanhBao != null)
                    {
                        KNChiTietInfo.NgayCanhBao = info.NgayCanhBao.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayCanhBao = "";
                    }

                    if (info.NgayChuyenPhongBan != null)
                    {
                        KNChiTietInfo.NgayChuyenPhongBan = info.NgayChuyenPhongBan.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayChuyenPhongBan = "";
                    }
                    if (info.NgayQuaHanPhongBanXuLy != null)
                    {
                        KNChiTietInfo.NgayQuaHanPhongBanXuLy = info.NgayQuaHanPhongBanXuLy.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayQuaHanPhongBanXuLy = "";
                    }
                    if (info.NgayTraLoiKN != null)
                    {
                        KNChiTietInfo.NgayTraLoiKN = info.NgayTraLoiKN.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        KNChiTietInfo.NgayTraLoiKN = "";
                    }

                    KNChiTietInfo.NgayDongKN = "";
                    KNChiTietInfo.NgayDongKNSort = "";

                    KNChiTietInfo.KQXuLy_SHCV = info.KQXuLy_SHCV;
                    KNChiTietInfo.KQXuLy_CCT = info.KQXuLy_CCT.ToString();
                    KNChiTietInfo.KQXuLy_CSL = info.KQXuLy_CSL.ToString();
                    KNChiTietInfo.KQXuLy_PTSL_IR = info.KQXuLy_PTSL_IR.ToString();
                    KNChiTietInfo.KQXuLy_PTSL_Khac = info.KQXuLy_PTSL_Khac;
                    KNChiTietInfo.KetQuaXuLy = info.KetQuaXuLy;
                    KNChiTietInfo.NoiDungXuLy = info.NoiDungXuLy;
                    KNChiTietInfo.GhiChu = info.GhiChu;
                    KNChiTietInfo.KNHangLoat = info.KNHangLoat.ToString();
                    KNChiTietInfo.SoTienKhauTru_TKC = info.SoTienKhauTru_TKC.ToString();
                    KNChiTietInfo.SoTienKhauTru_KM1 = info.SoTienKhauTru_KM1.ToString();
                    KNChiTietInfo.SoTienKhauTru_KM2 = info.SoTienKhauTru_KM2.ToString();
                    KNChiTietInfo.SoTienKhauTru_KM3 = "";
                    KNChiTietInfo.SoTienKhauTru_KM4 = "";
                    KNChiTietInfo.SoTienKhauTru_KM5 = "";
                    KNChiTietInfo.IsLuuKhieuNai = info.IsLuuKhieuNai.ToString();
                    KNChiTietInfo.CDate = info.CDate.ToString("dd/MM/yyyy HH:mm:ss");
                    KNChiTietInfo.CUser = info.CUser;
                    KNChiTietInfo.LDate = info.LDate.ToString("dd/MM/yyyy HH:mm:ss");
                    KNChiTietInfo.LUser = info.LUser;

                    
                }
                return KNChiTietInfo;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        private string GetNameLoaiKhieuNai(string id)
        {
            try
            {
                string strValues = "";
                LoaiKhieuNaiInfo info = _LoaiKhieuNaiImpl.QLKN_LoaiKhieuNaigetByID(Convert.ToInt32(id));
                if (info != null)
                {
                    strValues = info.Name;
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetNamePhongBan(string id)
        {
            try
            {
                string strValues = "";
                if (!string.IsNullOrEmpty(id))
                {

                    PhongBanInfo info = _PhongBanImpl.QLKN_PhongBangetByID(Convert.ToInt32(id));
                    if (info != null)
                    {
                        strValues = info.Name;
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
        private string BindInfoTraTruocToForm(TBTraTruocFullInfo info)
        {
            string strValues = "";
            strValues += info.FULLNAME + "#";
            strValues += info.ADDRESS;
            return strValues;
        }

        private string BindInfoTraSauToForm(TBFullInfo info)
        {
            string strValues = "";
            strValues += info.TEN_TB + "#";
            strValues += info.DIACHI_TT;
            return strValues;
        }
        private string BindThongTinThueBao(string dauSo, string stb)
        {
            try
            {
                string strValues = "";
                var admin = LoginAdmin.AdminLogin();

                var Impl = new SubinfoImpl();
                var ImplWS = new SubinfoTSImpl();
                BasicInfoFromSubinfo basicInfo = Impl.getBasicInfo(dauSo + stb, admin.Username, Utility.GetIP(), "");
                var strJSON = "";
                if (basicInfo != null)
                {
                    var infoTB = ImplWS.VNP_TRACUU_TB(dauSo + stb);

                    if (basicInfo.MA_TINH != "GPC")
                    {
                        var infoTBTH = ImplWS.VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TB(basicInfo.MA_TINH, dauSo + stb);
                        if (infoTBTH != null)
                        {

                            var infoFull = new TBFullInfo(infoTBTH, basicInfo, infoTB);
                            if (basicInfo.TEN_LOAI == "Post" || basicInfo.TEN_LOAI.ToLower().Contains("itouch"))
                            {
                                strValues = BindInfoTraSauToForm(infoFull);
                            }
                            else
                            {
                                var infoTBTraTruoc = ImplWS.PrepaidSubscriberInfo(dauSo + stb);
                                var tbnot84 = dauSo;

                                var infoTaiKhoan = ImplWS.getSubscriber(new Random().Next(111111, 999999), tbnot84, DateTime.Now.ToString("yyyyMMddHHmmss"));
                                var infoTTFull = new TBTraTruocFullInfo(infoTBTraTruoc, infoFull, infoTaiKhoan);
                                strValues = BindInfoTraTruocToForm(infoTTFull);
                            }
                        }
                    }
                    else
                    {
                        var infoFull = new TBFullInfo(null, basicInfo, infoTB);
                        strValues = BindInfoTraSauToForm(infoFull);
                    }
                    //}
                }
                else
                {
                    var infoTB = ImplWS.VNP_TRACUU_TB(dauSo + stb);
                    if (infoTB != null)
                    {
                        if (infoTB.MA_TINH != "GPC")
                        {
                            var infoTBTH = ImplWS.VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TB(infoTB.MA_TINH, dauSo + stb);
                            if (infoTBTH != null)
                            {
                                var infoFull = new TBFullInfo(infoTBTH, basicInfo, infoTB);

                                var infoTBTraTruoc = ImplWS.PrepaidSubscriberInfo(dauSo + stb);
                                var tbnot84 = dauSo;

                                var infoTaiKhoan = ImplWS.getSubscriber(new Random().Next(111111, 999999), tbnot84, DateTime.Now.ToString("yyyyMMddHHmmss"));
                                var infoTTFull = new TBTraTruocFullInfo(infoTBTraTruoc, infoFull, infoTaiKhoan);
                                strValues = BindInfoTraSauToForm(infoTTFull);

                            }
                        }
                        else
                        {
                            var infoTTFull = new TBTraTruocFullInfo() { TEN_LOAI = "Test", MA_TINH = "GPC" };
                            strValues = BindInfoTraTruocToForm(infoTTFull);
                        }
                    }
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

      
        public string QLKN_KhieuNaiDongKN(int id, string GhiChu)
        {
            var item = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(id));

            if (item != null)
            {
                var activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(item.Id);
                activityCurr.ActivityTruoc = activityCurr.Id;
                activityCurr.GhiChu = GhiChu;
                activityCurr.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Đóng_KN;
                activityCurr.IsCurrent = true;
                activityCurr.NguoiDuocPhanHoi = string.Empty;

                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                buocXuLyInfo.NoiDung = "Đóng khiếu nại.";
                buocXuLyInfo.LUser = item.NguoiXuLy;
                buocXuLyInfo.KhieuNaiId = item.Id;
                buocXuLyInfo.IsAuto = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { LUser = item.NguoiXuLy, NoiDung = GhiChu, IsAuto = false, KhieuNaiId = item.Id });

                        ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',NgayDongKN=getdate(),NgayDongKNSort=" + DateTime.Now.ToString("yyyyMMdd") + ", TrangThai=" + (byte)KhieuNai_TrangThai_Type.Đóng, "Id=" + id);

                        ServiceFactory.GetInstanceKhieuNai_Activity().UpdateDynamic("LDate=getdate(),IsCurrent=0", "IsCurrent=1 AND KhieuNaiId=" + item.Id);
                        ServiceFactory.GetInstanceKhieuNai_Activity().Add(activityCurr);

                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(id), "Đóng khiếu nại", "Trạng thái", Enum.GetName(typeof(KhieuNai_TrangThai_Type), item.TrangThai).Replace("_", " "), Enum.GetName(typeof(KhieuNai_TrangThai_Type), (byte)KhieuNai_TrangThai_Type.Đóng).Replace("_", " "));

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

            }
            return "1";
        }


        #endregion
        #region LoaiKhieuNai
        private string GetTotalRecords(int PhongBanId, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _LoaiKhieuNaiImpl.QLKN_LoaiKhieuNai_GetAllWithPadding_TotalRecords(PhongBanId, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetQLKN_LoaiKhieuNai_TongSoKhieuNai(int PhongBanId, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _LoaiKhieuNaiImpl.QLKN_LoaiKhieuNai_GetAllWithPadding_TongSoKhieuNai(PhongBanId, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlLoaiKhieuNai(int PhongBanId, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _LoaiKhieuNaiImpl.QLKN_LoaiKhieuNai_GetAllWithPadding(PhongBanId, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowB\">";
                        }

                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"].ToString() + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"].ToString() + "</td>";
                        //strData += "        <td class =\"nowrap\" align=\"left\"><a href =\"/Views/QLKhieuNai/QuanLyKhieuNai.aspx?tab=tab1&ctrl=KNChoXuLy&catid=" + row["ID"].ToString() + "\"><span style =\"margin-left:10px;\">" + row["LinhVucChung"].ToString() + "</span></a></td>";
                        strData += "        <td align=\"left\"><span style =\"margin-left:10px;\">" + row["LinhVucChung"].ToString() + "</span></td>";


                        if (!string.IsNullOrEmpty(row["Total"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"QuanLyKhieuNai.aspx?ctrl=tab0-BieuDoKNChoXuLy&LoaiKhieuNaiId=" + row["LoaiKhieuNaiId"] + "&LinhVucChungId=" + row["LinhVucChungId"].ToString() + "\">" + Convert.ToInt32(row["Total"]).ToString("#,##0") + "</a></td>";
                            //strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToInt32(row["Total"]).ToString("#,##0") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">0</td>";
                        }
                        strData += " </tr>";

                    }
                    //strData += "<tr class=\"rowB\"><td colspan =\"3\" align=\"right\">Tổng số</td><td id =\"tongsokhieunai\" align=\"center\">1000</td></tr>";
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"4\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region KhieuNaiChuaXuLyTongHop
        private string QLKN_GetTotalKhieuNaiChuaXuLyTongHop(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiChuaXuLyTongHop_GetAllWithPadding(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiChuaXuLyTongHop_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiChuaXuLyTongHop(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
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
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));


                if (tab.Rows.Count > 0)
                {
                    int temp = 0;

                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab6-KNTongHopChoXuLy" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                    var admin = LoginAdmin.AdminLogin();
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }

                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {

                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";

                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion
        #region KhieuNaiChuaXuLy
        private string QLKN_GetTotalKhieuNaiChuaXuLy(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiChuaXuLy_GetAllWithPadding(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiChuaXuLy_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiChuaXuLy(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
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
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));


                if (tab.Rows.Count > 0)
                {
                    int temp = 0;

                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab1-KNChoXuLy" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                    var admin = LoginAdmin.AdminLogin();
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }

                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion
        #region Khieu Nai Chuyen Bo Phan Khac
        private string QLKN_GetTotalKhieuNaiChuyenBoPhanKhac(int PhongBanId)
        {
            try
            {

                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiChuyenBoPhanKhac_GetAllWithPadding(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiChuyenBoPhanKhac_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId,string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(phongBanXuLy),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                    KNHangLoat, GetAllKN, DoiTacId, isPermission,
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
        private string GetHtmlKhieuNaiChuyenBoPhanKhac(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string phongBanXuLy, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   Convert.ToInt32(LinhVucConId),
                   Convert.ToInt32(phongBanXuLy),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To,
                   KNHangLoat, GetAllKN, DoiTacId, isPermission,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNChuyenBoPhanKhac" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        //strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLy"] + "'>" + row["NguoiTienXuLy"] + "</a></td>";                        
                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";                        
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai BoPhanKhacChuyenVe
        private string QLKN_GetTotalKhieuNaiBoPhanKhacChuyenVe(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
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
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    var admin = LoginAdmin.AdminLogin();
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab3-KNBoPhanKhacChuyenVe" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiBoPhanKhacChuyenVeCheckBox(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
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
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab3-KNBoPhanKhacChuyenVe" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                    var admin = LoginAdmin.AdminLogin();
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai SapQuaHan
        private string QLKN_GetTotalKhieuNaiSapQuaHan(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiSapQuaHan_GetAllWithPadding(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiSapQuaHan_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiSapQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHan_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
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
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab4-KNSapQuaHan" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                    var admin = LoginAdmin.AdminLogin();
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";
                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiSapQuaHanCheckBox(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId,
            string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHan_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
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
                if (tab.Rows.Count > 0)
                {
                    int temp = 0; var admin = LoginAdmin.AdminLogin();
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab4-KNSapQuaHan" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";

                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai QuaHan
        private string QLKN_GetTotalKhieuNaiQuaHan(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiQuaHan_GetAllWithPadding(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiQuaHan_TotalRecords(string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiQuaHan(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId,
            string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiQuaHan_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
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
                if (tab.Rows.Count > 0)
                {
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, -1, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab5-KNCanDong" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                    int temp = 0; var admin = LoginAdmin.AdminLogin();
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != admin.Username)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";

                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai Da Phan Hoi
        private string QLKN_GetTotalKhieuNaiDaPhanHoi(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiDaPhanHoi_GetAllWithPadding(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
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
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords(contentSeach, Convert.ToInt32(TypeSearch),
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiDaPhanHoi(HttpContext context, string contentSeach, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string ShowNguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding(contentSeach, Convert.ToInt32(TypeSearch),
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
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    string strReturnURL = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}&Show={16}", TypeSearch, LoaiKhieuNaiId, LinhVucChungId, LinhVucConId, DoUuTien, trangThai, startPageIndex, pageSize, contentSeach, SoThueBao, NguoiTiepNhan, NguoiXuLy_Filter, NgayTiepNhan_From, NgayTiepNhan_To, NgayQuaHan_From, NgayQuaHan_To, ShowNguoiXuLy);
                    strReturnURL = context.Request.UrlReferrer.LocalPath + "?ctrl=tab2-KNDaPhanHoi" + strReturnURL;
                    strReturnURL = HttpUtility.UrlEncode(strReturnURL);

                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        //strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + strReturnURL + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"16\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region PhanViec
        private string QLKN_KhieuNai_PhanViec(string listID, string userName, AdminInfo infoUser)
        {
            try
            {
                string strValues = "";
                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        KhieuNaiInfo info = _KhieuNaiImpl.QLKN_KhieuNaigetByID(Convert.ToInt32(item));
                        if (info != null)
                        {
                            if (string.IsNullOrEmpty(info.NguoiXuLy))
                            {
                                strValues = _KhieuNaiImpl.QLKN_KhieuNaiUpdatePhanViec(Convert.ToInt32(item), userName).ToString();
                                _KhieuNai_ActivityImpl.QLKN_KhieuNai_ActivityUpdatePhanViec(Convert.ToInt32(item), userName);
                                LichSuPhanViecInfo lsInfo = new LichSuPhanViecInfo();

                                lsInfo.KhieuNaiId = Convert.ToInt32(item);
                                lsInfo.NguoiDuocPhanViec = userName;
                                lsInfo.PhongBanXuLyId = infoUser.PhongBanId;
                                lsInfo.LUser = infoUser.Username;
                                ServiceFactory.GetInstanceLichSuPhanViec().Add(lsInfo);
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
        #region KhieuNaiChuaXuLy
        private string PhanViec_QLKN_GetTotalKhieuNaiChuaXuLy(int PhongBanId)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_GetTotalKhieuNaiChuaXuLyPhanViec_GetAllWithPadding(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string PhanViec_GetKhieuNaiChuaXuLy_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
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
        private string PhanViec_GetHtmlKhieuNaiChuaXuLy(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }

                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";
                        //string s = row["NguoiXuLy"].ToString();
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"14\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion
        #region Khieu Nai BoPhanKhacChuyenVe

        private string PhanViec_GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
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

        private string PhanViec_GetHtmlKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   Convert.ToInt32(LinhVucConId),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"13\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai SapQuaHan

        private string PhanViec_GetKhieuNaiSapQuaHan_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
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

        private string PhanViec_GetHtmlKhieuNaiSapQuaHan(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),

                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";

                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"13\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu Nai QuaHan
        private string PhanViec_GetKhieuNaiQuaHan_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
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
        private string PhanViec_GetHtmlKhieuNaiQuaHan(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" disabled=\"disabled\" /></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input class=\"checkbox-item\" name=\"item\" value=\"" + row["ID"] + "\" id =\"checkbox" + row["ID"] + "\" type =\"checkbox\" /></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><span style=\"border: 1pt solid #CCC; background: " + GetColorTrangThaiXuLy(Convert.ToInt32(row["TrangThai"])) + "; width: 15px; height: 10px;\"></span></td>";

                        //strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        if (!string.IsNullOrEmpty(row["NguoiXuLy"].ToString()))
                        {
                            if (row["NguoiXuLy"].ToString() != NguoiXuLy)
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=View\" style=\"\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\" /Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["ID"] + "&ReturnUrl=" + "/Views" + Regex.Split(context.Request.UrlReferrer.ToString(), "Views")[1] + "&Mode=Process\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), row["DoUuTien"]).Replace("_", " ") + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LoaiKhieuNai"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucChung"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["LinhVucCon"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiXuLy"] + "'>" + row["NguoiXuLy"] + "</a></td>";
                        if (!string.IsNullOrEmpty(row["IsPhanViec"].ToString()) && row["IsPhanViec"].ToString() == "True")
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><input type=\"checkbox\" disabled=\"disabled\" id=\"checkbox-phanviec\" checked =\"yes\"></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        if (!string.IsNullOrEmpty(row["NgayQuaHanPhongBanXuLy"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayQuaHanPhongBanXuLy"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";
                        strData += " </tr>";

                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"13\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #endregion

        #region Chuyen Khieu Nai

        private string QLKN_UpdateKhieuNaiToHangLoat(string listID)
        {
                        
            try
            {
                if (!string.IsNullOrEmpty(listID))
                {
                    listID = listID.Substring(0, listID.Length - 1);
                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("LDate=getdate(),LUser=N'" + LoginAdmin.AdminLogin().Username + "',KNHangLoat=1", "Id in " + "(" + listID + ")");
                }               
                return "1";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
           
        }
        

        private string QLKN_DongKhieuNai(string listID, string Note)
        {
            try
            {
                string strValues = "";
                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        strValues = QLKN_KhieuNaiDongKN(Convert.ToInt32(item), Note);
                        if (strValues == "-1")
                        {
                            break;
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
        /// <summary>
        /// Chuyen Xu Lý
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="phongBanID"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        private string QLKN_ChuyenXuLy(string listID, int phongBanID, string username, string Note)
        {
            try
            {

                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        BuildKhieuNai_Activity.ActivityChuyenPhongBanToUserInPhongBan(Convert.ToInt32(item), phongBanID, username, KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban, Note);
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "0";
            }
        }
        private string QLKN_ChuyenPhanHoi(string listID, string Note)
        {
            try
            {

                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        BuildKhieuNai_Activity.ActivityChuyenPhanHoi(Convert.ToInt32(item), Note);
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "0";
            }
        }
        private string QLKN_ChuyenNgangHang(string listID, string Note)
        {
            try
            {

                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        BuildKhieuNai_Activity.ActivityChuyenNgangHang(Convert.ToInt32(item), Note);
                    }
                }
                return "1";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "0";
            }
        }
        #endregion
        #region SoTheoDoi
        private string CapNhatSoTheoDoi(string maKhieuNai, AdminInfo infoUser, string soThueBao, string HoTenLienHe, string DiaChiLienHe,
            string NoiDungPA, string NoiDungXuLy, string NgayTiepNhan, string NgayTraLoiKN, string KetQuaXuLy, string GhiChu, string LoaiKhieuNai, string LinhVucChung, string LinhVucCon)
        {

            int strValues = 0;
            try
            {
                var timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();
                var listLoaiKhieuNai = LoaiKhieuNai.Split('#');
                KhieuNaiInfo item = new KhieuNaiInfo();

                item.LUser = infoUser.Username;
                item.SoThueBao = Convert.ToInt64(soThueBao);

                item.KhuVucId = infoUser.KhuVucId;
                item.DoiTacId = infoUser.DoiTacId;
                item.PhongBanTiepNhanId = infoUser.PhongBanId;
                item.PhongBanXuLyId = infoUser.PhongBanId;
                item.LoaiKhieuNaiId = Convert.ToInt32(listLoaiKhieuNai[0]);
                item.LoaiKhieuNai = listLoaiKhieuNai[1];
                if (LinhVucChung != "-1")
                {
                    var listLinhVucChung = LinhVucChung.Split('#');
                    item.LinhVucChungId = Convert.ToInt32(listLinhVucChung[0]);
                    item.LinhVucChung = listLinhVucChung[1];


                }
                if (LinhVucCon != "-1")
                {
                    var listLinhVucCon = LinhVucCon.Split('#');
                    item.LinhVucConId = Convert.ToInt32(listLinhVucCon[0]);
                    item.LinhVucCon = listLinhVucCon[1];

                }
                item.HoTenLienHe = HoTenLienHe;
                item.DiaChiLienHe = DiaChiLienHe;
                //item.SDTLienHe = txtDienThoai.Text.Trim();
                //item.DiaDiemXayRa = txtDiaDiemSuCo.Text.Trim();
                //item.ThoiGianXayRa = txtThoiGianSuCo.Text.Trim();
                item.NoiDungPA = NoiDungPA;
                //item.NoiDungCanHoTro = txtNoiDungCanHoTro.Text.Trim();
                item.GhiChu = GhiChu;
                //item.TrangThai = (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                item.IsChuyenBoPhan = false;
                item.NguoiTiepNhan = infoUser.Username;
                item.NguoiXuLy = infoUser.Username;
                item.HTTiepNhan = 1;
                var listItemDate = NgayTiepNhan.Split('/');
                DateTime ngayTiepNhan = Convert.ToDateTime(listItemDate[1] + "/" + listItemDate[0] + "/" + listItemDate[2] + " " + DateTime.Now.ToString("HH:mm:ss"));

                item.NgayTiepNhan = ngayTiepNhan;
                item.NgayTiepNhanSort = Convert.ToInt32(ngayTiepNhan.ToString("yyyyMMdd"));

                item.NoiDungXuLy = NoiDungXuLy;
                item.KetQuaXuLy = KetQuaXuLy;

                var listItemDateTraLoi = NgayTraLoiKN.Split('/');
                DateTime ngayTraLoiKN = Convert.ToDateTime(listItemDateTraLoi[1] + "/" + listItemDateTraLoi[0] + "/" + listItemDateTraLoi[2] + " " + DateTime.Now.ToString("HH:mm:ss"));
                item.NgayTraLoiKN = ngayTraLoiKN;
                item.NgayTraLoiKNSort = Convert.ToInt32(ngayTraLoiKN.ToString("yyyyMMdd"));
                item.NgayDongKN = ngayTraLoiKN;
                item.NgayDongKNSort = 0;
                item.IsLuuKhieuNai = true;
                item.DoUuTien = (byte)KhieuNai_DoUuTien_Type.Thông_thường;
                item.TrangThai = (byte)KhieuNai_TrangThai_Type.Đóng;

                if (maKhieuNai != "0")
                {
                    item.Id = Convert.ToInt32(maKhieuNai);
                    try
                    {
                        strValues = ServiceFactory.GetInstanceKhieuNai().Update(item);
                        //scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        return "-1";
                    }
                    //using (TransactionScope scope = new TransactionScope())
                    //{
                    //    try
                    //    {
                    //        strValues = ServiceFactory.GetInstanceKhieuNai().Update(item);
                    //        scope.Complete();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Utility.LogEvent(ex);
                    //        return "-1";
                    //    }
                    //}
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            strValues = ServiceFactory.GetInstanceKhieuNai().Add(item);
                            //Activity
                            KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
                            itemActivity.KhieuNaiId = strValues;
                            itemActivity.ActivityTruoc = 0;
                            itemActivity.GhiChu = "Tạo mới khiếu nại";
                            itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                            itemActivity.IsCurrent = true;
                            itemActivity.NguoiXuLyTruoc = infoUser.Username;
                            itemActivity.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
                            itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
                            itemActivity.NgayTiepNhan = item.NgayTiepNhan;
                            itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                            itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;

                            //Báo lỗi và không thực hiện chức năng

                            ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            return "-1";
                        }
                    }

                }

                return strValues.ToString();

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetKhieuNaiSoTheoDoi_TotalRecords(string Select, string PhongBanId, string SoThueBao, string NguoiXuLy, string NguoiTiepNhan, string ThoiGianTiepNhanTu, string ThoiGianTiepNhanDen,
            bool GetAllKN, int DoiTacId, bool IsPermission, string startPageIndex, string pageSize)
        {
            try
            {
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
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiSoTheoDoi(string userName, string Select, string PhongBanId, string SoThueBao, string NguoiXuLy, string NguoiTiepNhan, string ThoiGianTiepNhanTu, string ThoiGianTiepNhanDen,
            bool GetAllKN, int DoiTacId, bool IsPermission, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
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
                DataTable tab = _KhieuNaiImpl.QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding(Convert.ToInt32(Select), Convert.ToInt32(PhongBanId),
                    Convert.ToInt64(SoThueBao),
                    NguoiXuLy,
                    NguoiTiepNhan,
                    thoiGianTu,
                    thoiGianDem, GetAllKN, DoiTacId, IsPermission,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["ID"] + "\" class=\"rowB\">";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["STT"] + "</td>";
                        if (Convert.ToBoolean(row["IsLuuKhieuNai"].ToString()) && row["NguoiTiepNhan"].ToString() == userName)
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:TaoMoiKhieuNai('" + row["ID"] + "');\" title=\"Chỉnh sửa khiếu nại\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                        }
                        else
                        {
                            if (Convert.ToBoolean(row["IsLuuKhieuNai"].ToString()) && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_dòng_trên_sổ_theo_dõi))
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"javascript:TaoMoiKhieuNai('" + row["ID"] + "');\" title=\"Chỉnh sửa khiếu nại\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</a></td>";
                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\">" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</td>";
                            }
                        }
                        if (!string.IsNullOrEmpty(row["NgayTiepNhan"].ToString()))
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTiepNhan"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["NguoiTiepNhan"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\">" + row["SoThueBao"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["HoTenLienHe"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungPA"] + "</td>";

                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetListFileDinhKem(row["ID"].ToString(), (int)FileDinhKem_Status.File_KH_Gửi, 0) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["NoiDungXuLy"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        if (!string.IsNullOrEmpty(row["NgayTraLoiKN"].ToString()))
                        {
                            if (Convert.ToDateTime(row["NgayTraLoiKN"]).ToString("dd/MM/yyyy") == "31/12/9999")
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\"></td>";

                            }
                            else
                            {
                                strData += "        <td class =\"nowrap\" align=\"center\">" + Convert.ToDateTime(row["NgayTraLoiKN"]).ToString("dd/MM/yyyy HH:mm:ss") + "</td>";
                            }
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"></td>";
                        }

                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["KetQuaXuLy"] + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row["GhiChu"] + "</td>";



                        strData += " </tr>";


                    }
                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"13\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string CheckQuyenThemMoi()
        {
            try
            {
                string strValues = "0";
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tạo_dòng_trên_sổ_theo_dõi))
                {
                    strValues = "1";
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string CheckQuyenCapNhat()
        {
            try
            {
                string strValues = "0";
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_dòng_trên_sổ_theo_dõi))
                {
                    strValues = "1";
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string CheckQuyenXoa()
        {
            try
            {
                string strValues = "0";
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_dòng_trên_sổ_theo_dõi_của_mình))
                {
                    strValues = "1";
                }
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion

        #region Khiếu Nại Cảnh báo
        private string GetKhieuNaiCanhBao_TotalRecords(string userName, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = _KhieuNaiImpl.KhieuNai_KhieuNaiCanhBaoTotalRecords_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string GetHtmlKhieuNaiCanhBao(string userName, string startPageIndex, string pageSize)
        {
            try
            {
                string strData = "";
                DataTable tab = _KhieuNaiImpl.KhieuNai_KhieuNaiCanhBao_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                if (tab.Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Rows)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + row["STT"] + "\" class=\"rowB\">";
                        }
                        strData += "        <td align=\"center\"><a href='/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN=" + row["Id"] + "&Mode=Process&ReturnUrl=/Views/QLKhieuNai/QuanLyKhieuNai.aspx'>" + GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, (object)row["Id"], 10) + "</td>";
                        strData += "        <td align=\"center\"><a href=\"javascript:ShowPoup('" + row["ID"] + "');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">" + row["SoThueBao"].ToString() + "</a></td>";
                        strData += "        <td align=\"center\">" + row["LoaiKhieuNai"].ToString() + "</td>";
                        strData += "        <td align=\"center\">" + row["NguoiXuLyTruoc"].ToString() + "</td>";
                        strData += "        <td align=\"center\">" + row["NguoiXuLy"].ToString() + "</td>";


                        strData += " </tr>";

                    }

                }
                else
                {
                    strData += "<tr class=\"rowB\"><td colspan =\"5\" align=\"center\">Không tìm thây bản ghi nào</td></tr>";
                }
                return strData;
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