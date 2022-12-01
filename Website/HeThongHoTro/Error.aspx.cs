using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Website.HeThongHoTro
{
    public partial class Error : System.Web.UI.Page
    {
        public string strError = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            strError = Server.UrlDecode(Request.QueryString["code"]);
        }
    }
}