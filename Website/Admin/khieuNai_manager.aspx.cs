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

public partial class admin_khieuNai_manager : Page
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
			BindGrid(0,0,0,0);
		}
	}

	private void BindDropDownList()
	{
		try
		{
			var khuVucIdObj = ServiceFactory.GetInstanceDoiTac().GetList();
			if (khuVucIdObj != null && khuVucIdObj.Count > 0)
			{
				khuVucIdObj.Insert(0,new DoiTacInfo() { Id=0, MaDoiTac ="[ Tất cả ]" });

				ddlKhuVucId.DataSource = khuVucIdObj;
                ddlKhuVucId.DataTextField = "MaDoiTac";
				ddlKhuVucId.DataValueField = "Id";
				ddlKhuVucId.DataBind();
			}

			var doiTacIdObj = ServiceFactory.GetInstanceDoiTac().GetList();
			if (doiTacIdObj != null && doiTacIdObj.Count > 0)
			{
				doiTacIdObj.Insert(0,new DoiTacInfo() { Id=0, MaDoiTac="[ Tất cả ]" });

				ddlDoiTacId.DataSource = doiTacIdObj;
                ddlDoiTacId.DataTextField = "MaDoiTac";
				ddlDoiTacId.DataValueField = "Id";
				ddlDoiTacId.DataBind();
			}

			var loaiKhieuNaiIdObj = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
			if (loaiKhieuNaiIdObj != null && loaiKhieuNaiIdObj.Count > 0)
			{
				loaiKhieuNaiIdObj.Insert(0,new LoaiKhieuNaiInfo() { Id=0, Name="[ Tất cả ]" });

				ddlLoaiKhieuNaiId.DataSource = loaiKhieuNaiIdObj;
				ddlLoaiKhieuNaiId.DataTextField = "Name";
				ddlLoaiKhieuNaiId.DataValueField = "Id";
				ddlLoaiKhieuNaiId.DataBind();
			}

			var maTinhObj = ServiceFactory.GetInstanceTinh().GetList();
			if (maTinhObj != null && maTinhObj.Count > 0)
			{
				maTinhObj.Insert(0,new TinhInfo() { MaTinh="[ Tất cả ]" });

				ddlMaTinh.DataSource = maTinhObj;
                ddlMaTinh.DataTextField = "MaTinh";
                ddlMaTinh.DataValueField = "MaTinh";
				ddlMaTinh.DataBind();
			}

		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
	}


	private void BindGrid(int _khuVucId,int _doiTacId,int _loaiKhieuNaiId,int _maTinh)
	{
		try
		{
			var khieuNaiObj = ServiceFactory.GetInstanceKhieuNai().GetList();
			if (khieuNaiObj != null && khieuNaiObj.Count > 0)
			{
				ltThongBao.Text = "<font color='red'>Có " + khieuNaiObj.Count + " KhieuNai được tìm thấy.</font>";
				grvView.DataSource = khieuNaiObj;
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
			e.Row.Cells[1].Text = "<a href='khieuNai_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
		}
	}

	private void PageIndexChanging(GridViewPageEventArgs e)
	{
		grvView.PageIndex = e.NewPageIndex;
			BindGrid(0,0,0,0);
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
			var obj = ServiceFactory.GetInstanceKhieuNai();
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
			BindGrid(0,0,0,0);
	}

	protected void ddlKhuVucId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0,0,0);
	}
	protected void ddlDoiTacId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0,0,0);
	}
	protected void ddlLoaiKhieuNaiId_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0,0,0);
	}
	protected void ddlMaTinh_SelectedIndexChanged(object sender, EventArgs e)
	{
			BindGrid(0,0,0,0);
	}

	protected void btThemMoi_Click(object sender, EventArgs e)
	{
		Response.Redirect("khieuNai_add.aspx", false);
	}
}

