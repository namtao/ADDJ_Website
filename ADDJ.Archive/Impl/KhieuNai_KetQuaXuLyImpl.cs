using System;
using System.Data;
using System.Data.SqlClient;
using GQKN.Archive.Entity;
using System.Text;
using GQKN.Archive.Core.Provider;
using GQKN.Archive.Core;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_KetQuaXuLy
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/08/2013</date>

    public class KhieuNai_KetQuaXuLyImpl : BaseImpl<KhieuNai_KetQuaXuLyInfo>
    {
        public KhieuNai_KetQuaXuLyImpl()
            : base()
        { }

        public KhieuNai_KetQuaXuLyImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_KetQuaXuLy";
        }

        #region Function

        public int UpdateToKhieuNai(int KhieuNaiId, bool isKNGiamTru)
        {
            int result = -1;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_KhieuNai_KetQuaXuLy_Update_To_KhieuNai");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                dsCmd.Parameters.AddWithValue("@Id", KhieuNaiId);
                dsCmd.Parameters.AddWithValue("@IsKNGiamTru", isKNGiamTru);
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
        /// Created date : 16/05/2014
        /// Todo : Nếu không có giảm trừ nào thì trả về true
        ///		Nếu có giảm trừ và có kết quả xử lý thì trả về true
        ///		Nếu có giảm trừ mà không có kết quả xử lý thì trả về false
        /// </summary>
        /// <param name="khieuNaiId"></param>
        /// <returns></returns>
        public bool IsKetQuaXuLyValidToClose(int khieuNaiId)
        {
            int result = -1;
            var conn = new SqlConnection(ConnectionString);
            var commandText = new StringBuilder("usp_KhieuNai_KetQuaXuLy_HasKetQuaXuLy");
            var dsCmd = new SqlCommand(commandText.ToString(), conn);
            dsCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                dsCmd.Parameters.AddWithValue("@KhieuNaiId", khieuNaiId);
                result = ConvertUtility.ToInt32(dsCmd.ExecuteScalar(), -1);
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

            return result == 1;
        }

        #endregion
    }
}
