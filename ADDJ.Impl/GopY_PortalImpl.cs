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
    /// Class Insert, Update, Delete, Get dữ liệu của GopY_Portal
    /// </summary>
    /// <author>Phi Hoang Hai</author>
    /// <date>13/06/2014</date>
	
	public class GopY_PortalImpl : BaseImpl<GopY_PortalInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "GopY_Portal";
        }
		
		#region Function 
		
		#endregion
    }
}
