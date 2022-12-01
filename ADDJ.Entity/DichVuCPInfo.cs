using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table DichVuCP in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/02/2014</date>
	
	[Serializable]
	public class DichVuCPInfo
	{
		public int Id { get; set; }
        public int LinhVucChungId { get; set; }
        public string LinhVucChung { get; set; }
		public string MaDichVu { get; set; }
		public DateTime NgayBatDau { get; set; }
		public DateTime NgayKetThuc { get; set; }
		public int Deactive { get; set; }
		public string GhiChu { get; set; }
        public bool TrangThai { get; set; }
		public string LUser { get; set; }
		public DateTime LDate { get; set; }
				
		public DichVuCPInfo()
		{
			Id = 0;
            LinhVucChungId = 0;
            LinhVucChung = string.Empty;
			MaDichVu = string.Empty;
			NgayBatDau = DateTime.Now;
			NgayKetThuc = DateTime.Now;
			Deactive = 0;
			GhiChu = string.Empty;
            TrangThai = false;
		}
	}
}
