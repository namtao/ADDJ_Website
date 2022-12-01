using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Core;
using Website.AppCode.Controller;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;
using Website.Views.BaoCao.UC;
using AIVietNam.Admin;

namespace Website.Views.BaoCao
{
    public partial class BaoCaoThongKe : AppCode.PageBase
    {
        private string LastLoadedControl
        {
            get
            {
                return ViewState["LastLoaded"] as string;
            }
            set
            {
                ViewState["LastLoaded"] = value;
            }
        }

        private int DoiTacId
        {
            get { return ConvertUtility.ToInt32(ViewState["DoiTacId"]); }
            set { ViewState["DoiTacId"] = value; }

        }

        private int PhongBanXuLyId
        {
            get { return ConvertUtility.ToInt32(ViewState["PhongBanXuLyId"]); }
            set { ViewState["PhongBanXuLyId"] = value; }
        }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserRead)
            {
                Response.Redirect(Config.PathNotRight, false);
                return;
            }

            if (!IsPostBack)
            {
                AdminInfo adminInfo = LoginAdmin.AdminLogin();
                if (adminInfo != null)
                {
                    this.DoiTacId = adminInfo.DoiTacId;
                    this.PhongBanXuLyId = adminInfo.PhongBanId;

                    switch (adminInfo.DoiTacId)
                    {
                        case 1: // VNP
                            listReportVNP.Visible = true;
                            break;
                        case 2: // VNP 1                           
                        case 3: // VNP 2                           
                        case 5: // VNP 3
                            if (adminInfo.PhongBanId == 58 || adminInfo.PhongBanId == 72 || adminInfo.PhongBanId == 73)
                            {
                                listReportCSKHKhuVuc.Visible = true;
                            }

                            break;
                        case 7: // ĐKT 1
                        case 14: // ĐKT 2
                        case 19: // ĐKT 3                   
                            switch (adminInfo.PhongBanId)
                            {
                                case 53: // Tổ GQKN
                                case 62:
                                case 67:
                                    listReportGQKN.Visible = true;
                                    break;
                                case 54: // Tổ XLNV
                                case 63:
                                case 68:
                                    listReportXLNV.Visible = true;
                                    break;
                                case 55: // Tổ KS
                                case 64:
                                case 69:
                                    listReportKS.Visible = true;
                                    break;
                                case 56: // Tổ OB
                                case 65:
                                case 70:
                                    listReportOB.Visible = true;
                                    break;
                                case 57: // Tổ KTV
                                case 66:
                                case 71:
                                    listReportKTV.Visible = true;
                                    break;
                            }
                            break;
                        case 10100: // TTTC
                            listReportTTTC.Visible = true;

                            // Trường hợp là phòng lãnh đạo thì mới hiển thị báo cáo của cả trung tâm
                            lbReportTTTC_TongHopPAKN.Visible = adminInfo.PhongBanId == 101;

                            // Đối với báo cáo phòng ban thì chỉ hiển thị đối với user của phòng lãnh đạo hoặc user là trưởng phòng
                            lbReportTTTC_TongHopPAKNTheoPhongBan.Visible = adminInfo.PhongBanId == 101 || BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng);

                            break;
                        case 10101: // TT VAS
                            listReportVAS.Visible = true;
                            break;
                        case 10111: // Trung tâm ĐHTT

                            break;
                        //case 10034: // VNPT KV 1
                        //case 10062: // VNPT KV 3
                        //case 10076: // VNPT KV 2

                        //    break;
                        case 10113: // Trung tâm Quy hoạch PTM

                            break;

                        case 10194:
                            listReportVNPTNET.Visible = true;
                            break;

                        case 10195: // HTM MB
                        case 10200: // HTM MT
                        case 10201: // HTM MN
                            listReportHaTangMang.Visible = true;
                            break;

                        case 10196: // DHTT MB
                        case 10202: // DHTT MB
                        case 10203: // DHTT MB
                            listReportDHTT.Visible = true;
                            break;

                        case 10198:
                            listReportNETCNTT.Visible = true;
                            break;

                        default:

                            // Kiểm tra xem có phải thuộc VNPT tỉnh thành không ?
                            DoiTacInfo objDoiTac = ServiceFactory.GetInstanceDoiTac().GetInfo(adminInfo.DoiTacId);
                            if (objDoiTac != null && adminInfo.NhomNguoiDung == (int)NguoiSuDung_NhomNguoiDung.Đối_tác)
                            {
                                if (objDoiTac.DoiTacType == (byte)DoiTacInfo.DoiTacTypeValue.VNPTTT)
                                {
                                    listReportVNPTTT.Visible = true;
                                }
                                else if (objDoiTac.DoiTacType == (byte)DoiTacInfo.DoiTacTypeValue.TDD_VNP)
                                {
                                    listReportTruongDaiDien.Visible = true;
                                }
                                else if (objDoiTac.DoiTacType == (byte)DoiTacInfo.DoiTacTypeValue.DOI_TAC_VNP)
                                {
                                    listReportDoiTac.Visible = true;
                                }
                            }
                            break;
                    } // end switch(adminInfo.DoiTacId)

                    // Phân quyền lấy báo cáo theo VB 601 theo đơn vị
                    // 1. Đối tác thuê ngoài VNP

                    switch (adminInfo.DoiTacId)
                    {
                        case 13:
                        case 10205:
                        case 10192:
                        case 20:
                        case 10206:
                        case 10204:
                        case 10207:
                        case 18:
                        case 23:
                            // kiểm tra xem user có là trưởng ca hay không
                            var checkTruongCa = ServiceFactory.GetInstancePhongBan().GetListDynamic("", "Name LIKE N'%TC' AND DoiTacId=" + adminInfo.DoiTacId, "");
                            if (checkTruongCa.Count > 0)
                            {
                                listReportGiamTruDVGTGT.Visible = true;
                            }
                            break;
                    }

                    // 2. TTKD (63 Tỉnh thành theo mã tỉnh)
                    // kiểm tra xem có thuộc phòng phụ trách không
                    var checkUserthuoctttt = ServiceFactory.GetInstanceDoiTac().GetListDynamicJoin("a.*", "INNER JOIN dbo.Province b on a.MaDoiTac=b.AbbRev", "b.ParentId IS NULL and a.Id=" + adminInfo.DoiTacId, "");
                    if (checkUserthuoctttt.Count > 0)
                    {
                        // 1 user không thuộc phòng phụ trách hoặc nó không thuộc phòng nào đều thỏa mãn
                        var checkUserPhuTrach = ServiceFactory.GetInstancePhongBan().GetListDynamicJoin("", "INNER JOIN	dbo.PhongBan_User b ON a.id=b.PhongBanId ", "Name LIKE N'%PT' AND b.NguoiSuDungId =" + adminInfo.Id, "");
                        if (checkUserPhuTrach != null)
                        {
                            listReportGiamTruDVGTGT.Visible = true;
                        }
                    }


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
                                    listReportGiamTruDVGTGT.Visible = true;
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
                                    listReportGiamTruDVGTGT.Visible = true;
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
                                    listReportGiamTruDVGTGT.Visible = true;
                                    break;
                            }
                            break;
                    }
                    // 6. Phòng CSKH
                    switch (adminInfo.PhongBanId)
                    {
                        case 60:
                            listReportGiamTruDVGTGT.Visible = true;
                            break;
                    }

                } // end if(adminInfo != null)
            }

            LoadUserControl();
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của VNP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportVNP_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportVNPCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của TTTC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportTTTC_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportTTTCCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của VAS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportVAS_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportVASCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/01/2014
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của Phòng CSKH khu vực
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportCSKHKhuVuc_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportCSKHKhuVucCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/12/2013
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của VNP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReportVNPTTT_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportVNPTTTCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        protected void lbReportTDD_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportTruongDaiDienCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/12/2014
        /// Todo : Hiển thị các báo cáo dùng chung : tổng hợp đơn vị, tổng hợp phòng ban, tổng hợp người dùng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Common_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportCommonCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }


        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/12/2014
        /// Todo : Hiển thị các báo cáo dùng chung : tổng hợp đơn vị, tổng hợp phòng ban, tổng hợp người dùng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_DoiTac_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReporDoiTacCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        protected void lbReportVNPTNET_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportVNPTNETCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }

        #endregion

        #region Private methods

        private void LoadUserControl()
        {
            string controlPath = LastLoadedControl;

            if (!string.IsNullOrEmpty(controlPath))
            {
                pnContainer.Controls.Clear();
                UserControl uc = (UserControl)LoadControl(controlPath);
                uc.ID = controlPath.Replace("/", "_").Replace(".ascx", "");
                pnContainer.Controls.Add(uc);
            }

            //ShowReportUserControl(controlPath);
        }

        private void ShowHideReportVNPCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string tvLoaiKhieuNaiId = string.Empty;
            //string CUOC_TB_TRA_TRUOC = "30";
            //string CUOC_TB_TRA_SAU = "35";
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_VNP_BaoCaoTongHop":
                    pathUc = "UC/uc_ReportType5_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType5_VNP)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại";
                    ((uc_ReportType5_VNP)uc).ReportType = reportType;
                    ((uc_ReportType5_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType5_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType5_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_KhieuNaiToanMang":
                    pathUc = "UC/ucReportType_2DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_2DateRange)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại toàn mạng";
                    ((ucReportType_2DateRange)uc).ReportType = reportType;
                    ((ucReportType_2DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_2DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_2DateRange)uc).IsFirstLoad = true;
                    ((ucReportType_2DateRange)uc).isNotShowRegion = false;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_KhieuNaiToanMangTheoTuan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại toàn mạng theo tuần";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_KhieuNaiToanMangTheoThang":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại toàn mạng theo tháng";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_ThongKeKhieuNaiTheoNguyenNhanLoi":
                    pathUc = "UC/uc_ReportType11_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType11_VNP)uc).ReportTitle = "Báo cáo thống kê khiếu nại theo nguyên nhân lỗi";
                    ((uc_ReportType11_VNP)uc).ReportType = reportType;
                    ((uc_ReportType11_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType11_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType11_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoChatLuongPhucVu":
                    pathUc = "UC/uc_ReportType12_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType12_VNP)uc).ReportTitle = "Báo cáo chất lượng phục vụ";
                    ((uc_ReportType12_VNP)uc).ReportType = reportType;
                    ((uc_ReportType12_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType12_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType12_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoChatLuongMang":
                    pathUc = "UC/uc_ReportType5_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType5_VNP)uc).ReportTitle = "Báo cáo tổng hợp chất lượng mạng";
                    ((uc_ReportType5_VNP)uc).ReportType = reportType;
                    ((uc_ReportType5_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType5_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType5_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoTonDongQuaHan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tồn đọng quá hạn";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoDVGTGTTapDoan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo dịch vụ giá trị gia tăng";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                // Dương DV - Báo cáo mới theo Lĩnh vực chung, Lĩnh vực con mới
                case "bc_VNP_BaoCaoDVGTGTTapDoan_New":
                    pathUc = "UC/ucReportType_DateRange_New.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange_New)uc).ReportTitle = "Báo cáo dịch vụ GTGT (cho tập đoàn - từ: 01/05/2016)";
                    ((ucReportType_DateRange_New)uc).ReportType = reportType;
                    ((ucReportType_DateRange_New)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange_New)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange_New)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                // End Dương DV
                case "bc_VNP_BaoCaoTongHopGiamTruTheoCP":
                    pathUc = "UC/uc_ReportType5_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType5_VNP)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    ((uc_ReportType5_VNP)uc).ReportType = reportType;
                    ((uc_ReportType5_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType5_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType5_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", string.Empty);
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_DoiSoatCSKHPTDV":
                    pathUc = "UC/uc_ReportType6_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType6_VNP)uc).ReportTitle = "Báo cáo đối soát doanh thu giữa CSKH và PTDV";
                    ((uc_ReportType6_VNP)uc).ReportType = reportType;
                    ((uc_ReportType6_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType6_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType6_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoTongHopGiamTru":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ toàn mạng";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc11":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo chi tiết giảm trừ cước dịch vụ trả trước";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc21":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    break;

                // Báo cáo DuongDv
                case "bc31":
                    pathUc = "UC/ucReportType2.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại tổ XLNV";
                    ((ucReportType2)uc).ReportType = reportType;
                    ((ucReportType2)uc).DoiTacId = doiTacId;
                    ((ucReportType2)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    //tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc41":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo chi tiết PPS";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết PPS";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc51":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo chi tiết POST";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết POST";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc61":
                    pathUc = "UC/ucReportType2.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2)uc).ReportTitle = "Báo cáo theo mẫu số 5";
                    ((ucReportType2)uc).ReportType = reportType;
                    ((ucReportType2)uc).DoiTacId = doiTacId;
                    ((ucReportType2)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo theo loại khiếu nại";
                    //tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc71":
                    //lblTittle.Text = "Báo cáo chi tiết khiếu nại";
                    break;
                case "bc_GQKN_ChiTietGiamTruTraSauGQKN":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau tại tổ GQKN";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);                    
                    break;
                case "bc_GQKN_ChiTietGiamTruTraSauVNPTTT":
                    pathUc = "UC/ucReportType1.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1)uc).ReportTitle = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau của VNPT tỉnh thành";
                    ((ucReportType1)uc).ReportType = reportType;
                    ((ucReportType1)uc).DoiTacId = doiTacId;
                    ((ucReportType1)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_SAU;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);                    
                    break;
                case "bc_XLNV_BaoCaoKhoiLuongCongViec":
                    pathUc = "UC/uc_ReportType4_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType4_VNP)uc).ReportTitle = "Báo cáo khối lượng công việc";
                    ((uc_ReportType4_VNP)uc).ReportType = reportType;
                    ((uc_ReportType4_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType4_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType4_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo khối lượng công việc";
                    break;
                case "bcBieuDo_TheoLinhVucCon":
                    pathUc = "UC/ucReportType3_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNP)uc).ReportTitle = "Báo cáo biểu đồ số lượng khiếu nại đã tạo";
                    ((ucReportType3_VNP)uc).ReportType = reportType;
                    ((ucReportType3_VNP)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType3_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo khối lượng công việc";
                    break;
                case "bc_KS_BaoCaoTongHopKN":
                    pathUc = "UC/ucReportType2.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2)uc).ReportTitle = "Báo cáo tổng hợp theo loại khiếu nại";
                    ((ucReportType2)uc).ReportType = reportType;
                    ((ucReportType2)uc).DoiTacId = doiTacId;
                    ((ucReportType2)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_OB_BaoCaoTongHopKN":
                    pathUc = "UC/ucReportType2.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2)uc).ReportTitle = "Báo cáo tổng hợp theo loại khiếu nại";
                    ((ucReportType2)uc).ReportType = reportType;
                    ((ucReportType2)uc).DoiTacId = doiTacId;
                    ((ucReportType2)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_TTKTV_BaoCaoSoLuongKNPhanHoiVeKTV":
                    pathUc = "UC/ucBaoCaoToTruongToKTV.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucBaoCaoToTruongToKTV)uc).ReportTitle = "Báo cáo số lượng KN bị phản hồi về của KTV";
                    ((ucBaoCaoToTruongToKTV)uc).ReportType = reportType;
                    //((ucBaoCaoToTruongToKTV)uc).DoiTacId = doiTacId;
                    ((ucBaoCaoToTruongToKTV)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucBaoCaoToTruongToKTV)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_TTKTV_BaoCaoSoLuongKNQuaHanCuaKTV":
                    pathUc = "UC/ucBaoCaoToTruongToKTV.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucBaoCaoToTruongToKTV)uc).ReportTitle = "Báo cáo khiếu nại quá hạn của KTV";
                    ((ucBaoCaoToTruongToKTV)uc).ReportType = reportType;
                    //((ucBaoCaoToTruongToKTV)uc).DoiTacId = doiTacId;
                    ((ucBaoCaoToTruongToKTV)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucBaoCaoToTruongToKTV)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_VNP_ThongKeSoLuongGQKNTheo1KhoangThoiGian":
                    pathUc = "UC/ucReportType31_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType31_VNP)uc).ReportTitle = "Thống kê số lượng GQKN theo 1 khoảng thời gian";
                    ((ucReportType31_VNP)uc).ReportType = reportType;
                    //((ucBaoCaoToTruongToKTV)uc).DoiTacId = doiTacId;
                    ((ucReportType31_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType31_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_VNP_ThongKeSoLuongGQKNTheo2KhoangThoiGian":
                    pathUc = "UC/ucReportType32_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType32_VNP)uc).ReportTitle = "Thống kê số lượng GQKN theo 2 khoảng thời gian";
                    ((ucReportType32_VNP)uc).ReportType = reportType;
                    //((ucBaoCaoToTruongToKTV)uc).DoiTacId = doiTacId;
                    ((ucReportType32_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType32_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_VNP_ThongKeTongSoTienGiamTruTheo1KhoangThoiGian":
                    pathUc = "UC/ucReportType31_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType31_VNP)uc).ReportTitle = "Thống kế tổng số tiền giảm trừ theo 1 khoảng thời gian";
                    ((ucReportType31_VNP)uc).ReportType = reportType;
                    //((ucReportType31_VNP)uc).DoiTacId = doiTacId;
                    ((ucReportType31_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType31_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_VNP_ThongKeTongSoTienGiamTruTheo2KhoangThoiGian":
                    pathUc = "UC/ucReportType32_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType32_VNP)uc).ReportTitle = "Thống kê tổng số tiền giảm trừ theo 2 khoảng thời gian";
                    ((ucReportType32_VNP)uc).ReportType = reportType;
                    //((ucReportType32_VNP)uc).DoiTacId = doiTacId;
                    ((ucReportType32_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType32_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;
                case "bc_XLNV_BaoCaoTonDongQuaHanCuaDoiTac":
                    pathUc = "UC/uc_ReportType7_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType7_VNP)uc).ReportTitle = "Báo cáo số lượng PAKN tồn đọng và quá hạn của các đối tác";
                    ((uc_ReportType7_VNP)uc).ReportType = reportType;
                    ((uc_ReportType7_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType7_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType7_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    break;

                case "bc_VNP_BaoCaoTongHopToGQKN":
                case "bc_VNP_BaoCaoGiamTruToGQKN":
                    pathUc = "UC/uc_ReportType8_GQKN_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType8_GQKN_VNP)uc).ReportTitle = "";
                    ((uc_ReportType8_GQKN_VNP)uc).ReportType = reportType;
                    ((uc_ReportType8_GQKN_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType8_GQKN_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType8_GQKN_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_VNP_DanhSachQuaHan":
                    pathUc = "UC/uc_ReportType9_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType9_VNP)uc).ReportTitle = "Danh sách khiếu nại quá hạn";
                    ((uc_ReportType9_VNP)uc).ReportType = reportType;
                    ((uc_ReportType9_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType9_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType9_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_BaoCaoKhieuNaiChuyenBoPhanKhac":
                    pathUc = "UC/uc_ReportType10_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType10_VNP)uc).ReportTitle = "Danh sách khiếu nại chuyển phòng ban khác";
                    ((uc_ReportType10_VNP)uc).ReportType = reportType;
                    ((uc_ReportType10_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType10_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType10_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //string script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/danhsachkhieunai.aspx?fromPage=xlnv_danhsachkhieunaichuyenbophankhac&phongBanId={0}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", phongBanXuLyId);
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
                    break;

                case "bc_VNP_BaoCaoTongHopLoaiKhieuNaiToGQKN":
                    pathUc = "UC/ucReportType2.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại tổ GQKN";
                    ((ucReportType2)uc).ReportType = reportType;
                    ((ucReportType2)uc).DoiTacId = doiTacId;
                    ((ucReportType2)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                #region Bao Cao Tong Hop Giam Tru DV GTGT
                case "bc_VNP_BaoCaoTongHopGiamTruDVGTGTDonVi":
                    pathUc = "UC/ucReportType15_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType15_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ DV GTGT theo đơn vị";
                    ((ucReportType15_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType15_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType15_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType15_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_VNP_BaoCaoTongHopGiamTruDVGTGTDichVu":
                    pathUc = "UC/ucReportType16_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType16_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ DV GTGT theo dịch vụ";
                    ((ucReportType16_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType16_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType16_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                #endregion

                case "bc_VNP_BaoCaoTongHopGiamTruVNPTTT":
                    pathUc = "UC/ucReportType6_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType6_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ";
                    ((ucReportType6_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType6_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType6_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType6_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_VNP_DanhSachGiamTruKhieuNai":
                    pathUc = "UC/uc_ReportType13_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType13_VNP)uc).ReportTitle = "Danh sách khiếu nại giảm trừ";
                    ((uc_ReportType13_VNP)uc).ReportType = reportType;
                    ((uc_ReportType13_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType13_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType13_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                //23/04/16
                case "bc_VNP_BaoCaoChiTietGiamTruDVGTGT":
                    pathUc = "UC/uc_ReportType14_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType14_VNP)uc).ReportTitle = "Báo cáo chi tiết giảm trừ DV GTGT";
                    ((uc_ReportType14_VNP)uc).ReportType = reportType;
                    ((uc_ReportType14_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType14_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType14_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_VNP_BaoCaoTongHopTheoChiTieuThoiGianGiaiQuyet":
                    pathUc = "UC/uc_ReportType15_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType15_VNP)uc).ReportTitle = "Báo cáo tổng hợp theo tiêu chí thời gian giải quyết";
                    ((uc_ReportType15_VNP)uc).ReportType = reportType;
                    ((uc_ReportType15_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType15_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType15_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNP_BaoCaoChiTietTheoChiTieuThoiGianGiaiQuyet":
                    pathUc = "UC/uc_ReportType16_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType16_VNP)uc).ReportTitle = "Báo cáo chi tiết theo chỉ tiêu thời gian giải quyết";
                    ((uc_ReportType16_VNP)uc).ReportType = reportType;
                    ((uc_ReportType16_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType16_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType16_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:
                    //lblTittle.Text = string.Empty;
                    break;
            } // end switch     

            LastLoadedControl = pathUc;

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);           
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/12/2013
        /// Todo : Hiển thị các danh sách báo cáo của TTTC
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        private void ShowHideReportTTTCCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string tvLoaiKhieuNaiId = string.Empty;
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_TTTC_TongHopPAKN":
                    pathUc = "UC/ucReportType1_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_TTTC)uc).ReportTitle = "Báo cáo tổng hợp PAKN";
                    ((ucReportType1_TTTC)uc).ReportType = reportType;
                    ((ucReportType1_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType1_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả trước";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc_TTTC_TongHopPAKNTheoPhongBan":
                    pathUc = "UC/ucReportType1_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_TTTC)uc).ReportTitle = "Báo cáo tổng hợp PAKN theo phòng ban";
                    ((ucReportType1_TTTC)uc).ReportType = reportType;
                    ((ucReportType1_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType1_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    break;
                case "bc_TTTC_TongHopPAKNTheoNguoiDung":
                    pathUc = "UC/ucReportType1_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_TTTC)uc).ReportTitle = "Báo cáo tổng hợp PAKN theo người dùng";
                    ((ucReportType1_TTTC)uc).ReportType = reportType;
                    ((ucReportType1_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType1_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    //tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc_TTTC_ChiTietPAKNTheoNguoiDung":
                    pathUc = "UC/ucReportType2_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_TTTC)uc).ReportTitle = "Báo cáo chi tiết PAKN theo người dùng";
                    ((ucReportType2_TTTC)uc).ReportType = reportType;
                    ((ucReportType2_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType2_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết PPS";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc_TTTC_BaoCaoPhoiHopGQKN":
                    pathUc = "UC/ucReportType1_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_TTTC)uc).ReportTitle = "Báo cáo phối hợp giải quyết khiếu nại";
                    ((ucReportType1_TTTC)uc).ReportType = reportType;
                    ((ucReportType1_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType1_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_TTTC_BaoCaoQuaHanPhongBan":
                    pathUc = "UC/ucReportType1_TTTC.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_TTTC)uc).ReportTitle = "Báo cáo tồn đọng và quá hạn phòng ban";
                    ((ucReportType1_TTTC)uc).ReportType = reportType;
                    ((ucReportType1_TTTC)uc).DoiTacId = doiTacId;
                    ((ucReportType1_TTTC)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_TTTC)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_TTTC_DanhSachQuaHan":
                    pathUc = "UC/uc_ReportType9_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType9_VNP)uc).ReportTitle = "Danh sách khiếu nại quá hạn";
                    ((uc_ReportType9_VNP)uc).ReportType = reportType;
                    ((uc_ReportType9_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType9_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType9_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:
                    //lblTittle.Text = string.Empty;
                    break;
            } // end switch     

            LastLoadedControl = pathUc;

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);           
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/12/2013
        /// Todo : Hiển thị các danh sách báo cáo của VAS
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        private void ShowHideReportVASCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string tvLoaiKhieuNaiId = string.Empty;
            //string CUOC_TB_TRA_TRUOC = "30";
            //string CUOC_TB_TRA_SAU = "35";
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_VAS_SoLieuPAKNDaXuLy":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo số liệu PAKN đã xử lý";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết giảm trừ cước dịch vụ trả trước";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc_VAS_TongHopSoLieuPAKNDangTonDong":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo tổng hợp số liệu PAKN đang tồn đọng";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    break;
                case "bc_VAS_TongHopSoLieuPAKNDaTiepNhan":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo tổng hợp số liệu PAKN đã tiếp nhận";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTitleReportType2.Text = "Báo cáo tổng hợp theo loại khiếu nại";
                    //tvLoaiKhieuNaiId = tvLoaiKhieuNai_ReportType2.ClientID;
                    break;
                case "bc_VAS_ChiTietPAKNDaTiepNhan":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo chi tiết PAKN đã tiếp nhận";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    //lblTittle.Text = "Báo cáo chi tiết PPS";
                    //ddlLoaiKhieuNai.SelectedValue = CUOC_TB_TRA_TRUOC;
                    //ddlLoaiKhieuNai_SelectedIndexChanged(null, null);
                    break;
                case "bc_VAS_DanhSachQuaHan":
                    pathUc = "UC/uc_ReportType9_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType9_VNP)uc).ReportTitle = "Danh sách khiếu nại quá hạn";
                    ((uc_ReportType9_VNP)uc).ReportType = reportType;
                    ((uc_ReportType9_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType9_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType9_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VAS_SoLieuPAKNDichVuToanMang":
                    pathUc = "UC/ucReportType2_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_VAS)uc).ReportTitle = "Báo cáo số liệu PAKN dịch vụ toàn mạng";
                    ((ucReportType2_VAS)uc).ReportType = reportType;
                    ((ucReportType2_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType2_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VAS_TongHopPAKNVAS":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo tổng hợp trung tâm phát triển dịch vụ";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VAS_TongHopPAKNTheoNguoiDung":
                    pathUc = "UC/ucReportType1_VAS.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VAS)uc).ReportTitle = "Báo cáo tổng hợp PAKN người dùng";
                    ((ucReportType1_VAS)uc).ReportType = reportType;
                    ((ucReportType1_VAS)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VAS)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VAS)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:
                    //lblTittle.Text = string.Empty;
                    break;
            } // end switch     

            LastLoadedControl = pathUc;

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);           
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/01/2014
        /// Todo : Hiển thị các vùng chọn điều kiện báo cáo của VNPT TT
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        private void ShowHideReportCSKHKhuVucCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                //case "bc_VNPTTT_ChiTietGiamTruTraSauVNPTTT":
                //    pathUc = "UC/ucReportType1_VNPTTT.ascx";
                //    uc = this.LoadControl(pathUc);
                //    ((ucReportType1_VNPTTT)uc).ReportTitle = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau của VNPT tỉnh thành";
                //    ((ucReportType1_VNPTTT)uc).ReportType = reportType;
                //    ((ucReportType1_VNPTTT)uc).DoiTacId = doiTacId;
                //    //((ucReportType1_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                //    ((ucReportType1_VNPTTT)uc).IsFirstLoad = true;
                //    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                //    pnContainer.Controls.Add(uc);
                //    break;
                //case "bc_VNPTTT_SoLuongKNGDVTiepNhanXuLy":
                //    pathUc = "UC/ucReportType2_VNPTTT.ascx";
                //    uc = this.LoadControl(pathUc);
                //    ((ucReportType2_VNPTTT)uc).ReportTitle = "Báo cáo số lượng khiếu nại tiếp nhận và xử lý";
                //    ((ucReportType2_VNPTTT)uc).ReportType = reportType;
                //    ((ucReportType2_VNPTTT)uc).DoiTacId = doiTacId;
                //    ((ucReportType2_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                //    ((ucReportType2_VNPTTT)uc).IsFirstLoad = true;
                //    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                //    pnContainer.Controls.Add(uc);
                //    break;
                case "bc_CSKHKV_GiamTruKhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo giảm trừ do khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_KhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tình hình khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_ThucTrangGQKN":
                    pathUc = "UC/ucReportType4_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType4_VNPTTT)uc).ReportTitle = "Báo cáo thực trạng GQKN";
                    ((ucReportType4_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType4_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType4_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_BaoCaoTongHop":
                    pathUc = "UC/uc_ReportType5_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType5_VNP)uc).ReportTitle = "Báo cáo tổng hợp khiếu nại";
                    ((uc_ReportType5_VNP)uc).ReportType = reportType;
                    ((uc_ReportType5_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType5_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType5_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_BaoCaoTongHopGiamTruVNPTTT":
                    pathUc = "UC/ucReportType6_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType6_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ";
                    ((ucReportType6_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType6_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType6_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType6_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_DanhSachQuaHan":
                    pathUc = "UC/uc_ReportType9_VNP.ascx";
                    uc = this.LoadControl(pathUc);
                    ((uc_ReportType9_VNP)uc).ReportTitle = "Danh sách khiếu nại quá hạn";
                    ((uc_ReportType9_VNP)uc).ReportType = reportType;
                    ((uc_ReportType9_VNP)uc).DoiTacId = doiTacId;
                    ((uc_ReportType9_VNP)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((uc_ReportType9_VNP)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_BaoCaoTongHopTaiPhongBan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp tại phòng CSKH khu vực";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_CSKHKV_BaoCaoTongHopNguoiDungTaiPhongBan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp của người dùng tại phòng CSKH khu vực";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:

                    break;
            }

            LastLoadedControl = pathUc;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 27/12/2013
        /// Todo : Hiển thị các vùng chọn điều kiện báo cáo của VNPT TT
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"> </param>
        private void ShowHideReportVNPTTTCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                //case "bc_VNPTTT_ChiTietGiamTruTraSauVNPTTT":
                //    pathUc = "UC/ucReportType1_VNPTTT.ascx";
                //    uc = this.LoadControl(pathUc);
                //    ((ucReportType1_VNPTTT)uc).ReportTitle = "Báo cáo chi tiết giảm trừ cước dịch vụ trả sau của VNPT tỉnh thành";
                //    ((ucReportType1_VNPTTT)uc).ReportType = reportType;
                //    ((ucReportType1_VNPTTT)uc).DoiTacId = doiTacId;
                //    //((ucReportType1_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                //    ((ucReportType1_VNPTTT)uc).IsFirstLoad = true;
                //    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                //    pnContainer.Controls.Add(uc);
                //    break;
                //case "bc_VNPTTT_SoLuongKNGDVTiepNhanXuLy":
                //    pathUc = "UC/ucReportType2_VNPTTT.ascx";
                //    uc = this.LoadControl(pathUc);
                //    ((ucReportType2_VNPTTT)uc).ReportTitle = "Báo cáo số lượng khiếu nại tiếp nhận và xử lý";
                //    ((ucReportType2_VNPTTT)uc).ReportType = reportType;
                //    ((ucReportType2_VNPTTT)uc).DoiTacId = doiTacId;
                //    ((ucReportType2_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                //    ((ucReportType2_VNPTTT)uc).IsFirstLoad = true;
                //    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                //    pnContainer.Controls.Add(uc);
                //    break;
                case "bc_VNPTTT_GiamTruKhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo giảm trừ do khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_KhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tình hình khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_ThucTrangGQKN":
                    pathUc = "UC/ucReportType4_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType4_VNPTTT)uc).ReportTitle = "Báo cáo thực trạng GQKN";
                    ((ucReportType4_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType4_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType4_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_TongHopPAKNTVTT":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp VTT";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_TongHopPAKNPhongBanVTT":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp phòng ban";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_TongHopPAKNTheoNguoiDung":
                    pathUc = "UC/ucReportType7_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType7_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp PAKN theo người dùng";
                    ((ucReportType7_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType7_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType7_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType7_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_TongHopPAKNTheoThang":
                    pathUc = "UC/ucReportType5_VNPTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType5_VNPTT)uc).ReportTitle = "Báo cáo tổng hợp PAKN theo tháng";
                    ((ucReportType5_VNPTT)uc).ReportType = reportType;
                    ((ucReportType5_VNPTT)uc).DoiTacId = doiTacId;
                    ((ucReportType5_VNPTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType5_VNPTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:

                    break;
            }

            LastLoadedControl = pathUc;

        }

        private void ShowHideReportTruongDaiDienCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            DoiTacInfo objDoiTac = new DoiTacImpl().GetVNPTTByTruongDaiDienId(doiTacId, DoiTacImpl.ListDoiTac);
            int doiTacVNPTTTId = -1;
            if (objDoiTac != null)
            {
                doiTacVNPTTTId = objDoiTac.Id;
            }

            switch (reportType)
            {
                case "bc_VNPTTT_GiamTruKhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo giảm trừ do khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacVNPTTTId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_KhieuNaiDichVu":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tình hình khiếu nại dịch vụ";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacVNPTTTId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_ThucTrangGQKN":
                    pathUc = "UC/ucReportType4_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType4_VNPTTT)uc).ReportTitle = "Báo cáo thực trạng GQKN";
                    ((ucReportType4_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType4_VNPTTT)uc).DoiTacId = doiTacVNPTTTId;
                    ((ucReportType4_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP":
                    pathUc = "UC/ucReportType3_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType3_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp giảm trừ cước DV GTGT theo CP";
                    ((ucReportType3_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType3_VNPTTT)uc).DoiTacId = doiTacVNPTTTId;
                    ((ucReportType3_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                default:

                    break;
            }

            LastLoadedControl = pathUc;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/12/2014
        /// Todo : Gọi màn hình hiển thị các báo cáo tổng hợp dùng chung của các đơn vị
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        private void ShowHideReportCommonCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_Common_TongHopDonVi":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp đơn vị";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_Common_TongHopPhongBan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp phòng ban";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_Common_TongHopNguoiDung":
                    pathUc = "UC/ucReportType7_VNPTTT.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType7_VNPTTT)uc).ReportTitle = "Báo cáo tổng hợp PAKN theo người dùng";
                    ((ucReportType7_VNPTTT)uc).ReportType = reportType;
                    ((ucReportType7_VNPTTT)uc).DoiTacId = doiTacId;
                    ((ucReportType7_VNPTTT)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType7_VNPTTT)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_Common_PhongBanCaNhan_TongHopPhongBan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp tại phòng";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_Common_PhongBanCaNhan_TongHopNguoiDung":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tổng hợp của người dùng tại phòng";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

                case "bc_CaNhan_TongHopCaNhan":
                    AdminInfo adminInfo = LoginAdmin.AdminLogin();
                    string nguoiXuLy = adminInfo != null ? adminInfo.Username : string.Empty;
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo cá nhân";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).NguoiXuLy = nguoiXuLy;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);

                    break;

                case "bc_Common_BaoCaoTonDongNguoiDungPhongBan":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo tồn đọng và quá hạn, người dùng phòng ban";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).NguoiXuLy = string.Empty;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);

                    break;
            }

            LastLoadedControl = pathUc;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/12/2014
        /// Todo : Gọi màn hình hiển thị các báo cáo tổng hợp dùng chung của các đơn vị
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        private void ShowHideReporDoiTacCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_DoiTac_BaoCaoSoLuongChuyenXuLyVNP":
                    pathUc = "UC/ucReportType_DateRange.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType_DateRange)uc).ReportTitle = "Báo cáo số lượng chuyển xử lý VNP";
                    ((ucReportType_DateRange)uc).ReportType = reportType;
                    ((ucReportType_DateRange)uc).DoiTacId = doiTacId;
                    ((ucReportType_DateRange)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType_DateRange)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
            }

            LastLoadedControl = pathUc;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 08/09/2015
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="doiTacId"></param>
        /// <param name="phongBanXuLyId"></param>
        private void ShowHideReportVNPTNETCondition(string reportType, int doiTacId, int phongBanXuLyId)
        {
            string pathUc = string.Empty;
            Control uc = null;

            switch (reportType)
            {
                case "bc_VNP_BaoCaoChatLuongMang":
                    pathUc = "UC/ucReportType1_VNPTNET.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType1_VNPTNET)uc).ReportTitle = "Báo cáo tổng hợp chất lượng mạng";
                    ((ucReportType1_VNPTNET)uc).ReportType = reportType;
                    ((ucReportType1_VNPTNET)uc).DoiTacId = doiTacId;
                    ((ucReportType1_VNPTNET)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType1_VNPTNET)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTNET_BaoCaoTongHopVNPTNET":
                    pathUc = "UC/ucReportType2_VNPTNET.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_VNPTNET)uc).ReportTitle = "Báo cáo tổng hợp VNPT NET";
                    ((ucReportType2_VNPTNET)uc).ReportType = reportType;
                    ((ucReportType2_VNPTNET)uc).DoiTacId = doiTacId;
                    ((ucReportType2_VNPTNET)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_VNPTNET)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTNET_BaoCaoTongHopDonVi":
                    pathUc = "UC/ucReportType2_VNPTNET.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_VNPTNET)uc).ReportTitle = "Báo cáo tổng hợp đơn vị";
                    ((ucReportType2_VNPTNET)uc).ReportType = reportType;
                    ((ucReportType2_VNPTNET)uc).DoiTacId = doiTacId;
                    ((ucReportType2_VNPTNET)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_VNPTNET)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTNET_BaoCaoTongHopPhongBan":
                    pathUc = "UC/ucReportType2_VNPTNET.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_VNPTNET)uc).ReportTitle = "Báo cáo tổng hợp phòng ban";
                    ((ucReportType2_VNPTNET)uc).ReportType = reportType;
                    ((ucReportType2_VNPTNET)uc).DoiTacId = doiTacId;
                    ((ucReportType2_VNPTNET)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_VNPTNET)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;
                case "bc_VNPTNET_BaoCaoTongHopNguoiDung":
                    pathUc = "UC/ucReportType2_VNPTNET.ascx";
                    uc = this.LoadControl(pathUc);
                    ((ucReportType2_VNPTNET)uc).ReportTitle = "Báo cáo tổng hợp người dùng";
                    ((ucReportType2_VNPTNET)uc).ReportType = reportType;
                    ((ucReportType2_VNPTNET)uc).DoiTacId = doiTacId;
                    ((ucReportType2_VNPTNET)uc).PhongBanXuLyId = phongBanXuLyId;
                    ((ucReportType2_VNPTNET)uc).IsFirstLoad = true;
                    uc.ID = pathUc.Replace("/", "_").Replace(".ascx", "");
                    pnContainer.Controls.Add(uc);
                    break;

            }

            LastLoadedControl = pathUc;
        }

        #endregion
        /// <summary>
        /// Author : Nguyen Chi Quang
        /// Created date : 01/08/2014
        /// Todo : Hiển thị các màn hình chọn điều kiện báo cáo của VNP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lblReport_GQKN_BaoCaoTongHopLoaiKhieuNai_Click(object sender, EventArgs e)
        {
            pnContainer.Controls.Clear();
            LinkButton lb = (LinkButton)sender;
            lblReportType.Text = lb.CommandArgument;
            ShowHideReportVNPCondition(lb.CommandArgument, this.DoiTacId, this.PhongBanXuLyId);
        }


    }
}
