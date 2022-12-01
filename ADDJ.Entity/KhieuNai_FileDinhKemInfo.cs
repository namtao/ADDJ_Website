using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_FileDinhKem in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>26/08/2013</date>

    [Serializable]
    public class KhieuNai_FileDinhKemInfo
    {
        public Int64 Id { get; set; }
        public int KhieuNaiId { get; set; }
        public string TenFile { get; set; }
        public decimal KichThuoc { get; set; }
        public string LoaiFile { get; set; }
        public string GhiChu { get; set; }
        public string URLFile { get; set; }
        public Int16 Status { get; set; }

        public string CUser { get; set; }
        public DateTime CDate { get; set; }
        public int ErrorId { get; set; }

        public KhieuNai_FileDinhKemInfo()
        {
            Id = 0;
            KhieuNaiId = 0;
            TenFile = string.Empty;
            KichThuoc = 0;
            LoaiFile = string.Empty;
            GhiChu = string.Empty;
            URLFile = string.Empty;
            Status = 0;

        }
    }
}