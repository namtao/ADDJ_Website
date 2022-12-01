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
using System.Text.RegularExpressions;
using System.Transactions;
using System.Globalization;
using AIVietNam.Core.Provider;
using System.Text;
using Website.Components.Info;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    /// 

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request.QueryString["key"].ToString();
            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            var SortName = context.Request.Form["sortname"];
            var SortOrder = context.Request.Form["sortorder"];
            if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            {

                AdminInfo infoUser = LoginAdmin.AdminLogin();
                if (infoUser != null)
                {
                    if (context.Request.QueryString["key"] == "5")
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["id"]))
                        {
                            string id = context.Request.QueryString["id"].ToString();
                            string view = context.Request.QueryString["view"].ToString();
                            context.Response.Write(JSSerializer.Serialize(GetInfoKhieuNaiByID(id, view, context, infoUser)));
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
                #region LoadDropList
                case "25":
                    strValue = GetItemDropListLoaiKhieuNai(infoUser.PhongBanId);
                    break;
                case "26":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["loaiKhieuNaiId"]))
                    {
                        //if (context.Request.QueryString["loaiKhieuNaiId"].Equals("undefined"))
                        //{
                        //    string logTest = "UrlReferrer: " + context.Request.UrlReferrer + Environment.NewLine + "Url: " + context.Request.Url;
                        //    Utility.LogEvent(logTest);
                        //}
                        //else
                        //{
                        if (!context.Request.QueryString["loaiKhieuNaiId"].Equals("undefined"))
                        {
                            strValue = GetItemDropListLinhVucChung(context.Request.QueryString["loaiKhieuNaiId"]);
                        }
                        //}
                    }
                    break;
                case "262":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["loaiKhieuNaiId"]))
                    {
                        if (!context.Request.QueryString["loaiKhieuNaiId"].Equals("undefined"))
                        {
                            strValue = GetItemDropListLinhVucChung(context.Request.QueryString["loaiKhieuNaiId"], false);
                        }


                        //{
                        //    string logTest = "UrlReferrer: " + context.Request.UrlReferrer + Environment.NewLine + "Url: " + context.Request.Url;
                        //    Utility.LogEvent(logTest);
                        //}
                        //else
                        //{

                        //}
                    }
                    break;
                case "27":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["linhVucChungId"]))
                    {
                        strValue = GetItemDropListLinhVucCon(context.Request.QueryString["linhVucChungId"]);
                    }
                    break;
                case "272":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["linhVucChungId"]))
                    {
                        strValue = GetItemDropListLinhVucConSoTheoDoi(context.Request.QueryString["linhVucChungId"]);
                    }
                    break;
                case "29":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["linhVucChungId"]))
                    {
                        strValue = GetItemDropListLinhVucConAll(context.Request.QueryString["linhVucChungId"]);
                    }
                    break;


                case "28":
                    strValue = GetItemDropListDoUuTien();
                    break;
                case "35":
                    strValue = GetItemDropListTrangThai();
                    break;
                case "352":
                    strValue = GetItemDropListTrangThaiCuaToi();
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
                case "10000":
                    strValue = CountAllTabChoXuLy(infoUser.PhongBanId);
                    try
                    {
                        if (CacheProvider.Exists("CT_" + infoUser.Id))
                        {
                            CacheProvider.Remove("CT_" + infoUser.Id);
                            CacheProvider.Add("CT_" + infoUser.Id, strValue);
                        }
                        else
                        {
                            CacheProvider.Add("CT_" + infoUser.Id, strValue);
                        }
                    }
                    catch { }
                    break;

                case "10001":
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_khiếu_nại))
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                        {
                            string listID = context.Request.QueryString["listID"];
                            strValue = QLKN_TiepNhan(listID, infoUser).ToString();
                        }
                    }
                    else
                    {
                        strValue = "-2";
                    }
                    break;

                #region Trang Chu

                case "2":
                    strValue = GetHtml_ThongKeKhieuNai(infoUser.PhongBanId);
                    break;
                case "10":
                    string catid = context.Request.QueryString["catid"].ToString();
                    strValue = GetNameLoaiKhieuNai(catid);
                    break;
                case "39":
                    strValue = CountTongSoKhieuNai_TrangChu(infoUser.PhongBanId);
                    break;
                #endregion

                #region Tat ca KN
                case "57":
                    var valueTemp57 = CountTongSoKhieuNai(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp57, infoUser.Id, infoUser.PhongBanId, 0);
                    break;
                #endregion

                #region KN cho xu ly

                case "8":
                    var valueTemp8 = CountTongSoKhieuNai_ChoXuLy(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp8, infoUser.Id, infoUser.PhongBanId, 1);
                    break;
                #endregion

                #region KhieuNaiChuyenBoPhanKhac

                case "9":
                    var valueTemp9 = CountTongSoKhieuNai_ChuyenBoPhanKhac(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp9, infoUser.Id, infoUser.PhongBanId, 2);
                    break;
                #endregion

                #region KhieuNaiBoPhanKhacChuyenVe

                case "13":
                    var valueTemp13 = CountTongSoKhieuNai_BoPhanKhacChuyenVe(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp13, infoUser.Id, infoUser.PhongBanId, 3);
                    break;
                #endregion

                #region KhieuNaiSapQuaHan

                case "16":
                    var valueTemp16 = CountTongSoKhieuNai_SapQuaHan(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp16, infoUser.Id, infoUser.PhongBanId, 4);
                    break;
                #endregion

                #region KhieuNaiQuaHan

                case "19":
                    var valueTemp19 = CountTongSoKhieuNai_QuaHan(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp19, infoUser.Id, infoUser.PhongBanId, 5);
                    break;
                #endregion

                #region KhieuNaiDaPhanHoi

                case "62":
                    var valueTemp62 = CountTongSoKhieuNai_DaPhanHoi(infoUser.PhongBanId);
                    strValue = UpdateDicTabChoXuLy(valueTemp62, infoUser.Id, infoUser.PhongBanId, 6);
                    break;
                #endregion

                #region ChuyenKhieuNai
                // Edited by	: Dao Van Duong
                // Datetime		: 2.8.2016 12:05
                // Note			: Chuyển xử lý phòng ban được chọn
                case "21":
                    // Đối tượng trả về dữ liệu
                    MessageInfo ret = new MessageInfo();

                    // Kiểm tra quyền                        
                    bool isAccessed = BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại);
                    if (isAccessed)
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]) && !string.IsNullOrEmpty(context.Request.QueryString["PhongBan"]))
                        {
                            string listID = context.Request.QueryString["ListID"];
                            string phongban = context.Request.QueryString["PhongBan"];
                            string dataNote = context.Request.Form["Data"];
                            string username = context.Request.QueryString["Username"];
                            if (username == "-1" || username == "undefined") username = String.Empty;

                            // strValue = QLKN_ChuyenXuLy(listID, Convert.ToInt32(phongban), username, dataNote);
                            ret = QLKN_ChuyenXuLy(listID, Convert.ToInt32(phongban), username, dataNote);
                        }
                    }
                    else
                    {
                        // strValue = "-2";
                        ret.Code = -2;
                        ret.Message = "Bạn chưa được cấp quyền xử lý chuyển khiếu nại";
                    }
                    strValue = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
                    // context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret)); // Trả về client
                    break;
                case "52"://Chuyen Phan Hoi
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                        {
                            string listID = context.Request.QueryString["listID"];

                            string dataNote = context.Request.Form["data"];
                            strValue = QLKN_ChuyenPhanHoi(listID, dataNote);
                        }
                    }
                    else
                    {
                        strValue = "-2";
                    }
                    break;
                case "53"://Chuyen Ngang Hang
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                        {
                            string listID = context.Request.QueryString["listID"];
                            string userName = context.Request.QueryString["userName"];
                            string dataNote = context.Request.Form["data"];
                            strValue = QLKN_ChuyenNgangHang(listID, userName, dataNote);
                        }
                    }
                    else
                    {
                        strValue = "-2";
                    }
                    break;

                case "24":
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại))
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                        {
                            string dataNote = context.Request.Form["data"];
                            string strDoHaiLong = context.Request.Form["DoHaiLong"];
                            string listID = context.Request.QueryString["listID"];

                            var nDoHaiLong = ConvertUtility.ToInt32(strDoHaiLong, 2);
                            int nguyenNhanLoiId = ConvertUtility.ToInt32(context.Request.Form["nguyenNhanLoiId"], LoiKhieuNaiInfo.LoiKhieuNaiValue.NGUYEN_NHAN_LOI_ID_KHAC);
                            int chiTietLoiId = ConvertUtility.ToInt32(context.Request.Form["chiTietLoiId"], 0);
                            strValue = QLKN_DongKhieuNai(listID, dataNote, nDoHaiLong, nguyenNhanLoiId, chiTietLoiId);
                        }
                    }
                    else
                    {
                        strValue = "-2";
                    }
                    break;
                #endregion

                #region SoTheoDoiKhieuNai
                case "31"://Tính tổng số bản ghi tren so theo dõi

                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string select = context.Request.QueryString["select"].ToString();
                        string typeSearch = context.Request.QueryString["typeSearch"].ToString();

                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
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
                        string SoThueBao = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                        string NguoiTiepNhan = context.Request.Form["NguoiTiepNhan"] ?? context.Request.QueryString["NguoiTiepNhan"];
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
                case "47":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["typeKhieuNai"]))
                    {
                        int typeKhieuNai = ConvertUtility.ToInt32(context.Request.QueryString["typeKhieuNai"]);
                        var isPhanHoi = typeKhieuNai == 2 || (typeKhieuNai == 4 && ConvertUtility.ToInt32(context.Request.QueryString["IsPhanHoi"].ToString()) == 1);
                        int ngayQuaHanPhongBanXuLyTu = -1;
                        int ngayQuaHanPhongBanXuLyDen = -1;
                        int ngayCanhBaoPhongBanXuLyTu = -1;
                        int ngayCanhBaoPhongBanXuLyDen = -1;

                        try
                        {
                            DateTime ngayQuaHanTu = Convert.ToDateTime(context.Request.QueryString["ngayQuaHanPhongBanXuLyTu"].ToString(), new CultureInfo("vi-VN"));
                            ngayQuaHanPhongBanXuLyTu = ConvertUtility.ToInt32(ngayQuaHanTu.ToString("yyyyMMdd"));
                        }
                        catch
                        {
                            ngayQuaHanPhongBanXuLyTu = -1;
                        }

                        try
                        {
                            DateTime ngayQuaHanDen = Convert.ToDateTime(context.Request.QueryString["ngayQuaHanPhongBanXuLyDen"].ToString(), new CultureInfo("vi-VN"));
                            ngayQuaHanPhongBanXuLyDen = ConvertUtility.ToInt32(ngayQuaHanDen.ToString("yyyyMMdd"));
                        }
                        catch
                        {
                            ngayQuaHanPhongBanXuLyDen = -1;
                        }

                        int typeSearch = ConvertUtility.ToInt32(context.Request.QueryString["typeSearch"]);
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        int loaiKhieuNaiId = ConvertUtility.ToInt32(context.Request.QueryString["loaiKhieuNai"]);
                        int linhVucChungId = ConvertUtility.ToInt32(context.Request.QueryString["linhVucChung"]);
                        int linhVucConId = ConvertUtility.ToInt32(context.Request.QueryString["linhVucCon"]);


                        int startPageIndex = ConvertUtility.ToInt32(context.Request.QueryString["startPageIndex"]);
                        int pageSize = ConvertUtility.ToInt32(context.Request.QueryString["pageSize"]);
                        int totalRecord = 0;
                        DataTable dtResult = ServiceFactory.GetInstanceKhieuNai().ListKhieuNaiPhanViec(typeKhieuNai, typeSearch, loaiKhieuNaiId, linhVucChungId, linhVucConId,
                                        infoUser.PhongBanId, doUuTien, infoUser.Username, isPhanHoi, ngayQuaHanPhongBanXuLyTu, ngayQuaHanPhongBanXuLyDen,
                                        ngayCanhBaoPhongBanXuLyTu, ngayCanhBaoPhongBanXuLyDen, startPageIndex, pageSize, ref totalRecord);

                        strValue = totalRecord.ToString();

                        //if (typeKhieuNai == "1") //Load danh sách khiếu nại chờ xử lý
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string trangThai = "-1";
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();


                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        //        strValue = PhanViec_GetKhieuNaiChuaXuLy_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, ngayQuaHanPhongBanXuLyTu, ngayQuaHanPhongBanXuLyDen, startPageIndex, pageSize, false);
                        //    }
                        //}
                        //else if (typeKhieuNai == "2")//Load danh sách khiếu nại bộ phận khác chuyển về
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string trangThai = "-1";
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        //        strValue = PhanViec_GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize, true);
                        //    }
                        //}
                        //else if (typeKhieuNai == "3")//Load danh sách khiếu nại sắp quá hạn
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        //        strValue = PhanViec_GetKhieuNaiSapQuaHan_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
                        //else if (typeKhieuNai == "4")//Load danh sách khiếu nại quá hạn
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();

                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //        strValue = PhanViec_GetKhieuNaiQuaHan_TotalRecords(typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
                    }
                    break;
                case "48":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["typeKhieuNai"]))
                    {
                        int typeKhieuNai = ConvertUtility.ToInt32(context.Request.QueryString["typeKhieuNai"]);
                        //var isPhanHoi = ConvertUtility.ToInt32(context.Request.QueryString["IsPhanHoi"].ToString()) == 1;
                        var isPhanHoi = typeKhieuNai == 2 || (typeKhieuNai == 4 && ConvertUtility.ToInt32(context.Request.QueryString["IsPhanHoi"].ToString()) == 1);
                        int ngayQuaHanPhongBanXuLyTu = -1;
                        int ngayQuaHanPhongBanXuLyDen = -1;
                        int ngayCanhBaoPhongBanXuLyTu = -1;
                        int ngayCanhBaoPhongBanXuLyDen = -1;

                        try
                        {
                            DateTime ngayQuaHanTu = Convert.ToDateTime(context.Request.QueryString["ngayQuaHanPhongBanXuLyTu"].ToString(), new CultureInfo("vi-VN"));
                            ngayQuaHanPhongBanXuLyTu = ConvertUtility.ToInt32(ngayQuaHanTu.ToString("yyyyMMdd"));
                        }
                        catch
                        {
                            ngayQuaHanPhongBanXuLyTu = -1;
                        }

                        try
                        {
                            DateTime ngayQuaHanDen = Convert.ToDateTime(context.Request.QueryString["ngayQuaHanPhongBanXuLyDen"].ToString(), new CultureInfo("vi-VN"));
                            ngayQuaHanPhongBanXuLyDen = ConvertUtility.ToInt32(ngayQuaHanDen.ToString("yyyyMMdd"));
                        }
                        catch
                        {
                            ngayQuaHanPhongBanXuLyDen = -1;
                        }

                        int typeSearch = ConvertUtility.ToInt32(context.Request.QueryString["typeSearch"]);
                        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        int loaiKhieuNaiId = ConvertUtility.ToInt32(context.Request.QueryString["loaiKhieuNai"]);
                        int linhVucChungId = ConvertUtility.ToInt32(context.Request.QueryString["linhVucChung"]);
                        int linhVucConId = ConvertUtility.ToInt32(context.Request.QueryString["linhVucCon"]);


                        int startPageIndex = ConvertUtility.ToInt32(context.Request.QueryString["startPageIndex"]);
                        int pageSize = ConvertUtility.ToInt32(context.Request.QueryString["pageSize"]);
                        int totalRecord = 0;

                        strValue = PhanViec_GetHtml(context, typeKhieuNai, typeSearch, loaiKhieuNaiId, linhVucChungId, linhVucConId,
                                        infoUser.PhongBanId, doUuTien, infoUser.Username, isPhanHoi, ngayQuaHanPhongBanXuLyTu, ngayQuaHanPhongBanXuLyDen,
                                        ngayCanhBaoPhongBanXuLyTu, ngayCanhBaoPhongBanXuLyDen, startPageIndex, pageSize);

                        //string typeKhieuNai = context.Request.QueryString["typeKhieuNai"];
                        //var isPhanHoi = ConvertUtility.ToInt32(context.Request.QueryString["IsPhanHoi"].ToString()) == 1;
                        //if (typeKhieuNai == "1") //Load danh sách khiếu nại chờ xử lý
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string trangThai = "-1";
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();

                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();
                        //        strValue = PhanViec_GetHtmlKhieuNaiChuaXuLy(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
                        //else if (typeKhieuNai == "2")//Load danh sách khiếu nại bộ phận khác chuyển về
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string trangThai = "-1";
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //        strValue = PhanViec_GetHtmlKhieuNaiBoPhanKhacChuyenVe(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, trangThai, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
                        //else if (typeKhieuNai == "3")//Load danh sách khiếu nại sắp quá hạn
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //        strValue = PhanViec_GetHtmlKhieuNaiSapQuaHan(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
                        //else if (typeKhieuNai == "4")//Load danh sách khiếu nại quá hạn
                        //{
                        //    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                        //    {
                        //        string typeSearch = context.Request.QueryString["typeSearch"].ToString();
                        //        string doUuTien = context.Request.QueryString["doUuTien"].ToString();
                        //        string loaiKhieuNai = context.Request.QueryString["loaiKhieuNai"].ToString();
                        //        string linhVucChung = context.Request.QueryString["linhVucChung"].ToString();
                        //        string linhVucCon = context.Request.QueryString["linhVucCon"].ToString();
                        //        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        //        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //        strValue = PhanViec_GetHtmlKhieuNaiQuaHan(context, typeSearch, loaiKhieuNai, linhVucChung, linhVucCon, infoUser.PhongBanId.ToString(), doUuTien, infoUser.Username, startPageIndex, pageSize, isPhanHoi);
                        //    }
                        //}
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
                case "201701":
                    if (!string.IsNullOrEmpty(context.Request.QueryString["startPageIndex"]) && !string.IsNullOrEmpty(context.Request.QueryString["pageSize"]))
                    {
                        string startPageIndex = context.Request.QueryString["startPageIndex"].ToString();
                        string pageSize = context.Request.QueryString["pageSize"].ToString();

                        //strValue = GetHtmlKhieuNaiCanhBaoNew(infoUser.Username, startPageIndex, pageSize);
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
                    if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xử_lý_khiếu_nại))
                    {
                        if (!string.IsNullOrEmpty(context.Request.QueryString["listID"]))
                        {
                            string listID = context.Request.QueryString["listID"];
                            strValue = BuildKhieuNai.UpdateKhieuNaiToHangLoat(listID, infoUser).ToString();
                        }
                    }
                    else
                    {
                        strValue = "-2";
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
                List<NguoiSuDungInfo> list = ServiceFactory.GetInstanceNguoiSuDung().NguoiSuDung_GetInfoNguoiSuDungByTenTruyCap(tenTruyCap);
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
                    List<PhongBanInfo> lst = ServiceFactory.GetInstancePhongBan().QLKN_PhongBanGetAll();
                    List<PhongBanInfo> listLoadPhongBan = new List<PhongBanInfo>();
                    List<PhongBan2PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(infoUser.PhongBanId);
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

        private string GetItemDropListTrangThai()
        {
            try
            {
                string strValues = "";
                strValues += "<option value=\"-1\">--Trạng thái--</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Chờ_xử_lý + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Chờ_xử_lý).Replace("_", " ") + "</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Đang_xử_lý + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ") + "</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Chờ_đóng + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") + "</option>";

                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetItemDropListTrangThaiCuaToi()
        {
            try
            {
                string strValues = "";
                strValues += "<option value=\"-1\">--Trạng thái--</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Chờ_xử_lý + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Chờ_xử_lý).Replace("_", " ") + "</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Đang_xử_lý + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Đang_xử_lý).Replace("_", " ") + "</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Chờ_đóng + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Chờ_đóng).Replace("_", " ") + "</option>";
                strValues += "<option value=\"" + (int)KhieuNai_TrangThai_Type.Đóng + "\">" + Enum.GetName(typeof(KhieuNai_TrangThai_Type), (int)KhieuNai_TrangThai_Type.Đóng).Replace("_", " ") + "</option>";

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
                    DataTable tab = ServiceFactory.GetInstanceLoaiKhieuNai().QLKN_LoaiKhieuNai_ByPhongBanId(PhongBanId);
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
        private string GetItemDropListLinhVucChung(string loaiKhieuNaiID, bool IsLoadEmpty = true)
        {
            try
            {
                string strValues = "";
                var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + loaiKhieuNaiID, "Sort");
                strValues += "<option value=\"-1\">--Lĩnh vực chung--</option>";
                if (IsLoadEmpty)
                    strValues += "<option value=\"0\">--Trống--</option>";
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
        private string GetItemDropListLinhVucConAll(string lihVucChungId, bool IsLoadEmpty = true)
        {
            try
            {
                string strValues = "";
                //var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + lihVucChungId, "Sort");
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("DISTINCT Name", "Cap = 3", "");
                strValues += "<option value=\"-1\">--Lĩnh vực con--</option>";
                //if (IsLoadEmpty)
                //    strValues += "<option value=\"0\">--Trống--</option>";
                strValues += "<option value=\"0\">--Trống--</option>";
                if (lstLinhVucCon.Count > 0)
                {
                    foreach (LoaiKhieuNaiInfo info in lstLinhVucCon)
                    {
                        strValues += "<option value=\"" + info.Name + "\">" + info.Name + "</option>";
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
                //var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + lihVucChungId, "Sort");
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("DISTINCT Name", "ParentId=" + lihVucChungId, "");
                strValues += "<option value=\"-1\">--Lĩnh vực con--</option>";
                strValues += "<option value=\"0\">--Trống--</option>";
                if (lstLinhVucCon.Count > 0)
                {
                    foreach (LoaiKhieuNaiInfo info in lstLinhVucCon)
                    {
                        strValues += "<option value=\"" + info.Name + "\">" + info.Name + "</option>";
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


        private string GetItemDropListLinhVucConSoTheoDoi(string lihVucChungId)
        {
            try
            {
                string strValues = "";
                var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + lihVucChungId, "");
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
                lst = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", "", "Sort");
                strValues += "<option value=\"-1\">--Phòng ban xử lý--</option>";
                if (lst != null && lst.Count > 0)
                {
                    foreach (PhongBanInfo info in lst)
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
                                    strValues += "<span id='fileId-" + info.Id + "'><a class='delete-file' style='color: Red;padding-right:5px;' href='javascript:DeleteFile(" + info.Id + ")'><img src ='/images/icons/del_file.png' /></a><a href ='/Views/ChiTietKhieuNai/Download.aspx?id=" + info.Id + "'>" + info.TenFile + "</a> <br /></span>";
                                }
                                else
                                {
                                    strValues += "<a href ='/Views/ChiTietKhieuNai/Download.aspx?id=" + info.Id + "'>" + info.TenFile + "</a> <br />";
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
        private KhieuNaiChiTietInfo GetInfoKhieuNaiByID(string id, string view, HttpContext context, AdminInfo infoUser)
        {
            try
            {
                KhieuNaiChiTietInfo KNChiTietInfo = new KhieuNaiChiTietInfo();
                //string strData = "";
                int ArchiveId = ConvertUtility.ToInt32(context.Request.QueryString["archive"] ?? context.Request.Form["archive"]);
                KhieuNaiInfo info = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetInfo(Convert.ToInt32(id));
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
                    KNChiTietInfo.NoiDungXuLyDongKN = info.NoiDungXuLyDongKN;
                    KNChiTietInfo.TinhThanhXayRaSuCo = info.MaTinh;
                    KNChiTietInfo.QuanHuyenXayRaSuCo = info.MaQuan;
                    KNChiTietInfo.PhuongXaXayRaSuCo = info.MaPhuong;

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
                    //KNChiTietInfo.HTTiepNhan = info.HTTiepNhan.ToString();
                    KNChiTietInfo.HTTiepNhan = Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), info.HTTiepNhan) != null ? Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), info.HTTiepNhan).Replace("_", " ") : string.Empty;
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
                    KNChiTietInfo.CallCount = info.CallCount;
                    KNChiTietInfo.CDate = info.CDate.ToString("dd/MM/yyyy HH:mm:ss");
                    KNChiTietInfo.CUser = info.CUser;
                    KNChiTietInfo.LDate = info.LDate.ToString("dd/MM/yyyy HH:mm:ss");
                    KNChiTietInfo.LUser = info.LUser;
                    KNChiTietInfo.TenDoHaiLong = Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), info.DoHaiLong) != null ? Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), info.DoHaiLong).Replace("_", " ") : string.Empty;

                    if (info.LyDoGiamTru > 0)
                    {
                        List<LoiKhieuNaiInfo> listLoiKhieuNai = new LoiKhieuNaiImpl().GetNguyenNhanLoiAndChiTietLoi(info.LyDoGiamTru, info.ChiTietLoiId);
                        if (listLoiKhieuNai != null)
                        {
                            for (int i = 0; i < listLoiKhieuNai.Count; i++)
                            {
                                if (listLoiKhieuNai[i].Cap == 1)
                                {
                                    KNChiTietInfo.TenLyDoGiamTru = listLoiKhieuNai[i].TenLoi;
                                }
                                else if (listLoiKhieuNai[i].Cap == 2)
                                {
                                    KNChiTietInfo.TenChiTietLoi = listLoiKhieuNai[i].TenLoi;
                                }
                            }

                            if (info.ChiTietLoiId == 0)
                            {
                                KNChiTietInfo.TenChiTietLoi = "Khác";
                            }
                        }
                    }

                    //Kiểm tra xem có đc quyền xử lý KN
                    var returnURL = context.Request.Form["ReturnUrl"] ?? context.Request.QueryString["ReturnUrl"];
                    //returnURL += "?SoThueBao=" + info.SoThueBao;
                    var strXuLyKN = string.Format("<a href='/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={0}&Mode=Process&ReturnUrl={1}' style='font-size:15px; font-weight: bold; color: red;'>Vào xử lý khiếu nại</a>", info.Id, returnURL);
                    KNChiTietInfo.XuLyKN = string.Format("<a href='/Views/ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={0}&Mode=View&archive={2}&ReturnUrl={1}' style='font-size:15px; font-weight: bold; color: red;'>Xem chi tiết khiếu nại</a>", info.Id, returnURL, ArchiveId);
                    if (info.TrangThai != (byte)KhieuNai_TrangThai_Type.Đóng)
                    {
                        if (info.NguoiXuLy == string.Empty && info.PhongBanXuLyId == infoUser.PhongBanId)
                        {
                            if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_khiếu_nại))
                            {
                                //Cho phép
                                KNChiTietInfo.XuLyKN = strXuLyKN;
                            }
                            else
                            {
                                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_KN_phản_hồi_về_người_gửi))
                                {
                                    var itemActivity = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(info.Id);
                                    if (itemActivity != null)
                                    {
                                        if (itemActivity.HanhDong == (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi && itemActivity.NguoiDuocPhanHoi == infoUser.Username)
                                        {
                                            //Cho phép
                                            KNChiTietInfo.XuLyKN = strXuLyKN;
                                        }
                                    }
                                }
                            }
                        }
                        else if (info.NguoiXuLy == infoUser.Username)
                        {
                            //Cho phép
                            KNChiTietInfo.XuLyKN = strXuLyKN;
                        }
                        else if (info.PhongBanXuLyId == infoUser.PhongBanId && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_KN_phản_hồi_của_phòng_ban_sau_thời_gian_cấu_hình)
                        && (DateTime.Now - info.LDate).TotalHours >= Config.TimeEditKhieuNai)
                        {
                            //Cho phép
                            KNChiTietInfo.XuLyKN = strXuLyKN;
                        }
                    }
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
                LoaiKhieuNaiInfo info = ServiceFactory.GetInstanceLoaiKhieuNai().GetDoc(Convert.ToInt32(id));
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

                PhongBanInfo info = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(id));
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
        private string BindInfoTraTruocToForm(ServiceVNP.TBTraTruocFullInfo info)
        {
            string strValues = "";
            strValues += info.FULLNAME + "#";
            strValues += info.ADDRESS;
            return strValues;
        }

        private string BindInfoTraSauToForm(ServiceVNP.TBTraTruocFullInfo info)
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
                ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                requestParam.SoThueBao = dauSo + stb;
                requestParam.Username = admin.Username;
                requestParam.Note = "";

                var result = obj.GetInfo(requestParam);
                if (result != null)
                {
                    if (result.TEN_LOAI == "Post" || result.TEN_LOAI.ToLower().Contains("itouch"))
                    {
                        BindInfoTraSauToForm(result);
                    }
                    else
                    {
                        BindInfoTraTruocToForm(result);
                    }
                }
                else
                    return string.Empty;

                //var Impl = new SubinfoImpl();
                //var ImplWS = new SubinfoTSImpl();
                //BasicInfoFromSubinfo basicInfo = Impl.getBasicInfo(dauSo + stb, admin.Username, Utility.GetIP(), "");
                //var strJSON = "";
                //if (basicInfo != null)
                //{
                //    var infoTB = ImplWS.VNP_TRACUU_TB(dauSo + stb);

                //    if (basicInfo.MA_TINH != "GPC")
                //    {
                //        var infoTBTH = ImplWS.VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TB(basicInfo.MA_TINH, dauSo + stb);
                //        if (infoTBTH != null)
                //        {

                //            var infoFull = new TBFullInfo(infoTBTH, basicInfo, infoTB);
                //            if (basicInfo.TEN_LOAI == "Post" || basicInfo.TEN_LOAI.ToLower().Contains("itouch"))
                //            {
                //                strValues = BindInfoTraSauToForm(infoFull);
                //            }
                //            else
                //            {
                //                var infoTBTraTruoc = ImplWS.PrepaidSubscriberInfo(dauSo + stb);
                //                var tbnot84 = dauSo;

                //                var infoTaiKhoan = ImplWS.getSubscriber(new Random().Next(111111, 999999), tbnot84, DateTime.Now.ToString("yyyyMMddHHmmss"));
                //                var infoTTFull = new TBTraTruocFullInfo(infoTBTraTruoc, infoFull, infoTaiKhoan);
                //                strValues = BindInfoTraTruocToForm(infoTTFull);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var infoFull = new TBFullInfo(null, basicInfo, infoTB);
                //        strValues = BindInfoTraSauToForm(infoFull);
                //    }
                //    //}
                //}
                //else
                //{
                //    var infoTB = ImplWS.VNP_TRACUU_TB(dauSo + stb);
                //    if (infoTB != null)
                //    {
                //        if (infoTB.MA_TINH != "GPC")
                //        {
                //            var infoTBTH = ImplWS.VNP_CCS_ADMIN_PTTB_LAPHD_LAYTT_TB(infoTB.MA_TINH, dauSo + stb);
                //            if (infoTBTH != null)
                //            {
                //                var infoFull = new TBFullInfo(infoTBTH, basicInfo, infoTB);

                //                var infoTBTraTruoc = ImplWS.PrepaidSubscriberInfo(dauSo + stb);
                //                var tbnot84 = dauSo;

                //                var infoTaiKhoan = ImplWS.getSubscriber(new Random().Next(111111, 999999), tbnot84, DateTime.Now.ToString("yyyyMMddHHmmss"));
                //                var infoTTFull = new TBTraTruocFullInfo(infoTBTraTruoc, infoFull, infoTaiKhoan);
                //                strValues = BindInfoTraSauToForm(infoTTFull);

                //            }
                //        }
                //        else
                //        {
                //            var infoTTFull = new TBTraTruocFullInfo() { TEN_LOAI = "Test", MA_TINH = "GPC" };
                //            strValues = BindInfoTraTruocToForm(infoTTFull);
                //        }
                //    }
                //}
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "";
            }
        }

        #endregion

        private string CountAllTabChoXuLy(int PhongBanId)
        {
            try
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}|", CountTongSoKhieuNai(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_ChoXuLy(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_ChuyenBoPhanKhac(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_BoPhanKhacChuyenVe(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_SapQuaHan(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_QuaHan(PhongBanId));
                sb.AppendFormat("{0}|", CountTongSoKhieuNai_DaPhanHoi(PhongBanId));

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string UpdateDicTabChoXuLy(string value, int userId, int phongBanId, int index)
        {
            var strValue = "";
            bool flagGetAll = false;
            try
            {
                if (CacheProvider.Exists("CT_" + userId))
                {
                    var strTemp57 = CacheProvider.Get("CT_" + userId).ToString();
                    string[] arrTemp57 = strTemp57.Split('|');
                    if (arrTemp57.Length == 8)
                    {
                        arrTemp57[index] = value;
                        foreach (var item in arrTemp57)
                        {
                            if (string.IsNullOrEmpty(item))
                                continue;
                            strValue += item + "|";
                        }

                        CacheProvider.Remove("CT_" + userId);
                        CacheProvider.Add("CT_" + userId, strValue);
                    }
                }
                else
                {
                    strValue = CountAllTabChoXuLy(phongBanId);
                    flagGetAll = true;
                    CacheProvider.Add("CT_" + userId, strValue);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                if (!flagGetAll)
                {
                    strValue = CountAllTabChoXuLy(phongBanId);
                }
            }
            return strValue;
        }

        #region Trang Chu

        private string GetTotalRecords(int PhongBanId, string startPageIndex, string pageSize)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceLoaiKhieuNai().QLKN_LoaiKhieuNai_GetAllWithPadding_TotalRecords(PhongBanId, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string CountTongSoKhieuNai_TrangChu(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceThongKeKhieuNai().CountThongKeKhieuNai_TrangChu(PhongBanId);
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string GetHtml_ThongKeKhieuNai(int PhongBanId)
        {
            try
            {
                string strData = "";
                var lst = ServiceFactory.GetInstanceThongKeKhieuNai().GetThongKeKhieuNai(PhongBanId);
                if (lst != null && lst.Count > 0)
                {
                    int temp = 0;
                    foreach (var row in lst)
                    {
                        if (temp % 2 == 0)
                        {
                            strData += "<tr id =\"row-" + temp + "\" class=\"rowA\">";
                        }
                        else
                        {
                            strData += "<tr id =\"row-" + temp + "\" class=\"rowB\">";
                        }

                        strData += "        <td class =\"nowrap\" align=\"center\">" + (temp + 1) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"left\">" + row.LoaiKhieuNai + "</td>";
                        strData += "        <td align=\"left\"><span style =\"margin-left:10px;\">" + row.LinhVucChung + "</span></td>";


                        if (row.SoLuong > 0)
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"QuanLyKhieuNai.aspx?ctrl=tab0-BieuDoKNChoXuLy&LoaiKhieuNaiId=" + row.LoaiKhieuNaiId + "&LinhVucChungId=" + row.LinhVucChungId + "\">" + row.SoLuong.ToString("#,##0") + "</a></td>";
                        }
                        else
                        {
                            strData += "        <td class =\"nowrap\" align=\"center\">0</td>";
                        }
                        strData += " </tr>";
                        temp++;
                    }
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

        #region Tat ca KN
        private string CountTongSoKhieuNai(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region KN cho xu ly
        private string CountTongSoKhieuNai_ChoXuLy(int PhongBanId)
        {
            try
            {
                // Utility.LogEvent(PhongBanId);
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_ChoXuLy(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Khieu Nai Chuyen Bo Phan Khac
        private string CountTongSoKhieuNai_ChuyenBoPhanKhac(int PhongBanId)
        {
            try
            {

                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_ChuyenBoPhanKhac(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Khieu Nai BoPhanKhacChuyenVe
        private string CountTongSoKhieuNai_BoPhanKhacChuyenVe(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_BoPhanKhacChuyenVe(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Khieu Nai SapQuaHan
        private string CountTongSoKhieuNai_SapQuaHan(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_SapQuaHan(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Khieu Nai QuaHan
        private string CountTongSoKhieuNai_QuaHan(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_QuaHan(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        #endregion

        #region Khieu Nai Da Phan Hoi
        private string CountTongSoKhieuNai_DaPhanHoi(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().CountTongSoKhieuNai_DaPhanHoi(PhongBanId);
                //return TotalRecords.ToString();
                return TotalRecords.ToString("#,##0");
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
                var userId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(userName);
                if (userId == 0)
                {
                    return "-1";
                }
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        KhieuNaiInfo info = ServiceFactory.GetInstanceKhieuNai().GetInfo(Convert.ToInt32(item));
                        if (info != null)
                        {
                            if (string.IsNullOrEmpty(info.NguoiXuLy))
                            {
                                using (TransactionScope scope = new TransactionScope())
                                {
                                    try
                                    {
                                        strValues = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiUpdatePhanViec(Convert.ToInt32(item), userId, userName).ToString();
                                        ServiceFactory.GetInstanceKhieuNai_Activity().QLKN_KhieuNai_ActivityUpdatePhanViec(Convert.ToInt32(item), userName);

                                        LichSuPhanViecInfo lsInfo = new LichSuPhanViecInfo();
                                        lsInfo.KhieuNaiId = Convert.ToInt32(item);
                                        lsInfo.NguoiDuocPhanViec = userName;
                                        lsInfo.PhongBanXuLyId = infoUser.PhongBanId;
                                        lsInfo.LUser = infoUser.Username;
                                        ServiceFactory.GetInstanceLichSuPhanViec().Add(lsInfo);
                                        scope.Complete();
                                    }
                                    catch (Exception ex)
                                    {
                                    }
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
        #region KhieuNaiChuaXuLy
        private string PhanViec_QLKN_GetTotalKhieuNaiChuaXuLy(int PhongBanId)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_GetTotalKhieuNaiChuaXuLyPhanViec_GetAllWithPadding(PhongBanId);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string PhanViec_GetKhieuNaiChuaXuLy_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string PhanViec_GetHtmlKhieuNaiChuaXuLy(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                string strData = "";
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
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

                        //strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap2"] + "'>" + row["NguoiTienXuLyCap2"] + "</a></td>";
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

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/10/2014
        /// Todo : Hiển thị danh sách khiếu nại chờ phân việc
        /// </summary>
        /// <param name="context"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="LinhVucConId"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isPhanHoi"></param>
        /// <returns></returns>
        private string PhanViec_GetHtml(HttpContext context, int typeKhieuNai, int typeSearch, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int phongBanId, string doUuTien,
                                                string nguoiXuLy, bool isPhanHoi, int ngayQuaHanPhongBanXuLySortTu, int ngayQuaHanPhongBanXuLySortDen,
                                                int ngayCanhBaoQuaHanPhongBanXuLySortTu, int ngayCanhBaoQuaHanPhongBanXuLySortDen, int startPageIndex, int pageSize)
        {
            try
            {
                string strData = "";
                int totalRecord = 0;
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().ListKhieuNaiPhanViec(typeKhieuNai, typeSearch, loaiKhieuNaiId, linhVucChungId, linhVucConId, phongBanId,
                                                                                            doUuTien, nguoiXuLy, isPhanHoi, ngayQuaHanPhongBanXuLySortTu, ngayQuaHanPhongBanXuLySortDen,
                                                                                            ngayCanhBaoQuaHanPhongBanXuLySortTu, ngayCanhBaoQuaHanPhongBanXuLySortDen,
                                                                                            startPageIndex, pageSize, ref totalRecord);


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
                            if (row["NguoiXuLy"].ToString() != nguoiXuLy)
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
                            if (row["NguoiXuLy"].ToString() != nguoiXuLy)
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

                        //strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap2"] + "'>" + row["NguoiTienXuLyCap2"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap3"] + "'>" + row["NguoiTienXuLyCap3"] + "</a></td>";
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

        private string PhanViec_GetKhieuNaiBoPhanKhacChuyenVe_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    Convert.ToInt32(trangThai),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string PhanViec_GetHtmlKhieuNaiBoPhanKhacChuyenVe(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string trangThai, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                string strData = "";
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                   Convert.ToInt32(LoaiKhieuNaiId),
                   Convert.ToInt32(LinhVucChungId),
                   Convert.ToInt32(LinhVucConId),
                   Convert.ToInt32(PhongBanId),
                   Convert.ToInt32(DoUuTien),
                   Convert.ToInt32(trangThai),
                   NguoiXuLy,
                   Convert.ToInt32(startPageIndex),
                   Convert.ToInt32(pageSize), isPhanHoi);
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

                        //strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap2"] + "'>" + row["NguoiTienXuLyCap2"] + "</a></td>";
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

        private string PhanViec_GetKhieuNaiSapQuaHan_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string PhanViec_GetHtmlKhieuNaiSapQuaHan(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                string strData = "";
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),

                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
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

                        //strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap2"] + "'>" + row["NguoiTienXuLyCap2"] + "</a></td>";
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
        private string PhanViec_GetKhieuNaiQuaHan_TotalRecords(string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false, int ngayQuaHanPhongBanXuLySortTu = 0, int ngayQuaHanPhongBanXuLySortDen = 0)
        {
            try
            {
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
                return TotalRecords.ToString();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        private string PhanViec_GetHtmlKhieuNaiQuaHan(HttpContext context, string TypeSearch, string LoaiKhieuNaiId, string LinhVucChungId, string LinhVucConId, string PhongBanId, string DoUuTien, string NguoiXuLy, string startPageIndex, string pageSize, bool isPhanHoi = false)
        {
            try
            {
                string strData = "";
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding(Convert.ToInt32(TypeSearch),
                    Convert.ToInt32(LoaiKhieuNaiId),
                    Convert.ToInt32(LinhVucChungId),
                    Convert.ToInt32(LinhVucConId),
                    Convert.ToInt32(PhongBanId),
                    Convert.ToInt32(DoUuTien),
                    NguoiXuLy,
                    Convert.ToInt32(startPageIndex),
                    Convert.ToInt32(pageSize), isPhanHoi);
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

                        //strData += "        <td class =\"nowrap\" align=\"left\">" + GetNamePhongBan(row["PhongBanXuLyId"].ToString()) + "</td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTiepNhan"] + "'>" + row["NguoiTiepNhan"] + "</a></td>";
                        strData += "        <td class =\"nowrap\" align=\"center\"><a href=\"#\" class=\"normalTip exampleTip\" title='" + row["NguoiTienXuLyCap2"] + "'>" + row["NguoiTienXuLyCap2"] + "</a></td>";
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

        //private string PhanViec_TotalRecords(string typeSearch, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int phongBanId, string doUuTien,
        //                                        string nguoiXuLy, bool isPhanHoi, int ngayQuaHanPhongBanXuLySortTu, int ngayQuaHanPhongBanXuLySortDen,
        //                                        int ngayCanhBaoQuaHanPhongBanXuLySortTu, int ngayCanhBaoQuaHanPhongBanXuLySortDen, string startPageIndex, string pageSize)
        //{
        //    try
        //    {
        //        int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords(Convert.ToInt32(TypeSearch),
        //            Convert.ToInt32(LoaiKhieuNaiId),
        //            Convert.ToInt32(LinhVucChungId),
        //            Convert.ToInt32(LinhVucConId),
        //            Convert.ToInt32(PhongBanId),
        //            Convert.ToInt32(DoUuTien),
        //            Convert.ToInt32(trangThai),
        //            NguoiXuLy,
        //            Convert.ToInt32(startPageIndex),
        //            Convert.ToInt32(pageSize), isPhanHoi);
        //        return TotalRecords.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return "-1";
        //    }
        //}

        #endregion

        #region Chuyen Khieu Nai

        private string QLKN_DongKhieuNai(string listID, string Note, int DoHaiLong, int nguyenNhanLoiId, int chiTietLoiId)
        {
            try
            {
                string strValues = string.Empty;
                string[] listItem = listID.Split(','); // Danh sách KhieuNaiId cần đóng
                int result = 0;
                int resultService = 0;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            BuildKhieuNai.DongKhieuNai(Convert.ToInt32(item), Note, ref resultService, nguyenNhanLoiId, chiTietLoiId, false, DoHaiLong);
                            result++;
                        }
                        catch (Exception ex) { Utility.LogEvent(ex); }
                    }
                }
                strValues = string.Format("{0}/{1}", result, listItem.Length - 1);
                return strValues;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        // Edited by	: Dao Van Duong
        // Datetime		: 2.8.2016 13:55
        // Note			: Chỉnh sửa hàm
        // Content      : Chuyển xử lý
        private MessageInfo QLKN_ChuyenXuLy(string listID, int phongBanID, string username, string Note)
        {
            MessageInfo retInfo = new MessageInfo();
            try
            {
                string[] listItem = listID.Split(',');
                int result = 0;
                // string resultService = string.Empty;
                foreach (string str in listItem)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            BuildKhieuNai_Activity.ActivityChuyenPhongBanToUserInPhongBan(Convert.ToInt32(str), phongBanID, username, KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban, Note);
                            result++;
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                        }
                    }
                }

                retInfo.Code = 1;
                retInfo.Message = result + "/" + (listItem.Length - 1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                retInfo.Code = 0;
                retInfo.Message = ex.Message;
            }
            return retInfo;
        }

        private string QLKN_ChuyenPhanHoi(string listID, string Note)
        {
            try
            {
                string[] listItem = listID.Split(',');
                var result = 0;
                var warning = 0;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            BuildKhieuNai_Activity.ActivityChuyenPhanHoi(Convert.ToInt32(item), 0, Note, ref warning);
                            result++;
                        }
                        catch (Exception ex) { Utility.LogEvent(ex); }
                    }
                }
                return string.Format("{0}/{1}", result, listItem.Length - 1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "0";
            }
        }

        private string QLKN_TiepNhan(string listID, AdminInfo infoUser)
        {
            try
            {
                string[] listItem = listID.Split(',');
                var result = 0;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            if (BuildKhieuNai.TiepNhanKhieuNai(Convert.ToInt32(item), infoUser))
                                result++;
                        }
                        catch (Exception ex) { Utility.LogEvent(ex); }
                    }
                }
                return string.Format("{0}/{1}", result, listItem.Length - 1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        private string QLKN_ChuyenNgangHang(string listID, string Note)
        {
            try
            {
                string[] listItem = listID.Split(',');
                var result = 0;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            if (BuildKhieuNai_Activity.ActivityChuyenNgangHangToUser(Convert.ToInt32(item), "", Note))
                                result++;
                            else
                                break;
                        }
                        catch (GQKNMessageException ge)
                        { }
                        catch (Exception ex) { Utility.LogEvent(ex); }
                    }
                }
                return string.Format("{0}/{1}", result, listItem.Length - 1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/10/2014
        /// Todo : Chuyển ngang hàng cho 1 user cụ thể
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="Note"></param>
        /// <returns></returns>
        private string QLKN_ChuyenNgangHang(string listID, string nguoiXuLy, string Note)
        {
            try
            {
                string[] listItem = listID.Split(',');
                var result = 0;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            if (BuildKhieuNai_Activity.ActivityChuyenNgangHangToUser(Convert.ToInt32(item), nguoiXuLy, Note))
                                result++;
                            else
                                break;
                        }
                        catch (GQKNMessageException ge)
                        { }
                        catch (Exception ex) { Utility.LogEvent(ex); }
                    }
                }
                return string.Format("{0}/{1}", result, listItem.Length - 1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
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

                item.KhuVucXuLyId = infoUser.KhuVucId;
                item.DoiTacXuLyId = infoUser.DoiTacId;
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
                item.NoiDungPA = NoiDungPA;
                item.GhiChu = GhiChu;
                item.IsChuyenBoPhan = false;
                item.NguoiTiepNhanId = infoUser.Id;
                item.NguoiTiepNhan = infoUser.Username;
                item.NguoiXuLyId = infoUser.Id;
                item.NguoiXuLy = infoUser.Username;

                item.HTTiepNhan = 1;
                DateTime ngayTiepNhan = Convert.ToDateTime(NgayTiepNhan, new CultureInfo("vi-VN"));

                item.NgayTiepNhan = ngayTiepNhan;
                item.NgayTiepNhanSort = Convert.ToInt32(ngayTiepNhan.ToString("yyyyMMdd"));

                item.NoiDungXuLy = NoiDungXuLy;
                item.KetQuaXuLy = KetQuaXuLy;

                DateTime ngayTraLoiKN = Convert.ToDateTime(NgayTraLoiKN, new CultureInfo("vi-VN"));
                item.NgayTraLoiKN = ngayTraLoiKN;
                item.NgayTraLoiKNSort = Convert.ToInt32(ngayTraLoiKN.ToString("yyyyMMdd"));

                item.NgayDongKN = ngayTraLoiKN;
                item.NgayDongKNSort = Convert.ToInt32(ngayTraLoiKN.ToString("yyyyMMdd"));

                item.IsLuuKhieuNai = true;
                item.DoUuTien = (byte)KhieuNai_DoUuTien_Type.Thông_thường;
                item.TrangThai = (byte)KhieuNai_TrangThai_Type.Đóng;

                bool flag = false;
                if (maKhieuNai != "0")
                {
                    item.Id = Convert.ToInt32(maKhieuNai);
                    var activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(item.Id);
                    try
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                ServiceFactory.GetInstanceKhieuNai_Activity().UpdateDynamic("LDate=getdate()", "IsCurrent=1 and KhieuNaiId=" + item.Id);
                                strValues = ServiceFactory.GetInstanceKhieuNai().Update(item);

                                scope.Complete();
                            }
                            catch { }
                        }
                        if (flag)
                        {
                            BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(item.Id), "Sửa sổ theo dõi.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        return "-1";
                    }
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            item.Id = ServiceFactory.GetInstanceKhieuNai().Add(item);
                            //Activity
                            KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
                            itemActivity.KhieuNaiId = item.Id;
                            itemActivity.ActivityTruoc = 0;
                            itemActivity.GhiChu = "Tạo mới khiếu nại";
                            itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                            itemActivity.IsCurrent = false;
                            itemActivity.NguoiXuLyTruoc = infoUser.Username;
                            itemActivity.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
                            itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
                            itemActivity.NgayTiepNhan = item.NgayTiepNhan;
                            itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                            itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;
                            ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);

                            //Activity
                            KhieuNai_ActivityInfo itemActivityDong = new KhieuNai_ActivityInfo();
                            itemActivityDong.KhieuNaiId = item.Id;
                            itemActivityDong.ActivityTruoc = 0;
                            itemActivityDong.GhiChu = "Đóng dòng trên sổ theo dõi.";
                            itemActivityDong.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Đóng_KN;
                            itemActivityDong.IsCurrent = true;
                            itemActivityDong.NguoiXuLyTruoc = infoUser.Username;
                            itemActivityDong.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
                            itemActivityDong.PhongBanXuLyId = item.PhongBanXuLyId;
                            itemActivityDong.NgayTiepNhan = item.NgayTiepNhan;
                            itemActivityDong.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                            itemActivityDong.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;
                            ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivityDong);

                            scope.Complete();
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            return "-1";
                        }
                    }

                    if (flag)
                    {
                        KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                        buocXuLyInfo.NoiDung = "Đóng dòng trên sổ theo dõi.";
                        buocXuLyInfo.LUser = infoUser.Username;
                        buocXuLyInfo.KhieuNaiId = item.Id;
                        buocXuLyInfo.IsAuto = true;
                        buocXuLyInfo.NguoiXuLyId = infoUser.Id;
                        buocXuLyInfo.PhongBanXuLyId = infoUser.PhongBanId;

                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);
                        BuildKhieuNai_Log.LogKhieuNai(item.Id, "Đóng dòng trên sổ theo dõi.");
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
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding_TotalRecords(Convert.ToInt32(Select),
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
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding(Convert.ToInt32(Select), Convert.ToInt32(PhongBanId),
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
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai().KhieuNai_KhieuNaiCanhBaoTotalRecords_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
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
                DataTable tab = ServiceFactory.GetInstanceKhieuNai().KhieuNai_KhieuNaiCanhBao_GetAllWithPadding(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
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
        private List<string> GetHtmlKhieuNaiCanhBaoNew(string userName, string startPageIndex, string pageSize)
        {
            try
            {
                var lstKncb = new List<string>();
                string strData = "";
                DataSet tab = ServiceFactory.GetInstanceKhieuNai().KhieuNai_KhieuNaiCanhBao_GetAllWithPaddingNew(userName, Convert.ToInt32(startPageIndex), Convert.ToInt32(pageSize));
                if (tab.Tables[0].Rows.Count > 0)
                {
                    int temp = 0;
                    foreach (DataRow row in tab.Tables[0].Rows)
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
                lstKncb.Add(strData);
                lstKncb.Add(Convert.ToString(tab.Tables[0].Rows[0]["TotalRecords"]));
                return lstKncb;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        #endregion

        #region Khieu Nai Da Gui Di
        private string CountTongSoKhieuNai_DaGuiDi(int PhongBanId)
        {
            try
            {
                //Utility.LogEvent(PhongBanId);
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai_Activity().GetTotalKhieuNaiGuiDi(PhongBanId, LoginAdmin.AdminLogin().Username, (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới);
                return TotalRecords.ToString("#,##0");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "-1";
            }
        }
        #endregion
        #region Khieu nan Phan Hoi
        private string CountTongSoKhieuNai_PhanHoi(int PhongBanId)
        {
            try
            {
                //Utility.LogEvent(PhongBanId);
                int TotalRecords = ServiceFactory.GetInstanceKhieuNai_Activity().GetTotalKhieuNaiPhanHoi(PhongBanId, LoginAdmin.AdminLogin().Username, (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi);
                return TotalRecords.ToString("#,##0");
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