using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ADDJ.Core
{
    public class AppSetting
    {
        private AppSetting() { }

        public static bool IsCatchEx => Convert.ToBoolean(ConfigurationManager.AppSettings["IsCatchEx"]);

        public static bool IsLoginLocal => Convert.ToBoolean(ConfigurationManager.AppSettings["IsLoginLocal"]);

        public static string ViewStateConnectionString => ConfigurationManager.AppSettings["ViewStateConnectionString"];

        public static string PasswordLoginDefault => ConfigurationManager.AppSettings["PLoginDefault"];
    }
}