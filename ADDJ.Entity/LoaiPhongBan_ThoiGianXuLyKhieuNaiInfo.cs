using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LoaiPhongBan_ThoiGianXuLyKhieuNai in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>19/08/2013</date>
	
	[Serializable]
	public class LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo
	{
		public int Id { get; set; }
		public int LoaiPhongBanId { get; set; }
		public int LoaiKhieuNaiId { get; set; }
		public string ThoiGianCanhBao { get; set; }
		public string ThoiGianUocTinh { get; set; }

        public int LoaiKhieuNai_ParentId { get; set; }
        public string LoaiKhieuNai_Name { get; set; }
        public string LoaiKhieuNai_ThoiGianUocTinh { get; set; }
        public int Cap { get; set; }
				
		public LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo()
		{
			Id = 0;
			LoaiPhongBanId = 0;
			LoaiKhieuNaiId = 0;
			ThoiGianCanhBao = string.Empty;
			ThoiGianUocTinh = string.Empty;
			
		}
	}
}
