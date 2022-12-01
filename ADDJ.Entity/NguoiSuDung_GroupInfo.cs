using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NguoiSuDung_Group in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class NguoiSuDung_GroupInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Int16 Status { get; set; }
		
				
		public NguoiSuDung_GroupInfo()
		{
			Id = 0;
			Name = string.Empty;
			Description = string.Empty;
			Status = 0;
			
		}
	}
}
