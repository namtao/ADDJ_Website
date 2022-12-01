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
    /// Class Insert, Update, Delete, Get dữ liệu của ConfigDisplayColumn
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/02/2014</date>
	
	public class ConfigDisplayColumnImpl : BaseImpl<ConfigDisplayColumnInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ConfigDisplayColumn";
        }
		
		#region Function 
		
		#endregion
    }
}
