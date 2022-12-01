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
    /// Class Insert, Update, Delete, Get dữ liệu của PermissionDefault
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	public class PermissionDefaultImpl : BaseImpl<PermissionDefaultInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PermissionDefault";
        }

        #region Longlx

        public List<PermissionDefaultInfo> GetListByAllRef(int _permissionSchemeId, int _defaultId)
        {
            List<PermissionDefaultInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
										new SqlParameter("@DefaultPermissionId",_defaultId),
									};
            try
            {
                list = ExecuteQuery("usp_PermissionDefault_GetListAllRef", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        #endregion
		
		#region Function 
		
		#endregion
    }
}
