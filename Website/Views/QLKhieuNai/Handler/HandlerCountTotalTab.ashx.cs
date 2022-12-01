using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerCountTotalTab
    /// </summary>
    public class HandlerCountTotalTab : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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