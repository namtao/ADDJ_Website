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
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_GiaiPhap
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class KhieuNai_GiaiPhapImpl : BaseImpl<KhieuNai_GiaiPhapInfo>
    {
        public KhieuNai_GiaiPhapImpl()
            : base()
        { }

        public KhieuNai_GiaiPhapImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_GiaiPhap";
        }
		
		#region Function 
		
		public List<KhieuNai_GiaiPhapInfo> GetListByKhieuNaiId(int _khieuNaiId)
		{
			List<KhieuNai_GiaiPhapInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
			try
			{
				list = ExecuteQuery("usp_KhieuNai_GiaiPhap_GetListByKhieuNaiId", param);
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
