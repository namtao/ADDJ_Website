using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_KetQuaXuLy in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/08/2013</date>
	
	[Serializable]
	public class KhieuNai_KetQuaXuLyInfo
	{
		public Int64 Id { get; set; }
		public int KhieuNaiId { get; set; }
        public int PhongBanXuLyId { get; set; }
        public string PhongBanXuLyName { get; set; }
		public bool IsCSL { get; set; }
		public bool IsCCT { get; set; }
		public string SHCV { get; set; }
		public bool PTSoLieu_IR { get; set; }
		public string PTSoLieu_Khac { get; set; }
		public string KetQuaXuLy { get; set; }
		public string NoiDungXuLy { get; set; }
        public int LyDoGiamTru { get; set; }
		public DateTime CDate { get; set; }
		public string CUser { get; set; }
		public DateTime LDate { get; set; }
		public string LUser { get; set; }
				
		public KhieuNai_KetQuaXuLyInfo()
		{
			Id = 0;
			KhieuNaiId = 0;
            PhongBanXuLyId = 0;
            PhongBanXuLyName = string.Empty;
			IsCSL = false;
			IsCCT = false;
			SHCV = string.Empty;
			PTSoLieu_IR = false;
			PTSoLieu_Khac = string.Empty;
			KetQuaXuLy = string.Empty;
			NoiDungXuLy = string.Empty;
		    LyDoGiamTru = 0;
		}
	}
}
