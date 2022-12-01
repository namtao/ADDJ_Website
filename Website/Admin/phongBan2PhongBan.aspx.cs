using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class admin_phongBan2PhongBan : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
       

        lblMsg.Text = "";

        if (!IsPostBack)
        {
            BindDropDownList();
            BindGrid();
        }

        txtTenDangNhap.Attributes.Add("onkeypress", "return handleEnter('" + btAddPhongBan2PhongBan.ClientID + "', event)");
    }

    private void BindDropDownList()
    {
        var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", "", "Sort");

        ddlPhongBan.DataSource = lstPhongBan;
        ddlPhongBan.DataValueField = "ID";
        ddlPhongBan.DataTextField = "Name";
        ddlPhongBan.DataBind();


        if (Request.QueryString["PhongBanId"] != null && Request.QueryString["PhongBanId"] != string.Empty)
        {
            ddlPhongBan.SelectedValue = Request.QueryString["PhongBanId"];
        }
    }

    private void CallJavaScriptInUpdatePanel()
    {
        txtTenDangNhap.Attributes.Add("onkeypress", "return handleEnter('" + btAddPhongBan2PhongBan.ClientID + "', event)");

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadAutoComplete", "AutocompleteTenDangNhap();", true);        
    }

    private void BindGrid()
    {
        var lstPhongBan2PhongBan = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(int.Parse(ddlPhongBan.SelectedValue));

        if (lstPhongBan2PhongBan != null && lstPhongBan2PhongBan.Count > 0)
        {
            var item = lstPhongBan2PhongBan[0];

            var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetList();

            List<int> lstPhongBanDen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(item.PhongBanDen);

            var lstResult = lstPhongBan.Where(t => lstPhongBanDen.Contains(t.Id)).OrderBy(t=>t.Sort).ToList();

            grvView.DataSource = lstResult;
            grvView.DataBind();
        }
        else
        {
            grvView.DataSource = null;
            grvView.DataBind();
        }


        if(IsPostBack)
            CallJavaScriptInUpdatePanel();
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {
            
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
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

    protected void ddlPhongBan_Changed(object sender, EventArgs e)
    {
        BindGrid();   
    }

    protected void btLeaver_Click(object sender, EventArgs e)
    {
        var i = 0;
        var phongbanId = int.Parse(ddlPhongBan.SelectedValue);
        var obj = ServiceFactory.GetInstancePhongBan2PhongBan();

        List<PhongBan2PhongBanInfo> lstPhongBan2PhongBan = obj.GetListByPhongBanId(int.Parse(ddlPhongBan.SelectedValue));
        PhongBan2PhongBanInfo item = lstPhongBan2PhongBan[0];        
        List<int> lstPhongBanDen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(item.PhongBanDen);

        //List<int> lst = new List<int>();
        foreach (GridViewRow row in grvView.Rows)
        {
            var status = (CheckBox)row.FindControl("cbSelectAll");
            if (status.Checked)
            {
                int ID = int.Parse(grvView.DataKeys[i].Value.ToString());

                lstPhongBanDen.Remove(ID);
            }
            i++;
        }

        string fieldUpdate = string.Format("PhongBanDen='{0}'", Newtonsoft.Json.JsonConvert.SerializeObject(lstPhongBanDen));
        string whereClause = "PhongBanId = " + phongbanId;
        obj.UpdateDynamic(fieldUpdate, whereClause);


        System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa Phòng ban chuyển xử lý thành công','info', " + txtTenDangNhap.ClientID + ");", true);
        BindGrid();
    }

    protected void btAddPhong_Click(object sender, EventArgs e)
    {
        try
        {
            var i = 0;
            var phongbanId = int.Parse(ddlPhongBan.SelectedValue);
            var obj = ServiceFactory.GetInstancePhongBan2PhongBan();

            var lstPhongBanInPhongBan = obj.GetListByPhongBanId(phongbanId);

            var phongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id", string.Format("Name=N'{0}'", txtTenDangNhap.Text.Trim()), "");

            if (phongBan != null && phongBan.Count == 1)
            {
                if (lstPhongBanInPhongBan != null && lstPhongBanInPhongBan.Count > 0)
                {
                    var itemUpdate = lstPhongBanInPhongBan[0];
                    var lstPhongBanDenTemp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(itemUpdate.PhongBanDen);


                    if (!lstPhongBanDenTemp.Where(t => t == phongBan[0].Id).Any())
                    {
                        var item = new PhongBan2PhongBanInfo();
                        item.Id = lstPhongBanInPhongBan[0].Id;
                        item.PhongBanId = phongbanId;
                        lstPhongBanDenTemp.Add(phongBan[0].Id);
                        item.PhongBanDen = Newtonsoft.Json.JsonConvert.SerializeObject(lstPhongBanDenTemp);

                        obj.Update(item);
                    }
                }
                else
                {
                    var item = new PhongBan2PhongBanInfo();
                    item.PhongBanId = phongbanId;
                    var lst = new List<int>();
                    lst.Add(phongBan[0].Id);
                    item.PhongBanDen = Newtonsoft.Json.JsonConvert.SerializeObject(lst);

                    obj.Add(item);
                }
            }
            else
            {
                CallJavaScriptInUpdatePanel();
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Tên phòng ban không tồn tại','error', " + txtTenDangNhap.ClientID + ");", true);
                return;
            }

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm phòng ban chuyển xử lý thành công.','info', " + txtTenDangNhap.ClientID + ");", true);

            BindGrid();

            txtTenDangNhap.Text = "";
            txtListTenDangNhap.Text = "";
        }
        catch (Exception ex)
        {
            CallJavaScriptInUpdatePanel();
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm phòng ban chuyển xử lý có lỗi','info', "+ txtTenDangNhap.ClientID+");", true);
        }
    }    
}

