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
    /// Class Insert, Update, Delete, Get dữ liệu của LoaiPhongBan
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>19/08/2013</date>
	
	public class LoaiPhongBanImpl : BaseImpl<LoaiPhongBanInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiPhongBan";
        }
		
		#region Function 
		
		#endregion
    }
}
