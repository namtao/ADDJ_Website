using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table UserAcc in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>07/12/2012</date>
	
	[Serializable]
	public class KhieuNaiChiTietInfo
	{
        public string Id { get; set; }
        public string MaKhieuNai { get; set; }
        public string KhuVucId { get; set; }
        public string DoiTacId { get; set; }
        public string PhongBanTiepNhanId { get; set; }
        public string PhongBanXuLyId { get; set; }
        public string LoaiKhieuNaiId { get; set; }
        public string LinhVucChungId { get; set; }
        public string LinhVucConId { get; set; }
        public string LoaiKhieuNai { get; set; }
        public string LinhVucChung { get; set; }
        public string LinhVucCon { get; set; }
        public string DoUuTien { get; set; }
        public string SoThueBao { get; set; }
        public string FileDinhKemKH { get; set; }
        public string FileDinhKemGQKN { get; set; }
        public string MaTinh { get; set; }
        public string HoTenLienHe { get; set; }
        public string DiaChiLienHe { get; set; }
        public string SDTLienHe { get; set; }
        public string DiaDiemXayRa { get; set; }
        public string ThoiGianXayRa { get; set; }
        public string NoiDungPA { get; set; }
        public string NoiDungCanHoTro { get; set; }
        public string TrangThai { get; set; }
        public string IsChuyenBoPhan { get; set; }
        public string NguoiTiepNhan { get; set; }
        public string HTTiepNhan { get; set; }
        public string NgayTiepNhan { get; set; }
        public string NguoiTienXuLyCap1 { get; set; }
        public string NguoiTienXuLyCap2 { get; set; }
        public string NguoiTienXuLyCap3 { get; set; }
        public string NguoiXuLy { get; set; }
        public string NgayQuaHan { get; set; }
        public string NgayCanhBao { get; set; }
        public string NgayChuyenPhongBan { get; set; }
        public string NgayQuaHanPhongBanXuLy { get; set; }
        public string NgayTraLoiKN { get; set; }
        public string NgayDongKN { get; set; }
        public string NgayDongKNSort { get; set; }
        public string KQXuLy_SHCV { get; set; }
        public string KQXuLy_CCT { get; set; }
        public string KQXuLy_CSL { get; set; }
        public string KQXuLy_PTSL_IR { get; set; }
        public string KQXuLy_PTSL_Khac { get; set; }
        public string KetQuaXuLy { get; set; }
        public string NoiDungXuLy { get; set; }
        public string NoiDungXuLyDongKN { get; set; }
        public string GhiChu { get; set; }
        public string KNHangLoat { get; set; }
        public string SoTienKhauTru_TKC { get; set; }
        public string SoTienKhauTru_KM1 { get; set; }
        public string SoTienKhauTru_KM2 { get; set; }
        public string SoTienKhauTru_KM3 { get; set; }
        public string SoTienKhauTru_KM4 { get; set; }
        public string SoTienKhauTru_KM5 { get; set; }
        public string IsLuuKhieuNai { get; set; }

        public int CallCount { get; set; }
        public string CDate { get; set; }
        public string CUser { get; set; }
        public string LDate { get; set; }
        public string LUser { get; set; }

        public string XuLyKN { get; set; }

        public string TenLyDoGiamTru { get; set; }
        public string TenChiTietLoi { get; set; }

        public string TinhThanhXayRaSuCo { get; set; }
        public string QuanHuyenXayRaSuCo { get; set; }
        public string PhuongXaXayRaSuCo { get; set; }

        public string TenDoHaiLong { get; set; }

        public KhieuNaiChiTietInfo()
        {
            Id = string.Empty;
            MaKhieuNai = string.Empty;
            KhuVucId = string.Empty;
            DoiTacId = string.Empty;
            PhongBanTiepNhanId = string.Empty;
            PhongBanXuLyId = string.Empty;
            LoaiKhieuNaiId = string.Empty;
            LinhVucChungId = string.Empty;
            LinhVucConId = string.Empty;
            LoaiKhieuNai = string.Empty;
            LinhVucChung = string.Empty;
            LinhVucCon = string.Empty;
            DoUuTien = string.Empty;
            SoThueBao = string.Empty;
            FileDinhKemKH = string.Empty;
            FileDinhKemGQKN = string.Empty;
            MaTinh = string.Empty;
            HoTenLienHe = string.Empty;
            DiaChiLienHe = string.Empty;
            SDTLienHe = string.Empty;
            DiaDiemXayRa = string.Empty;
            ThoiGianXayRa = string.Empty;
            NoiDungPA = string.Empty;

            NoiDungCanHoTro = string.Empty;

            TrangThai = string.Empty;


            IsChuyenBoPhan = string.Empty;
            NguoiTiepNhan = string.Empty;
            HTTiepNhan = string.Empty;
            NgayTiepNhan = string.Empty;


            NguoiTienXuLyCap1 = string.Empty;
            NguoiTienXuLyCap2 = string.Empty;
            NguoiTienXuLyCap3 = string.Empty;
            NguoiXuLy = string.Empty;
            NgayQuaHan = string.Empty;

            NgayCanhBao = string.Empty;

            NgayChuyenPhongBan = string.Empty;

            NgayQuaHanPhongBanXuLy = string.Empty;
            NgayTraLoiKN = string.Empty;

            NgayDongKN = string.Empty;
            NgayDongKNSort = string.Empty;

            KQXuLy_SHCV = string.Empty;
            KQXuLy_CCT = string.Empty;
            KQXuLy_CSL = string.Empty;
            KQXuLy_PTSL_IR = string.Empty;
            KQXuLy_PTSL_Khac = string.Empty;
            KetQuaXuLy = string.Empty;
            NoiDungXuLy = string.Empty;
            GhiChu = string.Empty;
            KNHangLoat = string.Empty;
            SoTienKhauTru_TKC = string.Empty;
            SoTienKhauTru_KM1 = string.Empty;
            SoTienKhauTru_KM2 = string.Empty;
            SoTienKhauTru_KM3 = string.Empty;
            SoTienKhauTru_KM4 = string.Empty;
            SoTienKhauTru_KM5 = string.Empty;
            IsLuuKhieuNai = string.Empty;
            CallCount = 1;
            CDate = string.Empty;
            CUser = string.Empty;
            LDate = string.Empty;
            LUser = string.Empty;

            XuLyKN = string.Empty;

            TenLyDoGiamTru = string.Empty;
            TenChiTietLoi = string.Empty;

            TinhThanhXayRaSuCo = string.Empty;
            QuanHuyenXayRaSuCo = string.Empty;
            PhuongXaXayRaSuCo = string.Empty;

            TenDoHaiLong = string.Empty;
        }
	}
}
