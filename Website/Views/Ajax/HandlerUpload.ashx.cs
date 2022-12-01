using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Website.AppCode;
using Website.AppCode.Controller;

namespace Website.Views.Ajax
{
    /// <summary>
    /// Summary description for HandlerUpload
    /// </summary>
    public class HandlerUpload : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var MaKN = context.Request.Form["MaKN"] ?? context.Request.QueryString["MaKN"];
            var ghichu = context.Request.Form["ghichu"] ?? context.Request.QueryString["ghichu"];
            var fType = context.Request.Form["fType"] ?? context.Request.QueryString["fType"];
            var strGhiChu = HttpUtility.UrlDecode(ghichu.ToString());

            var uInfo = LoginAdmin.AdminLogin();
            var json = "{ \"ErrorId\" : \"0\" }";
            if (context.Request.Files == null)
            {
                context.Response.Write(json);
                return;
            }

            var htc = context.Request.Files;

            if (htc.Count == 0)
            {
                context.Response.Write(json);
                return;
            }

            var file1 = htc[0];

            if (file1.ContentLength <= 0)
            {

                context.Response.Write(json);
                return;
            }
            var userLogin = LoginAdmin.AdminLogin();
            if (userLogin == null)
            {
                json = "{ \"ErrorId\" : \"-1\" }";
                context.Response.Write(json);
                return;
            }

            KhieuNaiInfo info = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(MaKN));
            KhieuNai_FileDinhKemInfo fileInfo = new KhieuNai_FileDinhKemInfo();

            fileInfo.TenFile = Path.GetFileName(file1.FileName);
            fileInfo.LoaiFile = Path.GetExtension(file1.FileName);
            fileInfo.KichThuoc = (decimal)(file1.ContentLength / 1024);

            byte[] bytes = null;

            string pathDel = string.Empty;
            var strFileName = AIVietNam.GQKN.Impl.KhieuNai_FileDinhKemImpl.UploadFile(file1, ConvertUtility.ToInt32(MaKN), info.CDate, ref pathDel);
            if (!string.IsNullOrEmpty(pathDel))
            {
                if (info.KhieuNaiFrom == 1)
                {
                    bytes = FileToByteArray(pathDel);
                }
                if (File.Exists(pathDel))
                    File.Delete(pathDel);
            }

            fileInfo.KhieuNaiId = info.Id;
            fileInfo.GhiChu = strGhiChu;
            fileInfo.Status = ConvertUtility.ToInt16(fType, 2);
            fileInfo.CUser = userLogin.Username;
            fileInfo.URLFile = strFileName;

            var id = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Add(fileInfo);
            if (id > 0)
            {
                if (info.KhieuNaiFrom == 1)
                {
                    var DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(info.DoiTacId);

                    if(DoiTacItem.DoiTacType== 4)
                    { 
                        BuildKhieuNai.ThemFileVNPT(info, fileInfo, file1.ContentType, bytes, DoiTacItem.MaDoiTac);
                    }
                    else
                    {
                        string strJoinClause = "left join PhongBan b on b.DoiTacId = a.Id left join KhieuNai_Activity c on c.PhongBanXuLyId = b.Id";
                        string strWhereClause = "c.KhieuNaiId = 212635 and a.DoiTacType = 4 and IsChuyenHNI = 1";
                        var lstDoiTac = ServiceFactory.GetInstanceDoiTac().GetListDynamicJoin("top 1 MaDoiTac", strJoinClause, strWhereClause, "c.Id DESC");
                        if (lstDoiTac != null && lstDoiTac.Count > 0)
                        {
                            var DoiTacSendFile = lstDoiTac[0];
                            BuildKhieuNai.ThemFileVNPT(info, fileInfo, file1.ContentType, bytes, DoiTacSendFile.MaDoiTac);
                        }
                    }
                }

                BuildKhieuNai_Log.LogKhieuNai(info.Id, "Thêm mới file đính kèm", "File đính kèm", "", fileInfo.TenFile);
            }
            else
            {
                BuildKhieuNai_Log.LogKhieuNai(info.Id, "Thêm mới thất bại", "File đính kèm", "", fileInfo.TenFile);
            }

            fileInfo.Id = id;
            fileInfo.URLFile = "/Views/ChiTietKhieuNai/Download.aspx?id=" + fileInfo.Id;
            fileInfo.LoaiFile = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            fileInfo.ErrorId = 0;
            json = Newtonsoft.Json.JsonConvert.SerializeObject(fileInfo);
            context.Response.Write(json);
            context.Response.End();
        }

        private static void StreamCopyToStream(Stream input, ref Stream output)
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



        private byte[] StreamToByte(FileStream input, int leng)
        {
            byte[] b;

            using (BinaryReader br = new BinaryReader(input))
            {
                b = br.ReadBytes(leng);
            }
            return b;
        }

        private void SaveFileFromByte(byte[] b, string fileName)
        {
            try
            {
                File.WriteAllBytes(fileName, b);
            }
            finally { }
        }

        public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null; FileStream fs = null;
            try
            {

                fs = new FileStream(fileName,
                                               FileMode.Open,
                                               FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileName).Length;
                buff = br.ReadBytes((int)numBytes);
                return buff;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private bool CheckFileType(string fileExtension)
        {
            //string fileExtension = Path.GetExtension(filename).ToLower();
            bool kt = false;
            switch (fileExtension)
            {
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".ppt":
                case ".pptx":
                case ".zip":
                case ".rar":
                case ".jpg":
                case ".7z":
                case ".mp3":
                    kt = true;
                    break;
            }
            return kt;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}