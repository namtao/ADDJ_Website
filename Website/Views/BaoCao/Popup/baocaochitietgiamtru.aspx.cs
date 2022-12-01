using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using AIVietNam.Core;
using AIVietNam.Admin;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaochitietgiamtru : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";
        protected string fullName = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            //var userLogin = LoginAdmin.AdminLogin();
            //fullName = userLogin.FullName;
            if (!IsPostBack)
            {                
                //lblFullName.Text = fullName;
                try
                {
                    var khuVucID = ConvertUtility.ToInt32(Request.QueryString["khuVucID"]);
                    var donViID = ConvertUtility.ToInt32(Request.QueryString["donViID"]);
                    var loaiKhieuNaiID = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiID"]);
                    var linhVucChungID = ConvertUtility.ToInt32(Request.QueryString["linhVucChungID"]);
                    var linhVucConID = ConvertUtility.ToInt32(Request.QueryString["linhVucConID"]);                    
                    var maDV = Request.QueryString["ma_dvu"];
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];

                    lbKhuVuc.Text = khuVucID != -1 ? Request.QueryString["khuVuc"].Trim() : string.Empty;
                    lbDonVi.Text = donViID != -1 ? Request.QueryString["donVi"] : string.Empty;
                    lbLoaiKhieuNai.Text = loaiKhieuNaiID != -1 ? Request.QueryString["loaiKhieuNai"] : string.Empty;
                    lbLinhVucChung.Text = linhVucChungID != -1 ? Request.QueryString["linhVucChung"] : string.Empty; ;
                    lbLinhVucCon.Text = linhVucConID != -1 ? Request.QueryString["linhVucCon"] : string.Empty;
                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    string where = string.Empty;
                    string sHeader = string.Empty;
                    if (khuVucID == 7)
                    {
                        where = "Hà Nội";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KVI
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVI</b>
                                    <br/>
                                    Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/KN-ĐKT VNP1";
                    }
                    else if (khuVucID == 14)
                    {
                        where = "TP Hồ Chí Minh";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV II
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II</b>
                                    <br/>
                                    Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/KN-ĐKT VNP2";
                        
                    }
                    else if (khuVucID == 19)
                    {
                        where = "Đà Nẵng";
                        sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV III
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV III</b>
                                    <br/>
                                    Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/KN-ĐKT VNP3";
                    }
                    else
                    {
                        where = "Hà Nội";
                    }

                    lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);
                    lblKhuVucHeader.Text = sHeader;

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = BuildBaoCao.BaoCaoChiTietGiamTruCuocDVTraTruoc(khuVucID, donViID, ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate), loaiKhieuNaiID, linhVucChungID, linhVucConID, isExportExcel);
                    if (isExportExcel)
                    {
                        export2excel("BaoCaoChiTietGiamTruTraTruoc_" + fromDate + "_" + toDate);
                    }
                }
                catch (Exception ex)
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