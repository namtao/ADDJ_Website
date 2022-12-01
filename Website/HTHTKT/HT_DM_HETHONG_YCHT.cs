using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_DM_HETHONG_YCHT
    {
        public int ID { get; set; }

        public string TENHETHONG { get; set; }
        public string MA_HETHONG { get; set; }

        public string MOTA { get; set; }
        public string MUCDO { get; set; }

        public bool? TRANGTHAI { get; set; }

        public DateTime? NGAYTAO { get; set; }

        public string NGUOITAO { get; set; }
    }
}