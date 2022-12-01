using System;
using System.Data;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.Admin;
using Website.AppCode;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using System.Web.UI;

public partial class admin_PhanQuyenNhomNguoiDung_AIKhieuNai : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {

        lbMess.Text = "";

        if (!IsPostBack)
        {
            BindDropDownList();
            BindGrid();
        }
    }

    private void BindDropDownList()
    {

        try
        {
            ddlPermissionDefault.Items.Add(new ListItem("[ Chọn nhóm quyền ]", "0"));

            foreach (var i in Enum.GetValues(typeof(PermissionDefaultType)))
            {
                ddlPermissionDefault.Items.Add(new ListItem(Enum.GetName(typeof(PermissionDefaultType), i).Replace("_", " "), i.GetHashCode().ToString()));
                //strValues += "<option value=\"" + i.ToString() + "\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ") + "</option>";
            }

            var dt = ServiceFactory.GetInstanceNhomNguoiDung_AI().GetListDynamic("Id,Name","","");

            dt.Insert(0, new NhomNguoiDung_AIInfo() { Id = 0, Name = "[ Chọn Phòng Ban ]" });

            ddlUser.DataSource = dt;
            ddlUser.DataValueField = "Id";
            ddlUser.DataTextField = "Name";
            ddlUser.DataBind();

            if (Request.QueryString["NhomNguoiDungAIId"] != null && Request.QueryString["NhomNguoiDungAIId"] != string.Empty)
            {
                ddlUser.SelectedValue = Request.QueryString["NhomNguoiDungAIId"];
            }
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void BindGrid()
    {
        try
        {
            string selectClause = "isnull(a.Id,0) Id, b.Id PermissionId, b.Name, b.MoTa,ISNULL(a.PermissionSchemeId,0) PermissionSchemeId, isnull(a.NhomNguoiDung_AIId,0) NhomNguoiDung_AIId, isnull(a.IsAllow ,0) IsAllow";

            string joinClause = "right join PermissionSchemes b on a.PermissionSchemeId = b.Id and a.NhomNguoiDung_AIId = " + ddlUser.SelectedValue;

            var _dtRight = ServiceFactory.GetInstanceNhomNguoiDung_AI_Permission().GetListDynamicJoin(selectClause, joinClause, "", "b.Sort");

            grvView.DataSource = _dtRight;
            grvView.DataBind();

            ddlPermissionDefault.SelectedIndex = 0;

            if (IsPostBack)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch(Exception ex)
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        var x = e.Row.DataItemIndex;

        if (e.Row.DataItemIndex != -1)
        {


            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }

    }

    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void ddlMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
 
    protected void ddlPermissionDefault_SelectedIndexChanged(object sender, EventArgs e)
    {
        PermissionDefault();
    }

    private void PermissionDefault()
    {
        try
        {
            string selectClause = "isnull(a.Id,0) Id, b.Id PermissionId, b.Name, b.MoTa,ISNULL(a.PermissionSchemeId,0) PermissionSchemeId, isnull(a.DefaultPermissionId,0) DefaultPermissionId, isnull(a.IsAllow ,0) IsAllow";

            string joinClause = "right join PermissionSchemes b on a.PermissionSchemeId = b.Id and a.DefaultPermissionId = " + ddlPermissionDefault.SelectedValue;

            var _dtRight = ServiceFactory.GetInstancePermissionDefault().GetListDynamicJoin(selectClause, joinClause, "", "b.Sort");

            grvView.DataSource = _dtRight;
            grvView.DataBind();

            //SetUserRight();

            if (IsPostBack)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void linkbtnPhanQuyen_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        if (ddlUser.Text.Equals("0"))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa chọn phòng ban để phân quyền.','error');", true);
            return;
        }
        try
        {

            int i = 0;
            var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI_Permission();

            foreach (GridViewRow row in grvView.Rows)
            {
                var chkRead = (CheckBox)row.FindControl("chkRead");


                var menuId = int.Parse(grvView.DataKeys[row.RowIndex].Value.ToString());

                //Check nếu chọn menu2 thì không cho update cha.
                NhomNguoiDung_AI_PermissionInfo item = new NhomNguoiDung_AI_PermissionInfo();

                item.IsAllow = true;

                item.NhomNguoiDung_AIId = int.Parse(ddlUser.SelectedValue);
                item.PermissionSchemeId = menuId;
                item.IsAllow = chkRead.Checked;

                var checkExists = obj.GetListByAllRef(item.NhomNguoiDung_AIId, item.PermissionSchemeId);
                if (checkExists != null && checkExists.Count > 0)
                {
                    item.Id = checkExists[0].Id;
                    obj.Update(item);
                }
                else
                {
                    obj.Add(item);
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phân quyền cho phòng ban thành công.','info');", true);
            }
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }

        BindGrid();
    }
}
