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
    /// Class Insert, Update, Delete, Get dữ liệu của DichVuCP
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/02/2014</date>
	
	public class DichVuCPImpl : BaseImpl<DichVuCPInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "DichVuCP";
        }
		
		#region Function 
		
		#endregion
    }
}
