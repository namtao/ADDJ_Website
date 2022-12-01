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
    /// Class Insert, Update, Delete, Get dữ liệu của NguoiSuDung_GroupDetail
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class NguoiSuDung_GroupDetailImpl : BaseImpl<NguoiSuDung_GroupDetailInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung_GroupDetail";
        }
		
		#region Function 
		
		public List<NguoiSuDung_GroupDetailInfo> GetListByNguoiSuDungId(int _nguoiSuDungId)
		{
			List<NguoiSuDung_GroupDetailInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
									};
			try
			{
				list = ExecuteQuery("usp_NguoiSuDung_GroupDetail_GetListByNguoiSuDungId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<NguoiSuDung_GroupDetailInfo> GetListByGroupId(int _groupId)
		{
			List<NguoiSuDung_GroupDetailInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@GroupId",_groupId),
									};
			try
			{
				list = ExecuteQuery("usp_NguoiSuDung_GroupDetail_GetListByGroupId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<NguoiSuDung_GroupDetailInfo> GetListByAllRef(int _nguoiSuDungId, int _groupId)
		{
			List<NguoiSuDung_GroupDetailInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
										new SqlParameter("@GroupId",_groupId),
									};
			try
			{
				list = ExecuteQuery("usp_NguoiSuDung_GroupDetail_GetListAllRef", param);
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
