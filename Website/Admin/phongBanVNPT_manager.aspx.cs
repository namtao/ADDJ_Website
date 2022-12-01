using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Web;

public partial class admin_phongBanVNPT_manager : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDropDownlist();

            grvView.PageSize = Config.RecordPerPage;
            BindGrid(false);
        }

        if (!Permission.UserEdit)
        {
            liUpdate.Visible = false;
        }
    }

    protected string BindHanhDong(object id, object cap)
    {
        string strReturn = string.Empty;

        if (Permission.UserEdit)
        {
            if (cap.ToString().Equals("1"))
                strReturn += string.Format("<a href='phongBanVNPT_Cap1_add.aspx?ID={0}'>Sửa</a> <br />", id);
            else if (cap.ToString().Equals("2"))
                strReturn += string.Format("<a href='phongBanVNPT_Cap2_add.aspx?ID={0}'>Sửa</a> <br />", id);
            else if (cap.ToString().Equals("3"))
                strReturn += string.Format("<a href='phongBanVNPT_Cap3_add.aspx?ID={0}'>Sửa</a> <br />", id);
            else
                strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Sửa</a> <br />");
        }
        else
        {
            strReturn += string.Format("<a onclick='MessageAlert.AlertJSON(-999);' href='#'>Sửa</a> <br />");
        }

        return strReturn;
    }

    protected string BindLoaiPhongBan(object id)
    {
        if (Convert.ToInt32(id) == ConstFixCode.PHONG_BAN_TIEP_NHAN)
            return "Phòng ban tiếp nhận";
        else
            return "Phòng ban xử lý";
    }

    protected string BindDoiTac(object doitacId)
    {
        try
        {
            return DoiTacImpl.ListDoiTac.Where(t => t.Id == Convert.ToInt32(doitacId)).Single().MaDoiTac;
        }
        catch
        {
            return doitacId.ToString();
        }
    }

    protected string BindHTTN(object objHTTN)
    {
        try
        {
            return Enum.GetName(typeof(KhieuNai_HTTiepNhan_Type), objHTTN).Replace("_", " ");
        }
        catch
        {
            return string.Empty;
        }
    }

    protected string BindCountNguoiDung(object id, object countUser)
    {
        return string.Format("{0} (<a href='nguoiSuDung_manager.aspx?PhongBanId={1}'>Xem danh sách</a>)", countUser, id);
    }

    private void BindDropDownlist()
    {
        //var lst = ServiceFactory.GetInstanceDoiTac().GetList();
        //List<DoiTacInfo> lstDoiTac = new List<DoiTacInfo>();
        //var admin = LoginAdmin.AdminLogin();

        //IEnumerable<DoiTacInfo> parent = null;

        //parent = lst.Where(t => t.Id == admin.DoiTacId);
        //foreach (var item in parent)
        //{
        //    lstDoiTac.Add(item);
        //    var lstChild = lst.Where(t => t.DonViTrucThuoc == item.Id);
        //    foreach (var itemChild in lstChild)
        //    {
        //        itemChild.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild.MaDoiTac);
        //        lstDoiTac.Add(itemChild);

        //        var lstChildC2 = lst.Where(t => t.DonViTrucThuoc == itemChild.Id);
        //        foreach (var itemChild2 in lstChildC2)
        //        {
        //            itemChild2.MaDoiTac = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild2.MaDoiTac);
        //            lstDoiTac.Add(itemChild2);
        //        }
        //    }
        //}        
    }

    private void BindGrid(bool isClearFilter)
    {
        try
        {

            string selectClause = string.Empty;
            selectClause = "*,(select count(*) from PhongBan_User where PhongBanId = PhongBan.Id) CountUser";

            string whereClause = "";
            whereClause += string.Format(" DoiTacId = {0}", UserInfo.DoiTacId);


            string orderbyClause = "";

            var phongBanObj = GetPhongBanSorted(selectClause, whereClause, orderbyClause);

            if (phongBanObj != null && phongBanObj.Count > 0)
            {
                ltThongBao.Text = "<font color='red'>Có " + phongBanObj.Count + " phòng ban được tìm thấy.</font>";
                grvView.DataSource = phongBanObj;
                grvView.DataBind();
            }
            else
            {
                ltThongBao.Text = "<font color='red'>Không có phòng ban được tìm thấy.</font>";
                grvView.DataSource = null;
                grvView.DataBind();
            }

        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        finally
        {
            if (IsPostBack)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "LoadJS();", true);
            }
        }
    }

    private List<PhongBanInfo> GetPhongBanSorted(string selectClause, string whereClause, string orderbyClause)
    {
        List<PhongBanInfo> lstReturn = null;
        var phongBanObj = ServiceFactory.GetInstancePhongBan().GetListDynamic(selectClause, whereClause, orderbyClause);
        if (phongBanObj != null)
        {
            lstReturn = new List<PhongBanInfo>();
            PhongBanInfo item;
            foreach (var itemParent in phongBanObj.Where(t => t.ParentId == 0))
            {
                item = new PhongBanInfo();
                item.Id = itemParent.Id;
                item.Cap = itemParent.Cap;
                item.CountUser = itemParent.CountUser;
                item.DefaultHTTN = itemParent.DefaultHTTN;
                item.Description = itemParent.Description;
                item.DoiTacId = itemParent.DoiTacId;
                item.IsChuyenTiepKN = itemParent.IsChuyenTiepKN;
                item.IsChuyenVNP = itemParent.IsChuyenVNP;
                item.IsDinhTuyenKN = itemParent.IsDinhTuyenKN;
                item.KhuVucId = itemParent.KhuVucId;
                item.LoaiPhongBanId = itemParent.LoaiPhongBanId;
                item.LUser = itemParent.LUser;
                item.ParentId = itemParent.ParentId;
                item.Sort = itemParent.Sort;
                item.SortOrder = itemParent.SortOrder;
                item.Name = itemParent.Name;
                lstReturn.Add(item);

                foreach (var itemChild in phongBanObj.Where(t => t.ParentId == itemParent.Id))
                {
                    item = new PhongBanInfo();
                    item.Id = itemChild.Id;
                    item.Cap = itemChild.Cap;
                    item.CountUser = itemChild.CountUser;
                    item.DefaultHTTN = itemChild.DefaultHTTN;
                    item.Description = itemChild.Description;
                    item.DoiTacId = itemChild.DoiTacId;
                    item.IsChuyenTiepKN = itemChild.IsChuyenTiepKN;
                    item.IsChuyenVNP = itemChild.IsChuyenVNP;
                    item.IsDinhTuyenKN = itemChild.IsDinhTuyenKN;
                    item.KhuVucId = itemChild.KhuVucId;
                    item.LoaiPhongBanId = itemChild.LoaiPhongBanId;
                    item.LUser = itemChild.LUser;
                    item.ParentId = itemChild.ParentId;
                    item.Sort = itemChild.Sort;
                    item.SortOrder = itemChild.SortOrder;
                    item.Name = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChild.Name);

                    lstReturn.Add(item);

                    foreach (var itemChildEnd in phongBanObj.Where(t => t.ParentId == itemChild.Id))
                    {
                        item = new PhongBanInfo();
                        item.Id = itemChildEnd.Id;
                        item.Cap = itemChildEnd.Cap;
                        item.CountUser = itemChildEnd.CountUser;
                        item.DefaultHTTN = itemChildEnd.DefaultHTTN;
                        item.Description = itemChildEnd.Description;
                        item.DoiTacId = itemChildEnd.DoiTacId;
                        item.IsChuyenTiepKN = itemChildEnd.IsChuyenTiepKN;
                        item.IsChuyenVNP = itemChildEnd.IsChuyenVNP;
                        item.IsDinhTuyenKN = itemChildEnd.IsDinhTuyenKN;
                        item.KhuVucId = itemChildEnd.KhuVucId;
                        item.LoaiPhongBanId = itemChildEnd.LoaiPhongBanId;
                        item.LUser = itemChildEnd.LUser;
                        item.ParentId = itemChildEnd.ParentId;
                        item.Sort = itemChildEnd.Sort;
                        item.SortOrder = itemChildEnd.SortOrder;
                        item.Name = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + itemChildEnd.Name);

                        lstReturn.Add(item);
                    }
                }
            }
        }

        return lstReturn;
    }


    private void RowDataBound(GridViewRowEventArgs e)
    {
        if (e.Row.DataItemIndex != -1)
        {

            e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();
        }
    }

    private void PageIndexChanging(GridViewPageEventArgs e)
    {
        grvView.PageIndex = e.NewPageIndex;
        BindGrid(false);
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
 

    protected void btThemMoiC1_Click(object sender, EventArgs e)
    {
        Response.Redirect("phongBanVNPT_Cap1_add.aspx", false);
    }
 

    protected void btDelete_Click(object sender, EventArgs e)
    {
        if (!Permission.UserDelete)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            int i = 0;
            var obj = ServiceFactory.GetInstancePhongBan();
            foreach (GridViewRow row in grvView.Rows)
            {
                var status = (CheckBox)row.FindControl("cbSelectAll");
                if (status.Checked)
                {

                    int ID = int.Parse(grvView.DataKeys[i].Value.ToString());
                    var checkCreate = ServiceFactory.GetInstancePhongBan().GetInfo(ID);
                    //if (checkCreate != null && checkCreate.CUser == userInfo.Username)
                    if (checkCreate != null) // Cho phép xóa phòng ban bất kỳ. Bên trong sẽ kiểm tra xem phòng ban có tồn tại khiếu nại hay không
                    {
                        var checkUserInPhongBan = ServiceFactory.GetInstancePhongBan_User().GetListByPhongBanId(ID);
                        if (checkUserInPhongBan != null && checkUserInPhongBan.Count == 0)
                        {
                            var checkKhieuNai = ServiceFactory.GetInstanceKhieuNai().GetListDynamic("Id", "PhongBanTiepNhanId=" + ID + " OR PhongBanXuLyId=" + ID, "");
                            if (checkKhieuNai != null && checkKhieuNai.Count > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phòng ban " + checkCreate.Name + " có dữ liệu khiếu nại. Hệ thống không cho phép xoá.','error');", true);
                                return;
                            }
                            else
                            {
                                if (checkCreate.Cap != 3)
                                {
                                    var checkCon = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id", "ParentId=" + checkCreate.Id, "");
                                    if (checkCon != null && checkCon.Count == 0)
                                    {
                                        obj.Delete(ID);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phòng ban " + checkCreate.Name + " đang có phòng ban trực thuộc. Bạn phải loại bỏ các phòng ban trực thuộc khỏi phòng ban trước khi xóa.','error');", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    obj.Delete(ID);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Phòng ban " + checkCreate.Name + " đang có người dùng. Bạn phải loại bỏ người dùng khỏi phòng ban trước khi xóa.','error');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa phòng ban mặc định của hệ thống.','error');", true);
                        return;
                    }
                }
                i++;
            }
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Không xóa được vì có dữ liệu liên quan.','error');", true);
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
        BindGrid(false);
    }

    protected void linkbtnThemMoiC2_Click(object sender, EventArgs e)
    {
        Response.Redirect("phongBanVNPT_Cap2_add.aspx", false);
    }

    protected void linkbtnThemMoiC3_Click(object sender, EventArgs e)
    {
        Response.Redirect("phongBanVNPT_Cap3_add.aspx", false);
    }
}

