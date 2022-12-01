using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Linq;
using ADDJ.Sercurity;

namespace ADDJ.Admin
{
    public class LoginAdmin
    {
        public static AdminInfo fLoginAdmin(string username, string password)
        {
            try
            {
                var obj = new AdminImpl();

                var item = obj.CheckLogin(username, password);
                return item;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw ex;
            }
        }

        public static void IsLoginAdmin()
        {
            HttpContext context = HttpContext.Current;

            if (HttpContext.Current.Session[Constant.SessionNameAccountAdmin] == null || HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() == string.Empty)
            {
                // Đọc cấu hình tại Web.Config
                if (AppSetting.IsLoginLocal)
                    context.Response.Redirect(string.Format("~/LoginAI.aspx?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl)));

                if (Config.IsLocal)
                {
                    HttpContext.Current.Response.Redirect("/LoginAI.aspx?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl), true);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(string.Format("~/Login.aspx?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl)));
                    // HttpContext.Current.Response.Redirect(string.Format("~/Login.aspx?ReturnUrl={0}", HttpUtility.UrlEncode("http://10.149.34.250" + context.Request.RawUrl)));
                    // HttpContext.Current.Response.Redirect(Config.LoginAdmin + "?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri), true);
                }
            }
        }

        public static AdminInfo AdminLogin()
        {
            if (HttpContext.Current.Session[Constant.SessionNameAccountAdmin] == null ||
                HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() == string.Empty)
            {
                return null;
            }
            return (AdminInfo)HttpContext.Current.Session[Constant.SessionNameAccountAdmin];
        }
    }

