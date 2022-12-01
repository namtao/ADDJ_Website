using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_GiaiPhap in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class KhieuNai_GiaiPhapInfo
	{
		public Int64 Id { get; set; }
		public int KhieuNaiId { get; set; }
		public string Name { get; set; }
		public string FAQ { get; set; }
		public string MoTa { get; set; }
		public string Comments { get; set; }
		
		public string CUser { get; set; }
		public DateTime CDate { get; set; }
				
		public KhieuNai_GiaiPhapInfo()
		{
			Id = 0;
			KhieuNaiId = 0;
			Name = string.Empty;
			FAQ = string.Empty;
			MoTa = string.Empty;
			Comments = string.Empty;
			
		}
	}
}
