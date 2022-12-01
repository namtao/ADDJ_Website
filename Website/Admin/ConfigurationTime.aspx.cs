using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

public partial class ConfigurationTime : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindDropDownlist();
            BindData();
            //BindGrid(true, 1);
        }
    }


    private void BindDropDownlist()
    {
        for (int i = 1; i <= 24; i++)
        {
            ddlGioBatDau.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlGioKetThuc.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        for (int i = 2010; i < 2020; i++)
        {
            ddlNam.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        ddlNam.SelectedValue = DateTime.Now.Year.ToString();

        foreach (var i in Enum.GetValues(typeof(DayOfWeek)))
        {
            ddlNgayBatDauTuan.Items.Add(new ListItem(Enum.GetName(typeof(DayOfWeek), i), i.GetHashCode().ToString()));
        }
    }

    private void BindData()
    {
        var config = ConfigurationTimeImpl.ConfigTime[0];
        if (config != null)
        {
            ddlGioBatDau.SelectedValue = config.GioBatDau.ToString();
            ddlGioKetThuc.SelectedValue = config.GioKetThuc.ToString();
            txtTongSoNgayLamViec.Text = config.TongSoNgayTrenTuan.ToString();
            ddlNgayBatDauTuan.SelectedValue = config.NgayBatDauCuaTuan.ToString();
        }

        BindGrid();
    }

    protected string Bind_NgayThang(object id)
    {
        try
        {
            return DateTime.ParseExact(id.ToString(), "yyyyMMdd", new CultureInfo("vi-VN")).ToString("dd/MM/yyyy");
            //return Convert.ToDateTime(id, new CultureInfo("yyyyMMdd")).ToString("dd/MM/yyyy");
        }
        catch { return id.ToString(); }
    }

    private void BindGrid()
    {
        try
        {
            var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(ConfigurationTimeImpl.ConfigTime[0].NgayLe);

            if (lstNgayLe != null && lstNgayLe.Count > 0)
            {
                int ngay_dau_nam = Convert.ToInt32(ddlNam.SelectedValue + "0101");
                int ngay_cuoi_nam = Convert.ToInt32(ddlNam.SelectedValue + "1231");

                grvView.DataSource = lstNgayLe.Where(t => t.NgayThang >= ngay_dau_nam && t.NgayThang <= ngay_cuoi_nam).OrderBy(t => t.NgayThang);
                grvView.DataBind();
            }
            else
            {
                grvView.DataSource = new List<NgayLeInfo>();
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
                var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(ConfigurationTimeImpl.ConfigTime[0].NgayLe);

                var ngayThang = ((TextBox)grvView.FooterRow.FindControl("txtNgayThang")).Text;


                var ngayThangInt = Convert.ToInt32(Convert.ToDateTime(ngayThang, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
                if (!lstNgayLe.Where(t => t.NgayThang == ngayThangInt).Any())
                {
                    var name = ((TextBox)grvView.FooterRow.FindControl("txtName")).Text;
                    if (lstNgayLe == null)
                        lstNgayLe = new List<NgayLeInfo>();

                    lstNgayLe.Add(new NgayLeInfo() { Name = name, NgayThang = ngayThangInt });
                    ConfigurationTimeImpl.ConfigTime[0].NgayLe = Newtonsoft.Json.JsonConvert.SerializeObject(lstNgayLe);

                    ServiceFactory.ConfigurationTime().Update(ConfigurationTimeImpl.ConfigTime[0]);
                }
                BindGrid();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
            }
        }
        else if (e.CommandName.Equals("emptyInsert"))
        {
            try
            {

                var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(ConfigurationTimeImpl.ConfigTime[0].NgayLe);

                var ngayThang = ((TextBox)grvView.Controls[0].Controls[1].FindControl("txtNgayThang")).Text;


                var ngayThangInt = Convert.ToInt32(Convert.ToDateTime(ngayThang, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));

                if (lstNgayLe == null)
                    lstNgayLe = new List<NgayLeInfo>();

                if (!lstNgayLe.Where(t => t.NgayThang == ngayThangInt).Any())
                {
                    var name = ((TextBox)grvView.Controls[0].Controls[1].FindControl("txtName")).Text;

                    lstNgayLe.Add(new NgayLeInfo() { Name = name, NgayThang = ngayThangInt });
                    ConfigurationTimeImpl.ConfigTime[0].NgayLe = Newtonsoft.Json.JsonConvert.SerializeObject(lstNgayLe);

                    ServiceFactory.ConfigurationTime().Update(ConfigurationTimeImpl.ConfigTime[0]);
                }
                BindGrid();


                BindGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
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
            var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(ConfigurationTimeImpl.ConfigTime[0].NgayLe);

            var key = Convert.ToInt32(grvView.DataKeys[e.RowIndex].Values[0].ToString());

            var itemUpdate = lstNgayLe.Where(t => t.NgayThang == key);
            if (itemUpdate != null && itemUpdate.Any())
            {
                var ngayThang = Convert.ToString(((TextBox)grvView.Rows[e.RowIndex].FindControl("txtNgayThang")).Text);
                var ngayThangInt = Convert.ToInt32(Convert.ToDateTime(ngayThang, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));

                var name = Convert.ToString(((TextBox)grvView.Rows[e.RowIndex].FindControl("txtName")).Text);
                if (lstNgayLe.Remove(itemUpdate.Single()))
                {
                    lstNgayLe.Add(new NgayLeInfo() { Name = name, NgayThang = ngayThangInt });

                    ConfigurationTimeImpl.ConfigTime[0].NgayLe = Newtonsoft.Json.JsonConvert.SerializeObject(lstNgayLe);

                    ServiceFactory.ConfigurationTime().Update(ConfigurationTimeImpl.ConfigTime[0]);
                }
            }

            grvView.EditIndex = -1;
            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
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
            var lstNgayLe = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NgayLeInfo>>(ConfigurationTimeImpl.ConfigTime[0].NgayLe);
            var key = Convert.ToInt32(grvView.DataKeys[e.RowIndex].Values[0].ToString());

            var itemUpdate = lstNgayLe.Where(t => t.NgayThang == key);
            if (itemUpdate != null && itemUpdate.Any())
            {
                if (lstNgayLe.Remove(itemUpdate.Single()))
                {
                    ConfigurationTimeImpl.ConfigTime[0].NgayLe = Newtonsoft.Json.JsonConvert.SerializeObject(lstNgayLe);

                    ServiceFactory.ConfigurationTime().Update(ConfigurationTimeImpl.ConfigTime[0]);
                }
            }

            BindGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
        }
    }

    protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void linkbtnCapNhat_Click(object sender, EventArgs e)
    {
        var config = ConfigurationTimeImpl.ConfigTime[0];

        try
        {
            config.GioBatDau = Convert.ToInt32(ddlGioBatDau.SelectedValue);
            config.GioKetThuc = Convert.ToInt32(ddlGioKetThuc.SelectedValue);
            config.NgayBatDauCuaTuan = Convert.ToInt32(ddlNgayBatDauTuan.SelectedValue);
            config.TongSoNgayTrenTuan = Convert.ToDecimal(txtTongSoNgayLamViec.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Dữ liệu chưa hợp lệ.','error');", true);
        }

        try
        {
            ServiceFactory.ConfigurationTime().Update(config);

            ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Lưu cấu hình thành công.','info');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePane, UpdatePane.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
        }
    }
}

