using System;
using System.Web.UI;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.admin
{
    public partial class DichVuCP_Add : PageBase
    {
        protected string strEditMode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            strEditMode = Request.QueryString["UIMODE"];

            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    ltTitle.Text = "Cập nhật dịch vụ CP";
                    txtIdDichVu.ReadOnly = true;
                    EditData();
                }
                else
                {
                    txtIdDichVu.ReadOnly = false;
                    ltTitle.Text = "Thêm mới dịch vụ CP";
                }
            }
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 24/02/2014 15:57
        /// Description: Edits the data.
        /// </summary>
        private void EditData()
        {
            try
            {
                var s = ServiceFactory.GetInstanceDichVuCP();
                var item = s.GetInfo(int.Parse(Request.QueryString["ID"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData DichVuCP_Add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                }
                else
                {
                    txtIdDichVu.Text = item.Id.ToString();
                    txtMaDichVu.Text = item.MaDichVu;
                    txtNgayBatDau.Text = item.NgayBatDau.ToString("dd/MM/yyyy");
                    txtNgayKetThuc.Text = item.NgayKetThuc.ToString("dd/MM/yyyy");
                    chkDeactive.Checked = item.Deactive == 1 ? true : false;
                    txtGhiChu.Text = item.GhiChu;
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
        /// Created on: 24/02/2014 16:00
        /// Description: Handles the Click event of the btSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này.','error');", true);
                return;
            }

            try
            {
                var s = ServiceFactory.GetInstanceDichVuCP();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    try
                    {
                        int idEdit = int.Parse(Request.QueryString["ID"]);
                        var item = s.GetInfo(idEdit);

                        if (item == null)
                        {
                            Utility.LogEvent("Function DichVuCP_Add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                            return;
                        }

                        try
                        {
                            item.MaDichVu = txtMaDichVu.Text;
                            item.NgayBatDau = Convert.ToDateTime(ConvertUtility.ConvertDMYtoMdy(txtNgayBatDau.Text));
                            item.NgayKetThuc = Convert.ToDateTime(ConvertUtility.ConvertDMYtoMdy(txtNgayKetThuc.Text));
                            item.Deactive = chkDeactive.Checked ? 1 : 0;
                            item.GhiChu = txtGhiChu.Text;
                        }
                        catch
                        {
                            lblMsg.Text = "Dữ liệu không hợp lệ";
                            return;
                        }

                        s.Update(item);
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        return;
                    }
                }
                else
                {
                    int id = Convert.ToInt32(txtIdDichVu.Text);
                    var obj = s.GetInfo(id);
                    if (obj != null)
                    {
                        lblMsg.Text = string.Format("Đã tồn tại dịch vụ {0} này trong hệ thống có ID như trên", obj.MaDichVu);
                        return;
                    }
                    var item = new DichVuCPInfo();
                    try
                    {
                        item.Id = Convert.ToInt32(txtIdDichVu.Text);
                        item.MaDichVu = txtMaDichVu.Text;
                        item.NgayBatDau = Convert.ToDateTime(ConvertUtility.ConvertDMYtoMdy(txtNgayBatDau.Text));
                        item.NgayKetThuc = Convert.ToDateTime(ConvertUtility.ConvertDMYtoMdy(txtNgayKetThuc.Text));
                        item.Deactive = chkDeactive.Checked ? 1 : 0;
                        item.GhiChu = txtGhiChu.Text;
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    s.Add(item);
                }
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "DichVuCP.Edit().okay();", true);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                lblMsg.Text = "Tên dịch vụ CP đã tồn tại.";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Có lỗi xảy ra khi cập nhật dữ liệu.";
                Utility.LogEvent(ex);
                return;
            }
        }
    }
}