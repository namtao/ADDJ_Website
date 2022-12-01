using System;
using System.Collections.Generic;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_Portal in Databasse
    /// </summary>
    /// <author>Phi Hoang Hai</author>
    /// <date>13/06/2014</date>
	
	[Serializable]
	public class KhieuNai_PortalInfo
	{
		public int Id { get; set; }
		public DateTime NgayTiepNhan { get; set; }
		public string HoTenNguoiKhieuNai { get; set; }
		public string SoCMNDPasspostGPKD { get; set; }
		public DateTime NgayCapCMNDPassportGPKD { get; set; }
		public string NoiCapCMNDPassportGPKD { get; set; }
		public string HoTenChuThueBao { get; set; }
		public Int64 SoThueBao { get; set; }
		public bool IsTraSau { get; set; }
		public string DiaChiLienHe { get; set; }
		public int Quan_DiaChiLienHe { get; set; }
		public int Tinh_DiaChiLienHe { get; set; }
		public string NoiDungPA { get; set; }
		public string GiaiThichCuaNhanVienTiepNhan { get; set; }
		public Int16 HinhThucTraLoiKhieuNai { get; set; }
		public string DiaChiQuayGiaoDichVNPT { get; set; }
		public Int64 DienThoaiNhanTraLoi { get; set; }
		public string EmailNhanTraLoi { get; set; }
		public string DiaChiNhanTraLoi { get; set; }
		public int KhieuNaiId { get; set; }
		public string NoiDungXuLy { get; set; }
		public int NguoiXuLyId { get; set; }
		public string NguoiXuLy { get; set; }
		public int PhongBanXuLyId { get; set; }
		public int DoiTacXuLyId { get; set; }
		public Int16 TrangThaiKhieuNai { get; set; }
		public int CUserId { get; set; }
		public int LUserId { get; set; }
		
		public string CUser { get; set; }
		public DateTime CDate { get; set; }
		public string LUser { get; set; }
		public DateTime LDate { get; set; }

        public List<KhieuNai_Portal_FileDinhKemInfo> ListKhieuNaiPortalFileDinhKem { get; set; }
				
		public KhieuNai_PortalInfo()
		{
			Id = 0;
			NgayTiepNhan = DateTime.Now;
			HoTenNguoiKhieuNai = string.Empty;
			SoCMNDPasspostGPKD = string.Empty;
			NgayCapCMNDPassportGPKD = DateTime.Now;
			NoiCapCMNDPassportGPKD = string.Empty;
			HoTenChuThueBao = string.Empty;
			SoThueBao = 0;
			IsTraSau = false;
			DiaChiLienHe = string.Empty;
			Quan_DiaChiLienHe = 0;
			Tinh_DiaChiLienHe = 0;
			NoiDungPA = string.Empty;
			GiaiThichCuaNhanVienTiepNhan = string.Empty;
			HinhThucTraLoiKhieuNai = 0;
			DiaChiQuayGiaoDichVNPT = string.Empty;
			DienThoaiNhanTraLoi = 0;
			EmailNhanTraLoi = string.Empty;
			DiaChiNhanTraLoi = string.Empty;
			KhieuNaiId = 0;
			NoiDungXuLy = string.Empty;
			NguoiXuLyId = 0;
			NguoiXuLy = string.Empty;
			PhongBanXuLyId = 0;
			DoiTacXuLyId = 0;
			TrangThaiKhieuNai = 0;
			CUserId = 0;
			LUserId = 0;

            ListKhieuNaiPortalFileDinhKem = new List<KhieuNai_Portal_FileDinhKemInfo>();
		}
	}
}
