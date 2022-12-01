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
using System.Web.UI.WebControls;

public partial class admin_nguoiSuDung_add : Website.AppCode.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDownList();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }

    private void BindDropDownList()
    { 
        var admin = LoginAdmin.AdminLogin();
        ddlNhomNguoiDung.DataSource = NguoiSuDung_GroupImpl.NhomNguoiDung;
        ddlNhomNguoiDung.DataValueField = "ID";
        ddlNhomNguoiDung.DataTextField = "Name";
        ddlNhomNguoiDung.DataBind();

        var lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,MaDoiTac,DonViTrucThuoc", "Id = 1 or DonViTrucThuoc = 1", "");

        lstKhuVuc = DoiTacImpl.BuildTree(lstKhuVuc, 0, 0, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

        ddlKhuVuc.DataSource = lstKhuVuc;
        ddlKhuVuc.DataValueField = "ID";
        ddlKhuVuc.DataTextField = "MaDoiTac";
        ddlKhuVuc.DataBind();
    }


    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            NguoiSuDungInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData nguoiSuDung_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                txtTenTruyCap.Text = item.TenTruyCap.ToString();
                txtTenDayDu.Text = item.TenDayDu.ToString();
                txtNgaySinh.Text = item.NgaySinh.ToString("dd/MM/yyyy");
                txtDiaChi.Text = item.DiaChi.ToString();
                txtDiDong.Text = item.DiDong.ToString();
                txtCoDinh.Text = item.CoDinh.ToString();
                txtEmail.Text = item.Email.ToString();
                txtCongTy.Text = item.CongTy.ToString();
                txtDiaChiCongTy.Text = item.DiaChiCongTy.ToString();
                txtFaxCongTy.Text = item.FaxCongTy.ToString();
                txtDienThoaiCongTy.Text = item.DienThoaiCongTy.ToString();

                ddlNhomNguoiDung.SelectedValue = item.NhomNguoiDung.ToString();
                ddlKhuVuc.SelectedValue = item.KhuVucId.ToString();
                BindDoiTac(item.KhuVucId.ToString());
                ddlDoiTac.SelectedValue = item.DoiTacId.ToString();

                if (item.Sex == 1)
                    rdNam.Checked = true;
                else rdNu.Checked = true;

                if (item.TrangThai == 1)
                    rdBinhThuong.Checked = true;
                else rdKhoa.Checked = true;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
 
    protected void ddlKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDoiTac(ddlKhuVuc.SelectedValue);
    }

    private void BindDoiTac(string khuvucId)
    {
        if (ddlNhomNguoiDung.SelectedValue.Equals("1"))
        {
            ddlDoiTac.Items.Clear();
            ddlDoiTac.Items.Add(new ListItem("Vinaphone", "1"));
        }
        else
        {
            var lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id,MaDoiTac,DonViTrucThuoc", string.Format("Id = {0} or DonViTrucThuoc = {0}", khuvucId), "");
            lstKhuVuc = DoiTacImpl.BuildTree(lstKhuVuc, 1, 0, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

            ddlDoiTac.DataSource = lstKhuVuc;
            ddlDoiTac.DataValueField = "ID";
            ddlDoiTac.DataTextField = "MaDoiTac";
            ddlDoiTac.DataBind();
        }
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
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            var url = string.Empty;
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    NguoiSuDungInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function nguoiSuDung_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    var userLogin = LoginAdmin.AdminLogin();
                    if (userLogin.NhomNguoiDung == 2 || userLogin.NhomNguoiDung == 4 || userLogin.NhomNguoiDung == 5)
                        return;

                    if (userLogin.NhomNguoiDung == 3)
                    {
                        if (item.NhomNguoiDung == userLogin.NhomNguoiDung)
                        {
                            url = "nguoiSuDung_manager.aspx";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Bạn không có quyền cập nhật người dùng cùng cấp.','error', '" + url + "');", true);
                            return;
                        }
                        item.KhuVucId = Convert.ToInt32(ddlKhuVuc.SelectedValue);
                        item.DoiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
                        item.TenDoiTac = ddlDoiTac.SelectedItem.Text;
                        item.NhomNguoiDung = Convert.ToInt32(ddlNhomNguoiDung.SelectedValue);
                        if (item.NhomNguoiDung == 4 || item.NhomNguoiDung == 5)
                        {
                            obj.Update(item);
                        }
                    }
                    else
                    {
                        item.KhuVucId = Convert.ToInt32(ddlKhuVuc.SelectedValue);
                        item.DoiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
                        item.TenDoiTac = ddlDoiTac.SelectedItem.Text;
                        item.NhomNguoiDung = Convert.ToInt32(ddlNhomNguoiDung.SelectedValue);
                        obj.Update(item);
                    }


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
            }

            url = "nguoiSuDung_manager.aspx";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Sửa người dùng thành công.','info', '" + url + "');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

