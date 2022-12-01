using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Admin;
using System.Transactions;
using Website.Components.Info;

namespace Website.AppCode.Controller
{
    public class BuildKhieuNai_Activity
    {
        public static string BuildListKhieuNai_Activity(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstanceKhieuNai_Activity().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListKhieuNai_Activity()
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstanceKhieuNai_Activity().GetList();
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListKhieuNai_ActivityByKhieuNaiId(int _khieuNaiId)
        {
            StringBuilder sb = new StringBuilder();
            var lst = ServiceFactory.GetInstanceKhieuNai_Activity().GetListByKhieuNaiId(_khieuNaiId);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        #region Longlx


        // Edited by	: Dao Van Duong
        // Datetime		: 2.8.2016 16:59
        // Note			: Chuyển xử lý
        public static MessageInfo ActivityChuyenPhongBanToUserInPhongBan(int KhieuNaiId, int PhongBanCanChuyen, string userNhanKN, KhieuNai_Actitivy_HanhDong action, string GhiChu)
        {
            MessageInfo objReturn = new MessageInfo();

            KhieuNai_ActivityInfo knActivityInfo = new KhieuNai_ActivityInfo();
            knActivityInfo.KhieuNaiId = KhieuNaiId;

            AdminInfo userLogin = LoginAdmin.AdminLogin();
            if (userLogin == null)
                throw new Exception(Constant.MESSAGE_HET_PHIEN_LAM_VIEC);

            DateTime now = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

            #region "Kiểm tra dữ liệu có hợp lệ, có bắn Exception"
            // Kiểm tra xem có phải là người cuối cùng đăng nội dung xử lý
            bool isThemBuocXuLy = false;
            if (string.IsNullOrEmpty(GhiChu))
            {
                string selectClause = "Top 1 *";
                string whereClause = string.Format("KhieuNaiId = {0}", KhieuNaiId);
                string orderClause = "Id desc";
                List<KhieuNai_BuocXuLyInfo> lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                if (lstCheck != null && lstCheck.Count > 0)
                {
                    KhieuNai_BuocXuLyInfo itemCheck = lstCheck[0];
                    if (!itemCheck.IsAuto && itemCheck.CUser.Equals(userLogin.Username))
                    {
                        GhiChu = itemCheck.NoiDung;
                        isThemBuocXuLy = true;
                    }
                    else
                    {
                        throw new Exception("Bạn chưa nhập nội dung xử lý.");
                    }
                }
            }

            // Lấy thông tin khiếu nại
            List<KhieuNaiInfo> itemKhieuNaiList = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("*", string.Format("Id = {0}", KhieuNaiId), string.Empty);
            if (itemKhieuNaiList == null || itemKhieuNaiList.Count == 0)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }
            KhieuNaiInfo itemKhieuNai = itemKhieuNaiList[0];

            // Mặc định là loại khiếu nại
            int loaiKNIdSelect = itemKhieuNai.LoaiKhieuNaiId;

            // Nếu chọn LV Con
            if (itemKhieuNai.LinhVucConId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucConId;
            }
            // Nếu chọn lĩnh vực chung
            else if (itemKhieuNai.LinhVucChungId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucChungId;
            }

            // Activity Curr
            KhieuNai_ActivityInfo itemCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);
            if (itemCurr == null)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }

            knActivityInfo.PhongBanXuLyTruocId = itemCurr.PhongBanXuLyId;
            itemCurr.NguoiXuLy = userLogin.Username;
            knActivityInfo.NguoiXuLyTruoc = itemCurr.NguoiXuLy;
            knActivityInfo.ActivityTruoc = itemCurr.Id;

            knActivityInfo.GhiChu = GhiChu;
            knActivityInfo.HanhDong = (byte)action;
            knActivityInfo.IsCurrent = true;

            // Tính ngày quá hạn và ngày cảnh báo cho phòng ban mới
            int NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(userNhanKN);
            if (NguoiXuLyId == 0)
                userNhanKN = string.Empty;

            knActivityInfo.NguoiXuLy = userNhanKN;

            knActivityInfo.NgayTiepNhan_NguoiXuLy = DateTime.MaxValue;
            if (userNhanKN.Length > 0)
            {
                //item.NgayTiepNhan_NguoiXuLy = DateTime.Now;
                knActivityInfo.NgayTiepNhan_NguoiXuLy = now;
            }

            // HaiPH
            knActivityInfo.NgayTiepNhan_PhongBanXuLyTruoc = itemCurr.NgayTiepNhan;
            knActivityInfo.NgayQuaHan_PhongBanXuLyTruoc = itemCurr.NgayQuaHan;
            knActivityInfo.NgayTiepNhan_NguoiXuLyTruoc = itemCurr.NgayTiepNhan_NguoiXuLy;

            // Nếu itemCurr.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year => người dùng không vào tiếp nhận khiếu nại mà thực hiện chuyển ngang hàng khiếu nại
            // ở ngoài danh sách => update luôn giá trị NgayTiepNhan_NguoiXuLyTruoc có giá trị khi 1 Activity mới được tạo ra.
            // Giá trị NgayTiepNhan_NguoiXuLy của Activity cũ sẽ được update trong mã lệnh Sql (hàm UpdateChuyenNgangHang)
            if (knActivityInfo.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year)
            {
                knActivityInfo.NgayTiepNhan_NguoiXuLyTruoc = now;
            }

