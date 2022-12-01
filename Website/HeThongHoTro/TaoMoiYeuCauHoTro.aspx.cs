using ADDJ.Admin;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.Internal;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.HTHTKT;
using Website.HTHTKT.Entity;

namespace Website.HeThongHoTro
{
    public partial class TaoMoiYeuCauHoTro : System.Web.UI.Page
    {
        public static List<string> fileNameAttachYC;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxHiddenField1["hidden_value"] = 0;
                ASPxHiddenField2["res_value"] = 0;
                ASPxHiddenField3["hidden_value2"] = 0;
                var dtbCheckUpdateSMSSList = new List<HT_DM_HETHONG_YCHT>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_HE_THONG_HOTROs where ss.TRANGTHAI.Value==true
                             select ss;
                    dtbCheckUpdateSMSSList = kl.ToList();
                }
                ASPxComboBox1.DataSource = dtbCheckUpdateSMSSList;
                ASPxComboBox1.TextField = "TENHETHONG";
                ASPxComboBox1.ValueField = "ID";
                ASPxComboBox1.DataBind();



                var dtbChonMucDo = new List<HT_MUCDO_SUCO>();
                using (var ctx = new ADDJContext())
                {
                    var kl = from ss in ctx.HT_MUCDO_SUCOs where ss.TRANGTHAI==1
                             select ss;
                    dtbChonMucDo = kl.ToList();
                }

                cboMucDoSuCo.DataSource = dtbChonMucDo;
                cboMucDoSuCo.TextField = "TENMUCDO";
                cboMucDoSuCo.ValueField = "ID";
                cboMucDoSuCo.DataBind();

