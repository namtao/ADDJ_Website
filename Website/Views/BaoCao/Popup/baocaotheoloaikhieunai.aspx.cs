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
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 25/09/2013
    /// Todo : Báo cáo theo loại khiếu nại
    /// </summary>
    public partial class baocaotheoloaikhieunai : AppCode.PageBase
    {
        protected string sNoiDungCongViec = "";
        protected string sNoiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan = "";
        protected string sNoiDungSoLieuHoTroVNPTTT = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int khuVucId = ConvertUtility.ToInt32(Request.QueryString["KhuVucID"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["donViId"]);
                    
                    //string loaiKhieuNai = Request.QueryString["loaiKhieuNai"];
                    //string linhVucChung = Request.QueryString["linhVucChung"];
                    //string linhVucCon = Request.QueryString["linhVucCon"];
                    string loaiKhieuNai = Session["LoaiKhieuNai"] != null ? Session["LoaiKhieuNai"].ToString() : string.Empty;
                    string linhVucChung = Session["LinhVucChung"] != null ? Session["LinhVucChung"].ToString() : string.Empty;
                    string linhVucCon = Session["LinhVucCon"] != null ? Session["LinhVucCon"].ToString() : string.Empty;
                    var loaibc = Request.QueryString["loaibc"];
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

                    int week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fromDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    lblReportMonth.Text = string.Format("Tuần {0} - Từ ngày {1} đến ngày {2}", week, fromDate.ToString("dd/MM"), toDate.ToString("dd/MM/yyyy"));
                    string sWeek = string.Format("{0}/{1}", week, fromDate.Year);
                    //int phongBanXuLyId = LoginAdmin.AdminLogin().PhongBanId;

                    string where = string.Empty;

                    string sHeader = string.Empty;
                    if (khuVucId == 7)
                    {
                        where = "Hà Nội";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KVI
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVI</b>";
                    }
                    else if (khuVucId == 14)
                    {
                        where = "TP Hồ Chí Minh";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV II
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II</b>";

                    }
                    else if (khuVucId == 19)
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


                    lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);
                    lblWho.Text = LoginAdmin.AdminLogin().FullName;

                    lblKhuVuc.Text = sHeader;
                    //lblPhongBan.Text = Request.QueryString["donVi"] != null && !Request.QueryString["donVi"].ToLower().Contains("chọn phòng ban") ? Request.QueryString["donVi"].Trim().ToUpper() : string.Empty;

                    BuildBaoCao buildBaoCao = new BuildBaoCao();
                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    if(isPhongCSKH)
                    {
                        sNoiDungCongViec = buildBaoCao.BaoCaoTheoLoaiKhieuNaiToGQKN(phongBanXuLyId, fromDate, toDate, listLoaiKhieuNai, listLinhVucChung, listLinhVucCon, out sNoiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan, isExportExcel);
                    }
                    else
                    {
                        sNoiDungCongViec = buildBaoCao.BaoCaoTheoLoaiKhieuNai(sWeek, khuVucId, phongBanXuLyId, fromDate, toDate,
                                                                               listLoaiKhieuNai, listLinhVucChung, listLinhVucCon, out sNoiDungPhanLoaiKhieuNaiPPSDaGiaiQuyetTrongTuan, out sNoiDungSoLieuHoTroVNPTTT, isExportExcel);
                    }
                                        
                    if (isExportExcel)
                    {
                        export2excel("BaoCaoTheoLoaiKhieuNai_" + fromDate + "_" + toDate);
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