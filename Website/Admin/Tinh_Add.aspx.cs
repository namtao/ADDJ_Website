using System;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Admin;
using System.Linq;
using System.Globalization;

public partial class admin_tinh_add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            lblMsg.Text = "";
            LoadTinh();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
                lblNote.Visible = false;
            }            
            else
            {
                linkbtnSubmit.Text = "<span class=\"glyphicon glyphicon-plus - sign\"></span> Thêm mới";                
            }
        }
    }
    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstanceProvince();
            ProvinceInfo item = obj.GetInfo(Convert.ToInt32(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData tinh_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {        
                if(item.LevelNbr == 1)
                {
                    //ddlTinh.SelectedValue = item.Id.ToString();
                    LoadDanhSachDonVi(1, item.Id);

                    lblTitleTenDonVi.Text = "Tên Tỉnh/Thành phố";
                    lblTitleMaDonVi.Text = "Mã Tỉnh/Thành phố";
                }
                else if(item.LevelNbr == 2)
                {
                    ddlTinh.SelectedValue = item.ParentId.ToString();
                    LoadQuanHuyen(item.ParentId);
                    //ddlQuan.SelectedValue = item.Id.ToString();
                    LoadDanhSachDonVi(2, item.Id);

                    lblTitleTenDonVi.Text = "Tên Quận/Huyện";
                    lblTitleMaDonVi.Text = "Mã Quận/Huyện";
                }
                else if(item.LevelNbr == 3)
                {
                    LoadTinh();
                    ProvinceInfo parentInfo = ServiceFactory.GetInstanceProvince().GetInfo(item.ParentId);
                    if(parentInfo != null)
                    {
                        ddlTinh.SelectedValue = parentInfo.ParentId.ToString();
                        LoadQuanHuyen(parentInfo.ParentId);
                        ddlQuan.SelectedValue = parentInfo.Id.ToString();
                        LoadDanhSachDonVi(2, parentInfo.Id);
                    }

                    lblTitleTenDonVi.Text = "Tên Phường/Xã";
                    lblTitleMaDonVi.Text = "Mã Phường/Xã";
                }
                

                txtMaDonVi.Text = item.AbbRev.ToString();
                txtTenDonVi.Text = item.Name.ToString();
                //chkTrangThai.Checked = item.
                if(item.KhuVucId > 0)
                {
                    ddlKhuVuc.SelectedValue = item.KhuVucId.ToString();
                }                
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void LoadTinh()
    {
        try
        {
            var lstTinh = ServiceFactory.GetInstanceProvince().GetListDynamic("Id,Name", "ParentId IS NULL OR ParentId = 0", "Name");
            lstTinh.Insert(0, new ProvinceInfo() { Id = 0, Name = "--Chọn Tỉnh/Thành Phố--" });
            ddlTinh.DataSource = lstTinh;
            ddlTinh.DataTextField = "Name";
            ddlTinh.DataValueField = "Id";
            ddlTinh.DataBind();
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }

    private void LoadQuanHuyen(int tinhId)
    {
        List<ProvinceInfo> listQuanHuyen = ServiceFactory.GetInstanceProvince().GetListDynamic("Id, Name", "ParentId=" + tinhId.ToString(), "Name ASC");
        listQuanHuyen.Insert(0, new ProvinceInfo {Id =0, Name = "--Chọn Quận/Huyện--"});
        ddlQuan.DataSource = listQuanHuyen;
        ddlQuan.DataTextField = "Name";
        ddlQuan.DataValueField = "Id";
        ddlQuan.DataBind();        
    }

    private void LoadDanhSachDonVi(int level, int parentId)
    {
        List<ProvinceInfo> listDonVi = ServiceFactory.GetInstanceProvince().GetListDynamic("Id, Name", "ParentId=" + parentId, "Name ASC");
        gvDonVi.DataSource = listDonVi;
        gvDonVi.DataBind();

        if(level == 1)
        {
            lblDanhSachDonVi.Text = "Danh sách Quận/Huyện";
        }
        else if (level == 2)
        {
            lblDanhSachDonVi.Text = "Danh sách Phường/Xã";
        }
    }

 

    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 12/10/2015
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTinh_SelectedIndexChanged(object sender, EventArgs e)
    {
        int tinhId = ConvertUtility.ToInt32(ddlTinh.SelectedValue);
        LoadQuanHuyen(tinhId);
        LoadDanhSachDonVi(1, tinhId);
        if(tinhId > 0)
        {
            ProvinceInfo provinceInfo = ServiceFactory.GetInstanceProvince().GetInfo(tinhId);
            if(provinceInfo != null)
            {
                ddlKhuVuc.SelectedValue = provinceInfo.KhuVucId.ToString();
            }

            lblTitleTenDonVi.Text = "Tên Quận/Huyện";
            lblTitleMaDonVi.Text = "Mã Quận/Huyện";
        }
        else
        {
            lblTitleTenDonVi.Text = "Tên Tỉnh/Thành phố";
            lblTitleMaDonVi.Text = "Mã Tỉnh/Thành phố";
        }
    }

    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 12/10/2015
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlQuan_SelectedIndexChanged(object sender, EventArgs e)
    {
        int quanHuyenId = ConvertUtility.ToInt32(ddlQuan.SelectedValue);
        LoadDanhSachDonVi(2, quanHuyenId);
        if(quanHuyenId > 0)
        {
            lblTitleTenDonVi.Text = "Tên Phường/Xã";
            lblTitleMaDonVi.Text = "Mã Phường/Xã";
        }
        else
        {
            lblTitleTenDonVi.Text = "Tên Quận/Huyện";
            lblTitleMaDonVi.Text = "Mã Quận/Huyện";
        }
    }

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTenDonVi.Text.Trim().Length <= 0)
        {
            lblMsg.Text = "Bạn phải nhập tên đơn vị";
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstanceProvince();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    ProvinceInfo item = obj.GetInfo(Convert.ToInt32(Request.QueryString["ID"]));

                    if (item == null)
                    {
                        Utility.LogEvent("Function tinh_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        if (ConvertUtility.ToInt32(ddlQuan.SelectedValue) > 0)
                        {
                            item.ParentId = ConvertUtility.ToInt32(ddlQuan.SelectedValue);
                            item.LevelNbr = 3;
                        }
                        else if (ConvertUtility.ToInt32(ddlTinh.SelectedValue) > 0)
                        {
                            item.ParentId = ConvertUtility.ToInt32(ddlTinh.SelectedValue);
                            item.LevelNbr = 2;
                        }
                        else
                        {
                            item.ParentId = 0;
                            item.LevelNbr = 1;
                        }

                        item.AbbRev = txtMaDonVi.Text.ToString().Trim();
                        item.Name = txtTenDonVi.Text.Trim();
                        item.KhuVucId = ConvertUtility.ToInt32(ddlKhuVuc.SelectedValue);
                        item.LUser = LoginAdmin.AdminLogin().Username;

                        string whereClause = string.Format("ParentId={0} AND Name=N'{1}' AND Id <> {2}", item.ParentId, item.Name, item.Id);
                        List<ProvinceInfo> listTemp = ServiceFactory.GetInstanceProvince().GetListDynamic("Id", whereClause, "");
                        if (listTemp != null && listTemp.Count > 0)
                        {
                            lblMsg.Text = string.Format("Tên \"{0}\" này đã tồn tại, hãy kiểm tra lại.", item.Name);
                            return;
                        }
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
                    Response.Redirect(Config.PathError, false);
                    return;
                }
            }
            else
            {
                try
                {
                    try
                    {
                        if (txtTenDonVi.Text.Contains(','))
                        {
                            string[] arrTenDonVi = txtTenDonVi.Text.Split(',');
                            if (arrTenDonVi != null && arrTenDonVi.Length > 0)
                            {
                                ProvinceInfo item = new ProvinceInfo();

                                if (ConvertUtility.ToInt32(ddlQuan.SelectedValue) > 0)
                                {
                                    item.ParentId = ConvertUtility.ToInt32(ddlQuan.SelectedValue);
                                    item.LevelNbr = 3;
                                }
                                else if (ConvertUtility.ToInt32(ddlTinh.SelectedValue) > 0)
                                {
                                    item.ParentId = ConvertUtility.ToInt32(ddlTinh.SelectedValue);
                                    item.LevelNbr = 2;
                                }
                                else
                                {
                                    item.ParentId = 0;
                                    item.LevelNbr = 1;
                                }

                                item.AbbRev = txtMaDonVi.Text.ToString().Trim();
                                item.KhuVucId = ConvertUtility.ToInt32(ddlKhuVuc.SelectedValue);
                                item.CUser = LoginAdmin.AdminLogin().Username;
                                item.LUser = LoginAdmin.AdminLogin().Username;

                                for (int i = 0; i < arrTenDonVi.Length; i++)
                                {
                                    item.Name = arrTenDonVi[i].Trim();

                                    string whereClause = string.Format("ParentId={0} AND Name=N'{1}'", item.ParentId, item.Name);
                                    List<ProvinceInfo> listTemp = ServiceFactory.GetInstanceProvince().GetListDynamic("Id", whereClause, "");
                                    if (listTemp != null && listTemp.Count > 0)
                                    {
                                        lblMsg.Text = string.Format("Tên \"{0}\" này đã tồn tại, hãy kiểm tra lại.", item.Name);
                                        return;
                                    }

                                    obj.Add(item);
                                } // end for(int i=0;i<arrTenDonVi.Length;i++)
                            } // end if(arrTenDonVi != null && arrTenDonVi.Length > 0)
                        } // end if(txtTenDonVi.Text.Contains(','))
                        else
                        {
                            ProvinceInfo item = new ProvinceInfo();

                            if (ConvertUtility.ToInt32(ddlQuan.SelectedValue) > 0)
                            {
                                item.ParentId = ConvertUtility.ToInt32(ddlQuan.SelectedValue);
                                item.LevelNbr = 3;
                            }
                            else if (ConvertUtility.ToInt32(ddlTinh.SelectedValue) > 0)
                            {
                                item.ParentId = ConvertUtility.ToInt32(ddlTinh.SelectedValue);
                                item.LevelNbr = 2;
                            }
                            else
                            {
                                item.ParentId = 0;
                                item.LevelNbr = 1;
                            }

                            item.AbbRev = txtMaDonVi.Text.ToString().Trim();
                            item.Name = txtTenDonVi.Text.Trim();
                            item.KhuVucId = ConvertUtility.ToInt32(ddlKhuVuc.SelectedValue);
                            item.CUser = LoginAdmin.AdminLogin().Username;
                            item.LUser = LoginAdmin.AdminLogin().Username;

                            string whereClause = string.Format("ParentId={0} AND Name=N'{1}'", item.ParentId, item.Name);
                            List<ProvinceInfo> listTemp = ServiceFactory.GetInstanceProvince().GetListDynamic("Id", whereClause, "");
                            if (listTemp != null && listTemp.Count > 0)
                            {
                                lblMsg.Text = string.Format("Tên \"{0}\" này đã tồn tại, hãy kiểm tra lại.", item.Name);
                                return;
                            }

                            obj.Add(item);
                        }

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
            Response.Redirect("tinh_manager.aspx", false);
        }
        catch (System.Data.SqlClient.SqlException se)
        {
            lblMsg.Text = "Tên tỉnh đã tồn tại.";
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
}

