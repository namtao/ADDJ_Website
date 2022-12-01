using System;
using System.Data;
using System.Web.UI.WebControls;
using ADDJ.Core;
using ADDJ.Admin;
using Website.AppCode;
using System.Collections.Generic;
using System.Web.UI;
using ADDJ.Entity;

public partial class PermissionDefault : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
       
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
            ddlUser.Items.Add(new ListItem("[ Chọn nhóm quyền ]", "0"));

            foreach (var i in Enum.GetValues(typeof(PermissionDefaultType)))
            {
                ddlUser.Items.Add(new ListItem(Enum.GetName(typeof(PermissionDefaultType), i).Replace("_", " "), i.GetHashCode().ToString()));
                //strValues += "<option value=\"" + i.ToString() + "\">" + Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ") + "</option>";
            }
        }
        catch(Exception ex)
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void BindGrid()
    {
        try
        {

            string selectClause = "isnull(a.Id,0) Id, b.Id PermissionId, b.Name, b.MoTa,ISNULL(a.PermissionSchemeId,0) PermissionSchemeId, isnull(a.DefaultPermissionId,0) DefaultPermissionId, isnull(a.IsAllow ,0) IsAllow";

            string joinClause = "right join PermissionSchemes b on a.PermissionSchemeId = b.Id and a.DefaultPermissionId = " + ddlUser.SelectedValue;

            var _dtRight = ServiceFactory.GetInstancePermissionDefault().GetListDynamicJoin(selectClause, joinClause, "", "b.Sort");

            grvView.DataSource = _dtRight;
            grvView.DataBind();

            if(IsPostBack)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch
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
 
    protected void linkbtnPhanQuyen_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        if (ddlUser.Text.Equals("0"))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn chưa chọn nhóm quyền để phân quyền.','error');", true);
            return;
        }
        try
        {

            int i = 0;
            var obj = ServiceFactory.GetInstancePermissionDefault();

            foreach (GridViewRow row in grvView.Rows)
            {
                var chkRead = (CheckBox)row.FindControl("chkRead");


                var menuId = int.Parse(grvView.DataKeys[row.RowIndex].Value.ToString());

                //Check nếu chọn menu2 thì không cho update cha.
                PermissionDefaultInfo item = new PermissionDefaultInfo();

                item.IsAllow = true;

                item.DefaultPermissionId = int.Parse(ddlUser.SelectedValue);
                item.PermissionSchemeId = menuId;
                item.IsAllow = chkRead.Checked;

                var checkExists = obj.GetListByAllRef(item.PermissionSchemeId, item.DefaultPermissionId);
                if (checkExists != null && checkExists.Count > 0)
                {
                    item.Id = checkExists[0].Id;
                    obj.Update(item);
                }
                else
                {
                    obj.Add(item);
                }


                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phân quyền cho nhóm quyền thành công.','info');", true);
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
