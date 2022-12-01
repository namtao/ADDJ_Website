using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADDJ.Core;
using System.Collections;
using System.Collections.Generic;
using ADDJ.Admin;
using Website.AppCode;
using System.Text;
using System.Linq;
using Website.AppCode.Controller;
using ADDJ.Entity;
using ADDJ.Impl;
using ADDJ.Core.Provider;
using System.Net;
using Website.HTHTKT;

public partial class Master_Default : MasterPage
{
    private const int _itemW = 100;
    protected string strMessagePerrmission = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
        LoginAdmin.IsLoginAdmin();
        if (!IsPostBack)
        {
            BuildMenu();
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Thêm_khiếu_nại))
            {
                strMessagePerrmission = Constant.MESSSAGE_NOT_PERMISSION;
                //spanAddKN.Visible = false;
            }
            // Lấy danh sách phòng ban của người dùng
            AdminInfo loginInfo = LoginAdmin.AdminLogin();
            if (loginInfo != null)
            {
                //List<PhongBanInfo> listPhongBan = new PhongBanImpl().GetListPhongBanByUserId(loginInfo.Id);
                //if (listPhongBan != null && listPhongBan.Count > 0)
                //{
                //    ddlPhongBan.DataSource = listPhongBan;
                //    ddlPhongBan.DataBind();
                //    ddlPhongBan.SelectedValue = loginInfo.PhongBanId.ToString();
                //}
                //else
                //{
                //    ddlPhongBan.Visible = false;
                //    liSp.Visible = false;
                //}

                using (var ctx = new ADDJContext())
                {
                    var strLstDonViNguoidung = string.Format(@"SELECT b.ID,b.MADONVI,b.TENDONVI 
                      FROM HT_NGUOIDUNG_DONVI a
                      INNER JOIN HT_DONVI b on a.ID_DONVI=b.ID
                      WHERE a.ID_NGUOIDUNG={0}", loginInfo.Id);
                    var lst = ctx.Database.SqlQuery<HT_DONVI_VIEW>(strLstDonViNguoidung).ToList();
                    if (lst.Any())
                    {
                        ddlPhongBan.DataSource = lst;
                        ddlPhongBan.DataBind();
                        ddlPhongBan.SelectedValue = loginInfo.PhongBanId.ToString();
                    }
                    else
                    {
                        ddlPhongBan.Visible = false;
                        liSp.Visible = false;
                    }
                }
            }
        }
        string sCurr = Request.Url.AbsolutePath;
        AdminInfo objAdmin = LoginAdmin.AdminLogin();
        ltAdmin.Text = objAdmin.FullName;
        List<MenuInfo> lstRight = MenuImpl.ListMenu;
        IEnumerable<MenuInfo> objRight = lstRight.Where(t => t.Link.ToLower().Contains(sCurr.ToLower()));
        if (objRight != null && objRight.Any())
        {
            MenuInfo objRight2 = objRight.ElementAt(0);
            Page.Title = objRight2.Name;
            ltTitlePage.Text = objRight2.Name2;
        }
        else if (sCurr.ToLower().Equals("/Default.aspx".ToLower()))
        {
            Page.Title = "Hệ thống hỗ trợ kỹ thuật tập trung";
            ltTitlePage.Text = "Dashboards";
        }
    }
    public string GetIPAddress()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        return ipAddress.ToString();
    }


    // menu mới:


    StringBuilder sbmn;
    private void buildmenuNET(List<MenuInfo> lstmenu, int level, int parent)
    {
        sbmn.AppendFormat("<ul  class=\"level{0}\">", level);
        foreach (var item in lstmenu.Where(p => p.ParentID == parent))
        {
            if (level == 1)  // level top style down
            {
                sbmn.AppendFormat("<li class=\"level{4}-li\"><a class=\"level{3}-a {2}\" href=\"{1}\">{0}</a>", item.Name, item.Link, lstmenu.Where(t => t.ParentID == item.ID).Any() == true ? "drop" : "", level, level);
            }
            else  // level next child style fly
            {
                sbmn.AppendFormat("<li class=\"level{4}-li\"><a class=\"level{3}-a {2}\" href=\"{1}\">{0}</a>", item.Name, item.Link, lstmenu.Where(t => t.ParentID == item.ID).Any() == true ? "fly" : "", level, level);
            }
            if (lstmenu.Where(t => t.ParentID == item.ID).Any())
            {
                buildmenuNET(lstmenu, level + 1, item.ID);
            }
            sbmn.AppendFormat("</li>");
        }
        sbmn.Append("</ul>");
    }

    private void buildmenuNET2(IEnumerable<UserRightInfo> lstmenu, int level, int parent)
    {
        sbmn.AppendFormat("<ul  class=\"level{0}\">", level);
        foreach (var item in lstmenu.Where(p => p.Menu_ParentId == parent))
        {
            if (level == 1) // level top style down
            {
                sbmn.AppendFormat("<li class=\"level{4}-li\"><a class=\"level{3}-a {2}\" href=\"{1}\">{0}</a>", item.Menu_Name, item.Menu_Url, lstmenu.Where(t => t.Menu_ParentId == item.MenuID).Any() == true ? "drop" : "", level, level);
            }
            else    // level next child style fly
            {
                sbmn.AppendFormat("<li class=\"level{4}-li\"><a class=\"level{3}-a {2}\" href=\"{1}\">{0}</a>", item.Menu_Name, item.Menu_Url, lstmenu.Where(t => t.Menu_ParentId == item.MenuID).Any() == true ? "fly" : "", level, level);
            }
            if (lstmenu.Where(t => t.Menu_ParentId == item.MenuID).Any())
            {
                buildmenuNET2(lstmenu, level + 1, item.MenuID);
            }
            sbmn.AppendFormat("</li>");
        }
        sbmn.Append("</ul>");
    }


    private void BuildMenu()
    {
        AdminInfo userInfo = LoginAdmin.AdminLogin();
        if (userInfo.Status == 2)
        {
            var lst = ServiceFactory.GetInstanceMenu().GetListDynamic("", "Display = 1", "STT");
            sbmn = new StringBuilder();
            buildmenuNET(lst, 1, 0);
            var feew = sbmn.ToString();
            ltMenu2.Text = feew;
            return;

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul  class=\"level1\">");
            foreach (var item in lst.Where(t => t.ParentID == 0))
            {
                sb.AppendFormat("<li class=\"level1-li\"><a class=\"level1-a {2}\" href=\"{1}\">{0}</a>", item.Name, item.Link, lst.Where(t => t.ParentID == item.ID).Any() == true ? "drop" : "");

                if (lst.Where(t => t.ParentID == item.ID).Any())
                {
                    sb.AppendFormat("<ul  class=\"level2\" style=\"width:300px\">");
                    foreach (var child in lst.Where(t => t.ParentID == item.ID))
                    {
                        sb.AppendFormat("<li><a class=\"{2}\" href=\"{1}\">{0}</a>", child.Name, child.Link, lst.Where(t => t.ParentID == child.ID).Any() == true ? "fly" : "");

                        if (lst.Where(t => t.ParentID == child.ID).Any())
                        {
                            sb.AppendFormat("<ul  class=\"level3\" style=\"width:300px\">");
                            foreach (var child2 in lst.Where(t => t.ParentID == child.ID))
                            {
                                sb.AppendFormat("<li><a href=\"{1}\">{0}</a></li>", child2.Name, child2.Link);
                            }
                            sb.AppendFormat("</ul>");
                        }
                        sb.AppendFormat("</li>");
                    }
                    sb.AppendFormat("</ul>");
                }
                sb.AppendFormat("</li>");
            }
            sb.Append("</ul>");
            ltMenu2.Text = sb.ToString();
        }
        else
        {
            IEnumerable<UserRightInfo> lst = new UserRightImpl().GetMenuByAdminID(userInfo, 140, BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Thêm_khiếu_nại));

            sbmn = new StringBuilder();
            buildmenuNET2(lst, 1, 0);
            var feew = sbmn.ToString();
            ltMenu2.Text = feew;
            return;


            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var item in lst.Where(t => t.Menu_ParentId == 0 && t.Menu_Display == 1))
            {
                sb.AppendFormat("<li><a href=\"{1}\">{0}</a>", item.Menu_Name, item.Menu_Url);
                if (lst.Where(t => t.Menu_ParentId == item.MenuID).Any())
                {
                    if (lst.Where(t => t.Menu_ParentId == item.MenuID && t.Menu_Display == 1).Any())
                    {
                        sb.AppendFormat("<ul style=\"width:300px\">");
                        foreach (var child in lst.Where(t => t.Menu_ParentId == item.MenuID && t.Menu_Display == 1))
                        {
                            sb.AppendFormat("<li><a href=\"{1}\">{0}</a></li>", child.Menu_Name, child.Menu_Url);
                        }
                        sb.AppendFormat("</ul>");
                    }
                }
                sb.AppendFormat("</li>");
            }
            sb.AppendFormat("</ul>");
            ltMenu2.Text = sb.ToString();
        }
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
    {
        AdminInfo item = LoginAdmin.AdminLogin();
        //PhongBanInfo loaiPB = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(ddlPhongBan.SelectedValue));
        //if (loaiPB != null)
        //{
        //    item.LoaiPhongBanId = loaiPB.LoaiPhongBanId;
        //    item.IsChuyenTiepKN = loaiPB.IsChuyenTiepKN;
        //    item.DefaultHTTN = loaiPB.DefaultHTTN;
        //    item.PhongBanId = Convert.ToInt32(ddlPhongBan.SelectedValue);
        //    item.DoiTacId = loaiPB.DoiTacId;
        //}

        item.PhongBanId = Convert.ToInt32(ddlPhongBan.SelectedValue);
        item.DoiTacId = Convert.ToInt32(ddlPhongBan.SelectedValue);

        // Get thong tin nhom nguoi dung
        var lstNhomNguoiDung = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail().GetListDynamic("NhomNguoiDung_AIId", "NguoiSuDungId=" + item.Id, "");
        if (lstNhomNguoiDung != null && lstNhomNguoiDung.Count > 0)
            item.ListNhomNguoiDung = lstNhomNguoiDung.Select(t => t.NhomNguoiDung_AIId).ToList();

        // Set quyen menu cua user
        new UserRightImpl().SetRight(item);

        // Xóa cache phân quyền
        if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_USERRIGHT + item.Id))
            CacheProvider.Remove(Constant.PREFIX_KEY_CACHE_USERRIGHT + item.Id);

        // Permission
        Dictionary<int, bool> permission = ServiceFactory.GetInstancePhongBan_Permission().GetPermission(item);
        Session[Constant.SESSION_PERMISSION_SCHEMES] = permission;
        Session[Constant.SessionNameAccountAdmin] = item;
        Response.Redirect("/");
    }
}