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
    /// Class Insert, Update, Delete, Get dữ liệu của CongTyDoiTac
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>14/10/2013</date>
	
	public class CongTyDoiTacImpl : BaseImpl<CongTyDoiTacInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "CongTyDoiTac";
        }
		
		#region Function 
		
		#endregion
    }
}
