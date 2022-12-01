using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GQKN.Archive.Entity;
using GQKN.Archive.Core.Provider;
using GQKN.Archive.Core;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_Activity
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNai_ActivityImpl : BaseImpl<KhieuNai_ActivityInfo>
    {
        public KhieuNai_ActivityImpl() : base()
        {}

        public KhieuNai_ActivityImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_Activity";
        }

        #region Function

        public List<KhieuNai_ActivityInfo> GetListByKhieuNaiId(int _khieuNaiId)
        {
            List<KhieuNai_ActivityInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_Activity_GetListByKhieuNaiId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }


        public int QLKN_KhieuNai_ActivityUpdatePhanViec(int KhieuNaiId, string userName)
        {
            try
            {
                string fieldUpdate = string.Format("NguoiXuLy = '{0}'", userName);
                string whereClause = string.Format("KhieuNaiId={0} And IsCurrent = 1", KhieuNaiId);
                int value = this.UpdateDynamic(fieldUpdate, whereClause);
                return value;

            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region Longlx



        public KhieuNai_ActivityInfo GetActivityCurrent(int KhieuNaiId)
        {
            List<KhieuNai_ActivityInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_Activity_GetActivityCurrent", param);
                if (list != null && list.Count > 0)
                    return list[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public List<KhieuNai_ActivityInfo> GetListXuLyTruoc(int KhieuNaiId, int PhongBanXuLyId)
        {
            List<KhieuNai_ActivityInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLyId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_Activity_GetListXuLyTruoc", param);
                return list;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public KhieuNai_ActivityInfo GetActivity4PhanHoi(int KhieuNaiId, int PhongBanXuLyId)
        {
            List<KhieuNai_ActivityInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLyId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_Activity_GetActivity4PhanHoi", param);
                return list[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public List<KhieuNai_ActivityInfo> GetListPhongBanPhanHoi(int KhieuNaiId, int PhongBanXuLyId)
        {
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLyId),
									};
            try
            {
                return ExecuteQuery("usp_KhieuNai_GetListPhongBanPhanHoi", param);                
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public KhieuNai_ActivityInfo GetActivtyLastSend2PhongBan(int KhieuNaiId, int PhongBanXuLyTruocId)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@PhongBanXuLyTruocId",PhongBanXuLyTruocId),
									};
            try
            {
                var lst = ExecuteQuery("usp_KhieuNai_GetActivityLastSend2PhongBan", param);
                if (lst != null && lst.Count > 0)
                {
                    return lst[0];
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public List<KhieuNai_ActivityInfo> GetListTrungTamPhanHoi(int KhieuNaiId, int DoiTacId)
        {
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@DoiTacId",DoiTacId),
									};
            try
            {
                return ExecuteQuery("usp_KhieuNai_GetListTrungTamPhanHoi", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public KhieuNai_ActivityInfo GetActivtyLastSend2TrungTam(int KhieuNaiId, int DoiTacId, int PhongBanXuLyTruocId)
        {
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@DoiTacId",DoiTacId),
                                        new SqlParameter("@PhongBanXuLyTruocId",PhongBanXuLyTruocId),
									};
            try
            {
                var lst = ExecuteQuery("usp_KhieuNai_GetActivtyLastSend2TrungTam", param);
                if (lst != null && lst.Count > 0)
                {
                    return lst[0];
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return null;
        }

        public int GetTotalKhieuNaiPhanHoi(int PhongBanXuLyId, string NguoiXuLy, byte HanhDong)
        {
            SqlParameter[] param = {
										new SqlParameter("@PhongBanXuLyId",PhongBanXuLyId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@HanhDong",HanhDong),
									};
            try
            {
                var obj = ExecuteScalar("usp_KhieuNai_Activity_GetTotalKhieuNaiPhanHoi", param);
                if (obj != null)
                    return Convert.ToInt32(obj);
                return 0;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }
        }

        public int GetTotalKhieuNaiGuiDi(int PhongBanXuLyTruocId, string NguoiXuLyTruoc, byte HanhDong)
        {
            SqlParameter[] param = {
										new SqlParameter("@PhongBanXuLyTruocId",PhongBanXuLyTruocId),
                                        new SqlParameter("@NguoiXuLyTruoc",NguoiXuLyTruoc),
                                        new SqlParameter("@HanhDong",HanhDong),
									};
            try
            {
                var obj = ExecuteScalar("usp_KhieuNai_Activity_GetTotalKhieuNaiGuiDi", param);
                if (obj != null)
                    return Convert.ToInt32(obj);
                return 0;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return 0;
            }
        }

        public List<KhieuNai_ActivityInfo> GetTrungTamActivity(int KhieuNaiId)
        {
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",KhieuNaiId),
									};
            try
            {
                return ExecuteQuery("usp_KhieuNai_Activity_GetTrungTamActivity", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }

        public int UpdateCurentActivity(Int64 id, int KhieuNaiId, string NguoiXuLy)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@KhieuNaiId",KhieuNaiId),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
									};
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_Activity_UpdateCurentActivity", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw;
            }
            return 0;
        }

        public int UpdateTiepNhan(int id, string NguoiXuLy, int PhongBanXuLyId)
        {
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",id),
                                        new SqlParameter("@NguoiXuLy",NguoiXuLy),
                                        new SqlParameter("@PhongBanXuLyId",PhongBanXuLyId),
									};
            try
            {
                return ExecuteNonQuery("usp_KhieuNai_Activity_UpdateTiepNhan", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw;
            }
            return 0;
        }

        #endregion
    }
}
