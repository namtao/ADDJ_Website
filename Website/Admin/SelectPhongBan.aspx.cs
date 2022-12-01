using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.GQKN.Entity;

namespace Website.admin
{    
    public partial class SelectPhongBan : System.Web.UI.Page
    {       

        protected void Page_Load(object sender, EventArgs e)
        {
            BindDropDownList();
        }

        private void BindDropDownList()
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnOkay_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "onload", "onSuccess();", true);
        }
    }
}