using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.admin
{
    public partial class NguoiSuDung_GioiHanGiamTru : System.Web.UI.Page
    {
        #region Event methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2015
        /// Todo : 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvUserGioiHanGiamTru.PageSize = Config.RecordPerPage;
                LoadDoiTac();
                LoadNguoiDungGioiHanGiamTru();
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2015
        /// Todo : Add người dùng vào danh sách được giới hạn giảm trừ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            AdminInfo adminInfo = LoginAdmin.AdminLogin();
            if (adminInfo == null) return;

            List<NguoiSuDung_GioiHanGiamTruInfo> listNguoiSuDung = new List<NguoiSuDung_GioiHanGiamTruInfo>();
            foreach (GridViewRow row in gvUser.Rows)
            {
                CheckBox chkChon = (CheckBox)row.Cells[0].FindControl("chkChon");
                if (chkChon.Checked)
                {
                    NguoiSuDung_GioiHanGiamTruInfo obj = new NguoiSuDung_GioiHanGiamTruInfo();
                    obj.UserId = ConvertUtility.ToInt32(gvUser.DataKeys[row.RowIndex].Value);
                    obj.TenTruyCap = row.Cells[1].Text;
                    obj.MocKhauTruMax = ConvertUtility.ToDecimal(txtGioiHanKhauTruMax.Text);
                    obj.MocKhauTruMin = ConvertUtility.ToDecimal(txtGioiHanKhauTruMin.Text);
                    obj.IsDeleted = false;
                    obj.CUser = adminInfo.Username;
                    obj.LUser = adminInfo.Username;
                    listNguoiSuDung.Add(obj);
                }
            }

            List<string> listUserNameError = new List<string>();
            for (int i = 0; i < listNguoiSuDung.Count; i++)
            {
                try
                {
                    ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().Add(listNguoiSuDung[i]);
                }
                catch (Exception ex)
                {
                    listUserNameError.Add(listNguoiSuDung[i].TenTruyCap);
                    Utility.LogEvent(listNguoiSuDung[i].TenTruyCap + " : " + ex.Message);
                }

            }

            LoadNguoiDung();
            LoadNguoiDungGioiHanGiamTru();
            if (listUserNameError.Count > 0)
            {
                lblErrorMessage.Text = listUserNameError[0];
                for (int i = 1; i < listUserNameError.Count; i++)
                {
                    lblErrorMessage.Text = string.Format("{0}, {1}", listUserNameError[i]);
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// Todo : Thực hiện xóa 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> listIdDeleted = new List<int>();
            foreach (GridViewRow row in gvUserGioiHanGiamTru.Rows)
            {
                CheckBox chkChon = (CheckBox)row.Cells[0].FindControl("chkChon");
                if (chkChon.Checked)
                {
                    listIdDeleted.Add(ConvertUtility.ToInt32(gvUserGioiHanGiamTru.DataKeys[row.RowIndex].Value));
                }
            }

            if (listIdDeleted.Count > 0)
            {
                string whereClause = string.Format("Id IN ({0}", listIdDeleted[0]);
                for (int i = 1; i < listIdDeleted.Count; i++)
                {
                    whereClause = string.Format("{0},{1}", whereClause, listIdDeleted[i]);
                }

                whereClause = string.Format("{0})", whereClause);

                try
                {
                    ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().UpdateDynamic("IsDeleted=1", whereClause);
                }
                catch (Exception ex)
                {
                    Utility.LogEvent("btnDelete_Click : " + ex.Message);
                }

                LoadNguoiDungGioiHanGiamTru();
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 22/11/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            AdminInfo adminInfo = LoginAdmin.AdminLogin();
            if (adminInfo == null) return;

            foreach (GridViewRow row in gvUserGioiHanGiamTru.Rows)
            {
                CheckBox chkChon = (CheckBox)row.Cells[0].FindControl("chkChon");
                if (chkChon.Checked)
                {
                    string id = gvUserGioiHanGiamTru.DataKeys[row.RowIndex].Value.ToString();
                    decimal soTienMax = ConvertUtility.ToDecimal(((TextBox)row.Cells[1].FindControl("txtMocKhauTruMax")).Text, 0);
                    decimal soTienMin = ConvertUtility.ToDecimal(((TextBox)row.Cells[1].FindControl("txtMocKhauTruMin")).Text, 0);
                    string updateClause = string.Format("MocKhauTruMax={0}, MocKhauTruMin={1}, LUser='{2}', LDate=GETDATE()", soTienMax, soTienMin, adminInfo.Username);
                    ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().UpdateDynamic(updateClause, "Id=" + id);
                }
            }

            LoadNguoiDungGioiHanGiamTru();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUserGioiHanGiamTru_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = ConvertUtility.ToInt32(e.CommandArgument);
            if (id > 0)
            {
                ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().UpdateDynamic("IsDeleted=1", "Id=" + id);
            }

            LoadNguoiDungGioiHanGiamTru();
        }

        protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPhongBan(ConvertUtility.ToInt32(ddlDonVi.SelectedValue));
            gvUser.DataSource = null;
            gvUser.DataBind();
        }

        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNguoiDung();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// Todo : 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDonVi_GioiHanGiamTru_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPhongBanGioiHanTru(ConvertUtility.ToInt32(ddlDonVi_GioiHanGiamTru.SelectedValue));
            LoadNguoiDungGioiHanGiamTru();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// Todo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPhongBan_GioiHanGiamTru_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNguoiDungGioiHanGiamTru();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2015
        /// Todo : Lấy ra đối tác
        /// </summary>
        private void LoadDoiTac()
        {
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            string space = string.Empty;
            if (listDoiTac != null)
            {
                List<DoiTacInfo> listDoiTacRoot = listDoiTac.FindAll(delegate(DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });

                for (int i = 0; i < listDoiTacRoot.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    item.Value = listDoiTacRoot[i].Id.ToString();
                    ddlDonVi.Items.Add(item);

                    ListItem itemGioiHanGiamTru = new ListItem();
                    itemGioiHanGiamTru.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    itemGioiHanGiamTru.Value = listDoiTacRoot[i].Id.ToString();
                    ddlDonVi_GioiHanGiamTru.Items.Add(itemGioiHanGiamTru);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
            }

            ListItem item0 = new ListItem("--Chọn đơn vị--", "0");
            ddlDonVi_GioiHanGiamTru.Items.Insert(0, item0);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Lấy ra các đối tác của đơn vị trực thuộc
        /// </summary>
        /// <param name="donViTrucThuoc"></param>
        /// <param name="listDoiTacInfo"></param>
        private void LoadChildDoiTac(string space, int donViTrucThuoc, List<DoiTacInfo> listDoiTacInfo)
        {
            space = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}", space);
            for (int i = 0; i < listDoiTacInfo.Count; i++)
            {
                if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlDonVi.Items.Add(item);

                    ListItem itemGioiHanGiamTru = new ListItem();
                    itemGioiHanGiamTru.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    itemGioiHanGiamTru.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlDonVi_GioiHanGiamTru.Items.Add(itemGioiHanGiamTru);

                    int donViTrucThuocChild = listDoiTacInfo[i].Id;

                    listDoiTacInfo.RemoveAt(i);
                    i = -1;

                    if (listDoiTacInfo.Count == 0)
                    {
                        break;
                    }

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTacInfo);
                }
            }

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2015
        /// Todo : Lấy ra danh sách phòng ban theo đối tác
        /// </summary>
        private void LoadPhongBan(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("*", "DoiTacId=" + doiTacId, "");
            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataTextField = "Name";
            ddlPhongBan.DataValueField = "Id";
            ddlPhongBan.DataBind();

            ListItem item = new ListItem("Chọn phòng ban", "0");
            ddlPhongBan.Items.Insert(0, item);
        }



        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2015
        /// Todo : Lấy ra danh sách người dùng được chọn để có quyền xác nhận giảm trừ
        /// </summary>
        private void LoadNguoiDung()
        {
            int doiTacId = ConvertUtility.ToInt32(ddlDonVi.SelectedValue);
            int phongBanId = ConvertUtility.ToInt32(ddlPhongBan.SelectedValue);

            string innerJoinClause = @" INNER JOIN PhongBan_User pu ON a.Id = pu.NguoiSuDungId
                                        INNER JOIN PhongBan pb ON pu.PhongBanId = pb.Id";
            string whereClause = string.Format(@" a.Id NOT IN (SELECT UserId FROM NguoiSuDung_GioiHanGiamTru WHERE IsDeleted=0)");

            if (doiTacId > 0)
            {
                whereClause = string.Format("{0} AND pb.DoiTacId={1}", whereClause, doiTacId);
            }
            if (phongBanId > 0)
            {
                whereClause = string.Format("{0} AND pb.Id={1}", whereClause, phongBanId);
            }
            List<NguoiSuDungInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung().GetListDynamicJoin("a.*, pb.Name AS TenPhongBan", innerJoinClause, whereClause, "TenTruyCap ASC");

            gvUser.DataSource = listNguoiSuDung;
            gvUser.DataBind();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanId"></param>
        private void LoadNguoiDungGioiHanGiamTru()
        {
            int doiTacId = ConvertUtility.ToInt32(ddlDonVi_GioiHanGiamTru.SelectedValue);
            int phongBanId = ConvertUtility.ToInt32(ddlPhongBan_GioiHanGiamTru.SelectedValue);
            string innerJoinClause = @" INNER JOIN PhongBan_User pu ON a.UserId = pu.NguoiSuDungId
                                        INNER JOIN PhongBan pb ON pu.PhongBanId = pb.Id";
            string whereClause = string.Format(@"IsDeleted = 0");

            if (doiTacId > 0)
            {
                whereClause = string.Format("{0} AND pb.DoiTacId={1}", whereClause, doiTacId);
            }
            if (phongBanId > 0)
            {
                whereClause = string.Format("{0} AND pb.Id={1}", whereClause, phongBanId);
            }
            List<NguoiSuDung_GioiHanGiamTruInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung_GioiHanGiamTru().GetListDynamicJoin("a.*, pb.Name AS TenPhongBan", innerJoinClause, whereClause, "TenTruyCap ASC");

            gvUserGioiHanGiamTru.DataSource = listNguoiSuDung;
            gvUserGioiHanGiamTru.DataBind();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 20/11/2015
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadPhongBanGioiHanTru(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListDynamic("*", "DoiTacId=" + doiTacId, "");
            ddlPhongBan_GioiHanGiamTru.DataSource = listPhongBan;
            ddlPhongBan_GioiHanGiamTru.DataTextField = "Name";
            ddlPhongBan_GioiHanGiamTru.DataValueField = "Id";
            ddlPhongBan_GioiHanGiamTru.DataBind();

            ListItem item = new ListItem("Chọn phòng ban", "0");
            ddlPhongBan_GioiHanGiamTru.Items.Insert(0, item);
        }

        #endregion

        protected void gvUserGioiHanGiamTru_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvUserGioiHanGiamTru.PageIndex = e.NewPageIndex;
            LoadNguoiDungGioiHanGiamTru();
        }



    }
}