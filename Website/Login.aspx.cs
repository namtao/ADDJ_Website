using System;
using ADDJ.Core;
using System.Web.UI;
using System.Web;

namespace Website
{
    public partial class Login : Page
    {
        protected string error = string.Empty;
        protected string sso_login = string.Empty;
        protected string sso_url = string.Empty;
        protected string sso_service = string.Empty;
        protected string UrlReturn
        {
            get; private set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (AppSetting.IsLoginLocal) // Cấu hình ép buộc login local
            {
                string returnUrl = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])) returnUrl = string.Format("?ReturnUrl={0}", Request.QueryString["ReturnUrl"]);
                Response.Redirect("~/LoginAI.aspx" + returnUrl);
            }
            if (Config.IsLocal) Response.Redirect("LoginAI.aspx");

            sso_login = System.Configuration.ConfigurationManager.AppSettings["sso_login"];
            sso_url = System.Configuration.ConfigurationManager.AppSettings["sso_url"];
            sso_service = System.Configuration.ConfigurationManager.AppSettings["sso_service"];
            string sso_KeyId = System.Configuration.ConfigurationManager.AppSettings["Key_Id"];

            string urlReturn = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(urlReturn)) // Null => Trang chủ
                urlReturn = string.Concat(Request.Url.Scheme, "://", Request.Url.Authority);
            else
                urlReturn = string.Concat(Request.Url.Scheme, "://", Request.Url.Authority, urlReturn);

            UrlReturn = string.Format("?KeyId={0}&Url={1}", sso_KeyId, HttpUtility.UrlEncode(urlReturn));

        }
    }
}