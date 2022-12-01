using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using Website.AppCode.Controller;
using System.IO;
using System.Globalization;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghopgiamtrutheocpvnp : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    
                    var loaibc = Request.QueryString["loaibc"];
                   
                    lblKhuVucHeader.Text = lbKhuVuc.Text;
                    
                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    if (fromDate.Month == toDate.Month)
                    {
                        lblReportMonth.Text = string.Format("THÁNG {0}", fromDate.ToString("MM/yyyy"));
                    }
                    else
                    {
                        lblReportMonth.Text = string.Format("TỪ {0} - {1}", fromDate.ToString("MM/yyyy"), toDate.ToString("MM/yyyy"));
                    }

                    string where = "Hà Nội";
                    string sHeader = string.Empty;

                    DoiTacInfo doiTacInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(doiTacId);
                    if(doiTacInfo != null)
                    {
                        lbKhuVuc.Text = doiTacInfo.TenDoiTac;
                    }
                    
                    sHeader = string.Format("Đơn vị {0}", lbKhuVuc.Text);                    

                    lblKhuVucHeader.Text = sHeader;
                    lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = BuildBaoCao.BaoCaoTongHopGiamTru(doiTacId, fromDate, toDate, isExportExcel);

                    if (isExportExcel)
                    {
                        export2excel("BaoCaoTongHopGiamTru" + fromDate + "_" + toDate);
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