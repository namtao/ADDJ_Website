using System;
using AIVietNam.Core;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Web;
using System.Data;
using AIVietNam.News.Entity;

public partial class admin_news_add : System.Web.UI.Page
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
            BindDropdownList();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }

    private void BindDropdownList()
    {
        var lstCatNews = ServiceFactory.GetInstanceCategory().GetList();
        ddlCategoryNews.DataTextField = "Name";
        ddlCategoryNews.DataValueField = "Id";
        ddlCategoryNews.DataSource = lstCatNews;
        ddlCategoryNews.DataBind();
    }

    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceNews();
            NewsInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData news_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Utility.UrlRoot + Config.PathError, false);
                return;
            }
            else
            {
                txtTitle.Text = item.Title.ToString();
                txtDescription.Text = item.Description.ToString();
                txtContent.Text = item.Content.ToString();
                imgOld.ImageUrl = "/Upload/News/" + item.ImagePath;
                imgOld.Height = 128;
                ddlCategoryNews.SelectedValue = item.CategoryId.ToString();
                //txtImagePath.Text = item.ImagePath.ToString();
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
            var obj = ServiceFactory.GetInstanceNews();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    NewsInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function news_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Utility.UrlRoot + Config.PathError, false);
                        return;
                    }

                    try
                    {
                        AdminInfo adminInfo = (AdminInfo)Session[Constant.SessionNameAccountAdmin];
                        item.LUser = adminInfo.Username;

                        item.CategoryId = Convert.ToInt32(ddlCategoryNews.SelectedValue);
                        item.Title = txtTitle.Text.Trim();
                        item.Description = txtDescription.Text.Trim();
                        item.Content = txtContent.Text.Trim();
                        
                        string strImage = UploadImage();
                        if (!string.IsNullOrEmpty(strImage))
                            item.ImagePath = strImage;
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
                var item = new NewsInfo();

                try
                {
                    AdminInfo adminInfo = (AdminInfo)Session[Constant.SessionNameAccountAdmin];

                    item.CategoryId = Convert.ToInt32(ddlCategoryNews.SelectedValue);
                    item.Title = txtTitle.Text.Trim();
                    item.Description = txtDescription.Text.Trim();
                    item.Content = txtContent.Text.Trim();

                    string strImage = UploadImage();
                    if (!string.IsNullOrEmpty(strImage))
                        item.ImagePath = strImage;
                    else
                        item.ImagePath = "NoImage.jpg";

                    item.LUser = adminInfo.Username;

                    //Upload Image
                }
                catch { lblMsg.Text = "Dữ liệu không hợp lệ"; return; }

                obj.Add(item);
            }

            Response.Redirect("news_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    private string UploadImage()
    {
        string path = Server.MapPath("~\\Upload\\News");
        return Utility.UploadFile(fImage, path, true, true, "*.*");
    }
}


