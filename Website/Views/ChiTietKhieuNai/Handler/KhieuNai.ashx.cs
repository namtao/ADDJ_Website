using AIVietNam.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Website.AppCode.Controller;
using System.Text;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.Components.Info;

namespace Website.Views.ChiTietKhieuNai.Handler
{
    /// <summary>
    /// Summary description for KhieuNai
    /// </summary>
    public class KhieuNai : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.Form["type"] ?? context.Request.QueryString["type"];

            switch (type)
            {
                case "AddCountCall":
                    AddCountCall(context);
                    break;
                case "LoadPhongBanChuyenXuLy":
                    LoadPhongBanChuyenXuLy(context);
                    break;
                case "LoadUserInPhongBanChuyenXuLy":
                    LoadUserInPhongBanChuyenXuLy(context);
                    break;
                case "ChuyenXuLy":
                    ChuyenXuLy(context);
                    break;
                case "CheckDinhTuyen":
                    CheckDinhTuyen(context);
                    break;
                case "CheckKetQuaXuLyDong":
                    CheckKetQuaXuLyDong(context);
                    break;
                case "CheckTiepNhan":
                    CheckTiepNhan(context);
                    break;
                case "PhongBanNhanPhanHoi":
                    PhongBanNhanPhanHoi(context);
                    break;
                case "LoadTuDongDinhTuyenAndPhongBanCungDoiTac":
                    LoadTuDongDinhTuyenAndPhongBanCungDoiTac(context);
                    break;
            }
        }

        private void AddCountCall(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                if (userLogin == null)
                {
                    context.Response.Write(OutputJSONToAJax.ToJSON(1, "Bạn chưa đăng nhập hệ thống."));
                }

                var maKhieuNai = context.Request.Form["MaKhieuNai"] ?? context.Request.QueryString["MaKhieuNai"];
                maKhieuNai = maKhieuNai.Replace("PA-", "").Trim();

                var KhieuNaiId = ConvertUtility.ToInt32(maKhieuNai);
                var KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai().GetInfo(KhieuNaiId);
                if (KhieuNaiItem != null && KhieuNaiItem.Id > 0)
                {
                    if (KhieuNaiItem.TrangThai == (short)KhieuNai_TrangThai_Type.Đóng)
                    {
                        context.Response.Write(OutputJSONToAJax.ToJSON(1, "Khiếu nại đã đóng trên hệ thống."));
                        return;
                    }

                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("CallCount=CallCount+1", "Id=" + KhieuNaiItem.Id);
                    context.Response.Write(OutputJSONToAJax.ToJSON(0, (KhieuNaiItem.CallCount + 1).ToString()));
                }
                else
                    context.Response.Write(OutputJSONToAJax.ToJSON(1, "Không tìm thấy mã khiếu nại."));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(OutputJSONToAJax.ToJSON(1, "Có lỗi xảy ra khi cập nhật hệ thống, bạn thử lại vào lúc khác."));
            }
        }

        private void LoadPhongBanChuyenXuLy(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var lst = BuildPhongBan.GetListPhongBanChuyenXuLy(userLogin.PhongBanId);
                StringBuilder sb = new StringBuilder();
                sb.Append("<option value=\"0\">[ Chọn phòng ban chuyển xử lý ]</option>");

                //Neu dinh tuyen len Vinaphone
                if (userLogin.IsChuyenVNP)
                {
                    //var lstPhongBanCha = lst.Where(t => t.Cap == infoUser.CapPhongBan - 1 && t.DoiTacId == admin.DoiTacId);
                    sb.Append("<option value=\"-1\">Chuyển xử lý lên Vinaphone</option>");
                    //listLoadPhongBan.Insert(0, new PhongBanInfo() { Id = -1, Name = "Chuyển xử lý lên Vinaphone." });
                }

                //Lay ra cac phong ban cha
                if (userLogin.CapPhongBan > 1)
                {
                    var lstPhongBanCha = ServiceFactory.GetInstancePhongBan().GetListDynamic("", "Cap = " + (userLogin.CapPhongBan - 1) + " AND DoiTacId=" + userLogin.DoiTacId, "");
                    foreach (var itemCha in lstPhongBanCha)
                    {
                        if (!lst.Any(t => t.Id == itemCha.Id))
                        {
                            lst.Add(itemCha);
                        }
                    }
                }

                foreach (var item in lst)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("");
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/05/2014
        /// Todo : Hiển thị giá trị tự động định tuyến và các phòng ban cùng khu vực
        /// </summary>
        /// <param name="context"></param>
        private void LoadTuDongDinhTuyenAndPhongBanCungDoiTac(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                StringBuilder sb = new StringBuilder();
                sb.Append("<option value=\"0\">[ Khiếu nại sẽ được tự động định tuyến đến phòng ban xử lý ]</option>");

                if (userLogin.IsChuyenVNP)
                {
                    sb.AppendFormat("<option value=\"-1\">{0}</option>", "Chuyển xử lý lên Vinaphone");
                }

                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_khiếu_nại_cho_phòng_ban_cấp_dưới_xử_lý))
                {
                    string whereClause = string.Format("DoiTacId = {0} AND Id <> {1}", userLogin.DoiTacId, userLogin.PhongBanId);
                    var lst = new PhongBanImpl().GetListDynamic("*", whereClause, "Name ASC");
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
                    }
                }

                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("");
            }
        }

        private void LoadUserInPhongBanChuyenXuLy(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var phongBanId = context.Request.Form["PhongBanId"] ?? context.Request.QueryString["PhongBanId"];


                var lst = ServiceFactory.GetInstancePhongBan_User().GetListDynamicJoin("b.Id, b.TenTruyCap", "LEFT JOIN NguoiSuDung b on a.NguoiSuDungId = b.Id", "b.TrangThai = 1 AND PhongBanId=" + phongBanId, "");
                StringBuilder sb = new StringBuilder();
                sb.Append("<option value=\"\">[ Chọn người sử dụng tiếp nhận ]</option>");
                foreach (var item in lst)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.TenTruyCap, item.TenTruyCap);
                }
                context.Response.Write(sb.ToString());

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("");
            }
        }
        // Edited by	: Dao Van Duong
        // Datetime		: 3.8.2016 13:46
        // Note			: Chuyển xử lý KN
        private void ChuyenXuLy(HttpContext context)
        {
            MessageInfo ret = new MessageInfo();

            try
            {
                AdminInfo userLogin = LoginAdmin.AdminLogin();
                string phongBanId = context.Request.Form["PhongBanId"] ?? context.Request.QueryString["PhongBanId"];
                string userNhanKN = context.Request.Form["Username"] ?? context.Request.QueryString["Username"];
                if (userNhanKN == null) userNhanKN = string.Empty;
                string KhieuNaiId = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];
                string Mode = context.Request.Form["Mode"] ?? context.Request.QueryString["Mode"];
                string Note = context.Request.Form["Note"] ?? context.Request.QueryString["Note"];

                if (Mode.ToLower().Equals("process"))
                {
                    try
                    {
                        ret = BuildKhieuNai_Activity.ActivityChuyenPhongBanToUserInPhongBan(Convert.ToInt32(KhieuNaiId), Convert.ToInt32(phongBanId), userNhanKN, KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban, Note);
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                        ret.Code = -1;
                        ret.Message = ex.Message;
                    }
                }
                else
                {
                    ret.Code = -1;
                    ret.Message = "Bạn đang ở chế độ khiếu nại, kiểm tra lại";
                }
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                ret.Code = -1;
                ret.Message = ex.Message;
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <return>
        ///     1 : Khiếu nại từ phòng ban này sẽ không được tự động định tuyến sang phòng ban kế tiếp
        ///     2 : Khiếu nại từ phòng ban này sẽ được tự động định tuyến sang phòng ban kế tiếp
        ///     other : Lỗi exception
        /// </return>
        private void CheckDinhTuyen(HttpContext context)
        {
            try
            {
                AdminInfo userLogin = LoginAdmin.AdminLogin();
                PhongBanInfo pbItem = ServiceFactory.GetInstancePhongBan().GetInfo(userLogin.PhongBanId);
                if (pbItem == null)
                {
                    context.Response.Write("Không tìm thấy phòng ban trong hệ thống.");
                    return;
                }

                if (pbItem.IsDinhTuyenKN)
                    context.Response.Write(2);
                else
                    context.Response.Write(1);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(ex.Message);
            }
        }



        private void CheckKetQuaXuLyDong(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var khieuNaiId = context.Request.Form["KhieuNaiId"] ?? context.Request.QueryString["KhieuNaiId"];
                string selectClause = "Top 1 *";
                string whereClause = string.Format("KhieuNaiId={0}", khieuNaiId);
                string orderClause = "Id desc";
                var lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                if (lstCheck != null && lstCheck.Count > 0)
                {
                    var item = lstCheck[0];
                    if (!item.IsAuto && item.CUser.Equals(userLogin.Username))
                    {
                        context.Response.Write(0);
                    }
                    else
                        context.Response.Write(1);
                }
                else
                {
                    context.Response.Write(1);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(ex.Message);
            }
        }

        private void CheckTiepNhan(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var khieuNaiId = context.Request.Form["KhieuNaiId"] ?? context.Request.QueryString["KhieuNaiId"];
                var item = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(khieuNaiId));
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.NguoiXuLy) || item.NguoiXuLy == userLogin.Username)
                    {
                        context.Response.Write("0");
                    }
                    else
                    {
                        if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_KN_phản_hồi_của_phòng_ban_sau_thời_gian_cấu_hình)
                        && (DateTime.Now - item.LDate).TotalHours >= Config.TimeEditKhieuNai)
                        {
                            context.Response.Write("0");
                        }
                        else
                        {
                            context.Response.Write("1");
                        }
                    }
                }
                else
                {
                    context.Response.Write("1");
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("1");
            }
        }

        private void PhongBanNhanPhanHoi(HttpContext context)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var khieuNaiId = context.Request.Form["KhieuNaiId"] ?? context.Request.QueryString["KhieuNaiId"];
                var lst = ServiceFactory.GetInstanceKhieuNai_Activity().GetListPhongBanPhanHoi(ConvertUtility.ToInt32(khieuNaiId), userLogin.PhongBanId);
                StringBuilder sb = new StringBuilder();
                if (lst != null)
                {
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.PhongBanXuLyTruocId, item.PhongBan_Name);
                    }
                    context.Response.Write(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("1");
            }

        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}