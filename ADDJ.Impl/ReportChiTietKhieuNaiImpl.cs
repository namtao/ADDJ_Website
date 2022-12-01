using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Entity;

namespace ADDJ.Impl
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 12/08/2014
    /// Todo : Class dùng để lấy chi tiết các khiếu nại (phục vụ cho báo cáo) dùng để đối chứng
    /// </summary>
    public class ReportChiTietKhieuNaiImpl : BaseImpl<KhieuNai_ReportInfo>
    {
        private const string SOLR_GQKN = "GQKN";
        private const string SOLR_ACTIVITY = "Activity";
        private const string SOLR_SOTIEN = "SoTien";
        private const string SOLR_KETQUAXULY = "KetQuaXuLy";

        private string URL_SOLR_GQKN
        {
            get
            {
                return Config.ServerSolr + SOLR_GQKN;
            }
        }

        private string URL_SOLR_ACTIVITY
        {
            get
            {
                return Config.ServerSolr + SOLR_ACTIVITY;
            }
        }

        private string URL_SOLR_SOTIEN
        {
            get
            {
                return Config.ServerSolr + SOLR_SOTIEN;
            }
        }

        private string URL_SOLR_KETQUAXULY
        {
            get
            {
                return Config.ServerSolr + SOLR_KETQUAXULY;
            }
        }

        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai";
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>

        ///
        /// nvhung 27/10/2014
        /// Refactoring get data from solr db           
        ///

        private List<KhieuNai_ReportInfo> GetDataFromSolr(string fieldList, Dictionary<string, Order> sortOrders, string swhereClause)
        {
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = null;
            List<string> listGroupField = new List<string>();
            listGroupField.Add("Id");

            SolrQuery solrQuery = null;

            //Các đối tượng dùng chung cho Solr query 
            //Add SortOrder
            Dictionary<string, string> extParams = new Dictionary<string, string>();
            extParams.Add("fl", fieldList);
            QueryOptions qo = new QueryOptions();
            qo.ExtraParams = extParams;
            qo.Start = 0;
            qo.Rows = int.MaxValue;

            List<SolrNet.SortOrder> lstRangeSortOrder = new List<SortOrder>();

            if (sortOrders.Count > 0)
            {
                foreach (var obj in sortOrders)
                {
                    lstRangeSortOrder.Add(new SolrNet.SortOrder(obj.Key, obj.Value));
                }
            }
            GroupingParameters gp = new GroupingParameters();
            gp.Fields = listGroupField;
            gp.Main = true;
            gp.OrderBy = lstRangeSortOrder;
            qo.Grouping = gp;
            solrQuery = new SolrQuery(swhereClause);
            lstKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qo);
            return lstKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/12/2014
        /// Todo : Lấy danh sách trả về từ Solr
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="sortOrders"></param>
        /// <param name="swhereClause"></param>
        /// <returns></returns>
        private List<KhieuNai_ReportInfo> GetDataFromSolr(string urlSolr, string fieldList, string swhereClause, Dictionary<string, Order> sortOrders)
        {
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = null;
            SolrQuery solrQuery = null;

            //Các đối tượng dùng chung cho Solr query 
            //Add SortOrder
            Dictionary<string, string> extParams = new Dictionary<string, string>();
            extParams.Add("fl", fieldList);
            QueryOptions qo = new QueryOptions();
            qo.ExtraParams = extParams;
            qo.Start = 0;
            qo.Rows = int.MaxValue;

            List<SolrNet.SortOrder> lstRangeSortOrder = new List<SortOrder>();

            if (sortOrders.Count > 0)
            {
                foreach (var obj in sortOrders)
                {
                    lstRangeSortOrder.Add(new SolrNet.SortOrder(obj.Key, obj.Value));
                }
            }
            qo.OrderBy = lstRangeSortOrder;

            solrQuery = new SolrQuery(swhereClause);
            lstKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(urlSolr, solrQuery, qo);
            return lstKhieuNaiInfo;
        }

        /// <summary>
        /// get danh sach khieu nai theo doi tac
        /// </summary>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> GetData_KhieuNaiTheoDoiTac(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = null;
            //luu tru danh sach ID khieu nai ID ton dong ky truoc
            List<int> lstKhieuNaiIdTonDong = null;

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);
            //Danh sach KhieuNaiId ton dong theo ReportType
            if (reportType == 2 || reportType == 3)
            {
                lstKhieuNaiIdTonDong = new List<int>();
                string fListTonDong = "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong";
                Dictionary<string, Order> dSortOrdersTonDong = new Dictionary<string, Order>();
                dSortOrdersTonDong.Add("NgayTiepNhan", Order.DESC);
                dSortOrdersTonDong.Add("ActivityId", Order.DESC);
                string sWhereTonDong = string.Empty;
                if (reportType == 2)
                {
                    sWhereTonDong = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                }
                else if (reportType == 3)
                {
                    sWhereTonDong = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                }
                lstKhieuNaiInfo = GetDataFromSolr(fListTonDong, dSortOrdersTonDong, sWhereTonDong);
                if (lstKhieuNaiInfo != null)
                {
                    lstKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                    {
                        return obj.DoiTacXuLyId != doiTacId;
                    });
                    if (lstKhieuNaiInfo != null)
                    {
                        for (int i = 0; i < lstKhieuNaiInfo.Count; i++)
                        {
                            lstKhieuNaiIdTonDong.Add(lstKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }
                }
            }

            switch (reportType)
            {
                case 1://So luong ton dong truoc ky
                    string fList = "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong";
                    Dictionary<string, Order> dSortOrders = new Dictionary<string, Order>();
                    dSortOrders.Add("NgayTiepNhan", Order.DESC);
                    dSortOrders.Add("ActivityId", Order.DESC);
                    string sWhere = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    lstKhieuNaiInfo = GetDataFromSolr(fList, dSortOrders, sWhere);
                    if (lstKhieuNaiInfo != null)
                    {
                        lstKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.DoiTacXuLyId != doiTacId;
                        });
                    }

                    break;
                case 2:// So luong tiep nhan
                    string fList1 = "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy";
                    Dictionary<string, Order> dSortOrders1 = new Dictionary<string, Order>();
                    dSortOrders1.Add("NgayTiepNhan", Order.ASC);
                    string sWhere1 = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId:{2} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    lstKhieuNaiInfo = GetDataFromSolr(fList1, dSortOrders1, sWhere1);

                    if (lstKhieuNaiIdTonDong != null && lstKhieuNaiIdTonDong.Count > 0)
                    {
                        lstKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return lstKhieuNaiIdTonDong.Contains(obj.KhieuNaiId);
                        });
                    }

                    break;
                case 3://so luong da xu ly      
                    string fList2 = "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy";
                    Dictionary<string, Order> dSortOrders2 = new Dictionary<string, Order>();
                    dSortOrders2.Add("NgayTiepNhan", Order.DESC);
                    string sWhere2 = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    lstKhieuNaiInfo = GetDataFromSolr(fList2, dSortOrders2, sWhere2);
                    if (lstKhieuNaiInfo != null)
                    {
                        lstKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return lstKhieuNaiIdTonDong.Contains(obj.KhieuNaiId);
                        });
                    }
                    break;
                case 4://Số lượng tồn đọng
                    string fList3 = "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate,NguoiXuLyTruoc, NgayQuaHan,NgayQuaHanPhongBanXuLyTruoc,PhongBanXuLyId,PhongBanXuLyTruocId,DoiTacXuLyId";
                    Dictionary<string, Order> dSortOrders3 = new Dictionary<string, Order>();
                    dSortOrders3.Add("NgayTiepNhan", Order.DESC);
                    dSortOrders3.Add("ActivityId", Order.DESC);
                    string sWhere3 = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    lstKhieuNaiInfo = GetDataFromSolr(fList3, dSortOrders3, sWhere3);
                    if (lstKhieuNaiInfo != null)
                    {
                        lstKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                    }
                    break;

            }
            lstKhieuNaiInfo = SortNgayTiepNhanASCByLinQ(lstKhieuNaiInfo);
            return lstKhieuNaiInfo;
        }

        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDoiTac(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))

                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Đóng_KN || obj.DoiTacXuLyId != doiTacId; });                        
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận

                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    // Lấy ra số lượng tồn đọng kỳ trước
                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    //whereClause = string.Format("LDate:[* TO {0}] OR (NgayTiepNhan : [* TO {0}] AND LDate : [{1} TO *])", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId: {2} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    List<int> listKhieuNaiIdTonDong = new List<int>();

                    Dictionary<string, string> extraParamTonDong_2 = new Dictionary<string, string>();
                    extraParamTonDong_2.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_2;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_2.Add(sortOrderTonDongNgayTiepNhan_2);
                    listSortOrderTonDong_2.Add(sortOrderTonDongActivityId_2);

                    GroupingParameters gpTonDong_2 = new GroupingParameters();
                    gpTonDong_2.Fields = listGroupField;
                    gpTonDong_2.Limit = 1;
                    gpTonDong_2.Main = true;
                    gpTonDong_2.OrderBy = listSortOrderTonDong_2;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();

                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongbanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng
                    //// Số lượng tồn đọng
                    //// Số lượng tồn đọng quá hạn            
                    //QueryOptions queryOptionTonDong = new QueryOptions();
                    ////Lấy ra những trường nào
                    //var extraParamTonDong = new Dictionary<string, string>();
                    //extraParamTonDong.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, LDate, DoiTacXuLyTruocId, DoiTacXuLyId, HanhDong");
                    //queryOptionTonDong.ExtraParams = extraParamTonDong;
                    //queryOptionTonDong.Start = 0;
                    //queryOptionTonDong.Rows = 0;

                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn
                    //// Số lượng tồn đọng
                    //// Số lượng tồn đọng quá hạn            
                    //QueryOptions queryOptionTonDongQuaHan = new QueryOptions();
                    ////Lấy ra những trường nào
                    //var extraParamTonDongQuaHan = new Dictionary<string, string>();
                    //extraParamTonDongQuaHan.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, LDate, DoiTacXuLyTruocId, DoiTacXuLyId, HanhDong");
                    //queryOptionTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    //queryOptionTonDongQuaHan.Start = 0;
                    //queryOptionTonDongQuaHan.Rows = 0;

                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || toDate < obj.NgayQuaHan; });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);
            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDoiTac(int doiTacId, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string whereClauseLoaiKhieuNai = string.Empty;
            if (loaiKhieuNai_NhomId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNai_NhomId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNai_NhomId);
            }
            if (loaiKhieuNaiId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNaiId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNaiId);
            }
            if (linhVucChungId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucChungId:{1}", whereClauseLoaiKhieuNai, linhVucChungId);
            }
            if (linhVucConId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucConId:{1}", whereClauseLoaiKhieuNai, linhVucConId);
            }

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))

                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Đóng_KN || obj.DoiTacXuLyId != doiTacId; });                        
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận

                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    // Lấy ra số lượng tồn đọng kỳ trước
                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    //whereClause = string.Format("LDate:[* TO {0}] OR (NgayTiepNhan : [* TO {0}] AND LDate : [{1} TO *])", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId: {2} AND HanhDong:(2 3) {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    List<int> listKhieuNaiIdTonDong = new List<int>();

                    Dictionary<string, string> extraParamTonDong_2 = new Dictionary<string, string>();
                    extraParamTonDong_2.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_2;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_2.Add(sortOrderTonDongNgayTiepNhan_2);
                    listSortOrderTonDong_2.Add(sortOrderTonDongActivityId_2);

                    GroupingParameters gpTonDong_2 = new GroupingParameters();
                    gpTonDong_2.Fields = listGroupField;
                    gpTonDong_2.Limit = 1;
                    gpTonDong_2.Main = true;
                    gpTonDong_2.OrderBy = listSortOrderTonDong_2;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();

                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng
                    //// Số lượng tồn đọng
                    //// Số lượng tồn đọng quá hạn            
                    //QueryOptions queryOptionTonDong = new QueryOptions();
                    ////Lấy ra những trường nào
                    //var extraParamTonDong = new Dictionary<string, string>();
                    //extraParamTonDong.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, LDate, DoiTacXuLyTruocId, DoiTacXuLyId, HanhDong");
                    //queryOptionTonDong.ExtraParams = extraParamTonDong;
                    //queryOptionTonDong.Start = 0;
                    //queryOptionTonDong.Rows = 0;

                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn
                    //// Số lượng tồn đọng
                    //// Số lượng tồn đọng quá hạn            
                    //QueryOptions queryOptionTonDongQuaHan = new QueryOptions();
                    ////Lấy ra những trường nào
                    //var extraParamTonDongQuaHan = new Dictionary<string, string>();
                    //extraParamTonDongQuaHan.Add("fl", @"KhieuNaiId, NgayTiepNhan, NgayQuaHan, LDate, DoiTacXuLyTruocId, DoiTacXuLyId, HanhDong");
                    //queryOptionTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    //queryOptionTonDongQuaHan.Start = 0;
                    //queryOptionTonDongQuaHan.Rows = 0;

                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA, LoaiKhieuNai, LinhVucChung, LinhVucCon, ArchiveId");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || toDate < obj.NgayQuaHan; });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);
            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>        
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoPhongBanDoiTac(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong:(1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                    
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.PhongBanXuLyId != phongBanId || toDate < obj.NgayQuaHan;
                        });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoPhongBanDoiTac(int phongBanId, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string whereClauseLoaiKhieuNai = string.Empty;
            if (loaiKhieuNai_NhomId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNai_NhomId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNai_NhomId);
            }
            if (loaiKhieuNaiId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNaiId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNaiId);
            }
            if (linhVucChungId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucChungId:{1}", whereClauseLoaiKhieuNai, linhVucChungId);
            }
            if (linhVucConId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucConId:{1}", whereClauseLoaiKhieuNai, linhVucConId);
            }

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong:(1 2 3) {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NoiDungPA");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NoiDungPA");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                    
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.PhongBanXuLyId != phongBanId || toDate < obj.NgayQuaHan;
                        });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>        
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan(int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId:{2} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *])",
                                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_NguoiXuLy");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    

                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Đóng_KN || obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   

                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan(int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;
            string whereClauseLoaiKhieuNai = string.Empty;
            if (loaiKhieuNai_NhomId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNai_NhomId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNai_NhomId);
            }
            if (loaiKhieuNaiId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNaiId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNaiId);
            }
            if (linhVucChungId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucChungId:{1}", whereClauseLoaiKhieuNai, linhVucChungId);
            }
            if (linhVucConId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucConId:{1}", whereClauseLoaiKhieuNai, linhVucConId);
            }

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy < DateTime.MaxValue); });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId:{2} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy < DateTime.MaxValue); });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *]) {4}",
                                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(1 2 3) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy <= toDate); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc, NoiDungPA");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_NguoiXuLy");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc, NoiDungPA");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    whereClause = string.Empty;// string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (tenTruyCap.Length == 0)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    

                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.HanhDong == (int)KhieuNai_Actitivy_HanhDong.Đóng_KN || obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   

                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.PhongBanXuLyId != phongBanId || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>        
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDoiTac_V2(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1:
                    // Số lượng tồn đọng kỳ trước
                    // Nếu đối tác = 10000 => Phòng ban CSKH
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDongKyTruoc(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDongKyTruoc(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);
                    break;

                case 2:
                    // Số lượng tiếp nhận
                    // Nếu đối tác = 10000 => Phòng ban CSKH
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTiepNhan(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTiepNhan(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);
                    break;

                case 3:
                    // Số lượng đã xử lý 
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_BoTonDong(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_BoTonDong(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);
                    break;

                case 4:
                    // Số lượng quá hạn đã xử lý
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_QuaHan(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaXuLy_QuaHan(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);
                    break;
                //Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                //extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong, DoiTacXuLyId");

                //QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                //qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                //qoKhieuNaiTonDong_4.Start = 0;
                //qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                //SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                //SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                //List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                //listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                //listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                //GroupingParameters gpTonDong_4 = new GroupingParameters();
                //gpTonDong_4.Fields = listGroupField;
                //gpTonDong_4.Limit = 1;
                //gpTonDong_4.Main = true;
                //gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                //qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                //whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                //solrQuery = new SolrQuery(whereClause);
                //listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                //List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                //if (listKhieuNaiInfo != null)
                //{
                //    int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                //    if (listKhieuNaiInfo != null)
                //    {
                //        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                //        {
                //            listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                //        }
                //    }
                //    //listKhieuNaiInfo.RemoveAll(delegate);
                //}

                //Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                //QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                //qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                //qoKhieuNaiXuLyQuaHan.Start = 0;
                //qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                //SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                //List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                //listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                //GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                //gpXuLyQuaHan.Fields = listGroupField;
                //gpXuLyQuaHan.Limit = 1;
                //gpXuLyQuaHan.Main = true;
                //gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                //qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                ////whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                //solrQuery = new SolrQuery(whereClause);
                //listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);

                //listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                //break;

                case 5:
                    // Số lượng tồn đọng                    
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDong(CapBaoCaoEnum.PhongBan, 60, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDong(CapBaoCaoEnum.DoiTac, doiTacId, toDate);
                    break;

                case 6: // Số lượng tồn đọng quá hạn                    
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDongQuaHan(CapBaoCaoEnum.PhongBan, 60, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTonDongQuaHan(CapBaoCaoEnum.DoiTac, doiTacId, toDate);
                    break;

                case 7:
                    // Số lượng tạo mới
                    // Nếu đối tác = 10000 => Phòng ban CSKH
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTaoMoi(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiTaoMoi(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);

                    // Đổi vị trị mã PA trên view!
                    foreach (var obj in listKhieuNaiInfo) obj.Id = obj.KhieuNaiId;

                    break;

                case 8:
                    // Số lượng đã đóng
                    if (doiTacId == 10000) listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaDong(CapBaoCaoEnum.PhongBan, 60, fromDate, toDate);
                    else listKhieuNaiInfo = new BaoCaoPAKNImpl().LayKhieuNaiDaDong(CapBaoCaoEnum.DoiTac, doiTacId, fromDate, toDate);
                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10:
                    // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11:
                    // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>        
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoPhongBanDoiTac_V2(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND ((PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2}) OR (PhongBanXuLyId:{2} AND HanhDong:4))", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                    
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong,KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            return obj.PhongBanXuLyId != phongBanId || toDate < obj.NgayQuaHan;
                        });
                    }
                    break;
                case 7: // Số lượng tiếp nhận
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan, GhiChu");

                    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    qoKhieuNaiTaoMoi.Start = 0;
                    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanTiepNhanId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    break;

                case 8: // Số lượng đã đóng
                    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy, GhiChu");

                    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    qoKhieuNaiDaDong.Start = 0;
                    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND PhongBanXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10: // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11: // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            //SetValueGhiChuKhieuNai(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/08/2014
        /// Todo : Danh sách khiếu nại của đối tác
        /// </summary>        
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
        ///     2 : Số lượng tiếp nhận
        ///     3 : Số lượng đã xử lý
        ///     4 : Số lượng quá hạn đã xử lý
        ///     5 : Số lượng tồn đọng
        ///     6 : Số lượng tồn đọng quá hạn
        ///     7 : Số lượng tạo mới
        ///     8 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan_V2(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
            int NO_VALUE = -1;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap ; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận                    
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *])",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    doiTacId, ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *])",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);                       
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            //whereClause = string.Format("((NgayTiepNhan:[{0} TO {1}] AND HanhDong:(2 3)) OR (NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND HanhDong:1)) AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);                        
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}  AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            //whereClause = string.Format("((NgayTiepNhan:[{0} TO {1}] AND HanhDong:(2 3)) OR (NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND HanhDong:1)) AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA ");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy >= fromDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            //return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) 
                            //        || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                            //        || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                            return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE)
                                    || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;

                case 7: // Số lượng tiếp nhận
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan");

                    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    qoKhieuNaiTaoMoi.Start = 0;
                    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND DoiTacId :  {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND PhongBanTiepNhanId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    break;

                case 8: // Số lượng đã đóng
                    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy");

                    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    qoKhieuNaiDaDong.Start = 0;
                    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND DoiTacXuLyId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId);
                    }
                    else
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND PhongBanXuLyId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10: // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11: // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 19/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan_V2(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
            int NO_VALUE = -1;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;

            string whereClauseLoaiKhieuNai = string.Empty;
            if (loaiKhieuNai_NhomId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNai_NhomId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNai_NhomId);
            }
            if (loaiKhieuNaiId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LoaiKhieuNaiId:{1}", whereClauseLoaiKhieuNai, loaiKhieuNaiId);
            }
            if (linhVucChungId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucChungId:{1}", whereClauseLoaiKhieuNai, linhVucChungId);
            }
            if (linhVucConId > 0)
            {
                whereClauseLoaiKhieuNai = string.Format("{0} AND LinhVucConId:{1}", whereClauseLoaiKhieuNai, linhVucConId);
            }

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap ; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận                    
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), whereClauseLoaiKhieuNai);
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *]) {4}",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    doiTacId, ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *]) {4}",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);                       
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            //whereClause = string.Format("((NgayTiepNhan:[{0} TO {1}] AND HanhDong:(2 3)) OR (NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND HanhDong:1)) AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);                        
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}  AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            //whereClause = string.Format("((NgayTiepNhan:[{0} TO {1}] AND HanhDong:(2 3)) OR (NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND HanhDong:1)) AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA ");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            //whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            //whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                            whereClause = string.Format("LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan_NguoiXuLyTruoc >= obj.NgayQuaHan_PhongBanXuLyTruoc || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy >= fromDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            //return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) 
                            //        || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                            //        || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                            return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE)
                                    || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;

                case 7: // Số lượng tiếp nhận
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan, NoiDungPA");

                    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    qoKhieuNaiTaoMoi.Start = 0;
                    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND DoiTacId :  {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId, whereClauseLoaiKhieuNai);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND PhongBanTiepNhanId : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId, whereClauseLoaiKhieuNai);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    break;

                case 8: // Số lượng đã đóng
                    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy, NoiDungPA");

                    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    qoKhieuNaiDaDong.Start = 0;
                    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND DoiTacXuLyId : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId, whereClauseLoaiKhieuNai);
                    }
                    else
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND PhongBanXuLyId : {3} {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId, whereClauseLoaiKhieuNai);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {4}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10: // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11: // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *] {2}", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), whereClauseLoaiKhieuNai);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLyTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4) {4}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, whereClauseLoaiKhieuNai);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 16/06/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="tenTruyCap"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan_V3(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
            int NO_VALUE = -1;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate)); // replace old
                    //whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate)); //old
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap ; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận                    
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *])",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    doiTacId, ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *])",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999),
                                                    phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    if (phongBanId == NO_VALUE)
                    {
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}  AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA ");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    //listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan_NguoiXuLyTruoc >= obj.NgayQuaHan_PhongBanXuLyTruoc || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN : [{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy >= fromDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN : [{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            //return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) 
                            //        || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                            //        || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                            return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE)
                                    || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;

                case 7: // Số lượng tiếp nhận
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan");

                    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    qoKhieuNaiTaoMoi.Start = 0;
                    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND DoiTacId :  {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND PhongBanTiepNhanId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    break;

                case 8: // Số lượng đã đóng
                    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy");

                    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    qoKhieuNaiDaDong.Start = 0;
                    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND DoiTacXuLyId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId);
                    }
                    else
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND PhongBanXuLyId : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10: // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11: // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }


        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguoiDungPhongBan_V4(int doiTacId, int phongBanId, string tenTruyCap, DateTime fromDate, DateTime toDate, int reportType, int tinhId, int huyenId)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
            int NO_VALUE = -1;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;
            string tenTruyCapSolr = string.Empty;

            string dkTinh = tinhId > 0 ? string.Format(" AND MaTinhId : {0}", tinhId) : string.Empty;
            string dkHuyen = huyenId > 0 ? string.Format(" AND MaQuanId : {0}", huyenId) : string.Empty;

            switch (reportType)
            {

                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, LDate, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), dkTinh, dkHuyen);
                    //whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate)); //old
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap ; });
                    }

                    break;

                // Edited by	: Dao Van Duong
                // Datetime		: 12.8.2016 11:23
                // Note			: Cập nhật lọc theo tỉnh, huyện
                case 2: // Số lượng tiếp nhận                    

                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;
                    whereClause = string.Format("((NgayTiepNhan:[* TO {0}] AND -HanhDong:1) OR (LDate_ActivityTruoc:[* TO {0}] AND HanhDong:1)) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate), dkTinh, dkHuyen);
                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || (obj.NgayTiepNhan_NguoiXuLy >= fromDate && obj.NgayTiepNhan_NguoiXuLy.Year < DateTime.MaxValue.Year); });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan_NguoiXuLy, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy : {3} AND HanhDong:(2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *]){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999), doiTacId, ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND DoiTacXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND (NgayTiepNhan_NguoiXuLy:[{3} TO *]){4}{5}",
                                                    ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 9999), phongBanId, ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("NgayTiepNhan_NguoiXuLy:[{0} TO {1}] AND PhongBanXuLyId : {2} AND NguoiXuLy: {3} AND HanhDong:(0 1 2 3){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    //whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    //extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA");
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan,  NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    if (phongBanId == NO_VALUE)
                    {
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3}  AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    //extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA ");
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    //listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan_NguoiXuLyTruoc >= obj.NgayQuaHan_PhongBanXuLyTruoc || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN : [{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy >= fromDate; });
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap || obj.NgayTiepNhan_NguoiXuLy > toDate; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                case 6: // Số lượng tồn đọng quá hạn                   
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, NgayTiepNhan_NguoiXuLy");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    //whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN : [{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                        {
                            //return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) 
                            //        || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                            //        || obj.NgayTiepNhan_NguoiXuLy < fromDate || obj.NgayTiepNhan_NguoiXuLy > toDate;
                            return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE)
                                    || obj.NguoiXuLy != tenTruyCap || toDate < obj.NgayQuaHan
                                    || obj.NgayTiepNhan_NguoiXuLy > toDate;
                        });
                    }
                    break;

                case 7: // Số lượng tiếp nhận
                    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan");

                    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    qoKhieuNaiTaoMoi.Start = 0;
                    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND DoiTacId : {3}{4}{5}",
                            ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId, dkTinh, dkHuyen);
                    }
                    else
                    {
                        whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NguoiTiepNhan : {2} AND PhongBanTiepNhanId : {3}{4}{5}",
                            ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId, dkTinh, dkHuyen);
                    }

                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    break;

                case 8: // Số lượng đã đóng
                    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy");

                    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    qoKhieuNaiDaDong.Start = 0;
                    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap);
                    if (phongBanId == NO_VALUE)
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND DoiTacXuLyId : {3}{4}{5}",
                            ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, doiTacId, dkTinh, dkHuyen);
                    }
                    else
                    {
                        whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND NguoiXuLy : {2} AND PhongBanXuLyId : {3}{4}{5}",
                            ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), tenTruyCap, phongBanId, dkTinh, dkHuyen);
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    break;

                case 9: // chuyển ngang hàng
                    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    qoKhieuNaiTonDong_39.Start = 0;
                    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    gpTonDong_39.Fields = listGroupField;
                    gpTonDong_39.Limit = 1;
                    gpTonDong_39.Main = true;
                    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Start = 0;
                    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    gpChuyenNgangHang.Fields = listGroupField;
                    gpChuyenNgangHang.Limit = 1;
                    gpChuyenNgangHang.Main = true;
                    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    }

                    break;

                case 10: // chuyển xử lý
                    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    qoKhieuNaiTonDong_310.Start = 0;
                    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    gpTonDong_310.Fields = listGroupField;
                    gpTonDong_310.Limit = 1;
                    gpTonDong_310.Main = true;
                    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Start = 0;
                    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    gpChuyenXuLy.Fields = listGroupField;
                    gpChuyenXuLy.Limit = 1;
                    gpChuyenXuLy.Main = true;
                    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    }

                    break;

                case 11: // chuyển phản hồi
                    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    qoKhieuNaiTonDong_311.Start = 0;
                    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    gpTonDong_311.Fields = listGroupField;
                    gpTonDong_311.Limit = 1;
                    gpTonDong_311.Main = true;
                    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    whereClause = string.Format("((-NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND NgayTiepNhan_NguoiXuLy:[* TO {0}]) OR (NgayTiepNhan_NguoiXuLy : [9999-12-31T00:00:00.000Z TO 9999-12-31T23:59:59.999Z] AND  NgayTiepNhan:[* TO {0}])) AND KhieuNai_NgayDongKN:[{1} TO *]{2}{3}",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate), dkTinh, dkHuyen);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE) || obj.NguoiXuLy != tenTruyCap; });
                        //int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId || (obj.PhongBanXuLyId != phongBanId && phongBanId != NO_VALUE); });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NguoiXuLyTruoc,PhongBanXuLyTruocId, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, NgayTiepNhan_PhongBanXuLyTruoc, NgayTiepNhan_NguoiXuLyTruoc, NgayQuaHan_PhongBanXuLyTruoc");

                    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    gpChuyenPhanHoi.Fields = listGroupField;
                    gpChuyenPhanHoi.Limit = 1;
                    gpChuyenPhanHoi.Main = true;
                    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    tenTruyCapSolr = tenTruyCap;
                    if (tenTruyCapSolr.Trim().Length == 0)
                    {
                        tenTruyCapSolr = "\"\"";
                    }

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId: {2} AND NguoiXuLyTruoc:{3} AND -NguoiXuLy : {3}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                    if (phongBanId == NO_VALUE)
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND DoiTacXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    else
                    {
                        //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -NguoiXuLy : {3} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId :{2} AND NguoiXuLy:{3} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr);
                        if (tenTruyCap.Length == 0)
                        {
                            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                        else
                        {
                            whereClause = string.Format("(LDate_ActivityTruoc:[{0} TO {1}] AND PhongBanXuLyTruocId :{2} AND NguoiXuLyTruoc: {3} AND -HanhDong:(0 4)) OR (LDate:[{0} TO {1}] AND  NguoiXuLy:{3} AND HanhDong:4){4}{5}",
                                ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId, tenTruyCapSolr, dkTinh, dkHuyen);
                        }
                    }
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    }

                    break;
            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }


        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/09/2014
        /// Todo : Danh sách chi tiết khiếu nại cho PTDV
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     1 : Danh sách khiếu nại tiếp nhận
        ///     2 : Danh sách khiếu nại đã đóng
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiToanMangCuaPTDV_Solr(int khuVucId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
            string whereClause = string.Empty;


            List<DoiTacInfo> listDoiTac = null;
            if (khuVucId != DoiTacInfo.DoiTacIdValue.VNP)
            {
                listDoiTac = new DoiTacImpl().GetListByDonViTrucThuoc(khuVucId);
                if (listDoiTac != null && listDoiTac.Count > 0)
                {
                    whereClause = listDoiTac[0].Id.ToString();
                    for (int i = 1; i < listDoiTac.Count; i++)
                    {
                        whereClause = string.Format("{0} {1}", whereClause, listDoiTac[i].Id);
                    }
                }
            }


            SolrQuery solrQuery = null;

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, NgayTiepNhan, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, NgayDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            List<SortOrder> listSortOrder = new List<SortOrder>();
            SortOrder sortOrderNgay = null;

            switch (reportType)
            {
                case 1:
                    sortOrderNgay = new SortOrder("NgayTiepNhan", Order.ASC);
                    listSortOrder.Add(sortOrderNgay);

                    qoKhieuNai.OrderBy = listSortOrder;

                    if (listDoiTac != null && listDoiTac.Count > 0)
                    {
                        whereClause = string.Format("DoiTacId : ({0}) AND ", whereClause);
                    }

                    whereClause = string.Format("{0} NgayTiepNhan:[{1} TO {2}] AND LoaiKhieuNaiId:{3} AND LinhVucChungId:{4} AND LinhVucConId:{5}", whereClause,
                                                        ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999),
                                                        loaiKhieuNaiId, linhVucChungId, linhVucConId);
                    solrQuery = new SolrQuery(whereClause);

                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;
                case 2:
                    sortOrderNgay = new SortOrder("NgayDongKN", Order.ASC);
                    listSortOrder.Add(sortOrderNgay);

                    qoKhieuNai.OrderBy = listSortOrder;

                    if (listDoiTac != null && listDoiTac.Count > 0)
                    {
                        whereClause = string.Format("DoiTacXuLyId : ({0}) AND ", whereClause);
                    }

                    whereClause = string.Format("{0} NgayDongKN:[{1} TO {2}] AND LoaiKhieuNaiId:{3} AND LinhVucChungId:{4} AND LinhVucConId:{5}", whereClause,
                                                        ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999),
                                                        loaiKhieuNaiId, linhVucChungId, linhVucConId);
                    solrQuery = new SolrQuery(whereClause);

                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;
            }

            return listKhieuNaiInfo;
        }

        #region Private methods
        /// <summary>
        /// Author: nvhung 27/10/2014
        /// Sort danh sach khieu nai theo NgayTiepNhan tang dan
        /// </summary>
        /// <param name="lstKhieuNai"></param>
        /// <returns></returns>
        private List<KhieuNai_ReportInfo> SortNgayTiepNhanASCByLinQ(List<KhieuNai_ReportInfo> lstKhieuNai)
        {
            if (lstKhieuNai == null || lstKhieuNai.Count == 0 || lstKhieuNai[0].NgayTiepNhan == null) return lstKhieuNai;

            var sortedList = (from p in lstKhieuNai
                              orderby p.NgayTiepNhan ascending
                              select p
                                   );
            List<KhieuNai_ReportInfo> newObj = sortedList.ToList<KhieuNai_ReportInfo>();
            return newObj;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/08/2014
        /// Todo : Sắp xếp theo thứ tự ngày tiếp nhận tăng dần 
        /// </summary>
        /// <param name="listKhieuNai"></param>
        /// <returns></returns>               
        private List<KhieuNai_ReportInfo> SortListByNgayTiepNhanASC(List<KhieuNai_ReportInfo> listKhieuNai)
        {
            if (listKhieuNai == null || listKhieuNai.Count == 0 || listKhieuNai[0].NgayTiepNhan == null) return listKhieuNai;

            for (int i = 0; i < listKhieuNai.Count - 1; i++)
            {
                for (int j = i + 1; j < listKhieuNai.Count; j++)
                {
                    if (listKhieuNai[i].NgayTiepNhan > listKhieuNai[j].NgayTiepNhan)
                    {
                        KhieuNai_ReportInfo objTemp = listKhieuNai[i];
                        listKhieuNai[i] = listKhieuNai[j];
                        listKhieuNai[j] = objTemp;
                    }
                }

            }

            return listKhieuNai;
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 19/04/2015
        ///// </summary>
        ///// <param name="listKhieuNaiInfo"></param>
        //private void SetValueGhiChuKhieuNai(List<KhieuNai_ReportInfo> listKhieuNaiInfo)
        //{
        //    if (listKhieuNaiInfo == null) return;

        //    Dictionary<string, string> extraParam = new Dictionary<string, string>();
        //    extraParam.Add("fl", "Id, GhiChu");

        //    QueryOptions qoOption = new QueryOptions();
        //    qoOption.ExtraParams = extraParam;
        //    qoOption.Start = 0;
        //    qoOption.Rows = int.MaxValue;

        //    string whereClause = string.Empty;
        //    for(int i=0;i<listKhieuNaiInfo.Count;i++)
        //    {
        //        whereClause = string.Format("{0} {1}",whereClause, listKhieuNaiInfo[i].KhieuNaiId);
        //    }

        //    whereClause = string.Format("Id:({0})", whereClause.Trim());

        //    SolrQuery solrQuery = new SolrQuery(whereClause);

        //    QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, null, null); // Khởi tạo solr
        //    List<KhieuNai_ReportInfo> listResult = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolrPostMethod(URL_SOLR_GQKN, solrQuery, qoOption);

        //    if(listResult != null)
        //    {
        //        for(int i=0;i<listResult.Count;i++)
        //        {
        //            for(int j=0;j<listKhieuNaiInfo.Count;j++)
        //            {
        //                if(listResult[i].Id == listKhieuNaiInfo[j].KhieuNaiId)
        //                {
        //                    listKhieuNaiInfo[j].GhiChu = listResult[i].GhiChu;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //}

        #endregion

        #region Bao cao VNP

        /// <summary>
        /// Báo cáo tổng hợp theo loại khiếu nại toàn mạng vnp
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="LoaiKNId"></param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        /// created by: nvhung 07/11/2014
        public List<KhieuNai_ReportInfo> ListLoaiKhieuNaiVNP_Solr(int doiTacId, DateTime fromDate, DateTime toDate, int loaiKhieuNai_NhomId, int linhVucChungId, int nguonKhieuNai, int reportType)
        {
            List<KhieuNai_ReportInfo> lstKhieuNaiInfo = null;
            List<DoiTacInfo> lstDoiTac = null;
            //case theo từng loại : 1: Số lượng tiếp nhận 2: số lượng đã đóng trong kỳ 3: Số lượng đã đóng 4: số lượng quá hạn toàn trình 5: Số lượng quá hạn toàn trình trong kỳ
            string sFromDate = fromDate.ToString("yyyyMMdd");
            string sToDate = toDate.ToString("yyyyMMdd");
            string sNextToDate = toDate.AddDays(1).ToString("yyyyMMdd");
            //lấy danh sách doitac có donvitructhuoc thuộc khuvucID
            string sKhuVucId = string.Empty;
            string sKhuVucXuLyId = string.Empty;
            if (doiTacId != DoiTacInfo.DoiTacIdValue.VNP)//neu don vi lay bao cao =VNP (toàn mạng)
            {
                sKhuVucId = " AND KhuVucId:" + doiTacId.ToString();
                sKhuVucXuLyId = " AND KhuVucXuLyId:" + doiTacId.ToString();
            }

            string whereClauseNguonKhieuNai = string.Empty;
            if (nguonKhieuNai != -1)
            {
                whereClauseNguonKhieuNai = string.Format(" AND KhieuNaiFrom:{0}", nguonKhieuNai);
            }

            Dictionary<string, Order> dSortOrders = new Dictionary<string, Order>();
            dSortOrders.Add("NgayTiepNhan", Order.DESC);
            dSortOrders.Add("Id", Order.DESC);
            string fList = "Id,MaKhieuNai,SoThueBao,NgayTiepNhan,NguoiTiepNhan,NguoiXuLy,NgayQuaHan,LoaiKhieuNaiId,NgayDongKN";

            switch (reportType)
            {
                case 1: //Số lượng tiếp nhận kỳ trước
                    string sWhereKyTruoc = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND LoaiKhieuNaiId:{2} AND LinhVucChungId:{5} {3} {4}", sFromDate, sToDate, loaiKhieuNai_NhomId, sKhuVucId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhereKyTruoc, dSortOrders);
                    break;
                case 2: //Số lượng tiếp nhận
                    string sWhere = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND LoaiKhieuNaiId:{2} AND LinhVucChungId:{5} {3} {4}", sFromDate, sToDate, loaiKhieuNai_NhomId, sKhuVucId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhere, dSortOrders);
                    break;
                case 3://Số lượng khiếu nại đã đóng trong khoảng thời gian báo cáo
                    string sWhereDaDongTK = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND NgayDongKNSort:[{0} TO {1}] AND LoaiKhieuNaiId:{2} AND LinhVucChungId:{5} {3} {4}", sFromDate, sToDate, loaiKhieuNai_NhomId, sKhuVucXuLyId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhereDaDongTK, dSortOrders);
                    break;
                case 4:  //Số lượng đã đóng (bao gom ton ky truoc)    
                    string sWhereDaDong = string.Format("NgayDongKNSort:[{0} TO {1}] AND LoaiKhieuNaiId:{2} AND LinhVucChungId:{5} {3} {4}", sFromDate, sToDate, loaiKhieuNai_NhomId, sKhuVucXuLyId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhereDaDong, dSortOrders);
                    break;
                case 5://Số lượng toàn trình quá hạn trong kỳ                    
                    string sWhereQHAndTTK = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND (NgayDongKNSort:[{2} TO *] OR NgayDongKNSort : 0) AND NgayQuaHanSort:[* TO {1}] AND LoaiKhieuNaiId:{3} AND LinhVucChungId:{6} {4} {5}", sFromDate, sToDate, sNextToDate, loaiKhieuNai_NhomId, sKhuVucXuLyId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhereQHAndTTK, dSortOrders);
                    break;
                case 6://Số lượng toàn trình quá hạn 
                    string sWhereQH = string.Format("(NgayDongKNSort:[{0} TO *] OR NgayDongKNSort : 0) AND NgayQuaHanSort:[* TO {1}] AND LoaiKhieuNaiId:{2} AND LinhVucChungId:{5} {3} {4}", sNextToDate, sToDate, loaiKhieuNai_NhomId, sKhuVucXuLyId, whereClauseNguonKhieuNai, linhVucChungId);
                    lstKhieuNaiInfo = GetDataFromSolr(URL_SOLR_GQKN, fList, sWhereQH, dSortOrders);
                    break;
            }

            lstKhieuNaiInfo = SortNgayTiepNhanASCByLinQ(lstKhieuNaiInfo);
            return lstKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/01/2015
        /// Todo : Lấy danh sách khiếu nại toàn mạng theo tuần
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     = 1 : Số lượng KN tiếp nhận trong tuần
        ///     = 2 : Số lượng KN đã giải quyết trong tuần
        ///     = 3 : Số lượng KN quá hạn (chưa đóng) trong tuần
        ///     = 4 : Số lượng KN đã giải quyết từ đầu tháng đến hết ngày toDate
        ///     = 5 : Số lượng KN đã giải quyết từ đầu năm đến hết ngày toDate
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiToanMangTheoTuan(DateTime fromDate, DateTime toDate, int nguonKhieuNai, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = null;
            string sFromDate = fromDate.ToString("yyyyMMdd");
            string sToDate = toDate.ToString("yyyyMMdd");
            string whereClauseNguonKhieuNai = string.Empty;
            if (nguonKhieuNai != -1)
            {
                whereClauseNguonKhieuNai = string.Format(" AND KhieuNaiFrom:{0}", nguonKhieuNai);
            }

            SolrQuery solrQuery = null;

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, NgayTiepNhan, NgayQuaHan, NgayDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            switch (reportType)
            {
                case 1: // SL tiếp nhận trong tuần
                    string whereClauseTiepNhan = string.Format("NgayTiepNhanSort:[{0} TO {1}] {2}", sFromDate, sToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseTiepNhan);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 2: // SL đã giải quyết trong tuần
                    //string whereClauseDaDong = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND NgayDongKNSort:[{0} TO {1}]", sFromDate, sToDate);
                    string whereClauseDaDong = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sFromDate, sToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseDaDong);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 3: // SL quá hạn trong tuần
                    string sNextToDate = toDate.AddDays(1).ToString("yyyyMMdd");
                    string whereClauseQuaHan = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND (NgayDongKNSort:[{2} TO *] OR NgayDongKNSort : 0) AND NgayQuaHanSort:[* TO {1}] {3}", sFromDate, sToDate, sNextToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseQuaHan);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 4: // SL Giải quyết từ đầu tháng đến toDate
                    string sStartMonth = new DateTime(toDate.Year, toDate.Month, 1).ToString("yyyyMMdd");
                    string whereClauseDaDongTrongThang = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sStartMonth, sToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseDaDongTrongThang);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 5: // SL đã giải quyết từ đầu năm đến toDate
                    string sStartYear = new DateTime(toDate.Year, 1, 1).ToString("yyyyMMdd");
                    string whereClauseDaDongTuDauNam = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sStartYear, sToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseDaDongTuDauNam);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                default:

                    break;
            }

            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/01/2015
        /// Todo : Lấy danh sách khiếu nại toàn mạng theo tháng
        /// </summary>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     = 1 : SL đã giải quyết tháng hiện tại
        ///     = 2 : SL đã giải quyết tháng trước
        ///     = 3 : SL đã giải quyết từ đầu năm đến cuối tháng toDate
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     -1 : all
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiToanMangTheoThang(DateTime toDate, int nguonKhieuNai, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = null;
            string sFromDate = new DateTime(toDate.Year, toDate.Month, 1).ToString("yyyyMMdd");
            string sToDate = new DateTime(toDate.Year, toDate.Month, DateTime.DaysInMonth(toDate.Year, toDate.Month)).ToString("yyyyMMdd");

            DateTime nextToDate = toDate.AddMonths(1);
            string sNextToDate = new DateTime(nextToDate.Year, nextToDate.Month, 1).ToString("yyyyMMdd");

            DateTime previousDate = toDate.AddMonths(-1);
            string sPreviousFromDate = new DateTime(previousDate.Year, previousDate.Month, 1).ToString("yyyyMMdd");
            string sPreviousToDate = new DateTime(previousDate.Year, previousDate.Month, DateTime.DaysInMonth(previousDate.Year, previousDate.Month)).ToString("yyyyMMdd");

            string sFromYear = new DateTime(toDate.Year, 1, 1).ToString("yyyyMMdd");
            string sToYear = new DateTime(toDate.Year, toDate.Month, DateTime.DaysInMonth(toDate.Year, toDate.Month)).ToString("yyyyMMdd");

            string whereClauseNguonKhieuNai = string.Empty;
            if (nguonKhieuNai != -1)
            {
                whereClauseNguonKhieuNai = string.Format(" AND KhieuNaiFrom:{0}", nguonKhieuNai);
            }

            SolrQuery solrQuery = null;

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, NgayDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            switch (reportType)
            {
                case 1: // SL đã giải quyết tháng hiện tại
                    string whereClauseSLDaGiaiQuyetThangNay = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sFromDate, sToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseSLDaGiaiQuyetThangNay);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 2: // SL đã giải quyết tháng trước
                    string whereClauseSLDaGiaiQuyetThangTruoc = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sPreviousFromDate, sPreviousToDate, whereClauseNguonKhieuNai);
                    solrQuery = new SolrQuery(whereClauseSLDaGiaiQuyetThangTruoc);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                case 3: // SL đã giải quyết từ đầu năm đến cuối tháng toDate
                    string whereClauseDaDongTuDauNam = string.Format("NgayDongKNSort:[{0} TO {1}] AND TrangThai:3 {2}", sFromYear, sToYear, whereClauseNguonKhieuNai);
                    //string whereClauseQuaHan = string.Format("NgayTiepNhanSort:[{0} TO {1}] AND (NgayDongKNSort:[{2} TO *] OR NgayDongKNSort : 0) AND NgayQuaHanSort:[* TO {1}]", sFromDate, sToDate, sNextToDate);
                    solrQuery = new SolrQuery(whereClauseDaDongTuDauNam);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                default:

                    break;
            }

            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/06/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="nguyenNhanLoiId"></param>
        /// <param name="chiTietLoiId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoNguyenNhanLoi(int khuVucId, int doiTacId, int phongBanId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int nguyenNhanLoiId, int chiTietLoiId,
                                                DateTime fromDate, DateTime toDate, int nguonKhieuNai)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = null;
            string whereClause = string.Format("LoaiKhieuNaiId:{0} AND NgayTiepNhanSort:[{1} TO {2}] AND NgayDongKNSort:[{1} TO {2}]", loaiKhieuNaiId, fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));

            if (linhVucChungId >= 0)
            {
                whereClause = string.Format("{0} AND LinhVucChungId:{1}", whereClause, linhVucChungId);
            }

            if (linhVucConId >= 0)
            {
                whereClause = string.Format("{0} AND LinhVucConId:{1}", whereClause, linhVucConId);
            }

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, KhuVucXuLyId, DoiTacXuLyId, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayDongKN, NoiDungPA, LyDoGiamTru, ChiTietLoiId");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            if (khuVucId > 1)
            {
                whereClause = string.Format("{0} AND KhuVucXuLyId:{1}", whereClause, khuVucId);
            }

            if (doiTacId > 0)
            {
                whereClause = string.Format("{0} AND DoiTacXuLyId:{1}", whereClause, doiTacId);
            }

            if (phongBanId > 0)
            {
                whereClause = string.Format("{0} AND PhongBanXuLyId:{1}", whereClause, phongBanId);
            }

            if (nguyenNhanLoiId > 0)
            {
                if (nguyenNhanLoiId == LoiKhieuNaiInfo.LoiKhieuNaiValue.NGUYEN_NHAN_LOI_ID_KHAC)
                {
                    whereClause = string.Format("{0} AND (LyDoGiamTru:{1} OR LyDoGiamTru:0 OR ChiTietLoiId:[* TO -1])", whereClause, nguyenNhanLoiId);
                }
                else
                {
                    whereClause = string.Format("{0} AND LyDoGiamTru:{1}", whereClause, nguyenNhanLoiId);
                }

            }

            if (nguyenNhanLoiId > 0 && chiTietLoiId >= 0)
            {
                whereClause = string.Format("{0} AND ChiTietLoiId:{1}", whereClause, chiTietLoiId);
            }

            if (nguonKhieuNai > -1)
            {
                whereClause = string.Format("{0} AND KhieuNaiFrom:{1}", whereClause, nguonKhieuNai);
            }

            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);

            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 23/06/2015
        /// Todo : Lấy danh sách khiếu nại chất lượng phục vụ
        /// </summary>
        /// <param name="typeReport">
        ///     1 : Toàn mạng
        ///     2 : Khu vực
        ///     3 : Đối tác
        /// </param>
        /// <param name="doiTacId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="khenChe">
        ///     = 1 : Khen
        ///     = 2 : Chê
        /// </param>
        /// <param name="nguonKhieuNai">
        ///     = -1 : all
        /// </param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiChatLuongPhucVu(int typeReport, int doiTacId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int typeKhenChe, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();
            string whereClause = string.Empty;
            string sFromDate = ConvertUtility.ConvertDateTimeToSolr(fromDate);
            string sToDate = ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999);
            whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND LoaiKhieuNaiId:{2}", sFromDate, sToDate, loaiKhieuNaiId);
            if (linhVucChungId > 0)
            {
                whereClause = string.Format("{0} AND LinhVucChungId:{1}", whereClause, linhVucChungId);
            }
            else
            {
                string whereClauseLinhVucChung = "ParentId=" + loaiKhieuNaiId;
                if (typeKhenChe == 1)
                {
                    whereClauseLinhVucChung = string.Format("{0} AND Name LIKE N'Khen%'", whereClauseLinhVucChung);
                }
                else
                {
                    whereClauseLinhVucChung = string.Format("{0} AND Name LIKE N'Chê%'", whereClauseLinhVucChung);
                }

                List<LoaiKhieuNaiInfo> listLinhVucChung = new LoaiKhieuNaiImpl().GetListDynamic("*", whereClauseLinhVucChung, "");
                if (listLinhVucChung != null && listLinhVucChung.Count > 0)
                {
                    whereClause = string.Format("{0} AND LinhVucChungId:{1}", whereClause, listLinhVucChung[0].Id);
                }
            }

            if (linhVucConId > 0)
            {
                whereClause = string.Format("{0} AND LinhVucConId:{1}", whereClause, linhVucConId);
            }


            if (nguonKhieuNai != -1)
            {
                whereClause = string.Format("{0} AND KhieuNaiFrom:{1}", whereClause, nguonKhieuNai);
            }

            if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP1 || doiTacId == DoiTacInfo.DoiTacIdValue.VNP2 || doiTacId == DoiTacInfo.DoiTacIdValue.VNP3)
            {
                whereClause = string.Format("{0} AND KhuVucId:{1}", whereClause, doiTacId);
            }
            else if (doiTacId > DoiTacInfo.DoiTacIdValue.VNP)
            {
                whereClause = string.Format("{0} AND DoiTacId:{1}", whereClause, doiTacId);
            }

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, KhuVucId, DoiTacId, NoiDungPA, NgayTiepNhan, NguoiTiepNhan, NoiDungXuLyDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/07/2015
        /// </summary>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoLoaiKhieuNai(int khuVucId, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();
            string sFromDate = ConvertUtility.ConvertDateTimeToSolr(fromDate);
            string sToDate = ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999);

            string whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND NgayDongKN:[{0} TO {1}]", sFromDate, sToDate);

            if (khuVucId != DoiTacInfo.DoiTacIdValue.VNP)
            {
                whereClause = string.Format("{0} AND KhuVucXuLyId:{1}", whereClause, khuVucId);
            }

            if (nguonKhieuNai != -1)
            {
                whereClause = string.Format("{0} AND KhieuNaiFrom:{1}", whereClause, nguonKhieuNai);
            }

            string whereClauseLoaiKhieuNai = string.Empty;
            string whereClauseLinhVucChung = string.Empty;
            string whereClauseLinhVucCon = string.Empty;

            if (listLoaiKhieuNaiId != null && listLoaiKhieuNaiId.Count > 0)
            {
                whereClauseLoaiKhieuNai = string.Format(" AND LoaiKhieuNaiId:({0}", listLoaiKhieuNaiId[0]);
                for (int i = 1; i < listLoaiKhieuNaiId.Count; i++)
                {
                    whereClauseLoaiKhieuNai = string.Format("{0} {1}", whereClauseLoaiKhieuNai, listLoaiKhieuNaiId[i]);
                }

                whereClauseLoaiKhieuNai = string.Format("{0})", whereClauseLoaiKhieuNai);
            }

            if (listLinhVucChungId != null && listLinhVucChungId.Count > 0)
            {
                whereClauseLinhVucChung = string.Format(" AND LinhVucChungId:(0 {0}", listLinhVucChungId[0]);
                for (int i = 1; i < listLinhVucChungId.Count; i++)
                {
                    whereClauseLinhVucChung = string.Format("{0} {1}", whereClauseLinhVucChung, listLinhVucChungId[i]);
                }

                whereClauseLinhVucChung = string.Format("{0})", whereClauseLinhVucChung);
            }

            if (listLinhVucConId != null && listLinhVucConId.Count > 0)
            {
                whereClauseLinhVucCon = string.Format(" AND LinhVucConId:(0 {0}", listLinhVucConId[0]);
                for (int i = 1; i < listLinhVucConId.Count; i++)
                {
                    whereClauseLinhVucCon = string.Format("{0} {1}", whereClauseLinhVucCon, listLinhVucConId[i]);
                }

                whereClauseLinhVucCon = string.Format("{0})", whereClauseLinhVucCon);
            }

            whereClause = string.Format("{0} {1} {2} {3}", whereClause, whereClauseLoaiKhieuNai, whereClauseLinhVucChung, whereClauseLinhVucCon);
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, LoaiKhieuNai, LinhVucChung, LinhVucCon, NgayTiepNhan, NgayDongKN, NguoiXuLy, NoiDungPA, NoiDungXuLyDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/07/2015
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="maTinhId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoLoaiKhieuNai(int khuVucId, List<int> listLoaiKhieuNaiId, List<int> listLinhVucChungId, List<int> listLinhVucConId, int maTinhId, int nguonKhieuNai, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();
            string sFromDate = ConvertUtility.ConvertDateTimeToSolr(fromDate);
            string sToDate = ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999);

            string whereClause = string.Format("NgayDongKN:[{0} TO {1}]", sFromDate, sToDate);

            if (maTinhId > 0)
            {
                whereClause = string.Format("{0} AND MaTinhId:{1}", whereClause, maTinhId);
            }
            else
            {
                // Khi lọc theo khu vực thì phải lấy theo tỉnh chứ không lấy theo KhuVucXuLyId (vì giờ đang thống kê theo Tỉnh có thuê bao khiếu nại)
                if (khuVucId != DoiTacInfo.DoiTacIdValue.VNP)
                {
                    List<ProvinceInfo> listProvince = new ProvinceImpl().GetListDynamic("*", "KhuVucId=" + khuVucId, "");
                    if (listProvince != null && listProvince.Count > 0)
                    {
                        whereClause = string.Format("{0} AND MaTinhId:({1}", whereClause, listProvince[0].Id);

                        for (int i = 1; i < listProvince.Count; i++)
                        {
                            whereClause = string.Format("{0} {1}", whereClause, listProvince[i].Id);
                        }

                        whereClause = string.Format("{0})", whereClause);
                    }
                }
            }




            if (nguonKhieuNai != -1)
            {
                whereClause = string.Format("{0} AND KhieuNaiFrom:{1}", whereClause, nguonKhieuNai);
            }

            string whereClauseLoaiKhieuNai = string.Empty;
            string whereClauseLinhVucChung = string.Empty;
            string whereClauseLinhVucCon = string.Empty;

            if (listLoaiKhieuNaiId != null && listLoaiKhieuNaiId.Count > 0)
            {
                whereClauseLoaiKhieuNai = string.Format(" AND LoaiKhieuNaiId:({0}", listLoaiKhieuNaiId[0]);
                for (int i = 1; i < listLoaiKhieuNaiId.Count; i++)
                {
                    whereClauseLoaiKhieuNai = string.Format("{0} {1}", whereClauseLoaiKhieuNai, listLoaiKhieuNaiId[i]);
                }

                whereClauseLoaiKhieuNai = string.Format("{0})", whereClauseLoaiKhieuNai);
            }

            if (listLinhVucChungId != null && listLinhVucChungId.Count > 0)
            {
                whereClauseLinhVucChung = string.Format(" AND LinhVucChungId:({0}", listLinhVucChungId[0]);
                for (int i = 1; i < listLinhVucChungId.Count; i++)
                {
                    whereClauseLinhVucChung = string.Format("{0} {1}", whereClauseLinhVucChung, listLinhVucChungId[i]);
                }

                whereClauseLinhVucChung = string.Format("{0})", whereClauseLinhVucChung);
            }

            if (listLinhVucConId != null && listLinhVucConId.Count > 0)
            {
                whereClauseLinhVucCon = string.Format(" AND LinhVucConId:({0}", listLinhVucConId[0]);
                for (int i = 1; i < listLinhVucConId.Count; i++)
                {
                    whereClauseLinhVucCon = string.Format("{0} {1}", whereClauseLinhVucCon, listLinhVucConId[i]);
                }

                whereClauseLinhVucCon = string.Format("{0})", whereClauseLinhVucCon);
            }

            whereClause = string.Format("{0} {1} {2} {3}", whereClause, whereClauseLoaiKhieuNai, whereClauseLinhVucChung, whereClauseLinhVucCon);
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, LoaiKhieuNai, LinhVucChung, LinhVucCon, NgayTiepNhan, NgayDongKN, NguoiXuLy, NoiDungPA, NoiDungXuLyDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author: Vu Van Truong
        /// Created date: 14/04/2016
        /// </summary>
        /// <param name="khuVucId"></param>
        /// <param name="listLoaiKhieuNaiId"></param>
        /// <param name="listLinhVucChungId"></param>
        /// <param name="listLinhVucConId"></param>
        /// <param name="maTinhId"></param>
        /// <param name="nguonKhieuNai"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoLoaiKhieuNaiActivity(string queryWhereClause)
        {
            SolrQuery solrQuery = null;
            SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            // 1. Tiếp nhận
            // Số lượng tiếp nhận
            QueryOptions queryOptionTiepNhan = new QueryOptions();
            //Lấy ra những trường nào
            //var extraParamLoaiKhieuNai = new Dictionary<string, string>();
            //extraParamLoaiKhieuNai.Add("fl", @"KhieuNaiId");
            //queryOptionTiepNhan.ExtraParams = extraParamLoaiKhieuNai;
            queryOptionTiepNhan.Start = 0;
            queryOptionTiepNhan.Rows = int.MaxValue;


            FacetParameters facetParamTiepNhan = new FacetParameters();
            SolrFacetPivotQuery facetPivotQueryTiepNhan = new SolrFacetPivotQuery();
            List<string> listPivotTiepNhan = new List<string>();
            listPivotTiepNhan.Add("MaTinhId,LoaiKhieuNaiId,LinhVucChungId,LinhVucConId,KhieuNaiId");
            facetPivotQueryTiepNhan.Fields = listPivotTiepNhan;
            facetParamTiepNhan.Queries.Add(facetPivotQueryTiepNhan);
            queryOptionTiepNhan.Facet = facetParamTiepNhan;


            SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
            List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
            listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

            GroupingParameters gpTiepNhan = new GroupingParameters();
            gpTiepNhan.Fields = listGroupField;
            gpTiepNhan.Limit = 1;
            gpTiepNhan.Main = true;
            gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
            queryOptionTiepNhan.Grouping = gpTiepNhan;
            //string whereClauseTiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] AND LoaiKhieuNaiId:71 AND HanhDong:(0 1 2 3)", sFromDate, sToDate);
            solrQuery = new SolrQuery(queryWhereClause);
            listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTiepNhan);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author: Vu Van Truong
        /// Created date: 19/04/2016
        /// </summary>
        /// <param name="queryWhereClause"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDanhSachKhieuNaiId(string queryWhereClause)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();
            string whereClause = string.Format("Id:({0})", queryWhereClause);
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, LoaiKhieuNai, LinhVucChung, LinhVucCon, NgayTiepNhan, NgayDongKN, NguoiXuLy, NoiDungPA, NoiDungXuLyDongKN");
            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;
            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author: Vu Van Truong
        /// Created date: 19/04/2016
        /// V2: Xử lý vấn đề truy vấn solr
        /// </summary>
        /// <param name="queryWhereClause"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDanhSachKhieuNaiId_V2(string queryWhereClause)
        {
            var lst = queryWhereClause.Split(' ').ToList<string>();
            int sliceSize = 500;

            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < lst.Count; i += sliceSize)
                list.Add(lst.GetRange(i, Math.Min(sliceSize, lst.Count - i)));


            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();

            for (int i = 0; i < list.Count; i++)
            {
                string dogCsv = string.Join(" ", list[i].ToArray());

                string whereClause = string.Format("Id:({0})", dogCsv);
                Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
                extraParamKhieuNai.Add("fl", "Id, SoThueBao, LoaiKhieuNai, LinhVucChung, LinhVucCon, NgayTiepNhan, NgayDongKN, NguoiXuLy, NoiDungPA, NoiDungXuLyDongKN");
                QueryOptions qoKhieuNai = new QueryOptions();
                qoKhieuNai.ExtraParams = extraParamKhieuNai;
                qoKhieuNai.Start = 0;
                qoKhieuNai.Rows = int.MaxValue;
                SolrQuery solrQuery = new SolrQuery(whereClause);
                listKhieuNaiReportInfo.AddRange(QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai));
            }

            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="ngayQuaHan"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiQuaHanPhongBan(int doiTacId, int phongBanId, DateTime ngayQuaHan)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = null;
            string sNgayQuaHan = ConvertUtility.ConvertDateTimeToSolr(ngayQuaHan, ngayQuaHan.Hour, ngayQuaHan.Minute, ngayQuaHan.Second, 999);
            DateTime nextToDate = ngayQuaHan.AddDays(1);

            string whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND (KhieuNai_NgayDongKN : [{1} TO *])", sNgayQuaHan, ConvertUtility.ConvertDateTimeToSolr(nextToDate));

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");
            QueryOptions qoKhieuNai = new QueryOptions();
            //Lấy ra những trường nào
            var extraParam = new Dictionary<string, string>();
            extraParam.Add("fl", @"Id,KhieuNaiId, SoThueBao, DoiTacXuLyId, PhongBanXuLyId, NgayTiepNhan, NgayQuaHan");
            qoKhieuNai.ExtraParams = extraParam;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

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
            qoKhieuNai.Grouping = gpTonDongKyTruoc;

            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNai);
            if (listKhieuNaiReportInfo != null && listKhieuNaiReportInfo.Count > 0)
            {
                listKhieuNaiReportInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.NgayQuaHan > ngayQuaHan || (doiTacId > 0 && obj.DoiTacXuLyId != doiTacId) || (phongBanId > 0 && obj.PhongBanXuLyId != phongBanId); });
            }

            return listKhieuNaiReportInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 25/11/2015
        /// </summary>
        /// <param name="linhVucConId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType">
        ///     = 1 : Số lượng tiếp nhận
        ///     = 2 : Số lượng đã đóng
        /// </param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiDichVuGTGTTapDoan(int linhVucConId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = null;
            string whereClause = string.Empty;
            SolrQuery solrQuery = null;

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, SoThueBao, NgayTiepNhan, LinhVucCon, NoiDungPA, NgayDongKN, NoiDungXuLyDongKN");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            // xu ly lấy lỗi thuộc về khiếu nại
            var dsLoiKn = "";
            List<LoiKhieuNaiInfo> lstLoiKN = new LoiKhieuNaiImpl().GetListDynamic("", "Loai=2 AND HoatDong=1 AND cap=2", "");
            if (lstLoiKN != null && lstLoiKN.Count > 0)
            {
                foreach (var item in lstLoiKN)
                {
                    dsLoiKn += item.Id + " ";
                }
                dsLoiKn = string.Format("AND ChiTietLoiId: ({0})", dsLoiKn);
            }


            switch (reportType)
            {
                case 1: // Số lượng tiếp nhận
                    whereClause = string.Format("NgayTiepNhanSort:[{0} TO {1}]  AND LoaiKhieuNaiId:(1265 1339 1467 1537 1661 1726) AND LinhVucConId:{2}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"), linhVucConId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;
                case 2: // Số lượng đã xử lý
                    //whereClause = string.Format("NgayDongKNSort:[{0} TO {1}] AND LoaiKhieuNaiId:(1265 1339 1467 1537 1661 1726) AND ChiTietLoiId:(8 9 10 11 12 13 14 16 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 34 35 48 49 50 51 52 53 55 57 59 60 61 62 63 64 65 66 68 70 73 76 78 80 81 83 84 85 86 87 89 90 91 92 94 96 97 99 100 102 106 107 108 109 110 114 117 118 119 127 128 130 136 137 144 145 150 152 155 158 160 161 163 164 165 166 170 172 175 176 178) AND LinhVucConId:{2}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"), linhVucConId);
                    whereClause = string.Format("NgayDongKNSort:[{0} TO {1}] AND LoaiKhieuNaiId:(1265 1339 1467 1537 1661 1726) {3} AND LinhVucConId:{2}", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"), linhVucConId, dsLoiKn);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                    break;

                default:

                    break;
            }

            return listKhieuNaiReportInfo;
        }

        #endregion

        #region Bao cao doi tac VNP

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/07/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiChuyenXuLyVNPTheoUser(int doiTacId, int phongBanId, string nguoiXuLy, DateTime fromDate, DateTime toDate)
        {
            string sFromDate = ConvertUtility.ConvertDateTimeToSolr(fromDate);
            string sToDate = ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999);

            List<KhieuNai_ReportInfo> listKhieuNaiReportInfo = new List<KhieuNai_ReportInfo>();

            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", "Id, KhieuNaiId, SoThueBao, NguoiXuLyTruoc, NgayTiepNhan, TenPhongBanXuLy");

            QueryOptions qoKhieuNai = new QueryOptions();
            qoKhieuNai.ExtraParams = extraParamKhieuNai;
            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;

            List<SolrNet.SortOrder> listSortOrder = new List<SolrNet.SortOrder>();
            listSortOrder.Add(new SolrNet.SortOrder("Id", Order.DESC));

            List<string> listField = new List<string>();
            listField.Add("KhieuNaiId");
            GroupingParameters gp = new GroupingParameters();
            gp.Fields = listField;
            gp.Limit = 1;
            gp.Main = true;
            gp.OrderBy = listSortOrder;
            qoKhieuNai.Grouping = gp;

            string whereClause = string.Format("NgayTiepNhan : [{0} TO {1}] AND HanhDong:2 AND DoiTacXuLyTruocId:{2} AND DoiTacXuLyId:({3} {4} {5})", sFromDate, sToDate, doiTacId, DoiTacInfo.DoiTacIdValue.DKT1, DoiTacInfo.DoiTacIdValue.DKT2, DoiTacInfo.DoiTacIdValue.DKT3);
            SolrQuery solrQuery = new SolrQuery(whereClause);
            listKhieuNaiReportInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNai);
            listKhieuNaiReportInfo = listKhieuNaiReportInfo.FindAll(delegate (KhieuNai_ReportInfo obj) { return obj.NguoiXuLyTruoc == nguoiXuLy; });

            return listKhieuNaiReportInfo;
        }

        #endregion

        #region VNPT NET

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/10/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoVNPTX(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, KhuVucXuLyId, LDate, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);

                    // List<KhieuNai_ReportInfo> newObjs = listKhieuNaiInfo.Where(v => v.KhuVucXuLyId == doiTacId).ToList();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, KhuVucXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, DoiTacXuLyId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND KhuVucXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  KhuVucXuLyTruocId: {2} AND -KhuVucXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  KhuVucXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  KhuVucXuLyTruocId: {2} AND -KhuVucXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  KhuVucXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                // Dương Dv
                // Xử lý lỗi không đúng dữ liệu
                case 6: // Số lượng tồn đọng quá hạn                    
                    Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Start = 0;
                    qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    gpTonDongQuaHan.Fields = listGroupField;
                    gpTonDongQuaHan.Limit = 1;
                    gpTonDongQuaHan.Main = true;
                    gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]",
                        ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999),
                        ConvertUtility.ConvertDateTimeToSolr(nextToDate)
                        );
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);

                    if (listKhieuNaiInfo != null) listKhieuNaiInfo = listKhieuNaiInfo.Where(v => v.KhuVucXuLyId == doiTacId && toDate.AddDays(1) >= v.NgayQuaHan).ToList();

                    // Phiên bản cũ
                    // if (listKhieuNaiInfo != null)
                    // {
                    //     int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                    //     {
                    //         return obj.KhuVucXuLyId != doiTacId || toDate < obj.NgayQuaHan;
                    //    });
                    // }
                    //  var objRet = listKhieuNaiInfo.Where(v => v.KhuVucXuLyId == doiTacId && toDate.AddDays(1) >= v.NgayQuaHan);

                    break;

                case 7: // Số lượng xử lý của số lượng tiếp nhận trong kỳ báo cáo
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_7 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_7.Add("fl", "KhieuNaiId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonKyTruoc_7 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_7.ExtraParams = extraParamTonKyTruoc_7;
                    qoKhieuNaiTonKyTruoc_7.Start = 0;
                    qoKhieuNaiTonKyTruoc_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyNgayTiepNhan_7);
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyActivityId_7);

                    GroupingParameters gpTonDongKyTruoc_7 = new GroupingParameters();
                    gpTonDongKyTruoc_7.Fields = listGroupField;
                    gpTonDongKyTruoc_7.Limit = 1;
                    gpTonDongKyTruoc_7.Main = true;
                    gpTonDongKyTruoc_7.OrderBy = listSortOrderTonDongTruocKy_7;
                    qoKhieuNaiTonKyTruoc_7.Grouping = gpTonDongKyTruoc_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_7);
                    List<int> listKhieuNaiIdTonDongKyTruoc_7 = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan_7 = new Dictionary<string, string>();
                    extraParamTiepNhan_7.Add("fl", "KhieuNaiId");

                    QueryOptions qoKhieuNaiTiepNhan_7 = new QueryOptions();
                    qoKhieuNaiTiepNhan_7.ExtraParams = extraParamTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Start = 0;
                    qoKhieuNaiTiepNhan_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan_7.Add(sortOrderNgayTiepNhan_7);

                    GroupingParameters gpTiepNhan_7 = new GroupingParameters();
                    gpTiepNhan_7.Fields = listGroupField;
                    gpTiepNhan_7.Limit = 1;
                    gpTiepNhan_7.Main = true;
                    gpTiepNhan_7.OrderBy = listSortOrderNgayTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Grouping = gpTiepNhan_7;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND KhuVucXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan_7);

                    if (listKhieuNaiIdTonDongKyTruoc_7 != null && listKhieuNaiIdTonDongKyTruoc_7.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc_7.Contains(obj.KhieuNaiId); });
                    }

                    // listKhieuNaiIdTiepNhan
                    List<int> listKhieuNaiIdTiepNhan = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTiepNhan.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTonDong_7 = new Dictionary<string, string>();
                    extraParamTonDong_7.Add("fl", "KhieuNaiId, KhuVucXuLyId");

                    QueryOptions qoKhieuNaiTonDong_7 = new QueryOptions();
                    qoKhieuNaiTonDong_7.ExtraParams = extraParamTonDong_7;
                    qoKhieuNaiTonDong_7.Start = 0;
                    qoKhieuNaiTonDong_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_7.Add(sortOrderTonDongNgayTiepNhan_7);
                    listSortOrderTonDong_7.Add(sortOrderTonDongActivityId_7);

                    GroupingParameters gpTonDong_7 = new GroupingParameters();
                    gpTonDong_7.Fields = listGroupField;
                    gpTonDong_7.Limit = 1;
                    gpTonDong_7.Main = true;
                    gpTonDong_7.OrderBy = listSortOrderTonDong_7;
                    qoKhieuNaiTonDong_7.Grouping = gpTonDong_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_7);
                    List<int> listKhieuNaiIdTonDong_7 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.KhuVucXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy_7 = new Dictionary<string, string>();
                    extraParamXuLy_7.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLy_7 = new QueryOptions();
                    qoKhieuNaiXuLy_7.ExtraParams = extraParamXuLy_7;
                    qoKhieuNaiXuLy_7.Start = 0;
                    qoKhieuNaiXuLy_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy_7.Add(sortOrderNgayXuLy_7);

                    GroupingParameters gpXuLy_7 = new GroupingParameters();
                    gpXuLy_7.Fields = listGroupField;
                    gpXuLy_7.Limit = 1;
                    gpXuLy_7.Main = true;
                    gpXuLy_7.OrderBy = listSortOrderNgayXuLy_7;
                    qoKhieuNaiXuLy_7.Grouping = gpXuLy_7;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  KhuVucXuLyTruocId: {2} AND -KhuVucXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  KhuVucXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy_7);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_7.Contains(obj.KhieuNaiId); });
                        numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return !listKhieuNaiIdTiepNhan.Contains(obj.KhieuNaiId); });
                    }

                    break;

                    #region Code số liệu không dùng

                    //case 7: // Số lượng tiếp nhận
                    //    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    //    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan");

                    //    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    //    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    //    qoKhieuNaiTaoMoi.Start = 0;
                    //    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);

                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    //    break;

                    //case 8: // Số lượng đã đóng
                    //    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    //    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy");

                    //    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    //    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    //    qoKhieuNaiDaDong.Start = 0;
                    //    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    //    break;

                    //case 9: // chuyển ngang hàng
                    //    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    //    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    //    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    //    qoKhieuNaiTonDong_39.Start = 0;
                    //    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    //    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    //    gpTonDong_39.Fields = listGroupField;
                    //    gpTonDong_39.Limit = 1;
                    //    gpTonDong_39.Main = true;
                    //    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    //    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    //    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    //    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    //    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Start = 0;
                    //    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    //    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    //    gpChuyenNgangHang.Fields = listGroupField;
                    //    gpChuyenNgangHang.Limit = 1;
                    //    gpChuyenNgangHang.Main = true;
                    //    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    //    }

                    //    break;

                    //case 10: // chuyển xử lý
                    //    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    //    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    //    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    //    qoKhieuNaiTonDong_310.Start = 0;
                    //    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    //    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    //    gpTonDong_310.Fields = listGroupField;
                    //    gpTonDong_310.Limit = 1;
                    //    gpTonDong_310.Main = true;
                    //    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    //    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    //    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    //    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    //    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Start = 0;
                    //    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    //    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    //    gpChuyenXuLy.Fields = listGroupField;
                    //    gpChuyenXuLy.Limit = 1;
                    //    gpChuyenXuLy.Main = true;
                    //    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    //    }

                    //    break;

                    //case 11: // chuyển phản hồi
                    //    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    //    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    //    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    //    qoKhieuNaiTonDong_311.Start = 0;
                    //    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    //    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    //    gpTonDong_311.Fields = listGroupField;
                    //    gpTonDong_311.Limit = 1;
                    //    gpTonDong_311.Main = true;
                    //    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    //    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    //    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    //    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    //    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    //    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    //    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    //    gpChuyenPhanHoi.Fields = listGroupField;
                    //    gpChuyenPhanHoi.Limit = 1;
                    //    gpChuyenPhanHoi.Main = true;
                    //    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    //    }

                    //    break;

                    #endregion

            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/10/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoDoiTac_V2_NET(int doiTacId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, KhuVucXuLyId, LDate, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);

                    int objs = listKhieuNaiInfo.Count(v => v.KhuVucXuLyId == doiTacId);

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId, ActivityId, DoiTacXuLyId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                // Số lượng tồn đọng quá hạn
                // DuongDv
                // Sai số liệu, điều chỉnh lại
                case 6:
                    //Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    //extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    //qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    //qoKhieuNaiTonDongQuaHan.Start = 0;
                    //qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    //SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    //listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    //listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    //GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    //gpTonDongQuaHan.Fields = listGroupField;
                    //gpTonDongQuaHan.Limit = 1;
                    //gpTonDongQuaHan.Main = true;
                    //gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    //qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    //whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //solrQuery = new SolrQuery(whereClause);
                    //listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    //if (listKhieuNaiInfo != null)
                    //{
                    //    int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                    //    {
                    //        return obj.DoiTacXuLyId != doiTacId || toDate < obj.NgayQuaHan;
                    //    });
                    //}

                    listKhieuNaiInfo = LayKhieuNaiTonDongQuaHan(doiTacId, fromDate, toDate);
                    break;

                case 7: // Số lượng xử lý của số lượng tiếp nhận trong kỳ báo cáo
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_7 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_7.Add("fl", "KhieuNaiId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonKyTruoc_7 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_7.ExtraParams = extraParamTonKyTruoc_7;
                    qoKhieuNaiTonKyTruoc_7.Start = 0;
                    qoKhieuNaiTonKyTruoc_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyNgayTiepNhan_7);
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyActivityId_7);

                    GroupingParameters gpTonDongKyTruoc_7 = new GroupingParameters();
                    gpTonDongKyTruoc_7.Fields = listGroupField;
                    gpTonDongKyTruoc_7.Limit = 1;
                    gpTonDongKyTruoc_7.Main = true;
                    gpTonDongKyTruoc_7.OrderBy = listSortOrderTonDongTruocKy_7;
                    qoKhieuNaiTonKyTruoc_7.Grouping = gpTonDongKyTruoc_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_7);
                    List<int> listKhieuNaiIdTonDongKyTruoc_7 = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan_7 = new Dictionary<string, string>();
                    extraParamTiepNhan_7.Add("fl", "KhieuNaiId");

                    QueryOptions qoKhieuNaiTiepNhan_7 = new QueryOptions();
                    qoKhieuNaiTiepNhan_7.ExtraParams = extraParamTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Start = 0;
                    qoKhieuNaiTiepNhan_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan_7.Add(sortOrderNgayTiepNhan_7);

                    GroupingParameters gpTiepNhan_7 = new GroupingParameters();
                    gpTiepNhan_7.Fields = listGroupField;
                    gpTiepNhan_7.Limit = 1;
                    gpTiepNhan_7.Main = true;
                    gpTiepNhan_7.OrderBy = listSortOrderNgayTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Grouping = gpTiepNhan_7;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan_7);

                    if (listKhieuNaiIdTonDongKyTruoc_7 != null && listKhieuNaiIdTonDongKyTruoc_7.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc_7.Contains(obj.KhieuNaiId); });
                    }

                    // listKhieuNaiIdTiepNhan
                    List<int> listKhieuNaiIdTiepNhan = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTiepNhan.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTonDong_7 = new Dictionary<string, string>();
                    extraParamTonDong_7.Add("fl", "KhieuNaiId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiTonDong_7 = new QueryOptions();
                    qoKhieuNaiTonDong_7.ExtraParams = extraParamTonDong_7;
                    qoKhieuNaiTonDong_7.Start = 0;
                    qoKhieuNaiTonDong_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_7.Add(sortOrderTonDongNgayTiepNhan_7);
                    listSortOrderTonDong_7.Add(sortOrderTonDongActivityId_7);

                    GroupingParameters gpTonDong_7 = new GroupingParameters();
                    gpTonDong_7.Fields = listGroupField;
                    gpTonDong_7.Limit = 1;
                    gpTonDong_7.Main = true;
                    gpTonDong_7.OrderBy = listSortOrderTonDong_7;
                    qoKhieuNaiTonDong_7.Grouping = gpTonDong_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_7);
                    List<int> listKhieuNaiIdTonDong_7 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy_7 = new Dictionary<string, string>();
                    extraParamXuLy_7.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLy_7 = new QueryOptions();
                    qoKhieuNaiXuLy_7.ExtraParams = extraParamXuLy_7;
                    qoKhieuNaiXuLy_7.Start = 0;
                    qoKhieuNaiXuLy_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy_7.Add(sortOrderNgayXuLy_7);

                    GroupingParameters gpXuLy_7 = new GroupingParameters();
                    gpXuLy_7.Fields = listGroupField;
                    gpXuLy_7.Limit = 1;
                    gpXuLy_7.Main = true;
                    gpXuLy_7.OrderBy = listSortOrderNgayXuLy_7;
                    qoKhieuNaiXuLy_7.Grouping = gpXuLy_7;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy_7);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_7.Contains(obj.KhieuNaiId); });
                        numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return !listKhieuNaiIdTiepNhan.Contains(obj.KhieuNaiId); });
                    }

                    break;

                    #region Code cũ số lượng không sử dụng

                    //case 7: // Số lượng tiếp nhận
                    //    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    //    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan");

                    //    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    //    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    //    qoKhieuNaiTaoMoi.Start = 0;
                    //    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);

                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    //    break;

                    //case 8: // Số lượng đã đóng
                    //    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    //    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy");

                    //    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    //    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    //    qoKhieuNaiDaDong.Start = 0;
                    //    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND DoiTacXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    //    break;

                    //case 9: // chuyển ngang hàng
                    //    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    //    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    //    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    //    qoKhieuNaiTonDong_39.Start = 0;
                    //    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    //    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    //    gpTonDong_39.Fields = listGroupField;
                    //    gpTonDong_39.Limit = 1;
                    //    gpTonDong_39.Main = true;
                    //    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    //    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    //    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    //    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    //    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Start = 0;
                    //    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    //    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    //    gpChuyenNgangHang.Fields = listGroupField;
                    //    gpChuyenNgangHang.Limit = 1;
                    //    gpChuyenNgangHang.Main = true;
                    //    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    //    }

                    //    break;

                    //case 10: // chuyển xử lý
                    //    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    //    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    //    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    //    qoKhieuNaiTonDong_310.Start = 0;
                    //    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    //    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    //    gpTonDong_310.Fields = listGroupField;
                    //    gpTonDong_310.Limit = 1;
                    //    gpTonDong_310.Main = true;
                    //    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    //    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    //    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    //    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    //    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Start = 0;
                    //    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    //    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    //    gpChuyenXuLy.Fields = listGroupField;
                    //    gpChuyenXuLy.Limit = 1;
                    //    gpChuyenXuLy.Main = true;
                    //    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    //    }

                    //    break;

                    //case 11: // chuyển phản hồi
                    //    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    //    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    //    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    //    qoKhieuNaiTonDong_311.Start = 0;
                    //    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    //    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    //    gpTonDong_311.Fields = listGroupField;
                    //    gpTonDong_311.Limit = 1;
                    //    gpTonDong_311.Main = true;
                    //    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    //    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    //    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.DoiTacXuLyId != doiTacId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    //    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHanPhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhanPhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId");

                    //    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    //    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    //    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    //    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    //    gpChuyenPhanHoi.Fields = listGroupField;
                    //    gpChuyenPhanHoi.Limit = 1;
                    //    gpChuyenPhanHoi.Main = true;
                    //    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  DoiTacXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), doiTacId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    //    }
                    //    break;
                    #endregion

            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/10/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public List<KhieuNai_ReportInfo> ListKhieuNaiTheoPhongBanDoiTac_V2_NET(int phongBanId, DateTime fromDate, DateTime toDate, int reportType)
        {
            List<KhieuNai_ReportInfo> listKhieuNaiInfo = null;

            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");

            DateTime previousFromDate = fromDate.AddDays(-1);
            DateTime nextToDate = toDate.AddDays(1);

            SolrQuery solrQuery = null;
            string whereClause = string.Empty;

            switch (reportType)
            {
                case 1: // Số lượng khiếu nại tồn trước kỳ (tính đến ngày fromDate.AddDays(-1))
                    Dictionary<string, string> extraParamTonKyTruoc = new Dictionary<string, string>();
                    extraParamTonKyTruoc.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, PhongBanXuLyId, NguoiXuLy, NgayTiepNhan, NgayQuaHan, DoiTacXuLyId, LDate, HanhDong, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTonKyTruoc = new QueryOptions();
                    qoKhieuNaiTonKyTruoc.ExtraParams = extraParamTonKyTruoc;
                    qoKhieuNaiTonKyTruoc.Start = 0;
                    qoKhieuNaiTonKyTruoc.Rows = int.MaxValue;

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
                    qoKhieuNaiTonKyTruoc.Grouping = gpTonDongKyTruoc;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                    }

                    break;

                case 2: // Số lượng tiếp nhận
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_2 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_2.Add("fl", "KhieuNaiId,  ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonKyTruoc_2 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_2.ExtraParams = extraParamTonKyTruoc_2;
                    qoKhieuNaiTonKyTruoc_2.Start = 0;
                    qoKhieuNaiTonKyTruoc_2.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_2 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_2 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_2 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyNgayTiepNhan_2);
                    listSortOrderTonDongTruocKy_2.Add(sortOrderTonDongTruocKyActivityId_2);

                    GroupingParameters gpTonDongKyTruoc_2 = new GroupingParameters();
                    gpTonDongKyTruoc_2.Fields = listGroupField;
                    gpTonDongKyTruoc_2.Limit = 1;
                    gpTonDongKyTruoc_2.Main = true;
                    gpTonDongKyTruoc_2.OrderBy = listSortOrderTonDongTruocKy_2;
                    qoKhieuNaiTonKyTruoc_2.Grouping = gpTonDongKyTruoc_2;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_2);
                    List<int> listKhieuNaiIdTonDongKyTruoc = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan = new Dictionary<string, string>();
                    extraParamTiepNhan.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTiepNhan = new QueryOptions();
                    qoKhieuNaiTiepNhan.ExtraParams = extraParamTiepNhan;
                    qoKhieuNaiTiepNhan.Start = 0;
                    qoKhieuNaiTiepNhan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan.Add(sortOrderNgayTiepNhan);

                    GroupingParameters gpTiepNhan = new GroupingParameters();
                    gpTiepNhan.Fields = listGroupField;
                    gpTiepNhan.Limit = 1;
                    gpTiepNhan.Main = true;
                    gpTiepNhan.OrderBy = listSortOrderNgayTiepNhan;
                    qoKhieuNaiTiepNhan.Grouping = gpTiepNhan;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan);

                    if (listKhieuNaiIdTonDongKyTruoc != null && listKhieuNaiIdTonDongKyTruoc.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 3: // Số lượng đã xử lý
                    Dictionary<string, string> extraParamTonDong_3 = new Dictionary<string, string>();
                    extraParamTonDong_3.Add("fl", "KhieuNaiId,  ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_3 = new QueryOptions();
                    qoKhieuNaiTonDong_3.ExtraParams = extraParamTonDong_3;
                    qoKhieuNaiTonDong_3.Start = 0;
                    qoKhieuNaiTonDong_3.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_3 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_3 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_3 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_3.Add(sortOrderTonDongNgayTiepNhan_3);
                    listSortOrderTonDong_3.Add(sortOrderTonDongActivityId_3);

                    GroupingParameters gpTonDong_3 = new GroupingParameters();
                    gpTonDong_3.Fields = listGroupField;
                    gpTonDong_3.Limit = 1;
                    gpTonDong_3.Main = true;
                    gpTonDong_3.OrderBy = listSortOrderTonDong_3;
                    qoKhieuNaiTonDong_3.Grouping = gpTonDong_3;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_3);
                    List<int> listKhieuNaiIdTonDong_3 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_3.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy = new Dictionary<string, string>();
                    extraParamXuLy.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiXuLy = new QueryOptions();
                    qoKhieuNaiXuLy.ExtraParams = extraParamXuLy;
                    qoKhieuNaiXuLy.Start = 0;
                    qoKhieuNaiXuLy.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy.Add(sortOrderNgayXuLy);

                    GroupingParameters gpXuLy = new GroupingParameters();
                    gpXuLy.Fields = listGroupField;
                    gpXuLy.Limit = 1;
                    gpXuLy.Main = true;
                    gpXuLy.OrderBy = listSortOrderNgayXuLy;
                    qoKhieuNaiXuLy.Grouping = gpXuLy;

                    //whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND ((PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2}) OR (PhongBanXuLyId:{2} AND HanhDong:4))", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_3.Contains(obj.KhieuNaiId); });
                    }

                    break;

                case 4: // Số lượng quá hạn đã xử lý
                    Dictionary<string, string> extraParamTonDong_4 = new Dictionary<string, string>();
                    extraParamTonDong_4.Add("fl", "KhieuNaiId,  ActivityId, PhongBanXuLyId, HanhDong");

                    QueryOptions qoKhieuNaiTonDong_4 = new QueryOptions();
                    qoKhieuNaiTonDong_4.ExtraParams = extraParamTonDong_4;
                    qoKhieuNaiTonDong_4.Start = 0;
                    qoKhieuNaiTonDong_4.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_4 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_4 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_4 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_4.Add(sortOrderTonDongNgayTiepNhan_4);
                    listSortOrderTonDong_4.Add(sortOrderTonDongActivityId_4);

                    GroupingParameters gpTonDong_4 = new GroupingParameters();
                    gpTonDong_4.Fields = listGroupField;
                    gpTonDong_4.Limit = 1;
                    gpTonDong_4.Main = true;
                    gpTonDong_4.OrderBy = listSortOrderTonDong_4;
                    qoKhieuNaiTonDong_4.Grouping = gpTonDong_4;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_4);
                    List<int> listKhieuNaiIdTonDong_4 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_4.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLyQuaHan = new Dictionary<string, string>();
                    extraParamXuLyQuaHan.Add("fl", "KhieuNaiId,LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiXuLyQuaHan = new QueryOptions();
                    qoKhieuNaiXuLyQuaHan.ExtraParams = extraParamXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Start = 0;
                    qoKhieuNaiXuLyQuaHan.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLyQuaHan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLyQuaHan = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLyQuaHan.Add(sortOrderNgayXuLyQuaHan);

                    GroupingParameters gpXuLyQuaHan = new GroupingParameters();
                    gpXuLyQuaHan.Fields = listGroupField;
                    gpXuLyQuaHan.Limit = 1;
                    gpXuLyQuaHan.Main = true;
                    gpXuLyQuaHan.OrderBy = listSortOrderNgayXuLyQuaHan;
                    qoKhieuNaiXuLyQuaHan.Grouping = gpXuLyQuaHan;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLyQuaHan);
                    listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_4.Contains(obj.KhieuNaiId) || obj.NgayTiepNhan < obj.NgayQuaHan_PhongBanXuLyTruoc; });
                    break;

                case 5: // Số lượng tồn đọng                    
                    Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();
                    extraParamTonDong.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, KhieuNai_GhiChu");

                    QueryOptions qoKhieuNaiTonDong = new QueryOptions();
                    qoKhieuNaiTonDong.ExtraParams = extraParamTonDong;
                    qoKhieuNaiTonDong.Start = 0;
                    qoKhieuNaiTonDong.Rows = int.MaxValue;

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
                    qoKhieuNaiTonDong.Grouping = gpTonDong;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }
                    break;

                // Số lượng tồn đọng quá hạn 
                case 6:
                    //Dictionary<string, string> extraParamTonDongQuaHan = new Dictionary<string, string>();
                    //extraParamTonDongQuaHan.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanXuLyId, NguoiXuLy, HanhDong, KhieuNai_GhiChu");

                    //QueryOptions qoKhieuNaiTonDongQuaHan = new QueryOptions();
                    //qoKhieuNaiTonDongQuaHan.ExtraParams = extraParamTonDongQuaHan;
                    //qoKhieuNaiTonDongQuaHan.Start = 0;
                    //qoKhieuNaiTonDongQuaHan.Rows = int.MaxValue;

                    //SolrNet.SortOrder sortOrderTonDongQuaHanNgayTiepNhan = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //SolrNet.SortOrder sortOrderTonDongQuaHanActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //List<SolrNet.SortOrder> listSortOrderTonDongQuaHan = new List<SolrNet.SortOrder>();
                    //listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanNgayTiepNhan);
                    //listSortOrderTonDongQuaHan.Add(sortOrderTonDongQuaHanActivityId);

                    //GroupingParameters gpTonDongQuaHan = new GroupingParameters();
                    //gpTonDongQuaHan.Fields = listGroupField;
                    //gpTonDongQuaHan.Limit = 1;
                    //gpTonDongQuaHan.Main = true;
                    //gpTonDongQuaHan.OrderBy = listSortOrderTonDongQuaHan;
                    //qoKhieuNaiTonDongQuaHan.Grouping = gpTonDongQuaHan;

                    //whereClause = string.Format("NgayTiepNhan : [* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //solrQuery = new SolrQuery(whereClause);
                    //listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDongQuaHan);
                    //if (listKhieuNaiInfo != null)
                    //{
                    //    int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj)
                    //    {
                    //        return obj.PhongBanXuLyId != phongBanId || toDate < obj.NgayQuaHan;
                    //    });
                    //}
                    List<KhieuNai_ReportInfo> tmp = new BaoCaoPAKNImpl().LayKhieuNaiTonDongQuaHan(CapBaoCaoEnum.PhongBan, phongBanId, toDate);
                    listKhieuNaiInfo = (tmp != null && tmp.Count > 0) ? tmp : null;
                    break;

                case 7: // Số lượng xử lý của số lượng tiếp nhận trong kỳ báo cáo
                    // Lấy ra số lượng tồn đọng kỳ trước
                    Dictionary<string, string> extraParamTonKyTruoc_7 = new Dictionary<string, string>();
                    extraParamTonKyTruoc_7.Add("fl", "KhieuNaiId, PhongBanXuLyId");

                    QueryOptions qoKhieuNaiTonKyTruoc_7 = new QueryOptions();
                    qoKhieuNaiTonKyTruoc_7.ExtraParams = extraParamTonKyTruoc_7;
                    qoKhieuNaiTonKyTruoc_7.Start = 0;
                    qoKhieuNaiTonKyTruoc_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongTruocKyNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongTruocKyActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDongTruocKy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyNgayTiepNhan_7);
                    listSortOrderTonDongTruocKy_7.Add(sortOrderTonDongTruocKyActivityId_7);

                    GroupingParameters gpTonDongKyTruoc_7 = new GroupingParameters();
                    gpTonDongKyTruoc_7.Fields = listGroupField;
                    gpTonDongKyTruoc_7.Limit = 1;
                    gpTonDongKyTruoc_7.Main = true;
                    gpTonDongKyTruoc_7.OrderBy = listSortOrderTonDongTruocKy_7;
                    qoKhieuNaiTonKyTruoc_7.Grouping = gpTonDongKyTruoc_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(previousFromDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(fromDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonKyTruoc_7);
                    List<int> listKhieuNaiIdTonDongKyTruoc_7 = new List<int>();

                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTonDongKyTruoc_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTiepNhan_7 = new Dictionary<string, string>();
                    extraParamTiepNhan_7.Add("fl", "KhieuNaiId");

                    QueryOptions qoKhieuNaiTiepNhan_7 = new QueryOptions();
                    qoKhieuNaiTiepNhan_7.ExtraParams = extraParamTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Start = 0;
                    qoKhieuNaiTiepNhan_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.ASC);
                    List<SolrNet.SortOrder> listSortOrderNgayTiepNhan_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayTiepNhan_7.Add(sortOrderNgayTiepNhan_7);

                    GroupingParameters gpTiepNhan_7 = new GroupingParameters();
                    gpTiepNhan_7.Fields = listGroupField;
                    gpTiepNhan_7.Limit = 1;
                    gpTiepNhan_7.Main = true;
                    gpTiepNhan_7.OrderBy = listSortOrderNgayTiepNhan_7;
                    qoKhieuNaiTiepNhan_7.Grouping = gpTiepNhan_7;

                    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyId : {2} AND HanhDong:(0 1 2 3)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTiepNhan_7);

                    if (listKhieuNaiIdTonDongKyTruoc_7 != null && listKhieuNaiIdTonDongKyTruoc_7.Count > 0)
                    {
                        listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDongKyTruoc_7.Contains(obj.KhieuNaiId); });
                    }

                    // listKhieuNaiIdTiepNhan
                    List<int> listKhieuNaiIdTiepNhan = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                        {
                            listKhieuNaiIdTiepNhan.Add(listKhieuNaiInfo[i].KhieuNaiId);
                        }
                    }

                    Dictionary<string, string> extraParamTonDong_7 = new Dictionary<string, string>();
                    extraParamTonDong_7.Add("fl", "KhieuNaiId, PhongBanXuLyId");

                    QueryOptions qoKhieuNaiTonDong_7 = new QueryOptions();
                    qoKhieuNaiTonDong_7.ExtraParams = extraParamTonDong_7;
                    qoKhieuNaiTonDong_7.Start = 0;
                    qoKhieuNaiTonDong_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    SolrNet.SortOrder sortOrderTonDongActivityId_7 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderTonDong_7 = new List<SolrNet.SortOrder>();
                    listSortOrderTonDong_7.Add(sortOrderTonDongNgayTiepNhan_7);
                    listSortOrderTonDong_7.Add(sortOrderTonDongActivityId_7);

                    GroupingParameters gpTonDong_7 = new GroupingParameters();
                    gpTonDong_7.Fields = listGroupField;
                    gpTonDong_7.Limit = 1;
                    gpTonDong_7.Main = true;
                    gpTonDong_7.OrderBy = listSortOrderTonDong_7;
                    qoKhieuNaiTonDong_7.Grouping = gpTonDong_7;

                    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_7);
                    List<int> listKhieuNaiIdTonDong_7 = new List<int>();
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                        if (listKhieuNaiInfo != null)
                        {
                            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                            {
                                listKhieuNaiIdTonDong_7.Add(listKhieuNaiInfo[i].KhieuNaiId);
                            }
                        }
                        //listKhieuNaiInfo.RemoveAll(delegate);
                    }

                    Dictionary<string, string> extraParamXuLy_7 = new Dictionary<string, string>();
                    extraParamXuLy_7.Add("fl", "KhieuNaiId, LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, PhongBanXuLyId, DoiTacXuLyTruocId, DoiTacXuLyId");

                    QueryOptions qoKhieuNaiXuLy_7 = new QueryOptions();
                    qoKhieuNaiXuLy_7.ExtraParams = extraParamXuLy_7;
                    qoKhieuNaiXuLy_7.Start = 0;
                    qoKhieuNaiXuLy_7.Rows = int.MaxValue;

                    SolrNet.SortOrder sortOrderNgayXuLy_7 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    List<SolrNet.SortOrder> listSortOrderNgayXuLy_7 = new List<SolrNet.SortOrder>();
                    listSortOrderNgayXuLy_7.Add(sortOrderNgayXuLy_7);

                    GroupingParameters gpXuLy_7 = new GroupingParameters();
                    gpXuLy_7.Fields = listGroupField;
                    gpXuLy_7.Limit = 1;
                    gpXuLy_7.Main = true;
                    gpXuLy_7.OrderBy = listSortOrderNgayXuLy_7;
                    qoKhieuNaiXuLy_7.Grouping = gpXuLy_7;

                    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND DoiTacXuLyTruocId: {2} AND -DoiTacXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}] AND  PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    solrQuery = new SolrQuery(whereClause);
                    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiXuLy_7);
                    if (listKhieuNaiInfo != null)
                    {
                        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_7.Contains(obj.KhieuNaiId); });
                        numberDeleted = listKhieuNaiInfo.RemoveAll(delegate (KhieuNai_ReportInfo obj) { return !listKhieuNaiIdTiepNhan.Contains(obj.KhieuNaiId); });
                    }

                    break;

                    #region Code cũ số lượng không sử dụng

                    //case 7: // Số lượng tiếp nhận
                    //    Dictionary<string, string> extraParamTaoMoi = new Dictionary<string, string>();
                    //    extraParamTaoMoi.Add("fl", "Id, SoThueBao, NgayTiepNhan, LDate, NgayQuaHan, PhongBanTiepNhanId, NguoiTiepNhan, GhiChu");

                    //    QueryOptions qoKhieuNaiTaoMoi = new QueryOptions();
                    //    qoKhieuNaiTaoMoi.ExtraParams = extraParamTaoMoi;
                    //    qoKhieuNaiTaoMoi.Start = 0;
                    //    qoKhieuNaiTaoMoi.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayTiepNhan:[{0} TO {1}] AND PhongBanTiepNhanId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);

                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiTaoMoi);

                    //    break;

                    //case 8: // Số lượng đã đóng
                    //    Dictionary<string, string> extraParamDaDong = new Dictionary<string, string>();
                    //    extraParamDaDong.Add("fl", "Id, SoThueBao, LoaiKhieuNai, NoiDungPA, NgayDongKN, NgayQuaHanPhongBanXuLy, GhiChu");

                    //    QueryOptions qoKhieuNaiDaDong = new QueryOptions();
                    //    qoKhieuNaiDaDong.ExtraParams = extraParamDaDong;
                    //    qoKhieuNaiDaDong.Start = 0;
                    //    qoKhieuNaiDaDong.Rows = int.MaxValue;

                    //    whereClause = string.Format("NgayDongKN:[{0} TO {1}] AND PhongBanXuLyId : {2}", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNaiDaDong);

                    //    break;

                    //case 9: // chuyển ngang hàng
                    //    Dictionary<string, string> extraParamTonDong_39 = new Dictionary<string, string>();
                    //    extraParamTonDong_39.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_39 = new QueryOptions();
                    //    qoKhieuNaiTonDong_39.ExtraParams = extraParamTonDong_39;
                    //    qoKhieuNaiTonDong_39.Start = 0;
                    //    qoKhieuNaiTonDong_39.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_39 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_39 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_39 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongNgayTiepNhan_39);
                    //    listSortOrderTonDong_39.Add(sortOrderTonDongActivityId_39);

                    //    GroupingParameters gpTonDong_39 = new GroupingParameters();
                    //    gpTonDong_39.Fields = listGroupField;
                    //    gpTonDong_39.Limit = 1;
                    //    gpTonDong_39.Main = true;
                    //    gpTonDong_39.OrderBy = listSortOrderTonDong_39;
                    //    qoKhieuNaiTonDong_39.Grouping = gpTonDong_39;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_39);
                    //    List<int> listKhieuNaiIdTonDong_39 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_39.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenNgangHang = new Dictionary<string, string>();
                    //    extraParamChuyenNgangHang.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    //    QueryOptions qoKhieuNaiChuyenNgangHang = new QueryOptions();
                    //    qoKhieuNaiChuyenNgangHang.ExtraParams = extraParamChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Start = 0;
                    //    qoKhieuNaiChuyenNgangHang.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenNgangHang = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenNgangHang = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenNgangHang.Add(sortOrderNgayChuyenNgangHang);

                    //    GroupingParameters gpChuyenNgangHang = new GroupingParameters();
                    //    gpChuyenNgangHang.Fields = listGroupField;
                    //    gpChuyenNgangHang.Limit = 1;
                    //    gpChuyenNgangHang.Main = true;
                    //    gpChuyenNgangHang.OrderBy = listSortOrderNgayChuyenNgangHang;
                    //    qoKhieuNaiChuyenNgangHang.Grouping = gpChuyenNgangHang;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenNgangHang);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_39.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng; });
                    //    }

                    //    break;

                    //case 10: // chuyển xử lý
                    //    Dictionary<string, string> extraParamTonDong_310 = new Dictionary<string, string>();
                    //    extraParamTonDong_310.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_310 = new QueryOptions();
                    //    qoKhieuNaiTonDong_310.ExtraParams = extraParamTonDong_310;
                    //    qoKhieuNaiTonDong_310.Start = 0;
                    //    qoKhieuNaiTonDong_310.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_310 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_310 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_310 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongNgayTiepNhan_310);
                    //    listSortOrderTonDong_310.Add(sortOrderTonDongActivityId_310);

                    //    GroupingParameters gpTonDong_310 = new GroupingParameters();
                    //    gpTonDong_310.Fields = listGroupField;
                    //    gpTonDong_310.Limit = 1;
                    //    gpTonDong_310.Main = true;
                    //    gpTonDong_310.OrderBy = listSortOrderTonDong_310;
                    //    qoKhieuNaiTonDong_310.Grouping = gpTonDong_310;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_310);
                    //    List<int> listKhieuNaiIdTonDong_310 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_310.Add(listKhieuNaiInfo[i].KhieuNaiId);

                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenXuLy = new Dictionary<string, string>();
                    //    extraParamChuyenXuLy.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    //    QueryOptions qoKhieuNaiChuyenXuLy = new QueryOptions();
                    //    qoKhieuNaiChuyenXuLy.ExtraParams = extraParamChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Start = 0;
                    //    qoKhieuNaiChuyenXuLy.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenXuLy = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenXuLy = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenXuLy.Add(sortOrderNgayChuyenXuLy);

                    //    GroupingParameters gpChuyenXuLy = new GroupingParameters();
                    //    gpChuyenXuLy.Fields = listGroupField;
                    //    gpChuyenXuLy.Limit = 1;
                    //    gpChuyenXuLy.Main = true;
                    //    gpChuyenXuLy.OrderBy = listSortOrderNgayChuyenXuLy;
                    //    qoKhieuNaiChuyenXuLy.Grouping = gpChuyenXuLy;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenXuLy);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_310.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban; });
                    //    }

                    //    break;

                    //case 11: // chuyển phản hồi
                    //    Dictionary<string, string> extraParamTonDong_311 = new Dictionary<string, string>();
                    //    extraParamTonDong_311.Add("fl", "KhieuNaiId, ActivityId, PhongBanXuLyId, NguoiXuLy, HanhDong, DoiTacXuLyId");

                    //    QueryOptions qoKhieuNaiTonDong_311 = new QueryOptions();
                    //    qoKhieuNaiTonDong_311.ExtraParams = extraParamTonDong_311;
                    //    qoKhieuNaiTonDong_311.Start = 0;
                    //    qoKhieuNaiTonDong_311.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderTonDongNgayTiepNhan_311 = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    SolrNet.SortOrder sortOrderTonDongActivityId_311 = new SolrNet.SortOrder("ActivityId", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderTonDong_311 = new List<SolrNet.SortOrder>();
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongNgayTiepNhan_311);
                    //    listSortOrderTonDong_311.Add(sortOrderTonDongActivityId_311);

                    //    GroupingParameters gpTonDong_311 = new GroupingParameters();
                    //    gpTonDong_311.Fields = listGroupField;
                    //    gpTonDong_311.Limit = 1;
                    //    gpTonDong_311.Main = true;
                    //    gpTonDong_311.OrderBy = listSortOrderTonDong_311;
                    //    qoKhieuNaiTonDong_311.Grouping = gpTonDong_311;

                    //    whereClause = string.Format("NgayTiepNhan:[* TO {0}] AND KhieuNai_NgayDongKN:[{1} TO *]", ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), ConvertUtility.ConvertDateTimeToSolr(nextToDate));
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiTonDong_311);
                    //    List<int> listKhieuNaiIdTonDong_311 = new List<int>();
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return obj.PhongBanXuLyId != phongBanId; });

                    //        if (listKhieuNaiInfo != null)
                    //        {
                    //            for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                    //            {
                    //                listKhieuNaiIdTonDong_311.Add(listKhieuNaiInfo[i].KhieuNaiId);
                    //            }
                    //        }
                    //        //listKhieuNaiInfo.RemoveAll(delegate);
                    //    }

                    //    Dictionary<string, string> extraParamChuyenPhanHoi = new Dictionary<string, string>();
                    //    extraParamChuyenPhanHoi.Add("fl", "KhieuNaiId, ActivityId, SoThueBao, NgayTiepNhan, NgayQuaHan_PhongBanXuLyTruoc, NguoiXuLyTruoc,PhongBanXuLyTruocId, NgayTiepNhan_PhongBanXuLyTruoc, LoaiKhieuNai, NoiDungPA, NguoiXuLy, HanhDong, PhongBanXuLyId, KhieuNai_GhiChu");

                    //    QueryOptions qoKhieuNaiChuyenPhanHoi = new QueryOptions();
                    //    qoKhieuNaiChuyenPhanHoi.ExtraParams = extraParamChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Start = 0;
                    //    qoKhieuNaiChuyenPhanHoi.Rows = int.MaxValue;

                    //    SolrNet.SortOrder sortOrderNgayChuyenPhanHoi = new SolrNet.SortOrder("NgayTiepNhan", Order.DESC);
                    //    List<SolrNet.SortOrder> listSortOrderNgayChuyenPhanHoi = new List<SolrNet.SortOrder>();
                    //    listSortOrderNgayChuyenPhanHoi.Add(sortOrderNgayChuyenPhanHoi);

                    //    GroupingParameters gpChuyenPhanHoi = new GroupingParameters();
                    //    gpChuyenPhanHoi.Fields = listGroupField;
                    //    gpChuyenPhanHoi.Limit = 1;
                    //    gpChuyenPhanHoi.Main = true;
                    //    gpChuyenPhanHoi.OrderBy = listSortOrderNgayChuyenPhanHoi;
                    //    qoKhieuNaiChuyenPhanHoi.Grouping = gpChuyenPhanHoi;

                    //    //whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyTruocId : {2}) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyTruocId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);                        
                    //    whereClause = string.Format("(NgayTiepNhan:[{0} TO {1}]  AND PhongBanXuLyTruocId: {2} AND -PhongBanXuLyId : {2} AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND  PhongBanXuLyId:{2} AND HanhDong:4)", ConvertUtility.ConvertDateTimeToSolr(fromDate), ConvertUtility.ConvertDateTimeToSolr(toDate, 23, 59, 59, 999), phongBanId);
                    //    solrQuery = new SolrQuery(whereClause);
                    //    listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, qoKhieuNaiChuyenPhanHoi);
                    //    if (listKhieuNaiInfo != null)
                    //    {
                    //        int numberDeleted = listKhieuNaiInfo.RemoveAll(delegate(KhieuNai_ReportInfo obj) { return listKhieuNaiIdTonDong_311.Contains(obj.KhieuNaiId) || obj.HanhDong != (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi; });
                    //    }

                    //    break;

                    #endregion


            }

            listKhieuNaiInfo = SortListByNgayTiepNhanASC(listKhieuNaiInfo);

            //SetValueGhiChuKhieuNai(listKhieuNaiInfo);

            return listKhieuNaiInfo;
        }


        // Lấy chi tiết danh sách SLTonDongQuaHan
        private List<KhieuNai_ReportInfo> LayKhieuNaiTonDong(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            string URL_SOLR_ACTIVITY = string.Concat(Config.ServerSolr, "Activity");
            List<string> listGroupField = new List<string>();
            listGroupField.Add("KhieuNaiId");


            List<int> listKhieuNaiIdTonDong = new List<int>();

            QueryOptions queryOptionTonDong = new QueryOptions();
            //Lấy ra những trường nào
            Dictionary<string, string> extraParamTonDong = new Dictionary<string, string>();

            extraParamTonDong.Add("fl", @"Id, KhieuNaiId, SoThueBao, NguoiXuLy,PhongBanXuLyId, TenPhongBanXuLy, IsCurrent, NgayQuaHan, NgayTiepNhan, LDate");
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

            whereClauseTonDong += string.Format(" AND DoiTacXuLyId : {0}", doiTacId);
            whereClauseTonDong += string.Format(" AND (LDate : [{0} TO *] OR IsCurrent : 1)", toDate.AddDays(1).StartOfDay().FormatSolrDateTime());

            SolrQuery solrQuery = new SolrQuery(whereClauseTonDong);
            return QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery, queryOptionTonDong);
        }
        private List<KhieuNai_ReportInfo> LayKhieuNaiTonDongQuaHan(int doiTacId, DateTime fromDate, DateTime toDate)
        {
            List<KhieuNai_ReportInfo> lst = LayKhieuNaiTonDong(doiTacId, fromDate, toDate);
            if (lst != null) return lst.Where(obj => (obj.IsCurrent && obj.NgayQuaHan <= toDate.EndOfDay()) || (obj.LDate > obj.NgayQuaHan && toDate.EndOfDay() >= obj.NgayQuaHan)).ToList().ToList();
            return null;
        }
        #endregion

    }
}
