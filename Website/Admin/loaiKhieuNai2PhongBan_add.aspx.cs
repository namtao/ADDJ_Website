using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_loaiKhieuNai2PhongBan_add : System.Web.UI.Page
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
			lblMsg.Text ="";
			BindDropDownList();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				EditData();
			}
		}
	}
	private void BindDropDownList() 
	{
		try
		{
			var loaiKhieuNaiIdObj = ServiceFactory.GetInstanceLoaiKhieuNai().GetList();
			if (loaiKhieuNaiIdObj != null && loaiKhieuNaiIdObj.Count > 0)
			{
				ddlLoaiKhieuNaiId.DataSource = loaiKhieuNaiIdObj;
				ddlLoaiKhieuNaiId.DataTextField = "Name";
				ddlLoaiKhieuNaiId.DataValueField = "Id";
				ddlLoaiKhieuNaiId.DataBind();
			}

			var phongBanIdObj = ServiceFactory.GetInstancePhongBan().GetList();
			if (phongBanIdObj != null && phongBanIdObj.Count > 0)
			{
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

	private void EditData()
	{
		try
		{
			var obj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan();
			LoaiKhieuNai2PhongBanInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData loaiKhieuNai2PhongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
			else
			{
				ddlLoaiKhieuNaiId.SelectedValue = item.LoaiKhieuNaiId.ToString();
				ddlPhongBanId.SelectedValue = item.PhongBanId.ToString();
			}
		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Utility.UrlRoot + Config.PathError, false);
			return;
		}
	}
	protected void btSubmit_Click(object sender, EventArgs e)
	{
		if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
		{
			Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
			return;
		}

		try
		{
			var obj = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					LoaiKhieuNai2PhongBanInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function loaiKhieuNai2PhongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Utility.UrlRoot + Config.PathError, false);
						return;
					}

					item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiId.SelectedValue);
					item.PhongBanId = Convert.ToInt32(ddlPhongBanId.SelectedValue);
				try 
				{ 
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

					obj.Update(item);
				}
				catch(Exception ex)
				{
					Utility.LogEvent(ex);
					Response.Redirect(Utility.UrlRoot + Config.PathError, false);
					return;
				}
			}
			else
			{
				var item = new LoaiKhieuNai2PhongBanInfo();

				item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiId.SelectedValue);
				item.PhongBanId = Convert.ToInt32(ddlPhongBanId.SelectedValue);
				try 
				{ 
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("loaiKhieuNai2PhongBan_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
		}
	}

