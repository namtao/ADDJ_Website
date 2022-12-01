using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_Log in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class KhieuNai_LogInfo
	{
		public Int64 Id { get; set; }
		public int KhieuNaiId { get; set; }
		public string TruongThayDoi { get; set; }
		public string GiaTriCu { get; set; }
		public string GiaTriMoi { get; set; }
		public string ThaoTac { get; set; }
        public int PhongBanId { get; set; }
		public string CUser { get; set; }
		public DateTime CDate { get; set; }
				
		public KhieuNai_LogInfo()
		{
			Id = 0;
			KhieuNaiId = 0;
			TruongThayDoi = string.Empty;
			GiaTriCu = string.Empty;
			GiaTriMoi = string.Empty;
			ThaoTac = string.Empty;
            PhongBanId = 0;
            CUser = string.Empty;			
		}
	}
}
