using ADDJ.Admin;
using DevExpress.Utils;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.ADDJ_TH.entity;
using Website.HTHTKT;

namespace Website.ADDJ_TH.views
{
    public partial class WebUserControl1_q10 : System.Web.UI.UserControl
    {
        public int PageSize
        {
            get
            {
                return (base.ViewState["PageSize"] != null) ? (int)base.ViewState["PageSize"] : 10;
            }
            set
            {
                base.ViewState["PageSize"] = value;
            }
        }
        public int CurrentPage
        {
            get
            {
                return (base.ViewState["CurrentPage"] != null) ? (int)base.ViewState["CurrentPage"] : 0;
            }
            set
            {
                base.ViewState["CurrentPage"] = value;
            }
        }
        public int RowCount
        {
            get { return (base.ViewState["RowCount"] != null) ? (int)base.ViewState["RowCount"] : 0; }
            set { base.ViewState["RowCount"] = value; }
        }

        RightInfo quyenNguoiDung;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterRequiresControlState(ASPxPager1);
            if (!Page.IsPostBack)
            {
                // phân quyền người dung
                quyenNguoiDung = UserRightImpl.CheckRightAdminnistrator_NoCache();
                // lấy thông tin của cột thêm sửa xóa
                GridViewCommandColumn column = grvCustomer.Columns[0] as GridViewCommandColumn;
                GridViewCommandColumnCustomButton buttonXem = column.CustomButtons[0] as GridViewCommandColumnCustomButton;
                GridViewCommandColumnCustomButton buttonSua = column.CustomButtons[1] as GridViewCommandColumnCustomButton;
                GridViewCommandColumnCustomButton buttonXoa = column.CustomButtons[2] as GridViewCommandColumnCustomButton;

                if (!quyenNguoiDung.Other1)
                {
                    ASPxHiddenField1["quyenXem"] = 0;
                    buttonXem.Visibility = GridViewCustomButtonVisibility.Invisible;
                }
                else
                {
                    ASPxHiddenField1["quyenXem"] = 1;
                    buttonXem.Visibility = GridViewCustomButtonVisibility.AllDataRows;
                }

                if (!quyenNguoiDung.Other2)
                {
                    ASPxHiddenField1["quyenThem"] = 0;
                    btnThemMoi.Visible = false;
                }
                else
                {
                    ASPxHiddenField1["quyenThem"] = 1;
                    btnThemMoi.Visible = true;
                }

                if (!quyenNguoiDung.Other3)
                {
                    ASPxHiddenField1["quyenIn"] = 0;
                }
                else
                {
                    ASPxHiddenField1["quyenIn"] = 1;
                }

                if (!quyenNguoiDung.Other4)
                {
                    ASPxHiddenField1["quyenTaive"] = 0;
                }
                else
                {
                    ASPxHiddenField1["quyenTaive"] = 1;
                }

                if (!quyenNguoiDung.UserEdit)
                {
                    ASPxHiddenField1["quyenSua"] = 0;
                    buttonSua.Visibility = GridViewCustomButtonVisibility.Invisible;
                }
                else
                {
                    ASPxHiddenField1["quyenSua"] = 1;
                    buttonSua.Visibility = GridViewCustomButtonVisibility.AllDataRows;
                }

                if (!quyenNguoiDung.UserDelete)
                {
                    ASPxHiddenField1["quyenXoa"] = 0;
                    buttonXoa.Visibility = GridViewCustomButtonVisibility.Invisible;
                }
                else
                {
                    ASPxHiddenField1["quyenXoa"] = 1;
                    buttonXoa.Visibility = GridViewCustomButtonVisibility.AllDataRows;
                }


                BindData();
                napDuLieuTimKiem();





                // xử lý tạo key để upload dữ liệu lên server
                SubmissionID = UploadControlHelper.GenerateUploadedFilesStorageKey();
                UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
                System.Web.HttpContext.Current.Session["SubmissionID"] = SubmissionID;
                fileNameAttachYC = new List<string>();
            }
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        private void napDuLieuTimKiem()
        {
            try
            {
                // lấy danh sách hộp số
                var lstHS = new List<hopsoinfo>();
                string sqlHS = "select DISTINCT hopso,CAST(REPLACE(hopso,'H','') AS INT) hs from mucluchoso_mlvb_53 ORDER BY CAST(REPLACE(hopso,'H','') AS INT),hopso";
                /*sqlHS = "select DISTINCT hopso,CAST(REPLACE(TRANSLATE([hopso], 'abcdefghijklmnopqrstuvwxyz+()- ,#+\\/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT) " +
                    "hs from mucluchoso_mlvb_53 ORDER BY CAST(REPLACE(TRANSLATE([hopso], 'abcdefghijklmnopqrstuvwxyz+()- ,#+\\/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT),hopso";*/

                sqlHS = "select DISTINCT hopso" +
                    " from mucluchoso_mlvb_53 ORDER BY hopso";

                using (var ctx = new ADDJContext())
                {
                    lstHS = ctx.Database.SqlQuery<hopsoinfo>(sqlHS).ToList();
                }
                ASPxListBox list = ((ASPxListBox)checkComboBoxHopSo.FindControl("listBoxHopSo"));
                list.DataSource = lstHS;
                list.TextField = "hopso";
                list.ValueField = "hopso";
                list.DataBind();

                //// lấy danh sách hồ sơ số
                var lstHSS = new List<hoso_soinfo>();
                /*string sqlHSS = "select DISTINCT hoso_so,CAST(REPLACE(hoso_so,'HS','') AS INT) hss from mucluchoso ORDER BY CAST(REPLACE(hoso_so,'HS','') AS INT),hoso_so";
                sqlHSS = "select DISTINCT hoso_so,CAST(REPLACE(TRANSLATE([hoso_so], 'abcdefghijklmnopqrstuvwxyz+()- ,#+/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT) hss " +
                    "from mucluchoso_mlvb_53 ORDER BY CAST(REPLACE(TRANSLATE([hoso_so], 'abcdefghijklmnopqrstuvwxyz+()- ,#+/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT),hoso_so";*/

                string sqlHSS = "select DISTINCT hoso_so,CAST(REPLACE(hoso_so,'HS','') AS INT) hss from mucluchoso ORDER BY CAST(REPLACE(hoso_so,'HS','') AS INT),hoso_so";
                /*sqlHSS = "select DISTINCT hoso_so,CAST(REPLACE(TRANSLATE([hoso_so], 'abcdefghijklmnopqrstuvwxyz+()- ,#+\\/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT) hss " +
                    "from mucluchoso_mlvb_53 ORDER BY CAST(REPLACE(TRANSLATE([hoso_so], 'abcdefghijklmnopqrstuvwxyz+()- ,#+\\/', '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'), '@', '') AS INT),hoso_so";*/

                sqlHSS = "select DISTINCT hoso_so " +
                    "from mucluchoso_mlvb_53 ORDER BY hoso_so";
                using (var ctx = new ADDJContext())
                {
                    lstHSS = ctx.Database.SqlQuery<hoso_soinfo>(sqlHSS).ToList();
                }

                var s = checkComboBoxHopSo.FindControl("listBoxHoSoSo");
                ASPxListBox listHSS = ((ASPxListBox)checkComboBoxHoSoSo.FindControl("listBoxHoSoSo"));
                listHSS.DataSource = lstHSS;
                listHSS.TextField = "hoso_so";
                listHSS.ValueField = "hoso_so";
                listHSS.DataBind();


                // lấy danh sách phòng ban
                var lstPB = new List<phongbaninfo>();
                string sqlPB = "select distinct bosung2, phongdoi from mucluchoso";
                using (var ctx = new ADDJContext())
                {
                    lstPB = ctx.Database.SqlQuery<phongbaninfo>(sqlPB).ToList();
                }
                ASPxListBox listPB = ((ASPxListBox)checkComboBoxPhongBan.FindControl("listBoxPhongBan"));
                listPB.DataSource = lstPB;
                listPB.TextField = "phongdoi";
                listPB.ValueField = "phongdoi";
                listPB.DataBind();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string strSQl = "";

        // http://stackoverflow.com/questions/21537511/sql-server-query-with-pagination-and-count
        public void BindData()
        {
            grvCustomer.Columns.Clear();

            strSQl = "";

            int count = 0;
            //DataTable dtSource = DBUtils.GetDataPaging(PageSize, CurrentPage, ref count);
            var lstl = GetDataPaging(PageSize, CurrentPage);
            if (lstl.Count > 0)
            {
                this.RowCount = lstl[0].ToltalRecords.Value;
                ASPxPager1.ItemCount = lstl[0].ToltalRecords.Value;
                ASPxPager1.ItemsPerPage = this.PageSize;
                grvCustomer.DataSource = lstl;
                grvCustomer.DataBind();

            }
            else
            {
                grvCustomer.DataSource = null;
                grvCustomer.DataBind();
            }

        }
        protected void ASPxPager1_PageIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = ASPxPager1.PageIndex;
            BindData();
        }

        private string layFiedbyKey(string key)
        {
            string fiel = "";
            switch (key)
            {
                case "-1":  // nếu chọn tên đơn vị
                    break;
                case "1":
                    fiel = "HOPSO";
                    break;
                case "2":
                    fiel = "HOSO_SO";
                    break;
                case "3":
                    fiel = "TRICHYEUNOIDUNG_MLHS";
                    break;
                case "4":
                    fiel = "SOTO";
                    break;
                case "5":
                    fiel = "THOIGIAN";
                    break;
                case "6":
                    fiel = "NAM";
                    break;
                case "7":
                    fiel = "THOIHANBAOQUAN";
                    break;
                case "8":
                    fiel = "GHICHU";
                    break;
                case "9":
                    fiel = "BOSUNG1";
                    break;
                case "10":
                    fiel = "BOSUNG2";
                    break;
                case "11":
                    fiel = "SOKYHIEUVB";
                    break;
                case "12":
                    fiel = "PHONGDOI";
                    break;
                case "13":
                    fiel = "MLHS";
                    break;
                case "14":
                    fiel = "MST";
                    break;
                case "15":
                    fiel = "DOMATKHAN";
                    break;
                case "16":
                    fiel = "MATOKHAI";
                    break;
                case "17":
                    fiel = "DIACHI";
                    break;
                case "18":
                    fiel = "SOGIAYCN";
                    break;
                case "19":
                    fiel = "STT";
                    break;
                case "20":
                    fiel = "NGUOILAP_HS";
                    break;
                case "21":
                    fiel = "TUYCHON1";
                    break;
                case "22":
                    fiel = "TUYCHON2";
                    break;
                case "23":
                    fiel = "TUYCHON3";
                    break;
                case "24":
                    fiel = "SOHOSO_TAM";
                    break;
                case "25":
                    fiel = "TUYCHON4";
                    break;
                case "26":
                    fiel = "TUYCHON5";
                    break;
                case "27":
                    fiel = "GIA";
                    break;
                case "28":
                    fiel = "DAY";
                    break;
                case "29":
                    fiel = "KHOANG";
                    break;
                case "30":
                    fiel = "TANG";
                    break;
                default: break;
            }
            return fiel;
        }

        public List<mucluchoso> GetDataPaging(int pagesize, int currentpage)
        {
            var lsthh = new List<mucluchoso>();
            try
            {
                using (var ctx = new ADDJContext())
                {
                    //var strSQl = "select * from HT_NODE_LUONG_HOTRO0 where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";

                    var noidung_timkiemchung = txtNoiDungTimKiem.Text.Trim();
                    // các từ khóa nâng cao
                    if (txtNoiDungTimKiem.Text.Trim() == "Nhập nội dung tìm kiếm" || txtNoiDungTimKiem.Text.Trim() == "")
                    {
                        noidung_timkiemchung = "";
                    }
                    var search_com = noidung_timkiemchung.Trim().Replace(";", " ") + " " + checkComboBoxHopSo.Text.Trim().Replace(";", " ") + " " + 
                        checkComboBoxHoSoSo.Text.Trim().Replace(";", " ") + " " + checkComboBoxPhongBan.Text.Trim().Replace(";", " ");
                    string[] keyWordMLVBSearch2;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    var tempo = regex.Replace(search_com.ToLower().Replace(";", ""), " ");
                    keyWordMLVBSearch2 = Regex.Replace(tempo, @"\t|\n|\r", "").Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // xử lý thành các điều kiện like theo từ khóa tìm kiếm
                    string sqlwhere = " where 1=1 ";
                    if (keyWordMLVBSearch2.Length > 0)
                    {
                        for (int i = 0; i < keyWordMLVBSearch2.Length; i++)
                        {
                            sqlwhere = sqlwhere + " and query.full_search LIKE N'%" + keyWordMLVBSearch2[i] + "%'";
                        }
                    }

                    if (checkComboBoxHopSo.Text.Trim() != "")
                    {
                        sqlwhere = sqlwhere + " and hopso in(N'" + string.Join("',N'", checkComboBoxHopSo.Text.Trim().Split(';')) + "') ";
                    }
                    if (checkComboBoxHoSoSo.Text.Trim() != "")
                    {
                        sqlwhere = sqlwhere + " and hoso_so in(N'" + string.Join("',N'", checkComboBoxHoSoSo.Text.Trim().Split(';')) + "') ";
                    }
                    if (checkComboBoxPhongBan.Text.Trim() != "")
                    {
                        sqlwhere = sqlwhere + " and bosung2 in(N'" + string.Join("',N'", checkComboBoxPhongBan.Text.Trim().Split(';')) + "') ";
                    }


                    var commandSQL = string.Format(" ORDER BY CASE WHEN query.FULL_SEARCH = '{0}' THEN 0  ", txtNoiDungTimKiem.Text.Trim()) +
                                            string.Format(" WHEN query.FULL_SEARCH LIKE '{0}%' THEN 1  ", txtNoiDungTimKiem.Text.Trim()) +
                                            string.Format(" WHEN query.FULL_SEARCH LIKE '%{0}%' THEN 2  ", txtNoiDungTimKiem.Text.Trim()) +
                                            string.Format(" WHEN query.FULL_SEARCH LIKE '%{0}' THEN 3  ", txtNoiDungTimKiem.Text.Trim()) +
                                            " ELSE 4  " +
                                            " END, query.ID, query.FULL_SEARCH ASC  ";


                    strSQl = string.Format(@";with query as
                    (
                      select *,ROW_NUMBER() OVER(ORDER BY ID ASC) as line from dbo.mucluchoso_mlvb_53
                    ) 
                    --order by clause is required to use offset-fetch
                    select query.id
                          ,query.guid
                          ,query.hopso
                          ,query.hoso_so
                          ,query.trichyeunoidung
                          ,query.trichyeunoidung_mlhs
                          ,query.soto
                          ,query.thoigian
                          ,query.nam
                          ,query.thoihanbaoquan
                          ,query.ghichu
                          ,query.bosung1
                          ,query.bosung2
                          ,query.sokyhieuvb
                          ,query.phongdoi
                          ,query.mlhs
                          ,query.mst
                          ,query.domatkhan
                          ,query.matokhai
                          ,query.diachi
                          ,query.sogiaycn
                          ,query.stt
                          ,query.url
                          ,query.trangthai_muontra
                          ,query.nguoilap_hs
                          ,query.tuychon1
                          ,query.tuychon2
                          ,query.tuychon3
                          ,query.madulieu
                          ,query.mavanban
                          ,query.mahop
                          ,query.mahoso
                          ,query.sohoso_tam
                          ,query.mahoso_tam
                          ,query.full_search
                          ,query.tuychon4
                          ,query.tuychon5
                          ,query.gia
                          ,query.day
                          ,query.khoang
                          ,query.tang
                          ,query.vitri ,
                           query.line 
	                       ,tCountOrders.CountOrders ToltalRecords 
                    from query CROSS JOIN (SELECT Count(*) AS CountOrders FROM query {4}) AS tCountOrders
                    {3} 
                    {5}
                    offset (({0} - 1) * {1}) rows
                    fetch next {2} rows only", currentpage + 1, pagesize, pagesize, sqlwhere, sqlwhere, commandSQL);
                    var lstColumn = ctx.Database.SqlQuery<mucluchoso>(strSQl);
                    lsthh = lstColumn.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lsthh;
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindData();
        }

        protected void grvCustomer_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindData();
        }


        public static List<string> fileNameAttachYC;
        protected void DocumentsUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            bool isSubmissionExpired = false;
            if (UploadedFilesStorage == null)
            {
                isSubmissionExpired = true;
                UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
            }
            UploadedFileInfo tempFileInfo = UploadControlHelper.AddUploadedFileInfo(SubmissionID, e.UploadedFile.FileName);

            e.UploadedFile.SaveAs(tempFileInfo.FilePath);

            fileNameAttachYC.Add(e.UploadedFile.FileName);
            System.Web.HttpContext.Current.Session["ListFileAttach"] = fileNameAttachYC;

            if (e.IsValid)
                e.CallbackData = tempFileInfo.UniqueFileName + "|" + isSubmissionExpired;
        }


        protected void ProcessSubmit(string description, List<UploadedFileInfo> fileInfos, string guid)
        {
            //DescriptionLabel.Value = Server.HtmlEncode(description);

            if (fileInfos.Count > 0)
            {
                using (var ctx = new ADDJContext())
                {
                    var sqlInsrt = string.Format(@"DELETE FROM mucluchoso_file WHERE guid='{0}';", guid);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);
                }
            }


            foreach (UploadedFileInfo fileInfo in fileInfos)
            {
                // process uploaded files here
                byte[] fileContent = File.ReadAllBytes(fileInfo.FilePath);

                //Save the Byte Array as File.
                string timeSave = DateTime.Now.ToString("ddMMyyyhhmmss");
                string filePath = "~/ADDJ_TH/PdfFiles/" + Path.GetFileName(timeSave + "_" + fileInfo.OriginalFileName);
                var ddd = System.Web.HttpContext.Current.Server.MapPath(filePath);
                File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(filePath), fileContent);

                // lưu thông tin 
                using (var ctx = new ADDJContext())
                {
                    var sqlInsrt = string.Format(@"INSERT INTO mucluchoso_file (guid, tenfile, trangthai, ngaytao)
                                                   VALUES (N'{1}', N'{2}', 1, GETDATE())",
                                                   guid, guid, timeSave + "_" + fileInfo.OriginalFileName);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);
                }
            }
            //SubmittedFilesListBox.DataSource = fileInfos;
            //SubmittedFilesListBox.DataBind();
            //FormLayout.FindItemOrGroupByName("ResultGroup").Visible = true;
        }

