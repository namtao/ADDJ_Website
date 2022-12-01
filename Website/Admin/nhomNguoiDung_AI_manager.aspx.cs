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

public partial class admin_nhomNguoiDung_AI_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grvView.PageSize = Config.RecordPerPage;
            BindGrid();
        }
    }

    protected string BindHanhDong(object id)
    {
        return string.Format("<a href='NhomNguoiDungAI2User.aspx?NhomNguoiDungAiId={0}'>Thêm, xóa user</a> | <a href='PhanQuyenNhomNguoiDungAI_System.aspx?NhomNguoiDungAiId={0}' target='_blank'>Phân quyền hệ thống</a> | <a href='PhanQuyenNhomNguoiDung_AIKhieuNai.aspx?NhomNguoiDungAiId={0}'  target='_blank'>Phân quyền KN</a>", id);
    }

    protected string BindCountNguoiDung(object id)
    {
        return id.ToString();
    }

    private void BindGrid()
    {
        try
        {
            string selectClause = "*,(select count(*) from NhomNguoiDung_AI_Detail where NhomNguoiDung_AI_Detail.NhomNguoiDung_AIId = NhomNguoiDung_AI.Id) CountNguoiSuDung";
            var nhomNguoiDung_AIObj = ServiceFactory.GetInstanceNhomNguoiDung_AI().GetListDynamic(selectClause,"","");
            if (nhomNguoiDung_AIObj != null && nhomNguoiDung_AIObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + nhomNguoiDung_AIObj.Count + " NhomNguoiDung_AI được tìm thấy.</font>";
                grvView.DataSource = nhomNguoiDung_AIObj;
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
            //if (e.Row.RowIndex % 2 != 0)
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#DEEAF3'");
            //}
            //else
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
            //}
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
            e.Row.Cells[1].Text = "<a href='nhomNguoiDung_AI_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
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
        Response.Redirect("nhomNguoiDung_AI_add.aspx", false);
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
            var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI();
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

