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
    /// Class Insert, Update, Delete, Get dữ liệu của NguoiSuDung_Group
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class NguoiSuDung_GroupImpl : BaseImpl<NguoiSuDung_GroupInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung_Group";
        }

        private static List<NguoiSuDung_GroupInfo> lstNhomNguoiDung;

        public static List<NguoiSuDung_GroupInfo> NhomNguoiDung
        {
            get{
                if (lstNhomNguoiDung != null)
                    return lstNhomNguoiDung;

                lstNhomNguoiDung = new NguoiSuDung_GroupImpl().GetList();
                return lstNhomNguoiDung;
            }
            set {
                lstNhomNguoiDung = value;
            }
        }
		
		#region Function 
		
		#endregion
    }
}
