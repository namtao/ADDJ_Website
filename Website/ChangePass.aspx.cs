using System;
using System.Web.UI;
using ADDJ.Core;
using ADDJ.Admin;
using System.Text.RegularExpressions;

public partial class ChangePass : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginAdmin.IsLoginAdmin();

        if (!IsPostBack)
        {
            AdminInfo objAdmin = (AdminInfo)Session[Constant.SessionNameAccountAdmin];
            if (objAdmin == null)
            {
                Response.Redirect(Config.LoginAdmin, false);
                return;
            }
            txtUsername.Text = objAdmin.Username;
            txtUsername.ReadOnly = true;
        }
    }

    private bool checkPassEqualSoVaChu(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] > 47 && str[i] < 58)
            {
                return true;
            }
        }
        return false;
    }

    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string mk = txtPass.Text;
        string reMk = txtRePass.Text;

        if (mk.Length < 6 || mk.Length > 20)
        {
            lblMsg.Text = "Mật khẩu từ 6 đến 20 ký tự.";
            lblMsg.Visible = true;
            return;
        }

        if (!checkPassEqualSoVaChu(mk))
        {
            lblMsg.Text = "Mật khẩu phải bao gồm cả số và chữ.";
            lblMsg.Visible = true;
            return;
        }

        if (mk.StartsWith(" ") || mk.EndsWith(" "))
        {
            lblMsg.Text = "Mật khẩu không bao gồm khoảng trắng ở đầu và cuối.";
            lblMsg.Visible = true;
            return;
        }

        if (!mk.Equals(reMk))
        {
            lblMsg.Text = "Nhập lại mật khẩu chưa đúng.";
            lblMsg.Visible = true;
            return;
        }


        string pattern = @"([a-z|A-Z|\d]+)$";
        Regex myRegex = new Regex(pattern);
        Match match = myRegex.Match(mk);
        if (match.Success)
        {
            AdminInfo objAdmin = (AdminInfo)Session[Constant.SessionNameAccountAdmin];
            objAdmin.Password = txtPass.Text;
            AdminImpl obj = new AdminImpl();
            obj.Update(objAdmin);
            Session[Constant.SessionNameAccountAdmin] = objAdmin;
            lblMsg.Text = "Đổi mật khẩu thành công.";
        }
        else
        {
            lblMsg.Text = "Mật khẩu chứa những ký tự không hợp lệ, xin kiểm tra lại.";
            lblMsg.Visible = true;
            return;
        }
    }
}
