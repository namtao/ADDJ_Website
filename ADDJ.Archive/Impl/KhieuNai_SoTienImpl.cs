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
		#endregion
    }
}
