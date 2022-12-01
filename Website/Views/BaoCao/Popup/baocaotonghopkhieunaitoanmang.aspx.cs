﻿using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghopkhieunaitoanmang : System.Web.UI.Page
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
                    string sFromDateBefore = Request.QueryString["fromDateBefore"];
                    string sToDateBefore = Request.QueryString["toDateBefore"];
                    string sFromDate = Request.QueryString["fromDate"];
                    string sToDate = Request.QueryString["toDate"];
                    var khuvucid = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    DateTime fromDateBefore = Convert.ToDateTime(sFromDateBefore, new CultureInfo("vi-VN"));
                    DateTime toDateBefore = Convert.ToDateTime(sToDateBefore, new CultureInfo("vi-VN"));
                    var fromDate = Convert.ToDateTime(sFromDate, new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(sToDate, new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    int nguonKhieuNai = ConvertUtility.ToInt32(Request.QueryString["nguonKhieuNai"], -1);

                    DoiTacInfo dtInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(khuvucid);

                    if (dtInfo != null)
                    {
                        lblTenDoiTac.Text = dtInfo.TenDoiTac.ToUpper();
                    }
                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", sFromDate, sToDate);
                    lblFullName.Text = LoginAdmin.AdminLogin().FullName;

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    //Build nội dung báo cáo
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopTheoLoaiKhieuNaiToanMangVNP(khuvucid, fromDateBefore, toDateBefore, fromDate,toDate, nguonKhieuNai);
                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoKhieuNaiDichVuToanMangVNP_" + fromDate + "_" + toDate);
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