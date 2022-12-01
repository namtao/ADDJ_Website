using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT.Entity
{
    public class ViewTimelineXuLyYCHT
    {
        public int ID { get; set; }
        public string NGAYXULY { get; set; }
        public string TENHETHONG { get; set; }
        public string TEN_LUONG { get; set; }
        public string LINHVUC { get; set; }
        public string NOIDUNG_YEUCAU { get; set; }
        public int ID_NODE_LUONG_HOTRO { get; set; }
        public string NOIDUNGXULY { get; set; }
        public int? ID_DONVI_TO { get; set; }
        public string DONVIXULY { get; set; }
        public string NGUOIXULY { get; set; }
        public string TEN_NGUOIXULY { get; set; }
        public string DT_NGUOIXULY { get; set; }
    }
}