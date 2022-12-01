using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using Website.AppCode.Controller;
using AIVietNam.Core;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaosolieupakntoanmangptdv : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = "";

        private readonly int TT_PTDV = 10101;
        private readonly int PHONG_PTDV = 52;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"], 1);
                var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));

                var loaibc = Request.QueryString["loaibc"];

                if(khuVucId == DoiTacInfo.DoiTacIdValue.VNP1)
                {
                    lblKhuVuc.Text = "VNP 1";
                }
                if (khuVucId == DoiTacInfo.DoiTacIdValue.VNP2)
                {
                    lblKhuVuc.Text = "VNP 2";
                }
                if (khuVucId == DoiTacInfo.DoiTacIdValue.VNP3)
                {
                    lblKhuVuc.Text = "VNP 3";
                }
                if (khuVucId == DoiTacInfo.DoiTacIdValue.VNP)
                {
                    lblKhuVuc.Text = "Toàn mạng";
                }

                lbFromDate.Text = fromDate.ToString("dd/MM/yyyy");
                lbToDate.Text = toDate.ToString("dd/MM/yyyy");
                List<int> listLoaiKhieuNaiId = new List<int>();
                //listLoaiKhieuNaiId.Add(26);

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }

                sNoiDungBaoCao = new BuildBaoCao().BaoCaoKhieuNaiDichVuToanMang(khuVucId, listLoaiKhieuNaiId, fromDate, toDate, isExportExcel);

                if (isExportExcel)
                {
                    export2excel("BaoCaoKhieuNaiDichVuToanMangTTPTDV_" + fromDate + "_" + toDate);
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