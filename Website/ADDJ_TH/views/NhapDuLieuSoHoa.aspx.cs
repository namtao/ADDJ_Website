using Aspose.Cells;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;

namespace Website.ADDJ_TH.views
{
    public partial class NhapDuLieuSoHoa : System.Web.UI.Page
    {
        private bool isXls = false; // nếu file xử lý là excel XLS, ngược lại là XLSX
        private string tenHoSo = string.Empty;
        string madvhinhthanhphong = "";
        string maphongdv = "";
        string namchinhlyphong = "";
        string tenCoQuan = "";
        private string path; // Khai báo đường dẫn thư mục lưu cấu hình
        private List<string> nhanBietCotExcel_MLHS = new List<string>(); // Biến này sẽ nhận biết sự thay đổi cột trong file excel 
                                                                         // để cảnh báo người nhập excel vào.
        private List<string> nhanBietCotExcel_MLVB = new List<string>(); // Biến này sẽ nhận biết sự thay đổi cột trong file excel 
                                                                         // để cảnh báo người nhập excel vào.
                                                                         // Khởi tạo bộ ghi log
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                //this.Invoke(new Action(() =>
                //{
                //    radLabel2.Text = string.Format("{0}/{1} ({2:N0}%)", e.BytesTransferred, e.TotalBytesToTransfer,
                //         e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));