            if (PhongBanCanChuyen == -1)  // Chuyển lên VNP
            {
                #region "Chuyển lên VNP"
                List<int> phongBanDuocChuyen_JSON = null;

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
                    throw new GQKNMessageException("Tài khoản của bạn không nằm trong khu vực định tuyến lên Vinaphone, Liên hệ người quản trị hệ thống tỉnh hoặc Vinaphone");
                }

                phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<int>>(strPhongBanDen);

                // Lấy ra phòng ban có thể xử lý lỗi khiếu nại
                List<LoaiKhieuNai2PhongBanInfo> lstPhongBanXuLy = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByLoaiKhieuNaiId(loaiKNIdSelect);
                if (lstPhongBanXuLy == null || lstPhongBanXuLy.Count == 0)
                {
                    throw new GQKNMessageException("Chưa có phòng ban nào xử lý lỗi khiếu nại này. Vui lòng liên hệ người quản trị hệ thống");
                }
                // Kiem tra xem phong ban co khong
                bool isExistsPhongBanXuLy = false;
                foreach (LoaiKhieuNai2PhongBanInfo lstPhongBanXuLyItem in lstPhongBanXuLy)
                    if (phongBanDuocChuyen_JSON.Contains(lstPhongBanXuLyItem.PhongBanId))
                    {
                        knActivityInfo.PhongBanXuLyId = lstPhongBanXuLyItem.PhongBanId;
                        isExistsPhongBanXuLy = true;
                        break;
                    }
                if (!isExistsPhongBanXuLy)
                {
                    throw new GQKNMessageException("Chưa có phòng ban nào xử lý lỗi khiếu nại này. Vui lòng liên hệ người quản trị hệ thống");
                }
                #endregion

            }
            else // Định tuyến bình thường
            {
                knActivityInfo.PhongBanXuLyId = PhongBanCanChuyen;
            }

            int LoaiPhongBanXuLy = 0;
            PhongBanInfo PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(knActivityInfo.PhongBanXuLyId);

            if (PhongBanXuLyItem != null)
                LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

            if (LoaiPhongBanXuLy == 0)
            {
                throw new Exception("Phòng ban chuyển khiếu nại chưa thuộc loại phòng ban nào.");
            }

            // Thời gian xử lý trước đó của phòng ban
            List<KhieuNai_ActivityInfo> lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(KhieuNaiId, knActivityInfo.PhongBanXuLyId);

