using ADDJ.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode.Controller;

namespace Website.admin
{
    public partial class managerCacheInbound : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btGetKey_Click(object sender, EventArgs e)
        {
            var obj = CustomCache.Get(txtKey.Text);
            if (obj != null)
                txtResult.Text = obj.ToString();
            else
                txtResult.Text = "null";
        }

        protected void btGetListKey_Click(object sender, EventArgs e)
        {
            var dic = CustomCache.GetList();
            txtResult.Text = string.Empty;
            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach(var item in dic)
            {
                i++;
                if (i > 20)
                    break;
                sb.Append(item.Key + ":" + item.Value).AppendLine();
            }
            txtResult.Text = sb.ToString();
        }

        protected void btRemoveKey_Click(object sender, EventArgs e)
        {
            if(CustomCache.Remove(txtKey.Text))
            {
                txtResult.Text = "Remove Ok";
            }
            else
            {
                txtResult.Text = "Not Exists";
            }
        }

        protected void btCountKey_Click(object sender, EventArgs e)
        {
            txtResult.Text = CustomCache.Count().ToString();
        }

        protected void btRemoveAllKey_Click(object sender, EventArgs e)
        {
            CustomCache.RemoveAll();
        }
    }
}