using System;
using System.Web.UI;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using Website.Ws_EmailReference;
public partial class admin_loaiKhieuNai_VASUpdate_add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = "";
            BindDropDownlist();
            ChangeLoaiKN();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
            else
            {
                ChangeLoaiKN();
            }
        }
    }
    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
            LoaiKhieuNai_VASUpdateInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData loaiKhieuNai_VASUpdate_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                try
                {
                    if (item.Cap == 1)
                        Response.Redirect("/admin/loaiKhieuNai_VASUpdate_manager.aspx");
                }
                catch { }
                ddlParrent.SelectedValue = item.ParentId.ToString(CultureInfo.InvariantCulture);
                ddlParrent.Attributes.Add("valueOld", item.ParentId.ToString(CultureInfo.InvariantCulture));
                txtMaDichVu.Text = item.MaDichVu.ToString(CultureInfo.InvariantCulture);
                txtName.Text = item.Name.ToString(CultureInfo.InvariantCulture);
                txtDescription.Text = item.Description;
                txtSort.Text = item.Sort.ToString(CultureInfo.InvariantCulture);
                //chkStatus.Checked = item.Status == 1 ? true : false;
                txtSort.Attributes.Add("valueOld", item.Sort.ToString(CultureInfo.InvariantCulture));
                chkIsDeleted.Checked = item.IsDeleted;
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
  
    private void BindDropDownlist()
    {
        List<LoaiKhieuNai_VASUpdateInfo> list = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListLoaiKhieuNai_VASUpdateCap();

        ddlParrent.DataTextField = "Name";
        ddlParrent.DataValueField = "Id";
        ddlParrent.DataSource = list;
        ddlParrent.DataBind();
    }
    void ChangeLoaiKN()
    {
        if (ddlParrent.SelectedValue.Equals(ddlParrent.Attributes["valueOld"]))
        {
            txtSort.Text = txtSort.Attributes["valueOld"];
            return;
        }
        if (ddlParrent.SelectedValue.Equals("0"))
        {
            var lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListDynamic("MAX(Sort)+1000000 as Sort", "ParentId=0", "");
            if (lstSortParentMax != null && lstSortParentMax.Count > 0)
                txtSort.Text = lstSortParentMax[0].Sort.ToString();
            else
                txtSort.Text = "1000000";
        }
        else
        {
            //if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;")))
            //{
                var lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListDynamic("MAX(Sort)+1 as Sort", "Id= " + ddlParrent.SelectedValue + " OR ParentId=" + ddlParrent.SelectedValue, "");
                if (lstSortParentMax != null && lstSortParentMax.Count > 0)
                    txtSort.Text = lstSortParentMax[0].Sort.ToString();
                else
                    txtSort.Text = "1000000";
            //}
            //else
            //{
            //    var lstSortParentMax = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().GetListDynamic("MAX(Sort)+1000 as Sort", "Id= " + ddlParrent.SelectedValue + " OR ParentId=" + ddlParrent.SelectedValue, "");
            //    if (lstSortParentMax != null && lstSortParentMax.Count > 0)
            //        txtSort.Text = lstSortParentMax[0].Sort.ToString();
            //    else
            //        txtSort.Text = "1000000";
            //}
        }
    }
    protected void ddlParrent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeLoaiKN();
    }
    void ChangeSort(int id, int cap, int sortNew)
    {
        if (!txtSort.Attributes["valueOld"].Equals(sortNew.ToString()))
        {
            var lst = LoaiKhieuNai_VASUpdateImpl.ListLoaiKhieuNai_VASUpdateInfo.Where(t => t.ParentId == id);
            if (lst.Any())
            {
                if (cap == 1)
                {
                    foreach (var item in lst)
                    {
                        sortNew = sortNew + 1000;
                        ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().UpdateDynamic("Cap=2,Sort=" + sortNew, "Id=" + item.Id);

                        var lstChild = LoaiKhieuNai_VASUpdateImpl.ListLoaiKhieuNai_VASUpdateInfo.Where(t => t.ParentId == item.Id);
                        if (lstChild.Any())
                        {
                            var sortChild = sortNew;
                            foreach (var child in lstChild)
                            {
                                sortChild = sortChild + 1;
                                ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().UpdateDynamic("Cap=3,Sort=" + sortChild, "Id=" + child.Id);
                            }
                        }
                    }
                }
                else if (cap == 2)
                {
                    foreach (var item in lst)
                    {
                        sortNew = sortNew + 1;
                        ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate().UpdateDynamic("Cap=3,Sort=" + sortNew, "Id=" + item.Id);
                    }
                }
            }
        }
    }

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!Permission.UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai_VASUpdate();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    LoaiKhieuNai_VASUpdateInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function loaiKhieuNai_VASUpdate_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    if (item.Cap == 1 || item.ParentId == 0)
                    {
                        lblMsg.Text = "Không sửa dịch vụ cha.";
                        return;
                    }

                    try
                    {
                        item.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                        item.MaDichVu = txtMaDichVu.Text.Trim();
                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text;
                        item.Sort = ConvertUtility.ToInt32(txtSort.Text);
                        byte capNew = 1;
                        if (item.ParentId == 0)
                            capNew = 1;
                        else if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;")))
                            capNew = 3;
                        else
                            capNew = 2;

                        //if (item.Cap < capNew)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Hệ thống không cho phép chuyển loại dịch vụ từ cấp cha xuống cấp con.','error');", true);
                        //    return;
                        //}

                        item.Cap = capNew;
                        if (item.CDate == null)
                        {
                            item.CDate = DateTime.Now;
                            item.CUser = UserInfo.Username;
                        }
                        item.LUser = UserInfo.Username;
                        item.IsDeleted = chkIsDeleted.Checked;
                        item.NgayHetHan = txtNgayHetHan.Text;
                        item.IsUpdate = false;
                        obj.Update(item);
                        ChangeSort(item.Id, item.Cap, item.Sort);
                        try
                        {
                            Website.Ws_EmailReference.Ws_EmailSoapClient sendEmailXmlvnp = new Ws_EmailSoapClient();

                            SendEmailXMLVNPListRequest objSend = new SendEmailXMLVNPListRequest();
                            //objSend.
                            ArrayOfString lstTo = new ArrayOfString();
                            lstTo.Add("haintt@vinaphone.vn");
                            lstTo.Add("longlx@aivietnam.net");

                            var sendTed = sendEmailXmlvnp.SendEmailXMLVNPList(lstTo, null, null, "VAS thay đổi loại dịch vụ", "Người cập nhật " + UserInfo.Username + "<br/> Cập nhật loại dịch vụ " + item.Name + "(" + item.MaDichVu + ").<br/> Ngày cập nhật:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), null, string.Empty, "603335ee7c3141268941761c48032fb2");
                        }
                        catch
                        {
                        }


                        //item.IsUpdate = chkIsUpdate.Checked;
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }


                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    Response.Redirect(Config.PathError, false);
                    return;
                }
            }
            else
            {
                var item = new LoaiKhieuNai_VASUpdateInfo();

                try
                {
                    item.ParentId = Convert.ToInt32(ddlParrent.SelectedValue);
                    item.Name = txtName.Text.Trim();
                    item.Description = txtDescription.Text.Trim();
                    item.Sort = Convert.ToInt32(txtSort.Text.Trim());
                    item.MaDichVu = txtMaDichVu.Text.Trim();

                    if (item.ParentId == 0)
                        item.Cap = 1;
                    else if (ddlParrent.SelectedItem.Text.StartsWith(System.Web.HttpUtility.HtmlDecode("&nbsp;&nbsp;")))
                        item.Cap = 3;
                    else
                        item.Cap = 2;

                    //item.Status = chkStatus.Checked ? (byte)1 : (byte)0;
                    item.CDate = DateTime.Now;
                    item.CUser = LoginAdmin.AdminLogin().Username;
                    item.IsUpdate = false;
                    item.IsDeleted = chkIsDeleted.Checked;
                    item.NgayHetHan = txtNgayHetHan.Text;
                    obj.Add(item);
                    Website.Ws_EmailReference.Ws_EmailSoapClient sendEmailXmlvnp = new Ws_EmailSoapClient();
                    try
                    {
                        SendEmailXMLVNPListRequest objSend = new SendEmailXMLVNPListRequest();
                        //objSend.
                        ArrayOfString lstTo = new ArrayOfString();
                        lstTo.Add("haintt@vinaphone.vn");
                        lstTo.Add("longlx@aivietnam.net");
                        var sendTed = sendEmailXmlvnp.SendEmailXMLVNPList(lstTo, null, null, "VAS thay đổi loại dịch vụ", "Người cập nhật " + UserInfo.Username + "<br/> Thêm mới  dịch vụ " + item.Name + "(" + item.MaDichVu + ").<br/> Ngày cập nhật:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), null, string.Empty, "603335ee7c3141268941761c48032fb2");
                        //item.IsUpdate = chkIsUpdate.Checked;
                    }
                    catch
                    {
                    }
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }


            }

            Response.Redirect("loaiKhieuNai_VASUpdate_manager.aspx", false);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

