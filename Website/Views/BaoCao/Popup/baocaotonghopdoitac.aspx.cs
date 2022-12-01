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
    public partial class baocaotonghopdoitac : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;

            LoginAdmin.IsLoginAdmin();            
            if (!IsPostBack)
            {                
                try
                {
                    var doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];                   

                    DoiTacInfo doiTacInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(doiTacId);
                    if (doiTacInfo != null)
                    {
                        lblTenDoiTac.Text = doiTacInfo.TenDoiTac.ToUpper();
                    }

                    lblFromDateToDate.Text = string.Format("<div><b>Từ ngày:</b> {0} &nbsp;&nbsp;&nbsp;&nbsp;<b>Đến ngày:</b> {1}", Request.QueryString["fromDate"], Request.QueryString["toDate"]);

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopDoiTac_V2(doiTacId, fromDate, toDate);
                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoTongHopDoiTac_" + lblTenDoiTac.Text + "_" + fromDate + "_" + toDate);
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