using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_NODE_LUONG_HOTRO0
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_LUONG_HOTRO { get; set; }

        public int? ID_DONVI { get; set; }

        public int? BUOCXULY { get; set; }

        public DateTime? NGAYTAO { get; set; }

        public int? ToltalRecords { get; set; }

    }
    public class HT_NODE_LUONG_HOTRO
    {
        public int ID { get; set; }
        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_LUONG_HOTRO { get; set; }

        public int? ID_DONVI { get; set; }

        public int? BUOCXULY { get; set; }

        public DateTime? NGAYTAO { get; set; }

        public int? SOBUOC { get; set; } // tổng số bước của 1 luồng hỗ trợ
        public string TENDONVI { get; set; }
    }
    public class HT_NODE_LUONG_HOTRO2
    {
        public int ID { get; set; }
        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_LUONG_HOTRO { get; set; }

        public int? ID_DONVI { get; set; }

        public int? BUOCXULY { get; set; }

        public DateTime? NGAYTAO { get; set; }
    }

    public class HT_NODE_LUONG_HOTRO3
    {
        public int ID { get; set; }
        public int IDDonVi { get; set; }
        public string MaDonVi { get; set; }
        public string TenDonVi { get; set; }
    }
    public class HT_NODE_LUONG_HOTRO4
    {
        public int ID { get; set; }
        public int IDDonVi { get; set; }
        public string MADONVI { get; set; }
        public string TENDONVI { get; set; }
    }

    public class HT_NODE_LUONG_HOTRO6
    {
        public int ID { get; set; }
        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_LUONG_HOTRO { get; set; }

        public int? ID_DONVI { get; set; }

        public int? BUOCXULY { get; set; }

        public DateTime? NGAYTAO { get; set; }
    }
}