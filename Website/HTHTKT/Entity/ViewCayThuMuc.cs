using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT.Entity
{
    public class ViewCayThuMuc
    {
        public int ID { get; set; }

        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }

        public bool HasChild { get; set; }
    }

    public class ViewCayThuMucDoiTac
    {
        public int ID { get; set; }
        public string MaDonVi { get; set; }

        public string TenDoiTac { get; set; }

        public bool HasChild { get; set; }
        public bool NotAllowChecked { get; set; }
    }
    public class ViewCayThuMucDoiTac2
    {
        public int ID { get; set; }
        public string MaDonVi { get; set; }

        public string TENDONVI { get; set; }

        public bool HasChild { get; set; }
        public bool NotAllowChecked { get; set; }
    }

    public class ViewCayThuMucYeuCauHT
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }
        public string MA_HETHONG { get; set; }

        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }

        public int? ID_CHA { get; set; }

        public string GHICHU { get; set; }

        public bool? TRANGTHAI { get; set; }

        public DateTime? NGAYCAPNHAT { get; set; }
        public bool HasChild { get; set; }
    }
}