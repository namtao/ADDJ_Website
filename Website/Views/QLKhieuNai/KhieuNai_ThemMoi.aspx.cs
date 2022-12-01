using System;
using System.Web;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using System.Transactions;

namespace Website.Views.QLKhieuNai
{
    public partial class KNThemMoi : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownlist();
                BindData();
            }
        }
        private void BindData()
        {
            AdminInfo admin = LoginAdmin.AdminLogin();
            if (admin != null)
            {
                ddlHTTiepNhan.SelectedValue = admin.DefaultHTTN.ToString();
                chkIsChuyenTiep.Checked = admin.IsChuyenTiepKN;
            }

            if (Request.QueryString["SoThueBao"] != null)
            {
                txtSoThueBao.Text = Request.QueryString["SoThueBao"];
            }
            if (Request.QueryString["LoaiTB"] != null && !Request.QueryString["LoaiTB"].Equals(""))
            {
                var loaiTB = Request.QueryString["LoaiTB"];
                hdIsCallService.Value = "1";
                if (loaiTB == "Post" || loaiTB.ToLower().Contains("itouch"))
                {
                    hdIsThueBao.Value = "1";
                    ddlLoaiThueBao.SelectedValue = hdIsThueBao.Value;
                }
            }
            if (Request.QueryString["TenTB"] != null)
            {
                txtHoTen.Text = HttpUtility.UrlDecode(Request.QueryString["TenTB"]);
            }

            if (Request.QueryString["DCTT"] != null)
            {
                txtDiaChi.Text = HttpUtility.UrlDecode(Request.QueryString["DCTT"]);
            }
        }

        private void BindDropDownlist()
        {
            foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
            {
                ddlDoUuTien.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " "), i.ToString()));
            }

            foreach (byte i in Enum.GetValues(typeof(KhieuNai_HTTiepNhan_Type)))
            {
                ddlHTTiepNhan.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " "), i.ToString()));
            }

        }





        private void Reset()
        {
            txtDiaChi.Text = "";
            txtDiaChiLienHe.Text = "";
            txtDiaDiemSuCo.Text = "";
            txtDienThoai.Text = "";
            txtGhiChu.Text = "";
            txtHoTen.Text = "";
            txtNoiDungCanHoTro.Text = "";
            txtNoiDungPA.Text = "";
            txtSoThueBao.Text = "";
            txtThoiGianSuCo.Text = "";
            ddlDoUuTien.SelectedIndex = 0;
            ddlHTTiepNhan.SelectedIndex = 0;
            //ddlHuyen.SelectedIndex = 0;
            //ddlLinhVucChung.SelectedIndex = 0;
            //ddlLinhVucCon.SelectedIndex = 0;
            //ddlLoaiKhieuNai.SelectedIndex = 0;
            //ddlTinh.SelectedIndex = 0;
        }
        private string ChuyenTiepKhieuNai(AdminInfo userLogin, int loaiKNIdSelect, KhieuNaiInfo item, KhieuNai_ActivityInfo activityCurr)
        {
            //Nếu chuyển KN ngay
            var strReturn = "1";

            if (chkIsChuyenTiep.Checked)
            {
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
                            logInfo.PhongBanId = UserInfo.PhongBanId;
                            ServiceFactory.GetInstanceKhieuNai_Log().Add(logInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        return ex.Message;
                    }
                }
            }

            return strReturn;
        }
    }
}