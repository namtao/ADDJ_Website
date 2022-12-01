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
    public partial class ucReportType3_VNPTTT : System.Web.UI.UserControl
    {
        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string ReportTitle { get; set; }

        public bool IsFirstLoad { get; set; }

        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);

            if (IsFirstLoad)
            {
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblDoiTacId.Text = this.DoiTacId.ToString();
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                
                List<DoiTacInfo> listDoiTac = null;

                switch(this.DoiTacId)
                {
                    case 2 : // VNP1
                    case 7: // ĐKT1
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP1 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 3: // VNP2
                    case 14: // ĐKT2
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP2 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    case 5: // VNP3
                    case 19: // ĐKT3
                        listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", "DonViTrucThuoc=" + DoiTacInfo.DoiTacIdValue.VNP3 + " AND DoiTacType=" + DoiTacInfo.DoiTacTypeValue.VNPTTT, "TenDoiTac ASC");
                        break;
                    default:
                        listDoiTac = new List<DoiTacInfo>();
                        DoiTacInfo objDoiTac = ServiceFactory.GetInstanceDoiTac().GetInfo(ConvertUtility.ToInt32(lblDoiTacId.Text, 0));
                        listDoiTac.Add(objDoiTac);
                        ddlVNPTTT.Enabled = false;
                        rowVNPTTT.Visible = false;
                        break;
                }

                ddlVNPTTT.DataSource = listDoiTac;
                ddlVNPTTT.DataBind();
                if(listDoiTac != null && listDoiTac.Count >0)
                {
                    ddlVNPTTT.Items[0].Selected = true;
                }
                if (ReportType == "bc_VNPTTT_TongHopPAKNTheoNguoiDung")
                {
                    LoadComboPhongBan(this.DoiTacId);

                }
                else
                {
                    LoadComboPhongBan(ConvertUtility.ToInt32(ddlVNPTTT.Items[0].Value));
                }


                rowChkDongKhieuNai.Visible = lblReportType.Text == "bc_CSKHKV_GiamTruKhieuNaiDichVu"
                                            || lblReportType.Text == "bc_VNPTTT_GiamTruKhieuNaiDichVu"
                                            || lblReportType.Text == "bc_CSKHKV_KhieuNaiDichVu"
                                            || lblReportType.Text == "bc_VNPTTT_KhieuNaiDichVu";
            }

            //lblDoiTacId.Text = this.DoiTacId.ToString();
        }

        protected void ddlVNPTTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComboPhongBan(ConvertUtility.ToInt32(ddlVNPTTT.SelectedValue));
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string path = string.Empty;
            string doiTacId = ddlVNPTTT.SelectedValue;
            string tenDoiTac = ddlVNPTTT.SelectedItem.Text;
            string phongBanId = ddlPhongBan.SelectedValue;
            string tenPhongBan = ddlPhongBan.SelectedItem.Text;
            string layDuLieuTheo1HoacNhieuPhongBan = rblLayDuLieuTheo1HoacNhieuPhongBan.SelectedItem.Value;
            string isDongKN = rblThongKeTheoThoiGian.SelectedItem.Value == "1" ? "true" : "false";

            switch (lblReportType.Text)
            {
                case "bc_CSKHKV_GiamTruKhieuNaiDichVu":
                case "bc_VNPTTT_GiamTruKhieuNaiDichVu":
                    path = string.Format("/Views/BaoCao/Popup/baocaogiamtrudokhieunaidichvuvnpttt.aspx?doiTacId={0}&tenDoiTac={1}&fromDate={2}&toDate={3}&loaibc={4}&phongBanId={5}&tenPhongBan={6}&layDuLieuTheo1HoacNhieuPhongBan={7}&isDongKN={8}", doiTacId, tenDoiTac, fromDate, toDate, loaibc, phongBanId, tenPhongBan, layDuLieuTheo1HoacNhieuPhongBan, isDongKN);
                    break;
                case "bc_CSKHKV_KhieuNaiDichVu":
                case "bc_VNPTTT_KhieuNaiDichVu":
                    path = string.Format("/Views/BaoCao/Popup/baocaotinhhinhkhieunaidichvuvnpttt.aspx?doiTacId={0}&tenDoiTac={1}&fromDate={2}&toDate={3}&loaibc={4}&phongBanId={5}&tenPhongBan={6}&layDuLieuTheo1HoacNhieuPhongBan={7}&isDongKN={8}", doiTacId, tenDoiTac, fromDate, toDate, loaibc, phongBanId, tenPhongBan, layDuLieuTheo1HoacNhieuPhongBan, isDongKN);
                    break;
                case "bc_CSKHKV_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP":
                case "bc_VNPTTT_BaoCaoTongHopGiamTruCuocDVGTGTTheoCP":
                    path = string.Format("/Views/BaoCao/Popup/baocaotonghopgiamtru.aspx?khuVucID={0}&khuVuc={1}&fromDate={2}&toDate={3}&loaibc={4}&donViId={5}&donVi={6}&layDuLieuTheo1HoacNhieuPhongBan={7}", doiTacId, tenDoiTac, fromDate, toDate, loaibc, phongBanId, tenPhongBan, layDuLieuTheo1HoacNhieuPhongBan);
                    break;
                case "bc_VNPTTT_TongHopPAKNTheoNguoiDung":
                    path = string.Format("/Views/BaoCao/Popup/baocaotonghoppakntheonguoidungvnpttt.aspx?doitacId={0}&fromDate={1}&toDate={2}&loaibc={3}&phongBanId={4}&layDuLieuTheo1HoacNhieuPhongBan={5}", doiTacId, fromDate, toDate, loaibc, phongBanId, layDuLieuTheo1HoacNhieuPhongBan);
                    break;
                default:

                    break;
            }

            //string script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"{1}\">');</script>", lblTitle.Text, path);
            string script = string.Format("<script type='text/javascript'> window.open(url=\"{1}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", lblTitle.Text, path);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/06/2014
        /// Todo : Load combo phòng ban
        /// </summary>
        private void LoadComboPhongBan(int doiTacId)
        {
            int phongBanId = ConvertUtility.ToInt32(lblPhongBanXuLyId.Text);
            List<PhongBanInfo> listPhongBan = null;
            
            if(phongBanId <= 0)
            {
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfDoiTacId(doiTacId);
            }
            else
            {
                listPhongBan = ServiceFactory.GetInstancePhongBan().GetAllPhongBanOfAllOfParentId(phongBanId);
            }
            
            if(listPhongBan == null)
            {
                listPhongBan = new List<PhongBanInfo>();
            }

            listPhongBan = ServiceFactory.GetInstancePhongBan().SortListPhongBanForTree(listPhongBan);

            PhongBanInfo objPhongBan = new PhongBanInfo();
            objPhongBan.Id = -1;
            objPhongBan.Name = "--Tất cả--";
            listPhongBan.Insert(0, objPhongBan);

            ddlPhongBan.DataSource = listPhongBan;
            ddlPhongBan.DataBind();            
        }

        #endregion       
    }
}