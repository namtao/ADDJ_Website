using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table ConfigurationSystem in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	[Serializable]
	public class ConfigurationSystemInfo
	{
		public int Id { get; set; }
		public string Variable { get; set; }
		
				
		public ConfigurationSystemInfo()
		{
			Id = 0;
			Variable = string.Empty;
			
		}
	}
}
