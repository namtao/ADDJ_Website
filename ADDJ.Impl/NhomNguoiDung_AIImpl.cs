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
    /// Class Insert, Update, Delete, Get dữ liệu của NhomNguoiDung_AI
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	public class NhomNguoiDung_AIImpl : BaseImpl<NhomNguoiDung_AIInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NhomNguoiDung_AI";
        }
		
		#region Function 
		
		#endregion
    }
}
