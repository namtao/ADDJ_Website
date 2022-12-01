using System.Web;
using ADDJ.Admin;
using ADDJ.Core;

namespace Website.AppCode
{
    public class MyControl : System.Web.UI.UserControl
    {
        public bool IsLogin
        {
            get
            {
                return HttpContext.Current.Session[Constant.SessionNameAccountAdmin] != null &&
                HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() != string.Empty;
            }
        }

        public AdminInfo ContextUser
        {
            get
            {
                if (HttpContext.Current.Session[Constant.SessionNameAccountAdmin] == null ||
              HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() == string.Empty)
                {
                    return null;
                }
                return (AdminInfo)HttpContext.Current.Session[Constant.SessionNameAccountAdmin];
            }
        }
    }
}