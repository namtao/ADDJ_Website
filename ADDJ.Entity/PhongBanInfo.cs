using ADDJ.Core.AILucene;
using System;

namespace ADDJ.Entity
{
    /// <summary>
    /// Class Mapping table PhongBan in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/06/2014</date>

    [Serializable]
    public class PhongBanInfo
    {
        [AIFieldUnikey("Id")]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int LoaiPhongBanId { get; set; }
        public int KhuVucId { get; set; }
        public int DoiTacId { get; set; }
        [AIField("Name")]
        public string Name { get; set; }
        [AIField("Description")]
        public string Description { get; set; }
        public int Sort { get; set; }
        public int Cap { get; set; }
        public int Status { get; set; }
        public string SortOrder { get; set; }
        public bool IsDinhTuyenKN { get; set; }
        public Int16 DefaultHTTN { get; set; }
        public bool IsChuyenTiepKN { get; set; }

        public bool IsChuyenVNP { get; set; }
        public bool IsChuyenHNI { get; set; }
        public string CUser { get; set; }
        public DateTime CDate { get; set; }
        public string LUser { get; set; }
        public DateTime LDate { get; set; }

        public int CountUser { get; set; }

        public PhongBanInfo()
        {
            Id = 0;
            ParentId = 0;
            LoaiPhongBanId = 0;
            KhuVucId = 0;
            DoiTacId = 0;
            Name = string.Empty;
            Description = string.Empty;
            Sort = 0;
            Cap = 0;
            Status = 0;
            SortOrder = string.Empty;
            IsDinhTuyenKN = false;
            DefaultHTTN = 0;
            IsChuyenTiepKN = false;
            IsChuyenVNP = false;
            IsChuyenHNI = false;
        }

        public class PhongBanValueId
        {
            public static readonly int PHONG_CSKH_VNP = 60;
        }
    }
}