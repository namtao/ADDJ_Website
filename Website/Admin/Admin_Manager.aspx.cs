using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AIVietNam.Core;


using AIVietNam.Admin;

public partial class admin_admin_manager : System.Web.UI.Page
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

            
            
            BindGird();
        }

    }

    private void BindGird()
    {
        try
        {
            AdminImpl obj = new AdminImpl();
            grvView.DataSource = obj.GetList();
            grvView.DataBind();
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {        
        var x = e.Row.DataItemIndex;

        if (e.Row.DataItemIndex != -1)
        {
            if (e.Row.RowIndex % 2 != 0)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#DEEAF3'");
            }
            else
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
            }


            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();

            var status = (CheckBox)e.Row.Cells[4].FindControl("cbSelectAll");

            if (((AdminInfo)Session[Constant.SessionNameAccountAdmin]).ID == int.Parse(grvView.DataKeys[e.Row.RowIndex].Value.ToString()))
                status.Enabled = false;

            e.Row.Cells[1].Text = "<a href=\"admin_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[1].Text + "</a>";

            //var isLogin = (CheckBox)e.Row.Cells[3].FindControl("IsLogin");

            //int adminID = int.Parse(grvView.DataKeys[e.Row.RowIndex].Value.ToString());
            //AdminImpl obj = new AdminImpl();
            //AdminInfo item = obj.GetAdmin(obj.SelectOne(adminID))[0];

            //if (item.IsLogin==1)
            //{
            //    isLogin.Checked = true;
            //}
            //else
            //{
            //    isLogin.Checked = false;
            //}
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGird();
    }


    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            RowDataBound(e);
        }
        catch(Exception ex) {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
    }

    protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator().UserDelete)
        {
            Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
            return;
        }
        try
        {
            int i = 0;
            AdminImpl obj = new AdminImpl();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");

                if (status.Checked)
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    obj.Delete(adminID);
                }

                i++;
            }
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }
        BindGird();
    }

    protected void btThemMoi_Click(object sender, EventArgs e)
    {
        //if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
        //{
        //    Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
        //    return;
        //}

        Response.Redirect("admin_add.aspx", false);
    }

    protected void btUpdate_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator().UserEdit)
        {
           Response.Redirect(Utility.UrlRoot + Config.PathNotRight, false);
           return;
        }

        try
        {
            int i = 0;
            AdminImpl obj = new AdminImpl();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.Cells[3].FindControl("IsLogin");

                if (status.Checked)
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    AdminInfo item = obj.GetInfo(adminID);
                    item.IsLogin = 1;
                    obj.Update(item);
                }
                else
                {
                    int adminID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    AdminInfo item = obj.GetInfo(adminID);
                    item.IsLogin = 0;
                    obj.Update(item);
                }
                i++;
            }
        }
        catch {
            Response.Redirect(Utility.UrlRoot + Config.PathError, false);
            return;
        }

        BindGird();
    }
    
   
}
