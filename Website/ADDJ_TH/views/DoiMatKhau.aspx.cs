using ADDJ.Admin;
using ADDJ.Sercurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.ADDJ_TH.views
{
    public partial class DoiMatKhau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        string strSql = string.Format(@"UPDATE HT_NGUOIDUNG SET MatKhau='{0}' WHERE Id='{1}'", Encrypt.MD5Admin(npsw.Text.Trim()), loginInfo.Id);
                        var rt = ctx.Database.ExecuteSqlCommand(strSql);
                    }
                    ASPxLabel1.Text = "Đổi mật khẩu thành công";
                }
            }
            catch (Exception ex)
            {
                ASPxLabel1.Text = ex.Message;
            }

        }
    }
}