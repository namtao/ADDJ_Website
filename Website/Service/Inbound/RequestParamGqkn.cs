using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Service.Inbound
{
    public enum LoaiThueBao : byte
    {
        Trả_trước = 1,
        Trả_sau = 2
    }

    public enum HTTiepNhan_Type : byte
    {
        Điểm_giao_dịch = 1,
        Cổng_CSKH_9191 = 2,
        Email = 3,
        Đơn_thư = 5,
        Điện_thoại = 6,
        Khác = 99,
    }

    public enum DoUuTien_Type : byte
    {
        Thông_thường = 1,
        Khẩn_cấp = 2,
    }

    public class RequestParamGqkn
    {
        public int SoThueBao { get; set; }
        public LoaiThueBao LoaiThuBao { get; set; }
        public int LoaiKhieuNaiId { get; set; }
        public int LinhVucChungId { get; set; }
        public int LinhVucConId { get; set; }
        public string LoaiKhieuNai { get; set; }
        public string LinhVucChung { get; set; }
        public string LinhVucCon { get; set; }

        public string HoTen { get; set; }
        public string DienThoaiLienHe { get; set; }

        public string DiaChi { get; set; }
        public string ThoiGianSuCo { get; set; }
        public string DiaChiLienHe { get; set; }
        public DoUuTien_Type DoUuTien { get; set; }
        public HTTiepNhan_Type HinhThucTiepNhan { get; set; }
        public int TinhId { get; set; }
        public string Tinh { get; set; }
        public int QuanHuyenId { get; set; }
        public string QuanHuyen { get; set; }
        public string DiaChiSuCo { get; set; }
        public string NoiDungPA { get; set; }
        public string NoiDungHoTro { get; set; }
        public string GhiChu { get; set; }
        public bool IsChuyenKN { get; set; }
        public string TenFileDinhKem{get;set;}
        public string LoaiFileDinhKem{get;set;}
        public byte[] FileDinhKem{get;set;}
        
    }
}