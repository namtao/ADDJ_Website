using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.BaoCao.UC
{
    public partial class ucReportType_DateRange_New : MyControl
    {
        public string ReportType { get; set; }

        public int DoiTacId { get; set; }

        public int PhongBanXuLyId { get; set; }

        public string NguoiXuLy { get; set; }

        public string ReportTitle { get; set; }

        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(UdPnlMain, UdPnlMain.GetType(), "onload", "jsBaoCaoThongKeOnLoad('');", true);

            if (IsFirstLoad)
            {
                lblTittle.Text = this.ReportTitle;
                lblReportType.Text = this.ReportType;
                lblPhongBanXuLyId.Text = this.PhongBanXuLyId.ToString();
                lblNguoiXuLy.Text = this.NguoiXuLy;
                lblDoiTacId.Text = this.DoiTacId.ToString();

                BindNumber(ddlMonth, 1, 12, DateTime.Now.Month);
                BindNumber(ddlYear, DateTime.Now.Year - 5, DateTime.Now.Year, DateTime.Now.Year);
                BindDonViBaoCao(chkDonViBaoCao, string.Empty);
                //ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                //ddlNguonKhieuNai.DataTextField = "Name";
                //ddlNguonKhieuNai.DataValueField = "Id";
                //ddlNguonKhieuNai.DataBind();

                //rowNguonKhieuNai.Visible = LoginAdmin.AdminLogin().PhongBanId == PhongBanInfo.PhongBanValueId.PHONG_CSKH_VNP;
            }
        }


        protected void lbShowReport_Click(object sender, EventArgs e)
        {
            DateTime nullDateTime = new DateTime(1900, 01, 01);

            string loaibc = rblLoaiBaoCao_ReportType.SelectedItem.Value;
            //string nguonKhieuNai = ddlNguonKhieuNai.SelectedValue;
            string script = string.Empty;
            List<ListItem> lst = chkDonViBaoCao.Items.Cast<ListItem>().Where(x => x.Selected).ToList();

            if (lst.Count == 0) script = string.Format("<script type=\"text/javascript\">alert('Vui lòng chọn đơn vị!');</script>");
            else
            {
                // Lấy danh sách được chọn
                List<string> vals = new List<string>();
                foreach (ListItem item in lst) vals.Add(item.Value);

                string page = string.Empty;
                switch (lblReportType.Text)
                {
                    case "bc_VNP_BaoCaoDVGTGTTapDoan_New":
                        page = "BaoCaoDVGTGTChoTapDoan.aspx?";
                        // page = "EmptyPage.aspx?"; // Old
                        script = string.Format("<script type='text/javascript'> window.open(url=\"/Views/BaoCao/Popup/{1}Page={2}&Month={3}&Year={4}&LoaiBC={5}&Ids={6}\",\"_blank\", \"width=980, height=550,scrollbars=1,location=0\");</script>",
                            lblTittle.Text,
                            page,
                             // reportType,
                             lblReportType.Text.ToLower(),
                            ddlMonth.SelectedValue,
                            ddlYear.SelectedValue,
                            loaibc,
                            string.Format("{0}", string.Join(",", vals))
                            );
                        break;
                    default:
                        page = string.Empty;
                        break;
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openWindow", script, false);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "onload", "jsBaoCaoThongKeOnLoad('" + tvLoaiKhieuNai.ClientID + "');", true);
        }

        private void BindNumber(DropDownList ddl, int start, int end, int selected)
        {

            for (int i = start; i <= end; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddl.SelectedValue = selected.ToString();
        }

        private void BindDonViBaoCao(CheckBoxList cblist, string selected)
        {
            if (IsLogin)
            {

                AdminInfo user = ContextUser;

                int phongBanId = user.PhongBanId;

                // Vinaphone (Phòng CSKH Công ty) 
                int phongBanVinaphone = 60;

                // NET
                int phongBanNet = 921;

                // DoiTac - Media (Gộp của 2 thằng => Luôn thấy)

                // Vinaphone, Media, Đối tác (mã cứng)
                // (3572, 3387, 3486)
                string sql = string.Empty;
                if (user.Username.ToUpper() == "Administrator".ToUpper()) // Tài khoản "Administrator"
                {
                    sql = "SELECT Id, Name FROM LoaiKhieuNai WHERE Cap = 1 AND LoaiKhieuNai_NhomId = 22 AND Status = 1 ORDER BY Sort";
                }
                else
                {
                    if (phongBanId == phongBanVinaphone) // Thuộc phòng ban CSKH Vinaphone
                    {
                        sql = "SELECT Id, Name FROM LoaiKhieuNai WHERE Cap = 1 AND LoaiKhieuNai_NhomId = 22 AND Status = 1 AND Id IN (3572, 3486) ORDER BY Sort";
                    }
                    else if (phongBanId == phongBanNet) // Thuộc phòng ban NET
                    {
                        sql = "SELECT Id, Name FROM LoaiKhieuNai WHERE Cap = 1 AND LoaiKhieuNai_NhomId = 22 AND Status = 1 AND Id IN (3387, 3486) ORDER BY Sort";
                    }

                    else
                    {
                        sql = "SELECT Id, Name FROM LoaiKhieuNai WHERE Cap = 1 AND LoaiKhieuNai_NhomId = 22 AND Status = 1 AND Id IN (3486) ORDER BY Sort";
                    }
                }
                DataTable tbl = SqlHelper.ExecuteDataset(Config.ConnectionString, CommandType.Text, sql).Tables[0];
                cblist.Items.Clear();
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow r in tbl.Rows)
                    {
                        cblist.Items.Add(new ListItem(r["Name"].ToString(), r["Id"].ToString()));
                    }
                }
                cblist.SelectedValue = selected;
            }
        }
    }
}