                cboMucDoSuCo.Items.Insert(0, new ListEditItem("-- chọn mức độ ---", 0));
                cboMucDoSuCo.SelectedIndex = 0;



                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    NoiChuyenDenID["nguoixuly"] = loginInfo.Username;
                    NoiChuyenDenID["id_nguoixuly"] = loginInfo.Id;
                    NoiChuyenDenID["iddonvitao"] = loginInfo.PhongBanId;
                }



                SubmissionID = UploadControlHelper.GenerateUploadedFilesStorageKey();
                UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
                Session["SubmissionID"] = SubmissionID;

                fileNameAttachYC = new List<string>();
            } 
            var ddd1 = Convert.ToInt32(ASPxHiddenField2["res_value"]);
            var ddd2 = Convert.ToInt32(ASPxHiddenField3["hidden_value2"]);   // id loại yêu cầu
            if (Convert.ToInt32(ASPxHiddenField2["res_value"]) != 0 && Convert.ToInt32(ASPxHiddenField3["hidden_value2"]) != 0)
            {
                ASPxTreeList hdfData = ((ASPxTreeList)LoaiHoTro.FindControl("treelist_donvi_cc"));
                hdfData.RefreshVirtualTree();
            }
        }
        protected void treelist_donvi_cc_VirtualModeCreateChildren(object sender, TreeListVirtualModeCreateChildrenEventArgs e)
        {
            ASPxHiddenField2["res_value"] = 0;
            ASPxHiddenField3["hidden_value2"] = 0;   // id loại yêu cầu
            object sekkkk = ASPxHiddenField1["hidden_value"];
            object sekkkksss = ASPxHiddenField2["res_value"];
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

        private void loadDonViChuyenDen(object donvi)
        {
            try
            {
                // xử lý nạp nơi chuyển đến theo đơn vị gốc ban đầu của user đăng nhập vào
                // ví dụ: với hệ thống CCBS có 2 nhánh luồng sau
                // CCBS: Nhánh 1: Luồng 63TTKD - BanKTNV - TTCNTT / KTM
                // CCBS: Nhánh 2: Luồng Ban KTVN - TTCNTT / KTM
                // nếu user đăng nhập thuộc 63TTKD => luồng 1
                // nếu user đăng nhập vào thuộc KTNV => luồng 2
                // ở đây bước xử lý là 1 (do user tạo mới luồng)

                AdminInfo loginInfo = LoginAdmin.AdminLogin();
                if (loginInfo != null)
                {
                    // thông tin đơn vị
                    var id_donvi = loginInfo.PhongBanId;
                    object idhethonghotro = ASPxHiddenField1["hidden_value"];
                    int sobuocxuly = 0;
                    // xác định xem user login đang ở node mấy?
                    // lấy danh sách các luồng mà khi user này login vào
                    // có thể thực hiện
                    var strSqlNode = string.Format(@"SELECT hnlh.ID
                                                  ,hlh.ID_HETHONG_YCHT
                                                  ,hnlh.ID_LUONG_HOTRO
                                                  ,hnlh.ID_DONVI
                                                  ,hnlh.BUOCXULY
                                                  ,hnlh.NGAYTAO
                                                  ,hlh.SOBUOC
                                                  ,dt.TENDONVI
                                              FROM HT_NODE_LUONG_HOTRO hnlh 
                                              LEFT JOIN HT_LUONG_HOTRO hlh ON hlh.ID = hnlh.ID_LUONG_HOTRO
                                              LEFT JOIN HT_DM_HETHONG_YCHT hhh ON hhh.ID = hlh.ID_HETHONG_YCHT
                                              LEFT JOIN HT_DONVI dt ON dt.Id=hnlh.ID_DONVI
                                            WHERE hhh.ID={0} AND hnlh.ID_DONVI={1} and buocxuly=1 ",
                                                Convert.ToInt32(idhethonghotro), id_donvi);
                    List<HT_NODE_LUONG_HOTRO> lstNodeLuong = new List<HT_NODE_LUONG_HOTRO>();
                    using (var ctx = new ADDJContext())
                    {
                        lstNodeLuong = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSqlNode).ToList();
                    }



                    // kiểm tra bước xử lý và luồng hỗ trợ
                    int idluonghotro = 0;
                    int buocxulyhienhanh = 0;
                    if (lstNodeLuong.Any())
                    {
                        var lstAllDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                        foreach (var item in lstNodeLuong)
                        {
                            idluonghotro = item.ID_LUONG_HOTRO.Value;
                            buocxulyhienhanh = item.BUOCXULY.Value;
                            sobuocxuly = item.SOBUOC.Value;

                            // lấy thông tin bước xử lý tiếp theo
                            // kiểm tra xem đã là bước cuối chưa
                            if (sobuocxuly >= buocxulyhienhanh + 1)
                            {
                                var strSqlNodeNext = string.Format(@"SELECT hnlh.ID
                                                          ,hlh.ID_HETHONG_YCHT
                                                          ,hnlh.ID_LUONG_HOTRO
                                                          ,hnlh.ID_DONVI
                                                          ,hnlh.BUOCXULY
                                                          ,hnlh.NGAYTAO
                                                          ,hlh.SOBUOC
                                                          ,dt.TENDONVI
                                                      FROM HT_NODE_LUONG_HOTRO hnlh 
                                                      LEFT JOIN HT_LUONG_HOTRO hlh ON hlh.ID = hnlh.ID_LUONG_HOTRO
                                                      LEFT JOIN HT_DM_HETHONG_YCHT hhh ON hhh.ID = hlh.ID_HETHONG_YCHT
                                                      LEFT JOIN HT_DONVI dt ON dt.Id=hnlh.ID_DONVI
                                                    WHERE hhh.ID={0} AND hnlh.ID_LUONG_HOTRO={1} and hnlh.BUOCXULY={2} ",
                                                       Convert.ToInt32(idhethonghotro),
                                                       idluonghotro,
                                                       buocxulyhienhanh + 1);
                                var lstDSDonViXuLyTiep = new List<HT_NODE_LUONG_HOTRO>();
                                using (var ctx = new ADDJContext())
                                {
                                    lstDSDonViXuLyTiep = ctx.Database.SqlQuery<HT_NODE_LUONG_HOTRO>(strSqlNodeNext).ToList();
                                }
                                if (lstDSDonViXuLyTiep.Any())
                                    lstAllDonViXuLyTiep.AddRange(lstDSDonViXuLyTiep);
                            }
                        }
                        noiChuyenDen.DataSource = lstAllDonViXuLyTiep;
                        noiChuyenDen.ValueField = "ID_DONVI";
                        noiChuyenDen.TextField = "TENDONVI";
                        noiChuyenDen.DataBind();
                        noiChuyenDen.Items.Insert(0, new ListEditItem("-- Chọn nơi chuyển đến --", 0));
                        noiChuyenDen.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void noiChuyenDen_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            object id_donvi_cha = e.Parameter;
            loadDonViChuyenDen(id_donvi_cha);
        }

        protected void btnChuyenDenHT_Click(object sender, EventArgs e)
        {
            var idHeThongYeuCauHoTro = ThongHTKTID["ThongHTKTID"];
            var idLoaiHoTro = LoaiHTKTID["LoaiHTKTID"];
            var txtNoiDungYeuCauHoTro = noiDungYCHoTro.Text;
            var idNoiChuyenDen = NoiChuyenDenID["NoiChuyenDenID"];

            // Xử lý lấy thông tin Node_Luong_Ho_Tro
            // Lấy Node trong bảng: HT_NODE_LUONG_HOTRO theo idNoiChuyen
            var idNodLuongHT = 0;
            using (var ctx = new ADDJContext())
            {
                var strSql = "select id from HT_NODE_LUONG_HOTRO where iddonvi=" + idNoiChuyenDen;
                var lstId = ctx.Database.SqlQuery<int>(strSql);
                var ls = lstId.FirstOrDefault();
                idNodLuongHT = Convert.ToInt32(ls);
            }

            AdminInfo loginInfo = LoginAdmin.AdminLogin();
            decimal idHoTro = 0;
            if (loginInfo != null)
            {
                using (var ctx = new ADDJContext())
                {
                    using (var trans = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            // lưu thông tin hỗ trợ: HT_DM_YEUCAU_HOTRO_HT
                            var strSQLChiTietHT = string.Format(@"INSERT INTO dbo.HT_DM_YEUCAU_HOTRO_HT
                                (
                                  ID_HETHONG_YCHT
                                 , MAHOTRO
                                 , ID_CAYTHUMUC_YCHT
                                 , TENHOTRO
                                 , LOAIHOTRO
                                 , NOIDUNG
                                 , NGAYTAO
                                 , NGUOITAO
                                 , ID_LUONG_HOTRO
                                )
                                VALUES
                                (
                                  {0}-- ID_HE_THONG - int
                                 , N'{1}'-- MA_HO_TRO - nvarchar(50)
                                 , {2}-- ID_CAYTHUMUC_YCHT - int
                                 , N'{3}'-- TEN_HO_TRO - nvarchar(50)
                                 , N'{4}'-- LOAI_HO_TRO - nvarchar(50)
                                 , N'{5}'-- NOI_DUNG - nvarchar(50)
                                 , GETDATE()-- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAY_TAO - datetime
                                 , N'{6}'-- NGUOI_TAO - nvarchar(50)
                                 , {7}
                                ); SELECT SCOPE_IDENTITY();",
                                   idHeThongYeuCauHoTro,
                                   new Random().Next(0, 9999).ToString(),
                                   idLoaiHoTro,
                                   "tên hỗ trợ " + DateTime.Now,
                                   "loại hỗ trợ " + DateTime.Now,
                                   txtNoiDungYeuCauHoTro,
                                   loginInfo.Username,
                                   idHeThongYeuCauHoTro);
                            var rest = ctx.Database.SqlQuery<decimal>(strSQLChiTietHT).ToList();
                            idHoTro = rest.FirstOrDefault();


                            // lưu thông tin khởi tạo luồng đầu tiên xử lý: HT_XULY_HOTRO
                            // đây là bước khởi tạo thì LOAI_HANH_DONG: 1: Chuyển tiếp
                            // (1: Chuyển tiếp, 2: Xử lý luôn, 3, Gửi phản hồi)
                            var strSQLChiTietXuLyHT = string.Format(@"INSERT INTO dbo.HT_XULY_YEUCAU_HOTRO
                                (
                                  ID_HETHONG_YCHT
                                 , ID_NODE_LUONG_HOTRO
                                 , NGUOIHOTRO
                                 , LOAIHANHDONG
                                 , NOIDUNGXULY
                                 , NGAYXULY
                                 , TRANGTHAI
                                 , ID_DONVI_FROM
                                 , ID_DONVI_TO
                                 , NGAYTIEPNHAN
                                 , NGUOITAO
                                )
                                VALUES
                                (
                                  {0}-- ID_HO_TRO - int
                                 , {1}-- ID_NODE_LUONG_HO_TRO - int
                                 , N'{2}'-- NGUOI_HO_TRO - nvarchar(50)
                                 , {3}-- LOAI_HANH_DONG - int
                                 , N'{4}'-- NOI_DUNG_XU_LY - nvarchar(500)
                                 , GETDATE()-- 'YYYY-MM-DD hh:mm:ss[.nnn]'-- NGAY_XU_LY - datetime
                                 , {5}
                                 , {6}
                                 , {7}
                                 , GETDATE()
                                 , {8}
                                )",
                                        idHoTro,
                                        idNodLuongHT,
                                        "",
                                        1,
                                        txtNoiDungYeuCauHoTro,
                                        1,
                                        loginInfo.PhongBanId,
                                        idNoiChuyenDen,
                                        loginInfo.Username);
                            var rest1 = ctx.Database.ExecuteSqlCommand(strSQLChiTietXuLyHT);

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
            }
        }


    
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
            Session["ListFileAttach"] = fileNameAttachYC;

            if (e.IsValid)
                e.CallbackData = tempFileInfo.UniqueFileName + "|" + isSubmissionExpired;
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
                    UploadedFilesTokenBox.ErrorText = "Submit failed because files have been removed from the server due to the 5 minute timeout.";
                    UploadedFilesTokenBox.IsValid = false;
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
                TaoMoiYeuCauHoTro sf = new TaoMoiYeuCauHoTro();
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