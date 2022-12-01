using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_BuocXuLy_Autocomplete in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/07/2014</date>
	
	[Serializable]
	public class KhieuNai_BuocXuLy_AutocompleteInfo
	{
		public int Id { get; set; }
		public int PhongBanId { get; set; }
		public string Name { get; set; }
		
		public string CUser { get; set; }
		public string LUser { get; set; }
		public DateTime CDate { get; set; }
		public DateTime LDate { get; set; }
				
		public KhieuNai_BuocXuLy_AutocompleteInfo()
		{
			Id = 0;
			PhongBanId = 0;
			Name = string.Empty;
			
		}
	}
}
