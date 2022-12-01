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
    /// Class Insert, Update, Delete, Get dữ liệu của NhomNguoiDung_AI_Detail
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	public class NhomNguoiDung_AI_DetailImpl : BaseImpl<NhomNguoiDung_AI_DetailInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NhomNguoiDung_AI_Detail";
        }
		
		#region Function 
		
		#endregion
    }
}
