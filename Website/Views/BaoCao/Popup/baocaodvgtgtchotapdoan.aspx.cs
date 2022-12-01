using AIVietNam.Core;
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
    public partial class baocaodvgtgtchotapdoan : PageBase
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = Request.QueryString["Page"] != null ? Request.QueryString["Page"] : string.Empty;

                string loaiBC = Request.QueryString["LoaiBC"];
                string month = Request.QueryString["Month"];
                string year = Request.QueryString["Year"];

                switch (page)
                {
                    // Dương Dv
                    case "bc_vnp_baocaodvgtgttapdoan_new":
                        bool isOk = false;
                        int intMonth = 0;
                        int intYear = 0;
                        if (int.TryParse(month, out intMonth))
                        {
                            if (intMonth > 0 && intMonth <= 12) isOk = true;
                        };
                        if (int.TryParse(year, out intYear))
                        {
                            if (intYear >= DateTime.Now.Year - 10 && intYear <= DateTime.Now.Year) isOk = true;
                            else isOk = false;
                        }
                        else isOk = false;

                        bool isIdsOK = false;
                        List<string> myIds = new List<string>();
                        string lsIds = Request.QueryString["Ids"]; // Danh sách đơn vị báo cáo
                        if (!string.IsNullOrEmpty(lsIds))
                        {
                            string[] lstIds = lsIds.Split(',');
                            if (lstIds.Length > 0)
                            {
                                foreach (string s in lstIds)
                                {
                                    int id = 0;
                                    if (!int.TryParse(s, out id)) break;
                                    myIds.Add(id.ToString());
                                }
                                isIdsOK = true;
                            }
                        }

                        if (isOk && isIdsOK)
                        {
                            lblReportTitle.Text = string.Format("Báo cáo dịch vụ giá trị gia tăng tháng {0}/{1}", month, year);
                            sNoiDungBaoCao = new BuildBaoCao().BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan(intMonth, intYear, myIds.ToArray());
                        }
                        else
                        {
                            btnExportExcel.Visible = false;
                            btnExportExcelTop.Visible = false;
                            sNoiDungBaoCao = "Dữ liệu ngày tháng không hợp lệ, vui lòng kiểm tra lại";
                            Helper.GhiLogs("LoiBaoCao", "Đường dẫn: {0}, Lỗi do ngày tháng, hoạc danh sách đơn vị báo cáo", Request.RawUrl);
                        }
                        break;
                }

                lblTenFile.Text = page;
                lblNoiDungBaoCao.Text = sNoiDungBaoCao;

                string loaiBc = Request.QueryString["LoaiBC"];

                lblTenFile.Text = string.Join("_", "BaoCaoDichVuGiaTriGiaTangTapDoan", year, month);

                if (loaiBc.ToUpper() == "Excel".ToUpper())
                {
                    btnExportExcelTop.Visible = false;
                    btnExportExcel.Visible = false;
                    Export2Excel(lblTenFile.Text);
                }
            }
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            //string url = string.Format("{0}&loaibc=excel", Request.Url.AbsoluteUri);
            //Response.Redirect(url);
            sNoiDungBaoCao = lblNoiDungBaoCao.Text;
            btnExportExcelTop.Visible = false;
            btnExportExcel.Visible = false;
            Export2Excel(lblTenFile.Text);
        }

        private void Export2Excel(string tenbc)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCss = Server.MapPath("~/Css/BaoCao.css");
            StreamReader reader = new StreamReader(pathCss);
            //reader.ReadToEnd();

            Response.Write("<style type=\"text/Css\">");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}