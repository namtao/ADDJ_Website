using System;
using System.IO;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.Adapters;
using ADDJ.Core;
using Website.AppCode;

/// <summary>
/// Summary description for PageStateAdapter
/// </summary>
public class PageStateAdapter : PageAdapter
{
    public enum StateStorageTypes { Default, Cache, Session, InDatabase }

    /// <summary>
    /// Returns an object that is used by the Web page to maintain the control and view states.
    /// </summary>
    /// <returns>
    /// An object derived from <see cref="T:System.Web.UI.PageStatePersister"/> that supports creating and extracting the 
    /// combined control and view states for the <see cref="T:System.Web.UI.Page"/>.
    /// </returns>
    public override PageStatePersister GetStatePersister()
    {
        PageViewStateStorageAttribute psa =
            Attribute.GetCustomAttribute(Page.GetType(), typeof(PageViewStateStorageAttribute), true) as PageViewStateStorageAttribute ??
            new PageViewStateStorageAttribute(StateStorageTypes.Default);

        PageStatePersister psp;

        //switch (psa.StorageType)
        //{
        //    case StateStorageTypes.Session:
        //        psp = new SessionPageStatePersister(Page);
        //        break;
        //    case StateStorageTypes.Cache:
        //        psp = new CachePageStatePersister(Page);
        //        break;
        //    case StateStorageTypes.InDatabase:
        //        psp = new DatabasePageStatePersister(Page);
        //        break;
        //    case StateStorageTypes.Default:
        //        psp = new DatabasePageStatePersister(Page);
        //        // psp = new HiddenFieldPageStatePersister(Page);
        //        break;
        //    default:
        //        // If Default = Database
        //        psp = new HiddenFieldPageStatePersister(Page);
        //        break;

        //}
        //return psp;
        return null;
    }

    /// <summary>
    /// Attribute to be applied to a page object.  When applied, determins how the viewstate storage is treated for that individual page.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PageViewStateStorageAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewStateStorageAttribute"/> class.
        /// </summary>
        /// <param name="stateStorageType">Type of the state storage.</param>
        public PageViewStateStorageAttribute(StateStorageTypes stateStorageType)
        {
            StorageType = stateStorageType;
        }
        public StateStorageTypes StorageType { get; } = StateStorageTypes.Default;
    }

}
public class DatabasePageStatePersister : PageStatePersister
{
    private const string VSKEY = "__VSKEY";
    private const string VSPREFIX = "Viewstate_";

    public DatabasePageStatePersister(Page page) : base(page) { }
    public override void Load()
    {
        if (!Page.IsPostBack) return;   // We don't want to load up anything if this is an inital request

        string vsKey = Page.Request.Form[VSKEY];

        // Sanity Checks
        if (string.IsNullOrEmpty(vsKey)) throw new ViewStateException();
        if (!vsKey.StartsWith(VSPREFIX)) throw new ViewStateException();

        IStateFormatter frmt = StateFormatter;
        string state = string.Empty;

        state = (string)SqlHelper.ExecuteScalar(AppSetting.ViewStateConnectionString, "SqlViewStateProvider_GetViewState", vsKey);

        if (string.IsNullOrEmpty(state)) return;

        Pair statePair = frmt.Deserialize(state) as Pair;

        if (statePair == null) return;

        ViewState = statePair.First;
        ControlState = statePair.Second;
    }
    public override void Save()
    {
        if (ViewState != null || ControlState != null)
        {
            if (Page.Session == null) throw new InvalidOperationException("Session is required for CachePageStatePersister (SessionID -> Key)");

            string vsKey;
            // string cacheFile;


            // Get Method
            if (!Page.IsPostBack) // create a unique cache file and key based on this user's session and page instance (time)
            {
                string sessionId = Page.Session.SessionID;
                string pageUrl = Page.Request.Path;

                // Render key
                vsKey = string.Format("{0}{1}_{2}_{3}", VSPREFIX, pageUrl, sessionId, DateTime.Now.Ticks);
            }
            else
            {
                // Get key
                vsKey = Page.Request.Form[VSKEY];
                if (string.IsNullOrEmpty(vsKey)) throw new ViewStateException();
            }

            IStateFormatter frmt = StateFormatter;
            string state = frmt.Serialize(new Pair(ViewState, ControlState));

            SqlHelper.ExecuteNonQuery(AppSetting.ViewStateConnectionString, "SqlViewStateProvider_SaveViewState", vsKey, state);

            Page.ClientScript.RegisterHiddenField(VSKEY, vsKey);
        }
    }
}
public class CachePageStatePersister : PageStatePersister
{

    private const string VSKEY = "__VSKEY";
    private const string VSPREFIX = "Viewstate_";
    private const string CACHEFOLDER = "~/App_Data/Cache";

    public CachePageStatePersister(Page page) : base(page) { }


