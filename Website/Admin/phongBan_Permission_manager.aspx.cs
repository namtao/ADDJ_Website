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

public partial class admin_phongBan_Permission_manager : Page
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
			BindDropDownList();
			BindGrid(0,0);
		}
	}

	private void BindDropDownList()
	{
		try
		{
			var permissionSchemeIdObj = ServiceFactory.GetInstancePermissionSchemes().GetList();
			if (permissionSchemeIdObj != null && permissionSchemeIdObj.Count > 0)
			{
				permissionSchemeIdObj.Insert(0,new PermissionSchemesInfo() { Id=0, Name="[ Tất cả ]" });

				ddlPermissionSchemeId.DataSource = permissionSchemeIdObj;
				ddlPermissionSchemeId.DataTextField = "Name";
				ddlPermissionSchemeId.DataValueField = "Id";
				ddlPermissionSchemeId.DataBind();
			}

			var phongBanIdObj = ServiceFactory.GetInstancePhongBan().GetList();
			if (phongBanIdObj != null && phongBanIdObj.Count > 0)
			{
				phongBanIdObj.Insert(0,new PhongBanInfo() { Id=0, Name="[ Tất cả ]" });

				ddlPhongBanId.DataSource = phongBanIdObj;
				ddlPhongBanId.DataTextField = "Name";
				ddlPhongBanId.DataValueField = "Id";
				ddlPhongBanId.DataBind();
			}

		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
	}


	private void BindGrid(int _permissionSchemeId,int _phongBanId)
	{
		try
		{
			var phongBan_PermissionObj = ServiceFactory.GetInstancePhongBan_Permission().GetList();
			if (phongBan_PermissionObj != null && phongBan_PermissionObj.Count > 0)
			{
				ltThongBao.Text = "<font color='red'>Có " + phongBan_PermissionObj.Count + " PhongBan_Permission được tìm thấy.</font>";
				grvView.DataSource = phongBan_PermissionObj;
				grvView.DataBind();
			}

		}
		catch(Exception ex)
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
			}			else			{
				e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
				e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
			}
			e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
			e.Row.Cells[1].Text = "<a href='phongBan_Permission_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
		}
	}

	private void PageIndexChanging(GridViewPageEventArgs e)
	{
		grvView.PageIndex = e.NewPageIndex;
			BindGrid(0,0);
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
			var obj = ServiceFactory.GetInstancePhongBan_Permission();
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
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
			BindGrid(0,0);
	}

	protected void ddlPermissionSchemeId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0);
	}
	protected void ddlPhongBanId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0);
	}

	protected void btThemMoi_Click(object sender, EventArgs e)
	{
		Response.Redirect("phongBan_Permission_add.aspx", false);
	}
}

