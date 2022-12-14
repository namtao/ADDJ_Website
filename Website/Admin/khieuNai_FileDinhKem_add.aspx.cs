using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_khieuNai_FileDinhKem_add : System.Web.UI.Page
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
			var obj = ServiceFactory.GetInstanceKhieuNai_FileDinhKem();
			KhieuNai_FileDinhKemInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData khieuNai_FileDinhKem_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
			else
			{
				ddlKhieuNaiId.SelectedValue = item.KhieuNaiId.ToString();
				txtTenFile.Text = item.TenFile.ToString();
				txtKichThuoc.Text = item.KichThuoc.ToString();
				txtLoaiFile.Text = item.LoaiFile.ToString();
				txtGhiChu.Text = item.GhiChu.ToString();
				txtURLFile.Text = item.URLFile.ToString();
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
			var obj = ServiceFactory.GetInstanceKhieuNai_FileDinhKem();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					KhieuNai_FileDinhKemInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function khieuNai_FileDinhKem_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Utility.UrlRoot + Config.PathError, false);
						return;
					}

					item.KhieuNaiId = Convert.ToInt32(ddlKhieuNaiId.SelectedValue);
				try 
				{ 
					item.TenFile = txtTenFile.Text.Trim();
					item.KichThuoc = Convert.ToDecimal(txtKichThuoc.Text.Trim());
					item.LoaiFile = Convert.ToInt16(txtLoaiFile.Text.Trim());
					item.GhiChu = txtGhiChu.Text.Trim();
					item.URLFile = txtURLFile.Text.Trim();
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
				var item = new KhieuNai_FileDinhKemInfo();

				item.KhieuNaiId = Convert.ToInt32(ddlKhieuNaiId.SelectedValue);
				try 
				{ 
					item.TenFile = txtTenFile.Text.Trim();
					item.KichThuoc = Convert.ToDecimal(txtKichThuoc.Text.Trim());
					item.LoaiFile = Convert.ToInt16(txtLoaiFile.Text.Trim());
					item.GhiChu = txtGhiChu.Text.Trim();
					item.URLFile = txtURLFile.Text.Trim();
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("khieuNai_FileDinhKem_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
		}
	}

