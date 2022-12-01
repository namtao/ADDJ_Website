using System;
using System.IO;
using System.Web;
using AIVietNam.Core;
using Website.AppCode;
using Website.AppCode.Controller;
using AIVietNam.GQKN.Entity;

namespace Website.Views.ChiTietKhieuNai
{
    public partial class Dowload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string id = Request.QueryString["Id"];
                string urlFile = string.Empty;
                KhieuNai_FileDinhKemInfo fileDinhKem = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetInfo(Convert.ToInt32(id));
                if (fileDinhKem != null)
                {
                    urlFile = Config.DomainDownload + fileDinhKem.URLFile;

                    if (RemoteFileToFtp.CheckFileExistOnFtp(fileDinhKem.URLFile))
                    {
                        string FileName = Common.GetFileName(fileDinhKem.TenFile);
                        string pathLocal = Common.GetPathLocal(fileDinhKem.URLFile);
                        string path = HttpContext.Current.ApplicationInstance.Server.MapPath(pathLocal);
                        RemoteFileToFtp.DownloadFileToLocal(fileDinhKem.URLFile, path, HttpContext.Current.ApplicationInstance.Server.MapPath("/FtpClient/"));
                        string filePath = path.EndsWith("/") ? path + FileName : path + "/" + fileDinhKem.URLFile.Substring(fileDinhKem.URLFile.LastIndexOf('/'));
                        FileInfo file = new FileInfo(filePath);
                        if (file.Exists)
                        {
                            try
                            {
                                BuildKhieuNai_Log.LogKhieuNai(fileDinhKem.KhieuNaiId, "Tải file " + fileDinhKem.TenFile, string.Empty, string.Empty);
                            }
                            catch (Exception ex)
                            {
                                Helper.GhiLogs("Download", ex);
                            }
                            //return the file
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileDinhKem.TenFile);
                            Response.AddHeader("Content-Length", file.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.WriteFile(file.FullName);

                            Response.Flush();
                            Response.SuppressContent = true;
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            file.Delete();
                        }
                    }
                    // ResponseClient();
                }
            }
            catch (Exception ex)
            {
                Helper.GhiLogs("Download", ex);
            }
        }
        private void ResponseClient()
        {
            //using (WebClient ftpClient = new WebClient())
            //{
            //    ftpClient.Credentials = new System.Net.NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
            //    ftpClient.DownloadFile(urlFile, @"D:\abc.rar");
            //}
            //Utility.LogEvent(urlFile);
            //WebRequest myFtpWebRequest;

            //WebResponse myFtpWebResponse;
            //StreamWriter myStreamWriter;
            //string fileName = Path.GetFileName(urlFile);
            //myFtpWebRequest = WebRequest.Create(urlFile);

            //myFtpWebRequest.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);

            //myFtpWebRequest.Method = WebRequestMethods.File.DownloadFile;
            //// myFtpWebRequest.UseBinary = true;

            //myFtpWebResponse = myFtpWebRequest.GetResponse();

            //myStreamWriter = new StreamWriter(Server.MapPath(fileName));
            //var stream = myFtpWebResponse.GetResponseStream();
            //myStreamWriter.Write(new StreamReader(stream).ReadToEnd());
            //myStreamWriter.Close();

            //myFtpWebResponse.Close();
            //var reqFTP = (WebRequest)WebRequest.Create(new Uri(urlFile));
            //reqFTP.Credentials = new NetworkCredential(Config.FtpUserID, Config.FtpPassWord);
            //reqFTP.Method = WebRequestMethods.File.DownloadFile;
            ////reqFTP.UseBinary = true;
            //WebResponse response = reqFTP.GetResponse();
            //Stream ftpStream = response.GetResponseStream();
            //long cl = response.ContentLength;
            //int bufferSize = 2048;
            //int readCount;
            //byte[] buffer = new byte[bufferSize];
            //readCount = ftpStream.Read(buffer, 0, bufferSize);
            //while (readCount > 0)
            //{
            //    readCount = ftpStream.Read(buffer, 0, bufferSize);
            //}
            //ftpStream.Flush();
            //ftpStream.Close();
            //response.Close();
            //var fileInfo = new System.IO.FileInfo(urlFile);
            //Response.ContentType = "application/octet-stream";
            //Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", urlFile));
            //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            //Response.WriteFile(urlFile);
            //Response.End();
        }
    }
}
