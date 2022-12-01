using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Web;

public partial class ConfigurationSystem : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDownlist();
            BindData();
        }
    }


    private void BindDropDownlist()
    {
        
    }

    private void BindData()
    {
        BindGrid();
    }

    private void BindGrid()
    {
        try
        {
            var lstNgayLe = ServiceFactory.GetInstanceConfigurationSystem().GetInfo(1);

            var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(lstNgayLe.Variable);

            if (dic != null && dic.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("key");
                dt.Columns.Add("value");
                DataRow dr;

                foreach (var item in dic)
                {
                    dr = dt.NewRow();
                    dr[0] = item.Key;
                    dr[1] = item.Value;
                    dt.Rows.Add(dr);
                }
                grvView.DataSource = dt;
                grvView.DataBind();
            }
            else
            {
                grvView.DataSource = new Dictionary<string, string>();
                grvView.DataBind();
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
        if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
        }
    }
    protected void grvView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.Equals("Insert"))
        {
            try
            {
                var item = ServiceFactory.GetInstanceConfigurationSystem().GetInfo(1);

                var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Variable);
                if (dic == null)
                    dic = new Dictionary<string, string>();

                var ngayThang = ((TextBox)grvView.FooterRow.FindControl("txtNgayThang")).Text;
                var name = ((TextBox)grvView.FooterRow.FindControl("txtName")).Text;

                if (!dic.ContainsKey(name))
                {
                    dic.Add(name, ngayThang);

                    item.Variable = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
                    ServiceFactory.GetInstanceConfigurationSystem().Update(item);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Đã tồn tại tên biến trong hệ thống.','error');", true);
                }

                BindGrid();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
            }
        }
        else if (e.CommandName.Equals("emptyInsert"))
        {
            try
            {
                var item = ServiceFactory.GetInstanceConfigurationSystem().GetInfo(1);

                var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Variable);
                if (dic == null)
                    dic = new Dictionary<string, string>();

                var ngayThang = ((TextBox)grvView.Controls[0].Controls[1].FindControl("txtNgayThang")).Text;
                var name = ((TextBox)grvView.Controls[0].Controls[1].FindControl("txtName")).Text;

                if (!dic.ContainsKey(name))
                {
                    dic.Add(name, ngayThang);

                    item.Variable = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
                    ServiceFactory.GetInstanceConfigurationSystem().Update(item);
                }
                else {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Đã tồn tại tên biến trong hệ thống.','error');", true);
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
            }
        }
    }
    protected void grvView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvView.EditIndex = e.NewEditIndex;
        BindGrid();
    }
    protected void grvView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            var item = ServiceFactory.GetInstanceConfigurationSystem().GetInfo(1);

            var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Variable);
            if (dic == null)
                dic = new Dictionary<string, string>();

            var ngayThang = ((TextBox)grvView.Rows[e.RowIndex].FindControl("txtNgayThang")).Text;
            var key = grvView.DataKeys[e.RowIndex].Values[0].ToString();

            if (dic.ContainsKey(key))
            {
                dic[key] = ngayThang;

                item.Variable = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
                ServiceFactory.GetInstanceConfigurationSystem().Update(item);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không tồn tại tên biến trong hệ thống.','error');", true);
            }
            Config.UpdateConfig();
            
            grvView.EditIndex = -1;
            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
        }
    }
    protected void grvView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvView.EditIndex = -1;
        BindGrid();
    }
    protected void grvView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            var item = ServiceFactory.GetInstanceConfigurationSystem().GetInfo(1);

            var dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Variable);
            if (dic == null)
                return;

            var key = grvView.DataKeys[e.RowIndex].Values[0].ToString();

            if (dic.ContainsKey(key))
            {
                dic.Remove(key);

                item.Variable = Newtonsoft.Json.JsonConvert.SerializeObject(dic);
                ServiceFactory.GetInstanceConfigurationSystem().Update(item);
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không tồn tại tên biến trong hệ thống.','error');", true);

            
            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
        }
    }

    protected void linkbtnUpdate_Click(object sender, EventArgs e)
    {
        Config.UpdateConfig();
    }
}

