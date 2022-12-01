using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ADDJ.Core
{
    public class Utility
    {
        #region "Event Log"
        public static void LogEvent(string message, EventLogEntryType type)
        {
            string fileName = string.Concat("Logs_", DateTime.Now.ToString("yyyyMMdd"), ".txt");

            // Logs theo ngày
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string pathFolder = string.Concat(Config.LogPath, string.Concat(time, "\\"));
            if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);

            string fullFileName = Path.Combine(pathFolder, fileName);

            StreamWriter writer = null;
            string msgTime = string.Format("[{1}] Time: {2}{0}", Environment.NewLine, type.ToString(), DateTime.Now);

            // message = string.Format("[{0}] Time: {1}{3}{2}{3}", type.ToString(), DateTime.Now, message, Environment.NewLine);

            // Thêm Request
            string msgRequest = string.Empty;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                IHttpHandler handler = context.Handler;
                if (handler != null) // ".aspx", ".ashx"
                {
                    var sS = context.Session;
                    string msgUser = string.Empty;
                    if (sS != null) // Có Session
                    {
                        object ssUser = sS[Constant.SessionNameAccountAdmin];
                        if (ssUser != null && ssUser.ToString() != string.Empty)
                        {
                            string strUser = JsonConvert.SerializeObject(ssUser);
                            IEnumerable<KeyValuePair<string, JToken>> objUser = JsonConvert.DeserializeObject(strUser) as IEnumerable<KeyValuePair<string, JToken>>;
                            // Lấy thông tin User đang tương tác
                            string objUserName = ((JValue)objUser.SingleOrDefault(v => v.Key.ToLower() == "Username".ToLower()).Value).Value as string;

                            msgUser = string.Format("UserName: {0}{1}", objUserName, Environment.NewLine);

                        }
                    }

                    string urlRefer = string.Empty;
                    Uri obj = context.Request.UrlReferrer;
                    if (obj != null) urlRefer = string.Format("UrlReferrer: {0}{1}", obj.AbsolutePath, Environment.NewLine);

                    string method = string.Format("Method: {0}{1}", context.Request.HttpMethod, Environment.NewLine);

                    msgRequest = string.Format("Request: {0}{1}{3}{4}{2}", context.Request.RawUrl, Environment.NewLine, msgUser, urlRefer, method);
                }
            }


            message = string.Concat(msgTime, msgRequest, string.Format("{1}{0}", Environment.NewLine, message));

            try
            {
                writer = new StreamWriter(fullFileName, true);
                writer.WriteLine(message);
            }
            catch { }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer.Close();
                }
            }
        }
        public static void LogEventService(string message, EventLogEntryType type)
        {
            // Edited by	: Dao Van Duong
            // Datetime		: 2.8.2016 17:35
            // Note			: Tên file sẽ được lưu log
            string fileName = string.Concat("LogServices_", DateTime.Now.ToString("yyyyMMdd"), ".txt");
            string time = DateTime.Now.ToString("yyyy-MM-dd"); // Thư mục ngày tháng sẽ lưu
            string path = string.Concat(Config.LogPath, string.Concat(time, "\\"));

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string Source = Path.Combine(path, fileName);

            StreamWriter writer = null;
            message = "[" + type.ToString() + "] " + DateTime.Now.ToString() + ":" + message;

            try
            {
                writer = new StreamWriter(Source, true);
                writer.WriteLine(message);
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer.Close();
                }
            }
        }
        public static void LogEventService(Exception ex)
        {
            string message = "EXCEPTION: " + ex.GetType().Name;
            message += ex.Message + Environment.NewLine + ex.StackTrace;

            LogEventService(message, EventLogEntryType.Error);
        }
        public static void LogEventService(string message)
        {
            LogEventService(message, EventLogEntryType.Information);
        }
        public static void LogEventService(object obj)
        {
            string message = "Information: " + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            LogEventService(message, EventLogEntryType.Information);
        }
        public static void LogEvent(Exception ex)
        {
            string message = string.Format("Exception of type: {0}{1}", ex.GetType().Name, Environment.NewLine); // Loại lỗi
            message += string.Format("Message: {0}{1}", ex.Message, Environment.NewLine); // Thông điệp
            if (ex.StackTrace != null) message += string.Format("Detail: {0}", ex.StackTrace, Environment.NewLine); // Chi tiết lỗi

            LogEvent(message, EventLogEntryType.Error);
        }
        public static void LogEvent(string mess)
        {
            LogEvent(mess, EventLogEntryType.Information);
        }
        public static void LogEvent(string mess, Exception ex)
        {
            string message = string.Format("Exception of type: {0}{1}", ex.GetType().Name, Environment.NewLine); // Loại lỗi
            message += string.Format("Message: {0} && {2}{1}", mess, Environment.NewLine, ex.Message); // Thông điệp

            if (ex.StackTrace != null) message += string.Format("Detail: {0}", ex.StackTrace, Environment.NewLine); // Chi tiết lỗi
            LogEvent(message, EventLogEntryType.Error);
        }
        public static void LogEvent(object obj)
        {
            string message = "Information: " + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            LogEvent(message, EventLogEntryType.Information);
        }
        public static void LogEventADODB(Exception ex, EventLogEntryType type)
        {
            string message = "Exception: " + ex.GetType().Name;
            message += ex.Message + Environment.NewLine;
            message += ex.InnerException.Message + Environment.NewLine;
            message += ex.StackTrace;

            LogEvent(message, type);
        }
        public static void LogEventADODB(Exception ex)
        {
            string message = "Exception: " + ex.GetType().Name;
            message += ex.Message + Environment.NewLine;
            message += ex.InnerException.Message + Environment.NewLine;
            message += ex.StackTrace;

            LogEvent(message, EventLogEntryType.Error);
        }

        #endregion

        #region "Upload File To Server"
        public static long GetFileSizeOnDisk(string file)
        {
            FileInfo info = new FileInfo(file);
            return info.Length;
        }
        public static string UploadFile(HtmlInputFile clientFile, string folderToUp, bool autoGenerateName, bool overwrite, string limitExtension)
        {
            if ((!(clientFile == null)) && (!(clientFile.PostedFile == null))
                && !string.IsNullOrEmpty(clientFile.PostedFile.FileName))
            {
                try
                {
                    HttpPostedFile postedFile = clientFile.PostedFile;
                    string sFolder = folderToUp;
                    if (postedFile != null)
                    {
                        //Check exist folder
                        try
                        {
                            if (!Directory.Exists(sFolder))
                            {
                                Directory.CreateDirectory(sFolder);
                            }
                        }
                        catch
                        {
                            throw new Exception("Thư mục upload chưa được chỉ định quyền ghi dữ liệu");
                        }

                        //Check validate file extension
                        string fileExtension = Path.GetExtension(postedFile.FileName.ToLower());
                        limitExtension = limitExtension.ToLower();
                        if (limitExtension.IndexOf("*.*") == -1 && limitExtension.IndexOf(fileExtension) == -1)
                        {
                            throw new Exception("Không cho upload định dạng file này");
                        }

                        //Generate file name and check overwrite
                        string fileName = Path.GetFileName(postedFile.FileName);
                        string vFileName = fileName;

                        if (autoGenerateName)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf("."));
                            vFileName = fileName.Insert(fileName.LastIndexOf("."),
                                                        DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                        }

                        vFileName = vFileName.Replace(" ", string.Empty);

                        if (UploadFile(postedFile.InputStream, folderToUp, vFileName, false))
                            return vFileName;
                        throw new Exception("Upload file không thành công!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return string.Empty;
        }
        public static string UploadFile(System.Web.UI.WebControls.FileUpload clientFile, string folderToUp, bool autoGenerateName, bool overwrite, string limitExtension)
        {
            if ((!(clientFile == null)) && (!(clientFile.PostedFile == null))
                && !string.IsNullOrEmpty(clientFile.PostedFile.FileName))
            {
                try
                {
                    HttpPostedFile postedFile = clientFile.PostedFile;
                    string sFolder = folderToUp;
                    if (postedFile != null)
                    {
                        //Check exist folder
                        try
                        {
                            if (!Directory.Exists(sFolder))
                            {
                                Directory.CreateDirectory(sFolder);
                            }
                        }
                        catch
                        {
                            throw new Exception("Thư mục upload chưa được chỉ định quyền ghi dữ liệu");
                        }

                        //Check validate file extension
                        string fileExtension = Path.GetExtension(postedFile.FileName.ToLower());
                        limitExtension = limitExtension.ToLower();
                        if (limitExtension.IndexOf("*.*") == -1 && limitExtension.IndexOf(fileExtension) == -1)
                        {
                            throw new Exception("Không cho upload định dạng file này");
                        }

                        //Generate file name and check overwrite
                        string fileName = Path.GetFileName(postedFile.FileName);
                        string vFileName = fileName;

                        if (autoGenerateName)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf("."));
                            vFileName = fileName.Insert(fileName.LastIndexOf("."),
                                                        DateTime.Now.ToString("yyyyMMdd_hhmmss") + DateTime.Now.Millisecond.ToString());
                        }

                        vFileName = vFileName.Replace(" ", string.Empty);

                        if (UploadFile(postedFile.InputStream, folderToUp, vFileName, false))
                            return vFileName;
                        throw new Exception("Upload file không thành công!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return string.Empty;
        }
        public static string UploadFile(HttpPostedFile postedFile, string folderToUp, bool autoGenerateName, bool overwrite, string limitExtension)
        {
            if ((!(postedFile == null))
                && !string.IsNullOrEmpty(postedFile.FileName))
            {
                try
                {
                    //HttpPostedFile postedFile = clientFile.PostedFile;
                    string sFolder = folderToUp;
                    if (postedFile != null)
                    {
                        //Check exist folder
                        try
                        {
                            if (!Directory.Exists(sFolder))
                            {
                                Directory.CreateDirectory(sFolder);
                            }
                        }
                        catch
                        {
                            throw new Exception("Thư mục upload chưa được chỉ định quyền ghi dữ liệu");
                        }

                        //Check validate file extension
                        string fileExtension = Path.GetExtension(postedFile.FileName.ToLower());
                        limitExtension = limitExtension.ToLower();
                        if (limitExtension.IndexOf("*.*") == -1 && limitExtension.IndexOf(fileExtension) == -1)
                        {
                            throw new Exception("Không cho upload định dạng file này");
                        }

                        //Generate file name and check overwrite
                        string fileName = Path.GetFileName(postedFile.FileName);
                        string vFileName = fileName;

                        if (autoGenerateName)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf("."));
                            vFileName = fileName.Insert(fileName.LastIndexOf("."),
                                                        DateTime.Now.ToString("yyyyMMdd_hhmmss") + DateTime.Now.Millisecond.ToString());
                        }

                        vFileName = vFileName.Replace(" ", string.Empty);

                        if (UploadFile(postedFile.InputStream, folderToUp, vFileName, false))
                            return vFileName;
                        throw new Exception("Upload file không thành công!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return string.Empty;
        }
        public static string UploadFileByte(HttpPostedFile postedFile, string folderToUp, bool autoGenerateName,
                                       bool overwrite, string limitExtension)
        {
            if ((!(postedFile == null)) &&
                !string.IsNullOrEmpty(postedFile.FileName))
            {
                try
                {
                    //HttpPostedFile postedFile = clientFile.PostedFile;
                    string sFolder = folderToUp;
                    if (postedFile != null)
                    {
                        //Check exist folder
                        try
                        {
                            if (!Directory.Exists(sFolder))
                            {
                                Directory.CreateDirectory(sFolder);
                            }
                        }
                        catch
                        {
                            throw new Exception("Thư mục upload chưa được chỉ định quyền ghi dữ liệu");
                        }

                        //Check validate file extension
                        string fileExtension = Path.GetExtension(postedFile.FileName.ToLower());
                        limitExtension = limitExtension.ToLower();
                        if (limitExtension.IndexOf("*.*") == -1 && limitExtension.IndexOf(fileExtension) == -1)
                        {
                            throw new Exception("Không cho upload định dạng file này");
                        }

                        //Generate file name and check overwrite
                        string fileName = Path.GetFileName(postedFile.FileName);
                        string vFileName = fileName;

                        if (autoGenerateName)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf("."));
                            vFileName = fileName.Insert(fileName.LastIndexOf("."),
                                                        DateTime.Now.ToString("yyyyMMdd_hhmmss") + DateTime.Now.Millisecond.ToString());
                        }

                        vFileName = vFileName.Replace(" ", string.Empty);

                        if (UploadFile(postedFile.InputStream, folderToUp, vFileName, false))
                            return vFileName;
                        throw new Exception("Upload file không thành công!");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return string.Empty;
        }

        public static string UploadFile(HtmlInputFile clientFile, string strFileName, string folderToUp, bool overwrite,
                                        string limitExtension)
        {
            if ((clientFile != null) && (clientFile.PostedFile != null) &&
                !string.IsNullOrEmpty(clientFile.PostedFile.FileName))
            {
                try
                {
                    HttpPostedFile postedFile = clientFile.PostedFile;
                    //string ContentTypeFile = postedFile.ContentType;

                    {
                        string sFolder = folderToUp;
                        try
                        {
                            if (Directory.Exists(sFolder) == false)
                            {
                                Directory.CreateDirectory(sFolder);
                            }
                        }
                        catch
                        {
                            return string.Empty;
                        }

                        //Check validate file extension
                        string fileExtension = Path.GetExtension(postedFile.FileName.ToLower());
                        limitExtension = limitExtension.ToLower();
                        if (limitExtension.IndexOf("*.*") == -1 && limitExtension.IndexOf(fileExtension) == -1)
                            return "";

                        string vFileName = strFileName;

                        UploadFile(postedFile.InputStream, folderToUp, vFileName, overwrite);
                        return vFileName;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private static bool UploadFile(Stream inputStream, string uploadPath, string fileName, bool overwrite)
        {
            if (overwrite && File.Exists(Path.Combine(uploadPath, fileName)))
            {
                File.Delete(Path.Combine(uploadPath, fileName));
            }

            bool result = true;
            string filePath = Path.Combine(uploadPath, fileName);
            string dir = Path.GetDirectoryName(filePath);

            try
            {
                using (FileStream outputStream = new FileStream(filePath, FileMode.Create))
                {
                    StreamCopyToStream(inputStream, outputStream);

                    inputStream.Flush();
                    outputStream.Flush();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private static void StreamCopyToStream(Stream input, Stream output)
        {
            const int bufferSize = 2048;
            var buffer = new byte[bufferSize];
            int bytes;
            input.Position = 0;
            while ((bytes = input.Read(buffer, 0, bufferSize)) > 0)
            {
                output.Write(buffer, 0, bytes);
            }
        }

        public static string CreateFolder(string FolderName)
        {
            if ((!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + FolderName)))
            {
                DirectoryInfo oDirectoryInfo = default(DirectoryInfo);
                oDirectoryInfo = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + FolderName);
            }
            return FolderName;
        }

        #endregion

        #region Read File
        public static string ReadFile(string path)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
                return reader.ReadToEnd();
            }
            catch { return string.Empty; }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        #endregion

        #region Resize Image

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            //byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void ImageNoResize(byte[] data, string despath, long quality)
        {
            Stream stream = new MemoryStream(data);
            var sourceBitmap = new Bitmap(stream);
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            var param = new EncoderParameters(1);
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            sourceBitmap.Save(despath, info[1], param);
        }

        public static void ImageResize(string filePath, string desPath, bool overwrite, int thumbWidth, int thumbHeight, long quality)
        {
            if (File.Exists(desPath))
                if (overwrite)
                    File.Delete(desPath);
                else
                    return;

            if (!Directory.Exists(Path.GetDirectoryName(desPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(desPath));

            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] data = ReadFully(fs);
            Stream stream = new MemoryStream(data);
            Bitmap sourceBitmap = new Bitmap(stream);
            Bitmap thumbBitmap = new Bitmap(thumbWidth, thumbHeight);
            Graphics g = Graphics.FromImage(thumbBitmap);
            try
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, thumbWidth, thumbHeight);
                g.DrawImage(sourceBitmap, 0, 0, thumbWidth, thumbHeight);

                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                thumbBitmap.Save(desPath, info[1], param);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                // logger.Error(ex);
            }
            finally
            {
                if (fs != null) fs.Close();
                sourceBitmap.Dispose();
                thumbBitmap.Dispose();
                g.Dispose();
            }
        }

        public static byte[] SaveData(string file, int size, long quality)
        {
            var sourceBitmap = new Bitmap(file);

            int h = sourceBitmap.Height;
            int w = sourceBitmap.Width;

            int nw = 0;
            int nh = 0;
            if (w > h)
            {
                nh = size;
                nw = w * nh / h;
            }
            else
            {
                nw = size;
                nh = h * nw / w;
            }

            Bitmap thumbBitmap = new Bitmap(nw, nh);
            Graphics g = Graphics.FromImage(thumbBitmap);
            try
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, nw, nh);
                g.DrawImage(sourceBitmap, 0, 0, nw, nh);

                var info = ImageCodecInfo.GetImageEncoders();
                var param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                MemoryStream ms = new MemoryStream();
                thumbBitmap.Save(ms, info[1], param);

                return ms.GetBuffer();
            }
            catch
            {
                //  logger.Error(ex);
                return null;
            }
            finally
            {
                sourceBitmap.Dispose();
                thumbBitmap.Dispose();
                g.Dispose();
            }
        }

        public static void ImageCrop(byte[] data, string desPath, int width, int height, long quality)
        {
            Stream stream = new MemoryStream(data);
            Image imgPhoto = new Bitmap(stream);
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int destX = 0;
            int destY = 0;

            float nPercent;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (width / (float)sourceWidth);
            nPercentH = (height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                destY = (int)((height - (sourceHeight * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentH;
                destX = (int)((width - (sourceWidth * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            if (nPercentH < nPercentW)
            {
                destWidth += 1;
            }
            else
            {
                destHeight += 1;
            }

            Bitmap bmPhoto = new Bitmap(width, height);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            try
            {
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(imgPhoto,
                                  new Rectangle(destX, destY, destWidth, destHeight),
                                  new Rectangle(0, 0, sourceWidth, sourceHeight),
                                  GraphicsUnit.Pixel);

                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                bmPhoto.Save(desPath, info[1], param);
            }
            catch
            {
                //logger.Error(ex);
            }
            finally
            {
                bmPhoto.Dispose();
                stream.Close();
                stream.Dispose();
                imgPhoto.Dispose();
            }
        }


        public static byte[] Crop(string fileImg, int width, int height, int x, int y)
        {
            using (Image originalImage = Image.FromFile(fileImg))
            {
                using (Bitmap bmp = new Bitmap(width, height))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (var graphic = Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.DrawImage(originalImage, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
                        var ms = new MemoryStream();
                        bmp.Save(ms, originalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
        }

        #endregion

        #region "Read file Excel"

        public static DataTable ExcelToDataTable(string filelocation)
        {
            try
            {
                var excelCommand = new OleDbCommand();

                var excelDataAdapter = new OleDbDataAdapter();

                string excelConnStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filelocation + "; Extended Properties =Excel 8.0;";

                var excelConn = new OleDbConnection(excelConnStr);

                excelConn.Open();

                DataTable dtSheetName = excelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dtSheetName.Rows.Count > 0)
                {
                    var dtPatterns = new DataTable();

                    //excelCommand = new OleDbCommand("Select * from [" + dtSheetName.Rows[0]["TABLE_NAME"].ToString() + "] order by trdate,trref", excelConn);
                    excelCommand = new OleDbCommand("Select * from [" + dtSheetName.Rows[0]["TABLE_NAME"] + "]",
                                                    excelConn);

                    excelDataAdapter.SelectCommand = excelCommand;

                    excelDataAdapter.Fill(dtPatterns);

                    excelCommand.Dispose();
                    excelConn.Close();
                    excelConn.Dispose();

                    return dtPatterns;
                }
                return null;
            }
            catch (Exception ex)
            {
                Helper.GhiLogs(ex);
                throw new Exception("Hiện tại hệ thống server đang bận, bạn hãy quay lại vào lúc khác.");
            }
            finally
            {
                if (File.Exists(filelocation)) File.Delete(filelocation);
            }
        }

        #endregion

        #region Datetime

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddMMyyyy"></param>
        /// <returns></returns>
        public static DateTime fGetDateTimeBy(string ddMMyyyy)
        {
            try
            {
                string[] sArr = ddMMyyyy.Split('/');
                if (sArr.Length == 3)
                {
                    int dd = 0;
                    int MM = 0;
                    int yyyy = 0;
                    if (int.TryParse(sArr[0], out dd) && dd > 0 && dd <= 31)
                    {
                        if (int.TryParse(sArr[1], out MM) && MM > 0 && MM <= 12)
                            if (int.TryParse(sArr[2], out yyyy) && yyyy > 1987 && yyyy <= 9999)
                                return (new DateTime(yyyy, MM, dd));
                    }
                }
                return DateTime.MinValue;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Kiểm tra xem dữ liệu vào đã đúng định dạng dd/MM/yyyy hay chưa?
        /// Nếu là dạng d/M/yyyy thì đưa về dạng dd/MM/yyyy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string fFormatToDDMMYYYY(string input)
        {
            try
            {
                string result = string.Empty;
                input = input.Trim();
                if (input.Length == 0)
                {
                    return string.Empty;
                }
                if (input.Length < 8 || input.Length > 10)
                {
                    throw new Exception("Ngày tháng phải định dạng theo dd/MM/yyyy.");
                }
                string[] sArr = input.Split('/');
                if (sArr.Length != 3)
                {
                    throw new Exception("Ngày tháng phải định dạng theo dd/MM/yyyy.");
                }
                int dd = int.Parse(sArr[0]);
                int MM = int.Parse(sArr[1]);
                int yyyy = int.Parse(sArr[2]);
                if (MM == 1 || MM == 3 || MM == 5 || MM == 7 || MM == 8 || MM == 10 || MM == 12)
                {
                    if (dd <= 0 || dd > 31)
                        throw new Exception("Ngày không hợp lệ.");
                }
                else if (MM == 4 || MM == 6 || MM == 9 || MM == 11)
                {
                    if (dd <= 0 || dd > 30)
                        throw new Exception("Tháng " + MM + " chỉ có 30 ngày.");
                }
                else if (MM == 2)
                {
                    if (yyyy % 4 == 0)
                    {
                        if (dd <= 0 || dd > 29)
                            throw new Exception("Năm nhuận tháng 2 chỉ có 29 ngày.");
                    }
                    else if (dd <= 0 || dd > 28)
                    {
                        throw new Exception("Tháng 2 chỉ có 28 ngày.");
                    }
                }
                else
                {
                    throw new Exception("Tháng không hợp lệ.");
                }

                if (dd < 10)
                    result += "0" + dd;
                else
                    result += dd.ToString();

                result += "/";
                if (MM <= 0 || MM > 12)
                {
                    throw new Exception("Tháng không hợp lệ.");
                }
                else if (MM < 10)
                    result += "0" + MM;
                else
                    result += MM.ToString();

                result += "/";
                if (yyyy < 1900 || yyyy > 9999)
                {
                    throw new Exception("Năm không hợp lệ.");
                }
                else
                    result += yyyy;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Datetime

        #region Functions Utility for Numberic

        public static int InitializeInteger => 0;

        public static int InitializeDouble => 0;

        public static bool IsInteger(object obj)
        {
            try
            {
                if (obj == null) return false;

                Convert.ToInt32(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsIntegerNull(object obj)
        {
            if (obj == null) return true;
            if (IsInteger(obj) && obj.ToString().Trim() == "") return true;
            if (IsInteger(obj) && obj.ToString().Trim() == "0") return true;
            return false;
        }


        public static bool IsDouble(object obj)
        {
            try
            {
                Convert.ToDouble(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Functions Utility for DateTime

        public static DateTime InitializeDateTime => Convert.ToDateTime("01/01/1900");

        public static bool IsDateTime(object obj)
        {
            try
            {
                Convert.ToDateTime(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTimeNull(object obj)
        {
            if (obj == null) return true;
            if (IsDateTime(obj) && obj.ToString().Trim() == "") return true;
            if (IsDateTime(obj) && Convert.ToDateTime(obj) == DateTime.MinValue) return true;
            if (IsDateTime(obj) && obj.ToString().IndexOf("1/1/1900") > -1) return true;
            if (IsDateTime(obj) && obj.ToString().IndexOf("01/01/1900") > -1) return true;
            return false;
        }

        public static int TongSoNgayNghiT7CN(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.EndOfDay() > denNgay.EndOfDay()) return 0;

            tuNgay = tuNgay.StartOfDay();
            denNgay = denNgay.EndOfDay();

            DateTime FirstSunday = tuNgay.DayOfWeek == DayOfWeek.Sunday ? tuNgay : tuNgay.AddDays(7 - (double)tuNgay.DayOfWeek); // ngày chủ nhật đầu tiên
            DateTime LastSunday = denNgay.DayOfWeek == DayOfWeek.Sunday ? denNgay : denNgay.AddDays(-(double)denNgay.DayOfWeek); // ngày chủ nhật cuối cùng
            int SundayCount = LastSunday.Subtract(FirstSunday).Days / 7 + 1; // tổng số ngày chủ nhật


            DateTime FirstSaturday = tuNgay.DayOfWeek == DayOfWeek.Saturday ? tuNgay : tuNgay.AddDays(7 - (double)tuNgay.DayOfWeek - 1);
            DateTime LastSaturday = denNgay.DayOfWeek == DayOfWeek.Saturday ? denNgay : denNgay.AddDays(-(double)denNgay.DayOfWeek - 1);

            int SaturdayCount = LastSaturday.Subtract(FirstSaturday).Days / 7 + 1;

            return SaturdayCount + SundayCount;
        }

        #endregion

        #region Functions Utility for String

        public static string InitializeString => string.Empty;

        public static bool IsStringNullOrEmpty(object obj)
        {
            if (obj == null) return true;
            if (obj.ToString().Trim() == string.Empty) return true;
            if (obj.ToString().Trim() == "") return true;
            return false;
        }

        public static bool IsMultilineNullOrEmpty(object obj)
        {
            if (obj == null) return true;
            if (obj.ToString() == string.Empty) return true;
            if (obj.ToString() == "") return true;
            return false;
        }

        public static string CollapseString(string value, int length)
        {
            if (value.Length > length)
            {
                return value.Substring(0, length) + "...";
            }
            return value;
        }

        #endregion

        #region Functions Utility for Boolean

        public static bool InitializeBool => false;

        #endregion

        #region Functions Utility
        public static List<string> GetTag(string str)
        {
            string strRegex = "<(?<tag>.*?)>";
            Match mt = (new Regex(strRegex)).Match(str);
            List<string> listTag = new List<string>();
            while (mt.Success)
            {
                listTag.Add(mt.Value);
                mt = mt.NextMatch();
            }
            if (listTag.Count > 0)
                return listTag;
            else
                return null;
        }

        public static string RemoveAllTag(string str)
        {
            string strReturn = str.Replace("TEXT:", "");
            List<string> listTag = GetTag(str);
            if (listTag != null && listTag.Count > 0)
                foreach (string item in listTag)
                {
                    if (item.ToLower().Contains("<br"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (item.ToLower().Contains("<p>"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (item.ToLower().Contains("</p>"))
                        strReturn = strReturn.Replace(item, Environment.NewLine);
                    else if (strReturn.Contains(item))
                        strReturn = strReturn.Replace(item, "");
                }
            strReturn = strReturn.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
            return strReturn.Trim();
        }

        #endregion
        public static string GetIP()
        {
            try
            {
                string strIpAddress = null;
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (strIpAddress == null)
                        strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (strIpAddress == null)
                {
                    string strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
                    if (ipHostInfo.AddressList.Length > 2)
                    {
                        IPAddress ipAddress = ipHostInfo.AddressList[2];
                        strIpAddress = ipAddress.ToString();
                    }
                }

                if (string.IsNullOrEmpty(strIpAddress))
                    strIpAddress = "10.146.34.231";
                return strIpAddress;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return "10.146.34.231";
            }
        }
        public static string ConvertIntToDate(string time, string seprate)
        {
            if (time.Length != 8) return "";
            var nam = time.Substring(0, 4);
            var thang = time.Substring(4, 2);
            var ngay = time.Substring(6, 2);
            return string.Format("{0}{3}{1}{3}{2}", ngay, thang, nam, seprate);
        }

        #region Utility Remove Tag HTML
        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string RemoveTagComment(string str)
        {
            if (str != null && str.Trim() != "")
            {
                string strReturn = str;
                string strRegex = "<!--(?<tag>.*?)-->|<script.*[^CR]*?</script>|<SCRIPT.*[^CR]*?</SCRIPT>";
                Match mt = (new Regex(strRegex)).Match(str);
                while (mt.Success)
                {
                    strReturn = strReturn.Replace(mt.Value, "");
                    mt = mt.NextMatch();
                }
                return strReturn.Trim();
            }
            return "";
        }

        public static string RemoveTagStyle(string str)
        {
            if (str != null && str.Trim() != "")
            {
                string strReturn = str;
                string strRegex = "<(script|style)\\b[^>]*?>.*?</\\1>";
                return Regex.Replace(str, strRegex, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            return "";
        }
        #endregion

        #region Process Dir
        public static bool DeleteDirectory(string sPathDirectoryName)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(sPathDirectoryName);
                if (EmptyDirectory(dInfo))
                {
                    dInfo.Delete(true);
                }
            }
            catch
            {

                return false;
            }
            return true;
        }
        private static bool EmptyDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {

                    file.Delete();
                }
                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    EmptyDirectory(subfolder);
                }
            }
            catch { return false; }
            return true;
        }
        #endregion
        public static void Tooltip(System.Web.UI.WebControls.ListControl lc)
        {
            for (int d = 0; d < lc.Items.Count; d++)
            {
                lc.Items[d].Attributes.Add("title", lc.Items[d].Text);
            }
        }
    }
}