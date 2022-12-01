using ADDJ.Core.Provider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ADDJ.Core
{
    public static class ExtensionMethods
    {
        #region Extentions of DateTime
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0);
        }
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            DateTime nextMonth = dateTime.AddMonths(1);
            DateTime startOfNextMonth = nextMonth.StartOfMonth().AddDays(-1);
            return new DateTime(startOfNextMonth.Year, startOfNextMonth.Month, startOfNextMonth.Day, 23, 59, 59, 999);
        }
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return dateTime.AddYears(1).StartOfYear().AddDays(-1).EndOfDay();
        }
        public static string FormatSolrDateTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
        }
        public static string Format_ddMMyyyy(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
        public static string Format_ddMMyyy_HHmm(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }
        public static string Format_yyyyMMdd(this DateTime datTime)
        {
            return datTime.ToString("yyyyMMdd");
        }

        #endregion

        #region Extentions of CheckBoxList
        public static List<ListItem> GetSelectedItems(this ListControl checkBoxList)
        {
            return checkBoxList.Items.Cast<ListItem>().Where(v => v.Selected).ToList();
        }
        #endregion

        public static T Data<T>(this ICacheProvider cache, string cacheKey, Func<T> func)
        {
            // Kiểm cacheKey xem có hay không
            if (cache.Exists(cacheKey)) return (T)CacheProvider.Get(cacheKey);
            else // Nếu không có thì tiến hành thêm vào
            {
                T data = func();
                cache.Add(cacheKey, data);
                return data;
            }
        }
        public static T GetData<T>(this object obj, Func<T> method)
        {
            return method();
        }
        public static bool EqualsIgnorCase(this string soure, string strCompare)
        {
            if (string.IsNullOrEmpty(soure) && string.IsNullOrEmpty(strCompare)) return true;
            else if (!string.IsNullOrEmpty(soure) && !string.IsNullOrEmpty(strCompare)) return string.Equals(soure.ToLower(), strCompare.ToLower());
            else return false;
        }
    }
}