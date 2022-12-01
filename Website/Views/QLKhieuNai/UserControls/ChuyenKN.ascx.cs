using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIVietNam.GQKN.Impl;
using AIVietNam.GQKN.Entity;

namespace Website.Views.QLKhieuNai.UserControls
{
    public partial class ChuyenKN : System.Web.UI.UserControl
    {
        PhongBanImpl _PhongBanImpl = new PhongBanImpl();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPhongBan();
            }
        }
        private void LoadPhongBan()
        {
            List<PhongBanInfo> lst = _PhongBanImpl.QLKN_PhongBanGetAll();
            if (lst.Count > 0)
            {
                RadPhongBan.DataSource = lst;
                RadPhongBan.DataTextField = "Name";
                RadPhongBan.DataValueField = "ID";
                RadPhongBan.DataBind();
            }
        }
    }
}