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

public partial class admin_phongBanVNPT_Cap2_add : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMsg.Text = "";

            BindDropDownlist();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                EditData();
            }
        }
    }

    private void BindDropDownlist()
    {

        var lstPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("Id,ParentId,Name,Cap", "(Cap = 1) AND DoiTacId=" + UserInfo.DoiTacId, "Cap");
        foreach (var item in lstPhongBan)
        {
            item.Name = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + item.Name);

        }
        ddlPhongBanDinhTuyen.DataSource = lstPhongBan;
        ddlPhongBanDinhTuyen.DataValueField = "Id";
        ddlPhongBanDinhTuyen.DataTextField = "Name";
        ddlPhongBanDinhTuyen.DataBind();
    }

    private void EditData()
    {
        try
        {
            var obj = ServiceFactory.GetInstancePhongBan();
            PhongBanInfo item = obj.GetInfo(int.Parse(Request.QueryString["ID"]));
            if (item == null)
            {
                Utility.LogEvent("Function EditData phongBan_add get NullId " + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {

                txtName.Text = item.Name.ToString();
                txtDescription.Text = item.Description.ToString();

                //chkIsChuyenTiepKN.Checked = item.IsChuyenTiepKN;
                //ddlHTTiepNhan.SelectedValue = item.DefaultHTTN.ToString();
                chkChuyenTiepVNP.Checked = item.IsChuyenVNP;
                ddlPhongBanDinhTuyen.SelectedValue = item.ParentId.ToString();
            }
        }
        catch (Exception ex)
        {
            Utility.LogEvent(ex);
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
    

    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            var obj = ServiceFactory.GetInstancePhongBan();
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                try
                {
                    int idEdit = int.Parse(Request.QueryString["ID"]);
                    PhongBanInfo item = obj.GetInfo(idEdit);

                    if (item == null)
                    {
                        Utility.LogEvent("Function phongBan_add Edit Khong tim thay ban ghi nao voi Id" + Request.QueryString["ID"], System.Diagnostics.EventLogEntryType.Warning);
                        Response.Redirect(Config.PathError, false);
                        return;
                    }

                    try
                    {
                        item.ParentId = Convert.ToInt32(ddlPhongBanDinhTuyen.SelectedValue);
                        if (item.ParentId == 0)
                            item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_XU_LY;
                        else
                            item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_TIEP_NHAN;


                        item.KhuVucId = UserInfo.KhuVucId;
                        item.DoiTacId = UserInfo.DoiTacId;

                        item.Name = txtName.Text.Trim();
                        item.Description = txtDescription.Text.Trim();


                        item.IsDinhTuyenKN = false;
                        item.DefaultHTTN = (byte)KhieuNai_HTTiepNhan_Type.Điểm_giao_dịch;
                        item.IsChuyenTiepKN = false;
                        item.IsChuyenVNP = chkChuyenTiepVNP.Checked;

                        if (item.ParentId == 0)
                        {
                            var lstSort = ServiceFactory.GetInstancePhongBan().GetListDynamic("Sort", "ParentId=0 and DoiTacId=" + UserInfo.DoiTacId, "");
                            if (lstSort != null && lstSort.Count > 0)
                            {
                                item.Sort = lstSort[0].Sort;
                            }
                            else
                                item.Sort = 0;
                            item.Cap = 1;
                        }
                        else
                        {
                            var itemSort = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(ddlPhongBanDinhTuyen.SelectedValue));
                            if (itemSort != null)
                            {
                                item.Sort = itemSort.Sort;
                                item.Cap = itemSort.Cap + 1;
                            }
                            else
                                throw new Exception("Phòng ban cha không tồn tại.");
                        }

                        item.LUser = UserInfo.Username;
                    }
                    catch
                    {
                        lblMsg.Text = "Dữ liệu không hợp lệ";
                        return;
                    }

                    var strPhongBanDen = string.Empty;
                    if (item.ParentId == 0)
                    {
                        if (item.KhuVucId == 2)
                        {
                            strPhongBanDen = "[53,54,55,56,58,134]";
                        }
                        else if (item.KhuVucId == 3)
                        { strPhongBanDen = "[62,63,64,65,72,217]"; }
                        else if (item.KhuVucId == 5)
                        { strPhongBanDen = "[67,68,69,70,73,188]"; }
                        else
                        {
                            lblMsg.Text = "Không tìm thấy danh sách phòng ban Vinaphone gửi khiếu nại.";
                            return;
                        }
                    }
                    else
                    {
                        strPhongBanDen = string.Format("[{0}]", item.ParentId);
                    }

                    var checkExists = ServiceFactory.GetInstancePhongBan2PhongBan().GetListByPhongBanId(item.Id);
                    if (checkExists != null && checkExists.Any())
                    {
                        var itemUpdatePhongban2Phongban = checkExists[0];
                        itemUpdatePhongban2Phongban.PhongBanDen = strPhongBanDen;
                        ServiceFactory.GetInstancePhongBan2PhongBan().Update(itemUpdatePhongban2Phongban);
                        obj.Update(item);
                    }
                    else
                    {

                        ServiceFactory.GetInstancePhongBan2PhongBan().Add(new PhongBan2PhongBanInfo() { PhongBanId = item.Id, PhongBanDen = strPhongBanDen });
                        obj.Update(item);
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
                var item = new PhongBanInfo();

                try
                {
                    item.ParentId = Convert.ToInt32(ddlPhongBanDinhTuyen.SelectedValue);

                    if (item.ParentId == 0)
                        item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_XU_LY;
                    else
                        item.LoaiPhongBanId = ConstFixCode.PHONG_BAN_TIEP_NHAN;


                    item.KhuVucId = UserInfo.KhuVucId;
                    item.DoiTacId = UserInfo.DoiTacId;

                    item.Name = txtName.Text.Trim();
                    item.Description = txtDescription.Text.Trim();


                    item.IsDinhTuyenKN = true;
                    item.DefaultHTTN = (byte)KhieuNai_HTTiepNhan_Type.Điểm_giao_dịch;
                    item.IsChuyenTiepKN = false;
                    item.IsChuyenVNP = chkChuyenTiepVNP.Checked;

                    if (item.ParentId == 0)
                    {
                        var lstSort = ServiceFactory.GetInstancePhongBan().GetListDynamic("Sort", "ParentId=0 and DoiTacId=" + UserInfo.DoiTacId, "");
                        if (lstSort != null && lstSort.Count > 0)
                        {
                            item.Sort = lstSort[0].Sort;
                        }
                        else
                            item.Sort = 0;
                        item.Cap = 1;
                        //item.Sort = Convert.ToInt32(txtSort.Text);
                    }
                    else
                    {
                        var itemSort = ServiceFactory.GetInstancePhongBan().GetInfo(Convert.ToInt32(ddlPhongBanDinhTuyen.SelectedValue));
                        if (itemSort != null)
                        {
                            item.Sort = itemSort.Sort;
                            item.Cap = itemSort.Cap + 1;
                        }
                        else
                            throw new Exception("Phòng ban cha không tồn tại.");
                    }

                    item.LUser = UserInfo.Username;
                }
                catch
                {
                    lblMsg.Text = "Dữ liệu không hợp lệ";
                    return;
                }


                var strPhongBanDen = string.Empty;
                if (item.ParentId == 0)
                {
                    if (item.KhuVucId == 2)
                    {
                        strPhongBanDen = "[53,54,55,56,58,134]";
                    }
                    else if (item.KhuVucId == 3)
                    { strPhongBanDen = "[62,63,64,65,72,217]"; }
                    else if (item.KhuVucId == 5)
                    { strPhongBanDen = "[67,68,69,70,73,188]"; }
                    else
                    {
                        lblMsg.Text = "Không tìm thấy danh sách phòng ban Vinaphone gửi khiếu nại.";
                        return;
                    }
                }
                else
                {
                    strPhongBanDen = string.Format("[{0}]", item.ParentId);
                }
                item.Id = obj.Add(item);
                if (item.Id > 0)
                    ServiceFactory.GetInstancePhongBan2PhongBan().Add(new PhongBan2PhongBanInfo() { PhongBanId = item.Id, PhongBanDen = strPhongBanDen });

            }

            string url = "phongBanVNPT_manager.aspx";
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

