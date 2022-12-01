using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table ConfigurationTime in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>22/08/2013</date>

    [Serializable]
    public class ConfigurationTimeInfo
    {
        public int Id { get; set; }
        public int NgayBatDauCuaTuan { get; set; }
        public decimal TongSoNgayTrenTuan { get; set; }
        public int GioBatDau { get; set; }
        public int GioKetThuc { get; set; }
        public string NgayLe { get; set; }


        public ConfigurationTimeInfo()
        {
            Id = 0;
            NgayBatDauCuaTuan = 0;
            TongSoNgayTrenTuan = 0;
            GioBatDau = 0;
            GioKetThuc = 0;
            NgayLe = string.Empty;

        }
    }
}