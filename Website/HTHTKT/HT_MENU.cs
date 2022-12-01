using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class HT_MENU_F
    {
        public int ID { get; set; }

        public int? STT { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

        public string Name2 { get; set; }

        public string Name3 { get; set; }

        public string Link { get; set; }

        public int? Display { get; set; }

        public int? MenuType { get; set; }

    }
    public class HT_MENU0
    {
        public int ID { get; set; }

        public int? STT { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

        public string Name2 { get; set; }

        public string Name3 { get; set; }

        public string Link { get; set; }

        public int? Display { get; set; }

        public int? MenuType { get; set; }
        public bool HasChild { get; set; }

    }
    public class HT_MENU
    {
        public int ID { get; set; }

        public int? STT { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

        public string Name2 { get; set; }

        public string Name3 { get; set; }

        public string Link { get; set; }

        public int? Display { get; set; }

        public int? MenuType { get; set; }
        public bool HasChild { get; set; }

        public bool UserRead { get; set; }

        public bool UserEdit { get; set; }

        public bool UserDelete { get; set; }

        public bool Other1 { get; set; }

        public bool Other2 { get; set; }

    }
}