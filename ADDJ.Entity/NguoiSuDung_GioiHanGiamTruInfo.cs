using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public class NguoiSuDung_GioiHanGiamTruInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TenTruyCap { get; set; }
        public decimal MocKhauTruMax { get; set; }
        public decimal MocKhauTruMin { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CDate { get; set; }
        public string CUser { get; set; }
        public DateTime LDate { get; set; }
        public string LUser { get; set; }
    }
}
