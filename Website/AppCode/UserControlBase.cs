using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Website.AppCode
{
    public class UserControlBase : UserControl
    {
        public string AppPath { get { return HttpContext.Current.Request.ApplicationPath; } }

    }
}