        public void saveFile(string guid)
        {
            try
            {
                List<UploadedFileInfo> resultFileInfos = new List<UploadedFileInfo>();
                //string description = DescriptionTextBox.Text;
                string description = "";
                bool allFilesExist = true;

                var lst = System.Web.HttpContext.Current.Session["ListFileAttach"] as List<string>;
                //foreach (string fileName in UploadedFilesTokenBox.Tokens)
                if (lst == null)
                    return;
                foreach (string fileName in lst)
                {
                    UploadedFileInfo demoFileInfo = UploadControlHelper.GetDemoFileInfo(Convert.ToString(System.Web.HttpContext.Current.Session["SubmissionID"]), fileName);
                    FileInfo fileInfo = new FileInfo(demoFileInfo.FilePath);
                    if (fileInfo.Exists)
                    {
                        demoFileInfo.FileSize = TienIch.FormatSize(fileInfo.Length);
                        resultFileInfos.Add(demoFileInfo);
                    }
                    else
                        allFilesExist = false;
                }

                if (allFilesExist && resultFileInfos.Count > 0)
                {
                    ProcessSubmit(description, resultFileInfos, guid);
                    UploadControlHelper.RemoveUploadedFilesStorage(Convert.ToString(System.Web.HttpContext.Current.Session["SubmissionID"]));
                    //ASPxEdit.ClearEditorsInContainer(FormLayout, true);
                }
                else
                {
                    UploadedFilesTokenBox.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //[WebMethod]
        public static string saveFileAttach(string guid)
        {
            try
            {
                WebUserControl1_q10 sf = new WebUserControl1_q10();
                sf.saveFile(guid);
            }
            catch (Exception ex)
            {
                throw;
            }
            return "";
        }

        protected string SubmissionID
        {
            get
            {
                return HiddenField.Get("SubmissionID").ToString();
            }
            set
            {
                HiddenField.Set("SubmissionID", value);
            }
        }

        UploadedFilesStorage UploadedFilesStorage
        {
            get { return UploadControlHelper.GetUploadedFilesStorageByKey(SubmissionID); }
        }

        protected void grvCustomer_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;
            if (e.ButtonID == "Xem")
            {
                //if (quyenNguoiDung.Other1)
                //{
                //    e.Visible = DefaultBoolean.True;
                //}
                //else
                //{
                //    e.Visible = DefaultBoolean.False;
                //}
            }
            return;
            if (e.ButtonID == "Sua")
            {
                if (quyenNguoiDung.UserEdit)
                {
                    e.Visible = DefaultBoolean.True;
                }
                else
                {
                    e.Visible = DefaultBoolean.False;
                }
            }
            if (e.ButtonID == "Xoa")
            {
                if (quyenNguoiDung.UserDelete)
                {
                    e.Visible = DefaultBoolean.True;
                }
                else
                {
                    e.Visible = DefaultBoolean.False;
                }
            }
        }

        protected void grvCustomer_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            var noidung_timkiemchung = txtNoiDungTimKiem.Text.Trim();
            // các từ khóa nâng cao
            var search_com = txtNoiDungTimKiem.Text.Trim().Replace(";", " ") + " " + checkComboBoxHopSo.Text.Trim().Replace(";", " ") + " " + checkComboBoxHopSo.Text.Trim().Replace(";", " ") + " " + checkComboBoxHopSo.Text.Trim().Replace(";", " ");
            if (txtNoiDungTimKiem.Text.Trim() == "Nhập nội dung tìm kiếm" || txtNoiDungTimKiem.Text.Trim() == "")
            {
                noidung_timkiemchung = "";
                if (search_com.Trim() == "")
                {
                    return;
                }
            }
            string[] keyWordMLVBSearch2;
            switch (e.Column.FieldName.ToUpper())
            {
                case "HOPSO":
                case "HOSO_SO":
                case "TRICHYEUNOIDUNG":
                case "TRICHYEUNOIDUNG_MLHS":
                case "SOTO":
                case "THOIGIAN":
                case "NAM":
                case "THOIHANBAOQUAN":
                case "GHICHU":
                case "BOSUNG1":
                case "BOSUNG2":
                case "SOKYHIEUVB":
                case "PHONGDOI":
                case "MLHS":
                case "MST":
                case "DOMATKHAN":
                case "MATOKHAI":
                case "DIACHI":
                case "SOGIAYCN":
                case "STT":
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    var tempo = regex.Replace(noidung_timkiemchung.Trim().ToLower() + " " + search_com.ToLower().Replace(";", ""), " ");
                    keyWordMLVBSearch2 = Regex.Replace(tempo, @"\t|\n|\r", "").Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    if (keyWordMLVBSearch2.Where(x => e.Value.ToString().Trim().ToLower().Contains(x)).Any() &&
                        keyWordMLVBSearch2 != null)
                    {
                        e.DisplayText = Extensions.HighlightKeywords(e.Value.ToString().Trim(), tempo.Trim());
                    }
                    break;
                default:
                    break;
            }
            e.EncodeHtml = false;
        }

        protected void grvCustomer_DataBound(object sender, EventArgs e)
        {
            /*int i = 1;
            var ctx = new ADDJContext();
            var lstColumn = ctx.Database.SqlQuery<cauhinh>("SELECT [tencot], [tenhienthi], [chophephienthi] FROM[HT_HTKTTT].[dbo].[cauhinh]");

            foreach(cauhinh column in lstColumn.ToList())
            {
                if (column.chophephienthi == true) 
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Name = i.ToString();
                    col.VisibleIndex = i;
                    col.Caption = column.tenhienthi;
                    col.FieldName = column.tencot;
                    col.VisibleIndex = i++;
                    col.HeaderStyle.Wrap = DefaultBoolean.True;
                    grvCustomer.Columns.Add(col); 
                }
            }   */



            for (int i = 0; i < 30; i++)
            {
                var ctx = new ADDJContext();
                List<string> lstColumn = ctx.Database.SqlQuery<string>("SELECT cot" + (i + 1) + " FROM[HT_HTKTTT].[dbo].[thongtincauhinh]").ToList();


                if (lstColumn[0].Split('_')[0].Contains("1"))
                {
                    GridViewDataColumn col = new GridViewDataColumn();
                    col.Name = i.ToString();
                    if (i == 3)
                    {
                        col.Caption = "Trích yếu nội dung";
                        col.FieldName = "trichyeunoidung";
                    }
                    else
                    {
                        col.Caption = lstColumn[0].Split('_')[1];
                        col.FieldName = layFiedbyKey((i + 1).ToString()).ToLower();
                    }
                    col.VisibleIndex = i + 1;
                    col.CellStyle.Wrap = DefaultBoolean.True;
                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    col.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
                    col.HeaderStyle.Wrap = DefaultBoolean.True;
                    col.AdaptivePriority = i + 1;
                    grvCustomer.Columns.Add(col);
                }
            }

            GridViewCommandColumn gridViewCommandColumn = new GridViewCommandColumn();
            gridViewCommandColumn.VisibleIndex = 50;
            gridViewCommandColumn.Name = "50";
            gridViewCommandColumn.Caption = "#";

            GridViewCommandColumnCustomButton submitButton1 = new GridViewCommandColumnCustomButton() { ID = "Xem", Text = "Xem" };
            submitButton1.Image.Url = "../../HTHTKT/icons/view_16x16.gif";
            gridViewCommandColumn.CustomButtons.Add(submitButton1);

            GridViewCommandColumnCustomButton submitButton2 = new GridViewCommandColumnCustomButton() { ID = "Sua", Text = "Sửa" };
            submitButton2.Image.Url = "../../HTHTKT/icons/edit_16x16.gif";
            gridViewCommandColumn.CustomButtons.Add(submitButton2);

            GridViewCommandColumnCustomButton submitButton3 = new GridViewCommandColumnCustomButton() { ID = "Xoa", Text = "Xóa" };
            submitButton3.Image.Url = "../../HTHTKT/icons/delete_16x16.gif";
            gridViewCommandColumn.CustomButtons.Add(submitButton3);

            gridViewCommandColumn.Width = 100;
            gridViewCommandColumn.CellStyle.Wrap = DefaultBoolean.True;
            grvCustomer.Columns.Add(gridViewCommandColumn);

            ASPxGridView grid = (ASPxGridView)sender;
            foreach (var column in grid.DataColumns)
            {
                if (column.PropertiesEdit != null)
                    column.PropertiesEdit.EncodeHtml = true;
            }

            grvCustomer.Columns[0].MaxWidth = 60;
            grvCustomer.Columns[1].MaxWidth = 60;

            grvCustomer.Columns[2].Width = 200;
            grvCustomer.Columns[3].Width = 200;

            grvCustomer.Columns[2].AdaptivePriority = 0;
            grvCustomer.Columns["#"].AdaptivePriority = 50;


        }
    }

