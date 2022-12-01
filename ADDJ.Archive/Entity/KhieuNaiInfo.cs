using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>27/09/2013</date>

    [Serializable]
    public class KhieuNaiInfo
    {
        public int Id { get; set; }
        public string MaKhieuNai { get; set; }
        public int KhuVucId { get; set; }
        public int DoiTacId { get; set; }
        public int PhongBanTiepNhanId { get; set; }
        public int KhuVucXuLyId { get; set; }
        public int DoiTacXuLyId { get; set; }
        public int PhongBanXuLyId { get; set; }
        public int LoaiKhieuNaiId { get; set; }
        public int LinhVucChungId { get; set; }
        public int LinhVucConId { get; set; }
        public string LoaiKhieuNai { get; set; }
        public string LinhVucChung { get; set; }
        public string LinhVucCon { get; set; }
        public Int16 DoUuTien { get; set; }
        public bool IsKNGiamTru { get; set; }
        public Int64 SoThueBao { get; set; }
        public bool IsTraSau { get; set; }
        public int MaTinhId { get; set; }
        public string MaTinh { get; set; }
        public int MaQuanId { get; set; }
        public string MaQuan { get; set; }

        public int MaPhuongId { get; set; }
        public string MaPhuong { get; set; }
        public string HoTenLienHe { get; set; }
        public string DiaChi_CCBS { get; set; }
        public string DiaChiLienHe { get; set; }
        public string SDTLienHe { get; set; }
        public string DiaDiemXayRa { get; set; }
        public string ThoiGianXayRa { get; set; }
        public string NoiDungPA { get; set; }
        public string NoiDungCanHoTro { get; set; }
        public Int16 TrangThai { get; set; }
        public bool IsChuyenBoPhan { get; set; }
        public int NguoiTiepNhanId { get; set; }
        public string NguoiTiepNhan { get; set; }
        public Int16 HTTiepNhan { get; set; }
        public DateTime NgayTiepNhan { get; set; }
        public int NgayTiepNhanSort { get; set; }
        public int NguoiTienXuLyCap1Id { get; set; }
        public string NguoiTienXuLyCap1 { get; set; }
        public int NguoiTienXuLyCap2Id { get; set; }
        public string NguoiTienXuLyCap2 { get; set; }
        public int NguoiTienXuLyCap3Id { get; set; }
        public string NguoiTienXuLyCap3 { get; set; }
        public int NguoiXuLyId { get; set; }
        public string NguoiXuLy { get; set; }
        public DateTime NgayQuaHan { get; set; }
        public int NgayQuaHanSort { get; set; }
        public DateTime NgayCanhBao { get; set; }
        public int NgayCanhBaoSort { get; set; }
        public DateTime NgayChuyenPhongBan { get; set; }
        public int NgayChuyenPhongBanSort { get; set; }
        public DateTime NgayCanhBaoPhongBanXuLy { get; set; }
        public int NgayCanhBaoPhongBanXuLySort { get; set; }
        public DateTime NgayQuaHanPhongBanXuLy { get; set; }
        public int NgayQuaHanPhongBanXuLySort { get; set; }
        public DateTime NgayTraLoiKN { get; set; }
        public int NgayTraLoiKNSort { get; set; }
        public DateTime NgayDongKN { get; set; }
        public int NgayDongKNSort { get; set; }
        public string NoiDungXuLyDongKN { get; set; }
        public string KQXuLy_SHCV { get; set; }
        public bool KQXuLy_CCT { get; set; }
        public bool KQXuLy_CSL { get; set; }
        public bool KQXuLy_PTSL_IR { get; set; }
        public string KQXuLy_PTSL_Khac { get; set; }
        public string KetQuaXuLy { get; set; }
        public string NoiDungXuLy { get; set; }
        public string GhiChu { get; set; }
        public bool KNHangLoat { get; set; }
        public decimal SoTienKhauTru_TKC { get; set; }
        public decimal SoTienKhauTru_KM { get; set; }
        public decimal SoTienKhauTru_KM1 { get; set; }
        public decimal SoTienKhauTru_KM2 { get; set; }
        public decimal SoTienKhauTru_Data { get; set; }
        public decimal SoTienKhauTru_Khac { get; set; }
        public decimal SoTienKhauTru_TS_GPRS { get; set; }
        public decimal SoTienKhauTru_TS_CP { get; set; }
        public decimal SoTienKhauTru_TS_Thoai { get; set; }
        public decimal SoTienKhauTru_TS_SMS { get; set; }
        public decimal SoTienKhauTru_TS_IR { get; set; }
        public decimal SoTienKhauTru_TS_Khac { get; set; }
        public bool IsLuuKhieuNai { get; set; }
        public bool IsPhanViec { get; set; }
        public bool IsPhanHoi { get; set; }

        public int KhieuNaiFrom { get; set; }
        public int LyDoGiamTru { get; set; }
        public int ArchiveId { get; set; }
        public int CallCount { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string Other3 { get; set; }
        public string Other4 { get; set; }
        public string Other5 { get; set; }
        public int DoHaiLong { get; set; }
        public DateTime CDate { get; set; }
        public string CUser { get; set; }
        public DateTime LDate { get; set; }
        public string LUser { get; set; }

        public Int64 STT { get; set; }
        public string PhongBan_Name { get; set; }
        public string NguoiXuLyTruoc { get; set; }
        public string NguoiDuocPhanHoi { get; set; }

        public int ChiTietLoiId { get; set; }

        // TRUONGVV: Xử lý số tiền giảm trừ
        public decimal SoTien { get; set; }
        public decimal SoTien_Edit { get; set; }

        public KhieuNaiInfo()
        {
            Id = 0;
            MaKhieuNai = string.Empty;
            KhuVucId = 0;
            DoiTacId = 0;
            PhongBanTiepNhanId = 0;
            KhuVucXuLyId = 0;
            DoiTacXuLyId = 0;
            PhongBanXuLyId = 0;
            LoaiKhieuNaiId = 0;
            LinhVucChungId = 0;
            LinhVucConId = 0;
            LoaiKhieuNai = string.Empty;
            LinhVucChung = string.Empty;
            LinhVucCon = string.Empty;
            DoUuTien = 0;
            IsKNGiamTru = false;
            SoThueBao = 0;
            IsTraSau = false;
            MaTinhId = 0;
            MaTinh = string.Empty;
            MaQuanId = 0;
            MaQuan = string.Empty;
            MaPhuongId = 0;
            MaPhuong = string.Empty;
            HoTenLienHe = string.Empty;
            DiaChi_CCBS = string.Empty;
            DiaChiLienHe = string.Empty;
            SDTLienHe = string.Empty;
            DiaDiemXayRa = string.Empty;
            ThoiGianXayRa = string.Empty;
            NoiDungPA = string.Empty;
            NoiDungCanHoTro = string.Empty;
            TrangThai = 0;
            IsChuyenBoPhan = false;
            NguoiTiepNhanId = 0;
            NguoiTiepNhan = string.Empty;
            HTTiepNhan = 0;
            NgayTiepNhan = DateTime.Now;
            NgayTiepNhanSort = 0;
            NguoiTienXuLyCap1 = string.Empty;
            NguoiTienXuLyCap2 = string.Empty;
            NguoiTienXuLyCap3 = string.Empty;
            NguoiXuLyId = 0;
            NguoiXuLy = string.Empty;
            NgayQuaHan = DateTime.Now;
            NgayQuaHanSort = 0;
            NgayCanhBao = DateTime.Now;
            NgayCanhBaoSort = 0;
            NgayChuyenPhongBan = DateTime.Now;
            NgayChuyenPhongBanSort = 0;
            NgayCanhBaoPhongBanXuLy = DateTime.Now;
            NgayCanhBaoPhongBanXuLySort = 0;
            NgayQuaHanPhongBanXuLy = DateTime.Now;
            NgayQuaHanPhongBanXuLySort = 0;
            NgayTraLoiKN = DateTime.Now;
            NgayTraLoiKNSort = 0;
            NgayDongKN = DateTime.Now;
            NgayDongKNSort = 0;
            NoiDungXuLyDongKN = string.Empty;
            KQXuLy_SHCV = string.Empty;
            KQXuLy_CCT = false;
            KQXuLy_CSL = false;
            KQXuLy_PTSL_IR = false;
            KQXuLy_PTSL_Khac = string.Empty;
            KetQuaXuLy = string.Empty;
            NoiDungXuLy = string.Empty;
            GhiChu = string.Empty;
            KNHangLoat = false;
            SoTienKhauTru_TKC = 0;
            SoTienKhauTru_KM = 0;
            SoTienKhauTru_KM1 = 0;
            SoTienKhauTru_KM2 = 0;
            SoTienKhauTru_Data = 0;
            SoTienKhauTru_Khac = 0;
            SoTienKhauTru_TS_GPRS = 0;
            SoTienKhauTru_TS_CP = 0;
            SoTienKhauTru_TS_Thoai = 0;
            SoTienKhauTru_TS_SMS = 0;
            SoTienKhauTru_TS_IR = 0;
            SoTienKhauTru_TS_Khac = 0;
            IsLuuKhieuNai = false;
            IsPhanViec = false;
            IsPhanHoi = false;
            KhieuNaiFrom = 0;
            LyDoGiamTru = 0;
            ArchiveId = 0;
            CallCount = 1;
            DoHaiLong = 2;
            Other1 = string.Empty;
            Other2 = string.Empty;
            Other3 = string.Empty;
            Other4 = string.Empty;
            Other5 = string.Empty;
            ChiTietLoiId = -1; // Trường hợp = 0 có nghĩa là "Khác" trong tập danh sách con của Nguyên nhân lỗi
        }

        
    }
}