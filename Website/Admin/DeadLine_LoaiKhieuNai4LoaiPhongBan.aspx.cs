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

namespace Website.admin
{
    public partial class DeadLine_LoaiKhieuNai4LoaiPhongBan : AppCode.PageBase
    {
        protected string strEditMode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            strEditMode = Request.QueryString["UIMODE"];

            if (!IsPostBack)
            {
                BindDropDownlist();

                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    ltTitle.Text = "Cập nhật thời gian xử lý khiếu nại";
                    EditData();
                }
                else
                    ltTitle.Text = "Thêm mới thời gian xử lý khiếu nại";
            }
        }

        private void BindDropDownlist()
        {
            

            ddlLoaiPhongBanSelect.DataSource = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
            ddlLoaiPhongBanSelect.DataValueField = "ID";
            ddlLoaiPhongBanSelect.DataTextField = "Name";
            ddlLoaiPhongBanSelect.DataBind();

            List<LoaiKhieuNaiInfo> list = ServiceFactory.GetInstanceLoaiKhieuNai().GetListLoaiKhieuNaiSortParent();

            ddlLoaiKhieuNaiSelect.DataTextField = "Name";
            ddlLoaiKhieuNaiSelect.DataValueField = "Id";
            ddlLoaiKhieuNaiSelect.DataSource = list;
            ddlLoaiKhieuNaiSelect.DataBind();

        }

        private void EditData()
        {
            try
            {
                var obj = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai();
                var item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
                if (item == null)
                {
                    Utility.LogEvent("Function EditData loaiPhongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
                else
                {
                    ddlLoaiKhieuNaiSelect.SelectedValue = item.LoaiKhieuNaiId.ToString();
                    ddlLoaiPhongBanSelect.SelectedValue = item.LoaiPhongBanId.ToString();
                    txtThoiGianCanhBao.Text = item.ThoiGianCanhBao;
                    txtThoiGianUocTinh.Text = item.ThoiGianUocTinh;
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
                var obj = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai();
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    try
                    {
                        int idEdit = int.Parse(Request.QueryString["ID"]);
                        LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo item = obj.GetInfo(idEdit);

                        if (item == null)
                        {
                            Utility.LogEvent("Function loaiPhongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                            return;
                        }

                        try
                        {
                            item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiSelect.SelectedValue);
                            item.LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBanSelect.SelectedValue);
                            item.ThoiGianCanhBao = txtThoiGianCanhBao.Text.Trim();
                            item.ThoiGianUocTinh = txtThoiGianUocTinh.Text.Trim();
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
                else
                {
                    var item = new LoaiPhongBan_ThoiGianXuLyKhieuNaiInfo();

                    try
                    {
                        item.LoaiKhieuNaiId = Convert.ToInt32(ddlLoaiKhieuNaiSelect.SelectedValue);
                        item.LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBanSelect.SelectedValue);
                        item.ThoiGianCanhBao = txtThoiGianCanhBao.Text.Trim();
                        item.ThoiGianUocTinh = txtThoiGianUocTinh.Text.Trim();
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
                lblMsg.Text = "Có lỗi xảy ra khi cập nhật dữ liệu.";
                Utility.LogEvent(se);
                return;
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