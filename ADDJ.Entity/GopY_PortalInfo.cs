using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table GopY_Portal in Databasse
    /// </summary>
    /// <author>Phi Hoang Hai</author>
    /// <date>13/06/2014</date>
	
	[Serializable]
	public class GopY_PortalInfo
	{
		public int Id { get; set; }
		public string TieuDe { get; set; }
		public string NoiDung { get; set; }
		public string URLFile { get; set; }
		public DateTime NgayTaoGopY { get; set; }
		public string HoTenNguoiTraLoi { get; set; }
		public string NguoiTraLoi { get; set; }
		public string NoiDungTraLoi { get; set; }
		public int CUserId { get; set; }
		public int LUserId { get; set; }
		
		public string CUser { get; set; }
		public DateTime CDate { get; set; }
		public string LUser { get; set; }
		public DateTime LDate { get; set; }

        public byte[] BinaryFile { get; set; }
				
		public GopY_PortalInfo()
		{
			Id = 0;
			TieuDe = string.Empty;
			NoiDung = string.Empty;
			URLFile = string.Empty;
			NgayTaoGopY = DateTime.Now;
			HoTenNguoiTraLoi = string.Empty;
			NguoiTraLoi = string.Empty;
			NoiDungTraLoi = string.Empty;
			CUserId = 0;
			LUserId = 0;
			
		}
	}
}
