using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Services;
using Website.AppCode;
using Website.AppCode.Controller;


namespace Website.Service.Inbound
{


    /// <summary>
    /// Summary description for WSGqkn
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSGqkn : System.Web.Services.WebService
    {

        [WebMethod]
        public List<ProvinceInfo> BindTinh()
        {
            List<ProvinceInfo> lstResult = null;
            try
            {
                //StringBuilder sb = new StringBuilder();
                lstResult = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId is null", "Name");
                //lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Tỉnh/Thành Phố--" });

                //if (lstTinh != null && lstTinh.Count > 0)
                //{
                //    foreach (var item in lstTinh)
                //    {
                //        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                //    }
                //}
                //context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
            }
            return lstResult;
        }

        [WebMethod]
        public List<ProvinceInfo> BindQuanHuyen(int TinhId)
        {
            List<ProvinceInfo> lstResult = null;
            try
            {
                //StringBuilder sb = new StringBuilder();
                //var TinhId = context.Request.Form["TinhId"] ?? context.Request.QueryString["TinhId"];
                lstResult = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + TinhId, "Name");
                //lstQuanHuyen.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Quận/Huyện--" });

                //if (lstQuanHuyen != null && lstQuanHuyen.Count > 0)
                //{
                //    foreach (var item in lstQuanHuyen)
                //    {
                //        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                //    }
                //}
                //context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
            }
            return lstResult;
        }

        [WebMethod]
        public List<LoaiKhieuNaiInfo> BindLoaiKhieuNai()
        {
            List<LoaiKhieuNaiInfo> lstResult = null;
            try
            {
                //StringBuilder sb = new StringBuilder();
                lstResult = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", "cap = 1 and Status = 1", "sort");
                //sb.Append("<option value='0' title='--Loại khiếu nại--'>--Loại khiếu nại--</option>");
                //if (lst != null && lst.Count > 0)
                //{
                //    foreach (var item in lst)
                //    {
                //        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                //    }
                //}
                //context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
            }
            return lstResult;
        }

        [WebMethod]
        public List<LoaiKhieuNaiInfo> BindLinhVucChung(int LoaiKhieuNaiId)
        {
            List<LoaiKhieuNaiInfo> lstResult = null;
            try
            {
                //var LoaiKhieuNaiId = context.Request.Form["LoaiKhieuNaiId"] ?? context.Request.QueryString["LoaiKhieuNaiId"];

                //StringBuilder sb = new StringBuilder();
                lstResult = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", "cap = 2 and ParentId = " + LoaiKhieuNaiId, "sort");
                //sb.Append("<option value='0' title='--Loại khiếu nại--'>--Lĩnh vực chung--</option>");
                //if (lst != null && lst.Count > 0)
                //{
                //    foreach (var item in lst)
                //    {
                //        sb.AppendFormat("<option value='{0}' title='{1}'>{1}</option>", item.Id, item.Name);
                //    }
                //}
                //context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
            }
            return lstResult;

        }

        [WebMethod]
        public List<LoaiKhieuNaiInfo> BindLinhVucCon(int LoaiKhieuNaiId, int LinhVucChungId)
        {
            List<LoaiKhieuNaiInfo> lstResult = null;
            try
            {
                //var LoaiKhieuNaiId = context.Request.Form["LoaiKhieuNaiId"] ?? context.Request.QueryString["LoaiKhieuNaiId"];
                //var LinhVucChungId = context.Request.Form["LinhVucChungId"] ?? context.Request.QueryString["LinhVucChungId"];

                //StringBuilder sb = new StringBuilder();

                string strWhereClause = "cap = 3 and Status=1";
                if (LoaiKhieuNaiId != 0)
                    strWhereClause += " AND ParentLoaiKhieuNaiId = " + LoaiKhieuNaiId;
                if (LinhVucChungId != 0)
                    strWhereClause += " AND ParentId = " + LinhVucChungId;

                lstResult = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("", strWhereClause, "sort");
                //sb.Append("<option value='0' title='--Loại khiếu nại--' LoaiKhieuNaiId='0' LinhVucChungId='0'>--Lĩnh vực con--</option>");
                //if (lst != null && lst.Count > 0)
                //{
                //    foreach (var item in lst)
                //    {
                //        sb.AppendFormat("<option value='{0}' title='{1}' LoaiKhieuNaiId='{2}' LinhVucChungId='{3}' LoaiKhieuNai='{4}'>{1}</option>", item.Id, item.Name, item.ParentLoaiKhieuNaiId, item.ParentId, item.NameLoaiKhieuNai);
                //    }
                //}
                //context.Response.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
            }
            return lstResult;
        }

