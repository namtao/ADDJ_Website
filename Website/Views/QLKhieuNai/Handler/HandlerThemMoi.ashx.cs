using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.SessionState;
using Website.AppCode;
using Website.AppCode.Controller;
using Website.Components.Info;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for HandlerThemMoi
    /// </summary>
    public class HandlerThemMoi : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string type = context.Request.Form["key"] ?? context.Request.QueryString["key"];

            switch (type)
            {
                case "1":
                    BindLoaiKhieuNai(context);
                    break;
                case "BindLoaiKhieuNaiNew":
                    BindLoaiKhieuNaiNew(context);
                    break;
                case "2":
                    BindLinhVucChung(context);
                    break;
                case "3":
                    BindLinhVucCon(context);
                    break;
                case "4":
                    BindTinh(context);
                    break;
                case "5":
                    BindQuanHuyen(context);
                    break;
                case "6":
                    BindPhuongXa(context);
                    break;
                case "Add":
                    ThemMoiKhieuNai(context);
                    break;

                case "UploadFile":
                    UploadFile(context);
                    break;
            }
        }

        private void ThemMoiKhieuNai(HttpContext context)
        {
            try
            {
                DateTime timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();
                AdminInfo userLogin = LoginAdmin.AdminLogin();
                if (userLogin == null)
                {
                    context.Response.Write(OutputJSONToAJax.ToJSON(-1000));
                }

                KhieuNaiInfo info = new KhieuNaiInfo();
                info.LUser = userLogin.Username;

                info.SoThueBao = Convert.ToInt64(context.Request.Form["SoThueBao"] ?? context.Request.QueryString["SoThueBao"]);


                info.KhuVucId = userLogin.KhuVucId;
                info.DoiTacId = userLogin.DoiTacId;
                info.PhongBanTiepNhanId = userLogin.PhongBanId;

                info.PhongBanXuLyId = 0;

                info.LoaiKhieuNaiId = Convert.ToInt32(context.Request.Form["LoaiKhieuNaiId"] ?? context.Request.QueryString["LoaiKhieuNaiId"]);
                info.LinhVucChungId = Convert.ToInt32(context.Request.Form["LinhVucChungId"] ?? context.Request.QueryString["LinhVucChungId"]);
                info.LinhVucConId = Convert.ToInt32(context.Request.Form["LinhVucConId"] ?? context.Request.QueryString["LinhVucConId"]);
                info.LoaiKhieuNai = context.Request.Form["LoaiKhieuNai"] ?? context.Request.QueryString["LoaiKhieuNai"];

                if (info.LinhVucChungId != 0)
                    info.LinhVucChung = context.Request.Form["LinhVucChung"] ?? context.Request.QueryString["LinhVucChung"];

                if (info.LinhVucConId != 0)
                    info.LinhVucCon = context.Request.Form["LinhVucCon"] ?? context.Request.QueryString["LinhVucCon"];

                info.DoUuTien = Convert.ToByte(context.Request.Form["DoUuTien"] ?? context.Request.QueryString["DoUuTien"]);

                int MaTinhId = ConvertUtility.ToInt32(context.Request.Form["TinhId"] ?? context.Request.QueryString["TinhId"]);
                info.MaTinhId = MaTinhId;
                if (MaTinhId != 0) info.MaTinh = context.Request.Form["Tinh"] ?? context.Request.QueryString["Tinh"];
                info.MaQuanId = Convert.ToInt32(context.Request.Form["QuanHuyenId"] ?? context.Request.QueryString["QuanHuyenId"]);

                if (info.MaQuanId != 0) info.MaQuan = context.Request.Form["QuanHuyen"] ?? context.Request.QueryString["QuanHuyen"];

                info.MaPhuongId = Convert.ToInt32(context.Request.Form["PhuongXaId"] ?? context.Request.QueryString["PhuongXaId"]);
                if (info.MaPhuongId != 0) info.MaPhuong = context.Request.Form["PhuongXa"] ?? context.Request.QueryString["PhuongXa"];

                info.HoTenLienHe = context.Request.Form["HoTen"] ?? context.Request.QueryString["HoTen"];
                info.DiaChi_CCBS = context.Request.Form["DiaChi"] ?? context.Request.QueryString["DiaChi"];
                info.DiaChiLienHe = context.Request.Form["DiaChiLienHe"] ?? context.Request.QueryString["DiaChiLienHe"];
                info.SDTLienHe = context.Request.Form["DTLienHe"] ?? context.Request.QueryString["DTLienHe"];
                info.DiaDiemXayRa = context.Request.Form["DiaDiemSuCo"] ?? context.Request.QueryString["DiaDiemSuCo"];
                info.ThoiGianXayRa = context.Request.Form["ThoiGianSuCo"] ?? context.Request.QueryString["ThoiGianSuCo"];
                info.NoiDungPA = context.Request.Form["NoiDung"] ?? context.Request.QueryString["NoiDung"];
                info.NoiDungCanHoTro = context.Request.Form["NoiDungHoTro"] ?? context.Request.QueryString["NoiDungHoTro"];
                info.GhiChu = context.Request.Form["GhiChu"] ?? context.Request.QueryString["GhiChu"];
                info.TrangThai = (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                info.IsChuyenBoPhan = false;

                info.NguoiTiepNhanId = userLogin.Id;
                info.NguoiTiepNhan = userLogin.Username;

                info.HTTiepNhan = Convert.ToByte(context.Request.Form["HTTN"] ?? context.Request.QueryString["HTTN"]);
                info.NgayTiepNhan = timeNow;
                info.NgayTiepNhanSort = Convert.ToInt32(timeNow.ToString("yyyyMMdd"));

                // Tính thời gian deadline cho loại khiếu nại.
                // Nếu chọn lĩnh vực con
                int loaiKNIdSelect = info.LoaiKhieuNaiId;
                if (info.LinhVucConId != 0)
                {
                    loaiKNIdSelect = info.LinhVucConId;
                }
                //Nếu chọn lĩnh vực chung
                else if (info.LinhVucChungId != 0)
                {
                    loaiKNIdSelect = info.LinhVucChungId;
                }

                // Tính ngày quá hạn và ngày cảnh báo
                info.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect);
                info.NgayQuaHanSort = Convert.ToInt32(info.NgayQuaHan.ToString("yyyyMMdd"));

                info.NgayCanhBao = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 2);
                info.NgayCanhBaoSort = Convert.ToInt32(info.NgayCanhBao.ToString("yyyyMMdd"));

                info.DoiTacXuLyId = info.DoiTacId;
                info.KhuVucXuLyId = info.KhuVucId;
                info.PhongBanXuLyId = info.PhongBanTiepNhanId;

                info.NguoiXuLyId = info.NguoiTiepNhanId;
                info.NguoiXuLy = info.NguoiTiepNhan;

                //Người tiền xử lý phòng ban. Đây là người tiếp nhận khiếu nại của phòng ban
                info.NguoiTienXuLyCap1Id = 0;
                info.NguoiTienXuLyCap1 = string.Empty;

                info.NgayChuyenPhongBan = timeNow;
                info.NgayChuyenPhongBanSort = Convert.ToInt32(info.NgayChuyenPhongBan.ToString("yyyyMMdd"));

                info.NgayQuaHanPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 1, null, null);
                info.NgayCanhBaoPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 2, null, null);

                info.NgayQuaHanPhongBanXuLySort = Convert.ToInt32(info.NgayQuaHanPhongBanXuLy.ToString("yyyyMMdd"));
                info.NgayCanhBaoPhongBanXuLySort = Convert.ToInt32(info.NgayCanhBaoPhongBanXuLy.ToString("yyyyMMdd"));

                info.NgayTraLoiKN = DateTime.MaxValue;
                info.NgayTraLoiKNSort = 0;
                info.NgayDongKN = DateTime.MaxValue;
                info.NgayDongKNSort = 0;

                string strKhieuNaiFrom = context.Request.Form["KhieuNaiFrom"] ?? context.Request.QueryString["KhieuNaiFrom"];
                info.KhieuNaiFrom = ConvertUtility.ToInt32(strKhieuNaiFrom, 0);

                string strLoaiThueBao = context.Request.Form["LoaiThueBao"] ?? context.Request.QueryString["LoaiThueBao"];
                if (strLoaiThueBao.Equals("1")) info.IsTraSau = true;

                //item.DoHaiLong = (int)KhieuNai_DoHaiLong_Type.Chưa_đóng_khiếu_nại;
                //var hdIsCallService = context.Request.Form["IsCallService"] ?? context.Request.QueryString["IsCallService"];
                //var hdIsThueBao = context.Request.Form["IsThueBao"] ?? context.Request.QueryString["IsThueBao"];
                //if (hdIsCallService.Equals("1"))
                //{
                //    if (hdIsThueBao.Equals("1"))
                //        item.IsTraSau = true;
                //}
                //else
                //{
                //    if (!Config.IsLocal)
                //    {
                //        try
                //        {
                //            ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
                //            ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
                //            requestParam.KhuVucId = userLogin.KhuVucId;
                //            requestParam.Username = userLogin.Username;
                //            requestParam.SoThueBao = "84" + item.SoThueBao.ToString();

                //            var infoThueBao = obj.GetInfo(requestParam);
                //            if (infoThueBao != null)
                //            {
                //                if (infoThueBao.TEN_LOAI == "Post" || infoThueBao.TEN_LOAI.IndexOf("iTouch") != -1)
                //                {
                //                    item.IsTraSau = true;
                //                }
                //            }
                //        }
                //        catch (Exception ex) { }
                //    }
                //}

                KhieuNai_ActivityInfo activiteInfo = null;

                // Activity
                activiteInfo = new KhieuNai_ActivityInfo();

                activiteInfo.ActivityTruoc = 0;
                activiteInfo.GhiChu = "Tạo mới khiếu nại";
                activiteInfo.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                activiteInfo.IsCurrent = true;
                activiteInfo.NguoiXuLyTruoc = userLogin.Username;
                activiteInfo.NguoiXuLy = userLogin.Username;
                activiteInfo.PhongBanXuLyTruocId = info.PhongBanTiepNhanId;
                activiteInfo.PhongBanXuLyId = info.PhongBanXuLyId;
                activiteInfo.NgayTiepNhan = info.NgayTiepNhan;
                activiteInfo.NgayQuaHan = info.NgayQuaHanPhongBanXuLy;
                activiteInfo.NgayCanhBao = info.NgayCanhBaoPhongBanXuLy;
                activiteInfo.NgayTiepNhan_NguoiXuLy = timeNow;
                activiteInfo.NgayTiepNhan_NguoiXuLyTruoc = timeNow;
                activiteInfo.NgayTiepNhan_PhongBanXuLyTruoc = timeNow;
                activiteInfo.NgayQuaHan_PhongBanXuLyTruoc = info.NgayQuaHanPhongBanXuLy;

                bool flag = false;

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        info.Id = ServiceFactory.GetInstanceKhieuNai().Add(info);
                        activiteInfo.KhieuNaiId = info.Id;
                        activiteInfo.Id = ServiceFactory.GetInstanceKhieuNai_Activity().Add(activiteInfo);

                        scope.Complete();
                        flag = true;
                    }
                    catch (TransactionAbortedException tae)
                    {
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                        Utility.LogEvent(tae);
                        return;
                    }
                    catch (Exception ex)
                    {
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                        Utility.LogEvent(ex);
                        return;
                    }
                }

                if (flag)
                {
                    KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                    buocXuLyInfo.NoiDung = "Tạo mới khiếu nại.";
                    buocXuLyInfo.LUser = info.NguoiTiepNhan;
                    buocXuLyInfo.KhieuNaiId = info.Id;
                    buocXuLyInfo.IsAuto = true;
                    buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                    buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                    KhieuNai_LogInfo logInfo = new KhieuNai_LogInfo();
                    logInfo.KhieuNaiId = info.Id;
                    logInfo.TruongThayDoi = "[Hành động]";
                    logInfo.GiaTriCu = string.Empty;
                    logInfo.ThaoTac = "Thêm mới khiếu nại";
                    logInfo.PhongBanId = userLogin.PhongBanId;
                    logInfo.CUser = userLogin.Username;
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(logInfo);
                }

                string messgeChuyenTiep = "1";
                string IsChuyenTiep = context.Request.Form["IsChuyenTiep"] ?? context.Request.QueryString["IsChuyenTiep"];

                if (IsChuyenTiep.Equals("1") || IsChuyenTiep.Equals("true"))
                    messgeChuyenTiep = ChuyenTiepKhieuNai(userLogin, loaiKNIdSelect, info, activiteInfo);

                context.Response.Write(OutputJSONToAJax.ToJSON(0, messgeChuyenTiep, info.Id.ToString()));
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(OutputJSONToAJax.ToJSON(99, "Có lỗi xảy ra khi thêm mới khiếu nại. Bạn thử lại vào lúc khác."));
            }
        }

        private string ChuyenTiepKhieuNai(AdminInfo userLogin, int loaiKNIdSelect, KhieuNaiInfo item, KhieuNai_ActivityInfo activityCurr)
        {
            //Nếu chuyển KN ngay
            string strReturn = "1";


            PhongBanInfo phongBanItem = ServiceFactory.GetInstancePhongBan().GetInfo(userLogin.PhongBanId);
            if (phongBanItem.IsDinhTuyenKN)
            {
                strReturn = "2";
                try
                {
                    //Lấy ra List phòng ban chuyển của phòng ban mình
                    List<PhongBan2PhongBanInfo> lstPhongBanDuocChuyen = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(userLogin.PhongBanId);
                    if (lstPhongBanDuocChuyen == null || lstPhongBanDuocChuyen.Count == 0)
                    {
                        return "Không tìm thấy phòng ban nào cần chuyển khiếu nại";
                    }

                    List<int> phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<int>>(lstPhongBanDuocChuyen[0].PhongBanDen);
                    if (phongBanDuocChuyen_JSON == null || phongBanDuocChuyen_JSON.Count == 0)
                    {
                        return "Không tìm thấy phòng ban nào cần chuyển khiếu nại.";
                    }

                    // Nếu chỉ có 1 phòng ban được chuyển khiếu nại => Tất cả khiếu nại sẽ được chuyển đến đây.
                    if (phongBanDuocChuyen_JSON.Count == 1)
                    {
                        item.PhongBanXuLyId = phongBanDuocChuyen_JSON[0];
                    }
                    else // Nếu lớn hơn 2 phòng ban thì cần xét duyệt từng loại khiếu nại đến phòng ban nào trong list.
                    {
                        // Lấy ra phòng ban có thể xử lý loại khiếu nại.
                        var lstPhongBanXuLy = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByLoaiKhieuNaiId(loaiKNIdSelect);
                        if (lstPhongBanXuLy == null || lstPhongBanXuLy.Count == 0)
                        {
                            return "Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.";
                        }
                        // Kiem tra xem phong ban co khong
                        bool isExistsPhongBanXuLy = false;
                        foreach (LoaiKhieuNai2PhongBanInfo lstPhongBanXuLyItem in lstPhongBanXuLy)
                            if (phongBanDuocChuyen_JSON.Contains(lstPhongBanXuLyItem.PhongBanId))
                            {
                                item.PhongBanXuLyId = lstPhongBanXuLyItem.PhongBanId;
                                isExistsPhongBanXuLy = true;
                                break;
                            }
                        if (!isExistsPhongBanXuLy)
                        {
                            return "Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.";
                        }
                    }

                    //Lấy ra loại phòng ban của phòng ban xử lý
                    int LoaiPhongBanXuLy = 0;

                    PhongBanInfo PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

                    if (PhongBanXuLyItem != null)
                        LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

                    if (LoaiPhongBanXuLy == 0)
                    {
                        return "Phòng ban xử lý khiếu nại chưa thuộc loại phòng ban.";
                    }

                    item.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                    item.KhuVucXuLyId = PhongBanXuLyItem.KhuVucId;

                    item.NguoiTienXuLyCap1Id = 0;
                    item.NguoiTienXuLyCap1 = string.Empty;

                    //Người tiền xử lý.
                    item.NguoiTienXuLyCap2Id = item.NguoiXuLyId;
                    item.NguoiTienXuLyCap2 = item.NguoiXuLy;

                    item.NguoiXuLyId = 0;
                    item.NguoiXuLy = string.Empty;

                    //Thời gian xử lý trước đó của phòng ban
                    //var lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(item.Id, item.PhongBanXuLyId);

                    item.NgayChuyenPhongBan = item.NgayChuyenPhongBan;
                    item.NgayChuyenPhongBanSort = Convert.ToInt32(item.NgayChuyenPhongBan.ToString("yyyyMMdd"));

                    item.NgayQuaHanPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(item.NgayChuyenPhongBan, loaiKNIdSelect, LoaiPhongBanXuLy, 1);
                    item.NgayQuaHanPhongBanXuLySort = Convert.ToInt32(item.NgayQuaHanPhongBanXuLy.ToString("yyyyMMdd"));

                    item.NgayCanhBaoPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(item.NgayChuyenPhongBan, loaiKNIdSelect, LoaiPhongBanXuLy, 2);
                    item.NgayCanhBaoPhongBanXuLySort = Convert.ToInt32(item.NgayCanhBaoPhongBanXuLy.ToString("yyyyMMdd"));

                    item.LUser = userLogin.Username;



                    KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
                    itemActivity.KhieuNaiId = Convert.ToInt32(item.Id);
                    itemActivity.ActivityTruoc = activityCurr.Id;
                    itemActivity.GhiChu = "Chuyển xử lý khiếu nại";
                    itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
                    itemActivity.IsCurrent = true;
                    itemActivity.NguoiXuLyTruoc = userLogin.Username;
                    itemActivity.PhongBanXuLyTruocId = userLogin.PhongBanId;
                    itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
                    itemActivity.NgayTiepNhan = item.NgayTiepNhan;
                    itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                    itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;

                    itemActivity.NgayTiepNhan_PhongBanXuLyTruoc = activityCurr.NgayTiepNhan;
                    itemActivity.NgayQuaHan_PhongBanXuLyTruoc = activityCurr.NgayQuaHan;
                    itemActivity.NgayTiepNhan_NguoiXuLyTruoc = activityCurr.NgayTiepNhan_NguoiXuLy;
                    itemActivity.NgayTiepNhan_PhongBanXuLyTruoc = activityCurr.NgayTiepNhan;

                    bool flag = false;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            ServiceFactory.GetInstanceKhieuNai().Update(item);

                            ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(activityCurr.Id, item.Id, userLogin.Username);

                            ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);

                            scope.Complete();
                            flag = true;
                        }
                        catch (TransactionAbortedException tae)
                        {
                            Utility.LogEvent(tae);
                            return Constant.MESSAGE_SERVER_QUA_TAI;
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            return ex.Message;
                        }
                    }
                    if (flag)
                    {
                        KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                        buocXuLyInfo.NoiDung = "Chuyển khiếu nại tới " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                        buocXuLyInfo.LUser = userLogin.Username;
                        buocXuLyInfo.KhieuNaiId = item.Id;
                        buocXuLyInfo.IsAuto = true;
                        buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                        buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                        KhieuNai_LogInfo logInfo = new KhieuNai_LogInfo();
                        logInfo.KhieuNaiId = item.Id;
                        logInfo.TruongThayDoi = "[Hành động]";
                        logInfo.GiaTriCu = string.Empty;
                        logInfo.ThaoTac = buocXuLyInfo.NoiDung;
                        logInfo.CUser = userLogin.Username;
                        logInfo.PhongBanId = userLogin.PhongBanId;
                        ServiceFactory.GetInstanceKhieuNai_Log().Add(logInfo);
                    }
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    return ex.Message;
                }
            }


            return strReturn;
        }

        private void UploadFile(HttpContext context)
        {
            try
            {
                var htc = context.Request.Files;
                var MaKN = context.Request.Form["KhieuNaiId"] ?? context.Request.QueryString["KhieuNaiId"];

                if (htc.Count == 0)
                {
                    context.Response.Write(OutputJSONToAJax.ToJSON(0));
                    context.Response.End();
                    return;
                }

                var file1 = htc[0];

                if (file1.ContentLength <= 0)
                {
                    context.Response.Write(OutputJSONToAJax.ToJSON(0));
                    context.Response.End();

                    return;
                }
                var userLogin = LoginAdmin.AdminLogin();


                KhieuNaiInfo info = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(MaKN));
                KhieuNai_FileDinhKemInfo fileInfo = new KhieuNai_FileDinhKemInfo();

                fileInfo.TenFile = Path.GetFileName(file1.FileName);
                fileInfo.LoaiFile = Path.GetExtension(file1.FileName);
                fileInfo.KichThuoc = (decimal)(file1.ContentLength / 1024);

                byte[] bytes = null;

                var strFileName = AIVietNam.GQKN.Impl.KhieuNai_FileDinhKemImpl.UploadFileDelTemp(file1, ConvertUtility.ToInt32(MaKN), info.CDate);

                fileInfo.KhieuNaiId = info.Id;
                fileInfo.GhiChu = "File KH gửi";
                fileInfo.Status = (byte)FileDinhKem_Status.File_KH_Gửi;
                fileInfo.CUser = userLogin.Username;
                fileInfo.URLFile = strFileName;

                var id = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Add(fileInfo);
                if (id > 0)
                {
                    BuildKhieuNai_Log.LogKhieuNai(info.Id, "Thêm mới file đính kèm", "File đính kèm", "", fileInfo.TenFile);
                }
                else
                {
                    BuildKhieuNai_Log.LogKhieuNai(info.Id, "Thêm mới thất bại", "File đính kèm", "", fileInfo.TenFile);
                }

                fileInfo.Id = id;
                fileInfo.URLFile = Config.DomainDownload + "/" + strFileName;
                fileInfo.LoaiFile = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                fileInfo.ErrorId = 0;
                context.Response.Write(OutputJSONToAJax.ToJSON(0));
                context.Response.End();
            }
            catch (Exception ex)
            {
                context.Response.Write(OutputJSONToAJax.ToJSON(0));
                context.Response.End();
                Utility.LogEvent(ex);
            }
        }

        private void BindTinh(HttpContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId is null OR ParentId = 0", "Name");
                lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Tỉnh/Thành Phố--" });

                if (lstTinh != null && lstTinh.Count > 0)
                {
                    foreach (var item in lstTinh)
                    {
                        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                    }
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindQuanHuyen(HttpContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string TinhId = context.Request.Form["TinhId"] ?? context.Request.QueryString["TinhId"];
                if (TinhId != null && TinhId.Length > 0)
                {
                    var lstQuanHuyen = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + TinhId, "Name");
                    lstQuanHuyen.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Quận/Huyện--" });

                    if (lstQuanHuyen != null && lstQuanHuyen.Count > 0)
                    {
                        foreach (var item in lstQuanHuyen)
                        {
                            sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                        }
                    }
                    context.Response.Write(sb.ToString());
                }
                else
                {
                    context.Response.Write("");
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/09/2015
        /// Todo : Load danh sách Phường/Xã theo Quận/Huyện Id
        /// </summary>
        /// <param name="context"></param>
        private void BindPhuongXa(HttpContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string quanHuyenId = context.Request.Form["QuanHuyenId"] ?? context.Request.QueryString["QuanHuyenId"];
                if (quanHuyenId != null && quanHuyenId.Length > 0)
                {
                    var lstPhuongXa = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + quanHuyenId, "Name");
                    lstPhuongXa.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Phường/Xã--" });

                    if (lstPhuongXa != null && lstPhuongXa.Count > 0)
                    {
                        foreach (var item in lstPhuongXa)
                        {
                            sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                        }
                    }
                    context.Response.Write(sb.ToString());
                }
                else
                {
                    context.Response.Write("");
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindLoaiKhieuNai(HttpContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var lst = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", "cap = 1", "sort");
                sb.Append("<option value='0' title='--Loại khiếu nại--'>--Loại khiếu nại--</option>");
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                    }
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindLoaiKhieuNaiNew(HttpContext context)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                List<LoaiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", "cap = 1 and (Status = 1 or Status=2)", "sort");
                sb.Append("<option value='0' title='--Loại khiếu nại--'>-- Loại khiếu nại --</option>");
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                    }
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindLinhVucChung(HttpContext context)
        {
            try
            {
                string LoaiKhieuNaiId = context.Request.Form["LoaiKhieuNaiId"] ?? context.Request.QueryString["LoaiKhieuNaiId"];

                StringBuilder sb = new StringBuilder();
                List<LoaiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic(string.Empty, "Cap = 2 AND (Status = 1 OR Status=2) AND ParentId=" + LoaiKhieuNaiId, "sort");
                sb.Append("<option value='0' title='-- Loại khiếu nại --'>-- Lĩnh vực chung --</option>");
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                    }
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void BindLinhVucCon(HttpContext context)
        {
            try
            {
                string LoaiKhieuNaiId = context.Request.Form["LoaiKhieuNaiId"] ?? context.Request.QueryString["LoaiKhieuNaiId"];
                string LinhVucChungId = context.Request.Form["LinhVucChungId"] ?? context.Request.QueryString["LinhVucChungId"];

                StringBuilder sb = new StringBuilder();

                string strWhereClause = "cap = 3 and (Status = 1 or Status=2)";
                if (LoaiKhieuNaiId != null && !LoaiKhieuNaiId.Equals("0")) strWhereClause += " AND ParentLoaiKhieuNaiId = " + LoaiKhieuNaiId;
                if (LinhVucChungId != null && !LinhVucChungId.Equals("0")) strWhereClause += " AND ParentId = " + LinhVucChungId;

                List<LoaiKhieuNaiInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", strWhereClause, "sort");
                sb.Append("<option value='0' title='-- Loại khiếu nại --' LoaiKhieuNaiId='0' LinhVucChungId='0'>-- Lĩnh vực con --</option>");
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        sb.AppendFormat("<option value='{0}' title='{1}' LoaiKhieuNaiId='{2}' LinhVucChungId='{3}' LoaiKhieuNai='{4}'>{1}</option>", item.Id, item.Name, item.ParentLoaiKhieuNaiId, item.ParentId, item.NameLoaiKhieuNai);
                    }
                }
                context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
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