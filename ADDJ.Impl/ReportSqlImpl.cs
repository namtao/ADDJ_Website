using ADDJ.Core.Provider;
using ADDJ.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class ReportSqlImpl : BaseImpl<KhieuNai_ReportInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai";
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/08/2014
        /// Todo : Danh sách khiếu nại được chuyển đi từ phongBanId
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <returns></returns>
        public DataTable ListKhieuNaiDaChuyenDonViKhac(int phongBanId, List<int> listPhongBanTiepNhanId)
        {
            DataTable dtResult = new KhieuNai_ActivityImpl().ListKhieuNaiDaChuyenDonViKhac(phongBanId, listPhongBanTiepNhanId);

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/08/2014
        /// Todo : Lấy danh sách khiếu nại quá hạn phòng ban
        /// </summary>
        /// <param name="listDoiTacId"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataTable ListKhieuNaiQuaHanPhongBan(List<int> listDoiTacId, DateTime toDate)
        {
            DataTable dtResult = null;

            string sDoiTacId = string.Empty;

            try
            {
                if (listDoiTacId != null && listDoiTacId.Count > 0)
                {
                    sDoiTacId = string.Format("{0}", listDoiTacId[0]);
                    for (int i = 1; i < listDoiTacId.Count; i++)
                    {
                        sDoiTacId = string.Format("{0},{1}", sDoiTacId, listDoiTacId[i]);
                    }

                    sDoiTacId = string.Format(" AND DoiTacXuLyId IN ({0})", sDoiTacId);
                }

                string sql = string.Format(@"
                                        WITH cte AS
                                        (   
                                            SELECT ROW_NUMBER() OVER (PARTITION BY KhieuNai_Activity.KhieuNaiId ORDER BY KhieuNai_Activity.NgayTiepNhan DESC, KhieuNai_activity.Id DESC ) AS rn,
	                                            KhieuNai_Activity.Id AS ActivityId, KhieuNai.Id AS KhieuNaiId, KhieuNai.SoThueBao, KhieuNai_Activity.PhongBanXuLyTruocId
                                                ,KhieuNai_Activity.PhongBanXuLyId, KhieuNai_Activity.NguoiXuLy, KhieuNai_Activity.NgayTiepNhan_NguoiXuLy
		                                        , KhieuNai_Activity.NgayTiepNhan , KhieuNai_Activity.LDate, KhieuNai_Activity.NgayQuaHan
                                                , KhieuNai.NgayTiepNhan AS NgayTaoKhieuNai, PhongBan.DoiTacId AS DoiTacXuLyId
                                                , KhieuNai.LoaiKhieuNai, KhieuNai.LinhVucChung, KhieuNai.LinhVucCon
	                                        FROM KhieuNai_Activity
		                                        INNER JOIN KhieuNai ON KhieuNai_Activity.KhieuNaiId = KhieuNai.Id
		                                        INNER JOIN PhongBan ON KhieuNai_Activity.PhongBanXuLyId = PhongBan.Id
	                                        WHERE KhieuNai.NgayDongKN > @ToDate
		                                        AND KhieuNai_Activity.NgayTiepNhan	<= @ToDate	
		                                        AND KhieuNai.IsLuuKhieuNai = 0		
                                        )
                                        SELECT cte.*
                                        FROM cte    
                                        WHERE rn = 1		
	                                        AND (NgayQuaHan <= @ToDate)
                                            {0}", sDoiTacId);

                SqlParameter[] param = {
										new SqlParameter("@ToDate", toDate),
									};

                dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 05/06/2014
        /// Todo : Lấy danh sách khiếu nại đang tồn đọng và quá hạn tại các phòng ban của doiTacId
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId">
        ///     != -1 : Lấy theo phongBanId truyền vào
        /// </param>
        /// <returns></returns>
        public DataTable CountKhieuNaiTonDongVaQuaHanTaiThoiDiemHienTai(int doiTacId, int phongBanId)
        {
            DataTable dtResult = null;
            string sql = @"SELECT Id AS PhongBanId, Name AS TenPhongBan,
	                        (SELECT COUNT(Id) FROM KhieuNai
		                        WHERE PhongBanXuLyId = PhongBan.Id
		                        AND TrangThai <> 3) AS SoLuongTonDong,
	                        (SELECT COUNT(Id) FROM KhieuNai
		                        WHERE PhongBanXuLyId = PhongBan.Id
		                        AND TrangThai <> 3
		                        AND NgayQuaHanPhongBanXuLy <= GETDATE()) AS SoLuongQuaHan
                        FROM PhongBan
                        WHERE DoiTacId = @DoiTacId
	                        AND (Id = @PhongBanId OR @PhongBanId = -1)";
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                        new SqlParameter("@PhongBanId", phongBanId),
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 17/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public DataTable CountKhieuNaiTonDongVaQuaHanTaiThoiDiemHienTai(int doiTacId, int phongBanId, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            DataTable dtResult = null;
            string sql = @"SELECT PhongBan.Id AS PhongBanId, Name AS TenPhongBan,
	                        (SELECT COUNT(KhieuNai.Id) FROM KhieuNai
                                INNER JOIN LoaiKhieuNai ON KhieuNai.LoaiKhieuNaiId = LoaiKhieuNai.Id
		                    WHERE PhongBanXuLyId = PhongBan.Id                
		                        AND TrangThai <> 3
                                AND (LoaiKhieuNai_NhomId = @LoaiKhieuNai_NhomId OR @LoaiKhieuNai_NhomId <=0)
                                AND (LoaiKhieuNaiId = @LoaiKhieuNaiId OR @LoaiKhieuNaiId <= 0)
                                AND (LinhVucChungId = @LinhVucChungId OR @LinhVucChungId <= 0)
                                AND (LinhVucConId = @LinhVucConId OR @LinhVucConId <=0)) AS SoLuongTonDong,
	                        (SELECT COUNT(KhieuNai.Id) FROM KhieuNai
                                INNER JOIN LoaiKhieuNai ON KhieuNai.LoaiKhieuNaiId = LoaiKhieuNai.Id
		                    WHERE PhongBanXuLyId = PhongBan.Id
		                        AND TrangThai <> 3
		                        AND NgayQuaHanPhongBanXuLy <= GETDATE()
                                AND (LoaiKhieuNai_NhomId = @LoaiKhieuNai_NhomId OR @LoaiKhieuNai_NhomId <=0)
                                AND (LoaiKhieuNaiId = @LoaiKhieuNaiId OR @LoaiKhieuNaiId <= 0)
                                AND (LinhVucChungId = @LinhVucChungId OR @LinhVucChungId <= 0)
                                AND (LinhVucConId = @LinhVucConId OR @LinhVucConId <=0)) AS SoLuongQuaHan
                        FROM PhongBan
                        WHERE DoiTacId = @DoiTacId
	                        AND (PhongBan.Id = @PhongBanId OR @PhongBanId = -1)";
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                        new SqlParameter("@PhongBanId", phongBanId),
                                        new SqlParameter("@LoaiKhieuNai_NhomId", loaiKhieuNai_NhomId),
                                        new SqlParameter("@LoaiKhieuNaiId", loaiKhieuNaiId),
                                        new SqlParameter("@LinhVucChungId", linhVucChungId),
                                        new SqlParameter("@LinhVucConId", linhVucConId)
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/02/2015
        /// Todo : Thống kê số lượng tồn đọng và quá hạn hiện tại của các user thuộc phongBanId
        /// </summary>
        /// <param name="nguoiXuLy"></param>
        /// <param name="phongBanId"></param>
        /// <returns></returns>
        public DataTable CountKhieuNaiTonDongVaQuaHanHienTaiCuaNguoiDungPhongBan(int phongBanId, string nguoiXuLy)
        {
            DataTable dtResult = null;
            string sql = @"SELECT '' AS TenTruyCap, (SELECT COUNT(1) FROM KhieuNai
		                            WHERE PhongBanXuLyId = @PhongBanId
		                            AND LEN(KhieuNai.NguoiXuLy) = 0
		                            AND TrangThai <> 3) AS SoLuongTonDong,
	                            (SELECT COUNT(1) FROM KhieuNai
		                            WHERE PhongBanXuLyId = @PhongBanId
		                            AND LEN(KhieuNai.NguoiXuLy) = 0
		                            AND TrangThai <> 3
		                            AND NgayQuaHanPhongBanXuLy <= GETDATE()) AS SoLuongQuaHan
                            UNION ALL
                            SELECT TenTruyCap,
	                            (SELECT COUNT(1) FROM KhieuNai
		                            WHERE PhongBanXuLyId = @PhongBanId
		                            AND KhieuNai.NguoiXuLy = NguoiSuDung.TenTruyCap
		                            AND TrangThai <> 3) AS SoLuongTonDong,
	                            (SELECT COUNT(1) FROM KhieuNai
		                            WHERE PhongBanXuLyId = @PhongBanId
		                            AND KhieuNai.NguoiXuLy = NguoiSuDung.TenTruyCap
		                            AND TrangThai <> 3
		                            AND NgayQuaHanPhongBanXuLy <= GETDATE()) AS SoLuongQuaHan
                            FROM NguoiSuDung
	                            INNER JOIN PhongBan_User ON NguoiSuDung.Id = PhongBan_User.NguoiSuDungId
                            WHERE 
	                            (PhongBan_User.PhongBanId = @PhongBanId OR @PhongBanId = -1)
	                            AND (TenTruyCap LIKE @NguoiXuLy OR LEN(@NguoiXuLy) = 0)
                            ORDER BY TenTruyCap ASC";
            SqlParameter[] param = {                                        
                                        new SqlParameter("@PhongBanId", phongBanId),
                                        new SqlParameter("@NguoiXuLy", nguoiXuLy),
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 23/08/2014
        /// Todo : Lấy danh sách khiếu nại đang tồn đọng hoặc quá hạn
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="reportType">
        ///     = 1: Tồn đọng
        ///     = 2 : Quá hạn
        /// </param>
        /// <returns></returns>
        public DataTable ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(int doiTacId, int phongBanId, int reportType)
        {
            DataTable dtResult = null;
            string sql = string.Empty;

            if(reportType == 1)
            {
                sql = @"SELECT Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy FROM KhieuNai
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND DoiTacXuLyId = @DoiTacId
		                    AND TrangThai <> 3";
            }
            else if(reportType == 2)
            {
                sql = @"SELECT Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy FROM KhieuNai
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND DoiTacXuLyId = @DoiTacId
		                    AND TrangThai <> 3
                            AND NgayQuaHanPhongBanXuLy <= GETDATE()";
            }
           
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                        new SqlParameter("@PhongBanId", phongBanId),
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/04/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        /// <param name="reportType"></param>
        /// <param name="loaiKhieuNai_NhomId"></param>
        /// <param name="loaiKhieuNaiId"></param>
        /// <param name="linhVucChungId"></param>
        /// <param name="linhVucConId"></param>
        /// <returns></returns>
        public DataTable ListKhieuNaiDoiTacPhongBanTaiThoiDiemHienTai(int doiTacId, int phongBanId, int reportType, int loaiKhieuNai_NhomId, int loaiKhieuNaiId, int linhVucChungId, int linhVucConId)
        {
            DataTable dtResult = null;
            string sql = string.Empty;

            if (reportType == 1)
            {
                sql = @"SELECT KhieuNai.Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy 
                            , LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA
                        FROM KhieuNai
                            INNER JOIN LoaiKhieuNai ON KhieuNai.LoaiKhieuNaiId = LoaiKhieuNai.Id
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND DoiTacXuLyId = @DoiTacId
		                    AND TrangThai <> 3
                            AND (LoaiKhieuNai_NhomId = @LoaiKhieuNai_NhomId OR @LoaiKhieuNai_NhomId <= 0)
                            AND (LoaiKhieuNaiId = @LoaiKhieuNaiId OR @LoaiKhieuNaiId <= 0)
                            AND (LinhVucChungId = @LinhVucChungId OR @LinhVucChungId <= 0)
                            AND (LinhVucConId = @LinhVucConId OR @LinhVucConId <=0)";
            }
            else if (reportType == 2)
            {
                sql = @"SELECT KhieuNai.Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy
                            , LoaiKhieuNai, LinhVucChung, LinhVucCon, NoiDungPA
                        FROM KhieuNai
                            INNER JOIN LoaiKhieuNai ON KhieuNai.LoaiKhieuNaiId = LoaiKhieuNai.Id
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND DoiTacXuLyId = @DoiTacId
		                    AND TrangThai <> 3
                            AND NgayQuaHanPhongBanXuLy <= GETDATE()
                            AND (LoaiKhieuNai_NhomId = @LoaiKhieuNai_NhomId OR @LoaiKhieuNai_NhomId <= 0)
                            AND (LoaiKhieuNaiId = @LoaiKhieuNaiId OR @LoaiKhieuNaiId <= 0)
                            AND (LinhVucChungId = @LinhVucChungId OR @LinhVucChungId <= 0)
                            AND (LinhVucConId = @LinhVucConId OR @LinhVucConId <=0)";
            }

            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                        new SqlParameter("@PhongBanId", phongBanId),
                                        new SqlParameter("@LoaiKhieuNai_NhomId", loaiKhieuNai_NhomId),
                                        new SqlParameter("@LoaiKhieuNaiId", loaiKhieuNaiId),
                                        new SqlParameter("@LinhVucChungId", linhVucChungId),
                                        new SqlParameter("@LinhVucConId", linhVucConId)
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/02/2015
        /// </summary>
        /// <param name="phongBanId"></param>
        /// <param name="nguoiXuLy"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public DataTable ListKhieuNaiTonDongQuaHanNguoiDungPhongBanHienTai(int phongBanId, string nguoiXuLy, int reportType)
        {
            DataTable dtResult = null;
            string sql = string.Empty;

            if (reportType == 1)
            {
                sql = @"SELECT Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy FROM KhieuNai
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND (NguoiXuLy = @NguoiXuLy OR LEN(@NguoiXuLy) = 0)
		                    AND TrangThai <> 3";
            }
            else if (reportType == 2)
            {
                sql = @"SELECT Id, SoThueBao, PhongBanXuLyId, DoiTacXuLyId, NguoiXuLy, NgayChuyenPhongBan, NgayQuaHanPhongBanXuLy FROM KhieuNai
		                WHERE (PhongBanXuLyId = @PhongBanId OR @PhongBanId = -1)
                            AND (NguoiXuLy = @NguoiXuLy OR LEN(@NguoiXuLy) = 0)
		                    AND TrangThai <> 3
                            AND NgayQuaHanPhongBanXuLy <= GETDATE()";
            }

            SqlParameter[] param = {
                                        new SqlParameter("@NguoiXuLy", nguoiXuLy),
                                        new SqlParameter("@PhongBanId", phongBanId),
                                    };

            dtResult = ExecuteQueryToDataSet(sql, CommandType.Text, param).Tables[0];

            return dtResult;
        }
    }
}
