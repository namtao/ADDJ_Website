using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.BaoCao
{
    public partial class BaoCaoTongHopPAKN : PageBase
    {
        public string ReportType { get; set; }
        public int DoiTacId { get; set; }
        public int PhongBanXuLyId { get; set; }
        public string ReportTitle { get; set; }
        public bool isNotShowRegion { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExportExcel.Click += BtnExportExcel_Click;
            if (!IsPostBack)
            {
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                this.DoiTacId = 1;

                switch (this.DoiTacId)
                {
                    case 7:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP1;
                        break;
                    case 14:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP2;
                        break;
                    case 19:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP3;
                        break;
                    case 10100:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP;
                        break;
                }
                LoadKhuVuc(this.DoiTacId);
            }
            else // Nút Submit bị click
            {
                string val = hdfReportCode.Value;
                string html = string.Empty;
                if (val != string.Empty)
                {
                    btnExportExcel.Visible = true;
                    Dictionary<string, object> data = new JavaScriptSerializer().Deserialize(val, typeof(object)) as Dictionary<string, object>;
                    int reportId = (int)data.Single(v => v.Key == "reportId").Value;
                    string securityCode = (string)data.Single(v => v.Key == "securityCode").Value;
                    string tenDayDu = string.Empty;

                    html = RenderTableTag(BaoCaoTongHopPAKN.RenderBodyReport(reportId, securityCode, out tenDayDu)); // Gắn báo cáo
                    html = BaoCaoTongHopPAKN.RenderReportFooter(html, tenDayDu); // Gắn Footer

                    RptContent.Controls.Add(new LiteralControl(html));

                    // Xuất Excel
                    if (rdlSelectedExport.SelectedValue == "0") ExportExcel2Client();
                }
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            ExportExcel2Client();
        }

        private void ExportExcel2Client()
        {
            string bcName = "BaoCaoTongHopPAKN";
            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text, new CultureInfo("vi-VN"));
            DateTime toDate = Convert.ToDateTime(txtToDate.Text, new CultureInfo("vi-VN"));
            bcName = string.Format("{0}_Tu{1:yyyyMMdd}_Den{2:yyyyMMdd}", bcName, fromDate, toDate);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + bcName + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            RptContent.RenderControl(htmlWrite);
            Response.Write(BaoCaoTongHopPAKN.RenderReportHeader(stringWrite.ToString(), fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy")));
            Response.End();
        }

        private void LoadKhuVuc(int doiTacId)
        {
            ddlKhuVuc.Items.Clear();

            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            List<DoiTacInfo> listDoiTacRoot = null;
            string space = string.Empty;

            if (listDoiTac != null)
            {
                if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate (DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });
                }
                else
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate (DoiTacInfo obj) { return obj.Id == doiTacId; });
                }

                for (int i = 0; i < listDoiTacRoot.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    item.Value = listDoiTacRoot[i].Id.ToString();
                    ddlKhuVuc.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
            }
        }
        private void LoadChildDoiTac(string space, int donViTrucThuoc, List<DoiTacInfo> listDoiTacInfo)
        {
            space = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}", space);
            for (int i = 0; i < listDoiTacInfo.Count; i++)
            {
                //if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                if (listDoiTacInfo[i].DonViTrucThuocChoBaoCao == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlKhuVuc.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacInfo[i].Id;

                    listDoiTacInfo.RemoveAt(i);
                    i = -1;

                    if (listDoiTacInfo.Count == 0)
                    {
                        break;
                    }

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTacInfo);
                }
            }

        }
        [WebMethod]
        public static string CalReport(int khuVucId, string fromDate, string toDate, int typeReport)
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();

            bool isHopLe = true;
            if (isHopLe)
            {
                List<DoiTacInfo> listDoiTacInfo = new DoiTacImpl().GetAllDoiTacOfDonViTrucThuocChoBaoCao(khuVucId);
                IEnumerable<int> obj = listDoiTacInfo.Select(v => v.Id);
                if (listDoiTacInfo != null && listDoiTacInfo.Count > 0)
                {
                    string sql = @"
                                    INSERT INTO Report_PAKN_Main (
                                    ReportName, 
                                    DoiTacId,                            
                                    DateFrom,
                                    DateEnd,
                                    UserName, 
                                    FullName, 
                                    CreateDate, 
                                    TimeOut, 
                                    SecurityKey, 
                                    CurrentStep) 
                                    VALUES (
                                    @ReportName, 
                                    @DoiTacId,
                                    @DateFrom,
                                    @DateEnd,
                                    @UserName, 
                                    @FullName, 
                                    @CreateDate,
                                    @TimeOut, 
                                    @SecurityKey,
                                    @CurrentStep) 
                                    SELECT @@IDENTITY";
                    string keyRandom = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                    // DateTime fromDate = Convert.ToDateTime(Request.QueryString["fromDate"], new CultureInfo("vi-VN"));
                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@ReportName", "Báo cáo tổng hợp PAKN"),
                        new SqlParameter("@DoiTacId", khuVucId),
                        new SqlParameter("@DateFrom", Convert.ToDateTime(fromDate, new CultureInfo("vi-VN"))),
                        new SqlParameter("@DateEnd",  Convert.ToDateTime(toDate, new CultureInfo("vi-VN"))),
                         new SqlParameter("@UserName", userInfo.Username ),
                        new SqlParameter("@FullName", userInfo.FullName ),
                        new SqlParameter("@CreateDate", DateTime.Now),
                        new SqlParameter("@TimeOut", 20),
                        new SqlParameter("@SecurityKey",keyRandom),
                        new SqlParameter("@CurrentStep", 1)
                    };

                    object objId = SqlHelper.ExecuteScalar(Config.ConnectionString, CommandType.Text, sql, prms);

                    if (objId != null)
                    {
                        int reportId = Convert.ToInt32(objId);
                        int numer = 0;
                        foreach (DoiTacInfo dtInfo in listDoiTacInfo)
                        {
                            numer += 1;
                            sql = @"
                                    INSERT INTO Report_PAKN_Data(
                                          ReportId,
                                          DoiTacId,
                                          ParentDoiTacId,
                                          TrangThai,
                                          STT,
                                          TenDoiTac,
                                          DoiTacCap1,
                                          DoiTacCap2,
                                          DoiTacCap3,
                                          --SLTonDongKyTruoc,
                                          --SlTiepNhan,
                                          --SLTaoMoi,
                                          --SLDaXuLy,
                                          --SLChuyenXuLy,
                                          --SLChuyenPhanHoi,
                                          --SLDaDong,
                                          --SLDaXuLyQuaHan,
                                          --SLTonDong,
                                          --SLTonDongQuaHan,
                                          Level)
                                    VALUES(
                                          @ReportId,
                                          @DoiTacId,
                                          @ParentDoiTacId,
                                          @TrangThai,
                                          @STT,
                                          @TenDoiTac,
                                          @DoiTacCap1,
                                          @DoiTacCap2,
                                          @DoiTacCap3,
                                          --@SLTonDongKyTruoc,
                                          --@SLTiepNhan,
                                          --@SLTaoMoi,
                                          --@SLDaXuLy,
                                          --@SLChuyenXuLy,
                                          --@SLChuyenPhanHoi,
                                          --@SLDaDong,
                                          --@SLDaXuLyQuaHan,
                                          --@SLTonDong,
                                          --@SLTonDongQuaHan,
                                          @Level
                                    )";
                            SqlParameter[] prms1 = new SqlParameter[]
                            {
                                new SqlParameter("@ReportId", reportId),
                                new SqlParameter("@DoiTacId", dtInfo.Id),
                                new SqlParameter("@ParentDoiTacId", dtInfo.DonViTrucThuocChoBaoCao),
                                new SqlParameter("@TrangThai", false),
                                new SqlParameter("@TenDoiTac", dtInfo.Level == 1 ? dtInfo.TenDoiTac : null),
                                new SqlParameter("@DoiTacCap1", dtInfo.Level == 2 ? dtInfo.TenDoiTac : null),
                                new SqlParameter("@DoiTacCap2", dtInfo.Level == 3 ? dtInfo.TenDoiTac : null),
                                new SqlParameter("@DoiTacCap3", dtInfo.Level == 4 ? dtInfo.TenDoiTac : null),
                                new SqlParameter("@STT", numer),
                                new SqlParameter("@Level", dtInfo.Level),
                            };
                            SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, sql, prms1);
                        }
                    }

                    var result = new
                    {
                        ErrorCode = 1,
                        Message = "Lấy dữ liệu thành công",
                        ReportId = objId,
                        SecurityKey = keyRandom,
                        DoiTacIds = new { Value = string.Join(",", listDoiTacInfo.Select(v => v.Id)) }
                    };
                    return new JavaScriptSerializer().Serialize(result);
                }
            }
            return new JavaScriptSerializer().Serialize(new { ErrorCode = -1, Messaage = "Không tìm thấy dữ liệu phù hợp, vui lòng kiểm tra lại" });
        }
        [WebMethod]
        public static string CalTonDongKyTruoc(int reportId, string securityCode)
        {
            bool isHopLe = true;
            string sql = "SELECT * FROM Report_PAKN_Main WHERE Id = @Id AND SecurityKey = @SecurityKey";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Id", reportId),
                new SqlParameter("@SecurityKey", securityCode),
            };

            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql, prms).Tables[0];
            if (tbl != null && tbl.Rows.Count > 0) isHopLe = true; else isHopLe = false;
            if (isHopLe)
            {
                int doiTacId = (int)tbl.Rows[0]["DoiTacId"];
                DateTime fromDate = (DateTime)tbl.Rows[0]["DateFrom"];

                DateTime tempDate = (DateTime)tbl.Rows[0]["DateEnd"];
                DateTime toDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59); // Thời điểm cuối ngày

                sql = "SELECT * FROM Report_PAKN_Data WHERE ReportId = @ReportId";
                DataTable tblDoiTac = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql, new SqlParameter("@ReportId", reportId)).Tables[0];

                List<int> listDoiTacId = null;
                if (doiTacId != DoiTacInfo.DoiTacIdValue.VNP)
                {
                    listDoiTacId = new List<int>();
                    for (int i = 0; i < tblDoiTac.Rows.Count; i++)
                    {
                        listDoiTacId.Add((int)tblDoiTac.Rows[i]["DoiTacId"]);
                    }
                }

                SolrQuery solrQuery = null;
                SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

                List<string> listGroupField = new List<string>();
                listGroupField.Add("KhieuNaiId");

                #region Số lượng tồn đọng kỳ trước
                QueryOptions queryOptionTonDongTruocKy = new QueryOptions();
                //Lấy ra những trường nào
                var extraParamTonDongTruocKy = new Dictionary<string, string>();
                extraParamTonDongTruocKy.Add("fl", @"KhieuNaiId, HanhDong, DoiTacXuLyId,PhongBanXuLyId");
                queryOptionTonDongTruocKy.ExtraParams = extraParamTonDongTruocKy;
                queryOptionTonDongTruocKy.Start = 0;
                queryOptionTonDongTruocKy.Rows = int.MaxValue;

                SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                SolrNet.SortOrder sortOrderTonDongTruocKyActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                List<SolrNet.SortOrder> listSortOrderTonDongTruocKy = new List<SolrNet.SortOrder>();
                listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyNgayTiepNhan);
                listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyActivityId);

                GroupingParameters gpTonDongKyTruoc = new GroupingParameters();
                gpTonDongKyTruoc.Fields = listGroupField;
                gpTonDongKyTruoc.Limit = 1;
                gpTonDongKyTruoc.Main = true;
                gpTonDongKyTruoc.OrderBy = listSortOrderTonDongTruocKy;
                queryOptionTonDongTruocKy.Grouping = gpTonDongKyTruoc;

                DateTime previousFromDate = fromDate.AddDays(-1);
                string whereClauseTonDongKyTruoc = string.Format("NgayTiepNhan:[* TO {0}] AND (KhieuNai_NgayDongKN : [{1} TO *])", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                solrQuery = new SolrQuery(whereClauseTonDongKyTruoc);

                string URL_SOLR_ACTIVITY = string.Concat(Config.ServerSolr, "Activity");

                listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDongTruocKy);

                Dictionary<int, int> listKhieuNaiIdTonDongKyTruoc = new Dictionary<int, int>();

                if (listKhieuNaiInfo != null)
                {
                    if (listDoiTacId != null && listDoiTacId.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(item => !listDoiTacId.Contains(item.DoiTacXuLyId));
                    }

                    foreach (DataRow rowDoiTac in tblDoiTac.Rows)
                    {
                        int Id = (int)rowDoiTac["Id"];
                        int curDoiTacXuLyId = ConvertUtility.ToInt32(rowDoiTac["DoiTacId"]);
                        if (curDoiTacXuLyId == 10000)
                        {
                            int sl = listKhieuNaiInfo.Count(p => p.PhongBanXuLyId == 60);
                            rowDoiTac["SLTonDongKyTruoc"] = listKhieuNaiInfo.Count(p => p.PhongBanXuLyId == 60);
                        }
                        else
                        {
                            rowDoiTac["SLTonDongKyTruoc"] = listKhieuNaiInfo.Count(p => p.DoiTacXuLyId == curDoiTacXuLyId);
                        }

                        int soLuongTonDongKyTruoc = (int)rowDoiTac["SLTonDongKyTruoc"];
                        // Cập nhật SQL
                        if (soLuongTonDongKyTruoc > 0)
                        {
                            sql = "UPDATE Report_PAKN_Data SET SLTonDongKyTruoc = " + soLuongTonDongKyTruoc + " WHERE Id = " + Id;
                            SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, sql);
                        }
                    }

                    for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    {
                        listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].DoiTacXuLyId);
                    }
                }
                #endregion

                #region Số lượng tồn đọng, tồn đọng quá hạn
                Dictionary<int, int> listKhieuNaiIdTonDong = new Dictionary<int, int>();

                QueryOptions queryOptionTonDong = new QueryOptions();
                //Lấy ra những trường nào
                var extraParamTonDong = new Dictionary<string, string>();
                extraParamTonDong.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, DoiTacXuLyId,PhongBanXuLyId");
                queryOptionTonDong.ExtraParams = extraParamTonDong;
                queryOptionTonDong.Start = 0;
                queryOptionTonDong.Rows = int.MaxValue;

                SolrNet.SortOrder sortOrderTonDongNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                SolrNet.SortOrder sortOrderTonDongActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                List<SolrNet.SortOrder> listSortOrderTonDong = new List<SolrNet.SortOrder>();
                listSortOrderTonDong.Add(sortOrderTonDongNgayTiepNhan);
                listSortOrderTonDong.Add(sortOrderTonDongActivityId);

                GroupingParameters gpTonDong = new GroupingParameters();
                gpTonDong.Fields = listGroupField;
                gpTonDong.Limit = 1;
                gpTonDong.Main = true;
                gpTonDong.OrderBy = listSortOrderTonDong;
                queryOptionTonDong.Grouping = gpTonDong;

                DateTime nextToDate = toDate.AddDays(1);
                string whereClauseTonDong = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                solrQuery = new SolrQuery(whereClauseTonDong);
                listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDong);

                if (listKhieuNaiInfo != null)
                {

                    if (listDoiTacId != null && listDoiTacId.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(item => !listDoiTacId.Contains(item.DoiTacXuLyId));
                    }

                    if (listKhieuNaiInfo != null)
                    {
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDong.Add(listKhieuNaiInfo[i].KhieuNaiId, listKhieuNaiInfo[i].DoiTacXuLyId);
                        }

                        foreach (DataRow rowDoiTac in tblDoiTac.Rows)
                        {
                            int id = (int)rowDoiTac["Id"];
                            int curDoiTacXuLyId = Convert.ToInt32(rowDoiTac["DoiTacId"]);
                            if (curDoiTacXuLyId == 10000)
                            {
                                int tondong = listKhieuNaiInfo.Count(p => p.PhongBanXuLyId == 60);
                                rowDoiTac["SLTonDong"] = tondong;

                                int quahantondong = listKhieuNaiInfo.Count(p => p.PhongBanXuLyId == 60 && p.NgayQuaHan <= toDate);
                                rowDoiTac["SLTonDongQuaHan"] = quahantondong;
                            }
                            else
                            {
                                int tondong = listKhieuNaiInfo.Count(p => p.DoiTacXuLyId == curDoiTacXuLyId);
                                rowDoiTac["SLTonDong"] = tondong;

                                int quahantondong = listKhieuNaiInfo.Count(p => p.DoiTacXuLyId == curDoiTacXuLyId && p.NgayQuaHan <= toDate);
                                rowDoiTac["SLTonDongQuaHan"] = quahantondong;
                            }

                            sql = string.Format("UPDATE Report_PAKN_Data SET SLTonDong = {0}, SLTonDongQuaHan = {1} WHERE Id = " + id, rowDoiTac["SLTonDong"], rowDoiTac["SLTonDongQuaHan"]);
                            SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, sql);
                        }
                    }
                }
                #endregion

                List<string> lstKhieuNaiTonDongKyTruoc = new List<string>();
                foreach (KeyValuePair<int, int> tonDongKyTruoc in listKhieuNaiIdTonDongKyTruoc)
                {
                    string obj = string.Join(",", tonDongKyTruoc.Key, tonDongKyTruoc.Value);
                    lstKhieuNaiTonDongKyTruoc.Add(obj);
                }
                string strTonDongKyTruoc = string.Join(";", lstKhieuNaiTonDongKyTruoc);


                List<string> lstKhieuNaiIdTonDong = new List<string>();
                foreach (KeyValuePair<int, int> khieuNaiIdTonDong in listKhieuNaiIdTonDong)
                {
                    string obj = string.Join(",", khieuNaiIdTonDong.Key, khieuNaiIdTonDong.Value);
                    lstKhieuNaiIdTonDong.Add(obj);
                }
                string strKhieuNaiIdTonDong = string.Join(";", lstKhieuNaiIdTonDong);

                sql = string.Format("UPDATE Report_PAKN_Main SET TonDongKyTruoc = @TonDongKyTruoc, KhieuNaiIdTonDong = @KhieuNaiIdTonDong WHERE Id = {0}", reportId);
                SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, sql, new SqlParameter("@TonDongKyTruoc", strTonDongKyTruoc), new SqlParameter("@KhieuNaiIdTonDong", strKhieuNaiIdTonDong));

                var result = new
                {
                    ErrorCode = 1,
                    Message = "Thành công",
                };
                return new JavaScriptSerializer().Serialize(result);
            }
            return new JavaScriptSerializer().Serialize(new { ErrorCode = 0, Message = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại" });
        }
        [WebMethod]
        public static string CalEveryPartner(int reportId, string securityCode, int doiTacId)
        {
            bool isHopLe = true;
            string sql = "SELECT * FROM Report_PAKN_Main WHERE Id = @Id AND SecurityKey = @SecurityKey";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Id", reportId),
                new SqlParameter("@SecurityKey", securityCode),
            };

            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql, prms).Tables[0];
            if (tbl != null && tbl.Rows.Count > 0) isHopLe = true; else isHopLe = false;
            if (isHopLe)
            {
                sql = string.Format("SELECT * FROM Report_PAKN_Data WHERE ReportId = {0} AND DoiTacId = {1}", reportId, doiTacId);
                DataTable tblDoiTac = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql).Tables[0];
                if (tblDoiTac != null && tblDoiTac.Rows.Count > 0)
                {
                    DataRow row = tblDoiTac.Rows[0];

                    int id = (int)tblDoiTac.Rows[0]["Id"];

                    DateTime fromDate = (DateTime)tbl.Rows[0]["DateFrom"]; // Ngày bắt đầu

                    DateTime tempDate = (DateTime)tbl.Rows[0]["DateEnd"]; // Ngày kết thúc
                    DateTime toDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59); // Thời điểm cuối ngày

                    // Tính toán tồn đọng kỳ trước
                    Dictionary<int, int> listKhieuNaiIdTonDongKyTruoc = new Dictionary<int, int>();
                    string strTonDongKyTruong = (string)tbl.Rows[0]["TonDongKyTruoc"];
                    if (!string.IsNullOrEmpty(strTonDongKyTruong))
                    {
                        string[] val = strTonDongKyTruong.Split(';');

                        foreach (string item in val)
                        {
                            string[] retTemp = item.Split(',');
                            int key = Convert.ToInt32(retTemp[0]);
                            int value = Convert.ToInt32(retTemp[1]);
                            listKhieuNaiIdTonDongKyTruoc.Add(key, value);
                        }
                    }
                    // Tinh toán khiếu nại Id tồn đọng
                    Dictionary<int, int> listKhieuNaiIdTonDong = new Dictionary<int, int>();
                    string strKhieuNaiIdTonDong = (string)tbl.Rows[0]["KhieuNaiIdTonDong"];
                    if (!string.IsNullOrEmpty(strKhieuNaiIdTonDong))
                    {
                        string[] val1 = strKhieuNaiIdTonDong.Split(';');
                        foreach (string item in val1)
                        {
                            string[] retTemp = item.Split(',');
                            int key = Convert.ToInt32(retTemp[0]);
                            int value = Convert.ToInt32(retTemp[1]);
                            listKhieuNaiIdTonDong.Add(key, value);
                        }
                    }

                    int curDoiTacXuLyId = ConvertUtility.ToInt32(tblDoiTac.Rows[0]["DoiTacId"]);

                    // Số lượng tiếp nhận
                    QueryOptions queryOptionTiepNhan = new QueryOptions();
                    //Lấy ra những trường nào
                    var extraParamLoaiKhieuNai = new Dictionary<string, string>();
                    extraParamLoaiKhieuNai.Add("fl", @"KhieuNaiId");
                    queryOptionTiepNhan.ExtraParams = extraParamLoaiKhieuNai;
                    queryOptionTiepNhan.Start = 0;
                    queryOptionTiepNhan.Rows = int.MaxValue;

                    List<string> listGroupField = new List<string>();
                    listGroupField.Add("KhieuNaiId");

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    queryOptionTiepNhan.Grouping = gpTiepNhan;
                    string whereClauseTiepNhan = "";
                    if (curDoiTacXuLyId == 10000) // Phòng CSKH
                    {
                        whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId: {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), 60);
                    }
                    else
                    {
                        whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId: {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), curDoiTacXuLyId);
                    }

                    string URL_SOLR_ACTIVITY = string.Concat(Config.ServerSolr, "Activity");

                    SolrQuery solrQuery = new SolrQuery(whereClauseTiepNhan);
                    SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTiepNhan);

                    if (listKhieuNaiInfo != null)
                    {
                        if (listKhieuNaiIdTonDongKyTruoc.Count > 0)
                        {
                            //listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(new KeyValuePair<int, int>(obj.KhieuNaiId, curDoiTacXuLyId)); });
                            listKhieuNaiInfo.RemoveAll(
                                item =>
                                    listKhieuNaiIdTonDongKyTruoc.Contains(new KeyValuePair<int, int>(item.KhieuNaiId, curDoiTacXuLyId)));
                        }

                        tblDoiTac.Rows[0]["SLTiepNhan"] = listKhieuNaiInfo.Count;
                    }

                    // Số lượng đã xử lý
                    // Số lượng quá hạn đã xử lý
                    QueryOptions queryOptionXuLy = new QueryOptions();
                    //Lấy ra những trường nào
                    var extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, NgayQuaHan_PhongBanXuLyTruoc, LDate, HanhDong");
                    queryOptionXuLy.ExtraParams = extraParamXuLy;
                    queryOptionXuLy.Start = 0;
                    queryOptionXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters
                    {
                        Fields = listGroupField,
                        Limit = 1,
                        Main = true,
                        OrderBy = listSortOrderNgayXuLy
                    };
                    queryOptionXuLy.Grouping = gpXuLy;
                    string whereClauseXuLy = "";
                    //string whereClauseXuLy = string.Format("NgayTiepNhan:[{0} TO {1}] AND ((DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2}) OR (DoiTacXuLyId:{2} AND HanhDong:4))", ConvertDateToSolr(fromDate.ToString("yyyyMMdd"), true, false), ConvertDateToSolr(toDate.ToString("yyyyMMdd"), false, true), doiTacId);
                    if (curDoiTacXuLyId == 10000)
                    {
                        whereClauseXuLy = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", new ReportImpl().ConvertDateToSolr(fromDate.ToString("yyyyMMdd"), true, false), new ReportImpl().ConvertDateToSolr(toDate.ToString("yyyyMMdd"), false, true), 60);
                    }
                    else
                    {
                        whereClauseXuLy = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", new ReportImpl().ConvertDateToSolr(fromDate.ToString("yyyyMMdd"), true, false), new ReportImpl().ConvertDateToSolr(toDate.ToString("yyyyMMdd"), false, true), curDoiTacXuLyId);
                    }

                    solrQuery = new SolrQuery(whereClauseXuLy);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionXuLy);

                    if (listKhieuNaiInfo != null)
                    {
                        if (listKhieuNaiIdTonDong != null && listKhieuNaiIdTonDong.Count > 0)
                        {
                            listKhieuNaiInfo.RemoveAll(item => listKhieuNaiIdTonDong.Contains(new KeyValuePair<int, int>(item.KhieuNaiId, curDoiTacXuLyId)));
                        }
                        row["SLDaXuLy"] = listKhieuNaiInfo.Count;

                        row["SLQuaHanDaXuLy"] = listKhieuNaiInfo.Count(p => p.NgayTiepNhan > p.NgayQuaHan_PhongBanXuLyTruoc);

                        row["SLChuyenXuLy"] = listKhieuNaiInfo.Count(p => p.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban);

                        row["SLChuyenPhanHoi"] = listKhieuNaiInfo.Count(p => p.HanhDong == (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi);

                        row["SLChuyenNgangHang"] = listKhieuNaiInfo.Count(p => p.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng);

                    }
                    // Số lượng tạo mới                    
                    QueryOptions queryOptionTaoMoi = new QueryOptions();
                    //Lấy ra những trường nào
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();

                    // Điểu khiện
                    string whereClauseTaoMoi = string.Empty;

                    extraParamTaoMoi.Add("fl", @"Id");
                    queryOptionTaoMoi.ExtraParams = extraParamTaoMoi;
                    queryOptionTaoMoi.Start = 0;
                    queryOptionTaoMoi.Rows = int.MaxValue;

                    queryOptionTaoMoi.Grouping = new GroupingParameters()
                    {
                        Fields = new List<string>() { "KhieuNaiId" },
                        Limit = 1,
                        Main = true,
                        OrderBy = new List<SolrNet.SortOrder>() { new SolrNet.SortOrder("NgayTiepNhan", Order.ASC) }
                    };

                    if (curDoiTacXuLyId == 10000)
                    {
                        whereClauseTaoMoi = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong : 0 AND ActivityTruoc : 0",
                            fromDate.StartOfDay().FormatSolrDateTime(),
                           toDate.EndOfDay().FormatSolrDateTime(), 60);
                    }
                    else
                    {
                        whereClauseTaoMoi = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacId : {2} AND HanhDong : 0 AND ActivityTruoc : 0",
                            fromDate.StartOfDay().FormatSolrDateTime(),
                            toDate.EndOfDay().FormatSolrDateTime(),
                            curDoiTacXuLyId);
                    }


                    solrQuery = new SolrQuery(whereClauseTaoMoi);

                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(string.Concat(Config.ServerSolr, "Activity"), solrQuery, queryOptionTaoMoi);

                    row["SLTaoMoi"] = listKhieuNaiInfo != null ? listKhieuNaiInfo.Count : 0;

                    // Số lượng tạo đã đóng                   
                    QueryOptions queryOptionDaDong = new QueryOptions();
                    //Lấy ra những trường nào
                    var extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", @"Id");
                    queryOptionDaDong.ExtraParams = extraParamDaDong;
                    queryOptionDaDong.Start = 0;
                    queryOptionDaDong.Rows = int.MaxValue;
                    string whereClauseDaDong = "";
                    if (curDoiTacXuLyId == 10000)
                    {
                        whereClauseDaDong = string.Format("NgayDongKN:[{0} TO {1}] AND PhongBanXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), 60);
                    }
                    else
                    {
                        whereClauseDaDong = string.Format("NgayDongKN:[{0} TO {1}] AND DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), curDoiTacXuLyId);
                    }
                    solrQuery = new SolrQuery(whereClauseDaDong);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(string.Concat(Config.ServerSolr, "GQKN"), solrQuery, queryOptionDaDong);

                    row["SLDaDong"] = listKhieuNaiInfo != null ? listKhieuNaiInfo.Count : 0;

                    sql = string.Format(@"UPDATE Report_PAKN_Data SET 
                                            SLTiepNhan = {0}, 
                                            SLTaoMoi = {1}, 
                                            SLDaXuLy = {2}, 
                                            SLChuyenXuLy = {3}, 
                                            SLChuyenPhanHoi = {4},  
                                            SLDaDong = {5}, 
                                            SLQuaHanDaXuLy = {6},  
                                            SLChuyenNgangHang = {7} WHERE Id = {8}",
                                           row["SLTiepNhan"],
                                           row["SLTaoMoi"],
                                           row["SLDaXuLy"],
                                           row["SLChuyenXuLy"],
                                           row["SLChuyenPhanHoi"],
                                           row["SLDaDong"],
                                           row["SLQuaHanDaXuLy"],
                                           row["SLChuyenNgangHang"],
                                           row["Id"]);
                    SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.Text, sql);
                    return new JavaScriptSerializer().Serialize(new { ErrorCode = 1, Message = "Thành công" });
                }
            }
            return new JavaScriptSerializer().Serialize(new { ErrorCode = 0, Message = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại" });
        }
        [WebMethod]
        public static string CalExportReport(int reportId, string securityCode)
        {
            string fullName = string.Empty;
            string html = RenderBodyReport(reportId, securityCode, out fullName);

            JavaScriptSerializer jsJson = new JavaScriptSerializer();
            jsJson.MaxJsonLength = int.MaxValue; // Không giới hạn

            if (html != string.Empty) return jsJson.Serialize(new { ErrorCode = 1, Message = "Thành công", Value = RenderReportFooter(RenderTableTag(html), fullName) }); // Trả dữ liệu về cho Client
            return jsJson.Serialize(new { ErrorCode = 0, Message = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại" });
        }
        private static string RenderBodyReport(int reportId, string securityCode, out string tenDayDu)
        {
            tenDayDu = string.Empty;
            string sql = "SELECT * FROM Report_PAKN_Main WHERE Id = @Id AND SecurityKey = @SecurityKey";
            DataTable tblMain = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql, new SqlParameter("@Id", reportId), new SqlParameter("@SecurityKey", securityCode)).Tables[0];

            if (tblMain != null && tblMain.Rows.Count > 0)
            {
                tenDayDu = (string)tblMain.Rows[0]["FullName"];
                string fromDate = ((DateTime)tblMain.Rows[0]["DateFrom"]).ToString("dd/MM/yyyy");
                string toDate = ((DateTime)tblMain.Rows[0]["DateEnd"]).ToString("dd/MM/yyyy");

                sql = "SELECT * FROM Report_PAKN_Data WHERE ReportId = " + reportId;
                DataTable tblData = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql).Tables[0];
                if (tblData != null && tblData.Rows.Count > 0)
                {
                    #region Xử lý
                    int minLevel = 1;
                    minLevel = tblData.AsEnumerable().Select(v => v.Field<int>("Level")).Distinct().Min(); // Lấy ra danh sách Level

                    EnumerableRowCollection<DataRow> dsCap1 = tblData.AsEnumerable().Where(v => v.Field<int>("Level") == minLevel);
                    int i = 0;
                    string strRow = string.Empty;

                    foreach (DataRow r1 in dsCap1)
                    {
                        #region Level1
                        i += 1;
                        strRow += "<tr>";
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", i);
                        strRow += string.Format("<td>{0}</td>", r1["TenDoiTac"]);
                        strRow += string.Format("<td>{0}</td>", r1["DoiTacCap1"]);

                        strRow += string.Format("<td>{0}</td>", r1["DoiTacCap2"]);
                        strRow += string.Format("<td>{0}</td>", r1["DoiTacCap3"]);

                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLTonDongKyTruoc"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 1));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLTiepNhan"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 2));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLTaoMoi"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 7));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLDaXuLy"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 3));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLChuyenXuLy"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 10));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLChuyenPhanHoi"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 11));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLDaDong"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 8));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLQuaHanDaXuLy"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 4));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLTonDong"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 5));
                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r1["SLTonDongQuaHan"], string.Empty, (int)r1["DoiTacId"], fromDate, toDate, 6));
                        strRow += "</tr>";
                        #endregion

                        // Tìm kiếm con của nó
                        EnumerableRowCollection<DataRow> dsCap2 = tblData.AsEnumerable().Where(v => v.Field<int>("ParentDoiTacId") == (int)r1["DoiTacId"]);
                        if (dsCap2 != null)
                        {
                            foreach (DataRow r2 in dsCap2)
                            {
                                #region Level2
                                i += 1;
                                strRow += "<tr>";
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", i);
                                strRow += string.Format("<td>{0}</td>", r2["TenDoiTac"]);
                                strRow += string.Format("<td>{0}</td>", r2["DoiTacCap1"]);

                                strRow += string.Format("<td>{0}</td>", r2["DoiTacCap2"]);
                                strRow += string.Format("<td>{0}</td>", r2["DoiTacCap3"]);

                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLTonDongKyTruoc"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 1));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLTiepNhan"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 2));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLTaoMoi"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 7));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLDaXuLy"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 3));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLChuyenXuLy"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 10));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLChuyenPhanHoi"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 11));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLDaDong"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 8));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLQuaHanDaXuLy"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 4));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLTonDong"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 5));
                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r2["SLTonDongQuaHan"], string.Empty, (int)r2["DoiTacId"], fromDate, toDate, 6));
                                strRow += "</tr>";
                                #endregion

                                // Tìm kiếm con nó => Danh sách cấp 3
                                EnumerableRowCollection<DataRow> dsCap3 = tblData.AsEnumerable().Where(v => v.Field<int>("ParentDoiTacId") == (int)r2["DoiTacId"]);
                                if (dsCap3 != null)
                                {
                                    foreach (DataRow r3 in dsCap3)
                                    {
                                        #region Level3
                                        i += 1;
                                        strRow += "<tr>";
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", i);
                                        strRow += string.Format("<td>{0}</td>", r3["TenDoiTac"]);
                                        strRow += string.Format("<td>{0}</td>", r3["DoiTacCap1"]);

                                        strRow += string.Format("<td>{0}</td>", r3["DoiTacCap2"]);
                                        strRow += string.Format("<td>{0}</td>", r3["DoiTacCap3"]);

                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLTonDongKyTruoc"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 1));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLTiepNhan"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 2));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLTaoMoi"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 7));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLDaXuLy"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 3));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLChuyenXuLy"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 10));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLChuyenPhanHoi"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 11));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLDaDong"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 8));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLQuaHanDaXuLy"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 4));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLTonDong"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 5));
                                        strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r3["SLTonDongQuaHan"], string.Empty, (int)r3["DoiTacId"], fromDate, toDate, 6));
                                        strRow += "</tr>";
                                        #endregion

                                        // Tìm kiếm con nó => Danh sách cấp 4 => Kết thúc
                                        EnumerableRowCollection<DataRow> dsCap4 = tblData.AsEnumerable().Where(v => v.Field<int>("ParentDoiTacId") == (int)r3["DoiTacId"]);
                                        if (dsCap4 != null)
                                        {
                                            foreach (DataRow r4 in dsCap4)
                                            {
                                                #region Level 4
                                                i += 1;
                                                strRow += "<tr>";
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", i);
                                                strRow += string.Format("<td>{0}</td>", r4["TenDoiTac"]);
                                                strRow += string.Format("<td>{0}</td>", r4["DoiTacCap1"]);

                                                strRow += string.Format("<td>{0}</td>", r4["DoiTacCap2"]);
                                                strRow += string.Format("<td>{0}</td>", r4["DoiTacCap3"]);

                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLTonDongKyTruoc"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 1));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLTiepNhan"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 2));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLTaoMoi"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 7));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLDaXuLy"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 3));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLChuyenXuLy"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 10));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLChuyenPhanHoi"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 11));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLDaDong"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 8));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLQuaHanDaXuLy"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 4));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLTonDong"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 5));
                                                strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", ProcessNumber(r4["SLTonDongQuaHan"], string.Empty, (int)r4["DoiTacId"], fromDate, toDate, 6));
                                                strRow += "</tr>";
                                                #endregion
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    #endregion

                    // Tính toán tổng

                    sql = @"SELECT 
	                            SUM(SLTonDongKyTruoc) SLTonDongKyTruoc, 
	                            SUM(SLTiepNhan) SLTiepNhan,
	                            SUM(SLTaoMoi) SLTaoMoi,
	                            SUM(SLDaXuLy) SLDaXuLy,
	                            SUM(SLChuyenXuLy) SLChuyenXuLy,
	                            SUM(SLChuyenPhanHoi) SLChuyenPhanHoi,
	                            SUM(SLDaDong) SLDaDong,
	                            SUM(SLQuaHanDaXuLy) SLQuaHanDaXuLy,
	                            SUM(SLTonDong) SLTonDong,
	                            SUM(SLTonDongQuaHan) SLTonDongQuaHan
                            FROM Report_PAKN_Data
                            WHERE ReportId = @ReportId";
                    DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql, new SqlParameter("@ReportId", reportId)).Tables[0];

                    i += 1;
                    strRow += "<tr>";
                    strRow += string.Format("<td style=\"text-align: center;\">{0}</td>", i);
                    strRow += string.Format("<td colspan=\"4\" style=\"font-weight:bold;\">{0}</td>", "Tổng cộng");

                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTonDongKyTruoc"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTiepNhan"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTaoMoi"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTaoMoi"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLChuyenXuLy"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLChuyenPhanHoi"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLDaDong"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLQuaHanDaXuLy"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTonDong"], string.Empty));
                    strRow += string.Format("<td style=\"text-align: center; font-weight:bold;\">{0}</td>", ProcessNumber(tbl.Rows[0]["SLTonDongQuaHan"], string.Empty));
                    strRow += "</tr>";

                    return strRow;
                }
            }
            return string.Empty;
        }
        private static string RenderTableTag(string body)
        {
            string tableHtml = string.Empty;
            tableHtml = "<table border = '1' style = 'border-collapse: collapse;' class='tbl_style tblcus'>";
            tableHtml += "<tbody>";
            tableHtml += "<tr>";
            tableHtml += "<th rowspan = '2' > STT </ th>";
            tableHtml += "<th rowspan='2'>Khu vực</th>";
            tableHtml += "<th rowspan = '2' > Đối tác cấp 1</th>";
            tableHtml += "<th rowspan = '2' > Đối tác cấp 2</th>";
            tableHtml += "<th rowspan = '2' > Đối tác cấp 3</th>";
            tableHtml += "<th rowspan = '2' > SL tồn kỳ trước</ br>";
            tableHtml += "[1]</th>";
            tableHtml += "<th colspan = '2' > Số lượng tiếp nhận</th>";
            tableHtml += "<th colspan = '5' > Số lượng đã xử lý</th>";
            tableHtml += "<th rowspan = '2' > Số lượng tồn đọng</ br>";
            tableHtml += "[4]</th>";
            tableHtml += "<th rowspan = '2' > Số lượng tồn đọng quá hạn</ br>";
            tableHtml += "[4.1]</th>";
            tableHtml += "</tr>";
            tableHtml += "<tr>";
            tableHtml += "<th>SL tiếp nhận</ br>";
            tableHtml += "[2]</th>";
            tableHtml += "<th>SL tạo mới</ br>";
            tableHtml += "[2.1]</th>";
            tableHtml += "<th>SL đã xử lý";
            tableHtml += "</ br>";
            tableHtml += "[3] = [3.1] + [3.2] + [3.3]</th>";
            tableHtml += "<th>SL chuyển xử lý";
            tableHtml += "</ br>";
            tableHtml += "[3.1]</th>";
            tableHtml += "<th>SL chuyển phản hồi";
            tableHtml += "</ br>";
            tableHtml += "[3.2]</th>";
            tableHtml += "<th>SL đã đóng</ br>";
            tableHtml += "[3.3]</th>";
            tableHtml += "<th>Số lượng đã xử lý quá hạn</ br>";
            tableHtml += "[3.4]</th>";
            tableHtml += "</tr>";
            tableHtml += body;
            tableHtml += "</tbody>";
            tableHtml += "</table>";

            return tableHtml;
        }
        private static string RenderReportHeader(string body, string tuNgay, string denNgay)
        {
            string strTemp = string.Empty;
            strTemp += "<table border = '0' style = 'width: 100%'>";
            strTemp += "<tbody>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15' style='text-align: center' >";
            strTemp += "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
            strTemp += "<br>";
            strTemp += "Độc lập - Tự do -Hạnh phúc";
            strTemp += "</td>";
            strTemp += "</tr>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15'></td>";
            strTemp += "</tr>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15' style='text-align: center'>";
            strTemp += "<h1> BÁO CÁO TỔNG HỢP SỐ LIỆU PAKN</h1> ";
            strTemp += " </td> ";
            strTemp += "</tr>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15' style='text-align: center'>";
            strTemp += string.Format("<b> Từ ngày:</b> {0} <b> Đến ngày:</b> {1}", tuNgay, denNgay);
            strTemp += "</td>";
            strTemp += "</tr>";
            strTemp += "<tr colspan = '15' >";
            strTemp += "<td> &nbsp;</td>";
            strTemp += "</tr>";
            strTemp += "</tbody>";
            strTemp += "</table>";
            strTemp += body; // Đặt nội dung chèn ở đây

            return strTemp;
        }
        private static string RenderReportFooter(string body, string nguoiky)
        {
            string strTemp = body;
            strTemp += "<table width = '100%' border = '0'>";
            strTemp += "<tbody>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15' style='height: 15px;' ></td>";
            strTemp += "</tr>";
            strTemp += "<tr valign = 'top'>";
            strTemp += "<td colspan = '15' style = 'text-align: center; font-weight: bold; border: 0px'> Người báo cáo";
            strTemp += "<br><br><br><br>";
            strTemp += nguoiky;
            strTemp += "</td>";
            strTemp += "</tr>";
            strTemp += "<tr>";
            strTemp += "<td colspan = '15' >";
            strTemp += "<b><i> Giải thích </ i></b>";
            strTemp += "<br>";
            strTemp += "[1] : Số lượng khiếu nại tồn tính đến trước ngày thực hiện báo cáo";
            strTemp += "<br>";
            strTemp += "[2] : Số lượng khiếu nại tiếp nhận (khiếu nại từ nơi khác chuyển đến + tạo mới) ngoại trừ các khiếu nại tồn đọng từ kỳ trước";
            strTemp += "<br>";
            strTemp += "[2.1] : Số lượng khiếu nại do người dùng tạo ra";
            strTemp += "<br>";
            strTemp += "[3] = [3.1] + [3.2] + [3.3] : Tổng số khiếu nại đã được xử lý (chuyển ngang hàng / chuyển xử lý / chuyển phản hồi / đóng khiếu nại)    ";
            strTemp += "";
            strTemp += "<br>";
            strTemp += "[3.1] : Số lượng khiếu nại đã được chuyển xử lý";
            strTemp += "<br>";
            strTemp += "[3.2] : Số lượng khiếu nại đã được chuyển chuyển phản hồi";
            strTemp += "<br>";
            strTemp += "[3.3] : Số lượng khiếu nại người dùng đã đóng";
            strTemp += "<br>";
            strTemp += "[3.4] : Số lượng khiếu nại đã được xử lý nhưng quá hạn phòng ban quy định";
            strTemp += "<br>";
            strTemp += "[4] : Số lượng khiếu nại tồn tính tới thời điểm cuối cùng thực hiện báo cáo";
            strTemp += "<br>";
            strTemp += "[4.1] : Số lượng khiếu nại tồn quá hạn tính tới thời điểm cuối cùng thực hiện báo cáo";
            strTemp += "</td>";
            strTemp += "</tr>";
            strTemp += "</tbody>";
            strTemp += " </table>";

            return strTemp;
        }
        private static object ProcessNumber(object obj, string strReplace, int doiTacId, string fromDate, string toDate, int rptTypeId)
        {
            if (obj == null) return strReplace; ;
            if (obj.ToString() == string.Empty || Convert.ToInt32(obj) == 0) return strReplace;
            return string.Format("<a onclick=\"window.open(url='/Views/BaoCao/Popup/DanhSachKhieuNai.aspx?FromPage=BaoCaoTongHopPaknDoiTac&amp;DoiTacId={1}&amp;FromDate={2}&amp;ToDate={3}&amp;ReportType={4}','_blank', 'width=990, height=550,scrollbars=1,location=0'); return false;\" href=\"#\">{0}</a>", string.Format("<span class=\"number\">{0}</span>", obj), doiTacId, fromDate, toDate, rptTypeId);
        }
        private static object ProcessNumber(object obj, string strReplace)
        {
            if (obj == null) return strReplace; ;
            if (obj.ToString() == string.Empty || Convert.ToInt32(obj) == 0) return strReplace;
            return string.Format("<span class=\"number\">{0}</span>", obj);
        }
    }
}

