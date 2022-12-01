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
using System.Web;
using System.Collections.Generic;

public partial class admin_congTyDoiTac_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        liJs.Text = string.Empty;
        btnSearch.Click += BtnSearch_Click;
        if (!IsPostBack)
        {
            txtKeySearch.Text = Request.QueryString["Keyword"];
            BindData();
        }
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        string keyword = txtKeySearch.Text.Trim();
        if (!string.IsNullOrEmpty(keyword)) Response.Redirect(string.Format("CongTyDoiTac_Manager.aspx?Keyword={0}", HttpUtility.UrlEncode(txtKeySearch.Text)));
        else Response.Redirect("CongTyDoiTac_Manager.aspx");
    }

    private void BindData()
    {

        string key = Request.QueryString["Keyword"];
        int pageIndex = 0;
        if (!int.TryParse(Request.QueryString["Page"], out pageIndex))
        {
            pageIndex = 1;
        }
        try
        {
            int totalCount = 0;
            Pager.PageSize = Config.RecordPerPage;
            // Pager.PageSize = 1; // Demo

            string where = string.Format("1 = 1{0}", !string.IsNullOrEmpty(key) ? string.Format(" AND Ten LIKE '%' + N'{0}' + '%'", key.Trim()) : string.Empty);
            List<CongTyDoiTacInfo> lstObjs = new CongTyDoiTacImpl().GetPaged("*", where, "LDate DESC", pageIndex, Pager.PageSize, ref totalCount);

            if (lstObjs != null && lstObjs.Count > 0)
            {
                ltThongBao.Text = string.Format("Tìm thấy <span class=\"red\">{0}</span> công ty", totalCount);
                GrvView.DataSource = lstObjs;
                GrvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<span class=\"red\">không tìm kết quả phù hợp</span>";
                GrvView.DataSource = null;
                GrvView.DataBind();
            }

            Pager.RecordCount = totalCount;

            Pager.EnableUrlRewriting = true;
            Pager.UrlRewritePattern = (string.Format("{0}{1}", string.Empty.GetData<string>(() =>
            {
                string urlKey = "?";
                if (!string.IsNullOrEmpty(Request.QueryString["KeyWord"]))
                {
                    urlKey = string.Format("?Keyword={0}", HttpUtility.UrlEncode(Request.QueryString["KeyWord"]));
                }
                return "CongTyDoiTac_Manager.aspx" + urlKey;
            }), "&Page={0}")).Replace("?&", "?");

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false); return;
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
            e.Row.Cells[1].Text = "<a href='CongTyDoiTac_Add.aspx?Id=" + GrvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        GrvView.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void GrvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false); return;
        }
    }

    protected void GrvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    // Chuyển đổi công năng, chỉ thay đổi trạng thái chứ không phải xóa
 

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("CongTyDoiTac_Add.aspx", false);
    }

    protected void linkbtnXoa_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false); return;

        }

        try
        {
            int i = 0;
            CongTyDoiTacImpl ctl = ServiceFactory.GetInstanceCongTyDoiTac();
            foreach (GridViewRow row in GrvView.Rows)
            {
                CheckBox status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int congTyDoiTacId = int.Parse(GrvView.DataKeys[i].Value.ToString());
                    CongTyDoiTacInfo obj = ctl.GetInfo(congTyDoiTacId);
                    obj.TrangThai = 0;
                    ctl.Update(obj);

                    liJs.Text = string.Format("alert('{0}');", "Cập nhật thành công");
                    // ctl.Delete(congTyDoiTacId);
                }
                i++;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false); return;
        }
        BindData();
    }
}

