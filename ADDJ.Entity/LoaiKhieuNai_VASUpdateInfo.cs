using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LoaiKhieuNai_VASUpdate in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>08/05/2014</date>
	
	[Serializable]
	public class LoaiKhieuNai_VASUpdateInfo
	{
        public int Id { get; set; }
        public int ParentLoaiKhieuNaiId { get; set; }
        public int ParentId { get; set; }
        public string MaDichVu { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public byte Status { get; set; }
        public byte Cap { get; set; }
        public bool IsUpdate { get; set; }

        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }
        public string CUser { get; set; }
        public string LUser { get; set; }
        public bool IsDeleted { get; set; }

        public string NgayHetHan { get; set; }

        public LoaiKhieuNai_VASUpdateInfo()
        {
            Id = 0;
            ParentLoaiKhieuNaiId = 0;
            ParentId = 0;
            MaDichVu = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Sort = 0;
            Status = 0;
            Cap = 0;
            IsUpdate = false;
            NgayHetHan = string.Empty;

        }


	}
}
