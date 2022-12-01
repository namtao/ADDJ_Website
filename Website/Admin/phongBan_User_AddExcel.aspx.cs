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
using System.IO;
using System.Transactions;

public partial class phongBan_User_AddExcel : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbMessage.Text = "";

        if (!IsPostBack)
        {
            BindDropDownlist();

            grvView.PageSize = Config.RecordPerPage;
            BindGrid(true);            
        }
    }

    protected string HanhDong(object id)
    {
        return string.Format("<a href='phongBan_add.aspx?ID={0}'>Sửa</a> | <a href='phongBan_User_add.aspx?ID={0}'>Thêm, xóa user</a> | <a href='phongBan2PhongBan.aspx?PhongBanId={0}'>Thêm, xóa Phòng ban chuyển xử lý</a> | <a href='PhanQuyenPhongBan.aspx?PhongBanId={0}'>Phân quyền</a>", id);
    }

    protected string BindDoiTac(object doitacId)
    {
        return DoiTacImpl.ListDoiTac.Where(t => t.Id == Convert.ToInt32(doitacId)).Single().MaDoiTac;
    }

    protected string BindCountNguoiDung(object id, object countUser)
    {
        return string.Format("{0} (<a href='nguoiSuDung_manager.aspx?PhongBanId={1}'>Xem danh sách</a>)", countUser, id);
    }

    private void BindDropDownlist()
    {
        var admin = LoginAdmin.AdminLogin();
        string whereClause = "DoiTacId=" + admin.DoiTacId;
        whereClause += " or DoiTacId in (select Id from DoiTac where DonViTrucThuoc = " + admin.DoiTacId + ")";
        whereClause += " or DoiTacId in (select Id from DoiTac where DonViTrucThuoc in (select Id from DoiTac where DonViTrucThuoc = " + admin.DoiTacId + "))";

        var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", whereClause, "DoiTacId");
        ddlPhongBan.DataSource = lstPhongBan;
        ddlPhongBan.DataValueField = "ID";
        ddlPhongBan.DataTextField = "Name";
        ddlPhongBan.DataBind();
    }

    private void BindGrid(bool isClearFilter)
    {
        try
        {
            //if (isClearFilter)
            //{
            //    txtKeyword.Text = "";
            //    ddlDonVi.SelectedIndex = 0;
            //}
            //string selectClause = string.Empty;
            //selectClause = "*,(select count(*) from PhongBan_User where PhongBanId = PhongBan.Id) CountUser";

            //string whereClause = "1 = 1 ";
            //whereClause += string.Format(" AND (DoiTacId = {0} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {0} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {0} ) ) ))", ddlDonVi.SelectedValue);

            //if (!txtKeyword.Text.Trim().Equals(""))
            //{
            //    whereClause += string.Format(" AND Name like N'%{0}%'",txtKeyword.Text.Trim());
            //}

            //if (!ddlLoaiPhongBan.SelectedValue.ToString().Equals("0"))
            //{
            //    whereClause += string.Format(" AND LoaiPhongBanId = {0}", ddlLoaiPhongBan.SelectedValue);
            //}

            //string orderbyClause = string.Empty;

            //var phongBanObj = ServiceFactory.GetInstancePhongBan().GetListDynamic(selectClause, whereClause, orderbyClause);
            //if (phongBanObj != null && phongBanObj.Count > 0)
            //{
            //    ltThongBao.Text = "<font color='red'>Có " + phongBanObj.Count + " phòng ban được tìm thấy.</font>";
            //    grvView.DataSource = phongBanObj;
            //    grvView.DataBind();
            //}
            //else
            //{
            //    ltThongBao.Text = "<font color='red'>Không có phòng ban được tìm thấy.</font>";
            //    grvView.DataSource = null;
            //    grvView.DataBind();
            //}

            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
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
        grvView.PageIndex = e.NewPageIndex;
        BindGrid(false);
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
 

    DataTable dt;
    protected void btLayDanhSach_Click(object sender, EventArgs e)
    {
        string pathUpload = Server.MapPath("UploadTransaction");

        try
        {
            string fileName = Utility.UploadFile(fUpload, pathUpload, true, true, Constant.FILE_UPLOAD_EXTENSIVE);
            if (string.IsNullOrEmpty(fileName))
            {
                lbMessage.Text = "Bạn chưa chọn file tạo người dùng.";
                return;
            }
            dt = Utility.ExcelToDataTable(Path.Combine(pathUpload, fileName));

            if (dt.Rows.Count > 1000)
            {
                lbMessage.Text = "Số lượng người dùng trong file quá lớn 1000 người. Bạn cần cắt file ra mỗi file 400 người dùng để đảm bảo việc tạo người dùng.";
                return;
            }

            if (dt.Columns["Username"] == null || dt.Columns["Fullname"] == null || dt.Columns["Phone"] == null || dt.Columns["Đơn vị"] == null)
            {
                lbMessage.Text = "Cấu trúc file không hợp lệ. Xem lại file tạo ngươi dùng<br />File dữ liệu phải có các cột: \"Username\", \"Fullname\", \"Phone\", \"Đơn vị\"";
                return;
            }

            dt.Columns.Add("UserId");
            dt.Columns.Add("PhongBanID");
            dt.Columns.Add("PhongBanName");

            var lstDoiTac = ServiceFactory.GetInstanceDoiTac().GetList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (string.IsNullOrEmpty(dr["Username"].ToString().Trim()))
                {
                    dt.Rows.Remove(dr);
                    i--;
                    continue;
                }

                var userId = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(dr["Username"].ToString().Trim());
                if (userId == 0)
                {
                    lbMessage.Text = "Người sử dụng \"" + dr["Username"].ToString() + "\" không tồn tại trên hệ thống.";
                    return;
                }

                var TenPhongBan = dr[6].ToString().Trim();
                dr["UserId"] = userId;
                if (string.IsNullOrEmpty(TenPhongBan))
                {
                    dr["PhongBanID"] = ddlPhongBan.SelectedValue;
                    dr["PhongBanName"] = ddlPhongBan.SelectedItem.Text;
                }
                else
                {
                    var item = ServiceFactory.GetInstancePhongBan().CheckExistsPhongBan(TenPhongBan);
                    if (item == null)
                    {
                        lbMessage.Text = "Phòng ban \"" + TenPhongBan + "\" không tồn tại trên hệ thống.";
                        return;
                    }
                    dr["PhongBanID"] = item.Id;
                    dr["PhongBanName"] = item.Name;
                }

                
                
            }

            tableResult.Visible = true;
            btLayDanhSach.Enabled = false;
            btLayDanhSach.Visible = false;
            ddlPhongBan.Enabled = false;
            btHuy.Visible = true;

            ltThongBao.Text = "Có " + dt.Rows.Count + " người sử dụng hợp lệ.";

            ViewState["TableImport"] = dt;
            grvView.DataSource = ViewState["TableImport"];

            grvView.DataBind();
        }
        catch (Exception ex)
        { }
    }

    protected void btHuy_Click(object sender, EventArgs e)
    {
        ViewState["TableImport"] = null;
        grvView.DataSource = null;
        grvView.DataBind();

        btHuy.Visible = false;
        tableResult.Visible = false;
        btLayDanhSach.Enabled = true;
        btLayDanhSach.Visible = true;
        ddlPhongBan.Enabled = true;
    }

    protected void linkbtnCapNhat_Click(object sender, EventArgs e)
    {
        var dt = ViewState["TableImport"] as DataTable;
        //var lstUserInPhongBan = ServiceFactory.GetInstancePhongBan_User().GetListDynamic("Id,NguoiSuDungId", "PhongBanId=" + ddlPhongBan.SelectedValue, "");
        bool flag = true;
        using (TransactionScope scope = new TransactionScope())
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    var checkExists = ServiceFactory.GetInstancePhongBan_User().GetListDynamic("", "NguoiSuDungId=" + row["UserId"], "");
                    if (checkExists != null && checkExists.Count == 0)
                    {
                        ServiceFactory.GetInstancePhongBan_User().Add(new PhongBan_UserInfo() { NguoiSuDungId = Convert.ToInt32(row["UserId"]), PhongBanId = Convert.ToInt32(row["PhongBanID"]) });
                    }
                    else
                    {
                        lbMessage.Text += "Người sử dụng \"" + row["Username"] + "\" đã tồn tại trong phòng ban khác.<br/>";
                        flag = false;
                    }
                }
                if (flag)
                    scope.Complete();
            }
            catch { }
        }
    }
}

