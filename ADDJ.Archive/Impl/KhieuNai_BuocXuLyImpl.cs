using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GQKN.Archive.Entity;
using GQKN.Archive.Core.Provider;
using GQKN.Archive.Core;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_BuocXuLy
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNai_BuocXuLyImpl : BaseImpl<KhieuNai_BuocXuLyInfo>
    {
        public KhieuNai_BuocXuLyImpl() : base()
        {}

        public KhieuNai_BuocXuLyImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_BuocXuLy";
        }
		
		#region Function 
		
		public List<KhieuNai_BuocXuLyInfo> GetListByKhieuNaiId(int _khieuNaiId)
		{
			List<KhieuNai_BuocXuLyInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
			try
			{
				list = ExecuteQuery("usp_KhieuNai_BuocXuLy_GetListByKhieuNaiId", param);
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
