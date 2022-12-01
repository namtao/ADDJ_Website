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

public partial class admin_configDisplayColumn_manager : MyPage
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
			var configDisplayColumnObj = ServiceFactory.GetInstanceConfigDisplayColumn().GetList();
			if (configDisplayColumnObj != null && configDisplayColumnObj.Count > 0)
			{
				ltThongBao.Text = "<font color='red'>Có " + configDisplayColumnObj.Count + " ConfigDisplayColumn được tìm thấy.</font>";
				grvView.DataSource = configDisplayColumnObj;
				grvView.DataBind();
			}
			else
			{
				ltThongBao.Text = "<font color='red'>Không có Log được tìm thấy.</font>";
grvView.DataSource = null;
grvView.DataBind();
			}

		}
		catch(Exception ex)
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
			}			else			{
				e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
				e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
			}
			e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
			e.Row.Cells[1].Text = "<a href='configDisplayColumn_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
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
		catch(Exception ex)
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
		if (!UserRightImpl.CheckRightAdminnistrator().UserDelete)
		{
			Response.Redirect(Config.PathNotRight, false);
			return;
		}

		try
		{
			int i = 0;
			var obj = ServiceFactory.GetInstanceConfigDisplayColumn();
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
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Config.PathError, false);
			return;
		}
			BindGrid();
	}

}

