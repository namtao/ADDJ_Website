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
    public partial class baocaotonghopvnptnet : System.Web.UI.Page
    {
        protected string sNoiDungBaoCao = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int doiTacId = ConvertUtility.ToInt32(Request.QueryString["doiTacId"], -1);
                int phongBanId = ConvertUtility.ToInt32(Request.QueryString["phongBanId"], -1);
                DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                DateTime toDate = Convert.ToDateTime(Request.QueryString["toDate"], new CultureInfo("vi-VN"));
                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

                string reportType = Request.QueryString["reportType"];

                var loaibc = Request.QueryString["loaibc"];

                lbFromDate.Text = fromDate.ToString("dd/MM/yyyy");
                lbToDate.Text = toDate.ToString("dd/MM/yyyy");

                bool isExportExcel = false;
                if (loaibc == "excel")
                {
                    isExportExcel = true;
                }

                string fileName = string.Empty;
                switch (reportType.ToLower())
                {
                    case "bc_vnptnet_baocaotonghopvnptnet":
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopVNPTX_V2(doiTacId, fromDate, toDate);
                        fileName = "BaoCaoTongHopVNPTNET";
                        lblTitle.Text = "BÁO CÁO TỔNG HỢP VNPT NET";
                        break;
                    case "bc_vnptnet_baocaotonghopdonvi":
                        List<int> listDoiTacId = new List<int>();
                        if (doiTacId != DoiTacInfo.DoiTacIdValue.VNPT_NET)
                        {
                            listDoiTacId.Add(doiTacId);
                        }
                        else
                        {
                            List<DoiTacInfo> listDoiTacInfo = ServiceFactory.GetInstanceDoiTac().GetListByDonViTrucThuoc(doiTacId);
                            if (listDoiTacInfo != null)
                            {
                                for (int i = 0; i < listDoiTacInfo.Count; i++)
                                {
                                    listDoiTacId.Add(listDoiTacInfo[i].Id);
                                }
                            }
                        }

                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopDoiTac_V2_NET(doiTacId, listDoiTacId, fromDate, toDate);
                        fileName = "BaoCaoTongHopDonVi";
                        lblTitle.Text = "BÁO CÁO TỔNG HỢP ĐƠN VỊ";
                        break;
                    case "bc_vnptnet_baocaotonghopphongban":
                        List<int> listPhongBanId = new List<int>();
                        if (phongBanId > 0)
                        {
                            listPhongBanId.Add(phongBanId);
                        }
                        else
                        {
                            List<PhongBanInfo> listPhongBanInfo = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfDoiTacId(doiTacId);
                            if (listPhongBanInfo != null)
                            {
                                for (int i = 0; i < listPhongBanInfo.Count; i++)
                                {
                                    listPhongBanId.Add(listPhongBanInfo[i].Id);
                                }
                            }
                        }

                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopPAKNPhongBan_V2_NET(doiTacId, listPhongBanId, fromDate, toDate);
                        fileName = "BaoCaoTongHopPhongBan";
                        lblTitle.Text = "BÁO CÁO TỔNG HỢP PHÒNG BAN";
                        break;
                    case "bc_vnptnet_baocaotonghopnguoidung":
                        sNoiDungBaoCao = new BuildBaoCao().BaoCaoTongHopNguoiDungPhongBan_V3(doiTacId, phongBanId, fromDate, toDate);
                        fileName = "BaoCaotongHopNguoiDung";
                        lblTitle.Text = "BÁO CÁO TỔNG HỢP NGƯỜI DÙNG";
                        break;
                }



                if (isExportExcel)
                {
                    export2excel("BaoCaoTongHop_" + fromDate + "_" + toDate);
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