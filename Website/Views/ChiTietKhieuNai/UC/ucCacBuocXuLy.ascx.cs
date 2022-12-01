using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using Website.AppCode.Controller;
using Aspose.Cells;
using AIVietNam.GQKN.Impl;


namespace Website.Views.KhieuNai.UC
{
    public partial class ucCacBuocXuLy : System.Web.UI.UserControl
    {
        protected AdminInfo userlogin;
        public int KhieuNaiId { get; set; }
        public string Mode { get; set; }
        public int reload { get; set; }
        private Boolean IsPageRefresh = false;
        protected bool taomoi = false, sua = false, xoa = false;
        private bool AllowAction = true; //Nếu các bước xử lý sang phòng ban khác sẽ gán = false
        protected string WordAutocomplete = string.Empty;

        private int ArchiveId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            WordAutocomplete = BuildKhieuNai_BuocXuLy_Autocomplete.BuildWord();
            userlogin = LoginAdmin.AdminLogin();

            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            Mode = ConvertUtility.ToString(Request.QueryString["Mode"]).ToLower();
            if (Mode != "view")
            {
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tạo_các_bước_xử_lý))
                    taomoi = true;
            }
            if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_các_bước_xử_lý)) xoa = true;
            if (KhieuNaiId != 0)
            {
                if (!IsPostBack)
                {
                    ViewState["BuocXuLy"] = System.Guid.NewGuid().ToString();
                    Session["BuocXuLy"] = ConvertUtility.ToString(ViewState["BuocXuLy"]);
                    FillBuocXuLyGrid();
                }
                else
                {
                    if (ConvertUtility.ToString(ViewState["BuocXuLy"]) != ConvertUtility.ToString(Session["BuocXuLy"]))
                        IsPageRefresh = true;
                    Session["BuocXuLy"] = System.Guid.NewGuid().ToString();
                    ViewState["BuocXuLy"] = Session["BuocXuLy"];
                }
            }
        }
        public void FillBuocXuLyGrid()
        {
            try
            {
                if (Mode == "view")
                {
                    gvCacBuocXuLy.EditRowStyle.CssClass = "dpn";
                    gvCacBuocXuLy.FooterStyle.CssClass = "dpn";
                }

                string whereClause = "IsAuto=0 AND KhieuNaiId = " + KhieuNaiId;
                List<KhieuNai_BuocXuLyInfo> objBuocXuLyList = new List<KhieuNai_BuocXuLyInfo>();
                objBuocXuLyList = ServiceFactory.GetInstanceKhieuNai_BuocXuLy(ArchiveId).GetListDynamic("", whereClause, "CDate DESC");
                if (objBuocXuLyList.Any())
                {
                    hidNguoiXuLy.Value = objBuocXuLyList[0].CUser;
                }

                gvCacBuocXuLy.DataSource = objBuocXuLyList;
                gvCacBuocXuLy.DataBind();
            }
            finally
            {

                //ScriptManager.RegisterClientScriptBlock(UPCacBuocXuLy, UPCacBuocXuLy.GetType(), "onloadTooltip", "ShowToolTip();", true);
            }
        }

        protected void gvCacBuocXuLy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var hdUser = e.Row.FindControl("hdCUser") as HiddenField;
                var hdPhongBanXuLy = e.Row.FindControl("hdPhongBanXuLyId") as HiddenField;

                //if (AllowAction && ConvertUtility.ToInt32(hdPhongBanXuLy.Value) != userlogin.PhongBanId)
                if (AllowAction && hdUser.Value != userlogin.Username)
                    AllowAction = false;

                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    var btnEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                    var btnDel = e.Row.FindControl("lnkDelete") as LinkButton;
                    btnEdit.Visible = false;
                    btnDel.Visible = false;
                    if (Mode != "view")
                    {
                        if (hdUser.Value == userlogin.Username && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_các_bước_xử_lý_của_mình))
                        {
                            if (AllowAction)
                                btnEdit.Visible = true;
                        }
                        if (xoa || (hdUser.Value == userlogin.Username && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_các_bước_xử_lý_của_mình)))
                        {
                            if (AllowAction)
                                btnDel.Visible = true;
                        }
                    }
                }
                else if (e.Row.RowState == DataControlRowState.Edit)
                {
                    var btnDel = e.Row.FindControl("lnkDelete") as LinkButton;
                    btnDel.Visible = false;
                    if (Mode != "view")
                    {
                        if (xoa || (hdUser.Value == userlogin.Username && BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_các_bước_xử_lý_của_mình)))
                        {
                            btnDel.Visible = true;
                        }
                        var ctrlNoiDung = e.Row.FindControl("txtNoiDung") as System.Web.UI.WebControls.TextBox;
                        ScriptManager.RegisterClientScriptBlock(UPCacBuocXuLy, UPCacBuocXuLy.GetType(), "onloadAutocomplete", "AutocompleteText('" + ctrlNoiDung.ClientID + "')", true);
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                if (Mode == "view")
                {
                    var groupValid = e.Row.FindControl("vsInsertCacBuocXuLy") as ValidationSummary;
                    if (groupValid != null)
                    {
                        groupValid.Enabled = false;
                    }
                }
                //else
                //{
                //    //var ctrlNoiDung = e.Row.FindControl("txtNoiDung") as System.Web.UI.WebControls.TextBox;
                //    //if (ctrlNoiDung != null)
                //    //    ScriptManager.RegisterClientScriptBlock(UPCacBuocXuLy, UPCacBuocXuLy.GetType(), "onloadAutocomplete", "AutocompleteText(" + ctrlNoiDung.ClientID + ")", true);
                //}
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Mode == "view")
                {
                    var groupValid = e.Row.FindControl("vsInsertCacBuocXuLy") as ValidationSummary;
                    if (groupValid != null)
                    {
                        groupValid.Enabled = false;
                    }

                    LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                    if (btAddFooter != null)
                        btAddFooter.Visible = false;
                }
                else
                {
                    if (!taomoi)
                    {
                        gvCacBuocXuLy.FooterStyle.CssClass = "dpn";
                    }
                }
            }
        }
        protected void gvCacBuocXuLy_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var flagCallAutoComplete = false;
            try
            {
                if (!IsPageRefresh) // check that page is not refreshed by browser.
                {
                    if (e.CommandName.Equals("Insert"))
                    {
                        try
                        {
                            flagCallAutoComplete = true;
                            if (!taomoi)
                                return;
                            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy();
                            KhieuNai_BuocXuLyInfo eInfo = new KhieuNai_BuocXuLyInfo();
                            eInfo.NoiDung = ((System.Web.UI.WebControls.TextBox)gvCacBuocXuLy.FooterRow.FindControl("txtNoiDung")).Text;
                            eInfo.LUser = userlogin.Username;
                            eInfo.KhieuNaiId = KhieuNaiId;
                            eInfo.NguoiXuLyId = userlogin.Id;
                            eInfo.PhongBanXuLyId = userlogin.PhongBanId;
                            if (obj.Add(eInfo) > 0)
                                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới bước xử lý", "Các bước xử lý", "", eInfo.NoiDung);
                            FillBuocXuLyGrid();
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                        }
                    }
                    else if (e.CommandName.Equals("emptyInsert"))
                    {
                        try
                        {
                            flagCallAutoComplete = true;
                            if (!taomoi)
                                return;
                            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy();
                            KhieuNai_BuocXuLyInfo eInfo = new KhieuNai_BuocXuLyInfo();
                            GridViewRow emptyRow = gvCacBuocXuLy.Controls[0].Controls[1] as GridViewRow;
                            eInfo.NoiDung = ConvertUtility.ToString(((System.Web.UI.WebControls.TextBox)emptyRow.FindControl("txtNoiDung")).Text);
                            eInfo.LUser = userlogin.Username;
                            eInfo.KhieuNaiId = KhieuNaiId;
                            eInfo.NguoiXuLyId = userlogin.Id;
                            eInfo.PhongBanXuLyId = userlogin.PhongBanId;
                            if (obj.Add(eInfo) > 0)
                                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới bước xử lý", "Các bước xử lý", "", eInfo.NoiDung);
                            FillBuocXuLyGrid();
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                        }
                    }
                    else if (e.CommandName.Equals("Cancel"))
                    {
                        flagCallAutoComplete = true;
                    }
                }
                else
                    FillBuocXuLyGrid();
            }
            catch { }
            finally
            {
                if (flagCallAutoComplete)
                    ScriptManager.RegisterClientScriptBlock(UPCacBuocXuLy, UPCacBuocXuLy.GetType(), "onloadAutocomplete", "AutocompleteText('ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung')", true);
            }
        }
        protected void gvCacBuocXuLy_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCacBuocXuLy.EditIndex = e.NewEditIndex;
            FillBuocXuLyGrid();
        }
        protected void gvCacBuocXuLy_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy();
            KhieuNai_BuocXuLyInfo eInfo = obj.GetInfo(Convert.ToInt32(gvCacBuocXuLy.DataKeys[e.RowIndex].Values[0].ToString()));
            if (eInfo != null)
            {
                eInfo.NoiDung = Convert.ToString(((System.Web.UI.WebControls.TextBox)gvCacBuocXuLy.Rows[e.RowIndex].FindControl("txtNoiDung")).Text);
                eInfo.LUser = userlogin.Username;

                if (obj.Update(eInfo) > 0)
                    BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Cập nhật", "Các bước xử lý", "", eInfo.NoiDung);
                gvCacBuocXuLy.EditIndex = -1;
                FillBuocXuLyGrid();
                ScriptManager.RegisterClientScriptBlock(UPCacBuocXuLy, UPCacBuocXuLy.GetType(), "onloadAutocomplete", "AutocompleteText('ContentPlaceHolder_Main_ContentPlaceHolder_Text_ucChiTietKhieuNai_TabContainer1_TabPanel1_UcCacBuocXuLy_gvCacBuocXuLy_txtNoiDung')", true);
            }
        }
        protected void gvCacBuocXuLy_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCacBuocXuLy.EditIndex = -1;
            FillBuocXuLyGrid();
        }
        protected void gvCacBuocXuLy_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_BuocXuLy();
            KhieuNai_BuocXuLyInfo eInfo = new KhieuNai_BuocXuLyInfo();
            int ID = Convert.ToInt32(gvCacBuocXuLy.DataKeys[e.RowIndex].Values[0].ToString());
            if (obj.Delete(ID) > 0)
                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Xóa", "Các bước xử lý", "Xóa dữ liệu", "Xóa dữ liệu");
            FillBuocXuLyGrid();
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvCacBuocXuLy.PageIndex = e.NewPageIndex;
            FillBuocXuLyGrid();
        }

        protected void gvCacBuocXuLy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/04/2014
        /// Todo : Xuất excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbExportExcel_Click(object sender, EventArgs e)
        {
            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_BuocXuLyInfo> objBuocXuLyList = new List<KhieuNai_BuocXuLyInfo>();
            objBuocXuLyList = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic("", whereClause, "CDate DESC");

            #region ExportExcel
            Workbook workbookExport = new Workbook();
            Worksheet sheetExport = workbookExport.Worksheets[0];

            Workbook workbookTemp = new Workbook();
            Worksheet sheet = null;

            string path = HttpContext.Current.Server.MapPath("~/ExportExcel");
            path += @"\Template\CacBuocXuLy.xlsx";
            workbookTemp.Open(path);
            sheet = workbookTemp.Worksheets[0];

            sheet.Cells[2, 2].PutValue(KhieuNaiId);
            KhieuNaiInfo khieuNaiInfo = new KhieuNaiImpl().GetInfo(KhieuNaiId);
            string soThueBao = khieuNaiInfo != null ? "0" + khieuNaiInfo.SoThueBao.ToString() : string.Empty;
            sheet.Cells[3, 2].PutValue(soThueBao);

            sheet.Cells.DeleteRows(7, sheet.Cells.Rows.Count);
            int RowIndex = 7;

            if (objBuocXuLyList != null && objBuocXuLyList.Count > 0)
            {
                if (!string.IsNullOrEmpty(sheet.Cells[0, 0].StringValue))
                {
                    sheet.Cells[0, 0].PutValue(sheet.Cells[0, 0].StringValue + "");
                    Aspose.Cells.Style style = sheet.Cells[0, 0].GetStyle();
                    style.IsTextWrapped = true;
                    sheet.Cells[0, 0].SetStyle(style);
                }

                for (int i = 0; i < objBuocXuLyList.Count; i++)
                {
                    sheet.Cells[RowIndex, 0].PutValue((i + 1));
                    sheet.Cells[RowIndex, 0].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 1].PutValue(objBuocXuLyList[i].NoiDung);
                    sheet.Cells[RowIndex, 1].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 2].PutValue(objBuocXuLyList[i].CUser);
                    sheet.Cells[RowIndex, 2].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 3].PutValue(objBuocXuLyList[i].CDate.ToString("dd/MM/yyyy HH:mm"));
                    sheet.Cells[RowIndex, 3].SetStyle(StyleCell());

                    sheet.Cells[RowIndex, 4].PutValue(objBuocXuLyList[i].IsAuto);
                    sheet.Cells[RowIndex, 4].SetStyle(StyleCell());

                    RowIndex++;
                }

            }

            string fileName = "CacBuocXuLy" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + DateTime.Now.Millisecond + ".xls";
            string pathSave = HttpContext.Current.Server.MapPath("~/ExportExcel") + @"\\Excel";
            string pathChild = "";
            if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Year.ToString()))
            {
                Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Year.ToString());
                pathSave += "\\" + DateTime.Now.Year.ToString();
                pathChild += "/" + DateTime.Now.Year.ToString();
            }
            else
            {
                pathSave += "\\" + DateTime.Now.Year.ToString();
                pathChild += "/" + DateTime.Now.Year.ToString();
            }

            if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Month.ToString()))
            {
                Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Month.ToString());
                pathSave += "\\" + DateTime.Now.Month.ToString();
                pathChild += "/" + DateTime.Now.Month.ToString();
            }
            else
            {
                pathSave += "\\" + DateTime.Now.Month.ToString();
                pathChild += "/" + DateTime.Now.Month.ToString();
            }

            if (!Directory.Exists(pathSave + "\\" + DateTime.Now.Day.ToString()))
            {
                Directory.CreateDirectory(pathSave + "\\" + DateTime.Now.Day.ToString());
                pathSave += "\\" + DateTime.Now.Day.ToString();
                pathChild += "/" + DateTime.Now.Day.ToString();
            }
            else
            {
                pathSave += "\\" + DateTime.Now.Day.ToString();
                pathChild += "/" + DateTime.Now.Day.ToString();
            }

            pathSave += "\\" + fileName;
            workbookTemp.Save(pathSave, SaveFormat.Excel97To2003);

            #endregion

            string script = string.Format("<script type='text/javascript'> window.location.href = \"/ExportExcel/Excel{0}/{1}\";</script>", pathChild, fileName);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        private Aspose.Cells.Style StyleCell()
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            Aspose.Cells.Cell cell = worksheet.Cells["A1"];
            Aspose.Cells.Style style = cell.GetStyle();

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Black;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Black;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.Black;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Black;
            return style;
        }
    }
}