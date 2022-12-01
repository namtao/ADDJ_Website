using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.AppCode.Controller;
using System.IO;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Website.Views.QLKhieuNai
{
    public partial class KhieuNai_ThemMoi : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownlist();
                BindData();
            }
        }

        private void CallJavaScriptInUpdatePanel()
        {
            txtSoThueBao.Attributes.Add("onkeypress", "return handleEnter('" + btTraCuu.ClientID + "', event)");
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
                    if (Request.QueryString["MaTinh"] != null)
                    {
                        //var check = ProvinceImpl.ListProvince.Where(t => t.AbbRev == Request.QueryString["MaTinh"] && t.LevelNbr == 1);
                        //if (check.Any())
                        //{
                        //    ddltinh.SelectedValue = check.Single().Id.ToString();

                        //    //var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + ddlTinh.SelectedValue, "Name");
                        //    //lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Quận/Huyện--" });
                        //    //ddlHuyen.DataSource = lstTinh;
                        //    //ddlHuyen.DataTextField = "Name";
                        //    //ddlHuyen.DataValueField = "Id";
                        //    //ddlHuyen.DataBind();
                        //}
                    }
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

            //var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId is null", "Name");
            //lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Tỉnh/Thành Phố--" });
            //ddlTinh.DataSource = lstTinh;
            //ddlTinh.DataTextField = "Name";
            //ddlTinh.DataValueField = "Id";
            //ddlTinh.DataBind();

            //var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
            //lstLoaiKN.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "--Loại khiếu nại--" });

            //ddlLoaiKhieuNai.DataSource = lstLoaiKN;
            //ddlLoaiKhieuNai.DataTextField = "Name";
            //ddlLoaiKhieuNai.DataValueField = "Id";
            //ddlLoaiKhieuNai.DataBind();
        }


        protected void ddlTinh_Changed(object sender, EventArgs e)
        {
            //var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId =" + ddlTinh.SelectedValue, "Name");
            //lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Quận/Huyện--" });
            //ddlHuyen.DataSource = lstTinh;
            //ddlHuyen.DataTextField = "Name";
            //ddlHuyen.DataValueField = "Id";
            //ddlHuyen.DataBind();
        }


        //private bool ValidForm()
        //{
        //    if (txtSoThueBao.Text.Trim().Equals(""))
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa nhập số thuê bao.','error','" + txtSoThueBao.ClientID + "');", true);
        //        return false;
        //    }

        //    var tb = txtDauSo.Text.Trim() + txtSoThueBao.Text.Trim();
        //    var strPattern = "^(84)((9[14]([0-9]){7})|(12[34579]([0-9]){7}))$";
        //    Regex rg = new Regex(strPattern);
        //    if (!rg.Match(tb).Success)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Số thuê bao chưa hợp lệ.','error','" + txtSoThueBao.ClientID + "');", true);
        //        return false;
        //    }

        //    if (hdLoaiKhieuNai.Value.Equals("0"))
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa chọn loại khiếu nại.','error');", true);
        //        return false;
        //    }
        //    if (txtDienThoai.Text.Trim().Equals(""))
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa nhập điện thoại liên hệ.','error','" + txtDienThoai.ClientID + "');", true);
        //        return false;
        //    }

        //    if (txtNoiDungPA.Text.Trim().Equals(""))
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa nhập nội dung phản ánh.','error','" + txtNoiDungPA.ClientID + "');", true);
        //        return false;
        //    }

        //    return true;
        //}

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

        //protected void btSubmit_Click(object sender, EventArgs e)
        //{
        //    if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này.','error');", true);
        //        return;
        //    }

        //    try
        //    {
        //        if (ValidForm())
        //        {
        //            var userLogin = LoginAdmin.AdminLogin();

        //            if (userLogin != null && userLogin.PhongBanId == 0)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Người dùng này chưa được phân vào phòng nên không có quyền tạo khiếu nại.','error');", true);
        //                return;
        //            }

        //            var timeNow = ServiceFactory.GetInstanceGetData().GetTimeFromServer();

        //            KhieuNaiInfo item = new KhieuNaiInfo();
        //            item.LUser = userLogin.Username;

        //            item.SoThueBao = Convert.ToInt64(txtSoThueBao.Text.Trim());

        //            item.KhuVucId = userLogin.KhuVucId;
        //            item.DoiTacId = userLogin.DoiTacId;
        //            item.PhongBanTiepNhanId = userLogin.PhongBanId;

        //            item.PhongBanXuLyId = 0;

        //            item.LoaiKhieuNaiId = Convert.ToInt32(hdLoaiKhieuNai.Value);
        //            item.LinhVucChungId = Convert.ToInt32(hdLinhVucChung.Value);
        //            item.LinhVucConId = Convert.ToInt32(hdLinhVucCon.Value);
        //            item.LoaiKhieuNai = hdLoaiKhieuNai.Attributes["text"];

        //            //if (item.LinhVucChungId != 0)
        //            //    item.LinhVucChung = ddlLinhVucChung.SelectedItem.Text;
        //            //if (item.LinhVucConId != 0)
        //            //    item.LinhVucCon = ddlLinhVucCon.SelectedItem.Text;
        //            item.DoUuTien = Convert.ToByte(ddlDoUuTien.SelectedValue);

        //            //var MaTinhId = Convert.ToInt32(ddlTinh.SelectedValue);
        //            //item.MaTinhId = MaTinhId;
        //            //if (MaTinhId != 0)
        //            //    item.MaTinh = ddlTinh.SelectedItem.Text;
        //            //item.MaQuanId = Convert.ToInt32(ddlHuyen.SelectedValue);

        //            //if (item.MaQuanId != 0)
        //            //    item.MaQuan = ddlHuyen.SelectedItem.Text;

        //            item.HoTenLienHe = txtHoTen.Text.Trim();
        //            item.DiaChi_CCBS = txtDiaChi.Text.Trim();
        //            item.DiaChiLienHe = txtDiaChiLienHe.Text.Trim();
        //            item.SDTLienHe = txtDienThoai.Text.Trim();
        //            item.DiaDiemXayRa = txtDiaDiemSuCo.Text.Trim();
        //            item.ThoiGianXayRa = txtThoiGianSuCo.Text.Trim();
        //            item.NoiDungPA = txtNoiDungPA.Text.Trim();
        //            item.NoiDungCanHoTro = txtNoiDungCanHoTro.Text.Trim();
        //            item.GhiChu = txtGhiChu.Text.Trim();
        //            item.TrangThai = (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý;
        //            item.IsChuyenBoPhan = false;

        //            item.NguoiTiepNhanId = userLogin.Id;
        //            item.NguoiTiepNhan = userLogin.Username;

        //            item.HTTiepNhan = Convert.ToByte(ddlHTTiepNhan.SelectedValue);
        //            item.NgayTiepNhan = timeNow;
        //            item.NgayTiepNhanSort = Convert.ToInt32(timeNow.ToString("yyyyMMdd"));

        //            //Tính thời gian deadline cho loại khiếu nại.
        //            // Nếu chọn lĩnh vực con
        //            var loaiKNIdSelect = item.LoaiKhieuNaiId;
        //            if (item.LinhVucConId != 0)
        //            {
        //                loaiKNIdSelect = item.LinhVucConId;
        //            }
        //            //Nếu chọn lĩnh vực chung
        //            else if (item.LinhVucChungId != 0)
        //            {
        //                loaiKNIdSelect = item.LinhVucChungId;
        //            }

        //            //Tính ngày quá hạn và ngày cảnh báo
        //            item.NgayQuaHan = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect);
        //            item.NgayQuaHanSort = Convert.ToInt32(item.NgayQuaHan.ToString("yyyyMMdd"));

        //            item.NgayCanhBao = GetDataImpl.GetTimeConfig_KhieuNai(timeNow, loaiKNIdSelect, 2);
        //            item.NgayCanhBaoSort = Convert.ToInt32(item.NgayCanhBao.ToString("yyyyMMdd"));

        //            item.DoiTacXuLyId = item.DoiTacId;
        //            item.KhuVucXuLyId = item.KhuVucId;
        //            item.PhongBanXuLyId = item.PhongBanTiepNhanId;

        //            item.NguoiXuLyId = item.NguoiTiepNhanId;
        //            item.NguoiXuLy = item.NguoiTiepNhan;

        //            //Người tiền xử lý phòng ban. Đây là người tiếp nhận khiếu nại của phòng ban
        //            item.NguoiTienXuLyCap1Id = 0;
        //            item.NguoiTienXuLyCap1 = string.Empty;

        //            item.NgayChuyenPhongBan = timeNow;
        //            item.NgayChuyenPhongBanSort = Convert.ToInt32(item.NgayChuyenPhongBan.ToString("yyyyMMdd"));

        //            item.NgayQuaHanPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 1, null, null);
        //            item.NgayCanhBaoPhongBanXuLy = AIVietNam.GQKN.Impl.GetDataImpl.GetTimeConfig_PhongBan(timeNow, loaiKNIdSelect, userLogin.LoaiPhongBanId, 2, null, null);

        //            item.NgayQuaHanPhongBanXuLySort = Convert.ToInt32(item.NgayQuaHanPhongBanXuLy.ToString("yyyyMMdd"));
        //            item.NgayCanhBaoPhongBanXuLySort = Convert.ToInt32(item.NgayCanhBaoPhongBanXuLy.ToString("yyyyMMdd"));

        //            item.NgayTraLoiKN = DateTime.MaxValue;
        //            item.NgayTraLoiKNSort = 0;
        //            item.NgayDongKN = DateTime.MaxValue;
        //            item.NgayDongKNSort = 0;

        //            if (hdIsCallService.Value.Equals("1"))
        //            {
        //                if (hdIsThueBao.Value.Equals("1"))
        //                    item.IsTraSau = true;
        //            }
        //            else
        //            {
        //                try {
        //                    ServiceVNP.ServiceVinaphoneClient obj = new ServiceVNP.ServiceVinaphoneClient();
        //                    ServiceVNP.RequestParam request = new ServiceVNP.RequestParam();
        //                    request.KhuVucId = userInfo.KhuVucId;
        //                    request.Username = userInfo.Username;
        //                    request.SoThueBao = item.SoThueBao.ToString();

        //                    var infoThueBao = obj.GetInfo(request);
        //                    if (infoThueBao != null)
        //                    {
        //                        if(infoThueBao.TEN_LOAI == "Post" || infoThueBao.TEN_LOAI.IndexOf("iTouch") != -1)
        //                        {
        //                            item.IsTraSau = true;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex) { }
        //            }

        //            KhieuNai_ActivityInfo itemActivity = null;

        //            //Activity
        //            itemActivity = new KhieuNai_ActivityInfo();

        //            itemActivity.ActivityTruoc = 0;
        //            itemActivity.GhiChu = "Tạo mới khiếu nại";
        //            itemActivity.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Tạo_Mới;
        //            itemActivity.IsCurrent = true;
        //            itemActivity.NguoiXuLyTruoc = userLogin.Username;
        //            itemActivity.NguoiXuLy = userLogin.Username;
        //            itemActivity.PhongBanXuLyTruocId = item.PhongBanTiepNhanId;
        //            itemActivity.PhongBanXuLyId = item.PhongBanXuLyId;
        //            itemActivity.NgayTiepNhan = item.NgayTiepNhan;
        //            itemActivity.NgayQuaHan = item.NgayQuaHanPhongBanXuLy;
        //            itemActivity.NgayCanhBao = item.NgayCanhBaoPhongBanXuLy;
        //            bool flag = false;

        //            using (TransactionScope scope = new TransactionScope())
        //            {
        //                try
        //                {
        //                    item.Id = ServiceFactory.GetInstanceKhieuNai().Add(item);
        //                    itemActivity.KhieuNaiId = item.Id;
        //                    itemActivity.Id = ServiceFactory.GetInstanceKhieuNai_Activity().Add(itemActivity);

        //                    scope.Complete();
        //                    flag = true;
        //                }
        //                catch (TransactionAbortedException tae)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
        //                    Utility.LogEvent(tae);
        //                    return;
        //                }
        //                catch (Exception ex)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
        //                    Utility.LogEvent(ex);
        //                    return;
        //                }
        //            }

        //            if (flag)
        //            {
        //                KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
        //                buocXuLyInfo.NoiDung = "Tạo mới khiếu nại.";
        //                buocXuLyInfo.LUser = item.NguoiTiepNhan;
        //                buocXuLyInfo.KhieuNaiId = item.Id;
        //                buocXuLyInfo.IsAuto = true;
        //                buocXuLyInfo.NguoiXuLyId = userLogin.Id;
        //                buocXuLyInfo.PhongBanXuLyId = userLogin.PhongBanId;
        //                ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

        //                KhieuNai_LogInfo logInfo = new KhieuNai_LogInfo();
        //                logInfo.KhieuNaiId = item.Id;
        //                logInfo.TruongThayDoi = "[Hành động]";
        //                logInfo.GiaTriCu = string.Empty;
        //                logInfo.ThaoTac = "Thêm mới khiếu nại";
        //                logInfo.PhongBanId = userInfo.PhongBanId;
        //                logInfo.CUser = userLogin.Username;
        //                ServiceFactory.GetInstanceKhieuNai_Log().Add(logInfo);
        //            }

        //            //Upload File
        //            if ((!(FileUploadJquery == null)) && (!(FileUploadJquery.PostedFile == null)) &&
        //                    !string.IsNullOrEmpty(FileUploadJquery.PostedFile.FileName))
        //            {
        //                KhieuNai_FileDinhKemInfo fileInfo = new KhieuNai_FileDinhKemInfo();
        //                fileInfo.TenFile = Path.GetFileName(FileUploadJquery.PostedFile.FileName);
        //                fileInfo.LoaiFile = Path.GetExtension(FileUploadJquery.PostedFile.FileName);
        //                fileInfo.KichThuoc = FileUploadJquery.PostedFile.ContentLength / 1024;

        //                var strFileName = KhieuNai_FileDinhKemImpl.UploadFile(FileUploadJquery, item.Id, timeNow);

        //                fileInfo.KhieuNaiId = item.Id;
        //                fileInfo.GhiChu = "File KH gửi";
        //                fileInfo.Status = (byte)FileDinhKem_Status.File_KH_Gửi;
        //                fileInfo.CUser = userLogin.Username;
        //                fileInfo.URLFile = strFileName;

        //                ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Add(fileInfo);
        //            }

        //            Reset();

        //            var messgeChuyenTiep = ChuyenTiepKhieuNai(userLogin, loaiKNIdSelect, item, itemActivity);

        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onload", "window.parent.ThemKNSuccess('" + messgeChuyenTiep + "');", true);
        //        }
        //    }
        //    catch (System.Data.SqlClient.SqlException se)
        //    {
        //        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Có lỗi xảy ra khi cập nhật dữ liệu.','error');", true);
        //        //Utility.LogEvent(se);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Có lỗi xảy ra khi cập nhật dữ liệu.','error');", true);
        //        //Utility.LogEvent(ex);
        //        //return;
        //    }
        //}

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