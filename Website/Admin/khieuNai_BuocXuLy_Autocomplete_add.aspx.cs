using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_khieuNai_BuocXuLy_Autocomplete_add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = "";
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }
    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete();
            KhieuNai_BuocXuLy_AutocompleteInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData khieuNai_BuocXuLy_Autocomplete_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                //txtPhongBanId.Text = item.PhongBanId.ToString();
                txtName.Text = item.Name.ToString();
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
 

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!this.Permission.UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    KhieuNai_BuocXuLy_AutocompleteInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function khieuNai_BuocXuLy_Autocomplete_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        item.PhongBanId = UserInfo.PhongBanId;
                        item.Name = txtName.Text.Trim();
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    obj.Update(item);
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
                var item = new KhieuNai_BuocXuLy_AutocompleteInfo();

                try
                {
                    item.PhongBanId = UserInfo.PhongBanId;
                    item.Name = txtName.Text.Trim();
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }

                obj.Add(item);
            }

            Response.Redirect("khieuNai_BuocXuLy_Autocomplete_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

