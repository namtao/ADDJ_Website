using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_UpdateArchive in Databasse
    /// </summary>
    /// <author>DaoVanDuong</author>
    /// <date>23/10/2016</date>

    [Serializable]
    public class KhieuNai_UpdateArchiveInfo
    {
        public int KhieuNaiId { get; set; }
        public int TrangThai { get; set; }
        public int ArchiveId { get; set; }
        public int NgayTiepNhanSort { get; set; }
        public string GhiChu { get; set; }

        public KhieuNai_UpdateArchiveInfo()
        {
            KhieuNaiId = 0;
            TrangThai = 0;
            ArchiveId = 0;
            NgayTiepNhanSort = 0;
            GhiChu = string.Empty;

        }
    }
}