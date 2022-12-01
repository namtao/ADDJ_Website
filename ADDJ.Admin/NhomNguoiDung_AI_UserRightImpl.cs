using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.Admin
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của NhomNguoiDung_AI_UserRight
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	public class NhomNguoiDung_AI_UserRightImpl : BaseImpl<NhomNguoiDung_AI_UserRightInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NhomNguoiDung_AI_UserRight";
        }
		
		#region Function 

        public DataTable GetQuyenByGroupID(int groupID, int type)
        {
            DataTable dt = null;
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@ID", groupID);
            prm[1] = new SqlParameter("@Type", type);

            try
            {
                var ds = this.ExecuteQueryToDataSet("usp_NhomNguoiDung_AI_UserRight_GetQuyenGroupByMenu", prm);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
            return dt;
        }

        public NhomNguoiDung_AI_UserRightInfo GetRightByMenuAndGroupAdmin(int menuID, int groupAdminId)
        {
            NhomNguoiDung_AI_UserRightInfo item = null;
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@MenuID", menuID);
            prm[1] = new SqlParameter("@GroupAdminId", groupAdminId);

            try
            {
                var lst = this.ExecuteQuery("usp_NhomNguoiDung_AI_UserRight_GetRightByMenuIDAndGroupAdminID", prm);
                if (lst != null && lst.Count > 0)
                    item = lst[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
            return item;
        }
		
		#endregion
    }
}
