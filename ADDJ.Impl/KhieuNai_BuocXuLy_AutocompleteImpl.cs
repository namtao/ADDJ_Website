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
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_BuocXuLy_Autocomplete
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/07/2014</date>
	
	public class KhieuNai_BuocXuLy_AutocompleteImpl : BaseImpl<KhieuNai_BuocXuLy_AutocompleteInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_BuocXuLy_Autocomplete";
        }
		
		#region Function 
		
		#endregion
    }
}
