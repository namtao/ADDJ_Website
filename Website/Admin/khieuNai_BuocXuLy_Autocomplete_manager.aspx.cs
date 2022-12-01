using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_khieuNai_BuocXuLy_Autocomplete_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grvView.PageSize = Config.RecordPerPage;
            BindGrid();
        }
    }
    private void BindGrid()
    {
        try
        {
            var khieuNai_BuocXuLy_AutocompleteObj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete().GetListDynamic("","PhongBanId=" + UserInfo.PhongBanId,"");
            if (khieuNai_BuocXuLy_AutocompleteObj != null && khieuNai_BuocXuLy_AutocompleteObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + khieuNai_BuocXuLy_AutocompleteObj.Count + " KhieuNai_BuocXuLy_Autocomplete được tìm thấy.</font>";
                grvView.DataSource = khieuNai_BuocXuLy_AutocompleteObj;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có Log được tìm thấy.</font>";
                grvView.DataSource = null;
                grvView.DataBind();
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
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
            e.Row.Cells[1].Text = "<a href='khieuNai_BuocXuLy_Autocomplete_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
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
 

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("khieuNai_BuocXuLy_Autocomplete_add.aspx", false);
    }

    protected void linkbtnDelete_Click(object sender, EventArgs e)
    {
        if (!this.Permission.UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete();
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
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid();
    }
}

