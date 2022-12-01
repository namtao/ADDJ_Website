using System;
using ADDJ.Core.AILucene;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table DauSoCP in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>14/10/2013</date>

    [Serializable]
    public class DauSoCPInfo
    {
        [AIFieldUnikey("ID")]
        public int ID { get; set; }
        [AIField("DauSo")]
        public string DauSo { get; set; }
        public string LoaiDauSo { get; set; }
        public int CongTyDoiTacId { get; set; }

        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }
        public string CUser { get; set; }
        public string LUser { get; set; }

        public DauSoCPInfo()
        {
            ID = 0;
            DauSo = string.Empty;
            LoaiDauSo = string.Empty;
            CongTyDoiTacId = 0;

        }
    }
}