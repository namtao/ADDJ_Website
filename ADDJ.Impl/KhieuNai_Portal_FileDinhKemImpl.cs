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
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_Portal_FileDinhKem
    /// </summary>
    /// <author>Phi Hoang Hai</author>
    /// <date>13/06/2014</date>
	
	public class KhieuNai_Portal_FileDinhKemImpl : BaseImpl<KhieuNai_Portal_FileDinhKemInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_Portal_FileDinhKem";
        }
		
		#region Function 
		
		#endregion
    }
}
