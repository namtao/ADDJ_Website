using AIVietNam.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class emptypage : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = Request.QueryString["page"] != null ? Request.QueryString["page"] : string.Empty;
                BuildBaoCao buildBaoCao = new BuildBaoCao();
                bool isExportExcel = false;
                string loaibc = Request.QueryString["loaibc"];
                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));

                switch (page)
                {
                    case "soluongkhieunaiphongbantiepnhan":
                        int phongBanXuLyTruocId = ConvertUtility.ToInt32(Request.QueryString["phongBanXuLyTruocId"]);
                        int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"]);

                        sNoiDungBaoCao = buildBaoCao.BaoCaoSoLuongKNCacPhongBanDangDaTiepNhan(phongBanXuLyTruocId, loaiKhieuNaiId, fromDate, toDate);
                        lblTenFile.Text = "";
                        break;

                    case "danhsachkhieunaichuadong":
                        int phongBanXuLyId_danhSachKhieuNaiChuaDong = ConvertUtility.ToInt32(Request.QueryString["phongBanXuLyId"]);
                        int loaiKhieuNaiId_danhSachKhieuNaiChuaDong = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"]);
                        sNoiDungBaoCao = buildBaoCao.DanhSachKhieuNaiChuaDongTheoPhongBanXuLy(phongBanXuLyId_danhSachKhieuNaiChuaDong,
                                            loaiKhieuNaiId_danhSachKhieuNaiChuaDong, DateTime.MaxValue, DateTime.MaxValue);
                        break;

                    case "bc_vnp_baocaotonghopgiamtru":
                        lblReportTitle.Text = "Báo cáo tổng hợp giảm trừ toàn mạng";
                        lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                        sNoiDungBaoCao = buildBaoCao.BaoCaoTongHopGiamTruToanMang(fromDate, toDate, isExportExcel);
                        break;

                    case "bc_common_baocaotondongnguoidungphongban":
                        int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["phongBanXuLyId"]);
                        string nguoiXuLy = Request.QueryString["nguoiXuLy"];
                        sNoiDungBaoCao = buildBaoCao.BaoCaoTonDongVaQuaHanNguoiDungPhongBan(phongBanXuLyId, nguoiXuLy, isExportExcel);
                        break;

                    case "bc_vnp_baocaotondongquahan":
                        lblReportTitle.Text = "Báo cáo tồn đọng quá hạn";
                        sNoiDungBaoCao = buildBaoCao.BaoCaoTyLeKNTonDongQuaHanPhongBan(fromDate, toDate);
                        break;

                    case "bc_vnp_baocaodvgtgttapdoan":
                        lblReportTitle.Text = "Báo cáo dịch vụ giá trị gia tăng";
                        sNoiDungBaoCao = buildBaoCao.BaoCaoKhieuNaiDichVuGTGTTapDoan(fromDate, toDate);
                        break;

                    // Dương Dv
                    case "bc_vnp_baocaodvgtgttapdoan_new":

                        string month = Request.QueryString["Month"];
                        string year = Request.QueryString["Year"];

                        bool isOk = false;
                        int intMonth = 0;
                        int intYear = 0;
                        if (int.TryParse(month, out intMonth))
                        {
                            if (intMonth > 0 && intMonth <= 12) isOk = true;
                        };
                        if (int.TryParse(year, out intYear))
                        {
                            if (intYear >= DateTime.Now.Year - 10 && intYear <= DateTime.Now.Year) isOk = true;
                            else isOk = false;
                        }
                        else isOk = false;

                        bool isIdsOK = false;
                        List<string> myIds = new List<string>();
                        string lsIds = Request.QueryString["Ids"]; // Danh sách đơn vị báo cáo
                        if (!string.IsNullOrEmpty(lsIds))
                        {
                            string[] lstIds = lsIds.Split(',');
                            if (lstIds.Length > 0)
                            {
                                foreach (string s in lstIds)
                                {
                                    int id = 0;
                                    if (!int.TryParse(s, out id)) break;
                                    myIds.Add(id.ToString());
                                }
                                isIdsOK = true;
                            }
                        }

                        if (isOk && isIdsOK)
                        {
                            lblReportTitle.Text = string.Format("Báo cáo dịch vụ giá trị gia tăng tháng {0}/{1}", month, year);
                            sNoiDungBaoCao = buildBaoCao.BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan(intMonth, intYear, myIds.ToArray());
                        }
                        else
                        {
                            btnExportExcel.Visible = false;
                            btnExportExcelTop.Visible = false;
                            sNoiDungBaoCao = "Dữ liệu ngày tháng không hợp lệ, vui lòng kiểm tra lại";
                            Helper.GhiLogs("LoiBaoCao", "Đường dẫn: {0}, Lỗi do ngày tháng, hoạc danh sách đơn vị báo cáo", Request.RawUrl);
                        }
                        break;
                }

                lblTenFile.Text = page;
                lblNoiDungBaoCao.Text = sNoiDungBaoCao;

                if (loaibc == "excel")
                {
                    btnExportExcelTop.Visible = false;
                    btnExportExcel.Visible = false;
                    export2excel(lblTenFile.Text + "_" + fromDate.ToString("ddMMyyyy") + "_" + toDate.ToString("ddMMyyyy"));
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
    }
}