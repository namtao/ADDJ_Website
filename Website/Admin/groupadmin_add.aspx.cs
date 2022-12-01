using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


using System.Collections.Generic;
using System.Text.RegularExpressions;
using AIVietNam.Core;


using AIVietNam.Admin;
using Website.AppCode;
using AIVietNam.GQKN.Entity;
using AIVietNam.GQKN.Impl;

public partial class admin_groupadmin_add : Website.AppCode.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                if (Utility.IsInteger(Request.QueryString["ID"]))
                {                    
                    EditData();                    
                }
            }


        }


        lblMsg.Text = "";
    }

    private void EditData()
    {
        try
        {
            var item =ServiceFactory.GetInstanceNguoiSuDung_Group().GetInfo(Convert.ToInt32(Request.QueryString["ID"]));
            if (item == null)
            {
                Response.Redirect(Config.PathError, false);
                return;
            }
            else
            {
                txtName.Text = item.Name;
                txtDesc.Text = item.Description;
                chkStatus.Checked = item.Status == 1;
            }
        }
        catch
        {
            Response.Redirect(Config.PathError, false);
            return;
        }
    }
 
    protected void linkbtnSubmit_Click(object sender, EventArgs e)
    {
        if (!UserRightImpl.CheckRightAdminnistrator_NoCache().UserEdit)
        {
            Response.Redirect(Config.PathNotRight, false);
            return;
        }

        try
        {
            NguoiSuDung_GroupInfo item = new NguoiSuDung_GroupInfo();

            item.Name = txtName.Text.Trim();

            item.Description = txtDesc.Text;
            item.Status = chkStatus.Checked ? (short)1 : (short)0;

            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                item.Id = Convert.ToInt32(Request.QueryString["ID"]);
                ServiceFactory.GetInstanceNguoiSuDung_Group().Update(item);
            }
            else
            {
                ServiceFactory.GetInstanceNguoiSuDung_Group().Add(item);
            }
            NguoiSuDung_GroupImpl.NhomNguoiDung = ServiceFactory.GetInstanceNguoiSuDung_Group().GetList();
            Response.Redirect("groupadmin_manager.aspx", false);
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Tên nhóm đã tồn tại. Bạn hãy chọn một tên nhóm khác";
        }
    }
}
