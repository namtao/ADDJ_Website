using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using SolrNet.Commands.Parameters;
using SolrNet;
using AIVietNam.GQKN.Impl;

namespace Website.Views.BaoCao.Popup
{
    public partial class BaoCaoTongHopPAKNTheoNguoiDungVNPTTT : Page
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            AdminInfo user = LoginAdmin.AdminLogin();
            // lblFullName.Text = user.FullName;
            if (!IsPostBack)
            {
                // lblCurDate.Text = string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                // lblFullName.Text = fullName;
                try
                {
                    int doitacId = ConvertUtility.ToInt32(Request.QueryString["doitacId"]);
                    int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    string loaibc = Request.QueryString["loaibc"];
                    int loaiKhieuNai_NhomId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNai_NhomId"], -1);
                    int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], -1);
                    int linhVucChungId = ConvertUtility.ToInt32(Request.QueryString["LinhVucChungId"], -1);
                    int linhVucConId = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"], -1);

                    int tinhId = ConvertUtility.ToInt32(Request.QueryString["TinhId"], 0);
                    int huyenId = ConvertUtility.ToInt32(Request.QueryString["HuyenId"], 0);

                    string tenTinh = tinhId > 0 ? ServiceFactory.GetInstanceProvince().GetInfo(tinhId).Name : "Tất cả";
                    string tenHuyen = huyenId > 0 ? ServiceFactory.GetInstanceProvince().GetInfo(huyenId).Name : "Tất cả";

                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);
                    liLocation.Text = string.Format("<b>Tỉnh/thành phố:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp; <b>Quận/huyện:</b> {1}", tenTinh, tenHuyen);

                    string tenTruyCap = Request.QueryString["tenTruyCap"];

                    // AdminInfo user = LoginAdmin.AdminLogin();
                    if (tenTruyCap != null && tenTruyCap.Length > 0)
                    {
                        // Kiểm tra tenTruyCap == người đăng nhập không
                        // Nếu true : Hiển thị báo cáo
                        // Nếu false : Không hiển thị báo cáo (vì đây là báo cáo cá nhân)

                        if (user != null && user.Username.ToLower() == tenTruyCap.ToLower())
                        {
                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNTheoNguoiDung(doitacId, phongBanId, tenTruyCap, fromDate, toDate, tinhId, huyenId);
                        }
                    }
                    else
                    {
                        // Edited by	: Dao Van Duong
                        // Datetime		: 9.8.2016 10:23
                        // Note			: Nâng cấp thêm tỉnh, huyện
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopNguoiDungPhongBan_V4(doitacId, phongBanId, fromDate, toDate, tinhId, huyenId);
                    }

                    //sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNTheoNguoiDungVNPTTT(doitacId, phongBanId, nguoiSuDungId, ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate));

                    if (loaibc.ToLower() == "Excel".ToLower())
                    {
                        export2excel("BaoCaoTongHopPAKNTheoNguoiDung_" + fromDate.ToString("yyyyMMdd") + "_" + toDate.ToString("yyyyMMdd"));
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