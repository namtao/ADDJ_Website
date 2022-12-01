using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.Core;
using System.Globalization;
using SolrNet;
using AIVietNam.GQKN.Impl;
using SolrNet.Commands.Parameters;
using AIVietNam.GQKN.Entity;
using System.Data;

namespace Website.BaoCao
{
    public partial class BaoCaoTongHopVNPTNET : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExport.Click += BtnExport_Click;
            if (!IsPostBack)
            {
                bool paramsIsOK = false;
                txtFromDate.Text = DateTime.Now.StartOfMonth().ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["FromDate"])
                        && !string.IsNullOrEmpty(Request.QueryString["ToDate"])
                        && !string.IsNullOrEmpty(Request.QueryString["DoiTacId"]))
                    {
                        txtFromDate.Text = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["FromDate"]), new CultureInfo("vi-VN")).ToString("dd/MM/yyyy");
                        txtToDate.Text = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["ToDate"]), new CultureInfo("vi-VN")).ToString("dd/MM/yyyy");
                        ddlDoiTac.SelectedValue = Request.QueryString["DoiTacId"];

                        if (!string.IsNullOrEmpty(Request.QueryString["PhongBanId"]))
                        {
                            // Giá trị phong ban
                            ddlPhongBan.SelectedValue = Request.QueryString["PhongBanId"];
                        }

                        paramsIsOK = true;
                    }
                }
                catch { }
                if (paramsIsOK)
                {
                    BindReport();
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int donViId = Convert.ToInt32(ddlDoiTac.SelectedValue);
                int phongBanId = 0;
                string tmpPhongBan = string.Empty;
                if (int.TryParse(ddlPhongBan.SelectedValue, out phongBanId))
                {
                    if (phongBanId > 0)
                    {
                        tmpPhongBan = string.Format("&PhongBanId={0}", phongBanId);
                    }
                }

                string url = string.Format("BaoCaoTongHopVNPTNET.aspx?DoiTacId={0}&FromDate={1}&ToDate={2}{3}", ddlDoiTac.SelectedValue, HttpUtility.UrlEncode(txtFromDate.Text), HttpUtility.UrlEncode(txtToDate.Text), tmpPhongBan);
                Response.Redirect(url);
            }
        }

        protected void BindReport()
        {
            int doiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
            int phongBanId = 0;
            int.TryParse(ddlPhongBan.SelectedValue, out phongBanId);

            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text, new CultureInfo("vi-VN"));
            DateTime toDate = Convert.ToDateTime(txtToDate.Text, new CultureInfo("vi-VN"));


            DataTable tbl = BaoCaoTongHopPAKNTheoVNPTX_Solr(doiTacId, fromDate, toDate);

            string reportType = "BaoCaoTongHopVNPTNET";
            string keyRandom = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);

            HttpCookie skeyClient = new HttpCookie("TokenKey");
            skeyClient.Value = AIVietNam.Sercurity.Encrypt.MD5(keyRandom);
            skeyClient.HttpOnly = true;
            skeyClient.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(skeyClient); // Quẳng xuống client để đối chiếu

            EnumerableRowCollection objData = tbl.AsEnumerable().Select(v => new
            {
                DoiTacId = v.Field<object>("DoiTacId"),
                TenDoiTac = v.Field<object>("TenDoiTac"),

                // Số lượng tồn đọng kỳ trước
                SLTonDongKyTruoc = string.Empty.GetData<string>(() =>
                {
                    string reportName = "SLTonDongKyTruoc";
                    return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                        v.Field<object>("SLTonDongKyTruoc"),
                        string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                            reportType,
                            reportName,
                            doiTacId,
                            HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                            HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                            keyRandom
                            ));
                }),

                // Số lượng tiếp nhận
                SLTiepNhan = string.Empty.GetData<string>(() =>
                {
                    string reportName = "SLTiepNhan";
                    return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                        v.Field<object>("SLTiepNhan"),
                        string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                            reportType,
                            reportName,
                            doiTacId,
                            HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                            HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                            keyRandom
                            ));
                }),

                SLTiepNhanTrongKy = v.Field<object>("SLTiepNhanTrongKy"),

                // Số lượng đã xử lý tiếp nhận
                SLDaXuLyTiepNhan = string.Empty.GetData<string>(() =>
                {
                    string reportName = "SLDaXuLyTiepNhan";
                    return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                        v.Field<object>("SLDaXuLyTiepNhan"),
                        string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                            reportType,
                            reportName,
                            doiTacId,
                            HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                            HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                            keyRandom
                            ));
                }),

                // SL đã xử lý (lũy kế)
                SLDaXuLyLuyKe = string.Empty.GetData<string>(() =>
                {
                    string reportName = "SLDaXuLyLuyKe";
                    return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                        v.Field<object>("SLDaXuLyLuyKe"),
                        string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                            reportType,
                            reportName,
                            doiTacId,
                            HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                            HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                            keyRandom
                            ));
                }),

                SLQuaHanDaXuLy = string.Empty.GetData<string>(() =>
               {
                   string reportName = "SLQuaHanDaXuLy";
                   return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                       v.Field<object>("SLQuaHanDaXuLy"),
                       string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                           reportType,
                           reportName,
                           doiTacId,
                           HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                           HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                           keyRandom
                           ));
               }),

                // Số lượng tồn đọng
                SLTonDong = string.Empty.GetData<string>(() =>
                {
                    string reportName = "SLTonDong";
                    return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                        v.Field<object>("SLTonDong"),
                        string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                            reportType,
                            reportName,
                            doiTacId,
                            HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                            HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                            keyRandom
                            ));
                }),

                // Số lượng tồn đọng quá hạn
                SLQuaHanTonDong = string.Empty.GetData<string>(() =>
               {
                   string reportName = "SLTonDongQuaHan";
                   return string.Format("<a href=\"{1}\"><span class=\"number\">{0}</span></a>",
                       v.Field<object>("SLQuaHanTonDong"),
                       string.Format("PopupDetail.aspx?Type={0}&ReportName={1}&DoiTacId={2}&FromDate={3}&ToDate={4}&Skey={5}",
                           reportType,
                           reportName,
                           doiTacId,
                           HttpUtility.UrlEncode(fromDate.ToString("dd/MM/yyyy")),
                           HttpUtility.UrlEncode(toDate.ToString("dd/MM/yyyy")),
                           keyRandom
                           ));
               }),
                SLTaoMoi = v.Field<object>("SLTaoMoi"),
                SLDaDong = v.Field<object>("SLDaDong"),

                SLChuyenXuLy = v.Field<object>("SLChuyenXuLy"),
                SLChuyenPhanHoi = v.Field<object>("SLChuyenPhanHoi"),
                SLChuyenNgangHang = v.Field<object>("SLChuyenNgangHang"),


            });

            GrvView.DataSource = objData;
            GrvView.DataBind();
        }

        public string ConvertDateToSolr(string date, bool isStart, bool isEnd)
        {
            string sDate = string.Empty;
            sDate = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));

            if (isStart)
            {
                sDate = string.Format("{0}T00:00:00.00Z", sDate);
            }
            else if (isEnd)
            {
                sDate = string.Format("{0}T23:59:59.999Z", sDate);
            }
            else
            {
                sDate = string.Format("{0}T00:00:00.00Z", sDate);
            }

            return sDate;
        }
        public DataTable BaoCaoTongHopPAKNTheoVNPTX_Solr(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            string URL_SOLR_GQKN = string.Concat(Config.ServerSolr, "GQKN");
            string URL_SOLR_ACTIVITY = string.Concat(Config.ServerSolr, "Activity");


            SolrQuery solrQuery = null;
            SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            DoiTacInfo doiTacInfo = new DoiTacImpl().GetInfo(doiTacId);

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DataTable dtDoiTac = new DataTable();
            dtDoiTac.Columns.Add("DoiTacId");
            dtDoiTac.Columns.Add("TenDoiTac");
            dtDoiTac.Columns.Add("SLTonDongKyTruoc");
            dtDoiTac.Columns.Add("SLTiepNhan");
            dtDoiTac.Columns.Add("SLTiepNhanTrongKy");
            dtDoiTac.Columns.Add("SLDaXuLyTiepNhan");
            dtDoiTac.Columns.Add("SLDaXuLyLuyKe");
            dtDoiTac.Columns.Add("SLQuaHanDaXuLy");
            dtDoiTac.Columns.Add("SLTonDong");
            dtDoiTac.Columns.Add("SLQuaHanTonDong");
            dtDoiTac.Columns.Add("SLTaoMoi");
            dtDoiTac.Columns.Add("SLDaDong");
            dtDoiTac.Columns.Add("SLChuyenXuLy");
            dtDoiTac.Columns.Add("SLChuyenPhanHoi");
            dtDoiTac.Columns.Add("SLChuyenNgangHang");

            DataRow row = dtDoiTac.NewRow();
            row["DoiTacId"] = doiTacId;
            row["TenDoiTac"] = doiTacInfo != null ? doiTacInfo.TenDoiTac : string.Empty;
            row["SLTonDongKyTruoc"] = 0;
            row["SLTiepNhan"] = 0;
            row["SLTiepNhanTrongKy"] = 0;
            row["SLDaXuLyTiepNhan"] = 0;
            row["SLDaXuLyLuyKe"] = 0;
            row["SLQuaHanDaXuLy"] = 0;
            row["SLTonDong"] = 0;
            row["SLQuaHanTonDong"] = 0;
            row["SLTaoMoi"] = 0;
            row["SLDaDong"] = 0;
            row["SLChuyenXuLy"] = 0;
            row["SLChuyenPhanHoi"] = 0;
            row["SLChuyenNgangHang"] = 0;
            dtDoiTac.Rows.Add(row);

            // Số lượng tồn đọng kỳ trước
            QueryOptions queryOptionTonDongTruocKy = new QueryOptions();
            //Lấy ra những trường nào
            var extraParamTonDongTruocKy = new Dictionary<string, string>();
            extraParamTonDongTruocKy.Add("fl", @"KhieuNaiId, HanhDong, KhuVucXuLyId");
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
            string whereClauseTonDongKyTruoc = string.Format("NgayTiepNhan:[* TO {0}] AND (KhieuNai_NgayDongKN : [{1} TO *])",
                fromDate.AddDays(-1).EndOfDay().FormatSolrDateTime(),
                fromDate.StartOfDay().FormatSolrDateTime());


            whereClauseTonDongKyTruoc += string.Format(" AND KhuVucXuLyId : {0}", doiTacId);
            whereClauseTonDongKyTruoc += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", fromDate.StartOfDay().FormatSolrDateTime()); // Ngày chuyển đi sau thời điểm đầu kỳ báo cáo

            solrQuery = new SolrQuery(whereClauseTonDongKyTruoc);

            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDongTruocKy);

            // Danh sách khiếu nại tồn đọng kỳ trước
            List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();
            if (listKhieuNaiInfo != null)
            {
                // Kết quả SL tồn đọng kỳ trước
                dtDoiTac.Rows[0]["SLTonDongKyTruoc"] = listKhieuNaiInfo.Count;

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                }
            }

            // Số lượng tồn đọng
            // Số lượng tồn đọng quá hạn            
            List<int> listKhieuNaiIdTonDong = new List<int>();

            QueryOptions queryOptionTonDong = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
            extraParamTonDong.Add("fl", @"Id, KhieuNaiId, NgayTiepNhan, NgayQuaHan, IsCurrent , LDate, HanhDong, KhuVucXuLyId");
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

            // Điều kiện tồn đọng
            string whereClauseTonDong = string.Format("NgayTiepNhan:[* TO {1}] AND KhieuNai_NgayDongKN:[{2} TO *]",
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            whereClauseTonDong += string.Format(" AND KhuVucXuLyId : {0}", doiTacId);
            whereClauseTonDong += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            solrQuery = new SolrQuery(whereClauseTonDong);
            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDong);


            if (listKhieuNaiInfo != null)
            {
                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    listKhieuNaiIdTonDong.Add(listKhieuNaiInfo[i].KhieuNaiId);
                }

                dtDoiTac.Rows[0]["SLTonDong"] = listKhieuNaiInfo.Count;
                dtDoiTac.Rows[0]["SLQuaHanTonDong"] = listKhieuNaiInfo.FindAll(delegate (KhieuNai_ReportInfo obj)
               {
                   return (obj.IsCurrent && obj.NgayQuaHan <= toDate.EndOfDay()) || (obj.LDate > obj.NgayQuaHan && toDate.EndOfDay() >= obj.NgayQuaHan);
               }).Count;
            }

            // Số lượng tiếp nhận
            QueryOptions queryOptionTiepNhan = new QueryOptions();
            //Lấy ra những trường nào
            var extraParamLoaiKhieuNai = new Dictionary<string, string>();
            extraParamLoaiKhieuNai.Add("fl", @"KhieuNaiId");
            queryOptionTiepNhan.ExtraParams = extraParamLoaiKhieuNai;
            queryOptionTiepNhan.Start = 0;
            queryOptionTiepNhan.Rows = int.MaxValue;


            SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
            List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
            listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

            GroupingParameters gpTiepNhan = new GroupingParameters();
            gpTiepNhan.Fields = listGroupField;
            gpTiepNhan.Limit = 1;
            gpTiepNhan.Main = true;
            gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
            queryOptionTiepNhan.Grouping = gpTiepNhan;

            // listKhieuNaiIdTiepNhan : Biến dùng để lưu các KhieuNaiId tiếp nhận, mục đích dùng để xác định các KhieuNaiId nào đã được xử lý
            List<int> listKhieuNaiIdTiepNhan = new List<int>();

            string whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND KhuVucXuLyId: {2} AND HanhDong:(0 1 2 3)",
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                doiTacId);

            solrQuery = new SolrQuery(whereClauseTiepNhan);
            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTiepNhan);

            // Loại bỏ các khiếu nại tồn đọng kỳ trước
            if (listKhieuNaiInfo != null)
            {
                if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                {
                    int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                }

                dtDoiTac.Rows[0]["SLTiepNhan"] = listKhieuNaiInfo.Count;
                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    listKhieuNaiIdTiepNhan.Add(listKhieuNaiInfo[i].KhieuNaiId);
                }

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

            GroupingParameters gpXuLy = new GroupingParameters();
            gpXuLy.Fields = listGroupField;
            gpXuLy.Limit = 1;
            gpXuLy.Main = true;
            gpXuLy.OrderBy = listSortOrderNgayXuLy;
            queryOptionXuLy.Grouping = gpXuLy;

            string whereClauseXuLy = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  KhuVucXuLyTruocId: {2} AND -KhuVucXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  KhuVucXuLyId:{2} AND HanhDong: 4)", ConvertDateToSolr(fromDate.ToString("yyyyMMdd"), true, false), ConvertDateToSolr(toDate.ToString("yyyyMMdd"), false, true), doiTacId);
            solrQuery = new SolrQuery(whereClauseXuLy);

            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionXuLy);
            if (listKhieuNaiInfo != null)
            {
                int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong.Contains(obj.KhieuNaiId); });
                if (listKhieuNaiInfo != null)
                {
                    dtDoiTac.Rows[0]["SLDaXuLyLuyKe"] = listKhieuNaiInfo.Count;
                    dtDoiTac.Rows[0]["SLQuaHanDaXuLy"] = listKhieuNaiInfo.FindAll(delegate (KhieuNai_ReportInfo obj) { return obj.NgayTiepNhan >= obj.NgayQuaHan_PhongBanXuLyTruoc; }).Count;

                    // Tính số lượng tiếp nhận đã xử lý trong kỳ lấy báo cáo.
                    dtDoiTac.Rows[0]["SLDaXuLyTiepNhan"] = listKhieuNaiInfo.FindAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTiepNhan.Contains(obj.KhieuNaiId); }).Count;
                }
            }

            // Số lượng tạo mới                    
            QueryOptions queryOptionTaoMoi = new QueryOptions();
            //Lấy ra những trường nào
            var extraParamTaoMoi = new Dictionary<string, string>();
            extraParamTaoMoi.Add("fl", @"Id");
            queryOptionTaoMoi.ExtraParams = extraParamTaoMoi;
            queryOptionTaoMoi.Start = 0;
            queryOptionTaoMoi.Rows = int.MaxValue;

            string whereClauseTaoMoi = string.Format("NgayTiepNhan:[{0} TO {1}] AND KhuVucId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
            solrQuery = new SolrQuery(whereClauseTaoMoi);
            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, queryOptionTaoMoi);

            row["SLTaoMoi"] = listKhieuNaiInfo != null ? listKhieuNaiInfo.Count : 0;

            // Số lượng tạo đã đóng                   
            QueryOptions queryOptionDaDong = new QueryOptions();
            //Lấy ra những trường nào
            var extraParamDaDong = new Dictionary<string, string>();
            extraParamDaDong.Add("fl", @"Id");
            queryOptionDaDong.ExtraParams = extraParamDaDong;
            queryOptionDaDong.Start = 0;
            queryOptionDaDong.Rows = int.MaxValue;

            string whereClauseDaDong = string.Format("NgayDongKN:[{0} TO {1}] AND KhuVucXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
            solrQuery = new SolrQuery(whereClauseDaDong);
            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, queryOptionDaDong);

            row["SLDaDong"] = listKhieuNaiInfo != null ? listKhieuNaiInfo.Count : 0;

            return dtDoiTac;
        }
    }
}