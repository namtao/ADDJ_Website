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
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class admin_doiTac_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ddlLevel.AutoPostBack = true;
        ddlLevel.SelectedIndexChanged += DdlLevel_SelectedIndexChanged;
        if (!IsPostBack)
        {
            // GrvView.PageSize = Config.RecordPerPage;
            BindDoiTac(Request.QueryString["ParentId"]);
            BindLoaiTimKiem(Request.QueryString["SType"]);
            ddlLevel.SelectedValue = Request.QueryString["Level"];
            BindGrid(true);
        }
    }

    private void DdlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("DoiTac_Manager.aspx?ParentId={0}&Level={1}", ddlDoiTac.SelectedValue, ddlLevel.SelectedValue));
    }

    private void BindDoiTac(string seleted)
    {
        AdminInfo userInfo = LoginAdmin.AdminLogin();
        int parentId = 0;

        if (userInfo.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET || userInfo.Username.ToLower() == "Administrator".ToLower()) parentId = 0;
        else parentId = userInfo.DoiTacId;

        DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, "DoiTac_TimKiem", string.Empty, parentId, 2).Tables[0];
        foreach (DataRow row in tbl.Rows)
        {
            row["MaDoiTac"] = Website.AppCode.Common.GiveEName(row["MaDoiTac"], (int)row["Level"], "+ ");
        }

        ddlDoiTac.DataSource = tbl;
        ddlDoiTac.DataValueField = "ID";
        ddlDoiTac.DataTextField = "MaDoiTac";
        ddlDoiTac.DataBind();

        ddlDoiTac.Items.Insert(0, new ListItem("-- Đối tác --", "0"));

        ddlDoiTac.SelectedValue = Request.QueryString["ParentId"];
    }
    private void BindLoaiTimKiem(string selected)
    {
        ddlFilter.Items.Add(new ListItem("-- Lọc theo --", "0"));
        ddlFilter.Items.Add(new ListItem("Mã đối tác", "MaDoiTac"));
        ddlFilter.Items.Add(new ListItem("Tên đối tác", "TenDoiTac"));
        ddlFilter.Items.Add(new ListItem("Địa chỉ", "DiaChi"));
        ddlFilter.SelectedValue = selected;
    }
    private void BindGrid(bool isClearFilter)
    {
        if (isClearFilter)
        {
            ddlDoiTac.SelectedIndex = 0;
            txtKeyword.Text = string.Empty;
            ddlFilter.SelectedIndex = 0;
        }

        int DoiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
        string filterType = ddlFilter.SelectedValue;
        string query = txtKeyword.Text;

        string spName = "DoiTac_TimKiem";
        SqlParameter[] prms = new SqlParameter[]
        {
                new SqlParameter("@TuKhoa", txtKeyword.Text),
                new SqlParameter("@ParentId", DoiTacId)
        };

        DataTable newTable = SqlHelper.ExecuteDataset(Config.ConnectionString, spName, txtKeyword.Text, DoiTacId, Convert.ToInt32(ddlLevel.SelectedValue)).Tables[0];
        ltThongBao.Text = "<font color='red'>Có " + newTable.Rows.Count + " đối tác được tìm thấy.</font>";
        GrvView.DataSource = newTable;
        GrvView.DataBind();
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
                    SqlHelper.ExecuteNonQuery(Config.ConnectionString, "DoiTac_SapXepLen", prms);
                    this.BindGrid(false);
                    break;
                }
            case "Down":
                {
                    SqlParameter[] prms = new SqlParameter[] {
                        new SqlParameter("@Id", menuId)
                    };
                    SqlHelper.ExecuteNonQuery(Config.ConnectionString, "DoiTac_SapXepXuong", prms);
                    this.BindGrid(false);
                    break;
                }
        }
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        GrvView.PageIndex = e.NewPageIndex;
        BindGrid(false);
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
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void GrvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    protected void btThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("doiTac_add.aspx", false);
    }

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false);
    }
}

