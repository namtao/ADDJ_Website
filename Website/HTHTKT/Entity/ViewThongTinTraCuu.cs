using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT.Entity
{
    public class ViewThongTinTraCuu
    {
        public int ID { get; set; }
        public string TENDONVI { get; set; }
        public string MADONVI { get; set; }
        public string SODIENTHOAI { get; set; }
        public string MA_YEUCAU { get; set; }
        public string NOIDUNG_YEUCAU { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string NGUOITAO { get; set; }
        public string TENDAYDU { get; set; }
        public string TENHETHONG { get; set; }
        public string MA_HETHONG { get; set; }
        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }
        public string TENMUCDO { get; set; }
        public string TEN_LUONG { get; set; }
        public string DONVIPHOIHOP { get; set; }
    }

    public class ViewThongTinTraCuu2
    {
        public int ID { get; set; }
        public string TENDONVI { get; set; }
        public string MADONVI { get; set; }
        public string SODIENTHOAI { get; set; }
        public string MA_YEUCAU { get; set; }
        public string NOIDUNG_YEUCAU { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string NGUOITAO { get; set; }
        public string TENDAYDU { get; set; }
        public string TENHETHONG { get; set; }
        public string MA_HETHONG { get; set; }
        public string LINHVUC { get; set; }
        public string MA_LINHVUC { get; set; }
        public string TENMUCDO { get; set; }
        public string TEN_LUONG { get; set; }
        public string DONVIPHOIHOP { get; set; }
        public string CountData { get; set; }
        public string NumberOfPage { get; set; }
        public int? ToltalRecords { get; set; }
    }
}