    public class hopsoinfo
    {
        public string hopso { get; set; }
        public int hs { get; set; }
    }
    public class hoso_soinfo
    {
        public string hoso_so { get; set; }
        public int hss { get; set; }
    }
    public class phongbaninfo
    {
        public string phongdoi { get; set; }
    }

    public class UploadedFilesStorage
    {
        public string Path { get; set; }
        public string Key { get; set; }
        public DateTime LastUsageTime { get; set; }

        public IList<UploadedFileInfo> Files { get; set; }
    }

    public class UploadedFileInfo
    {
        public string UniqueFileName { get; set; }
        public string OriginalFileName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
    }

    public static class UploadControlHelper
    {
        const int DisposeTimeout = 5;
        const string FolderKey = "~/ADDJ_TH/UploadDirectory";
        const string TempDirectory = "~/ADDJ_TH/UploadControl/Temp/";
        static readonly object storageListLocker = new object();

        static HttpContext Context { get { return HttpContext.Current; } }
        static string RootDirectory { get { return Context.Request.MapPath(TempDirectory); } }

        static IList<UploadedFilesStorage> uploadedFilesStorageList;
        static IList<UploadedFilesStorage> UploadedFilesStorageList
        {
            get
            {
                return uploadedFilesStorageList;
            }
        }

