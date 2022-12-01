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

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaochitietpakntheonguoidungtttc : AppCode.PageBase
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
                    var khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"]);
                    var phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    var listTenTruyCap = Request.QueryString["tenTruyCap"];
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    int pageIndex = 0;
                    int pageSize = int.MaxValue;
                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);

                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoChiTietPAKNTheoNguoiDungTTTC_Solr(khuVucId, phongBanId, listTenTruyCap, fromDate, toDate,pageIndex, pageSize);
                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoChiTietPAKNTheoNguoiDungTTTC_" + fromDate + "_" + toDate);
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