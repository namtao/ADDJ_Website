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
    /// Class Insert, Update, Delete, Get dữ liệu của PermissionSchemes
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class PermissionSchemesImpl : BaseImpl<PermissionSchemesInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PermissionSchemes";
        }
		
		#region Function 
		
		#endregion
    }
}
