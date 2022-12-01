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

public partial class admin_loaiKhieuNai2PhongBan_manager : Page
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
			var loaiKhieuNaiIdObj = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
			if (loaiKhieuNaiIdObj != null && loaiKhieuNaiIdObj.Count > 0)
			{
				loaiKhieuNaiIdObj.Insert(0,new LoaiKhieuNaiInfo() { Id=0, Name="[ Tất cả ]" });

				ddlLoaiKhieuNaiId.DataSource = loaiKhieuNaiIdObj;
				ddlLoaiKhieuNaiId.DataTextField = "Name";
				ddlLoaiKhieuNaiId.DataValueField = "Id";
				ddlLoaiKhieuNaiId.DataBind();
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


	private void BindGrid(int _loaiKhieuNaiId,int _phongBanId)
	{
		try
		{
			var loaiKhieuNai2PhongBanObj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetList();
			if (loaiKhieuNai2PhongBanObj != null && loaiKhieuNai2PhongBanObj.Count > 0)
			{
				ltThongBao.Text = "<font color='red'>Có " + loaiKhieuNai2PhongBanObj.Count + " LoaiKhieuNai2PhongBan được tìm thấy.</font>";
				grvView.DataSource = loaiKhieuNai2PhongBanObj;
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
			e.Row.Cells[1].Text = "<a href='loaiKhieuNai2PhongBan_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
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
			var obj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan();
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

	protected void ddlLoaiKhieuNaiId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0);
	}
	protected void ddlPhongBanId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0);
	}

	protected void btThemMoi_Click(object sender, EventArgs e)
	{
		Response.Redirect("loaiKhieuNai2PhongBan_add.aspx", false);
	}
}

