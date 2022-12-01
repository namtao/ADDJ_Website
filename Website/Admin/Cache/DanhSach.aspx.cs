using ADDJ.Core.Provider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Admin.Cache
{
    public partial class DanhSach : PageBase
    {
        private class CacheObject
        {
            public string Key { get; set; }
            public string TypeName { get; set; }
            public object Value { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCache();
            }
        }

        private void BindCache()
        {
            // Session.Add("Key", "Nội dung");
            // IEnumerable<string> ss = Session.Cast<string>();

            // string ssKey = ss.SingleOrDefault<string>();
            // object ssValue = Session[ssKey];

            List<CacheObject> listCache = new List<CacheObject>();
            IEnumerable<DictionaryEntry> values = Cache.Cast<DictionaryEntry>();
            foreach (DictionaryEntry item in values)
            {
                string key = (string)item.Key;
                string type = item.Value.GetType().Name;

                CacheObject cacheItem = new CacheObject();
                cacheItem.Key = key;
                cacheItem.TypeName = type;
                cacheItem.Value = item.Value;
                listCache.Add(cacheItem);

            }
            GrvViews.DataSource = listCache;
            GrvViews.DataBind();

            // Customer cache
            List<CacheObject> lstCustomize = new List<CacheObject>();
            foreach (var item in (Dictionary<string, object>)CacheProvider.GetAllCache())
            {
                string key = (string)item.Key;
                string type = item.Value.GetType().Name;

                CacheObject cacheItem = new CacheObject();
                cacheItem.Key = key;
                cacheItem.TypeName = type;
                cacheItem.Value = item.Value;
                lstCustomize.Add(cacheItem);
            }
            GrvViewCus.DataSource = lstCustomize;
            GrvViewCus.DataBind();
        }

    }
}