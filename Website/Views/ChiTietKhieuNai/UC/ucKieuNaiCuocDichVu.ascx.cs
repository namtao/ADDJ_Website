using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.AppCode;
using Website.AppCode.Controller;
using System.Transactions;
using System.Collections;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucKieuNaiCuocDichVu : System.Web.UI.UserControl
    {
        public string username = string.Empty;
        public int KhieuNaiId { get; set; }
        public string Mode { get; set; }
        private Boolean IsPageRefresh = false;
        protected bool taomoi = false, sua = false, xoa = false;
        public bool IsTraSau { get; set; }
        private int ArchiveId = 0;
        private AdminInfo userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {


            userInfo = LoginAdmin.AdminLogin();
            username = userInfo.Username;

            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            Mode = ConvertUtility.ToString(Request.QueryString["Mode"]).ToLower();
            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            if (Mode != "view")
            {
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tạo_KN_cước))
                    taomoi = true;
            }

            var KhieuNaiItem = ServiceFactory.GetInstanceKhieuNai(ArchiveId).GetListDynamic("LyDoGiamTru,IsTraSau", "Id=" + KhieuNaiId, "");
            if (KhieuNaiItem != null && KhieuNaiItem.Count() > 0)
            {
                IsTraSau = KhieuNaiItem[0].IsTraSau;
            }
            if (!IsPostBack)
            {
                //foreach (int i in Enum.GetValues(typeof(LyDoGiamTru_GiaCuoc)))
                //{
                //    ddlLyDoGiamTru.Items.Add(new ListItem(Enum.GetName(typeof(LyDoGiamTru_GiaCuoc), i).Replace("_", " "), i.ToString()));
                //}
                //if (KhieuNaiItem != null && KhieuNaiItem.Count > 0)
                //    ddlLyDoGiamTru.SelectedValue = KhieuNaiItem[0].LyDoGiamTru.ToString();
                //if (Mode == "view")
                //    ddlLyDoGiamTru.Enabled = false;
            }
        }

        protected string BindNguoiSua(object LUser, object TienEdit)
        {
            if (ConvertUtility.ToDecimal(TienEdit) > 0)
                return LUser.ToString();
            return string.Empty;
        }

        public void FillKieuNaiCuocDichVuGrid()
        {
            if (Mode == "view")
            {
                gvKieuNaiCuocDichVu.EditRowStyle.CssClass = "dpn";
                gvKieuNaiCuocDichVu.FooterStyle.CssClass = "dpn";
            }
            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_SoTienInfo> objKieuNaiCuocDichVuList = new List<KhieuNai_SoTienInfo>();
            objKieuNaiCuocDichVuList = ServiceFactory.GetInstanceKhieuNai_SoTien(ArchiveId).GetListDynamic("", whereClause, "LDate DESC");
            gvKieuNaiCuocDichVu.DataSource = objKieuNaiCuocDichVuList;
            gvKieuNaiCuocDichVu.DataBind();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AnUniqueKey", "AutocompleteDauSoCP();", true);
        }

        private void LoadDDlLoaiTien(DropDownList ddlLoaiTien)
        {
            if (IsTraSau)
            {
                foreach (int i in Enum.GetValues(typeof(KhieuNai_LoaiTien_TraSau)))
                {
                    ddlLoaiTien.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_LoaiTien_TraSau), i).Replace("_", " "), i.ToString()));
                }
            }
            else
            {
                foreach (int i in Enum.GetValues(typeof(KhieuNai_LoaiTien)))
                {
                    ddlLoaiTien.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_LoaiTien), i).Replace("_", " "), i.ToString()));
                }
            }
        }

        protected void gvKieuNaiCuocDichVu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TextBox txtSoTien = e.Row.FindControl("txtSoTien") as TextBox;
            if (txtSoTien != null)
                txtSoTien.Attributes.Add("onkeyup", "FormatNumber(" + txtSoTien.ClientID + ");");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hdUser = e.Row.FindControl("hdCUser") as HiddenField;

                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    var btnDaBu = e.Row.FindControl("lnkChuaBuTien") as LinkButton;
                    var btnChuaBu = e.Row.FindControl("lnkDaBuTien") as LinkButton;
                    //var hdDichVuId = e.Row.FindControl("hdDichVuId") as HiddenField;

                    //var ltLyDoGiamTru = e.Row.FindControl("ltLyDoGiamTru") as Literal;
                    //ltLyDoGiamTru.Text = Enum.GetName(typeof(LyDoGiamTru_GiaCuoc), ConvertUtility.ToInt32(ltLyDoGiamTru.Text, (byte)LyDoGiamTru_GiaCuoc.Lý_Do_Khác)).Replace("_", " ");

                    var hdIsBuTien = e.Row.FindControl("hdIsBuTien") as HiddenField;

                    // HaiPH 21/11/2015
                    // Kiểm tra nếu user không phải là VNPT và hạn mức xác nhận giảm trừ tiền thì mới hiển thị nút xác nhận bù tiền, nếu không thì ẩn đi                                        
                    decimal soTienGiamTru = 0;
                    Label lblSoTien = e.Row.FindControl("lblSoTien") as Label;
                    Label lblSoTien_Edit = e.Row.FindControl("lblSoTien_Edit") as Label;
                    if (ConvertUtility.ToDecimal(lblSoTien_Edit.Text.Replace(".", ",")) > 0)
                    {
                        soTienGiamTru = ConvertUtility.ToDecimal(lblSoTien_Edit.Text.Replace(".", ","));
                    }
                    else
                    {
                        soTienGiamTru = ConvertUtility.ToDecimal(lblSoTien.Text.Replace(".", ","));
                    }

                    if (userInfo.DoiTacType != DoiTacInfo.DoiTacTypeValue.VNPTTT && userInfo.GioiHanGiamTruMax >= soTienGiamTru)
                    {
                        if (hdIsBuTien.Value.ToLower().Equals("true"))
                        {
                            btnDaBu.Visible = false;
                        }
                        else
                        {
                            btnChuaBu.Visible = false;
                        }
                    }
                    else
                    {
                        btnDaBu.Visible = false;
                        btnChuaBu.Visible = false;
                    }

                    #region Không sử dụng
                    //if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xác_nhận_bù_tiền))
                    //{                        
                    //    if (hdIsBuTien.Value.ToLower().Equals("true"))
                    //    {
                    //        btnDaBu.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        btnChuaBu.Visible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    btnDaBu.Visible = false;
                    //    btnChuaBu.Visible = false;
                    //} 
                    #endregion

                    var btnEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                    var btnDel = e.Row.FindControl("lnkDelete") as LinkButton;
                    btnEdit.Visible = false;
                    btnDel.Visible = false;

                    var DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(userInfo.DoiTacId);
                    if (Mode != "view" && (hdIsBuTien.Value.ToLower().Equals("false") || DoiTacItem.DoiTacType == 4))
                    {
                        if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_KN_cước))
                        {
                            btnEdit.Visible = true;
                        }

                        if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_KN_cước_của_mình) && hdUser.Value == username)
                        {
                            btnDel.Visible = true;
                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_KN_cước))
                        {
                            btnDel.Visible = true;
                        }
                    }
                }
                else if ((e.Row.RowState & DataControlRowState.Edit) > 0) // Trạng thái cập nhật
                {
                    #region Trạng thái chỉnh sửa
                    TextBox txtSoTien_Edit = e.Row.FindControl("txtSoTien_Edit") as TextBox;
                    if (txtSoTien_Edit != null) txtSoTien_Edit.Attributes.Add("onkeyup", "FormatNumber(" + txtSoTien_Edit.ClientID + ");");

                    DropDownList ddlLoaiTien = e.Row.FindControl("ddlLoaiTien") as DropDownList;
                    //TextBox txtDauSoCP = e.Row.FindControl("txtDauSoCP") as TextBox;

                    LoadDDlLoaiTien(ddlLoaiTien);
                    HiddenField hdLoaiTien = (HiddenField)e.Row.FindControl("hdLoaiTien");
                    ddlLoaiTien.SelectedValue = hdLoaiTien.Value;


                    //HiddenField hdDauSo = (HiddenField)e.Row.FindControl("hdDauSo");
                    //HiddenField hdDauSoId = (HiddenField)e.Row.FindControl("hdDauSoId");

                    DropDownList ddlLinhVucChung = e.Row.FindControl("ddlLinhVucChung") as DropDownList;

                    #region Phiên bản cũ
                    //var lstLinhVucChung = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("LinhVucChungId, LinhVucChung", "TrangThai = 1 GROUP BY LinhVucChungId,LinhVucChung", "");
                    //lstLinhVucChung.Insert(0, new DichVuCPInfo() { Id = 0, LinhVucChung = "--Chọn lĩnh vực chung--" });
                    //ddlLinhVucChung.DataSource = lstLinhVucChung;
                    //ddlLinhVucChung.DataTextField = "LinhVucChung";
                    //ddlLinhVucChung.DataValueField = "LinhVucChungId";
                    //ddlLinhVucChung.DataBind(); 
                    #endregion

                    // Dương DV
                    // Bind thông tin "Lĩnh vực chung trên bảng Loại Khiếu Nại, Với Nhóm Media = 22
                    int nhomId = 22; // Fix trong bảng "LoaiKhieuNai_Nhom"
                    LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

                    // Giá trị Lĩnh vực chung
                    HiddenField hdfLinhVucChungId = e.Row.FindControl("hdfLinhVucChungId") as HiddenField;

                    // Kiểm tra có xử lý Validate
                    bool isValidate = true;

                    List<LoaiKhieuNaiInfo> lstLoaiKhieuNai = ctl.GetListDynamic("*", string.Format("ParentId = 0 AND LoaiKhieuNai_NhomId = {0} AND (Status = 1 OR Status = 100)", nhomId), "Sort");
                    ddlLinhVucChung.Items.Clear();
                    List<string> lstIds = new List<string>(); // Chứa danh sách loại khiếu nại

                    foreach (LoaiKhieuNaiInfo info in lstLoaiKhieuNai)
                    {
                        ListItem item = new ListItem(info.Name, info.Id.ToString());
                        lstIds.Add(info.Id.ToString());
                        item.Attributes.Add("disabled", "disabled");
                        item.Attributes.Add("class", "not-selected");
                        ddlLinhVucChung.Items.Add(item);

                        // Lấy danh sách con nó
                        List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND  (Status = 1 OR Status = 100)", info.Id), "Sort");
                        foreach (LoaiKhieuNaiInfo info1 in lst)
                        {
                            ddlLinhVucChung.Items.Add(new ListItem(string.Format("+ {0}", info1.Name), info1.Id.ToString()));
                        }

                        if (info.Status == 100)
                        {
                            List<LoaiKhieuNaiInfo> lst2 = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND  (Status = 1 OR Status = 100)", info.Id), "Sort");
                            foreach (LoaiKhieuNaiInfo info2 in lst2)
                            {
                                if (info2.Status == 100 && info2.Id.ToString() == hdfLinhVucChungId.Value)
                                {
                                    isValidate = false;
                                    break;
                                }
                            }
                        }

                    }

                    // Xử lý cho phép (không) validate "Lĩnh vực con"
                    RequiredFieldValidator obj = e.Row.FindControl("ValidateDichVuCon") as RequiredFieldValidator;
                    obj.Visible = isValidate;

                    ddlLinhVucChung.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                    ddlLinhVucChung.Attributes.Add("Ids", string.Join(",", lstIds.ToArray<string>()));

                    // Lĩnh vực con
                    //  ddlDichVuCP.Items.Add(new ListItem("-- Chọn --", "0"));

                    DropDownList ddlDichVuCP = e.Row.FindControl("ddlDichVuCP") as DropDownList; // Lĩnh vực con

                    HiddenField hdDichVuId = (HiddenField)e.Row.FindControl("hdDichVuId");

                    string linhVucChungId = hdfLinhVucChungId.Value;

                    int linhVuConId = Convert.ToInt32(hdDichVuId.Value);
                    if (linhVuConId > 0)
                    {
                        LoaiKhieuNaiInfo info = ctl.GetInfo(linhVuConId);
                        ddlLinhVucChung.SelectedValue = info.ParentId.ToString();

                        ddlDichVuCP.Items.Clear();
                        List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND Status = 1", info.ParentId), "Sort");
                        ddlDichVuCP.DataSource = lst;
                        ddlDichVuCP.DataTextField = "Name";
                        ddlDichVuCP.DataValueField = "Id";
                        ddlDichVuCP.DataBind();
                        ddlDichVuCP.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                        ddlDichVuCP.SelectedValue = linhVuConId.ToString();

                    }
                    else
                    {
                        if (hdfLinhVucChungId.Value != string.Empty)
                        {
                            ddlLinhVucChung.SelectedValue = hdfLinhVucChungId.Value;

                            ddlDichVuCP.Items.Clear();
                            List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND (Status = 1 OR Status = 100)", hdfLinhVucChungId.Value), "Sort");
                            ddlDichVuCP.DataSource = lst;
                            ddlDichVuCP.DataTextField = "Name";
                            ddlDichVuCP.DataValueField = "Id";
                            ddlDichVuCP.DataBind();
                        }

                        ddlDichVuCP.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                    }
                    #endregion

                    #region Không sử dụng
                    //var valueSelected = Convert.ToInt32(hdDichVuId.Value);
                    //if (valueSelected == -80 || valueSelected == -57 || valueSelected == -56 || valueSelected == -54)
                    //{
                    //    txtDauSoCP.Enabled = true;
                    //}
                    //else
                    //{
                    //    txtDauSoCP.Enabled = false;
                    //}
                    //txtDauSoCP.Text = hdDauSoId.Value + "-" + hdDauSo.Value;

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AutocompleteEditDauSoCP", "AutocompleteEditDauSoCP();", true);
                    //var hdLyDoGiamTruId = e.Row.FindControl("hdLyDoGiamTruId") as HiddenField;
                    //DropDownList ddlLyDoGiamTru = (DropDownList)e.Row.FindControl("ddlLyDoGiamTru");
                    //foreach (int i in Enum.GetValues(typeof(LyDoGiamTru_GiaCuoc)))
                    //{
                    //    ddlLyDoGiamTru.Items.Add(new ListItem(Enum.GetName(typeof(LyDoGiamTru_GiaCuoc), i).Replace("_", " "), i.ToString()));
                    //}
                    //ddlLyDoGiamTru.SelectedValue = hdLyDoGiamTruId.Value; 
                    #endregion

                    var btnDel = e.Row.FindControl("lnkDelete") as LinkButton;
                    btnDel.Visible = false;


                    if (Mode != "view")
                    {
                        if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_KN_cước_của_mình) && hdUser.Value == username)
                        {
                            btnDel.Visible = true;
                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_KN_cước))
                        {
                            btnDel.Visible = true;
                        }
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                #region Cột rỗng
                if (taomoi)
                {
                    DropDownList ddlLoaiTien = e.Row.FindControl("ddlLoaiTien") as DropDownList;
                    LoadDDlLoaiTien(ddlLoaiTien);

                    DropDownList ddlDichVuCP = e.Row.FindControl("ddlDichVuCP") as DropDownList;
                    DropDownList ddlLinhVucChung = e.Row.FindControl("ddlLinhVucChung") as DropDownList;

                    // Dương DV
                    // Bind thông tin "Lĩnh vực chung trên bảng Loại Khiếu Nại, Với Nhóm Media = 22
                    int nhomId = 22; // Fix trong bảng "LoaiKhieuNai_Nhom"
                    LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

                    List<LoaiKhieuNaiInfo> lstLoaiKhieuNai = ctl.GetListDynamic("*", string.Format("ParentId = 0 AND LoaiKhieuNai_NhomId = {0} AND (Status = 1 OR Status = 100)", nhomId), "Sort");
                    ddlLinhVucChung.Items.Clear();
                    List<string> lstIds = new List<string>(); // Chứa danh sách loại khiếu nại

                    foreach (LoaiKhieuNaiInfo info in lstLoaiKhieuNai)
                    {
                        ListItem item = new ListItem(info.Name, info.Id.ToString());
                        lstIds.Add(info.Id.ToString());
                        item.Attributes.Add("disabled", "disabled");
                        item.Attributes.Add("class", "not-selected");
                        ddlLinhVucChung.Items.Add(item);

                        // Lấy danh sách con nó
                        List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND (Status = 1 OR Status = 100)", info.Id), "Sort");
                        foreach (LoaiKhieuNaiInfo info1 in lst)
                        {
                            ddlLinhVucChung.Items.Add(new ListItem(string.Format("+ {0}", info1.Name), info1.Id.ToString()));
                        }

                    }

                    ddlLinhVucChung.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                    ddlLinhVucChung.Attributes.Add("Ids", string.Join(",", lstIds.ToArray<string>()));

                    // Lĩnh vực con
                    ddlDichVuCP.Items.Add(new ListItem("-- Chọn --", "0"));

                    // End Dương DV

                    #region "Phiên bản cũ"
                    // Phiên bản cũ
                    // var lstLinhVucChung = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("LinhVucChungId, LinhVucChung", "TrangThai = 1 GROUP BY LinhVucChungId, LinhVucChung", string.Empty);
                    // lstLinhVucChung.Insert(0, new DichVuCPInfo() { Id = 0, LinhVucChung = "-- Chọn lĩnh vực chung --" });
                    // ddlLinhVucChung.DataSource = lstLinhVucChung;
                    // ddlLinhVucChung.DataTextField = "LinhVucChung";
                    // ddlLinhVucChung.DataValueField = "LinhVucChungId";
                    // ddlLinhVucChung.DataBind(); 


                    //var lstDichVU = new List<DichVuCPInfo>();
                    //lstDichVU.Insert(0, new DichVuCPInfo() { Id = 0, MaDichVu = "--Chọn dịch vụ--" });
                    //ddlDichVuCP.DataSource = lstDichVU;
                    //ddlDichVuCP.DataTextField = "MaDichVu";
                    //ddlDichVuCP.DataValueField = "Id";
                    //ddlDichVuCP.DataBind();
                    #endregion

                }
                else
                {
                    LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                    if (btAddFooter != null) btAddFooter.Visible = false;
                }
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LoadOnchageDichVu", "fnChangeDichVu(" + ddlDichVuCP.ClientID + ");", true);
                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Mode == "view")
                {
                    ValidationSummary groupValid = e.Row.FindControl("vsInsertKieuNaiCuocDichVu") as ValidationSummary;
                    if (groupValid != null)
                    {
                        groupValid.Enabled = false;
                    }

                    LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                    if (btAddFooter != null) btAddFooter.Visible = false;
                }
                else
                {
                    if (taomoi)
                    {
                        DropDownList ddlLoaiTien = e.Row.FindControl("ddlLoaiTien") as DropDownList;
                        LoadDDlLoaiTien(ddlLoaiTien);

                        DropDownList ddlDichVuCP = e.Row.FindControl("ddlDichVuCP") as DropDownList;
                        DropDownList ddlLinhVucChung = e.Row.FindControl("ddlLinhVucChung") as DropDownList;

                        #region Phiên bản cũ không sử dụng
                        //var lstLinhVucChung = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("LinhVucChungId,LinhVucChung", "TrangThai = 1 GROUP BY LinhVucChungId,LinhVucChung", "");
                        //lstLinhVucChung.Insert(0, new DichVuCPInfo() { Id = 0, LinhVucChung = "--Chọn lĩnh vực chung--" });
                        //ddlLinhVucChung.DataSource = lstLinhVucChung;
                        //ddlLinhVucChung.DataTextField = "LinhVucChung";
                        //ddlLinhVucChung.DataValueField = "LinhVucChungId";
                        //ddlLinhVucChung.DataBind();

                        //var lstDichVU = new List<DichVuCPInfo>();
                        //lstDichVU.Insert(0, new DichVuCPInfo() { Id = 0, MaDichVu = "--Chọn dịch vụ--" });
                        //ddlDichVuCP.DataSource = lstDichVU;
                        //ddlDichVuCP.DataTextField = "MaDichVu";
                        //ddlDichVuCP.DataValueField = "Id";
                        //ddlDichVuCP.DataBind();
                        #endregion

                        // Dương DV
                        // Bind thông tin "Lĩnh vực chung trên bảng Loại Khiếu Nại, Với Nhóm Media = 22
                        int nhomId = 22; // Fix trong bảng "LoaiKhieuNai_Nhom"
                        LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

                        List<LoaiKhieuNaiInfo> lstLoaiKhieuNai = ctl.GetListDynamic("*", string.Format("ParentId = 0 AND LoaiKhieuNai_NhomId = {0} AND (Status = 1 OR Status = 100)", nhomId), "Sort");
                        ddlLinhVucChung.Items.Clear();
                        List<string> lstIds = new List<string>(); // Chứa danh sách loại khiếu nại

                        foreach (LoaiKhieuNaiInfo info in lstLoaiKhieuNai)
                        {
                            ListItem item = new ListItem(info.Name, info.Id.ToString());
                            lstIds.Add(info.Id.ToString());
                            item.Attributes.Add("disabled", "disabled");
                            item.Attributes.Add("class", "not-selected");
                            ddlLinhVucChung.Items.Add(item);

                            // Lấy danh sách con nó
                            List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND (Status = 1 OR Status = 100)", info.Id), "Sort");
                            foreach (LoaiKhieuNaiInfo info1 in lst)
                            {
                                ddlLinhVucChung.Items.Add(new ListItem(string.Format("+ {0}", info1.Name), info1.Id.ToString()));
                            }

                        }

                        ddlLinhVucChung.Items.Insert(0, new ListItem("-- Chọn --", "0"));
                        ddlLinhVucChung.Attributes.Add("Ids", string.Join(",", lstIds.ToArray<string>()));

                        // Lĩnh vực con
                        ddlDichVuCP.Items.Add(new ListItem("-- Chọn --", "0"));

                        // End Dương DV
                    }
                    else
                    {
                        gvKieuNaiCuocDichVu.FooterStyle.CssClass = "dpn";
                        LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                        if (btAddFooter != null)
                            btAddFooter.Visible = false;
                    }
                }
            }
        }
        protected void gvKieuNaiCuocDichVu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!IsPageRefresh) // check that page is not refreshed by browser.
            {
                var obj = ServiceFactory.GetInstanceKhieuNai_SoTien();
                KhieuNai_SoTienInfo eInfo = new KhieuNai_SoTienInfo();
                if (e.CommandName.Equals("Insert"))
                {
                    try
                    {
                        eInfo.SoTien = Convert.ToDecimal(((TextBox)gvKieuNaiCuocDichVu.FooterRow.FindControl("txtSoTien")).Text.Replace(".", ""));
                        eInfo.LoaiTien = ConvertUtility.ToInt16(((DropDownList)gvKieuNaiCuocDichVu.FooterRow.FindControl("ddlLoaiTien")).SelectedValue);
                        var ghichu = (((TextBox)gvKieuNaiCuocDichVu.FooterRow.FindControl("txtGhiChu")).Text).Trim();
                        //string strDauSo = ((TextBox)gvKieuNaiCuocDichVu.FooterRow.FindControl("txtDauSoCP")).Text;

                        //string strDauSo = ((TextBox)gvKieuNaiCuocDichVu.FooterRow.FindControl("txtDauSoCP")).Text;

                        DropDownList ddlLinhVucChung = gvKieuNaiCuocDichVu.FooterRow.FindControl("ddlLinhVucChung") as DropDownList;
                        DropDownList ddlDichVuCP = gvKieuNaiCuocDichVu.FooterRow.FindControl("ddlDichVuCP") as DropDownList;
                        //eInfo.LyDoGiamTru = ConvertUtility.ToInt32(((DropDownList)gvKieuNaiCuocDichVu.FooterRow.FindControl("ddlLyDoGiamTru")).SelectedValue, (int)KhieuNai_HTTiepNhan_Type.Khác);
                        eInfo.LUser = username;
                        eInfo.KhieuNaiId = KhieuNaiId;
                        eInfo.GhiChu = ghichu;

                        // eInfo.DichVuCPId = Convert.ToInt32(ddlDichVuCP.SelectedValue);
                        eInfo.LinhVucChung = ddlLinhVucChung.SelectedItem.Text.Replace("+ ", string.Empty);
                        eInfo.LinhVucChungId = Convert.ToInt32(ddlLinhVucChung.SelectedItem.Value);

                        if (ddlDichVuCP.SelectedValue != string.Empty && ddlDichVuCP.SelectedValue != "0")
                        {
                            eInfo.LinhVucCon = ddlDichVuCP.SelectedItem.Text;
                            eInfo.MaDichVu = eInfo.LinhVucCon;
                        }
                        eInfo.LinhVucConId = Convert.ToInt32(ddlDichVuCP.SelectedItem.Value);

                        DoiTacInfo DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(userInfo.DoiTacId);

                        if (DoiTacItem.DoiTacType == 4) eInfo.IsDaBuTien = true;
                        else eInfo.IsDaBuTien = false;

                        // if (eInfo.DichVuCPId != 0) eInfo.MaDichVu = ddlDichVuCP.SelectedItem.Text;

                        //if (eInfo.DichVuCPId == -80 || eInfo.DichVuCPId == -57 || eInfo.DichVuCPId == -56 || eInfo.DichVuCPId == -54)
                        //{
                        //    eInfo.DauSo = strDauSo;
                        //}

                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                obj.Add(eInfo);

                                //obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfo.SoTien, eInfo.LoaiTien, 0, ConvertUtility.ToInt32(ddlLyDoGiamTru.SelectedValue), 1);
                                obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfo.SoTien, eInfo.LoaiTien, 0, 1);

                                scope.Complete();
                            }
                            catch (TransactionAbortedException tae)
                            {
                                Utility.LogEvent(tae);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                                return;
                            }
                            catch (Exception ex)
                            {
                                Utility.LogEvent(ex);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                                return;
                            }
                        }

                        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới cước dịch vụ", "KN cước dịch vụ", "",
                                   string.Format("Số tiền giảm trừ: {0} - Loại tài khoản: {1} - Ghi chú: {2}",
                                   eInfo.SoTien, ((DropDownList)gvKieuNaiCuocDichVu.FooterRow.FindControl("ddlLoaiTien")).SelectedItem.Text, ghichu));

                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE + "','error');", true);
                    }
                    FillKieuNaiCuocDichVuGrid();
                }

                // Thêm mới (Dạng empty)
                else if (e.CommandName.Equals("emptyInsert"))
                {
                    try
                    {
                        GridViewRow emptyRow = gvKieuNaiCuocDichVu.Controls[0].Controls[1] as GridViewRow;
                        eInfo.SoTien = Convert.ToDecimal(((TextBox)emptyRow.FindControl("txtSoTien")).Text.Replace(".", string.Empty));
                        var ghichu = (((TextBox)emptyRow.FindControl("txtGhiChu")).Text).Trim();

                        //string strDauSo = ((TextBox)emptyRow.FindControl("txtDauSoCP")).Text;

                        // Dương Dv => Cải tiến cho "Lĩnh vực chung, Lĩnh vực con"
                        DropDownList ddlLinhVucChung = emptyRow.FindControl("ddlLinhVucChung") as DropDownList; // Lĩnh vực con
                        ListItem linhVucChung = ddlLinhVucChung.SelectedItem;
                        int lvcId = Convert.ToInt32(linhVucChung.Value);
                        string lvcName = linhVucChung.Text;

                        DropDownList ddlDichVuCP = emptyRow.FindControl("ddlDichVuCP") as DropDownList; // Lĩnh vực con

                        //eInfo.LyDoGiamTru = ConvertUtility.ToInt32(((DropDownList)emptyRow.FindControl("ddlLyDoGiamTru")).SelectedValue, (int)KhieuNai_HTTiepNhan_Type.Khác);
                        eInfo.LoaiTien = ConvertUtility.ToInt16(((DropDownList)emptyRow.FindControl("ddlLoaiTien")).SelectedValue);
                        eInfo.LUser = username;
                        eInfo.KhieuNaiId = KhieuNaiId;
                        eInfo.GhiChu = ghichu;

                        // Dương Dv
                        if (ddlLinhVucChung.SelectedValue != string.Empty && ddlLinhVucChung.SelectedValue != "0")
                        {
                            eInfo.LinhVucChung = linhVucChung.Text.Replace("+ ", string.Empty);
                            eInfo.LinhVucChungId = lvcId;

                            if (ddlDichVuCP.SelectedValue != string.Empty && ddlDichVuCP.SelectedValue != "0")
                            {
                                eInfo.LinhVucCon = ddlDichVuCP.SelectedItem.Text;
                                eInfo.LinhVucConId = Convert.ToInt32(ddlDichVuCP.SelectedValue);

                                eInfo.MaDichVu = eInfo.LinhVucCon;
                            }
                        }


                        DoiTacInfo DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(userInfo.DoiTacId);
                        if (DoiTacItem.DoiTacType == 4) eInfo.IsDaBuTien = true;
                        else eInfo.IsDaBuTien = false;

                        #region "Phiên bản cũ"
                        //eInfo.DichVuCPId = Convert.ToInt32(ddlDichVuCP.SelectedValue);
                        //if (eInfo.DichVuCPId != 0) eInfo.MaDichVu = ddlDichVuCP.SelectedItem.Text;
                        #endregion

                        //if (eInfo.DichVuCPId == -80 || eInfo.DichVuCPId == -57 || eInfo.DichVuCPId == -56 || eInfo.DichVuCPId == -54)
                        //{
                        //    eInfo.DauSo = strDauSo;
                        //}

                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                obj.Add(eInfo);

                                //obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfo.SoTien, eInfo.LoaiTien, 0, ConvertUtility.ToInt32(ddlLyDoGiamTru.SelectedValue), 1);
                                obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfo.SoTien, eInfo.LoaiTien, 0, 1);


                                scope.Complete();

                            }
                            catch (TransactionAbortedException tae)
                            {
                                Utility.LogEvent(tae);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                                return;
                            }
                            catch (Exception ex)
                            {
                                Utility.LogEvent(ex);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                                return;
                            }
                        }

                        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới cước dịch vụ", "KN cước dịch vụ", "",
                                       string.Format("Số tiền giảm trừ: {0} - Loại tài khoản: {1} - Ghi chú: {2}",
                                       eInfo.SoTien, ((DropDownList)emptyRow.FindControl("ddlLoaiTien")).SelectedItem.Text, ghichu));

                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE + "','error');", true);
                    }

                    FillKieuNaiCuocDichVuGrid();
                }
                else if (e.CommandName.Equals("cmdChuaBuTien"))
                {
                    try
                    {
                        ServiceFactory.GetInstanceKhieuNai_SoTien().UpdateDynamic("IsDaBuTien=1,LDate=getdate(),LUser='" + LoginAdmin.AdminLogin().Username + "'", "Id=" + e.CommandArgument);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xác nhận bù tiền không thành công.','error');", true);
                    }
                    FillKieuNaiCuocDichVuGrid();
                }
                else if (e.CommandName.Equals("cmdDaBuTien"))
                {
                    try
                    {
                        ServiceFactory.GetInstanceKhieuNai_SoTien().UpdateDynamic("IsDaBuTien=0,LDate=getdate(),LUser='" + LoginAdmin.AdminLogin().Username + "'", "Id=" + e.CommandArgument);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Hủy bù tiền không thành công.','error');", true);
                    }
                    FillKieuNaiCuocDichVuGrid();
                }
            }
            else
                FillKieuNaiCuocDichVuGrid();
        }
        protected void gvKieuNaiCuocDichVu_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvKieuNaiCuocDichVu.EditIndex = e.NewEditIndex;
            FillKieuNaiCuocDichVuGrid();
        }
        protected void gvKieuNaiCuocDichVu_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_SoTien();
            KhieuNai_SoTienInfo eInfoUpdate = new KhieuNai_SoTienInfo();
            try
            {
                var idUpdate = Convert.ToInt32(gvKieuNaiCuocDichVu.DataKeys[e.RowIndex].Values[0].ToString());
                eInfoUpdate = obj.GetInfo(idUpdate);
                if (eInfoUpdate == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không tìm thấy dữ liệu khiếu nại cước.','error');", true);
                    return;
                }

                var loaiTienOld = eInfoUpdate.LoaiTien;
                var txtTienEdit = (TextBox)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("txtSoTien_Edit");
                if (txtTienEdit != null)
                {
                    eInfoUpdate.SoTien_Edit = Convert.ToDecimal(txtTienEdit.Text.Replace(".", ""));
                }

                eInfoUpdate.GhiChu = ((TextBox)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("txtGhiChu")).Text;

                //string strDauSo = ((TextBox)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("txtDauSoCP")).Text;
                //if (!string.IsNullOrEmpty(strDauSo))
                //{
                //    string[] dauSoCP = strDauSo.Split('-');
                //    int DauSoId = Convert.ToInt32(dauSoCP[0]);
                //    string DauSo = dauSoCP[1];
                //    eInfoUpdate.DauSo = DauSo;
                //    //eInfoUpdate.DauSoId = DauSoId;
                //}

                //string strDauSo = ((TextBox)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("txtDauSoCP")).Text;
                DropDownList ddlDichVuCP = ((DropDownList)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("ddlDichVuCP")) as DropDownList;

                //eInfoUpdate.LyDoGiamTru = ConvertUtility.ToInt32(((DropDownList)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("ddlLyDoGiamTru")).SelectedValue, (int)KhieuNai_HTTiepNhan_Type.Khác);
                eInfoUpdate.LoaiTien = ConvertUtility.ToInt16(((DropDownList)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("ddlLoaiTien")).SelectedValue);
                eInfoUpdate.LUser = username;
                eInfoUpdate.KhieuNaiId = KhieuNaiId;
                //eInfoUpdate.DauSo = strDauSo;

                var DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(userInfo.DoiTacId);
                if (DoiTacItem.DoiTacType == 4) eInfoUpdate.IsDaBuTien = true;
                else eInfoUpdate.IsDaBuTien = false;

                DropDownList ddlLinhVucChung = gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("ddlLinhVucChung") as DropDownList;

                int linhVucCon = Convert.ToInt32(ddlDichVuCP.SelectedValue);

                if (ddlLinhVucChung.SelectedValue != string.Empty && ddlLinhVucChung.SelectedValue != "0")
                {
                    eInfoUpdate.LinhVucChung = ddlLinhVucChung.SelectedItem.Text.Replace("+ ", string.Empty);
                    eInfoUpdate.LinhVucChungId = Convert.ToInt32(ddlLinhVucChung.SelectedItem.Value);

                    if (ddlDichVuCP.SelectedValue != string.Empty && ddlDichVuCP.SelectedValue != "0")
                    {
                        eInfoUpdate.LinhVucCon = ddlDichVuCP.SelectedItem.Text;
                        eInfoUpdate.LinhVucConId = Convert.ToInt32(ddlDichVuCP.SelectedValue);

                        eInfoUpdate.MaDichVu = eInfoUpdate.LinhVucCon;
                    }
                    else
                    {
                        eInfoUpdate.LinhVucConId = 0;
                        eInfoUpdate.LinhVucCon = string.Empty;

                        eInfoUpdate.MaDichVu = string.Empty;
                    }
                }
                else
                {
                    eInfoUpdate.LinhVucChung = null;
                    eInfoUpdate.LinhVucChungId = 0;

                    eInfoUpdate.LinhVucCon = null;
                    eInfoUpdate.LinhVucConId = 0;

                    eInfoUpdate.MaDichVu = string.Empty;
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        obj.Update(eInfoUpdate);

                        //obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfoUpdate.SoTien, eInfoUpdate.LoaiTien, loaiTienOld, ConvertUtility.ToInt32(ddlLyDoGiamTru.SelectedValue), 2);
                        obj.UpdateToKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), eInfoUpdate.SoTien, eInfoUpdate.LoaiTien, loaiTienOld, 2);


                        scope.Complete();
                    }
                    catch (TransactionAbortedException tae)
                    {
                        Utility.LogEvent(tae);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                        return;
                    }
                }

                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới cước dịch vụ", "KN cước dịch vụ", "",
                                       string.Format("Tiền chỉnh sửa: {0} - Loại tài khoản: {1}",
                                       eInfoUpdate.SoTien_Edit, ((DropDownList)gvKieuNaiCuocDichVu.Rows[e.RowIndex].FindControl("ddlLoaiTien")).SelectedItem.Text));


                gvKieuNaiCuocDichVu.EditIndex = -1;
                FillKieuNaiCuocDichVuGrid();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE + "','error');", true);
            }
        }
        protected void gvKieuNaiCuocDichVu_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvKieuNaiCuocDichVu.EditIndex = -1;
            FillKieuNaiCuocDichVuGrid();
        }
        protected void gvKieuNaiCuocDichVu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_SoTien();
            KhieuNai_SoTienInfo eInfo = new KhieuNai_SoTienInfo();
            try
            {
                var username_add = obj.GetInfo(Convert.ToInt32(gvKieuNaiCuocDichVu.DataKeys[e.RowIndex].Values[0].ToString())).CUser;
                if (ConvertUtility.ToString(username_add) == username)
                {
                    int ID = Convert.ToInt32(gvKieuNaiCuocDichVu.DataKeys[e.RowIndex].Values[0].ToString());
                    if (obj.Delete(ID) > 0)
                        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Xóa", "KN cước dịch vụ", "Xóa dữ liệu", "Xóa dữ liệu");
                    FillKieuNaiCuocDichVuGrid();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvKieuNaiCuocDichVu.PageIndex = e.NewPageIndex;
            FillKieuNaiCuocDichVuGrid();
        }

        protected void gvKieuNaiCuocDichVu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        protected string GetLoaiTien(object obj)
        {
            try
            {
                if (IsTraSau)
                {
                    return Enum.GetName(typeof(KhieuNai_LoaiTien_TraSau), Convert.ToInt32(obj)).Replace("_", " ");
                }
                else
                {
                    return Enum.GetName(typeof(KhieuNai_LoaiTien), Convert.ToInt32(obj)).Replace("_", " ");
                }
            }
            catch { return obj.ToString(); }
        }


        protected void ddlDichVuCPEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var valueSelected = Convert.ToInt32((sender as DropDownList).SelectedValue);

            //GridViewRow editRow = gvKieuNaiCuocDichVu.Rows[gvKieuNaiCuocDichVu.EditIndex];
            //TextBox txtDauSo = editRow.FindControl("txtDauSoCP") as TextBox;

            //if (valueSelected == -80 || valueSelected == -57 || valueSelected == -56 || valueSelected == -54)
            //{
            //    txtDauSo.Enabled = true;
            //}
            //else
            //{
            //    txtDauSo.Enabled = false;
            //}
        }

        protected void ddlDichVuCPFooter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var valueSelected = Convert.ToInt32((sender as DropDownList).SelectedValue);

            ////GridViewRow emptyRow = gvKieuNaiCuocDichVu.Controls[0].Controls[1] as GridViewRow;
            ////TextBox txtDauSo = emptyRow.FindControl("txtDauSoCP") as TextBox;
            ////if (txtDauSo == null)
            ////{
            //GridViewRow footerRow = gvKieuNaiCuocDichVu.FooterRow;
            //TextBox txtDauSo = footerRow.FindControl("txtDauSoCP") as TextBox;
            ////}

            //if (valueSelected == -80 || valueSelected == -57 || valueSelected == -56 || valueSelected == -54)
            //{
            //    txtDauSo.Enabled = true;
            //}
            //else
            //{
            //    txtDauSo.Enabled = false;
            //}
        }

        protected void ddlDichVuCP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var valueSelected = Convert.ToInt32((sender as DropDownList).SelectedValue);

            //GridViewRow emptyRow = gvKieuNaiCuocDichVu.Controls[0].Controls[1] as GridViewRow;
            //TextBox txtDauSo = emptyRow.FindControl("txtDauSoCP") as TextBox;
            ////if (txtDauSo == null)
            ////{
            ////    GridViewRow footerRow = gvKieuNaiCuocDichVu.FooterRow;
            ////    txtDauSo = footerRow.FindControl("txtDauSoCP") as TextBox;
            ////}

            //if (valueSelected == -80 || valueSelected == -57 || valueSelected == -56 || valueSelected == -54)
            //{
            //    txtDauSo.Enabled = true;
            //}
            //else
            //{
            //    txtDauSo.Enabled = false;
            //}
        }

        protected void ddlLinhVucChungEdit_SelectedIndexChanged(object sender, EventArgs e)
        {


            var valueSelected = Convert.ToInt32((sender as DropDownList).SelectedValue);

            GridViewRow editRow = gvKieuNaiCuocDichVu.Rows[gvKieuNaiCuocDichVu.EditIndex];
            DropDownList ddlDichVu = editRow.FindControl("ddlDichVuCP") as DropDownList;

            // Dương DV
            // Bind lĩnh vực con


            DropDownList ddlLinhVucChung = sender as DropDownList;
            string[] dsLoaiKhieuNaiId = ddlLinhVucChung.Attributes["Ids"].ToString().Split(',');
            foreach (ListItem lstItem in ddlLinhVucChung.Items)
            {

                foreach (string val in dsLoaiKhieuNaiId)
                {
                    if (lstItem.Value == val)
                    {
                        lstItem.Attributes.Add("disabled", "disabled");
                        lstItem.Attributes.Add("class", "not-selected");
                    }

                }
            }


            LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

            List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND Status = 1", valueSelected), "Sort");
            ddlDichVu.DataSource = lst;
            ddlDichVu.DataTextField = "Name";
            ddlDichVu.DataValueField = "Id";
            ddlDichVu.DataBind();

            ddlDichVu.Items.Insert(0, new ListItem("-- Chọn --", "0"));

            //var lstDichVu = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("Id, MaDichVu", "TrangThai = 1 And LinhVucChungId =" + valueSelected, "MaDichVu");
            //lstDichVu.Insert(0, new DichVuCPInfo() { Id = 0, MaDichVu = "--Chọn dịch vụ--" });
            //ddlDichVu.DataSource = lstDichVu;
            //ddlDichVu.DataTextField = "MaDichVu";
            //ddlDichVu.DataValueField = "Id";
            //ddlDichVu.DataBind();
            //if (valueSelected == -80 || valueSelected == -57 || valueSelected == -56 || valueSelected == -54)
            //{
            //    txtDauSo.Enabled = true;
            //}
            //else
            //{
            //    txtDauSo.Enabled = false;
            //}

            // Bật tắt kiểm tra validate
            RequiredFieldValidator validateDichVuCP = ((sender as DropDownList).Parent.Parent).FindControl("ValidateDichVuCon") as RequiredFieldValidator;
            if (lst == null || lst.Count() == 0) validateDichVuCP.Visible = false;
            else validateDichVuCP.Visible = true;
        }

        protected void ddlLinhVucChungFooter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLinhVucChung = sender as DropDownList;

            var valueSelected = Convert.ToInt32(ddlLinhVucChung.SelectedValue);

            //GridViewRow emptyRow = gvKieuNaiCuocDichVu.Controls[0].Controls[1] as GridViewRow;
            //TextBox txtDauSo = emptyRow.FindControl("txtDauSoCP") as TextBox;
            //if (txtDauSo == null)
            //{
            GridViewRow footerRow = gvKieuNaiCuocDichVu.FooterRow;
            DropDownList ddlDichVu = footerRow.FindControl("ddlDichVuCP") as DropDownList;
            //}

            //var lstDichVu = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("Id,MaDichVu", "TrangThai = 1 And LinhVucChungId=" + valueSelected, "MaDichVu");
            //lstDichVu.Insert(0, new DichVuCPInfo() { Id = 0, MaDichVu = "--Chọn dịch vụ--" });
            //ddlDichVu.DataSource = lstDichVu;
            //ddlDichVu.DataTextField = "MaDichVu";
            //ddlDichVu.DataValueField = "Id";
            //ddlDichVu.DataBind();

            // Dương DV
            // Bind lĩnh vực con

            string[] dsLoaiKhieuNaiId = ddlLinhVucChung.Attributes["Ids"].ToString().Split(',');
            foreach (ListItem lstItem in ddlLinhVucChung.Items)
            {

                foreach (string val in dsLoaiKhieuNaiId)
                {
                    if (lstItem.Value == val)
                    {
                        lstItem.Attributes.Add("disabled", "disabled");
                        lstItem.Attributes.Add("class", "not-selected");
                    }

                }
            }

            LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

            List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND Status = 1", valueSelected), "Sort");
            ddlDichVu.DataSource = lst;
            ddlDichVu.DataTextField = "Name";
            ddlDichVu.DataValueField = "Id";
            ddlDichVu.DataBind();

            ddlDichVu.Items.Insert(0, new ListItem("-- Chọn --", "0"));

            // Bật tắt kiểm tra validate
            RequiredFieldValidator validateDichVuCP = ((sender as DropDownList).Parent.Parent).FindControl("ValidateDichVuCon") as RequiredFieldValidator;
            if (lst == null || lst.Count() == 0) validateDichVuCP.Visible = false;
            else validateDichVuCP.Visible = true;

        }


        // Emty Gridview
        protected void ddlLinhVucChung_SelectedIndexChanged(object sender, EventArgs e)
        {
            var valueSelected = Convert.ToInt32((sender as DropDownList).SelectedValue);

            GridViewRow emptyRow = gvKieuNaiCuocDichVu.Controls[0].Controls[1] as GridViewRow;
            DropDownList ddlDichVu = emptyRow.FindControl("ddlDichVuCP") as DropDownList;

            #region "Phiên bản cũ"
            //if (txtDauSo == null)
            //{
            //    GridViewRow footerRow = gvKieuNaiCuocDichVu.FooterRow;
            //    txtDauSo = footerRow.FindControl("txtDauSoCP") as TextBox;
            //}

            //var lstDichVu = ServiceFactory.GetInstanceDichVuCP().GetListDynamic("Id,MaDichVu", "TrangThai = 1 And LinhVucChungId=" + valueSelected, "MaDichVu");
            //lstDichVu.Insert(0, new DichVuCPInfo() { Id = 0, MaDichVu = "--Chọn dịch vụ--" });
            //ddlDichVu.DataSource = lstDichVu;
            //ddlDichVu.DataTextField = "MaDichVu";
            //ddlDichVu.DataValueField = "Id";
            //ddlDichVu.DataBind();
            #endregion

            // Dương DV


            LoaiKhieuNaiImpl ctl = new LoaiKhieuNaiImpl();

            // Đánh dấu "Loại Khiếu Nại"
            DropDownList ddlLinhVucChung = sender as DropDownList;
            string[] dsLoaiKhieuNaiId = ddlLinhVucChung.Attributes["Ids"].ToString().Split(',');
            foreach (ListItem lstItem in ddlLinhVucChung.Items)
            {

                foreach (string val in dsLoaiKhieuNaiId)
                {
                    if (lstItem.Value == val)
                    {
                        lstItem.Attributes.Add("disabled", "disabled");
                        lstItem.Attributes.Add("class", "not-selected");
                    }

                }
            }

            // Bind lĩnh vực con
            ddlDichVu.Items.Clear();
            List<LoaiKhieuNaiInfo> lst = ctl.GetListDynamic("*", string.Format("ParentId = {0} AND Status = 1", valueSelected), "Sort");
            ddlDichVu.DataSource = lst;
            ddlDichVu.DataTextField = "Name";
            ddlDichVu.DataValueField = "Id";
            ddlDichVu.DataBind();

            ddlDichVu.Items.Insert(0, new ListItem("-- Chọn --", "0"));

            // Bật tắt kiểm tra validate
            RequiredFieldValidator validateDichVuCP = ((sender as DropDownList).Parent).FindControl("ValidateDichVuCon") as RequiredFieldValidator;
            if (lst == null || lst.Count() == 0) validateDichVuCP.Visible = false;
            else validateDichVuCP.Visible = true;
        }

    }
}