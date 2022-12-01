using ADDJ.Admin;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro.Dashboards
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        public static List<string> fileNameAttachYC;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HiddenField["hidden_value"] = 0;
                HiddenField["res_value"] = 0;
                HiddenField["hidden_value2"] = 0;


                hiddenIDHoTroChiTietXuLy["hiddenIDHoTroChiTietXuLy"] = 0;

                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs where ss.TRANGTHAI.Value==true
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                cboChonHeThongYCHT.DataSource = dtbCheckUpdateSMSSList;
                cboChonHeThongYCHT.TextField = "TENHETHONG";
                cboChonHeThongYCHT.ValueField = "ID";
                cboChonHeThongYCHT.DataBind();

                cboChonHeThongYCHT.Items.Insert(0, new ListEditItem("-- chọn hệ thống ---", 0));
                cboChonHeThongYCHT.SelectedIndex = 0;


                var dtbChonMucDo = new List<HT_MUCDO_SUCO>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_MUCDO_SUCOs where ss.TRANGTHAI==1
                             select ss;
                    dtbChonMucDo = kl.ToList();
                }

                cboMucDoYeuCauXLSearch.DataSource = dtbChonMucDo;
                cboMucDoYeuCauXLSearch.TextField = "TENMUCDO";
                cboMucDoYeuCauXLSearch.ValueField = "ID";
                cboMucDoYeuCauXLSearch.DataBind();

                cboMucDoYeuCauXLSearch.Items.Insert(0, new ListEditItem("-- chọn mức độ ---", 0));
                cboMucDoYeuCauXLSearch.SelectedIndex = 0;


                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    hiddenIDHoTroChiTietXuLy["nguoixuly"] = loginInfo.Username;
                    hiddenIDHoTroChiTietXuLy["id_nguoixuly"] = loginInfo.Id;
                    hiddenIDHoTroChiTietXuLy["donvixuly"] = loginInfo.PhongBanId;
                    hiddenIDHoTroChiTietXuLy["iddonvi_from"] = loginInfo.PhongBanId;
                }

                SubmissionID = UploadControlHelper.GenerateUploadedFilesStorageKey();
                UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
                Session["SubmissionID"] = SubmissionID;


                HiddenField["SubmissionID"] = SubmissionID;

                fileNameAttachYC = new List<string>();
            }

            var ddd1 = Convert.ToInt32(HiddenField["res_value"]);
            var ddd2 = Convert.ToInt32(HiddenField["hidden_value2"]);
            if (Convert.ToInt32(HiddenField["res_value"]) != 0 && Convert.ToInt32(HiddenField["hidden_value2"]) != 0)
            {
                ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTro.FindControl("treelist_donvi_cc"));
                hdfData.RefreshVirtualTree();
            }
        }  
        

           protected void treelist_donvi_cc_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            HiddenField["res_value"] = 0;
            HiddenField["hidden_value2"] = 0;
            object sekkkk = HiddenField["hidden_value"];
            object sekkkksss = HiddenField["res_value"];
            object iddonvi = 0;

            string sqlWhere = string.Empty;
            if (Convert.ToInt32(sekkkk) != 0)
            {
                sqlWhere = " and ID_HETHONG_YCHT=" + Convert.ToInt32(sekkkk);
            }
            if (e.NodeObject == null)
            {
                var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                             where ss.ID_CHA == null
                             select ss;
                    string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,
                                      HasChild = cast((case when exists(
			                                    select *
			                                    from HT_CAYTHUMUC_YCHT a
			                                    where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1 
		                                    ) then 1 else 0 end) as bit)
                                      from HT_CAYTHUMUC_YCHT hhthtc
                                      WHERE hhthtc.ID_CHA is null " + sqlWhere;
                    var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
                    var dt = lsss.ToList();

                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(dt))
                    {
                        table.Load(reader);
                    }
                    e.Children = new DataView(table);
                }
            }
            else
            {
                if (Convert.ToInt32(sekkkksss) == 0)
                {

                    int id_cha = Convert.ToInt32(((DataRowView)e.NodeObject)["id"]);
                    //DataTable tb_dstt = SqlHelper.ExecuteDataset(strconn, "DM_THONGTIN_CUAHANG_GET_ALL", id_cha, 1).Tables[0];
                    //e.Children = new DataView(tb_dstt);

                    var dtbCheckUpdateSMSSList = new List<HT_CAYTHUMUC_YCHT>();
                    using (var ctx = new ADDJContext())
                    {
                        var kl = from ss in ctx.HT_CAYTHUMUC_YCHTs
                                 where ss.ID_CHA == null
                                 select ss;
                        string sqlStr = @"SELECT hhthtc.ID,hhthtc.LINHVUC,
                                          HasChild = cast((case when exists(
			                                        select *
			                                        from HT_CAYTHUMUC_YCHT a
			                                        where a.ID_CHA = hhthtc.ID and a.TRANGTHAI=1  
		                                        ) then 1 else 0 end) as bit)
                                          from HT_CAYTHUMUC_YCHT hhthtc
                                          WHERE hhthtc.ID_CHA =" + id_cha + sqlWhere;
                        var lsss = ctx.Database.SqlQuery<ViewCayThuMuc>(sqlStr);
                        var dt = lsss.ToList();

                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(dt))
                        {
                            table.Load(reader);
                        }
                        e.Children = new DataView(table);
                    }
                }
            }
        }

        protected void treelist_donvi_cc_VirtualModeNodeCreating(object sender, TreeListVirtualModeNodeCreatingEventArgs e)
        {
            var node = (DataRowView)e.NodeObject;
            e.IsLeaf = !(bool)node["HasChild"];
            e.NodeKeyValue = node["id"];
            e.SetNodeValue("linhvuc", node["linhvuc"]);
        }
        protected void treelist_donvi_cc_OnCustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            ASPxTreeList tree = sender as ASPxTreeList;
            tree.RefreshVirtualTree();
            tree.ExpandToLevel(1);
        }
        //+++++++++++++++++++++++++++++++++++++

        public static string idHeThong;
        protected void ASPxGridView2_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("tblHeThongXL");
            if (e.Parameters != "" && e.Parameters != "0")
                idHeThong = e.Parameters;// e.Parameters[0].ToString();
            else
                idHeThong = "";
            ASPxGridView2.DataBind();
        }

        protected void ASPxGridView2_PageIndexChanged(object sender, EventArgs e)
        {
            ASPxGridView2.DataBind();
        }

        protected void ASPxGridView2_DataBinding(object sender, EventArgs e)
        {
            var tbl = Session["tblHeThongXL"] as List<HT_XULY_YEUCAU_HOTRO6>;
            if (tbl == null)
            {
                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    using (var ctx = new ADDJContext())
                    {
                        var strWhereIdHeThong = "";
                        if (idHeThong != "")
                        {
                            var param = idHeThong.Split('|');
                            var idhethong = param[0];
                            var mucdo = param[1];
                            var loaihotro = param[2];
                            var sodienthoai = param[3];

                            if (idhethong != "0" && idhethong != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and a.ID_HETHONG_YCHT=" + idhethong;
                            if (mucdo != "0" && mucdo != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and b.ID_MUCDO_SUCO=" + mucdo;
                            if (loaihotro != "0" && loaihotro != "")
                                strWhereIdHeThong = strWhereIdHeThong + @" and b.ID_CAYTHUMUC_YCHT=" + loaihotro;
                            if (sodienthoai != "0" && sodienthoai != "")
                                strWhereIdHeThong = strWhereIdHeThong + string.Format(@" and b.SODIENTHOAI LIKE N'%{0}%'", sodienthoai); ;
                        }

                        // những user cùng đơn vị thì được vào xem
                        var strSQl = @";WITH cte AS
                                        (
                                           SELECT a.*,b.TRANGTHAI as TRANGTHAI_YC_XULY,b.SODIENTHOAI,b.ID_MUCDO_SUCO,
                                                 ROW_NUMBER() OVER (PARTITION BY ID_YEUCAU_HOTRO_HT ORDER BY NGAYTIEPNHAN DESC) AS rn
                                           FROM HT_XULY_YEUCAU_HOTRO a
                                        INNER JOIN HT_DM_YEUCAU_HOTRO_HT b ON a.ID_YEUCAU_HOTRO_HT=b.ID
                                                                where a.la_buoc_hientai=1 and b.trangthai!=4 and a.ID_DONVI_TO=" + loginInfo.PhongBanId + "" +
                                                            " and a.nguoitao!='" + loginInfo.Username + "' " + strWhereIdHeThong +
                                                            ") " + @"
                                        SELECT a.ID,
                                          a.SODIENTHOAI,
                                          a.TRANGTHAI,
                                          a.TRANGTHAI_YC_XULY,
                                          a.NOIDUNGXULY,
                                          a.NGAYTIEPNHAN,
                                          DATEDIFF(DAY, a.NGAYTIEPNHAN,GETDATE()) SONGAY,
                                          TINH_TRANG_XL=CASE WHEN DATEDIFF(DAY, a.NGAYTIEPNHAN,GETDATE())>1 THEN N'Quá hạn' ELSE N'Đang xử lý' end ,
                                          b.MA_YEUCAU,
                                          k.TENDONVI DONVI_TAO_YC,
                                          b.NOIDUNG_YEUCAU,
                                          c.BUOCXULY,
                                          d.TENHETHONG,
                                          d.MOTA,
                                          e.LINHVUC,  
                                          f.TENDONVI,
                                          g.TENMUCDO
                                        FROM cte a
                                        LEFT JOIN HT_DM_YEUCAU_HOTRO_HT b on b.ID=a.ID_YEUCAU_HOTRO_HT
                                        LEFT JOIN HT_NODE_LUONG_HOTRO c on c.ID=a.ID_NODE_LUONG_HOTRO
                                        LEFT JOIN HT_DM_HETHONG_YCHT d on d.ID=a.ID_HETHONG_YCHT
                                        LEFT JOIN HT_CAYTHUMUC_YCHT e ON e.ID=b.ID_CAYTHUMUC_YCHT
                                        LEFT JOIN HT_DONVI k ON k.ID=a.ID_DONVI_FROM
                                        LEFT JOIN HT_DONVI f ON f.Id=a.ID_DONVI_TO
                                        LEFT JOIN HT_MUCDO_SUCO g on g.ID=a.ID_MUCDO_SUCO
                                        WHERE rn = 1
                                        ORDER BY NGAYTIEPNHAN DESC";
                        var lstHoTro = ctx.Database.SqlQuery<HT_XULY_YEUCAU_HOTRO6>(strSQl);
                        var lst = lstHoTro.ToList();
                        Session["tblHeThongXL"] = lst;
                        tbl = lst;
                    }
                }
            }
            ASPxGridView2.DataSource = tbl;
        }

        protected void ASPxGridView2_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            //if (e.VisibleIndex == -1) return;
            //var ishoatdong = ASPxGridView2.GetRowValues(e.VisibleIndex, "TRANG_THAI");
            //if (e.ButtonID == "Xem" && Convert.ToInt32(ishoatdong) == 0)
            //    e.Visible = DefaultBoolean.False;
        }

        protected void ASPxGridView2_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "TRANGTHAI_YC_XULY")
            {
                object getvalTrangthai = e.GetFieldValue("TRANGTHAI_YC_XULY");
                switch (Convert.ToInt32(getvalTrangthai))
                {
                    case 4:
                        e.DisplayText = "Xử lý xong";
                        break;
                    case 3:
                        e.DisplayText = "Chờ xác nhận...";
                        break;
                    case 2:  // trạng thái này dành cho user đã phản hồi 1 vòng về người yêu cầu hỗ trợ để xác nhận
                        e.DisplayText = "Chuyển phản hồi...";
                        break;
                    case 1:
                        e.DisplayText = "Chuyển tiếp...";
                        break;
                    case 0:
                        e.DisplayText = "Khởi tạo và chuyển tiếp.";
                        break;
                    default:
                        break;
                }
            }           
        }

        protected void ASPxGridView2_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "TINH_TRANG_XL")
            {
                int songay = Convert.ToInt32(e.GetValue("SONGAY"));
                if (songay <= 1)
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                else
                    e.Cell.BackColor = System.Drawing.Color.Red;
            }
            if (e.DataColumn.FieldName != "TRANGTHAI_YC_XULY") return;
            switch (Convert.ToInt32(e.CellValue))
            {
                case 4://4: Người ra yêu cầu xác nhận ok
                    e.Cell.BackColor = System.Drawing.Color.Green;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    break;
                case 3://3: Chờ xác nhận của người yêu cầu hỗ trợ, trạng thái này dành cho user đã phản hồi 1 vòng về người yêu cầu hỗ trợ để xác nhận
                    e.Cell.BackColor = System.Drawing.Color.Magenta;
                    break;
                case 2:  //2: Chuyển phản hồi
                    e.Cell.BackColor = System.Drawing.Color.Maroon;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    break;
                case 1://1: Chuyển tiếp
                    e.Cell.BackColor = System.Drawing.Color.LightCoral;
                    break;
                case 0://0: khởi tạo,chuyen tiep
                    e.Cell.BackColor = System.Drawing.Color.LightCyan;
                    break;
                default:
                    break;
            }
        }

        
        protected void DocumentsUploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            //bool isSubmissionExpired = false;
            //if (UploadedFilesStorage == null)
            //{
            //    isSubmissionExpired = true;
            //    UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
            //}
            //UploadedFileInfo tempFileInfo = UploadControlHelper.AddUploadedFileInfo(SubmissionID, e.UploadedFile.FileName);


            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            baseUrl = baseUrl + "HeThongHoTro/Services/ws_tapTinDinhKem.asmx";
            string soap = string.Format(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
               <soapenv:Header/>
                <soapenv:Body>
                <tem:addTempFileAttach>
                     <tem:SubmissionID>{0}</tem:SubmissionID>
                           <tem:filename>{1}</tem:filename>
                             </tem:addTempFileAttach>
                           </soapenv:Body>
                         </soapenv:Envelope> ", SubmissionID, e.UploadedFile.FileName);


            string url = baseUrl;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(soap);
            //req.Headers.Add("SOAPAction", ""); 
            req.Method = "POST";
            req.ContentType = "text/xml;charset=utf-8";
            req.ContentLength = requestBytes.Length;
            //req.KeepAlive = false;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();
            var res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
            string backstr = sr.ReadToEnd();
            XDocument xDoc;
            using (StringReader s = new StringReader(backstr))
            {
                xDoc = XDocument.Load(s);
            }
            var feeds = from feed in xDoc.Descendants(XNamespace.Get("http://tempuri.org/") + "addTempFileAttachResult")
                        select new
                        {
                            packageid = feed.Value
                        };
            var lstsvuse = "";
            foreach (var feed in feeds)
            {
                lstsvuse += feed.packageid + " ";
            }



            e.UploadedFile.SaveAs(lstsvuse);

            fileNameAttachYC.Add(e.UploadedFile.FileName);



            // lưu tạm thông tin file đính kèm trước

            using (var ctx = new ADDJContext())
            {
                var strSqlFileTemp = string.Format(@"INSERT INTO dbo.HT_FILES_DINHKEM_TEMP
                                        (
                                          ID_XULY_YEUCAU_HOTRO
                                         ,SUBMISSIONID
                                         ,FILENAME
                                         ,NGAYTAO
                                        )
                                        VALUES
                                        (
                                            {0} -- ID_XULY_YEUCAU_HOTRO - int
                                         ,N'{1}' -- SUBMISSIONID - nvarchar(50)
                                         ,N'{2}' -- FILENAME - nvarchar(500)
                                         ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                        )", 0, SubmissionID, e.UploadedFile.FileName);
                ctx.Database.ExecuteSqlCommand(strSqlFileTemp);
            }
            Session["ListFileAttach"] = fileNameAttachYC;

            if (e.IsValid)
                e.CallbackData = e.UploadedFile.FileName;
        }


        protected void ProcessSubmit(string description, List<UploadedFileInfo> fileInfos, string idhethong, string idyeucauhotro, string idxuly_ycht)
        {
            //DescriptionLabel.Value = Server.HtmlEncode(description);

            foreach (UploadedFileInfo fileInfo in fileInfos)
            {
                // process uploaded files here
                byte[] fileContent = File.ReadAllBytes(fileInfo.FilePath);

                //Save the Byte Array as File.
                string timeSave = DateTime.Now.ToString("ddMMyyyhhmmss");
                string filePath = "~/HTHTKT/UploadAttachFiles/" + Path.GetFileName(timeSave + "_" + fileInfo.OriginalFileName);
                File.WriteAllBytes(Server.MapPath(filePath), fileContent);


                // lưu thông tin 

                using (var ctx = new ADDJContext())
                {
                    var sqlInsrt = string.Format(@"INSERT INTO dbo.HT_FILES_DINHKEM_YCHT
                                                    (
                                                      ID_HETHONG_YCHT
                                                     ,ID_YEUCAU_HOTTRO_HT
                                                     ,ID_XULY_YCHT
                                                     ,TENFILE
                                                     ,TRANGTHAI
                                                     ,NGAYTAO
                                                    )
                                                    VALUES
                                                    (
                                                      {0} -- ID_HETHONG_YCHT - int
                                                     ,{1} -- ID_YEUCAU_HOTTRO_HT - int
                                                     ,{2} -- ID_XULY_YCHT - int
                                                     ,N'{3}' -- TENFILE - nvarchar(50)
                                                     ,1 -- TRANGTHAI - int
                                                     ,GETDATE() -- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAYTAO - datetime
                                                    )", idhethong, idyeucauhotro,idxuly_ycht, fileInfo.OriginalFileName);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);
                }
            }

            //SubmittedFilesListBox.DataSource = fileInfos;
            //SubmittedFilesListBox.DataBind();
            //FormLayout.FindItemOrGroupByName("ResultGroup").Visible = true;
        }


        public void saveFile(string idhethong, string idyeucauhotro, string idxuly_ycht)
        {
            try
            {
                List<UploadedFileInfo> resultFileInfos = new List<UploadedFileInfo>();
                //string description = DescriptionTextBox.Text;
                string description = "";
                bool allFilesExist = true;

                var lst = Session["ListFileAttach"] as List<string>;
                //foreach (string fileName in UploadedFilesTokenBox.Tokens)
                if (lst == null)
                    return;
                foreach (string fileName in lst)
                {
                    UploadedFileInfo demoFileInfo = UploadControlHelper.GetDemoFileInfo(Convert.ToString(Session["SubmissionID"]), fileName);
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
                    ProcessSubmit(description, resultFileInfos, idhethong, idyeucauhotro, idxuly_ycht);

                    UploadControlHelper.RemoveUploadedFilesStorage(Convert.ToString(Session["SubmissionID"]));

                    //ASPxEdit.ClearEditorsInContainer(FormLayout, true);
                }
                else
                {
                    UploadedFilesTokenBox0001.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox0001.IsValid = false;

                    UploadedFilesTokenBox0002.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox0002.IsValid = false;

                    UploadedFilesTokenBox0003.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox0003.IsValid = false;

                    UploadedFilesTokenBox0004.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox0004.IsValid = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [WebMethod]
        public static string saveFileAttach(string idhethong, string idyeucauhotro, string idxuly_ycht)
        {
            try
            {
                WebUserControl1 sf = new WebUserControl1();
                sf.saveFile(idhethong, idyeucauhotro, idxuly_ycht);
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
        const string FolderKey = "~/HTHTKT/UploadDirectory";
        const string TempDirectory = "~/HTHTKT/UploadControl/Temp/";
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