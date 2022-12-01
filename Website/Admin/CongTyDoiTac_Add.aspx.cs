using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_congTyDoiTac_add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                BindEdit();
            }
        }
    }
    private void BindEdit()
    {
        try
        {
            CongTyDoiTacImpl ctl = ServiceFactory.GetInstanceCongTyDoiTac();
            CongTyDoiTacInfo info = ctl.GetInfo(int.Parse(Request.QueryString["Id"]));
            if (info == null)
            {
                Utility.LogEvent("Function EditData congTyDoiTac_add get NullId " + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false); return;

            }
            else
            {
                txtTen.Text = info.Ten.ToString();
                txtDiaChi.Text = info.DiaChi.ToString();
                txtDienThoai_Fax.Text = info.DienThoai_Fax.ToString();
                txtWebsite.Text = info.Website.ToString();
                txtHoTroKhachHang.Text = info.HoTroKhachHang.ToString();

                chkTrangThai.Checked = info.TrangThai > 0;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false); return;
        }
    }

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false); return;
        }

        try
        {
            CongTyDoiTacImpl ctl = ServiceFactory.GetInstanceCongTyDoiTac();
            if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != string.Empty)
            {
                try
                {
                    int editId = int.Parse(Request.QueryString["Id"]);
                    CongTyDoiTacInfo info = ctl.GetInfo(editId);

                    if (info == null)
                    {
                        Utility.LogEvent("Function congTyDoiTac_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false); return;
                    }

                    List<CongTyDoiTacInfo> objs = ctl.GetListDynamic("Id", string.Format("Ten = N'{0}' AND Id <> {1}", txtTen.Text.Trim(), editId), string.Empty);
                    if (objs != null && objs.Count > 0)
                    {
                        lblMsg.Text = "Tên công ty đã tồn tại, vui lòng xem lại";
                        return;
                    }


                    info.Ten = txtTen.Text.Trim();
                    info.DiaChi = txtDiaChi.Text.Trim();
                    info.DienThoai_Fax = txtDienThoai_Fax.Text.Trim();
                    info.Website = txtWebsite.Text.Trim();
                    info.HoTroKhachHang = txtHoTroKhachHang.Text.Trim();
                    info.TrangThai = chkTrangThai.Checked ? 1 : 0;
                    info.CUser = UserInfo.Username;
                    info.LUser = UserInfo.Username;

                    ctl.Update(info);
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Response.Redirect(Config.PathError, false); return;
                }
            }
            else
            {
                CongTyDoiTacInfo info = new CongTyDoiTacInfo();
                List<CongTyDoiTacInfo> objs = ctl.GetListDynamic("Id", string.Format("Ten = N'{0}' AND Id <> {1}", txtTen.Text.Trim(), 0), string.Empty);
                if (objs != null && objs.Count > 0)
                {
                    lblMsg.Text = "Tên công ty đã tồn tại, vui lòng xem lại";
                    return;
                }
                try
                {
                    info.Ten = txtTen.Text.Trim();
                    info.DiaChi = txtDiaChi.Text.Trim();
                    info.DienThoai_Fax = txtDienThoai_Fax.Text.Trim();
                    info.Website = txtWebsite.Text.Trim();
                    info.HoTroKhachHang = txtHoTroKhachHang.Text.Trim();
                    info.TrangThai = chkTrangThai.Checked ? 1 : 0;
                    info.CUser = UserInfo.Username;
                    info.LUser = UserInfo.Username;
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ"; return;
                }

                ctl.Add(info);
            }

            Response.Redirect("CongTyDoiTac_Manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false); return;
        }
    }
}