    public class AdminImpl : BaseImpl<AdminInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "Admin";
        }

        public AdminInfo CheckLogin(string username, string pass)
        {
            AdminInfo userInfo = null;
            SqlParameter[] prms = new SqlParameter[2];

            prms[0] = new SqlParameter("@Username", username);
            prms[1] = new SqlParameter("@Pass", Encrypt.MD5Admin(pass));

            try
            {
                DataTable ds = this.ExecuteQueryToDataSet("usp_Admin_CheckAdminLoginNET", prms).Tables[0];
                if (ds != null && ds.Rows.Count > 0)
                {
                    userInfo = new AdminInfo();

                    userInfo.Username = ds.Rows[0]["TenTruyCap"].ToString();
                    userInfo.FullName = ds.Rows[0]["TenDayDu"].ToString();
                    userInfo.Phone = ds.Rows[0]["DiDong"].ToString();
                    userInfo.NhomNguoiDung = Convert.ToInt32(ds.Rows[0]["NhomNguoiDung"]);
                    userInfo.KhuVucId = Convert.ToInt32(ds.Rows[0]["KhuVucId"]);
                    userInfo.DoiTacId = Convert.ToInt32(ds.Rows[0]["DoiTacId"]);
                    userInfo.Id = Convert.ToInt32(ds.Rows[0]["Id"]);
                    userInfo.IsLogin = Convert.ToInt16(ds.Rows[0]["IsLogin"]);
                    userInfo.Status = Convert.ToInt16(ds.Rows[0]["TrangThai"]);
                    userInfo.PhongBanId = Convert.ToInt32(ds.Rows[0]["PhongBanId"]);
                    userInfo.XOA = Convert.ToInt32(ds.Rows[0]["XOA"]);
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return userInfo;
        }

        public void UpdateLogin(string username)
        {
            var prm = new SqlParameter[1];

            prm[0] = new SqlParameter("@Username", username);

            try
            {
                ExecuteNonQuery("sp_UpdateLoginNET", prm);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }
    }

    public class GroupAdminImpl : BaseImpl<GroupAdminInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "GroupAdmin";
        }
    }

    public class GroupAdminDetailImpl : BaseImpl<GroupAdminDetailInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "GroupAdminDetail";
        }

        #region IAdmin Members

        public DataTable GetAdminByGroup(int groupId)
        {
            DataTable dt = null;
            var prm = new SqlParameter[1];

            prm[0] = new SqlParameter("@GroupAdminId", groupId);

            try
            {
                var ds = this.ExecuteQueryToDataSet("usp_Admin_GetListAdminInGroup", prm);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.", ex);
            }
        }
        #endregion
    }

    public class MenuImpl : BaseImpl<MenuInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "Menu";
        }

        private static List<MenuInfo> _listMenu;
        public static List<MenuInfo> ListMenu
        {
            get
            {
                if (_listMenu == null)
                    _listMenu = new MenuImpl().GetList();
                return _listMenu;
            }
            set { _listMenu = value; }
        }


        public int GetSTTParent()
        {
            try
            {
                return Convert.ToInt32(this.ExecuteScalar("usp_Menu_GetSTTParent", null));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.", ex);
            }
        }

        public int GetSTTByMenu(int id)
        {
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@Id", id)

                                   };
                return Convert.ToInt32(this.ExecuteScalar("usp_Menu_GetSTTByMenuId", param));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.", ex);
            }
        }

        public void UpdateDisplay(int id, int display)
        {
            try
            {
                SqlParameter[] param = {
                                       new SqlParameter("@ID", id),
                                       new SqlParameter("@Display", display)
                                   };

                this.ExecuteNonQuery("usp_Menu_UpdateDisplay", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.", ex);
            }
        }
    }

    public class UserRightImpl : BaseImpl<UserRightInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "UserRight";
        }

        private const string STORE_GET_ALL_QUYEN_BY_ADMINID = "usp_UserRight_GetQuyenByMenu";

        private const string STORE_GET_PARENT_MENU_BY_ADMINID = "usp_UserRight_UserRightGroup_GetRightByAdminID";

        private const string STORE_GET_MENU_BY_ADMINID_AND_PARENTID = "usp_UserRight_UserRightGroup_GetRightByAdminIdAndParentId";

        private const string STORE_GET_FULL_PARENT_MENU = "usp_Menu_GetFullParent";

        private const string STORE_GET_FULL_MENU_BY_PARENTID = "usp_Menu_GetMenuByParentId";

        private const string STORE_GET_PARENT_MENU_BY_ID = "usp_Menu_GetMenuById";

        private const string STORE_GET_RIGHT_BY_MENUID = "usp_UserRight_UserRightGroup_GetRightByAdminIdAndLink";

        private const string STORE_GET_RIGHT_BY_MENUID_ADMINID = "usp_UserRight_GetRightByMenuIDAndAdminID";

        protected const string STORE_GET_PARENT_ID = "usp_Menu_GetParentIDByLink";

        #region IUserRight Members

        public UserRightInfo GetRightByMenuAndAdmin(int menuID, int adminID)
        {
            UserRightInfo item = null;
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@MenuID", menuID);
            prm[1] = new SqlParameter("@AdminID", adminID);

            try
            {
                var lst = this.ExecuteQuery(STORE_GET_RIGHT_BY_MENUID_ADMINID, prm);
                if (lst != null && lst.Count > 0)
                    item = lst[0];

                return item;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public DataTable GetFullParentMenu()
        {
            try
            {
                return this.ExecuteQueryToDataSet(STORE_GET_FULL_PARENT_MENU, null).Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public DataTable GetFullMenuByParentID(int parentID)
        {
            var prm = new SqlParameter[1];

            prm[0] = new SqlParameter("@ParentID", parentID);

            try
            {
                return this.ExecuteQueryToDataSet(STORE_GET_FULL_MENU_BY_PARENTID, prm).Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public DataTable GetParentMenuByID(int ID)
        {
            var prm = new SqlParameter[1];

            prm[0] = new SqlParameter("@ID", ID);

            try
            {
                return this.ExecuteQueryToDataSet(STORE_GET_PARENT_MENU_BY_ID, prm).Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public DataTable GetQuyenByAdminID(int adminID, int type)
        {
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@ID", adminID);
            prm[1] = new SqlParameter("@Type", type);

            try
            {
                return this.ExecuteQueryToDataSet(STORE_GET_ALL_QUYEN_BY_ADMINID, prm).Tables[0];

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public List<UserRightInfo> GetMenuByAdminID(int adminID)
        {
            if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_USERRIGHT + adminID))
            {
                return CacheProvider.Get(Constant.PREFIX_KEY_CACHE_USERRIGHT + adminID) as List<UserRightInfo>;
            }
            else
            {
                var prm = new SqlParameter[1];

                prm[0] = new SqlParameter("@ID", adminID);

                try
                {
                    var lst = this.ExecuteQuery(STORE_GET_PARENT_MENU_BY_ADMINID, prm);

                    if (lst != null && lst.Count > 0)
                    {
                        CacheProvider.Add(Constant.PREFIX_KEY_CACHE_USERRIGHT + adminID, lst);
                    }
                    return lst;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
                }
            }
        }


        //Loai bo menu
        public IEnumerable<UserRightInfo> GetMenuByAdminID(AdminInfo admin, int menuId, bool flag)
        {
            if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_USERRIGHT + admin.Id))
            {
                IEnumerable<UserRightInfo> ret = CacheProvider.Get(Constant.PREFIX_KEY_CACHE_USERRIGHT + admin.Id) as IEnumerable<UserRightInfo>;
                return ret;
            }
            else
            {
                var prm = new SqlParameter[1];

                prm[0] = new SqlParameter("@ID", admin.Id);
                try
                {
                    //var lst = this.ExecuteQuery(STORE_GET_PARENT_MENU_BY_ADMINID, prm);
                    var lst = this.ExecuteQuery("usp_UserRight_UserRightGroup_GetRightByAdminID_NET", prm);


                    //Lấy danh sách quyền từ nhóm người dùng
                    //if (admin.ListNhomNguoiDung != null && admin.ListNhomNguoiDung.Count > 0)
                    //{
                    //    var objRightNhom = new NhomNguoiDung_AI_UserRightImpl();
                    //    foreach (var itemNhomNguoiDung in admin.ListNhomNguoiDung)
                    //    {
                    //        var lstRightNhom = objRightNhom.GetListDynamicJoin("b.ID MenuID,b.Name Menu_Name,b.Link Menu_Url, ParentID Menu_ParentId, STT Menu_STT, Display Menu_Display", "right join Menu b on b.ID = a.MenuID and a.NhomNguoiDung_AIId = " + itemNhomNguoiDung, "UserRead = 1", "");
                    //        if (lstRightNhom != null && lstRightNhom.Count > 0)
                    //        {
                    //            foreach (var item in lstRightNhom)
                    //            {
                    //                if (!lst.Where(t => t.MenuID == item.MenuID).Any())
                    //                {

                    //                    lst.Add(new UserRightInfo()
                    //                    {
                    //                        MenuID = item.MenuID,
                    //                        Menu_Name = item.Menu_Name,
                    //                        Menu_Url = item.Menu_Url,
                    //                        Menu_ParentId = item.Menu_ParentId,
                    //                        Menu_STT = item.Menu_STT,
                    //                        Menu_Display = item.Menu_Display,
                    //                        UserRead = true
                    //                    });
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    var lstSort = lst.OrderBy(t => t.Menu_STT);

                    if (flag)
                    {
                        if (lst != null && lst.Count > 0)
                        {
                            CacheProvider.Add(Constant.PREFIX_KEY_CACHE_USERRIGHT + admin.Id, lstSort);
                        }
                    }
                    else
                    {
                        var itemRemove = lst.Where(t => t.MenuID == menuId);
                        if (itemRemove != null && itemRemove.Any())
                            lst.Remove(itemRemove.Single());

                        CacheProvider.Add(Constant.PREFIX_KEY_CACHE_USERRIGHT + admin.Id, lstSort);
                    }
                    return lstSort;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
                }
            }
        }

        public DataTable GetMenuByAdminIDAndParentID(int adminID, int parrentID)
        {
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@ID", adminID);
            prm[1] = new SqlParameter("@ParentID", parrentID);

            try
            {
                return this.ExecuteQueryToDataSet(STORE_GET_MENU_BY_ADMINID_AND_PARENTID, prm).Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
        }

        public UserRightInfo CheckRightAdmin_NoCache()
        {
            var right = new UserRightInfo();
            if (HttpContext.Current.Session[Constant.SessionNameAccountAdmin] != null &&
                HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() != string.Empty)
            {
                string sCurr = HttpContext.Current.Request.Url.AbsolutePath;

                var objAdmin = (AdminInfo)HttpContext.Current.Session[Constant.SessionNameAccountAdmin];

                if (objAdmin.Status == 2)
                {
                    right.UserEdit = true;
                    right.UserRead = true;
                    right.UserDelete = true;
                    right.Other1 = true;
                    right.Other2 = true;
                }
                else
                {
                    var lstRight = new UserRightImpl().GetMenuByAdminID(objAdmin.Id);

                    if (lstRight != null && lstRight.Count > 0)
                    {
                        var objRight = lstRight.Where(t => t.Menu_Url == sCurr);
                        if (objRight != null && objRight.Count() > 0)
                        {
                            var objRight2 = objRight.ElementAt(0);

                            right.UserRead = objRight2.UserRead;
                            right.UserEdit = objRight2.UserEdit;
                            right.UserDelete = objRight2.UserDelete;
                            right.Other1 = objRight2.Other1;
                            right.Other2 = objRight2.Other2;
                        }
                    }
                    else
                    {
                        right.UserRead = false;
                        right.UserEdit = false;
                        right.UserDelete = false;
                        right.Other1 = false;
                        right.Other2 = false;
                    }
                }

                if (!right.UserRead)
                    HttpContext.Current.Response.Redirect(Config.PathNotRight, true);
            }
            else
            {
                HttpContext.Current.Response.Redirect(Config.LoginAdmin, true);
            }


            return right;
        }

        [Serializable]
        public class TestLengDic
        {
            public AdminInfo Admin
            { get; set; }
            public List<UserRightGroupInfo> Nhom
            { get; set; }
        }

        public RightInfo CheckRightAdmin_Cache()
        {
            RightInfo right = new RightInfo();
            if (HttpContext.Current.Session[Constant.SessionNameAccountAdmin] != null &&
                HttpContext.Current.Session[Constant.SessionNameAccountAdmin].ToString() != string.Empty)
            {

                HttpContext context = HttpContext.Current;

                string sCurr = HttpContext.Current.Request.Url.AbsolutePath.Replace(context.Request.ApplicationPath, "/").Replace("//", "/");
                // nếu sCurr là /default.aspx => gán là: /ADDJ_TH/views/TraCuuHoSoADDJ.aspx (theo quyền)
                if (sCurr == "/default.aspx")
                {
                    sCurr = "/ADDJ_TH/views/TraCuuHoSoADDJ.aspx";
                }

                AdminInfo objAdmin = (AdminInfo)HttpContext.Current.Session[Constant.SessionNameAccountAdmin];

                if (objAdmin.Status == 2)
                {
                    right.UserEdit = true;
                    right.UserRead = true;
                    right.UserDelete = true;
                    right.Other1 = true;
                    right.Other2 = true;
                    right.Other3 = true;
                    right.Other4 = true;
                }
                else
                {
                    if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id))
                    {
                        var dicRightCache = CacheProvider.Get(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id) as Dictionary<string, RightInfo>;
                        if (dicRightCache != null && dicRightCache.Count > 0)
                        {
                            //if (dicRightCache.ContainsKey(sCurr))
                            //{
                            //    right = dicRightCache[sCurr];
                            //}
                            List<string> keys = dicRightCache.Where(d => d.Key.ToLower().Contains(sCurr.ToLower())).Select(v => v.Key).ToList<string>();
                            if (keys != null && keys.Count > 0)
                            {
                                // Vừa bằng
                                if (keys.Count == 1)
                                { }
                                else // Nhiều hơn
                                {
                                    //  Helper.GhiLogs("ExistsCache", string.Join(Environment.NewLine, keys));
                                }
                                string cKey = keys[0];
                                right = dicRightCache[cKey];
                            }
                        }
                        else
                        {
                            SetRight(objAdmin);
                            var dicRight = CacheProvider.Get(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id) as Dictionary<string, RightInfo>;
                            CacheProvider.Add(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id, dicRight);
                            if (dicRight != null && dicRight.Count > 0)
                            {
                                //if (dicRight.ContainsKey(sCurr))
                                //{
                                //    right = dicRight[sCurr];
                                //}

                                List<string> keys = dicRightCache.Where(d => d.Key.ToLower().Contains(sCurr.ToLower())).Select(v => v.Key).ToList<string>();
                                if (keys != null && keys.Count > 0)
                                {
                                    // Vừa bằng
                                    if (keys.Count == 1)
                                    { }
                                    else // Nhiều hơn
                                    {
                                        //  Helper.GhiLogs("NoFound_AddCache", string.Join(Environment.NewLine, keys));
                                    }
                                    string cKey = keys[0];
                                    right = dicRightCache[cKey];
                                }
                            }
                        }
                    }
                    else
                    {
                        SetRight(objAdmin);
                        Dictionary<string, RightInfo> dicRight = CacheProvider.Get(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id) as Dictionary<string, RightInfo>;

                        // CacheProvider.Add(Constant.PREFIX_KEY_CACHE_USERRIGHT + objAdmin.Id, dicRight);
                        // CacheProvider.Add(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id, dicRight);
                        if (dicRight != null && dicRight.Count > 0)
                        {
                            //if (dicRight.ContainsKey(sCurr))
                            //{
                            //    right = dicRight[sCurr];
                            //}

                            List<string> keys = dicRight.Where(d => d.Key.ToLower().Contains(sCurr.ToLower())).Select(v => v.Key).ToList<string>();
                            if (keys != null && keys.Count > 0)
                            {
                                // Vừa bằng
                                if (keys.Count == 1)
                                { }
                                else // Nhiều hơn
                                {
                                    // Helper.GhiLogs("NoExist_AddCache", string.Join(Environment.NewLine, keys));
                                }
                                string cKey = keys[0];
                                right = dicRight[cKey];
                            }
                        }
                    }
                }

                if (!right.UserRead)
                    HttpContext.Current.Response.Redirect(Config.PathNotRight, true);
            }
            else
            {
                HttpContext.Current.Response.Redirect(Config.LoginAdmin, true);
            }

            return right;
        }

        public void SetRight(AdminInfo objAdmin)
        {
            if (objAdmin.Status == 2)
                return;

            var objMenu = new MenuImpl();
            Dictionary<string, RightInfo> dicRight = new Dictionary<string, RightInfo>();

            //Quyen ca nhan
            var lstRightCaNhan = objMenu.GetListDynamicJoin("UserRead, UserEdit, UserDelete, Other1, Other2, Other3, Other4, a.Link", "left join UserRight b on a.ID = b.MenuID", "b.AdminID = " + objAdmin.Id + " and b.UserRead = 1", "");
            if (lstRightCaNhan != null && lstRightCaNhan.Count > 0)
            {
                foreach (var rItem in lstRightCaNhan)
                {
                    if (!dicRight.ContainsKey(rItem.Link))
                    {
                        var dRight = new RightInfo() { UserRead = rItem.UserRead, UserEdit = rItem.UserEdit, UserDelete = rItem.UserDelete, Other1 = rItem.Other1, Other2 = rItem.Other2, Other3 = rItem.Other3, Other4 = rItem.Other4 };
                        dicRight.Add(rItem.Link, dRight);
                    }
                }
            }

            //Quyen nhom VNP
            var lstRightNhom = objMenu.GetListDynamicJoin("UserRead, UserEdit, UserDelete, Other1, Other2, Other3, Other4, a.Link", "left join UserRightGroup b on a.ID = b.MenuID", "b.GroupAdminID = " + objAdmin.NhomNguoiDung + " and b.UserRead = 1", "");
            if (lstRightNhom != null && lstRightNhom.Count > 0)
            {
                foreach (var rItem in lstRightNhom)
                {
                    if (dicRight.ContainsKey(rItem.Link))
                    {
                        var rightOld = dicRight[rItem.Link];
                        if (rItem.UserEdit)
                            rightOld.UserEdit = true;
                        if (rItem.UserDelete)
                            rightOld.UserDelete = true;
                        if (rItem.Other1)
                            rightOld.Other1 = true;
                        if (rItem.Other2)
                            rightOld.Other2 = true;
                        if (rItem.Other3)
                            rightOld.Other3 = true;
                        if (rItem.Other4)
                            rightOld.Other4 = true;

                        dicRight[rItem.Link] = rightOld;
                    }
                    else
                    {
                        var dRight = new RightInfo() { UserRead = rItem.UserRead, UserEdit = rItem.UserEdit, UserDelete = rItem.UserDelete, Other1 = rItem.Other1, Other2 = rItem.Other2, Other3 = rItem.Other3, Other4 = rItem.Other4 };
                        dicRight.Add(rItem.Link, dRight);
                    }
                }
            }

            //Quyen nhom nguoi dung
            if (objAdmin.ListNhomNguoiDung != null && objAdmin.ListNhomNguoiDung.Count > 0)
            {
                foreach (var item in objAdmin.ListNhomNguoiDung)
                {
                    var lstRightNhomNguoiDung = objMenu.GetListDynamicJoin("UserRead, UserEdit, UserDelete, Other1, Other2, a.Link", "left join NhomNguoiDung_AI_UserRight b on a.ID = b.MenuID", "b.NhomNguoiDung_AIId = " + item + " and b.UserRead = 1", "");
                    if (lstRightNhomNguoiDung != null && lstRightNhomNguoiDung.Count > 0)
                    {
                        foreach (var rItem in lstRightNhomNguoiDung)
                        {
                            if (dicRight.ContainsKey(rItem.Link))
                            {
                                var rightOld = dicRight[rItem.Link];
                                if (rItem.UserEdit)
                                    rightOld.UserEdit = true;
                                if (rItem.UserDelete)
                                    rightOld.UserDelete = true;
                                if (rItem.Other1)
                                    rightOld.Other1 = true;
                                if (rItem.Other2)
                                    rightOld.Other2 = true;
                                if (rItem.Other3)
                                    rightOld.Other3 = true;
                                if (rItem.Other4)
                                    rightOld.Other4 = true;

                                dicRight[rItem.Link] = rightOld;
                            }
                            else
                            {
                                var dRight = new RightInfo() { UserRead = rItem.UserRead, UserEdit = rItem.UserEdit, UserDelete = rItem.UserDelete, Other1 = rItem.Other1, Other2 = rItem.Other2 };
                                dicRight.Add(rItem.Link, dRight);
                            }
                        }
                    }
                }
            }

            if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id))
            {
                CacheProvider.Remove(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id);
            }
            CacheProvider.Add(Constant.PREFIX_KEY_CACHE_SYSTEM + objAdmin.Id, dicRight);
        }

        #endregion

        /// <summary>
        /// Version 1
        /// </summary>
        /// <returns></returns>
        public static RightInfo CheckRightAdminnistrator_NoCache()
        {
            //return new UserRightImpl().CheckRightAdmin_NoCache();
            return new UserRightImpl().CheckRightAdmin_Cache();
        }

        /// <summary>
        /// Version 2
        /// </summary>
        /// <returns></returns>
        public static RightInfo CheckRightAdminnistrator_Cache()
        {
            return new UserRightImpl().CheckRightAdmin_Cache();
        }

        public static int GetParentID(string link)
        {
            DataTable dt = null;

            #region Parameter

            var prm = new SqlParameter[1];
            prm[0] = new SqlParameter("@Link", link);

            #endregion

            #region Execute

            try
            {
                dt = new UserRightImpl().ExecuteQueryToDataSet(STORE_GET_PARENT_ID, prm).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    int SoLonNhat = Convert.ToInt32(dt.Rows[0]["ID"]);

                    return SoLonNhat;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("GetData::GetParentID::Error", ex);
            }

            #endregion

            return 0;
        }
    }

    public class UserRightGroupImpl : BaseImpl<UserRightGroupInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "UserRightGroup";
        }

        private const string STORE_GET_RIGHT_BY_MENUID_GROUPADMINID = "usp_UserRightGroup_GetRightByMenuIDAndGroupAdminID";

        private const string STORE_GET_ALL_QUYEN_BY_GROUPID = "usp_UserRightGroup_GetQuyenGroupByMenu";

        public UserRightGroupInfo GetRightByMenuAndGroupAdmin(int menuID, int groupAdminId)
        {
            UserRightGroupInfo item = null;
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@MenuID", menuID);
            prm[1] = new SqlParameter("@GroupAdminId", groupAdminId);

            try
            {
                var lst = this.ExecuteQuery(STORE_GET_RIGHT_BY_MENUID_GROUPADMINID, prm);
                if (lst != null && lst.Count > 0)
                    item = lst[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
            return item;
        }

        public DataTable GetQuyenByGroupID(int groupID, int type)
        {
            DataTable dt = null;
            var prm = new SqlParameter[2];

            prm[0] = new SqlParameter("@ID", groupID);
            prm[1] = new SqlParameter("@Type", type);

            try
            {
                var ds = this.ExecuteQueryToDataSet(STORE_GET_ALL_QUYEN_BY_GROUPID, prm);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception("Hiện tại server đạng bận, xin vui lòng truy vấn lại sau.");
            }
            return dt;
        }
    }
}