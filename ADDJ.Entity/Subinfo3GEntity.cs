using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIVietNam.GQKN.Entity
{
    public class Subinfo3GEntity
    {
        public int ErrorID { get; set; }

        public int Id { get; set; }
        public string ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Extension { get; set; }
        public string SubsType { get; set; }
        public string LoaiTB { get; set; }

        public string DungLuongCon { get; set; }
        public string DungLuongDaSD { get; set; }
        public string Ngay { get; set; }

        public Subinfo3GEntity()
        {
            ErrorID = 99999;
        }
    }
}
