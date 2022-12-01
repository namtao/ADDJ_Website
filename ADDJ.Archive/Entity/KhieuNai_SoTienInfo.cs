using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_SoTien in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>27/02/2014</date>

    [Serializable]
    public class KhieuNai_SoTienInfo
    {
        public Int64 Id { get; set; }
        public int KhieuNaiId { get; set; }
        public int DichVuCPId { get; set; }
        public string MaDichVu { get; set; }
        public string DauSo { get; set; }
        public Int16 LoaiTien { get; set; }
        public int LyDoGiamTru { get; set; }
        public decimal SoTien { get; set; }
        public decimal SoTien_Edit { get; set; }
        public string GhiChu { get; set; }
        public bool IsDaBuTien { get; set; }
        public string CUser { get; set; }
        public DateTime CDate { get; set; }
        public string LUser { get; set; }
        public DateTime LDate { get; set; }

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