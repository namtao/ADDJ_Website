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

public partial class admin_LoaiKhieuNai2PhongBan : Website.AppCode.PageBase
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
        var lstFilter = ServiceFactory.GetInstanceLoaiKhieuNai().GetListLoaiKhieuNaiSortParent();
        lstFilter.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "[ Tất cả ]" });

        ddlLoaiKhieuNai.DataSource = lstFilter;
        ddlLoaiKhieuNai.DataTextField = "Name";
        ddlLoaiKhieuNai.DataValueField = "Id";
        ddlLoaiKhieuNai.DataBind();


        if (Request.QueryString["LoaiKhieuNaiId"] != null && Request.QueryString["LoaiKhieuNaiId"] != string.Empty)
        {
            ddlLoaiKhieuNai.SelectedValue = Request.QueryString["LoaiKhieuNaiId"];
        }
    }

    private void CallJavaScriptInUpdatePanel()
    {
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadAutoComplete", "AutocompleteTenDangNhap();", true);
    }

    private void BindGrid()
    {
        string selectClause = "a.Id, b.Name PhongBanName";
        string joinClause = "left join PhongBan b on a.PhongBanId = b.Id";
        string whereClause = "a.LoaiKhieuNaiId = " + ddlLoaiKhieuNai.SelectedValue;
        var lstPhongBan2PhongBan = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListDynamicJoin(selectClause, joinClause, whereClause, "");

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
        var obj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan();

        List<int> lst = new List<int>();
        foreach (GridViewRow row in grvView.Rows)
        {
            var status = (CheckBox)row.FindControl("cbSelectAll");
            if (status.Checked)
            {
                int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                obj.Delete(ID);
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
            var i = 0;
            var loaiKNId = int.Parse(ddlLoaiKhieuNai.SelectedValue);
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan();

            var lstLoaiKhieuNai2PhongBan = obj.GetListByLoaiKhieuNaiId(loaiKNId);

            var phongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id", string.Format("Name=N'{0}'", txtTenDangNhap.Text.Trim()), "");


            if (phongBan != null && phongBan.Count == 1)
            {
                if(!lstLoaiKhieuNai2PhongBan.Where(t=>t.PhongBanId== phongBan[0].Id).Any())
                    obj.Add(new LoaiKhieuNai2PhongBanInfo() { LoaiKhieuNaiId = loaiKNId, PhongBanId = phongBan[0].Id });
            }
            else
            {
                CallJavaScriptInUpdatePanel();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Tên phòng ban không tồn tại','error');", true);
                return;
            }

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban chuyển xử lý thành công','info');", true);
            
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

