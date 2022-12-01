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
using Website.HTHTKT;

public partial class admin_nguoiSuDung_manager : PageBase
{
    protected string message = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pageIndex = 1;
            pageSize = 10;
            BindDropDownlist();
            BindGrid(false, 0);
        }

        if (!Permission.UserEdit)
        {
            liUpdate.Visible = false;
            liUpdateToDoiTac.Visible = false;
            liUpdateToKTV.Visible = false;
        }

        if(!Permission.Other2)
        {
            btRemoveUserToPhongBan.Visible = false;
            btAddUserToPhongBan.Visible = false;
        }

        if (!Permission.UserDelete)
        {
            liDelete.Visible = false;
        }
    }

    protected string BindHanhDong(object obj)
    {
        //Phân quyền
        if (Permission.Other1)
        {
            return string.Format("<a target='_blank' href='PhanQuyen.aspx?ID={0}'>Phân quyền</a><br /><a target='_blank' href='PhanQuyenKN2NSD.aspx?UserId={0}'>Phân quyền KN</a>", obj);
        }
        else
        {
            return "<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Phân quyền</a><br /><a onclick='MessageAlert.AlertJSON(-999);' href='#'>Phân quyền KN</a>";
        }
    }

    protected string BindNhomNguoiDung(object obj)
    {
        return NguoiSuDung_GroupImpl.NhomNguoiDung.Where(t => t.Id == Convert.ToInt32(obj)).Single().Name;
    }

    protected string BindTrangThai(object obj)
    {
        try
        {
            return Enum.GetName(typeof(NguoiSuDung_TrangThai), obj).Replace("_", " ");
        }
        catch
        {
            return "";
        }
    }

    private void BindDropDownlist()
    {
        ddlFilter.Items.Add(new ListItem("Tên truy cập", "TenTruyCap"));
        ddlFilter.Items.Add(new ListItem("Tên đầy đủ", "TenDayDu"));
        ddlFilter.Items.Add(new ListItem("Đối tác", "TenDoiTac"));
        ddlFilter.Items.Add(new ListItem("Địa chỉ", "DiaChi"));
        ddlFilter.Items.Add(new ListItem("Di động", "DiDong"));
        ddlFilter.Items.Add(new ListItem("Cố định", "CoDinh"));
        ddlFilter.Items.Add(new ListItem("Email", "Email"));
        ddlFilter.Items.Add(new ListItem("Trạng thái", "TrangThai"));

        var lst = ServiceFactory.GetInstanceDoiTac().GetList();
        List<DoiTacInfo> lstDoiTac = new List<DoiTacInfo>();
        var admin = LoginAdmin.AdminLogin();

        IEnumerable<DoiTacInfo> parent = null;

        if (admin.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET || admin.Username.ToLower() == "administrator")
        {
            parent = lst.Where(t => t.Id == DoiTacInfo.DoiTacIdValue.VNP || t.Id == DoiTacInfo.DoiTacIdValue.VNPT_NET || t.Id == DoiTacInfo.DoiTacIdValue.VNPT_MEDIA);
        }
        else
        {
            parent = lst.Where(t => t.Id == admin.DoiTacId);
        }
        
        foreach (var item in parent)
        {
            lstDoiTac.Add(item);
            var lstChild = lst.Where(t => t.DonViTrucThuoc == item.Id);
            foreach (var itemChild in lstChild)
            {
                itemChild.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild.MaDoiTac);
                lstDoiTac.Add(itemChild);

                var lstChildC2 = lst.Where(t => t.DonViTrucThuoc == itemChild.Id);
                foreach (var itemChild2 in lstChildC2)
                {
                    itemChild2.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild2.MaDoiTac);
                    lstDoiTac.Add(itemChild2);
                }
            }
        }

        ddlDonVi.DataSource = lstDoiTac;
        ddlDonVi.DataValueField = "ID";
        ddlDonVi.DataTextField = "MaDoiTac";
        ddlDonVi.DataBind();

        string whereClausePhongBan = string.Empty;

        if (admin.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET || admin.Username.ToLower() == "administrator")
        {
            parent = lst.Where(t => t.Id == DoiTacInfo.DoiTacIdValue.VNP || t.Id == DoiTacInfo.DoiTacIdValue.VNPT_NET || t.Id == DoiTacInfo.DoiTacIdValue.VNPT_MEDIA);
        }
        else
        {
            whereClausePhongBan = string.Format("DoiTacId = {0} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {0} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {0} ) ) )", ddlDonVi.SelectedValue);
        }

        var strSql = string.Format(@"WITH pban(Id,Name,Description,ParentId,Cap,iddonvi)
                                         AS (
                                         SELECT a.Id,Name,a.Description,ParentId,Cap,b.Id AS iddonvi FROM dbo.PhongBan a
                                         INNER JOIN dbo.DoiTac b ON a.DoiTacId=b.Id
                                         WHERE b.DonViTrucThuoc in (SELECT id FROM func_BangIdDonViCha_CacCon({0})) or b.Id={1}
                                         UNION ALL
                                         SELECT a.Id,a.Name,a.Description,a.ParentId,a.Cap,b.Id AS iddonvi FROM dbo.PhongBan a
                                         INNER JOIN dbo.DoiTac b ON a.DoiTacId=b.Id
                                         INNER JOIN pban ON pban.Id = a.ParentId
                                         )
                                          SELECT DISTINCT Id,Name,ParentId FROM pban;", ddlDonVi.SelectedValue, ddlDonVi.SelectedValue);
        var dsxlyc = new List<PhongBan>();
        using (var ctx = new ADDJContext())
        {
            dsxlyc = ctx.Database.SqlQuery<PhongBan>(strSql).ToList(); ;
        }

        //var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name,ParentId,Cap", whereClausePhongBan, "");

        List<PhongBanInfo> lstPhongBanSort = new List<PhongBanInfo>();

        var lstParentSort = dsxlyc.Where(t => t.ParentId == 0).OrderBy(t => t.Sort);
        foreach (var parentPhongBan in lstParentSort)
        {
            var itemParent = new PhongBanInfo();
            itemParent.Id = parentPhongBan.Id;
            itemParent.Name = parentPhongBan.Name;
            lstPhongBanSort.Add(itemParent);

            if (dsxlyc.Any(t => t.ParentId == parentPhongBan.Id))
            {
                foreach (var childPhongBan in dsxlyc.Where(t => t.ParentId == parentPhongBan.Id))
                {
                    itemParent = new PhongBanInfo();
                    itemParent.Id = childPhongBan.Id;
                    itemParent.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + childPhongBan.Name);
                    lstPhongBanSort.Add(itemParent);

                    if (dsxlyc.Any(t => t.ParentId == childPhongBan.Id))
                    {
                        foreach (var childEndPhongBan in dsxlyc.Where(t => t.ParentId == childPhongBan.Id))
                        {
                            itemParent = new PhongBanInfo();
                            itemParent.Id = childEndPhongBan.Id;
                            itemParent.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + childEndPhongBan.Name);
                            lstPhongBanSort.Add(itemParent);
                        }
                    }
                }
            }
        }

        ddlPhongBan.DataSource = lstPhongBanSort;
        //ddlPhongBan.DataSource = dsxlyc;
        ddlPhongBan.DataValueField = "ID";
        ddlPhongBan.DataTextField = "Name";
        ddlPhongBan.DataBind();
        ddlPhongBan.Items.Insert(0, new ListItem("[ Tất cả ]", "0"));

        if (Request.QueryString["PhongBanId"] != null && Request.QueryString["PhongBanId"] != string.Empty)
        {
            ddlPhongBan.SelectedValue = Request.QueryString["PhongBanId"];
        }


        ddlPhongBanSelect.DataSource = dsxlyc;
        ddlPhongBanSelect.DataValueField = "ID";
        ddlPhongBanSelect.DataTextField = "Name";
        ddlPhongBanSelect.DataBind();

        ddlPhongBanSelectRemove.DataSource = dsxlyc;
        ddlPhongBanSelectRemove.DataValueField = "ID";
        ddlPhongBanSelectRemove.DataTextField = "Name";
        ddlPhongBanSelectRemove.DataBind();
    }

    private void BindGrid(bool isClearFilter, int _pageIndex)
    {
        pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);

        if (_pageIndex != 0)
            pageIndex = _pageIndex;

        if (isClearFilter)
        {
            pageIndex = 1;

            ddlDonVi.SelectedIndex = 0;
            txtKeyword.Text = "";
            ddlFilter.SelectedIndex = 0;
            ddlPhongBan.SelectedIndex = 0;
        }

        int DoiTacId = Convert.ToInt32(ddlDonVi.SelectedValue);
        string filterType = ddlFilter.SelectedValue;
        string query = txtKeyword.Text;
        string PhongBanId = ddlPhongBan.SelectedValue;

        List<NguoiSuDungInfo> lst = null;

        int totalRecord = 0;

        string selectClause = string.Empty;
        selectClause = "*,dbo.fnGetPhongBanFromNguoiSuDung(NguoiSuDung.Id) PhongBan_Name";

        string orderClause = "LDate DESC";

        string whereClause = "TrangThai <> 2 AND Id != " + UserInfo.Id;

        whereClause += string.Format(" AND (DoiTacId = {0} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {0} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {0} ) ) ))", DoiTacId);

        if (!PhongBanId.Equals("0"))
        {
            whereClause += string.Format(" AND Id in (select NguoiSuDungId from PhongBan_User where PhongBanId = {0})", PhongBanId);
        }

        if (!filterType.Equals("0") && !string.IsNullOrEmpty(filterType))
            whereClause += " AND " + filterType + " like N'%" + query + "%'";

        lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged(selectClause, whereClause, orderClause, pageIndex, pageSize, ref totalRecord);

        ltThongBao.Text = "<font color='red'>Có " + totalRecord + " người sử dụng được tìm thấy.</font>";

        rptView.DataSource = lst;
        rptView.DataBind();

        TextBoxPage.Text = pageIndex.ToString();
        var totalPage = totalRecord / pageSize;
        if (totalRecord % pageSize != 0)
            totalPage++;
        LabelNumberOfPages.Text = totalPage.ToString();

        ltTongSoBanGhi.Text = totalRecord.ToString();

        if(IsPostBack)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
    }
 
 
    protected void btThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("nguoiSuDung_add.aspx", false);
    }

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true, 0);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false, 0);
    }

    #region Pageing
    protected int pageIndex = 1;
    protected int pageSize = 10;

    protected void TextBoxPage_TextChanged(object sender, EventArgs e)
    {
        var pIndex = 1;
        try {
            pIndex = Convert.ToInt32(TextBoxPage.Text);
            BindGrid(false, pIndex);
        }
        catch { }
    }

    protected void ImageButtonFirst_Click(object sender, EventArgs e)
    {
        BindGrid(false, 1);
    }

    protected void ImageButtonPrev_Click(object sender, EventArgs e)
    {
        int pIndex = Convert.ToInt32(TextBoxPage.Text) - 1;

        if (pIndex == 0)
            return;

        BindGrid(false, Convert.ToInt32(TextBoxPage.Text) - 1);
    }

    protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid(false, 1);
    }

    protected void ImageButtonNext_Click(object sender, EventArgs e)
    {
        int pIndex = Convert.ToInt32(TextBoxPage.Text) - 1;

        if (pIndex == Convert.ToInt32(LabelNumberOfPages.Text))
            return;

        BindGrid(false, Convert.ToInt32(TextBoxPage.Text) + 1);
    }

    protected void ImageButtonLast_Click(object sender, EventArgs e)
    {
        BindGrid(false, Convert.ToInt32(LabelNumberOfPages.Text));
    }
    #endregion

    protected void btnOkay_Click(object sender, EventArgs e)
    {
        if (!Permission.Other2)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        try
        {
            var obj = ServiceFactory.GetInstancePhongBan_User();

            var phongbanId = int.Parse(ddlPhongBanSelect.SelectedValue);

            var lstUserInPhongBan = obj.GetListDynamic("Id,NguoiSuDungId", "PhongBanId=" + ddlPhongBanSelect.SelectedValue, "");
            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    var userId = Convert.ToInt32(hdId.Value);


                    var checkExists = lstUserInPhongBan.Where(t => t.NguoiSuDungId == userId).Any();
                    if (!checkExists)
                    {
                        obj.Add(new PhongBan_UserInfo() { NguoiSuDungId = userId, PhongBanId = phongbanId });
                    }
                }
            }

            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban cho người dùng thành công','info');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Thêm Phòng ban cho người dùng có lỗi','error');", true);
        }
    }


    protected void btnOkayRemove_Click(object sender, EventArgs e)
    {
        if (!Permission.Other2)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        try
        {
            var obj = ServiceFactory.GetInstancePhongBan_User();

            var phongbanId = int.Parse(ddlPhongBanSelectRemove.SelectedValue);

            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    var userId = Convert.ToInt32(hdId.Value);
                    var whereClause = string.Format("NguoiSuDungId={0} AND PhongBanId={1}", userId, phongbanId);
                    obj.DeleteDynamic(whereClause);
                }
            }

            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa Phòng ban cho người dùng thành công','info');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa Phòng ban cho người dùng có lỗi','error');", true);
        }
    }

    protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string whereClausePhongBan = string.Format("DoiTacId = {0} or DoiTacId in ( select id from DoiTac where DonViTrucThuoc = {0} or Id in ( select Id from DoiTac where DonViTrucThuoc in (  select id from DoiTac where DonViTrucThuoc = {0} ) ) )", ddlDonVi.SelectedValue);
        //var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,Name", whereClausePhongBan, "Sort");

        var strSql = string.Format(@"WITH pban(Id,Name,Description,ParentId,Cap,iddonvi)
                                         AS (
                                         SELECT a.Id,Name,a.Description,ParentId,Cap,b.Id AS iddonvi FROM dbo.PhongBan a
                                         INNER JOIN dbo.DoiTac b ON a.DoiTacId=b.Id
                                         WHERE b.DonViTrucThuoc in (SELECT id FROM func_BangIdDonViCha_CacCon({0})) or b.Id={1}
                                         UNION ALL
                                         SELECT a.Id,a.Name,a.Description,a.ParentId,a.Cap,b.Id AS iddonvi FROM dbo.PhongBan a
                                         INNER JOIN dbo.DoiTac b ON a.DoiTacId=b.Id
                                         INNER JOIN pban ON pban.Id = a.ParentId
                                         )
                                          SELECT DISTINCT Id,Name,ParentId FROM pban;", ddlDonVi.SelectedValue, ddlDonVi.SelectedValue);
        var dsxlyc = new List<PhongBan>();
        using (var ctx = new ADDJContext())
        {
            dsxlyc = ctx.Database.SqlQuery<PhongBan>(strSql).ToList(); ;
        }

        List<PhongBanInfo> lstPhongBanSort = new List<PhongBanInfo>();

        var lstParentSort = dsxlyc.Where(t => t.ParentId == 0).OrderBy(t => t.Sort);
        foreach (var parentPhongBan in lstParentSort)
        {
            var itemParent = new PhongBanInfo();
            itemParent.Id = parentPhongBan.Id;
            itemParent.Name = parentPhongBan.Name;
            lstPhongBanSort.Add(itemParent);

            if (dsxlyc.Any(t => t.ParentId == parentPhongBan.Id))
            {
                foreach (var childPhongBan in dsxlyc.Where(t => t.ParentId == parentPhongBan.Id))
                {
                    itemParent = new PhongBanInfo();
                    itemParent.Id = childPhongBan.Id;
                    itemParent.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + childPhongBan.Name);
                    lstPhongBanSort.Add(itemParent);

                    if (dsxlyc.Any(t => t.ParentId == childPhongBan.Id))
                    {
                        foreach (var childEndPhongBan in dsxlyc.Where(t => t.ParentId == childPhongBan.Id))
                        {
                            itemParent = new PhongBanInfo();
                            itemParent.Id = childEndPhongBan.Id;
                            itemParent.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + childEndPhongBan.Name);
                            lstPhongBanSort.Add(itemParent);
                        }
                    }
                }
            }
        }

        ddlPhongBan.DataSource = lstPhongBanSort;
        ddlPhongBan.DataValueField = "ID";
        ddlPhongBan.DataTextField = "Name";
        ddlPhongBan.DataBind();
        ddlPhongBan.Items.Insert(0, new ListItem("[ Tất cả ]", "0"));
    }
 
    protected void linkbtnTrangThai_Click(object sender, EventArgs e)
    {
        if (!Permission.UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("chkUpdateTrangThai");
                var hdTrangThai = (HiddenField)row.FindControl("hdTrangThai");

                var blTrangThaiOld = Convert.ToBoolean(Convert.ToInt32(hdTrangThai.Value));

                if (status.Checked != blTrangThaiOld)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    obj.UpdateStatus(Convert.ToInt32(hdId.Value), status.Checked ? 1 : 0);
                }
                i++;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }

        BindGrid(false, Convert.ToInt32(TextBoxPage.Text));
        //BindGrid(false, 0);

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật trạng thái thành công','info');", true);
    }

    protected void linkbtnUpdateToDoiTac_Click(object sender, EventArgs e)
    {
        if (!Permission.UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");

                if (status.Checked)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    string strUpdate = "NhomNguoiDung=" + (int)NguoiSuDung_NhomNguoiDung.Đối_tác;
                    string whereClause = "Id=" + Convert.ToInt32(hdId.Value);
                    obj.UpdateDynamic(strUpdate, whereClause);
                }
            }

            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật người dùng thành đối tác thành công.','info');", true);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    protected void linkbtnUpdateToKTV_Click(object sender, EventArgs e)
    {
        if (!Permission.UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");

                if (status.Checked)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    string strUpdate = "NhomNguoiDung=" + (int)NguoiSuDung_NhomNguoiDung.Khai_thác_viên;
                    string whereClause = "Id=" + Convert.ToInt32(hdId.Value);
                    obj.UpdateDynamic(strUpdate, whereClause);
                }
            }

            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật người dùng thành khai thác viên thành công.','info');", true);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
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
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            foreach (RepeaterItem row in rptView.Items)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");

                if (status.Checked)
                {
                    var hdId = (HiddenField)row.FindControl("hdId");
                    obj.Delete(Convert.ToInt32(hdId.Value));
                }
            }

            BindGrid(false, 0);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

