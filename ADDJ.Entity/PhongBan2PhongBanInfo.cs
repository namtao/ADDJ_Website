using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table PhongBan2PhongBan in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class PhongBan2PhongBanInfo
	{
		public int Id { get; set; }
		public int PhongBanId { get; set; }
		public string PhongBanDen { get; set; }
		
				
		public PhongBan2PhongBanInfo()
		{
			Id = 0;
			PhongBanId = 0;
			PhongBanDen = string.Empty;
			
		}
	}
}
