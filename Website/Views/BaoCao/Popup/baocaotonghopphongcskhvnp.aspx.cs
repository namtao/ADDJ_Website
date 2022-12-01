using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using Website.AppCode;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghopphongcskhvnp : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            btnExportExcel.Click += BtnExportExcel_Click;
            btnExportExcel2.Click += BtnExportExcel_Click;
            if (!IsPostBack)
            {
                try
                {
                    lblFullName.Text = LoginAdmin.AdminLogin().FullName;

                    int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    string loaibc = Request.QueryString["loaibc"];

                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

                    bool isExportExcel = false;
                    if (loaibc == "excel") isExportExcel = true;

                    string sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiVNP(doiTacId, fromDate, toDate, isExportExcel);
                    liReport.Text = FillHeader(sNoiDungBaoCao);

                    if (isExportExcel) Export2Excel("BaoCaoTongHopKhieuNaiVNP_" + fromDate.ToString("ddMMyyyy") + "_" + toDate.ToString("ddMMyyyy"));

                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Helper.GhiLogs("Logs_Bc", ex);
                }
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
            DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));

            Export2Excel("BaoCaoTongHopKhieuNaiVNP_" + fromDate.ToString("ddMMyyyy") + "_" + toDate.ToString("ddMMyyyy"));
        }

        public void Export2Excel(string tenbc)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            //string pathCSS = Server.MapPath("~/CSS");
            //pathCSS += @"\BaoCao.css";
            //StreamReader reader = new StreamReader(pathCSS);
            ////reader.ReadToEnd();

            //Response.Write("<style>");
            //Response.Write(reader.ReadToEnd());
            //Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        private string FillHeader(string body)
        {
            string tblHeader = string.Empty;
            tblHeader = "<table class=\"tbl_style\" border=\"1\" style=\"border-collapse: collapse; \">";
            tblHeader += "<tr>";
            tblHeader += "<th rowspan =\"2\">STT</th>";
            tblHeader += "<th rowspan =\"2\">Khu vực</th>";
            tblHeader += "<th rowspan =\"2\">Đối tác cấp 1</th>";
            tblHeader += "<th rowspan =\"2\">Đối tác cấp 2</th>";
            tblHeader += "<th rowspan =\"2\">Đối tác cấp 3</th>";
            tblHeader += "<th rowspan =\"2\">";
            tblHeader += "SL tồn kỳ trước";
            tblHeader += "<br />";
            tblHeader += "[1]";
            tblHeader += "</th>";
            tblHeader += "<th colspan =\"2\">";
            tblHeader += "Số lượng tiếp nhận";
            tblHeader += "</th>";
            tblHeader += "<th colspan =\"5\">";
            tblHeader += "Số lượng đã xử lý";
            tblHeader += "</th>";
            tblHeader += "<th rowspan =\"2\">";
            tblHeader += "Số lượng tồn đọng";
            tblHeader += "<br />";
            tblHeader += "[4]";
            tblHeader += "</th>";
            tblHeader += "<th rowspan =\"2\">";
            tblHeader += "Số lượng tồn đọng quá hạn";
            tblHeader += "<br />";
            tblHeader += "[4.1]";
            tblHeader += "</th>";
            tblHeader += "</tr>";
            tblHeader += "<tr>";
            tblHeader += "<th>";
            tblHeader += "SL tiếp nhận<br />";
            tblHeader += "[2]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += "SL tạo mới<br />";
            tblHeader += "[2.1]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += "SL đã xử lý <br />";
            tblHeader += "[3] = [3.1] + [3.2] + [3.3]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += " SL chuyển xử lý <br />";
            tblHeader += "[3.1]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += "SL chuyển phản hồi <br />";
            tblHeader += "[3.2]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += " SL đã đóng<br />";
            tblHeader += "[3.3]";
            tblHeader += "</th>";
            tblHeader += "<th>";
            tblHeader += "Số lượng đã xử lý quá hạn<br />";
            tblHeader += " [3.4]";
            tblHeader += "</th>";
            tblHeader += "</tr>";

            // Đặt nội dung báo cáo vào đây
            tblHeader += body;
            tblHeader += "</ table> ";

            return tblHeader;
        }
    }
}