//using SimpleExcelImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_EXCELFILE
    {
        //[ExcelImport("TONGCT", order = 1)]
        public string TONGCT { get; set; }
        //[ExcelImport("KHUVUC", order = 2)]
        public string KHUVUC { get; set; }
        //[ExcelImport("DONVI", order = 3)]
        public string DONVI { get; set; }

        //[ExcelImport("BOPHAN", order = 4)]
        public string BOPHAN { get; set; }

        //[ExcelImport("CANHAN", order = 5)]
        public string CANHAN { get; set; }

        //[ExcelImport("HOTEN", order = 6)]
        public string HOTEN { get; set; }

        //[ExcelImport("DIENTHOAI", order = 7)]
        public string DIENTHOAI { get; set; }
        //[ExcelImport("TENTRUYCAP", order = 8)]
        public string TENTRUYCAP { get; set; }
        //[ExcelImport("EMAIL", order = 9)]
        public string EMAIL { get; set; }
        //[ExcelImport("QUYEN", order = 10)]
        public string QUYEN { get; set; }

    }
}