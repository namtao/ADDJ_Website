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
    /// Class Insert, Update, Delete, Get dữ liệu của NguoiSuDung_Permission
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	public class NguoiSuDung_PermissionImpl : BaseImpl<NguoiSuDung_PermissionInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung_Permission";
        }

        #region Longlx

        public List<NguoiSuDung_PermissionInfo> GetListByAllRef(int _permissionSchemeId, int _nguoisudungId)
        {
            List<NguoiSuDung_PermissionInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
										new SqlParameter("@NguoiSuDungId",_nguoisudungId),
									};
            try
            {
                list = ExecuteQuery("usp_NguoiSuDung_Permission_GetListAllRef", param);
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
