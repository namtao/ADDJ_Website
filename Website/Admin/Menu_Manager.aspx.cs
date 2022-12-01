using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADDJ.Core;
using Website.AppCode;
using System.Linq;
using System.Collections.Generic;
using ADDJ.Admin;
using System.Data.SqlClient;

public partial class admin_menu_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ddlParentId.AutoPostBack = true;
        this.ddlParentId.SelectedIndexChanged += DdlParentId_SelectedIndexChanged;
        this.ddlLevel.AutoPostBack = true;
        this.ddlLevel.SelectedIndexChanged += DdlParentId_SelectedIndexChanged;
        if (!IsPostBack)
        {
            grvView.PageSize = 100;
            BindRoot(Request.QueryString["ParentId"]);
            ddlLevel.SelectedValue = Request.QueryString["Level"];
            BindGrid();
        }
    }

    private void DdlParentId_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect($"Menu_Manager.aspx?ParentId={this.ddlParentId.SelectedValue}&Level={this.ddlLevel.SelectedValue}");
    }

    protected string GetMenuType(object obj)
    {
        if (obj.ToString().Equals("1"))
            return "Menu";
        return "Separator";
    }

    private void BindGrid()
    {
        try
        {
            int num = 0;
            int num2 = Convert.ToInt32(this.ddlLevel.SelectedValue);
            string parentId = this.ddlParentId.SelectedValue;
            List<MenuInfo> list = ServiceFactory.GetInstanceMenu().GetList();
            if ((list != null) && (list.Count > 0))
            {
                List<MenuInfo> list2 = new List<MenuInfo>();
                if ((num2 == 0) || (num2 >= 1))
                {
                    IOrderedEnumerable<MenuInfo> enumerable = from t in list
                                                              where t.ParentID.ToString() == parentId
                                                              orderby t.STT
                                                              select t;
                    using (IEnumerator<MenuInfo> enumerator = enumerable.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            MenuInfo menuInfo = enumerator.Current;
                            list2.Add(menuInfo);
                            num++;
                            if ((num2 == 0) || (num2 >= 2))
                            {
                                IOrderedEnumerable<MenuInfo> enumerable2 = from t in list
                                                                           where t.ParentID.ToString() == menuInfo.ID.ToString()
                                                                           orderby t.STT
                                                                           select t;
                                if (enumerable2 != null)
                                {
                                    foreach (MenuInfo info in enumerable2)
                                    {
                                        info.Name = ("|— " + info.Name);
                                        list2.Add(info);
                                        num++;
                                    }
                                }
                            }
                        }
                    }
                }
                this.grvView.DataSource = list2;
                this.grvView.DataBind();
                this.ltThongBao.Text = string.Format("Tổng số lượng menu tìm thấy: <span style=\"color: red; font - weight: bold;\">{0}</span>", num);
            }
        }
        catch (Exception exception)
        {
            Helper.GhiLogs(exception);
            this.ltThongBao.Text = string.Format("Có lỗi xảy ra, vui lòng thử lại");
        }
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        LinkButton button = (LinkButton)sender;
        string[] separator = new string[] { ";" };
        string[] strArray = button.CommandArgument.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        int menuId = Convert.ToInt32(strArray[0]);
        switch (strArray[1].ToString())
        {
            case "Up":
                {
                    SqlParameter[] prms = new SqlParameter[] {
                        new SqlParameter("@Id", menuId)
                    };
                    SqlHelper.ExecuteNonQuery(Config.ConnectionString, "Menu_SapXepLen", prms);
                    this.BindGrid();
                    break;
                }
            case "Down":
                {
                    SqlParameter[] prms = new SqlParameter[] {
                        new SqlParameter("@Id", menuId)
                    };
                    SqlHelper.ExecuteNonQuery(Config.ConnectionString, "Menu_SapXepXuong", prms);
                    this.BindGrid();
                    break;
                }
        }
    }
    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
            if (e.Row.Cells[1].Text.StartsWith("_"))
                e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"Menu_Add.aspx?Id=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text.Replace("_", "") + "</a>";
            else
                e.Row.Cells[1].Text = "<a href=\"Menu_Add.aspx?Id=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text + "</a>";
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

    private void BindRoot(string selected)
    {
        this.ddlParentId.Items.Clear();
        string commandText = string.Format("SELECT Id, Name FROM Menu a WHERE a.ParentId = 0 OR a.ParentId  IS NULL");
        DataTable table = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, commandText).Tables[0];
        this.ddlParentId.DataSource = table;
        this.ddlParentId.DataTextField = "Name";
        this.ddlParentId.DataValueField = "Id";
        this.ddlParentId.DataBind();
        this.ddlParentId.Items.Insert(0, new ListItem("-- Chọn tất --", "0"));
        this.ddlParentId.SelectedValue = selected;
    }
 
   
    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu_Add.aspx", false);
    }

    protected void linkbtnUpdate_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceMenu();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("Display");
                if (status.Checked)
                {
                    int menuId = int.Parse(grvView.DataKeys[i].Value.ToString());
                    obj.UpdateDisplay(menuId, 1);
                }
                else
                {
                    int menuId = int.Parse(grvView.DataKeys[i].Value.ToString());
                    obj.UpdateDisplay(menuId, 0);
                }
                i++;
            }

            MenuImpl.ListMenu = ServiceFactory.GetInstanceMenu().GetList();
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }

        //Refresh Cache
        MenuImpl.ListMenu = ServiceFactory.GetInstanceMenu().GetList();

        BindGrid();
    }

    protected void linkbtnDelete_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        try
        {
            int i = 0;
            MenuImpl obj = ServiceFactory.GetInstanceMenu();
            foreach (GridViewRow row in grvView.Rows)
            {
                CheckBox status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int menuId = int.Parse(grvView.DataKeys[i].Value.ToString());
                    obj.Delete(menuId);
                }

                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            Helper.GhiLogs(se);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        // Refresh Cache
        MenuImpl.ListMenu = ServiceFactory.GetInstanceMenu().GetList();
        BindGrid();
    }
}

