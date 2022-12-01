using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Text;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_SoTien
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>06/08/2013</date>
	
	public class KhieuNai_SoTienImpl : BaseImpl<KhieuNai_SoTienInfo>
    {
        public KhieuNai_SoTienImpl()
            : base()
        { }

        public KhieuNai_SoTienImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_SoTien";
        }
		
		#region Function 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KhieuNaiId"></param>
        /// <param name="SoTien"></param>
        /// <param name="Type"></param>
        /// <param name="action">1: Them moi, 2: Update</param>
        /// <returns></returns>
        public int UpdateToKhieuNai(int KhieuNaiId, decimal SoTien, int Type, int TypeOld, int LyDoGiamTru, int action)
        {
            int result = -1;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_KhieuNai_SoTien_Update_To_KhieuNai_v2");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                dsCmd.Parameters.AddWithValue("@Id", KhieuNaiId);
                dsCmd.Parameters.AddWithValue("@SoTien", SoTien);
                dsCmd.Parameters.AddWithValue("@Type", Type);
                dsCmd.Parameters.AddWithValue("@TypeOld", TypeOld);
                dsCmd.Parameters.AddWithValue("@Action", action);
                dsCmd.Parameters.AddWithValue("@LyDoGiamTru", LyDoGiamTru);

                result = dsCmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Utility.LogEvent(se);
                throw se;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/05/2015
        /// Todo : Thực hiện update số tiền khiếu nại
        /// </summary>
        /// <param name="KhieuNaiId"></param>
        /// <param name="SoTien"></param>
        /// <param name="Type"></param>
        /// <param name="TypeOld"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int UpdateToKhieuNai(int KhieuNaiId, decimal SoTien, int Type, int TypeOld, int action)
        {
            int result = -1;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_KhieuNai_SoTien_Update_To_KhieuNai_v3");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                dsCmd.Parameters.AddWithValue("@Id", KhieuNaiId);
                dsCmd.Parameters.AddWithValue("@SoTien", SoTien);
                dsCmd.Parameters.AddWithValue("@Type", Type);
                dsCmd.Parameters.AddWithValue("@TypeOld", TypeOld);
                dsCmd.Parameters.AddWithValue("@Action", action);               

                result = dsCmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Utility.LogEvent(se);
                throw se;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/11/2015
        /// </summary>
        /// <param name="khieuNaiId"></param>
        /// <returns></returns>
        public DataSet CheckValidCloseKhieuNai(int khieuNaiId)
        {
            DataSet dsResult = null;

            try
            {
                string sql = @" DECLARE @SoLuongGiamTru int
                             SELECT @SoLuongGiamTru = COUNT(*) FROM KhieuNai_SoTien
                                WHERE KhieuNaiId = @KhieuNaiId
                             DECLARE @SoLuongKetQuaXuLy int
                             SELECT @SoLuongKetQuaXuLy = COUNT(*) FROM KhieuNai_KetQuaXuLy
                                WHERE KhieuNaiId = @KhieuNaiId

                             IF (@SoLuongGiamTru > 0 AND @SoLuongKetQuaXuLy > 0 OR @SoLuongGiamTru = 0)
                                 BEGIN
                                  SELECT 1
                                 END
                             ELSE
                                 BEGIN
                                  SELECT 0
                                 END;

                            SELECT * FROM KhieuNai_SoTien WHERE IsDaBuTien = 0 AND KhieuNaiId = @KhieuNaiId";
                SqlParameter[] param = { new SqlParameter("@KhieuNaiId", khieuNaiId) };

                dsResult = this.ExecuteQueryToDataSet(sql, CommandType.Text, param);
            }
            catch(Exception ex)
            {
                Utility.LogEvent(ex.Message);
            }

            return dsResult;
        }
		#endregion
    }
}
