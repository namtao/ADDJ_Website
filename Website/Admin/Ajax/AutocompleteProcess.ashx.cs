using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADDJ.Admin;
using Website.AppCode;
using System.Web.SessionState;

namespace Website.admin.Ajax
{
    /// <summary>
    /// Summary description for AutocompleteProcess
    /// </summary>
    public class AutocompleteProcess : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["type"] ?? context.Request.QueryString["type"];

            switch (type)
            {
                case "AutoCompleteNguoiSuDung":
                    AutoCompleteNguoiSuDung(context);
                    break;
                case "AutoCompletePhongBan":
                    AutoCompletePhongBan(context);
                    break;  
            }
        }

        private void AutoCompleteNguoiSuDung(HttpContext context)
        {
            var admin = LoginAdmin.AdminLogin();
            if (admin == null)
            {
                context.Response.Write("[]");
            }
            else
            {
                var obj = ServiceFactory.GetInstanceNguoiSuDung();
                var lst = obj.Suggestion(admin.KhuVucId, admin.DoiTacId, admin.NhomNguoiDung, context.Request.QueryString["q"]);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
        }

        private void AutoCompletePhongBan(HttpContext context)
        {
            var admin = LoginAdmin.AdminLogin();
            if (admin == null)
            {
                context.Response.Write("[]");
            }
            else
            {
                var obj = ServiceFactory.GetInstancePhongBan();
                var lst = obj.Suggestion(context.Request.QueryString["q"]);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}