        static UploadControlHelper()
        {
            uploadedFilesStorageList = new List<UploadedFilesStorage>();
        }

        static string CreateTempDirectoryCore()
        {
            string uploadDirectory = Path.Combine(RootDirectory, Path.GetRandomFileName());
            Directory.CreateDirectory(uploadDirectory);

            return uploadDirectory;
        }
        public static UploadedFilesStorage GetUploadedFilesStorageByKey(string key)
        {
            lock (storageListLocker)
            {
                return GetUploadedFilesStorageByKeyUnsafe(key);
            }
        }
        static UploadedFilesStorage GetUploadedFilesStorageByKeyUnsafe(string key)
        {
            UploadedFilesStorage storage = UploadedFilesStorageList.Where(i => i.Key == key).SingleOrDefault();
            if (storage != null)
                storage.LastUsageTime = DateTime.Now;
            return storage;
        }
        public static string GenerateUploadedFilesStorageKey()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static void AddUploadedFilesStorage(string key)
        {
            lock (storageListLocker)
            {
                UploadedFilesStorage storage = new UploadedFilesStorage
                {
                    Key = key,
                    Path = CreateTempDirectoryCore(),
                    LastUsageTime = DateTime.Now,
                    Files = new List<UploadedFileInfo>()
                };
                UploadedFilesStorageList.Add(storage);
            }
        }
        public static void RemoveUploadedFilesStorage(string key)
        {
            lock (storageListLocker)
            {
                UploadedFilesStorage storage = GetUploadedFilesStorageByKeyUnsafe(key);
                if (storage != null)
                {
                    Directory.Delete(storage.Path, true);
                    UploadedFilesStorageList.Remove(storage);
                }
            }
        }
        public static void RemoveOldStorages()
        {
            if (!Directory.Exists(RootDirectory))
                Directory.CreateDirectory(RootDirectory);

            lock (storageListLocker)
            {
                string[] existingDirectories = Directory.GetDirectories(RootDirectory);
                foreach (string directoryPath in existingDirectories)
                {
                    UploadedFilesStorage storage = UploadedFilesStorageList.Where(i => i.Path == directoryPath).SingleOrDefault();
                    if (storage == null || (DateTime.Now - storage.LastUsageTime).TotalMinutes > DisposeTimeout)
                    {
                        Directory.Delete(directoryPath, true);
                        if (storage != null)
                            UploadedFilesStorageList.Remove(storage);
                    }
                }
            }
        }
        public static UploadedFileInfo AddUploadedFileInfo(string key, string originalFileName)
        {
            UploadedFilesStorage currentStorage = GetUploadedFilesStorageByKey(key);
            UploadedFileInfo fileInfo = new UploadedFileInfo
            {
                FilePath = Path.Combine(currentStorage.Path, Path.GetRandomFileName()),
                OriginalFileName = originalFileName,
                UniqueFileName = GetUniqueFileName(currentStorage, originalFileName)
            };
            currentStorage.Files.Add(fileInfo);

            return fileInfo;
        }
        public static UploadedFileInfo GetDemoFileInfo(string key, string fileName)
        {
            UploadedFilesStorage currentStorage = GetUploadedFilesStorageByKey(key);
            return currentStorage.Files.Where(i => i.UniqueFileName == fileName).SingleOrDefault();
        }
        public static string GetUniqueFileName(UploadedFilesStorage currentStorage, string fileName)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            int index = 1;

            while (currentStorage.Files.Any(i => i.UniqueFileName == fileName))
                fileName = string.Format("{0} ({1}){2}", baseName, index++, ext);

            return fileName;
        }
    }
}