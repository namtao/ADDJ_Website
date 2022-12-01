using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using ADDJ.Core;
using ADDJ.Admin;
using Website.AppCode;
using System.Text;

namespace Website.JS.Admin
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>    
    public class SendMessageHander : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["type"] ?? context.Request.QueryString["type"];

            switch (type)
            { 
                case "ChangeParentMenu":
                    ChangeParentMenu(context);
                    break;                
            }
        }

        public void ChangeParentMenu(HttpContext context)
        {
            var data = context.Request.Form;
            var parentId = ConvertUtility.ToString(data["pareintId"]);

            int result = 0;
            try
            {
                if(parentId.Equals("0"))
                    result = ServiceFactory.GetInstanceMenu().GetSTTParent();
                else
                    result = ServiceFactory.GetInstanceMenu().GetSTTByMenu(Convert.ToInt32(parentId));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            context.Response.Write(result);
            context.Response.End();
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
