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

public partial class admin_loaiPhongBan_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            grvView.PageSize = Config.RecordPerPage;
            BindGrid(false);
        }
    }

    protected string BindHanhDong(object id)
    {
        return string.Format(" | <a href='LoaiPhongBan2PhongBan.aspx?LoaiPhongBanId={0}'>Thêm, xóa Phòng ban</a> | <a href='loaiPhongBan_ThoiGianXuLyKhieuNai_manager.aspx?LoaiPhongBanId={0}'>Xem thời gian xử lý KN</a> | <a href='PhanQuyenLoaiPhongBan.aspx?LoaiPhongBanId={0}'>Phân quyền KN</a>", id);
    }

    protected string BindCountPhongBan(object id, object countPhongBan)
    {
        return string.Format("{0} (<a href='phongBan_manager.aspx?LoaiPhongBanId={1}'>Xem danh sách</a>)", countPhongBan, id);
    }

    private void BindGrid(bool isClearFilter)
    {
        try
        {
            if (isClearFilter)
            {
                txtNameFilter.Text = "";
            }

            string selectClause = "*,(select count(*) from PhongBan where LoaiPhongBanId = LoaiPhongBan.Id) CountPhongBan";

            string whereClause = string.Empty;

            if (!txtNameFilter.Text.Equals(""))
                whereClause = "Name like N'%" + txtNameFilter.Text + "%'";

            var loaiPhongBanObj = ServiceFactory.GetInstanceLoaiPhongBan().GetListDynamic(selectClause, whereClause, "");
            if (loaiPhongBanObj != null && loaiPhongBanObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + loaiPhongBanObj.Count + " loại phòng ban được tìm thấy.</font>";
                grvView.DataSource = loaiPhongBanObj;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có loại phòng ban được tìm thấy.</font>";
                grvView.DataSource = null;
                grvView.DataBind();
            }

            if (IsPostBack)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
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
            (e.Row.FindControl("lnkEdit") as LinkButton).Attributes.Add("onClick", "ShowEditModal('" + grvView.DataKeys[e.Row.RowIndex].Value + "');");
            (e.Row.FindControl("lnkCopy") as LinkButton).Attributes.Add("onClick", "ShowCopyModal('" + grvView.DataKeys[e.Row.RowIndex].Value + "');");

            if(e.Row.Cells[4].Text == "1")
            {
                e.Row.Cells[4].Text = "Ngày giờ";
            }
            else if(e.Row.Cells[4].Text == "2")
            {
                e.Row.Cells[4].Text = "%";
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }
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
  

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false);
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
            var obj = ServiceFactory.GetInstanceLoaiPhongBan();
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

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa loại phòng ban thành công.','info');", true);
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

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("loaiPhongBan_add.aspx", false);
    }
}

