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

namespace Website.Views.BaoCao.UC
{
    public partial class uc_ReportType14_VNP : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }

        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);

            if (this.IsFirstLoad)
            {
                // Trường hợp ĐKT sử dụng báo cáo này thì phải gán lại giá trị DoiTacId theo giá trị Id của VNP khu vực => mới ra được các VNPT trực thuộc khu vực
                switch (this.DoiTacId)
                {
                    case 7:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP1;
                        break;
                    case 14:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP2;
                        break;
                    case 19:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP3;
                        break;
                }

                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                LoadComboDoiTac(this.DoiTacId);
                LoadComboDanhSachNguoiDungThuocNhom();
            }
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            int isDonVi;
            if (rdoTheoDonVi.Checked) 
                isDonVi = 1;  // nếu theo đơn vị
            else
                isDonVi = 0;  // nếu theo cá nhân
            int khuVucId = ConvertUtility.ToInt32(ddlDoiTac.SelectedValue);
            int doiTacId = ConvertUtility.ToInt32(ddlDoiTac.SelectedValue);
            int caNhanXuLy = ConvertUtility.ToInt32(ddlDSNguoiDungThuocNhom.SelectedValue);
            string tenDoiTac = ddlDoiTac.SelectedItem != null ? ddlDoiTac.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;

            // báo cáo xử lý theo khu vực và đối tác
            int isKhuVuc = 0; // mặc định là đối tác
            if (khuVucId == 1 || khuVucId == 2 || khuVucId == 5) // nếu theo khu vực
            {
                isKhuVuc = 1;
            }

            string errorMessage = string.Empty;
            string script = string.Empty;

            if(!rdoTheoDonVi.Checked && !rdoTheoCaNhan.Checked)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn kiểu báo cáo lấy theo đơn vị hoặc cá nhân", errorMessage);
            }
            if (fromDate.Length == 0 || toDate.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải nhập ngày báo cáo", errorMessage);
            }
            else
            {
                DateTime dateCheck = ConvertUtility.ToDateTime(fromDate, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nTừ ngày không hợp lệ", errorMessage);
                }

                dateCheck = ConvertUtility.ToDateTime(toDate, "dd/MM/yyyy", nullDateTime);
                if (dateCheck == nullDateTime)
                {
                    errorMessage = string.Format("{0}\\nĐến ngày không hợp lệ", errorMessage);
                }
            }

            var page = "";
            if (lblReportType.Text == "bc_VNP_BaoCaoChiTietGiamTruDVGTGT")
            {
                page = "baocaogiamtru_dv_gtgt.aspx?";
            }

            if (page == "")
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại báo cáo", errorMessage);
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}donViId={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", lblTittle.Text, page, donViId, donVi, fromDate, toDate, loaibc);                      
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}fromPage=baocaogiamtru_dv_gtgt&isDonVi={2}&isKhuVuc={3}&khuVucId={4}&doiTacId={5}&tenDoiTac={6}&caNhanXuLy={7}&fromDate={8}&toDate={9}&loaibc={10}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", 
                    lblTittle.Text,                  
                    page,
                    isDonVi,
                    isKhuVuc,
                    khuVucId,
                    doiTacId,
                    tenDoiTac,
                    caNhanXuLy,
                    fromDate,
                    toDate, 
                    loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 11/12/2013
        /// </summary>
        /// <param name="doiTacId"></param>
        private void LoadComboDoiTac(int doiTacId)
        {
            AdminInfo adminInfo = LoginAdmin.AdminLogin();
            if (adminInfo != null)
            {
                List<DoiTacInfo> listDoiTac = null;

                if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                {
                    Dictionary<int, string> listKhuVuc = new Dictionary<int, string>();
                    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP1, "VNPT Khu vực 1");
                    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP2, "VNPT Khu vực 2");
                    listKhuVuc.Add(DoiTacInfo.DoiTacIdValue.VNP3, "VNPT Khu vực 3");

                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType IN (1, 4)", "DoiTacType ASC, TenDoiTac ASC");
                    if (listDoiTac != null)
                    {
                        ListItem item = null;
                        foreach (KeyValuePair<int, string> itemKhuVuc in listKhuVuc)
                        {
                            item = new ListItem(itemKhuVuc.Value, itemKhuVuc.Key.ToString());
                            ddlDoiTac.Items.Add(item);

                            for (int i = 0; i < listDoiTac.Count; i++)
                            {
                                if (listDoiTac[i].DonViTrucThuoc == itemKhuVuc.Key)
                                {
                                    item = new ListItem("\xA0\xA0\xA0\xA0\xA0" + listDoiTac[i].TenDoiTac, listDoiTac[i].Id.ToString());
                                    ddlDoiTac.Items.Add(item);
                                    listDoiTac.RemoveAt(i);
                                    i--;
                                }
                            }
                        } // end foreach(KeyValuePair<int, string> itemKhuVuc in listKhuVuc)

                        item = new ListItem("Tất cả", DoiTacInfo.DoiTacIdValue.VNP.ToString());
                        ddlDoiTac.Items.Insert(0, item);
                    }
                }
                else
                {
                    // phân quyền xem báo cáo theo công văn 601
                    switch (adminInfo.DoiTacId)
                    {
                        // nếu là đối tác thuê ngoài VNP
                        case 13: // Minh phúc HN
                        case 10205: // Trường minh HN
                        case 10192: // Hoa sao
                        case 20: // Minh phúc ĐN
                        case 10206: // Trường minh ĐN
                        case 10204: // Minh phúc HCM
                        case 10207: // Trường minh HCM
                        case 18:  // Kasaco
                        case 23:  // 9192
                                  // chỉ được xem chính đơn vị mình
                            listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND Id=" + doiTacId, "TenDoiTac ASC");
                            if (listDoiTac.Count > 0)
                            {
                                ddlDoiTac.DataSource = listDoiTac;
                                ddlDoiTac.DataBind();
                            }
                            break;
                    }
                    // 2. TTKD (63 Tỉnh thành theo mã tỉnh)
                    // kiểm tra xem có thuộc phòng phụ trách không
                    //var checkUserthuoctttt = ServiceFactory.GetInstanceDoiTac().GetListDynamicJoin("a.*", "INNER JOIN dbo.Province b on a.MaDoiTac=b.AbbRev", "b.ParentId IS NULL and a.Id=" + adminInfo.PhongBanId, "");
                    //if (checkUserthuoctttt.Count > 0)
                    //{
                        // 1 user không thuộc phòng phụ trách hoặc nó không thuộc phòng nào đều thỏa mãn
                        var checkUserPhuTrach = ServiceFactory.GetInstancePhongBan().GetListDynamicJoin("", "INNER JOIN	dbo.PhongBan_User b ON a.id=b.PhongBanId ", "Name LIKE N'%PT' AND b.NguoiSuDungId =" + adminInfo.Id, "");
                        if (checkUserPhuTrach != null)
                        {
                            // chỉ được xem đơn vị mình
                            listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND Id=" + adminInfo.DoiTacId, "TenDoiTac ASC");
                            if (listDoiTac.Count > 0)
                            {
                                ddlDoiTac.DataSource = listDoiTac;
                                ddlDoiTac.DataBind();
                            }
                        }
                    //}

                    // 3. Đài HTKH Miền Bắc: Tổ XLNV, GQKN, IB
                    switch (adminInfo.DoiTacId)
                    {
                        case 7:
                            switch (adminInfo.PhongBanId)
                            {
                                case 54: // Tổ XLNV
                                case 63:
                                case 68:
                                case 53: // Tổ GQKN
                                case 62:
                                case 67:
                                case 56: // Tổ OB
                                case 65:
                                case 70:
                                    // xem số liệu đơn vị mình, 3 đối tác thuê ngoài và 28 ttkd miền bắc
                                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND Id=" + adminInfo.DoiTacId + " or Id in(13, 10205, 10192,10035,10036,10037,10038,10039,10040,10041,10042,10043,10044,10045,10046,10047,10048,10049,10050,10051,10052,10053,10054,10055,10056,10057,10058,10059,10060,10061,10099)", "TenDoiTac ASC");
                                    if (listDoiTac.Count > 0)
                                    {
                                        ddlDoiTac.DataSource = listDoiTac;
                                        ddlDoiTac.DataBind();
                                    }
                                    break;
                            }
                            break;
                    }
                    // 4. Đài HTKH Miền Trung: Tổ XLNV, GQKN, IB
                    switch (adminInfo.DoiTacId)
                    {
                        case 14:
                            switch (adminInfo.PhongBanId)
                            {
                                case 54: // Tổ XLNV
                                case 63:
                                case 68:
                                case 53: // Tổ GQKN
                                case 62:
                                case 67:
                                case 56: // Tổ OB
                                case 65:
                                case 70:
                                    // xem số liệu đơn vị mình, 2 đối tác thuê ngoài và 13 ttkd miền trung
                                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND Id=" + adminInfo.DoiTacId + " or Id in(20, 10206, 10063,10064,10065,10066,10067,10068,10069,10070,10071,10072,10073,10074,10075)", "TenDoiTac ASC");
                                    if (listDoiTac.Count > 0)
                                    {
                                        ddlDoiTac.DataSource = listDoiTac;
                                        ddlDoiTac.DataBind();
                                    }
                                    break;
                            }
                            break;
                    }
                    // 5. Đài HTKH Miền Nam: Tổ XLNV, GQKN, IB
                    switch (adminInfo.DoiTacId)
                    {
                        case 19:
                            switch (adminInfo.PhongBanId)
                            {
                                case 54: // Tổ XLNV
                                case 63:
                                case 68:
                                case 53: // Tổ GQKN
                                case 62:
                                case 67:
                                case 56: // Tổ OB
                                case 65:
                                case 70:
                                    // xem số liệu đơn vị mình, 4 đối tác thuê ngoài và 22 ttkd miền nam
                                    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND Id=" + adminInfo.DoiTacId + " or Id in(10204, 10207, 18, 23, 10077,10078,10079,10080,10081,10082,10083,10084,10085,10086,10087,10088,10089,10090,10091,10092,10093,10094,10095,10096,10097,10098)" , "TenDoiTac ASC");
                                    if (listDoiTac.Count > 0)
                                    {
                                        ddlDoiTac.DataSource = listDoiTac;
                                        ddlDoiTac.DataBind();
                                    }
                                    break;
                            }
                            break;
                    }
                    // 6. Phòng CSKH
                    switch (adminInfo.PhongBanId)
                    {
                        case 60:
                            // xem tất cả
                            listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
                            if (listDoiTac.Count > 0)
                            {
                                ddlDoiTac.DataSource = listDoiTac;
                                ddlDoiTac.DataBind();
                            }
                            break;
                    }

                    //listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND DonViTrucThuoc=" + doiTacId, "TenDoiTac ASC");
                    //if (listDoiTac.Count > 0)
                    //{
                    //    ddlDoiTac.DataSource = listDoiTac;
                    //    ddlDoiTac.DataBind();

                    //    ListItem item = new ListItem("VNPT khu vực", doiTacId.ToString());
                    //    ddlDoiTac.Items.Insert(0, item);
                    //}
                    //else
                    //{
                    //    listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DoiTacType=4 AND id=" + doiTacId, "TenDoiTac ASC");
                    //    ddlDoiTac.DataSource = listDoiTac;
                    //    ddlDoiTac.DataBind();
                    //}
                }
            }
        }
        protected AdminInfo userInfo;
        /// <summary>
        /// Authod: Vu Van Truong
        /// Created Date: 25/04/2016
        /// To do: lay danh sach nguoi dung thuoc nhom
        /// </summary>
        private void LoadComboDanhSachNguoiDungThuocNhom()
        {
            // 1: lấy phòng ban người dùng đăng nhập thuộc
            userInfo = LoginAdmin.AdminLogin();
            int phongbanid = userInfo.PhongBanId;

            // 2: lấy thông tin nhóm người dùng thuộc phòng ban đó
            int nhomnguoidungid = userInfo.NhomNguoiDung;

            // 3: lấy danh sách các user thuộc phòng ban đó (tùy theo quyền)

            string selectClause = "a.* ";
            string joinClause = "LEFT JOIN dbo.PhongBan_User b on a.id=b.NguoiSuDungId LEFT JOIN dbo.NguoiSuDung_Group c ON a.NhomNguoiDung=c.Id";
            
            string whereClause = "b.PhongBanId="+phongbanid;
            string orderbyClause = "";
            var listDsNguoiDungPhongBan = ServiceFactory.GetInstanceNguoiSuDung().GetListDynamicJoin(selectClause, joinClause, whereClause, orderbyClause);
            if (listDsNguoiDungPhongBan.Count>0)
            {
                ddlDSNguoiDungThuocNhom.DataSource = listDsNguoiDungPhongBan;
                ddlDSNguoiDungThuocNhom.DataBind();
            }
        }
        #endregion
    }
}