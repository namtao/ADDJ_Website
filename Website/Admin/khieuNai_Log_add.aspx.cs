using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_khieuNai_Log_add : System.Web.UI.Page
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
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				EditData();
			}
		}
	}
	private void EditData()
	{
		try
		{
			var obj = ServiceFactory.GetInstanceKhieuNai_Log();
			KhieuNai_LogInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData khieuNai_Log_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
			else
			{
				txtKhieuNaiId.Text = item.KhieuNaiId.ToString();
				txtTruongThayDoi.Text = item.TruongThayDoi.ToString();
				txtGiaTriCu.Text = item.GiaTriCu.ToString();
				txtGiaTriMoi.Text = item.GiaTriMoi.ToString();
				txtThaoTac.Text = item.ThaoTac.ToString();
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
			var obj = ServiceFactory.GetInstanceKhieuNai_Log();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					KhieuNai_LogInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function khieuNai_Log_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Utility.UrlRoot + Config.PathError, false);
						return;
					}

				try 
				{ 
						item.KhieuNaiId = Convert.ToInt32(txtKhieuNaiId.Text.Trim());
						item.TruongThayDoi = txtTruongThayDoi.Text.Trim();
						item.GiaTriCu = txtGiaTriCu.Text.Trim();
						item.GiaTriMoi = txtGiaTriMoi.Text.Trim();
						item.ThaoTac = txtThaoTac.Text.Trim();
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
				var item = new KhieuNai_LogInfo();

				try 
				{ 
					item.KhieuNaiId = Convert.ToInt32(txtKhieuNaiId.Text.Trim());
					item.TruongThayDoi = txtTruongThayDoi.Text.Trim();
					item.GiaTriCu = txtGiaTriCu.Text.Trim();
					item.GiaTriMoi = txtGiaTriMoi.Text.Trim();
					item.ThaoTac = txtThaoTac.Text.Trim();
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("khieuNai_Log_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Utility.UrlRoot + Config.PathError, false);
				return;
			}
		}
	}

