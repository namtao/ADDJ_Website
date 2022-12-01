using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.Admin;

namespace Website.Views.BaoCao.UC
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 02/12/2013
    /// Todo : Usercontrol điều kiện lọc báo cáo của báo cáo chi tiết cước giảm trừ trả sau
    /// </summary>
    public partial class ucDKTGiamTruCuocDVTraSau : System.Web.UI.UserControl
    {
        #region Event methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadKhuVuc();
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

            string reportType = string.Empty;
            string title = string.Empty;
            string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;
            string khuVuc = ddlKhuVuc_ReportType1.SelectedItem != null ? ddlKhuVuc_ReportType1.SelectedItem.Text : string.Empty;
            string donViId = ddlPhongBan_ReportType1.SelectedValue;
            string donVi = ddlPhongBan_ReportType1.SelectedItem != null ? ddlPhongBan_ReportType1.SelectedItem.Text : string.Empty;
            string doiTacId = ddlDoiTac_ReportType1.SelectedValue;
            string tenDoiTac = ddlDoiTac_ReportType1.SelectedItem != null ? ddlDoiTac_ReportType1.SelectedItem.Text : string.Empty;
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao_ReportType1.SelectedItem.Value;
            string loaiKhieuNaiId = ddlLoaiKhieuNai.SelectedValue;
            string loaiKhieuNai = ddlLoaiKhieuNai.SelectedItem != null ? ddlLoaiKhieuNai.SelectedItem.Text : string.Empty;
            string linhVucChungId = ddlLinhVucChung.SelectedValue;
            string linhVucChung = ddlLinhVucChung.SelectedItem != null ? ddlLinhVucChung.SelectedItem.Text : string.Empty;
            string linhVucConId = ddlLinhVucCon.SelectedValue;
            string linhVucCon = ddlLinhVucCon.SelectedItem != null ? ddlLinhVucCon.SelectedItem.Text : string.Empty;

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

            var page = "";
            
            page = "baocaochitietgiamtrutrasau.aspx";
            title = "Báo cáo chi tiết giảm trừ trả sau";
            

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
                //script = string.Format("<script type='text/javascript'> parent.$.messager.alertAuto('{0}', '<iframe style=\"border:none\" width=\"980px\" height=\"550px\" src=\"/Views/BaoCao/Popup/{1}?khuVucID={2}&khuVuc={3}&donViID={4}&donVi={5}&fromDate={6}&toDate={7}&loaiKhieuNaiID={8}&loaiKhieuNai={9}&linhVucChungID={10}&linhVucChung={11}&linhVucConID={12}&linhVucCon={13}&loaibc={14}\">');</script>", title, page, khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc);
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}?khuVucID={2}&khuVuc={3}&donViID={4}&donVi={5}&fromDate={6}&toDate={7}&loaiKhieuNaiID={8}&loaiKhieuNai={9}&linhVucChungID={10}&linhVucChung={11}&linhVucConID={12}&linhVucCon={13}&loaibc={14}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", title, page, khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaiKhieuNaiId, loaiKhieuNai, linhVucChungId, linhVucChung, linhVucConId, linhVucCon, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/10/2013
        /// Todo : Lấy ra các phòng ban thuộc khu vực
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlKhuVuc_ReportType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhongBan_ReportType1.Visible)
            {
                ddlPhongBan_ReportType1.Items.Clear();
                int doiTacId = ConvertUtility.ToInt32(ddlKhuVuc_ReportType1.SelectedValue);
                List<PhongBanInfo> listPhongBan = ServiceFactory.GetInstancePhongBan().GetListPhongBanByDoiTacId(doiTacId);

                if (listPhongBan == null)
                {
                    listPhongBan = new List<PhongBanInfo>();
                }

                PhongBanInfo objPhongBan = new PhongBanInfo();
                objPhongBan.Id = -1;
                objPhongBan.Name = "Chọn phòng ban..";
                listPhongBan.Insert(0, objPhongBan);
                ddlPhongBan_ReportType1.DataSource = listPhongBan;
                ddlPhongBan_ReportType1.DataBind();
            }
            else if (ddlDoiTac_ReportType1.Visible)
            {
                LoadDoiTacVNPTTT();
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
        }

        protected void ddlLoaiKhieuNai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLoaiKhieuNai.SelectedItem != null && ddlLoaiKhieuNai.SelectedValue.Length > 0)
            {
                List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId =" + ddlLoaiKhieuNai.SelectedValue, "Name ASC");
                ddlLinhVucChung.DataSource = listLoaiKhieuNai;
                ddlLinhVucChung.DataBind();

                ListItem item = new ListItem("Chọn lĩnh vực chung..", "-1");
                ddlLinhVucChung.Items.Insert(0, item);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
            }
        }

        protected void ddlLinhVucChung_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLinhVucChung.SelectedItem != null && ddlLinhVucChung.SelectedValue.Length > 0)
            {
                List<LoaiKhieuNaiInfo> listLoaiKhieuNai = ServiceFactory.GetInstanceLoaiKhieuNai().GetListDynamic("Id,Name", "ParentId =" + ddlLinhVucChung.SelectedValue, "Name ASC");
                ddlLinhVucCon.DataSource = listLoaiKhieuNai;
                ddlLinhVucCon.DataBind();

                ListItem item = new ListItem("Chọn lĩnh vực con..", "-1");
                ddlLinhVucCon.Items.Insert(0, item);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad();", true);
            }
        }

        #endregion   
     
        #region Private methods

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 02/10/2013
        /// Todo : Load combo đối tác
        /// </summary>
        private void LoadKhuVuc()
        {             
            ddlDoiTac_ReportType1.Items.Clear();

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
                    ddlDoiTac_ReportType1.Items.Add(item);

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
                if (listDoiTacInfo[i].DonViTrucThuoc == donViTrucThuoc)
                {
                    ListItem item = new ListItem();
                    item.Text = string.Format("{0}{1}", Server.HtmlDecode(space), listDoiTacInfo[i].TenDoiTac);
                    item.Value = listDoiTacInfo[i].Id.ToString(); ;
                    ddlDoiTac_ReportType1.Items.Add(item);

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
        /// Created date : 21/10/2013
        /// Todo : 
        /// </summary>
        private void LoadDoiTacVNPTTT()
        {
            string khuVucId = ddlKhuVuc_ReportType1.SelectedValue;

            string whereDonViTrucThuoc = string.Empty;
            if (khuVucId != "-1")
            {
                whereDonViTrucThuoc = " DonViTrucThuoc=" + khuVucId;
            }
            List<DoiTacInfo> listDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamic("*", whereDonViTrucThuoc, "TenDoiTac ASC");
            if (listDoiTac == null)
            {
                listDoiTac = new List<DoiTacInfo>();
            }

            DoiTacInfo objDoiTac = new DoiTacInfo();
            objDoiTac.Id = -1;
            objDoiTac.TenDoiTac = "Chọn VNPT TT..";
            listDoiTac.Insert(0, objDoiTac);

            ddlDoiTac_ReportType1.DataSource = listDoiTac;
            ddlDoiTac_ReportType1.DataBind();
        }

        #endregion
    }
}