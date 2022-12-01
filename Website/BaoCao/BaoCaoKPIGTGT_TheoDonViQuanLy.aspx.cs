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
using System.Text;

namespace Website.BaoCao
{
    public partial class BaoCaoKPIGTGT_TheoDonViQuanLy : PageBase
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
                BindDonViBaoCao(chkDonViBaoCao);
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
                List<string> dSDonViQuanLy = chkDonViBaoCao.Items.Cast<ListItem>().Where(V => V.Selected == true).Select(v => v.Value).ToList();
                if (dSDonViQuanLy.Count > 0) isSelectedIdOK = true;

                string sNoiDungBaoCao = string.Empty;

                // Dữ liệu hợp lệ
                if (isSelectedIdOK) sNoiDungBaoCao = new BuildBaoCao().BaoCaoKhieuNaiDoanhThuDichVuGTGTTapDoanTheoDonViQuanLy(intMonth, intYear, dSDonViQuanLy.ToArray());
                else liJs.Text = string.Format("alert('{0}');", "Vui lòng chọn đơn vị báo cáo");

                RptContent.Controls.Add(new LiteralControl(sNoiDungBaoCao));

                if (rpLoaiBaoCao.SelectedValue == "0") // Xuất file Excel
                {
                    string bcName = string.Format("BaoCaoDichVuGtgtChoTapDoanTheoDonVi_Thang{0}Nam{1}", intMonth, intYear);
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

        private void BindDonViBaoCao(CheckBoxList cblist)
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

            new EnumDonViQuanLyHelper().Bind2Control(chkDonViBaoCao);

        }
        private void Export2Excel(string tenbc, Control baocao)
        {
            string html = RenderControlToHtml(baocao);
            // string fileName = SaveStringHtml2Excel(html);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + tenbc + ".xls");
            Response.Charset = string.Empty;

            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(html);
            Response.End();
        }

        public string RenderControlToHtml(Control ControlToRender)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter stWriter = new System.IO.StringWriter(sb);
            System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stWriter);
            ControlToRender.RenderControl(htmlWriter);
            return sb.ToString();
        }

        protected string SaveStringHtml2Excel(string html)
        {
            string dirTemp = Server.MapPath("~/Temp/");
            if (!Directory.Exists(dirTemp)) Directory.CreateDirectory(dirTemp);

            Aspose.Cells.LoadOptions loadOptions = new Aspose.Cells.LoadOptions(Aspose.Cells.LoadFormat.Html);
            System.IO.Stream stream = new System.IO.MemoryStream(ASCIIEncoding.Default.GetBytes(html));
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(stream, loadOptions);
            string fileName = string.Format("File_{0:yyyyMMddHHmmss}.xlsx", DateTime.Now);
            wb.Save(string.Concat(dirTemp, fileName), Aspose.Cells.SaveFormat.Xlsx);
            stream.Close();
            return string.Concat(dirTemp, fileName);
        }
    }
}