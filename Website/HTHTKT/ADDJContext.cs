using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Website.HTHTKT
{
    public class ADDJContext: DbContext
    {
        public ADDJContext(): base("name=HTKTKT")
        {
            this.Configuration.UseDatabaseNullSemantics = false;
        }

        public DbSet<HT_MUCDO_SUCO> HT_MUCDO_SUCOs { get; set; }
        public DbSet<HT_DM_HETHONG_YCHT> HT_HE_THONG_HOTROs { get; set; }
        public DbSet<HT_CAYTHUMUC_YCHT> HT_CAYTHUMUC_YCHTs { get; set; }
        public DbSet<HT_DM_YEUCAU_HOTRO_HT> HT_CHITIET_HOTROs { get; set; }
        public DbSet<HT_XULY_YEUCAU_HOTRO> HT_CHITIET_XULY_HOTROs { get; set; }
        public DbSet<HT_LUONG_HOTRO> HT_LUONG_HOTROs { get; set; }
        public DbSet<HT_NODE_LUONG_HOTRO> HT_NODE_LUONG_HOTROs { get; set; }
    }
}