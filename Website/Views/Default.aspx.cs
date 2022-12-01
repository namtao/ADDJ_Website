using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Aspose.Cells;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAction.Click += BtnAction_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
            if (!IsPostBack)
            {
                GrvView.DataSource = null;
                GrvView.DataBind();
            }
        }

        private bool RenderDatas(out DataTable tblView)
        {
            bool isok = false;
            tblView = new DataTable();
            tblView.Columns.Add(new DataColumn("Id", typeof(long)));
            tblView.Columns.Add(new DataColumn("KhieuNaiId", typeof(long)));
            tblView.Columns.Add(new DataColumn("TenPhongBan", typeof(string)));
            tblView.Columns.Add(new DataColumn("NguoiXuLy", typeof(string)));

            tblView.Columns.Add(new DataColumn("NgayTiepNhanNguoiXuLy", typeof(string)));
            tblView.Columns.Add(new DataColumn("NgayTiepNhan", typeof(string))); // Tiếp nhận
            tblView.Columns.Add(new DataColumn("NgayQuaHan", typeof(string))); // Xử lý
            tblView.Columns.Add(new DataColumn("LDate", typeof(string)));
            tblView.Columns.Add(new DataColumn("OffSetNXL", typeof(string)));
            tblView.Columns.Add(new DataColumn("OffSetPBXL", typeof(string)));
            tblView.Columns.Add(new DataColumn("IsQuaHan", typeof(string)));


            int phongBanId = 0;
            int.TryParse(txtPhongBanId.Text, out phongBanId);

            string khieuNaiId = txtKhieuNaiId.Text.Trim();
            List<long> dsKhieuNaiId = new List<long>();
            if (khieuNaiId != string.Empty)
            {
                string[] dsMaKhieuNai = khieuNaiId.Split(new string[] { ",", " ", ";", ":" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string kn in dsMaKhieuNai)
                {
                    long knId = 0;
                    if (long.TryParse(kn, out knId))
                    {
                        dsKhieuNaiId.Add(knId);
                    }
                }
                if (dsKhieuNaiId.Count > 0)
                {
                    string URL_SOLR_ACTIVITY = string.Concat(Config.ServerSolr, "Activity");

                    List<int> listKhieuNaiIdTonDong = new List<int>();

                    QueryOptions queryOptionTonDong = new QueryOptions();
                    //Lấy ra những trường nào
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();

                    extraParamTonDong.Add("fl", @"Id, KhieuNaiId, TenPhongBanXuLy, NguoiXuLy, NgayTiepNhan_NguoiXuLy, NgayTiepNhan, HanhDong, IsCurrent, NgayQuaHan, LDate");
                    queryOptionTonDong.ExtraParams = extraParamTonDong;
                    queryOptionTonDong.Start = 0;
                    queryOptionTonDong.Rows = int.MaxValue;

                    // Điều kiện truy vấn
                    string whereClause = string.Empty;
                    whereClause += string.Format("KhieuNaiId : ({0})", string.Join(" ", dsKhieuNaiId));
                    if (phongBanId > 0) whereClause += string.Format(" AND PhongBanXuLyId : {0}", phongBanId);

                    SolrQuery solrQuery = new SolrQuery(whereClause);
                    List<KhieuNai_ReportInfo> lst = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDong).ToList();

                    IEnumerable<IGrouping<int, KhieuNai_ReportInfo>> objs = lst.GroupBy(v => v.KhieuNaiId);
                    foreach (var listItem in objs)
                    {
                        List<KhieuNai_ReportInfo> lstKhieuNaiInfo = listItem.ToList<KhieuNai_ReportInfo>();
                        foreach (var info in lstKhieuNaiInfo.OrderBy(v => v.Id))
                        {
                            DataRow newRow = tblView.NewRow();
                            newRow["Id"] = info.Id;
                            newRow["KhieuNaiId"] = info.KhieuNaiId;
                            newRow["TenPhongBan"] = info.TenPhongBanXuLy;
                            newRow["NguoiXuLy"] = info.NguoiXuLy;
                            newRow["NgayTiepNhanNguoiXuLy"] = string.Empty.GetData(() => info.NgayTiepNhan_NguoiXuLy.ToString("dd/MM/yyyy HH:mm"));
                            newRow["NgayTiepNhan"] = string.Empty.GetData(() => info.NgayTiepNhan.ToString("dd/MM/yyyy HH:mm"));
                            newRow["NgayQuaHan"] = string.Empty.GetData<string>(() => info.NgayQuaHan.ToString("dd/MM/yyyy HH:mm"));
                            newRow["LDate"] = string.Empty.GetData<string>(() => info.LDate.ToString("dd/MM/yyyy HH:mm"));
                            newRow["OffSetNXL"] = string.Empty.GetData<string>(() =>
                            {
                                if (!info.IsCurrent || (info.HanhDong == 4)) // Đã chuyển đi
                                {
                                    DateTime ngayTiepNhanNXL = info.NgayTiepNhan_NguoiXuLy;
                                    DateTime ngayChuyenXuLy = info.LDate;
                                    TimeSpan obj = ngayChuyenXuLy - ngayTiepNhanNXL;
                                    return string.Format("{0}:{1}:{2}:{3}", obj.Days, obj.Hours, obj.Minutes, obj.Seconds);
                                }
                                return "Đang xử lý";
                            });
                            newRow["OffSetPBXL"] = string.Empty.GetData<string>(() =>
                            {
                                if (!info.IsCurrent || (info.HanhDong == 4)) // Đã chuyển đi
                                {
                                    DateTime ngayTiepNhanPBXL = info.NgayTiepNhan;
                                    DateTime ngayChuyenXuLy = info.LDate;
                                    TimeSpan obj = ngayChuyenXuLy - ngayTiepNhanPBXL;
                                    return string.Format("{0}:{1}:{2}:{3}", obj.Days, obj.Hours, obj.Minutes, obj.Seconds);
                                }
                                return "Đang xử lý";
                            });
                            newRow["IsQuaHan"] = string.Empty.GetData<string>(() => (info.LDate - info.NgayQuaHan).Ticks > 0 ? "Quá hạn " : "-");
                            tblView.Rows.Add(newRow);
                        }

                        tblView.Rows.Add(tblView.NewRow());

                        isok = true;
                    }

                    if (tblView.Rows.Count > 1) tblView.Rows.RemoveAt(tblView.Rows.Count - 1);
                }
            }

            return isok;
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable tblView = new DataTable();

            bool isok = RenderDatas(out tblView);

            if (isok) // Xuất exel
            {
                string fileNameTemp = "Temp1.xlsx";
                string pathFile = Server.MapPath("~/Views/ExportExcel/Template/" + fileNameTemp);

                WorkbookDesigner designer = new WorkbookDesigner();
                LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
                designer.Workbook = new Workbook(pathFile, loadOptions);

                string[] dsMaKhieuNai = txtKhieuNaiId.Text.Trim().Split(new string[] { ",", " ", ";", ":" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> ds = new List<string>();
                foreach (string kn in dsMaKhieuNai)
                {
                    long knId = 0;
                    if (long.TryParse(kn, out knId))
                    {
                        ds.Add(string.Format("PA-{0}", knId.ToString("0000000000")));
                    }
                }

                designer.SetDataSource("TieuDe", string.Format("Tổng hợp activity của khiếu nại:  {0}", string.Join(", ", ds)));
                //  designer.SetDataSource("TongSoLuong", string.Concat("Tổng số lượng: ", tblView.Rows.Count));
                designer.SetDataSource("TableChiTiet", tblView.DefaultView);
                designer.Process();

                string exportPath = HttpContext.Current.Server.MapPath("~/Views/ExportExcel/Datas/");
                if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);

                string newFileName = string.Format("Temp1_{0:yyMMddHHmmss}.xlsx", DateTime.Now);
                string fullFileName = Path.Combine(exportPath, newFileName);

                designer.Workbook.Save(fullFileName, SaveFormat.Xlsx);

                ExportExcel2Client(exportPath, newFileName);
            }
        }
        private void ExportExcel2Client(string path, string fileName)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = string.Empty;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            string fullFileName = string.Concat(path, fileName);
            Response.WriteFile(fullFileName);
            Response.End();
        }
        private void BtnAction_Click(object sender, EventArgs e)
        {

            DataTable tblView = null;
            bool isok = RenderDatas(out tblView);
            if (isok)
            {
                GrvView.DataSource = tblView.DefaultView;
                GrvView.DataBind();
            }
            else
            {
                GrvView.DataSource = null;
                GrvView.DataBind();
            }
        }
    }
}