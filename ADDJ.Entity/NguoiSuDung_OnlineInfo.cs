using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    [Serializable]
    public class NguoiSuDung_OnlineInfo
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public int NguoiSuDungId { get; set; }
        public string TenTruyCap { get; set; }
        public string Ip { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }

        public NguoiSuDung_OnlineInfo()
        {
            Id = 0;
            SessionId = string.Empty;
            NguoiSuDungId = 0;
            TenTruyCap = string.Empty;
            Ip = string.Empty;
            ThoiGianBatDau = DateTime.MaxValue;
            ThoiGianKetThuc = DateTime.MaxValue;
        }
    }
}
