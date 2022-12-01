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
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNaiRef_VNPT
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>06/05/2014</date>
	
	public class KhieuNaiRef_VNPTImpl : BaseImpl<KhieuNaiRef_VNPTInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNaiRef_VNPT";
        }
		
		#region Function 
        public KhieuNaiRef_VNPTInfo GetInfoByKhieuNaiIdVNP(int id)
        {
            var lst = GetListDynamic("", "KhieuNaiIdVNP=" + id, "");
            if (lst != null && lst.Count > 0)
                return lst[0];
            return null;
        }

        public KhieuNaiRef_VNPTInfo GetInfoByKhieunaiIdVnptMain(int idMain)
        {
            var lst = GetListDynamic("", "KhieuNaiIdVNPTMain='" + idMain + "'", "");
            if (lst != null && lst.Count > 0)
                return lst[0];
            return null;
        }

        public KhieuNaiRef_VNPTInfo GetInfoByKhieunaiIdVnptPhu(int idPhu)
        {
            var lst = GetListDynamic("", "KhieuNaiIdVNPT='" + idPhu + "'", "");
            if (lst != null && lst.Count > 0)
                return lst[0];
            return null;
        }
        public int GetIdVNPByIdVNPT(string IdVnpt)
        {
            int result = 0;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiIdVNPT",IdVnpt)                                       
									};
            try
            {
                var obj = ExecuteScalar("usp_KhieuNaiRef_VNPT_GetByIdVNPT", param);
                if (obj != null)
                    return ConvertUtility.ToInt32(obj, 0);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return result;

        }

        public string GetIdVNPTByIdVNP(int IdVnp)
        {
            var result = string.Empty;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiIdVNP",IdVnp)                                       
									};
            try
            {
                var obj = ExecuteScalar("usp_KhieuNaiRef_VNPT_GetByIdVNP", param);
                if (obj != null)
                    return ConvertUtility.ToString(obj);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return string.Empty;
        }

        public string GetIdVNPTMainByIdVNP(int IdVnp)
        {
            var result = string.Empty;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiIdVNP",IdVnp)                                       
									};
            try
            {
                var obj = ExecuteScalar("usp_KhieuNaiRef_VNPT_GetIdMainByIdVNP", param);
                if (obj != null)
                    return ConvertUtility.ToString(obj);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return string.Empty;
        }


        
		#endregion
    }
}
