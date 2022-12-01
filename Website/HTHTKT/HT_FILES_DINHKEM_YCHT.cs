using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_FILES_DINHKEM_YCHT
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_YEUCAU_HOTTRO_HT { get; set; }

        public int? ID_XULY_YCHT { get; set; }

        public string TENFILE { get; set; }

        public int? TRANGTHAI { get; set; }

        public DateTime? NGAYTAO { get; set; }

        public string TENFILETAIVE { get; set; }
    }
}