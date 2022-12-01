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
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_Log
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class KhieuNai_LogImpl : BaseImpl<KhieuNai_LogInfo>
    {
        public KhieuNai_LogImpl()
            : base()
        { }

        public KhieuNai_LogImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_Log";
        }
		
		#region Function 

        
		
		#endregion
    }
}
