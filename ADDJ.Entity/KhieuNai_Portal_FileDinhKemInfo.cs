using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_Portal_FileDinhKem in Databasse
    /// </summary>
    /// <author>Phi Hoang Hai</author>
    /// <date>13/06/2014</date>
	
	[Serializable]
	public class KhieuNai_Portal_FileDinhKemInfo
	{
		public int Id { get; set; }
		public int KhieuNai_Portal_Id { get; set; }
		public string TenFile { get; set; }
		public decimal KichThuoc { get; set; }
		public string LoaiFile { get; set; }
		public string GhiChu { get; set; }
		public string URLFile { get; set; }
		public Int16 Status { get; set; }
		
		public string CUser { get; set; }
		public DateTime CDate { get; set; }

        public byte [] BinaryFile {get;set;}
				
		public KhieuNai_Portal_FileDinhKemInfo()
		{
			Id = 0;
			KhieuNai_Portal_Id = 0;
			TenFile = string.Empty;
			KichThuoc = 0;
			LoaiFile = string.Empty;
			GhiChu = string.Empty;
			URLFile = string.Empty;
			Status = 0;            
			
		}
	}
}
