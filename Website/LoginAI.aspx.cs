using System;
using System.Web;
using ADDJ.Core;
using ADDJ.Admin;
using ADDJ.Log.Impl;
using ADDJ.Entity;
using Website.AppCode;
using ADDJ.Core.Provider;
using System.Linq;
using System.Data;
using System.Collections.Generic;

public partial class LoginAI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AdminInfo user = LoginAdmin.AdminLogin();
            if (user != null) // Đang đăng nhập
            {
                string backUrl = Request.QueryString["Url"];
                if (!string.IsNullOrEmpty(backUrl))
                    Response.Redirect(string.Format("http://{0}", HttpUtility.UrlDecode(backUrl)));
                else
                    Response.Redirect(Config.PathAdmin); // Default when is login = ex: Home
            }
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                string user = txtUsername.Text.Trim().ToLower();
                string pass = txtPassword.Text.Trim();

                string passDefault = AppSetting.PasswordLoginDefault;
                if (string.IsNullOrEmpty(passDefault))
                    passDefault = "Hehehaha";

                // Mật khẩu bắt buộc, mặc định khi Login AI
                // if (pass.ToLower() != passDefault.ToLower())
                if (true)
                {
                    // Đăng nhập không cần mật khẩu
                    AdminInfo userInfo = LoginAdmin.fLoginAdmin(user, pass);
                    if (userInfo != null)
                    {
                        if (userInfo.XOA == 0)
                        {
                            //PhongBanInfo loaiPB = ServiceFactory.GetInstancePhongBan().GetInfo(userInfo.PhongBanId);
                            //if (loaiPB != null)
                            //{
                            //    userInfo.LoaiPhongBanId = loaiPB.LoaiPhongBanId;
                            //    userInfo.IsChuyenTiepKN = loaiPB.IsChuyenTiepKN;
                            //    userInfo.DefaultHTTN = loaiPB.DefaultHTTN;
                            //    userInfo.IsChuyenVNP = loaiPB.IsChuyenVNP;
                            //    userInfo.CapPhongBan = loaiPB.Cap;
                            //}

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

                            // Lấy ra DoiTacType và giới hạn giảm trừ của KTV nếu có
                            //DataSet dsGioiHanGiamTru = ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().GetInfoGioiHanGiamTruOfUser(userInfo.Id);
                            //if (dsGioiHanGiamTru != null)
                            //{
                            //    // Trường hợp nếu DoiTacType = VNPT thì user được mặc định khi nhập số tiền giảm trừ thì mặc định sẽ được xác nhận luôn
                            //    if (dsGioiHanGiamTru.Tables.Count > 0 && dsGioiHanGiamTru.Tables[0].Rows.Count > 0)
                            //    {
                            //        userInfo.DoiTacType = ConvertUtility.ToInt32(dsGioiHanGiamTru.Tables[0].Rows[0]["DoiTacType"]);
                            //        if (userInfo.DoiTacType == DoiTacInfo.DoiTacTypeValue.VNPTTT)
                            //        {
                            //            userInfo.IsDaBuTienAuto = true;
                            //        }
                            //    }

                            //    // Lấy ra giới hạn giảm trừ của KTV
                            //    if (dsGioiHanGiamTru.Tables.Count > 1 && dsGioiHanGiamTru.Tables[1].Rows.Count > 0)
                            //    {
                            //        userInfo.GioiHanGiamTruMax = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMax"]);
                            //        userInfo.GioiHanGiamTruMin = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMin"]);
                            //    }
                            //}

                            // Gán Session để làm việc
                            Session[Constant.SessionNameAccountAdmin] = userInfo;

                            // Ghi log đăng nhập
                            LogImpl.Log(ADDJ.Log.Entity.ObjTypeLog.System, ADDJ.Log.Entity.ActionLog.Login, txtUsername.Text + " đăng nhập");

                            // Chuyển hướng đang nhập thành công
                            if (Request.QueryString["ReturnUrl"] != null)
                            {
                                Response.Redirect(HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"]), false);
                            }
                            else
                            {
                                Response.Redirect("/", false);
                            }
                        }
                        else
                        {
                            Response.Redirect("HTHTKT/Notify/AccountLock.aspx", false);
                            //liJs.Text = string.Format("alert('Tài khoản đã bị khóa hoặc chưa kích hoạt, vui lòng liên hệ quản trị')");
                        }
                    }
                    else
                    {
                        Response.Redirect("HTHTKT/Notify/AccountNotExist.aspx", false);
                        //liJs.Text = string.Format("alert('Vui lòng nhập mật khẩu chính xác')");
                    }
                }
                else
                {
                    Response.Redirect("HTHTKT/Notify/NotValidPass.aspx", false);  //liJs.Text = string.Format("alert('{0}');", "Mật khẩu không chính xác");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("HTHTKT/Notify/ErrorSystem.aspx", false);
                Utility.LogEvent(ex);
                //liJs.Text = string.Format("alert('{0}');", "Có lỗi xảy ra, vui lòng thử lại");
            }
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