            knActivityInfo.NgayTiepNhan = now;
            knActivityInfo.NgayQuaHan = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 1, null, lstXuLyTruoc);
            knActivityInfo.NgayCanhBao = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 2, null, lstXuLyTruoc);

            bool flag = false;

            int NguoiTienXLPhongBanId = 0;
            string NguoiTienXLPhongBan = string.Empty;

            int NguoiTienXuLyId = itemKhieuNai.NguoiXuLyId;
            string NguoiTienXuLy = itemKhieuNai.NguoiXuLy;
            if (NguoiTienXuLyId == 0)
            {
                NguoiTienXuLyId = userLogin.Id;
                NguoiTienXuLy = userLogin.Username;
            }

            int NguoiPhanHoiId = 0;
            string NguoiPhanHoi = string.Empty;
            bool IsPhanHoi = false;
            #endregion

            if (Config.IsDebug) // Chỉ là Debug thôi
            {
                #region "Vùng Debug chuyển HNI"
                if (PhongBanXuLyItem.IsChuyenHNI)
                {
                    DoiTacInfo DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(PhongBanXuLyItem.DoiTacId);

                    string GhiChuVNP = string.Format("{0} /// {1}-{2}", GhiChu, userLogin.FullName, userLogin.Phone);
                    try
                    {
                        objReturn = BuildKhieuNai.TiepNhanVNPT_HNI(itemKhieuNai, GhiChuVNP, string.Empty, string.Empty, null, DoiTacItem.MaDoiTac);
                    }
                    catch (System.Exception ex)
                    {
                        Helper.GhiLogs(ex);
                        objReturn.Code = -1;
                        objReturn.Message = "Lỗi gọi chuyển xử lý HNI: " + ex.Message;
                    }
                }
                else // Không chuyển HNI
                {
                    objReturn.Code = 1;
                    objReturn.Message = "Chế độ bug không chuyển cho HNI, không xử lý chuyển";
                }
                #endregion

                // Chuyển về ngay không cần xử lý tiếp
                return objReturn;
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        ServiceFactory.GetInstanceKhieuNai().UpdateKhieuNai_Activity(
                            KhieuNaiId,
                            (byte)KhieuNai_TrangThai_Type.Đang_xử_lý,
                            PhongBanXuLyItem.DoiTacId,
                            PhongBanXuLyItem.KhuVucId,
                            knActivityInfo.PhongBanXuLyId,
                            NguoiXuLyId,
                            userNhanKN,
                            NguoiTienXLPhongBanId,
                            NguoiTienXLPhongBan,
                            NguoiTienXuLyId,
                            NguoiTienXuLy,
                            NguoiPhanHoiId,
                            NguoiPhanHoi,
                            knActivityInfo.NgayTiepNhan,
                            knActivityInfo.NgayQuaHan,
                            knActivityInfo.NgayCanhBao,
                            IsPhanHoi,
                            userLogin.Username);

                        ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(itemCurr.Id, KhieuNaiId, userLogin.Username);

                        ServiceFactory.GetInstanceKhieuNai_Activity().Add(knActivityInfo);

                        // Xử lý chuyển dữ liệu đi qua service
                        if (PhongBanXuLyItem.IsChuyenHNI)
                        {
                            if (itemKhieuNai.IsTraSau) // Nếu là thuê bao trả sau
                            {
                                try
                                {
                                    if (itemKhieuNai.KhieuNaiFrom == 1)
                                    {
                                        itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                                        itemKhieuNai.DoiTacId = PhongBanXuLyItem.DoiTacId;
                                        itemKhieuNai.NoiDungXuLyDongKN = string.Format("{0} /// {1}-{2}", GhiChu, userLogin.FullName, userLogin.Phone);
                                        BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);

                                        objReturn.Code = 1;
                                        objReturn.Message = "Chuyển khiếu nại thành công";
                                    }
                                    else
                                    {
                                        DoiTacInfo DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(PhongBanXuLyItem.DoiTacId);
                                        string GhiChuVNP = string.Format("{0} /// {1}-{2}", GhiChu, userLogin.FullName, userLogin.Phone);
                                        objReturn = BuildKhieuNai.TiepNhanVNPT_HNI(itemKhieuNai, GhiChuVNP, string.Empty, string.Empty, null, DoiTacItem.MaDoiTac);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    Helper.GhiLogs(ex);
                                    throw new Exception("Lỗi chuyển xử lý HNI: " + ex.Message);
                                }
                            }
                            else
                            {
                                throw new Exception("Để chuyển phiếu về HNI, số thuê bao phải là trả sau!");
                            }

                        }
                        // Chuyển về PTDV
                        else if (PhongBanXuLyItem.DoiTacId == 10101)
                        {
                            objReturn = BuildKhieuNai.TiepNhanVAS(itemKhieuNai, knActivityInfo);
                        }
                        else // Chuyển trong GQKN
                        {
                            objReturn.Code = 1;
                            objReturn.Message = "Chuyển khiếu nại thành công";
                        }
                        scope.Complete();
                        flag = true;
                    }
                    catch (GQKNMessageException ge)
                    {
                        Helper.GhiLogs(ge);
                        objReturn.Code = -1;
                        objReturn.Message = ge.Message;
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                        objReturn.Code = -1;
                        objReturn.Message = ex.Message;
                    }
                }

                // Edited by	: Dao Van Duong
                // Datetime		: 3.8.2016 16:27
                // Note			: Cập nhật thành công, ghi lại bước xử lý vào bảng
                if (flag)
                {
                    KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                    buocXuLyInfo.NoiDung = "Chuyển xử lý tới " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(knActivityInfo.PhongBanXuLyId);
                    buocXuLyInfo.LUser = userLogin.Username;
                    buocXuLyInfo.KhieuNaiId = KhieuNaiId;
                    buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                    buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                    buocXuLyInfo.IsAuto = true;

                    if (!isThemBuocXuLy)
                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { NguoiXuLyId = userLogin.Id, PhongBanXuLyId = userLogin.PhongBanId, LUser = userLogin.Username, NoiDung = GhiChu, IsAuto = false, KhieuNaiId = KhieuNaiId });

                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                    BuildKhieuNai_Log.LogKhieuNai(Convert.ToInt32(KhieuNaiId), buocXuLyInfo.NoiDung, "[Hành động]");
                }
                else // Cập nhật không thành công 
                {
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    {
                        CUser = knActivityInfo.NguoiXuLy,
                        ThaoTac = objReturn.Message,
                        PhongBanId = knActivityInfo.PhongBanXuLyId,
                        KhieuNaiId = itemKhieuNai.Id,
                    });
                }
            }


            return objReturn;
        }

        public static void ActivityChuyenPhanHoi(int KhieuNaiId, int PhongBanXuLyId, string GhiChu, ref int ResultService)
        {
            KhieuNai_ActivityInfo item = new KhieuNai_ActivityInfo();
            item.KhieuNaiId = KhieuNaiId;

            var userLogin = LoginAdmin.AdminLogin();

            //Tính ngày quá hạn và ngày cảnh báo cho phòng ban mới
            var now = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

            //Kiểm tra xem có phải là người cuối cùng đăng nội dung xử lý
            bool isThemBuocXuLy = false;
            if (string.IsNullOrEmpty(GhiChu))
            {
                string selectClause = "Top 1 *";
                string whereClause = string.Format("KhieuNaiId={0}", KhieuNaiId);
                string orderClause = "Id desc";
                var lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                if (lstCheck != null && lstCheck.Count > 0)
                {
                    var itemCheck = lstCheck[0];
                    if (!itemCheck.IsAuto && itemCheck.CUser.Equals(userLogin.Username))
                    {
                        GhiChu = itemCheck.NoiDung;
                        isThemBuocXuLy = true;
                    }
                    else
                    {
                        throw new Exception("Bạn chưa nhập nội dung xử lý.");
                    }
                }
            }

            //Get KhieuNai Item
            List<KhieuNaiInfo> itemKhieuNaiList = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id,SoThueBao, KhieuNaiFrom,LoaiKhieuNaiId,LinhVucChungId,LinhVucConId,NguoiXuLyId, NguoiXuLy", "Id=" + KhieuNaiId, "");
            if (itemKhieuNaiList == null || itemKhieuNaiList.Count == 0)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }
            var itemKhieuNai = itemKhieuNaiList[0];

            //Mặc định là loại khiếu nại
            var loaiKNIdSelect = itemKhieuNai.LoaiKhieuNaiId;
            //Nếu chọn LV Con
            if (itemKhieuNai.LinhVucConId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucConId;
            }
            //Nếu chọn lĩnh vực chung
            else if (itemKhieuNai.LinhVucChungId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucChungId;
            }

            //Activity Curr
            KhieuNai_ActivityInfo itemCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);

            if (itemCurr == null)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }

            // Edit : Phi Hoang Hai 17/12/2014
            // Todo : Xử lý trường hợp KhieuNai_Activity đã được chuyển cho phòng ban khác nhưng trong bản ghi KhieuNai vẫn chưa được cập nhật
            //          và KhieuNai_Activity.NguoiXuLyTruoc == user đăng nhập 
            //      thì cập nhật lại các trường KhuVucXuLyId, DoiTacXuLyId, PhongBanXuLyId, NguoiXuLyId, NguoiXuLy
            //      (vì nếu KhieuNai_Activity.NguoiXuLyTruoc != user đăng nhập => có nhiều bản ghi KhieuNai_Activity rác được sinh ra, cần phải nhờ kỹ thuật xóa)

            if (itemCurr.PhongBanXuLyId != userLogin.PhongBanId)
            {
                if (itemCurr.PhongBanXuLyTruocId == userLogin.PhongBanId)
                {
                    if (itemCurr.NguoiXuLyTruoc == userLogin.Username)
                    {
                        PhongBanInfo phongBanInfo = ServiceFactory.GetInstancePhongBan().GetInfo(PhongBanXuLyId);
                        if (phongBanInfo != null)
                        {
                            string updateClause = string.Format("KhuVucXuLyId={0}, DoiTacXuLyId={1}, PhongBanXuLyId={2}, NguoiXuLyId=0, NguoiXuLy=''", phongBanInfo.KhuVucId, phongBanInfo.DoiTacId, PhongBanXuLyId);
                            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                            return;
                        }
                        else
                        {
                            throw new Exception("Có lỗi trong quá trình lấy dữ liệu từ server, bạn hãy thử lại");
                        }
                    }
                    else
                    {
                        throw new Exception("Hãy gọi cho số hotline để được hỗ trợ");
                    }

                    //return;
                }
                else
                {
                    throw new Exception("Không phải phòng ban xử lý khiếu nại này.");
                }
            }

            item.PhongBanXuLyTruocId = userLogin.PhongBanId;
            item.NguoiXuLyTruoc = userLogin.Username;

            itemCurr.NguoiXuLy = userLogin.Username;

            item.ActivityTruoc = itemCurr.Id;
            item.GhiChu = GhiChu;
            item.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
            item.IsCurrent = true;

            // HaiPH
            item.NgayTiepNhan_PhongBanXuLyTruoc = itemCurr.NgayTiepNhan;
            item.NgayQuaHan_PhongBanXuLyTruoc = itemCurr.NgayQuaHan;
            item.NgayTiepNhan_NguoiXuLyTruoc = itemCurr.NgayTiepNhan_NguoiXuLy;

            // Nếu itemCurr.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year => người dùng không vào tiếp nhận khiếu nại mà thực hiện chuyển ngang hàng khiếu nại
            //   ở ngoài danh sách => update luôn giá trị NgayTiepNhan_NguoiXuLyTruoc có giá trị khi 1 Activity mới được tạo ra.
            //      Giá trị NgayTiepNhan_NguoiXuLy của Activity cũ sẽ được update trong mã lệnh Sql (hàm UpdateChuyenNgangHang)
            if (item.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year)
            {
                item.NgayTiepNhan_NguoiXuLyTruoc = now;
            }

            //Lấy ra phòng ban đã gửi khiếu nại cho nó
            var NguoiDuocPhanHoiId = 0;
            KhieuNai_ActivityInfo activityFirst = null;
            if (PhongBanXuLyId > 0)
            {
                activityFirst = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivtyLastSend2PhongBan(KhieuNaiId, PhongBanXuLyId);
                if (activityFirst == null)
                {
                    throw new Exception("Không có phòng ban để phản hồi.");
                }
                item.PhongBanXuLyId = activityFirst.PhongBanXuLyTruocId;
                item.NguoiXuLy = string.Empty;
                item.NguoiDuocPhanHoi = activityFirst.NguoiXuLyTruoc;
                NguoiDuocPhanHoiId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(item.NguoiDuocPhanHoi);
                if (NguoiDuocPhanHoiId == 0)
                    item.NguoiDuocPhanHoi = string.Empty;
            }
            else
            {
                activityFirst = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivity4PhanHoi(KhieuNaiId, userLogin.PhongBanId);
                if (activityFirst == null)
                {
                    throw new Exception("Phòng ban của người dùng này không phải là phòng ban tiếp nhận.");
                }

                if (activityFirst.HanhDong == 0)
                {
                    throw new Exception("Không chuyển phản hồi khiếu nại cho chính mình.");
                }

                item.PhongBanXuLyId = activityFirst.PhongBanXuLyTruocId;
                item.NguoiXuLy = string.Empty;
                item.NguoiDuocPhanHoi = activityFirst.NguoiXuLyTruoc;
                NguoiDuocPhanHoiId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(item.NguoiDuocPhanHoi);
                if (NguoiDuocPhanHoiId == 0)
                    item.NguoiDuocPhanHoi = string.Empty;
            }

            var LoaiPhongBanXuLy = 0;
            var PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

            if (PhongBanXuLyItem != null)
                LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

            if (LoaiPhongBanXuLy == 0)
            {
                throw new Exception("Phòng ban chuyển khiếu nại chưa thuộc loại phòng ban nào.");
            }

            //Thời gian xử lý trước đó của phòng ban
            var lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(KhieuNaiId, item.PhongBanXuLyId);

            item.NgayTiepNhan = now;
            item.NgayQuaHan = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 1, null, lstXuLyTruoc);
            item.NgayCanhBao = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 2, null, lstXuLyTruoc);


            bool flag = false;

            var NguoiTienXuLyId = itemKhieuNai.NguoiXuLyId;
            var NguoiTienXuLy = itemKhieuNai.NguoiXuLy;
            if (NguoiTienXuLyId == 0)
            {
                NguoiTienXuLyId = userLogin.Id;
                NguoiTienXuLy = userLogin.Username;
            }

            if (Config.IsDebug)
            {
                //Utility.LogEvent("Bug Test:" + 1);
                try
                {
                    if (itemKhieuNai.KhieuNaiFrom == 1 && PhongBanXuLyItem.IsChuyenHNI)
                    {
                        itemKhieuNai.NoiDungXuLyDongKN = GhiChu;
                        itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                        itemKhieuNai.DoiTacId = PhongBanXuLyItem.DoiTacId;
                        //Utility.LogEvent("Bug Test:" + 2);
                        ResultService = BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);

                    }
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    throw;
                }
            }
            else
            {
                bool IsCallHNI = false;
                bool CallVNP = true;
                //if (itemKhieuNai.KhieuNaiFrom == 1 && PhongBanXuLyItem.IsChuyenHNI)
                //{
                //    try
                //    {
                //        CallVNP = false;
                //        IsCallHNI = true;

                //        itemKhieuNai.NoiDungXuLyDongKN = GhiChu;
                //        itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                //        itemKhieuNai.DoiTacId = PhongBanXuLyItem.DoiTacId;
                //        ResultService = BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);
                //    }
                //    catch(Exception ex) {
                //        throw new Exception("Không khóa được phiếu tại service HNI. Mã lỗi service HNI: " + ex.Message);
                //    }
                //}

                //if(IsCallHNI)
                //{
                //    if (ResultService == 10000)
                //        CallVNP = true;
                //    else
                //        CallVNP = false;
                //}

                if (CallVNP)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            ServiceFactory.GetInstanceKhieuNai().UpdateKhieuNai_Activity(KhieuNaiId, (byte)KhieuNai_TrangThai_Type.Đang_xử_lý,
                                PhongBanXuLyItem.DoiTacId, PhongBanXuLyItem.KhuVucId, item.PhongBanXuLyId,
                                0, item.NguoiXuLy, 0, string.Empty, NguoiTienXuLyId, NguoiTienXuLy,
                                NguoiDuocPhanHoiId, item.NguoiDuocPhanHoi,
                                item.NgayTiepNhan, item.NgayQuaHan, item.NgayCanhBao, true, userLogin.Username);

                            ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(itemCurr.Id, KhieuNaiId, userLogin.Username);
                            ServiceFactory.GetInstanceKhieuNai_Activity().Add(item);

                            if (itemKhieuNai.KhieuNaiFrom == 1 && PhongBanXuLyItem.IsChuyenHNI)
                            {
                                itemKhieuNai.NoiDungXuLyDongKN = GhiChu;
                                itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                                itemKhieuNai.DoiTacId = PhongBanXuLyItem.DoiTacId;
                                ResultService = BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);
                            }

                            scope.Complete();
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            //Utility.LogEvent("ActivityChuyenPhanHoi - Transaction Update KhieuNai KhieuNai_Activity : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            if (flag)
            {
                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                buocXuLyInfo.NoiDung = "Chuyển phản hồi khiếu nại về " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                buocXuLyInfo.LUser = userLogin.Username;
                buocXuLyInfo.KhieuNaiId = KhieuNaiId;
                buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                buocXuLyInfo.IsAuto = true;

                if (!isThemBuocXuLy)
                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { NguoiXuLyId = userLogin.Id, PhongBanXuLyId = userLogin.PhongBanId, LUser = userLogin.Username, NoiDung = GhiChu, IsAuto = false, KhieuNaiId = KhieuNaiId });

                ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                BuildKhieuNai_Log.LogKhieuNai(Convert.ToInt32(KhieuNaiId), buocXuLyInfo.NoiDung, "[Hành động]");
            }
        }

        public static bool ActivityChuyenNgangHangToUser(int KhieuNaiId, string username, string GhiChu)
        {
            var userLogin = LoginAdmin.AdminLogin();
            var now = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

            //Activity Curr
            var itemCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);

            if (itemCurr == null)
            {
                throw new GQKNMessageException("Khiếu nại không hợp lệ");
            }

            if (itemCurr.PhongBanXuLyId != userLogin.PhongBanId)
            {
                throw new GQKNMessageException("Khiếu nại đã được chuyển sang phòng ban khác. Bạn hãy ấn Ctrl + F5 để kiểm tra lại. (Trường hợp khiếu nại vẫn đang ở tại phòng ban và đang được xử lý mà không chuyển ngang hàng được thì gọi cho số hotline để được hỗ trợ)");
            }

            //Kiểm tra xem có phải là người cuối cùng đăng nội dung xử lý
            bool isThemBuocXuLy = false;
            if (string.IsNullOrEmpty(GhiChu))
            {
                string selectClause = "Top 1 *";
                string whereClause = string.Format("KhieuNaiId={0}", KhieuNaiId);
                string orderClause = "Id desc";
                var lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                if (lstCheck != null && lstCheck.Count > 0)
                {
                    var itemCheck = lstCheck[0];
                    if (!itemCheck.IsAuto && itemCheck.CUser.Equals(userLogin.Username))
                    {
                        GhiChu = itemCheck.NoiDung;
                        isThemBuocXuLy = true;
                    }
                    else
                    {
                        throw new GQKNMessageException("Bạn chưa nhập nội dung xử lý.");
                    }
                }
            }

            string NguoiXuLyOld = itemCurr.NguoiXuLy;
            var NguoiXuLyOldId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(NguoiXuLyOld);
            if (NguoiXuLyOldId == 0)
            {
                NguoiXuLyOldId = userLogin.Id;
                NguoiXuLyOld = userLogin.Username;
            }

            var NguoiXuLyId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(username);
            if (NguoiXuLyId == 0)
            {
                username = string.Empty;
            }

            itemCurr.NguoiXuLyTruoc = userLogin.Username;
            itemCurr.PhongBanXuLyTruocId = userLogin.PhongBanId;
            itemCurr.NguoiXuLy = username;
            itemCurr.ActivityTruoc = itemCurr.Id;
            itemCurr.GhiChu = GhiChu;
            itemCurr.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Chuyển_Ngang_Hàng;
            itemCurr.IsCurrent = true;

            // HaiPH
            itemCurr.NgayTiepNhan_NguoiXuLyTruoc = itemCurr.NgayTiepNhan_NguoiXuLy;
            itemCurr.NgayTiepNhan_PhongBanXuLyTruoc = itemCurr.NgayTiepNhan;
            itemCurr.NgayQuaHan_PhongBanXuLyTruoc = itemCurr.NgayQuaHan;

            // Nếu itemCurr.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year => người dùng không vào tiếp nhận khiếu nại mà thực hiện chuyển ngang hàng khiếu nại
            //   ở ngoài danh sách => update luôn giá trị NgayTiepNhan_NguoiXuLyTruoc có giá trị khi 1 Activity mới được tạo ra.
            //      Giá trị NgayTiepNhan_NguoiXuLy của Activity cũ sẽ được update trong mã lệnh Sql (hàm UpdateChuyenNgangHang)
            if (itemCurr.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year)
            {
                itemCurr.NgayTiepNhan_NguoiXuLyTruoc = now;
            }

            if (username.Length > 0)
            {
                itemCurr.NgayTiepNhan_NguoiXuLy = now;
            }

            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    ServiceFactory.GetInstanceKhieuNai().UpdateChuyenNgangHang(KhieuNaiId, NguoiXuLyId, itemCurr.NguoiXuLy, NguoiXuLyOldId, NguoiXuLyOld, userLogin.Username);
                    ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(itemCurr.Id, KhieuNaiId, userLogin.Username);
                    ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemCurr);
                    scope.Complete();
                    flag = true;
                }
                catch
                {
                    throw;
                }
            }

            if (flag)
            {
                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                buocXuLyInfo.NoiDung = "Chuyển ngang hàng khiếu nại";
                if (!string.IsNullOrEmpty(username))
                    buocXuLyInfo.NoiDung += " về user " + username;
                buocXuLyInfo.LUser = userLogin.Username;
                buocXuLyInfo.KhieuNaiId = KhieuNaiId;
                buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                buocXuLyInfo.IsAuto = true;

                if (!isThemBuocXuLy)
                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { NguoiXuLyId = userLogin.Id, PhongBanXuLyId = userLogin.PhongBanId, LUser = userLogin.Username, NoiDung = GhiChu, IsAuto = false, KhieuNaiId = KhieuNaiId });


                ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                BuildKhieuNai_Log.LogKhieuNai(Convert.ToInt32(KhieuNaiId), buocXuLyInfo.NoiDung, "[Hành động]");
            }

            return true;
        }

        public static void ActivityChuyenPhanHoiTrungTam(int KhieuNaiId, int PhongBanXuLyTruocId, string GhiChu)
        {
            KhieuNai_ActivityInfo item = new KhieuNai_ActivityInfo();
            item.KhieuNaiId = KhieuNaiId;

            AdminInfo userLogin = LoginAdmin.AdminLogin();

            //Tính ngày quá hạn và ngày cảnh báo cho phòng ban mới
            DateTime now = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

            //Kiểm tra xem có phải là người cuối cùng đăng nội dung xử lý
            string selectClause = "Top 1 *";
            string whereClause = string.Format("KhieuNaiId={0}", KhieuNaiId);
            string orderClause = "Id desc";
            List<KhieuNai_BuocXuLyInfo> lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
            if (lstCheck != null && lstCheck.Count > 0)
            {
                KhieuNai_BuocXuLyInfo itemCheck = lstCheck[0];
                if (!itemCheck.IsAuto && itemCheck.CUser.Equals(userLogin.Username))
                {
                    GhiChu = itemCheck.NoiDung;
                }
                else
                {
                    throw new Exception("Bạn chưa nhập nội dung xử lý.");
                }
            }

            // Get KhieuNai Item
            List<KhieuNaiInfo> itemKhieuNaiList = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id,SoThueBao, KhieuNaiFrom,LoaiKhieuNaiId,LinhVucChungId,LinhVucConId,NguoiXuLyId,NguoiXuLy", "Id=" + KhieuNaiId, "");
            if (itemKhieuNaiList == null || itemKhieuNaiList.Count == 0)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }
            KhieuNaiInfo itemKhieuNai = itemKhieuNaiList[0];

            // Mặc định là loại khiếu nại
            int loaiKNIdSelect = itemKhieuNai.LoaiKhieuNaiId;
            // Nếu chọn LV Con
            if (itemKhieuNai.LinhVucConId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucConId;
            }
            //Nếu chọn lĩnh vực chung
            else if (itemKhieuNai.LinhVucChungId != 0)
            {
                loaiKNIdSelect = itemKhieuNai.LinhVucChungId;
            }

            //Activity Curr
            KhieuNai_ActivityInfo itemCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(KhieuNaiId);

            if (itemCurr == null)
            {
                throw new Exception("Khiếu nại không hợp lệ");
            }

            if (itemCurr.HanhDong == 0)
            {
                throw new Exception("Không chuyển phản hồi khiếu nại cho chính mình.");
            }

            // Edit : Phi Hoang Hai 17/12/2014
            // Todo : Xử lý trường hợp KhieuNai_Activity đã được chuyển cho phòng ban khác nhưng trong bản ghi KhieuNai vẫn chưa được cập nhật
            //          và KhieuNai_Activity.NguoiXuLyTruoc == user đăng nhập 
            //      thì cập nhật lại các trường KhuVucXuLyId, DoiTacXuLyId, PhongBanXuLyId, NguoiXuLyId, NguoiXuLy
            //      (vì nếu KhieuNai_Activity.NguoiXuLyTruoc != user đăng nhập => có nhiều bản ghi KhieuNai_Activity rác được sinh ra, cần phải nhờ kỹ thuật xóa)
            if (itemCurr.PhongBanXuLyId != userLogin.PhongBanId)
            {
                if (itemCurr.PhongBanXuLyTruocId == userLogin.PhongBanId)
                {
                    if (itemCurr.NguoiXuLyTruoc == userLogin.Username)
                    {
                        PhongBanInfo phongBanInfo = ServiceFactory.GetInstancePhongBan().GetInfo(PhongBanXuLyTruocId);
                        if (phongBanInfo != null)
                        {
                            string updateClause = string.Format("KhuVucXuLyId={0}, DoiTacXuLyId={1}, PhongBanXuLyId={2}, NguoiXuLyId=0, NguoiXuLy=''", phongBanInfo.KhuVucId, phongBanInfo.DoiTacId, PhongBanXuLyTruocId);
                            ServiceFactory.GetInstanceKhieuNai().UpdateDynamic(updateClause, "Id=" + KhieuNaiId);
                            return;
                        }
                        else
                        {
                            throw new Exception("Có lỗi trong quá trình lấy dữ liệu từ server, bạn hãy thử lại");
                        }
                    }
                    else
                    {
                        throw new Exception("Hãy gọi cho số hotline để được hỗ trợ");
                    }
                    //return;
                }
                else
                {
                    throw new Exception("Không phải phòng ban xử lý khiếu nại này.");
                }
            }



            item.PhongBanXuLyTruocId = userLogin.PhongBanId;
            item.NguoiXuLyTruoc = userLogin.Username;

            item.ActivityTruoc = itemCurr.Id;
            item.GhiChu = GhiChu;
            item.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.KN_Phản_Hồi;
            item.IsCurrent = true;

            // HaiPH
            item.NgayTiepNhan_PhongBanXuLyTruoc = itemCurr.NgayTiepNhan;
            item.NgayQuaHan_PhongBanXuLyTruoc = itemCurr.NgayQuaHan;
            item.NgayTiepNhan_NguoiXuLyTruoc = itemCurr.NgayTiepNhan_NguoiXuLy;

            // Nếu itemCurr.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year => người dùng không vào tiếp nhận khiếu nại mà thực hiện chuyển ngang hàng khiếu nại
            //   ở ngoài danh sách => update luôn giá trị NgayTiepNhan_NguoiXuLyTruoc có giá trị khi 1 Activity mới được tạo ra.
            //      Giá trị NgayTiepNhan_NguoiXuLy của Activity cũ sẽ được update trong mã lệnh Sql (hàm UpdateChuyenNgangHang)
            if (item.NgayTiepNhan_NguoiXuLyTruoc.Year == DateTime.MaxValue.Year)
            {
                item.NgayTiepNhan_NguoiXuLyTruoc = now;
            }

            KhieuNai_ActivityInfo activityFirst = null;
            int NguoiDuocPhanHoiId = 0;
            if (PhongBanXuLyTruocId > 0)
            {
                activityFirst = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivtyLastSend2TrungTam(KhieuNaiId, userLogin.DoiTacId, PhongBanXuLyTruocId);
                if (activityFirst == null)
                {
                    throw new Exception("Không có phòng ban để phản hồi.");
                }
                item.PhongBanXuLyId = activityFirst.PhongBanXuLyTruocId;
                item.NguoiXuLy = string.Empty;
                item.NguoiDuocPhanHoi = activityFirst.NguoiXuLyTruoc;
                NguoiDuocPhanHoiId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(item.NguoiDuocPhanHoi);
                if (NguoiDuocPhanHoiId == 0)
                    item.NguoiDuocPhanHoi = string.Empty;
            }
            else
            {
                //Lấy ra phòng ban của trung tâm đã gửi khiếu nại cho nó            
                List<KhieuNai_ActivityInfo> lstActivity = ServiceFactory.GetInstanceKhieuNai_Activity().GetTrungTamActivity(KhieuNaiId);

                // Lấy ra phòng ban xử lý
                PhongBanInfo phongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(itemCurr.PhongBanXuLyId);
                if (lstActivity == null || lstActivity.Count == 0)
                {
                    throw new Exception("Phòng ban của người dùng này không phải là phòng ban tiếp nhận.");
                }

                if (lstActivity.Count > 1)
                {
                    for (int i = 0; i < lstActivity.Count; i++)
                    {
                        if (lstActivity[i].DoiTacId == phongBanXuLyItem.DoiTacId)
                        {
                            continue;
                        }
                        activityFirst = lstActivity[i];
                        break;
                    }
                }
                else
                {
                    throw new Exception("Khiếu nại chưa được chuyển qua trung tâm khác.");
                }

                if (activityFirst == null)
                {
                    throw new Exception("Khiếu nại chưa được chuyển qua trung tâm khác.");
                }

                item.PhongBanXuLyId = activityFirst.PhongBanXuLyId;
                item.NguoiXuLy = string.Empty;
                item.NguoiDuocPhanHoi = activityFirst.NguoiXuLy;
                NguoiDuocPhanHoiId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(item.NguoiDuocPhanHoi);
                if (NguoiDuocPhanHoiId == 0)
                    item.NguoiDuocPhanHoi = string.Empty;
            }

            int LoaiPhongBanXuLy = 0;
            PhongBanInfo PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

            if (PhongBanXuLyItem != null)
                LoaiPhongBanXuLy = PhongBanXuLyItem.LoaiPhongBanId;

            if (LoaiPhongBanXuLy == 0)
            {
                throw new Exception("Phòng ban chuyển khiếu nại chưa thuộc loại phòng ban nào.");
            }

            //Thời gian xử lý trước đó của phòng ban
            List<KhieuNai_ActivityInfo> lstXuLyTruoc = ServiceFactory.GetInstanceKhieuNai_Activity().GetListXuLyTruoc(KhieuNaiId, item.PhongBanXuLyId);

            item.NgayTiepNhan = now;
            item.NgayQuaHan = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 1, null, lstXuLyTruoc);
            item.NgayCanhBao = GetDataImpl.GetTimeConfig_PhongBan(now, loaiKNIdSelect, LoaiPhongBanXuLy, 2, null, lstXuLyTruoc);

            bool flag = false;

            int NguoiTienXuLyId = itemKhieuNai.NguoiXuLyId;
            string NguoiTienXuLy = itemKhieuNai.NguoiXuLy;
            if (NguoiTienXuLyId == 0)
            {

                NguoiTienXuLyId = userLogin.Id;
                NguoiTienXuLy = userLogin.Username;
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    ServiceFactory.GetInstanceKhieuNai().UpdateKhieuNai_Activity(KhieuNaiId, (byte)KhieuNai_TrangThai_Type.Đang_xử_lý,
                        PhongBanXuLyItem.DoiTacId, PhongBanXuLyItem.KhuVucId, item.PhongBanXuLyId,
                        0, item.NguoiXuLy, 0, string.Empty, NguoiTienXuLyId, NguoiTienXuLy,
                        NguoiDuocPhanHoiId, item.NguoiDuocPhanHoi,
                        item.NgayTiepNhan, item.NgayQuaHan, item.NgayCanhBao, true, userLogin.Username);

                    ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(itemCurr.Id, KhieuNaiId, userLogin.Username);
                    ServiceFactory.GetInstanceKhieuNai_Activity().Add(item);

                    if (itemKhieuNai.KhieuNaiFrom == 1 && PhongBanXuLyItem.IsChuyenHNI)
                    {
                        itemKhieuNai.NoiDungXuLyDongKN = GhiChu;
                        itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;
                        itemKhieuNai.DoiTacId = PhongBanXuLyItem.DoiTacId;
                        BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);
                        //if (PhongBanXuLyItem.DoiTacId == 10042)
                        //{
                        //    BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);
                        //}
                        ////CBG
                        //else if (PhongBanXuLyItem.DoiTacId == 10038)
                        //{
                        //    itemKhieuNai.NoiDungXuLyDongKN = GhiChu;
                        //    itemKhieuNai.DoiTacXuLyId = PhongBanXuLyItem.DoiTacId;                            
                        //}
                        ////HNI
                        //else if (PhongBanXuLyItem.DoiTacId == 10060)
                        //{
                        //    BuildKhieuNai.DongKhieuNaiVNPT(userLogin, itemKhieuNai);
                        //}
                    }
                    scope.Complete();
                    flag = true;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent("ActivityChuyenPhanHoiTrungTam - Transaction Update KhieuNai KhieuNai_Activity : " + ex.Message);
                    throw;
                }
            }
            if (flag)
            {


                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                buocXuLyInfo.NoiDung = "Chuyển phản hồi trung tâm về " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(item.PhongBanXuLyId);
                buocXuLyInfo.LUser = userLogin.Username;
                buocXuLyInfo.KhieuNaiId = KhieuNaiId;
                buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                buocXuLyInfo.IsAuto = true;

                ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                BuildKhieuNai_Log.LogKhieuNai(Convert.ToInt32(KhieuNaiId), buocXuLyInfo.NoiDung, "[Hành động]");
            }
        }

        #endregion

    }
}

