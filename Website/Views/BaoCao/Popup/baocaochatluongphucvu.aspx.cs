using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using System.Globalization;
using System.IO;


namespace Website.Views.BaoCao.Popup
{
    public partial class baocaochatluongphucvu : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {           
            if (!IsPostBack)
            {
                try
                {
                    var doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    var loaibc = Request.QueryString["loaibc"];
                    int nguonKhieuNai = ConvertUtility.ToInt32(Request.QueryString["nguonKhieuNai"], -1);
                    string reportType = Request.QueryString["reportType"];

                    toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                    int khenChe = -1;
                    List<int> listLoaiKhieuNaiId = new List<int>();
                    List<int> listLinhVucChungId = new List<int>();
                    List<int> listLinhVucConId = new List<int>();

                    int loaiKhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["loaiKhieuNaiId"]);
                    int linhVucChungId = ConvertUtility.ToInt32(Request.QueryString["linhVucChungId"]);
                    int linhVucConId = ConvertUtility.ToInt32(Request.QueryString["linhVucConId"]);

                    if (reportType == "baocaotonghop")
                    {
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopChatLuongPhucVu(doiTacId, loaiKhieuNaiId, linhVucChungId, linhVucConId, fromDate, toDate, nguonKhieuNai);
                    }
                    else if (reportType == "baocaochitiet")
                    {
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoChiTietChatLuongPhucVu(doiTacId, loaiKhieuNaiId, linhVucChungId, linhVucConId, nguonKhieuNai, fromDate, toDate);
                    }

                    if (loaibc == "excel")
                    {
                        export2excel("BaoCaoTongHopChatLuongPhucVu_" + fromDate.ToString("yyyyMMdd") + "_" + toDate.ToString("yyyyMMdd"));
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