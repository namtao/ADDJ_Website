using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class baocaotonghoptheokhieunai_gqkn : Page
    {
        protected string sNoiDungBaoCao = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucID"]);
                    var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                    int phongBanXuLyId = ConvertUtility.ToInt32(Request.QueryString["donViId"]);

                    string doiTac = Request.QueryString["doiTac"];
                 
                    string loaiKhieuNai = Session["LoaiKhieuNai"] != null ? Session["LoaiKhieuNai"].ToString() : string.Empty;
                    string linhVucChung = Session["LinhVucChung"] != null ? Session["LinhVucChung"].ToString() : string.Empty;
                    string linhVucCon = Session["LinhVucCon"] != null ? Session["LinhVucCon"].ToString() : string.Empty;
                    var loaibc = Request.QueryString["loaibc"];

                    lblTuNgay.Text = Request.QueryString["fromDate"];
                    lblDenNgay.Text = Request.QueryString["toDate"];

                    var listDoiTac = new List<string>();
                    var listLoaiKhieuNai = new List<string>();
                    var listLinhVucChung = new List<string>();
                    var listLinhVucCon = new List<string>();

                    if (doiTac.Length > 0)
                    {
                        string[] arrDoiTac = doiTac.Split(',');
                        listDoiTac = arrDoiTac.ToList<string>();
                    }

                    if (loaiKhieuNai.Length > 0)
                    {
                        string[] arrLoaiKhieuNai = loaiKhieuNai.Split(',');
                        listLoaiKhieuNai = arrLoaiKhieuNai.ToList<string>();
                    }

                    if (linhVucChung.Length > 0)
                    {
                        string[] arrLinhVucChung = linhVucChung.Split(',');
                        listLinhVucChung = arrLinhVucChung.ToList<string>();
                    }

                    if (linhVucCon.Length > 0)
                    {
                        string[] arrLinhVucCon = linhVucCon.Split(',');
                        listLinhVucCon = arrLinhVucCon.ToList<string>();
                    }

                    //int phongBanXuLyId = LoginAdmin.AdminLogin().PhongBanId;
                    string where = string.Empty;

                    if (khuVucId == 7)
                    {
                        where = "Hà Nội";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV I";
                    }
                    else if (khuVucId == 14)
                    {
                        where = "TP Hồ Chí Minh";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV II";
                    }
                    else if (khuVucId == 19)
                    {
                        where = "Đà Nẵng";
                        lblKhuVuc.Text = "TRUNG TÂM DỊCH VỤ VIỄN THÔNG KV III";
                    }
                    else
                    {
                        where = "Hà Nội";
                    }


                    lblWhereWhen.Text = string.Format("{0}, ngày {1} tháng {2} năm {3}", where, DateTime.Now.Day.ToString("D2"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Year);
                    lblWho.Text = LoginAdmin.AdminLogin().FullName;

                    var buildBaoCao = new BuildBaoCao();
                    sNoiDungBaoCao = buildBaoCao.BaoCaoTongHopTheoKhieuNai(khuVucId, phongBanXuLyId, fromDate, toDate,
                                                                            listDoiTac, listLoaiKhieuNai, listLinhVucChung, listLinhVucCon);

                    if (loaibc == "excel")
                    {
                        export2excel("To_GQKN_BaoCaoTongHopTheoKhieuNai_" + fromDate + "_" + toDate);
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
            EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            var stringWrite = new System.IO.StringWriter();
            var htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCSS = Server.MapPath("~/CSS");
            pathCSS += @"\BaoCao.css";
            var reader = new StreamReader(pathCSS);
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