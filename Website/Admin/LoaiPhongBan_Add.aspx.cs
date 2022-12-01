using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;

namespace Website.admin
{
    public partial class LoaiPhongBan_Add : AppCode.PageBase
    {
        protected string strEditMode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            strEditMode = Request.QueryString["UIMODE"];

            if (!IsPostBack)
            {   
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    if (strEditMode.Equals("COPY"))
                    {
                        divInfoBox.Visible = true;
                        ltTitle.Text = "Sao chép loại phòng ban";
                    }
                    else
                        ltTitle.Text = "Cập nhật loại phòng ban";
                    EditData();
                }
                else
                    ltTitle.Text = "Thêm mới loại phòng ban";

                btnResetTime.Visible = Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty;
            }
        }

        private void EditData()
        {
            try
            {
                var obj = ServiceFactory.GetInstanceLoaiPhongBan();
                LoaiPhongBanInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData loaiPhongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                else
                {
                    txtName.Text = item.Name.ToString();
                    txtDescription.Text = item.Description.ToString();
                    txtThoiGianXuLyMacDinh.Text = item.ThoiGianXuLyMacDinh;
                    ddlLoaiDuLieu.SelectedValue = item.LoaiDuLieu.ToString();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                Response.Redirect(Config.PathError, false);
                return;
            }
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Bạn không có quyền thực hiện chức năng này.','error');", true);
                return;
            }

            try
            {
                // Check điều kiện
                if(txtThoiGianXuLyMacDinh.Text.Length > 0 && ddlLoaiDuLieu.SelectedValue == "0")
                {
                    lblMsg.Text = "Bạn phải chọn loại dữ liệu";
                    ddlLoaiDuLieu.Focus();
                    return;                    
                }

                if(txtThoiGianXuLyMacDinh.Text.Trim().Length == 0 && ddlLoaiDuLieu.SelectedValue != "0")
                {
                    lblMsg.Text = "Bạn phải nhập thời gian xử lý mặc định";
                    txtThoiGianXuLyMacDinh.Focus();
                    return; 
                }

                // Đưa dữ liệu vào hệ thống
                var obj = ServiceFactory.GetInstanceLoaiPhongBan();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    if (strEditMode.Equals("COPY"))
                    {
                        var item = new LoaiPhongBanInfo();

                        try
                        {
                            item.Name = txtName.Text.Trim();
                            item.Description = txtDescription.Text.Trim();
                            item.ThoiGianXuLyMacDinh = txtThoiGianXuLyMacDinh.Text.Trim();
                            item.LoaiDuLieu = ConvertUtility.ToByte(ddlLoaiDuLieu.SelectedValue, 1);
                        }
                        catch
                        {
                            lblMsg.Text = "Dữ liệu không hợp lệ";
                            return;
                        }

                        item.Id = obj.Add(item);

                        //Lấy dữ liệu cũ sao chép sang mới
                        int idEdit = int.Parse(Request.QueryString["ID"]);
                        
                        //Sao chép dữ liệu thời gian xử lý KN
                        var lstThoiGianXuLy = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetListDynamic("", "LoaiPhongBanId=" + idEdit, "");

                        foreach (var itemThoiGianXuLy in lstThoiGianXuLy)
                        {
                            itemThoiGianXuLy.LoaiPhongBanId = item.Id;
                            ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().Add(itemThoiGianXuLy);
                        }

                        //Sao chép dữ liệu quyền KN
                        var lstPermissionKN = ServiceFactory.GetInstanceLoaiPhongBan_Permission().GetListDynamic("", "LoaiPhongBanId=" + idEdit, "");

                        foreach (var itemPermission in lstPermissionKN)
                        {
                            itemPermission.LoaiPhongBanId = item.Id;
                            ServiceFactory.GetInstanceLoaiPhongBan_Permission().Add(itemPermission);
                        }
                    }
                    else
                    {
                        try
                        {
                            int idEdit = int.Parse(Request.QueryString["ID"]);
                            LoaiPhongBanInfo item = obj.GetInfo(idEdit);

                            if (item == null)
                            {
                                Utility.LogEvent("Function loaiPhongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                                return;
                            }

                            try
                            {
                                item.Name = txtName.Text.Trim();
                                item.Description = txtDescription.Text.Trim();
                                item.ThoiGianXuLyMacDinh = txtThoiGianXuLyMacDinh.Text.Trim();
                                item.LoaiDuLieu = ConvertUtility.ToByte(ddlLoaiDuLieu.SelectedValue, 1);
                            }
                            catch
                            {
                                lblMsg.Text = "Dữ liệu không hợp lệ";
                                return;
                            }

                            obj.Update(item);
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            return;
                        }
                    }
                }
                else
                {
                    var item = new LoaiPhongBanInfo();

                    try
                    {
                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();
                        item.ThoiGianXuLyMacDinh = txtThoiGianXuLyMacDinh.Text.Trim();
                        item.LoaiDuLieu = ConvertUtility.ToByte(ddlLoaiDuLieu.SelectedValue, 1);
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    obj.Add(item);
                }
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "okay();", true);
            }
            catch (System.Data.SqlClient.SqlException se)
            {
                lblMsg.Text = "Tên loại phòng ban đã tồn tại.";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Có lỗi xảy ra khi cập nhật dữ liệu.";
                Utility.LogEvent(ex);
                return;
            }
        }

        protected void btnResetTime_Click(object sender, EventArgs e)
        {
            int idEdit = int.Parse(Request.QueryString["ID"]);
            int resultCode = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().ResetTime(idEdit, txtThoiGianXuLyMacDinh.Text, ddlLoaiDuLieu.SelectedValue);
            if(resultCode == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "onload", "okay();", true);
            }
            else
            {
                lblMsg.Text = "Có lỗi xảy ra khi cập nhật dữ liệu.";                
            }
        }
    }
}