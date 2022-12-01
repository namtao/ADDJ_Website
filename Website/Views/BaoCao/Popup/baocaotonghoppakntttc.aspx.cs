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
using Website.AppCode;

namespace Website.Views.BaoCao.Popup
{
    /// <summary>
    /// Page này mặc định chỉ có lãnh đạo của TTTC mới vào xem được thông tin
    /// </summary>
    public partial class baocaotonghoppakntttc : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;

            LoginAdmin.IsLoginAdmin();
            //var userLogin = LoginAdmin.AdminLogin();
            //fullName = userLogin.FullName;
            if (!IsPostBack)
            {
                //lblCurDate.Text = string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                //lblFullName.Text = fullName;
                try
                {
                    var khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);                    
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    int phongBanId = -1; //ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    int loaiKhieuNai_NhomId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNai_NhomId"], -1);
                    int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"], -1);
                    int linhVucChungId = ConvertUtility.ToInt32(Request.QueryString["LinhVucChungId"], -1);
                    int linhVucConId = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"], -1);

                    DoiTacInfo doiTacInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(khuVucId);
                    if(doiTacInfo != null)
                    {
                        lblTenDoiTac.Text = doiTacInfo.TenDoiTac.ToUpper();
                    }

                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopTTTC(khuVucId, fromDate, toDate, loaiKhieuNai_NhomId, loaiKhieuNaiId, linhVucChungId, linhVucConId);
                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoTongHopTT_" + fromDate + "_" + toDate);
                    }
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }

            DateTime endTime = DateTime.Now;
            lblTime.Text = string.Format("Thời gian : {0} - {1} : {2}", startTime.ToString("HH:mm:ss"), endTime.ToString("HH:mm:ss"), endTime.Subtract(startTime).TotalMinutes);
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