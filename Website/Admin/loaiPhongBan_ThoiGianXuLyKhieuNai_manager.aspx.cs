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

public partial class admin_loaiPhongBan_ThoiGianXuLyKhieuNai_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDownlist();

            grvView.PageSize = Config.RecordPerPage;
            BindGrid(false, 1);
        }
    }


    private void BindDropDownlist()
    {
        var lstFilter = ServiceFactory.GetInstanceLoaiKhieuNai().GetListLoaiKhieuNai2Cap();
        lstFilter.Insert(0, new LoaiKhieuNaiInfo() { Id = 0, Name = "[ Tất cả ]" });

        ddlLoaiKhieuNai.DataSource = lstFilter;
        ddlLoaiKhieuNai.DataTextField = "Name";
        ddlLoaiKhieuNai.DataValueField = "Id";
        ddlLoaiKhieuNai.DataBind();

        ddlLoaiPhongBan.DataSource = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        ddlLoaiPhongBan.DataValueField = "ID";
        ddlLoaiPhongBan.DataTextField = "Name";
        ddlLoaiPhongBan.DataBind();
        
        if (Request.QueryString["LoaiPhongBanId"] != null && Request.QueryString["LoaiPhongBanId"].ToString() != "")
        {
            ddlLoaiPhongBan.SelectedValue = Request.QueryString["LoaiPhongBanId"];
        }


        cblLoaiPhongBan.DataSource = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        cblLoaiPhongBan.DataValueField = "ID";
        cblLoaiPhongBan.DataTextField = "Name";
        cblLoaiPhongBan.DataBind();
        
    }

    private void BindGrid(bool isClearFilter, int _pageIndex)
    {
        try
        {
            
            if (isClearFilter)
            {
                ddlLoaiKhieuNai.SelectedIndex = 0;
                DropDownListPageSize.SelectedIndex = 0;
            }


            pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);
            if (_pageIndex != 0)
                pageIndex = _pageIndex;

            string selectClause = "b.Id, b.Name LoaiKhieuNai_Name, b.ThoiGianUocTinh LoaiKhieuNai_ThoiGianUocTinh, b.ParentId LoaiKhieuNai_ParentId,b.Cap, isnull(a.ThoiGianCanhBao,'0') ThoiGianCanhBao, isnull(a.ThoiGianUocTinh,'0') ThoiGianUocTinh";
            string joinClause = "right join LoaiKhieuNai b on a.LoaiKhieuNaiId = b.Id and Loaiphongbanid = " + ddlLoaiPhongBan.SelectedValue;
            string whereClause = string.Empty;

            if (!ddlLoaiKhieuNai.SelectedValue.Equals("0"))
            {
                whereClause = string.Format(" (b.Id={0} or b.ParentId = {0} or b.ParentId in (select LoaiKhieuNai.Id from LoaiKhieuNai where ParentId = {0}))", ddlLoaiKhieuNai.SelectedValue);
            }            

            int totalRecord = 0;
            var loaiPhongBan_ThoiGianXuLyKhieuNaiObj = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, "b.Sort", pageIndex, pageSize, ref totalRecord);
            if (loaiPhongBan_ThoiGianXuLyKhieuNaiObj != null && loaiPhongBan_ThoiGianXuLyKhieuNaiObj.Count > 0)
            {
                var capMin = loaiPhongBan_ThoiGianXuLyKhieuNaiObj.Min(t => t.Cap);
                var pId = loaiPhongBan_ThoiGianXuLyKhieuNaiObj.Where(t => t.Cap == capMin).First();

                var lst = BuildTree(loaiPhongBan_ThoiGianXuLyKhieuNaiObj, pId.LoaiKhieuNai_ParentId, 0, "&nbsp;&nbsp;&nbsp;&nbsp;");

                grvView.DataSource = lst;
                grvView.DataBind();
            }
            else
            {
                grvView.DataSource = null;
                grvView.DataBind();
            }

            TextBoxPage.Text = pageIndex.ToString();
            var totalPage = totalRecord / pageSize;
            if (totalRecord % pageSize != 0)
                totalPage++;
            LabelNumberOfPages.Text = totalPage.ToString();

            ltTongSoBanGhi.Text = totalRecord.ToString();

            if (IsPostBack)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private List<LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo> BuildTree(List<LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo> lst, int pId, int cap, string replaceSpace)
    {
        string strSpace2 = string.Empty;
        string strSpace3 = string.Empty;

        for (int i = 0; i < 1; i++)
            strSpace2 += replaceSpace;

        for (int i = 0; i < 2; i++)
            strSpace3 += replaceSpace;

        foreach (var item in lst)
        {
            if(item.Cap == 2)
                item.LoaiKhieuNai_Name = HttpUtility.HtmlDecode(strSpace2 + item.LoaiKhieuNai_Name);
            else if(item.Cap == 3)
                item.LoaiKhieuNai_Name = HttpUtility.HtmlDecode(strSpace3 + item.LoaiKhieuNai_Name);            
        }
        return lst;
    }

    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {            
            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }
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


    private bool ValidThoiGianXuLy(string str)
    {
        string[] arr = str.Split('d', 'h', 'm');

        foreach (var item in arr)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            if (!Utility.IsInteger(item))
                return false;
        }

        return true;
    }
 

    protected void btClearFilter_Click(object sender, EventArgs e)
    {
        BindGrid(true, 1);
    }

    protected void btFilter_Click(object sender, EventArgs e)
    {
        BindGrid(false, 1);
    }

    #region Pageing
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
    #endregion
 
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
            var obj = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai();
            var loaiPBId = int.Parse(ddlLoaiPhongBan.SelectedValue);
            var lstCheck = obj.GetListDynamic("LoaiKhieuNaiId", "LoaiPhongBanId=" + loaiPBId, "");
            foreach (GridViewRow row in grvView.Rows)
            {
                var loaiKNId = int.Parse(grvView.DataKeys[i].Value.ToString());

                var timeCanhBao = (TextBox)row.FindControl("txtCanhBao");
                var timePhongBan = (TextBox)row.FindControl("txtPhongBan");

                if (!ValidThoiGianXuLy(timeCanhBao.Text) || !ValidThoiGianXuLy(timePhongBan.Text))
                {
                    string mess = "Loại khiếu nại:  <b>" + row.Cells[1].Text + "</b> thời gian chưa hợp lệ.<br /><br />Quá trình cập nhật thời gian xử lý dừng tại đây.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + HttpUtility.HtmlDecode(mess) + "','error');", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onload", "LoadJS();", true);
                    return;
                }

                if (!timeCanhBao.Text.Equals(timeCanhBao.ToolTip) || !timePhongBan.Text.Equals(timePhongBan.ToolTip))
                {
                    if (lstCheck.Where(t => t.LoaiKhieuNaiId == loaiKNId).Any())
                    {
                        string whereClause = string.Format("LoaiPhongBanId={0} AND LoaiKhieuNaiId={1}", loaiPBId, loaiKNId);
                        string fieldUpdate = string.Format("ThoiGianCanhBao='{0}', ThoiGianUocTinh='{1}'", timeCanhBao.Text.Trim(), timePhongBan.Text.Trim());
                        obj.UpdateDynamic(fieldUpdate, whereClause);
                    }
                    else
                    {
                        obj.Add(new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo() { LoaiPhongBanId = loaiPBId, LoaiKhieuNaiId = loaiKNId, ThoiGianCanhBao = timeCanhBao.Text.Trim(), ThoiGianUocTinh = timePhongBan.Text.Trim() });
                    }
                }

                i++;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật thời gian xử lý khiếu nại thành công.','info');", true);
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

        //Refresh Cache
        LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl.ListThoiGianXuLyPhongBan = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetList();

        BindGrid(false, Convert.ToInt32(TextBoxPage.Text));
    }

    protected void linkbtnMultiUpdate_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {

            var obj = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai();

            for (int indexLoaiPhongBan = 0; indexLoaiPhongBan < cblLoaiPhongBan.Items.Count; indexLoaiPhongBan++)
            {
                if (!cblLoaiPhongBan.Items[indexLoaiPhongBan].Selected) continue;

                int i = 0;

                var loaiPBId = int.Parse(cblLoaiPhongBan.Items[indexLoaiPhongBan].Value);
                var lstCheck = obj.GetListDynamic("LoaiKhieuNaiId", "LoaiPhongBanId=" + loaiPBId, "");
                foreach (GridViewRow row in grvView.Rows)
                {
                    var loaiKNId = int.Parse(grvView.DataKeys[i].Value.ToString());

                    var timeCanhBao = (TextBox)row.FindControl("txtCanhBao");
                    var timePhongBan = (TextBox)row.FindControl("txtPhongBan");

                    if (!ValidThoiGianXuLy(timeCanhBao.Text) || !ValidThoiGianXuLy(timePhongBan.Text))
                    {
                        string mess = "Loại khiếu nại:  <b>" + row.Cells[1].Text + "</b> thời gian chưa hợp lệ.<br /><br />Quá trình cập nhật thời gian xử lý dừng tại đây.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + HttpUtility.HtmlDecode(mess) + "','error');", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onload", "LoadJS();", true);
                        return;
                    }

                    if (!timeCanhBao.Text.Equals(timeCanhBao.ToolTip) || !timePhongBan.Text.Equals(timePhongBan.ToolTip))
                    {
                        if (lstCheck.Where(t => t.LoaiKhieuNaiId == loaiKNId).Any())
                        {
                            string whereClause = string.Format("LoaiPhongBanId={0} AND LoaiKhieuNaiId={1}", loaiPBId, loaiKNId);
                            string fieldUpdate = string.Format("ThoiGianCanhBao='{0}', ThoiGianUocTinh='{1}'", timeCanhBao.Text.Trim(), timePhongBan.Text.Trim());
                            obj.UpdateDynamic(fieldUpdate, whereClause);
                        }
                        else
                        {
                            obj.Add(new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo() { LoaiPhongBanId = loaiPBId, LoaiKhieuNaiId = loaiKNId, ThoiGianCanhBao = timeCanhBao.Text.Trim(), ThoiGianUocTinh = timePhongBan.Text.Trim() });
                        }
                    }

                    i++;
                }

            } // end for(int indexLoaiPhongBan =0;indexLoaiPhongBan < lbLoaiPhongBan.Items.Count;indexLoaiPhongBan ++)

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật thời gian xử lý khiếu nại thành công.','info');", true);
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

        //Refresh Cache
        LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl.ListThoiGianXuLyPhongBan = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetList();

        BindGrid(false, Convert.ToInt32(TextBoxPage.Text));

        for (int i = 0; i < cblLoaiPhongBan.Items.Count; i++)
        {
            cblLoaiPhongBan.Items[i].Selected = false;
        }
    }
}

