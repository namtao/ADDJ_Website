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
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType2_TTTC : System.Web.UI.UserControl
    {
        private readonly int TTTC = 10100;
        private readonly int PHONG_LANH_DAO_TTTC = 101;

        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }

        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);   

            if (IsFirstLoad)
            {
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                LoadPhongBan(TTTC);

                AdminInfo adminInfo = LoginAdmin.AdminLogin();
                ddlPhongBan_ReportType3.Enabled = adminInfo != null && adminInfo.PhongBanId == PHONG_LANH_DAO_TTTC;

                if(!ddlPhongBan_ReportType3.Enabled && adminInfo != null)
                {
                    ddlPhongBan_ReportType3.SelectedValue = adminInfo.PhongBanId.ToString();
                    ddlPhongBan_ReportType3_SelectedIndexChanged(null, null);
                }
            }
        }

        #region ReportType 3

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 12/11/2013
        /// Todo : Lấy ra danh sách phòng ban của TTTC
        /// </summary>
        /// <param name="idTTTC"></param>
        private void LoadPhongBan(int idTTTC)
        {
            List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(idTTTC);
            if (listPhongBan == null)
            {
                listPhongBan = new List<PhongBanInfo>();
            }

            PhongBanInfo objPhongBan = new PhongBanInfo();
            objPhongBan.Id = -1;
            objPhongBan.Name = "Chọn phòng ban..";
            listPhongBan.Insert(0, objPhongBan);

            ddlPhongBan_ReportType3.DataSource = listPhongBan;
            ddlPhongBan_ReportType3.DataBind();           

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/10/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPhongBan_ReportType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblNguoiDung.Items.Clear();
            int phongBanId = ConvertUtility.ToInt32(ddlPhongBan_ReportType3.SelectedValue, -1);
            List<NguoiSuDungInfo> listNguoiSuDung = ServiceFactory.GetInstanceNguoiSuDung().GetListNguoiSuDungByPhongBanId(phongBanId);

            AdminInfo userLogin = LoginAdmin.AdminLogin();
            if (userLogin != null && userLogin.PhongBanId != PHONG_LANH_DAO_TTTC && !BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng))
            {
                for(int i=0;i<listNguoiSuDung.Count;i++)
                {
                    if(listNguoiSuDung[i].Id != userLogin.Id)
                    {
                        listNguoiSuDung.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (listNguoiSuDung != null)
            {
                for (int i = 0; i < listNguoiSuDung.Count; i++)
                {
                    ListItem item = new ListItem(listNguoiSuDung[i].TenDayDu, listNguoiSuDung[i].TenTruyCap);
                    cblNguoiDung.Items.Add(item);
                    cblNguoiDung.Items[cblNguoiDung.Items.Count - 1].Selected = true;
                }
            }

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/10/2013
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReport_ReportType3_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            //string khuVucId = ddlKhuVuc_ReportType2.SelectedValue;
            //string khuVuc = ddlKhuVuc_ReportType2.SelectedItem != null ? ddlKhuVuc_ReportType2.SelectedItem.Text : string.Empty;
            //string donViId = ddlPhongBan_ReportType2.SelectedValue;
            //string donVi = ddlPhongBan_ReportType2.SelectedItem != null ? ddlPhongBan_ReportType2.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate_ReportType3.Text;
            string toDate = txtToDate_ReportType3.Text;
            string loaibc = rblLoaiBaoCao_ReportType3.SelectedItem.Value;

            //string loaiKhieuNaiId = string.Empty;
            //string linhVucChungId = string.Empty;
            //string linhVucConId = string.Empty;

            string tenTruyCap = string.Empty;

            foreach (ListItem item in cblNguoiDung.Items)
            {
                if (item.Selected)
                {
                    tenTruyCap = string.Format("{0}{1},", tenTruyCap, item.Value);
                }
            }

            if (tenTruyCap.Length > 0)
            {
                tenTruyCap = tenTruyCap.TrimEnd(',');
            }

            //foreach (TreeNode nodeLoaiKhieuNai in tvLoaiKhieuNai_ReportType2.Nodes)
            //{
            //    if (nodeLoaiKhieuNai.Checked)
            //    {
            //        loaiKhieuNaiId = string.Format("{0}{1},", loaiKhieuNaiId, nodeLoaiKhieuNai.Value);
            //    }

            //    GetListLinhVucChung(nodeLoaiKhieuNai, ref linhVucChungId, ref linhVucConId);
            //}

            //if (loaiKhieuNaiId.Length > 0)
            //{
            //    loaiKhieuNaiId = loaiKhieuNaiId.TrimEnd(',');
            //}

            //if (linhVucChungId.Length > 0)
            //{
            //    linhVucChungId = linhVucChungId.TrimEnd(',');
            //}

            //if (linhVucConId.Length > 0)
            //{
            //    linhVucConId = linhVucConId.TrimEnd(',');
            //}

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

            if (tenTruyCap.Length == 0)
            {
                errorMessage = string.Format("{0}\\nBạn phải chọn người dùng", errorMessage);
            }


            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {
                //script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo chi tiết PAKN theo người dùng', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaochitietpakntheonguoidungtttc.aspx?tenTruyCap={0}&fromDate={1}&toDate={2}&loaibc={3}&khuVucId={4}\">');</script>", tenTruyCap, fromDate, toDate, loaibc, TTTC);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaochitietpakntheonguoidungtttc.aspx?tenTruyCap={0}&fromDate={1}&toDate={2}&loaibc={3}&khuVucId={4}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", tenTruyCap, fromDate, toDate, loaibc, TTTC);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        #endregion    
    }
}