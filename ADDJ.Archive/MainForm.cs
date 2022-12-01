using GQKN.Archive.Core;
using GQKN.Archive.Entity;
using GQKN.Archive.Forms;
using GQKN.Archive.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace GQKN.Archive
{
    public partial class MainForm : Form
    {

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.CenterToScreen();

            // Chọn trong Config
            string isChooseDate = ConfigurationManager.AppSettings["ChooseDate"];
            if (!string.IsNullOrEmpty(isChooseDate) && isChooseDate.Equals("1"))
            {
                try
                {
                    string fromDate = ConfigurationManager.AppSettings["FromDate"];
                    string toDate = ConfigurationManager.AppSettings["ToDate"];

                    dtpFromDate.Value = Convert.ToDateTime(fromDate, new CultureInfo("vi-VN"));
                    dtpToDate.Value = Convert.ToDateTime(toDate, new CultureInfo("vi-VN"));
                }
                catch
                {
                    // Ngày tháng mặc định
                    dtpFromDate.Value = dtpToDate.Value.StartOfYear();
                    dtpToDate.Value = DateTime.Now.AddMonths(-3).EndOfMonth();
                }

            }
            else
            {
                // Ngày tháng mặc định
                dtpFromDate.Value = dtpToDate.Value.StartOfYear();
                dtpToDate.Value = DateTime.Now.AddMonths(-3).EndOfMonth();
            }

        }

        private bool KiemTraKetNoiSolr()
        {
            if (Config.IsCommitSolr)
            {
                try
                {
                    string svSolr = string.Concat(Config.ServerSolr, "#/", "GQKN");
                    HttpWebResponse rp = RequestSolr(svSolr);
                    if (rp.StatusCode != HttpStatusCode.OK)
                    {
                        ShowText(rp.StatusDescription);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Helper.GhiLogs(ex);
                    ShowText(ex.Message);
                    return false;
                }
                ShowText("Kết nối Solr thành công");
                return true;
            }
            return true;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 14/07/2014
        /// Todo : Thực hiện lưu trữ dữ liệu
        /// </summary>
        private void btnArchive_Click(object sender, EventArgs e)
        {
            // Xử lý 1 lần
            MainProcess(false);
        }
        private void MainProcess(bool isRepeat2End)
        {
            txtMessage.Text = string.Empty;
            lblDelete.Text = 0.ToString();
            lblCopy.Text = 0.ToString();
            lblTotal.Text = 0.ToString();

            // Nếu muốn commit dữ liệu lên Solr
            if (KiemTraKetNoiSolr())
            {

                //ShowText(dtpFromDate.Text);
                DateTime fromDate = dtpFromDate.Value.StartOfDay();
                DateTime toDate = dtpToDate.Value.EndOfDay();

                // Năm không hợp lệ
                if (fromDate.Year != toDate.Year)
                {
                    ShowText("Thời gian Archive phải cùng trong năm");
                    return;
                }

                TimeSpan ts = toDate - fromDate;
                // Ngày bắt đầu > ngày kết thúc => Không hợp lệ
                if (ts.Ticks < 0)
                {
                    ShowText("Thời điểm kết thúc phải lớn hơn bắt đầu");
                    return;
                }

                ShowText("Bắt đầu thực hiện lưu trữ");
                Thread t = new Thread(() =>
                {
                    try
                    {
                        ArchiveImpl.IsContinue = true;

                        DisabelForm(false);

                        ShowText("Archive From " + dtpFromDate.Text);
                        ShowText("Archive End " + dtpToDate.Text);

                        lblStartTime.Text = DateTime.Now.ToString("HH:mm:ss");

                        ArchiveImpl ctl = new ArchiveImpl();
                        ctl.ArchiveInfo = LayCauHinhLuuTru(fromDate.Year);

                        // Xử lý dữ liệu
                        ctl.XuLyLuuTruDuLieu(fromDate, toDate, txtMessage, lblDelete, lblCopy, lblTotal, isRepeat2End);

                        ShowText("Kết thúc tiến trình lưu trữ");

                        lblEndTime.Text = DateTime.Now.ToString("HH:mm:ss");
                    }
                    catch (Exception ex)
                    {
                        Helper.GhiLogs(ex);
                        ShowText("Có lỗi xảy ra. Hệ thống tự động dừng Archive dữ liệu.");
                    }
                    finally
                    {
                        DisabelForm(true);
                        Helper.GhiLogs("Archive", txtMessage.Text);
                    }

                });
                t.IsBackground = true;
                t.Start();
            }
        }

        private void MainProcessCommitSolr(bool isRepeat2End)
        {
            txtMessage.Text = string.Empty;
            lblDelete.Text = 0.ToString();
            lblCopy.Text = 0.ToString();
            lblTotal.Text = 0.ToString();

            ShowText("Start update solr");
            Thread t = new Thread(() =>
            {
                try
                {
                    ArchiveImpl.IsContinue = true;

                    DisabelForm(false);

                    DateTime fromDate = dtpFromDate.Value.StartOfDay();
                    DateTime toDate = dtpToDate.Value.EndOfDay();

                    ShowText("Archive From " + dtpFromDate.Text);
                    ShowText("Archive End " + dtpToDate.Text);

                    lblStartTime.Text = DateTime.Now.ToString("HH:mm:ss");

                    ArchiveImpl ctl = new ArchiveImpl();
                    ctl.ArchiveInfo = LayCauHinhLuuTru(fromDate.Year);

                    ctl.CapNhatKhieuNaiArchive2Solr(fromDate, toDate, txtMessage, lblDelete, lblCopy, lblTotal, isRepeat2End);

                    // Hiệu lực cho hạng mục update
                    // ctl.CommitSolr4Update();
                    ShowText("Finish update solr.");

                    lblEndTime.Text = DateTime.Now.ToString("HH:mm:ss");
                }
                catch (Exception ex)
                {
                    Helper.GhiLogs(ex);
                    lblEndTime.Text = DateTime.Now.ToString("HH:mm:ss");
                    ShowText("Có lỗi xảy ra. Hệ thống tự động dừng Archive dữ liệu.");
                }
                finally
                {
                    DisabelForm(true);
                    Helper.GhiLogs("CommitSolr", txtMessage.Text);
                }

            });
            t.IsBackground = true;
            t.Start();
        }
        private void btnActive2End_Click(object sender, EventArgs e)
        {
            // Xử lý đến khi hết điều kiện ngày tháng
            // Số lượng mỗi lần cấu hình trong config, default = 250 (lớn hơn gây lỗi bảng KhieuNai_SoTien)
            MainProcess(true);
        }
        private void btnCommitSolr_Click(object sender, EventArgs e)
        {
            MainProcessCommitSolr(false);
        }
        private void btnCommitSolr2_Click(object sender, EventArgs e)
        {
            // Commit đến hến hết danh sách được chọn
            MainProcessCommitSolr(true);
        }
        private ArchiveConfigInfo LayCauHinhLuuTru(int namLuuTru)
        {
            ArchiveConfigImpl archiveConfigImpl = new ArchiveConfigImpl();
            string whereClause = string.Format("NamLuuTru = {0}", namLuuTru);
            List<ArchiveConfigInfo> listArchiveConfigInfo = archiveConfigImpl.GetListDynamic("*", whereClause, "Id ASC");
            return listArchiveConfigInfo[0];
        }
        private void ShowText(TextBox txtThongBao, string str)
        {
            txtThongBao.AppendText(string.Format("{0}:{1}{2}", DateTime.Now.ToString(), str, Environment.NewLine));
        }
        private void cậpNhậtLoạiKhiếuNạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateLoaiKhieuNaiForm frmLoaiKhieuNai = new UpdateLoaiKhieuNaiForm();
            frmLoaiKhieuNai.Show();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            ShowText("Đang tiến hành xử lý stop...");
            ArchiveImpl.IsContinue = false;
        }
        public ConfigArchive LoadConfig(int action = 0)
        {
            try
            {
                if (File.Exists(SETTING))
                {
                    //Utility.LogEvent("3");
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigArchive));
                    //Utility.LogEvent("4");
                    StreamReader sr = new StreamReader(SETTING);
                    //Utility.LogEvent("5");
                    ConfigArchive config = xs.Deserialize(sr) as ConfigArchive;
                    //Utility.LogEvent("6");
                    sr.Close();
                    //Utility.LogEvent("7");
                    return config;
                }
                else
                {
                    ConfigArchive c = new ConfigArchive();
                    c.FromDate = dtpFromDate.Value;
                    c.ToDate = dtpToDate.Value;
                    c.Action = action;
                    return c;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return null;
            }
        }
        public void SaveConfig(ConfigArchive item)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ConfigArchive));
            StreamWriter sw = new StreamWriter(SETTING);
            xs.Serialize(sw, item);
            sw.Close();
        }

        private void btnUpdateArchive_Click(object sender, EventArgs e)
        {
            try
            {
                ShowText(txtMessageUpdateSolr, "Bắt đầu cập nhật");

                int archiveId = Convert.ToInt32(txtArchiveId.Text);
                int khieuNaiId = Convert.ToInt32(txtKhieuNaiId.Text);

                List<int> lstKhieuNaiId = new List<int>();
                lstKhieuNaiId.Add(khieuNaiId);

                UpdateKhieuNaiArchive(lstKhieuNaiId, archiveId);

                ShowText(txtMessageUpdateSolr, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                ShowText(txtMessageUpdateSolr, ex.Message);
                ShowText(txtMessageUpdateSolr, ex.StackTrace);
            }

            ShowText(txtMessageUpdateSolr, "Kết thúc xử lý");
        }
        /// <summary>
        /// Kiểu Json do response trả về
        /// </summary>
        private string UpdateKhieuNaiArchive(List<int> dsKhieuNaiIds, int archiveId)
        {
            string core = "GQKN";
            string url = string.Format("{0}{1}/update?commit=true", Config.ServerSolr, core);

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            List<object> objs = new List<object>();
            foreach (int khieuNaiId in dsKhieuNaiIds)
            {
                var objKhieuNai = new
                {
                    Id = khieuNaiId.ToString(),
                    ArchiveId = new
                    {
                        set = archiveId
                    }
                };
                objs.Add(objKhieuNai);
            }

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(objs);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream())) sw.Write(data);

            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream stream = httpWebResponse.GetResponseStream();

            StreamReader responseReader = new StreamReader(httpWebResponse.GetResponseStream());

            // Cũng trả về kiểu Json
            return responseReader.ReadToEnd();
        }

        private void btnClearTextbox_Click(object sender, EventArgs e)
        {
            txtMessage.Text = string.Empty;
        }
        private HttpWebResponse RequestSolr(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = true;
            request.Timeout = 5 * 60 * 1000; // 5 phút  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
            request.CookieContainer = new CookieContainer();

            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                ShowText(url);
                ShowText(ex.Message);
                throw ex;
            }
        }
        public MainForm()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void DisabelForm(bool flag)
        {
            txtMessage.ReadOnly = !flag;
            btnStop.Enabled = !flag;

            dtpFromDate.Enabled = flag;
            dtpToDate.Enabled = flag;
            btnArchive.Enabled = flag;
            btnActive2End.Enabled = flag;
            btnComitSolr.Enabled = flag;
            btnCommitSolr2.Enabled = flag;
            btnUpdateTemp.Enabled = flag;
        }
        private void ShowText(string str)
        {
            txtMessage.AppendText(string.Format("{0}:{1}{2}", DateTime.Now.ToString(), str, Environment.NewLine));
        }

        private const string SETTING = "SETTING.DAT";

        private void btnUpdateTemp_Click(object sender, EventArgs e)
        {
            // Kịch bản
            // => Đưa dữ liệu từ bảng chính vào lưu trữ (đồng thời đã đổi ArchiveId)
            // => Cần đưa danh sách các KN này vào bảng tạm để sử dụng cho việc đánh dấu KN nào đã đổi ArchiveId trên solr (mặc định = 0)
            // => Câu truy vấn là thêm vào bảng tạm dữ liệu này
            // => Chỉ tính theo năm, được chọn trên FromDate

            DisabelForm(false);
            txtMessage.Text = string.Empty;
            ShowText("Bắt đầu thực hiện");

            Thread t = new Thread(() =>
            {
                try
                {
                    ArchiveImpl ctl = new ArchiveImpl();
                    ctl.ArchiveInfo = LayCauHinhLuuTru(dtpFromDate.Value.Year); // Lấy ra chuỗi kết nối của Database phụ

                    string sql = @" INSERT INTO KhieuNai_UpdateArchive(	KhieuNaiId,	ArchiveId,	NgayTiepNhanSort)
                                    SELECT 
	                                    Id,
	                                    ArchiveId,
	                                    NgayTiepNhanSort
                                    FROM
	                                    KhieuNai
                                    WHERE
	                                    Id NOT IN (SELECT KhieuNaiId FROM KhieuNai_UpdateArchive)";

                    int number = SqlHelper.ExecuteNonQuery(ctl.ArchiveInfo.ConnectionString, CommandType.Text, sql);
                    ShowText("Số lượng cập nhật vào bảng phụ: " + number);
                }
                catch (Exception ex)
                {
                    Helper.GhiLogs(ex);
                    ShowText(ex.Message);
                    ShowText(ex.StackTrace);
                }

                ShowText("Kết thúc");
                DisabelForm(true);
            });
            t.IsBackground = true;
            t.Start();

        }
    }
}
