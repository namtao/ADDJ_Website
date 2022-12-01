using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using Website.AppCode;
using AIVietNam.Admin;
using System.Transactions;

namespace Website
{
    public partial class ConfigurationArchive : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                grvView.PageSize = Config.RecordPerPage;
                BindGrid(true);
            }
        }

        private void BindGrid(bool isClearFilter)
        {
            try
            {
                var phongBanObj = ServiceFactory.GetInstanceArchiveConfig().GetList();
                if (phongBanObj != null && phongBanObj.Count > 0)
                {
                    ltThongBao.Text = "<font color='red'>Có " + phongBanObj.Count + " archive được tìm thấy.</font>";
                    grvView.DataSource = phongBanObj;
                    grvView.DataBind();
                }
                else
                {
                    ltThongBao.Text = "<font color='red'>Không có archive được tìm thấy.</font>";
                    grvView.DataSource = null;
                    grvView.DataBind();
                }

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onload", "LoadJS();", true);
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

                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
                e.Row.Cells[2].Text = "<a href=\"ConfigurationArchive_add.aspx?ID=" + grvView.DataKeys[e.Row.RowIndex].Value + "\">" + e.Row.Cells[2].Text + "</a>";
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


        protected void linkbtnThemMoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfigurationArchive_add.aspx", false);
        }

        protected void linkbtnXoa_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserDelete)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            try
            {
                int i = 0;
                var obj = ServiceFactory.GetInstanceArchiveConfig();

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        bool flagExe = true;
                        foreach (GridViewRow row in grvView.Rows)
                        {
                            var status = (CheckBox)row.FindControl("cbSelectAll");
                            if (status.Checked)
                            {
                                int ID = int.Parse(grvView.DataKeys[i].Value.ToString());

                                if (ArchiveConfigImpl.CheckExistsDataArchive(ID))
                                    obj.Delete(ID);
                                else
                                {
                                    flagExe = false;
                                    break;
                                }
                            }
                            i++;
                        }

                        if (flagExe)
                            scope.Complete();
                        else
                            ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Có dữ liệu archive. Không xóa được.','error');", true);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + ex.Message + "','error');", true);
                    }
                }

            }
            catch (System.Data.SqlClient.SqlException se)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePnl, UpdatePnl.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
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