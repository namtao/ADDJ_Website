using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table ConfigDisplayColumn in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/02/2014</date>
	
	[Serializable]
	public class ConfigDisplayColumnInfo
	{
		public int Id { get; set; }
		public int TypeDisplay { get; set; }
        public string FormDisplay { get; set; }
		public int ConfigColumnId { get; set; }
		public string TenTruyCap { get; set; }
		
				
		public ConfigDisplayColumnInfo()
		{
			Id = 0;
			TypeDisplay = 0;
			FormDisplay = string.Empty;
			ConfigColumnId = 0;
			TenTruyCap = string.Empty;
			
		}
	}
}
