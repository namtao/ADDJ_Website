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
    /// Class Insert, Update, Delete, Get dữ liệu của ConfigurationTime
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>22/08/2013</date>
	
	public class ConfigurationTimeImpl : BaseImpl<ConfigurationTimeInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "ConfigurationTime";
        }

        private static List<ConfigurationTimeInfo> _ListConfigTime;
        public static List<ConfigurationTimeInfo> ConfigTime
        {
            get
            {
                if (_ListConfigTime == null)
                    _ListConfigTime = new ConfigurationTimeImpl().GetList();
                return _ListConfigTime;
            }
            set { _ListConfigTime = value; }
        }

		
		#region Function 
		
		#endregion
    }
}
