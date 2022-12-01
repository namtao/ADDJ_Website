using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_phongBan_Permission_add : PageBase
	{
	protected void Page_Load(object sender, EventArgs e)
	{
		
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
			var permissionSchemeIdObj = ServiceFactory.GetInstancePermissionSchemes().GetList();
			if (permissionSchemeIdObj != null && permissionSchemeIdObj.Count > 0)
			{
				ddlPermissionSchemeId.DataSource = permissionSchemeIdObj;
				ddlPermissionSchemeId.DataTextField = "Name";
				ddlPermissionSchemeId.DataValueField = "Id";
				ddlPermissionSchemeId.DataBind();
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
			Response.Redirect(Config.PathError, false);
			return;
		}
	}

	private void EditData()
	{
		try
		{
			var obj = ServiceFactory.GetInstancePhongBan_Permission();
			PhongBan_PermissionInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData phongBan_Permission_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Config.PathError, false);
				return;
			}
			else
			{
				ddlPermissionSchemeId.SelectedValue = item.PermissionSchemeId.ToString();
				ddlPhongBanId.SelectedValue = item.PhongBanId.ToString();
				chkIsAllow.Checked = item.IsAllow;
				//txtIsAllow.Text = item.IsAllow.ToString();
			}
		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Config.PathError, false);
			return;
		}
	}
	protected void btSubmit_Click(object sender, EventArgs e)
	{
		if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
		{
			Response.Redirect(Config.PathNotRight, false);
			return;
		}

		try
		{
			var obj = ServiceFactory.GetInstancePhongBan_Permission();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					PhongBan_PermissionInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function phongBan_Permission_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Config.PathError, false);
						return;
					}

					item.PermissionSchemeId = Convert.ToInt32(ddlPermissionSchemeId.SelectedValue);
					item.PhongBanId = Convert.ToInt32(ddlPhongBanId.SelectedValue);
				try 
				{ 
					item.IsAllow = chkIsAllow.Checked;
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
					Response.Redirect(Config.PathError, false);
					return;
				}
			}
			else
			{
				var item = new PhongBan_PermissionInfo();

				item.PermissionSchemeId = Convert.ToInt32(ddlPermissionSchemeId.SelectedValue);
				item.PhongBanId = Convert.ToInt32(ddlPhongBanId.SelectedValue);
				try 
				{ 
					item.IsAllow = chkIsAllow.Checked;
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("phongBan_Permission_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Config.PathError, false);
				return;
			}
		}
	}

