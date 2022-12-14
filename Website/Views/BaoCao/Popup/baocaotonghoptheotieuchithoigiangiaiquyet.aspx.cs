using System;
using System.Web.UI;
using AIVietNam.Core;
using Website.AppCode.Controller;
using System.IO;
using System.Globalization;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghoptheotieuchithoigiangiaiquyet : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string fromPage = Request.QueryString["fromPage"];
                    string dsDonViTiepNhan = Request.QueryString["DsDonViTiepNhan"];
                    string mucTien1 = Request.QueryString["mucTien1"];
                    string mucTien2 = Request.QueryString["mucTien2"];

                    DateTime fromDate;//= Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    DateTime toDate;//= Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int reportType = ConvertUtility.ToInt32(Request.QueryString["reportType"]);
                    int pageIndex = ConvertUtility.ToInt32(Request.QueryString["pageIndex"]);
                    int pageSize = ConvertUtility.ToInt32(Request.QueryString["pageSize"]);

                    fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    lblTenFile.Text = string.Format("{0}_{1}_{2}", fromPage, fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));

                    switch (fromPage.ToLower())
                    {
                        case "baocaotonghoptheotieuchithoigiangiaiquyet":
                            ltTitle.Text = "BÁO CÁO TỔNG HỢP THEO CHỈ TIÊU THỜI GIAN GIẢI QUYẾT";
                            sNoiDungBaoCao = BuildBaoCao2.BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet(dsDonViTiepNhan, mucTien1, mucTien2, fromDate, toDate);
                            break;
                    }
                    lblNoiDungBaoCao.Text = sNoiDungBaoCao;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/03/2014
        /// Todo : Xuất excel nội dung html
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            //string url = string.Format("{0}&loaibc=excel", Request.Url.AbsoluteUri);
            //Response.Redirect(url);
            sNoiDungBaoCao = lblNoiDungBaoCao.Text;
            btnExportExcelTop.Visible = false;
            btnExportExcel.Visible = false;
            export2excel(lblTenFile.Text);
        }

        private void export2excel(string tenbc)
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