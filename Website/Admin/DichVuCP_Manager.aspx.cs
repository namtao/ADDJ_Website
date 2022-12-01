using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using Website.AppCode;

namespace Website.admin
{
    public partial class DichVuCP_Manager : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pageIndex = 1;
                pageSize = Config.RecordPerPage;
                BindGrid(false, 0);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();",
                        true);
            }
        }

        private const string TableName = "DichVuCP";

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 22/02/2014 09:30
        /// Description: Binds the grid.
        /// </summary>
        /// <param name="isClearFilter">if set to <c>true</c> [is clear filter].</param>
        private void BindGrid( bool isClearFilter, int _pageIndex)
        { 
            try
            {
                if (isClearFilter)
                {
                    txtNameFilter.Text = "";
                 
                }
                string whereClause = string.Empty;

                if (!txtNameFilter.Text.Equals(""))
                    whereClause = "MaDichVu like N'%" + txtNameFilter.Text + "%'";
                var s = ServiceFactory.GetInstanceDichVuCP();
                int count = 0;

                int totalRecord = 0;
                int pageIndex = 1;


                pageSize = Convert.ToInt32(DropDownListPageSize.SelectedValue);

                if (_pageIndex != 0)
                    pageIndex = _pageIndex;
                if (isClearFilter)
                {
                    pageIndex = 1;
                }

                var dichVuCpInfos = s.GetPaged(string.Empty, whereClause, string.Empty, pageIndex, pageSize, ref totalRecord);
                if (totalRecord > 0)
                {
                    ltThongBao.Text = "<font color='red'>Có " + totalRecord + " dịch vụ CP được tìm thấy.</font>";
                    grvView.DataSource = dichVuCpInfos;
                    grvView.DataBind();

                    TextBoxPage.Text = pageIndex.ToString();
                    var totalPage = totalRecord / pageSize;
                    if (totalRecord % pageSize != 0)
                        totalPage++;
                    LabelNumberOfPages.Text = totalPage.ToString();

                    ltTongSoBanGhi.Text = totalRecord.ToString();
                }
                else
                {
                    ltThongBao.Text = "<font color='red'>Không có dịch vụ CP được tìm thấy.</font>";
                    grvView.DataSource = null;
                    grvView.DataBind();
                }

                if (IsPostBack)
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();",
                        true);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
            }
        }

        public string BuildPage(int totalRecord, int pageIndex, int pageSize, string param)
        {
            StringBuilder sb = new StringBuilder();
            if (totalRecord > pageSize)
            {
                sb.Append(HtmlUtility.BuildPagerNormal(totalRecord, pageSize, pageIndex, param, "", "active", 5));
            }
            return sb.ToString();
        }

        protected void grvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.DataItemIndex != -1)
                {
                    //if (e.Row.RowIndex % 2 != 0)
                    //{
                    //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                    //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#DEEAF3'");
                    //}
                    //else
                    //{
                    //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#f5eeb8'");
                    //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'");
                    //}
                    e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
            }
        }

 
        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 22/02/2014 09:40
        /// Description: Handles the Click event of the btThemMoi control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btThemMoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("DichVuCP_Add.aspx", false);
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 22/02/2014 09:40
        /// Description: Handles the Click event of the btClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btClearFilter_Click(object sender, EventArgs e)
        {
            BindGrid(false, 0);
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 22/02/2014 09:40
        /// Description: Handles the Click event of the btFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
            try
            {
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

        protected void linkbtnDelete_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                int i = 0;
                var s = ServiceFactory.GetInstanceDichVuCP();
                foreach (GridViewRow row in grvView.Rows)
                {
                    var status = (CheckBox)row.FindControl("cbSelectAll");
                    if (status.Checked)
                    {
                        int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                        s.Delete(ID);
                    }
                    i++;
                }

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage",
                    "MessageAlert.AlertNormal('Xóa dịch vụ CP thành công.','info');", true);
            }
            catch (SqlException se)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage",
                    "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
            BindGrid(false, Convert.ToInt32(TextBoxPage.Text));
        }
    }
}