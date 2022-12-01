using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.BaoCao.UC
{
    public partial class uc_ReportType4_VNP : System.Web.UI.UserControl
    {
        private readonly string _pathXMLFile = "~/Views/BaoCao/XMLFiles/DKT_ToXLNV/NguoiDungToXLNV{0}.xml";


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
                lblTitle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblDoiTacId.Text = this.DoiTacId.ToString();

                if(BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Phân_việc_cho_người_dùng_trong_phòng))
                {
                    Get_Xml();                
                }
                else
                {
                    UpdatePanel2.Visible = false;
                }
                
            }                     
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 04/01/2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            DataSet dsNguoiDung = new DataSet();
            dsNguoiDung.ReadXml(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));
            Session["NguoiDungToXLNV"] = dsNguoiDung != null && dsNguoiDung.Tables.Count > 0 ? dsNguoiDung.Tables[0] : null;

            string khuVucId = string.Empty;
            string khuVuc = string.Empty;
            string donViId = lblPhongBanXuLyId.Text;
            string donVi = string.Empty;

            PhongBanInfo objPhongBan = ServiceFactory.GetInstancePhongBan().GetInfo(ConvertUtility.ToInt32(lblPhongBanXuLyId.Text));
            if(objPhongBan != null)
            {
                khuVucId = objPhongBan.DoiTacId.ToString();
                donVi = objPhongBan.Name;
            }
            
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;
            string loaibc = rblLoaiBaoCao.SelectedItem.Value;
            string doiTac = string.Empty;
            
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

            if (errorMessage.Length > 0)
            {
                script = string.Format("<script type='text/javascript'>alert('{0}');</script>", errorMessage);
            }
            else
            {                
                //script = string.Format("<script type='text/javascript'>parent.$.messager.alertAuto('Báo cáo khối lượng công việc', '<iframe style=\"border:none\" width=\"980px\" height=\"540px\" src=\"/Views/BaoCao/Popup/baocaokhoiluongcongviecdkt.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\">');</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);               
                script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/baocaokhoiluongcongviecdkt.aspx?khuVucID={0}&khuVuc={1}&donViID={2}&donVi={3}&fromDate={4}&toDate={5}&loaibc={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>", khuVucId, khuVuc, donViId, donVi, fromDate, toDate, loaibc);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai_ReportType2.ClientID + "');", true);
        }

        #region Event methods

        private void Get_Xml()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));           

            if (ds != null && ds.HasChanges())
            {               
                XmlGridView.DataSource = ds;
                XmlGridView.DataBind();
            }
            else
            {
                XmlGridView.DataBind();
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            Insert_XML();
        }

        private void Insert_XML()
        {
            TextBox txtSoTT = XmlGridView.FooterRow.FindControl("txtSoTT") as TextBox;
            TextBox txtTenDayDu = XmlGridView.FooterRow.FindControl("txtTenDayDu") as TextBox;
            TextBox txtTenTruyCap = XmlGridView.FooterRow.FindControl("txtTenTruyCap") as TextBox;
            CheckBox chkIsHoatDong = XmlGridView.FooterRow.FindControl("chkIsHoatDong") as CheckBox;

            XmlDocument MyXmlDocument = new XmlDocument();
            MyXmlDocument.Load(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));
            XmlElement ParentElement = MyXmlDocument.CreateElement("NguoiSuDung");
            XmlElement xeSoTT = MyXmlDocument.CreateElement("SoTT");
            xeSoTT.InnerText = txtSoTT.Text;
            XmlElement xeTenDayDu = MyXmlDocument.CreateElement("TenDayDu");
            xeTenDayDu.InnerText = txtTenDayDu.Text;
            XmlElement xeTenTruyCap = MyXmlDocument.CreateElement("TenTruyCap");
            xeTenTruyCap.InnerText = txtTenTruyCap.Text;
            XmlElement xeIsHoatDong = MyXmlDocument.CreateElement("IsHoatDong");
            xeIsHoatDong.InnerText = chkIsHoatDong.Checked.ToString();           
            ParentElement.AppendChild(xeSoTT);
            ParentElement.AppendChild(xeTenDayDu);
            ParentElement.AppendChild(xeTenTruyCap);
            ParentElement.AppendChild(xeIsHoatDong);            
            MyXmlDocument.DocumentElement.AppendChild(ParentElement);
            MyXmlDocument.Save(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));

            Get_Xml();

        }

        protected void XmlGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            XmlGridView.EditIndex = e.NewEditIndex;           

            Get_Xml();
        }

        protected void XmlGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            XmlGridView.EditIndex = -1;
            Get_Xml();
        }

        protected void XmlGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = XmlGridView.Rows[e.RowIndex].DataItemIndex;
            TextBox txtEditSoTT = XmlGridView.Rows[e.RowIndex].FindControl("txtEditSoTT") as TextBox;
            TextBox txtEditTenDayDu = XmlGridView.Rows[e.RowIndex].FindControl("txtEditTenDayDu") as TextBox;
            TextBox txtEditTenTruyCap = XmlGridView.Rows[e.RowIndex].FindControl("txtEditTenTruyCap") as TextBox;
            CheckBox chkEditIsHoatDong = XmlGridView.Rows[e.RowIndex].FindControl("chkEditIsHoatDong") as CheckBox;           
            XmlGridView.EditIndex = -1;
            Get_Xml();

            DataSet ds = XmlGridView.DataSource as DataSet;            
            ds.Tables[0].Rows[id]["SoTT"] = txtEditSoTT.Text;
            ds.Tables[0].Rows[id]["TenDayDu"] = txtEditTenDayDu.Text;
            ds.Tables[0].Rows[id]["TenTruyCap"] = txtEditTenTruyCap.Text;
            ds.Tables[0].Rows[id]["IsHoatDong"] = chkEditIsHoatDong.Checked;
            ds.WriteXml(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));
            Get_Xml();
        }

        protected void XmlGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Get_Xml();
            DataSet ds = XmlGridView.DataSource as DataSet;
            ds.Tables[0].Rows[XmlGridView.Rows[e.RowIndex].DataItemIndex].Delete();
            ds.WriteXml(string.Format(Server.MapPath(_pathXMLFile), lblPhongBanXuLyId.Text));
            Get_Xml();

        }

        #endregion                      

    }
}