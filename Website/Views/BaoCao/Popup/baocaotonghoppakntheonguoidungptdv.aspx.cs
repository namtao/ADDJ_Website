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
using AIVietNam.GQKN.Entity;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghoppakntheonguoidungptdv : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;

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
                    var doitacId = ConvertUtility.ToInt32(Request.QueryString["doitacId"]);
                    var phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    var loaibc = Request.QueryString["loaibc"];
                    int loaiKhieuNai_NhomId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNai_NhomId"], -1);
                    int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], -1);
                    int linhVucChungId = ConvertUtility.ToInt32(Request.QueryString["LinhVucChungId"], -1);
                    int linhVucConId = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"], -1);

                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);
                    string tenTruyCap = Request.QueryString["tenTruyCap"];

                    AdminInfo userLogin = LoginAdmin.AdminLogin();
                    if (tenTruyCap != null && tenTruyCap.Length > 0)
                    {
                        // Kiểm tra tenTruyCap == người đăng nhập không
                        // Nếu true : Hiển thị báo cáo
                        //  Nếu false : Không hiển thị báo cáo (vì đây là báo cáo cá nhân)
                        if (userLogin != null && userLogin.Username.ToLower() == tenTruyCap.ToLower())
                        {
                            // TinhId = 0, HuyenId = 0 => Full tỉnh huyện
                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNTheoNguoiDung(doitacId, phongBanId, tenTruyCap, fromDate, toDate, 0, 0);
                        }
                    }
                    else
                    {
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopNguoiDungPhongBan_V2(doitacId, phongBanId, fromDate, toDate);
                    }

                    //sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNTheoNguoiDungVNPTTT(doitacId, phongBanId, nguoiSuDungId, ConvertUtility.ToInt32(fromDate), ConvertUtility.ToInt32(toDate));

                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoTongHopPAKNTheoNguoiDungPTDV_" + fromDate.ToString("yyyyMMdd") + "_" + toDate.ToString("yyyyMMdd"));
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