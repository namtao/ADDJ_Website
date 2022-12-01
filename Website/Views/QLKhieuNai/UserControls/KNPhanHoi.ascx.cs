using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Globalization;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class KNPhanHoi : System.Web.UI.UserControl
    {
        private AdminInfo admin;
        private string strReturnUrlBind;
        protected void Page_Load(object sender, EventArgs e)
        {
            admin = LoginAdmin.AdminLogin();
            if (!IsPostBack)
            {
               // DropDownListPageSize.SelectedValue = "10";

              //  BindDropDownlist();

               // BindParamURL();

               // TextBoxPage.Attributes.Add("PageOld", "1");
            }      

        }

        //protected string BindUrl(object id)
        //{
        //    //var strMa = BindMaKN(id);

        //    string strReturnURL = Request.Url.AbsolutePath + "?ctrl=KNPhanHoi" + strReturnUrlBind;
        //    strReturnURL = HttpUtility.UrlEncode(strReturnURL);
        //    return string.Format("<a href='../ChiTietKhieuNai/XuLyKhieuNai.aspx?MaKN={0}&ReturnUrl={2}&Mode=Process'>{1}</a>", id, strMa, strReturnURL);

        //}

        //private void BindDropDownlist()
        //{
        //    ddlKhieuNaiFilter.Items.Add(new ListItem("Phòng ban", "3"));
        //    ddlKhieuNaiFilter.Items.Add(new ListItem("Cá nhân", "1"));
            
        //    foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
        //    {
        //        ddlDoUuTien.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " "), i.ToString()));
        //    }

        //    foreach (byte i in Enum.GetValues(typeof(KhieuNai_TrangThai_Type)))
        //    {
        //        ddlTinhTrangXuLy.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_TrangThai_Type), i).Replace("_", " "), i.ToString()));
        //    }

        //    var lstLoaiKN = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=0", "Sort");
        //    lstLoaiKN.Insert(0, new LoaiKhieuNaiInfo() { Id = -1, Name = "--Loại khiếu nại--" });
        //    ddlLoaiKhieuNai.DataSource = lstLoaiKN;
        //    ddlLoaiKhieuNai.DataTextField = "Name";
        //    ddlLoaiKhieuNai.DataValueField = "Id";
        //    ddlLoaiKhieuNai.DataBind();
        //}

        //private void LoadTooltip()
        //{
        //    Utility.Tooltip(ddlLoaiKhieuNai);
        //    Utility.Tooltip(ddlLinhVucChung);
        //    Utility.Tooltip(ddlLinhVucCon);
        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        //}

        //private void BindParamURL()
        //{
        //    if (Request.QueryString["TypeSearch"] != null && !Request.QueryString["TypeSearch"].Equals(""))
        //    {
        //        ddlKhieuNaiFilter.SelectedValue = Request.QueryString["TypeSearch"];
        //    }

        //    if (Request.QueryString["LoaiKhieuNaiId"] != null && !Request.QueryString["LoaiKhieuNaiId"].Equals(""))
        //    {
        //        ddlLoaiKhieuNai.SelectedValue = Request.QueryString["LoaiKhieuNaiId"];

        //        LoaiKhieuNaiChange();
        //    }

        //    if (Request.QueryString["LinhVucChungId"] != null && !Request.QueryString["LinhVucChungId"].Equals(""))
        //    {
        //        ddlLinhVucChung.SelectedValue = Request.QueryString["LinhVucChungId"];

        //        LinhVucChungChange();
        //    }

        //    if (Request.QueryString["LinhVucConId"] != null && !Request.QueryString["LinhVucConId"].Equals(""))
        //    {
        //        ddlLinhVucCon.SelectedValue = Request.QueryString["LinhVucConId"];
        //    }

        //    if (Request.QueryString["DoUuTien"] != null && !Request.QueryString["DoUuTien"].Equals(""))
        //    {
        //        ddlDoUuTien.SelectedValue = Request.QueryString["DoUuTien"];
        //    }

        //    if (Request.QueryString["TrangThai"] != null && !Request.QueryString["TrangThai"].Equals(""))
        //    {
        //        ddlTinhTrangXuLy.SelectedValue = Request.QueryString["TrangThai"];
        //    }

        //    if (Request.QueryString["PSize"] != null && !Request.QueryString["PSize"].Equals(""))
        //    {
        //        DropDownListPageSize.SelectedValue = Request.QueryString["PSize"];
        //    }

        //    if (Request.QueryString["PSize"] != null && !Request.QueryString["PSize"].Equals(""))
        //    {
        //        DropDownListPageSize.SelectedValue = Request.QueryString["PSize"];
        //    }

        //    if (Request.QueryString["ContentSeach"] != null && !Request.QueryString["ContentSeach"].Equals(""))
        //    {
        //        txtNoiDung.Text = Request.QueryString["ContentSeach"];
        //    }

        //    if (Request.QueryString["STB"] != null && !Request.QueryString["STB"].Equals(""))
        //    {
        //        txtSoThueBao.Text = Request.QueryString["STB"];
        //    }

        //    if (Request.QueryString["NTNhan"] != null && !Request.QueryString["NTNhan"].Equals(""))
        //    {
        //        txtNguoiTiepNhan.Text = Request.QueryString["NTNhan"];
        //    }

        //    if (Request.QueryString["NXLy"] != null && !Request.QueryString["NXLy"].Equals(""))
        //    {
        //        txtNguoiXuLy.Text = Request.QueryString["NXLy"];
        //    }

        //    if (Request.QueryString["TNTu"] != null && !Request.QueryString["TNTu"].Equals(""))
        //    {
        //        if (Request.QueryString["TNTu"].Length == 8)
        //        {
        //            txtNgayTiepNhan_From.Text = Request.QueryString["TNTu"].Substring(5) + "/" + Request.QueryString["TNTu"].Substring(3, 5) + "/" + Request.QueryString["TNTu"].Substring(0, 3);
        //        }
        //        else
        //        {
        //            txtNgayTiepNhan_From.Text = Request.QueryString["TNTu"];
        //        }
        //    }

        //    if (Request.QueryString["TNDen"] != null && !Request.QueryString["TNDen"].Equals(""))
        //    {
        //        if (Request.QueryString["TNDen"].Length == 8)
        //        {
        //            txtNgayTiepNhan_To.Text = Request.QueryString["TNDen"].Substring(5) + "/" + Request.QueryString["TNDen"].Substring(3, 5) + "/" + Request.QueryString["TNDen"].Substring(0, 3);
        //        }
        //        else
        //        {
        //            txtNgayTiepNhan_To.Text = Request.QueryString["TNDen"];
        //        }
        //    }

        //    if (Request.QueryString["QHTu"] != null && !Request.QueryString["QHTu"].Equals(""))
        //    {
        //        if (Request.QueryString["QHTu"].Length == 8)
        //        {
        //            txtNgayQuaHan_From.Text = Request.QueryString["QHTu"].Substring(5) + "/" + Request.QueryString["QHTu"].Substring(3, 5) + "/" + Request.QueryString["QHTu"].Substring(0, 3);
        //        }
        //        else
        //        {
        //            txtNgayQuaHan_From.Text = Request.QueryString["QHTu"];
        //        }
        //    }

        //    if (Request.QueryString["QHDen"] != null && !Request.QueryString["QHDen"].Equals(""))
        //    {
        //        if (Request.QueryString["QHDen"].Length == 8)
        //        {
        //            txtNgayQuaHan_To.Text = Request.QueryString["QHDen"].Substring(5) + "/" + Request.QueryString["QHDen"].Substring(3, 5) + "/" + Request.QueryString["QHDen"].Substring(0, 3);
        //        }
        //        else
        //        {
        //            txtNgayQuaHan_To.Text = Request.QueryString["QHDen"];
        //        }
        //    }

        //    int pIndex = 1;
        //    if (Request.QueryString["PIndex"] != null && !Request.QueryString["PIndex"].Equals(""))
        //    {
        //        pIndex = Convert.ToInt32(Request.QueryString["PIndex"]);
        //    }

        //    BindGrid(false, pIndex);
        //}


        //private void BindGrid(bool isClearFilter, int _pageIndex)
        //{
        //    try
        //    {
        //        pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);

        //        if (_pageIndex != 0)
        //            pageIndex = _pageIndex;
        //        if (isClearFilter)
        //        {
        //            ddlTinhTrangXuLy.SelectedIndex = 0;
        //            ddlDoUuTien.SelectedIndex = 0;
        //            ddlKhieuNaiFilter.SelectedIndex = 0;

        //            ddlLoaiKhieuNai.SelectedIndex = 0;
        //            ddlLinhVucChung.SelectedIndex = 0;
        //            ddlLinhVucCon.SelectedIndex = 0;

        //            txtSoThueBao.Text = "Số thuê bao...";
        //            txtNguoiTiepNhan.Text = "Người tiếp nhận...";
        //            txtNguoiXuLy.Text = "Người xử lý...";
        //            txtNoiDung.Text = "Nội dung khiếu nại...";
        //            txtNgayQuaHan_From.Text = "Từ ngày...";
        //            txtNgayQuaHan_To.Text = "Đến ngày...";
        //            txtNgayTiepNhan_From.Text = "Từ ngày...";
        //            txtNgayTiepNhan_To.Text = "Đến ngày...";
        //        }

        //        var TrangThai = ddlTinhTrangXuLy.SelectedValue;
        //        var DoUuTien = ddlDoUuTien.SelectedValue;

        //        var totalRecord = 0;
        //        string selectClause = "a.Id,a.DoUuTien, a.SoThueBao, a.LoaiKhieuNai, a.NguoiTiepNhan, a.NoiDungPA, a.NguoiXuLy ,b.GhiChu,b.NguoiXuLyTruoc,b.NguoiDuocPhanHoi";
        //        string joinClause = " right join KhieuNai_Activity b on b.KhieuNaiId = a.Id";
        //        string whereClause = " IsCurrent = 1 and HanhDong = 3 and  a.PhongBanXuLyId=" + LoginAdmin.AdminLogin().PhongBanId;

                
        //        if (ddlKhieuNaiFilter.SelectedValue.Equals("1"))
        //            whereClause += " AND a.NguoiXuLy='" + LoginAdmin.AdminLogin().Username + "'";

        //        if (!TrangThai.Equals("-1"))
        //            whereClause += " AND a.TrangThai=" + TrangThai;

        //        if (!DoUuTien.Equals("-1"))
        //            whereClause += " AND a.DoUuTien=" + DoUuTien;

        //        if (!txtSoThueBao.Text.Trim().Equals("") && !txtSoThueBao.Text.Trim().Equals(txtSoThueBao.Attributes["placeholder"]))
        //            whereClause += " AND a.SoThueBao=" + txtSoThueBao.Text.Trim();

        //        if (!ddlLoaiKhieuNai.SelectedValue.Equals("-1"))
        //            whereClause += " AND LoaiKhieuNaiId=" + ddlLoaiKhieuNai.SelectedValue;

        //        if (!ddlLinhVucChung.SelectedValue.Equals("-1"))
        //            whereClause += " AND LinhVucChungId=" + ddlLinhVucChung.SelectedValue;

        //        if (!ddlLinhVucCon.SelectedValue.Equals("-1"))
        //            whereClause += " AND LinhVucConId=" + ddlLinhVucCon.SelectedValue;

        //        if (!txtNguoiTiepNhan.Text.Trim().Equals("") && !txtNguoiTiepNhan.Text.Trim().Equals(txtNguoiTiepNhan.Attributes["placeholder"]))
        //            whereClause += " AND a.NguoiTiepNhan='" + txtNguoiTiepNhan.Text.Trim() + "'";

        //        if (!txtNguoiXuLy.Text.Trim().Equals("") && !txtNguoiXuLy.Text.Trim().Equals(txtNguoiXuLy.Attributes["placeholder"]))
        //            whereClause += " AND a.NguoiXuLy='" + txtNguoiXuLy.Text.Trim() + "'";

        //        if (!txtNoiDung.Text.Trim().Equals("") && !txtNoiDung.Text.Trim().Equals(txtNoiDung.Attributes["placeholder"]))
        //            whereClause += " AND a.NoiDungPA like N'%" + txtNoiDung.Text + "%'";

        //        if ((!txtNgayTiepNhan_From.Text.Equals("") && !txtNgayTiepNhan_From.Text.Trim().Equals(txtNgayTiepNhan_From.Attributes["placeholder"])) && (!txtNgayTiepNhan_To.Text.Equals("") && !txtNgayTiepNhan_From.Text.Trim().Equals(txtNgayTiepNhan_From.Attributes["placeholder"])))
        //        {
        //            int nNgayTiepNhan_From = 0;
        //            int nNgayTIepNhan_To = 0;
        //            try
        //            {
        //                nNgayTiepNhan_From = Convert.ToInt32(Convert.ToDateTime(txtNgayTiepNhan_From.Text, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                nNgayTIepNhan_To = Convert.ToInt32(Convert.ToDateTime(txtNgayTiepNhan_To.Text, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //            }
        //            catch { }

        //            if (nNgayTiepNhan_From > 0 && nNgayTIepNhan_To > 0)
        //            {
        //                whereClause += string.Format("AND (NgayTiepNhanSort>={0} AND NgayTiepNhanSort<={1})", nNgayTiepNhan_From, nNgayTIepNhan_To);
        //            }
        //        }

        //        if ((!txtNgayQuaHan_From.Text.Equals("") && !txtNgayQuaHan_From.Text.Trim().Equals(txtNgayQuaHan_From.Attributes["placeholder"])) && (!txtNgayQuaHan_To.Text.Equals("") && !txtNgayQuaHan_To.Text.Trim().Equals(txtNgayQuaHan_To.Attributes["placeholder"])))
        //        {
        //            int nNgayQuaHan_From = 0;
        //            int nNgayQuaHan_To = 0;
        //            try
        //            {
        //                nNgayQuaHan_From = Convert.ToInt32(Convert.ToDateTime(txtNgayQuaHan_From.Text, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //                nNgayQuaHan_To = Convert.ToInt32(Convert.ToDateTime(txtNgayQuaHan_To.Text, new CultureInfo("vi-VN")).ToString("yyyyMMdd"));
        //            }
        //            catch { }

        //            if (nNgayQuaHan_From > 0 && nNgayQuaHan_To > 0)
        //            {
        //                whereClause += string.Format("AND (NgayQuaHanSort>={0} AND NgayQuaHanSort<={1})", nNgayQuaHan_From, nNgayQuaHan_To);
        //            }
        //        }

        //        string orderClause = "a.LDate desc";

        //        strReturnUrlBind = string.Format("&TypeSearch={0}&LoaiKhieuNaiId={1}&LinhVucChungId={2}&LinhVucConId={3}&DoUuTien={4}&TrangThai={5}&PIndex={6}&PSize={7}&ContentSeach={8}&STB={9}&NTNhan={10}&NXLy={11}&TNTu={12}&TNDen={13}&QHTu={14}&QHDen={15}",
        //            ddlKhieuNaiFilter.SelectedValue, ddlLoaiKhieuNai.SelectedValue, ddlLinhVucChung.SelectedValue,
        //            ddlLinhVucCon.SelectedValue, ddlDoUuTien.SelectedValue, ddlTinhTrangXuLy.SelectedValue, _pageIndex,
        //            DropDownListPageSize.SelectedValue, txtNoiDung.Text, txtSoThueBao.Text, txtNguoiTiepNhan.Text, txtNguoiXuLy.Text,
        //            txtNgayTiepNhan_From.Text, txtNgayTiepNhan_To.Text, txtNgayQuaHan_From.Text, txtNgayQuaHan_To.Text);

        //        var lst = ServiceFactory.GetInstanceKhieuNai().GetPagedJoin(selectClause, joinClause, whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                
        //        rptView.DataSource = lst;
        //        rptView.DataBind();

        //        TextBoxPage.Text = pageIndex.ToString();

        //        TextBoxPage.Attributes["PageOld"] = pageIndex.ToString();

        //        var totalPage = totalRecord / pageSize;
        //        if (totalRecord % pageSize != 0)
        //            totalPage++;
        //        LabelNumberOfPages.Text = totalPage.ToString();

        //        ltTongSoBanGhi.Text = totalRecord.ToString();
        //    }
        //    catch (Exception ex) { Utility.LogEvent(ex); }
        //    finally
        //    {
        //        LoadTooltip();
        //        //if (IsPostBack)
        //        //{
        //        //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
        //        //}
        //    }
        //}

        //protected string BindTinhTrangXuLy(object obj)
        //{
        //    return string.Empty;
        //}

        //protected string BindMaKN(object obj)
        //{
        //    return GetDataImpl.GetMaTuDong(Constant.PREFIX_PHIEU_KHIEU_NAI, obj, 10);
        //}

        //protected string BindDoUuTien(object obj)
        //{
        //    try
        //    {
        //        return Enum.GetName(typeof(KhieuNai_DoUuTien_Type), Convert.ToByte(obj)).Replace("_", " ");
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.LogEvent(ex);
        //        return string.Empty;
        //    }
        //}

        //protected void btClearFilter_Click(object sender, EventArgs e)
        //{
        //    BindGrid(true, 0);
        //}

        //protected void btFilter_Click(object sender, EventArgs e)
        //{
        //    BindGrid(false, 0);
        //}

        //#region Pageing
        //protected int pageIndex = 1;
        //protected int pageSize = 10;

        //protected void TextBoxPage_TextChanged(object sender, EventArgs e)
        //{
        //    if (Utility.IsInteger(TextBoxPage.Text))
        //    {
        //        pageIndex = Convert.ToInt32(TextBoxPage.Text);
        //        BindGrid(false, pageIndex);
        //    }
        //    else
        //    {
        //        TextBoxPage.Text = TextBoxPage.Attributes["PageOld"];
        //        BindGrid(false, Convert.ToInt32(TextBoxPage.Text));
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Số trang không hợp lệ.','error');", true);
        //    }
        //}

        //protected void ImageButtonFirst_Click(object sender, EventArgs e)
        //{
        //    BindGrid(false, 1);
        //}

        //protected void ImageButtonPrev_Click(object sender, EventArgs e)
        //{
        //    int pIndex = Convert.ToInt32(TextBoxPage.Text) - 1;

        //    if (pIndex == 0)
        //        return;

        //    BindGrid(false, Convert.ToInt32(TextBoxPage.Text) - 1);
        //}

        //protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGrid(false, 1);
        //}

        //protected void ImageButtonNext_Click(object sender, EventArgs e)
        //{
        //    int pIndex = Convert.ToInt32(TextBoxPage.Text) - 1;

        //    if (pIndex == Convert.ToInt32(LabelNumberOfPages.Text))
        //        return;

        //    BindGrid(false, Convert.ToInt32(TextBoxPage.Text) + 1);
        //}

        //protected void ImageButtonLast_Click(object sender, EventArgs e)
        //{
        //    BindGrid(false, Convert.ToInt32(LabelNumberOfPages.Text));
        //}
        //#endregion

        //protected void ddlLoaiKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoaiKhieuNaiChange();
        //}

        //private void LoaiKhieuNaiChange()
        //{
        //    ddlLinhVucCon.Items.Clear();
        //    ddlLinhVucCon.Items.Add(new ListItem("--Lĩnh vực con--", "-1"));
        //    if (ddlLoaiKhieuNai.SelectedValue.Equals("-1"))
        //    {
        //        ddlLinhVucChung.Items.Clear();
        //        ddlLinhVucChung.Items.Add(new ListItem("--Lĩnh vực chung--", "-1"));
        //    }
        //    else
        //    {
        //        var lstLinhVucChung = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLoaiKhieuNai.SelectedValue, "Sort");
        //        lstLinhVucChung.Insert(0, new LoaiKhieuNaiInfo() { Id = -1, Name = "--Lĩnh vực chung--" });
        //        ddlLinhVucChung.DataSource = lstLinhVucChung;
        //        ddlLinhVucChung.DataTextField = "Name";
        //        ddlLinhVucChung.DataValueField = "Id";
        //        ddlLinhVucChung.DataBind();
        //    }

        //    LoadTooltip();
        //}

        //protected void ddlLinhVucChung_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LinhVucChungChange();
        //}

        //private void LinhVucChungChange()
        //{
        //    if (ddlLinhVucChung.SelectedValue.Equals("-1"))
        //    {
        //        ddlLinhVucCon.Items.Clear();
        //        ddlLinhVucCon.Items.Add(new ListItem("--Lĩnh vực con--", "-1"));
        //    }
        //    else
        //    {
        //        var lstLinhVucCon = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId=" + ddlLinhVucChung.SelectedValue, "Sort");
        //        lstLinhVucCon.Insert(0, new LoaiKhieuNaiInfo() { Id = -1, Name = "--Lĩnh vực con--" });
        //        ddlLinhVucCon.DataSource = lstLinhVucCon;
        //        ddlLinhVucCon.DataTextField = "Name";
        //        ddlLinhVucCon.DataValueField = "Id";
        //        ddlLinhVucCon.DataBind();
        //    }

        //    LoadTooltip();
        //}

        //protected void ddlKhieuNaiFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGrid(false, 1);
        //}
    }
}