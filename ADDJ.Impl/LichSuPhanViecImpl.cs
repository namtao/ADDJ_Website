using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của LichSuPhanViec
    /// </summary>
    /// <author>Lê Anh Dũng</author>
    /// <date>04/11/2013</date>
	
	public class LichSuPhanViecImpl : BaseImpl<LichSuPhanViecInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LichSuPhanViec";
        }


        #region LichSuPhanViec_Thongke
        public int LichSuPhanViec_ThongkeTotalRecords_GetAllWithPadding(int PhongBanId, string userName,DateTime tuNgay,DateTime denNgay, int StartPageIndex, int PageSize)
        {
            try
            {
                int TotalRecords = 0;

                SqlParameter[] sqlParam = {                                   
		                                   new SqlParameter("PhongBanXuLyId", PhongBanId),
                                            new SqlParameter("NguoiDuocPhanViec", userName),
                                             new SqlParameter("TuNgay", tuNgay.ToString("MM/dd/yyyy") + " " + "00:00:00"),
                                            new SqlParameter("DenNgay", denNgay.ToString("MM/dd/yyyy") + " " + "23:59:59"),
		                                    new SqlParameter("StartPageIndex", StartPageIndex),
		                                    new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("LichSuPhanViec_Thongke_GetAllWithPadding", sqlParam);
                TotalRecords = Convert.ToInt32(dt.Tables[1].Rows[0]["TotalRecords"]);
                return TotalRecords;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }
        }
        public DataTable LichSuPhanViec_Thongke_GetAllWithPadding(int PhongBanId, string userName, DateTime tuNgay, DateTime denNgay, int StartPageIndex, int PageSize)
        {

            try
            {
                SqlParameter[] sqlParam = {
                                            new SqlParameter("PhongBanXuLyId", PhongBanId),
                                            new SqlParameter("NguoiDuocPhanViec", userName),
                                             new SqlParameter("TuNgay", tuNgay.ToString("MM/dd/yyyy") + " " + "00:00:00"),
                                            new SqlParameter("DenNgay", denNgay.ToString("MM/dd/yyyy") + " " + "23:59:59"),
		                                    new SqlParameter("StartPageIndex", StartPageIndex),
		                                    new SqlParameter("PageSize", PageSize)
                                      };
                DataSet dt = this.ExecuteQueryToDataSet("LichSuPhanViec_Thongke_GetAllWithPadding", sqlParam);
                DataTable tabReturn = dt.Tables[0];
                return tabReturn;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
		#endregion
    }
}
