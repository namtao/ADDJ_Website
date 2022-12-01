using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Transactions;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của LichSuTruyVan
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	public class LichSuTruyVanImpl : BaseImpl<LichSuTruyVanInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LichSuTruyVan";
        }
		
		#region Function 
        public LichSuTruyVanInfo LichSuTruyVanGetByID(int id)
        {
            LichSuTruyVanInfo info = new LichSuTruyVanInfo();

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
        public List<LichSuTruyVanInfo> GetListLichSuTruyVanByName(string Name)
        {
            List<LichSuTruyVanInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@Name",Name),
									};
            try
            {
                list = ExecuteQuery("usp_LichSuTruyVan_GetByName", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }
        public DataTable QLKN_LichSuTruyVan_GetAllWithPadding(string userName, int StartPageIndex, int PageSize)
        {
            try
            {

                SqlParameter[] sqlParam = {
                                            new SqlParameter("UserName", userName),		                                    
		                                    new SqlParameter("StartPageIndex", StartPageIndex),
		                                    new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_QLKN_LichSuTruyVan_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public int QLKN_LichSuTruyVan_GetAllWithPadding_TotalRecords(string userName, int StartPageIndex, int PageSize)
        {

            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("UserName", userName),		                                    
		                                    new SqlParameter("StartPageIndex", StartPageIndex),
		                                    new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("usp_QLKN_LichSuTruyVan_GetAllWithPadding", sqlParam);
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
    }
}
