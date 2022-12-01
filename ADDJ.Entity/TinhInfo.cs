using ADDJ.Core.AILucene;
using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table Tinh in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class TinhInfo
	{
        [AIFieldUnikey("Id")]
		public string MaTinh { get; set; }
        [AIField("TenDoiTac")]
		public string TenTinh { get; set; }
		
				
		public TinhInfo()
		{
			MaTinh = string.Empty;
			TenTinh = string.Empty;
			
		}
	}
}
