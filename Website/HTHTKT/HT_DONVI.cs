using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_DONVI
    {
        public int ID { get; set; }

        public string MADONVI { get; set; }

        public string TENDONVI { get; set; }

        public int? ID_CHA { get; set; }

        public string MOTA { get; set; }

        public string LOAIDONVI { get; set; }

        public string GHICHU { get; set; }

        public int? XOA { get; set; }

        public string DIENTHOAI { get; set; }

        public string FAX { get; set; }

        public string DIACHI { get; set; }

        public int? TRANGTHAI { get; set; }

        public int? ID_TINHTHANH { get; set; }

        public DateTime? NGAYCAPNHAT { get; set; }
    }
    public class HT_DONVI2
    {
        public int ID { get; set; }

        public string MADONVI { get; set; }

        public string TENDONVI { get; set; }

        public int? ID_CHA { get; set; }

        public string MOTA { get; set; }

        public string LOAIDONVI { get; set; }

        public string GHICHU { get; set; }

        public int? XOA { get; set; }

        public string DIENTHOAI { get; set; }

        public string FAX { get; set; }

        public string DIACHI { get; set; }

        public int? TRANGTHAI { get; set; }

        public int? ID_TINHTHANH { get; set; }

        public DateTime? NGAYCAPNHAT { get; set; }
        public bool HasChild { get; set; }
    }

    public class HT_DONVI_VIEW
    {
        public int ID { get; set; }

        public string MADONVI { get; set; }

        public string TENDONVI { get; set; }
    }
}