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
    /// Class Insert, Update, Delete, Get dữ liệu của NhomNguoiDung_AI_Permission
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	public class NhomNguoiDung_AI_PermissionImpl : BaseImpl<NhomNguoiDung_AI_PermissionInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NhomNguoiDung_AI_Permission";
        }
		
		#region Function 
		
		public List<NhomNguoiDung_AI_PermissionInfo> GetListByNhomNguoiDung_AIId(int _nhomNguoiDung_AIId)
		{
			List<NhomNguoiDung_AI_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NhomNguoiDung_AIId",_nhomNguoiDung_AIId),
									};
			try
			{
				list = ExecuteQuery("usp_NhomNguoiDung_AI_Permission_GetListByNhomNguoiDung_AIId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<NhomNguoiDung_AI_PermissionInfo> GetListByPermissionSchemeId(int _permissionSchemeId)
		{
			List<NhomNguoiDung_AI_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
									};
			try
			{
				list = ExecuteQuery("usp_NhomNguoiDung_AI_Permission_GetListByPermissionSchemeId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<NhomNguoiDung_AI_PermissionInfo> GetListByAllRef(int _nhomNguoiDung_AIId, int _permissionSchemeId)
		{
			List<NhomNguoiDung_AI_PermissionInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NhomNguoiDung_AIId",_nhomNguoiDung_AIId),
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
									};
			try
			{
				list = ExecuteQuery("usp_NhomNguoiDung_AI_Permission_GetListAllRef", param);
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
