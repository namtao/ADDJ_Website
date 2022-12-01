using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GQKN.Archive.Entity;
using GQKN.Archive.Core.Provider;
using GQKN.Archive.Core;
//using System.Web.UI.HtmlControls;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_FileDinhKem
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class KhieuNai_FileDinhKemImpl : BaseImpl<KhieuNai_FileDinhKemInfo>
    {
        public KhieuNai_FileDinhKemImpl()
            : base()
        { }

        public KhieuNai_FileDinhKemImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_FileDinhKem";
        }

        #region Function

        public List<KhieuNai_FileDinhKemInfo> GetListByKhieuNaiId(int _khieuNaiId)
        {
            List<KhieuNai_FileDinhKemInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@KhieuNaiId",_khieuNaiId),
									};
            try
            {
                list = ExecuteQuery("usp_KhieuNai_FileDinhKem_GetListByKhieuNaiId", param);
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
