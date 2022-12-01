using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ADDJ.Core
{
    public class RemoteFileToFtp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckFileExistOnFtp(string fileName)
        {
            bool flag = false;
            var request = (FtpWebRequest)WebRequest.Create(new Uri(Config.DomainDownload + fileName));
            request.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                flag = true;
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                }
            }
            finally { request = null; }
            return flag;
        }
        public static void DownloadFileToLocal(string addressFtp, string addLocal, string localTemp)
        {
            DirectoryInfo df = new DirectoryInfo(localTemp);
            DeleteFolderAndFileTemp(df);
            FtpWebRequest reqFTP;
            string fileName = Path.GetFileName(addressFtp);
            try
            {
                if (CheckFileExistOnFtp(addressFtp))
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Config.DomainDownload + addressFtp));
                    reqFTP.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.UseBinary = true;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream ftpStream = response.GetResponseStream();
                    CreateFolderOnLocal(addLocal);
                    if (!File.Exists(Path.Combine(addLocal, fileName)))
                    {
                        FileStream outputStream = new FileStream(Path.Combine(addLocal, fileName), FileMode.Create);
                        long cl = response.ContentLength;
                        int bufferSize = 2048;
                        int readCount;
                        byte[] buffer = new byte[bufferSize];
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            outputStream.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                        }
                        outputStream.Flush();
                        outputStream.Close();
                    }

                    ftpStream.Flush();
                    ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.GhiLogs("Download", ex);
            }
        }

        public static byte[] DownloadFileToByte(string addressFtp)
        {

            byte[] result = null;
            try
            {
                WebClient request = new WebClient();

                string url = new Uri(Config.DomainDownload + addressFtp).AbsoluteUri;
                request.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);

                result = request.DownloadData(url);
            }
            catch (WebException ex)
            {
                Helper.GhiLogs("Download", ex);
            }
            return result;
        }

        private static byte[] StreamToByte(Stream input, int leng)
        {
            byte[] b;

            using (BinaryReader br = new BinaryReader(input))
            {
                b = br.ReadBytes(leng);
            }
            return b;
        }

        private static void DeleteFolderAndFileTemp(DirectoryInfo dinfo)
        {

            //  DirectoryInfo dinfo = new DirectoryInfo(path);
            if (dinfo.Exists)
            {
                try
                {
                    var subDir = dinfo.GetDirectories();
                    if (subDir.Length > 0)
                    {
                        foreach (var item in subDir)
                        {
                            DeleteFolderAndFileTemp(item);
                        }
                    }
                    var files = dinfo.GetFiles();
                    if (files.Length > 0)
                    {
                        foreach (var item in files)
                        {
                            if (item.LastAccessTime < DateTime.Now.AddDays(-1))
                                item.Delete();
                        }
                    }
                    if (dinfo.GetDirectories().Length > 0 && dinfo.GetFiles().Length > 0)
                    {
                        dinfo.Delete();
                    }
                }
                catch (Exception ex)
                {
                    Helper.GhiLogs("Download", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="pathTemp"></param>
        /// <returns></returns>
        public static string getFolderContainFile(string strPath, string pathTemp)
        {
            string path = pathTemp;
            if (CheckFileExistOnFtp(strPath))
            {
                int ExtractPos = strPath.LastIndexOf("\\") + 1;
                path = strPath.Substring(0, ExtractPos);
                if (!FtpDirectoryExists(Config.DomainDownload + path, Config.FtpUserID, Config.FtpPassWord))
                {
                    CreateFolderOnFtp(path);
                }
            }
            else
            {
                CreateFolderOnFtp(pathTemp);// Utility.CreateFolderOnServer(pathTemp);
            }
            return path;
        }
        /// <summary>
        /// Kiểm tra thư mục trên ftp đã tồn tại hay chưa
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public static bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            bool IsExists = true;
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(directoryPath));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
                response = (FtpWebResponse)reqFTP.GetResponse();

                //Stream ftpStream = response.GetResponseStream();
                //ftpStream.Flush();
                //ftpStream.Close();                       
                response.Close();

            }
            catch (WebException ex)
            {
                IsExists = true;
                if (response != null)
                {
                    response.Close();
                }
                Helper.GhiLogs("Download", ex);
            }

            return IsExists;
        }
        /// <summary>
        /// upload file to ftp and delete file exits
        /// </summary>
        /// <param name="fileNameDel">name file exits</param>
        /// <param name="pathFileDel">path file exits</param>
        /// <param name="fileData">byte file save</param>
        /// <param name="fileNamePath">file name save to ftp</param>
        /// <param name="urlString">path save</param>
        /// <returns>path save</returns>
        public static string UploadFileToFtpExits(string fileNameDel, string pathFileDel, byte[] fileData, string fileName)
        {
            string pathSave = string.Empty;
            FileDeleteFromFTP(fileNameDel);
            string pth = getFolderContainFile(fileNameDel, pathFileDel);
            bool result = FtpDirectoryExists(Config.DomainDownload + pth, Config.FtpUserID, Config.FtpPassWord);
            if (result)
            {
                RemoteUpload uploadClient = null;
                uploadClient = new FtpRemoteUpload(fileData,
                    fileName, Config.DomainDownload + pth);
                if (uploadClient.UploadFile())
                {
                    Utility.LogEvent("Upload is complete");
                    pathSave = pth.EndsWith("/") ? pth + fileName : pth + "/" + fileName;
                }
                else
                {
                    Utility.LogEvent("Failed to upload");
                }

            }
            return pathSave;

        }
        /// <summary>
        /// Upload file 
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="fileName"></param>
        public static string UploadFileToFtpNews(byte[] fileData, string pathFile, string fileName)
        {
            //  bool ug = FtpDirectoryExists(Config.DomainDownload, Config.FtpUserID, Config.FtpPassWord);
            string pathSave = string.Empty;
            CreateFolderOnFtp(pathFile);
            bool result = FtpDirectoryExists(Config.DomainDownload + pathFile, Config.FtpUserID, Config.FtpPassWord);
            if (result)
            {
                int i = 0;
                string fileNameSave = fileName;
                string pathCheck = pathFile.EndsWith("/") ? pathFile + fileNameSave : pathFile + "/" + fileNameSave;
                while (CheckFileExistOnFtp(pathCheck))
                {
                    fileNameSave = fileName.Replace(".", i + ".");// + i;
                    pathCheck = pathFile.EndsWith("/") ? pathFile + fileNameSave : pathFile + "/" + fileNameSave;
                    i++;
                }

                RemoteUpload uploadClient = null;
                uploadClient = new FtpRemoteUpload(fileData,
                    fileNameSave, Config.DomainDownload + pathFile);
                if (uploadClient.UploadFile())
                {
                    Utility.LogEvent("Upload is complete");
                    pathSave = pathFile.EndsWith("/") ? pathFile + fileNameSave : pathFile + "/" + fileNameSave;

                }
                else
                {
                    Utility.LogEvent("Failed to upload");
                }

            }
            return pathSave;

        }
        public static void CreateFolderOnLocal(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static bool CreateFolderOnFtp(string path)
        {
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            try
            {
                string[] arrPath = path.Split('/');
                string pathInit = Config.DomainDownload;
                for (int i = 0; i < arrPath.Length; i++)
                {
                    pathInit = pathInit.EndsWith("/") ? pathInit + arrPath[i] : pathInit + "/" + arrPath[i];
                    if (!FtpDirectoryExists(pathInit, Config.FtpUserID, Config.FtpPassWord))
                    {
                        // dirName = name of the directory to create.
                        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(pathInit));
                        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        reqFTP.UseBinary = true;
                        reqFTP.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
                        response = (FtpWebResponse)reqFTP.GetResponse();

                        //Stream ftpStream = response.GetResponseStream();
                        //ftpStream.Flush();
                        //ftpStream.Close();                       
                        response.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                if (response != null)
                {
                    response.Close();
                }
                Helper.GhiLogs("Download", ex);
                return false;

            }
            return true;
        }
        public static bool FileDeleteFromFTP(string fileName)
        {
            bool flag = false;
            if (CheckFileExistOnFtp(fileName))
            {
                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(new Uri(Config.DomainDownload + fileName));
                requestFileDelete.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                flag = true;
            }
            return flag;

        }
        public static void RetrieveFileListFromFTPDirectory(string path)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Config.DomainDownload + path);
            request.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());

            string fileName = streamReader.ReadLine();

            while (fileName != null)
            {
                //Console.Writeline(fileName);
                fileName = streamReader.ReadLine();
            }

            request = null;
            streamReader = null;
        }
    }
}
