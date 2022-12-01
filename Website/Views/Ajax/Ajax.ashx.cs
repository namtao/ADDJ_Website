using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using System.Web.SessionState;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.Ajax
{
    /// <summary>
    /// Summary description for Ajax
    /// </summary>
    public class Ajax : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request.Form["type"] ?? context.Request.QueryString["type"];
            switch (ConvertUtility.ToString(type))
            {
                case "rf":
                    DeleteFile(context);
                    break;
                case "uf":
                    UpdateFile(context);
                    break;
                case "DongKN":
                    DongKN(context);
                    break;
                case "DongKNHaiLong":
                    DongKNHaiLong(context);
                    break;
                case "ChuyenKNAuto":
                    ChuyenKNAuto(context);
                    break;
                case "ChuyenKNAutoVNP":
                    ChuyenKNAutoVNP(context);
                    break;
                case "ChuyenListKN":
                    ChuyenListKN(context);
                    break;
                case "ChuyenListKNVNP":
                    ChuyenListKNVNP(context);
                    break;
                case "KhoaPhieuVNPT":
                    KhoaPhieuVNPT(context);
                    break;
            }
            context.Response.End();
        }

        private void UpdateFile(HttpContext context)
        {
            string id = context.Request.Form["id"] ?? context.Request.QueryString["id"];
            KhieuNai_FileDinhKemInfo info = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetInfo(ConvertUtility.ToInt32(id));
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_thông_tin_file_đính_kèm))
            {
                //Báo lỗi và không thực hiện chức năng
                context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                return;
            }
            string ghichu = context.Request.Form["ghichu"] ?? context.Request.QueryString["ghichu"];
            int ret = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().UpdateDynamic(string.Format("GhiChu=N'{0}'", ConvertUtility.ToString(ghichu)), "Id=" + ConvertUtility.ToString(id));
            if (ret >= 0)
            {
                context.Response.Write("1");
                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(info.KhieuNaiId), "Sửa", "File đính kèm", "", info.TenFile);
                return;
            }
            context.Response.Write("0");
        }

        private void DeleteFile(HttpContext context)
        {
            //string fileExtension = Path.GetExtension(filename).ToLower();
            string id = context.Request.Form["id"] ?? context.Request.QueryString["id"];
            KhieuNai_FileDinhKemInfo info = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetInfo(ConvertUtility.ToInt32(id));
            if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_file_đính_kèm)
                || !BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_file_đính_kèm_của_mình))
            {
                //Báo lỗi và không thực hiện chức năng
                context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                return;
            }
            if (info != null)
            {
                string pathFile = Path.Combine(Config.PathUploadFile, info.URLFile);
                try
                {

                    var ret = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Delete(ConvertUtility.ToInt32(id));
                    if (ret > 0)
                    {
                        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(info.KhieuNaiId), "Xóa", "File đính kèm", "", info.TenFile);
                        //Xoa file thong qua FTP
                        string ftpServerIP = AIVietNam.Core.Config.FtpServerIP;
                        string ftpUserID = AIVietNam.Core.Config.FtpUserID;
                        string ftpPassWord = AIVietNam.Core.Config.FtpPassWord;
                        FTPClient ftpClient = new FTPClient(ftpServerIP, ftpUserID, ftpPassWord);
                        string[] arrFileName = info.URLFile.Split('/');
                        string fileName = "";
                        if (arrFileName.Length > 0)
                        {
                            fileName = arrFileName[arrFileName.Length - 1];
                        }
                        ftpClient.DeleteFTP(fileName, info.URLFile.Replace("/" + fileName, ""));
                        context.Response.Write(1);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message);
                    return;
                }
            }
            context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
        }

        #region Longlx
        private void DongKN(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];
                int resultService = 0;
                int nguyenNhanLoiId = LoiKhieuNaiInfo.LoiKhieuNaiValue.NGUYEN_NHAN_LOI_ID_KHAC;
                int chiTietLoiId = 0;
                int result = BuildKhieuNai.DongKhieuNai(ConvertUtility.ToInt32(id), string.Empty, ref resultService, nguyenNhanLoiId, chiTietLoiId, true);
                if (result != 0)
                    throw new Exception("Đóng khiếu nại có lỗi, bạn hãy thực hiện thao tác sau ít phút.");
                if (resultService != 0)
                    context.Response.Write(resultService);
            }
            catch (GQKNMessageException ge)
            {
                context.Response.Write(ge.Message);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("Chức năng đóng khiếu nại có lỗi, bạn hãy thực hiện thao tác sau ít phút.");
            }
        }

        private void DongKNHaiLong(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];
                string DoHaiLong = context.Request.Form["DoHaiLong"] ?? context.Request.QueryString["DoHaiLong"];
                int nguyenNhanLoiId = ConvertUtility.ToInt32(context.Request.Form["nguyenNhanLoiId"] ?? context.Request.QueryString["nguyenNhanLoiId"], LoiKhieuNaiInfo.LoiKhieuNaiValue.NGUYEN_NHAN_LOI_ID_KHAC);
                int chiTietLoiId = ConvertUtility.ToInt32(context.Request.Form["chiTietLoiId"] ?? context.Request.QueryString["chiTietLoiId"], 0);
                int resultService = 0;

                int nDoHaiLong = ConvertUtility.ToInt32(DoHaiLong, 2);
                int result = BuildKhieuNai.DongKhieuNai(ConvertUtility.ToInt32(id), string.Empty, ref resultService, nguyenNhanLoiId, chiTietLoiId, true, nDoHaiLong);
                if (result != 0)
                    throw new Exception("Đóng khiếu nại có lỗi, bạn hãy thực hiện thao tác sau ít phút.");
                if (resultService != 0)
                    context.Response.Write(resultService);
            }
            catch (GQKNMessageException ge)
            {
                context.Response.Write(ge.Message);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write("Chức năng đóng khiếu nại có lỗi, bạn hãy thực hiện thao tác sau ít phút.");
            }
        }

        private void ChuyenKNAuto(HttpContext context)
        {
            try
            {
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                {
                    context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                    return;
                }

                string id = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];

                string note = context.Request.Form["NoiDungXuLy"] ?? context.Request.QueryString["NoiDungXuLy"];

                ChuyenListKN(id, note);
            }
            catch (GQKNMessageException ge)
            {
                context.Response.Write(ge.Message);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
            }
        }

        private void ChuyenKNAutoVNP(HttpContext context)
        {
            try
            {
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                {
                    context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                    return;
                }

                string id = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];

                string note = context.Request.Form["NoiDungXuLy"] ?? context.Request.QueryString["NoiDungXuLy"];

                bool isChuyenLenVNP = true;
                ChuyenListKN(id, note, isChuyenLenVNP);
            }
            catch (GQKNMessageException ge)
            {
                context.Response.Write(ge.Message);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
            }
        }

        private void ChuyenListKN(string id, string note, bool isChuyenLenVNP = false)
        {
            AdminInfo userLogin = LoginAdmin.AdminLogin();

            if (userLogin != null && userLogin.PhongBanId == 0)
            {
                throw new GQKNMessageException("Người dùng này chưa được phân vào phòng nên không có quyền thao tác vào khiếu nại.");
            }

            //Kiểm tra xem có phải là người cuối cùng đăng nội dung xử lý
            bool isThemBuocXuLy = false;
            if (string.IsNullOrEmpty(note))
            {
                string selectClause = "Top 1 *";
                string whereClause = string.Format("KhieuNaiId={0}", id);
                string orderClause = "Id desc";
                var lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                if (lstCheck != null && lstCheck.Count > 0)
                {
                    var itemCheck = lstCheck[0];
                    if (!itemCheck.IsAuto && itemCheck.CUser.Equals(userLogin.Username))
                    {
                        note = itemCheck.NoiDung;
                        isThemBuocXuLy = true;
                    }
                    else
                    {
                        throw new GQKNMessageException("Bạn chưa nhập nội dung xử lý.");
                    }
                }
            }

            DateTime timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

            KhieuNaiInfo item = ServiceFactory.GetInstanceKhieuNai().GetInfo(Convert.ToInt32(id));

            int loaiKNIdSelect = item.LoaiKhieuNaiId;
            if (item.LinhVucConId != 0)
            {
                loaiKNIdSelect = item.LinhVucConId;
            }
            //Nếu chọn lĩnh vực chung
            else if (item.LinhVucChungId != 0)
            {
                loaiKNIdSelect = item.LinhVucChungId;
            }

            KhieuNai_ActivityInfo activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(item.Id);

            KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
            itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
            itemActivity.GhiChu = "Chuyển xử lý khiếu nại";

            //Nếu là chuyển phản hồi về đúng phòng ban tiếp nhận KN thì chuyển xử lý auto sẽ là chuyển phản hồi.
            item.IsPhanHoi = false;
            KhieuNai_ActivityInfo ActivityPhanHoi = null;
            if (item.PhongBanTiepNhanId == item.PhongBanXuLyId)
            {
                //Kiem tra xem khieu nai da duoc gui di phong ban khac hay chua.
                List<KhieuNai_ActivityInfo> lstCheckKhieuNaiPhanHoi = ServiceFactory.GetInstanceKhieuNai_Activity().GetListDynamic("*", string.Format("KhieuNaiId = {0} and PhongBanXuLyId = {1} and PhongBanXuLyTruocId != {1}", item.Id, item.PhongBanXuLyId), "");
                if (lstCheckKhieuNaiPhanHoi != null && lstCheckKhieuNaiPhanHoi.Count > 0)
                {
                    ActivityPhanHoi = lstCheckKhieuNaiPhanHoi[lstCheckKhieuNaiPhanHoi.Count - 1];
                }
            }

            //Tính ngày quá hạn và ngày cảnh báo
            //Thời gian xử lý trước đó của phòng ban
            //var lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(item.Id, item.PhongBanXuLyId);

            KhieuNai_ActivityInfo timeDSD = new KhieuNai_ActivityInfo();
            timeDSD.CDate = item.NgayTiepNhan;
            timeDSD.LDate = timeNow;
            item.NgayQuaHan = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 1, timeDSD);
            item.NgayQuaHanSort = Convert.ToInt32(item.NgayQuaHan.ToString("yyyyMMdd"));
            item.NgayCanhBao = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 2, timeDSD);
            item.NgayCanhBaoSort = Convert.ToInt32(item.NgayCanhBao.ToString("yyyyMMdd"));

            //Phòng ban cần chuyển KN          
            List<int> phongBanDuocChuyen_JSON = null;
            if (isChuyenLenVNP)  //Chuyển lên VNP luôn giành cho tỉnh
            {
                string strPhongBanDen = string.Empty;

                if (userLogin.KhuVucId == 2)
                {
                    strPhongBanDen = "[53,54,55,56,58,134]";
                }
                else if (userLogin.KhuVucId == 3)
                { strPhongBanDen = "[62,63,64,65,72,217]"; }
                else if (userLogin.KhuVucId == 5)
                { strPhongBanDen = "[67,68,69,70,73,188]"; }
                else
                {
                    throw new GQKNMessageException("Tài khoản của bạn không nằm trong khu vực định tuyến lên Vinaphone. Liên hệ người quản trị hệ thống tỉnh hoặc vinaphone");
                }

                phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<int>>(strPhongBanDen);
            }
            else  //Dịnh tuyến bình thường
            {
                //Lấy ra List phòng ban chuyển của phòng ban mình
                List<PhongBan2PhongBanInfo> lstPhongBanDuocChuyen = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(userLogin.PhongBanId);
                if (lstPhongBanDuocChuyen == null || lstPhongBanDuocChuyen.Count == 0)
                {
                    throw new GQKNMessageException("Không tìm thấy phòng ban nào cần chuyển khiếu nại.");
                }

                phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<int>>(lstPhongBanDuocChuyen[0].PhongBanDen);
                if (phongBanDuocChuyen_JSON == null || phongBanDuocChuyen_JSON.Count == 0)
                {
                    throw new GQKNMessageException("Không tìm thấy phòng ban nào cần chuyển khiếu nại.");
                }
            }

            //Nếu chỉ có 1 phòng ban được chuyển khiếu nại => Tất cả khiếu nại sẽ được chuyển đến đây.
            if (phongBanDuocChuyen_JSON.Count == 1)
            {
                item.PhongBanXuLyId = phongBanDuocChuyen_JSON[0];
            }
            else //Nếu lớn hơn 2 phòng ban thì cần xét duyệt từng loại khiếu nại đến phòng ban nào trong list.
            {
                //Lấy ra phòng ban có thể xử lý loại khiếu nại.
                var lstPhongBanXuLy = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByLoaiKhieuNaiId(loaiKNIdSelect);
                if (lstPhongBanXuLy == null || lstPhongBanXuLy.Count == 0)
                {
                    throw new GQKNMessageException("Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.");

                }
                //Kiem tra xem phong ban co khong
                var isExistsPhongBanXuLy = false;
                foreach (var lstPhongBanXuLyItem in lstPhongBanXuLy)
                    if (phongBanDuocChuyen_JSON.Contains(lstPhongBanXuLyItem.PhongBanId))
                    {
                        item.PhongBanXuLyId = lstPhongBanXuLyItem.PhongBanId;
                        isExistsPhongBanXuLy = true;
                        break;
                    }
                if (!isExistsPhongBanXuLy)
                {
                    throw new GQKNMessageException("Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.");

                }
            }

            //Kiểm tra xem có phải phòng ban gửi về là phòng ban mình gửi lại lên. Nếu đúng thì đúng là phản hồi :D
            if (ActivityPhanHoi != null && item.PhongBanXuLyId == ActivityPhanHoi.PhongBanXuLyTruocId)
            {
                //Đây là chuyển phản hồi.
                itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
                itemActivity.NguoiDuocPhanHoi = ActivityPhanHoi.NguoiXuLyTruoc;
                itemActivity.GhiChu = "Chuyển phản hồi khiếu nại.";
                item.IsPhanHoi = true;
            }


            //Lấy ra loại phòng ban của phòng ban xử lý
            int LoaiPhongBanXuLy = 0;

            PhongBanInfo PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

            if (PhongBanXuLyItem != null)
                LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

            if (LoaiPhongBanXuLy == 0)
            {
                throw new GQKNMessageException("Phòng ban xử lý khiếu nại chưa thuộc loại phòng ban.");
            }

            //Thời gian xử lý trước đó của phòng ban
            var lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(item.Id, item.PhongBanXuLyId);

            item.NgayChuyenPhongBan = timeNow;
            item.NgayChuyenPhongBanSort = Convert.ToInt32(item.NgayChuyenPhongBan.ToString("yyyyMMdd"));

            item.NgayQuaHanPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, LoaiPhongBanXuLy, 1, null, lstXuLyTruoc);
            item.NgayQuaHanPhongBanXuLySort = Convert.ToInt32(item.NgayQuaHanPhongBanXuLy.ToString("yyyyMMdd"));

            item.NgayCanhBaoPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, LoaiPhongBanXuLy, 2, null, lstXuLyTruoc);
            item.NgayCanhBaoPhongBanXuLySort = Convert.ToInt32(item.NgayCanhBaoPhongBanXuLy.ToString("yyyyMMdd"));

            item.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
            item.KhuVucXuLyId = PhongBanXuLyItem.KhuVucId;

            item.NguoiTienXuLyCap1Id = 0;
            item.NguoiTienXuLyCap1 = string.Empty;

            //Người tiền xử lý.
            item.NguoiTienXuLyCap2Id = item.NguoiXuLyId;
            item.NguoiTienXuLyCap2 = item.NguoiXuLy;

            item.NguoiXuLyId = 0;
            item.NguoiXuLy = string.Empty;

            item.LUser = userLogin.Username;

            //var activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(Convert.ToInt32(id));

            //KhieuNai_ActivityInfo itemActivity = new KhieuNai_ActivityInfo();
            itemActivity.KhieuNaiId = Convert.ToInt32(id);
            itemActivity.ActivityTruoc = activityCurr.Id;
            //itemActivity.GhiChu = "Chuyển xử lý khiếu nại";
            //itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Phòng_Ban;
            itemActivity.IsCurrent = true;
            itemActivity.NguoiXuLyTruoc = userLogin.Username;
            itemActivity.PhongBanXuLyTruocId = userLogin.PhongBanId;
            itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
            itemActivity.NgayTiepNhan = timeNow;
            itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
            itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;

            // HaiPH : Set thời gian NgayTiepNhan_NguoiXuLyTruoc.
            if (itemActivity.NgayTiepNhan_NguoiXuLy.Year == DateTime.MaxValue.Year)
            {
                itemActivity.NgayTiepNhan_NguoiXuLyTruoc = timeNow;
            }
            else
            {
                itemActivity.NgayTiepNhan_NguoiXuLyTruoc = itemActivity.NgayTiepNhan_NguoiXuLy;
            }


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
                    //context.Response.Write(Constant.MESSAGE_SERVER_QUA_TAI);
                    Utility.LogEvent(tae);
                    return;
                }
                catch (Exception ex)
                {
                    //context.Response.Write(ex.Message);
                    Utility.LogEvent(ex);
                    return;
                }
            }

            if (flag)
            {
                //if (item.KhieuNaiFrom == 1)
                //{
                //    BuildKhieuNai.DongKhieuNaiVNPT(item, true);
                //}

                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                if (item.IsPhanHoi)
                    buocXuLyInfo.NoiDung = "Chuyển phản hồi tự động khiếu nại tới " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                else
                    buocXuLyInfo.NoiDung = "Chuyển xử lý tự động tới " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                buocXuLyInfo.LUser = userLogin.Username;
                buocXuLyInfo.KhieuNaiId = item.Id;
                buocXuLyInfo.IsAuto = true;
                buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;

                if (!isThemBuocXuLy)
                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { NguoiXuLyId = userLogin.Id, PhongBanXuLyId = userLogin.PhongBanId, LUser = userLogin.Username, NoiDung = note, IsAuto = false, KhieuNaiId = Convert.ToInt32(id) });

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

        private void ChuyenListKN(HttpContext context)
        {
            try
            {
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                {
                    context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                    return;
                }
                var listID = context.Request.Form["ListID"] ?? context.Request.QueryString["ListID"];
                var note = context.Request.Form["NoiDungXuLy"] ?? context.Request.QueryString["NoiDungXuLy"];
                string[] listItem = listID.Split(',');
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            ChuyenListKN(item, note);
                        }
                        catch { }
                    }
                }
                context.Response.Write("");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
            }
        }

        private void KhoaPhieuVNPT(HttpContext context)
        {
            try
            {
                string id = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];
                KhieuNaiInfo KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(id));
                if (KhieuNaiItem == null)
                {
                    context.Response.Write("Mã khiếu nại không hợp lệ.");
                    return;
                }

                AdminInfo userLogin = LoginAdmin.AdminLogin();

                if (userLogin != null && userLogin.PhongBanId == 0)
                {
                    throw new GQKNMessageException("Người dùng này chưa được phân vào phòng nên không có quyền thao tác vào khiếu nại.");
                }

                List<KhieuNai_BuocXuLyInfo> lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic("top 1 NoiDung", "KhieuNaiId = " + KhieuNaiItem.Id + " and IsAuto = 0", "Id desc");
                if (lstCheck == null || lstCheck.Count == 0)
                {
                    context.Response.Write("Khiếu nại chưa được xử lý.");
                    return;
                }

                KhieuNaiItem.NoiDungXuLyDongKN = lstCheck[0].NoiDung;

                int resultService = BuildKhieuNai.DongKhieuNaiVNPT(userLogin, KhieuNaiItem);
                if (resultService != 0)
                {
                    context.Response.Write(resultService);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
            }
        }
        private void ChuyenListKNVNP(HttpContext context)
        {
            try
            {
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Chuyển_xử_lý_khiếu_nại))
                {
                    context.Response.Write(Constant.MESSSAGE_NOT_PERMISSION);
                    return;
                }
                var listID = context.Request.Form["ListID"] ?? context.Request.QueryString["ListID"];
                var note = context.Request.Form["NoiDungXuLy"] ?? context.Request.QueryString["NoiDungXuLy"];
                string[] listItem = listID.Split(',');
                bool isChuyenVNP = true;
                foreach (string item in listItem)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        try
                        {
                            ChuyenListKN(item, note, isChuyenVNP);
                        }
                        catch { }
                    }
                }
                context.Response.Write("");
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                context.Response.Write(Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE);
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}