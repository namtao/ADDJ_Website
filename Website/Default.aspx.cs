using System;
using System.Web.UI;
using ADDJ.Admin;
using System.Web.Services;
using Website.HeThongHoTro;
using Website.HeThongHoTro.Dashboards;

public partial class Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginAdmin.IsLoginAdmin();
    }


    [WebMethod]
    public static string saveFileAttach(string guid)
    {
        return Website.ADDJ_TH.views.WebUserControl1_q10.saveFileAttach(guid.Replace("\"","")); // call the method which 
                                         // exists in the user control
    }
}
