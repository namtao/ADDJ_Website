using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Entity;
using Website.AppCode;
using AIVietNam.GQKN.Impl;
using AIVietNam.Core;
using System.IO;
using AIVietNam.Admin;
using System.Text.RegularExpressions;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class SoTheoDoiEdit : System.Web.UI.UserControl
    {
        KhieuNaiImpl _KhieuNaiImpl = new KhieuNaiImpl();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        public String GetValue()
        {
            return "-OKES";
        }
        public string UploadFile()
        {
            string strValues = "0";
            HttpFileCollection FileCollection = Request.Files;            
            int khieuNaiId = 0;
            
            try
            {
                khieuNaiId = Convert.ToInt32(HiddenField1.Value);
            }
            catch
            {
                khieuNaiId = Convert.ToInt32(HiddenField1.Value.Replace("\"", ""));                
            }
            KhieuNaiInfo info = _KhieuNaiImpl.GetInfo(khieuNaiId);
            var userLogin = LoginAdmin.AdminLogin();
            if (FileCollection.Count > 0)
            {
                for (int i = 0; i < FileCollection.Count; i++)
                {
                    HttpPostedFile PostedFile = FileCollection[i];
                    if (!string.IsNullOrEmpty(PostedFile.FileName))
                    {
                        try
                        {
                            KhieuNai_FileDinhKemInfo fileInfo = new KhieuNai_FileDinhKemInfo();
                            fileInfo.TenFile = Path.GetFileName(PostedFile.FileName);
                            fileInfo.LoaiFile = Path.GetExtension(PostedFile.FileName);
                            fileInfo.KichThuoc = PostedFile.ContentLength / 1024;

                            var strFileName = KhieuNai_FileDinhKemImpl.UploadFileDelTemp(PostedFile, khieuNaiId, info.CDate);

                            fileInfo.KhieuNaiId = khieuNaiId;
                            fileInfo.GhiChu = "File KH gửi";
                            fileInfo.Status = (byte)FileDinhKem_Status.File_KH_Gửi;
                            fileInfo.CUser = userLogin.Username;
                            fileInfo.URLFile = strFileName;
                            ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Add(fileInfo);

                        }
                        catch
                        {
                            strValues = "-1";
                            return strValues;
                        }

                    }

                }
                strValues = "1";
            }

            return strValues;
        }
        //public string UploadFile()
        //{
        //    string strValues = "0";
        //    int khieuNaiId = 0;
        //    try
        //    {
        //        khieuNaiId = Convert.ToInt32(HiddenField1.Value);
        //    }
        //    catch {
        //        khieuNaiId = Convert.ToInt32(HiddenField1.Value.Replace("\"",""));
        //    }
        //    var userLogin = LoginAdmin.AdminLogin();
        //    if ((!(FileUploadJquery == null)) && (!(FileUploadJquery.PostedFile == null)) &&
        //                   !string.IsNullOrEmpty(FileUploadJquery.PostedFile.FileName))
        //    {
        //        try
        //        {
        //            KhieuNai_FileDinhKemInfo fileInfo = new KhieuNai_FileDinhKemInfo();
        //            fileInfo.TenFile = Path.GetFileName(FileUploadJquery.PostedFile.FileName);
        //            fileInfo.LoaiFile = Path.GetExtension(FileUploadJquery.PostedFile.FileName);
        //            fileInfo.KichThuoc = FileUploadJquery.PostedFile.ContentLength / 1024;

        //            var strFileName = KhieuNai_FileDinhKemImpl.UploadFile(FileUploadJquery, khieuNaiId, Config.PathUploadFile);

        //            fileInfo.KhieuNaiId = khieuNaiId;
        //            fileInfo.GhiChu = "File KH gửi";
        //            fileInfo.Status = (byte)FileDinhKem_Status.File_KH_Gửi;
        //            fileInfo.CUser = userLogin.Username;
        //            fileInfo.URLFile = strFileName;

        //            ServiceFactory.GetInstanceKhieuNai_FileDinhKem().Add(fileInfo);
        //            strValues = "1";
        //        }
        //        catch
        //        {
        //            return "-1";
        //        }

        //    }
        //    return strValues;
        //}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            var values = UploadFile();
            if (values == "1")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Lưu thành công.','info');", true);
            }
            else
            {
                if (values == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onloadMessage", "MessageAlert.AlertNormal('Upload file bị lỗi.','warning');", true);
                }
            }
           
        }

        
        
    }
}