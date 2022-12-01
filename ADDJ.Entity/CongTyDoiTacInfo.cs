using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table CongTyDoiTac in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>14/10/2013</date>

    [Serializable]
    public class CongTyDoiTacInfo
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string TenVietTat { get; set; }
        public string TenVietTatKhac { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai_Fax { get; set; }
        public string Website { get; set; }
        public string HoTroKhachHang { get; set; }
        public int TrangThai { get; set; }
        public int Specal { get; set; }
        public int SpecalStatus { get; set; }

        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }
        public string CUser { get; set; }
        public string LUser { get; set; }

        public CongTyDoiTacInfo()
        {
            ID = 0;
            Ten = string.Empty;
            TenVietTat = string.Empty;
            TenVietTatKhac = string.Empty;
            DiaChi = string.Empty;
            DienThoai_Fax = string.Empty;
            Website = string.Empty;
            HoTroKhachHang = string.Empty;
            TrangThai = 0;
            Specal = 0;
            SpecalStatus = 0;
        }
    }

}
