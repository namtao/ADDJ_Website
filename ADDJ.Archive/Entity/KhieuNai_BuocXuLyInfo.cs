using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_BuocXuLy in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/10/2013</date>

    [Serializable]
    public class KhieuNai_BuocXuLyInfo
    {
        public Int64 Id { get; set; }
        public int KhieuNaiId { get; set; }
        public string NoiDung { get; set; }
        public bool IsAuto { get; set; }
        public int PhongBanXuLyId { get; set; }
        public int NguoiXuLyId { get; set; }        
        public string CUser { get; set; }
        public DateTime CDate { get; set; }
        public string LUser { get; set; }
        public DateTime LDate { get; set; }

        public KhieuNai_BuocXuLyInfo()
        {
            Id = 0;
            KhieuNaiId = 0;
            NoiDung = string.Empty;
            IsAuto = false;
            PhongBanXuLyId = 0;
            NguoiXuLyId = 0;
        }
    }
}