using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    [Serializable]
    public class LoiKhieuNaiInfo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string MaLoi { get; set; }
        public string TenLoi { get; set; }
        public int ThuTu { get; set; }
        public int Cap { get; set; }
        public bool HoatDong { get; set; }
        public int Loai { get; set; }
        public int TuNgay { get; set; }
        public int DenNgay { get; set; }
        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }
        public string CUser { get; set; }
        public string LUser { get; set; }



        public class LoiKhieuNaiValue
        {
            public static readonly int NGUYEN_NHAN_LOI_ID_KHAC = 39;
        }
    }
}
