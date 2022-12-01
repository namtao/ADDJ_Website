using System;
using System.Data;
using System.Web.UI.WebControls;
using ADDJ.Core;


using ADDJ.Admin;
using Website.AppCode;
using ADDJ.Core.Provider;

public partial class admin_PhanQuyen : PageBase
{
    private DataTable _dtRight;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        lbMess.Text = "";

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
            var dt = ServiceFactory.GetInstanceNguoiSuDung().GetList();

            dt.Insert(0, new ADDJ.Entity.NguoiSuDungInfo() { Id = 0, TenTruyCap = "[ Chọn Tên đăng nhập ]" });

            ddlUser.DataSource = dt;
            ddlUser.DataValueField = "ID";
            ddlUser.DataTextField = "TenTruyCap";
            ddlUser.DataBind();

            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                ddlUser.SelectedValue = Request.QueryString["ID"];
            }

            ddlMenu.Items.Add("Menu cấp 1");
            ddlMenu.Items.Add("Menu cấp 2");
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void BindGrid(int type)
    {
        try
        {
            UserRightImpl obj = new UserRightImpl();
            var adminID = int.Parse(ddlUser.Text.ToString());
            _dtRight = obj.GetQuyenByAdminID(adminID, type);

            grvView.DataSource = _dtRight;
            grvView.DataBind();

            SetUserRight();

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch(Exception ex)
        {
            Utility.LogEvent(ex);
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

    private void SetUserRight()
    {
        int menuIndex = ddlMenu.SelectedIndex;
        for (var i = 0; i < _dtRight.Rows.Count; i++)
        {
            
            var row = grvView.Rows[i];

            var chkRead = (CheckBox)row.FindControl("chkRead");
            var chkEdit = (CheckBox)row.FindControl("chkEdit");
            var chkDelete = (CheckBox)row.FindControl("chkDelete");
            var chkO1 = (CheckBox)row.FindControl("chkOther1");
            var chkO2 = (CheckBox)row.FindControl("chkOther2");

            if (menuIndex == 1)
            {
                if (_dtRight.Rows[i]["ParentId"].ToString().Equals("0"))
                {
                    chkRead.Enabled = false;
                    chkEdit.Visible = false;
                    chkDelete.Visible = false;
                    chkO1.Visible = false;
                    chkO2.Visible = false;
                    grvView.Rows[i].Cells[1].Text = "<b>" + grvView.Rows[i].Cells[1].Text + "</b>";
                }
                else
                {
                    grvView.Rows[i].Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + grvView.Rows[i].Cells[1].Text + "";
                }
            }
            else
            {
                chkEdit.Visible = false;
                chkDelete.Visible = false;
                chkO1.Visible = false;
                chkO2.Visible = false;
            }
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
        BindGrid(ddlMenu.SelectedIndex);
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid(ddlMenu.SelectedIndex);
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
            lbMess.Text = "<p><font color='red'>Bạn phải chọn username phân quyền trước.</font></p>";
            return;
        }
        try
        {
            int menuIndex = ddlMenu.SelectedIndex;
            int i = 0;
            UserRightImpl obj = new UserRightImpl();
            foreach (GridViewRow row in grvView.Rows)
            {
                var chkRead = (CheckBox)row.FindControl("chkRead");
                var chkEdit = (CheckBox)row.FindControl("chkEdit");
                var chkDelete = (CheckBox)row.FindControl("chkDelete");
                var chkO1 = (CheckBox)row.FindControl("chkOther1");
                var chkO2 = (CheckBox)row.FindControl("chkOther2");

                if (chkRead.Enabled)
                {
                    var menuId = int.Parse(grvView.DataKeys[row.RowIndex].Value.ToString());

                    //Check nếu chọn menu2 thì không cho update cha.
                    var menuItem = ServiceFactory.GetInstanceMenu().GetInfo(menuId);
                    if (menuItem.ParentID == 0 && menuIndex == 1)
                        continue;

                    UserRightInfo item = new UserRightInfo();

                    item.MenuID = menuId;
                    var adminID = int.Parse(ddlUser.SelectedValue.ToString());
                    item.AdminID = adminID;

                    item.UserRead = chkRead.Checked;
                    if (menuIndex == 1)
                    {
                        item.UserEdit = chkEdit.Checked;
                        item.UserDelete = chkDelete.Checked;
                        item.Other1 = chkO1.Checked;
                        item.Other2 = chkO2.Checked;
                    }
                    else
                    {
                        item.UserEdit = true;
                        item.UserDelete = true;
                        item.Other1 = chkO1.Checked;
                        item.Other2 = chkO2.Checked;
                    }
                    UserRightInfo item2 = obj.GetRightByMenuAndAdmin(item.MenuID, item.AdminID);
                    if (item2 != null)
                    {
                        item.ID = item2.ID;
                        obj.Update(item);
                    }
                    else
                    {
                        obj.Add(item);
                    }
                }

            }

            lbMess.Text = "<p><font color='red'>Phân quyền thành công</font></p>";
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}
