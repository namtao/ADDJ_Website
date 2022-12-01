using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADDJ.Log.Entity;
using ADDJ.Admin;
using ADDJ.Core;
using System.Data;
using System.Globalization;
using Website.AppCode;

namespace Website.admin
{

    //[PageStateAdapter.PageViewStateStorage(PageStateAdapter.StateStorageTypes.InDatabase)]
    public partial class LogManager : AppCode.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                grvView.PageSize = Config.RecordPerPage;
                if (Request.QueryString["page"] == null)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                BindCommon();
                BindGrid();
            }
        }

        public string GetObjType(object obj)
        {
            return Enum.GetName(typeof(ObjTypeLog), Convert.ToInt32(obj)).Replace("_", " ");
        }

        public string GetActionType(object obj)
        {
            try
            {
                List<ActionLog> lst = Enum.GetValues(typeof(ActionLog)).Cast<ActionLog>().ToList();
                return lst.Single(v => (int)v == Convert.ToInt32(obj)).Name();
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                return "Không xác định";
            }
        }

        public string GetDateCreate(object obj, object objTime)
        {
            try
            {
                while (objTime.ToString().Length < 4) objTime = "0" + objTime;
                DateTime d = DateTime.ParseExact(obj.ToString() + objTime, "yyyyMMddHHmm", null);
                return d.ToString("dd/MM/yyyy HH:mm");
            }
            catch
            {
                return obj.ToString() + " " + objTime.ToString();
            }
        }
        public void BindDoiTuong(DropDownList ddl, string header, string selected)
        {
            ddl.Items.Clear();
            List<ObjTypeLog> lstObjType = Enum.GetValues(typeof(ObjTypeLog)).Cast<ObjTypeLog>().ToList();
            foreach (ObjTypeLog item in lstObjType.OrderBy(v => v.ThuTu()))
            {
                string name = item.Name();
                int value = (int)item;
                ddl.Items.Add(new ListItem(name, value.ToString()));
            }
            if (!string.IsNullOrEmpty(header)) ddl.Items.Insert(0, new ListItem(header, "0"));
            if (!string.IsNullOrEmpty(selected)) ddl.SelectedValue = selected;
        }
        public void BindHanhDong(DropDownList ddl, string header, string selected)
        {
            ddl.Items.Clear();
            List<ActionLog> lstObjType = Enum.GetValues(typeof(ActionLog)).Cast<ActionLog>().ToList();
            foreach (ActionLog item in lstObjType.OrderBy(v => v.ThuTu()))
            {
                string name = item.Name();
                int value = (int)item;
                ddl.Items.Add(new ListItem(name, value.ToString()));
            }
            if (!string.IsNullOrEmpty(header)) ddl.Items.Insert(0, new ListItem(header, "0"));
            if (!string.IsNullOrEmpty(selected)) ddl.SelectedValue = selected;
        }

        private void BindCommon()
        {
            BindDoiTuong(ddlObjType, "- Tất cả -", string.Empty);
            BindHanhDong(ddlAction, "- Tất cả -", string.Empty);
        }

        private int countTemp = 0;
        private void BindGrid()
        {
            BindGrid(1);
        }

        private void BindGrid(bool isClearFilter, int pageIndex)
        {
            if (isClearFilter)
            {
                ddlAction.SelectedIndex = 0;
                ddlObjType.SelectedIndex = 0;
                DropDownListPageSize.SelectedIndex = 0;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            BindGrid(pageIndex);
        }

        private void BindGrid(int pageIndex)
        {
            try
            {
                string selectClause = string.Empty;
                string whereClause = string.Empty;
                string orderClause = "DateCreate, TimeCreate DESC";

                DateTime fromDate = Convert.ToDateTime(txtFromDate.Text, new CultureInfo("vi-VN"));
                DateTime toDate = Convert.ToDateTime(txtToDate.Text, new CultureInfo("vi-VN"));

                whereClause += "DateCreate >=" + fromDate.ToString("yyyyMMdd");
                whereClause += " AND DateCreate <=" + toDate.ToString("yyyyMMdd");

                if (!ddlObjType.SelectedValue.Equals("0"))
                {
                    whereClause += " AND ObjType=" + ddlObjType.SelectedValue;
                }
                if (!ddlAction.SelectedValue.Equals("0"))
                {
                    whereClause += " AND Action=" + ddlAction.SelectedValue;
                }

                if (!txtMaDoiTuong.Text.Equals(""))
                {
                    whereClause += " AND ObjId=" + txtMaDoiTuong.Text;
                }
                if (!txtChiTietLoi.Text.Equals(""))
                {
                    whereClause += " AND Note like N'%" + ddlAction.SelectedValue + "%'";
                }


                int pageSize = int.Parse(DropDownListPageSize.SelectedValue);
                int totalRecord = 0;
                countTemp = (pageIndex - 1) * pageSize;

                List<LogInfo> logObj = ServiceFactory.GetInstanceLog().GetPaged(selectClause, whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                if (logObj != null && logObj.Count > 0)
                {
                    ltThongBao.Text = "<font color='red'>Có " + totalRecord + " Log được tìm thấy.</font>";
                    grvView.DataSource = logObj;
                    grvView.DataBind();

                }
                else
                {
                    ltThongBao.Text = "<font color='red'>Không có bản ghi Log nào được tìm thấy.</font>";
                    grvView.DataSource = null;
                    grvView.DataBind();
                }

                TextBoxPage.Text = pageIndex.ToString();
                int totalPage = totalRecord / pageSize;
                if (totalRecord % pageSize != 0) totalPage++;
                LabelNumberOfPages.Text = totalPage.ToString();

                ltTongSoBanGhi.Text = totalRecord.ToString();

                // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);

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

                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1 + countTemp).ToString();
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            grvView.PageIndex = e.NewPageIndex;
            BindGrid();
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

        protected void btSearch_Click(object sender, EventArgs e)
        {
            BindGrid(1);
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

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}