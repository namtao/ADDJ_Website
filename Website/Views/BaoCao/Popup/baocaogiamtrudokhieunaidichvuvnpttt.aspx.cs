using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Website.AppCode.Controller;
using AIVietNam.Core;
using AIVietNam.Admin;
using System.IO;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaogiamtrudokhieunaidichvuvnpttt : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DateTime nullDateTime = new DateTime(1900, 1, 1);
                    int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    DateTime fromDate = ConvertUtility.ToDateTime(Request.QueryString["fromDate"], "dd/MM/yyyy", nullDateTime);
                    DateTime toDate = ConvertUtility.ToDateTime(Request.QueryString["toDate"], "dd/MM/yyy", nullDateTime);
                    var loaibc = Request.QueryString["loaibc"];
                    int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"]);
                    string tenPhongBan = Request.QueryString["tenPhongBan"];
                    int layDuLieuTheo1HoacNhieuPhongBan = ConvertUtility.ToInt32(Request.QueryString["layDuLieuTheo1HoacNhieuPhongBan"]);
                    bool isDongKN = ConvertUtility.ToBoolean(Request.QueryString["isDongKN"]);

                    lblPhongBan.Text = Request.QueryString["tenDoiTac"] != null ? "VNPT " + Request.QueryString["tenDoiTac"].ToUpper() : string.Empty;
                    if(phongBanId > 0)
                    {
                        lblPhongBan.Text = string.Format("{0}<br/>Phòng {1}", lblPhongBan.Text, tenPhongBan);

                        if(layDuLieuTheo1HoacNhieuPhongBan == 2)
                        {
                            lblPhongBan.Text = lblPhongBan.Text + " và các phòng trực thuộc";
                        }
                    }

                    lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                    bool isExportExcel = false;
                    if (loaibc == "excel")
                    {
                        isExportExcel = true;
                    }

                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoGiamTruDoKhieuNaiDichVuVNPTTT(doiTacId, phongBanId, layDuLieuTheo1HoacNhieuPhongBan, fromDate, toDate, isDongKN, isExportExcel);

                    if (isExportExcel)
                    {
                        export2excel("baocaogiamtrudokhieunaidichvuvnpttt" + fromDate + "_" + toDate);
                    }
                }
                catch(Exception ex)
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