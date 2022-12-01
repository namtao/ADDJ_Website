using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using Website.AppCode;

namespace Website.Views.KhieuNai.UC
{
    public partial class ucFileDinhKem_Thieu : System.Web.UI.UserControl
    {
        public string username = "";
        public int KhieuNaiId { get; set; }
        private Boolean IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Form.Enctype = "multipart/form-data";
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            var obj_admin = LoginAdmin.AdminLogin();
            username = obj_admin.Username;

            // KieuNaiId test
            //KhieuNaiId = 3;
            if (!IsPostBack)
            {
                ViewState["FileDinhKem"] = System.Guid.NewGuid().ToString();
                Session["FileDinhKem"] = ConvertUtility.ToString(ViewState["FileDinhKem"]);
                FillFileDinhKemGrid();
            }
            else
            {
                if (ConvertUtility.ToString(ViewState["FileDinhKem"]) != ConvertUtility.ToString(Session["FileDinhKem"]))
                    IsPageRefresh = true;
                Session["FileDinhKem"] = System.Guid.NewGuid().ToString();
                ViewState["FileDinhKem"] = Session["FileDinhKem"];
            }
        }

        private void FillFileDinhKemGrid()
        {
            string whereClause = string.Format("KhieuNaiId = {0} AND Status = {1}", KhieuNaiId, (byte)FileDinhKem_Status.File_GQKN_Gửi);
            List<KhieuNai_FileDinhKemInfo> objFileDinhKemList = new List<KhieuNai_FileDinhKemInfo>();
            objFileDinhKemList = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListDynamic("", whereClause, "");
            gvFileDinhKem.DataSource = objFileDinhKemList;
            gvFileDinhKem.DataBind();
        }
        protected void gvFileDinhKem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }
        protected void gvFileDinhKem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!IsPageRefresh) // check that page is not refreshed by browser.
            {
                if (e.CommandName.Equals("Insert"))
                {
                    try
                    {
                        var ghichu = ConvertUtility.ToString(((TextBox)gvFileDinhKem.FooterRow.FindControl("txtGhiChu")).Text);
                        var fileDinhKem = (FileUpload)gvFileDinhKem.FindControl("FileUploadJquery");
                        File_Add(ghichu, fileDinhKem.PostedFile); 
                        FillFileDinhKemGrid();
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                    }
                }
                else if (e.CommandName.Equals("emptyInsert"))
                {
                    try
                    {
                        GridViewRow emptyRow = gvFileDinhKem.Controls[0].Controls[0] as GridViewRow;
                        var ghichu = ConvertUtility.ToString(((TextBox)emptyRow.FindControl("txtGhiChu")).Text);
                        var fileDinhKem = emptyRow.FindControl("FileUploadJquery") as FileUpload;
                        File_Add(ghichu, fileDinhKem.PostedFile);
                        FillFileDinhKemGrid();
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                    }
                }
            }
            else
                FillFileDinhKemGrid();
        }
        protected void gvFileDinhKem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvFileDinhKem.EditIndex = e.NewEditIndex;
            FillFileDinhKemGrid();
        }
        protected void gvFileDinhKem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var obj = ServiceFactory.GetInstanceKhieuNai_FileDinhKem();
                KhieuNai_FileDinhKemInfo eInfo = new KhieuNai_FileDinhKemInfo();
                eInfo.Id = Convert.ToInt64(gvFileDinhKem.DataKeys[e.RowIndex].Values[0].ToString());
                eInfo.GhiChu = ConvertUtility.ToString((((TextBox)gvFileDinhKem.Rows[e.RowIndex].FindControl("txtGhiChu")).Text).Trim());

                obj.Update(eInfo);
                gvFileDinhKem.EditIndex = -1;
                FillFileDinhKemGrid();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }
        protected void gvFileDinhKem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvFileDinhKem.EditIndex = -1;
            FillFileDinhKemGrid();
        }
        protected void gvFileDinhKem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var obj = ServiceFactory.GetInstanceKhieuNai_FileDinhKem();
            KhieuNai_FileDinhKemInfo eInfo = new KhieuNai_FileDinhKemInfo();
            try
            {
                var item = obj.GetInfo(Convert.ToInt32(gvFileDinhKem.DataKeys[e.RowIndex].Values[0].ToString()));
                var username_add = item.CUser;
                if (ConvertUtility.ToString(username_add) == username)
                {
                    int ID = Convert.ToInt32(gvFileDinhKem.DataKeys[e.RowIndex].Values[0].ToString());
                    obj.Delete(ID);
                    // Delete a file by using File class static method... 
                    if (System.IO.File.Exists(Server.MapPath(item.URLFile)))
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath(item.URLFile));
                        }
                        catch (System.IO.IOException ex)
                        {
                            Utility.LogEvent(ex);
                            return;
                        }
                    }
                    FillFileDinhKemGrid();
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvFileDinhKem.PageIndex = e.NewPageIndex;
            FillFileDinhKemGrid();
        }

        protected void gvFileDinhKem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        //Upload file and insert db
        private void File_Add(string strGhiChu, HttpPostedFile file1)
        {
            string fileUrl = "";
            string tenthumuc = ConvertUtility.ToString(KhieuNaiId);

            var thumuc0 = (int)(KhieuNaiId / 100000);
            var thumuc1 = (int)(KhieuNaiId / 10000);
            var thumuc2 = (int)(KhieuNaiId / 1000);

            //Config.PathUploadFile
            string caythumuc = thumuc0 + "\\" + thumuc1 + "\\" + thumuc2 + "\\" + KhieuNaiId;
            string duongdan = thumuc0 + "/" + thumuc1 + "/" + thumuc2 + "/" + KhieuNaiId;
            Utility.CreateFolder(caythumuc);
            string fileName1 = "";
            string extfile = "";

            string ngaythang = (DateTime.Now).ToString("ddMMyyyy");
            string giophut = (DateTime.Now).ToString("HHmmss");

            var obj = ServiceFactory.GetInstanceKhieuNai_FileDinhKem();
            KhieuNai_FileDinhKemInfo eInfo = new KhieuNai_FileDinhKemInfo();

            //for (int fileCount = 0; fileCount < uploads1.Count; fileCount++)
            //{
                //if (fileCount < uploads1.Count)
                //{
                    HttpPostedFile uploadedFile = file1;
                    //fileName1 = Path.GetFileName(uploadedFile.FileName);
                    extfile = Path.GetExtension(uploadedFile.FileName).ToLower();
                    if (uploadedFile.ContentLength > 0)
                    {
                        try
                        {
                            //var extfile = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf(".") + 1);
                            var tenfilefile = uploadedFile.FileName.Substring(0, uploadedFile.FileName.LastIndexOf("."));
                            if (CheckFileType(extfile))
                            {
                                fileName1 = tenfilefile + "_" + ngaythang + "_" + giophut + extfile.ToString();
                                fileUrl = duongdan + "/" + fileName1;
                                uploadedFile.SaveAs(Server.MapPath(fileUrl));

                                eInfo.KichThuoc = GetFileSizeOnDisk(Server.MapPath(fileUrl)) / 1024;
                                eInfo.TenFile = tenfilefile;
                                eInfo.KhieuNaiId = KhieuNaiId;
                                eInfo.URLFile = fileUrl;
                                eInfo.LoaiFile = extfile;
                                eInfo.GhiChu = strGhiChu;
                                eInfo.CUser = username;
                                obj.Add(eInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Utility.LogEvent(ex);
                            lblMsg.Text = "Dữ liệu không hợp lệ";
                            return;
                        }
                        finally
                        {
                            lblMsg.Text = "Thêm mới file thành công";
                        }
                    }
                //}
            //}
        }

        public static long GetFileSizeOnDisk(string file)
        {
            FileInfo info = new FileInfo(file);

            uint dummy, sectorsPerCluster, bytesPerSector;
            int result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy);
            if (result == 0) throw new Win32Exception();
            uint clusterSize = sectorsPerCluster * bytesPerSector;
            uint hosize;
            uint losize = GetCompressedFileSizeW(file, out hosize);
            long size;
            size = (long)hosize << 32 | losize;
            return ((size + clusterSize - 1) / clusterSize) * clusterSize;
        }

        [DllImport("kernel32.dll")]
        static extern uint GetCompressedFileSizeW([In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
           [Out, MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        static extern int GetDiskFreeSpaceW([In, MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
           out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
           out uint lpTotalNumberOfClusters);

        //check loai file
        private bool CheckFileType(string fileExtension)
        {
            //string fileExtension = Path.GetExtension(filename).ToLower();
            bool kt = false;
            switch (fileExtension)
            {
                case ".pdf":
                    kt = true;
                    break;

                case ".doc":
                    kt = true;
                    break;

                case ".docx":
                    kt = true;
                    break;

                case ".xls":
                    kt = true;
                    break;

                case ".xlsx":
                    kt = true;
                    break;

                case ".ppt":
                    kt = true;
                    break;

                case ".pptx":
                    kt = true;
                    break;

                case ".zip":
                    kt = true;
                    break;

                case ".rar":
                    kt = true;
                    break;

                case ".jpg":
                    kt = true;
                    break;
                case ".7z":
                    kt = true;
                    break;
            }
            return kt;
        }
    }
}