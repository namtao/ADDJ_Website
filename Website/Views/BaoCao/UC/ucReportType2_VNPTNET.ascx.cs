using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;


namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType2_VNPTNET : System.Web.UI.UserControl
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
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                AdminInfo adminInfo = LoginAdmin.AdminLogin();
                if (adminInfo == null) return;

                LoadDonVi();

                ddlDonVi.Enabled = false;
                ddlPhongBan.Enabled = false;

                if(adminInfo.DoiTacId == DoiTacInfo.DoiTacIdValue.VNPT_NET)
                {
                    if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopVNPTNET")
                    {

                    }
                    else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopDonVi")
                    {

                    }
                    else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopPhongBan")
                    {
                        ddlDonVi.Enabled = true;
                    }
                    else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopNguoiDung")
                    {
                        ddlDonVi.Enabled = true;
                        ddlPhongBan.Enabled = true;
                    }           
                }
                else // Các đơn vị con thuộc NET
                {
                    ddlDonVi.SelectedValue = adminInfo.DoiTacId.ToString();
                    ddlDonVi_SelectedIndexChanged(null, null);

                    if(adminInfo.PhongBanId != 904 && adminInfo.PhongBanId !=  928 && adminInfo.PhongBanId != 930)
                    {
                        ddlPhongBan.SelectedValue = adminInfo.PhongBanId.ToString();
                    }
                    else // Là phòng lãnh đạo
                    {
                        if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopPhongBan")
                        {
                            ddlDonVi.Enabled = true;
                        }
                        else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopNguoiDung")
                        {                            
                            ddlPhongBan.Enabled = true;
                        }          
                    }
                }
                
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 21/10/2013
        /// Todo : Hiển thị báo cáo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string title = string.Empty;
            string donViId = ddlDonVi.SelectedValue;
            string phongBanId = ddlPhongBan.SelectedValue;
            string reportType = lblReportType.Text;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;           

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

            if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopVNPTNET")
            {                
                page = "baocaotonghopvnptnet.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopDonVi")
            {
                page = "baocaotonghopvnptnet.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopPhongBan")
            {
                page = "baocaotonghopvnptnet.aspx?";
            }
            else if (lblReportType.Text == "bc_VNPTNET_BaoCaoTongHopNguoiDung")
            {
                page = "baocaotonghoppakntheonguoidungvnpttt.aspx?";
                if(ConvertUtility.ToInt32(phongBanId) <= 0)
                {
                    errorMessage = string.Format("{0}\\nBạn phải chọn phòng ban", errorMessage);
                }
            }           


            if (page == "")
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn loại báo cáo", errorMessage);
            }

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else if (lblReportType.Text != "bc_VNPTNET_BaoCaoTongHopNguoiDung")
            {
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}reportType={2}&doiTacId={3}&phongBanId={4}&fromDate={5}&toDate={6}&loaibc={7}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, reportType, donViId, phongBanId, fromDate, toDate, loaibc);
            }
            else
            {
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}doiTacId={2}&phongBanId={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, donViId, phongBanId, fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        protected void ddlDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            int doiTacId = ConvertUtility.ToInt32(ddlDonVi.SelectedValue);
            LoadPhongBan(doiTacId);
        }

        #endregion

        #region Private methods

        private void LoadDonVi()
        {
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListByDonViTrucThuoc(DoiTacInfo.DoiTacIdValue.VNPT_NET);
            
            ddlDonVi.DataSource = listDoiTac;
            ddlDonVi.DataTextField = "TenDoiTac";
            ddlDonVi.DataValueField = "Id";
            ddlDonVi.DataBind();

            ListItem item = new ListItem("VNPT NET", DoiTacInfo.DoiTacIdValue.VNPT_NET.ToString());
            ddlDonVi.Items.Insert(0, item);
        }

        private void LoadPhongBan(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = null;
            if(doiTacId != DoiTacInfo.DoiTacIdValue.VNPT_NET)
            {
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);
            }
            
            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataTextField = "Name";
            ddlPhongBan.DataValueField = "Id";
            ddlPhongBan.DataBind();

            ListItem item = new ListItem("-- Tất cả --", "-1");
            ddlPhongBan.Items.Insert(0, item);
        }

        #endregion


    }
}