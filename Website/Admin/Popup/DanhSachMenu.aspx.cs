using ADDJ.Admin;
using ADDJ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website
{
    public partial class DanhSachMenu : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            cboLevel.AutoPostBack = true;
            cboParentId.AutoPostBack = true;
            cboLevel.SelectedIndexChanged += cboLevel_SelectedIndexChanged;
            cboParentId.SelectedIndexChanged += cboLevel_SelectedIndexChanged;

            if (!IsPostBack)
            {
                grvView.PageSize = 100;
                BindParentValues(cboParentId);
                BindGrid();
                if (!string.IsNullOrEmpty(Request.QueryString["IdGiaTri"]))
                {
                    string[] ids = Request.QueryString["IdGiaTri"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (ids.Length > 0) hdfMenu.Value = string.Join(",", ids);
                }
            }
        }

        protected void cboLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected string GetMenuType(object obj)
        {
            if (obj.ToString().Equals("1")) return "Menu";
            return "Separator";
        }

        private void BindParentValues(DropDownList obj)
        {
            List<MenuInfo> menuObj = new MenuImpl().GetList();
            IEnumerable<MenuInfo> root = menuObj.Where(t => t.ParentID == 0).OrderBy(t => t.STT);
            obj.DataSource = root;
            obj.DataTextField = "Name";
            obj.DataValueField = "Id";
            obj.DataBind();
            obj.Items.Insert(0, new ListItem("- Tất cả -", "0"));
        }
        private void BindGrid()
        {
            int level = Convert.ToInt32(cboLevel.SelectedValue);
            if (level == 0) level = 99;
            int parentId = Convert.ToInt32(cboParentId.SelectedValue);
            try
            {
                var menuObj = new MenuImpl().GetList();
                if (menuObj != null && menuObj.Count > 0)
                {
                    //ltrMsg.Text = string.Format("<span style=\"color:red;\">Tổng số có {0} menu</span>", menuObj.Count);
                    //HtmlUtility.ShowMsg(ltrMsg, "style=\"margin:0px; margin-top:10px;\"", "Số lượng bản gi được tìm thấy là: " + menuObj.Count);
                    var lstMenus = new List<MenuInfo>();
                    var root = menuObj.Where(t => t.ParentID == parentId).OrderBy(t => t.STT);
                    foreach (var item in root)
                    {
                        lstMenus.Add(item);
                        if (level > 1)
                        {
                            var menuLevel1 = menuObj.Where(t => t.ParentID == item.ID).OrderBy(t => t.STT);
                            if (menuLevel1 != null)
                            {
                                foreach (var itemChild in menuLevel1)
                                {
                                    itemChild.Name = "_" + itemChild.Name;
                                    lstMenus.Add(itemChild);
                                }
                            }
                        }
                    }

                    grvView.DataSource = lstMenus;
                    grvView.DataBind();
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
            }
        }

        private void RowDataBound(GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex != -1)
            {
                //e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
                if (e.Row.Cells[1].Text.StartsWith("_"))
                    e.Row.Cells[1].Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"Menu_Add.aspx?Id=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text.Replace("_", "") + "</a>";
                else
                    e.Row.Cells[1].Text = "<a href=\"Menu_Add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text + "</a>";
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            grvView.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                RowDataBound(e);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }

        protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }
    }
}