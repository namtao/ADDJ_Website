using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Website.Components;

public partial class Admin_LoaiKhieuNai_Add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = string.Empty;
            BindDropDownlist();
            ChangeLoaiKN();
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                EditData();
            }
        }
    }

    private void RenderServiceCode()
    {
        string str = "CP1234";
        try
        {
            string commandText = "SELECT MAX(CAST(SUBSTRING(ServiceCode, 3, 4) AS INT)) + 1  FROM LoaiKhieuNai a  WHERE  1 = 1  AND a.ServiceCode IS NOT NULL AND a.ServiceCode LIKE 'CP%'";
            str = $"CP{((int)SqlHelper.ExecuteScalar(Config.ConnectionString, CommandType.Text, commandText)).ToString("0000")}";
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
        }
        txtServiceCode.Text = str;
    }
    private void BindDropDownlist()
    {
        List<LoaiKhieuNaiInfo> list = ServiceFactory.GetInstanceLoaiKhieuNai().GetListLoaiKhieuNai2Cap();

        list.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "-- Là cha --" });
        ddlParrent.DataTextField = "Name";
        ddlParrent.DataValueField = "Id";
        ddlParrent.DataSource = list;
        ddlParrent.DataBind();

        List<LoaiKhieuNai_NhomInfo> listLoaiKhieuNaiNhom = ServiceFactory.GetInstanceLoaiKhieuNaiNhom().GetListDynamic("*", "", "TenNhom ASC");
        listLoaiKhieuNaiNhom.Insert(0, new LoaiKhieuNai_NhomInfo() { Id = 0, TenNhom = "-- Chọn nhóm --" });
        ddlLoaiKhieuNaiNhom.DataSource = listLoaiKhieuNaiNhom;
        ddlLoaiKhieuNaiNhom.DataTextField = "TenNhom";
        ddlLoaiKhieuNaiNhom.DataValueField = "Id";
        ddlLoaiKhieuNaiNhom.DataBind();

        // Công ty cung cấp dịch vụ
        List<CongTyDoiTacInfo> lstDoiTac = new CongTyDoiTacImpl().GetListDynamic("Id, Ten", string.Format("TrangThai = 1"), string.Empty);
        ddlCompany.DataSource = lstDoiTac;
        ddlCompany.DataValueField = "Id";
        ddlCompany.DataTextField = "Ten";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("-- Chọn công ty --", "0"));

        // Bind Đơn vị quản lý
        new EnumDonViQuanLyHelper().Bind2Control(ddlDonViQuanLy, "--  Lựa chọn --", string.Empty);
    }

    private bool ValidThoiGianXuLy(string str)
    {
        string[] arr = str.Split('d', 'h');

        foreach (string item in arr)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            if (!Utility.IsInteger(item))
                return false;
        }

        return true;
    }

    private void EditData()
    {
        try
        {
            LoaiKhieuNaiImpl obj = ServiceFactory.GetInstanceLoaiKhieuNai();
            LoaiKhieuNaiInfo info = obj.GetInfo(int.Parse(Request.QueryString["Id"]));
            if (info == null)
            {
                Utility.LogEvent("Function EditData loaiKhieuNai_add get NullId " + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                ddlParrent.SelectedValue = info.ParentId.ToString();
                ddlParrent.Attributes.Add("valueOld", info.ParentId.ToString());

                txtName.Text = info.Name.ToString();
                txtDescription.Text = info.Description.ToString();
                txtSort.Text = info.Sort.ToString();
                txtSort.Attributes.Add("valueOld", info.Sort.ToString());

                chkStatus.Checked = info.Status == 1 ? true : false;
                txtThoiGianUocTinh.Text = info.ThoiGianUocTinh;
                txtThoiGianCanhBao.Text = info.ThoiGianCanhBao;
                txtMaDV.Text = info.MaDichVu;
                ddlThuocDonVi.SelectedValue = info.ThuocDonVi.ToString();
                ddlLoaiKhieuNaiNhom.SelectedValue = info.LoaiKhieuNai_NhomId.ToString();

                ddlCompany.SelectedValue = info.CongTyDoiTacId.ToString();

                // Thêm ServiceCode => Mã dịch vụ lấy doanh thu
                txtServiceCode.Text = info.ServiceCode;
                try
                {
                    // Đơn vị quản lý
                    ddlDonViQuanLy.SelectedValue = info.DonViQuanLyId.ToString();
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
    protected void btSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            LoaiKhieuNaiImpl ctl = ServiceFactory.GetInstanceLoaiKhieuNai();
            if (!string.IsNullOrEmpty(Request.QueryString["Id"])) // Trạng thái Update
            {
                #region Trạng thái cập nhật
                try
                {
                    int idEdit = int.Parse(Request.QueryString["Id"]);
                    LoaiKhieuNaiInfo info = ctl.GetInfo(idEdit);

                    if (info == null)
                    {
                        Utility.LogEvent("Function loaiKhieuNai_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        info.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                        info.Name = txtName.Text.Trim();
                        info.Description = txtDescription.Text.Trim();
                        info.Sort = Convert.ToInt32(txtSort.Text.Trim());
                        info.MaDichVu = txtMaDV.Text.Trim();

                        byte capNew = 1;
                        if (info.ParentId == 0) capNew = 1;
                        else if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;"))) capNew = 3;
                        else capNew = 2;

                        if (info.Cap < capNew)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Hệ thống không cho phép chuyển loại khiếu nại từ cấp cha xuống cấp con.','error');", true);
                            return;
                        }

                        info.Cap = capNew;
                        info.Status = chkStatus.Checked ? (byte)1 : (byte)0;
                        info.ThoiGianUocTinh = txtThoiGianUocTinh.Text.Trim();
                        info.ThoiGianCanhBao = txtThoiGianCanhBao.Text.Trim();

                        info.ThuocDonVi = ConvertUtility.ToInt32(ddlThuocDonVi.SelectedValue, DoiTacInfo.DoiTacIdValue.VNP);
                        info.LoaiKhieuNai_NhomId = ConvertUtility.ToInt32(ddlLoaiKhieuNaiNhom.SelectedValue);

                        info.LoaiKhieuNai_TenNhom = info.LoaiKhieuNai_NhomId != 0 ? new LoaiKhieuNai_NhomImpl().GetInfo(info.LoaiKhieuNai_NhomId).TenNhom : string.Empty;

                        // Đơn vị quản lý
                        info.DonViQuanLyId = Convert.ToInt32(ddlDonViQuanLy.SelectedValue);

                        if (ddlParrent.SelectedValue != "0")
                        {
                            string sql = string.Format("SELECT dbo.LoaiKhieuNai_GetRootByChildId ({0})", ddlParrent.SelectedValue);
                            object val = SqlHelper.ExecuteScalar(Config.ConnectionString, CommandType.Text, sql);
                            if (val != null && val.ToString() != string.Empty)
                            {
                                int rootId = Convert.ToInt32(val);
                                info.ParentLoaiKhieuNaiId = rootId;
                            }
                        }

                        // Xử lý "Công ty cung cấp dịch vu"
                        info.CongTyDoiTacId = Convert.ToInt32(ddlCompany.SelectedValue);
                        if (info.CongTyDoiTacId > 0)
                        {
                            CongTyDoiTacInfo dtInfo = new CongTyDoiTacImpl().GetInfo(info.CongTyDoiTacId);
                            info.TenCongTyDoiTac = dtInfo.Ten;
                        }
                        else info.TenCongTyDoiTac = string.Empty;

                        if (info.ServiceCode == null) info.ServiceCode = string.Empty;

                        if (txtServiceCode.Text.Trim().ToLower() != info.ServiceCode.ToLower()) // Nếu có sự thay đổi về ServiceCode
                        {
                            // Xử lý ServiceCode
                            string oldServiceCode = info.ServiceCode;
                            byte oldIsChungMa = info.IsChungMa;

                            if (!string.IsNullOrEmpty(txtServiceCode.Text))
                            {
                                // Nếu cái cũ có chung mã => Cần xử lý
                                if (oldIsChungMa == 1)
                                {
                                    // Lấy danh sách chung mã cũ
                                    List<LoaiKhieuNaiInfo> lstCu = ctl.GetListDynamic("*", string.Format("ServiceCode = N'{0}' AND Id <> {1}", oldServiceCode, info.Id), string.Empty);
                                    if (lstCu != null && lstCu.Count == 1) // Chỉ xử lý nếu chỉ còn 1 cái mà thôi, 2 cái => Không cần thay đổ vì nó vẫn chung mã
                                    {
                                        lstCu[0].IsChungMa = 0; // Bỏ chung mã
                                        ctl.Update(lstCu[0]);
                                    }
                                }

                                // Lấy ra danh sách mà có chung mã với mã mới, khác với cái đang làm việc
                                List<LoaiKhieuNaiInfo> dsLoaiKhieuNai = ctl.GetListDynamic("*", string.Format("ServiceCode = N'{0}' AND Id <> {1}", txtServiceCode.Text.Trim(), info.Id), string.Empty);
                                if (dsLoaiKhieuNai != null && dsLoaiKhieuNai.Count > 0)
                                {
                                    info.IsChungMa = 1;
                                    // Cần cập nhật danh sách chung mã
                                    foreach (LoaiKhieuNaiInfo item in dsLoaiKhieuNai)
                                    {
                                        item.IsChungMa = 1;
                                        ctl.Update(item);
                                    }
                                }
                                else
                                {
                                    info.IsChungMa = 0;
                                }
                                info.ServiceCode = txtServiceCode.Text;
                            }
                            else
                            {
                                if (oldIsChungMa == 1) // Nếu chung mã cần loại bỏ nếu chỉ còn 1 cái
                                {
                                    // Lấy danh sách chung mã cũ
                                    List<LoaiKhieuNaiInfo> lstCu = ctl.GetListDynamic("*", string.Format("ServiceCode = N'{0}' AND Id <> {1}", oldServiceCode, info.Id), string.Empty);
                                    if (lstCu != null && lstCu.Count == 1) // Chỉ xử lý nếu chỉ còn 1 cái mà thôi, 2 cái => Không cần thay đổ vì nó vẫn chung mã
                                    {
                                        lstCu[0].IsChungMa = 0; // Bỏ chung mã
                                        ctl.Update(lstCu[0]);
                                    }
                                }

                                info.ServiceCode = txtServiceCode.Text;
                                info.IsChungMa = 0;
                            }
                        }

                        if (!ValidThoiGianXuLy(info.ThoiGianUocTinh) || !ValidThoiGianXuLy(info.ThoiGianCanhBao))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thời gian chưa hợp lệ.','error');", true);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    info.LDate = DateTime.Now;
                    info.LUser = UserInfo.Username;

                    ctl.Update(info);
                    ChangeSort(info.Id, info.Cap, info.Sort);
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                #endregion
            }
            else
            {
                #region Trạng thái thêm mới

                LoaiKhieuNaiInfo info = new LoaiKhieuNaiInfo();

                try
                {
                    info.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                    info.Name = txtName.Text.Trim();
                    info.Description = txtDescription.Text.Trim();
                    info.Sort = Convert.ToInt32(txtSort.Text.Trim());
                    info.MaDichVu = txtMaDV.Text.Trim();

                    if (info.ParentId == 0) info.Cap = 1;
                    else if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;"))) info.Cap = 3;
                    else info.Cap = 2;

                    info.Status = chkStatus.Checked ? (byte)1 : (byte)0;
                    info.ThoiGianUocTinh = txtThoiGianUocTinh.Text.Trim();
                    info.ThoiGianCanhBao = txtThoiGianCanhBao.Text.Trim();

                    info.ThuocDonVi = ConvertUtility.ToInt32(ddlThuocDonVi.SelectedValue, DoiTacInfo.DoiTacIdValue.VNP);

                    info.LoaiKhieuNai_NhomId = Convert.ToInt32(ddlLoaiKhieuNaiNhom.SelectedValue);
                    info.LoaiKhieuNai_TenNhom = info.LoaiKhieuNai_NhomId != 0 ? new LoaiKhieuNai_NhomImpl().GetInfo(info.LoaiKhieuNai_NhomId).TenNhom : string.Empty;

                    // Đơn vị quản lý
                    info.DonViQuanLyId = Convert.ToInt32(ddlDonViQuanLy.SelectedValue);

                    // Xử lý "ParentLoaiKhieuNaiId"
                    if (ddlParrent.SelectedValue != "0")
                    {
                        string sql = string.Format("SELECT dbo.LoaiKhieuNai_GetRootByChildId ({0})", ddlParrent.SelectedValue);
                        object val = SqlHelper.ExecuteScalar(Config.ConnectionString, CommandType.Text, sql);
                        if (val != null && val.ToString() != string.Empty)
                        {
                            int rootId = Convert.ToInt32(val);
                            info.ParentLoaiKhieuNaiId = rootId;
                        }
                    }
                    else
                    {
                        info.ParentLoaiKhieuNaiId = 0; // Gốc gác
                    }
                    // Công ty cung cấp dịch vụ
                    info.CongTyDoiTacId = Convert.ToInt32(ddlCompany.SelectedValue);
                    if (info.CongTyDoiTacId > 0)
                    {
                        CongTyDoiTacInfo dtInfo = new CongTyDoiTacImpl().GetInfo(info.CongTyDoiTacId);
                        info.TenCongTyDoiTac = dtInfo.Ten;
                    }

                    // Mã dịch vụ lấy doanh thu
                    if (!string.IsNullOrEmpty(txtServiceCode.Text))
                    {
                        info.ServiceCode = txtServiceCode.Text; // Cập nhật thêm ServiceCode
                        List<LoaiKhieuNaiInfo> dsLoaiKhieuNai = ctl.GetListDynamic("*", string.Format("ServiceCode = N'{0}'", txtServiceCode.Text), string.Empty);
                        if (dsLoaiKhieuNai != null && dsLoaiKhieuNai.Count > 0)
                        {
                            foreach (LoaiKhieuNaiInfo item in dsLoaiKhieuNai)
                            {
                                item.IsChungMa = 1;
                                ctl.Update(item);
                            }
                            info.IsChungMa = 1;
                        }
                    }

                    if (!ValidThoiGianXuLy(info.ThoiGianUocTinh) || !ValidThoiGianXuLy(info.ThoiGianCanhBao))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thời gian chưa hợp lệ.','error');", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string mesage = "Dữ liệu không hợp lệ";
                    if (UserInfo.Username.ToLower() == "Administrator".ToLower()) mesage += string.Format(" - Lỗi: {0}", ex.Message);
                    lblMsg.Text = mesage;
                    return;
                }

                // Thông tin thời gian
                info.CDate = DateTime.Now;
                info.LDate = DateTime.Now;

                // Thông tin người xử lý
                info.CUser = UserInfo.Username;
                info.LUser = UserInfo.Username;
                ctl.Add(info);
                #endregion
            }

            LoaiKhieuNaiImpl.ListLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
            Response.Redirect("LoaiKhieuNai_Manager.aspx", false);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            lblMsg.Text = "Tên loại khiếu nại đã tồn tại.";
            Helper.GhiLogs(se);
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Có lỗi xảy ra khi cập nhật dữ liệu.";
            Helper.GhiLogs(ex);
            return;
        }
    }

    protected void ddlParrent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeLoaiKN();
    }

    protected void ChangeLoaiKN()
    {
        if (ddlParrent.SelectedValue.Equals(ddlParrent.Attributes["valueOld"]))
        {
            txtSort.Text = txtSort.Attributes["valueOld"];
            return;
        }
        if (ddlParrent.SelectedValue.Equals("0"))
        {
            List<LoaiKhieuNaiInfo> lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("MAX(Sort)+1000000 as Sort", "ParentId=0", "");
            if (lstSortParentMax != null && lstSortParentMax.Count > 0)
                txtSort.Text = lstSortParentMax[0].Sort.ToString();
            else
                txtSort.Text = "1000000";
        }
        else
        {
            if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;")))
            {
                List<LoaiKhieuNaiInfo> lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("MAX(Sort)+1 as Sort", "Id= " + ddlParrent.SelectedValue + " OR ParentId=" + ddlParrent.SelectedValue, "");
                if (lstSortParentMax != null && lstSortParentMax.Count > 0)
                    txtSort.Text = lstSortParentMax[0].Sort.ToString();
                else
                    txtSort.Text = "1000000";
            }
            else
            {
                List<LoaiKhieuNaiInfo> lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("MAX(Sort)+1000 as Sort", "Id= " + ddlParrent.SelectedValue + " OR ParentId=" + ddlParrent.SelectedValue, "");
                if (lstSortParentMax != null && lstSortParentMax.Count > 0)
                    txtSort.Text = lstSortParentMax[0].Sort.ToString();
                else
                    txtSort.Text = "1000000";
            }
        }

        // Cập nhật vào các trường LoaiKhieuNai_NhomId, LoaiKhieuNai_TenNhom, ParentLoaiKhieuNaiId, NameLoaiKhieuNai
        if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;")))
        {
            List<LoaiKhieuNaiInfo> listLoaiKhieuNaiTemp = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("*", "Id IN (SELECT ParentId FROM LoaiKhieuNai child where child.Id = " + ddlParrent.SelectedValue + ")", "");
            if (listLoaiKhieuNaiTemp != null && listLoaiKhieuNaiTemp.Count > 0)
            {
                lblParentLoaiKhieuNaiId.Text = listLoaiKhieuNaiTemp[0].Id.ToString();
            }
        }

        int mod = 0; // Edit
        int.TryParse(Request.QueryString["Id"], out mod);
        if (mod == 0) // Chế độ thêm mới
        {
            bool isOk = false;
            int parentId = Convert.ToInt32(ddlParrent.SelectedValue);
            if (parentId > 0)
            {
                LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();
                LoaiKhieuNaiInfo info = ctl.GetInfo(parentId);
                string[] list = "3572, 3387, 3486".Split(',');
                if (list.Count(v => v.Trim() != info.Id.ToString()) > 0) // Sửa hiện tại không được trong danh sách                    
                    if (list.Count(v => v.Trim() == info.ParentLoaiKhieuNaiId.ToString()) > 0 || list.Count(v => v.Trim() == info.Id.ToString()) > 0)
                    {
                        isOk = true;
                        RenderServiceCode();
                        ddlLoaiKhieuNaiNhom.SelectedValue = 22.ToString();
                    }

            }
            if (!isOk)
            {
                txtServiceCode.Text = string.Empty;
                ddlLoaiKhieuNaiNhom.SelectedValue = 0.ToString();
            }
        }
    }
    protected void ChangeSort(int id, int cap, int sortNew)
    {
        if (!txtSort.Attributes["valueOld"].Equals(sortNew.ToString()))
        {
            IEnumerable<LoaiKhieuNaiInfo> lst = LoaiKhieuNaiImpl.ListLoaiKhieuNai.Where(t => t.ParentId == id);
            if (lst.Any())
            {
                if (cap == 1)
                {
                    foreach (var item in lst)
                    {
                        sortNew = sortNew + 1000;
                        ServiceFactory.GetInstanceLoaiKhieuNai().UpdateDynamic("Cap=2,Sort=" + sortNew, "Id=" + item.Id);

                        var lstChild = LoaiKhieuNaiImpl.ListLoaiKhieuNai.Where(t => t.ParentId == item.Id);
                        if (lstChild.Any())
                        {
                            var sortChild = sortNew;
                            foreach (var child in lstChild)
                            {
                                sortChild = sortChild + 1;
                                ServiceFactory.GetInstanceLoaiKhieuNai().UpdateDynamic("Cap=3,Sort=" + sortChild, "Id=" + child.Id);
                            }
                        }
                    }
                }
                else if (cap == 2)
                {
                    foreach (var item in lst)
                    {
                        sortNew = sortNew + 1;
                        ServiceFactory.GetInstanceLoaiKhieuNai().UpdateDynamic("Cap=3,Sort=" + sortNew, "Id=" + item.Id);
                    }
                }
            }
        }
    }
}

