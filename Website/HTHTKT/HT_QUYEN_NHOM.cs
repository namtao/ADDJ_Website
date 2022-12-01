using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_QUYEN_NHOM
    {
        public int? MenuID { get; set; }

        public int? GroupAdminID { get; set; }

        public bool? UserRead { get; set; }

        public bool? UserEdit { get; set; }

        public bool? UserDelete { get; set; }

        public bool? Other1 { get; set; }

        public bool? Other2 { get; set; }
        public bool? Other3 { get; set; }
        public bool? Other4 { get; set; }
    }
}