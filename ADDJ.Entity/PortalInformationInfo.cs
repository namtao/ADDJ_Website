using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Log.Entity
{
    public class PortalInformationInfo
    {
        public Int64 SoThueBao { get; set; }
        public string DisplayName { get; set; }
        public string NgaySinh { get; set; }
        public int GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public int QuocGia { get; set; }
        public int Tinh { get; set; }
        public int QuanHuyen { get; set; }
        public int XaPhuong { get; set; }
        public int NgheNghiep { get; set; }
        public string SoDienThoai { get; set; }
        public int LoaiDienThoai { get; set; }
        public string Email { get; set; }
        public string NgonNgu { get; set; }

        public string DiaChiCoQuan { get; set; }
        public string DienThoaiCoDinh { get; set; }
        public string SoGiayTo { get; set; }
        public string NoiCapGiayTo { get; set; }
        public string NgayCapGiayTo { get; set; }

        public PortalInformationInfo()
        {
            SoThueBao = 0;
            DisplayName = string.Empty;
            NgaySinh = string.Empty;
            GioiTinh = 0;
            DiaChi = string.Empty;
            QuocGia = 0;
            Tinh = 0;
            QuanHuyen = 0;
            XaPhuong = 0;
            NgheNghiep = 0;
            NgheNghiep = 0;
            SoDienThoai = string.Empty;
            LoaiDienThoai = 0;
            Email = string.Empty;
            NgonNgu = string.Empty;

            DiaChiCoQuan = string.Empty;
            DienThoaiCoDinh = string.Empty;
            SoGiayTo = string.Empty;
            NoiCapGiayTo = string.Empty;
            NgayCapGiayTo = string.Empty;
        }
    }
}
