using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NhomNguoiDung_AI_Detail in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	[Serializable]
	public class NhomNguoiDung_AI_DetailInfo
	{
		public int Id { get; set; }
		public int NhomNguoiDung_AIId { get; set; }
		public int NguoiSuDungId { get; set; }

        public string TenTruyCap { get; set; }
				
		public NhomNguoiDung_AI_DetailInfo()
		{
			Id = 0;
			NhomNguoiDung_AIId = 0;
			NguoiSuDungId = 0;
			
		}
	}
}
