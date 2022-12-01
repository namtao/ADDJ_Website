using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_dauSoCP_add : Website.AppCode.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            lblMsg.Text = "";
            BindDropDownList();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }
    private void BindDropDownList()
    {
        try
        {
            var congTyDoiTacIdObj = ServiceFactory.GetInstanceCongTyDoiTac().GetList();
            if (congTyDoiTacIdObj != null && congTyDoiTacIdObj.Count > 0)
            {
                ddlCongTyDoiTacId.DataSource = congTyDoiTacIdObj;
                ddlCongTyDoiTacId.DataTextField = "Ten";
                ddlCongTyDoiTacId.DataValueField = "Id";
                ddlCongTyDoiTacId.DataBind();
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceDauSoCP();
            DauSoCPInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData dauSoCP_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                ddlCongTyDoiTacId.SelectedValue = item.CongTyDoiTacId.ToString();
                txtDauSo.Text = item.DauSo.ToString();
                txtLoaiDauSo.Text = item.LoaiDauSo.ToString();
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
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceDauSoCP();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    DauSoCPInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function dauSoCP_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    item.CongTyDoiTacId = Convert.ToInt32(ddlCongTyDoiTacId.SelectedValue);
                    try
                    {
                        item.DauSo = txtDauSo.Text.Trim();
                        item.LoaiDauSo = txtLoaiDauSo.Text.Trim();
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
                var item = new DauSoCPInfo();

                item.CongTyDoiTacId = Convert.ToInt32(ddlCongTyDoiTacId.SelectedValue);
                try
                {
                    item.DauSo = txtDauSo.Text.Trim();
                    item.LoaiDauSo = txtLoaiDauSo.Text.Trim();
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }

                obj.Add(item);
            }

            Response.Redirect("dauSoCP_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

