using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Globalization;
using System.Transactions;
using SolrNet.Commands.Parameters;
using SolrNet;
using ADDJ.Core.Cache;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNaiImpl : BaseImpl<KhieuNaiInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai";
        }

        public KhieuNaiImpl()
            : base()
        { }

        public KhieuNaiImpl(string connectionString)
            : base(connectionString)
        {

        }


        #region Longlx

        public int CountKhieuNaiInSolr(string where)
        {
            if (!Config.IsCallSolr)
                return 0;
            SolrQuery q = new SolrNet.SolrQuery(where);
            QueryOptions queryOption = new QueryOptions();
            queryOption.Start = 0;
            queryOption.Rows = 0;

            var extraParam = new Dictionary<string, string>();
            extraParam.Add("fl", "");
            queryOption.ExtraParams = extraParam;

            string URL_SOLR = Config.ServerSolr + "GQKN/";

            var lst = QuerySolrBase<object>.QuerySolr(URL_SOLR, q, queryOption);
            if (lst != null)
            {
                return lst.NumFound;
            }
            return 0;
        }

        public List<KhieuNaiSolrInfo> LichSuKhieuNai(string SoThueBao, int pIndex, int pSize, string qType, string qValue, string sortname, string sortorder, ref int totalRecord)
        {
            //if (!Config.IsCallSolr)
            //    return null;

            string strWhereClause = "SoThueBao:" + SoThueBao;
            if (!string.IsNullOrEmpty(qValue))
            {
                switch (qType)
                {
                    case "MaKhieuNai":
                        strWhereClause += " AND Id:" + qValue;
                        break;
                    case "NgayTiepNhanSort":
                        try
                        {
                            var dTemp = Convert.ToDateTime(qValue, new CultureInfo("vi-VN"));
                            strWhereClause += " AND NgayTiepNhanSort:" + dTemp.ToString("yyyyMMdd");
                        }
                        catch { }
                        break;
                    default:
                        strWhereClause += " AND " + qType + ":" + qValue;
                        break;
                }
            }

            SolrQuery q = new SolrNet.SolrQuery(strWhereClause);
            QueryOptions queryOption = new QueryOptions();
            queryOption.Start = (pIndex - 1) * pSize;
            queryOption.Rows = pSize;

            List<SolrNet.SortOrder> lstOrder = new List<SolrNet.SortOrder>();

            var _TrangThaiOrder = new SolrNet.SortOrder("TrangThai", Order.ASC);
            lstOrder.Add(_TrangThaiOrder);
            var _LDateOrder = new SolrNet.SortOrder("LDate", Order.DESC);
            lstOrder.Add(_LDateOrder);
            if (sortorder.Equals("asc"))
            {
                SolrNet.SortOrder order = new SolrNet.SortOrder(sortname, Order.ASC);
                lstOrder.Add(order);
            }
            else
            {
                SolrNet.SortOrder order = new SolrNet.SortOrder(sortname, Order.DESC);
                lstOrder.Add(order);
            }
            //lstOrder.Add(new SolrNet.SortOrder() { });
            queryOption.OrderBy = lstOrder;

            string URL_SOLR = Config.ServerSolr + "GQKN";

            var lst = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR, q, queryOption);
            if (lst != null)
            {
                totalRecord = lst.NumFound;
                return lst;
            }
            return null;
        }

        public int UpdateNoiDungPA(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@NoiDungPA",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateNoiDungPA", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateNoiDungCanHoTro(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@NoiDungCanHoTro",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateNoiDungCanHoTro", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateGhiChu(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@GhiChu",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateGhiChu", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateDiaDiemXayRa(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@DiaDiemXayRa",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateDiaDiemXayRa", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateDiaChiLienHe(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@DiaChiLienHe",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateDiaChiLienHe", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateThoiGianXayRa(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@ThoiGianXayRa",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateThoiGianXayRa", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateSDTLienHe(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@SDTLienHe",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateSDTLienHe", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int UpdateHoTenLienHe(int id, string luser, string NoiDung)
        {
            int result = 0;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",id),
                                        new SqlParameter("@HoTenLienHe",NoiDung),
                                        new SqlParameter("@LUser",luser),
                                    };
            try
            {
                result = ExecuteNonQuery("usp_KhieuNai_UpdateHoTenLienHe", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            return result;
        }

        public int DongKhieuNai(int Id, string NoiDungXuLyDongKN, short TrangThai, DateTime NgayDongKN, int NguoiXuLyId, string NguoiXuLy, int DoHaiLong = 2, int nguyenNhanLoiId = 39, int chiTietLoiId = 0)
        {

            SqlParameter[] param = {
                                       new SqlParameter("@Id",Id),
                                        new SqlParameter("@NoiDungXuLyDongKN",NoiDungXuLyDongKN),
                                        new SqlParameter("@TrangThai",TrangThai),
                                        new SqlParameter("@NgayDongKN",NgayDongKN),
                                        new SqlParameter("@NgayDongKNSort",NgayDongKN.ToString("yyyyMMdd")),
                                        new SqlParameter("@NguoiXuLyId",NguoiXuLyId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@DoHaiLong",DoHaiLong),
                                        new SqlParameter("@NguyenNhanLoiId", nguyenNhanLoiId),
                                        new SqlParameter("@ChiTietLoiId", chiTietLoiId),
                                        new SqlParameter("@LUser",NguoiXuLy),
                                    };
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_DongKhieuNai", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw;
            }
            return 0;

        }
        #endregion

        #region Function

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 27/02/2014 15:33
        /// Description: Gets the khieu nai_ so tien_ export file.
        /// </summary>
        /// <param name="curentDate">The curent date.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetKhieuNai_SoTien_ExportFile(DateTime curentDate)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@CurentDate",curentDate.ToString("yyyyMMdd"))
                                    };

            DataSet ds = new DataSet();
            try
            {
                ds = ExecuteQueryToDataSet("usp_KhieuNai_SoTien_ExportFile", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return ds.Tables[0];
        }

        public List<KhieuNaiInfo> GetKhieuNaiPhanHoi(int PhongBanXuLy, string DoUuTien, string NguoiXuLy)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLy),
                                        new SqlParameter("@DoUuTien",DoUuTien),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetKhieuNaiPhanHoi", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public int UpdateChuyenNgangHang(int khieuNaiId, int NguoiXuLyId, string NguoiXuLy, int NguoiTienXuLyCap1Id, string NguoiTienXuLyCap1, string LUser)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",khieuNaiId),
                                        new SqlParameter("@NguoiXuLyId",NguoiXuLyId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@NguoiTienXuLyCap1Id",NguoiTienXuLyCap1Id),
                                        new SqlParameter("@NguoiTienXuLyCap1",NguoiTienXuLyCap1),
                                        new SqlParameter("@LUser",LUser),
                                    };
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_UpdateChuyenNgangHang", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }
        }
        /// <summary>
        /// Update cho bảng KhieuNai - Acitivity
        /// </summary>
        public int UpdateKhieuNai_Activity(int khieuNaiId, short TrangThai, int DoiTacXuLyId, int KhuVucXuLyId, int PhongBanXuLy,
           int NguoiXuLyId, string NguoiXuLy, int NguoiTienXuLyCap1Id, string NguoiTienXuLyCap1,
           int NguoiTienXuLyCap2Id, string NguoiTienXuLyCap2, int NguoiTienXuLyCap3Id, string NguoiTienXuLyCap3,
           DateTime NgayChuyenPhongBan, DateTime NgayQuaHanPhongBan, DateTime NgayCanhBaoPhongBan,
           bool isPhanHoi, string LUser)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@Id",khieuNaiId),
                                        new SqlParameter("@TrangThai",TrangThai),
                                        new SqlParameter("@DoiTacXuLyId",DoiTacXuLyId),
                                        new SqlParameter("@KhuVucXuLyId",KhuVucXuLyId),
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLy),
                                        new SqlParameter("@NguoiXuLyId",NguoiXuLyId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@NguoiTienXuLyCap1Id",NguoiTienXuLyCap1Id),
                                        new SqlParameter("@NguoiTienXuLyCap1",NguoiTienXuLyCap1),
                                        new SqlParameter("@NguoiTienXuLyCap2Id",NguoiTienXuLyCap2Id),
                                        new SqlParameter("@NguoiTienXuLyCap2",NguoiTienXuLyCap2),
                                        new SqlParameter("@NguoiTienXuLyCap3Id",NguoiTienXuLyCap3Id),
                                        new SqlParameter("@NguoiTienXuLyCap3",NguoiTienXuLyCap3),
                                        new SqlParameter("@NgayChuyenPhongBan",NgayChuyenPhongBan),
                                        new SqlParameter("@NgayChuyenPhongBanSort",Convert.ToInt32(NgayChuyenPhongBan.ToString("yyyyMMdd"))),
                                        new SqlParameter("@NgayCanhBaoPhongBanXuLy",NgayCanhBaoPhongBan),
                                        new SqlParameter("@NgayCanhBaoPhongBanXuLySort",Convert.ToInt32(NgayCanhBaoPhongBan.ToString("yyyyMMdd"))),
                                        new SqlParameter("@NgayQuaHanPhongBanXuLy",NgayQuaHanPhongBan),
                                        new SqlParameter("@NgayQuaHanPhongBanXuLySort",Convert.ToInt32(NgayQuaHanPhongBan.ToString("yyyyMMdd"))),
                                        new SqlParameter("@IsPhanHoi",isPhanHoi),
                                        new SqlParameter("@LUser",LUser),
                                    };
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_UpdateActivity", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }

        }

        public int UpdateTrangThaiTiepNhan(int khieuNaiId, short TrangThai, int NguoiXuLyId, string NguoiXuLy, int NguoiTienXuLyCap1Id, string NguoiTienXuLyCap1)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@Id",khieuNaiId),
                                        new SqlParameter("@TrangThai",TrangThai),
                                        new SqlParameter("@NguoiXuLyId",NguoiXuLyId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@NguoiTienXuLyCap1Id",NguoiTienXuLyCap1Id),
                                        new SqlParameter("@NguoiTienXuLyCap1",NguoiTienXuLyCap1),
                                    };
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_UpdateTrangThaiTiepNhan_v2", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }

        }

        public List<KhieuNaiInfo> GetListByKhuVucId(int _khuVucId)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@KhuVucId",_khuVucId),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetListByKhuVucId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public List<KhieuNaiInfo> GetListByDoiTacId(int _doiTacId)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId",_doiTacId),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetListByDoiTacId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public List<KhieuNaiInfo> GetListByLoaiKhieuNaiId(int _loaiKhieuNaiId)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@LoaiKhieuNaiId",_loaiKhieuNaiId),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetListByLoaiKhieuNaiId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public List<KhieuNaiInfo> GetListByMaTinh(int _maTinh)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@MaTinh",_maTinh),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetListByMaTinh", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        public List<KhieuNaiInfo> GetListByAllRef(int _khuVucId, int _doiTacId, int _loaiKhieuNaiId, int _maTinh)
        {
            List<KhieuNaiInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@KhuVucId",_khuVucId),
                                        new SqlParameter("@DoiTacId",_doiTacId),
                                        new SqlParameter("@LoaiKhieuNaiId",_loaiKhieuNaiId),
                                        new SqlParameter("@MaTinh",_maTinh),
                                    };
            try
            {
                list = ExecuteQuery("usp_KhieuNai_GetListAllRef", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }


        #region Nghi-nv
        #region DungChung
        public int QLKN_KhieuNaiUpdatePhanViec(int id, int userId, string userName)
        {
            try
            {
                string fieldUpdate = string.Format("NguoiXuLyId={2}, NguoiXuLy = '{0}',IsPhanViec={1}", userName, 1, userId);
                string whereClause = string.Format("Id={0}", id);
                int value = this.UpdateDynamic(fieldUpdate, whereClause);
                return value;

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public KhieuNaiInfo QLKN_KhieuNaigetByID(int id)
        {
            KhieuNaiInfo info = new KhieuNaiInfo();

            try
            {
                info = this.GetInfo(id);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return info;
        }
        #endregion

        #region KhieuNaiChuaXuaLy

        public DataTable QLKN_KhieuNaiChuaXuLyTongHop_GetAllWithPadding(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),

                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuaXuLyTongHop_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiChuaXuLyTongHop_GetAllWithPadding_TotalRecords(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuaXuLyTongHop_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiChuaXuaLy
        public int QLKN_GetTotalKhieuNaiChuaXuLy_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int TotalRecords = 0;
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", ""),
                                            new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("TrangThai", -1),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", -1),
                                            new SqlParameter("NguoiTiepNhan", string.Empty),
                                            new SqlParameter("NguoiXuLy_Filter", string.Empty),
                                            new SqlParameter("NgayTiepNhan_From", -1),
                                            new SqlParameter("NgayTiepNhan_To", -1),
                                            new SqlParameter("NgayQuaHan_From", -1),
                                            new SqlParameter("NgayQuaHan_To", -1),
                                            new SqlParameter("KNHangLoat", -1),
                                            new SqlParameter("GetAllKN", false),
                                            new SqlParameter("IsPermission", false),
                                            new SqlParameter("DoiTacId", 1),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuaXuLy_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }
        public DataTable QLKN_KhieuNaiChuaXuLy_GetAllWithPadding(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuaXuLy_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiChuaXuLy_GetAllWithPadding_TotalRecords(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuaXuLy_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiChuyeBoPhanKhac
        public int QLKN_GetTotalKhieuNaiChuyenBoPhanKhac_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int TotalRecords = 0;
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", ""),
                                            new SqlParameter("TypeSearch", PhongBanId),
                                             new SqlParameter("TrangThai", -1),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanXuLy", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("NguoiXuLy", "-1"),
                                            new SqlParameter("NguoiTienXuLyId", -1),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", -1),
                                            new SqlParameter("NguoiTiepNhanId", -1),
                                            new SqlParameter("NguoiXuLy_Filter", "-1"),
                                            new SqlParameter("NgayTiepNhan_From", -1),
                                            new SqlParameter("NgayTiepNhan_To", -1),
                                            new SqlParameter("NgayQuaHan_From", -1),
                                            new SqlParameter("NgayQuaHan_To", -1),
                                            new SqlParameter("KNHangLoat", -1),
                                            new SqlParameter("GetAllKN", false),
                                            new SqlParameter("IsPermission", false),
                                            new SqlParameter("DoiTacId", 1),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuyenBoPhanKhac_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                return this.ExecuteQuery("usp_KhieuNai_KhieuNaiChuyenBoPhanKhac_GetAllWithPadding", sqlParam);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public int QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuyenBoPhanKhac_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int QLKN_KhieuNaiChuyeBoPhanKhac_GetAllWithPadding_TotalRecords2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiChuyenBoPhanKhac_GetAllWithPadding_v2", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiBoPhanKhacChuyenVe
        //public int QLKN_GetTotalKhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding(int PhongBanId)
        //{
        //    try
        //    {
        //        int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
        //        int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
        //        int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
        //        int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
        //        SqlParameter[] sqlParam = {
        //                                    new SqlParameter("contentSeach", ""),
        //                                    new SqlParameter("TypeSearch", -1),
        //                                    new SqlParameter("TrangThai1", TrangThai1),
        //                                    new SqlParameter("TrangThai2", TrangThai2),
        //                                    new SqlParameter("TrangThai3", TrangThai3),
        //                                    new SqlParameter("LoaiKhieuNaiId", -1),
        //                                    new SqlParameter("LinhVucChungId", -1),
        //                                    new SqlParameter("LinhVucConId", -1),
        //                                    new SqlParameter("PhongBanId", PhongBanId),
        //                                    new SqlParameter("DoUuTien", -1),
        //                                    new SqlParameter("TrangThai", -1),
        //                                    new SqlParameter("NguoiXuLy", "-1"),
        //                                    new SqlParameter("HanhDong", HanhDong),
        //                                    new SqlParameter("SoThueBao", -1),
        //                                    new SqlParameter("NguoiTiepNhan", string.Empty),
        //                                    new SqlParameter("NguoiXuLy_Filter", string.Empty),
        //                                    new SqlParameter("NgayTiepNhan_From", -1),
        //                                    new SqlParameter("NgayTiepNhan_To", -1),
        //                                    new SqlParameter("NgayQuaHan_From", -1),
        //                                    new SqlParameter("NgayQuaHan_To", -1),
        //                                    new SqlParameter("KNHangLoat", -1),         
        //                                    new SqlParameter("GetAllKN", false),  
        //                                    new SqlParameter("IsPermission", false),  
        //                                    new SqlParameter("DoiTacId", 1),      
        //                                    new SqlParameter("StartPageIndex", 1),
        //                                    new SqlParameter("PageSize", 10)
        //                              };
        //        DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
        //        int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
        //        return TotalRecords;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return -1;
        //    }
        //}

        public DataTable QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                //Utility.LogEvent(sqlParam);
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public int QLKN_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding_TotalRecords(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                //Utility.LogEvent(sqlParam);
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiSapQuaHan
        public int QLKN_GetTotalKhieuNaiSapQuaHan_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", ""),
                                            new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("NguoiXuLy", "-1"),
                                            new SqlParameter("SoThueBao", -1),
                                            new SqlParameter("NguoiTiepNhan", string.Empty),
                                            new SqlParameter("NguoiXuLy_Filter", string.Empty),
                                            new SqlParameter("NgayTiepNhan_From", -1),
                                            new SqlParameter("NgayTiepNhan_To", -1),
                                            new SqlParameter("NgayQuaHan_From", -1),
                                            new SqlParameter("NgayQuaHan_To", -1),
                                            new SqlParameter("KNHangLoat", -1),
                                            new SqlParameter("GetAllKN", false),
                                            new SqlParameter("DoiTacId", 1),
                                            new SqlParameter("IsPermission", false),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }
        public DataTable QLKN_KhieuNaiSapQuaHan_GetAllWithPadding1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public DataTable QLKN_KhieuNaiSapQuaHan_GetAllWithPadding2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding_v2", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable QLKN_KhieuNaiSapQuaHan_GetAllWithPadding3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding_v3", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public int QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int QLKN_KhieuNaiSapQuaHan_GetAllWithPadding_TotalRecords2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSapQuaHan_GetAllWithPadding_v2", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiQuaHan
        //public int QLKN_GetTotalKhieuNaiQuaHan_GetAllWithPadding(int PhongBanId)
        //{
        //    try
        //    {
        //        int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

        //        SqlParameter[] sqlParam = {

        //                                    new SqlParameter("contentSeach", ""),
        //                                    new SqlParameter("TypeSearch", -1),
        //                                    new SqlParameter("TrangThai", trangThai),		                                    
        //                                    new SqlParameter("LoaiKhieuNaiId", -1),
        //                                    new SqlParameter("LinhVucChungId", -1),
        //                                    new SqlParameter("LinhVucConId", -1),
        //                                    new SqlParameter("PhongBanId", PhongBanId),
        //                                    new SqlParameter("DoUuTien", -1),                                            
        //                                    new SqlParameter("NguoiXuLy", "-1"),     
        //                                    new SqlParameter("SoThueBao", -1),
        //                                    new SqlParameter("NguoiTiepNhan", string.Empty),
        //                                    new SqlParameter("NguoiXuLy_Filter", string.Empty),
        //                                    new SqlParameter("NgayTiepNhan_From", -1),
        //                                    new SqlParameter("NgayTiepNhan_To", -1),
        //                                    new SqlParameter("NgayQuaHan_From", -1),
        //                                    new SqlParameter("NgayQuaHan_To", -1),
        //                                    new SqlParameter("KNHangLoat", -1),         
        //                                    new SqlParameter("GetAllKN", false),  
        //                                    new SqlParameter("IsPermission", false),  
        //                                    new SqlParameter("DoiTacId", 1),      
        //                                    new SqlParameter("StartPageIndex", 1),
        //                                    new SqlParameter("PageSize", 10)
        //                              };
        //        DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiQuaHan_GetAllWithPadding", sqlParam);
        //        int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
        //        return TotalRecords;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return -1;
        //    }

        //}
        public DataTable QLKN_KhieuNaiQuaHan_GetAllWithPadding(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId,
            int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiQuaHan_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiQuaHan_GetAllWithPadding_TotalRecords(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                          new SqlParameter("contentSeach", contentSeach),
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiDaPhanHoi
        public int QLKN_GetTotalKhieuNaiDaPhanHoi_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int TotalRecords = 0;
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", ""),
                                            new SqlParameter("TypeSearch", PhongBanId),
                                             new SqlParameter("TrangThai", -1),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("NguoiXuLy", "-1"),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", -1),
                                            new SqlParameter("NguoiTiepNhan", string.Empty),
                                            new SqlParameter("NguoiXuLy_Filter", string.Empty),
                                            new SqlParameter("NgayTiepNhan_From", -1),
                                            new SqlParameter("NgayTiepNhan_To", -1),
                                            new SqlParameter("NgayQuaHan_From", -1),
                                            new SqlParameter("NgayQuaHan_To", -1),
                                            new SqlParameter("KNHangLoat", -1),
                                            new SqlParameter("GetAllKN", false),
                                            new SqlParameter("IsPermission", false),
                                            new SqlParameter("DoiTacId", 1),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public DataTable QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }


        public DataTable QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding_v2", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }


        public int QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }


        public int QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding_v2", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhan"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int QLKN_KhieuNaiDaPhanHoi_GetAllWithPadding_TotalRecords3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiDaPhanHoi_GetAllWithPadding_v3", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region SoTheoDoi
        #region KhieuNaiChuaXuaLy

        public DataTable QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding(int Select, int PhongBanId, Int64 SoThueBao, string NguoiXuLy, string NguoiTiepNhan, int ThoiGianTiepNhanTu, int ThoiGianTiepNhanDen,
            bool GetAllKN, int DoiTacId, bool IsPermission, int startPageIndex, int pageSize)
        {

            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("Select", Select),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", IsPermission),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NgayTiepNhanTu", ThoiGianTiepNhanTu),
                                            new SqlParameter("NgayTiepNhanDen", ThoiGianTiepNhanDen),


                                            new SqlParameter("StartPageIndex", startPageIndex),
                                            new SqlParameter("PageSize", pageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSoTheoDoi_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiSoTheoDoi_GetAllWithPadding_TotalRecords(int Select, int PhongBanId, Int64 SoThueBao, string NguoiXuLy, string NguoiTiepNhan, int ThoiGianTiepNhanTu, int ThoiGianTiepNhanDen,
            bool GetAllKN, int DoiTacId, bool IsPermission, int startPageIndex, int pageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("Select", Select),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", IsPermission),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NgayTiepNhanTu", ThoiGianTiepNhanTu),
                                            new SqlParameter("NgayTiepNhanDen", ThoiGianTiepNhanDen),
                                            new SqlParameter("StartPageIndex", startPageIndex),
                                            new SqlParameter("PageSize", pageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiSoTheoDoi_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion
        #endregion

        #region PhanViec
        #region KhieuNaiChuaXuaLy
        public int QLKN_GetTotalKhieuNaiChuaXuLyPhanViec_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int TotalRecords = 0;
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("TrangThai", -1),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecChuaXuLy_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }
        public DataTable QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                             new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecChuaXuLy_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiChuaXuLyPhanViec_GetAllWithPadding_TotalRecords(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                              new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecChuaXuLy_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KhieuNaiBoPhanKhacChuyenVe
        public int QLKN_GetTotalKhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("TrangThai", -1),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        public DataTable QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                               new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiBoPhanKhacChuyenVePhanViec_GetAllWithPadding_TotalRecords(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecBoPhanKhacChuyenVe_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion
        #region KhieuNaiSapQuaHan
        public int QLKN_GetTotalKhieuNaiSapQuaHanPhanViec_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecSapQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }
        public DataTable QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                                  new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecSapQuaHan_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiSapQuaHanPhanViec_GetAllWithPadding_TotalRecords(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                                  new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecSapQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion
        #region KhieuNaiQuaHan
        public int QLKN_GetTotalKhieuNaiQuaHanPhanViec_GetAllWithPadding(int PhongBanId)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", -1),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", -1),
                                            new SqlParameter("LinhVucChungId", -1),
                                            new SqlParameter("LinhVucConId", -1),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", -1),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", 1),
                                            new SqlParameter("PageSize", 10)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        public DataTable QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecQuaHan_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_KhieuNaiQuaHanPhanViec_GetAllWithPadding_TotalRecords(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, string NguoiXuLy, int StartPageIndex, int PageSize, bool isPhanHoi = false)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", ""),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_PhanViecQuaHan_GetAllWithPadding", sqlParam);
                int TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion
        #endregion
        #endregion

        #region Hanh dong xu ly
        public int QLKN_HanhDongXuLy_GetAllWithPadding_TotalRecords(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, string SoThueBao,
            int NguoiTiepNhanId, int NguoiXuLyId, string maKhieuNai, string NgayTiepNhan_tu, string NgayTiepNhan_den, string ThoiGianCapNhat_tu, string ThoiGianCapNhat_den, int trangthai, int StartPageIndex, int PageSize)
        {
            try
            {
                Int32 KhieuNaiId = -1;
                DateTime dateFrom = new DateTime();
                DateTime dateTo = new DateTime();
                if (NgayTiepNhan_tu == "-1" || NgayTiepNhan_den == "-1")
                {
                    dateFrom = new DateTime(1900, 1, 1);
                    dateTo = DateTime.Now;
                }
                else
                {
                    var dateFromItem = NgayTiepNhan_tu.Split('/');
                    var dateToItem = NgayTiepNhan_den.Split('/');
                    if (dateFromItem.Length > 0)
                    {
                        dateFrom = new DateTime(Convert.ToInt32(dateFromItem[2]), Convert.ToInt32(dateFromItem[1]), Convert.ToInt32(dateFromItem[0]));
                    }
                    if (dateToItem.Length > 0)
                    {
                        dateTo = new DateTime(Convert.ToInt32(dateToItem[2]), Convert.ToInt32(dateToItem[1]), Convert.ToInt32(dateToItem[0]));
                    }
                }

                DateTime dateUpdateFrom = new DateTime();
                DateTime dateUpdateTo = new DateTime();
                if (ThoiGianCapNhat_tu == "-1" || ThoiGianCapNhat_den == "-1")
                {
                    dateUpdateFrom = new DateTime(1900, 1, 1);
                    dateUpdateTo = DateTime.Now;
                }
                else
                {
                    var dateUpdateFromItem = ThoiGianCapNhat_tu.Split('/');
                    var dateUpdateToItem = ThoiGianCapNhat_den.Split('/');
                    if (dateUpdateFromItem.Length > 0)
                    {
                        dateUpdateFrom = new DateTime(Convert.ToInt32(dateUpdateFromItem[2]), Convert.ToInt32(dateUpdateFromItem[1]), Convert.ToInt32(dateUpdateFromItem[0]));
                    }
                    if (dateUpdateToItem.Length > 0)
                    {
                        dateUpdateTo = new DateTime(Convert.ToInt32(dateUpdateToItem[2]), Convert.ToInt32(dateUpdateToItem[1]), Convert.ToInt32(dateUpdateToItem[0]));
                    }
                }


                if (!string.IsNullOrEmpty(maKhieuNai))
                {
                    try
                    {
                        KhieuNaiId = Convert.ToInt32(maKhieuNai.Replace(Constant.PREFIX_PHIEU_KHIEU_NAI, ""));
                    }
                    catch
                    {
                        KhieuNaiId = -1;
                    }
                }
                SqlParameter[] sqlParam = {
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("KhieuNaiId", KhieuNaiId),
                                            new SqlParameter("NgayTiepNhan_tu", dateFrom),
                                            new SqlParameter("NgayTiepNhan_den", dateTo),
                                            new SqlParameter("ThoiGianCapNhat_tu", dateUpdateFrom),
                                            new SqlParameter("ThoiGianCapNhat_den", dateUpdateTo),
                                            new SqlParameter("TrangThai", trangthai)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountHanhDongXuLy", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public DataTable QLKN_HanhDongXuLy_GetAllWithPadding(int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, string SoThueBao,
            int NguoiTiepNhanId, int NguoiXuLyId, string maKhieuNai, string NgayTiepNhan_tu, string NgayTiepNhan_den, string ThoiGianCapNhat_tu, string ThoiGianCapNhat_den, int trangthai, int StartPageIndex, int PageSize)
        {
            try
            {
                Int32 KhieuNaiId = -1;
                DateTime dateFrom = new DateTime();
                DateTime dateTo = new DateTime();
                if (NgayTiepNhan_tu == "-1" || NgayTiepNhan_den == "-1")
                {
                    dateFrom = new DateTime(1900, 1, 1);
                    dateTo = DateTime.Now;
                }
                else
                {
                    var dateFromItem = NgayTiepNhan_tu.Split('/');
                    var dateToItem = NgayTiepNhan_den.Split('/');
                    if (dateFromItem.Length > 0)
                    {
                        dateFrom = new DateTime(Convert.ToInt32(dateFromItem[2]), Convert.ToInt32(dateFromItem[1]), Convert.ToInt32(dateFromItem[0]));
                    }
                    if (dateToItem.Length > 0)
                    {
                        dateTo = new DateTime(Convert.ToInt32(dateToItem[2]), Convert.ToInt32(dateToItem[1]), Convert.ToInt32(dateToItem[0]));
                    }
                }

                DateTime dateUpdateFrom = new DateTime();
                DateTime dateUpdateTo = new DateTime();
                if (ThoiGianCapNhat_tu == "-1" || ThoiGianCapNhat_den == "-1")
                {
                    dateUpdateFrom = new DateTime(1900, 1, 1);
                    dateUpdateTo = DateTime.Now;
                }
                else
                {
                    var dateUpdateFromItem = ThoiGianCapNhat_tu.Split('/');
                    var dateUpdateToItem = ThoiGianCapNhat_den.Split('/');
                    if (dateUpdateFromItem.Length > 0)
                    {
                        dateUpdateFrom = new DateTime(Convert.ToInt32(dateUpdateFromItem[2]), Convert.ToInt32(dateUpdateFromItem[1]), Convert.ToInt32(dateUpdateFromItem[0]));
                    }
                    if (dateUpdateToItem.Length > 0)
                    {
                        dateUpdateTo = new DateTime(Convert.ToInt32(dateUpdateToItem[2]), Convert.ToInt32(dateUpdateToItem[1]), Convert.ToInt32(dateUpdateToItem[0]));
                    }
                }

                if (!string.IsNullOrEmpty(maKhieuNai))
                {
                    try
                    {
                        KhieuNaiId = Convert.ToInt32(maKhieuNai.Replace("PA-", ""));
                    }
                    catch
                    {
                        KhieuNaiId = -1;
                    }
                }
                //int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("KhieuNaiId", KhieuNaiId),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NgayTiepNhan_tu", dateFrom),
                                            new SqlParameter("NgayTiepNhan_den", dateTo),
                                            new SqlParameter("ThoiGianCapNhat_tu", dateUpdateFrom),
                                            new SqlParameter("ThoiGianCapNhat_den", dateUpdateTo),
                                            new SqlParameter("TrangThai", trangthai),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_GetHanhDongXuLy_WithPage", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;

                //DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNai_HanhDongXuLy_GetAllWithPadding", sqlParam);
                //DataTable tabReturn = dt.Tables[0];
                //return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion

        #region KhieuNaiCanhBao
        public int KhieuNai_KhieuNaiCanhBaoTotalRecords_GetAllWithPadding(string userName, int StartPageIndex, int PageSize)
        {
            try
            {
                int TotalRecords = 0;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {

                                            new SqlParameter("NguoiDuocPhanHoi", userName),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiCanhBao_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        public DataTable KhieuNai_KhieuNaiCanhBao_GetAllWithPadding(string userName, int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                             new SqlParameter("NguoiDuocPhanHoi", userName),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiCanhBao_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public DataSet KhieuNai_KhieuNaiCanhBao_GetAllWithPaddingNew(string userName, int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                             new SqlParameter("NguoiDuocPhanHoi", userName),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_KhieuNaiCanhBao_GetAllWithPadding", sqlParam);
                return dt;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion
        #endregion

        #region Longlx

        #region Tat ca KN
        public int CountTongSoKhieuNai(int PhongBanId)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId)

                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }


        public int CountTongSoKhieuNai_WithPage1(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_WithPage_v1", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int CountTongSoKhieuNai_WithPage2(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_WithPage_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int CountTongSoKhieuNai_WithPage3(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_WithPage_v3", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }


        public List<KhieuNaiInfo> GetTongSoKhieuNai_WithPage1(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
             int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_GetTongSoKhieuNai_WithPage_v1", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> GetTongSoKhieuNai_WithPage2(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_GetTongSoKhieuNai_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// Todo : Tìm kiếm theo tên lĩnh vực con
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="LinhVucConId"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> GetTongSoKhieuNai_WithPage3(string contentSeach, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("@contentSeach", contentSeach),
                                            new SqlParameter("@TrangThai", TrangThai),
                                            new SqlParameter("@LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("@LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("@TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("@PhongBanId", PhongBanId),
                                            new SqlParameter("@DoUuTien", DoUuTien),
                                            new SqlParameter("@NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("@NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("@NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("@SoThueBao", SoThueBao),
                                            new SqlParameter("@NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("@NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("@NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("@NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("@NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("@NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("@NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("@KNHangLoat", KNHangLoat),
                                            new SqlParameter("@GetAllKN", GetAllKN),
                                            new SqlParameter("@DoiTacId", DoiTacId),
                                            new SqlParameter("@IsPermission", isPermission),
                                            new SqlParameter("@SortName", sortName),
                                            new SqlParameter("@SortOrder", sortOrder),
                                            new SqlParameter("@StartPageIndex", StartPageIndex),
                                            new SqlParameter("@PageSize", PageSize)
                                      };

                List<KhieuNaiInfo> lst = this.ExecuteQuery("usp_KhieuNai_GetTongSoKhieuNai_WithPage_v3", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion

        #region KN Cho xu ly

        public int Count_KhieuNaiChoXuLy_Dashboard(string NguoiXuLy)
        {
            try
            {
                int TrangThai = (int)KhieuNai_TrangThai_Type.Đóng;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("TrangThai", TrangThai)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiChoXuLy_Dashboard", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        public DataTable Get_KhieuNaiChoXuLy_Dashboard_WithPage(string NguoiXuLy, int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai = (int)KhieuNai_TrangThai_Type.Đóng;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                DataSet ds = this.ExecuteQueryToDataSet("usp_KhieuNai_Get_KhieuNaiChoXuLy_Dashboard_WithPage", sqlParam);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public int CountTongSoKhieuNai_ChoXuLy(int PhongBanId)
        {
            try
            {
                byte HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("HanhDong", HanhDong)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_ChoXuLy", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }

        public int CountKhieuNai_ChoXuLy_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_ChoXuLy_WithPage", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int CountKhieuNai_ChoXuLy_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_ChoXuLy_WithPage_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="LinhVucConId"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int CountKhieuNai_ChoXuLy_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_ChoXuLy_WithPage_v3", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }


        public List<KhieuNaiInfo> GetKhieuNai_ChoXuLy_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_ChoXuLy_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> GetKhieuNai_ChoXuLy_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_ChoXuLy_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// Todo : Tìm kiếm theo tên lĩnh vực con
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="TenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> GetKhieuNai_ChoXuLy_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_ChoXuLy_WithPage_v3", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion

        #region KN Chuyen BP khac
        public int CountTongSoKhieuNai_ChuyenBoPhanKhac(int PhongBanId)
        {
            try
            {
                byte HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("HanhDong", HanhDong)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_ChuyenBoPhanKhac", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiChuyenBoPhanKhac1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiChuyenBoPhanKhac", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiChuyenBoPhanKhac2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiChuyenBoPhanKhac_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="TenLinhVucCon"></param>
        /// <param name="PhongBanXuLy"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int Count_KhieuNaiChuyenBoPhanKhac3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiChuyenBoPhanKhac_v3", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiChuyenBoPhanKhac_WithPage(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                return this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiChuyenBoPhanKhac_WithPage", sqlParam);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiChuyenBoPhanKhac_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                return this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiChuyenBoPhanKhac_WithPage", sqlParam);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiChuyenBoPhanKhac_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string LinhVucConId, int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                return this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiChuyenBoPhanKhac_WithPage_v2", sqlParam);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>


        /// Thường xuyên deadlock ở đây
        public List<KhieuNaiInfo> Get_KhieuNaiChuyenBoPhanKhac_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string tenLinhVucCon, int PhongBanXuLy, int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
             int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanXuLy", PhongBanXuLy),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                             new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                return this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiChuyenBoPhanKhac_WithPage_v3", sqlParam);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion

        #region KN BP Chuyen ve
        public int CountTongSoKhieuNai_BoPhanKhacChuyenVe(int PhongBanId)
        {
            try
            {
                byte HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("HanhDong", HanhDong)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_BoPhanKhacChuyenVe", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiBoPhanKhacChuyenVe1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                //Utility.LogEvent(sqlParam);
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiBoPhanKhacChuyenVe", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiBoPhanKhacChuyenVe2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                //Utility.LogEvent(sqlParam);
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiBoPhanKhacChuyenVe_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int Count_KhieuNaiBoPhanKhacChuyenVe3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                //Utility.LogEvent(sqlParam);
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiBoPhanKhacChuyenVe_v3", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiBoPhanKhacChuyenVe_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiBoPhanKhacChuyenVe_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiBoPhanKhacChuyenVe_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiBoPhanKhacChuyenVe_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiBoPhanKhacChuyenVe_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiBoPhanKhacChuyenVe_WithPage_v3", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion

        #region KN Sap qua han
        public int CountTongSoKhieuNai_SapQuaHan(int PhongBanId)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_SapQuaHan", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        #endregion

        #region KN Qua han
        public int CountTongSoKhieuNai_QuaHan(int PhongBanId)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_QuaHan", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiQuaHan1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                          new SqlParameter("contentSeach", contentSeach),
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiQuaHan", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiQuaHan2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                          new SqlParameter("contentSeach", contentSeach),
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiQuaHan_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int Count_KhieuNaiQuaHan3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId,
            int LinhVucChungId, string tenLinhVucCon, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                          new SqlParameter("contentSeach", contentSeach),
                                           new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiQuaHan_v3", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiQuaHan_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId,
            int LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiQuaHan_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiQuaHan_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId,
            string LinhVucConId, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
            int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiQuaHan_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="NguoiXuLyId"></param>
        /// <param name="NguoiTienXuLyId"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhanId"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> Get_KhieuNaiQuaHan_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId,
           string tenLinhVucCon, int PhongBanId, int DoUuTien, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
           int StartPageIndex, int PageSize)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiQuaHan_WithPage_v3", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 12/02/2014 15:13
        /// Description: Lay danh sach cac phong ban va so kieu nai qua han cua phong ban do
        /// </summary>
        /// <param name="doiTacIds">The doi tac ids.</param>
        /// <returns>DataTable.</returns>
        public DataTable GetKhieuNaiQuaHan_GoupBy_PhongBan(string doiTacIds)
        {
            try
            {
                int trangThai = (int)KhieuNai_TrangThai_Type.Đóng;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("TrangThai", trangThai),
                                            new SqlParameter("DoiTacIds", doiTacIds)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_GetKhieuNaiQuaHan_Send_SMS_Email", sqlParam);
                if (dt != null && dt.Tables.Count > 0)
                {
                    return dt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        #endregion

        #region KN Da Phan Hoi
        public int CountTongSoKhieuNai_DaPhanHoi(int PhongBanId)
        {
            try
            {
                byte HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("HanhDong", HanhDong),
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_DaPhanHoi", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int Count_KhieuNaiDaPhanHoi(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int TrangThai1 = (int)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                int TrangThai2 = (int)KhieuNai_TrangThai_Type.Đang_xử_lý;
                int TrangThai3 = (int)KhieuNai_TrangThai_Type.Chờ_đóng;
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai1", TrangThai1),
                                            new SqlParameter("TrangThai2", TrangThai2),
                                            new SqlParameter("TrangThai3", TrangThai3),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var obj = this.ExecuteScalar("usp_KhieuNai_Count_KhieuNaiDaPhanHoi", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiDaPhanHoi_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiDaPhanHoi_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> Get_KhieuNaiDaPhanHoi_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiDaPhanHoi_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/07/2014
        /// </summary>
        /// <param name="contentSeach"></param>
        /// <param name="TypeSearch"></param>
        /// <param name="LoaiKhieuNaiId"></param>
        /// <param name="LinhVucChungId"></param>
        /// <param name="tenLinhVucCon"></param>
        /// <param name="PhongBanId"></param>
        /// <param name="DoUuTien"></param>
        /// <param name="TrangThai"></param>
        /// <param name="NguoiXuLy"></param>
        /// <param name="SoThueBao"></param>
        /// <param name="NguoiTiepNhan"></param>
        /// <param name="NguoiXuLy_Filter"></param>
        /// <param name="NgayTiepNhan_From"></param>
        /// <param name="NgayTiepNhan_To"></param>
        /// <param name="NgayQuaHan_From"></param>
        /// <param name="NgayQuaHan_To"></param>
        /// <param name="KNHangLoat"></param>
        /// <param name="GetAllKN"></param>
        /// <param name="DoiTacId"></param>
        /// <param name="isPermission"></param>
        /// <param name="sortName"></param>
        /// <param name="sortOrder"></param>
        /// <param name="StartPageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> Get_KhieuNaiDaPhanHoi_WithPage3(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string tenLinhVucCon,
            int PhongBanId, int DoUuTien, int TrangThai, string NguoiXuLy,
            int SoThueBao, string NguoiTiepNhan, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
            int NgayQuaHanPB_From, int NgayQuaHanPB_To, int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
            int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("TenLinhVucCon", tenLinhVucCon),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("NguoiXuLy", NguoiXuLy),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhan", NguoiTiepNhan),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("NgayQuaHanPB_From", NgayQuaHanPB_From),
                                            new SqlParameter("NgayQuaHanPB_To", NgayQuaHanPB_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };

                var lst = this.ExecuteQuery("usp_KhieuNai_Get_KhieuNaiDaPhanHoi_WithPage_v3", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        #endregion


        #region KN Da Gui Di
        /// <summary>
        /// Lay tong so ban kn da gui di
        /// </summary>
        /// <param name="PhongBanId">Id phong ban </param>
        /// <returns>Tong so ban ghi kn da gui</returns>
        public int CountTongSoKhieuNai_DaGuiDi(int PhongBanId)
        {
            try
            {
                byte HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("HanhDong", HanhDong)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_DaGuiDi", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj, -1);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }

        public int CountKhieuNai_DaGuiDi_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
          int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
          int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
          int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", "LDate"),
                                            new SqlParameter("SortOrder", "DESC"),

                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_DaGuiDi_WithPage", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int CountKhieuNai_DaGuiDi_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
          int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
          int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
          int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", "LDate"),
                                            new SqlParameter("SortOrder", "DESC"),

                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_DaGuiDi_WithPage_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public List<KhieuNaiInfo> GetKhieuNai_DaGuiDi_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
           int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Tạo_Mới;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_DaGuiDi_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> GetKhieuNai_DaGuiDi_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
           int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.Tạo_Mới;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_DaGuiDi_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        #endregion
        #region KN Phan hoi
        public int CountKhieuNai_PhanHoi_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
      int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
      int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
      int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", "LDate"),
                                            new SqlParameter("SortOrder", "DESC"),

                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_PhanHoi_WithPage", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }

        public int CountKhieuNai_PhanHoi_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
      int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
      int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission,
      int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", "LDate"),
                                            new SqlParameter("SortOrder", "DESC"),

                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var obj = this.ExecuteScalar("usp_KhieuNai_CountTongSoKhieuNai_PhanHoi_WithPage_v2", sqlParam);
                if (obj != null)
                {
                    return ConvertUtility.ToInt32(obj);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }


        public List<KhieuNaiInfo> GetKhieuNai_PhanHoi_WithPage1(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, int LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
           int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_PhanHoi_WithPage", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public List<KhieuNaiInfo> GetKhieuNai_PhanHoi_WithPage2(string contentSeach, int TypeSearch, int LoaiKhieuNaiId, int LinhVucChungId, string LinhVucConId, int PhongBanId, int DoUuTien, int TrangThai, int NguoiXuLyId, int NguoiTienXuLyId,
           int SoThueBao, int NguoiTiepNhanId, string NguoiXuLy_Filter, int NgayTiepNhan_From, int NgayTiepNhan_To, int NgayQuaHan_From, int NgayQuaHan_To,
           int KNHangLoat, bool GetAllKN, int DoiTacId, bool isPermission, string sortName, string sortOrder,
           int StartPageIndex, int PageSize)
        {
            try
            {
                int HanhDong = (int)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;

                SqlParameter[] sqlParam = {
                                            new SqlParameter("contentSeach", contentSeach),
                                            new SqlParameter("TypeSearch", TypeSearch),
                                            new SqlParameter("TrangThai", TrangThai),
                                            new SqlParameter("LoaiKhieuNaiId", LoaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", LinhVucChungId),
                                            new SqlParameter("LinhVucConId", LinhVucConId),
                                            new SqlParameter("PhongBanId", PhongBanId),
                                            new SqlParameter("DoUuTien", DoUuTien),
                                            new SqlParameter("NguoiXuLyId", NguoiXuLyId),
                                            new SqlParameter("NguoiTienXuLyId", NguoiTienXuLyId),
                                            new SqlParameter("HanhDong", HanhDong),
                                            new SqlParameter("SoThueBao", SoThueBao),
                                            new SqlParameter("NguoiTiepNhanId", NguoiTiepNhanId),
                                            new SqlParameter("NguoiXuLy_Filter", NguoiXuLy_Filter),
                                            new SqlParameter("NgayTiepNhan_From", NgayTiepNhan_From),
                                            new SqlParameter("NgayTiepNhan_To", NgayTiepNhan_To),
                                            new SqlParameter("NgayQuaHan_From", NgayQuaHan_From),
                                            new SqlParameter("NgayQuaHan_To", NgayQuaHan_To),
                                            new SqlParameter("KNHangLoat", KNHangLoat),
                                            new SqlParameter("GetAllKN", GetAllKN),
                                            new SqlParameter("DoiTacId", DoiTacId),
                                            new SqlParameter("IsPermission", isPermission),
                                            new SqlParameter("SortName", sortName),
                                            new SqlParameter("SortOrder", sortOrder),
                                            new SqlParameter("StartPageIndex", StartPageIndex),
                                            new SqlParameter("PageSize", PageSize)
                                      };
                var lst = this.ExecuteQuery("usp_KhieuNai_GetKhieuNai_PhanHoi_WithPage_v2", sqlParam);
                return lst;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        #endregion


        public int CountKhieuNaiPhanHoi(int UserId)
        {

            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("select COUNT(1) from KhieuNai where TrangThai != 3 AND IsPhanHoi = 1 AND NguoiTienXuLyCap3Id = {0}", UserId);

            try
            {
                var result = ExecuteScalar(command);
                if (result == null)
                    return 0;
                return ConvertUtility.ToInt32(result);
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region Phân việc

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/10/2014
        /// Todo : Lấy các khiếu nại để phân việc cho người dùng trong phòng ban
        /// </summary>
        /// <param name="typeSearch"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="doUuTien"></param>
        /// <param name="trangThai"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="isPhanHoi"></param>
        /// <param name="ngayQuaHanPhongBanXuLySortTu"></param>
        /// <param name="ngayQuaHanPhongBanXuLySortDen"></param>
        /// <param name="ngayCanhBaoQuaHanPhongBanXuLySortTu"></param>
        /// <param name="ngayCanhBaoQuaHanPhongBanXuLySortDen"></param>
        /// <param name="startPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable ListKhieuNaiPhanViec(int typeKhieuNai, int typeSearch, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId, int phongBanId, string doUuTien,
                                                string nguoiXuLy, bool isPhanHoi, int ngayQuaHanPhongBanXuLySortTu, int ngayQuaHanPhongBanXuLySortDen,
                                                int ngayCanhBaoQuaHanPhongBanXuLySortTu, int ngayCanhBaoQuaHanPhongBanXuLySortDen, int startPageIndex, int pageSize, ref int totalRecord)
        {
            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("TypeKhieuNai", typeKhieuNai),
                                            new SqlParameter("TypeSearch", typeSearch),
                                            new SqlParameter("LoaiKhieuNaiId", loaiKhieuNaiId),
                                            new SqlParameter("LinhVucChungId", linhVucChungId),
                                            new SqlParameter("LinhVucConId", linhVucConId),
                                            new SqlParameter("PhongBanId", phongBanId),
                                            new SqlParameter("DoUuTien", doUuTien),
                                            new SqlParameter("NguoiXuLy", nguoiXuLy),
                                            new SqlParameter("IsPhanHoi", isPhanHoi),
                                            new SqlParameter("NgayCanhBaoPhongBanXuLySortTu", ngayCanhBaoQuaHanPhongBanXuLySortTu),
                                            new SqlParameter("NgayCanhBaoPhongBanXuLySortDen", ngayCanhBaoQuaHanPhongBanXuLySortDen),
                                            new SqlParameter("NgayQuaHanPhongBanXuLySortTu", ngayQuaHanPhongBanXuLySortTu),
                                            new SqlParameter("NgayQuaHanPhongBanXuLySortDen", ngayQuaHanPhongBanXuLySortDen),
                                            new SqlParameter("StartPageIndex", startPageIndex),
                                            new SqlParameter("PageSize", pageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_KhieuNai_DanhSachPhanViec", sqlParam);
                totalRecord = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);

                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }

        }

        #endregion

        #region Danh sách nguồn khiếu nại

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/06/2015
        /// </summary>
        /// <param name="isFirstRowEmpty"></param>
        /// <returns></returns>
        public DataTable GetListNguonKhieuNai(bool isFirstRowEmpty)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Id");
            dtResult.Columns.Add("Name");

            if (isFirstRowEmpty)
            {
                dtResult.Rows.Add("-1", "-- Chọn nguồn khiếu nại --");
            }

            dtResult.Rows.Add(0, KhieuNai_NguonKhieuNai.GQKN);
            dtResult.Rows.Add(1, KhieuNai_NguonKhieuNai.HNI);
            dtResult.Rows.Add(2, KhieuNai_NguonKhieuNai.IB);
            dtResult.Rows.Add(3, KhieuNai_NguonKhieuNai.OB_SAM);
            return dtResult;
        }

        #endregion

    }
}
