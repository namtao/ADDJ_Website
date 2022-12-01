using System;
using ADDJ.Core;
using System.Collections.Generic;
using Website.AppCode;
using System.Linq;
using System.IO;
using ADDJ.Admin;
using System.Web.UI.WebControls;

namespace Website.Admin
{
    public partial class Menu_Add : Website.AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mặc định kểu thêm menu
                liJs.Text = string.Format("var typeId = {0};", 1);
                BindDropDownList();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    BindEdit();
                }
                else
                {
                    txtSTT.Text = ServiceFactory.GetInstanceMenu().GetSTTParent().ToString();
                }
            }
        }

        private void BindDropDownList()
        {
            ddlMenuType.Items.Add(new System.Web.UI.WebControls.ListItem("Menu", "1"));
            ddlMenuType.Items.Add(new System.Web.UI.WebControls.ListItem("Separator", "2"));

            List<MenuInfo> obj = ServiceFactory.GetInstanceMenu().GetList();
            List<MenuInfo> lstObj = obj.Where(t => t.ParentID == 0).OrderBy(t => t.STT).ToList();
            lstObj.Insert(0, new MenuInfo() { ID = 0, Name = "Là cha" });
            ddlParrent.DataSource = lstObj;
            ddlParrent.DataValueField = "ID";
            ddlParrent.DataTextField = "Name";
            ddlParrent.DataBind();

            string path = Server.MapPath("~");
            string[] arrFile = Directory.GetFiles(path, "*.aspx", SearchOption.AllDirectories);
            List<string> lstFile = new List<string>();

            foreach (string file in arrFile)
            {
                lstFile.Add("/" + file.Replace(path, "").Replace("\\", "/"));
            }

            ddlLink.DataSource = lstFile;
            ddlLink.DataBind();

            List<MenuInfo> menuObj = ServiceFactory.GetInstanceMenu().GetList();
            if (menuObj != null && menuObj.Count > 0)
            {
                var menuObj2 = new List<MenuInfo>();
                var parrent = menuObj.Where(t => t.ParentID == 0).OrderBy(t => t.STT);
                foreach (var item in parrent)
                {
                    menuObj2.Add(item);
                    var child = menuObj.Where(t => t.ParentID == item.ID).OrderBy(t => t.STT);
                    if (child != null)
                    {
                        foreach (var itemChild in child)
                        {
                            itemChild.Name = "|— " + itemChild.Name;
                            menuObj2.Add(itemChild);
                        }
                    }
                }

                ddlMenu.DataSource = menuObj2;
                ddlMenu.DataValueField = "ID";
                ddlMenu.DataTextField = "Name";
                ddlMenu.DataBind();
            }
        }

        protected string Script = string.Empty;

        private void BindEdit()
        {

            try
            {
                MenuImpl obj = ServiceFactory.GetInstanceMenu();
                MenuInfo item = obj.GetInfo(int.Parse(Request.QueryString["Id"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData menu_add get NullId " + Request.QueryString["Id"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                else
                {
                    txtSTT.Text = item.STT.ToString();
                    ddlParrent.SelectedValue = item.ParentID.ToString();

                    // Edited by	: Dao Van Duong
                    // Datetime		: 12.8.2016 23:41
                    // Note			: Fix vấn đề chữ hoa, chữ thường
                    txtLink.Text = item.Link;
                    ddlLink.SelectedValue = item.Link;
                    try
                    {
                        ListItem objSelected = ddlLink.Items.Cast<ListItem>().SingleOrDefault(v => v.Value.ToLower() == item.Link.ToLower());
                        if (objSelected != null)
                        {
                            ddlLink.SelectedValue = objSelected.Value;
                            txtLink.Text = objSelected.Value;
                        }
                    }
                    catch { } // Không làm gì cả

                    txtName.Text = item.Name.ToString();
                    txtName2.Text = item.Name2.ToString();
                    txtName3.Text = item.Name3.ToString();
                    chkDisplay.Checked = item.Display == 1;
                    ddlMenuType.SelectedValue = item.MenuType.ToString();
                    liJs.Text = string.Format("var typeId = {0}", item.MenuType);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
  

        protected void ddlLink_Changed(object sender, EventArgs e)
        {
            txtLink.Text = ddlLink.SelectedItem.Text;
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
                MenuImpl obj = ServiceFactory.GetInstanceMenu();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    try
                    {
                        int idEdit = int.Parse(Request.QueryString["ID"]);
                        MenuInfo item = obj.GetInfo(idEdit);

                        if (item == null)
                        {
                            Utility.LogEvent("Function menu_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                            Response.Redirect(Config.PathError, false);
                            return;
                        }

                        try
                        {
                            if (ddlMenuType.SelectedValue.Equals("1"))
                            {
                                item.STT = Convert.ToInt32(txtSTT.Text.Trim());
                                item.ParentID = Convert.ToInt32(ddlParrent.Text.Trim());
                                item.Name = txtName.Text.Trim();
                                item.Name2 = txtName2.Text.Trim();
                                item.Name3 = txtName3.Text.Trim();
                                //ddlLink.SelectedValue = item.Link;
                                item.Link = txtLink.Text;
                                item.Display = chkDisplay.Checked ? 1 : 0;
                                item.MenuType = 1;
                            }
                            else
                            {
                                MenuInfo menuSelect = ServiceFactory.GetInstanceMenu().GetInfo(Convert.ToInt32(ddlMenu.SelectedValue));

                                item.Name = txtNameSeparator.Text.Trim();
                                item.Name2 = item.Name3 = item.Name;
                                item.Link = txtLink.Text;
                                item.Display = 1;
                                item.MenuType = 2;
                                item.ParentID = menuSelect.ParentID;
                                item.STT = menuSelect.STT;
                            }
                        }
                        catch { lblMsg.Text = "Dữ liệu không hợp lệ"; return; }

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
                    MenuInfo item = new MenuInfo();

                    try
                    {
                        if (ddlMenuType.SelectedValue.Equals("1"))
                        {
                            item.STT = Convert.ToInt32(txtSTT.Text.Trim());
                            item.ParentID = Convert.ToInt32(ddlParrent.Text.Trim());
                            item.Name = txtName.Text.Trim();
                            item.Name2 = txtName2.Text.Trim();
                            item.Name3 = txtName3.Text.Trim();
                            //ddlLink.SelectedValue = item.Link;
                            item.Link = txtLink.Text;
                            item.Display = chkDisplay.Checked ? 1 : 0;
                            item.MenuType = 1;
                        }
                        else
                        {

                            MenuInfo menuSelect = ServiceFactory.GetInstanceMenu().GetInfo(Convert.ToInt32(ddlMenu.SelectedValue));

                            item.Name = txtNameSeparator.Text.Trim();
                            item.Name2 = item.Name3 = item.Name;
                            item.Link = txtLink.Text;
                            item.Display = 1;
                            item.MenuType = 2;
                            item.ParentID = menuSelect.ParentID;
                            item.STT = menuSelect.STT;
                        }
                    }
                    catch { lblMsg.Text = "Dữ liệu không hợp lệ"; return; }

                    obj.Add(item);
                }

                //Refresh Cache
                MenuImpl.ListMenu = ServiceFactory.GetInstanceMenu().GetList();

                Response.Redirect("Menu_Manager.aspx", false);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }
    }

}