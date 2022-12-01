using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class DoiTac
    {
        public int Id { get; set; }

        public int? DonViTrucThuoc { get; set; }

        public int? DonViTrucThuocChoBaoCao { get; set; }

        public int? ProvinceId { get; set; }

        public int? ProvinceHuyenId { get; set; }

        public byte? DoiTacType { get; set; }

        public string MaDoiTac { get; set; }

        public string TenDoiTac { get; set; }

        public string DienThoai { get; set; }

        public string MaSoThue { get; set; }

        public string Fax { get; set; }

        public string DiaChi { get; set; }

        public string Website { get; set; }

        public string NguoiDaiDien { get; set; }

        public string ChucVu { get; set; }

        public int? LoaiGiayTo { get; set; }

        public DateTime? NgayCap { get; set; }

        public string NoiCap { get; set; }

        public string SoGiayTo { get; set; }

        public string SoHopDong { get; set; }

        public DateTime? NgayHopDong { get; set; }

        public string GhiChu { get; set; }

        public int? Sort { get; set; }

        public DateTime? CDate { get; set; }

        public DateTime? LDate { get; set; }

        public string CUser { get; set; }

        public string LUser { get; set; }
    }
}