    /// <summary>
    /// Overridden by derived classes to deserialize and load persisted state information when 
    /// a <see cref="T:System.Web.UI.Page" /> object initializes its control hierarchy.
    /// <para>
    /// 1. Get the viewstate key from the hidden form field
    /// 2. Retrieved the serialized viewstate object from the physical cache
    /// 3. deserialize the vs into a pair object
    /// 4. populate our viewstate and control state objects in the page
    /// </para>
    /// </summary>
    public override void Load()
    {
        if (!Page.IsPostBack) return;   // We don't want to load up anything if this is an inital request

        string vsKey = Page.Request.Form[VSKEY];

        // Sanity Checks
        if (string.IsNullOrEmpty(vsKey)) throw new ViewStateException();
        if (!vsKey.StartsWith(VSPREFIX)) throw new ViewStateException();

        IStateFormatter frmt = StateFormatter;
        string state = string.Empty;

        string fileName = Page.Cache[vsKey] as string;
        if (!string.IsNullOrEmpty(fileName))
            if (File.Exists(fileName))
                using (StreamReader sr = File.OpenText(fileName))
                    state = sr.ReadToEnd();

        if (string.IsNullOrEmpty(state)) return;

        Pair statePair = frmt.Deserialize(state) as Pair;

        if (statePair == null) return;

        ViewState = statePair.First;
        ControlState = statePair.Second;
    }

    /// <summary>
    /// Overridden by derived classes to serialize persisted state information when 
    /// a <see cref="T:System.Web.UI.Page" /> object is unloaded from memory.
    /// <para>
    /// 1. Save our viewstate and controlstate to a new pair object
    /// 2. Serialize the pair object to a string
    /// 3. Save the string to persistant storage (cache folder)
    /// 4. save the pointer to the file in Page Cache
    /// 5. Save the cache pointer to a hidden field object on the page
    /// </para>
    /// </summary>
    public override void Save()
    {
        if (ViewState != null || ControlState != null)
        {
            if (Page.Session == null) throw new InvalidOperationException("Session is required for CachePageStatePersister (SessionID -> Key)");

            string vsKey;
            string cacheFile;


            // Get Method
            if (!Page.IsPostBack) // create a unique cache file and key based on this user's session and page instance (time)
            {
                string sessionId = Page.Session.SessionID;
                string pageUrl = Page.Request.Path;
                vsKey = string.Format("{0}{1}_{2}_{3}", VSPREFIX, pageUrl, sessionId, DateTime.Now.Ticks);

                string cachePath = Page.MapPath(CACHEFOLDER);
                if (!Directory.Exists(cachePath)) Directory.CreateDirectory(cachePath);
                cacheFile = Path.Combine(cachePath, BuildFileName());
            }

            // Method Post
            else    // get our vs key from the page, re use it, and the cache file (pulled from page.cache)
            {
                vsKey = Page.Request.Form[VSKEY];
                if (string.IsNullOrEmpty(vsKey)) throw new ViewStateException();
                cacheFile = Page.Cache[vsKey] as string;
                if (string.IsNullOrEmpty(cacheFile)) throw new ViewStateException();
            }

            IStateFormatter frmt = StateFormatter;
            string state = frmt.Serialize(new Pair(ViewState, ControlState));
            using (StreamWriter sw = File.CreateText(cacheFile)) sw.Write(state);

            Page.Cache.Add(vsKey, cacheFile, null, DateTime.Now.AddMinutes(Page.Session.Timeout), Cache.NoSlidingExpiration, CacheItemPriority.Low, ViewStateCacheRemoveCallback);

            Page.ClientScript.RegisterHiddenField(VSKEY, vsKey);
        }
    }

    /// <summary>
    /// Call back method for Page.Cache to remove the persisted storage of the cache object from the cache folder
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="reason"></param>
    public static void ViewStateCacheRemoveCallback(string key, object value, CacheItemRemovedReason reason)
    {
        string cacheFile = value as string;
        if (!string.IsNullOrEmpty(cacheFile))
            if (File.Exists(cacheFile))
                File.Delete(cacheFile);
    }

    /// <summary>
    /// Builds a valid file name for our persistant cache storage based on sessionid and requested path
    /// </summary>
    /// <returns><see cref="T:System.String" /></returns>
    private string BuildFileName()
    {
        string fileName = string.Format("{0}{1}__{2}__{3}", VSPREFIX, Page.Session.SessionID, Page.Request.Path, DateTime.Now.Ticks);
        char[] badChars = Path.GetInvalidPathChars();
        foreach (char c in badChars)
            fileName = fileName.Replace(c, '-');
        badChars = Path.GetInvalidFileNameChars();
        foreach (char c in badChars)
            fileName = fileName.Replace(c, '_');
        fileName = string.Concat(fileName, ".cache");
        return fileName;
    }
}
