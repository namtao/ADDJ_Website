using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADDJ.Core;
using ADDJ.Entity;
using SolrNet.Commands.Parameters;
using SolrNet;

namespace ADDJ.Impl
{

    public enum LoaiBaoCaoEnum : int
    {
        [Name("SL tồn đọng kỳ trước")]
        SoLuongTonDongKyTruoc = 1,

        [Name("SL tiếp nhận")]
        SoLuongTiepNhan = 5,

        [Name("SL đã xử lý (tiếp nhận)")]
        SoLuongDaXuLy_TiepNhan = 10,

        [Name("SL đã xử lý (lũy kế)")]
        SoLuongDaXuLy_LuyKe = 15,

        [Name("SL quá hạn đã xử lý")]
        SoLuongQuaHanDaXuLy = 20,

        [Name("SL Tồn đọng")]
        SoLuongTonDong = 25,

        [Name("SL Tồn đọng quá hạn")]
        SoLuongTonDongQuaHan = 30
    }
    public enum CapBaoCaoEnum : int
    {

        [Name("Khu vực")]
        [KeySolr4Query("KhuVucXuLyId")]
        [KeySolrBefore4Query("KhuVucXuLyTruocId")]
        KhuVuc = 1,

        [Name("Đối tác")]
        [KeySolr4Query("DoiTacXuLyId")]
        [KeySolrBefore4Query("DoiTacXuLyTruocId")]
        DoiTac = 2,

        [Name("Phòng ban")]
        [KeySolr4Query("PhongBanXuLyId")]
        [KeySolrBefore4Query("PhongBanXuLyTruocId")]
        PhongBan = 3
    }
    public class BaoCaoPAKNImpl
    {
        private string UrlSolrActivity { get; set; }
        private string UrlSolrGQKN { get; set; }
        public BaoCaoPAKNImpl()
        {
            this.UrlSolrActivity = string.Concat(Config.ServerSolr, "Activity");
            this.UrlSolrGQKN = string.Concat(Config.ServerSolr, "GQKN");

        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiTonDongKyTruoc(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            QueryOptions qrOptions = new QueryOptions();

            Dictionary<string, string> exPrms = new Dictionary<string, string>();
            exPrms.Add("fl", @"Id, KhieuNaiId, SoThueBao, NguoiXuLy, PhongBanXuLyId, PhongBanTiepNhanId, TenPhongBanXuLy, NgayQuaHan, NgayTiepNhan, LDate");
            qrOptions.ExtraParams = exPrms;
            qrOptions.Start = 0;
            qrOptions.Rows = int.MaxValue;

            SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
            SolrNet.SortOrder sortOrderTonDongTruocKyActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
            List<SolrNet.SortOrder> listSortOrderTonDongTruocKy = new List<SolrNet.SortOrder>();
            listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyNgayTiepNhan);
            listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyActivityId);

            GroupingParameters grpPrms = new GroupingParameters();
            grpPrms.Fields = listGroupField;
            grpPrms.Limit = 1;
            grpPrms.Main = true;
            grpPrms.OrderBy = listSortOrderTonDongTruocKy;
            qrOptions.Grouping = grpPrms;

            DateTime previousFromDate = fromDate.AddDays(-1);
            string whereClauseTonDongKyTruoc = string.Format("NgayTiepNhan:[* TO {0}] AND (KhieuNai_NgayDongKN : [{1} TO *])",
                fromDate.AddDays(-1).EndOfDay().FormatSolrDateTime(),
                fromDate.StartOfDay().FormatSolrDateTime());


            whereClauseTonDongKyTruoc += string.Format(" AND {0} : {1}", capBaoCao.KeySolr4Query(), giaTriId);
            whereClauseTonDongKyTruoc += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", fromDate.StartOfDay().FormatSolrDateTime()); // Ngày chuyển đi sau thời điểm đầu kỳ báo cáo

            SolrQuery solrQuery = new SolrQuery(whereClauseTonDongKyTruoc);

            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, qrOptions).ToList();
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiTiepNhan(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            // Số lượng tiếp nhận
            QueryOptions queryOptionTiepNhan = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamLoaiKhieuNai = new Dictionary<string, string>();
            extraParamLoaiKhieuNai.Add("fl", @"Id, KhieuNaiId, SoThueBao, NguoiXuLy, PhongBanXuLyId, TenPhongBanXuLy, NgayTiepNhan, NgayQuaHan, LDate");
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

            string whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND {2}: {3} AND HanhDong:(0 1 2 3)",
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                capBaoCao.KeySolr4Query(),
                giaTriId);

