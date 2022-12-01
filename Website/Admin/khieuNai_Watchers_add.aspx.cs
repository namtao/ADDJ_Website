using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_khieuNai_Watchers_add : System.Web.UI.Page
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
			var khieuNaiIdObj = ServiceFactory.GetInstanceKhieuNai().GetList();
			if (khieuNaiIdObj != null && khieuNaiIdObj.Count > 0)
			{
				ddlKhieuNaiId.DataSource = khieuNaiIdObj;
				ddlKhieuNaiId.DataTextField = "Name";
				ddlKhieuNaiId.DataValueField = "Id";
				ddlKhieuNaiId.DataBind();
			}

			var nguoiSuDungIdObj = ServiceFactory.GetInstanceNguoiSuDung().GetList();
			if (nguoiSuDungIdObj != null && nguoiSuDungIdObj.Count > 0)
			{
				ddlNguoiSuDungId.DataSource = nguoiSuDungIdObj;
				ddlNguoiSuDungId.DataTextField = "Name";
				ddlNguoiSuDungId.DataValueField = "Id";
				ddlNguoiSuDungId.DataBind();
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
			var obj = ServiceFactory.GetInstanceKhieuNai_Watchers();
			KhieuNai_WatchersInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData khieuNai_Watchers_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
			else
			{
				ddlKhieuNaiId.SelectedValue = item.KhieuNaiId.ToString();
				ddlNguoiSuDungId.SelectedValue = item.NguoiSuDungId.ToString();
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
			var obj = ServiceFactory.GetInstanceKhieuNai_Watchers();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					KhieuNai_WatchersInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function khieuNai_Watchers_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Utility.UrlRoot + Config.PathError, false);
						return;
					}

					item.KhieuNaiId = Convert.ToInt32(ddlKhieuNaiId.SelectedValue);
					item.NguoiSuDungId = Convert.ToInt32(ddlNguoiSuDungId.SelectedValue);
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
				var item = new KhieuNai_WatchersInfo();

				item.KhieuNaiId = Convert.ToInt32(ddlKhieuNaiId.SelectedValue);
				item.NguoiSuDungId = Convert.ToInt32(ddlNguoiSuDungId.SelectedValue);
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

				Response.Redirect("khieuNai_Watchers_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
		}
	}

