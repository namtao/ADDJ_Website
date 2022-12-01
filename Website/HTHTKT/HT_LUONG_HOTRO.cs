using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_LUONG_HOTRO
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }

        public string TEN_LUONG { get; set; }

        public bool? TRANGTHAI { get; set; }

        public string MOTA { get; set; }

        public int? SOBUOC { get; set; }

    }
    public class HT_LUONG_HOTRO2
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }
        public string TENHETHONG { get; set; }

        public string TEN_LUONG { get; set; }

        public bool? TRANGTHAI { get; set; }

        public string MOTA { get; set; }

        public int? SOBUOC { get; set; }

    }
}