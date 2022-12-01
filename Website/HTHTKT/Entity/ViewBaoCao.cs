using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT.Entity
{
    public class ViewBaoCaoCTTheoTT
    {
        public int? ID { get; set; }
        public string MA_YEUCAU { get; set; }
        public string SODIENTHOAI { get; set; }
        public string TENMUCDO { get; set; }
        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }
        public string NOIDUNG_YEUCAU { get; set; }
        public string NOIDUNG_XL_DONG_HOTRO { get; set; }
        public string TEN_LUONG { get; set; }
        public string MOTA { get; set; }
        public int? SOBUOC { get; set; }
        public string TENHETHONG { get; set; }
        public string MA_HETHONG { get; set; }
        public int? TRANGTHAI { get; set; }
        public string NGUOITAO { get; set; }
        public int? DONVITAO { get; set; }
        public string TENDONVI { get; set; }
        public string MADONVI { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string DONVIPHOIHOP { get; set; }
    }
    public class ViewBaoCaoCTTheoND
    {
        public int? ID_NGUOIDUNG { get; set; }
        public string TenTruyCap { get; set; }
        public string TenDayDu { get; set; }
        public int? SLTonKyTruoc { get; set; }
        public int? SLTiepNhan { get; set; }
        public int? SLDaXuLy { get; set; }
        public int? SLDaXuLyQuaHan { get; set; }
        public int? SLTonDong { get; set; }
        public int? SLTonDongQuaHan { get; set; }
    }
    public class ViewBaoCaoCTTheoPB
    {
        public int? ID_DONVI { get; set; }
        public string MADONVI { get; set; }
        public string TENDONVI { get; set; }
        public int? SLTonKyTruoc { get; set; }
        public int? SLTiepNhan { get; set; }
        public int? SLDaXuLy { get; set; }
        public int? SLDaXuLyQuaHan { get; set; }
        public int? SLTonDong { get; set; }
        public int? SLTonDongQuaHan { get; set; }
    }
    public class ViewBaoCaoCTTheoHT
    {
        public int? ID_DONVI { get; set; }
        public string MADONVI { get; set; }
        public string TENDONVI { get; set; }
        public string TENHETHONG { get; set; }
        public int? SLTonKyTruoc { get; set; }
        public int? SLTiepNhan { get; set; }
        public int? SLDaXuLy { get; set; }
        public int? SLDaXuLyQuaHan { get; set; }
        public int? SLTonDong { get; set; }
        public int? SLTonDongQuaHan { get; set; }
    }
    public class ViewBaoCaoCaNhan
    {
        public int? ID { get; set; }
        public string MA_YEUCAU { get; set; }
        public string SODIENTHOAI { get; set; }
        public string TENMUCDO { get; set; }
        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }
        public string NOIDUNG_YEUCAU { get; set; }
        public string NOIDUNG_XL_DONG_HOTRO { get; set; }
        public string MOTA { get; set; }
        public int? SOBUOC { get; set; }
        public string TENHETHONG { get; set; }
        public string MA_HETHONG { get; set; }
        public bool? TRANGTHAI { get; set; }
        public string NGUOITAO { get; set; }
        public string TENDONVI { get; set; }
        public string MADONVI { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string DONVIPHOIHOP { get; set; }
    }
}