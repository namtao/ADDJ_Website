using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADDJ.Entity;

namespace ADDJ.Impl
{
    public static class Extensions
    {
        public static string GetFormattedBreadCrumb(this PhongBanInfo pPhongBanInfo,
         
         string separator = ">>")
        {
            if (pPhongBanInfo == null)
                throw new ArgumentNullException("pPhongBanInfo");

            string result = string.Empty;

            //used to prevent circular references
            var alreadyProcessedPhongBanIds = new List<int>();

            while (pPhongBanInfo != null &&  //not null

                !alreadyProcessedPhongBanIds.Contains(pPhongBanInfo.Id)) //prevent circular references
            {
                result = String.IsNullOrEmpty(result) ? pPhongBanInfo.Name : string.Format("{0} {1} {2}", pPhongBanInfo.Name, separator, result);

                alreadyProcessedPhongBanIds.Add(pPhongBanInfo.Id);

                pPhongBanInfo = new PhongBanImpl().QLKN_PhongBangetByID(pPhongBanInfo.ParentId);

            }
            return result;
        }

    }
}
