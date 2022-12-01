using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    [Serializable]
    public class KhieuNai_TruyVanInfo
    {
      
        public string TenTruong { get; set; }
        public string KieuDuLieu { get; set; }
        public string PhepToan { get; set; }
        public string GiaTri { get; set; }        

        public KhieuNai_TruyVanInfo()
        {
            
            TenTruong = string.Empty;
            KieuDuLieu = string.Empty;
            PhepToan = string.Empty;
            GiaTri = string.Empty;           

        }
    }
}
