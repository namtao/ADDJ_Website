using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;


namespace ADDJ.Entity
{
    [Serializable]
    public class KhieuNai_ReportInfo
    {
        [SolrUniqueKey("Id")]
        public int Id { get; set; }

        [SolrUniqueKey("KhieuNaiId")]
        public int KhieuNaiId { get; set; }

        [SolrField("MaKhieuNai")]
        public string MaKhieuNai { get; set; }

        [SolrField("KhuVucId")]
        public int KhuVucId { get; set; }

        [SolrField("DoiTacId")]
        public int DoiTacId { get; set; }

        [SolrField("PhongBanTiepNhanId")]
        public int PhongBanTiepNhanId { get; set; }

        [SolrField("PhongBanXuLyId")]
        public int PhongBanXuLyId { get; set; }

        [SolrField("KhieuNai_PhongBanXuLyId")]
        public int KhieuNai_PhongBanXuLyId { get; set; }

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

        [SolrField("SoThueBao")]
        public Int64 SoThueBao { get; set; }

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

        [SolrField("LyDoGiamTru")]
        public int LyDoGiamTru { get; set; }

        [SolrField("IsChuyenBoPhan")]
        public bool IsChuyenBoPhan { get; set; }

        [SolrField("NguoiTiepNhan")]
        public string NguoiTiepNhan { get; set; }

        [SolrField("HTTiepNhan")]
        public int HTTiepNhan { get; set; }

        [SolrField("NgayTiepNhan")]
        public DateTime NgayTiepNhan { get; set; }

        [SolrField("KhieuNai_NgayTiepNhan")]
        public DateTime KhieuNai_NgayTiepNhan { get; set; }

        [SolrField("NgayTiepNhanSort")]
        public int NgayTiepNhanSort { get; set; }

        [SolrField("NguoiTienXuLyCap1")]
        public string NguoiTienXuLyCap1 { get; set; }

        [SolrField("NguoiTienXuLyCap2")]
        public string NguoiTienXuLyCap2 { get; set; }

        [SolrField("NguoiTienXuLyCap3")]
        public string NguoiTienXuLyCap3 { get; set; }

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

        [SolrField("KhieuNai_NgayDongKNSort")]
        public int KhieuNai_NgayDongKNSort { get; set; }

        [SolrField("KhieuNai_NgayQuaHan")]
        public DateTime KhieuNai_NgayQuaHan { get; set; }

        [SolrField("KhieuNai_NgayQuaHanSort")]
        public int KhieuNai_NgayQuaHanSort { get; set; }

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

        [SolrField("SHCV")]
        public string SHCV { get; set; }

        [SolrField("IsCCT")]
        public bool IsCCT { get; set; }

        [SolrField("IsCSL")]
        public bool IsCSL { get; set; }

        [SolrField("PTSoLieu_IR")]
        public bool PTSoLieu_IR { get; set; }

        [SolrField("PTSoLieu_Khac")]
        public string PTSoLieu_Khac { get; set; }

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

        [SolrField("CDate")]
        public DateTime CDate { get; set; }

        [SolrField("CUser")]
        public string CUser { get; set; }

        [SolrField("LDate")]
        public DateTime LDate { get; set; }

        [SolrField("LUser")]
        public string LUser { get; set; }

        [SolrField("PhongBan_Name")]
        public string PhongBan_Name { get; set; }

        [SolrField("STT")]
        public int STT { get; set; }

        [SolrField("SoTienKhauTru")]
        public decimal SoTienKhauTru { get; set; }

        [SolrField("TinhThanh")]
        public string TinhThanh { get; set; }

        [SolrField("NguoiXuLyFullName")]
        public string NguoiXuLyFullName { get; set; }

        [SolrField("MaDichVu")]
        public string MaDichVu { get; set; }

        [SolrField("SoLuongGiamTru")]
        public int SoLuongGiamTru { get; set; }

        [SolrField("KetQuaGiamTru")]
        public decimal KetQuaGiamTru { get; set; }

        [SolrField("DauSoId")]
        public int DauSoId { get; set; }

