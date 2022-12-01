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
    /// Class Insert, Update, Delete, Get dữ liệu của PhongBan2PhongBan
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class PhongBan2PhongBanImpl : BaseImpl<PhongBan2PhongBanInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PhongBan2PhongBan";
        }
		
		#region Function 
		
		public List<PhongBan2PhongBanInfo> GetListByPhongBanId(int _phongBanId)
		{
			List<PhongBan2PhongBanInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@PhongBanId",_phongBanId),
									};
			try
			{
				list = ExecuteQuery("usp_PhongBan2PhongBan_GetListByPhongBanId", param);
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
