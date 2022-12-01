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
    /// Class Insert, Update, Delete, Get dữ liệu của ThongBao
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>25/02/2014</date>
	
	public class ThongBaoImpl : BaseImpl<ThongBaoInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ThongBao";
        }
		
		#region Function 
		
		#endregion
    }
}
