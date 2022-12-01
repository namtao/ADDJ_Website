using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Collections.Generic;
using AIVietNam.News.Entity;

public partial class admin_category_manager : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginAdmin.IsLoginAdmin();

        if (!UserRightImpl.CheckRightAdminnistrator().UserRead)
        {
            Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
            return;
        }

        if (!IsPostBack)
        {
            grvView.PageSize = Config.RecordPerPage;
            BindGrid();
        }
    }

    List<CategoryInfo> categoryObj;
    private void BindGrid()
    {
        try
        {
            categoryObj = ServiceFactory.GetInstanceCategory().GetListCategorySortParent("@");
            if (categoryObj != null && categoryObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + categoryObj.Count + " Category được tìm thấy.</font>";
                grvView.DataSource = categoryObj;
                grvView.DataBind();
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }


    private void RowDataBound(GridViewRowEventArgs e)
    {
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
            
            if(e.Row.Cells[1].Text.IndexOf("@") != -1)
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, e.Row.Cells[1].Text.LastIndexOf("@") + 1).Replace("@", "&nbsp;&nbsp;&nbsp;") + "<a href='category_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text.Replace("@", "") + "</a>";
            else
                e.Row.Cells[1].Text =  "<a href='category_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
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
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator().UserDelete)
        {
            Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
            return;
        }
        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceCategory();
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
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
        BindGrid();
    }


    protected void btThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("category_add.aspx", false);
    }
}

