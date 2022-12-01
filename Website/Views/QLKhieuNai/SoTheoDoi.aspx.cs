using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;

namespace Website.Views.QLKhieuNai
{
    public partial class SoTheoDoi : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                if (Request.QueryString["ctrl"] != null && !Request.QueryString["ctrl"].Equals(""))
                {
                    System.Web.UI.Control usc = this.LoadControl("/Views/QLKhieuNai/UserControls/" + Request.QueryString["ctrl"] + ".ascx");
                    placeHolder.Controls.Add(usc);
                }
                else
                {
                    System.Web.UI.Control usc = this.LoadControl("/Views/QLKhieuNai/UserControls/KNSoTheoDoi.ascx");
                    placeHolder.Controls.Add(usc);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Console.WriteLine("Xảy ra lỗi khi tải trang :" + exc);
            }
        }
    }
}