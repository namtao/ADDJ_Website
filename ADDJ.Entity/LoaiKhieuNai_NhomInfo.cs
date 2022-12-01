using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    [Serializable]
    public class LoaiKhieuNai_NhomInfo
    {
        public int Id { get; set; }

        public string TenNhom { get; set; }

        public int Sort { get; set; }
    }
}
    