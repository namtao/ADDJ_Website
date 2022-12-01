using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_configColumn_add : MyPage
	{
	protected void Page_Load(object sender, EventArgs e)
	{
		
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
			var obj = ServiceFactory.GetInstanceConfigColumn();
			ConfigColumnInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData configColumn_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Config.PathError, false);
				return;
			}
			else
			{
				txtTypeDisplay.Text = item.TypeDisplay.ToString();
				txtColumnName.Text = item.ColumnName.ToString();
				txtDisplayName.Text = item.DisplayName.ToString();
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
		if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
		{
			Response.Redirect(Config.PathNotRight, false);
			return;
		}

		try
		{
			var obj = ServiceFactory.GetInstanceConfigColumn();
			if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
			{
				try
				{
					int idEdit = int.Parse(Request.QueryString["ID"]);
					ConfigColumnInfo item = obj.GetInfo(idEdit);

					if (item == null)
					{
						Utility.LogEvent("Function configColumn_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
						Response.Redirect(Config.PathError, false);
						return;
					}

				try 
				{ 
						item.TypeDisplay = Convert.ToInt32(txtTypeDisplay.Text.Trim());
						item.ColumnName = txtColumnName.Text.Trim();
						item.DisplayName = txtDisplayName.Text.Trim();
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
				var item = new ConfigColumnInfo();

				try 
				{ 
					item.TypeDisplay = Convert.ToInt32(txtTypeDisplay.Text.Trim());
					item.ColumnName = txtColumnName.Text.Trim();
					item.DisplayName = txtDisplayName.Text.Trim();
				}
				catch
				{
					lblMsg.Text = "Dữ liệu không hợp lệ"; 
					return; 
				}

				obj.Add(item);
				}

				Response.Redirect("configColumn_manager.aspx", false);
			}
			catch (Exception ex)
			{
				Utility.LogEvent(ex);
				Response.Redirect(Config.PathError, false);
				return;
			}
		}
	}