            SolrQuery solrQuery = new SolrQuery(whereClauseTiepNhan);
            SolrQueryResults<KhieuNai_ReportInfo> lstTiepNhan = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionTiepNhan);

            List<KhieuNai_ReportInfo> listTonDongKyTruoc = LayKhieuNaiTonDongKyTruoc(capBaoCao, giaTriId, fromDate, toDate);

            // Loại bỏ các khiếu nại tồn đọng kỳ trước
            if (lstTiepNhan != null)
            {
                if (listTonDongKyTruoc != null && listTonDongKyTruoc.Any())
                {
                    return lstTiepNhan.AsEnumerable<KhieuNai_ReportInfo>().Where(a => (listTonDongKyTruoc.Count(b => b.KhieuNaiId == a.KhieuNaiId)) == 0).ToList();
                }
            }

            return lstTiepNhan;

        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiTaoMoi(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            // Số lượng tiếp nhận
            QueryOptions queryOptionTiepNhan = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamLoaiKhieuNai = new Dictionary<string, string>();
            extraParamLoaiKhieuNai.Add("fl", @"Id, KhieuNaiId, SoThueBao, NguoiXuLy, PhongBanXuLyId, PhongBanTiepNhanId, TenPhongBanXuLy, NgayTiepNhan, NgayQuaHan, LDate");
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

            List<int> dsKhieuNaiTaoMoi = new List<int>();

            string whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND {2}: {3} AND HanhDong : 0 AND ActivityTruoc : 0",
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                capBaoCao.KeySolr4Query(),
                giaTriId);

            SolrQuery solrQuery = new SolrQuery(whereClauseTiepNhan);
            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionTiepNhan);
        }


        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            QueryOptions queryOptionXuLy = new QueryOptions();
            // Lấy ra những trường nào
            Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
            extraParamXuLy.Add("fl", @"Id, ActivityId, ActivityTruoc, KhieuNaiId, SoThueBao, NguoiXuLy, TenPhongBanXuLy, NgayTiepNhan, NgayQuaHan, LDate");
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

            string whereClauseXuLy = string.Empty;
            whereClauseXuLy += string.Format(@"(NgayTiepNhan:[{3} TO {4}] AND  {0}: {2} AND -{1} : {2} AND -HanhDong : 4)",
                capBaoCao.KeySolrBefore4Query(),
                capBaoCao.KeySolr4Query(),
                giaTriId,
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime());

            SolrQuery solrQuery = new SolrQuery(whereClauseXuLy);
            SolrQueryResults<KhieuNai_ReportInfo> list1 = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionXuLy);

            var objs = list1.Where(v => v.ActivityId > 0).Select((v, rowNumber) => new { RowNumber = rowNumber + 1, ActivityId = v.ActivityTruoc });

            List<KhieuNai_ReportInfo> listMain = new List<KhieuNai_ReportInfo>();

            if (objs.Any())
            {
                int totalCount = objs.Count();

                int pageSize = 500; // Không thể truy vấn với câu dài => phân trang cho nó để lấy
                int numberPage = totalCount / pageSize;
                int tempPage = totalCount % pageSize;

                numberPage += tempPage > 0 ? 1 : 0;

                for (int i = 0; i < numberPage; i++)
                {
                    var lstObjs = objs.OrderBy(v => v.RowNumber).Skip(i * pageSize).Take(pageSize);

                    string where = string.Format("ActivityId : ({0})", string.Join(" ", lstObjs.Select(v => v.ActivityId)));
                    solrQuery = new SolrQuery(where);

                    IEnumerable<KhieuNai_ReportInfo> data = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionXuLy);
                    listMain.AddRange(data);
                }
            }

            whereClauseXuLy = string.Format(@"LDate:[{0} TO {1}] AND  {2} : {3} AND HanhDong: 4",
                              fromDate.StartOfDay().FormatSolrDateTime(),
                              toDate.EndOfDay().FormatSolrDateTime(),
                              capBaoCao.KeySolr4Query(),
                              giaTriId);

            // Đổi câu điều kiện
            solrQuery = new SolrQuery(whereClauseXuLy);

            List<KhieuNai_ReportInfo> obj2 = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionXuLy).ToList();
            listMain = listMain.Where(v => obj2.Count(a => a.KhieuNaiId == v.KhieuNaiId) == 0).ToList(); // Loại bỏ trùng lặp
            listMain.AddRange(obj2);

            return listMain.ToList();

        }

        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy_BoTonDong(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiDaXuLy(capBaoCao, giaTriId, fromDate, toDate);
            List<KhieuNai_ReportInfo> lstKhieuNaiTonDong = LayKhieuNaiTonDong(capBaoCao, giaTriId, toDate);
            if (listKhieuNaiInfo != null)
            {
                if (lstKhieuNaiTonDong != null) return listKhieuNaiInfo.Where(v => (lstKhieuNaiTonDong.Count(a => a.KhieuNaiId == v.KhieuNaiId)) == 0).ToList();
            }
            return listKhieuNaiInfo;

        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy_TiepNhan(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiDaXuLy(capBaoCao, giaTriId, fromDate, toDate);
            List<KhieuNai_ReportInfo> lstKhieuNaiTonDong = LayKhieuNaiTonDong(capBaoCao, giaTriId, toDate);
            List<KhieuNai_ReportInfo> lstKhieuNaiTiepNhan = LayKhieuNaiTiepNhan(capBaoCao, giaTriId, fromDate, toDate);
            if (listKhieuNaiInfo != null)
            {
                // Loại bỏ khiểu nại tồn đọng
                if (lstKhieuNaiTonDong != null) listKhieuNaiInfo = listKhieuNaiInfo.Where(v => (lstKhieuNaiTonDong.Count(a => a.KhieuNaiId == v.KhieuNaiId)) == 0).ToList();
                if (lstKhieuNaiTiepNhan != null) return listKhieuNaiInfo.Where(a => (lstKhieuNaiTiepNhan.Count(b => b.KhieuNaiId == a.KhieuNaiId)) > 0).ToList();
            }
            return null;
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy_LuyKe(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiDaXuLy(capBaoCao, giaTriId, fromDate, toDate);
            List<KhieuNai_ReportInfo> lstKhieuNaiTonDong = LayKhieuNaiTonDong(capBaoCao, giaTriId, toDate);
            if (listKhieuNaiInfo != null)
            {
                // Loại bỏ khiểu nại tồn đọng
                if (lstKhieuNaiTonDong != null) return listKhieuNaiInfo.Where(v => (lstKhieuNaiTonDong.Count(a => a.KhieuNaiId == v.KhieuNaiId)) == 0).ToList();
                else return listKhieuNaiInfo;
            }
            return null;
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy_QuaHan(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiDaXuLy(capBaoCao, giaTriId, fromDate, toDate);
            if (listKhieuNaiInfo != null)
            {
                return listKhieuNaiInfo.Where(a => a.LDate >= a.NgayQuaHan).ToList();
            }
            return null;
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiDaXuLy_QuaHan_BoTonDong(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiDaXuLy_BoTonDong(capBaoCao, giaTriId, fromDate, toDate);
            if (listKhieuNaiInfo != null)
            {
                return listKhieuNaiInfo.Where(a => a.LDate >= a.NgayQuaHan).ToList();
            }
            return null;
        }

        public List<KhieuNai_ReportInfo> LayKhieuNaiTonDong(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            List<int> listKhieuNaiIdTonDong = new List<int>();

            QueryOptions queryOptionTonDong = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();

            extraParamTonDong.Add("fl", @"Id, KhieuNaiId, SoThueBao, NguoiXuLy, PhongBanXuLyId, TenPhongBanXuLy, IsCurrent, NgayQuaHan, NgayTiepNhan, LDate, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, KhieuNai_GhiChu");
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
            string whereClauseTonDong = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]",
                // fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            whereClauseTonDong += string.Format(" AND {0} : {1}", capBaoCao.KeySolr4Query(), giaTriId);
            whereClauseTonDong += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            SolrQuery solrQuery = new SolrQuery(whereClauseTonDong);
            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionTonDong).ToList();
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiTonDongQuaHan(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = LayKhieuNaiTonDong(capBaoCao, giaTriId, toDate);
            if (listKhieuNaiInfo != null)
            {
                // Điều kiện tồn đọng quá hạn
                return listKhieuNaiInfo.Where(obj => (obj.IsCurrent && obj.NgayQuaHan <= toDate.EndOfDay()) || (obj.LDate > obj.NgayQuaHan && toDate.EndOfDay() >= obj.NgayQuaHan)).ToList();
            }
            return null;
        }
        public List<KhieuNai_ReportInfo> LayKhieuNaiDaDong(CapBaoCaoEnum capBaoCao, int giaTriId, DateTime fromDate, DateTime toDate)
        {
            string URL_SOLR_GQKN = string.Concat(Config.ServerSolr, "GQKN");
            Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
            extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHan, PhongBanXuLyId, NguoiXuLy");

            QueryOptions qoKhieuNaiDaDong = new QueryOptions();
            qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
            qoKhieuNaiDaDong.Start = 0;
            qoKhieuNaiDaDong.Rows = int.MaxValue;

            string whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND {2} : {3}",
                fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                capBaoCao.KeySolr4Query(),
                giaTriId);
            SolrQuery solrQuery = new SolrQuery(whereClause);
            Helper.GhiLogs("DongKN", whereClause);
            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong).ToList();

        }
        public List<KhieuNai_ReportInfo> TongKhieuNaiTonDongKyTruoc(DateTime fromDate, DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            QueryOptions qrOptions = new QueryOptions();

            Dictionary<string, string> exPrms = new Dictionary<string, string>();
            exPrms.Add("fl", @"Id, KhieuNaiId, KhuVucXuLyId, DoiTacXuLyId, PhongBanXuLyId, SoThueBao, NguoiXuLy, TenPhongBanXuLy, NgayQuaHan, NgayTiepNhan, LDate");
            qrOptions.ExtraParams = exPrms;
            qrOptions.Start = 0;
            qrOptions.Rows = int.MaxValue;

            SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
            SolrNet.SortOrder sortOrderTonDongTruocKyActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
            List<SolrNet.SortOrder> listSortOrderTonDongTruocKy = new List<SolrNet.SortOrder>();
            listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyNgayTiepNhan);
            listSortOrderTonDongTruocKy.Add(sortOrderTonDongTruocKyActivityId);

            GroupingParameters grpPrms = new GroupingParameters();
            grpPrms.Fields = listGroupField;
            grpPrms.Limit = 1;
            grpPrms.Main = true;
            grpPrms.OrderBy = listSortOrderTonDongTruocKy;
            qrOptions.Grouping = grpPrms;

            DateTime previousFromDate = fromDate.AddDays(-1);
            string whereClauseTonDongKyTruoc = string.Format("NgayTiepNhan:[* TO {0}] AND (KhieuNai_NgayDongKN : [{1} TO *])",
                fromDate.AddDays(-1).EndOfDay().FormatSolrDateTime(),
                fromDate.StartOfDay().FormatSolrDateTime());

            // whereClauseTonDongKyTruoc += string.Format(" AND {0} : {1}", capBaoCao.KeySolr4Query(), giaTriId);
            whereClauseTonDongKyTruoc += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", fromDate.StartOfDay().FormatSolrDateTime()); // Ngày chuyển đi sau thời điểm đầu kỳ báo cáo

            SolrQuery solrQuery = new SolrQuery(whereClauseTonDongKyTruoc);

            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, qrOptions).ToList();
        }
        public List<KhieuNai_ReportInfo> TongKhieuNaiTonDong(DateTime toDate)
        {
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            List<int> listKhieuNaiIdTonDong = new List<int>();

            QueryOptions queryOptionTonDong = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();

            extraParamTonDong.Add("fl", @"Id, KhieuNaiId, KhuVucXuLyId, DoiTacXuLyId, PhongBanXuLyId, SoThueBao, NguoiXuLy, TenPhongBanXuLy, IsCurrent, NgayQuaHan, NgayTiepNhan, LDate, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, KhieuNai_GhiChu");
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
            string whereClauseTonDong = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]",
                // fromDate.StartOfDay().FormatSolrDateTime(),
                toDate.EndOfDay().FormatSolrDateTime(),
                toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            // whereClauseTonDong += string.Format(" AND {0} : {1}", capBaoCao.KeySolr4Query(), giaTriId);
            whereClauseTonDong += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            SolrQuery solrQuery = new SolrQuery(whereClauseTonDong);
            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(this.UrlSolrActivity, solrQuery, queryOptionTonDong).ToList();
        }
        public List<KhieuNai_ReportInfo> TongKhieuNaiTonDongQuaHan(DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = TongKhieuNaiTonDong(toDate);
            if (listKhieuNaiInfo != null)
            {
                // Điều kiện tồn đọng quá hạn
                return listKhieuNaiInfo.Where(obj => (obj.IsCurrent && obj.NgayQuaHan <= toDate.EndOfDay()) || (obj.LDate > obj.NgayQuaHan && toDate.EndOfDay() >= obj.NgayQuaHan)).ToList();
            }
            return null;
        }
    }
}