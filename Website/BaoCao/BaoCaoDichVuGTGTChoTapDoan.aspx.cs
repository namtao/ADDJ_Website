using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;
using AIVietNam.Admin;
using System.Data;
using AIVietNam.Core;
using Website.AppCode.Controller;
using System.IO;

namespace Website.BaoCao
{
    public partial class BaoCaoDichVuGTGTChoTapDoan : PageBase
    {
        // Biến IsFirstLoad : để xác định usercontrol có phải là load lần đầu tiên không (biến này được truyền vào từ page khác)
        // vì IsPostBack ăn theo Page nên không thể xác định được đối với user control       
        public bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            liJs.Text = string.Empty;
            btnExport.Click += BtnExport_Click;

            if (!IsPostBack)
            {
                BindNumber(ddlMonth, 1, 12, DateTime.Now.AddMonths(-1).Month);
                BindNumber(ddlYear, DateTime.Now.Year - 5, DateTime.Now.Year, DateTime.Now.Year);
                BindDonViBaoCao(chkDonViBaoCao, string.Empty);
                //ddlNguonKhieuNai.DataSource = ServiceFactory.GetInstanceKhieuNai().GetListNguonKhieuNai(true);
                //ddlNguonKhieuNai.DataTextField = "Name";
                //ddlNguonKhieuNai.DataValueField = "Id";
                //ddlNguonKhieuNai.DataBind();

                //rowNguonKhieuNai.Visible = LoginAdmin.AdminLogin().PhongBanId == PhongBanInfo.PhongBanValueId.PHONG_CSKH_VNP;
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            int intMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            int intYear = Convert.ToInt32(ddlYear.SelectedValue);
            // Chặn chỉ có doanh thu tháng 5, 6 năm 2016

            DateTime cDate = new DateTime(intYear, intMonth, 15); // Lấy ngày giữa được chọn
            if (cDate > new DateTime(2016, 4, 30))
            {
                bool isSelectedIdOK = false;
                List<string> listSeleted = chkDonViBaoCao.Items.Cast<ListItem>().Where(V => V.Selected == true).Select(v => v.Value).ToList();
                if (listSeleted.Count > 0) isSelectedIdOK = true;

                string sNoiDungBaoCao = string.Empty;

                // Dữ liệu hợp lệ
                if (isSelectedIdOK) sNoiDungBaoCao = new BuildBaoCao().BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoan(intMonth, intYear, listSeleted.ToArray());
                else liJs.Text = string.Format("alert('{0}');", "Vui lòng chọn đơn vị báo cáo");

                RptContent.Controls.Add(new LiteralControl(sNoiDungBaoCao));

                if (rpLoaiBaoCao.SelectedValue == "0") // Xuất file Excel
                {
                    string bcName = string.Format("BaoCaoDichVuGtgtChoTapDoan_Thang{0}Nam{1}", intMonth, intYear);
                    Export2Excel(bcName, RptContent);
                }

            }
            else // Ngày tháng < tháng 5/2016 => Không có báo cáo
            {
                liJs.Text = string.Format("alert('Báo cáo chỉ thực hiện từ 5/2016');");
            }
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

            AdminInfo user = LoginAdmin.AdminLogin();

            int phongBanId = user.PhongBanId;

            // Vinaphone (Phòng CSKH Công ty) 
            int phongBanVinaphone = 60;

            // NET
            int phongBanNet = 921;

            // DoiTac - Media (Gộp của 2 thằng => Luôn thấy)

            // Vinaphone, Media, Đối tác (mã cứng)
            // (3572, 3387, 3486)

            int phongBanMedia = 1018;

            string sql = string.Empty;

            // Danh sách tài khoản có thể xem đầy đủ báo cáo doanh thu
            string[] taiKhoanXemFull = new string[] {
                "Administrator",
                "TTCntt_Ncpt",
                "lucth_cl_vnp",
                "anhbtn",
                "mkt_khcn",
                "01_bancl_vnpt",
                "02_bancl_vnpt",
                "htkh.ktndvas"
            };

            if (taiKhoanXemFull.Count(v => v.ToLower() == user.Username.ToLower()) > 0) // Tài khoản "Administrator"
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
                else if (phongBanId == phongBanMedia)
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
        private void Export2Excel(string tenbc, Control baocao)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCss = Server.MapPath("~/Css/BaoCao.css");
            StreamReader reader = new StreamReader(pathCss);
            //reader.ReadToEnd();

            Response.Write("<style type=\"text/Css\">");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
    }
}