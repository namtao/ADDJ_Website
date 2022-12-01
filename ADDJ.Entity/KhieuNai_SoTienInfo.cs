using SolrNet.Attributes;
using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_SoTien in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>27/02/2014</date>

    [Serializable]
    public class KhieuNai_SoTienInfo
    {
        [SolrField("Id")]
        public Int64 Id { get; set; }
        [SolrField("KhieuNaiId")]
        public int KhieuNaiId { get; set; }
        [SolrField("DichVuCPId")]
        public int DichVuCPId { get; set; }
        [SolrField("MaDichVu")]
        public string MaDichVu { get; set; }
        [SolrField("DauSo")]
        public string DauSo { get; set; }
        [SolrField("LoaiTien")]
        public Int16 LoaiTien { get; set; }
        [SolrField("LyDoGiamTru")]
        public int LyDoGiamTru { get; set; }
        [SolrField("SoTien")]
        public decimal SoTien { get; set; }
        [SolrField("SoTien_Edit")]
        public decimal SoTien_Edit { get; set; }
        [SolrField("GhiChu")]
        public string GhiChu { get; set; }
        [SolrField("IsDaBuTien")]
        public bool IsDaBuTien { get; set; }
        [SolrField("CUser")]
        public string CUser { get; set; }
        [SolrField("CDate")]
        public DateTime CDate { get; set; }
        [SolrField("LUser")]
        public string LUser { get; set; }
        [SolrField("LDate")]
        public DateTime LDate { get; set; }
        [SolrField("LinhVucChung")]
        public string LinhVucChung { get; set; }

        // 07/04/2016: truongvv: thêm vào để lấy thông tin lịch sử bù tiền
        [SolrField("TenFile")]
        public string TenFile { get; set; }
        [SolrField("GhiChu2")]
        public string GhiChu2 { get; set; }

        // 26/04/2015: truongvv: thêm vào để lấy thông tin phục vụ báo cáo giảm trừ DV GTGT (view SoTienGiamTru)
        [SolrField("SoTienFinal")]
        public decimal SoTienFinal { get; set; }
        [SolrField("DoiTacXuLyId")]
        public int DoiTacXuLyId { get; set; }
        [SolrField("TenDoiTacXuLy")]
        public string TenDoiTacXuLy { get; set; }
        [SolrField("KhuVucId")]
        public int KhuVucId { get; set; }
        [SolrField("DoiTacId")]
        public int DoiTacId { get; set; }
        [SolrField("PhongBanXuLyId")]
        public int PhongBanXuLyId { get; set; }
        [SolrField("PhongBanTiepNhanId")]
        public int PhongBanTiepNhanId { get; set; }
        [SolrField("LoaiKhieuNaiId")]
        public int LoaiKhieuNaiId { get; set; }
        [SolrField("LinhVucChungId")]
        public int LinhVucChungId { get; set; }
        [SolrField("LinhVucConId")]
        public int LinhVucConId { get; set; }
        [SolrField("LoaiKhieuNai")]
        public string LoaiKhieuNai { get; set; }
        [SolrField("LinhVucCon")]
        public string LinhVucCon { get; set; }
        [SolrField("SoThueBao")]
        public string SoThueBao { get; set; }
        [SolrField("IsTraSau")]
        public bool IsTraSau { get; set; }
        [SolrField("MaTinhId")]
        public int MaTinhId { get; set; }
        [SolrField("MaQuanId")]
        public int MaQuanId { get; set; }
        [SolrField("MaTinh")]
        public string MaTinh { get; set; }
        [SolrField("MaQuan")]
        public string MaQuan { get; set; }
        [SolrField("TrangThai")]
        public int TrangThai { get; set; }
        [SolrField("NgayDongKN")]
        public DateTime NgayDongKN { get; set; }
        [SolrField("NgayDongKNSort")]
        public int NgayDongKNSort { get; set; }
        [SolrField("NoiDungPA")]
        public string NoiDungPA { get; set; }
        [SolrField("NoiDungXuLyDongKN")]
        public string NoiDungXuLyDongKN { get; set; }
        [SolrField("Expr1")]
        public string Expr1 { get; set; }
        [SolrField("NguoiXuLyId")]
        public int NguoiXuLyId { get; set; }
        [SolrField("NguoiXuLy")]
        public string NguoiXuLy { get; set; }
        // 10/05/2016 truongvv: thêm vào để lấy thông tin phục vụ báo cáo trong công văn 601 mẫu 04 báo cáo tổng hợp theo chỉ tiêu thời gian giải quyết
        [SolrField("NgayTiepNhan")]
        public DateTime NgayTiepNhan { get; set; }
        [SolrField("NgayTiepNhanSort")]
        public int NgayTiepNhanSort { get; set; }
        public KhieuNai_SoTienInfo()
        {
            Id = 0;
            KhieuNaiId = 0;
            DichVuCPId = 0;
            MaDichVu = string.Empty;
            DauSo = string.Empty;
            LoaiTien = 0;
            SoTien = 0;
            SoTien_Edit = 0;
            GhiChu = string.Empty;
            IsDaBuTien = false;

        }
    }
}