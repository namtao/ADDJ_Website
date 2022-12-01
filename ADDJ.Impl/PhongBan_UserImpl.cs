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
    /// Class Insert, Update, Delete, Get dữ liệu của PhongBan_User
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class PhongBan_UserImpl : BaseImpl<PhongBan_UserInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PhongBan_User";
        }
		
		#region Function 
		
		public List<PhongBan_UserInfo> GetListByNguoiSuDungId(int _nguoiSuDungId)
		{
			List<PhongBan_UserInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
									};
			try
			{
				list = ExecuteQuery("usp_PhongBan_User_GetListByNguoiSuDungId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<PhongBan_UserInfo> GetListByPhongBanId(int _phongBanId)
		{
			List<PhongBan_UserInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@PhongBanId",_phongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_PhongBan_User_GetListByPhongBanId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<PhongBan_UserInfo> GetListByAllRef(int _nguoiSuDungId, int _phongBanId)
		{
			List<PhongBan_UserInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
										new SqlParameter("@PhongBanId",_phongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_PhongBan_User_GetListAllRef", param);
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
