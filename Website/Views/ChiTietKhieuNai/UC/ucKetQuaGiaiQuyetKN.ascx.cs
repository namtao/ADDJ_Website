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

namespace Website.Views.KhieuNai.UC
{
    public partial class ucKetQuaGiaiQuyetKN : System.Web.UI.UserControl
    {
        public int KhieuNaiId { get; set; }
        public string Mode { get; set; }
        private Boolean IsPageRefresh = false;
        private AdminInfo login = null;

        protected bool taomoi = false;
        private bool IsCreateKQGQ = true;
        private int ArchiveId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            login = LoginAdmin.AdminLogin();

            Mode = ConvertUtility.ToString(Request.QueryString["Mode"]).ToLower();
            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            if (Mode != "view")
            {
                if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tạo_kết_quả_giải_quyết_KN))
                    taomoi = true;
            }

            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
        }

        public void FillKetQuaGiaiQuyetKNGrid()
        {
            if (Mode == "view")
            {
                gvKetQuaGiaiQuyetKN.EditRowStyle.CssClass = "dpn";
                gvKetQuaGiaiQuyetKN.FooterStyle.CssClass = "dpn";
            }

            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_KetQuaXuLyInfo> objKetQuaGiaiQuyetKNList = new List<KhieuNai_KetQuaXuLyInfo>();
            objKetQuaGiaiQuyetKNList = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy(ArchiveId).GetListDynamic("", whereClause, "LDate DESC");
            gvKetQuaGiaiQuyetKN.DataSource = objKetQuaGiaiQuyetKNList;
            gvKetQuaGiaiQuyetKN.DataBind();
        }

        protected void gvKetQuaGiaiQuyetKN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Check quyền
                var hdUser = e.Row.FindControl("hdCUser") as HiddenField;
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    var btnEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                    var btnDel = e.Row.FindControl("lnkDelete") as LinkButton;
                    var hdPhongBanXuLy = e.Row.FindControl("hdPhongBanXuLyId") as HiddenField;

                    if (hdPhongBanXuLy != null)
                    {
                        if (hdPhongBanXuLy.Value.Equals(login.PhongBanId.ToString()))
                            IsCreateKQGQ = false;
                    }

                    btnEdit.Visible = false;
                    btnDel.Visible = false;

                    if (Mode != "view")
                    {
                        if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_kết_quả_giải_quyết_KN_của_mình)) && hdUser.Value == login.Username)
                        {
                            btnEdit.Visible = true;
                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Sửa_kết_quả_giải_quyết_KN_của_người_khác))
                        {
                            btnEdit.Visible = true;
                        }

                        if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_kết_quả_giải_quyết_KN_của_mình) && hdUser.Value == login.Username))
                        {
                            btnDel.Visible = true;
                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_kết_quả_giải_quyết_KN))
                        {
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
                        if ((BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_kết_quả_giải_quyết_KN_của_mình) && hdUser.Value == login.Username))
                        {
                            btnDel.Visible = true;
                        }
                        else if (BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xóa_tất_cả_kết_quả_giải_quyết_KN))
                        {
                            btnDel.Visible = true;
                        }
                    }
                }

                Label lbIsCSL = (Label)e.Row.FindControl("lbIsCSL");
                if (lbIsCSL != null && ConvertUtility.ToString(lbIsCSL.Text.ToLower()) == "true")
                    lbIsCSL.Text = "CSL";
                else if (lbIsCSL != null)
                    lbIsCSL.Text = "";

                Label lbIsCCT = (Label)e.Row.FindControl("lbIsCCT");
                if (lbIsCCT != null && ConvertUtility.ToString(lbIsCCT.Text.ToLower()) == "true")
                    lbIsCCT.Text = "CCT";
                else if (lbIsCCT != null)
                    lbIsCCT.Text = "";

                Label lbPTSoLieu_IR = (Label)e.Row.FindControl("lbPTSoLieu_IR");
                if (lbPTSoLieu_IR != null && ConvertUtility.ToString(lbPTSoLieu_IR.Text.ToLower()) == "true")
                    lbPTSoLieu_IR.Text = "IR";
                else if (lbPTSoLieu_IR != null)
                    lbPTSoLieu_IR.Text = "";
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                if (!IsCreateKQGQ || !taomoi)
                {
                    LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                    if (btAddFooter != null)
                        btAddFooter.Visible = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Mode == "process")
                {
                    if (!IsCreateKQGQ || !taomoi)
                    {
                        gvKetQuaGiaiQuyetKN.FooterStyle.CssClass = "dpn";
                        LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                        if (btAddFooter != null)
                            btAddFooter.Visible = false;
                    }
                }
                else
                {
                    LinkButton btAddFooter = e.Row.FindControl("lnkAdd") as LinkButton;
                    if (btAddFooter != null)
                        btAddFooter.Visible = false;
                }
            }
        }
        protected void gvKetQuaGiaiQuyetKN_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!IsPageRefresh) // check that page is not refreshed by browser.
            {
                var obj = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy();
                KhieuNai_KetQuaXuLyInfo eInfo = new KhieuNai_KetQuaXuLyInfo();
                if (e.CommandName.Equals("Insert"))
                {
                    try
                    {
                        if (!taomoi)
                            return;

                        GridViewRow insertRow = gvKetQuaGiaiQuyetKN.FooterRow as GridViewRow;

                        eInfo.PhongBanXuLyId = login.PhongBanId;
                        eInfo.PhongBanXuLyName = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(login.PhongBanId);

                        var cbCSL = (CheckBox)insertRow.FindControl("cbIsCSL");
                        if (cbCSL != null && cbCSL.Checked)
                            eInfo.IsCSL = true;

                        var cbIR = (CheckBox)insertRow.FindControl("cbIsIR");
                        if (cbIR != null && cbIR.Checked)
                            eInfo.PTSoLieu_IR = true;

                        var txtPTSL_Khac = (TextBox)insertRow.FindControl("txtPTSL_Khac");
                        if (txtPTSL_Khac != null)
                            eInfo.PTSoLieu_Khac = txtPTSL_Khac.Text;

                        var cbCCT = (CheckBox)insertRow.FindControl("cbIsCCT");
                        if (cbCCT != null && cbCCT.Checked)
                            eInfo.IsCCT = true;

                        var txtSHCV = (TextBox)insertRow.FindControl("txtSHCV");
                        if (txtSHCV != null)
                            eInfo.SHCV = txtSHCV.Text;
                        eInfo.NoiDungXuLy = ((TextBox)insertRow.FindControl("txtNoiDungXuLy")).Text;
                        eInfo.KetQuaXuLy = ((TextBox)insertRow.FindControl("txtKetQuaXuLy")).Text;
                        eInfo.LUser = login.Username;
                        eInfo.KhieuNaiId = KhieuNaiId;

                        try
                        {
                            obj.Add(eInfo);

                            obj.UpdateToKhieuNai(eInfo.KhieuNaiId, true);

                            BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới kết quả GQKN", "Kết quả xử lý", "",
                                string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3} - Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                                eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                            return;
                        }

                        //using (TransactionScope scope = new TransactionScope())
                        //{
                        //    try
                        //    {
                        //        obj.Add(eInfo);

                        //        //obj.UpdateToKhieuNai(eInfo.KhieuNaiId, eInfo.IsCCT, eInfo.IsCSL, eInfo.PTSoLieu_IR, eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.KetQuaXuLy, eInfo.NoiDungXuLy, true);

                        //        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới kết quả GQKN", "Kết quả xử lý", "",
                        //            string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3} - Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                        //            eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));

                        //        scope.Complete();
                        //    }
                        //    catch (TransactionAbortedException tae)
                        //    {
                        //        Utility.LogEvent(tae);
                        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                        //        return;
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Utility.LogEvent(ex);
                        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                        //        return;
                        //    }
                        //}

                        FillKetQuaGiaiQuyetKNGrid();
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                    }
                    finally
                    {

                    }

                }
                else
                    if (e.CommandName.Equals("emptyInsert"))
                    {
                        try
                        {
                            GridViewRow emptyRow = gvKetQuaGiaiQuyetKN.Controls[0].Controls[1] as GridViewRow;

                            eInfo.PhongBanXuLyId = login.PhongBanId;
                            eInfo.PhongBanXuLyName = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(login.PhongBanId);

                            var cbCSL = (CheckBox)emptyRow.FindControl("cbIsCSL");
                            if (cbCSL != null && cbCSL.Checked)
                                eInfo.IsCSL = true;

                            var cbIR = (CheckBox)emptyRow.FindControl("cbIsIR");
                            if (cbIR != null && cbIR.Checked)
                                eInfo.PTSoLieu_IR = true;

                            var txtPTSL_Khac = (TextBox)emptyRow.FindControl("txtPTSL_Khac");
                            if (txtPTSL_Khac != null)
                                eInfo.PTSoLieu_Khac = txtPTSL_Khac.Text;

                            var cbCCT = (CheckBox)emptyRow.FindControl("cbIsCCT");
                            if (cbCCT != null && cbCCT.Checked)
                                eInfo.IsCCT = true;

                            var txtSHCV = (TextBox)emptyRow.FindControl("txtSHCV");
                            if (txtSHCV != null)
                                eInfo.SHCV = txtSHCV.Text;
                            eInfo.NoiDungXuLy = ((TextBox)emptyRow.FindControl("txtNoiDungXuLy")).Text;
                            eInfo.KetQuaXuLy = ((TextBox)emptyRow.FindControl("txtKetQuaXuLy")).Text;
                            eInfo.LUser = login.Username;
                            eInfo.KhieuNaiId = KhieuNaiId;

                            try
                            {
                                obj.Add(eInfo);

                                obj.UpdateToKhieuNai(eInfo.KhieuNaiId, true);

                                BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới kết quả GQKN", "Kết quả xử lý", "",
                                    string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3}- Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                                    eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));

                            }
                            catch (Exception ex)
                            {

                                Utility.LogEvent(ex);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                                return;
                            }
                            //using (TransactionScope scope = new TransactionScope())
                            //{
                            //    try
                            //    {
                            //        obj.Add(eInfo);

                            //        obj.UpdateToKhieuNai(eInfo.KhieuNaiId, eInfo.IsCCT, eInfo.IsCSL, eInfo.PTSoLieu_IR, eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.KetQuaXuLy, eInfo.NoiDungXuLy, true);

                            //        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Thêm mới kết quả GQKN", "Kết quả xử lý", "",
                            //            string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3} - Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                            //            eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));

                            //        scope.Complete();
                            //    }
                            //    catch (TransactionAbortedException tae)
                            //    {
                            //        Utility.LogEvent(tae);
                            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                            //        return;
                            //    }
                            //    catch (Exception ex)
                            //    {

                            //        Utility.LogEvent(ex);
                            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                            //        return;
                            //    }
                            //}

                            FillKetQuaGiaiQuyetKNGrid();
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE + "','error');", true);
                        }
                        finally
                        {

                        }
                    }
            }
            else
                FillKetQuaGiaiQuyetKNGrid();
        }

        protected void gvKetQuaGiaiQuyetKN_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvKetQuaGiaiQuyetKN.EditIndex = e.NewEditIndex;
            FillKetQuaGiaiQuyetKNGrid();
        }
        protected void gvKetQuaGiaiQuyetKN_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy();
            KhieuNai_KetQuaXuLyInfo eInfo = new KhieuNai_KetQuaXuLyInfo();
            try
            {
                eInfo.Id = Convert.ToInt64(gvKetQuaGiaiQuyetKN.DataKeys[e.RowIndex].Values[0].ToString());
                var rowUpdate = gvKetQuaGiaiQuyetKN.Rows[e.RowIndex];

                eInfo.PhongBanXuLyId = login.PhongBanId;
                eInfo.PhongBanXuLyName = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(login.PhongBanId);

                var cbCSL = (CheckBox)rowUpdate.FindControl("cbIsCSL");
                if (cbCSL != null && cbCSL.Checked)
                    eInfo.IsCSL = true;

                var cbIR = (CheckBox)rowUpdate.FindControl("cbIsIR");
                if (cbIR != null && cbIR.Checked)
                    eInfo.PTSoLieu_IR = true;

                var txtPTSL_Khac = (TextBox)rowUpdate.FindControl("txtPTSL_Khac");
                if (txtPTSL_Khac != null)
                    eInfo.PTSoLieu_Khac = txtPTSL_Khac.Text;

                var cbCCT = (CheckBox)rowUpdate.FindControl("cbIsCCT");
                if (cbCCT != null && cbCCT.Checked)
                    eInfo.IsCCT = true;

                var txtSHCV = (TextBox)rowUpdate.FindControl("txtSHCV");
                if (txtSHCV != null)
                    eInfo.SHCV = txtSHCV.Text;
                eInfo.NoiDungXuLy = ((TextBox)rowUpdate.FindControl("txtNoiDungXuLy")).Text;
                eInfo.KetQuaXuLy = ((TextBox)rowUpdate.FindControl("txtKetQuaXuLy")).Text;
                eInfo.LUser = login.Username;
                eInfo.KhieuNaiId = KhieuNaiId;


                try
                {
                    obj.Update(eInfo);

                    obj.UpdateToKhieuNai(eInfo.KhieuNaiId, true);

                    BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Sửa", "Kết quả xử lý", "",
                           string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3}- Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                           eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));

                }
                catch (Exception ex)
                {

                    Utility.LogEvent(ex);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                    return;
                }
                //using (TransactionScope scope = new TransactionScope())
                //{
                //    try
                //    {
                //        obj.Update(eInfo);

                //        obj.UpdateToKhieuNai(eInfo.KhieuNaiId, eInfo.IsCCT, eInfo.IsCSL, eInfo.PTSoLieu_IR, eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.KetQuaXuLy, eInfo.NoiDungXuLy, true);

                //        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Sửa", "Kết quả xử lý", "",
                //               string.Format("Cấp số liệu: {0} - IR: {1} - DV khác: {2} - Số hiệu công văn: {3} - Nội dung xử lý: {4} - Kết quả xử lý: {5}",
                //               eInfo.IsCSL ? "CSL" : "", eInfo.PTSoLieu_IR ? "IR" : "", eInfo.PTSoLieu_Khac, eInfo.SHCV, eInfo.NoiDungXuLy, eInfo.KetQuaXuLy));

                //        scope.Complete();
                //    }
                //    catch (TransactionAbortedException tae)
                //    {
                //        Utility.LogEvent(tae);
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                //        return;
                //    }
                //    catch (Exception ex)
                //    {

                //        Utility.LogEvent(ex);
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                //        return;
                //    }
                //}

                gvKetQuaGiaiQuyetKN.EditIndex = -1;
                FillKetQuaGiaiQuyetKNGrid();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_DU_LIEU_CHUA_HOP_LE + "','error');", true);
            }
        }
        protected void gvKetQuaGiaiQuyetKN_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvKetQuaGiaiQuyetKN.EditIndex = -1;
            FillKetQuaGiaiQuyetKNGrid();
        }
        protected void gvKetQuaGiaiQuyetKN_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_KetQuaXuLy();
            KhieuNai_KetQuaXuLyInfo eInfo = new KhieuNai_KetQuaXuLyInfo();
            try
            {
                int ID = Convert.ToInt32(gvKetQuaGiaiQuyetKN.DataKeys[e.RowIndex].Values[0].ToString());

                try
                {
                    obj.Delete(ID);
                    BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Xóa", "Kết quả giải quyết KN", "Xóa dữ liệu", "Xóa dữ liệu");

                    obj.UpdateToKhieuNai(eInfo.KhieuNaiId, false);
                }
                catch (Exception ex)
                {

                    Utility.LogEvent(ex);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                    return;
                }
                //using (TransactionScope scope = new TransactionScope())
                //{
                //    try
                //    {
                //        obj.Delete(ID);
                //        BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(KhieuNaiId), "Xóa", "Kết quả giải quyết KN", "Xóa dữ liệu", "Xóa dữ liệu");

                //        obj.UpdateToKhieuNai(eInfo.KhieuNaiId, false, false, false, string.Empty, string.Empty, string.Empty, string.Empty, false);
                //        scope.Complete();
                //    }
                //    catch (TransactionAbortedException tae)
                //    {
                //        Utility.LogEvent(tae);
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + Constant.MESSAGE_SERVER_QUA_TAI + "','error');", true);
                //        return;
                //    }
                //    catch (Exception ex)
                //    {

                //        Utility.LogEvent(ex);
                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                //        return;
                //    }
                //}

                FillKetQuaGiaiQuyetKNGrid();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvKetQuaGiaiQuyetKN.PageIndex = e.NewPageIndex;
            FillKetQuaGiaiQuyetKNGrid();
        }

        protected void gvKetQuaGiaiQuyetKN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }
    }
}