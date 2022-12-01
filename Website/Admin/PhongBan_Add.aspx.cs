using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

public partial class Admin_PhongBan_Add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = string.Empty;

            BindDropDownlist();
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                EditData();
            }
        }
    }

    private void BindDropDownlist()
    {
        if (UserInfo.NhomNguoiDung == 0 || UserInfo.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET || UserInfo.Username.ToLower() == "administrator" || UserInfo.NhomNguoiDung == (int)NguoiSuDung_NhomNguoiDung.Vinaphone)
        {
            string whereClause = string.Format("DonViTrucThuoc IN (0, {0})", DoiTacInfo.DoiTacIdValue.VNP);

            List<DoiTacInfo> lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id, MaDoiTac, DonViTrucThuoc", whereClause, string.Empty);
            ddlKhuVuc.DataSource = lstKhuVuc;
            ddlKhuVuc.DataValueField = "Id";
            ddlKhuVuc.DataTextField = "MaDoiTac";
            ddlKhuVuc.DataBind();
        }
        else
        {
            string whereClause = string.Format("ID = {0}", UserInfo.KhuVucId);

            List<DoiTacInfo> lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id, MaDoiTac, DonViTrucThuoc", whereClause, string.Empty);
            ddlKhuVuc.DataSource = lstKhuVuc;
            ddlKhuVuc.DataValueField = "ID";
            ddlKhuVuc.DataTextField = "MaDoiTac";
            ddlKhuVuc.DataBind();

            var lst = ServiceFactory.GetInstanceDoiTac().GetList();
            List<DoiTacInfo> lstDoiTac = new List<DoiTacInfo>();
            AdminInfo admin = LoginAdmin.AdminLogin();

            IEnumerable<DoiTacInfo> parent = null;

            parent = lst.Where(t => t.Id == admin.DoiTacId);
            foreach (var item in parent)
            {
                lstDoiTac.Add(item);
                var lstChild = lst.Where(t => t.DonViTrucThuoc == item.Id);
                foreach (var itemChild in lstChild)
                {
                    itemChild.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild.MaDoiTac);
                    lstDoiTac.Add(itemChild);

                    IEnumerable<DoiTacInfo> lstChildC2 = lst.Where(t => t.DonViTrucThuoc == itemChild.Id);
                    foreach (var itemChild2 in lstChildC2)
                    {
                        itemChild2.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild2.MaDoiTac);
                        lstDoiTac.Add(itemChild2);
                    }
                }
            }

            ddlDoiTac.DataSource = lstDoiTac;
            ddlDoiTac.DataValueField = "ID";
            ddlDoiTac.DataTextField = "MaDoiTac";
            ddlDoiTac.DataBind();
        }

        ddlLoaiPhongBan.DataSource = ServiceFactory.GetInstanceLoaiPhongBan().GetList();
        ddlLoaiPhongBan.DataValueField = "ID";
        ddlLoaiPhongBan.DataTextField = "Name";
        ddlLoaiPhongBan.DataBind();

        foreach (byte i in Enum.GetValues(typeof(KhieuNai_HTTiepNhan_Type)))
        {
            ddlHTTiepNhan.Items.Add(new ListItem(Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), i).Replace("_", " "), i.ToString()));
        }


        // Bind trạng thái phòng ban

        new PhongBanHelper().BindPhongBanTrangThai(ddlTrangThai, string.Empty, string.Empty);

    }

    private void EditData()
    {
        try
        {
            PhongBanImpl obj = ServiceFactory.GetInstancePhongBan();
            PhongBanInfo phongBanInfo = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (phongBanInfo == null)
            {
                Utility.LogEvent("Function EditData phongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                ddlLoaiPhongBan.SelectedValue = phongBanInfo.LoaiPhongBanId.ToString();
                ddlKhuVuc.SelectedValue = phongBanInfo.KhuVucId.ToString();
                if (UserInfo.NhomNguoiDung == 0 || UserInfo.NhomNguoiDung == (int)NguoiSuDung_NhomNguoiDung.Vinaphone)
                {
                    BindDoiTac(phongBanInfo.KhuVucId.ToString());
                }
                else
                {
                    ddlDoiTac.SelectedValue = phongBanInfo.DoiTacId.ToString();
                }
                ddlDoiTac.SelectedValue = phongBanInfo.DoiTacId.ToString();

                txtName.Text = phongBanInfo.Name.ToString();
                txtDescription.Text = phongBanInfo.Description.ToString();
                txtSort.Text = phongBanInfo.Sort.ToString();
                chkIsDinhTuyenKN.Checked = phongBanInfo.IsDinhTuyenKN;
                chkIsChuyenTiepKN.Checked = phongBanInfo.IsChuyenTiepKN;
                ddlHTTiepNhan.SelectedValue = phongBanInfo.DefaultHTTN.ToString();
                ddlCapPhongBan.SelectedValue = phongBanInfo.Cap.ToString();

                // Gán lại trạng thái
                ddlTrangThai.SelectedValue = phongBanInfo.Status.ToString();
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
 

    protected void ddlKhuVuc_Changed(object sender, EventArgs e)
    {
        BindDoiTac(ddlKhuVuc.SelectedValue);
    }

    private void BindDoiTac(string khuvucId)
    {
        int pId = DoiTacInfo.DoiTacIdValue.VNP;
        if (ddlKhuVuc.SelectedValue.Equals(DoiTacInfo.DoiTacIdValue.VNP.ToString()))
        {
            ddlDoiTac.Items.Clear();
            ddlDoiTac.Items.Add(new System.Web.UI.WebControls.ListItem("Vinaphone", DoiTacInfo.DoiTacIdValue.VNP.ToString()));
            return;
        }
        else if (ddlKhuVuc.SelectedValue.Equals(DoiTacInfo.DoiTacIdValue.VNPT_NET.ToString()))
        {
            pId = DoiTacInfo.DoiTacIdValue.VNPT_NET;
        }
        else if (ddlKhuVuc.SelectedValue.Equals("10208"))
        {
            pId = 10208;
        }


        List<DoiTacInfo> lstKhuVuc = ServiceFactory.GetInstanceDoiTac().GetListDynamic("Id, MaDoiTac, DonViTrucThuoc", string.Format("Id = {0} or DonViTrucThuoc = {0}", khuvucId), "");
        lstKhuVuc = DoiTacImpl.BuildTree(lstKhuVuc, pId, 0, "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

        ddlDoiTac.DataSource = lstKhuVuc;
        ddlDoiTac.DataValueField = "Id";
        ddlDoiTac.DataTextField = "MaDoiTac";
        ddlDoiTac.DataBind();
    }

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Debugger.Launch();

        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            PhongBanImpl obj = ServiceFactory.GetInstancePhongBan();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    PhongBanInfo phongBanInfo = obj.GetInfo(idEdit);

                    if (phongBanInfo == null)
                    {
                        Utility.LogEvent("Function phongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        phongBanInfo.LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBan.SelectedValue);
                        phongBanInfo.KhuVucId = Convert.ToInt32(ddlKhuVuc.SelectedValue);
                        phongBanInfo.DoiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
                        phongBanInfo.Name = txtName.Text.Trim();
                        phongBanInfo.Description = txtDescription.Text.Trim();
                        phongBanInfo.Sort = Convert.ToInt32(txtSort.Text);
                        phongBanInfo.IsDinhTuyenKN = chkIsDinhTuyenKN.Checked;
                        phongBanInfo.DefaultHTTN = Convert.ToInt16(ddlHTTiepNhan.SelectedValue);
                        phongBanInfo.IsChuyenTiepKN = chkIsChuyenTiepKN.Checked;
                        phongBanInfo.Cap = Convert.ToInt32(ddlCapPhongBan.SelectedValue);
                        phongBanInfo.LUser = UserInfo.Username;

                        // Gán trạng thái
                        phongBanInfo.Status = Convert.ToInt32(ddlTrangThai.SelectedValue);
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    obj.Update(phongBanInfo);
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
                PhongBanInfo phongBanInfo = new PhongBanInfo();

                try
                {
                    phongBanInfo.LoaiPhongBanId = Convert.ToInt32(ddlLoaiPhongBan.SelectedValue);
                    phongBanInfo.KhuVucId = Convert.ToInt32(ddlKhuVuc.SelectedValue);
                    phongBanInfo.DoiTacId = Convert.ToInt32(ddlDoiTac.SelectedValue);
                    phongBanInfo.Name = txtName.Text.Trim();
                    phongBanInfo.Description = txtDescription.Text.Trim();
                    phongBanInfo.Sort = Convert.ToInt32(txtSort.Text);
                    phongBanInfo.IsDinhTuyenKN = chkIsDinhTuyenKN.Checked;
                    phongBanInfo.DefaultHTTN = Convert.ToInt16(ddlHTTiepNhan.SelectedValue);
                    phongBanInfo.IsChuyenTiepKN = chkIsChuyenTiepKN.Checked;
                    phongBanInfo.Cap = Convert.ToInt32(ddlCapPhongBan.SelectedValue);
                    phongBanInfo.LUser = UserInfo.Username;

                    // Gán trạng thái
                    phongBanInfo.Status = Convert.ToInt32(ddlTrangThai.SelectedValue);
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }

                obj.Add(phongBanInfo);
            }

            string url = "PhongBanVNPT_Manager.aspx";
            if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Equals(""))
                url = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);

            Response.Redirect(url, false);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            lblMsg.Text = "Tên phòng ban đã tồn tại.";
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

