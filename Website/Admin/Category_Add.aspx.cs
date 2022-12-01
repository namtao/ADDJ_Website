using System;
using AIVietNam.News.Impl;
using AIVietNam.Core;
using AIVietNam.News.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;

public partial class admin_category_add : System.Web.UI.Page
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
            lblMsg.Text = "";
            BindDropDownlist();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }

    private void BindDropDownlist()
    {
        List<CategoryInfo> list = ServiceFactory.GetInstanceCategory().GetListCategorySortParent();
        
        list.Insert(0,new CategoryInfo() { Id = 0, Name = "--Là cha--" });
        ddlParrent.DataTextField = "Name";
        ddlParrent.DataValueField = "Id";
        ddlParrent.DataSource = list;
        ddlParrent.DataBind();
    }

    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceCategory();
            CategoryInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData category_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Utility.UrlRoot + Config.PathError, false);
                return;
            }
            else
            {
                ddlParrent.SelectedValue = item.ParentId.ToString();
                txtName.Text = item.Name.ToString();
                txtDescription.Text = item.Description.ToString();
                
            }
        }
        catch (Exception ex)
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
            var obj = ServiceFactory.GetInstanceCategory();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    CategoryInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function category_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Utility.UrlRoot + Config.PathError, false);
                        return;
                    }

                    try
                    {
                        item.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();
                        item.Status = chkStatus.Checked ? (byte)1 : (byte)0;
                    }
                    catch { lblMsg.Text = "Dữ liệu không hợp lệ"; return; }

                    obj.Update(item);
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Response.Redirect(Utility.UrlRoot + Config.PathError, false);
                    return;
                }
            }
            else
            {
                var item = new CategoryInfo();

                try
                {
                    item.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                    item.Name = txtName.Text.Trim();
                    item.Description = txtDescription.Text.Trim();
                    item.Status = chkStatus.Checked ? (byte)1: (byte)0;
                }
                catch { lblMsg.Text = "Dữ liệu không hợp lệ"; return; }

                obj.Add(item);
            }

            Response.Redirect("category_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }
}

