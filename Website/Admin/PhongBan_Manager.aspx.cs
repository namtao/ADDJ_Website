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
using System.Collections.Generic;
using System.Web;

public partial class Admin_PhongBan_Manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ddlDonVi.AutoPostBack = true;
        ddlDonVi.SelectedIndexChanged += DdlDonVi_SelectedIndexChanged;

        if (!IsPostBack)
        {
            GrvDatas.PageSize = Config.RecordPerPage;
            BindDonVi(Request.QueryString["ParentId"]);
            BindDoiTac(Request.QueryString["DoiTacid"]);
            BindLoaiPhongBan();
            BindGrid();
        }

        if (!Permission.Other2) btAddPhongBanToPhongBan.Visible = false;
        if (!Permission.UserEdit) liUpdate.Visible = false;
        if (!Permission.UserDelete) liDelete.Visible = false;
    }

    private void DdlDonVi_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDoiTac(ddlDonVi.SelectedValue);
    }

    protected string HanhDong(object id)
    {
        string returnURL = Request.Url.AbsolutePath + string.Format("?DoiTacId={0}&NameFilter={1}&LoaiPhongBanId={2}&PIndex={3}", ddlDonVi.SelectedValue, txtKeyword.Text, ddlLoaiPhongBan.SelectedValue, GrvDatas.PageIndex);
        returnURL = HttpUtility.UrlEncode(returnURL);

        string strReturn = string.Empty;

        if (Permission.UserEdit)
        {
            strReturn += string.Format("<a href='PhongBan_Add.aspx?Id={0}&ReturnUrl={1}'>Sửa</a> <br />", id, returnURL);
        }
        else
        {
            strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Sửa</a> <br />");
        }

        if (Permission.Other2)
        {
            strReturn += string.Format("<a target='_blank' href='PhongBan_User_Add.aspx?Id={0}&ReturnUrl={1}'>Thêm, xóa user</a> <br /> ", id, returnURL);
            strReturn += string.Format("<a target='_blank' href='PhongBan2PhongBan.aspx?PhongBanId={0}&ReturnUrl={1}'>Thêm, xóa Phòng ban chuyển xử lý</a> <br /> ", id, returnURL);
        }
        else
        {
            strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Thêm, xóa user</a> <br />");
            strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Thêm, xóa Phòng ban chuyển xử lý</a> <br />");
        }

        if (Permission.Other1)
        {
            strReturn += string.Format("<a target='_blank' href='PhanQuyenPhongBan.aspx?PhongBanId={0}&ReturnUrl={1}'>Phân quyền</a> ", id, returnURL);
        }
        else
        {
            strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Phân quyền</a> <br />");
        }
        return strReturn;
    }

    protected string BindDoiTac(object doitacId)
    {
        try
        {
            return DoiTacImpl.ListDoiTac.Where(t => t.Id == Convert.ToInt32(doitacId)).Single().MaDoiTac;
        }
        catch
        {
            return doitacId.ToString();
        }
    }

    protected string BindHTTN(object objHTTN)
    {
        try
        {
            return Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), objHTTN).Replace("_", " ");
        }
        catch
        {
            return string.Empty;
        }
    }

    protected string BindCountNguoiDung(object id, object countUser)
    {
        return string.Format("{0} (<a href='nguoiSuDung_manager.aspx?PhongBanId={1}'>Xem danh sách</a>)", countUser, id);
    }
    private void BindDonVi(string seleted)
    {
        AdminInfo userInfo = LoginAdmin.AdminLogin();
        int parentId = 0;

        if (userInfo.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET || userInfo.Username.ToLower() == "Administrator".ToLower()) parentId = 0;
        else parentId = userInfo.DoiTacId;

        DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, "DoiTac_TimKiem", string.Empty, parentId, 2).Tables[0];
        foreach (DataRow row in tbl.Rows)
        {
            row["MaDoiTac"] = Website.AppCode.Common.GiveEName(row["MaDoiTac"], (int)row["Level"], "+ ");
        }

        ddlDonVi.DataSource = tbl;
        ddlDonVi.DataValueField = "ID";
        ddlDonVi.DataTextField = "MaDoiTac";
        ddlDonVi.DataBind();

        // ddlDonVi.Items.Insert(0, new ListItem("-- Chọn đơn vị --", "0"));
        ddlDonVi.SelectedValue = Request.QueryString["ParentId"];
    }

    private void BindDoiTac(string seleted)
    {
        int parentId = Convert.ToInt32(ddlDonVi.SelectedValue);
        if (parentId > 0)
        {
            AdminInfo userInfo = LoginAdmin.AdminLogin();
            DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, "DoiTac_TimKiem", string.Empty, parentId, 2).Tables[0];
            int rowIndex = 0;
            foreach (DataRow row in tbl.Rows)
            {
                if (rowIndex != 0) row["MaDoiTac"] = Website.AppCode.Common.GiveEName(row["MaDoiTac"], (int)row["Level"], "+ ");
                else
                {
                    row["MaDoiTac"] = "-- Tất cả --";
                }
                rowIndex += 1;
            }
            ddlDonVi2.DataSource = tbl;
            ddlDonVi2.DataBind();
        }
        else
        {
            ddlDonVi2.Items.Clear();
            ddlDonVi2.Items.Insert(0, new ListItem("-- Chọn đối tác --", "0"));
        }
        ddlDonVi2.SelectedValue = Request.QueryString["DoiTacId"];
    }
    private void BinDonVi(string selected)
    {


        txtKeyword.Text = Request.QueryString["NameFilter"];
        ddlDonVi.SelectedValue = Request.QueryString["DoiTacId"];
    }
    private void BindLoaiPhongBan()
    {
        txtKeyword.Text = Request.QueryString["NameFilter"];

        List<PhongBanInfo> lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id, Name", string.Empty, "Sort");
        ddlPhongBanSelect.DataSource = lstPhongBan;
        ddlPhongBanSelect.DataValueField = "ID";
        ddlPhongBanSelect.DataTextField = "Name";
        ddlPhongBanSelect.DataBind();

        // Đưa ra danh sách "Loại phòng ban"
        ddlLoaiPhongBan.DataSource = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        ddlLoaiPhongBan.DataValueField = "ID";
        ddlLoaiPhongBan.DataTextField = "Name";
        ddlLoaiPhongBan.DataBind();
        ddlLoaiPhongBan.Items.Insert(0, new ListItem("-- Loại phòng ban --", "0"));

        // Phòng ban
        ddlLoaiPhongBan.SelectedValue = Request.QueryString["LoaiPhongBanId"];
        GrvDatas.PageIndex = ConvertUtility.ToInt32(Request.QueryString["PIndex"], 0);
    }

    private void BindGrid()
    {
        try
        {
            // Tìm kiếm đơn vị lọc
            int donViId = 0;
            if (ddlDonVi2.SelectedValue != "0") donViId = Convert.ToInt32(ddlDonVi2.SelectedValue);
            else if (ddlDonVi.SelectedValue != "0") donViId = Convert.ToInt32(ddlDonVi.SelectedValue);

            string selectClause = string.Empty;
            selectClause = "*,(SELECT count(*) FROM PhongBan_User WHERE PhongBanId = PhongBan.Id) CountUser";

            string whereClause = "1 = 1 ";
            whereClause += string.Format(" AND (DoiTacId = {0} or DoiTacId in ( SELECT id FROM DoiTac WHERE DonViTrucThuoc = {0} or Id in ( SELECT Id FROM DoiTac WHERE DonViTrucThuoc in (  SELECT id FROM DoiTac WHERE DonViTrucThuoc = {0} ) ) ))", donViId);

            if (!txtKeyword.Text.Trim().Equals(""))
            {
                whereClause += string.Format(" AND Name like N'%{0}%'", txtKeyword.Text.Trim());
            }

            if (!ddlLoaiPhongBan.SelectedValue.ToString().Equals("0"))
            {
                whereClause += string.Format(" AND LoaiPhongBanId = {0}", ddlLoaiPhongBan.SelectedValue);
            }

            string orderbyClause = "Sort";

            List<PhongBanInfo> phongBanObj = ServiceFactory.GetInstancePhongBan().GetListDynamic(selectClause, whereClause, orderbyClause);
            if (phongBanObj != null && phongBanObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + phongBanObj.Count + " phòng ban được tìm thấy.</font>";
                GrvDatas.DataSource = phongBanObj;
                GrvDatas.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có phòng ban được tìm thấy.</font>";
                GrvDatas.DataSource = null;
                GrvDatas.DataBind();
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        finally
        {
            if (IsPostBack)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePnl, updatePnl.GetType(), "onload", "LoadJS();", true);
            }
        }
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
        GrvDatas.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void GrvDatas_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void GrvDatas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PageIndexChanging(e);
    }
 

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        Response.Redirect("PhongBan_Manager.aspx");
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnOkay_Click(object sender, EventArgs e)
    {
        if (!Permission.Other2)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            PhongBan2PhongBanImpl obj = ServiceFactory.GetInstancePhongBan2PhongBan();

            int phongbanId = int.Parse(ddlPhongBanSelect.SelectedValue);

            List<PhongBan2PhongBanInfo> lstPhongBan2PhongBan = obj.GetListDynamic("", "PhongBanId=" + ddlPhongBanSelect.SelectedValue, "");
            bool IsUpdate = false;

            List<int> lstPhongBanDen = new List<int>();
            if (lstPhongBan2PhongBan != null && lstPhongBan2PhongBan.Count > 0)
            {
                IsUpdate = true;
                lstPhongBanDen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(lstPhongBan2PhongBan[0].PhongBanDen);
            }

            int i = 0;
            foreach (GridViewRow row in GrvDatas.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int ID = int.Parse(GrvDatas.DataKeys[i].Value.ToString());

                    if (!lstPhongBanDen.Contains(ID))
                    {
                        lstPhongBanDen.Add(ID);
                    }
                }
                i++;
            }

            if (IsUpdate)
            {
                PhongBan2PhongBanInfo itemUpdate = lstPhongBan2PhongBan[0];
                itemUpdate.PhongBanDen = Newtonsoft.Json.JsonConvert.SerializeObject(lstPhongBanDen);

                obj.Update(itemUpdate);
            }
            else
            {
                PhongBan2PhongBanInfo item = new PhongBan2PhongBanInfo();
                item.PhongBanId = int.Parse(ddlPhongBanSelect.SelectedValue);
                item.PhongBanDen = Newtonsoft.Json.JsonConvert.SerializeObject(lstPhongBanDen);
                obj.Add(item);
            }

            BindGrid();

            ScriptManager.RegisterClientScriptBlock(updatePnl, updatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban chuyển xử lý thành công','info');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(updatePnl, updatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban chuyển xử lý có lỗi','error');", true);
        }
    }

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        string returnURL = Request.Url.AbsolutePath + string.Format("?DoiTacId={0}&NameFilter={1}&LoaiPhongBanId={2}&PIndex={3}", ddlDonVi.SelectedValue, txtKeyword.Text, ddlLoaiPhongBan.SelectedValue, GrvDatas.PageIndex);
        returnURL = HttpUtility.UrlEncode(returnURL);
        Response.Redirect("PhongBan_Add.aspx?ReturnUrl=" + returnURL, false);
    }

    protected void linkbtnDelete_Click(object sender, EventArgs e)
    {
        if (!Permission.UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            PhongBanImpl obj = ServiceFactory.GetInstancePhongBan();
            foreach (GridViewRow row in GrvDatas.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int ID = int.Parse(GrvDatas.DataKeys[i].Value.ToString());
                    obj.Delete(ID);
                }
                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(updatePnl, updatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid();
    }
}

