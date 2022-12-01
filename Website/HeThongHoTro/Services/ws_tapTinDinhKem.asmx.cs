using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using Website.HTHTKT;

namespace Website.HeThongHoTro.Services
{
    /// <summary>
    /// Summary description for ws_tapTinDinhKem
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_tapTinDinhKem : System.Web.Services.WebService
    {

        [WebMethod]
        public string danhSachFileAttach(string id_xuly_ycht)
        {
            string ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var strSql = string.Format(@" SELECT hfdy.ID
                          ,hfdy.ID_HETHONG_YCHT
                          ,hfdy.ID_YEUCAU_HOTTRO_HT
                          ,hfdy.ID_XULY_YCHT
                          ,hfdy.TENFILE
                          ,hfdy.TRANGTHAI
                          ,hfdy.NGAYTAO,FORMAT(hfdy.NGAYTAO , 'ddMMyyyyhhmmss')+'_'+hfdy.TENFILE AS TENFILETAIVE
                          FROM HT_FILES_DINHKEM_YCHT hfdy 
                          WHERE hfdy.ID_XULY_YCHT={0} ", id_xuly_ycht);
                    var lst = ctx.Database.SqlQuery<HT_FILES_DINHKEM_YCHT>(strSql);
                    var lstfileattach = lst.ToList();
                    if (lstfileattach != null)
                        ret = Newtonsoft.Json.JsonConvert.SerializeObject(lstfileattach);
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "FILE");
            }
            return ret;
        }

   
        [WebMethod]
        public string addTempFileAttach(string SubmissionID, string filename)
        {
            var filePath = "";
            try
            {
                if (UploadControlHelper.GetUploadedFilesStorageByKey(SubmissionID) == null)
                {
                    UploadControlHelper.AddUploadedFilesStorage(SubmissionID);
                }
                UploadedFileInfo tempFileInfo = UploadControlHelper.AddUploadedFileInfo(SubmissionID, filename);
                filePath = tempFileInfo.FilePath;
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "FILE");
            }
            return filePath;
        }



        [WebMethod]
        public string updateSubmisisonInfo(string submissionid, string idhethong, string idyeucauhotro, string idxuly_ycht)
        {
            var ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var sqlInsrt = string.Format(@"update dbo.HT_FILES_DINHKEM_TEMP
                                                   set ID_XULY_YEUCAU_HOTRO={0} 
                                                   where SUBMISSIONID='{1}'", idxuly_ycht, submissionid);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);

                    var lstDanhSachFiles = new List<string>();
                    var strCmdF = string.Format(@"select FILENAME 
                                                  from HT_FILES_DINHKEM_TEMP 
                                                  where ID_XULY_YEUCAU_HOTRO={0}", idxuly_ycht);
                    lstDanhSachFiles = ctx.Database.SqlQuery<string>(strCmdF).ToList();
                    if(lstDanhSachFiles.Any())
                    {
                        luuTapTinDinhKem(submissionid, lstDanhSachFiles, idhethong, idyeucauhotro, idxuly_ycht);
                    }

                    ret = "1";
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "FILE");
                ret = "0";
            }
            return ret;
        }


       [WebMethod]
        public string luuTapTinDinhKem(string idkeysubmission, List<string> files, string idhethong, string idyeucauhotro, string idxuly_ycht)
        {
            try
            {
                List<UploadedFileInfo> resultFileInfos = new List<UploadedFileInfo>();
                bool allFilesExist = true;
                if (files.Count>0)
                {
                    foreach (string fileName in files)
                    {
                        UploadedFileInfo demoFileInfo = UploadControlHelper.GetDemoFileInfo(idkeysubmission, fileName);
                        FileInfo fileInfo = new FileInfo(demoFileInfo.FilePath);

                        if (fileInfo.Exists)
                        {
                            demoFileInfo.FileSize = Website.HTHTKT.TienIch.FormatSize(fileInfo.Length);
                            resultFileInfos.Add(demoFileInfo);
                        }
                        else
                            allFilesExist = false;
                    }
                    if (allFilesExist && resultFileInfos.Count > 0)
                    {
                        ProcessSubmit("description", resultFileInfos, idhethong, idyeucauhotro, idxuly_ycht);
                        UploadControlHelper.RemoveUploadedFilesStorage(idkeysubmission);
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "FILE");
            }
            return "";
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
                                                    )", idhethong, idyeucauhotro, idxuly_ycht, fileInfo.OriginalFileName);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);
                }
            }
            //SubmittedFilesListBox.DataSource = fileInfos;
            //SubmittedFilesListBox.DataBind();
            //FormLayout.FindItemOrGroupByName("ResultGroup").Visible = true;
        }




        // xử lý lưu file đối với trường hợp xử lý hàng loạt
        [WebMethod]
        public string updateSubmisisonInfoHangLoat(string submissionid, string[] idhethong, string[] idyeucauhotro, string[] idxuly_ycht)
        {
            var ret = "";
            try
            {
                using (var ctx = new ADDJContext())
                {
                    var sqlInsrt = string.Format(@"update dbo.HT_FILES_DINHKEM_TEMP
                                                   set ID_XULY_YEUCAU_HOTRO={0} 
                                                   where SUBMISSIONID='{1}'", idxuly_ycht, submissionid);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);

                    var lstDanhSachFiles = new List<string>();
                    var strCmdF = string.Format(@"select FILENAME 
                                                  from HT_FILES_DINHKEM_TEMP 
                                                  where ID_XULY_YEUCAU_HOTRO={0}", idxuly_ycht);
                    lstDanhSachFiles = ctx.Database.SqlQuery<string>(strCmdF).ToList();
                    if (lstDanhSachFiles.Any())
                    {
                        luuTapTinDinhKemHangLoat(submissionid, lstDanhSachFiles, idhethong, idyeucauhotro, idxuly_ycht);
                    }

                    ret = "1";
                }
            }
            catch (Exception ex)
            {
                ret = "0";
                Actions.ActionProcess.GhiLog(ex, "FILE");
            }
            return ret;
        }

        [WebMethod]
        public string luuTapTinDinhKemHangLoat(string idkeysubmission, List<string> files, string[] idhethong, string[] idyeucauhotro, string[] idxuly_ycht)
        {
            try
            {
                List<UploadedFileInfo> resultFileInfos = new List<UploadedFileInfo>();
                bool allFilesExist = true;
                if (files.Count > 0)
                {
                    foreach (string fileName in files)
                    {
                        UploadedFileInfo demoFileInfo = UploadControlHelper.GetDemoFileInfo(idkeysubmission, fileName);
                        FileInfo fileInfo = new FileInfo(demoFileInfo.FilePath);

                        if (fileInfo.Exists)
                        {
                            demoFileInfo.FileSize = Website.HTHTKT.TienIch.FormatSize(fileInfo.Length);
                            resultFileInfos.Add(demoFileInfo);
                        }
                        else
                            allFilesExist = false;
                    }
                    if (allFilesExist && resultFileInfos.Count > 0)
                    {
                        ProcessSubmitHangLoat("description", resultFileInfos, idhethong, idyeucauhotro, idxuly_ycht);
                        UploadControlHelper.RemoveUploadedFilesStorage(idkeysubmission);
                    }
                }
            }
            catch (Exception ex)
            {
                Actions.ActionProcess.GhiLog(ex, "FILE");
            }
            return "";
        }

        protected void ProcessSubmitHangLoat(string description, List<UploadedFileInfo> fileInfos, string[] idhethong, string[] idyeucauhotro, string[] idxuly_ycht)
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
                                                    )", idhethong, idyeucauhotro, idxuly_ycht, fileInfo.OriginalFileName);
                    ctx.Database.ExecuteSqlCommand(sqlInsrt);
                }
            }
            //SubmittedFilesListBox.DataSource = fileInfos;
            //SubmittedFilesListBox.DataBind();
            //FormLayout.FindItemOrGroupByName("ResultGroup").Visible = true;
        }



        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
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