        [WebMethod]
        public int CountKhieuNaiPhanHoi(string username)
        {
            System.Web.Caching.Cache cahe = HttpRuntime.Cache;
            string key = string.Concat("CountKhieuNaiPhanHoi_", username.ToLower());
            return cahe.GetData<int>(() =>
               {
                   // Cache trong vòng 1h
                   return cahe.Data<int>(key, (1 * 60 * 60), () =>
                               {
                                   try
                                   {
                                       string spName = "KhieuNai_CountKhieuNaiPhanHoi";
                                       object val = SqlHelper.ExecuteScalar(Config.ConnectionString, spName, username);
                                       if (val != null) return Convert.ToInt32(val);
                                       else return 0;
                                   }
                                   catch (Exception ex)
                                   {
                                       Helper.GhiLogs(ex);
                                   }
                                   return 0;
                               });
               });
        }

        [WebMethod]
        public void TiepNhanKhieuNai(RequestParamGqkn param, string username, ref int MaLoi, ref string MoTaLoi)
        {

            //Kiểm tra user tồn tại trong GQKN
            //Đăng nhập hệ thống
            AdminInfo userLogin = null;
            try
            {
                userLogin = LoginAdmin.fLoginAdmin(username, "123456");
                if (userLogin == null)
                    throw new Exception();
                var loaiPB = new PhongBanImpl().GetInfo(userLogin.PhongBanId);
                if (loaiPB != null)
                {
                    userLogin.LoaiPhongBanId = loaiPB.LoaiPhongBanId;
                    userLogin.IsChuyenTiepKN = loaiPB.IsChuyenTiepKN;
                    userLogin.DefaultHTTN = loaiPB.DefaultHTTN;
                }
            }
            catch
            {
                MaLoi = 10099;
                MoTaLoi = "Tài khoản chưa được tạo khiếu nại.";
                return;
            }

            //Kiểm tra hợp lệ dữ liệu
            try
            {
                if (!ValidSoThueBao(param.SoThueBao.ToString()))
                {
                    throw new ServiceAIException("Số thuê bao chưa hợp lệ.");
                }
                if (param.LoaiKhieuNaiId == 0 || param.LoaiKhieuNai.Equals(""))
                    throw new ServiceAIException("Loại khiếu nại chưa hợp lệ.");

                if (param.LinhVucChungId > 0 && param.LinhVucChung.Equals(""))
                    throw new ServiceAIException("Lĩnh vực chung chưa hợp lệ.");

                if (param.LinhVucConId > 0 && param.LinhVucCon.Equals(""))
                    throw new ServiceAIException("Lĩnh vực con chưa hợp lệ.");

                if (param.DienThoaiLienHe.Equals(""))
                    throw new ServiceAIException("Điện thoại liên hệ chưa hợp lệ.");

                if (param.TinhId == 0 || param.Tinh.Equals(""))
                    throw new ServiceAIException(@"Tỉnh\Thành phố chưa hợp lệ.");

                if (param.QuanHuyenId > 0 && param.QuanHuyen.Equals(""))
                    throw new ServiceAIException(@"Quận\Huyện chưa hợp lệ.");

                if (param.NoiDungPA.Equals(""))
                    throw new ServiceAIException("Nội dung phản ánh chưa hợp lệ.");
            }
            catch (ServiceAIException svex)
            {
                MaLoi = 10001;
                MoTaLoi = svex.Message;
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(param);
                MaLoi = 10002;
                MoTaLoi = "Dữ liệu chưa hợp lệ.";
            }

            try
            {
                var timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

                KhieuNaiInfo item = new KhieuNaiInfo();
                item.LUser = userLogin.Username;

                item.SoThueBao = param.SoThueBao;

                item.KhuVucId = userLogin.KhuVucId;
                item.DoiTacId = userLogin.DoiTacId;
                item.PhongBanTiepNhanId = userLogin.PhongBanId;

                item.PhongBanXuLyId = 0;

                item.LoaiKhieuNaiId = param.LoaiKhieuNaiId;
                item.LinhVucChungId = param.LinhVucChungId;
                item.LinhVucConId = param.LinhVucConId;
                item.LoaiKhieuNai = param.LoaiKhieuNai;

                if (item.LinhVucChungId != 0)
                    item.LinhVucChung = param.LinhVucChung;
                if (item.LinhVucConId != 0)
                    item.LinhVucCon = param.LinhVucCon;

                item.DoUuTien = (byte)param.DoUuTien;

                item.MaTinhId = param.TinhId;
                item.MaTinh = param.Tinh;

                item.MaQuanId = param.QuanHuyenId;
                if (item.MaQuanId != 0)
                    item.MaQuan = param.QuanHuyen;

                item.HoTenLienHe = param.HoTen;
                item.DiaChi_CCBS = param.DiaChi;
                item.DiaChiLienHe = param.DiaChiLienHe;
                item.SDTLienHe = param.DienThoaiLienHe;
                item.DiaDiemXayRa = param.DiaChiSuCo;
                item.ThoiGianXayRa = param.ThoiGianSuCo;
                item.NoiDungPA = param.NoiDungPA;
                item.NoiDungCanHoTro = param.NoiDungHoTro;
                item.GhiChu = param.GhiChu;
                item.TrangThai = (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý;
                item.IsChuyenBoPhan = false;
                item.KhieuNaiFrom = (int)KhieuNai_NguonKhieuNai.IB;

                item.NguoiTiepNhanId = userLogin.Id;
                item.NguoiTiepNhan = userLogin.Username;

                item.HTTiepNhan = (byte)param.HinhThucTiepNhan;
                item.NgayTiepNhan = timeNow;
                item.NgayTiepNhanSort = Convert.ToInt32(timeNow.ToString("yyyyMMdd"));

                //Tính thời gian deadline cho loại khiếu nại.
                // Nếu chọn lĩnh vực con
                var loaiKNIdSelect = item.LoaiKhieuNaiId;
                if (item.LinhVucConId != 0)
                {
                    loaiKNIdSelect = item.LinhVucConId;
                }
                //Nếu chọn lĩnh vực chung
                else if (item.LinhVucChungId != 0)
                {
                    loaiKNIdSelect = item.LinhVucChungId;
                }

                //Tính ngày quá hạn và ngày cảnh báo
                item.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect);
                item.NgayQuaHanSort = Convert.ToInt32(item.NgayQuaHan.ToString("yyyyMMdd"));

                item.NgayCanhBao = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 2);
                item.NgayCanhBaoSort = Convert.ToInt32(item.NgayCanhBao.ToString("yyyyMMdd"));

                item.DoiTacXuLyId = item.DoiTacId;
                item.KhuVucXuLyId = item.KhuVucId;
                item.PhongBanXuLyId = item.PhongBanTiepNhanId;

                item.NguoiXuLyId = item.NguoiTiepNhanId;
                item.NguoiXuLy = item.NguoiTiepNhan;

                //Người tiền xử lý phòng ban. Đây là người tiếp nhận khiếu nại của phòng ban
                item.NguoiTienXuLyCap1Id = 0;
                item.NguoiTienXuLyCap1 = string.Empty;

                item.NgayChuyenPhongBan = timeNow;
                item.NgayChuyenPhongBanSort = Convert.ToInt32(item.NgayChuyenPhongBan.ToString("yyyyMMdd"));

                item.NgayQuaHanPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 1, null, null);
                item.NgayCanhBaoPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 2, null, null);

                item.NgayQuaHanPhongBanXuLySort = Convert.ToInt32(item.NgayQuaHanPhongBanXuLy.ToString("yyyyMMdd"));
                item.NgayCanhBaoPhongBanXuLySort = Convert.ToInt32(item.NgayCanhBaoPhongBanXuLy.ToString("yyyyMMdd"));

                item.NgayTraLoiKN = DateTime.MaxValue;
                item.NgayTraLoiKNSort = 0;
                item.NgayDongKN = DateTime.MaxValue;
                item.NgayDongKNSort = 0;

                item.IsTraSau = false;
                if (param.LoaiThuBao == LoaiThueBao.Trả_sau)
                    item.IsTraSau = true;

                KhieuNai_ActivityInfo itemActivity = null;

                //Activity
                itemActivity = new KhieuNai_ActivityInfo();

                itemActivity.ActivityTruoc = 0;
                itemActivity.GhiChu = "Tạo mới khiếu nại";
                itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
                itemActivity.IsCurrent = true;
                itemActivity.NguoiXuLyTruoc = userLogin.Username;
                itemActivity.NguoiXuLy = userLogin.Username;
                itemActivity.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
                itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
                itemActivity.NgayTiepNhan = item.NgayTiepNhan;
                itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
                itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;
                bool flag = false;

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        item.Id = ServiceFactory.GetInstanceKhieuNai().Add(item);
                        itemActivity.KhieuNaiId = item.Id;
                        itemActivity.Id = ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);

                        scope.Complete();
                        flag = true;
                    }
                    catch (TransactionAbortedException tae)
                    {
                        AIVietNam.Core.Utility.LogEventService(tae);
                        return;
                    }
                    catch (Exception ex)
                    {
                        AIVietNam.Core.Utility.LogEventService(ex);
                        return;
                    }
                }

                if (flag)
                {
                    KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                    buocXuLyInfo.NoiDung = "Tạo mới khiếu nại.";
                    buocXuLyInfo.LUser = item.NguoiTiepNhan;
                    buocXuLyInfo.KhieuNaiId = item.Id;
                    buocXuLyInfo.IsAuto = true;
                    buocXuLyInfo.NguoiXuLyId = userLogin.Id;
                    buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                    KhieuNai_LogInfo logInfo = new KhieuNai_LogInfo();
                    logInfo.KhieuNaiId = item.Id;
                    logInfo.TruongThayDoi = "[Hành động]";
                    logInfo.GiaTriCu = string.Empty;
                    logInfo.ThaoTac = "Thêm mới khiếu nại";
                    logInfo.PhongBanId = userLogin.PhongBanId;
                    logInfo.CUser = userLogin.Username;
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(logInfo);
                }
                string msg = "Thêm mới khiếu nại thành công";
                if (param.IsChuyenKN)
                {

                    msg = "Thêm mới khiếu nại và chuyển tiếp thành công";
                    try
                    {
                        ChuyenTiepKhieuNai(userLogin, loaiKNIdSelect, item, itemActivity);
                    }
                    catch
                    {
                        msg = "Thêm mới khiếu nại thành công nhưng chưa chuyển tiếp được khiếu nại lên phòng ban cấp trên.";
                    }
                }
                MaLoi = 0;
                MoTaLoi = msg;
                //if (IsChuyenTiep.Equals("1") || IsChuyenTiep.Equals("true"))
                //    messgeChuyenTiep = 

                //context.Response.Write(OutputJSONToAJax.ToJSON(0, messgeChuyenTiep, item.Id.ToString()));
            }
            catch (Exception ex)
            {
                AIVietNam.Core.Utility.LogEventService(ex);
                AIVietNam.Core.Utility.LogEventService(param);
                MaLoi = 10003;
                MoTaLoi = "Có lỗi xảy ra khi thêm mới khiếu nại. Bạn thử lại vào lúc khác.";
            }
        }

        //[WebMethod]
        private int CountKhieuNaiDangXuLy(string SoThueBao, ref int MaLoi, ref string MoTaLoi)
        {
            try
            {
                Helper.GhiLogs("Inbound", "Hàm: {0}", "SLKhieuNaiDangXuLy");

                if (ValidSoThueBao(SoThueBao))
                {
                    var lstResult = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id", string.Format("SoThueBao={0} AND TrangThai != 3", SoThueBao), "");
                    if (lstResult != null)
                    {
                        MaLoi = 0;
                        MoTaLoi = "Thành công.";
                        return lstResult.Count;
                    }

                    throw new Exception("Thực hiện thao tác không thành công.");
                }
                else
                {
                    throw new ServiceAIException("Số thuê bao không hợp lệ.");
                }
            }
            catch (ServiceAIException svex)
            {
                MaLoi = 10001;
                MoTaLoi = svex.Message;
                return -1;
            }
            catch (Exception ex)
            {
                MaLoi = 99;
                MoTaLoi = ex.Message;
                return -1;
            }
        }

        private string ChuyenTiepKhieuNai(AdminInfo userLogin, int loaiKNIdSelect, KhieuNaiInfo item, KhieuNai_ActivityInfo activityCurr)
        {
            //Nếu chuyển KN ngay
            var strReturn = "1";


            var phongBanItem = ServiceFactory.GetInstancePhongBan().GetInfo(userLogin.PhongBanId);
            if (phongBanItem.IsDinhTuyenKN)
            {
                strReturn = "2";
                try
                {
                    //Lấy ra List phòng ban chuyển của phòng ban mình
                    var lstPhongBanDuocChuyen = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(userLogin.PhongBanId);
                    if (lstPhongBanDuocChuyen == null || lstPhongBanDuocChuyen.Count == 0)
                    {
                        return "Không tìm thấy phòng ban nào cần chuyển khiếu nại";
                    }

                    var phongBanDuocChuyen_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<int>>(lstPhongBanDuocChuyen[0].PhongBanDen);
                    if (phongBanDuocChuyen_JSON == null || phongBanDuocChuyen_JSON.Count == 0)
                    {
                        return "Không tìm thấy phòng ban nào cần chuyển khiếu nại.";
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
                            return "Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.";
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
                            return "Chưa có phòng ban nào xử lý loại khiếu nại này. Bạn vui lòng liên hệ người quản trị hệ thống.";
                        }
                    }

                    //Lấy ra loại phòng ban của phòng ban xử lý
                    var LoaiPhongBanXuLy = 0;

                    var PhongBanXuLyItem = ServiceFactory.GetInstancePhongBan().GetInfo(item.PhongBanXuLyId);

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
                            AIVietNam.Core.Utility.LogEventService(tae);
                            return Constant.MESSAGE_SERVER_QUA_TAI;
                        }
                        catch (Exception ex)
                        {
                            AIVietNam.Core.Utility.LogEventService(ex);
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
                    AIVietNam.Core.Utility.LogEventService(ex);
                    return ex.Message;
                }
            }


            return strReturn;
        }

        private bool ValidSoThueBao(string SoThueBao)
        {
            var SoThueBaoValid = string.Format("{0}{1}", "84", SoThueBao);

            var strPattern = "^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7}))$";
            Regex rg = new Regex(strPattern);
            if (!rg.Match(SoThueBaoValid).Success)
            {
                return false;
            }
            return true;
        }

        //public int CountKhieuNaiDangXuLy(string SoThueBao, ref int MaLoi, ref string MoTaLoi)
        //{
        //    try {
        //        if (ValidSoThueBao(SoThueBao))
        //        {
        //            var lstResult = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id",string.Format("SoThueBao={0} AND TrangThai != 3", SoThueBao),"");
        //            if (lstResult != null)
        //            {
        //                MaLoi = 0;
        //                MoTaLoi = "Thành công.";
        //                return lstResult.Count;
        //            }

        //            throw new Exception("Thực hiện thao tác không thành công.");
        //        }
        //        else
        //        {
        //            throw new ServiceAIException("Số thuê bao không hợp lệ.");
        //        }
        //    }
        //    catch(ServiceAIException svex)
        //    {
        //        MaLoi = 10001;
        //        MoTaLoi = svex.Message;
        //        return -1;
        //    }
        //    catch(Exception ex) {
        //        MaLoi = 99;
        //        MoTaLoi = ex.Message;
        //        return -1;
        //    }
        //}

    }
}
