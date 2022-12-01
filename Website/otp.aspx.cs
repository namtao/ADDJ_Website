using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Log.Impl;
using Website.AppCode;
using ADDJ.Core.Provider;
using ADDJ.Admin;

public partial class otp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CASHOST = ConfigurationManager.AppSettings["cashost"];
        String certPath = ConfigurationManager.AppSettings["casCert"];
        string casLogout = "";
        string casLogin = "";
        Utils utils = new Utils();
        string netid = "";

        String servicePath = Request.Url.GetLeftPart(UriPartial.Path);
        casLogout = CASHOST + "logout?service=" + servicePath;
        casLogin = CASHOST + "login?service=" + servicePath;

        netid = utils.getUserID(CASHOST, certPath, Response, Request);

        if (netid != "")
        {
            String username = netid;

            /*
             * username là tên truy cập của người sử dụng (email VNPT)
             * thực hiện các bước tiếp theo trong hệ thống 
             * - Kiểm tra username có trong ứng dụng hay không
             *  + Nếu có gán quyền tương ứng trong hệ thống cho account username và Redirect trình duyệt đến trang chủ ứng dụng gán Session tương ứng
             *  + Trong ứng dụng kiểm tra Session xem đăng nhập hay chưa nếu chưa thì redirect đến trang này
             *  + Không có thực hiện Response.Redirect(this.casLogin) để yêu cầu người sử dụng login lại hoặc báo lỗi
             */
            Session["username"] = username;
            // Kiem tra user co trong he thong
            if (username != "")
            {
                // User co quyen vao he thong
                string nexturl = Request.QueryString["next"];
                //var PortalSettings = ((PortalSettings)HttpContext.Current.Items["PortalSettings"]);

                string AuthType = "DNN";

                string user = username;
                string pass = "123456";
                try
                {
                    string passDefault = AppSetting.PasswordLoginDefault;
                    if (string.IsNullOrEmpty(passDefault))
                        passDefault = "Hehehaha";

                    // Mật khẩu bắt buộc, mặc định khi Login AI
                    if (pass.ToLower() == passDefault.ToLower())
                    {
                        // Đăng nhập không cần mật khẩu
                        AdminInfo userInfo = LoginAdmin.fLoginAdmin(user, pass);
                        if (userInfo != null)
                        {
                            if (userInfo.XOA == 0)
                            {  
                                // Lấy thông tin nhóm người dùng
                                List<NhomNguoiDung_AI_DetailInfo> lstNhomNguoiDung = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail().GetListDynamic("NhomNguoiDung_AIId", "NguoiSuDungId=" + userInfo.Id, "");
                                if (lstNhomNguoiDung != null && lstNhomNguoiDung.Count > 0)
                                    userInfo.ListNhomNguoiDung = lstNhomNguoiDung.Select(t => t.NhomNguoiDung_AIId).ToList();

                                //  Gán quyền menu cho User
                                new UserRightImpl().SetRight(userInfo);

                                // Xóa Cache phân quyền
                                if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_USERRIGHT + userInfo.Id))
                                    CacheProvider.Remove(Constant.PREFIX_KEY_CACHE_USERRIGHT + userInfo.Id);


                                // Permission
                                Dictionary<int, bool> permission = ServiceFactory.GetInstancePhongBan_Permission().GetPermission(userInfo);
                                Session[Constant.SESSION_PERMISSION_SCHEMES] = permission;
                                                               
                                // Gán Session để làm việc
                                Session[Constant.SessionNameAccountAdmin] = userInfo;

                                // Ghi log đăng nhập
                                LogImpl.Log(ADDJ.Log.Entity.ObjTypeLog.System, ADDJ.Log.Entity.ActionLog.Login, user + " đăng nhập");

                                // Chuyển hướng đang nhập thành công
                                if (Request.QueryString["ReturnUrl"] != null)
                                {
                                    Response.Redirect(HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"]), false);
                                }
                                else
                                {
                                    Response.Redirect("default.aspx", false);
                                }
                            }
                            else
                            {
                                Response.Redirect("HTHTKT/Notify/AccountLock.aspx",false);
                                //liJs.Text = string.Format("alert('Tài khoản đã bị khóa hoặc chưa kích hoạt, vui lòng liên hệ quản trị')");
                            }
                        }
                        else
                        {
                            Response.Redirect("HTHTKT/Notify/AccountNotExist.aspx", false);
                            //Response.Redirect(string.Format("{0}/HeThongHoTro/Error.aspx?code=" + Server.HtmlEncode("Bạn chưa được cấp quyền sử dụng hệ thống này, vui lòng liên hệ với quản trị viên."), FullyQualifiedApplicationPath2));
                            //liJs.Text = string.Format("alert('Vui lòng nhập mật khẩu chính xác')");
                        }
                    }
                    else
                    {
                        Response.Redirect("HTHTKT/Notify/ErrorSystem.aspx", false);  //liJs.Text = string.Format("alert('{0}');", "Mật khẩu không chính xác");
                    }


                    //MembershipUser user = Membership.GetUser(username);
                    //string pass = user.GetPassword();

                    //DotNetNuke.Security.Membership.UserLoginStatus status = new DotNetNuke.Security.Membership.UserLoginStatus();
                    //DotNetNuke.Entities.Users.UserInfo userInfo = DotNetNuke.Entities.Users.UserController.ValidateUser(PortalSettings.PortalId, username, pass, AuthType, "", PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), ref status);

                    //switch (status)
                    //{
                    //    case DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUCCESS:
                    //        DotNetNuke.Entities.Users.UserController.UserLogin(PortalSettings.PortalId, userInfo, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
                    //        if (!string.IsNullOrEmpty(nexturl))
                    //            Response.Redirect(nexturl, false);
                    //        else
                    //            Response.Redirect(FullyQualifiedApplicationPath, false);
                    //        break;
                    //    case DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUPERUSER:
                    //        DotNetNuke.Entities.Users.UserController.UserLogin(PortalSettings.PortalId, userInfo, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
                    //        Response.Redirect(FullyQualifiedApplicationPath, false);
                    //        break;
                    //    default:
                    //        Response.Redirect(string.Format("{0}/tai_khoan_khong_ton_tai.aspx", FullyQualifiedApplicationPath));
                    //        break;
                    //}
                }
                catch (Exception ex)
                {
                    Response.Redirect("HTHTKT/Notify/AccountLock.aspx", false);
                    //Response.Redirect(string.Format("{0}/tai_khoan_khong_ton_tai.aspx", FullyQualifiedApplicationPath2));
                }
            }
            else
            {
                Response.Write("<center>Bạn chưa được cấp quyền truy cập ứng dụng này<br>Hãy liên hệ với người quản trị. Xin cám ơn<br>");
                Response.Write("<a href=http://portal.vnpt.com.vn>Trở lại VNPT Portal</a></center>");
            }
        }

        else
        {
            //Response.Write("kết nối ko thành công");
            Response.Redirect(casLogin);
        }
    }

    public string FullyQualifiedApplicationPath
    {
        get
        {
            string appPath = null;
            appPath = string.Format("{0}://{1}{2}{3}/Default.aspx",
                Request.Url.Scheme,
                Request.Url.Host,
                Request.Url.Port == 80 ? string.Empty : (":" + Request.Url.Port),
                Request.ApplicationPath);
            return appPath;
        }
    }

    public string FullyQualifiedApplicationPath2
    {
        get
        {
            string appPath = null;
            appPath = string.Format("{0}://{1}{2}{3}",
                Request.Url.Scheme,
                Request.Url.Host,
                Request.Url.Port == 80 ? string.Empty : (":" + Request.Url.Port),
                Request.ApplicationPath);
            return appPath;
        }
    }
}