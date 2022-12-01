using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LoaiKhieuNai2PhongBan in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class LoaiKhieuNai2PhongBanInfo
	{
		public int Id { get; set; }
		public int LoaiKhieuNaiId { get; set; }
		public int PhongBanId { get; set; }

        public string PhongBanName { get; set; }
				
		public LoaiKhieuNai2PhongBanInfo()
		{
			Id = 0;
			LoaiKhieuNaiId = 0;
			PhongBanId = 0;
			
		}
	}
}
