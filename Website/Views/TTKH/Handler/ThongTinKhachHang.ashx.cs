
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.TTKH.Handler
{
    /// <summary>
    /// Summary description for ThongTinKhachHang
    /// </summary>
    public class ThongTinKhachHang : IHttpHandler, IRequiresSessionState
    {
        protected class DateKhieuNai
        {
            public int page { get; set; }
            public int total { get; set; }
            public List<DataItem> rows { get; set; }
        }

        protected class DataItem
        {
            public string MaKhieuNai { get; set; }
            public string TrangThai { get; set; }
            public string PhongBanXuLy { get; set; }
            public string NguoiXuLy { get; set; }
            public string NoiDungPA { get; set; }
            public string NgayTiepNhanSort { get; set; }
            public string PhongBanTiepNhan { get; set; }
            public string NguoiTiepNhan { get; set; }
            public string NgayDongKNSort { get; set; }
            public string NoiDungXuLyDongKN { get; set; }
            public string DoUuTien { get; set; }
            public string LoaiKhieuNai { get; set; }
            public string LinhVucChung { get; set; }
            public string LinhVucCon { get; set; }
            public string HoTenLienHe { get; set; }
            public string DiaChi_CCBS { get; set; }
            public string DiaChiLienHe { get; set; }

            public string SDTLienHe { get; set; }
            public string DiaDiemXayRa { get; set; }
            public string ThoiGianXayRa { get; set; }
            public string GhiChu { get; set; }
            public string MaTinh { get; set; }
            public string MaQuan { get; set; }
            public string HTTiepNhan { get; set; }
            public string NgayQuaHanSort { get; set; }

            public string KNHangLoat { get; set; }
            //public string IsKNGiamTru { get; set; }
            public string IsLuuKhieuNai { get; set; }
            public string IsKNGiamTru { get; set; }
            public int CallCount { get; set; }
            public string LDate { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.Form["type"] ?? context.Request.QueryString["type"];

            switch (type)
            {
                case "GetInfo":
                    GetInfo(context);
                    break;
                case "GetInfoBasic":
                    GetInfoBasic(context);
                    break;
                case "GetServiceDaDung":
                    GetServiceDaDung(context);
                    break;

                case "ExportThongTinKH":
                    ExportThongTinKH(context);
                    break;

                case "LSKhieuNai":
                    LichSuKN(context);
                    break;

                case "LichSuNapThe":
                    LichSuNapThe(context);
                    break;

                case "LichSuThueBao":
                    LichSuThueBao(context);
                    break;

                case "LichSu3G":
                    LichSu3G(context);
                    break;

                case "TraCuuSMS888":
                    TraCuuSMS888(context);
                    break;

                case "LichSuBuTien":
                    LichSuBuTien(context);
                    break;
            }
        }

        private void GetInfo(HttpContext context)
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();
            if (userInfo == null)
            {
                context.Response.Write(OutputJSONToAJax.ToJSON(-1, "Bạn chưa đăng nhập hoặc hết phiên làm việc."));
                return;
            }

            string tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
            tb = "84" + tb;

            if (Config.IsLocal)
            {
                //Nếu 1 ngày KN > 2 lần thì cảnh báo.
                string whereSolr = string.Format("SoThueBao:{0} AND NgayTiepNhanSort:{1}", context.Request.Form["tb"], DateTime.Now.ToString("yyyyMMdd"));
                int result = ServiceFactory.GetInstanceKhieuNai().CountKhieuNaiInSolr(whereSolr);

                //infoFull.MessageWarningKhieuNai = "";
                if (result > 0)
                {
                    context.Response.Write("{\"MessageWarningKhieuNai\":" + result + ",");
                }
                else
                    context.Response.Write("{\"MessageWarningKhieuNai\":0,");

                if (tb == "84942621224")
                    context.Response.Write("\"MSISDN\":84942621224,\"FULLNAME\":\"hoang  thi ngoc\",\"IDNUMBER\":\"135025312\",\"BIRTHDAY\":\"20/12/1988\",\"GENDER\":\"male\",\"COMPANY\":null,\"AGENTID\":\"280214\",\"REGISTERDATE\":\"30/01/2012\",\"REGISTERMETHODID\":\"8\",\"NATIONALITYID\":\"232\",\"ADDRESS\":null,\"IDNUMBERTYPE\":\"1\",\"PLACEOFISSUE\":\"15122003\",\"PLACEDATE\":null,\"ADDRESS_COMPANY\":null,\"Balance\":\"37.07\",\"BalanceKM\":\"0.0\",\"BalanceKM1\":\"0.0\",\"BalanceKM2\":\"0.0\",\"BalanceData\":\"0.0\",\"HSD\":\"06/02/2014\",\"SO_MSIN\":\"2116088251\",\"TRANG_THAI\":\"MO\",\"TEN_LOAI\":\"MyZone\",\"NGAY_KH\":\"30/01/2012\",\"SO_PIN\":\"1234\",\"SO_PUK\":\"16515844\",\"PIN2\":\"1234\",\"PUK2\":\"78516366\",\"Input_STB\":null,\"SO_MAY\":null,\"Ma_KH\":null,\"Ngay_Sinh\":null,\"Ten_TT\":null,\"Dia_Chi_TT\":null,\"GOI_DI\":\"A\",\"GOI_DEN\":\"D\",\"MA_TINH\":\"CBG\",\"MA_KH\":\"\",\"MA_TB\":\"\",\"LOAITB_ID\":\"\",\"TEN_TB\":\"\",\"TEN_TT\":\"\",\"DIACHI_TT\":\"\",\"DIACHI_CT\":\"\",\"SO_GT\":\"\",\"DIENTHOAI_LH\":\"\",\"MS_THUE\":\"\",\"TRANGTHAI_ID\":\"\",\"DIADIEMTT_ID\":\"\",\"QUOCTICH\":\"\",\"DIACHI_CT1\":\"\",\"TEN_KH\":\"\",\"LOAIKH_ID\":\"\",\"MIENCUOC\":\"\",\"PHUONG_ID\":\"\",\"PHO_ID\":\"\",\"QUAN_ID\":\"\",\"LOAIGT_ID\":\"\",\"SODAIDIEN\":\"\",\"SO_NHA\":\"\",\"NGAYCAP_GT\":\"\",\"PHAI\":\"\",\"MA_CQ\":\"\",\"MA_BC\":\"\",\"KHLON_ID\":\"\",\"NGAYSINH\":null,\"MA_T\":\"\",\"DONVIQL_ID\":\"\",\"NGANHNGHE_ID\":\"\",\"UUTIEN_ID\":\"\",\"DANGKY_DB\":\"\",\"DANGKY_TV\":\"\",\"QUANTT_ID\":\"\",\"PHUONGTT_ID\":\"\",\"PHOTT_ID\":\"\",\"SOTT_NHA\":\"\",\"MA_NV\":\"\",\"TTNO\":\"\"}");
                else
                    context.Response.Write("\"Balance\":null,\"BalanceKM\":null,\"BalanceKM1\":null,\"BalanceKM2\":null,\"BalanceData\":null,\"HSD\":null,\"SO_MSIN\":\"2121592279\",\"TRANG_THAI\":\"MO\",\"TEN_LOAI\":\"Post\",\"NGAY_KH\":\"18/12/2013\",\"SO_PIN\":\"1234\",\"SO_PUK\":\"71137215\",\"PIN2\":\"1234\",\"PUK2\":\"28043925\",\"Input_STB\":null,\"SO_MAY\":null,\"Ma_KH\":null,\"Ngay_Sinh\":null,\"Ten_TT\":null,\"Dia_Chi_TT\":null,\"GOI_DI\":\"A\",\"GOI_DEN\":\"A\",\"MA_TINH\":\"HNI\",\"MA_KH\":\"HNIDD00610872\",\"MA_TB\":\"20\",\"LOAITB_ID\":\"20\",\"TEN_TB\":\"Lê Xuân Long\",\"TEN_TT\":\"Lê Xuân Long\",\"DIACHI_TT\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"DIACHI_CT\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"SO_GT\":\"012866239\",\"DIENTHOAI_LH\":\"84916988386\",\"MS_THUE\":\"\",\"TRANGTHAI_ID\":\"1\",\"DIADIEMTT_ID\":\"0\",\"QUOCTICH\":\"Viet Nam\",\"DIACHI_CT1\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"TEN_KH\":\"Lê Xuân Long\",\"LOAIKH_ID\":\"Nhóm khách hàng cá nhân, hộ gia đình\",\"MIENCUOC\":\"0\",\"PHUONG_ID\":\"23716\",\"PHO_ID\":\"307566\",\"QUAN_ID\":\"120\",\"LOAIGT_ID\":\"1\",\"SODAIDIEN\":\"84916988386\",\"SO_NHA\":\"176 Kim Giang\",\"NGAYCAP_GT\":\"29/08/2012\",\"PHAI\":\"Nữ\",\"MA_CQ\":\"567136\",\"MA_BC\":\"DA\",\"KHLON_ID\":\"1\",\"NGAYSINH\":null,\"MA_T\":\"PDA601-99\",\"DONVIQL_ID\":\"5\",\"NGANHNGHE_ID\":\"1\",\"UUTIEN_ID\":\"0\",\"DANGKY_DB\":\"1\",\"DANGKY_TV\":\"1\",\"QUANTT_ID\":\"120\",\"PHUONGTT_ID\":\"23716\",\"PHOTT_ID\":\"307566\",\"SOTT_NHA\":\"Đội 6 - Thôn Bầu\",\"MA_NV\":\"PDA601\",\"TTNO\":\"0 VND\"}");
            }
            else
            {
                try
                {
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;

                    ServiceVNP.TBTraTruocFullInfo result = obj.GetInfo(requestParam);


                    if (result != null)
                    {
                        try
                        {
                            string whereSolr = string.Format("SoThueBao:{0} AND NgayTiepNhanSort:{1}", context.Request.Form["tb"], DateTime.Now.ToString("yyyyMMdd"));
                            var resultSolr = ServiceFactory.GetInstanceKhieuNai().CountKhieuNaiInSolr(whereSolr);
                            result.MessageWarningKhieuNai = "";
                            if (resultSolr > 0)
                            {
                                result.MessageWarningKhieuNai = resultSolr.ToString();
                            }
                        }
                        catch { }
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                    }
                    else
                        context.Response.Write("");
                    return;
                }
                catch (Exception ex)
                {
                    //Utility.LogEvent(ex);
                    //if(ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("nullpointerexception"))
                    context.Response.Write(OutputJSONToAJax.ToJSON(-1, string.Format("Số thuê bao {0} chưa đăng ký, bạn có thể tiếp tục tạo khiếu nại.", tb), string.Empty, "warning"));
                    //else
                    //    context.Response.Write(OutputJSONToAJax.ToJSON(-1, ex.Message));
                    return;
                }
                context.Response.Write(OutputJSONToAJax.ToJSON(-1, "Không lấy được dữ liệu từ TTTC."));
            }
        }

        private void GetInfoBasic(HttpContext context)
        {
            var userInfo = LoginAdmin.AdminLogin();
            if (userInfo == null)
            {
                context.Response.Write(OutputJSONToAJax.ToJSON(-1, "Bạn chưa đăng nhập hoặc hết phiên làm việc."));
                return;
            }

            string tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
            tb = "84" + tb;

            if (Config.IsLocal)
            {
                //Nếu 1 ngày KN > 2 lần thì cảnh báo.
                string whereSolr = string.Format("SoThueBao:{0} AND NgayTiepNhanSort:{1}", context.Request.Form["tb"], DateTime.Now.ToString("yyyyMMdd"));
                var result = ServiceFactory.GetInstanceKhieuNai().CountKhieuNaiInSolr(whereSolr);
                //infoFull.MessageWarningKhieuNai = "";
                if (result > 0)
                {
                    context.Response.Write("{\"MessageWarningKhieuNai\":" + result + ",");
                }
                else
                    context.Response.Write("{\"MessageWarningKhieuNai\":0,");

                if (tb == "84942621224")
                    context.Response.Write("\"MSISDN\":84942621224,\"FULLNAME\":\"hoang  thi ngoc\",\"IDNUMBER\":\"135025312\",\"BIRTHDAY\":\"20/12/1988\",\"GENDER\":\"male\",\"COMPANY\":null,\"AGENTID\":\"280214\",\"REGISTERDATE\":\"30/01/2012\",\"REGISTERMETHODID\":\"8\",\"NATIONALITYID\":\"232\",\"ADDRESS\":null,\"IDNUMBERTYPE\":\"1\",\"PLACEOFISSUE\":\"15122003\",\"PLACEDATE\":null,\"ADDRESS_COMPANY\":null,\"Balance\":\"37.07\",\"BalanceKM\":\"0.0\",\"BalanceKM1\":\"0.0\",\"BalanceKM2\":\"0.0\",\"BalanceData\":\"0.0\",\"HSD\":\"06/02/2014\",\"SO_MSIN\":\"2116088251\",\"TRANG_THAI\":\"MO\",\"TEN_LOAI\":\"MyZone\",\"NGAY_KH\":\"30/01/2012\",\"SO_PIN\":\"1234\",\"SO_PUK\":\"16515844\",\"PIN2\":\"1234\",\"PUK2\":\"78516366\",\"Input_STB\":null,\"SO_MAY\":null,\"Ma_KH\":null,\"Ngay_Sinh\":null,\"Ten_TT\":null,\"Dia_Chi_TT\":null,\"GOI_DI\":\"A\",\"GOI_DEN\":\"D\",\"MA_TINH\":\"CBG\",\"MA_KH\":\"\",\"MA_TB\":\"\",\"LOAITB_ID\":\"\",\"TEN_TB\":\"\",\"TEN_TT\":\"\",\"DIACHI_TT\":\"\",\"DIACHI_CT\":\"\",\"SO_GT\":\"\",\"DIENTHOAI_LH\":\"\",\"MS_THUE\":\"\",\"TRANGTHAI_ID\":\"\",\"DIADIEMTT_ID\":\"\",\"QUOCTICH\":\"\",\"DIACHI_CT1\":\"\",\"TEN_KH\":\"\",\"LOAIKH_ID\":\"\",\"MIENCUOC\":\"\",\"PHUONG_ID\":\"\",\"PHO_ID\":\"\",\"QUAN_ID\":\"\",\"LOAIGT_ID\":\"\",\"SODAIDIEN\":\"\",\"SO_NHA\":\"\",\"NGAYCAP_GT\":\"\",\"PHAI\":\"\",\"MA_CQ\":\"\",\"MA_BC\":\"\",\"KHLON_ID\":\"\",\"NGAYSINH\":null,\"MA_T\":\"\",\"DONVIQL_ID\":\"\",\"NGANHNGHE_ID\":\"\",\"UUTIEN_ID\":\"\",\"DANGKY_DB\":\"\",\"DANGKY_TV\":\"\",\"QUANTT_ID\":\"\",\"PHUONGTT_ID\":\"\",\"PHOTT_ID\":\"\",\"SOTT_NHA\":\"\",\"MA_NV\":\"\",\"TTNO\":\"\"}");
                else
                    context.Response.Write("\"Balance\":null,\"BalanceKM\":null,\"BalanceKM1\":null,\"BalanceKM2\":null,\"BalanceData\":null,\"HSD\":null,\"SO_MSIN\":\"2121592279\",\"TRANG_THAI\":\"MO\",\"TEN_LOAI\":\"Post\",\"NGAY_KH\":\"18/12/2013\",\"SO_PIN\":\"1234\",\"SO_PUK\":\"71137215\",\"PIN2\":\"1234\",\"PUK2\":\"28043925\",\"Input_STB\":null,\"SO_MAY\":null,\"Ma_KH\":null,\"Ngay_Sinh\":null,\"Ten_TT\":null,\"Dia_Chi_TT\":null,\"GOI_DI\":\"A\",\"GOI_DEN\":\"A\",\"MA_TINH\":\"HNI\",\"MA_KH\":\"HNIDD00610872\",\"MA_TB\":\"20\",\"LOAITB_ID\":\"20\",\"TEN_TB\":\"Lê Xuân Long\",\"TEN_TT\":\"Lê Xuân Long\",\"DIACHI_TT\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"DIACHI_CT\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"SO_GT\":\"012866239\",\"DIENTHOAI_LH\":\"84916988386\",\"MS_THUE\":\"\",\"TRANGTHAI_ID\":\"1\",\"DIADIEMTT_ID\":\"0\",\"QUOCTICH\":\"Viet Nam\",\"DIACHI_CT1\":\"Đội 6 - Thôn Bầu - Kim Chung - Đông Anh - Hà Nội\",\"TEN_KH\":\"Lê Xuân Long\",\"LOAIKH_ID\":\"Nhóm khách hàng cá nhân, hộ gia đình\",\"MIENCUOC\":\"0\",\"PHUONG_ID\":\"23716\",\"PHO_ID\":\"307566\",\"QUAN_ID\":\"120\",\"LOAIGT_ID\":\"1\",\"SODAIDIEN\":\"84916988386\",\"SO_NHA\":\"176 Kim Giang\",\"NGAYCAP_GT\":\"29/08/2012\",\"PHAI\":\"Nữ\",\"MA_CQ\":\"567136\",\"MA_BC\":\"DA\",\"KHLON_ID\":\"1\",\"NGAYSINH\":null,\"MA_T\":\"PDA601-99\",\"DONVIQL_ID\":\"5\",\"NGANHNGHE_ID\":\"1\",\"UUTIEN_ID\":\"0\",\"DANGKY_DB\":\"1\",\"DANGKY_TV\":\"1\",\"QUANTT_ID\":\"120\",\"PHUONGTT_ID\":\"23716\",\"PHOTT_ID\":\"307566\",\"SOTT_NHA\":\"Đội 6 - Thôn Bầu\",\"MA_NV\":\"PDA601\",\"TTNO\":\"0 VND\"}");
            }
            else
            {
                try
                {
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = "";

                    var result = obj.GetInfo(requestParam);
                    if (result != null)
                    {
                        if (result.TEN_LOAI == "Post" || result.TEN_LOAI.ToLower().Contains("itouch") || result.TEN_LOAI.ToLower().Contains("iplus2") || result.TEN_LOAI.ToLower().Contains("ezpost")) // Thue bao tra truoc
                        {
                            var check = ProvinceImpl.ListProvince.Where(t => t.AbbRev == result.MA_TINH && t.LevelNbr == 1);
                            if (check.Any())
                            {
                                result.MA_TINH = check.Single().Id.ToString();
                            }
                        }
                        try
                        {
                            string whereSolr = string.Format("SoThueBao:{0} AND NgayTiepNhanSort:{1}", context.Request.Form["tb"], DateTime.Now.ToString("yyyyMMdd"));
                            var resultSolr = ServiceFactory.GetInstanceKhieuNai().CountKhieuNaiInSolr(whereSolr);
                            result.MessageWarningKhieuNai = "";
                            if (resultSolr > 0)
                            {
                                result.MessageWarningKhieuNai = resultSolr.ToString();
                            }
                        }
                        catch { }
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                    }
                    else
                        context.Response.Write("");
                    return;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("nullpointerexception"))
                        context.Response.Write(OutputJSONToAJax.ToJSON(-1, ex.Message, string.Empty, "warning"));
                    else
                        context.Response.Write(OutputJSONToAJax.ToJSON(-1, ex.Message));
                    return;
                }
                context.Response.Write(OutputJSONToAJax.ToJSON(-1, "Không lấy được dữ liệu từ TTTC."));
            }
        }


        private void GetServiceDaDung(HttpContext context)
        {
            try
            {
                AdminInfo userInfo = LoginAdmin.AdminLogin();
                if (userInfo == null)
                {
                    context.Response.Write("{\"ErrorId\":1, \"Content\":\"\", \"Message\":\"" + Constant.MESSAGE_HET_PHIEN_LAM_VIEC + "\"}");
                    return;
                }

                if (Config.IsLocal)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table class='tbl_style' id='tblDichVuDaDung' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>");
                    sb.Append("<tr>");
                    sb.Append("<th align='center' scope='col'>Mã dịch vụ</th>");
                    sb.Append("<th align='center' scope='col'>Tên dịch vụ</th>");
                    sb.Append("</tr>");

                    sb.Append("<tr><td align=\"left\" style=\"padding-left:5px;\">UMB</td><td align=\"left\" style=\"padding-left:5px;\">Info 360</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CF</td><td align=\"left\" style=\"padding-left:5px;\">CF (Call forwarding)</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CH</td><td align=\"left\" style=\"padding-left:5px;\">Call hold</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CLIP</td><td align=\"left\" style=\"padding-left:5px;\">Clip</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CW</td><td align=\"left\" style=\"padding-left:5px;\">Call waitting</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">NR</td><td align=\"left\" style=\"padding-left:5px;\">National Roaming</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">SMO</td><td align=\"left\" style=\"padding-left:5px;\">Gui tin nhan</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CBR</td><td align=\"left\" style=\"padding-left:5px;\">Chan cuoc goi</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">SMT</td><td align=\"left\" style=\"padding-left:5px;\">Nhan tin nhan</td></tr><tr><td align=\"left\" style=\"padding-left:5px;\">CSM</td><td align=\"left\" style=\"padding-left:5px;\">Content Short Message (CCG)</td></tr>");
                    sb.Append("</table>");

                    OutputJSONToAJax objOutput = new OutputJSONToAJax();
                    objOutput.ErrorId = 0;
                    objOutput.Content = sb.ToString();
                    objOutput.Message = string.Empty;

                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objOutput));
                }
                else
                {
                    string tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    tb = "84" + tb;

                    //var Impl = new SubinfoImpl();
                    //var list0 = Impl.getBasicService(tb, "Outbounder", "", "");
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = "";

                    ServiceVNP.BasicServiceFromSubinfo list0 = obj.getBasicService(requestParam);

                    if (list0 == null)
                    {
                        context.Response.Write("{\"ErrorId\":0, \"Content\":\"\", \"Message\":\"\"}");
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table class='tbl_style' id='tblDichVuDaDung' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>");
                        sb.Append("<tr>");
                        sb.Append("<th align='center' scope='col'>Mã dịch vụ</th>");
                        sb.Append("<th align='center' scope='col'>Tên dịch vụ</th>");
                        sb.Append("</tr>");
                        if (list0.ErrorID == "0")
                        {
                            foreach (var item in list0.ListService)
                            {
                                sb.Append("<tr>");
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MA_DV);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TEN_DV);
                                sb.Append("</tr>");
                            }
                        }
                        sb.Append("</table>");
                        context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"ErrorId\":1, \"Content\":\"\", \"Message\":\"" + ex.Message + "\"}");
                Utility.LogEvent(ex);
            }
        }

        private void ExportThongTinKH(HttpContext context)
        {

        }

        #region Lịch Sử khiếu nại
        private void LichSuKN(HttpContext context)
        {
            try
            {
                string tb = context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"];
                if (string.IsNullOrEmpty(tb))
                {
                    context.Response.Write("");
                    return;
                }

                else // Kiểm tra sự hợp lê của số thuê bao
                {
                    long soThueBao = 0;
                    if (!long.TryParse(context.Request.Form["SoThueBao"], out soThueBao))
                    {
                        context.Response.Write(string.Empty);
                        return;
                    }
                }

                DateKhieuNai t = null;

                int pIndex = Convert.ToInt32(context.Request.Form["page"]); ;
                int leng = Convert.ToInt32(context.Request.Form["rp"]); ;


                // Nếu không có điều kiện lọc
                string qType = context.Request.Form["qtype"];
                string valueFilter = context.Request.Form["query"];
                string ViewAll = context.Request.Form["ViewAll"];
                string SortName = context.Request.Form["sortname"];
                string SortOrder = context.Request.Form["sortorder"];
                int totalRecord = 0;

                if (ViewAll.Equals("1"))
                {
                    t = GetKhieuNaiFromSolr(tb, pIndex, leng, qType, valueFilter, SortName, SortOrder);
                    t.page = pIndex;
                }
                else if (ViewAll.Equals("0"))
                {
                    //t = new DateKhieuNai();
                    int start = (pIndex - 1) * leng + 1;
                    //leng = pIndex * leng;

                    List<KhieuNaiInfo> dic;

                    string whereClause = "SoThueBao = " + tb;
                    string sortClause = SortName + " " + SortOrder;

                    if (!string.IsNullOrEmpty(qType))
                    {
                        if (!string.IsNullOrEmpty(valueFilter))
                        {
                            switch (qType)
                            {
                                case "MaKhieuNai":
                                    whereClause += " AND Id=" + valueFilter;
                                    break;
                                case "NgayTiepNhanSort":
                                    try
                                    {
                                        DateTime dTemp = Convert.ToDateTime(valueFilter, new CultureInfo("vi-VN"));
                                        whereClause += " AND NgayTiepNhanSort=" + dTemp.ToString("yyyyMMdd");
                                    }
                                    catch { }
                                    break;
                                default:
                                    whereClause += " AND " + qType + " like N'%" + valueFilter + "%'";
                                    break;
                            }
                        }
                    }

                    dic = ServiceFactory.GetInstanceKhieuNai().GetPaged(string.Empty, whereClause, sortClause, pIndex, leng, ref totalRecord);

                    if (dic != null && dic.Count == 0)
                    {
                        t = GetKhieuNaiFromSolr(tb, pIndex, leng, qType, valueFilter, SortName, SortOrder);
                        t.page = pIndex;
                    }
                    else
                    {
                        t = new DateKhieuNai();
                        t.page = pIndex;

                        List<DataItem> lstRow = new List<DataItem>();
                        foreach (KhieuNaiInfo item in dic)
                        {
                            DataItem i = new DataItem();
                            i.DiaChi_CCBS = item.DiaChi_CCBS;
                            i.DiaChiLienHe = item.DiaChiLienHe;
                            i.DiaDiemXayRa = item.DiaDiemXayRa;
                            i.DoUuTien = BindDoUuTien(item.DoUuTien);
                            i.GhiChu = item.GhiChu;
                            i.HoTenLienHe = item.HoTenLienHe;
                            i.HTTiepNhan = BindHTTiepNhan(item.HTTiepNhan);
                            i.IsKNGiamTru = BindBooleanToCheckbox("chkKNGiamTru_" + item.Id, item.IsKNGiamTru, true);
                            i.IsLuuKhieuNai = BindBooleanToCheckbox("chkKNLuuSo_" + item.Id, item.IsLuuKhieuNai, true);
                            i.KNHangLoat = BindBooleanToCheckbox("chkKNHangLoat_" + item.Id, item.KNHangLoat, true);
                            i.LDate = item.LDate.ToString("dd/MM/yyyy HH:mm");
                            i.LinhVucChung = item.LinhVucChung;
                            i.LinhVucCon = item.LinhVucCon;
                            i.LoaiKhieuNai = item.LoaiKhieuNai;
                            i.MaKhieuNai = BindMaKN(item.Id);
                            i.MaQuan = item.MaQuan;
                            i.MaTinh = item.MaTinh;
                            if (item.TrangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
                            {
                                i.CallCount = 1;
                                i.NgayDongKNSort = item.NgayDongKN.ToString("dd/MM/yyyy HH:mm");
                            }
                            else
                            {
                                i.NgayDongKNSort = string.Empty;
                                i.CallCount = item.CallCount;
                            }
                            i.NgayQuaHanSort = item.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");
                            i.NgayTiepNhanSort = item.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm");
                            i.NguoiTiepNhan = item.NguoiTiepNhan;
                            i.NguoiXuLy = item.NguoiXuLy;
                            i.NoiDungPA = item.NoiDungPA;
                            i.NoiDungXuLyDongKN = item.NoiDungXuLyDongKN;
                            i.PhongBanTiepNhan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanTiepNhanId);
                            i.PhongBanXuLy = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                            i.SDTLienHe = item.SDTLienHe;
                            i.ThoiGianXayRa = item.ThoiGianXayRa;
                            i.TrangThai = BindTinhTrangXuLy(item.TrangThai, item.IsPhanHoi, item.NgayQuaHanPhongBanXuLy);
                            lstRow.Add(i);
                        }
                        t.rows = lstRow;
                        t.total = totalRecord;
                    }
                }
                else if (ViewAll.Equals("2"))
                {
                    t = new DateKhieuNai();
                    t.page = pIndex;

                    ServiceSyncVNPT_HNI.VINAPHONE objVNPT = new ServiceSyncVNPT_HNI.VINAPHONE();
                    ServiceSyncVNPT_HNI.XACTHUC xacthuc = new ServiceSyncVNPT_HNI.XACTHUC();
                    xacthuc.Username = "gqkn_vnp";
                    xacthuc.Password = "gqkn_vnp";
                    objVNPT.XACTHUCValue = xacthuc;
                    int MaLoi = 0;
                    string MoTa = string.Empty;
                    DataSet ds = objVNPT.vnpTraCuuKhieuNai(tb, ref MaLoi, ref MoTa, "HNI");
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        List<DataItem> lstRow = new List<DataItem>();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            DataItem i = new DataItem();
                            i.MaKhieuNai = ConvertUtility.ToString(item["ID_BAOKN"]);
                            i.TrangThai = ConvertUtility.ToString(item["TEN_TRANGTHAI"]);
                            i.NgayTiepNhanSort = ConvertUtility.ToString(item["NGAY_NHAN"]);
                            i.NoiDungPA = ConvertUtility.ToString(item["ND_KHIEUNAI"]);
                            i.HoTenLienHe = ConvertUtility.ToString(item["TENDANGKY_LHE"]);
                            i.SDTLienHe = ConvertUtility.ToString(item["SMDANGKY_LHE"]);
                            i.HTTiepNhan = ConvertUtility.ToString(item["TEN_HINHTHUCKN"]);
                            i.NguoiTiepNhan = ConvertUtility.ToString(item["NGUOI_NHAN"]);
                            i.NguoiXuLy = ConvertUtility.ToString(item["NGUOI_GQ"]);
                            lstRow.Add(i);
                        }
                        t.rows = lstRow;
                        t.total = lstRow.Count;
                    }
                }
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(t));
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                context.Response.Write(string.Empty);
            }
        }

        private DateKhieuNai GetKhieuNaiFromSolr(string tb, int pIndex, int leng, string qType, string valueFilter, string SortName, string SortOrder)
        {
            DateKhieuNai t = new DateKhieuNai();
            List<KhieuNaiSolrInfo> dic;
            int totalRecord = 0;

            dic = ServiceFactory.GetInstanceKhieuNai().LichSuKhieuNai(tb, pIndex, leng, qType, valueFilter, SortName, SortOrder, ref totalRecord);
            List<DataItem> lstRow = new List<DataItem>();
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    DataItem i = new DataItem();
                    //i.so
                    i.DiaChi_CCBS = item.DiaChi_CCBS;
                    i.DiaChiLienHe = item.DiaChiLienHe;
                    i.DiaDiemXayRa = item.DiaDiemXayRa;
                    i.DoUuTien = BindDoUuTien(item.DoUuTien);
                    i.GhiChu = item.GhiChu;
                    i.HoTenLienHe = item.HoTenLienHe;
                    i.HTTiepNhan = BindHTTiepNhan(item.HTTiepNhan);
                    i.IsKNGiamTru = BindBooleanToCheckbox("chkKNGiamTru_" + item.Id, item.IsKNGiamTru, true);
                    i.IsLuuKhieuNai = BindBooleanToCheckbox("chkKNLuuSo_" + item.Id, item.IsLuuKhieuNai, true);
                    i.KNHangLoat = BindBooleanToCheckbox("chkKNHangLoat_" + item.Id, item.KNHangLoat, true);
                    i.LDate = item.LDate.ToString("dd/MM/yyyy HH:mm");
                    i.LinhVucChung = item.LinhVucChung;
                    i.LinhVucCon = item.LinhVucCon;
                    i.LoaiKhieuNai = item.LoaiKhieuNai;
                    i.MaKhieuNai = BindMaKN(item.Id, item.ArchiveId);
                    i.MaQuan = item.MaQuan;
                    i.MaTinh = item.MaTinh;
                    if (item.TrangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
                        i.NgayDongKNSort = item.NgayDongKN.ToString("dd/MM/yyyy HH:mm");
                    i.NgayQuaHanSort = item.NgayQuaHan.ToString("dd/MM/yyyy HH:mm");
                    i.NgayTiepNhanSort = item.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm");
                    i.NguoiTiepNhan = item.NguoiTiepNhan;
                    i.NguoiXuLy = item.NguoiXuLy;
                    i.NoiDungPA = item.NoiDungPA;
                    i.NoiDungXuLyDongKN = item.NoiDungXuLyDongKN;
                    i.PhongBanTiepNhan = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanTiepNhanId);
                    i.PhongBanXuLy = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                    i.SDTLienHe = item.SDTLienHe;
                    i.ThoiGianXayRa = item.ThoiGianXayRa;
                    i.TrangThai = BindTinhTrangXuLy(item.TrangThai, item.IsPhanHoi, item.NgayQuaHanPhongBanXuLy);
                    i.CallCount = 1;
                    lstRow.Add(i);
                }
            }
            t.rows = lstRow;
            t.total = totalRecord;

            return t;
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
                else if (NgayQuaHanPhongBan < DateTime.Now)
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

        protected string BindMaKN(object obj, int archiveId = 0)
        {
            return string.Format("<a href=\"javascript:ShowPoupChiTietKN('{0}','{1}');\" title=\"Hiển thị thông tin chi tiết khiếu nại\">{2}</a>", obj, archiveId, GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, obj, 10));
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

        private void LichSuNapThe(HttpContext context)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table class='tbl_style' id='tblLichSuNapThe' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>
                                                    <tr>
                                                        <th align='center' scope='col'>STT</th>
                                                        <th align='center' scope='col'>Mệnh giá</th>
                                                        <th align='center' scope='col'>Ngày nạp</th>
                                                        <th align='center' scope='col'>Phương thức</th>
                                                        <th align='center' scope='col'>TKC Trước nạp</th>
                                                        <th align='center' scope='col'>TKC Sau nạp</th>
                                                        <th align='center' scope='col'>TKKM Trước nạp</th>
                                                        <th align='center' scope='col'>TKKM Sau nạp</th>
                                                    </tr>
                                                ");
            if (Config.IsLocal)
            {
                sb.Append("<tr class='rowB'><td align='center'>1</td><td align='left' style='padding-left:5px;'>50000</td><td align='left' style='padding-left:5px;'>31/12/2012 17:52:05</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>28</td><td align='left' style='padding-left:5px;'>50028</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'></td></tr><tr class='rowA'><td align='center'>2</td><td align='left' style='padding-left:5px;'>50000</td><td align='left' style='padding-left:5px;'>27/12/2012 21:36:11</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>50000</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'></td></tr></table>");
            }
            else
            {
                try
                {
                    var userInfo = LoginAdmin.AdminLogin();

                    if (userInfo == null)
                    {
                        sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", Constant.MESSAGE_HET_PHIEN_LAM_VIEC);
                        return;
                    }

                    var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    tb = "84" + tb;

                    //var Impl = new SubinfoImpl();
                    //var list0 = Impl.getBasicService(tb, "Outbounder", "", "");

                    //var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    //tb = "84" + tb;
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = "";

                    var list0 = obj.getPPSCardHistoryByMsisdn(requestParam);
                    if (list0 == null)
                    {
                        sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", "Không có dữ liệu");
                    }
                    else
                    {
                        if (list0.ErrorID == "0")
                        {
                            int i = 1;
                            foreach (var item in list0.ListPPSCardHistoryByMsisdn)
                            {
                                if (i % 2 == 0)
                                    sb.Append("<tr class='rowA'>");
                                else
                                    sb.Append("<tr class='rowB'>");
                                sb.AppendFormat("<td align='center'>{0}</td>", i);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MENH_GIA);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NGAY_NAP);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.PHUONG_THUC_NAP);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TKC_TRUOC_KHI_NAP);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TKC_SAU_KHI_NAP);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TKKM_TRUOC_KHI_NAP);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TKKM_SAU_KHI_NAP);
                                sb.Append("</tr>");
                                i++;
                            }
                        }

                        //context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", ex.Message);
                    Utility.LogEvent(ex);
                }
                finally
                {
                    sb.Append("</table>");
                }
            }

            context.Response.Write(sb.ToString());
        }

        private void LichSuThueBao(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table class='tbl_style' id='tblLichSuThueBao' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>                                                    
                            <tr>
						        <th align='center' scope='col'>STT</th><th scope='col'>NGAY_THANG</th><th scope='col'>MA_DV</th><th scope='col'>THAO_TAC</th><th scope='col'>GHI_CHU</th><th scope='col'>NGUOI_DUNG</th><th scope='col'>SO_MSIN_CU</th><th scope='col'>SO_MSIN_MOI</th><th scope='col'>MA_TINH_CU</th><th scope='col'>MA_TINH_MOI</th>
					        </tr>                                                
                        ");
            if (Config.IsLocal)
            {
                sb.Append("<tr class='rowB'><td align='center'>1</td><td align='left' style='padding-left:5px;'>17/02/2014 02:02:50</td><td align='left' style='padding-left:5px;'>IC  </td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t hÕt h¹n gäi ®Õn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td></tr><tr class='rowA'><td align='center'>2</td><td align='left' style='padding-left:5px;'>17/02/2014 02:02:50</td><td align='left' style='padding-left:5px;'>SMT </td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>Cat/Mo SMT do cat/mo IC</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td></tr><tr class='rowB'><td align='center'>3</td><td align='left' style='padding-left:5px;'>07/02/2014 02:01:29</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td></tr><tr class='rowA'><td align='center'>4</td><td align='left' style='padding-left:5px;'>30/03/2013 10:13:38</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>5</td><td align='left' style='padding-left:5px;'>25/03/2013 13:24:22</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>6</td><td align='left' style='padding-left:5px;'>19/03/2013 13:45:57</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>7</td><td align='left' style='padding-left:5px;'>17/03/2013 19:38:00</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>8</td><td align='left' style='padding-left:5px;'>12/03/2013 22:50:27</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>9</td><td align='left' style='padding-left:5px;'>09/03/2013 23:10:10</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>10</td><td align='left' style='padding-left:5px;'>09/03/2013 23:10:09</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>11</td><td align='left' style='padding-left:5px;'>05/03/2013 12:24:50</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>12</td><td align='left' style='padding-left:5px;'>05/03/2013 12:24:47</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>13</td><td align='left' style='padding-left:5px;'>03/03/2013 22:30:56</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>14</td><td align='left' style='padding-left:5px;'>27/02/2013 20:41:23</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>15</td><td align='left' style='padding-left:5px;'>25/02/2013 19:22:18</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>16</td><td align='left' style='padding-left:5px;'>17/01/2013 17:44:43</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>0</td><td align='left' style='padding-left:5px;'>C¾t GPRS do hÕt tiÒn - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowB'><td align='center'>17</td><td align='left' style='padding-left:5px;'>14/01/2013 21:46:43</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr><tr class='rowA'><td align='center'>18</td><td align='left' style='padding-left:5px;'>13/01/2013 12:39:08</td><td align='left' style='padding-left:5px;'>GPRS</td><td align='left' style='padding-left:5px;'>1</td><td align='left' style='padding-left:5px;'>N¹p tµi kho¶n - SDP</td><td align='left' style='padding-left:5px;'>system</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>null</td><td align='left' style='padding-left:5px;'>CBG</td><td align='left' style='padding-left:5px;'>CBG</td></tr></table>");
            }
            else
            {
                try
                {
                    AdminInfo userInfo = LoginAdmin.AdminLogin();

                    if (userInfo == null)
                    {
                        sb.AppendFormat("<tr><td colspan='10'>{0}</td></tr>", Constant.MESSAGE_HET_PHIEN_LAM_VIEC);
                        return;
                    }

                    string tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    tb = "84" + tb;

                    //var Impl = new SubinfoImpl();
                    //var list0 = Impl.getBasicService(tb, "Outbounder", "", "");

                    //var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    //tb = "84" + tb;

                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = "Từ thông tin khách hàng";
                    requestParam.ViewAll = false;

                    ServiceVNP.SubHistoryFromSubinfo list0 = obj.getSubHistory(requestParam);
                    if (list0 == null)
                    {
                        sb.AppendFormat("<tr><td colspan='10'>{0}</td></tr>", "Không có dữ liệu");
                    }
                    else
                    {
                        if (list0.ErrorID == "0")
                        {
                            int i = 1;
                            foreach (var item in list0.ListHistory)
                            {
                                if (i % 2 == 0)
                                    sb.Append("<tr class='rowA'>");
                                else
                                    sb.Append("<tr class='rowB'>");
                                sb.AppendFormat("<td align='center'>{0}</td>", i);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NGAY_THANG);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MA_DV);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.THAO_TAC);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.GHI_CHU);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NGUOI_DUNG);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.SO_MSIN_CU);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.SO_MSIN_MOI);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MA_TINH_CU);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MA_TINH_MOI);
                                sb.Append("</tr>");
                                i++;
                            }
                        }

                        //context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("<tr><td colspan='10'>{0}</td></tr>", ex.Message);
                    Utility.LogEvent(ex);
                }
                finally
                {
                    sb.Append("</table>");
                }
            }

            context.Response.Write(sb.ToString());
        }

        private void LichSu3G(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table class='tbl_style' id='tblLichSu3G' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>
                                                    <tr>
                                                        <th align='center' scope='col'>STT</th>
                                                        <th scope='col'>MA_DV</th>
                                                        <th scope='col'>TEN_GOI</th>
                                                        <th scope='col'>NGAY_BAT_DAU</th>
                                                        <th scope='col'>NGAY_KET_THUC</th>
                                                        <th scope='col'>ACTIVE</th>
                                                        <th scope='col'>GIA_HAN</th>
                                                        <th scope='col'>LOAI_TB</th>
                                                    </tr>                                                                                                
                        ");
            if (Config.IsLocal)
            {
                sb.Append("<tr class='rowB'><td align='center'>1</td><td align='left' style='padding-left:5px;'>MI_MAX</td><td align='left' style='padding-left:5px;'>Mobile Internet MAX</td><td align='left' style='padding-left:5px;'>12/12/2013 18:08:23</td><td align='left' style='padding-left:5px;'>12/03/2014 21:11:34</td><td align='left' style='padding-left:5px;'>Y</td><td align='left' style='padding-left:5px;'>Y</td><td align='left' style='padding-left:5px;'>0</td></tr><tr class='rowA'><td align='center'>2</td><td align='left' style='padding-left:5px;'>MI_MAX</td><td align='left' style='padding-left:5px;'>Mobile Internet MAX</td><td align='left' style='padding-left:5px;'>21/06/2013 18:25:46</td><td align='left' style='padding-left:5px;'>27/09/2013 11:04:25</td><td align='left' style='padding-left:5px;'>N</td><td align='left' style='padding-left:5px;'>Y</td><td align='left' style='padding-left:5px;'>0</td></tr></table>");
            }
            else
            {
                try
                {
                    AdminInfo userInfo = LoginAdmin.AdminLogin();

                    if (userInfo == null)
                    {
                        sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", Constant.MESSAGE_HET_PHIEN_LAM_VIEC);
                        return;
                    }

                    var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    tb = "84" + tb;

                    //var Impl = new SubinfoImpl();
                    //var list0 = Impl.getBasicService(tb, "Outbounder", "", "");

                    //var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    //tb = "84" + tb;
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = string.Empty;
                    requestParam.ViewAll = false;

                    ServiceVNP.History3GFromSubinfo list0 = obj.get3GHistory(requestParam);
                    if (list0 == null)
                    {
                        sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", "Không có dữ liệu");
                    }
                    else
                    {
                        if (list0.ErrorID == "0")
                        {
                            int i = 1;
                            foreach (var item in list0.ListHistory3G)
                            {
                                if (i % 2 == 0)
                                    sb.Append("<tr class='rowA'>");
                                else
                                    sb.Append("<tr class='rowB'>");
                                sb.AppendFormat("<td align='center'>{0}</td>", i);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MA_DV);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.TEN_GOI);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NGAY_BAT_DAU);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NGAY_KET_THUC);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.ACTIVE);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.GIA_HAN);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.LOAI_TB);
                                sb.Append("</tr>");
                                i++;
                            }
                        }

                        //context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("<tr><td colspan='8'>{0}</td></tr>", ex.Message);
                    Utility.LogEvent(ex);
                }
                finally
                {
                    sb.Append("</table>");
                }
            }
            context.Response.Write(sb.ToString());
        }

        private void TraCuuSMS888(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<table class='tbl_style' id='tblTraCuuSMS888' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>
                                <tr>
                                    <th align='center' scope='col'>STT</th>
                                    <th scope='col'>DIA_CHI_NHAN</th>
                                    <th scope='col'>DIA_CHI_GUI</th>
                                    <th scope='col'>NOI_DUNG</th>
                                    <th scope='col'>THOI_GIAN</th>
                                </tr>                                                                                             
                        ");
            if (Config.IsLocal)
            {
                sb.Append("<tr class='rowB'><td align='center'>1</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>MAX</td><td align='left' style='padding-left:5px;'>14/02/2014 09:43:13</td></tr><tr class='rowA'><td align='center'>2</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>DK MAX</td><td align='left' style='padding-left:5px;'>12/12/2013 18:08:14</td></tr><tr class='rowB'><td align='center'>3</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>DK MAX</td><td align='left' style='padding-left:5px;'>12/12/2013 14:17:00</td></tr><tr class='rowA'><td align='center'>4</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>DK MAX70</td><td align='left' style='padding-left:5px;'>12/12/2013 14:15:48</td></tr><tr class='rowB'><td align='center'>5</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>Max off</td><td align='left' style='padding-left:5px;'>05/10/2013 18:52:38</td></tr><tr class='rowA'><td align='center'>6</td><td align='left' style='padding-left:5px;'>888</td><td align='left' style='padding-left:5px;'>84948451755</td><td align='left' style='padding-left:5px;'>Max50 off</td><td align='left' style='padding-left:5px;'>05/10/2013 18:52:22</td></tr></table>");
            }
            else
            {
                try
                {
                    var userInfo = LoginAdmin.AdminLogin();

                    if (userInfo == null)
                    {
                        sb.AppendFormat("<tr><td colspan='5'>{0}</td></tr>", Constant.MESSAGE_HET_PHIEN_LAM_VIEC);
                        return;
                    }

                    var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    tb = "84" + tb;

                    //var Impl = new SubinfoImpl();
                    //var list0 = Impl.getBasicService(tb, "Outbounder", "", "");
                    //var tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];
                    //tb = "84" + tb;
                    ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                    ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                    requestParam.SoThueBao = tb;
                    requestParam.Username = userInfo.Username;
                    requestParam.KhuVucId = userInfo.KhuVucId;
                    requestParam.Note = string.Empty;

                    ServiceVNP.DeliverSMHistoryFromSubinfo list0 = obj.getDeliverSMHistory(requestParam);
                    if (list0 == null)
                    {
                        sb.AppendFormat("<tr><td colspan='5'>{0}</td></tr>", "Không có dữ liệu");
                    }
                    else
                    {
                        if (list0.ErrorID == "0")
                        {
                            int i = 1;
                            foreach (var item in list0.ListDeliverSMHistory)
                            {
                                if (i % 2 == 0)
                                    sb.Append("<tr class='rowA'>");
                                else
                                    sb.Append("<tr class='rowB'>");
                                sb.AppendFormat("<td align='center'>{0}</td>", i);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.DIA_CHI_NHAN);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.DIA_CHI_GUI);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.NOI_DUNG);
                                sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.THOI_GIAN);
                                sb.Append("</tr>");
                                i++;
                            }
                        }

                        //context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("<tr><td colspan='5'>{0}</td></tr>", ex.Message);
                    Utility.LogEvent(ex);
                }
                finally
                {
                    sb.Append("</table>");
                }
            }
            context.Response.Write(sb.ToString());
        }

        private void LichSuBuTien(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                @"<table class='tbl_style' id='tblLichSuBuTien' cellspacing='0' rules='all' border='1' style='border-collapse: collapse;'>
                                <tr>
                                    <th align='center' scope='col'>STT</th>
                                    <th scope='col'>Mã KN</th>
                                    <th scope='col'>Người TH</th>
                                    <th scope='col'>Số tiền</th>
                                    <th scope='col'>Dịch vụ</th>
                                    <th scope='col'>Thời gian</th>
                                    <th scope='col'>Số công văn</th>
                                </tr>                                                                                             
                        ");
            if (false)
            {
                sb.Append("<tr class='rowB'>" +
                          "<td align='center'>1</td>" +
                          "<td align='left' style='padding-left:5px;'>user1</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>MAX</td>" +
                          "<td align='left' style='padding-left:5px;'>14/02/2014 09:43:13</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "<tr class='rowA'>" +
                          "<td align='center'>2</td>" +
                          "<td align='left' style='padding-left:5px;'>user2</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>DK MAX</td>" +
                          "<td align='left' style='padding-left:5px;'>12/12/2013 18:08:14</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "<tr class='rowB'>" +
                          "<td align='center'>3</td>" +
                          "<td align='left' style='padding-left:5px;'>user3</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>DK MAX</td>" +
                          "<td align='left' style='padding-left:5px;'>12/12/2013 14:17:00</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "<tr class='rowA'>" +
                          "<td align='center'>4</td>" +
                          "<td align='left' style='padding-left:5px;'>user4</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>DK MAX70</td>" +
                          "<td align='left' style='padding-left:5px;'>12/12/2013 14:15:48</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "<tr class='rowB'>" +
                          "<td align='center'>5</td>" +
                          "<td align='left' style='padding-left:5px;'>user5</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>Max off</td>" +
                          "<td align='left' style='padding-left:5px;'>05/10/2013 18:52:38</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "<tr class='rowA'>" +
                          "<td align='center'>6</td>" +
                          "<td align='left' style='padding-left:5px;'>user6</td>" +
                          "<td align='left' style='padding-left:5px;'>22273.00</td>" +
                          "<td align='left' style='padding-left:5px;'>Max50 off</td>" +
                          "<td align='left' style='padding-left:5px;'>05/10/2013 18:52:22</td>" +
                          "<td align='left' style='padding-left:5px;'>công văn...</td>" +
                          "</tr>" +
                          "</table>");
            }
            else
            {
                try
                {
                    AdminInfo userInfo = LoginAdmin.AdminLogin();

                    if (userInfo == null)
                    {
                        sb.AppendFormat("<tr><td colspan='5'>{0}</td></tr>", Constant.MESSAGE_HET_PHIEN_LAM_VIEC);
                        return;
                    }

                    string tb = context.Request.Form["tb"] ?? context.Request.QueryString["tb"];

                    string selectClause =
                        "distinct  b.id KhieuNaiId, a.Id, a.LUser, a.SoTien,a.SoTien_Edit,a.DichVuCPId,CASE WHEN a.DichVuCPId=0 THEN '' ELSE a.MaDichVu END MaDichVu,a.DauSo,a.CDate, c.TenFile,c.GhiChu as GhiChu2";
                    string joinClause =
                        "INNER JOIN dbo.KhieuNai b ON a.KhieuNaiId=b.Id LEFT JOIN dbo.KhieuNai_FileDinhKem c ON a.KhieuNaiId = c.KhieuNaiId";
                    string whereClause = " b.SoThueBao=" + tb;
                    string orderbyClause = "";
                    List<KhieuNai_SoTienInfo> lichSuBuTien = ServiceFactory.GetInstanceKhieuNai_SoTien()
                        .GetListDynamicJoin(selectClause, joinClause, whereClause, orderbyClause);

                    if (lichSuBuTien == null)
                    {
                        sb.AppendFormat("<tr><td colspan='5'>{0}</td></tr>", "Không có dữ liệu");
                    }
                    else
                    {
                        int i = 1;
                        foreach (var item in lichSuBuTien)
                        {
                            if (i % 2 == 0)
                                sb.Append("<tr class='rowA'>");
                            else
                                sb.Append("<tr class='rowB'>");
                            sb.AppendFormat("<td align='center'>{0}</td>", i);
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", BindMaKN(item.KhieuNaiId));
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.LUser);
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>",
                                String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c}", item.SoTien_Edit > 0 ? item.SoTien_Edit : item.SoTien).Replace(",00", ""));
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.MaDichVu);
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.CDate);
                            sb.AppendFormat("<td align='left' style='padding-left:5px;'>{0}</td>", item.GhiChu);
                            sb.Append("</tr>");
                            i++;
                        }
                        //context.Response.Write("{\"ErrorId\":0, \"Content\":\"" + sb.ToString() + "\", \"Message\":\"\"}");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("<tr><td colspan='6'>{0}</td></tr>", ex.Message);
                    Utility.LogEvent(ex);
                }
                finally
                {
                    sb.Append("</table>");
                }
            }
            context.Response.Write(sb.ToString());
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