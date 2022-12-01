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
    public partial class baocaosolieupakndaxulyttptdv : AppCode.PageBase
    {
        protected string sNoiDungBaoCao = "";

        private readonly int TT_PTDV = 10101;
        private readonly int PHONG_PTDV = 52;        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                                 
                var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));                    

                var loaibc = Request.QueryString["loaibc"];
                    
                lbFromDate.Text = Request.QueryString["fromDate"];
                lbToDate.Text = Request.QueryString["toDate"];

                List<NguoiSuDungInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung().GetListNguoiSuDungByPhongBanId(PHONG_PTDV);
                string sNguoiDung = string.Empty;
                if (listNguoiSuDung != null && listNguoiSuDung.Count > 0)
                {
                    sNguoiDung = listNguoiSuDung[0].TenTruyCap;
                    for (int i = 1; i < listNguoiSuDung.Count; i++)
                    {
                        sNguoiDung = string.Format("{0},{1}", sNguoiDung, listNguoiSuDung[i].TenTruyCap);
                    }
                }

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }

                sNoiDungBaoCao = new BuildBaoCao().BaoCaoSoLuongPAKNDaXuLyTTPTDV(TT_PTDV, PHONG_PTDV, sNguoiDung, fromDate, toDate, isExportExcel);

                if (isExportExcel)
                {
                    export2excel("BaoCaoSoLieuPAKNDaXuLy_" + fromDate + "_" + toDate);
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