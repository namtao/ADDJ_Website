using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GQKN.Archive.Entity;
using GQKN.Archive.Core.Provider;
using GQKN.Archive.Core;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_Watchers
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNai_WatchersImpl : BaseImpl<KhieuNai_WatchersInfo>
    {
        public KhieuNai_WatchersImpl()
            : base()
        { }

        public KhieuNai_WatchersImpl(string connection)
            : base(connection)
        { }

        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_Watchers";
        }
		
		#region Function 
		
		public List<KhieuNai_WatchersInfo> GetListByKhieuNaiId(int _khieuNaiId)
		{
			List<KhieuNai_WatchersInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
			try
			{
				list = ExecuteQuery("usp_KhieuNai_Watchers_GetListByKhieuNaiId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		
		public List<KhieuNai_WatchersInfo> GetListByNguoiSuDungId(int _nguoiSuDungId)
		{
			List<KhieuNai_WatchersInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
									};
			try
			{
				list = ExecuteQuery("usp_KhieuNai_Watchers_GetListByNguoiSuDungId", param);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
			}
			return list;
			
		}

		public List<KhieuNai_WatchersInfo> GetListByAllRef(int _khieuNaiId, int _nguoiSuDungId)
		{
			List<KhieuNai_WatchersInfo> list = null;
			SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
										new SqlParameter("@NguoiSuDungId",_nguoiSuDungId),
									};
			try
			{
				list = ExecuteQuery("usp_KhieuNai_Watchers_GetListAllRef", param);
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
