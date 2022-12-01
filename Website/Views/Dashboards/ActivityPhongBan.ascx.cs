using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.Dashboards
{
    public partial class ActivityPhongBan : System.Web.UI.UserControl
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltPhongBan.Text = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(LoginAdmin.AdminLogin().PhongBanId);

                BindContent(1);
            }
        }

        private void BindContent(int pIndex)
        {
            btShowMore.Attributes.Add("page", pIndex.ToString());
            int total = 0;
            var lst = ServiceFactory.GetInstanceKhieuNai_Log().GetPaged("", "", "CDate DESC", pIndex, 10, ref total);

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = null;
            var nDate = "";
            foreach (var item in lst)
            {
                if (item.CDate.ToString("yyyyMMdd").Equals(nDate))
                {
                    sb2.AppendFormat("<li class=\"activity-item\">{0} {1} <a href='#'>{2}</a></li>", item.CUser.ToUpper(), item.ThaoTac, GetDataImpl.GetMaTuDong("PA", item.KhieuNaiId, 10));
                }
                else
                {
                    if (sb2 != null)
                    {
                        sb2.AppendLine("</ul>");
                        sb.AppendLine(sb2.ToString());    
                    }                    

                    sb2 = new StringBuilder();
                    sb2.AppendLine("<ul>");
                    sb2.AppendFormat("<li class=\"activity-item\">{0} {1} <a href='#'>{2}</a></li>", item.CUser.ToUpper(), item.ThaoTac, GetDataImpl.GetMaTuDong("PA", item.KhieuNaiId, 10));
                    sb.AppendFormat("<h3 class=\"timestamp\">{0}</h3>", item.CDate.ToString());
                    nDate = item.CDate.ToString("yyyyMMdd");
                }
            }
            if (sb2.Length > 0)
            {
                sb2.AppendLine("</ul>");
                sb.AppendLine(sb2.ToString());
            }
            if (pIndex != 1)
            {
                ltContent.Text += sb.ToString();
            }            
            else
            {
                ltContent.Text = sb.ToString();
            }
        }

        protected void btShowMore_Click(object sender, EventArgs e)
        {
            var pIndex = ConvertUtility.ToInt32(btShowMore.Attributes["page"], 1);
            pIndex++;

            BindContent(pIndex);
        }
    }
}