using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LichSuTruyVan in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	[Serializable]
	public class LichSuTruyVanInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Data { get; set; }
		public string UserName { get; set; }
		
		public DateTime CDate { get; set; }
				
		public LichSuTruyVanInfo()
		{
			Id = 0;
			Name = string.Empty;
			Data = string.Empty;
			UserName = string.Empty;
			
		}
	}
}
