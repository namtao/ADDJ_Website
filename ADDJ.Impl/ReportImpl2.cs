using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Entity;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class ReportImpl2 : BaseImpl<KhieuNai_SoTienInfo>
    {
        private const string SOLR_GQKN = "GQKN";
        private const string SOLR_ACTIVITY = "Activity";
        private const string SOLR_SOTIEN = "SoTien";
        private const string SOLR_SOTIENGIAMTRU = "SoTienGiamTru";
        private const string SOLR_KETQUAXULY = "KetQuaXuLy";

        private const int PERMISSIONSCHEMES_DONGKN = 6; // PermissionSchemes.Id
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_SoTien";
        }
        private static List<KhieuNai_SoTienInfo> _ListKhieuNai;
        // Tạm thời để hàm ListAllKhieuNai.
        public static List<KhieuNai_SoTienInfo> ListKhieuNai
        {
            get
            {
                if (_ListKhieuNai == null)
                    _ListKhieuNai = new ReportImpl2().GetList();
                return _ListKhieuNai;
            }
            set { _ListKhieuNai = value; }
        }

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
        private string URL_SOLR_SOTIENGIAMTRU
        {
            get
            {
                return Config.ServerSolr + SOLR_SOTIENGIAMTRU;
            }
        }

        private string URL_SOLR_KETQUAXULY
        {
            get
            {
                return Config.ServerSolr + SOLR_KETQUAXULY;
            }
        }
        #region Báo cáo giảm trừ
        // Báo cáo chi tiết giảm trừ DV GTGT
        public DataTable BaoCaoChiTietGiamTruDVGTGT(int isDonVi, int isKhuVuc, int khuVucId,int doiTacId, int caNhanXuLy, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<KhieuNai_SoTienInfo> lstKhieuNaiSoTien = new List<KhieuNai_SoTienInfo>();

                // Định nghĩa thông tin
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("STT");
                dtResult.Columns.Add("MaPAKN");
                dtResult.Columns.Add("SoThueBao");
                dtResult.Columns.Add("LinhVucCon");
                dtResult.Columns.Add("NoiDungPA");
                dtResult.Columns.Add("NoiDungDongKN");
                dtResult.Columns.Add("NgayDongKN");
                dtResult.Columns.Add("DonVi_User_XuLy");
                dtResult.Columns.Add("SoTienGiamTru");

                // Lấy ra trường nào
                var queryOption = new QueryOptions();
                var extraParam = new Dictionary<string, string>();
                extraParam.Add("fl", @"KhieuNaiId,SoThueBao,LinhVucConId,LinhVucCon,LinhVucChungId,LinhVucChung,NoiDungPA,NoiDungXuLyDongKN,NgayDongKN,NguoiXuLyId,NguoiXuLy,DoiTacXuLyId,TenDoiTacXuLy,SoTien,SoTien_Edit,SoTienFinal");
                //extraParam.Add("fl", @"*");
                queryOption.ExtraParams = extraParam;
                queryOption.Start = 0;
                queryOption.Rows = int.MaxValue;

                // Điều kiện lọc dữ liệu
                string whereClause = "";

                if (isDonVi == 1) // nếu là theo đơn vị
                {
                    if (isKhuVuc == 1) // nếu là theo khu vực
                    {
                        if (khuVucId != 1)
                            whereClause = string.Format(" {0} AND KhuVucId: {1}", whereClause, khuVucId);
                    }
                    else // theo đơn vị
                    {
                        whereClause = string.Format(" {0} AND DoiTacXuLyId: {1}", whereClause, doiTacId);
                    }
                }
                else // theo cá nhân
                {
                    whereClause = string.Format(" {0} AND NguoiXuLyId: {1}", whereClause, caNhanXuLy);
                }
                
                string sFromDate = ConvertUtility.ConvertDateTimeToSolr(fromDate);
                string sToDate = ConvertUtility.ConvertDateTimeToSolr(toDate);

                string whereClauseDanhSach = string.Format("NgayDongKN:[{0} TO {1}] {2}", sFromDate, sToDate, whereClause);
                var solrQuery = new SolrQuery(whereClauseDanhSach);
                lstKhieuNaiSoTien = QuerySolrBase<KhieuNai_SoTienInfo>.QuerySolr(URL_SOLR_SOTIENGIAMTRU, solrQuery, queryOption);
                // Xử lý cho vào DataTable
                if (lstKhieuNaiSoTien!=null && lstKhieuNaiSoTien.Count>0)
                {
                    int index = 0;
                    foreach (var item in lstKhieuNaiSoTien)
                    {
                        index++;
                        DataRow row = dtResult.NewRow();
                        row["STT"] = index;
                        row["MaPAKN"] = item.KhieuNaiId;
                        row["SoThueBao"] = item.SoThueBao;
                        row["LinhVucCon"] = item.LinhVucCon;
                        row["NoiDungPA"] = item.NoiDungPA;
                        row["NoiDungDongKN"] = item.NoiDungXuLyDongKN;
                        row["NgayDongKN"] = string.Format("{0:dd/MM/yyyy h:m:s t}", item.NgayDongKN);
                        row["DonVi_User_XuLy"] = (isDonVi == 1 ? item.TenDoiTacXuLy : item.NguoiXuLy);
                        row["SoTienGiamTru"] = item.SoTienFinal;
                        dtResult.Rows.Add(row);
                    }
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }
        // Báo cáo tổng hợp theo chỉ tiêu thời gian giải quyết
        // 28/04/2016
        public DataTable BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet(string dsDonVi, string tuMucTien, string denMucTien, DateTime fromDate,DateTime toDate)
        {
            try
            {
                List<string> listGroupField = new List<string>();
                listGroupField.Add("KhieuNaiId");


                // Định nghĩa hình dạng theo mẫu báo cáo
                DataTable dResult = new DataTable();
                dResult.Columns.Add("TenCot");
                dResult.Columns.Add("ToanMang");
                dResult.Columns.Add("GTVDoiTac_GDV");
                dResult.Columns.Add("TruongCaDoiTac");
                dResult.Columns.Add("KTV_VNP");
                dResult.Columns.Add("NV_XLNV_GQKN_CuaHangTruong");
                dResult.Columns.Add("TT_XLNV_GQKN_CV_P_NghiepVu");
                dResult.Columns.Add("LD_Dai_HTKH_Truong_P_NghiepVu");
                dResult.Columns.Add("CV_P_CSKH");
                dResult.Columns.Add("LD_TTKD");
                dResult.Columns.Add("LD_P_CSKH");

                DataRow row0 = dResult.NewRow();
                row0["TenCot"] = "Số lượng KN tiếp nhận";
                row0["ToanMang"] = 0;
                row0["GTVDoiTac_GDV"] = "";
                row0["TruongCaDoiTac"] = "";
                row0["KTV_VNP"] = "";
                row0["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row0["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row0["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row0["CV_P_CSKH"] = "";
                row0["LD_TTKD"] = "";
                row0["LD_P_CSKH"] = "";
                dResult.Rows.Add(row0);

                DataRow row1 = dResult.NewRow();
                row1["TenCot"] = "Số lượng KN giải quyết trong kỳ tiếp nhận";
                row1["ToanMang"] = 1;
                row1["GTVDoiTac_GDV"] = "";
                row1["TruongCaDoiTac"] = "";
                row1["KTV_VNP"] = "";
                row1["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row1["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row1["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row1["CV_P_CSKH"] = "";
                row1["LD_TTKD"] = "";
                row1["LD_P_CSKH"] = "";
                dResult.Rows.Add(row1);


                DataRow row2 = dResult.NewRow();
                row2["TenCot"] = "Tổng thời gian giải quyết (phút)";
                row2["ToanMang"] =2;
                row2["GTVDoiTac_GDV"] = "";
                row2["TruongCaDoiTac"] = "";
                row2["KTV_VNP"] = "";
                row2["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row2["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row2["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row2["CV_P_CSKH"] = "";
                row2["LD_TTKD"] = "";
                row2["LD_P_CSKH"] = "";
                dResult.Rows.Add(row2);


                DataRow row3 = dResult.NewRow();
                row3["TenCot"] = "Tổng số KN được giảm trừ ";
                row3["ToanMang"] = "";
                row3["GTVDoiTac_GDV"] = "";
                row3["TruongCaDoiTac"] = "";
                row3["KTV_VNP"] = "";
                row3["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row3["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row3["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row3["CV_P_CSKH"] = "";
                row3["LD_TTKD"] = "";
                row3["LD_P_CSKH"] = "";
                dResult.Rows.Add(row3);


                DataRow row4 = dResult.NewRow();
                row4["TenCot"] = "Tổng số tiền được giảm trừ";
                row4["ToanMang"] = "";
                row4["GTVDoiTac_GDV"] = "";
                row4["TruongCaDoiTac"] = "";
                row4["KTV_VNP"] = "";
                row4["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row4["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row4["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row4["CV_P_CSKH"] = "";
                row4["LD_TTKD"] = "";
                row4["LD_P_CSKH"] = "";
                dResult.Rows.Add(row4);

                DataRow row5 = dResult.NewRow();
                row5["TenCot"] = "Tổng số KN được giải quyết/tổng số KN tiếp nhận (%)";
                row5["ToanMang"] = "";
                row5["GTVDoiTac_GDV"] = "";
                row5["TruongCaDoiTac"] = "";
                row5["KTV_VNP"] = "";
                row5["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row5["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row5["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row5["CV_P_CSKH"] = "";
                row5["LD_TTKD"] = "";
                row5["LD_P_CSKH"] = "";
                dResult.Rows.Add(row5);

                DataRow row6 = dResult.NewRow();
                row6["TenCot"] = "Tổng số KN được giảm trừ/tổng số KN được giải quyết (%) ";
                row6["ToanMang"] = "";
                row6["GTVDoiTac_GDV"] = "";
                row6["TruongCaDoiTac"] = "";
                row6["KTV_VNP"] = "";
                row6["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row6["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row6["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row6["CV_P_CSKH"] = "";
                row6["LD_TTKD"] = "";
                row6["LD_P_CSKH"] = "";
                dResult.Rows.Add(row6);

                DataRow row7 = dResult.NewRow();
                row7["TenCot"] = "So sánh thời gian giải quyết/thời hạn theo quy định (%)";
                row7["ToanMang"] = "";
                row7["GTVDoiTac_GDV"] = "";
                row7["TruongCaDoiTac"] = "";
                row7["KTV_VNP"] = "";
                row7["NV_XLNV_GQKN_CuaHangTruong"] = "";
                row7["TT_XLNV_GQKN_CV_P_NghiepVu"] = "";
                row7["LD_Dai_HTKH_Truong_P_NghiepVu"] = "";
                row7["CV_P_CSKH"] = "";
                row7["LD_TTKD"] = "";
                row7["LD_P_CSKH"] = "";
                dResult.Rows.Add(row7);

                // lấy thông tin đơn vị
                var userInfo = Admin.LoginAdmin.AdminLogin();

                // xử lý lấy thông tin liên quan
                // thông tin về số lượng KN tiếp nhận, 
                // giải quyết trong kỳ tiếp nhận, 
                // thời gian phút: ACTIVITY
                // lấy ra trường nào
                var queryOption_TiepNhan = new QueryOptions();
                var extraParam_TiepNhan = new Dictionary<string, string>();
                extraParam_TiepNhan.Add("fl", @"KhieuNaiId");
                queryOption_TiepNhan.ExtraParams = extraParam_TiepNhan;
                queryOption_TiepNhan.Start = 0;
                queryOption_TiepNhan.Rows = int.MaxValue;

                SolrNet.SortOrder solrOrderActivityId = new SolrNet.SortOrder("ActivityId", Order.DESC);
                List<SolrNet.SortOrder> lstSolrOrtOderTiepNhan = new List<SortOrder>();
                lstSolrOrtOderTiepNhan.Add(solrOrderActivityId);

                GroupingParameters gpTiepNhan = new GroupingParameters();
                gpTiepNhan.Fields = listGroupField;
                gpTiepNhan.Limit = 1;
                gpTiepNhan.Main = true;
                gpTiepNhan.OrderBy = lstSolrOrtOderTiepNhan;
                queryOption_TiepNhan.Grouping = gpTiepNhan;


                // điều kiện lọc dữ liệu
                // Số lượng tiếp nhận
                string whereClause_TiepNhan = "";
                whereClause_TiepNhan = string.Format("{0} AND DoiTacXuLyId:({1})", whereClause_TiepNhan, dsDonVi);
                string sfromDate_TiepNhan = ConvertUtility.ConvertDateTimeToSolr(fromDate);
                string stoDate_TiepNhan = ConvertUtility.ConvertDateTimeToSolr(toDate);
                string whereClauseDs_TiepNhan = string.Format("NgayTiepNhan:[{0} TO {1}] {2} AND HanhDong:(0 1 2 3)", sfromDate_TiepNhan, stoDate_TiepNhan, whereClause_TiepNhan);
                var solrQuery_TiepNhan = new SolrQuery(whereClauseDs_TiepNhan);
                SolrQueryResults<KhieuNai_ReportInfo> listKhieuNaiInfo = null;
                listKhieuNaiInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery_TiepNhan, queryOption_TiepNhan);
                //if (listKhieuNaiInfo != null)
                //{
                //    var sltiepnhan = listKhieuNaiInfo.Count;
                //    row0["ToanMang"] = sltiepnhan;
                //}

                // Số lượng KN giải quyết trong kỳ tiếp nhận
                var queryOption_GiaiQuyetTrongKy = new QueryOptions();
                var extraParam_GiaiQuyetTrongKy = new Dictionary<string, string>();
                extraParam_GiaiQuyetTrongKy.Add("fl", @"KhieuNaiId,KhieuNai_NgayTiepNhan,KhieuNai_NgayDongKN");
                queryOption_GiaiQuyetTrongKy.ExtraParams = extraParam_GiaiQuyetTrongKy;
                queryOption_GiaiQuyetTrongKy.Start = 0;
                queryOption_GiaiQuyetTrongKy.Rows = int.MaxValue;

                GroupingParameters gpGiaiQuyetTrongKy = new GroupingParameters();
                gpGiaiQuyetTrongKy.Fields = listGroupField;
                gpGiaiQuyetTrongKy.Limit = 1;
                gpGiaiQuyetTrongKy.Main = true;
                gpGiaiQuyetTrongKy.OrderBy = lstSolrOrtOderTiepNhan;
                queryOption_GiaiQuyetTrongKy.Grouping = gpGiaiQuyetTrongKy;

                string whereClause_GiaiQuyetTrongKy = "";
                whereClause_GiaiQuyetTrongKy = string.Format("{0} AND DoiTacXuLyId:({1})", whereClause_GiaiQuyetTrongKy, dsDonVi);
                string sfromDate_GiaiQuyetTrongKy = ConvertUtility.ConvertDateTimeToSolr(fromDate);
                string stoDate_GiaiQuyetTrongKy = ConvertUtility.ConvertDateTimeToSolr(toDate);
                //string whereClauseDs_GiaiQuyetTrongKy = string.Format("(NgayTiepNhan:[{0} TO {1}] AND PhongBanXuLyTruocId:({2}) AND -PhongBanXuLyId:({2}) AND -HanhDong:4) OR (LDate:[{0} TO {1}] AND PhongBanXuLyId:({2}) AND HanhDong:4)", sfromDate_GiaiQuyetTrongKy, stoDate_GiaiQuyetTrongKy, dsDonVi);
                string whereClauseDs_GiaiQuyetTrongKy = string.Format("NgayTiepNhan:[{0} TO {1}] AND DoiTacXuLyId:({2})  AND HanhDong:4", sfromDate_GiaiQuyetTrongKy, stoDate_GiaiQuyetTrongKy, dsDonVi);
                var solrQuery_GiaiQuyetTrongKy = new SolrQuery(whereClauseDs_GiaiQuyetTrongKy);
                SolrQueryResults<KhieuNai_ReportInfo> listKhieuNai_GiaiQuyetInfo = null;
                listKhieuNai_GiaiQuyetInfo = QuerySolrBase<KhieuNai_ReportInfo>.QuerySolr(URL_SOLR_ACTIVITY, solrQuery_GiaiQuyetTrongKy, queryOption_GiaiQuyetTrongKy);
                //if (listKhieuNai_GiaiQuyetInfo != null)
                //{
                //    var slgiaiquyettrongky = listKhieuNai_GiaiQuyetInfo.Count;
                //    row1["ToanMang"] = slgiaiquyettrongky;
                //}
                
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                // thông tin về số lượng KN được giảm trừ, số tiền được giảm trừ
                // Tổng số KN được giảm trừ

                var queryOption2 = new QueryOptions();
                var extraParam2 = new Dictionary<string, string>();
                extraParam2.Add("fl", @"KhieuNaiId,SoTienFinal");
                queryOption2.ExtraParams = extraParam2;
                queryOption2.Start = 0;
                queryOption2.Rows = int.MaxValue;

                string whereClause2 = "";
                whereClause2 = string.Format("{0} AND DoiTacXuLyId:({1})", whereClause2, dsDonVi);
                string sfromDate2 = ConvertUtility.ConvertDateTimeToSolr(fromDate);
                string stoDate2 = ConvertUtility.ConvertDateTimeToSolr(toDate);
                string whereClauseDs2 = string.Format("NgayTiepNhan:[{0} TO {1}] AND TrangThai:3 {2} AND IsDaBuTien:true AND IsDauSo:true AND -LoaiTien:5", sfromDate2, stoDate2, whereClause2);
                var solrQuery2 = new SolrQuery(whereClauseDs2);
                SolrQueryResults<KhieuNai_SoTienInfo> listKhieuNaiSoTienInfo = null;
                listKhieuNaiSoTienInfo = QuerySolrBase<KhieuNai_SoTienInfo>.QuerySolr(URL_SOLR_SOTIENGIAMTRU, solrQuery2, queryOption2);
                //if (listKhieuNaiSoTienInfo != null)
                //{
                //    var tongsoKNduocgiamtru = listKhieuNaiSoTienInfo.Count;
                //    row3["ToanMang"] = tongsoKNduocgiamtru;
                //}

                // Tổng số tiền được giảm trừ

                // Tổng hợp xử lý hiển thị theo mẫu báo cáo
                // Mẫu báo cáo chung:
                // +  1    2    3    4    5    6    7     8    9    10
                // 1  1.1  1.2  1.3  1.4  1.5  1.6  1.7   1.8  1.9  1.10
                // 2  2.1  2.2  2.3  2.4  2.5  2.6  2.7   2.8  2.9  2.10
                // 3  3.1  3.2  3.3  3.4  3.5  3.6  3.7   3.8  3.9  3.10
                // 4  4.1  4.2  4.3  4.4  4.5  4.6  4.7   4.8  4.9  4.10
                // 5  5.1  5.2  5.3  5.4  5.5  5.6  5.7   5.8  5.9  5.10
                // 6  6.1  6.2  6.3  6.4  6.5  6.6  6.7   6.8  6.9  6.10
                // 7  7.1  7.2  7.3  7.4  7.5  7.6  7.7   7.8  7.9  7.10
                // 8  8.1  8.2  8.3  8.4  8.5  8.6  8.7   8.8  8.9  8.10
                // Tiến hành xử lý:
                // Nguồn chính: KhieuNai Acivity/ SoTienGiamTru
                // Tính toán row: Số lượng KN tiếp nhận, giải quyết trong kỳ tiếp nhận, thời gian giải quyết(phút)
                // Tính toán row: giảm trừ
                //
                // listKhieuNaiInfo            : Tính toán thông tin về KN
                // listKhieuNai_GiaiQuyetInfo  : Tính toán thông tin về KN giải quyết
                // listKhieuNaiSoTienInfo      : Tính toán thông tin giảm trừ
                // Xử lý toàn mạng
                int tongsoKNTiepnhan = 0;
                if(listKhieuNaiInfo!=null)
                {
                    // KN tiếp nhận
                    tongsoKNTiepnhan = listKhieuNaiInfo.Count;
                    row0["ToanMang"] = tongsoKNTiepnhan;
                }

                int trongKNgiaiquyet = 0;
                double tongthoigianPhut = 0;
                if (listKhieuNai_GiaiQuyetInfo != null)
                {
                    // KN giải quyết trong kỳ tiếp nhận
                    trongKNgiaiquyet = listKhieuNai_GiaiQuyetInfo.Count;
                    row1["ToanMang"] = trongKNgiaiquyet;

                    // KN tổng thời gian giải quyết
                    
                    // Tổng thời gian giải quyết (phút) - tính từ thời điểm tiếp nhận đến đóng khiếu nại
                    // chỉ tính đối với trường hợp đối với khiếu nại giải quyết trong kỳ
                    //DateTime date1 = Convert.ToDateTime("2014-05-21T22:50:21.217Z");
                    //DateTime date2 = Convert.ToDateTime("2016-03-23T08:29:35.807Z");
                    //var tongPhut = (date1 < date2) ? (date2 - date1).TotalMinutes : (date1 - date2).TotalMinutes;
                  
                    foreach (var item in listKhieuNai_GiaiQuyetInfo)
                    {
                        DateTime date1 = Convert.ToDateTime(item.KhieuNai_NgayTiepNhan);
                        DateTime date2 = Convert.ToDateTime(item.KhieuNai_NgayDongKN);
                        var tongPhut = (date1 < date2) ? (date2 - date1).TotalMinutes : (date1 - date2).TotalMinutes;
                        tongthoigianPhut += tongPhut;
                    }
                    row2["ToanMang"] = tongthoigianPhut;
                }
                int tongsoKNgiamtru = 0;
                decimal sTongTienGiamTru = 0;
                if (listKhieuNaiSoTienInfo != null)
                {
                    // KN được giảm trừ
                    tongsoKNgiamtru = listKhieuNaiSoTienInfo.Count;
                    row3["ToanMang"] = tongsoKNgiamtru;
                    // KN tổng số tiền được giảm trừ                   
                    foreach (var item in listKhieuNaiSoTienInfo)
                    {
                        sTongTienGiamTru += item.SoTienFinal;
                    }
                    row4["ToanMang"] = sTongTienGiamTru;
                }

                // Tổng số KN giải quyết/tiếp nhận
                row5["ToanMang"] = Math.Round((double)trongKNgiaiquyet * 100 / tongsoKNTiepnhan, 2);
                // Tổng só KN giảm trừ/giải quyết
                row6["ToanMang"] = Math.Round((double)tongsoKNgiamtru * 100 / trongKNgiaiquyet, 2);
                // So sánh thời gian giải quyết/thời gian quy định
                // thời hạn theo quy định là 13 ngày 20 phút => 13*24*60+20 = 18740;
                row7["ToanMang"] = Math.Round(((double)tongthoigianPhut * 100) /(trongKNgiaiquyet* 18740), 2); ;

                return dResult;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Báo cáo chi tiết theo chỉ tiêu thời gian giải quyết
        // 10/05/2016
        public DataTable BaoCaoTongHopChiTietTheoChiTieuThoiGianGiaiQuyet(string dsDonVi, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<string> listGroupField = new List<string>();
                listGroupField.Add("KhieuNaiId");


                // Định nghĩa hình dạng theo mẫu báo cáo
                DataTable dResult = new DataTable();
                dResult.Columns.Add("SoTB");
                dResult.Columns.Add("MaKN");
                dResult.Columns.Add("NoiDungPA");
                dResult.Columns.Add("NguoiTiepNhan");
                dResult.Columns.Add("NguoiXuLy");
                dResult.Columns.Add("NguoiDongKN");
                dResult.Columns.Add("ThoiGianTiepNhan");
                dResult.Columns.Add("ThoiGianDongKN");
                dResult.Columns.Add("CapPheDuyetGiamTru");
                dResult.Columns.Add("SoTienGiamTru");
                 

                var queryOption2 = new QueryOptions();
                var extraParam2 = new Dictionary<string, string>();
                extraParam2.Add("fl", @"KhieuNaiId,SoThueBao,NoiDungPA,NguoiXuLy,SoTienFinal");
                queryOption2.ExtraParams = extraParam2;
                queryOption2.Start = 0;
                queryOption2.Rows = int.MaxValue;

                string whereClause2 = "";
                if (dsDonVi != "" && dsDonVi!="1")
                {
                    if (dsDonVi == "2" || dsDonVi == "3" || dsDonVi == "5")
                    {
                        whereClause2 = string.Format("{0} AND KhuVucId:({1})", whereClause2, dsDonVi);
                    }
                    else
                    {
                        whereClause2 = string.Format("{0} AND DoiTacXuLyId:({1})", whereClause2, dsDonVi);
                    }
                }
                                 
                string sfromDate2 = ConvertUtility.ConvertDateTimeToSolr(fromDate);
                string stoDate2 = ConvertUtility.ConvertDateTimeToSolr(toDate);
                //string whereClauseDs2 = string.Format("NgayTiepNhan:[{0} TO {1}] AND TrangThai:3 {2} AND IsDaBuTien:true AND IsDauSo:true AND -LoaiTien:5", sfromDate2, stoDate2, whereClause2);
                string whereClauseDs2 = string.Format("NgayTiepNhan:[{0} TO {1}] AND TrangThai:3 {2} AND IsDaBuTien:true AND -LoaiTien:5", sfromDate2, stoDate2, whereClause2);
                var solrQuery2 = new SolrQuery(whereClauseDs2);
                SolrQueryResults<KhieuNai_SoTienInfo> listKhieuNaiSoTienInfo = null;
                listKhieuNaiSoTienInfo = QuerySolrBase<KhieuNai_SoTienInfo>.QuerySolr(URL_SOLR_SOTIENGIAMTRU, solrQuery2, queryOption2);
                if (listKhieuNaiSoTienInfo!=null)
                {
                    foreach (var item in listKhieuNaiSoTienInfo)
                    {
                        DataRow row6 = dResult.NewRow();
                        row6["SoTB"] = item.SoThueBao;
                        row6["MaKN"] = item.KhieuNaiId;
                        row6["NoiDungPA"] = item.NoiDungPA;
                        row6["NguoiTiepNhan"] = "";
                        row6["NguoiXuLy"] = item.NguoiXuLy;
                        row6["NguoiDongKN"] = "";
                        row6["ThoiGianTiepNhan"] = "";
                        row6["ThoiGianDongKN"] = "";
                        row6["CapPheDuyetGiamTru"] = "";
                        row6["SoTienGiamTru"] = item.SoTienFinal;
                        dResult.Rows.Add(row6);
                    }                  
                }
                return dResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion
    }
}
