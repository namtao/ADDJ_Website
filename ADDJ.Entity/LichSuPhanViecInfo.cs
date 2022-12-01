using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LichSuPhanViec in Databasse
    /// </summary>
    /// <author>Lê Anh Dũng</author>
    /// <date>04/11/2013</date>
	
	[Serializable]
	public class LichSuPhanViecInfo
	{
		public int Id { get; set; }
		public int KhieuNaiId { get; set; }
		public string NguoiDuocPhanViec { get; set; }
		public int PhongBanXuLyId { get; set; }
		
		public DateTime CDate { get; set; }
		public DateTime LDate { get; set; }
		public string CUser { get; set; }
		public string LUser { get; set; }
				
		public LichSuPhanViecInfo()
		{
			Id = 0;
			KhieuNaiId = 0;
			NguoiDuocPhanViec = string.Empty;
			PhongBanXuLyId = 0;
			
		}
	}
}
