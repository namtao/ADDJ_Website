using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Data;
using AIVietNam.Log.Impl;
using AIVietNam.Log.Entity;

namespace Website.admin
{
    public partial class LoiKhieuNai_add : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
                BindPhienBan();
                BindDropDownlist(ddlPhienBan.SelectedItem.Value.Split('-')[0]);
                if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != string.Empty)
                {
                    EditData();
                    ddlPhienBan.Enabled = false;
                }
                else
                {
                    txtThuTu.Text = GetMaxLoiKhieuNai(0);
                }
            }
        }
        private void BindPhienBan()
        {
            var lst = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("MaLoi = SUBSTRING(CONVERT(varchar(8), TuNgay),7,2) + '/' + SUBSTRING(CONVERT(varchar(8), TuNgay),5,2) + '/' + SUBSTRING(CONVERT(varchar(8), TuNgay) ,1,4) + N' đến ' + SUBSTRING(CONVERT(varchar(8), DenNgay),7,2) + '/' + SUBSTRING(CONVERT(varchar(8), DenNgay),5,2) + '/' + SUBSTRING(CONVERT(varchar(8), DenNgay),1,4), TenLoi = CONVERT(varchar(8), TuNgay) + '-' + CONVERT(varchar(8), DenNgay)", "1=1 GROUP BY TuNGay, DenNgay", "TuNgay");
            ddlPhienBan.DataSource = lst;
            ddlPhienBan.DataTextField = "MaLoi";
            ddlPhienBan.DataValueField = "TenLoi";
            ddlPhienBan.DataBind();
            ListItem item = new ListItem("-- Tháng sau --", "1");
            ddlPhienBan.Items.Insert(ddlPhienBan.Items.Count, item);
            
        }
        private void BindDropDownlist(string tungay)
        {
            List<LoiKhieuNaiInfo> listLoiKhieuNai = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("*", "Cap = 1 AND TuNgay =" + tungay, "ThuTu ASC");

            ddlLoiKhieuNai.DataSource = listLoiKhieuNai;
            ddlLoiKhieuNai.DataTextField = "TenLoi";
            ddlLoiKhieuNai.DataValueField = "Id";
            ddlLoiKhieuNai.DataBind();

            ListItem item = new ListItem("-- Chọn nguyên nhân --", "0");
            ddlLoiKhieuNai.Items.Insert(0, item);
        }

        private void EditData()
        {
            try
            {
                LoiKhieuNaiImpl obj = ServiceFactory.GetInstanceLoiKhieuNai();
                LoiKhieuNaiInfo item = obj.GetInfo(int.Parse(Request.QueryString["Id"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData loikhieunai_add get NullId " + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                else
                {
                    ddlPhienBan.SelectedValue = item.TuNgay.ToString() + "-" + item.DenNgay.ToString();
                    BindDropDownlist(ddlPhienBan.SelectedItem.Value.Split('-')[0]);
                    ddlLoiKhieuNai.SelectedValue = item.ParentId.ToString();
                    txtMaLoi.Text = item.MaLoi;
                    txtTenLoi.Text = item.TenLoi;
                    txtThuTu.Text = item.ThuTu.ToString();
                    txtCap.Text = item.Cap.ToString();
                    chkHoatDong.Checked = item.HoatDong;
                    ddlLoai.SelectedValue = item.Loai.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
 

        protected void ddlLoiKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoiKhieuNai.SelectedValue == "0")
            {
                txtCap.Text = "1";
            }
            else
            {
                txtCap.Text = "2";
            }

            txtThuTu.Text = GetMaxLoiKhieuNai(ConvertUtility.ToInt32(ddlLoiKhieuNai.SelectedValue, 0));
        }

        public string GetMaxLoiKhieuNai(int parentId)
        {
            string sql = string.Empty;
            object soThuTu = null;

            if (parentId == 0)
            {
                sql = @"SELECT LEFT((MAX(ThuTu) + 10000), 1) + '0000'  FROM LoiKhieuNai 
                            WHERE Cap = 1
	                            AND ThuTu < 99000";
            }
            else
            {
                sql = string.Format(@"SELECT MAX(ThuTu) + 1  FROM LoiKhieuNai 
                        WHERE ParentId = {0}
	                        AND ThuTu < 99000", parentId);
            }

            soThuTu = ServiceFactory.GetInstanceLoaiKhieuNai().ExecuteScalar(sql, CommandType.Text, null);

            return soThuTu != null ? soThuTu.ToString() : "0";
        }
        protected void ddlPhienBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownlist(ddlPhienBan.SelectedItem.Value.Split('-')[0]);
        }

        protected void linkbtnSubmit_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                AdminInfo userInfo = LoginAdmin.AdminLogin();
                LoiKhieuNaiImpl obj = ServiceFactory.GetInstanceLoiKhieuNai();
                if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != string.Empty)
                {
                    try
                    {
                        int idEdit = int.Parse(Request.QueryString["Id"]);
                        LoiKhieuNaiInfo info = obj.GetInfo(idEdit);

                        if (info == null)
                        {
                            Utility.LogEvent("Function loikhieunai_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                            Response.Redirect(Config.PathError, false);
                            return;
                        }

                        try
                        {
                            List<string> danhsachThayDoi = new List<string>();

                            // Nguyên nhân thay đổi
                            int oldParentId = info.ParentId;

                            string oldParentName = "Là gốc";

                            if (oldParentId != 0) oldParentName = obj.GetInfo(oldParentId).TenLoi;

                            if (oldParentId != Convert.ToInt32(ddlLoiKhieuNai.SelectedValue))
                            {
                                // Log nguyên nhân thay đổi
                                string thayDoi = string.Format("[Nguyên nhân lỗi thay đổi: Tên cũ ({0}), Tên mới ({1})]", oldParentName, ddlLoiKhieuNai.SelectedItem.Text);
                                danhsachThayDoi.Add(thayDoi);
                            }

                            // Tên lỗi thay đổi
                            string oldName = info.TenLoi;
                            if (oldName.ToLower() != txtTenLoi.Text.Trim().ToLower())
                            {
                                // Log tên nguyên nhân thay đổi
                                string thayDoi = string.Format("[Tên lỗi thay đổi: Tên cũ ({0}), Tên mới ({1})]", oldName, txtTenLoi.Text.Trim());
                                danhsachThayDoi.Add(thayDoi);
                            }

                            int oldLoaiNoi = info.Loai; // 1: Hỗ trợ, 2: Khiếu nại
                            if (oldLoaiNoi != Convert.ToInt32(ddlLoai.SelectedValue))
                            {
                                // Loại lỗi thay đổi
                                string thayDoi = string.Format("[Loại nguyên nhân thay đổi: Tên cũ ({0}), Tên mới ({1})]", oldLoaiNoi == 1 ? "Hỗ trợ" : "Khiếu nại", ddlLoai.SelectedValue == "1" ? "Hỗ trợ" : "Khiếu nại");
                                danhsachThayDoi.Add(thayDoi);
                            }
                            info.ParentId = ConvertUtility.ToInt32(ddlLoiKhieuNai.SelectedValue, 0);
                            info.MaLoi = txtMaLoi.Text;
                            info.TenLoi = txtTenLoi.Text;
                            info.ThuTu = ConvertUtility.ToInt32(txtThuTu.Text);
                            info.Cap = ConvertUtility.ToInt32(txtCap.Text);
                            info.HoatDong = chkHoatDong.Checked;
                            info.Loai = ConvertUtility.ToInt32(ddlLoai.SelectedValue);
                            info.LUser = userInfo.Username;

                            if (danhsachThayDoi.Count > 0) // Có thay đổi giá trị
                            {
                                LogImpl ctl = new LogImpl();
                                LogInfo logInfo = new LogInfo();

                                logInfo.Action = (int)ActionLog.Sua;
                                //logInfo.ObjType = (int)ObjTypeLog.LoiKhieuNai;

                                string ipUser = Utility.GetIP();


                                logInfo.ObjId = info.Id;

                                //logInfo.ObjName = ObjTypeLog.LoiKhieuNai.Name();
                                logInfo.Note = string.Join(" : ", danhsachThayDoi);

                                logInfo.Ip = ipUser;
                                logInfo.UserId = userInfo.Id;
                                logInfo.Username = userInfo.Username;

                                logInfo.DateCreate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                                logInfo.TimeCreate = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                                ctl.Add(logInfo);
                            }
                        }
                        catch
                        {
                            lblMsg.Text = "Dữ liệu không hợp lệ";
                            return;
                        }

                        obj.Update(info);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }
                }
                else
                {
                    LoiKhieuNaiInfo info = new LoiKhieuNaiInfo();
                    try
                    {
                        info.ParentId = ConvertUtility.ToInt32(ddlLoiKhieuNai.SelectedValue, 0);
                        info.MaLoi = txtMaLoi.Text;
                        info.TenLoi = txtTenLoi.Text;
                        info.ThuTu = ConvertUtility.ToInt32(txtThuTu.Text);
                        info.Cap = ConvertUtility.ToInt32(txtCap.Text);
                        info.HoatDong = chkHoatDong.Checked;
                        info.Loai = ConvertUtility.ToInt32(ddlLoai.SelectedValue);
                        var tungay = 0;
                        var denngay = 50000101;
                        if (ConvertUtility.ToInt32(ddlPhienBan.SelectedItem.Value) == 1 || ConvertUtility.ToInt32(ddlPhienBan.SelectedItem.Value) == 2)
                        {
                            var ngayDauThang = DateTime.Now.AddMonths(ConvertUtility.ToInt32(ddlPhienBan.SelectedItem.Value)).StartOfMonth();
                            tungay = ConvertUtility.ToInt32(ngayDauThang.ToString("yyyyMMdd"));
                            //Nếu tungay mới không trùng tungay cuối cùng trong bảng thì cập nhật DenNgay của bản ghi cuối
                            if (tungay.ToString() != ddlPhienBan.Items[ddlPhienBan.Items.Count - 2].Value.Split('-')[0])
                                obj.UpdateDynamic("DenNgay=" + ngayDauThang.AddDays(-1).ToString("yyyyMMdd"), "DenNgay=50000101");
                        }
                        else
                        {
                            tungay = ConvertUtility.ToInt32(ddlPhienBan.SelectedItem.Value.Split('-')[0]);
                            denngay = ConvertUtility.ToInt32(ddlPhienBan.SelectedItem.Value.Split('-')[1]);
                        }
                        info.TuNgay = tungay;
                        info.DenNgay = denngay;
                        info.LUser = userInfo.Username;


                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    // Thực thi thêm mới
                    int loiKhieuNaiId = obj.Add(info);

                    // Ghi nhận thêm mới
                    LogImpl ctl = new LogImpl();
                    LogInfo logInfo = new LogInfo();

                    logInfo.Action = (int)ActionLog.ThemMoi;
                    //logInfo.ObjType = (int)ObjTypeLog.LoiKhieuNai;

                    string ipUser = Utility.GetIP();


                    logInfo.ObjId = loiKhieuNaiId;

                    //logInfo.ObjName = ObjTypeLog.LoiKhieuNai.Name();
                    string nguyenNhan = "Là gốc";
                    if (ddlLoiKhieuNai.SelectedValue != "0")
                        nguyenNhan = obj.GetInfo(Convert.ToInt32(ddlLoiKhieuNai.SelectedValue)).TenLoi;

                    logInfo.Note = string.Join(" : ", string.Format("Tên: {0}, Cấp cha: {1}, Loại: {2}", txtTenLoi.Text, nguyenNhan, ddlLoai.SelectedValue == "1" ? "Hỗ trợ" : "Khiếu nại"));

                    logInfo.Ip = ipUser;
                    logInfo.UserId = userInfo.Id;
                    logInfo.Username = userInfo.Username;

                    logInfo.DateCreate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    logInfo.TimeCreate = Convert.ToInt32(DateTime.Now.ToString("HHmm"));
                    ctl.Add(logInfo);

                }

                string url = "LoiKhieuNai_manager.aspx";
                if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                    url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);

                Response.Redirect(url, false);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                lblMsg.Text = "Lỗi khiếu nại đã tồn tại.";
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
    }
}