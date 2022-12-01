using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using AIVietNam.Admin;
using AIVietNam.Core;
using System.Globalization;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaodoisoatcskhptdv : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();            
            if (!IsPostBack)
            {                
                try
                {                    
                    int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    
                    lblCurDate.Text = string.Format("ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));                                                          
                    lblReportDate.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                    switch(phongBanId)
                    {
                        case 53:
                            lblLocation.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVI";
                            break;
                        case 62:
                            lblLocation.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVII";
                            break;
                        case 67:
                            lblLocation.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KVIII";
                            break;
                        default:

                            break;
                    }

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = BuildBaoCao.BaoCaoDoiSoatDoanhThuBUCuocDVGiuaVASVaCSKH(phongBanId, fromDate, toDate, isExportExcel);
                    if (isExportExcel)
                    {
                        export2excel("BaoCaoDoiSoatDoanhThuBUCuocDVGiuaVASVaCSKH_" + fromDate + "_" + toDate);
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