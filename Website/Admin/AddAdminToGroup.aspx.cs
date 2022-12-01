using System;
using System.Data;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.Admin;

public partial class admin_AddAdminToGroup : System.Web.UI.Page
{
    private int _adminID;
    private DataTable _dtRight;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoginAdmin.IsLoginAdmin();

        if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
        {
            Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
            return;
        }

        lbMess.Text = "";
        if (Page.Request["ID"] != null)
        {
            _adminID = int.Parse(Page.Request["ID"]);
        }
       
        if (!IsPostBack)
        {            
            BindDropDownList();
            BindGrid(0);
        }
    }

    private void BindDropDownList()
    {
        try
        {
            GroupAdminImpl obj = new GroupAdminImpl();

            var lst = obj.GetList();

            lst.Insert(0, new GroupAdminInfo() { Id = 0, Name = "[ Chọn Nhóm ]" });

            ddlUser.DataSource = lst;
            ddlUser.DataValueField = "Id";
            ddlUser.DataTextField = "Name";
            ddlUser.DataBind();
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    private void BindGrid(int type)
    {
        try
        {
            if (type == 0)
            {
                lbMess.Text = "Chọn nhóm cần thêm quản trị viên<br />";
                return;
            }
            GroupAdminDetailImpl obj = new GroupAdminDetailImpl();

            _dtRight = obj.GetAdminByGroup(type);

            grvView.DataSource = _dtRight;
            grvView.DataBind();

            SetUserRight();
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        var x = e.Row.DataItemIndex;

        if (e.Row.DataItemIndex != -1)
        {
            if (e.Row.RowIndex % 2 != 0)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#DEEAF3'");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
            }

            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }

    }

    private void SetUserRight()
    {
        for (var i = 0; i < _dtRight.Rows.Count; i++)
        {
            var row = grvView.Rows[i];

            var chkRead = (CheckBox)row.FindControl("chkRead");

            if ((Convert.ToInt32(_dtRight.Rows[i]["Active"])>0))
                chkRead.Checked = true;
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGrid(Convert.ToInt32(ddlUser.SelectedValue));
    }


    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch
        {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {   
        BindGrid(Convert.ToInt32(ddlUser.SelectedValue));
    }

    protected void btPhanQuyen_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
        {         
            Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
            return;
        }
        if (ddlUser.Text.Equals("0"))
        {
            lbMess.Text = "<p><font color='red'>Bạn phải chọn nhóm phân quyền trước.</font></p>";
            return;
        }
        try
        {
            int groupId = Convert.ToInt32(ddlUser.SelectedValue);
            GroupAdminDetailImpl obj = new GroupAdminDetailImpl();
            foreach (GridViewRow row in grvView.Rows)
            {
                var chkRead = (CheckBox)row.FindControl("chkRead");

                GroupAdminDetailInfo item = new GroupAdminDetailInfo();

                item.AdminId = int.Parse(grvView.DataKeys[row.RowIndex].Value.ToString());

                item.GroupAdminId = groupId;
                item.Active = chkRead.Checked?1:0;

                obj.Update(item);
            }
            lbMess.Text = "Cập nhật thành công<br />";
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }
}
