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
using Website.AppCode;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucTimKiemGiaiPhap : System.Web.UI.UserControl
    {
        public string username = "";
        public int KhieuNaiId { get; set; }
        private Boolean IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            var obj_admin = LoginAdmin.AdminLogin();
            username = obj_admin.Username;

            // KieuNaiId test
            //KhieuNaiId = 3;
            if (!IsPostBack)
            {
                ViewState["TimKiemGiaiPhap"] = System.Guid.NewGuid().ToString();
                Session["TimKiemGiaiPhap"] = ConvertUtility.ToString(ViewState["TimKiemGiaiPhap"]);
                FillTimKiemGiaiPhapGrid();
            }
            else
            {
                if (ConvertUtility.ToString(ViewState["TimKiemGiaiPhap"]) != ConvertUtility.ToString(Session["TimKiemGiaiPhap"]))
                    IsPageRefresh = true;
                Session["TimKiemGiaiPhap"] = System.Guid.NewGuid().ToString();
                ViewState["TimKiemGiaiPhap"] = Session["TimKiemGiaiPhap"];
            }
        }
        private void FillTimKiemGiaiPhapGrid()
        {
            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_GiaiPhapInfo> objBuocXuLyList = new List<KhieuNai_GiaiPhapInfo>();
            objBuocXuLyList = ServiceFactory.GetInstanceKhieuNai_GiaiPhap().GetListDynamic("", whereClause, "");
            gvTimKiemGiaiPhap.DataSource = objBuocXuLyList;
            gvTimKiemGiaiPhap.DataBind();
        }
        protected void gvTimKiemGiaiPhap_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void gvTimKiemGiaiPhap_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!IsPageRefresh) // check that page is not refreshed by browser.
            {
                if (e.CommandName.Equals("Insert"))
                {
                    try
                    {
                        var obj = ServiceFactory.GetInstanceKhieuNai_GiaiPhap();
                        KhieuNai_GiaiPhapInfo eInfo = new KhieuNai_GiaiPhapInfo();
                        eInfo.Name = ((TextBox)gvTimKiemGiaiPhap.FooterRow.FindControl("txtName")).Text;
                        eInfo.FAQ = ((TextBox)gvTimKiemGiaiPhap.FooterRow.FindControl("txtFAQ")).Text;
                        eInfo.MoTa = ((TextBox)gvTimKiemGiaiPhap.FooterRow.FindControl("txtMoTa")).Text;
                        eInfo.Comments = ((TextBox)gvTimKiemGiaiPhap.FooterRow.FindControl("txtComments")).Text;
                        eInfo.CUser = username;
                        eInfo.CDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("vi-VN"));
                        eInfo.KhieuNaiId = KhieuNaiId;

                        obj.Add(eInfo);
                        FillTimKiemGiaiPhapGrid();
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
                        var obj = ServiceFactory.GetInstanceKhieuNai_GiaiPhap();
                        KhieuNai_GiaiPhapInfo eInfo = new KhieuNai_GiaiPhapInfo();
                        GridViewRow emptyRow = gvTimKiemGiaiPhap.Controls[0].Controls[1] as GridViewRow;
                        eInfo.Name = ConvertUtility.ToString(((TextBox)emptyRow.FindControl("txtName")).Text);
                        eInfo.FAQ = ((TextBox)emptyRow.FindControl("txtFAQ")).Text;
                        eInfo.MoTa = ((TextBox)emptyRow.FindControl("txtMoTa")).Text;
                        eInfo.Comments = ((TextBox)emptyRow.FindControl("txtComments")).Text;
                        eInfo.CUser = username;
                        eInfo.CDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("vi-VN"));
                        eInfo.KhieuNaiId = KhieuNaiId;

                        obj.Add(eInfo);
                        FillTimKiemGiaiPhapGrid();
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                    }
                }
            }
            else
                FillTimKiemGiaiPhapGrid();
        }
        protected void gvTimKiemGiaiPhap_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTimKiemGiaiPhap.EditIndex = e.NewEditIndex;
            FillTimKiemGiaiPhapGrid();
        }
        protected void gvTimKiemGiaiPhap_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var obj = ServiceFactory.GetInstanceKhieuNai_GiaiPhap();
                KhieuNai_GiaiPhapInfo eInfo = new KhieuNai_GiaiPhapInfo();
                eInfo.Id = Convert.ToInt64(gvTimKiemGiaiPhap.DataKeys[e.RowIndex].Values[0].ToString());
                eInfo.Name = Convert.ToString(((TextBox)gvTimKiemGiaiPhap.Rows[e.RowIndex].FindControl("txtName")).Text);
                eInfo.FAQ = ((TextBox)gvTimKiemGiaiPhap.Rows[e.RowIndex].FindControl("txtFAQ")).Text;
                eInfo.MoTa = ((TextBox)gvTimKiemGiaiPhap.Rows[e.RowIndex].FindControl("txtMoTa")).Text;
                eInfo.Comments = ((TextBox)gvTimKiemGiaiPhap.Rows[e.RowIndex].FindControl("txtComments")).Text;
                eInfo.CUser = username;
                eInfo.CDate = Convert.ToDateTime(DateTime.Now, new CultureInfo("vi-VN"));
                eInfo.KhieuNaiId = KhieuNaiId;

                obj.Update(eInfo);
                gvTimKiemGiaiPhap.EditIndex = -1;
                FillTimKiemGiaiPhapGrid();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }
        protected void gvTimKiemGiaiPhap_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTimKiemGiaiPhap.EditIndex = -1;
            FillTimKiemGiaiPhapGrid();
        }
        protected void gvTimKiemGiaiPhap_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_GiaiPhap();
            KhieuNai_GiaiPhapInfo eInfo = new KhieuNai_GiaiPhapInfo();
            try
            {
                var username_add = obj.GetInfo(Convert.ToInt32(gvTimKiemGiaiPhap.DataKeys[e.RowIndex].Values[0].ToString())).CUser;
                if (ConvertUtility.ToString(username_add) == username)
                {
                    int ID = Convert.ToInt32(gvTimKiemGiaiPhap.DataKeys[e.RowIndex].Values[0].ToString());
                    obj.Delete(ID);
                    FillTimKiemGiaiPhapGrid();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvTimKiemGiaiPhap.PageIndex = e.NewPageIndex;
            FillTimKiemGiaiPhapGrid();
        }

        protected void gvTimKiemGiaiPhap_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }
    }
}