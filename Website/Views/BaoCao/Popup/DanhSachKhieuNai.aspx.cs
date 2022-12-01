using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using Website.AppCode.Controller;
using System.IO;
using System.Globalization;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using System.Web.Services;

namespace Website.Views.BaoCao.Popup
{
    public partial class danhsachkhieunai : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string fromPage = Request.QueryString["fromPage"];
                    int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    int doitacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    DateTime fromDate;//= Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate;//= Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int reportType = ConvertUtility.ToInt32(Request.QueryString["reportType"]);
                    int pageIndex = ConvertUtility.ToInt32(Request.QueryString["pageIndex"]);
                    int pageSize = ConvertUtility.ToInt32(Request.QueryString["pageSize"]);

                    int loaiKhieuNai_NhomId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNai_NhomId"], -1);
                    int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], -1);
                    int linhVucChungId = ConvertUtility.ToInt32(Request.QueryString["linhVucChungId"], -1);
                    int linhVucConId = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"], -1);

                    int nguonKhieuNai = ConvertUtility.ToInt32(Request.QueryString["nguonKhieuNai"], -1);

                    // Edited by	: Dao Van Duong
                    // Datetime		: 12.8.2016 10:27
                    // Note			: Bổ xung điều khiện lọc theo tỉnh, huyện
                    int tinhId = 0;
                    int.TryParse(Request.QueryString["TinhId"], out tinhId);
                    int huyenId = 0;
                    int.TryParse(Request.QueryString["HuyenId"], out huyenId);


                    string isActivity = Request.QueryString["isActivity"];
                    if (isActivity == "1")
                    {
                        sNoiDungBaoCao = (string)HttpContext.Current.Session["KhieuNaiId"];
                        return;
                    }

                    string userName = "";
                    string tenTruyCap = string.Empty;
                    string tenPhongBan = string.Empty;

                    string loaibc = Request.QueryString["loaibc"] != null ? Request.QueryString["loaibc"] : string.Empty;
                    if (fromPage == "baocaotonghoppakntheonguoidungvnpttt")
                    {
                        fromDate = DateTime.ParseExact(Request.QueryString["fromDate"],
                                        "yyyyMMdd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
                        toDate = DateTime.ParseExact(Request.QueryString["toDate"],
                                        "yyyyMMdd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
                        doitacId = ConvertUtility.ToInt32(Request.QueryString["doitacId"]);
                        lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                        lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                        userName = Request.QueryString["username"];
                    }
                    else
                    {
                        fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                        toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                        lblTuNgay.Text = Request.QueryString["fromDate"];
                        lblDenNgay.Text = Request.QueryString["toDate"];
                    }

                    lblTenFile.Text = string.Format("{0}_{1}_{2}", fromPage, fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));

                    switch (fromPage.ToLower())
                    {
                        case "baocaosoluongpakndaxulyttptdv":
                            tenTruyCap = Request.QueryString["tenTruyCap"];
                            lbPhongBan_NguoiDung.Text = string.Format("Người dùng : {0}", tenTruyCap);

                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoSoLuongPAKNDaXuLyTTPTDV_DanhSachKhieuNai(phongBanId, tenTruyCap, fromDate, toDate, reportType);
                            break;
                        case "bctonghoptheoloaiknvnp":
                            //int loaiKhieuNai_NhomId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNai_NhomId"]);                           
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            DoiTacInfo dtInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(doitacId);
                            LoaiKhieuNai_NhomInfo oLoaiKhieuNai = null;// ServiceFactory.GetInstanceLoaiKhieuNai() (LoaiKhieuNaiID); 
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListDanhsachLoaiKhieuNaiVNP(doitacId, fromDate, toDate, loaiKhieuNaiId, linhVucChungId, nguonKhieuNai, reportType);
                            switch (reportType)
                            {
                                case 1: // Số lượng tiếp nhận                                                      
                                    lblTenFile.Text = string.Format("VNP_DSSoLuongTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = dtInfo.TenDoiTac.ToUpper() + ": Số lượng khiếu nại tiếp nhận - " + oLoaiKhieuNai.TenNhom;
                                    break;
                                case 2: // Số lượng đã đóng trong kỳ                                                      
                                    lblTenFile.Text = string.Format("VNP_DSSoLuongDaDongTK_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = dtInfo.TenDoiTac.ToUpper() + ": Số lượng khiếu nại đã đóng trong kỳ - " + oLoaiKhieuNai.TenNhom;
                                    break;
                                case 3: // Số lượng đã đóng trong kỳ + tồn kỳ trước                                                      
                                    lblTenFile.Text = string.Format("VNP_DSSoLuongDaDongTKAndTonKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = dtInfo.TenDoiTac.ToUpper() + ": Số lượng khiếu nại đã đóng trong kỳ + Tồn kỳ trước - " + oLoaiKhieuNai.TenNhom;
                                    break;
                                case 4:// số lượng khiếu nại quá hạn toàn trình trong kỳ
                                    lblTenFile.Text = string.Format("VNP_DSSoLuongQuaHanToanTrinhTK_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = dtInfo.TenDoiTac.ToUpper() + ": Số lượng khiếu nại quá hạn toàn trình trong kỳ - " + oLoaiKhieuNai.TenNhom;
                                    break;
                                case 5: //số lượng khiếu nại quá hạn toàn trình trong kỳ + Tồn trước kỳ
                                    lblTenFile.Text = string.Format("VNP_DSSoLuongQuaHanToanTrinhTKAndTonTruocKy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = dtInfo.TenDoiTac.ToUpper() + ": Số lượng khiếu nại quá hạn toàn trình trong kỳ + Tồn trước kỳ - " + oLoaiKhieuNai.TenNhom;
                                    break;
                            }

                            break;

                        case "baocaotonghopphongcskhvnp":
                            PhongBanInfo phongBanInfo = ServiceFactory.GetInstancePhongBan().GetInfo(phongBanId);
                            if (phongBanInfo != null)
                            {
                                lbPhongBan_NguoiDung.Text = string.Format("Phòng ban : {0}", phongBanInfo.Name);
                            }

                            switch (reportType)
                            {
                                case 11:
                                    ltTitle.Text = "Số liệu PAKN tồn trước thời điểm đầu lấy báo cáo tại phòng ban (số liệu tồn quá hạn +số liệu tồn trong hạn)";
                                    break;
                                case 12:
                                    ltTitle.Text = "Số liệu PAKN tồn tính đến thời điểm lấy báo cáo tại phòng ban và được chuyển phòng khác xử lý";
                                    break;
                                case 2:
                                    ltTitle.Text = "";
                                    break;
                                case 21:
                                    ltTitle.Text = "Số liệu PAKN tạo mới trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN đã xử lý xong (đã đóng)";
                                    break;
                                case 22:
                                    ltTitle.Text = "Số liệu PAKN tạo mới trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN chưa xử lý (bao gồm: để thực hiện giải quyết và phải chuyển tiếp cho đơn vị khác)";
                                    break;
                                case 23:
                                    ltTitle.Text = "Số liệu PAKN tạo mới trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN đang ở tại đơn vị ";
                                    break;
                                case 3:
                                    ltTitle.Text = "";
                                    break;
                                case 31:
                                    ltTitle.Text = "Số liệu PAKN tiếp nhận trong khoảng thời gian lấy báo cáo tại phòng ban (số liệu này là do đơn vị khác chuyển đến )<br/>Số liệu xử lý tại phòng ban";
                                    break;
                                case 32:
                                    ltTitle.Text = "Số liệu PAKN tiếp nhận trong khoảng thời gian lấy báo cáo tại phòng ban (số liệu này là do đơn vị khác chuyển đến )<br/>Số liệu chuyển tiếp ngoài phòng ban";
                                    break;
                                case 4:
                                    ltTitle.Text = "";
                                    break;
                                case 41:
                                    ltTitle.Text = "Số liệu PAKN đã được xử lý trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN đã được đóng tại phòng ban";
                                    break;
                                case 42:
                                    ltTitle.Text = "Số liệu PAKN đã được xử lý trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN đã được phản hồi (KN đã được đóng trong khoảng thời gian lấy báo cáo)";
                                    break;
                                case 43:
                                    ltTitle.Text = "Số liệu PAKN đã được xử lý trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN phản hồi đã được trả lại (trả lại trong khoảng thời gian lấy báo cáo)";
                                    break;
                                case 44:
                                    ltTitle.Text = "Số liệu PAKN đã được xử lý trong khoảng thời gian lấy báo cáo tại phòng ban <br/> Số liệu PAKN phản hồi chưa được xử lý";
                                    break;
                                case 5:
                                    ltTitle.Text = "Số liệu PAKN được phản hồi về đơn vị  ";
                                    break;
                                case 6:
                                    ltTitle.Text = "Số liệu PAKN đang có tại thời điểm lấy báo cáo tại phòng ban";
                                    break;
                                case 7:
                                    ltTitle.Text = "Số liệu PAKN đã quá hạn tại thời điểm lấy báo cáo tại phòng ban";
                                    break;
                            }

                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiVNP_DanhSachKhieuNai(phongBanId, fromDate, toDate, reportType);
                            break;

                        case "dashboard":
                            switch (reportType)
                            {
                                case 11:
                                    ltTitle.Text = "Số liệu PAKN tiếp nhận trong khoảng thời gian lấy báo cáo tại phòng ban ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiTiepNhanCuaPhongTheoThoiGian("dashboard", "", 0, phongBanId, fromDate, toDate);
                                    break;
                                case 21:
                                    ltTitle.Text = "Số liệu PAKN đã đóng trong khoảng thời gian lấy báo cáo tại phòng ban ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiDaDongCuaPhongTheoThoiGian("dashboard", "", 0, phongBanId, fromDate, toDate);
                                    break;
                            }

                            break;
                        case "baocaotonghoppakntheonguoidungvnpttt":
                            switch (reportType)
                            {

                                case 11://tiep nhan
                                    ltTitle.Text = "Số liệu PAKN tiếp nhận trong khoảng thời gian lấy báo cáo của người dùng ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiTiepNhanCuaPhongTheoThoiGian("baocaotonghoppakntheonguoidungvnpttt", userName, doitacId, phongBanId, fromDate, toDate);
                                    break;
                                case 21: // pakn da xu ly
                                    ltTitle.Text = "Số liệu PAKN đã xử lý trong khoảng thời gian lấy báo cáo của người dùng ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiDaDongCuaPhongTheoThoiGian("baocaotonghoppakntheonguoidungvnpttt", userName, doitacId, phongBanId, fromDate, toDate);

                                    break;
                                case 22: // pakn ton dong
                                    ltTitle.Text = "Số liệu PAKN đang tồn đọng trong khoảng thời gian lấy báo cáo của người dùng ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopChiTietPAKNTheoNguoiDungVNPTTT("baocaotonghoppakntheonguoidungvnpttt", userName, doitacId, phongBanId, fromDate, toDate, 22);
                                    break;
                                case 23: // pakn qua han
                                    ltTitle.Text = "Số liệu PAKN đã quá hạn trong khoảng thời gian lấy báo cáo của người dùng ";
                                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopChiTietPAKNTheoNguoiDungVNPTTT("baocaotonghoppakntheonguoidungvnpttt", userName, doitacId, phongBanId, fromDate, toDate, 23);
                                    break;
                            }
                            break;
                        case "baocaosoluongtondonghoacquahancuadoitac":
                            doitacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                            phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"], -1);
                            string tenDoiTac = Request.QueryString["tenDoiTac"] != null ? Request.QueryString["tenDoiTac"] : string.Empty;
                            switch (reportType)
                            {
                                case 1:
                                    ltTitle.Text = "Số liệu PAKN tồn đọng " + tenDoiTac;
                                    break;
                                case 2:
                                    ltTitle.Text = "Số liệu PAKN quá hạn " + tenDoiTac;
                                    break;
                            }

                            sNoiDungBaoCao = new BuildBaoCao().ListKhieuNaiTonDongHoacQuaHan(doitacId, phongBanId, reportType);

                            break;
                        case "baocaotonghoptogqkn":
                            phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"], -1);
                            int loaiKhieuNaiType = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiType"], -1);
                            //int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], -1);
                            switch (reportType)
                            {
                                case 1:
                                    ltTitle.Text = "Lũy kế khiếu nại đã quyết trước ngày " + fromDate.ToString("dd/MM/yyyy");
                                    break;
                                case 2:
                                    ltTitle.Text = "Lũy kế khiếu nại tồn đọng trước ngày " + fromDate.ToString("dd/MM/yyyy");
                                    break;
                                case 3:
                                    ltTitle.Text = string.Format("Số lượng tiếp nhận từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                                    break;
                                case 4:
                                    ltTitle.Text = string.Format("Tổng số khiếu nại giải quyết được từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                                    break;
                            }

                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopToGQKN_DanhSachKhieuNai(phongBanId, fromDate, toDate, loaiKhieuNaiType, loaiKhieuNaiId, reportType);
                            break;

                        case "danhsachkhieunaiquahan":
                            ltTitle.Text = "Danh sách khiếu nại quá hạn";
                            List<int> listKhuVucXuLyId = new List<int>();
                            List<int> listDoiTacXuLyId = new List<int>();
                            List<int> listPhongBanXuLyId = new List<int>();
                            string toTime = Request.QueryString["toTime"] != null ? Request.QueryString["toTime"] : string.Empty;
                            if (toTime.Length > 0)
                            {
                                string[] arrTime = toTime.Split(':');
                                int hour = 0;
                                int minute = 0;
                                int second = 0;
                                if (arrTime.Length >= 1)
                                {
                                    hour = ConvertUtility.ToInt32(arrTime[0]);
                                }
                                if (arrTime.Length >= 2)
                                {
                                    minute = ConvertUtility.ToInt32(arrTime[1]);
                                }
                                if (arrTime.Length >= 3)
                                {
                                    second = ConvertUtility.ToInt32(arrTime[2]);
                                }

                                if (hour >= 0 && hour <= 23
                                    && minute >= 0 && minute <= 59 && second >= 0 && second <= 59)
                                {
                                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, hour, minute, second);
                                }
                                else
                                {
                                    sNoiDungBaoCao = "Dữ liệu thời gian nhập vào sai. Nhập HH:mm:ss";

                                    return;
                                }
                            }

                            string[] listDoiTac = Request.QueryString["listDoiTac"] != null ? Request.QueryString["listDoiTac"].Trim().Split(',') : null;
                            if (listDoiTac != null)
                            {
                                for (int i = 0; i < listDoiTac.Length; i++)
                                {
                                    listDoiTacXuLyId.Add(Convert.ToInt32(listDoiTac[i]));
                                }
                            }

                            //sNoiDungBaoCao = new BuildBaoCao().BaoCaoChiTietKhieuNaiQuaHan(listKhuVucXuLyId, listDoiTacXuLyId, listPhongBanXuLyId, fromDate, toDate);
                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoChiTietKhieuNaiQuaHan(listDoiTacXuLyId, toDate);

                            break;

                        case "bc_baocaokhieunaichuyenbophankhac":
                            lblDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"], -1);
                            string sPhongBanTiepNhan = Request.QueryString["listPhongBanTiepNhanId"];
                            List<int> listPhongBanTiepNhanId = new List<int>();
                            string[] arrPhongBanTiepNhanId = sPhongBanTiepNhan.Split(',');
                            if (arrPhongBanTiepNhanId != null && arrPhongBanTiepNhanId.Length > 0)
                            {
                                for (int i = 0; i < arrPhongBanTiepNhanId.Length; i++)
                                {
                                    listPhongBanTiepNhanId.Add(ConvertUtility.ToInt32(arrPhongBanTiepNhanId[i]));
                                }
                            }

                            sNoiDungBaoCao = new BuildBaoCao().ListKhieuNaiDaChuyenDonViKhac(phongBanId, listPhongBanTiepNhanId);
                            break;

                        case "baocaokhoiluongcongviecdkt":
                            phongBanId = -1;
                            tenTruyCap = Request.QueryString["tenTruyCap"];
                            ltTitle.Text = string.Format("Danh sách khiếu nại đã đóng bởi {0}", tenTruyCap);
                            sNoiDungBaoCao = new BuildBaoCao().ListKhieuNaiDaDongTheoNguoiDung(phongBanId, tenTruyCap, fromDate, toDate);
                            break;

                        case "baocaotonghoppakntttc":
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoDoiTac(doitacId, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("TTTC_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách tồn kỳ trước";
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("TTTC_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách tiếp nhận";
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("TTTC_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách đã xử lý";
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("TTTC_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách đã xử lý quá hạn";
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("TTTC_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách tồn đọng";
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("TTTC_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = "TTTC : Danh sách tồn đọng quá hạn";
                                    break;
                            }

                            break;

                        case "baocaotonghoppaknphongbantttc":
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoPhongBan(phongBanId, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                            tenPhongBan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(phongBanId);
                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("PB_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("PB_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("PB_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("PB_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("PB_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("PB_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Phòng ban : {0}", tenPhongBan);
                                    break;
                            }
                            break;

                        case "baocaotonghoppaknnguoidungphongbantttc":
                            tenTruyCap = Request.QueryString["tenTruyCap"];
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            //sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoNguoiDungPhongBan(phongBanId, tenTruyCap, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoNguoiDungPhongBan_V2(doitacId, phongBanId, tenTruyCap, fromDate, toDate, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);

                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("User_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("User_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("User_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("User_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("User_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("User_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                            }

                            break;

                        case "baocaotondongvaquahantttc":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(doitacId, phongBanId, reportType, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                            tenPhongBan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(phongBanId);
                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("PB_DSTonDong");
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Phòng ban : {0}", tenPhongBan);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("PB_DSTonDongQuaHan");
                                    ltTitle.Text = string.Format("Tồn đọng quá hạn - Phong ban : {0}", tenPhongBan);
                                    break;
                            }
                            break;

                        case "baocaotonghoppakndoitac":
                            //tenTruyCap = Request.QueryString["tenTruyCap"];
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoDoiTac_V2(doitacId, fromDate, toDate, reportType);

                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("DoiTac_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("DoiTac_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("DoiTac_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("DoiTac_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("DoiTac_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("DoiTac_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 7: // Số lượng tạo mới
                                    lblTenFile.Text = string.Format("DoiTac_DSTaoMoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tạo mới - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 8: // Số lượng đã đóng
                                    lblTenFile.Text = string.Format("DoiTac_DSDaDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã đóng - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 9: // Số lượng chuyển ngang hàng
                                    lblTenFile.Text = string.Format("DoiTac_DSChuyenNgangHang_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển ngang hàng - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 10: // Số lượng chuyển xử lý
                                    lblTenFile.Text = string.Format("DoiTac_DSChuyenXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển xử lý - Đơn vị : {0}", tenTruyCap);
                                    break;
                                case 11: // Số lượng chuyển phản hồi
                                    lblTenFile.Text = string.Format("DoiTac_DSChuyenPhanHoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển phản hồi - Đơn vị : {0}", tenTruyCap);
                                    break;
                            }
                            break;

                        case "baocaotonghoppaknphongban":
                            //tenTruyCap = Request.QueryString["tenTruyCap"];
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoPhongBan_V2(phongBanId, fromDate, toDate, reportType);

                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("PhongBan_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("PhongBan_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("PhongBan_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("PhongBan_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("PhongBan_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("PhongBan_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 7: // Số lượng tạo mới
                                    lblTenFile.Text = string.Format("PhongBan_DSTaoMoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tạo mới - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 8: // Số lượng đã đóng
                                    lblTenFile.Text = string.Format("PhongBan_DSDaDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã đóng - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 9: // Số lượng chuyển ngang hàng
                                    lblTenFile.Text = string.Format("PhongBan_DSChuyenNgangHang_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển ngang hàng - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 10: // Số lượng chuyển xử lý
                                    lblTenFile.Text = string.Format("PhongBan_DSChuyenXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển xử lý - Phòng ban : {0}", tenTruyCap);
                                    break;
                                case 11: // Số lượng chuyển phản hồi
                                    lblTenFile.Text = string.Format("PhongBan_DSChuyenPhanHoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển phản hồi - Phòng ban : {0}", tenTruyCap);
                                    break;
                            }
                            break;

                        case "baocaotonghoppaknnguoidung":
                            tenTruyCap = Request.QueryString["tenTruyCap"];
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoNguoiDungPhongBan_V2(doitacId, phongBanId, tenTruyCap, fromDate, toDate, reportType);

                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("User_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("User_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("User_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("User_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("User_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("User_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 7: // Số lượng tạo mới
                                    lblTenFile.Text = string.Format("User_DSTaoMoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tạo mới - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 8: // Số lượng đã đóng
                                    lblTenFile.Text = string.Format("User_DSDaDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã đóng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 9: // Số lượng chuyển ngang hàng
                                    lblTenFile.Text = string.Format("User_DSChuyenNgangHang_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển ngang hàng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 10: // Số lượng chuyển xử lý
                                    lblTenFile.Text = string.Format("User_DSChuyenXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển xử lý - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 11: // Số lượng chuyển phản hồi
                                    lblTenFile.Text = string.Format("User_DSChuyenPhanHoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển phản hồi - Người dùng : {0}", tenTruyCap);
                                    break;
                            }

                            break;
                        // Edited by	: Dao Van Duong
                        // Datetime		: 12.8.2016 10:30
                        // Note			: Bổ xung điều khiện lọc tỉnh, huyện
                        case "baocaotonghoppaknnguoidungv4":
                            tenTruyCap = Request.QueryString["tenTruyCap"];
                            lblTuNgay.Text = fromDate.ToString("dd/MM/yyyy");
                            lblDenNgay.Text = toDate.ToString("dd/MM/yyyy");
                            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoNguoiDungPhongBan_V4(doitacId, phongBanId, tenTruyCap, fromDate, toDate, reportType, tinhId, huyenId);

                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("User_DSTonDongKyTruoc_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn kỳ trước - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("User_DSTiepNhan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tiếp nhận - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 3: // Số lượng đã xử lý
                                    lblTenFile.Text = string.Format("User_DSDaXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 4: // Số lượng đã xử lý quá hạn
                                    lblTenFile.Text = string.Format("User_DSDaXuLyQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã xử lý quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 5: // Số lượng tồn đọng
                                    lblTenFile.Text = string.Format("User_DSTonDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 6: // Số lượng tồn đọng quá hạn
                                    lblTenFile.Text = string.Format("User_DSTonDongQuaHan_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tồn đọng quá hạn - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 7: // Số lượng tạo mới
                                    lblTenFile.Text = string.Format("User_DSTaoMoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách tạo mới - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 8: // Số lượng đã đóng
                                    lblTenFile.Text = string.Format("User_DSDaDong_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách đã đóng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 9: // Số lượng chuyển ngang hàng
                                    lblTenFile.Text = string.Format("User_DSChuyenNgangHang_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển ngang hàng - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 10: // Số lượng chuyển xử lý
                                    lblTenFile.Text = string.Format("User_DSChuyenXuLy_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển xử lý - Người dùng : {0}", tenTruyCap);
                                    break;
                                case 11: // Số lượng chuyển phản hồi
                                    lblTenFile.Text = string.Format("User_DSChuyenPhanHoi_{0}_{1}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
                                    ltTitle.Text = string.Format("Danh sách chuyển phản hồi - Người dùng : {0}", tenTruyCap);
                                    break;
                            }

                            break;

                        case "baocaosoluongdichvutoanmang":
                            int khuVucId_baocaosoluongdichvutoanmang = ConvertUtility.ToInt32(Request.QueryString["khuVucId"], 1);
                            int loaiKhieuNaiId_baocaosoluongdichvutoanmang = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], 0);
                            int linhVucChungId_baocaosoluongdichvutoanmang = ConvertUtility.ToInt32(Request.QueryString["linhVucChungId"], 0);
                            int linhVucConId_baocaosoluongdichvutoanmang = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"], 0);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiToanMangCuaPTDV(khuVucId_baocaosoluongdichvutoanmang, loaiKhieuNaiId_baocaosoluongdichvutoanmang,
                                                                                linhVucChungId_baocaosoluongdichvutoanmang, linhVucConId_baocaosoluongdichvutoanmang, fromDate, toDate, reportType);

                            switch (reportType)
                            {
                                case 1:
                                    ltTitle.Text = "Danh sách khiếu nại tiếp nhận";
                                    break;
                                case 2:
                                    ltTitle.Text = "Danh sách khiếu nại đã đóng";
                                    break;

                            }

                            break;

                        case "baocaotonghopkhieunaitoanmangtheotuan":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiToanMangTheoTuan(fromDate, toDate, nguonKhieuNai, reportType);
                            switch (reportType)
                            {
                                case 1:
                                    ltTitle.Text = "Số lượng khiếu nại tiếp nhận trong tuần";
                                    break;
                                case 2:
                                    ltTitle.Text = "Số lượng khiếu nại đã giải quyết trong tuần";
                                    break;
                                case 3:
                                    ltTitle.Text = "Số lượng khiếu nại quá hạn trong tuần";
                                    break;
                                case 4:
                                    ltTitle.Text = string.Format("Số lượng khiếu nại giải quyết từ đầu tháng đến {0}", toDate.ToString("dd/MM/yyyy"));
                                    break;
                                case 5:
                                    ltTitle.Text = string.Format("Số lượng khiếu nại đã giải quyết từ đầu năm đến {0}", toDate.ToString("dd/MM/yyyy"));
                                    break;
                            }
                            break;

                        case "baocaotonghopkhieunaitoanmangtheothang":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiToanMangTheoThang(toDate, nguonKhieuNai, reportType);
                            switch (reportType)
                            {
                                case 1:
                                    ltTitle.Text = string.Format("Số lượng khiếu nại đã giải quyết tháng {0}", toDate.ToString("MM/yyyy"));
                                    break;
                                case 2:
                                    ltTitle.Text = string.Format("Số lượng khiếu nại đã giải quyết tháng {0}", toDate.AddMonths(-1).ToString("MM/yyyy"));
                                    break;
                                case 3:
                                    ltTitle.Text = string.Format("Số lượng khiếu nại đã giải quyết từ đầu năm đến cuối tháng {0}", toDate.ToString("MM/yyyy"));
                                    break;
                            }
                            break;

                        case "bc_common_baocaotondongnguoidungphongban":
                            string nguoiXuLy_bc_common_baocaotondongnguoidungphongban = Request.QueryString["nguoiXuLy"];
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTonDongQuaHanNguoiDungPhongBanHienTai(phongBanId, nguoiXuLy_bc_common_baocaotondongnguoidungphongban, reportType);
                            tenPhongBan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(phongBanId);
                            lblTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            switch (reportType)
                            {
                                case 1: // Số lượng tồn đọng kỳ trước
                                    lblTenFile.Text = string.Format("PB_DSTonDong");
                                    ltTitle.Text = string.Format("Danh sách tồn đọng - Người xử lý : {0}", nguoiXuLy_bc_common_baocaotondongnguoidungphongban);
                                    break;
                                case 2: // Số lượng tiếp nhận
                                    lblTenFile.Text = string.Format("PB_DSTonDongQuaHan");
                                    ltTitle.Text = string.Format("Tồn đọng quá hạn - Người xử lý : {0}", nguoiXuLy_bc_common_baocaotondongnguoidungphongban);
                                    break;
                            }
                            break;

                        case "bc_vnp_thongkekhieunaitheonguyennhanloi":
                            int khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                            int nguyenNhanLoiId = ConvertUtility.ToInt32(Request.QueryString["nguyenNhanLoiId"]);
                            int chiTietLoiId = ConvertUtility.ToInt32(Request.QueryString["chiTietLoiId"]);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoNguyenNhanLoi(khuVucId, doitacId, phongBanId, loaiKhieuNaiId, linhVucChungId, linhVucConId, nguyenNhanLoiId, chiTietLoiId, fromDate, toDate, nguonKhieuNai);
                            lblTenFile.Text = "ListKhieuNaiTheoNguyenNhanLoi";

                            break;

                        case "bc_vnp_danhsachkhieunaitheoloaikhieunai":
                            int khuVucId_DanhSachKhieuNaiTheoLoaiKhieuNai = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);

                            List<int> listLoaiKhieuNaiId = (List<int>)Session["ListLoaiKhieuNaiId"];
                            List<int> listLinhVucChungId = (List<int>)Session["ListLinhVucChungId"];
                            List<int> listLinhVucConId = (List<int>)Session["ListLinhVucConId"];

                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoLoaiKhieuNai(khuVucId_DanhSachKhieuNaiTheoLoaiKhieuNai, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, nguonKhieuNai, fromDate, toDate);
                            lblTenFile.Text = "ListKhieuNaiTheoLoaiKhieuNai";

                            Session.Remove("ListLoaiKhieuNaiId");
                            Session.Remove("ListLinhVucChungId");
                            Session.Remove("ListLinhVucConId");

                            break;

                        case "baocaotonghopchatluongphucvu":
                            int typeKhenChe = ConvertUtility.ToInt32(Request.QueryString["typeKhenChe"], -1);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiChatLuongPhucVu(reportType, doitacId, loaiKhieuNaiId, linhVucChungId,
                                                                                                            linhVucConId, typeKhenChe, nguonKhieuNai, fromDate, toDate);
                            break;

                        case "baocaosoluongchuyenxulyvnp":
                            lblTenFile.Text = "So luong chuyen xu ly VNP";
                            string nguoiXuLy_BaoCaoSoLuongChuyenXuLyVNP = Request.QueryString["nguoiXuLy"];
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiChuyenXuLyVNPTheoUser(doitacId, phongBanId, nguoiXuLy_BaoCaoSoLuongChuyenXuLyVNP, fromDate, toDate);
                            break;

                        case "baocaotonghopchatluongmang":
                            int khuVucId_BCTHCLM = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                            int maTinhId_BCTHCLM = ConvertUtility.ToInt32(Request.QueryString["maTinhId"]);

                            List<int> listLoaiKhieuNaiId_BCTHCLM = new List<int>();
                            if (loaiKhieuNaiId != -1)
                            {
                                listLoaiKhieuNaiId_BCTHCLM.Add(loaiKhieuNaiId);
                            }

                            List<int> listLinhVucChungId_BCTHCLM = new List<int>();
                            if (linhVucChungId != -1)
                            {
                                listLinhVucChungId_BCTHCLM.Add(linhVucChungId);
                            }

                            List<int> listLinhVucConId_BCTHCLM = new List<int>();
                            if (linhVucConId != -1)
                            {
                                listLinhVucConId_BCTHCLM.Add(linhVucConId);
                            }

                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoLoaiKhieuNai(khuVucId_BCTHCLM, listLoaiKhieuNaiId_BCTHCLM, listLinhVucChungId_BCTHCLM, listLinhVucConId_BCTHCLM, maTinhId_BCTHCLM, nguonKhieuNai, fromDate, toDate);
                            lblTenFile.Text = "ListKhieuNaiTheoLoaiKhieuNai";
                            break;
                        case "baocaotonghopchatluongmangactivity":
                            string whereClause = ConvertUtility.ToString(Request.QueryString["whereClause"]);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoDanhSachKhieuNaiId(whereClause);
                            lblTenFile.Text = "ListKhieuNaiTheoLoaiKhieuNai";
                            break;
                        case "baocaotyletondongquahanphongban":
                            DateTime ngayQuaHan_BCTLTDQHPB = fromDate = Convert.ToDateTime(Request.QueryString["ngayQuaHan"], new CultureInfo("vi-VN"));
                            ngayQuaHan_BCTLTDQHPB = new DateTime(ngayQuaHan_BCTLTDQHPB.Year, ngayQuaHan_BCTLTDQHPB.Month, ngayQuaHan_BCTLTDQHPB.Day, 23, 59, 59, 999);
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiQuaHanPhongBan(doitacId, phongBanId, ngayQuaHan_BCTLTDQHPB);
                            lblDenNgay.Text = ngayQuaHan_BCTLTDQHPB.ToString("dd/MM/yyyy HH:mm");
                            break;

                        case "danhsachkhieunaigiamtru":
                            bool isExportExcel_DanhSachKhieuNaiGiamTru = false;
                            int khuVucId_DanhSachKhieuNaiGiamTru = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                            sNoiDungBaoCao = new BuildBaoCao().DanhSachKhieuNaiGiamTru(khuVucId_DanhSachKhieuNaiGiamTru, doitacId, phongBanId,
                                                                                        fromDate, toDate, nguonKhieuNai, isExportExcel_DanhSachKhieuNaiGiamTru);
                            break;

                        case "baocaotonghoppaknvnptx":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoVNPTX(doitacId, fromDate, toDate, reportType);
                            break;

                        case "baocaotonghoppakndoitacvnptnet":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoDoiTac_V2_NET(doitacId, fromDate, toDate, reportType);
                            break;

                        case "baocaotonghoppaknphongbanvnptnet":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoPhongBan_V2_NET(phongBanId, fromDate, toDate, reportType);
                            break;

                        case "baocaokhieunaidichvugtgttapdoan":
                            sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiDVGTGTTapDoan(linhVucConId, fromDate, toDate, reportType);
                            break;

                    }

                    lblNoiDungBaoCao.Text = sNoiDungBaoCao;

                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/03/2014
        /// Todo : Xuất excel nội dung html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            //string url = string.Format("{0}&loaibc=excel", Request.Url.AbsoluteUri);
            //Response.Redirect(url);
            sNoiDungBaoCao = lblNoiDungBaoCao.Text;
            btnExportExcelTop.Visible = false;
            btnExportExcel.Visible = false;
            export2excel(lblTenFile.Text);
        }

        private void export2excel(string tenbc)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCSS = Server.MapPath("~/CSS");
            pathCSS += @"\BaoCao.css";
            StreamReader reader = new StreamReader(pathCSS);
            //reader.ReadToEnd();

            Response.Write("<style>");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        [WebMethod]
        public static string GetListKhieuNaiId(string lstKhieuNaiId)
        {
            var sNoiDungBaoCao = new BuildBaoCaoChiTietKhieuNai().ListKhieuNaiTheoDanhSachKhieuNaiId(lstKhieuNaiId);
            HttpContext.Current.Session["KhieuNaiId"] = sNoiDungBaoCao;
            return "";
        }
    }
}