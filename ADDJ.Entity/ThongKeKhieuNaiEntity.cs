using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public class ThongKeKhieuNaiEntity
    {
        public int LoaiKhieuNaiId { get; set; }
        public string LoaiKhieuNai { get; set; }
        public int LinhVucChungId { get; set; }
        public string LinhVucChung { get; set; }
        public int SoLuong { get; set; }

        public ThongKeKhieuNaiEntity()
        {
            LoaiKhieuNaiId = 0;
            LoaiKhieuNai = string.Empty;
            LinhVucChungId = 0;
            LinhVucChung = string.Empty;
            SoLuong = 0;
        }
    }
}
