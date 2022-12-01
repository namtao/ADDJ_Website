using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using AIVietNam.Admin;
using AIVietNam.Core;
using SSO.Client.References;
using Website.AppCode.Controller;
using AIVietNam.Log.Impl;
using AIVietNam.Log.Entity;
using AIVietNam.GQKN.Impl;
using Website.AppCode;
using AIVietNam.Core.Provider;
using AIVietNam.GQKN.Entity;
using System.Data;

namespace Website.Views.CaNhan.Ajax
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["type"] ?? context.Request.QueryString["type"];
            switch (type)
            {
                case "login":
                    DangNhap(context);
                    break;
            }
        }

        private void DangNhap(HttpContext context)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["sso_url"];
                string token = ConvertUtility.ToString(context.Request.Form["token"] ?? context.Request.QueryString["token"]);
                string StrCheck = token + "<#-#>" + System.Configuration.ConfigurationManager.AppSettings["key_id"];
                SSOPartnerServiceClient client = new SSOPartnerServiceClient("partnerEndpoint");
                SSOUser user = client.ValidateToken(StrCheck);
                string user1 = user.Username;
                string pass1 = "123456";

                if (string.IsNullOrEmpty(user1) || string.IsNullOrEmpty(pass1))
                {
                    return;
                }
                AdminInfo item = LoginAdmin.fLoginAdmin(user1, pass1);
                if (item != null)
                {
                    if (item.Status == 0)
                    {
                        context.Response.Write(2);
                        return;
                    }

                    PhongBanInfo loaiPB = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanId);
                    if (loaiPB != null)
                    {
                        item.LoaiPhongBanId = loaiPB.LoaiPhongBanId;
                        item.IsChuyenTiepKN = loaiPB.IsChuyenTiepKN;
                        item.DefaultHTTN = loaiPB.DefaultHTTN;
                        item.IsChuyenVNP = loaiPB.IsChuyenVNP;
                        item.CapPhongBan = loaiPB.Cap;
                    }

                    //Get thong tin nhom nguoi dung
                    List<NhomNguoiDung_AI_DetailInfo> lstNhomNguoiDung = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail().GetListDynamic("NhomNguoiDung_AIId", "NguoiSuDungId=" + item.Id, "");
                    if (lstNhomNguoiDung != null && lstNhomNguoiDung.Count > 0)
                        item.ListNhomNguoiDung = lstNhomNguoiDung.Select(t => t.NhomNguoiDung_AIId).ToList();

                    //Set quyen menu cua user
                    new UserRightImpl().SetRight(item);

                    //Xóa cache phân quyền
                    if (CacheProvider.Exists(Constant.PREFIX_KEY_CACHE_USERRIGHT + item.Id))
                        CacheProvider.Remove(Constant.PREFIX_KEY_CACHE_USERRIGHT + item.Id);

                    //Permission
                    Dictionary<int, bool> permission = ServiceFactory.GetInstancePhongBan_Permission().GetPermission(item);
                    context.Session[Constant.SESSION_PERMISSION_SCHEMES] = permission;

                    // Lấy ra DoiTacType và giới hạn giảm trừ của KTV nếu có
                    DataSet dsGioiHanGiamTru = ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().GetInfoGioiHanGiamTruOfUser(item.Id);
                    if (dsGioiHanGiamTru != null)
                    {
                        // Trường hợp nếu DoiTacType = VNPT thì user được mặc định khi nhập số tiền giảm trừ thì mặc định sẽ được xác nhận luôn
                        if (dsGioiHanGiamTru.Tables.Count > 0 && dsGioiHanGiamTru.Tables[0].Rows.Count > 0)
                        {
                            item.DoiTacType = ConvertUtility.ToInt32(dsGioiHanGiamTru.Tables[0].Rows[0]["DoiTacType"]);
                            if (item.DoiTacType == DoiTacInfo.DoiTacTypeValue.VNPTTT)
                            {
                                item.IsDaBuTienAuto = true;
                            }
                        }

                        // Lấy ra giới hạn giảm trừ của KTV
                        if (dsGioiHanGiamTru.Tables.Count > 1 && dsGioiHanGiamTru.Tables[1].Rows.Count > 0)
                        {
                            item.GioiHanGiamTruMax = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMax"]);
                            item.GioiHanGiamTruMin = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMin"]);
                        }
                    }

                    new AdminImpl().UpdateLogin(user1);
                    context.Session[Constant.SessionNameAccountAdmin] = item;
                    // Utility.LogEvent(user + " đăng nhập quản trị thành công.", EventLogEntryType.Information);

                    try
                    {
                        // Đưa thông tin Session vào Database                   
                        NguoiSuDung_OnlineImpl nguoiSuDungOnlineImpl = new NguoiSuDung_OnlineImpl();
                        NguoiSuDung_OnlineInfo nguoiSuDungOnlineInfo = new NguoiSuDung_OnlineInfo();
                        nguoiSuDungOnlineInfo.SessionId = context.Session.SessionID;
                        nguoiSuDungOnlineInfo.NguoiSuDungId = item.Id;
                        nguoiSuDungOnlineInfo.TenTruyCap = item.Username;
                        nguoiSuDungOnlineInfo.Ip = context.Request.ServerVariables["REMOTE_ADDR"];
                        nguoiSuDungOnlineInfo.ThoiGianBatDau = DateTime.Now;
                        nguoiSuDungOnlineInfo.ThoiGianKetThuc = DateTime.MaxValue;
                        nguoiSuDungOnlineImpl.Add(nguoiSuDungOnlineInfo);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent("Lỗi đăng nhập : " + ex.Message);
                    }

                    LogImpl.Log(ObjTypeLog.System, ActionLog.Login);
                    context.Response.Write(1);
                }
                else
                {
                    UserAccInfo obj = new ProfileServiceImpl().GetProfileByUsername(user1);
                    if (obj != null)
                    {
                        bool flag = NguoiSuDungImpl.UpdateProfileCrossSell(obj.Username, obj.DoiTacId, obj.FullName, obj.Phone, obj.Mobile, obj.Email, obj.Address, obj.Birthday.ToString("dd/MM/yyyy"), obj.Sex, obj.Status, obj.LUser);
                        if (flag)
                        {
                            item = LoginAdmin.fLoginAdmin(user1, pass1);
                            if (item != null)
                            {
                                if (item.Status == 0)
                                {
                                    context.Response.Write(2);
                                    return;
                                }
                                PhongBanInfo loaiPB = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanId);
                                if (loaiPB != null)
                                    item.LoaiPhongBanId = loaiPB.LoaiPhongBanId;

                                //Permission
                                Dictionary<int, bool> permission = ServiceFactory.GetInstancePhongBan_Permission().GetPermission(item);
                                context.Session[Constant.SESSION_PERMISSION_SCHEMES] = permission;

                                // Lấy ra DoiTacType và giới hạn giảm trừ của KTV nếu có
                                DataSet dsGioiHanGiamTru = ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().GetInfoGioiHanGiamTruOfUser(item.Id);
                                if (dsGioiHanGiamTru != null)
                                {
                                    // Trường hợp nếu DoiTacType = VNPT thì user được mặc định khi nhập số tiền giảm trừ thì mặc định sẽ được xác nhận luôn
                                    if (dsGioiHanGiamTru.Tables.Count > 0 && dsGioiHanGiamTru.Tables[0].Rows.Count > 0)
                                    {
                                        item.DoiTacType = ConvertUtility.ToInt32(dsGioiHanGiamTru.Tables[0].Rows[0]["DoiTacType"]);
                                        if (item.DoiTacType == DoiTacInfo.DoiTacTypeValue.VNPTTT)
                                        {
                                            item.IsDaBuTienAuto = true;
                                        }
                                    }

                                    // Lấy ra giới hạn giảm trừ của KTV
                                    if (dsGioiHanGiamTru.Tables.Count > 1 && dsGioiHanGiamTru.Tables[1].Rows.Count > 0)
                                    {
                                        item.GioiHanGiamTruMax = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMax"]);
                                        item.GioiHanGiamTruMin = ConvertUtility.ToDecimal(dsGioiHanGiamTru.Tables[1].Rows[0]["MocKhauTruMin"]);
                                    }
                                }

                                new AdminImpl().UpdateLogin(user1);
                                context.Session[Constant.SessionNameAccountAdmin] = item;
                                // Utility.LogEvent(user + " đăng nhập quản trị thành công.", EventLogEntryType.Information);

                                try
                                {
                                    // Đưa thông tin Session vào Database                   
                                    NguoiSuDung_OnlineImpl nguoiSuDungOnlineImpl = new NguoiSuDung_OnlineImpl();
                                    NguoiSuDung_OnlineInfo nguoiSuDungOnlineInfo = new NguoiSuDung_OnlineInfo();
                                    nguoiSuDungOnlineInfo.SessionId = context.Session.SessionID;
                                    nguoiSuDungOnlineInfo.NguoiSuDungId = item.Id;
                                    nguoiSuDungOnlineInfo.TenTruyCap = item.Username;
                                    nguoiSuDungOnlineInfo.Ip = context.Request.ServerVariables["REMOTE_ADDR"];
                                    nguoiSuDungOnlineInfo.ThoiGianBatDau = DateTime.Now;
                                    nguoiSuDungOnlineInfo.ThoiGianKetThuc = DateTime.MaxValue;
                                    nguoiSuDungOnlineImpl.Add(nguoiSuDungOnlineInfo);
                                }
                                catch (Exception ex)
                                {
                                    Utility.LogEvent("Lỗi đang nhập: " + ex.Message);
                                }

                                LogImpl.Log(ObjTypeLog.System, ActionLog.Login);
                                context.Response.Write(1);
                            }
                            else
                            {
                                context.Session[Constant.SessionNameAccountAdmin] = string.Empty;
                                context.Response.Write(0);
                            }
                        }
                    }
                    else
                    {
                        context.Session[Constant.SessionNameAccountAdmin] = string.Empty;
                        context.Response.Write(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(0);
            }
        }
        public bool IsReusable => false;
    }
}
