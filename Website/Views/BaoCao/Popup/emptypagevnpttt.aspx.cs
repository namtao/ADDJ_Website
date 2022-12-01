using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.Popup
{
    public partial class emptypagevnpttt : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = Request.QueryString["page"] != null ? Request.QueryString["page"] : string.Empty;
                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                string loaibc = Request.QueryString["loaibc"];

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }

                BuildBaoCao buildBaoCao = new BuildBaoCao();

                switch (page)
                {
                    case "baocaotonghopgiamtruvnpttt":
                        int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                        string tenDoiTac = Request.QueryString["tenDoiTac"] != null ? Request.QueryString["tenDoiTac"] : string.Empty;

                        lblReportTitle.Text = "BÁO CÁO TỔNG HỢP GIẢM TRỪ VNPT TTP KV";
                        lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                        List<int> listDoiTacId = new List<int>();
                        DoiTacInfo objDoiTacInfo = ServiceFactory.GetInstanceDoiTac().GetInfo(doiTacId);
                        if (objDoiTacInfo != null)
                        {
                            string khuVuc = string.Empty;
                            if (objDoiTacInfo.Id == DoiTacInfo.DoiTacIdValue.VNP1 || objDoiTacInfo.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP1)
                            {
                                khuVuc = "KV 1";
                            }
                            else if (objDoiTacInfo.Id == DoiTacInfo.DoiTacIdValue.VNP2 || objDoiTacInfo.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP2)
                            {
                                khuVuc = "KV 2";
                            }
                            else if (objDoiTacInfo.Id == DoiTacInfo.DoiTacIdValue.VNP3 || objDoiTacInfo.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP3)
                            {
                                khuVuc = "KV 3";
                            }

                            lblReportTitle.Text = "BÁO CÁO TỔNG HỢP GIẢM TRỪ VNPT TTP " + khuVuc;

                            if (objDoiTacInfo.DoiTacType == DoiTacInfo.DoiTacTypeValue.VNPTTT)
                            {
                                listDoiTacId.Add(doiTacId);
                            }
                            else
                            {
                                string whereClause = string.Empty;
                                if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                                {
                                    whereClause = string.Format("DoiTacType=4 AND DonViTrucThuoc IN ({0},{1},{2})", DoiTacInfo.DoiTacIdValue.VNP1, DoiTacInfo.DoiTacIdValue.VNP2, DoiTacInfo.DoiTacIdValue.VNP3);
                                }
                                else
                                {
                                    whereClause = string.Format("DoiTacType=4 AND DonViTrucThuoc={0}", doiTacId);
                                }

                                List<DoiTacInfo> listDoiTacInfo = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereClause, "TenDoiTac ASC");
                                for (int i = 0; i < listDoiTacInfo.Count; i++)
                                {
                                    listDoiTacId.Add(listDoiTacInfo[i].Id);
                                }
                            }
                        }

                        sNoiDungBaoCao = buildBaoCao.BaoCaoTongHopGiamTruVNPTTT(listDoiTacId, fromDate, toDate, isExportExcel);
                        break;
                    case "bcth_giamtru_dvgtgt_donvi":

                        int doiTacId1 = ConvertUtility.ToInt32(Request.QueryString["doiTacId"]);
                        string tenDoiTac1 = Request.QueryString["tenDoiTac"] != null ? Request.QueryString["tenDoiTac"] : string.Empty;

                        lblReportTitle.Text = "BÁO CÁO TỔNG HỢP GIẢM TRỪ DV GTGT THEO ĐƠN VỊ";
                        lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));

                        List<int> listDoiTacId1 = new List<int>();
                        DoiTacInfo objDoiTacInfo1 = ServiceFactory.GetInstanceDoiTac().GetInfo(doiTacId1);
                        if (objDoiTacInfo1 != null)
                        {
                            string khuVuc = string.Empty;
                            if (objDoiTacInfo1.Id == DoiTacInfo.DoiTacIdValue.VNP1 || objDoiTacInfo1.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP1)
                            {
                                khuVuc = "KV 1";
                            }
                            else if (objDoiTacInfo1.Id == DoiTacInfo.DoiTacIdValue.VNP2 || objDoiTacInfo1.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP2)
                            {
                                khuVuc = "KV 2";
                            }
                            else if (objDoiTacInfo1.Id == DoiTacInfo.DoiTacIdValue.VNP3 || objDoiTacInfo1.DonViTrucThuoc == DoiTacInfo.DoiTacIdValue.VNP3)
                            {
                                khuVuc = "KV 3";
                            }

                            lblReportTitle.Text = "BÁO CÁO TỔNG HỢP GIẢM TRỪ DV GTGT " + khuVuc;

                            if (objDoiTacInfo1.DoiTacType == DoiTacInfo.DoiTacTypeValue.VNPTTT)
                            {
                                listDoiTacId1.Add(doiTacId1);
                            }
                            else
                            {
                                string whereClause = string.Empty;
                                if (doiTacId1 == DoiTacInfo.DoiTacIdValue.VNP)
                                {
                                    whereClause = string.Format("DoiTacType=4 AND DonViTrucThuoc IN ({0},{1},{2})", DoiTacInfo.DoiTacIdValue.VNP1, DoiTacInfo.DoiTacIdValue.VNP2, DoiTacInfo.DoiTacIdValue.VNP3);
                                }
                                else
                                {
                                    whereClause = string.Format("DoiTacType=4 AND DonViTrucThuoc={0}", doiTacId1);
                                }

                                List<DoiTacInfo> listDoiTacInfo = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereClause, "TenDoiTac ASC");
                                for (int i = 0; i < listDoiTacInfo.Count; i++)
                                {
                                    listDoiTacId1.Add(listDoiTacInfo[i].Id);
                                }
                            }
                        }

                        sNoiDungBaoCao = buildBaoCao.BCTH_GiamTru_DVGTGTDV(doiTacId1, fromDate, toDate, isExportExcel);

                        break;
                    case "bcth_giamtru_dvgtgt_dichvu":
                        lblReportTitle.Text = "BÁO CÁO TỔNG HỢP GIẢM TRỪ DV GTGT THEO DỊCH VỤ";
                        lblReportMonth.Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                        sNoiDungBaoCao = buildBaoCao.BCTH_GiamTru_DVGTGT_DIVU(fromDate, toDate, isExportExcel);

                        break;
                }// end switch

                if (isExportExcel)
                {
                    export2excel("BaoCaoTongHopTheoKhieuNai_" + fromDate + "_" + toDate);
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