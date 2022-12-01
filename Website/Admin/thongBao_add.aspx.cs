using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web.UI;
	public partial class admin_thongBao_add : PageBase
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
			var obj = ServiceFactory.GetInstanceThongBao();
			ThongBaoInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
			if (item == null)
			{
				Utility.LogEvent("Function EditData thongBao_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
				Response.Redirect(Config.PathError, false);
				return;
			}
			else
			{
				txtTieuDe.Text = item.TieuDe.ToString();
                txtNoiDung.Text = System.Web.HttpUtility.HtmlDecode(item.NoiDung.ToString());
				chkDisplay.Checked = item.Display;
                chkNew.Checked = item.IsNew;
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
            var obj = ServiceFactory.GetInstanceThongBao();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    ThongBaoInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function thongBao_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        if (txtTieuDe.Text.Trim() != "")
                        {
                            item.TieuDe = txtTieuDe.Text.Trim();
                        }
                        else
                        {
                            string msg = "Không để trống trường tiêu đề";
                            string alertScript = "<script type=\"text/javascript\">alert('" + msg + "'); location.reload()</script>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", alertScript, false);
                            return;
                        }

                        if (txtNoiDung.Text.Trim() != "")
                        {
                            item.NoiDung = Server.HtmlEncode(txtNoiDung.Text.Trim());
                        }
                        else
                        {
                            string msg = "Không để trống trường nội dung";
                            string alertScript = "<script type=\"text/javascript\">alert('" + msg + "'); location.reload()</script>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", alertScript, false);
                            return;
                        }
                        item.IsNew = chkNew.Checked;
                        item.Display = chkDisplay.Checked;
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
                var item = new ThongBaoInfo();

                try
                {
                    if (txtTieuDe.Text.Trim() != "")
                    {
                        item.TieuDe = txtTieuDe.Text.Trim();
                    }
                    else
                    {
                        string msg = "Không để trống trường tiêu đề";
                        string alertScript = "<script type=\"text/javascript\">alert('" + msg + "'); location.reload()</script>";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", alertScript, false);
                        return;
                    }

                    if (txtNoiDung.Text.Trim() != "")
                    {
                        item.NoiDung = Server.HtmlEncode(txtNoiDung.Text.Trim());
                    }
                    else
                    {
                        string msg = "Không để trống trường nội dung";
                        string alertScript = "<script type=\"text/javascript\">alert('" + msg + "'); location.reload()</script>";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", alertScript, false);
                        return;
                    }
                    item.IsNew = chkNew.Checked;
                    item.Display = chkDisplay.Checked;
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }

                obj.Add(item);
            }

            Response.Redirect("thongBao_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

