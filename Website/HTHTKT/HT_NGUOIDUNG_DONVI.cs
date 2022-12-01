using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_NGUOIDUNG_DONVI
    {
        public int ID { get; set; }

        public int? ID_NGUOIDUNG { get; set; }

        public int? ID_DONVI { get; set; }

        public int? TRANGTHAI { get; set; }

        public int? XOA { get; set; }

        public DateTime? NGAYTAO { get; set; }
    }
}