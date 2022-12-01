using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;

using Website.AppCode.Controller;
using AIVietNam.Core;
using AIVietNam.Admin;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotogqkn : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = "";       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {                    
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);

                    //string loaiKhieuNai = Request.QueryString["loaiKhieuNai"];
                    //string linhVucChung = Request.QueryString["linhVucChung"];
                    //string linhVucCon = Request.QueryString["linhVucCon"];
                    string loaiKhieuNai = Session["LoaiKhieuNai"] != null ? Session["LoaiKhieuNai"].ToString() : string.Empty;
                    string linhVucChung = Session["LinhVucChung"] != null ? Session["LinhVucChung"].ToString() : string.Empty;
                    string linhVucCon = Session["LinhVucCon"] != null ? Session["LinhVucCon"].ToString() : string.Empty;
                    string reportType = Request.QueryString["reportType"];
                    string loaibc = Request.QueryString["loaibc"];
                    bool isPhongCSKH = ConvertUtility.ToBoolean(Request.QueryString["isPhongCSKH"]);

                    List<string> listLoaiKhieuNai = new List<string>();
                    List<string> listLinhVucChung = new List<string>();
                    List<string> listLinhVucCon = new List<string>();

                    if (loaiKhieuNai.Length > 0)
                    {
                        string[] arrLoaiKhieuNai = loaiKhieuNai.Split(',');
                        listLoaiKhieuNai = arrLoaiKhieuNai.ToList<string>();
                    }

                    if (linhVucChung.Length > 0)
                    {
                        string[] arrLinhVucChung = linhVucChung.Split(',');
                        listLinhVucChung = arrLinhVucChung.ToList<string>();
                    }

                    if (linhVucCon.Length > 0)
                    {
                        string[] arrLinhVucCon = linhVucCon.Split(',');
                        listLinhVucCon = arrLinhVucCon.ToList<string>();
                    }
                    
                    lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM"), toDate.ToString("dd/MM/yyyy"));                    

                    string where = string.Empty;

                    string sHeader = string.Empty;
                    if (phongBanXuLyId == 53)
                    {
                        where = "Hà Nội";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KVI
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVI</b>";
                    }
                    else if (phongBanXuLyId == 62)
                    {
                        where = "TP Hồ Chí Minh";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV II
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II</b>";

                    }
                    else if (phongBanXuLyId == 67)
                    {
                        where = "Đà Nẵng";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV III
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV III</b>";
                    }
                    else
                    {
                        where = "Hà Nội";
                    }

                    lblKhuVuc.Text = sHeader;

                    BuildBaoCao buildBaoCao = new BuildBaoCao();

                    string sBaoCaoTongHop = string.Empty;
                    string sBaoCaoGiamTru = string.Empty;

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sBaoCaoTongHop = buildBaoCao.BaoCaoTheoLoaiKhieuNaiToGQKN(phongBanXuLyId, fromDate, toDate, listLoaiKhieuNai, listLinhVucChung, listLinhVucCon, out sBaoCaoGiamTru, isExportExcel);

                    if (reportType == "bc_VNP_BaoCaoTongHopToGQKN")
                    {
                        lblTitle.Text = "BÁO CÁO TỔNG HỢP TỔ GQKN";
                        sNoiDungBaoCao = sBaoCaoTongHop;
                    }
                    else if (reportType == "bc_VNP_BaoCaoGiamTruToGQKN")
                    {
                        lblTitle.Text = "BÁO CÁO GIẢM TRỪ TỔ GQKN";
                        sNoiDungBaoCao = sBaoCaoGiamTru;
                    }

                    if (isExportExcel)
                    {
                        export2excel("BaoCaoToGQKN_" + fromDate + "_" + toDate);
                    }

                    //Session.Remove("LoaiKhieuNai");
                    //Session.Remove("LinhVucChung");
                    //Session.Remove("LinhVucCon");
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