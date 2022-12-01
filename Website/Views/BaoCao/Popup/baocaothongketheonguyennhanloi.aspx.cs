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
using AIVietNam.Admin;


namespace Website.Views.BaoCao.Popup
{
    public partial class baocaothongketheonguyennhanloi : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = "";
        protected string sNoiDungGhiChu = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                int khuVucId = ConvertUtility.ToInt32(Request.QueryString["khuVucId"], 1);
                int doiTacId = -1;
                int phongBanId = -1;
                var fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                var toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                int nguonKhieuNai = ConvertUtility.ToInt32(Request.QueryString["nguonKhieuNai"]);
                string sLoaiKhieuNaiId = Request.QueryString["loaiKhieuNaiId"];
                string sLinhVucChungId = Request.QueryString["linhVucChungId"];
                string sLinhVucConId = Request.QueryString["linhVucConId"];
                string displayLevelLoaiKhieuNai = Request.QueryString["displayLevelLoaiKhieuNai"];
                var loaibc = Request.QueryString["loaibc"];

                if (khuVucId == DoiTacInfo.DoiTacIdValue.VNP1)
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
                List<int> listLinhVucChungId = new List<int>();
                List<int> listLinhVucConId = new List<int>();
                List<int> listNguyenNhanLoiId = new List<int>();
                
                if(sLoaiKhieuNaiId != null && sLoaiKhieuNaiId.Length > 0)
                {
                    string[] arrLoaiKhieuNaiId = sLoaiKhieuNaiId.Split(',');
                    if(arrLoaiKhieuNaiId != null)
                    {
                        for(int i=0;i<arrLoaiKhieuNaiId.Length;i++)
                        {
                            listLoaiKhieuNaiId.Add(ConvertUtility.ToInt32(arrLoaiKhieuNaiId[i]));
                        }
                    }
                }

                if (sLinhVucChungId != null && sLinhVucChungId.Length > 0)
                {
                    string[] arrLinhVucChungId = sLinhVucChungId.Split(',');
                    if (arrLinhVucChungId != null)
                    {
                        for (int i = 0; i < arrLinhVucChungId.Length; i++)
                        {
                            listLinhVucChungId.Add(ConvertUtility.ToInt32(arrLinhVucChungId[i]));
                        }
                    }
                }

                if (sLinhVucConId != null && sLinhVucConId.Length > 0)
                {
                    string[] arrLinhVucConId = sLinhVucConId.Split(',');
                    if (arrLinhVucConId != null)
                    {
                        for (int i = 0; i < arrLinhVucConId.Length; i++)
                        {
                            listLinhVucConId.Add(ConvertUtility.ToInt32(arrLinhVucConId[i]));
                        }
                    }
                }              

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }
                

                if(displayLevelLoaiKhieuNai == "1")
                {
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId, isExportExcel);
                }
                else if(displayLevelLoaiKhieuNai == "2")
                {
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId, listLinhVucChungId, isExportExcel);
                }
                else if (displayLevelLoaiKhieuNai == "3")
                {
                    sNoiDungBaoCao = new BuildBaoCao().BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL(khuVucId, doiTacId, phongBanId, fromDate, toDate, nguonKhieuNai, listLoaiKhieuNaiId, listLinhVucChungId, listLinhVucConId, isExportExcel);
                }

                FillNoiDungGhiChu();

                if (isExportExcel)
                {
                    export2excel("BaoCaoThongKeKNTheoLoaiKhieuNaiVaNguyenNhanLoi_PCL" + fromDate + "_" + toDate);
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

        private void FillNoiDungGhiChu()
        {
            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
            int indexCap1 = 1;

            if(listLoiKhieuNai != null)
            {
                for(int i=0;i<listLoiKhieuNai.Count;i++)
                {
                    string styleKN = string.Empty;
                    if(listLoiKhieuNai[i].Loai == (int)LoiKhieuNai_Loai.Khiếu_nại)
                    {
                        styleKN = " style='background-color: yellow'";
                    }
                    if(listLoiKhieuNai[i].Cap == 1)
                    {
                        sNoiDungGhiChu = string.Format("{0}<span {3}>{1} : {2}</span><br/>", sNoiDungGhiChu, indexCap1, listLoiKhieuNai[i].TenLoi, styleKN);
                        indexCap1++;
                    }
                    else
                    {
                        sNoiDungGhiChu = string.Format("{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-&nbsp; <span {3}>L{1} : {2}</span><br/>", sNoiDungGhiChu, listLoiKhieuNai[i].Id, listLoiKhieuNai[i].TenLoi, styleKN);
                    }
                }
            }
        }
    }
}