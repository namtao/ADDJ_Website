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
using AIVietNam.GQKN.Entity;

using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghopkhieunaitoanmangtheotuanthang : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;
            LoginAdmin.IsLoginAdmin();
            if (!IsPostBack)
            {
                try
                {
                    string reportType = Request.QueryString["reportType"];
                    string sFromDate = Request.QueryString["fromDate"];
                    string sToDate = Request.QueryString["toDate"];                    
                    var fromDate = Convert.ToDateTime(sFromDate, new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(sToDate, new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    int nguonKhieuNai = ConvertUtility.ToInt32(Request.QueryString["nguonKhieuNai"], -1);
                    
                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", sFromDate, sToDate);
                    lblFullName.Text = LoginAdmin.AdminLogin().FullName;

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    //Build nội dung báo cáo
                    if (reportType == "bc_VNP_KhieuNaiToanMangTheoTuan")
                    {
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiToanMangTheoTuan(fromDate, toDate, nguonKhieuNai);
                    }
                    else if (reportType == "bc_VNP_KhieuNaiToanMangTheoThang")
                    {
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopKhieuNaiToanMangTheoThang(toDate, nguonKhieuNai);
                    }

                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoKhieuNaiToanMangTheoTuanThangVNP_" + fromDate.ToString("ddMMyyyy") + "_" + toDate.ToString("ddMMyyyy"));
                    }
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }

        public void export2excel(string tenbc)
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
            Response.Write("<style>");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
   
    }
}