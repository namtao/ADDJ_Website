using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;
using AIVietNam.Core;
using System.IO;
using System.Globalization;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaochitietpps : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int khuVucID = ConvertUtility.ToInt32(Request.QueryString["khuVucID"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int donViID = ConvertUtility.ToInt32(Request.QueryString["donViID"]);
                    int loaiKhieuNaiID = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiID"]);
                    int linhVucChungID = ConvertUtility.ToInt32(Request.QueryString["linhVucChungID"]);
                    int linhVucConID = ConvertUtility.ToInt32(Request.QueryString["linhVucConID"]);
                    int trangThai = ConvertUtility.ToInt32(Request.QueryString["trangThai"]);

                    var loaibc = Request.QueryString["loaibc"];

                    lbKhuVuc.Text = khuVucID != -1 ? Request.QueryString["khuVuc"].Trim() : string.Empty;
                    lbDonVi.Text = donViID != -1 ? Request.QueryString["donVi"] : string.Empty;
                    lbLoaiKhieuNai.Text = loaiKhieuNaiID != -1 ? Request.QueryString["loaiKhieuNai"] : string.Empty;
                    lbLinhVucChung.Text = linhVucChungID != -1 ? Request.QueryString["linhVucChung"] : string.Empty; ;
                    lbLinhVucCon.Text = linhVucConID != -1 ? Request.QueryString["linhVucCon"] : string.Empty;
                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    // Tạm thời fix các loại khiếu nại của Cước thuê bao trả trước
                    List<int> listLoaiKhieuNaiId = new List<int>();
                    listLoaiKhieuNaiId.Add(loaiKhieuNaiID);
                    listLoaiKhieuNaiId.Add(1413);
                    listLoaiKhieuNaiId.Add(1443);
                    listLoaiKhieuNaiId.Add(1453);
                    listLoaiKhieuNaiId.Add(1460);
                    listLoaiKhieuNaiId.Add(1467);
                    listLoaiKhieuNaiId.Add(1483);
                    listLoaiKhieuNaiId.Add(1534);
                    listLoaiKhieuNaiId.Add(1537);
                    listLoaiKhieuNaiId.Add(1608);

                    List<int> listLinhVucChungId = new List<int>();
                    List<int> listLinhVucConId = new List<int>();

                    //int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fromDate,CalendarWeekRule.FirstFourDayWeek,DayOfWeek.Monday);
                    lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM"), toDate.ToString("dd/MM"));

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = BuildBaoCao.BaoCaoChiTietPPS(khuVucID, donViID, ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate), 
                                                                    listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, trangThai, isExportExcel);


                    if (isExportExcel)
                    {
                        export2excel("BaoCaoChiTietPPS" + fromDate + "_" + toDate);
                    }
                }
                catch
                { 
                
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