        [SolrField("DauSo")]
        public string DauSo { get; set; }

        [SolrField("SoTien")]
        public decimal SoTien { get; set; }

        [SolrField("SoTien_Edit")]
        public decimal SoTien_Edit { get; set; }

        [SolrField("SoTienFinal")]
        public decimal SoTienFinal { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("PhongBanXuLyTruocId")]
        public int PhongBanXuLyTruocId { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("ActivityId")]
        public int ActivityId { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("DoiTacXuLyTruocId")]
        public int DoiTacXuLyTruocId { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("DoiTacXuLyId")]
        public int DoiTacXuLyId { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("NguoiXuLyTruoc")]
        public string NguoiXuLyTruoc { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("IsCurrent")]
        public bool IsCurrent { get; set; }

        // Dùng cho KhieuNai_Activity
        [SolrField("HanhDong")]
        public int HanhDong { get; set; }

        [SolrField("LastPhongBanXuLyId")]
        public int LastPhongBanXuLyId { get; set; }

        [SolrField("LastDoiTacXuLyId")]
        public int LastDoiTacXuLyId { get; set; }

        [SolrField("CUser_KetQuaXuLy")]
        public string CUser_KetQuaXuLy { get; set; }

        [SolrField("LUser_KetQuaXuLy")]
        public string LUser_KetQuaXuLy { get; set; }

        [SolrField("DichVuCPId")]
        public int DichVuCPId { get; set; }

        [SolrField("TenPhongBanXuLyTruoc")]
        public string TenPhongBanXuLyTruoc { get; set; }

        [SolrField("TenPhongBanXuLy")]
        public string TenPhongBanXuLy { get; set; }

        [SolrField("NgayQuaHanPhongBanXuLyTruoc")]
        public DateTime NgayQuaHanPhongBanXuLyTruoc { get; set; }

        [SolrField("NgayTiepNhanPhongBanXuLyTruoc")]
        public DateTime NgayTiepNhanPhongBanXuLyTruoc { get; set; }

        [SolrField("NgayTiepNhan_NguoiXuLy")]
        public DateTime NgayTiepNhan_NguoiXuLy { get; set; }

        [SolrField("NgayTiepNhan_PhongBanXuLyTruoc")]
        public DateTime NgayTiepNhan_PhongBanXuLyTruoc { get; set; }

        [SolrField("NgayQuaHan_PhongBanXuLyTruoc")]
        public DateTime NgayQuaHan_PhongBanXuLyTruoc { get; set; }

        [SolrField("NgayTiepNhan_NguoiXuLyTruoc")]
        public DateTime NgayTiepNhan_NguoiXuLyTruoc { get; set; }

        [SolrField("KhuVucXuLyId")]
        public int KhuVucXuLyId { get; set; }

        [SolrField("LoaiKhieuNai_NhomId")]
        public int LoaiKhieuNai_NhomId { get; set; }

        [SolrField("LoaiKhieuNai_TenNhom")]
        public string LoaiKhieuNai_TenNhom { get; set; }

        [SolrField("KhieuNai_GhiChu")]
        public string KhieuNai_GhiChu { get; set; }

        [SolrField("ChiTietLoiId")]
        public int ChiTietLoiId { get; set; }

        [SolrField("NoiDungXuLyDongKN")]
        public string NoiDungXuLyDongKN { get; set; }

        [SolrField("ArchiveId")]
        public int ArchiveId { get; set; }

        [SolrField("KhieuNai_NgayDongKN")]
        public DateTime KhieuNai_NgayDongKN { get; set; }

        [SolrField("ActivityTruoc")]
        public long ActivityTruoc { get; set; }

        public KhieuNai_ReportInfo() : base()
        {
            STT = 0;
            SoTienKhauTru = 0;
            TinhThanh = string.Empty;
            NguoiXuLyFullName = string.Empty;
            MaDichVu = string.Empty;
            SoLuongGiamTru = 0;
            KetQuaGiamTru = 0;
            ActivityTruoc = 0;
        }
    }
}
