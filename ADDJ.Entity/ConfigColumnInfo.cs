using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table ConfigColumn in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/02/2014</date>
	
	[Serializable]
	public class ConfigColumnInfo
	{
		public int Id { get; set; }
		public int TypeDisplay { get; set; }
		public string ColumnName { get; set; }
		public string DisplayName { get; set; }
		
				
		public ConfigColumnInfo()
		{
			Id = 0;
			TypeDisplay = 0;
			ColumnName = string.Empty;
			DisplayName = string.Empty;
			
		}
	}
}
