using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.News.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.News.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của News
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/07/2012</date>
	
	public class NewsImpl : BaseImpl<NewsInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "News";
        }
		
		#region Function 
		
		#endregion
    }
}
