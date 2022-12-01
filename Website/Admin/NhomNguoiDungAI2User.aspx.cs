using System;
using System.Web.UI;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class admin_NhomNguoiDungAI2User : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        lblMsg.Text = "";

        if (!IsPostBack)
        {
            grvView.PageSize = Config.RecordPerPage;
            BindDropDownList();
            BindGrid();
        }
    }

    private void BindDropDownList()
    {
        var lstPhongBan = ServiceFactory.GetInstanceNhomNguoiDung_AI().GetListDynamic("Id,Name", "", "");

        ddlPhongBan.DataSource = lstPhongBan;
        ddlPhongBan.DataValueField = "ID";
        ddlPhongBan.DataTextField = "Name";
        ddlPhongBan.DataBind();


        if (Request.QueryString["NhomNguoiDungAiId"] != null && Request.QueryString["NhomNguoiDungAiId"] != string.Empty)
        {
            ddlPhongBan.SelectedValue = Request.QueryString["NhomNguoiDungAiId"];
        }
    }

    private void CallJavaScriptInUpdatePanel()
    {
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadAutocomplete", "AutocompleteTenDangNhap();", true);        
    }

    private void BindGrid()
    {
        string selectClause = "b.Id,b.TenTruyCap";
        string whereClause = "";
        if (!ddlPhongBan.SelectedValue.Equals("0"))
            whereClause += "NhomNguoiDung_AIId = " + ddlPhongBan.SelectedValue;

        string joinClause = "LEFT JOIN NguoiSuDung b on a.NguoiSuDungId = b.Id";

        string orderbyClause = "b.TenTruyCap";

        var lstPhongBanUser = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail().GetListDynamicJoin(selectClause, joinClause, whereClause, orderbyClause);
        grvView.DataSource = lstPhongBanUser;
        grvView.DataBind();

        if(IsPostBack)
            CallJavaScriptInUpdatePanel();
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {
            
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    protected void ddlPhongBan_Changed(object sender, EventArgs e)
    {
        BindGrid();   
    }

    protected void btLeaver_Click(object sender, EventArgs e)
    {
        var i = 0;
        var NhomNguoiDungAIId = int.Parse(ddlPhongBan.SelectedValue);
        var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail();

        foreach (GridViewRow row in grvView.Rows)
        {
            var status = (CheckBox)row.FindControl("cbSelectAll");
            if (status.Checked)
            {
                int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                var whereClause = string.Format("NguoiSuDungId={0} AND NhomNguoiDung_AIId={1}", ID, NhomNguoiDungAIId);

                obj.DeleteDynamic(whereClause);
            }
            i++;
        }

        BindGrid();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa Phòng ban cho người dùng thành công','info');", true);
    }

    protected void btAddPhong_Click(object sender, EventArgs e)
    {
        var i = 0;
        var NhomNguoiDungAIId = int.Parse(ddlPhongBan.SelectedValue);
        var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI_Detail();

        var lstUserInPhongBan = obj.GetListDynamic("Id,NguoiSuDungId", "NhomNguoiDung_AIId=" + NhomNguoiDungAIId, "");

        var strListTen = txtListTenDangNhap.Text;

        var arrTen = strListTen.Split(',');
        if (arrTen.Length > 0)
        {
            List<int> lstIdUser = new List<int>();
            foreach (var item in arrTen)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                var idUser = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(item);
                if(idUser == 0)
                {
                    lblMsg.Text = "Username " + item + " không tồn tại trong hệ thống.";
                    CallJavaScriptInUpdatePanel();
                    return;
                }
                var checkExists = lstUserInPhongBan.Where(t => t.NguoiSuDungId == idUser).Any();
                if (!checkExists)
                {
                    if (!lstIdUser.Contains(idUser))
                        lstIdUser.Add(idUser);
                }
            }

            foreach (int idUser in lstIdUser)
            {
                obj.Add(new NhomNguoiDung_AI_DetailInfo() { NguoiSuDungId = idUser, NhomNguoiDung_AIId = NhomNguoiDungAIId });                
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban cho người dùng thành công','info');", true);
        }
        else {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban cho người dùng thành công','error');", true);
        }

        BindGrid();

        txtListTenDangNhap.Text = "";
    }    

    protected void btAdd_Click(object sender, EventArgs e)
    {
        txtListTenDangNhap.Text = txtListTenDangNhap.Text + txtTenDangNhap.Text + ",";
        txtTenDangNhap.Text = string.Empty;

        CallJavaScriptInUpdatePanel();
    }    
    
}

