using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class admin_LoaiPhongBan2PhongBan : Website.AppCode.PageBase
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
        var lstFilter = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        lstFilter.Insert(0, new LoaiPhongBanInfo() { Id = 0, Name = "[ Tất cả ]" });

        ddlLoaiKhieuNai.DataSource = lstFilter;
        ddlLoaiKhieuNai.DataTextField = "Name";
        ddlLoaiKhieuNai.DataValueField = "Id";
        ddlLoaiKhieuNai.DataBind();


        if (Request.QueryString["LoaiPhongBanId"] != null && Request.QueryString["LoaiPhongBanId"] != string.Empty)
        {
            ddlLoaiKhieuNai.SelectedValue = Request.QueryString["LoaiPhongBanId"];
        }
    }

    private void CallJavaScriptInUpdatePanel()
    {
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadAutoComplete", "AutocompleteTenDangNhap();", true);
    }

    private void BindGrid()
    {
        string selectClause = "";
        string whereClause = "LoaiPhongBanId = " + ddlLoaiKhieuNai.SelectedValue;
        var lstPhongBan2PhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic(selectClause, whereClause, "");

        grvView.DataSource = lstPhongBan2PhongBan;
        grvView.DataBind();

        if (IsPostBack)
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
        var obj = ServiceFactory.GetInstancePhongBan();

        List<int> lst = new List<int>();
        foreach (GridViewRow row in grvView.Rows)
        {
            var status = (CheckBox)row.FindControl("cbSelectAll");
            if (status.Checked)
            {
                int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                obj.UpdateDynamic("LoaiPhongBanId=0", "Id=" + ID);
            }
            i++;
        }
        
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa Phòng ban chuyển xử lý thành công','info');", true);
        BindGrid();
    }

    protected void btAddPhong_Click(object sender, EventArgs e)
    {
        try
        {
            var loaiPhongBanId = int.Parse(ddlLoaiKhieuNai.SelectedValue);
            var obj = ServiceFactory.GetInstancePhongBan();

            var lstLoaiPhongBan2PhongBan = obj.GetListDynamic("Id,LoaiPhongBanId", string.Format("Name=N'{0}'", txtTenDangNhap.Text.Trim()), "");


            if (lstLoaiPhongBan2PhongBan != null && lstLoaiPhongBan2PhongBan.Count == 1)
            {
                if (lstLoaiPhongBan2PhongBan[0].LoaiPhongBanId == loaiPhongBanId)
                {
                    CallJavaScriptInUpdatePanel();
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phòng ban này đã tồn tại trong loại phòng ban.','error');", true);
                    return;
                }
                else
                {
                    obj.UpdateDynamic("LoaiPhongBanId=" + loaiPhongBanId, " Id=" + lstLoaiPhongBan2PhongBan[0].Id);
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm phòng ban chuyển xử lý thành công','info');", true);
                }
            }
            else
            {
                CallJavaScriptInUpdatePanel();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Tên phòng ban không tồn tại','error');", true);
                return;
            }
           
            
            
            BindGrid();

            txtTenDangNhap.Text = "";
            txtListTenDangNhap.Text = "";
        }
        catch (Exception ex)
        {
            CallJavaScriptInUpdatePanel();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "essageAlert.AlertNormal('Thêm Phòng ban chuyển xử lý có lỗi','error');", true);
        }
    }

}

