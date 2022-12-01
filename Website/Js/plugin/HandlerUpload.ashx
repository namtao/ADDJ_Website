<%@ WebHandler Language="C#" Class="HandlerUpload" %>

using System;
using System.Web;
using System.Web.SessionState;
using AIVietNam.Core;
using System.IO;

using Website.AppCode.Controller;

public class HandlerUpload : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        HttpFileCollection fUpload = context.Request.Files;

        if (fUpload.Count == 0)
        {
            context.Response.Write("0");
            return;
        }

        var htf = fUpload[0];

        if (htf.ContentLength <= 0)
        {
            context.Response.Write("0");
            return;
        }

        string filename = Guid.NewGuid() + "_" + htf.FileName;
        //string pathCreate = "";
        string pathSave = "";

        //string nam = DateTime.Now.Year.ToString();
        //string thang = DateTime.Now.Month.ToString();
        //string ngay = DateTime.Now.Day.ToString();

        //pathCreate = nam + "/" + thang + "/" + ngay + "/";
        var obj = AIVietNam.Admin.LoginAdmin.AdminLogin();
        if (obj == null)
        {
            return;
        }
        pathSave = HttpContext.Current.Server.MapPath(Config.PathUpload + obj.ID);
        lock (fUpload)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Upload")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Upload"));
            }
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Upload/Profile")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Upload/Profile"));
            }
            if (!Directory.Exists(pathSave))
            {
                Directory.CreateDirectory(pathSave);
            }

            //string pathImage = Utility.UploadFile(fUpload, pathSave, true, ".jpg;.jpeg;.png;.gif");
            htf.SaveAs(pathSave + filename);
        }

        context.Response.Clear();
        context.Response.Write(Config.ImgPathShow  + filename);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}