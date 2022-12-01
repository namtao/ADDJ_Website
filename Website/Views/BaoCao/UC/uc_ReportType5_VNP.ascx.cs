using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.BaoCao.UC
{
    public partial class uc_ReportType5_VNP : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }
        public bool isNotShowRegion { get; set; }

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
                    case 10100:
                        this.DoiTacId = DoiTacInfo.DoiTacIdValue.VNP;
                        break;
                }

                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();
                if (!isNotShowRegion)
                {
                    LoadKhuVuc(this.DoiTacId);
                    //LoadDoiTacVNPTTT();
                    ddlKhuVuc_ReportType1.SelectedValue = this.DoiTacId.ToString();
                }
                else
                {
                    pnRegion.Visible = false;
                }

                ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                ddlNguonKhieuNai.DataTextField = "Name";
                ddlNguonKhieuNai.DataValueField = "Id";
                ddlNguonKhieuNai.DataBind();

                rowNguonKhieuNai.Visible = LoginAdmin.AdminLogin().PhongBanId == PhongBanInfo.PhongBanValueId.PHONG_CSKH_VNP;
            }

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/10/2013
        /// Todo : Hiển thị báo cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_ReportType1_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;
            string khuVuc = ddlKhuVuc_ReportType1.SelectedItem != null ? ddlKhuVuc_ReportType1.SelectedItem.Text : string.Empty;
            string donViId = "-1"; //ddlPhongBan_ReportType1.Visible ? ddlPhongBan_ReportType1.SelectedValue : "-1";
            string donVi = string.Empty;// ddlPhongBan_ReportType1.SelectedItem != null ? ddlPhongBan_ReportType1.SelectedItem.Text : string.Empty;
            string doiTacId = "-1";// ddlDoiTac_ReportType1.SelectedValue;
            string tenDoiTac = string.Empty;// ddlDoiTac_ReportType1.SelectedItem != null ? ddlDoiTac_ReportType1.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;

            string errorMessage = string.Empty;
            string script = string.Empty;

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

            string page = string.Empty;

            if (lblReportType.Text == "bc_VNP_BaoCaoTongHop" || lblReportType.Text == "bc_CSKHKV_BaoCaoTongHop")
            {
                title = "Báo cáo tổng hợp số liệu giải quyết khiếu nại";
                page = "baocaotonghopphongcskhvnp.aspx?";
            }
            else if (lblReportType.Text == "bc_VNP_BaoCaoTongHopGiamTruTheoCP")
            {
                title = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                page = "baocaotonghopgiamtrutheocpvnp.aspx?";
            }
            else if (lblReportType.Text == "bc_VNP_KhieuNaiToanMang")
            {
                title = "Báo cáo tổng hợp khiếu nại toàn mạng";
                page = "baocaotonghopkhieunaitoanmang.aspx?";
            }
            else if (lblReportType.Text == "bc_VNP_BaoCaoChatLuongMang")
            {
                title = "Báo cáo tổng hợp chất lượng mạng";
                page = "baocaotonghopchatluongmang.aspx?";
            }
            //else if(lblReportType.Text == "bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi")
            //{
            //    title = "Báo cáo thống kê theo nguyên nhân lỗi";
            //    page = "baocaothongketheonguyennhanloi.aspx?";               
            //}


            if (page == string.Empty)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại báo cáo", errorMessage);
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}&nguonKhieuNai={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, khuVucId, donViId, fromDate, toDate, loaibc, nguonKhieuNai);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 09/12/2013
        /// Todo : Trường hợp nếu là VNPT  khu vực thì hiển thị các VNPTTT
        ///         Trường còn lại thì hiển thị các phòng ban thuộc trung tâm được chọn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlKhuVuc_ReportType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP1.ToString() || ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP2.ToString() || ddlKhuVuc_ReportType1.SelectedValue == DoiTacInfo.DoiTacIdValue.VNP3.ToString())
            {
                //LoadDoiTacVNPTTT();
                //ddlDoiTac_ReportType1.Visible = true;
                //ddlPhongBan_ReportType1.Visible = false;
                //lblPhongBan_DoiTac_ReportType1.Text = "Đối tác";
            }
            else
            {
                //ddlPhongBan_ReportType1.Items.Clear();
                //int doiTacId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType1.SelectedValue);
                //List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);

                //if (listPhongBan == null)
                //{
                //    listPhongBan = new List<PhongBanInfo>();
                //}

                //PhongBanInfo objPhongBan = new PhongBanInfo();
                //objPhongBan.Id = -1;
                //objPhongBan.Name = "Chọn phòng ban..";
                //listPhongBan.Insert(0, objPhongBan);
                //ddlPhongBan_ReportType1.DataSource = listPhongBan;
                //ddlPhongBan_ReportType1.DataBind();

                //if (!ddlPhongBan_ReportType1.Visible)
                //{
                //    ddlDoiTac_ReportType1.Visible = false;
                //    ddlPhongBan_ReportType1.Visible = true;
                //    lblPhongBan_DoiTac_ReportType1.Text = "Phòng";
                //}
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadKhuVuc(int doiTacId)
        {
            ddlKhuVuc_ReportType1.Items.Clear();

            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "", "TenDoiTac ASC");
            List<DoiTacInfo> listDoiTacRoot = null;
            string space = string.Empty;

            if (listDoiTac != null)
            {
                if (doiTacId == DoiTacInfo.DoiTacIdValue.VNP)
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate (DoiTacInfo obj) { return obj.DonViTrucThuoc == 0; });
                }
                else
                {
                    listDoiTacRoot = listDoiTac.FindAll(delegate (DoiTacInfo obj) { return obj.Id == doiTacId; });
                }

                for (int i = 0; i < listDoiTacRoot.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", space, listDoiTacRoot[i].TenDoiTac);
                    item.Value = listDoiTacRoot[i].Id.ToString();
                    ddlKhuVuc_ReportType1.Items.Add(item);

                    int donViTrucThuocChild = listDoiTacRoot[i].Id;
                    listDoiTacRoot.RemoveAt(i);
                    i--;

                    LoadChildDoiTac(space, donViTrucThuocChild, listDoiTac);
                }
            }
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
                //if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                if (listDoiTacInfo[i].DonViTrucThuocChoBaoCao == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlKhuVuc_ReportType1.Items.Add(item);

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

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 21/10/2013
        ///// Todo : 
        ///// </summary>
        //private void LoadDoiTacVNPTTT()
        //{
        //    string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;

        //    string whereDonViTrucThuoc = string.Empty;
        //    if (khuVucId != "-1")
        //    {
        //        //whereDonViTrucThuoc = " DonViTrucThuoc=" + khuVucId;
        //        whereDonViTrucThuoc = " DonViTrucThuocChoBaoCao=" + khuVucId;
        //    }
        //    List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereDonViTrucThuoc, "TenDoiTac ASC");
        //    if (listDoiTac == null)
        //    {
        //        listDoiTac = new List<DoiTacInfo>();
        //    }

        //    DoiTacInfo objDoiTac = new DoiTacInfo();
        //    objDoiTac.Id = -1;
        //    objDoiTac.TenDoiTac = "Chọn VNPT TT..";
        //    listDoiTac.Insert(0, objDoiTac);

        //    ddlDoiTac_ReportType1.DataSource = listDoiTac;
        //    ddlDoiTac_ReportType1.DataBind();
        //}

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 07/12/2013
        ///// </summary>
        ///// <param name="doiTacId"></param>
        //private void LoadComboPhongBan(int doiTacId)
        //{
        //    List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);
        //    if (listPhongBan != null)
        //    {
        //        PhongBanInfo objPhongBan = new PhongBanInfo();
        //        objPhongBan.Id = -1;
        //        objPhongBan.Name = "Chọn phòng ban..";
        //        listPhongBan.Insert(0, objPhongBan);
        //        ddlPhongBan_ReportType1.DataSource = listPhongBan;
        //        ddlPhongBan_ReportType1.DataBind();
        //    }
        //}      

        #endregion
    }
}