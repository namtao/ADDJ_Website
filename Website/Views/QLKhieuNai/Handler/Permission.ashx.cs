using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode.Controller;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for Permission
    /// </summary>
    /// 

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Permission : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string key = context.Request.QueryString["key"].ToString();
            System.Web.Script.Serialization.JavaScriptSerializer JSSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";


            if (!string.IsNullOrEmpty(context.Request.QueryString["key"]))
            {
                AdminInfo infoUser = (AdminInfo)context.Session[Constant.SessionNameAccountAdmin];
                if (infoUser != null)
                {                    
                    context.Response.Write(JSSerializer.Serialize(ProcessData(context.Request.QueryString["key"],context)));
                }
            }
        }
        private string ProcessData(string key, HttpContext context)
        {
            string strValue = "";
            switch (key)
            {
                case "1":
                    string valuePermission = context.Request.QueryString["valuePermission"];
                    strValue = CheckQuyenUser(valuePermission);
                    break;
            }
            return strValue;
        }
        private string CheckQuyenUser(string valuePermission)
        {
            string strValue = "";
            if (BuildPhongBan_Permission.CheckPermission(Convert.ToInt32(valuePermission)))
                strValue = valuePermission;
            else
                strValue = "0";
            return strValue;
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