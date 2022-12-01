using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using ADDJ.Admin;
using ADDJ.Core;
using ADDJ.Impl;

namespace Website.AppCode
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Kiểm tra chuỗi kết nối
            //Common.IsConnectDBOK();
        }

        protected void Application_End(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e)
        {
            //if (AppSetting.IsCatchEx)
            //{
            //    try
            //    {
            //        HttpContext context = HttpContext.Current;

            //        Exception ex = Server.GetLastError();
            //        if (ex.InnerException != null) ex = ex.InnerException;
            //        if (context.Session != null) context.Session["Error"] = ex;

            //        HttpContext.Current.ClearError();
            //        Helper.GhiLogs(ex);

            //        string backUrl = context.Request.QueryString["BackUrl"];
            //        bool isLoopBack = false;
            //        if (!string.IsNullOrEmpty(backUrl))
            //        {
            //            backUrl = HttpUtility.UrlDecode(backUrl);

            //            // Chạy vòng trang Error.aspx
            //            if (backUrl.ToLower().Contains("/Error.aspx".ToLower()))
            //            {
            //                isLoopBack = true;
            //            }
            //        }
            //        if (!isLoopBack) Response.Redirect("~/Error.aspx?Code=2&BackUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            //        else
            //        {
            //            context.Response.Clear();
            //            context.Response.Write("Kết nối dữ liệu bị gián đoạn, vui lòng liên hệ quản trị. Xin cảm ơn!");
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        try
            //        {
            //            Helper.GhiLogs(ex);
            //            HttpContext context = HttpContext.Current;

            //            string backUrl = context.Request.QueryString["BackUrl"];
            //            bool isLoopBack = false;
            //            if (!string.IsNullOrEmpty(backUrl))
            //            {
            //                backUrl = HttpUtility.UrlDecode(backUrl);

            //                // Chạy vòng trang Error.aspx
            //                if (backUrl.ToLower().Contains("/Error.aspx".ToLower()))
            //                {
            //                    isLoopBack = true;
            //                }
            //            }
            //            if (!isLoopBack) Response.Redirect("~/Error.aspx?Code=2&BackUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
            //            else context.Response.Write("Kết nối dữ liệu bị gián đoạn, vui lòng liên hệ quản trị. Xin cảm ơn!");
            //        }
            //        catch
            //        {
            //            Response.Write("Kết nối dữ liệu bị gián đoạn, vui lòng liên hệ quản trị. Xin cảm ơn!");
            //        }

            //    }
            //}
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Nếu không có kết nối tới Database
            //if (!Common.IsConnectDBOK()) Response.Redirect("~/Error.aspx?Code=1");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //AdminInfo userInfo = (AdminInfo)Session[Constant.SessionNameAccountAdmin];
            //if (userInfo != null)
            //{
            //    Utility.LogEvent(userInfo.Username + " thoát khỏi hệ thống!");
            //    ServiceFactory.GetInstanceNguoiSuDung().UpdateDynamic("IsLogin = 0", "Id = " + userInfo.Id);

            //    // Cập nhật thời gian timeout của Session
            //    try
            //    {
            //        NguoiSuDung_OnlineImpl nguoiSuDungOnlineImpl = new NguoiSuDung_OnlineImpl();
            //        nguoiSuDungOnlineImpl.UpdateDynamic("ThoiGianKetThuc='" + DateTime.Now + "'", "SessionId='" + Session.SessionID + "'");
            //    }
            //    catch (Exception ex)
            //    {
            //        Utility.LogEvent("Session End : " + ex.Message);
            //    }

            //}
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Kiểm tra có kết nối được với DB
            //if (!Common.IsDBConnected)
            //{
            //    // HttpContext context = HttpContext.Current;
            //    // Chỉ kiểm tra với Page bỏ qua trang Error
            //    if (Request.CurrentExecutionFilePathExtension == ".aspx" && Request.AppRelativeCurrentExecutionFilePath.ToLower() != "~/Error.aspx".ToLower())
            //    {
            //        Response.Redirect("~/Error.aspx?Code=1");
            //    }
            //}

            //// Kiểm tra có đang Update hay không
            //if (Config.UpdateSystem == "1" && Request.CurrentExecutionFilePathExtension.ToLower() == ".aspx" && Request.AppRelativeCurrentExecutionFilePath != "~/UpdateSystem.aspx")
            //{
            //    HttpCookie cookUpdate = Request.Cookies["IsTest"];

            //    // Nếu không tìm thấy => Chuyển qua Page SystemUpdate
            //    if (cookUpdate == null) Response.Redirect("~/UpdateSystem.aspx");
            //    else
            //    {
            //        // Nếu giá trị không hợp lệ => Chuyển qua Page SystemUpdate
            //        if (cookUpdate.Value != "1") Response.Redirect("~/UpdateSystem.aspx");
            //    }
            //}
        }
    }
}