                //    radProgressBar1.Maximum = (int)e.TotalBytesToTransfer;
                //    radProgressBar1.Value1 = (int)e.BytesTransferred;
                //}));
            }
            else if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                //this.Invoke(new Action(() =>
                //{
                //    radLabel1.Text = string.Format("Đang giải nén: {0}", e.CurrentEntry.FileName);
                //}));
            }
        }
        protected void btnXuLySoHoa_Click(object sender, EventArgs e)
        {
            // bước 0: lấy danh sách các file ZIP
            var uploadedFiles = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/SOHOA_TEMP"));
            foreach (var file in files)
            {
                //var fileInfo = new FileInfo(file);
                //var uploadedFile = new UploadedFile() { Name = Path.GetFileName(file) };
                //uploadedFile.Size = fileInfo.Length;
                //uploadedFile.Path = ("~/UploadedFiles/") + Path.GetFileName(file);
                uploadedFiles.Add(Path.GetFileName(file));
            }
            if (uploadedFiles.Count > 0)
            {
                foreach (var ten_file_xuly in uploadedFiles)
                {
                    // bước 1: giải nén file ZIP
                    var zipfile = Server.MapPath("~/SOHOA_TEMP/") + ten_file_xuly;
                    using (var zip = ZipFile.Read(zipfile))
                    {
                        zip.Password = "addj123456";
                        zip.ExtractProgress += ExtractProgress;
                        foreach (var ee in zip)
                        {
                            ee.Extract(Server.MapPath("~/SOHOA_TEMP/TEMP_ZIP"), ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                    // bước 2: lấy danh sách các file EXCEL trong thư mục ZIP đã giải nén
                    var danhSachFileExcelXuLy = new List<string>();
                    var lstDSFileExcel = Directory.GetFiles(Server.MapPath("~/SOHOA_TEMP/TEMP_ZIP"));
                    if (danhSachFileExcelXuLy.Count>0)
                    {
                        // bước 3: thực hiện xử lý lần lượt
                        foreach (var ten_file_excel in danhSachFileExcelXuLy)
                        {
                            // 3.1 duyệt lần lượt các file


                            


                        }

                        // bước 4: xóa thư mục tạm, ZIP


                        // bước 5: kết thúc
                    }
                }
            }
        }




        private void xuLyExcelMucLucHoSo(List<string> duongdan)
        {
            bool khong_hoi_lai = false; // dùng để nhập cấu hình không cần hỏi lại nếu chọn
            string ten_file_dang_xu_ly = "";
            string mlhs_hien_dang_xuly = "";
            Cell test = null;
            string i_j = "";
            try
            {
                // xác định thông tin các cột trước
                string HOPSO_CHA_CONFIG = "";             // key=1
                string HOSO_SO_CHA_CONFIG = "";           // key=2
                string TRICHYEUNOIDUNG_CHA_CONFIG = "";   // key=3
                string SOTO_CHA_CONFIG = "";              // key=4
                string THOIGIAN_CHA_CONFIG = "";          // key=5
                string NAM_CHA_CONFIG = "";               // key=6
                string THOIHANBAOQUAN_CHA_CONFIG = "";    // key=7
                string GHICHU_CHA_CONFIG = "";            // key=8
                string BOSUNG1_CHA_CONFIG = "";           // key=9
                string BOSUNG2_CHA_CONFIG = "";           // key=10
                string SOKYHIEUVB_CHA_CONFIG = "";        // key=11
                string PHONGDOI_CHA_CONFIG = "";          // key=12
                string MLHS_CHA_CONFIG = "";              // key=13
                string MST_CHA_CONFIG = "";               // key=14
                string DOMATKHAN_CHA_CONFIG = "";         // key=15
                string MATOKHAI_CHA_CONFIG = "";          // key=16
                string DIACHI_CHA_CONFIG = "";            // key=17
                string SOGIAYCN_CHA_CONFIG = "";          // key=18
                string STT_CHA_CONFIG = "";               // key=19
                string MADULIEU_CHA_CONFIG = "";
                string MAHOP_CHA_CONFIG = "";
                string MAHOSO_CHA_CONFIG = "";
                string MAVANBAN_CHA_CONFIG = "";
                string URL_CHA_CONFIG = "";
                string NGUOILAP_HS_CHA_CONFIG = "";       // key=20
                string TUYCHON1_CHA_CONFIG = "";          // key=21
                string TUYCHON2_CHA_CONFIG = "";          // key=22
                string TUYCHON3_CHA_CONFIG = "";          // key=23
                string SOHOSO_TAM_CHA_CONFIG = "";        // key=24
                string MAHOSO_TAM_CHA_CONFIG = "";
                string TUYCHON4_CHA_CONFIG = "";          // key=25
                string TUYCHON5_CHA_CONFIG = "";          // key=26
                string GIA_CHA_CONFIG = "";               // key=27
                string DAY_CHA_CONFIG = "";               // key=28
                string KHOANG_CHA_CONFIG = "";            // key=29
                string TANG_CHA_CONFIG = "";              // key=30

                string TRICHYEUNOIDUNG_SOHOA_CHA_CONFIG = "";   // key=99

                // tổng số tầng ở một dãy
                int tong_so_day = 0;
                int tong_so_khoang = 0;
                int tong_so_tang = 0;
                int tong_so_hop_mot_o = 0;
                int tong_so_o_mot_day = tong_so_khoang * tong_so_tang;
                int o_xu_ly = 1;
                int hop_batdau_xuly = 0;
                int tang_xuly = tong_so_tang;  // bắt đầu xử lý từ tầng cao nhất
                int khoang_xuly = 1;
                int day_batdau_xuly = 1;
                int chi_muc_xu_ly_day = 0; // chỉ mục xử lý dãy, mỗi dãy có 2 phía trái và phải

                int gia_bat_dau = 0;
                int day_bat_dau = 0;
                int tang_bat_dau = 0;
                int khoang_bat_dau = 0;

                day_batdau_xuly = day_bat_dau;
                tang_xuly = tang_bat_dau;
                khoang_xuly = khoang_bat_dau;
                int phia_bat_dau = 0;

                bool bien_xac_nhan_hoso_moi = false;
                string TRICHYEUNOIDUNG_SOHOA_XACNHAN = "";

                bool isSuccess = true; // đánh dấu xử lý thành công, xong
                object cellvalue = null;

                // tổng số cột xử lý excel
                int tongSoCotXuLy = 0;
                Dictionary<string, string> vitricotXuLy = new Dictionary<string, string>();

                string[] keys = new string[] { "vĩnh viễn", "lâu dài", "tạm thời", "có thời hạn" };

                // Xử lý từng file Excel đã được lựa chọn trước đó...
                int duongdanXuLy = 0;
                // Khởi tạo giá trị cho năm trước, hệ thống sẽ lấy năm xử lý hồ sơ để chọn làm năm 
                // lưu thông tin nếu mẫu hồ sơ ban đầu chưa chỉ định năm, dạng năm: [năm xử lý xxxx].
                // Bộ phận/thời gian
                var nam = "Năm xử lý " + DateTime.Now.Year;
                foreach (var item in duongdan)
                {
                    try
                    {
                        int soluongsheet;
                        Workbook wb;
                        var dinhDangFileExcel = Path.GetExtension(item);
                        if (dinhDangFileExcel != ".xls" && dinhDangFileExcel != ".xlsx")
                        {
                            continue; // Tiếp tục duyệt tiếp file nếu không phải là excel
                        }
                        else
                        {
                            //Open your template file.
                            wb = new Workbook(item);
                            soluongsheet = wb.Worksheets.Count;
                        }

                        for (int ik = 0; ik < soluongsheet; ik++)
                        {
                            // reset mỗi lần đẩy mới file
                            HOPSO_CHA_CONFIG = "";             // key=1
                            HOSO_SO_CHA_CONFIG = "";           // key=2
                            TRICHYEUNOIDUNG_CHA_CONFIG = "";   // key=3
                            SOTO_CHA_CONFIG = "";              // key=4
                            THOIGIAN_CHA_CONFIG = "";          // key=5
                            NAM_CHA_CONFIG = "";               // key=6
                            THOIHANBAOQUAN_CHA_CONFIG = "";    // key=7
                            GHICHU_CHA_CONFIG = "";            // key=8
                            BOSUNG1_CHA_CONFIG = "";           // key=9
                            BOSUNG2_CHA_CONFIG = "";           // key=10
                            SOKYHIEUVB_CHA_CONFIG = "";        // key=11
                            PHONGDOI_CHA_CONFIG = "";          // key=12
                            MLHS_CHA_CONFIG = "";              // key=13
                            MST_CHA_CONFIG = "";               // key=14
                            DOMATKHAN_CHA_CONFIG = "";         // key=15
                            MATOKHAI_CHA_CONFIG = "";          // key=16
                            DIACHI_CHA_CONFIG = "";            // key=17
                            SOGIAYCN_CHA_CONFIG = "";          // key=18
                            STT_CHA_CONFIG = "";               // key=19
                            MADULIEU_CHA_CONFIG = "";
                            MAHOP_CHA_CONFIG = "";
                            MAHOSO_CHA_CONFIG = "";
                            MAVANBAN_CHA_CONFIG = "";
                            URL_CHA_CONFIG = "";
                            NGUOILAP_HS_CHA_CONFIG = "";       // key=20
                            TUYCHON1_CHA_CONFIG = "";          // key=21
                            TUYCHON2_CHA_CONFIG = "";          // key=22
                            TUYCHON3_CHA_CONFIG = "";          // key=23
                            SOHOSO_TAM_CHA_CONFIG = "";        // key=24
                            MAHOSO_TAM_CHA_CONFIG = "";
                            TUYCHON4_CHA_CONFIG = "";          // key=25
                            TUYCHON5_CHA_CONFIG = "";          // key=26
                            GIA_CHA_CONFIG = "";               // key=27
                            DAY_CHA_CONFIG = "";               // key=28
                            KHOANG_CHA_CONFIG = "";            // key=29
                            TANG_CHA_CONFIG = "";              // key=30

                            TRICHYEUNOIDUNG_SOHOA_CHA_CONFIG = ""; // key=99

                            // xử lý dữ liệu theo từng sheet ở đây
                            //this.Invoke(new Action(() => frm.setSheetName = "Đang xử lý bảng tính thứ " + (ik + 1) + ". Tên: " + (wb.Worksheets[ik].Name)));

                            // Xử lý hàng đầu tiên trong Excel, để xét xem cụ thể Excel đó có những cột nào
                            // Giá trị cần xét đầu tiên thuộc hàng 1 và cụ thể là: A1 đến N1 (tối đa là có 14 cột)

                            // BƯỚC 1: Đếm số cột trong file Excel (cột này là cột hiển thị trong file excel nguồn)
                            List<string> soCotTrongFileExcel = new List<string>();

                            Worksheet worksheet = wb.Worksheets[ik];
                            var tongSoDuLieu = worksheet.Cells.MaxDataRow;

                            //Get the cells collection.
                            Cells cells = worksheet.Cells;
                            //Define the list.
                            List<string> myList = new List<string>();
                            //Get the AA column index. (Since "Status" is always @ AA column.
                            int col = CellsHelper.ColumnNameToIndex("X");

                            //Get the last row index in AA column.
                            int last_row = worksheet.Cells.GetLastDataRow(col);

                            // Định nghĩa vị trí cột trong file excel: Lưu mã - Vị trí
                            Dictionary<string, string> vitricot = new Dictionary<string, string>();

                            int soCotXuLy = 0;
                            int viTriCot = 0;
                            for (var i = 0; i < 1; i++) // Xử lý dòng 1 để lấy mã cột
                            {
                                for (var j = 0; j <= col; j++)
                                {
                                    viTriCot++;
                                    if (cells[i, j].Value == null)
                                    {
                                        soCotTrongFileExcel.Add("");
                                        viTriCot = viTriCot + 1;
                                        continue;
                                    }
                                    else
                                    {
                                        test = cells[i, j];
                                        i_j = i + "___" + j;
                                        cellvalue = cells[i, j].Value;

                                        // nếu giá trị không là số (chứng tỏ file đầu vào không đúng, hoặc lỗi nhiều sheet mà thực tế không thấy...)
                                        // thì bỏ qua không xử lý file hoặc sheet đó
                                        if (!Int32.TryParse(cells[i, j].Value.ToString(), out viTriCot))
                                            continue;
                                        //viTriCot = Convert.ToInt32(cells[i, j].Value);
                                    }
                                    soCotXuLy++;
                                    soCotTrongFileExcel.Add(cells[i, j].Value.ToString());
                                    vitricot.Add(j.ToString(), viTriCot.ToString());
                                }
                            }
                            // lấy thông tin cột xử lý để tạo index sau
                            if (tongSoCotXuLy == 0)
                                tongSoCotXuLy = soCotXuLy;
                            if (vitricotXuLy.Count == 0)
                                vitricotXuLy = vitricot;
                            
                            //=====================================================================
                            // chỉ xử lý nếu trong file Excel có dữ liệu
                            if (soCotXuLy > 0)
                            {
                                nhanBietCotExcel_MLHS.Add(soCotXuLy.ToString());
                            }
                            else
                            {
                                continue;
                            }
                            // xác định thông tin các cột trước
                            string HOPSO_SAPXEP_TUDONG = "";             // đánh dấu sự thay đổi của hộp để sắp xếp số hộp trong 1 khoang
                            string HOPSO = "";             // key=1
                            string HOSO_SO = "";           // key=2
                            string TRICHYEUNOIDUNG = "";   // key=3
                            string SOTO = "";              // key=4
                            string THOIGIAN = "";          // key=5
                            string NAM = "";               // key=6
                            string THOIHANBAOQUAN = "";    // key=7
                            string GHICHU = "";            // key=8
                            string BOSUNG1 = "";           // key=9
                            string BOSUNG2 = "";           // key=10
                            string SOKYHIEUVB = "";        // key=11
                            string PHONGDOI = "";          // key=12
                            string MLHS = "";              // key=13
                            string MST = "";               // key=14
                            string DOMATKHAN = "";         // key=15
                            string MATOKHAI = "";          // key=16
                            string DIACHI = "";            // key=17
                            string SOGIAYCN = "";          // key=18
                            string STT = "";               // key=19
                            string MADULIEU = "";
                            string MAHOP = "";
                            string MAHOSO = "";
                            string MAVANBAN = "";
                            string URL = "";
                            string NGUOILAP_HS = "";        // key=20
                            string TUYCHON1 = "";           // key=21
                            string TUYCHON2 = "";           // key=22
                            string TUYCHON3 = "";           // key=23
                            string SOHOSO_TAM = "";         // key=24
                            string MAHOSO_TAM = "";
                            string FULL_SEARCH = "";
                            string TUYCHON4 = "";           // key=25
                            string TUYCHON5 = "";           // key=26
                            string GIA = "";                // key=27
                            string DAY = "";                // key=28
                            string KHOANG = "";             // key=29
                            string TANG = "";               // key=30
                            string VITRI = "";              // key=31 (1: trái; 0: phải)

                            string TRICHYEUNOIDUNG_SOHOA = ""; // key=99 : thông tin số hóa

                            // Xử lý cho dữ liệu cha

                            string GUID_MLHS = "";
                            string TRICHYEUNOIDUNG_MLHS = "";
                            string HOPSO_CHA = "";
                            string HOSO_SO_CHA = "";
                            int STT_CHA = 0;
                            int STT_CON = 0;  // STT của mục lục văn bản
                            string THOIHANBAOQUAN_CHA = "";
                            string PHONGDOI_CHA = "";
                            string MLHS_CHA = "";
                            string SOHOSO_TAM_CHA = "";
                            // Xử lý cho tìm kiếm FULL_SEARCH
                            string TRICHYEUNOIDUNG_MLHS_FULLSEARCH = "";

                            int commit_theo_loat = 1;
                            int tong_so_luot_commit = tongSoDuLieu / 5000;
                           
                            for (int i = 1; i <= tongSoDuLieu; i++) // xử lý từ dòng 1 theo số -> mã
                            {
                                List<string> strArrayValue = new List<string>();
                                int soCotXuLy1 = 0;
                                for (var j1 = 0; j1 <= col; j1++)
                                {
                                    if (cells[i, j1].Value == null)
                                    {
                                        strArrayValue.Add("");
                                        continue;
                                    }
                                    soCotXuLy1++;
                                    strArrayValue.Add(cells[i, j1].Value.ToString());
                                }
                                // Reset số liệu trước khi lấy
                                HOPSO = "";
                                HOSO_SO = "";
                                TRICHYEUNOIDUNG = "";
                                TRICHYEUNOIDUNG_MLHS_FULLSEARCH = "";
                                TRICHYEUNOIDUNG_SOHOA = "";
                                SOTO = "";
                                THOIGIAN = "";
                                NAM = "";
                                THOIHANBAOQUAN = "";
                                GHICHU = "";
                                BOSUNG1 = "";
                                BOSUNG2 = "";
                                SOKYHIEUVB = "";
                                PHONGDOI = "";
                                MLHS = "";
                                MST = "";
                                DOMATKHAN = "";
                                MATOKHAI = "";
                                DIACHI = "";
                                SOGIAYCN = "";
                                STT = "";
                                MADULIEU = "";
                                MAHOP = "";
                                MAHOSO = "";
                                MAVANBAN = "";
                                URL = "";
                                NGUOILAP_HS = "";
                                TUYCHON1 = "";
                                TUYCHON2 = "";
                                TUYCHON3 = "";
                                SOHOSO_TAM = "";
                                MAHOSO_TAM = "";
                                TUYCHON4 = "";
                                TUYCHON5 = "";
                                GIA = "";
                                DAY = "";
                                KHOANG = "";
                                TANG = "";

                                for (int ikk = 0; ikk < strArrayValue.Count; ikk++)
                                {
                                    if (vitricot.ContainsKey((ikk).ToString()))
                                    {
                                        // xử lý đúng cột
                                        switch (vitricot[(ikk).ToString()])
                                        {
                                            case "1":
                                                HOPSO = strArrayValue[ikk].Trim();
                                                break;
                                            case "2":
                                                HOSO_SO = strArrayValue[ikk].Trim();
                                                break;
                                            case "3":
                                                TRICHYEUNOIDUNG = strArrayValue[ikk].Trim();
                                                break;
                                            case "4":
                                                SOTO = strArrayValue[ikk].Trim();
                                                break;
                                            case "5":
                                                THOIGIAN = strArrayValue[ikk].Contains("12:00:00 AM") ||
                                                           strArrayValue[ikk].Contains("Jan") ||
                                                           strArrayValue[ikk].Contains("Fed") ||
                                                           strArrayValue[ikk].Contains("Mar") ||
                                                           strArrayValue[ikk].Contains("Apr") ||
                                                           strArrayValue[ikk].Contains("May") ||
                                                           strArrayValue[ikk].Contains("Jun") ||
                                                           strArrayValue[ikk].Contains("Jul") ||
                                                           strArrayValue[ikk].Contains("Aug") ||
                                                           strArrayValue[ikk].Contains("Sep") ||
                                                           strArrayValue[ikk].Contains("Oct") ||
                                                           strArrayValue[ikk].Contains("Nov") ||
                                                           strArrayValue[ikk].Contains("Dec") ? Convert.ToDateTime(strArrayValue[ikk].Trim()).ToString("dd/MM/yyyy") : strArrayValue[ikk].Trim();
                                                break;
                                            case "6":
                                                NAM = strArrayValue[ikk].Trim();
                                                break;
                                            case "7":
                                                THOIHANBAOQUAN = strArrayValue[ikk].Trim();
                                                break;
                                            case "8":
                                                GHICHU = strArrayValue[ikk].Trim();
                                                break;
                                            case "9":
                                                BOSUNG1 = strArrayValue[ikk].Trim();
                                                break;
                                            case "10":
                                                BOSUNG2 = strArrayValue[ikk].Trim();
                                                break;
                                            case "11":
                                                SOKYHIEUVB = strArrayValue[ikk].Trim();
                                                break;
                                            case "12":
                                                PHONGDOI = strArrayValue[ikk].Trim();
                                                break;
                                            case "13":
                                                MLHS = strArrayValue[ikk].Trim();
                                                break;
                                            case "14":
                                                MST = strArrayValue[ikk].Trim();
                                                break;
                                            case "15":
                                                DOMATKHAN = strArrayValue[ikk].Trim();
                                                break;
                                            case "16":
                                                MATOKHAI = strArrayValue[ikk].Trim();
                                                break;
                                            case "17":
                                                DIACHI = strArrayValue[ikk].Trim();
                                                break;
                                            case "18":
                                                SOGIAYCN = strArrayValue[ikk].Trim();
                                                break;
                                            case "19":
                                                STT = strArrayValue[ikk].Trim();
                                                break;
                                            case "20":
                                                NGUOILAP_HS = strArrayValue[ikk];
                                                break;
                                            case "21":
                                                TUYCHON1 = strArrayValue[ikk];
                                                break;
                                            case "22":
                                                TUYCHON2 = strArrayValue[ikk];
                                                break;
                                            case "23":
                                                TUYCHON3 = strArrayValue[ikk];
                                                break;
                                            case "24":
                                                SOHOSO_TAM = strArrayValue[ikk];
                                                break;
                                            case "25":
                                                TUYCHON4 = strArrayValue[ikk];
                                                break;
                                            case "26":
                                                TUYCHON5 = strArrayValue[ikk];
                                                break;
                                            case "27":
                                                GIA = strArrayValue[ikk];
                                                break;
                                            case "28":
                                                DAY = strArrayValue[ikk];
                                                break;
                                            case "29":
                                                KHOANG = strArrayValue[ikk];
                                                break;
                                            case "30":
                                                TANG = strArrayValue[ikk];
                                                break;
                                            case "99":
                                                TRICHYEUNOIDUNG_SOHOA = strArrayValue[ikk];
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // nếu cha có, thì lấy theo cha
                                if (HOPSO != "")
                                {
                                    HOPSO_CHA = HOPSO;
                                    if (chkThemHHS.Checked.Value)
                                    {
                                        HOPSO_CHA = HOPSO.Replace("H", "");
                                    }
                                }
                                if (HOSO_SO != "")
                                {
                                    HOSO_SO_CHA = HOSO_SO;
                                    STT_CHA = 0;
                                    STT_CON = 0;
                                    if (chkThemHHS.Checked.Value)
                                    {
                                        HOSO_SO_CHA = HOSO_SO.Replace("HS", "");
                                    }
                                }

                                if (SOHOSO_TAM != "")
                                {
                                    SOHOSO_TAM_CHA = SOHOSO_TAM;
                                }

                                //STT_CHA++;  // số thứ tự MLVB của hồ sơ
                                if (THOIHANBAOQUAN != "")
                                {
                                    THOIHANBAOQUAN_CHA = THOIHANBAOQUAN;
                                }
                                if (PHONGDOI != "")
                                {
                                    PHONGDOI_CHA = PHONGDOI;
                                }
                                if (MLHS != "")
                                {
                                    MLHS_CHA = MLHS;
                                }

                                // nếu không có thông tin hộp số và hồ sơ số thì không xử lý
                                if (HOPSO_CHA.Trim() == "" && HOSO_SO_CHA.Trim() == "")
                                {
                                    continue;
                                }

                                // Xử lý bộ mã quy ước
                                // mã đơn vị
                                var madv = "A";
                                // mã phông
                                var maphong = "A2";
                                // năm chỉnh lý
                                mlhs_hien_dang_xuly = MLHS_CHA;
                                var namchinhly = MLHS_CHA.Substring(4, 2); // "18";
                                // mục lục HS
                                var mlhs = "";

                                string sKeyResult = keys.FirstOrDefault<string>(s => MLHS_CHA.ToLower().Contains(s));
                                switch (sKeyResult)
                                {
                                    case "vĩnh viễn":
                                        mlhs = "VV";
                                        break;
                                    case "lâu dài":
                                        mlhs = "LD";
                                        break;
                                    case "tạm thời":
                                        mlhs = "TT";
                                        break;
                                    case "có thời hạn":
                                        mlhs = "CH";
                                        break;
                                    default:
                                        mlhs = "KH";
                                        break;
                                }

                                // số hộp
                                var sohop = HOPSO_CHA.PadLeft(8, '0'); // "00000012";
                                // số hồ sơ
                                var sohoso = HOSO_SO_CHA.PadLeft(8, '0');// "0000013A";
                                // số hồ sơ tạm
                                var sohoso_tam = SOHOSO_TAM_CHA.PadLeft(8, '0');// "0000013A";
                                // số thứ tự VB
                                var stt = String.Format("{0:x2}", STT_CHA); // "01";
                                // mã dữ liệu
                                var madulieu = "#" + madv + maphong + namchinhly + mlhs + sohop + sohoso + stt;
                                MADULIEU = madulieu.ToUpper();
                                // mã hộp
                                var mahop = "#" + madv + maphong + namchinhly + mlhs + "1" + sohop;
                                MAHOP = mahop.ToUpper();
                                // mã hồ sơ
                                var mahoso = "#" + madv + maphong + namchinhly + mlhs + "2" + sohoso;
                                MAHOSO = mahoso.ToUpper();

                                // mã hồ sơ tạm
                                var mahosotam = "#" + madv + maphong + namchinhly + mlhs + "2" + sohoso_tam;
                                MAHOSO_TAM = mahosotam.ToUpper();

                                // mã văn bản
                                var mavanban = "#" + madv + maphong + namchinhly + mlhs + "3" + sohoso + stt;
                                MAVANBAN = mavanban.ToUpper();

                                if (SOTO == "" && HOSO_SO == "")  // nếu cả số tờ và hồ sơ k có thì k đẩy vào
                                {
                                    continue;
                                }

                                bien_xac_nhan_hoso_moi = false;
                                if (TRICHYEUNOIDUNG_SOHOA_XACNHAN != TRICHYEUNOIDUNG_SOHOA)
                                {
                                    TRICHYEUNOIDUNG_SOHOA_XACNHAN = TRICHYEUNOIDUNG_SOHOA;
                                    bien_xac_nhan_hoso_moi = true;
                                    GUID_MLHS = Guid.NewGuid().ToString();  // GUID của MLHS
                                }

                                // ==================================================================
                                string full_text_search_mlvb = (HOPSO_CHA + " " + HOSO_SO_CHA + " " + TRICHYEUNOIDUNG_MLHS_FULLSEARCH + " " + TRICHYEUNOIDUNG + " " + SOTO + " " + THOIGIAN + " " + NAM + " " + THOIHANBAOQUAN_CHA + " " + GHICHU + " " + BOSUNG1 + " " + BOSUNG2 + " " + SOKYHIEUVB + " " + PHONGDOI_CHA + " " + MLHS_CHA + " " + MST + " " + DOMATKHAN + " " + MATOKHAI + " " + DIACHI + " " + SOGIAYCN + " " + STT + " " + MADULIEU + " " + MAHOP + " " + MAHOSO + " " + MAVANBAN + " " + NGUOILAP_HS + " " + TUYCHON1 + " " + TUYCHON2 + " " + TUYCHON3 + " " + SOHOSO_TAM_CHA + " " + MAHOSO_TAM).ToLower();
                                string full_text_search = (HOPSO_CHA + " " + HOSO_SO_CHA + " " + TRICHYEUNOIDUNG + " " + SOTO + " " + THOIGIAN + " " + NAM + " " + THOIHANBAOQUAN_CHA + " " + GHICHU + " " + BOSUNG1 + " " + BOSUNG2 + " " + SOKYHIEUVB + " " + PHONGDOI_CHA + " " + MLHS_CHA + " " + MST + " " + DOMATKHAN + " " + MATOKHAI + " " + DIACHI + " " + SOGIAYCN + " " + STT + " " + MADULIEU + " " + MAHOP + " " + MAHOSO + " " + MAVANBAN + " " + NGUOILAP_HS + " " + TUYCHON1 + " " + TUYCHON2 + " " + TUYCHON3 + " " + SOHOSO_TAM_CHA + " " + MAHOSO_TAM).ToLower();

                                string URL_PATH = GetSafeFilename(tenHoSo) + "\\" + HOPSO + "\\" + HOSO_SO + "\\" + STT + ".pdf";

                                if (HOPSO != "" && HOSO_SO != "" && bien_xac_nhan_hoso_moi)
                                {
                                    if (HOPSO_SAPXEP_TUDONG != HOPSO_CHA)
                                    {
                                        //VITRI = "";
                                        //GIA = "";
                                        //DAY = "";
                                        //KHOANG = "";
                                        //TANG = "";
                                        HOPSO_SAPXEP_TUDONG = HOPSO_CHA;
                                        // xử lý tự động vị trí sắp xếp hộp
                                    }

                                    //GUID_MLHS = Guid.NewGuid().ToString();  // GUID của MLHS
                                    TRICHYEUNOIDUNG_MLHS = TRICHYEUNOIDUNG_SOHOA_XACNHAN; // nội dung thông tin MLHS
                                    TRICHYEUNOIDUNG_MLHS_FULLSEARCH = TRICHYEUNOIDUNG_SOHOA_XACNHAN;
                                    // Chèn vào cơ sở dữ liệu MLHS

                                    using (var ctx = new ADDJContext())
                                    {
                                        ctx.Database.ExecuteSqlCommand("insert into MUCLUCHOSO (" +
                                                      "GUID," +
                                                      "HOPSO," +
                                                      "HOSO_SO," +
                                                      "TRICHYEUNOIDUNG," +
                                                      "SOTO," +
                                                      "THOIGIAN," +
                                                      "NAM," +
                                                      "THOIHANBAOQUAN," +
                                                      "GHICHU," +
                                                      "BOSUNG1," +
                                                      "BOSUNG2," +
                                                      "SOKYHIEUVB," +
                                                      "PHONGDOI," +
                                                      "MLHS," +
                                                      "MST," +
                                                      "DOMATKHAN," +
                                                      "MATOKHAI," +
                                                      "DIACHI," +
                                                      "SOGIAYCN," +
                                                      "STT," +
                                                      "MADULIEU," +
                                                      "MAHOP," +
                                                      "MAHOSO," +
                                                      "MAVANBAN," +
                                                      "URL," +
                                                      "NGUOILAP_HS," +
                                                      "TUYCHON1," +
                                                      "TUYCHON2," +
                                                      "TUYCHON3," +
                                                      "SOHOSO_TAM," +
                                                      "MAHOSO_TAM," +
                                                      "FULL_SEARCH," +
                                                      "TUYCHON4," +
                                                      "TUYCHON5," +
                                                      "GIA," +
                                                      "DAY," +
                                                      "KHOANG," +
                                                      "TANG," +
                                                      "VITRI" +
                                                      ")  " +
                                                      "values (" +
                                                      "@guid," +
                                                      "@hopso," +
                                                      "@hoso_so," +
                                                      "@trichyeunoidung," +
                                                      "@soto," +
                                                      "@thoigian," +
                                                      "@nam," +
                                                      "@thoihanbaoquan," +
                                                      "@ghichu," +
                                                      "@bosung1," +
                                                      "@bosung2," +
                                                      "@sokyhieuvb," +
                                                      "@phongdoi," +
                                                      "@mlhs," +
                                                      "@mst," +
                                                      "@domatkhan," +
                                                      "@matokhai," +
                                                      "@diachi," +
                                                      "@sogiaycn," +
                                                      "@stt," +
                                                      "@madulieu," +
                                                      "@mahop," +
                                                      "@mahoso," +
                                                      "@mavanban," +
                                                      "@url," +
                                                      "@nguoilap_hs," +
                                                      "@tuychon1," +
                                                      "@tuychon2," +
                                                      "@tuychon3," +
                                                      "@sohoso_tam," +
                                                      "@mahoso_tam," +
                                                      "@full_search," +
                                                      "@tuychon4," +
                                                      "@tuychon5," +
                                                      "@gia," +
                                                      "@day," +
                                                      "@khoang," +
                                                      "@tang," +
                                                      "@vitri" +
                                                      ")",
                                                      new SqlParameter("@guid", GUID_MLHS),
                                                      new SqlParameter("@hopso", HOPSO_CHA),
                                                      new SqlParameter("@hoso_so", HOSO_SO_CHA),
                                                      new SqlParameter("@trichyeunoidung", TRICHYEUNOIDUNG_SOHOA_XACNHAN),
                                                      new SqlParameter("@soto", SOTO),
                                                      new SqlParameter("@thoigian", THOIGIAN),
                                                      new SqlParameter("@nam", NAM),
                                                      new SqlParameter("@thoihanbaoquan", THOIHANBAOQUAN_CHA),
                                                      new SqlParameter("@ghichu", GHICHU),
                                                      new SqlParameter("@bosung1", BOSUNG1),
                                                      new SqlParameter("@bosung2", BOSUNG2),
                                                      new SqlParameter("@sokyhieuvb", SOKYHIEUVB),
                                                      new SqlParameter("@phongdoi", PHONGDOI_CHA),
                                                      new SqlParameter("@mlhs", MLHS_CHA),
                                                      new SqlParameter("@mst", MST),
                                                      new SqlParameter("@domatkhan", DOMATKHAN),
                                                      new SqlParameter("@matokhai", MATOKHAI),
                                                      new SqlParameter("@diachi", DIACHI),
                                                      new SqlParameter("@sogiaycn", SOGIAYCN),
                                                      new SqlParameter("@stt", STT),
                                                      new SqlParameter("@madulieu", MADULIEU),
                                                      new SqlParameter("@mahop", MAHOP),
                                                      new SqlParameter("@mahoso", MAHOSO),
                                                      new SqlParameter("@mavanban", MAVANBAN),
                                                      new SqlParameter("@url", URL_PATH),
                                                      new SqlParameter("@nguoilap_hs", NGUOILAP_HS),
                                                      new SqlParameter("@tuychon1", TUYCHON1),
                                                      new SqlParameter("@tuychon2", TUYCHON2),
                                                      new SqlParameter("@tuychon3", TUYCHON3),
                                                      new SqlParameter("@sohoso_tam", SOHOSO_TAM_CHA),
                                                      new SqlParameter("@mahoso_tam", MAHOSO_TAM),
                                                      new SqlParameter("@full_search", full_text_search),
                                                      new SqlParameter("@tuychon4", TUYCHON4),
                                                      new SqlParameter("@tuychon5", TUYCHON5),
                                                      new SqlParameter("@gia", GIA_CHA_CONFIG),
                                                      new SqlParameter("@day", DAY_CHA_CONFIG),
                                                      new SqlParameter("@khoang", KHOANG_CHA_CONFIG),
                                                      new SqlParameter("@tang", TANG_CHA_CONFIG),
                                                      new SqlParameter("@vitri", VITRI));

                                        // bổ sung xử lý cho bản 5.3 MLHS
                                        ctx.Database.ExecuteSqlCommand("insert into MUCLUCHOSO_MLVB_53 (" +
                                                      "GUID," +
                                                      "GUID_MLHS," +
                                                      "HOPSO," +
                                                      "HOSO_SO," +
                                                      "TRICHYEUNOIDUNG_MLHS," +
                                                      "TRICHYEUNOIDUNG," +
                                                      "SOTO," +
                                                      "THOIGIAN," +
                                                      "NAM," +
                                                      "THOIHANBAOQUAN," +
                                                      "GHICHU," +
                                                      "BOSUNG1," +
                                                      "BOSUNG2," +
                                                      "SOKYHIEUVB," +
                                                      "PHONGDOI," +
                                                      "MLHS," +
                                                      "MST," +
                                                      "DOMATKHAN," +
                                                      "MATOKHAI," +
                                                      "DIACHI," +
                                                      "SOGIAYCN," +
                                                      "STT," +
                                                      "MADULIEU," +
                                                      "MAHOP," +
                                                      "MAHOSO," +
                                                      "MAVANBAN," +
                                                      "URL," +
                                                      "NGUOILAP_HS," +
                                                      "TUYCHON1," +
                                                      "TUYCHON2," +
                                                      "TUYCHON3," +
                                                      "SOHOSO_TAM," +
                                                      "MAHOSO_TAM," +
                                                      "FULL_SEARCH," +
                                                      "TUYCHON4," +
                                                      "TUYCHON5," +
                                                      "GIA," +
                                                      "DAY," +
                                                      "KHOANG," +
                                                      "TANG," +
                                                      "VITRI" +
                                                      ")  " +
                                                      "values (" +
                                                      "@guid," +
                                                      "@guid_mlhs," +
                                                      "@hopso," +
                                                      "@hoso_so," +
                                                      "@trichyeunoidung_mlhs," +
                                                      "@trichyeunoidung," +
                                                      "@soto," +
                                                      "@thoigian," +
                                                      "@nam," +
                                                      "@thoihanbaoquan," +
                                                      "@ghichu," +
                                                      "@bosung1," +
                                                      "@bosung2," +
                                                      "@sokyhieuvb," +
                                                      "@phongdoi," +
                                                      "@mlhs," +
                                                      "@mst," +
                                                      "@domatkhan," +
                                                      "@matokhai," +
                                                      "@diachi," +
                                                      "@sogiaycn," +
                                                      "@stt," +
                                                      "@madulieu," +
                                                      "@mahop," +
                                                      "@mahoso," +
                                                      "@mavanban," +
                                                      "@url," +
                                                      "@nguoilap_hs," +
                                                      "@tuychon1," +
                                                      "@tuychon2," +
                                                      "@tuychon3," +
                                                      "@sohoso_tam," +
                                                      "@mahoso_tam," +
                                                      "@full_search," +
                                                      "@tuychon4," +
                                                      "@tuychon5," +
                                                      "@gia," +
                                                      "@day," +
                                                      "@khoang," +
                                                      "@tang," +
                                                      "@vitri" +
                                                      ")",
                                                      new SqlParameter("@guid", GUID_MLHS),
                                                      new SqlParameter("@guid_mlhs", GUID_MLHS),
                                                      new SqlParameter("@hopso", HOPSO_CHA),
                                                      new SqlParameter("@hoso_so", HOSO_SO_CHA),
                                                      new SqlParameter("@trichyeunoidung_mlhs", TRICHYEUNOIDUNG_MLHS),
                                                      new SqlParameter("@trichyeunoidung", ""),
                                                      new SqlParameter("@soto", SOTO),
                                                      new SqlParameter("@thoigian", THOIGIAN),
                                                      new SqlParameter("@nam", NAM),
                                                      new SqlParameter("@thoihanbaoquan", THOIHANBAOQUAN_CHA),
                                                      new SqlParameter("@ghichu", GHICHU),
                                                      new SqlParameter("@bosung1", BOSUNG1),
                                                      new SqlParameter("@bosung2", BOSUNG2),
                                                      new SqlParameter("@sokyhieuvb", SOKYHIEUVB),
                                                      new SqlParameter("@phongdoi", PHONGDOI_CHA),
                                                      new SqlParameter("@mlhs", MLHS_CHA),
                                                      new SqlParameter("@mst", MST),
                                                      new SqlParameter("@domatkhan", DOMATKHAN),
                                                      new SqlParameter("@matokhai", MATOKHAI),
                                                      new SqlParameter("@diachi", DIACHI),
                                                      new SqlParameter("@sogiaycn", SOGIAYCN),
                                                      new SqlParameter("@stt", STT),
                                                      new SqlParameter("@madulieu", MADULIEU),
                                                      new SqlParameter("@mahop", MAHOP),
                                                      new SqlParameter("@mahoso", MAHOSO),
                                                      new SqlParameter("@mavanban", MAVANBAN),
                                                      new SqlParameter("@url", URL_PATH),
                                                      new SqlParameter("@nguoilap_hs", NGUOILAP_HS),
                                                      new SqlParameter("@tuychon1", TUYCHON1),
                                                      new SqlParameter("@tuychon2", TUYCHON2),
                                                      new SqlParameter("@tuychon3", TUYCHON3),
                                                      new SqlParameter("@sohoso_tam", SOHOSO_TAM_CHA),
                                                      new SqlParameter("@mahoso_tam", MAHOSO_TAM),
                                                      new SqlParameter("@full_search", full_text_search),
                                                      new SqlParameter("@tuychon4", TUYCHON4),
                                                      new SqlParameter("@tuychon5", TUYCHON5),
                                                      new SqlParameter("@gia", GIA_CHA_CONFIG),
                                                      new SqlParameter("@day", DAY_CHA_CONFIG),
                                                      new SqlParameter("@khoang", KHOANG_CHA_CONFIG),
                                                      new SqlParameter("@tang", TANG_CHA_CONFIG),
                                                      new SqlParameter("@vitri", VITRI));

                                    }
                                }

                                if (HOPSO != "" && HOSO_SO != "")
                                {
                                    STT_CON++;
                                    stt = String.Format("{0:x2}", STT_CON); // "01";
                                    // mã văn bản
                                    mavanban = "#" + madv + maphong + namchinhly + mlhs + "3" + sohoso + stt;
                                    MAVANBAN = mavanban.ToUpper();
                                    string guid = Guid.NewGuid().ToString();
                                    // Chèn vào cơ sở dữ liệu MLVB



                                    using (var ctx = new ADDJContext())
                                    {
                                        ctx.Database.ExecuteSqlCommand("insert into MUCLUCHOSO_MLVB (" +
                                                      "GUID," +
                                                      "GUID_MLHS," +
                                                      "HOPSO," +
                                                      "HOSO_SO," +
                                                      "TRICHYEUNOIDUNG_MLHS," +
                                                      "TRICHYEUNOIDUNG," +
                                                      "SOTO," +
                                                      "THOIGIAN," +
                                                      "NAM," +
                                                      "THOIHANBAOQUAN," +
                                                      "GHICHU," +
                                                      "BOSUNG1," +
                                                      "BOSUNG2," +
                                                      "SOKYHIEUVB," +
                                                      "PHONGDOI," +
                                                      "MLHS," +
                                                      "MST," +
                                                      "DOMATKHAN," +
                                                      "MATOKHAI," +
                                                      "DIACHI," +
                                                      "SOGIAYCN," +
                                                      "STT," +
                                                      "MADULIEU," +
                                                      "MAHOP," +
                                                      "MAHOSO," +
                                                      "MAVANBAN," +
                                                      "URL," +
                                                      "NGUOILAP_HS," +
                                                      "TUYCHON1," +
                                                      "TUYCHON2," +
                                                      "TUYCHON3," +
                                                      "SOHOSO_TAM," +
                                                      "MAHOSO_TAM," +
                                                      "FULL_SEARCH," +
                                                      "TUYCHON4," +
                                                      "TUYCHON5," +
                                                      "GIA," +
                                                      "DAY," +
                                                      "KHOANG," +
                                                      "TANG," +
                                                      "VITRI" +
                                                      ")  " +
                                                      "values (" +
                                                      "@guid," +
                                                      "@guid_mlhs," +
                                                      "@hopso," +
                                                      "@hoso_so," +
                                                      "@trichyeunoidung," +
                                                      "@soto," +
                                                      "@thoigian," +
                                                      "@nam," +
                                                      "@trichyeunoidung_mlhs," +
                                                      "@trichyeunoidung," +
                                                      "@ghichu," +
                                                      "@bosung1," +
                                                      "@bosung2," +
                                                      "@sokyhieuvb," +
                                                      "@phongdoi," +
                                                      "@mlhs," +
                                                      "@mst," +
                                                      "@domatkhan," +
                                                      "@matokhai," +
                                                      "@diachi," +
                                                      "@sogiaycn," +
                                                      "@stt," +
                                                      "@madulieu," +
                                                      "@mahop," +
                                                      "@mahoso," +
                                                      "@mavanban," +
                                                      "@url," +
                                                      "@nguoilap_hs," +
                                                      "@tuychon1," +
                                                      "@tuychon2," +
                                                      "@tuychon3," +
                                                      "@sohoso_tam," +
                                                      "@mahoso_tam," +
                                                      "@full_search," +
                                                      "@tuychon4," +
                                                      "@tuychon5," +
                                                      "@gia," +
                                                      "@day," +
                                                      "@khoang," +
                                                      "@tang," +
                                                      "@vitri" +
                                                      ")",
                                                      new SqlParameter("@guid", guid),
                                                      new SqlParameter("@guid_mlhs", GUID_MLHS),
                                                      new SqlParameter("@hopso", HOPSO_CHA),
                                                      new SqlParameter("@hoso_so", HOSO_SO_CHA),
                                                      new SqlParameter("@trichyeunoidung_mlhs", TRICHYEUNOIDUNG_SOHOA_XACNHAN),
                                                      new SqlParameter("@trichyeunoidung", TRICHYEUNOIDUNG),
                                                      new SqlParameter("@soto", SOTO),
                                                      new SqlParameter("@thoigian", THOIGIAN),
                                                      new SqlParameter("@nam", NAM),
                                                      new SqlParameter("@thoihanbaoquan", THOIHANBAOQUAN_CHA),
                                                      new SqlParameter("@ghichu", GHICHU),
                                                      new SqlParameter("@bosung1", BOSUNG1),
                                                      new SqlParameter("@bosung2", BOSUNG2),
                                                      new SqlParameter("@sokyhieuvb", SOKYHIEUVB),
                                                      new SqlParameter("@phongdoi", PHONGDOI_CHA),
                                                      new SqlParameter("@mlhs", MLHS_CHA),
                                                      new SqlParameter("@mst", MST),
                                                      new SqlParameter("@domatkhan", DOMATKHAN),
                                                      new SqlParameter("@matokhai", MATOKHAI),
                                                      new SqlParameter("@diachi", DIACHI),
                                                      new SqlParameter("@sogiaycn", SOGIAYCN),
                                                      new SqlParameter("@stt", STT),
                                                      new SqlParameter("@madulieu", MADULIEU),
                                                      new SqlParameter("@mahop", MAHOP),
                                                      new SqlParameter("@mahoso", MAHOSO),
                                                      new SqlParameter("@mavanban", MAVANBAN),
                                                      new SqlParameter("@url", URL_PATH),
                                                      new SqlParameter("@nguoilap_hs", NGUOILAP_HS),
                                                      new SqlParameter("@tuychon1", TUYCHON1),
                                                      new SqlParameter("@tuychon2", TUYCHON2),
                                                      new SqlParameter("@tuychon3", TUYCHON3),
                                                      new SqlParameter("@sohoso_tam", SOHOSO_TAM_CHA),
                                                      new SqlParameter("@mahoso_tam", MAHOSO_TAM),
                                                      new SqlParameter("@full_search", full_text_search),
                                                      new SqlParameter("@tuychon4", TUYCHON4),
                                                      new SqlParameter("@tuychon5", TUYCHON5),
                                                      new SqlParameter("@gia", GIA_CHA_CONFIG),
                                                      new SqlParameter("@day", DAY_CHA_CONFIG),
                                                      new SqlParameter("@khoang", KHOANG_CHA_CONFIG),
                                                      new SqlParameter("@tang", TANG_CHA_CONFIG),
                                                      new SqlParameter("@vitri", VITRI));

                                        // bổ sung xử lý cho bản 5.3 MLHS
                                        ctx.Database.ExecuteSqlCommand("insert into MUCLUCHOSO_MLVB_53 (" +
                                                      "GUID," +
                                                      "GUID_MLHS," +
                                                      "HOPSO," +
                                                      "HOSO_SO," +
                                                      "TRICHYEUNOIDUNG_MLHS," +
                                                      "TRICHYEUNOIDUNG," +
                                                      "SOTO," +
                                                      "THOIGIAN," +
                                                      "NAM," +
                                                      "THOIHANBAOQUAN," +
                                                      "GHICHU," +
                                                      "BOSUNG1," +
                                                      "BOSUNG2," +
                                                      "SOKYHIEUVB," +
                                                      "PHONGDOI," +
                                                      "MLHS," +
                                                      "MST," +
                                                      "DOMATKHAN," +
                                                      "MATOKHAI," +
                                                      "DIACHI," +
                                                      "SOGIAYCN," +
                                                      "STT," +
                                                      "MADULIEU," +
                                                      "MAHOP," +
                                                      "MAHOSO," +
                                                      "MAVANBAN," +
                                                      "URL," +
                                                      "NGUOILAP_HS," +
                                                      "TUYCHON1," +
                                                      "TUYCHON2," +
                                                      "TUYCHON3," +
                                                      "SOHOSO_TAM," +
                                                      "MAHOSO_TAM," +
                                                      "FULL_SEARCH," +
                                                      "TUYCHON4," +
                                                      "TUYCHON5," +
                                                      "GIA," +
                                                      "DAY," +
                                                      "KHOANG," +
                                                      "TANG," +
                                                      "VITRI" +
                                                      ")  " +
                                                      "values (" +
                                                      "@guid," +
                                                      "@guid_mlhs," +
                                                      "@hopso," +
                                                      "@hoso_so," +
                                                      "@trichyeunoidung_mlhs," +
                                                      "@trichyeunoidung," +
                                                      "@soto," +
                                                      "@thoigian," +
                                                      "@nam," +
                                                      "@thoihanbaoquan," +
                                                      "@ghichu," +
                                                      "@bosung1," +
                                                      "@bosung2," +
                                                      "@sokyhieuvb," +
                                                      "@phongdoi," +
                                                      "@mlhs," +
                                                      "@mst," +
                                                      "@domatkhan," +
                                                      "@matokhai," +
                                                      "@diachi," +
                                                      "@sogiaycn," +
                                                      "@stt," +
                                                      "@madulieu," +
                                                      "@mahop," +
                                                      "@mahoso," +
                                                      "@mavanban," +
                                                      "@url," +
                                                      "@nguoilap_hs," +
                                                      "@tuychon1," +
                                                      "@tuychon2," +
                                                      "@tuychon3," +
                                                      "@sohoso_tam," +
                                                      "@mahoso_tam," +
                                                      "@full_search," +
                                                      "@tuychon4," +
                                                      "@tuychon5," +
                                                      "@gia," +
                                                      "@day," +
                                                      "@khoang," +
                                                      "@tang," +
                                                      "@vitri" +
                                                      ")",
                                                      new SqlParameter("@guid", guid),
                                                      new SqlParameter("@guid_mlhs", GUID_MLHS),
                                                      new SqlParameter("@hopso", HOPSO_CHA),
                                                      new SqlParameter("@hoso_so", HOSO_SO_CHA),
                                                      new SqlParameter("@trichyeunoidung_mlhs", TRICHYEUNOIDUNG_SOHOA_XACNHAN),
                                                      new SqlParameter("@trichyeunoidung", TRICHYEUNOIDUNG),
                                                      new SqlParameter("@soto", SOTO),
                                                      new SqlParameter("@thoigian", THOIGIAN),
                                                      new SqlParameter("@nam", NAM),
                                                      new SqlParameter("@thoihanbaoquan", THOIHANBAOQUAN_CHA),
                                                      new SqlParameter("@ghichu", GHICHU),
                                                      new SqlParameter("@bosung1", BOSUNG1),
                                                      new SqlParameter("@bosung2", BOSUNG2),
                                                      new SqlParameter("@sokyhieuvb", SOKYHIEUVB),
                                                      new SqlParameter("@phongdoi", PHONGDOI_CHA),
                                                      new SqlParameter("@mlhs", MLHS_CHA),
                                                      new SqlParameter("@mst", MST),
                                                      new SqlParameter("@domatkhan", DOMATKHAN),
                                                      new SqlParameter("@matokhai", MATOKHAI),
                                                      new SqlParameter("@diachi", DIACHI),
                                                      new SqlParameter("@sogiaycn", SOGIAYCN),
                                                      new SqlParameter("@stt", STT),
                                                      new SqlParameter("@madulieu", MADULIEU),
                                                      new SqlParameter("@mahop", MAHOP),
                                                      new SqlParameter("@mahoso", MAHOSO),
                                                      new SqlParameter("@mavanban", MAVANBAN),
                                                      new SqlParameter("@url", URL_PATH),
                                                      new SqlParameter("@nguoilap_hs", NGUOILAP_HS),
                                                      new SqlParameter("@tuychon1", TUYCHON1),
                                                      new SqlParameter("@tuychon2", TUYCHON2),
                                                      new SqlParameter("@tuychon3", TUYCHON3),
                                                      new SqlParameter("@sohoso_tam", SOHOSO_TAM_CHA),
                                                      new SqlParameter("@mahoso_tam", MAHOSO_TAM),
                                                      new SqlParameter("@full_search", full_text_search),
                                                      new SqlParameter("@tuychon4", TUYCHON4),
                                                      new SqlParameter("@tuychon5", TUYCHON5),
                                                      new SqlParameter("@gia", GIA_CHA_CONFIG),
                                                      new SqlParameter("@day", DAY_CHA_CONFIG),
                                                      new SqlParameter("@khoang", KHOANG_CHA_CONFIG),
                                                      new SqlParameter("@tang", TANG_CHA_CONFIG),
                                                      new SqlParameter("@vitri", VITRI));

                                    }
                                }
                            }
                        }
                        //******************************************************************************************************************************
                    }
                    catch (Exception ex)
                    {
                        var kkk = cellvalue;
                        Cell d = test;
                        isSuccess = false;
                       
                        GC.Collect();
                    }
                    // end process sheet
                    duongdanXuLy++;
                }
            }
            catch (Exception ex)
            { 

            }
        }

        public string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

    }
}