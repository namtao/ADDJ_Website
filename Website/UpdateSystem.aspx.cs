using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADDJ.Core;

namespace Website
{
    public partial class UpdateSystem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Header cần xử lý
            Page.Header.DataBind();

            if (!IsPostBack)
            {
                if (Config.UpdateSystem == "1")
                {
                    string time = ConfigurationManager.AppSettings["UpdateSystemTimeUp"];
                    if (!string.IsNullOrEmpty(time)) liMessage.Text += string.Format(" Vui lòng trở lại sau: {0}", time);
                }
                else // Chuyển trang khi không còn cập nhật
                {
                    Response.Redirect("~/");
                }

                // Khóa chủ động vượt Update => Lưu vào Cookie
                if (!string.IsNullOrEmpty(Request.QueryString["IsTest"]))
                {
                    HttpCookie cookie = new HttpCookie("IsTest");
                    cookie.Value = "1";
                    Response.Cookies.Add(cookie);
                }

                // Kiểm tra Session vượt Update
                HttpCookie isTest = Request.Cookies["IsTest"];
                if (isTest != null) // Nếu tồn tại 
                {
                    if (isTest.Value == "1") Response.Redirect("~/"); // Về trang chủ
                }
            }

        }
    }
}