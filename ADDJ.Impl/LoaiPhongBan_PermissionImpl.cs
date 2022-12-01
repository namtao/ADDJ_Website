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
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiPhongBan_Permission
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02.04.2014</date>
	
	public class LoaiPhongBan_PermissionImpl : BaseImpl<LoaiPhongBan_PermissionInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiPhongBan_Permission";
        }
		
		#region Function 
		
		public List<LoaiPhongBan_PermissionInfo> GetListByLoaiPhongBanId(int _loaiPhongBanId)
		{
			List<LoaiPhongBan_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@LoaiPhongBanId",_loaiPhongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiPhongBan_Permission_GetListByLoaiPhongBanId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<LoaiPhongBan_PermissionInfo> GetListByPermissionSchemeId(int _permissionSchemeId)
		{
			List<LoaiPhongBan_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiPhongBan_Permission_GetListByPermissionSchemeId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<LoaiPhongBan_PermissionInfo> GetListByAllRef(int _loaiPhongBanId, int _permissionSchemeId)
		{
			List<LoaiPhongBan_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@LoaiPhongBanId",_loaiPhongBanId),
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
									};
			try
			{
				list = ExecuteQuery("usp_LoaiPhongBan_Permission_GetListAllRef", param);
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
