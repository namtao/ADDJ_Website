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

public partial class ConfigurationArchive_add : Website.AppCode.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDownList();
            lblMsg.Text = "";
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }

    private void BindDropDownList()
    {
        for (int i = 2010; i <= 2020; i++)
        {
            ddlNam.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
        }
        ddlNam.SelectedValue = DateTime.Now.Year.ToString();
    }

    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceArchiveConfig();
            ArchiveConfigInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData archiveConfig_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                ddlNam.SelectedValue = item.NamLuuTru.ToString();
                txtName.Text = item.Name.ToString();
                txtDescription.Text = item.Description.ToString();
                txtServerName.Text = item.ServerName.ToString();
                txtDatabaseName.Text = item.DatabaseName.ToString();
                txtUsername.Text = item.Username.ToString();
                txtPassword.Text = item.Password.ToString();
                txtPathFileSystem.Text = item.PathFileSystem.ToString();
                txtPathUrlFile.Text = item.PathUrlFile.ToString();
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
 
    protected void linkSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceArchiveConfig();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    ArchiveConfigInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function archiveConfig_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        item.NamLuuTru = Convert.ToInt32(ddlNam.SelectedValue);
                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();
                        item.ServerName = txtServerName.Text.Trim();
                        item.DatabaseName = txtDatabaseName.Text.Trim();
                        item.Username = txtUsername.Text.Trim();
                        item.Password = txtPassword.Text.Trim();
                        item.ConnectionString = ServiceFactory.GetInstanceArchiveConfig().BuildConnectionString(item.ServerName, item.DatabaseName, item.Username, item.Password);
                        item.PathFileSystem = txtPathFileSystem.Text.Trim();
                        item.PathUrlFile = txtPathUrlFile.Text.Trim();
                        item.IsCurrent = false;
                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Dữ liệu không hợp lệ','error');", true);
                        return;
                    }

                    if (ServiceFactory.GetInstanceArchiveConfig().CheckValidConnectionString(item.ConnectionString))
                        obj.Update(item);
                    else
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Dữ liệu kết nối đến archive chưa đúng.','error');", true);
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
                var item = new ArchiveConfigInfo();

                try
                {
                    item.NamLuuTru = Convert.ToInt32(ddlNam.SelectedValue);
                    item.Name = txtName.Text.Trim();
                    item.Description = txtDescription.Text.Trim();
                    item.ServerName = txtServerName.Text.Trim();
                    item.DatabaseName = txtDatabaseName.Text.Trim();
                    item.Username = txtUsername.Text.Trim();
                    item.Password = txtPassword.Text.Trim();
                    item.ConnectionString = ServiceFactory.GetInstanceArchiveConfig().BuildConnectionString(item.ServerName, item.DatabaseName, item.Username, item.Password);
                    item.PathFileSystem = txtPathFileSystem.Text.Trim();
                    item.PathUrlFile = txtPathUrlFile.Text.Trim();
                    item.IsCurrent = false;
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Dữ liệu không hợp lệ','error');", true);
                    return;
                }

                if (ServiceFactory.GetInstanceArchiveConfig().CheckValidConnectionString(item.ConnectionString))
                    obj.Add(item);
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Dữ liệu kết nối đến archive chưa đúng.','error');", true);
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertRedirect('Thêm mới archive thành công.','info','ConfigurationArchive.aspx');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

