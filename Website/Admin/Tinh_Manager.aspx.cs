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

public partial class admin_tinh_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDownList();
            grvView.PageSize = Config.RecordPerPage;
            BindGrid(false);

            AdminInfo adminInfo = LoginAdmin.AdminLogin();
            LinkButton1.Visible = adminInfo.NhomNguoiDung == 0 || adminInfo.NhomNguoiDung == 1;
        }
    }

    private void BindDropDownList()
    {
        var lstTinh = ServiceFactory.GetInstanceProvince().ListProvinceOrderByName(2);
        if(lstTinh != null)
        {
            string space = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            for(int i=0;i<lstTinh.Count;i++)
            {
                if(lstTinh[i].LevelNbr == 2)
                {
                    lstTinh[i].Name = Server.HtmlDecode(space + lstTinh[i].Name);
                }
            }
        }

        lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Tỉnh/Thành Phố--" });
        ddlTinh.DataSource = lstTinh;
        ddlTinh.DataTextField = "Name";
        ddlTinh.DataValueField = "Id";
        ddlTinh.DataBind();
    }

    private void BindGrid(bool isClearFilter)
    {
        try
        {
            if (isClearFilter)
            {
                ddlTinh.SelectedIndex = 0;
                chkTinh.Checked = false;
            }
            string strWhereClause = "LevelNbr in (1,2,3)";

            if (chkTinh.Checked)
                strWhereClause = "LevelNbr  = 1";
            else
                strWhereClause = "LevelNbr in (1,2,3)";

            if (!ddlTinh.SelectedValue.Equals("0"))
                strWhereClause += " AND (Id=" + ddlTinh.SelectedValue + " or ParentId=" + ddlTinh.SelectedValue + ")";



            var tinhObj = ServiceFactory.GetInstanceProvince().GetListDynamic("", strWhereClause, "");
            if (tinhObj != null && tinhObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + tinhObj.Count + " tỉnh được tìm thấy.</font>";


                grvView.DataSource = ProvinceImpl.BuildTree(tinhObj, tinhObj[0].ParentId, tinhObj[0].LevelNbr, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"); ;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có tỉnh được tìm thấy.</font>";
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

            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
            e.Row.Cells[1].Text = "<a href='tinh_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGrid(false);
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

    protected void btDelete_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceProvince();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    var id = grvView.DataKeys[i].Value.ToString();
                    obj.DeleteDynamic("Id='" + id + "'");
                }
                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false);
    }


    protected void btThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("tinh_add.aspx", false);
    }

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false);
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceProvince();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    var id = grvView.DataKeys[i].Value.ToString();
                    obj.DeleteDynamic("Id='" + id + "'");
                }
                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false);
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("tinh_add.aspx", false);
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        BindGrid(false);
    }

    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        BindGrid(false);
    }
}

