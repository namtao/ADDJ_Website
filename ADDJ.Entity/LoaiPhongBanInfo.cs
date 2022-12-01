using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LoaiPhongBan in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>19/08/2013</date>
	
	[Serializable]
	public class LoaiPhongBanInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

        public string ThoiGianXuLyMacDinh { get; set; }
        public byte LoaiDuLieu { get; set; }

        public int CountPhongBan { get; set; }
				
		public LoaiPhongBanInfo()
		{
			Id = 0;
			Name = string.Empty;
			Description = string.Empty;
            ThoiGianXuLyMacDinh = string.Empty;
            LoaiDuLieu = 0;
		}
	}
}
