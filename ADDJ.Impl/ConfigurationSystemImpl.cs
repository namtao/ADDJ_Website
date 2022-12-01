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
    /// Class Insert, Update, Delete, Get dữ liệu của ConfigurationSystem
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	public class ConfigurationSystemImpl : BaseImpl<ConfigurationSystemInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ConfigurationSystem";
        }
		
		#region Function 
		
		#endregion
    }
}
