using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_NGUOIDUNG
    {
        public int Id { get; set; }

        public string TenDoiTac { get; set; }

        public int? DoiTacId { get; set; }

        public int? KhuVucId { get; set; }

        public int? NhomNguoiDung { get; set; }

        public string TenTruyCap { get; set; }

        public string MatKhau { get; set; }

        public string TenDayDu { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string DiaChi { get; set; }

        public string DiDong { get; set; }

        public string CoDinh { get; set; }

        public byte? Sex { get; set; }

        public string Email { get; set; }

        public string CongTy { get; set; }

        public string DiaChiCongTy { get; set; }

        public string FaxCongTy { get; set; }

        public string DienThoaiCongTy { get; set; }

        public byte? TrangThai { get; set; }

        public bool? SuDungLDAP { get; set; }

        public int? LoginCount { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool? IsLogin { get; set; }

        public DateTime? LDate { get; set; }

        public DateTime? CDate { get; set; }

        public string CUser { get; set; }

        public string LUser { get; set; }

        public int? ID_DONVI { get; set; }
        public int? XOA { get; set; }
        public string MaNhanVienCMT { get; set; }
    }

    public class HT_NGUOIDUNG2
    {
        public int Id { get; set; }

        public string TenDoiTac { get; set; }

        public int? DoiTacId { get; set; }

        public int? KhuVucId { get; set; }

        public int? NhomNguoiDung { get; set; }

        public string TenTruyCap { get; set; }

        public string MatKhau { get; set; }

        public string TenDayDu { get; set; }

        public DateTime? NgaySinh { get; set; }

        public string DiaChi { get; set; }

        public string DiDong { get; set; }

        public string CoDinh { get; set; }

        public byte? Sex { get; set; }

        public string Email { get; set; }

        public string CongTy { get; set; }

        public string DiaChiCongTy { get; set; }

        public string FaxCongTy { get; set; }

        public string DienThoaiCongTy { get; set; }

        public int? TrangThai { get; set; }

        public bool? SuDungLDAP { get; set; }

        public int? LoginCount { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool? IsLogin { get; set; }

        public DateTime? LDate { get; set; }

        public DateTime? CDate { get; set; }

        public string CUser { get; set; }

        public string LUser { get; set; }

        public int? ID_DONVI { get; set; }
        public int? XOA { get; set; }
        public int? ID_DONVI_NGUOIDUNG { get; set; }
        public string TenNhom { get; set; }
        public int? ID_DONVI_ND { get; set; }
        public string MaNhanVienCMT { get; set; }
    }
}