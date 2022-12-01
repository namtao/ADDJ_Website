using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AIVietNam.Core;


using AIVietNam.Admin;
using Website.AppCode;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;

public partial class admin_groupadmin_manager : Website.AppCode.PageBase
{   

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindGird();
        }

    }

    protected string HanhDong(object id)
    {
        return string.Format("<a href='PhanQuyenGroup.aspx?ID={0}'>Phân quyền</a>", id);
    }

    private void BindGird()
    {
        try
        {
            grvView.DataSource = NguoiSuDung_GroupImpl.NhomNguoiDung;
            grvView.DataBind();
        }
        catch {
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

            var status = (CheckBox)e.Row.FindControl("cbSelectAll");

            e.Row.Cells[1].Text = "<a href=\"groupadmin_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text + "</a>";
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGird();
    }


    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch(Exception ex) {
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
        Response.Redirect("groupadmin_add.aspx", false);
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
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("IsStatus");

                if (status.Checked)
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    NguoiSuDung_GroupInfo item = ServiceFactory.GetInstanceNguoiSuDung_Group().GetInfo(adminID);
                    item.Status = 1;
                    ServiceFactory.GetInstanceNguoiSuDung_Group().Update(item);
                }
                else
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    NguoiSuDung_GroupInfo item = ServiceFactory.GetInstanceNguoiSuDung_Group().GetInfo(adminID);
                    item.Status = 0;
                    ServiceFactory.GetInstanceNguoiSuDung_Group().Update(item);
                }
                i++;
            }
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }

        NguoiSuDung_GroupImpl.NhomNguoiDung = ServiceFactory.GetInstanceNguoiSuDung_Group().GetList();
        BindGird();
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
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");

                if (status.Checked)
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    ServiceFactory.GetInstanceNguoiSuDung_Group().Delete(adminID);
                }

                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }

        NguoiSuDung_GroupImpl.NhomNguoiDung = ServiceFactory.GetInstanceNguoiSuDung_Group().GetList();

        BindGird();
    }
}
