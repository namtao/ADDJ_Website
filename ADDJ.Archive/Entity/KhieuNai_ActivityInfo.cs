using System;

namespace GQKN.Archive.Entity
{
    /// <summary>
    /// Class Mapping table KhieuNai_Activity in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>14/10/2013</date>

    [Serializable]
    public class KhieuNai_ActivityInfo
    {
        public Int64 Id { get; set; }
        public int KhieuNaiId { get; set; }
        public int PhongBanXuLyTruocId { get; set; }
        public string NguoiXuLyTruoc { get; set; }
        public int PhongBanXuLyId { get; set; }
        public string NguoiXuLy { get; set; }
        public string NguoiDuocPhanHoi { get; set; }
        public Int16 HanhDong { get; set; }
        public string GhiChu { get; set; }
        public bool IsCurrent { get; set; }
        public Int64 ActivityTruoc { get; set; }
        public DateTime NgayTiepNhan { get; set; }
        public DateTime NgayCanhBao { get; set; }
        public DateTime NgayQuaHan { get; set; }

        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }

        public int DoiTacId { get; set; }
        public string PhongBan_Name { get; set; }

        public DateTime NgayTiepNhan_NguoiXuLy { get; set; }
        public DateTime NgayTiepNhan_NguoiXuLyTruoc { get; set; }

        public DateTime NgayTiepNhan_PhongBanXuLyTruoc { get; set; }

        public DateTime NgayQuaHan_PhongBanXuLyTruoc { get; set; }

        public KhieuNai_ActivityInfo()
        {
            Id = 0;
            KhieuNaiId = 0;
            PhongBanXuLyTruocId = 0;
            NguoiXuLyTruoc = string.Empty;
            PhongBanXuLyId = 0;
            NguoiXuLy = string.Empty;
            NguoiDuocPhanHoi = string.Empty;
            HanhDong = 0;
            GhiChu = string.Empty;
            IsCurrent = false;
            ActivityTruoc = 0;
            NgayTiepNhan = DateTime.Now;
            NgayCanhBao = DateTime.Now;
            NgayQuaHan = DateTime.Now;
            NgayTiepNhan_NguoiXuLy = DateTime.MaxValue;
            NgayTiepNhan_PhongBanXuLyTruoc = DateTime.MaxValue;
            NgayQuaHan_PhongBanXuLyTruoc = DateTime.MaxValue;
            NgayTiepNhan_NguoiXuLyTruoc = DateTime.MaxValue;
        }
    }
}