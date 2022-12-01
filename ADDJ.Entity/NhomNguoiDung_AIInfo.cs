using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NhomNguoiDung_AI in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	[Serializable]
	public class NhomNguoiDung_AIInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }

        public int CountNguoiSuDung { get; set; }
				
		public NhomNguoiDung_AIInfo()
		{
			Id = 0;
			Name = string.Empty;
			Description = string.Empty;
			Active = false;
			
		}
	}
}
