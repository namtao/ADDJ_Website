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
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiKhieuNai2PhongBan
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class LoaiKhieuNai2PhongBanImpl : BaseImpl<LoaiKhieuNai2PhongBanInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiKhieuNai2PhongBan";
        }
		
		#region Function 
		
		public List<LoaiKhieuNai2PhongBanInfo> GetListByLoaiKhieuNaiId(int _loaiKhieuNaiId)
		{
			List<LoaiKhieuNai2PhongBanInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@LoaiKhieuNaiId",_loaiKhieuNaiId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiKhieuNai2PhongBan_GetListByLoaiKhieuNaiId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<LoaiKhieuNai2PhongBanInfo> GetListByPhongBanId(int _phongBanId)
		{
			List<LoaiKhieuNai2PhongBanInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@PhongBanId",_phongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiKhieuNai2PhongBan_GetListByPhongBanId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<LoaiKhieuNai2PhongBanInfo> GetListByAllRef(int _loaiKhieuNaiId, int _phongBanId)
		{
			List<LoaiKhieuNai2PhongBanInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@LoaiKhieuNaiId",_loaiKhieuNaiId),
										new SqlParameter("@PhongBanId",_phongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiKhieuNai2PhongBan_GetListAllRef", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		



		#endregion
    }
}
