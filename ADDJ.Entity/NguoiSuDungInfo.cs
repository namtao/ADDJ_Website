using System;
using ADDJ.Core.AILucene;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NguoiSuDung in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class NguoiSuDungInfo
	{
        [AIFieldUnikey("Id")]
		public int Id { get; set; }

        [AIField("TenDoiTac")]
		public string TenDoiTac { get; set; }

        [AIField("DoiTacId")]
		public int DoiTacId { get; set; }

        [AIField("KhuVucId")]
        public int KhuVucId { get; set; }

        [AIField("NhomNguoiDung")]
        public int NhomNguoiDung { get; set; }

        [AIField("TenTruyCap")]
        public string TenTruyCap { get; set; }

        public string MatKhau { get; set; }

        [AIField("TenDayDu")]
        public string TenDayDu { get; set; }

		public DateTime NgaySinh { get; set; }
		public string DiaChi { get; set; }
		public string DiDong { get; set; }
		public string CoDinh { get; set; }
		public byte Sex { get; set; }
		public string Email { get; set; }
		public string CongTy { get; set; }
		public string DiaChiCongTy { get; set; }
		public string FaxCongTy { get; set; }
		public string DienThoaiCongTy { get; set; }
		public byte TrangThai { get; set; }
		public bool SuDungLDAP { get; set; }
		public int LoginCount { get; set; }
		public DateTime LastLogin { get; set; }
		public bool IsLogin { get; set; }
		
		public DateTime CDate { get; set; }
		public DateTime LDate { get; set; }
		public string CUser { get; set; }
		public string LUser { get; set; }

        public string PhongBan_Name { get; set; }
				
		public NguoiSuDungInfo()
		{
			Id = 0;
			TenDoiTac = string.Empty;
			DoiTacId = 0;
			KhuVucId = 0;
			NhomNguoiDung = 0;
			TenTruyCap = string.Empty;
			MatKhau = string.Empty;
			TenDayDu = string.Empty;
			NgaySinh = DateTime.Now;
			DiaChi = string.Empty;
			DiDong = string.Empty;
			CoDinh = string.Empty;
			Sex = 0;
			Email = string.Empty;
			CongTy = string.Empty;
			DiaChiCongTy = string.Empty;
			FaxCongTy = string.Empty;
			DienThoaiCongTy = string.Empty;
			TrangThai = 0;
			SuDungLDAP = false;
			LoginCount = 0;
			LastLogin = DateTime.Now;
			IsLogin = false;
			
		}
	}
}
