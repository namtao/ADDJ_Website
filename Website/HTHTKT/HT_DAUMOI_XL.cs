using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_DAUMOI_XL
    {
        public int ID { get; set; }

        public string HOTEN { get; set; }

        public int? ID_DONVI { get; set; }
        public string TENDONVI { get; set; }

        public int? ID_HETHONG { get; set; }
        public string TENHETHONG { get; set; }

        public string DIENTHOAI { get; set; }

        public string EMAIL { get; set; }
        public bool? TRANGTHAI { get; set; }

        public string NGUOITAO { get; set; }

        public DateTime? NGAYTAO { get; set; }
    }
}