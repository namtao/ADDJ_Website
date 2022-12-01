using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using Website.Ws_EmailReference;

public partial class admin_loaiKhieuNai_VASUpdate_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CallJavaScriptInUpdatePanel();
        if (!IsPostBack)
        {
            BindDropDownlist();
            BindGrid(true, 1);
        }

        if(!Permission.UserEdit)
        {
            liUpdate.Visible = false;
        }

        if(!Permission.UserDelete)
        {
            liDelete.Visible = false;
        }

        if(!Permission.Other1)
        {
            liDeleteCSDL.Visible = false;
        }

    }
    private void BindDropDownlist()
    {
        List<LoaiKhieuNai_VASUpdateInfo> list = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListLoaiKhieuNai_VASUpdateCap();

        list.Insert(0, new LoaiKhieuNai_VASUpdateInfo() { Id = 0, Name = "--Là cha--" });
        ddlLoaiKhieuNai.DataTextField = "Name";
        ddlLoaiKhieuNai.DataValueField = "Id";
        ddlLoaiKhieuNai.DataSource = list;
        ddlLoaiKhieuNai.DataBind();
    }
    private void CallJavaScriptInUpdatePanel()
    {
        txtNameFilter.Attributes.Add("onkeypress", "return handleEnter('" + btFilter.ClientID + "', event)");
        txtMaDichVuFilter.Attributes.Add("onkeypress", "return handleEnter('" + btFilter.ClientID + "', event)");
    }
    private void BindGrid(bool isClearFilter, int _pageIndex)
    {
        try
        {            
            if (_pageIndex != 0)
                pageIndex = _pageIndex;

            string whereClause = " 1=1 ";
            if (!isClearFilter)
            {
                if (!ddlLoaiKhieuNai.SelectedValue.Equals("0"))
                {
                    whereClause += string.Format(" AND (Id={0} or ParentId = {0} or ParentId in (select Id from LoaiKhieuNai_VASUpdate where ParentId = {0}))", ddlLoaiKhieuNai.SelectedValue);
                }
                if (!txtMaDichVuFilter.Text.Trim().Equals(""))
                {
                    whereClause += string.Format(" {1} MaDichVu like N'%{0}%'", txtMaDichVuFilter.Text, whereClause.Length > 0 ? "AND" : ""); ;
                }
                if (!txtNameFilter.Text.Trim().Equals(""))
                {
                    whereClause += string.Format(" {1} Name like N'%{0}%'", txtNameFilter.Text, whereClause.Length > 0 ? "AND" : "");
                }
            }
            else
            {
                ddlLoaiKhieuNai.SelectedValue = "0";
                txtNameFilter.Text = "";
                DropDownListPageSize.SelectedIndex = 0;
            }

            pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);

            List<LoaiKhieuNai_VASUpdateInfo> loaiKhieuNai_VASUpdateObj = null;

            int totalRecord = 0;

            string selectClause = "*";

            if (!txtNameFilter.Text.Trim().Equals(""))
                loaiKhieuNai_VASUpdateObj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetPaged(selectClause, whereClause, "Sort", pageIndex, pageSize, ref totalRecord);
            else
                loaiKhieuNai_VASUpdateObj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListLoaiKhieuNai_VASUpdateInfoSortParentPage("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", selectClause, whereClause, pageIndex, pageSize, ref totalRecord);
            if (loaiKhieuNai_VASUpdateObj != null && loaiKhieuNai_VASUpdateObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + loaiKhieuNai_VASUpdateObj.Count + " trên tổng số "+ totalRecord + " loại dịch vụ VAS được tìm thấy.</font>";
                grvView.DataSource = loaiKhieuNai_VASUpdateObj;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có Loại Khieu Nai VAS Update được tìm thấy.</font>";
                grvView.DataSource = null;
                grvView.DataBind();
            }
            TextBoxPage.Text = pageIndex.ToString();
            var totalPage = totalRecord / pageSize;
            if (totalRecord % pageSize != 0)
                totalPage++;
            LabelNumberOfPages.Text = totalPage.ToString();

            ltTongSoBanGhi.Text = totalRecord.ToString();
            if (pageIndex == totalPage)
            {
                ImageButtonNext.Enabled = false;
                ImageButtonLast.Enabled = false;
            }
            else
            {
                ImageButtonNext.Enabled = true;
                ImageButtonLast.Enabled = true;
                if (pageIndex == 1)
                {
                    ImageButtonFirst.Enabled = false;
                    ImageButtonPrev.Enabled = false;
                }
                else
                {
                    ImageButtonFirst.Enabled = true;
                    ImageButtonPrev.Enabled = true;
                }
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }


    protected int pageIndex = 1;
    protected int pageSize = 10;

    protected void TextBoxPage_TextChanged(object sender, EventArgs e)
    {
        if (Utility.IsInteger(TextBoxPage.Text))
        {
            pageIndex = Convert.ToInt32(TextBoxPage.Text);
            BindGrid(false, pageIndex);
        }
        else
        {
            TextBoxPage.Text = TextBoxPage.Attributes["PageOld"];
            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Số trang không hợp lệ.','error');", true);
        }
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


    private void RowDataBound(GridViewRowEventArgs e)
    {
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
            if (grvView.DataKeys[e.Row.RowIndex].Value.ToString().StartsWith("&nbsp;"))
                e.Row.Cells[1].Text = "<a href='loaiKhieuNai_VASUpdate_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[1].Text + "</a>";
        }
    }

    //private void PageIndexChanging(GridViewPageEventArgs e)
    //{
    //    grvView.PageIndex = e.NewPageIndex;
    //    BindGrid();
    //}

    protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
                LoaiKhieuNai_VASUpdateInfo item = obj.GetInfo(int.Parse(grvView.DataKeys[e.Row.RowIndex].Value.ToString()));
                if (item != null)
                {
                    if (item.IsUpdate == false)
                    {
                        e.Row.Attributes["class"] = "rowB_Edit";
                    }
                    else
                    {
                        if (e.Row.DataItemIndex != -1)
                        {
                            if (e.Row.RowIndex % 2 != 0)
                            {
                                e.Row.Attributes.Add("style", "background: #F5F8FB;");
                                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CDDFF2'");
                                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F5F8FB'");
                            }
                            else
                            {

                                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#CDDFF2'");
                                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
                            }
                        }
                    }
                }
                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
                if (e.Row.Cells[2].Text.StartsWith("&#160;"))
                    e.Row.Cells[2].Text = "<a href='loaiKhieuNai_VASUpdate_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "'>" + e.Row.Cells[2].Text + "</a>";
            }
            // RowDataBound(e);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    //protected void grvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    PageIndexChanging(e);
    //}
 
    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true, 1);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false, 1);
    }
 

    protected void linkbtnThemMoi_Click(object sender, EventArgs e)
    {
        Response.Redirect("loaiKhieuNai_VASUpdate_add.aspx", false);
    }

    protected void linkbtnUpdate_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }
        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    var isUpdated = (CheckBox)row.FindControl("chkIsUpdate");
                    int ID = int.Parse(grvView.DataKeys[i].Value.ToString());

                    string whereClause = string.Format("Id={0} ", ID);
                    string fieldUpdate = string.Format("IsUpdate='{0}', LDate='{1}',LUser='{2}'", isUpdated.Checked, DateTime.Now, LoginAdmin.AdminLogin().Username);
                    obj.UpdateDynamic(fieldUpdate, whereClause);
                    i++;
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật loại  khiếu nại vas aupdate thành công.','info');", true);
        }

        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Có lỗi khi thực hiện cập nhật dữ liệu.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false, 1);
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
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                    var loaiDichVuVas = obj.GetInfo(ID);
                    if (loaiDichVuVas != null)
                    {
                        loaiDichVuVas.IsDeleted = true;
                        loaiDichVuVas.LUser = UserInfo.Username;
                        loaiDichVuVas.IsUpdate = false;
                        obj.Update(loaiDichVuVas);
                    }
                }
                i++;
            }
            try
            {
                Website.Ws_EmailReference.Ws_EmailSoapClient sendEmailXmlvnp = new Ws_EmailSoapClient();

                SendEmailXMLVNPListRequest objSend = new SendEmailXMLVNPListRequest();
                //objSend.
                ArrayOfString lstTo = new ArrayOfString();
                lstTo.Add("haintt@vinaphone.vn");
                lstTo.Add("longlx@aivietnam.net");
                var sendTed = sendEmailXmlvnp.SendEmailXMLVNPList(lstTo, null, null, "VAS thay đổi loại dịch vụ", "Người thực hiện " + UserInfo.Username + "<br/> Xóa  dịch vụ .<br/> Ngày cập nhật:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), null, string.Empty, "603335ee7c3141268941761c48032fb2");
            }
            catch { }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Khoá dịch vụ thành công.','info');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false, 1);
    }

    protected void linkbtnDeleteCSDL_Click(object sender, EventArgs e)
    {
        if (!Permission.Other1)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {
                    int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                    var loaiDichVuVas = obj.GetInfo(ID);
                    if (loaiDichVuVas != null)
                    {
                        obj.Delete(ID);
                    }
                }
                i++;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xoá dịch vụ thành công.','info');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false, 1);
    }
}

