using GQKN.Archive.Core.AILucene;
using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table LoaiKhieuNai in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    [Serializable]
    public class LoaiKhieuNaiInfo
    {
        [AIFieldUnikey("Id")]
        public int Id { get; set; }
        public int ParentLoaiKhieuNaiId { get; set; }
        [AIField("ParentId")]
        public int ParentId { get; set; }
        [AIField("Name")]
        public string Name { get; set; }
        [AIField("Description")]
        public string NameLoaiKhieuNai { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
        public byte Cap { get; set; }
        public byte Status { get; set; }
        public string ThoiGianCanhBao { get; set; }
        public string ThoiGianUocTinh { get; set; }
        [AIField("MaDichVu")]
        public string MaDichVu { get; set; }
        public int LoaiKhieuNai_NhomId { get; set; }
        public string LoaiKhieuNai_TenNhom { get; set; }
        public int ThuocDonVi { get; set; }
        public string PhongBanChuyenKhieuNai { get; set; }
        public int Test { get; set; }
        public string DonViTiepNhanXL { get; set; }
        public int DonViQuanLyId { get; set; }
        public string ServiceCode { get; set; }
        public byte IsChungMa { get; set; }
        public int CongTyDoiTacId { get; set; }
        public string TenCongTyDoiTac { get; set; }
        public int DoanhThuStatus { get; set; }
        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }
        public string CUser { get; set; }
        public string LUser { get; set; }
        public LoaiKhieuNaiInfo()
        {
            Id = 0;
            ParentLoaiKhieuNaiId = 0;
            ParentId = 0;
            Name = string.Empty;
            NameLoaiKhieuNai = string.Empty;
            Description = string.Empty;
            Sort = 0;
            Cap = 0;
            Status = 0;
            ThoiGianCanhBao = string.Empty;
            ThoiGianUocTinh = string.Empty;
            MaDichVu = string.Empty;
            Test = 0;
            DonViTiepNhanXL = string.Empty;

            LDate = CDate = DateTime.Now;
            CUser = LUser = "Administrator";
        }
    }
}
