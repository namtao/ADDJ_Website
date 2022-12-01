using System;
using System.Web;
using Website.AppCode;

namespace Website
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Header.DataBind();

                string backurl = string.Format("<div style=\"margin-top: 10px;\"> Về <a href=\"/\">trang chủ</a>");
                if (!string.IsNullOrEmpty(Request.QueryString["BackUrl"]))
                    backurl += string.Format(" - hoặc <a href=\"{0}\">trở lại</a></div>", HttpUtility.UrlDecode(Request.QueryString["BackUrl"]));

                switch (Request.QueryString["Code"])
                {
                    case "1":
                        bool IsConnected = Common.IsConnectDBOK();
                        if (!IsConnected)
                            liMessage.Text = "Không thực hiện được kết nối với dữ liệu";

                        // Kết nối lại được thì về trang chủ thôi
                        else
                            Response.Redirect("~/");
                        break;
                    case "2":
                        liMessage.Text = "Có lỗi xảy ra, vui lòng kiểm tra lại!";
                        liMessage.Text += backurl;
                        break;
                    default:
                        liMessage.Text = "Không xác định được nguyên nhân lỗi";
                        liMessage.Text += backurl;
                        break;
                }
            }
        }
    }
}