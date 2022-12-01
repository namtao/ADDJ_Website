using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

	public partial class admin_nhomNguoiDung_AI_add : PageBase
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
			var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI();
			NhomNguoiDung_AIInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData nhomNguoiDung_AI_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Config.PathError, false);
				return;
			}
			else
			{
				txtName.Text = item.Name.ToString();
				txtDescription.Text = item.Description.ToString();
				chkActive.Checked = item.Active;
			}
		}
		catch(Exception ex)
		{
			Utility.LogEvent(ex);
			Response.Redirect(Config.PathError, false);
			return;
		}
	}
 

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceNhomNguoiDung_AI();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    NhomNguoiDung_AIInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function nhomNguoiDung_AI_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();
                        item.Active = chkActive.Checked;
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    obj.Update(item);
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
            }
            else
            {
                var item = new NhomNguoiDung_AIInfo();

                try
                {
                    item.Name = txtName.Text.Trim();
                    item.Description = txtDescription.Text.Trim();
                    item.Active = chkActive.Checked;
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }

                obj.Add(item);
            }

            Response.Redirect("nhomNguoiDung_AI_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

