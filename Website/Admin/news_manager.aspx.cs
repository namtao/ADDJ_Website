using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;


using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Web;
using AIVietNam.News.Entity;

public partial class admin_news_manager : Page
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
            BindDropdownList();

            grvView.PageSize = Config.RecordPerPage;
            BindGrid();
        }
    }

    private void BindDropdownList()
    {
        var lstCatNews = ServiceFactory.GetInstanceCategory().GetListCategorySortParent();
        lstCatNews.Insert(0, new CategoryInfo() { Id = 0, Name = "[ Tất cả ]" });
        ddlCategoryNews.DataTextField = "Name";
        ddlCategoryNews.DataValueField = "Id";
        ddlCategoryNews.DataSource = lstCatNews;
        ddlCategoryNews.DataBind();
    }

    private void BindGrid()
    {
        try
        {
            string whereClause = string.Empty;
            int catNews = Convert.ToInt32(ddlCategoryNews.SelectedValue);
            string title = txtTitle.Text.Trim();

            if (!string.IsNullOrEmpty(title))
            {
                if (whereClause.Length == 0)
                    whereClause += " Title like N'%" + title + "%'";
                else
                    whereClause += " and Title like N'%" + title + "%'";
            }


            var newsObj = ServiceFactory.GetInstanceNews().GetListDynamicJoin("a.*,b.Name CategoryName", "left join Category b on a.CategoryId=b.id", whereClause, "Id desc");
            if (newsObj != null && newsObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + newsObj.Count + " bản tin được tìm thấy.</font>";
                grvView.DataSource = newsObj;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có bản tin nào được tìm thấy.</font>";
                grvView.DataSource = null;
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
            e.Row.Cells[2].Text = "<a href='news_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[2].Text + "</a>";
            e.Row.Cells[3].Text = "<a href='http://localhost:50954/Upload/News/" + e.Row.Cells[3].Text + "' target='_blank'>" + e.Row.Cells[3].Text + "</a>";
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



    protected void btCapNhat_Click(object sender, EventArgs e)
    {
        BindGrid();
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
            var obj = ServiceFactory.GetInstanceNews();
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
        Response.Redirect("news_add.aspx", false);
    }
}

