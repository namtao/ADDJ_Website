using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_CAYTHUMUC_YCHT
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }

        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }

        public int? ID_CHA { get; set; }

        public string GHICHU { get; set; }

        public bool? TRANGTHAI { get; set; }

        public DateTime? NGAYCAPNHAT { get; set; }
    }
    public class LOAI_TRONGCAY
    {
        public string LOAI { get; set; }
    }
    public class LINHVUCCHUNG_TRONGCAY
    {
        public string LINHVUCCHUNG { get; set; }
    }
    public class LINHVUCCON_TRONGCAY
    {
        public string LINHVUCCON { get; set; }
    }
    public class CHA_TRONGCAY
    {
        public int ID { get; set; }
        public string LINHVUCCON { get; set; }
    }
    public class DONVI_TIEPNHAN_XL_TRONGCAY
    {
        public string DONVI_TIEPNHAN_XL { get; set; }
    }
}