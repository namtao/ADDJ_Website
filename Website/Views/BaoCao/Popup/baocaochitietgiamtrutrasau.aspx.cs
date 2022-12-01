using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using System.Globalization;
using Website.AppCode.Controller;
using System.IO;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaochitietgiamtrutrasau : AppCode.PageBase
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
                //lblCurDate.Text = string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                //lblFullName.Text = fullName;
                try
                {
                    var parentDoiTacId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                    var doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    var donViId = ConvertUtility.ToInt32(Request.QueryString["donViId"]);
                    var loaiKhieuNaiID = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiID"]);
                    var linhVucChungID = ConvertUtility.ToInt32(Request.QueryString["linhVucChungID"]);
                    var linhVucConID = ConvertUtility.ToInt32(Request.QueryString["linhVucConID"]);                    
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];

                    string where = "Hà Nội";
                    //lbKhuVuc.Text = khuVucID != -1 ? Request.QueryString["khuVuc"].Trim() : string.Empty;
                    //lbDonVi.Text = donViID != -1 ? Request.QueryString["donVi"] : string.Empty;
                    string khuVuc = Request.QueryString["khuVuc"] != null ? Request.QueryString["khuVuc"].Trim() : string.Empty;
                    where = doiTacId != -1 && Request.QueryString["tenDoiTac"] != null ? Request.QueryString["tenDoiTac"].Trim() : Request.QueryString["khuVuc"].Trim();
                    lbLoaiKhieuNai.Text = loaiKhieuNaiID != -1 ? Request.QueryString["loaiKhieuNai"] : string.Empty;
                    lbLinhVucChung.Text = linhVucChungID != -1 ? Request.QueryString["linhVucChung"] : string.Empty; ;
                    lbLinhVucCon.Text = linhVucConID != -1 ? Request.QueryString["linhVucCon"] : string.Empty;
                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    

                    if(donViId != -1) // Tổ GQKN
                    {
                        string sHeader = string.Empty;
                        if (parentDoiTacId == 7)
                        {
                            where = "Hà Nội";
                            sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KVI
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVI</b>
                                    <br/>
                                    Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/KN-ĐKT VNP1";
                        }
                        else if (parentDoiTacId == 14)
                        {
                            where = "TP Hồ Chí Minh";
                            sHeader = @"CÔNG TY DỊCH VỤ VIỄN THÔNG KV II
                                    <br/>
                                    <b>TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II</b>
                                    <br/>
                                    Số:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;/KN-ĐKT VNP2";

                        }
                        else if (parentDoiTacId == 19)
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

                        lblKhuVucHeader.Text = sHeader;
                        lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);                    
                    }
                    else // VNPT TT
                    {                       
                        lblVNPTTT.Text = where.Contains("VNPT") ? where : string.Format("VNPT {0}", where);
                        lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);                    
                    }
                                        
                    rowToGQKN.Visible = donViId != -1;
                    rowVNPTTT.Visible = donViId == -1;

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = BuildBaoCao.BaoCaoChiTietGiamTruCuocDVTraSau(parentDoiTacId, doiTacId, donViId, ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate), loaiKhieuNaiID, linhVucChungID, linhVucConID, isExportExcel);
                    if (isExportExcel)
                    {
                        export2excel("BaoCaoChiTietGiamTruTraSau_" + fromDate + "_" + toDate);
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