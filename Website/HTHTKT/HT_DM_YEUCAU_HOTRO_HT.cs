using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_DM_YEUCAU_HOTRO_HT
    {
        public int ID { get; set; }

        public int? ID_HETHONG_YCHT { get; set; }

        public int? ID_CAYTHUMUC_YCHT { get; set; }

        public int? ID_MUCDO_SUCO { get; set; }

        public int? ID_LUONG_HOTRO { get; set; }

        public int? ID_DONVI { get; set; }

        public string MA_YEUCAU { get; set; }

        public string NOIDUNG_YEUCAU { get; set; }

        public DateTime? NGAYTAO { get; set; }

        public string NGUOITAO { get; set; }

        public int? TRANGTHAI { get; set; }

        public string NOIDUNG_XL_DONG_HOTRO { get; set; }

        public string SODIENTHOAI { get; set; }
    }
    public class HT_DM_YEUCAU_HOTRO_HT2
    {
        public int ID { get; set; }

        public string NOIDUNG_YEUCAU { get; set; }

        public string NOIDUNG_XL_DONG_HOTRO { get; set; }

        public int? TRANGTHAI { get; set; }

        public string NGUOITAO { get; set; }

        public string TEN_LUONG { get; set; }

        public string MOTA { get; set; }

        public int? SOBUOC { get; set; }

        public string TENHETHONG { get; set; }

        public string LOAI { get; set; }

        public string LINHVUCCHUNG { get; set; }

        public string LINHVUCCON { get; set; }

        public string GHICHU { get; set; }
    }
    public class HT_DM_YEUCAU_HOTRO_HT3
    {
        public int ID { get; set; }
        public string SODIENTHOAI { get; set; }
        public string TENMUCDO { get; set; }
        public string MA_YEUCAU { get; set; }

        public string NOIDUNG_YEUCAU { get; set; }

        public string NOIDUNG_XL_DONG_HOTRO { get; set; }

        public int? TRANGTHAI { get; set; }

        public string NGUOITAO { get; set; }

        public string TEN_LUONG { get; set; }

        public string MOTA { get; set; }

        public int? SOBUOC { get; set; }

        public string TENHETHONG { get; set; }

        public string LINHVUC { get; set; }

        public string GHICHU { get; set; }

        public int? ToltalRecords { get; set; }
    }
}