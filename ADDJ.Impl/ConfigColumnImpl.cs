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
    /// Class Insert, Update, Delete, Get dữ liệu của ConfigColumn
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/02/2014</date>
	
	public class ConfigColumnImpl : BaseImpl<ConfigColumnInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ConfigColumn";
        }
		
		#region Function 
		
		#endregion
    }
}
