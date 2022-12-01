using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Data.SqlClient;

namespace Website.admin
{
    public partial class LoiKhieuNai_manager : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GrvView.PageSize = Config.RecordPerPage;
                BindPhienBan();
                BindGrid(false);
            }
        }
        protected void btFilter_Click(object sender, EventArgs e)
        {
            BindGrid(false);
        }

        protected void btClearFilter_Click(object sender, EventArgs e)
        {
            BindGrid(true);
        }

        private void BindGrid(bool isClearFilter)
        {
            try
            {
                if (isClearFilter)
                {
                    txtNameFilter.Text = "";
                }

                string selectClause = "*";

                string whereClause = string.Format("TuNgay = {0}", ddlPhienBan.SelectedValue);

                if (!txtNameFilter.Text.Equals(string.Empty)) whereClause += string.Format(" AND TenLoi LIKE N'%{0}%'", txtNameFilter.Text);

                List<LoiKhieuNaiInfo> listLoiKhieuNaiInfo = null;

                listLoiKhieuNaiInfo = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic(selectClause, whereClause, "ThuTu ASC");

                //if (whereClause.Length > 0)
                //{
                    
                //}
                //else
                //{
                //    listLoiKhieuNaiInfo = ServiceFactory.GetInstanceLoiKhieuNai().GetListSortHierarchy();
                //}

                if (listLoiKhieuNaiInfo != null && listLoiKhieuNaiInfo.Count > 0)
                {
                    ltThongBao.Text = "<font color='red'>Có " + listLoiKhieuNaiInfo.Count + " lỗi khiếu nại được tìm thấy.</font>";
                    GrvView.DataSource = listLoiKhieuNaiInfo;
                    GrvView.DataBind();
                }
                else
                {
                    ltThongBao.Text = "<font color='red'>Không có lỗi khiếu nại được tìm thấy.</font>";
                    GrvView.DataSource = null;
                    GrvView.DataBind();
                }

                if (IsPostBack)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(udPanel1, udPanel1.GetType(), "onload", "LoadJS();", true);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }

        private void BindPhienBan()
        {
            var lst = ServiceFactory.GetInstanceLoiKhieuNai().GetListDynamic("MaLoi = SUBSTRING(CONVERT(varchar(8), TuNgay),7,2) + '/' + SUBSTRING(CONVERT(varchar(8), TuNgay),5,2) + '/' + SUBSTRING(CONVERT(varchar(8), TuNgay) ,1,4) + N' đến ' + SUBSTRING(CONVERT(varchar(8), DenNgay),7,2) + '/' + SUBSTRING(CONVERT(varchar(8), DenNgay),5,2) + '/' + SUBSTRING(CONVERT(varchar(8), DenNgay),1,4), TuNgay", "1=1 GROUP BY TuNGay, DenNgay","TuNgay");
            ddlPhienBan.DataSource = lst;
            ddlPhienBan.DataTextField = "MaLoi";
            ddlPhienBan.DataValueField = "TuNgay";
            ddlPhienBan.DataBind();
        }

        private void RowDataBound(GridViewRowEventArgs e)
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
                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();

                if (e.Row.Cells[4].Text == "2")
                {
                    e.Row.Cells[2].Text = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}", e.Row.Cells[2].Text);
                }

                if (e.Row.Cells[5].Text == "0")
                {
                    e.Row.Cells[5].Text = "";
                }
                else
                {
                    e.Row.Cells[5].Text = Enum.GetName(typeof(LoiKhieuNai_Loai), ConvertUtility.ToInt32(e.Row.Cells[5].Text)).Replace("_", " ");
                }
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            GrvView.PageIndex = e.NewPageIndex;
            BindGrid(false);
        }

        protected void GrvView_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GrvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                int i = 0;
                var obj = ServiceFactory.GetInstanceLoiKhieuNai();
                foreach (GridViewRow row in GrvView.Rows)
                {
                    var status = (CheckBox)row.FindControl("cbSelectAll");
                    if (status.Checked)
                    {
                        int ID = int.Parse(GrvView.DataKeys[i].Value.ToString());
                        obj.Delete(ID);
                    }
                    i++;
                }

                ScriptManager.RegisterClientScriptBlock(udPanel1, udPanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Xóa lỗi khiếu thành công.','info');", true);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                Helper.GhiLogs(se);
                ScriptManager.RegisterClientScriptBlock(udPanel1, udPanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
            BindGrid(false);
        }
  
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            LinkButton button = (LinkButton)sender;
            string[] separator = new string[] { ";" };
            string[] strArray = button.CommandArgument.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int itemId = Convert.ToInt32(strArray[0]);
            switch (strArray[1].ToString())
            {
                case "Up":
                    {
                        SqlParameter[] prms = new SqlParameter[] {
                        new SqlParameter("@Id", itemId)
                    };
                        SqlHelper.ExecuteNonQuery(Config.ConnectionString, "LoiKhieuNai_SapXepLen", prms);
                        this.BindGrid(false);
                        break;
                    }
                case "Down":
                    {
                        SqlParameter[] prms = new SqlParameter[] {
                        new SqlParameter("@Id", itemId)
                    };
                        SqlHelper.ExecuteNonQuery(Config.ConnectionString, "LoiKhieuNai_SapXepXuong", prms);
                        this.BindGrid(false);
                        break;
                    }
            }
        }
        protected string HanhDong(object id)
        {
            string returnURL = Request.Url.AbsolutePath + string.Format("?&NameFilter={0}&PIndex={1}", txtNameFilter.Text, GrvView.PageIndex);
            returnURL = HttpUtility.UrlEncode(returnURL);

            string strReturn = string.Empty;

            if (Permission.UserEdit)
            {
                strReturn += string.Format("<a href='LoiKhieuNai_Add.aspx?Id={0}&ReturnUrl={1}'>Sửa</a> <br />", id, returnURL);
            }
            else
            {
                strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Sửa</a> <br />");
            }

            return strReturn;
        }

        protected void ddlPhienBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid(false);
        }

        protected void linkbtnThemMoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoiKhieuNai_add.aspx", false);
        }

        protected void linkbtnUpdateHoatDong_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                int i = 0;
                var obj = ServiceFactory.GetInstanceLoiKhieuNai();
                foreach (GridViewRow row in GrvView.Rows)
                {
                    CheckBox status = (CheckBox)row.FindControl("cbSelectAll");
                    if (status.Checked)
                    {
                        int ID = int.Parse(GrvView.DataKeys[i].Value.ToString());
                        bool isSelected = (row.Cells[5].Controls[0] as CheckBox).Checked;

                        int hoatDong = isSelected ? 0 : 1;
                        obj.UpdateDynamic("HoatDong=" + hoatDong, "Id=" + ID);
                    }

                    i++;
                }

                ScriptManager.RegisterClientScriptBlock(udPanel1, udPanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Cập nhật thành công.','info');", true);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                Helper.GhiLogs(se);
                ScriptManager.RegisterClientScriptBlock(udPanel1, udPanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không cập nhật hoạt động được.','error');", true);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
            BindGrid(false);
        }
    }
}