using System;
using SolrNet.Attributes;

namespace ADDJ.Entity
{
    public class KhieuNaiSolrInfo
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrField("MaKhieuNai")]
        public string MaKhieuNai { get; set; }

        [SolrField("KhuVucId")]
        public int KhuVucId { get; set; }

        [SolrField("DoiTacId")]
        public int DoiTacId { get; set; }

        [SolrField("PhongBanTiepNhanId")]
        public int PhongBanTiepNhanId { get; set; }

        [SolrField("KhuVucXuLyId")]
        public int KhuVucXuLyId { get; set; }

        [SolrField("DoiTacXuLyId")]
        public int DoiTacXuLyId { get; set; }

        [SolrField("PhongBanXuLyId")]
        public int PhongBanXuLyId { get; set; }

        [SolrField("LoaiKhieuNaiId")]
        public int LoaiKhieuNaiId { get; set; }

        [SolrField("LinhVucChungId")]
        public int LinhVucChungId { get; set; }

        [SolrField("LinhVucConId")]
        public int LinhVucConId { get; set; }

        [SolrField("LoaiKhieuNai")]
        public string LoaiKhieuNai { get; set; }

        [SolrField("LinhVucChung")]
        public string LinhVucChung { get; set; }

        [SolrField("LinhVucCon")]
        public string LinhVucCon { get; set; }

        [SolrField("DoUuTien")]
        public int DoUuTien { get; set; }

        [SolrField("IsKNGiamTru")]
        public bool IsKNGiamTru { get; set; }

        [SolrField("SoThueBao")]
        public Int64 SoThueBao { get; set; }

        [SolrField("IsTraSau")]
        public bool IsTraSau { get; set; }

        [SolrField("MaTinhId")]
        public int MaTinhId { get; set; }

        [SolrField("MaTinh")]
        public string MaTinh { get; set; }

        [SolrField("MaQuanId")]
        public int MaQuanId { get; set; }

        [SolrField("MaQuan")]
        public string MaQuan { get; set; }

        [SolrField("HoTenLienHe")]
        public string HoTenLienHe { get; set; }

        [SolrField("DiaChi_CCBS")]
        public string DiaChi_CCBS { get; set; }

        [SolrField("DiaChiLienHe")]
        public string DiaChiLienHe { get; set; }

        [SolrField("SDTLienHe")]
        public string SDTLienHe { get; set; }

        [SolrField("DiaDiemXayRa")]
        public string DiaDiemXayRa { get; set; }

        [SolrField("ThoiGianXayRa")]
        public string ThoiGianXayRa { get; set; }

        [SolrField("NoiDungPA")]
        public string NoiDungPA { get; set; }

        [SolrField("NoiDungCanHoTro")]
        public string NoiDungCanHoTro { get; set; }

        [SolrField("TrangThai")]
        public int TrangThai { get; set; }

        [SolrField("IsChuyenBoPhan")]
        public bool IsChuyenBoPhan { get; set; }

        //[SolrField("NguoiTiepNhanId")]
        //public string NguoiTiepNhanId { get; set; }

        [SolrField("NguoiTiepNhan")]
        public string NguoiTiepNhan { get; set; }

        [SolrField("HTTiepNhan")]
        public int HTTiepNhan { get; set; }

        [SolrField("NgayTiepNhan")]
        public DateTime NgayTiepNhan { get; set; }

        [SolrField("NgayTiepNhanSort")]
        public int NgayTiepNhanSort { get; set; }

        //[SolrField("NguoiTienXuLyCap1Id")]
        //public string NguoiTienXuLyCap1Id { get; set; }

        [SolrField("NguoiTienXuLyCap1")]
        public string NguoiTienXuLyCap1 { get; set; }

        //[SolrField("NguoiTienXuLyCap2Id")]
        //public string NguoiTienXuLyCap2Id { get; set; }

        [SolrField("NguoiTienXuLyCap2")]
        public string NguoiTienXuLyCap2 { get; set; }

        //[SolrField("NguoiTienXuLyCap3Id")]
        //public string NguoiTienXuLyCap3Id { get; set; }

        [SolrField("NguoiTienXuLyCap3")]
        public string NguoiTienXuLyCap3 { get; set; }

        //[SolrField("NguoiXuLyId")]
        //public string NguoiXuLyId { get; set; }

        [SolrField("NguoiXuLy")]
        public string NguoiXuLy { get; set; }

        [SolrField("NgayQuaHan")]
        public DateTime NgayQuaHan { get; set; }

        [SolrField("NgayQuaHanSort")]
        public int NgayQuaHanSort { get; set; }

        [SolrField("NgayCanhBao")]
        public DateTime NgayCanhBao { get; set; }

        [SolrField("NgayCanhBaoSort")]
        public int NgayCanhBaoSort { get; set; }

        [SolrField("NgayChuyenPhongBan")]
        public DateTime NgayChuyenPhongBan { get; set; }

        [SolrField("NgayChuyenPhongBanSort")]
        public int NgayChuyenPhongBanSort { get; set; }

        [SolrField("NgayCanhBaoPhongBanXuLy")]
        public DateTime NgayCanhBaoPhongBanXuLy { get; set; }

        [SolrField("NgayCanhBaoPhongBanXuLySort")]
        public int NgayCanhBaoPhongBanXuLySort { get; set; }

        [SolrField("NgayQuaHanPhongBanXuLy")]
        public DateTime NgayQuaHanPhongBanXuLy { get; set; }

        [SolrField("NgayQuaHanPhongBanXuLySort")]
        public int NgayQuaHanPhongBanXuLySort { get; set; }

        [SolrField("NgayTraLoiKN")]
        public DateTime NgayTraLoiKN { get; set; }

        [SolrField("NgayTraLoiKNSort")]
        public int NgayTraLoiKNSort { get; set; }

        [SolrField("NgayDongKN")]
        public DateTime NgayDongKN { get; set; }

        [SolrField("NgayDongKNSort")]
        public int NgayDongKNSort { get; set; }

        [SolrField("NoiDungXuLyDongKN")]
        public string NoiDungXuLyDongKN { get; set; }

        [SolrField("KQXuLy_SHCV")]
        public string KQXuLy_SHCV { get; set; }

        [SolrField("KQXuLy_CCT")]
        public bool KQXuLy_CCT { get; set; }

        [SolrField("KQXuLy_CSL")]
        public bool KQXuLy_CSL { get; set; }

        [SolrField("KQXuLy_PTSL_IR")]
        public bool KQXuLy_PTSL_IR { get; set; }

        [SolrField("KQXuLy_PTSL_Khac")]
        public string KQXuLy_PTSL_Khac { get; set; }

        [SolrField("KetQuaXuLy")]
        public string KetQuaXuLy { get; set; }

        [SolrField("NoiDungXuLy")]
        public string NoiDungXuLy { get; set; }

        [SolrField("GhiChu")]
        public string GhiChu { get; set; }

        [SolrField("KNHangLoat")]
        public bool KNHangLoat { get; set; }

        [SolrField("SoTienKhauTru_TKC")]
        public decimal SoTienKhauTru_TKC { get; set; }

        [SolrField("SoTienKhauTru_KM")]
        public decimal SoTienKhauTru_KM { get; set; }

        [SolrField("SoTienKhauTru_KM1")]
        public decimal SoTienKhauTru_KM1 { get; set; }
        [SolrField("SoTienKhauTru_KM2")]
        public decimal SoTienKhauTru_KM2 { get; set; }

        [SolrField("SoTienKhauTru_Data")]
        public decimal SoTienKhauTru_Data { get; set; }

        [SolrField("SoTienKhauTru_Khac")]
        public decimal SoTienKhauTru_Khac { get; set; }

        [SolrField("SoTienKhauTru_TS_GPRS")]
        public decimal SoTienKhauTru_TS_GPRS { get; set; }

        [SolrField("SoTienKhauTru_TS_CP")]
        public decimal SoTienKhauTru_TS_CP { get; set; }

        [SolrField("SoTienKhauTru_TS_Thoai")]
        public decimal SoTienKhauTru_TS_Thoai { get; set; }

        [SolrField("SoTienKhauTru_TS_SMS")]
        public decimal SoTienKhauTru_TS_SMS { get; set; }

        [SolrField("SoTienKhauTru_TS_IR")]
        public decimal SoTienKhauTru_TS_IR { get; set; }

        [SolrField("SoTienKhauTru_TS_Khac")]
        public decimal SoTienKhauTru_TS_Khac { get; set; }

        [SolrField("IsLuuKhieuNai")]
        public bool IsLuuKhieuNai { get; set; }

        [SolrField("IsPhanViec")]
        public bool IsPhanViec { get; set; }

        [SolrField("IsPhanHoi")]
        public bool IsPhanHoi { get; set; }

        [SolrField("KhieuNaiFrom")]
        public int KhieuNaiFrom { get; set; }

        [SolrField("LyDoGiamTru")]
        public int LyDoGiamTru { get; set; }

        [SolrField("ArchiveId")]
        public int ArchiveId { get; set; }

        [SolrField("Other1")]
        public string Other1 { get; set; }

        [SolrField("Other2")]
        public string Other2 { get; set; }

        [SolrField("Other3")]
        public string Other3 { get; set; }

        [SolrField("Other4")]
        public string Other4 { get; set; }

        [SolrField("Other5")]
        public string Other5 { get; set; }

        [SolrField("CDate")]
        public DateTime CDate { get; set; }

        [SolrField("CUser")]
        public string CUser { get; set; }

        [SolrField("LDate")]
        public DateTime LDate { get; set; }

        [SolrField("LUser")]
        public string LUser { get; set; }

        [SolrField("PhongBanTiepNhan")]
        public string PhongBanTiepNhan { get; set; }

        [SolrField("PhongBanXuLy")]
        public string PhongBanXuLy { get; set; }

        [SolrField("IsQuaHanToanTrinh")]
        public bool IsQuaHanToanTrinh { get; set; }

        [SolrField("IsQuaHanPhongBanXuLy")]
        public bool IsQuaHanPhongBanXuLy { get; set; }

        [SolrField("IsPhongBanXuLyLaPhongBanTiepNhan")]
        public bool IsPhongBanXuLyLaPhongBanTiepNhan { get; set; }

        [SolrField("CallCount")]
        public int CallCount { get; set; }

        [SolrField("DoHaiLong")]
        public int DoHaiLong { get; set; }

        [SolrField("ChiTietLoiId")]
        public int ChiTietLoiId { get; set; }
    }
}
