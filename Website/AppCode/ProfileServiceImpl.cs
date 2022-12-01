using System;
using System.Collections.Generic;
using AIVietNam.Core;
using AIVietNam.Log.Impl;
using AIVietNam.Log.Entity;

namespace Website.AppCode.Controller
{
    public class ProfileServiceImpl
    {
        private ProfileService.UserAcc profile;

        private void OpenSubinfoPortal()
        {
            try
            {
                profile = new ProfileService.UserAcc();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                profile = null;
            }
        }

        #region Portal Information
        public UserAccInfo GetProfileByUsername(string username)
        {
            if (profile == null) OpenSubinfoPortal();
            if (profile == null) return null;
            try
            {
                string strResult = profile.GetValueByUsername(string.Empty, username, System.Configuration.ConfigurationManager.AppSettings["sso_url"]);
                if (string.IsNullOrEmpty(strResult)) return null;

                List<UserAccInfo> lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserAccInfo>>(strResult);
                if (lst != null && lst.Count > 0) return lst[0];
                return null;
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getProfile", "username: " + username + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Portal);
                Utility.LogEvent(ex);
                return null;
            }
        }

        #endregion
    }
}