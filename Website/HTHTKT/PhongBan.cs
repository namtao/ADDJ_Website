using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class PhongBan
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? LoaiPhongBanId { get; set; }

        public int? KhuVucId { get; set; }

        public int? DoiTacId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Sort { get; set; }

        public int? Cap { get; set; }

        public int? Status { get; set; }

        public string SortOrder { get; set; }

        public bool? IsDinhTuyenKN { get; set; }

        public short? DefaultHTTN { get; set; }

        public bool? IsChuyenTiepKN { get; set; }

        public bool? IsChuyenVNP { get; set; }

        public bool? IsChuyenHNI { get; set; }

        public string CUser { get; set; }

        public DateTime? CDate { get; set; }

        public string LUser { get; set; }

        public DateTime? LDate { get; set; }
    }
}