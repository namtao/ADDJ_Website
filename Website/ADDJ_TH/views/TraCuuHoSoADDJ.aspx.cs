using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.ADDJ_TH.entity;
using Website.HTHTKT;

namespace Website.ADDJ_TH.views
{
    public partial class TraCuuHoSoADDJ : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }
        public int PageSize
        {
            get
            {
                return (base.ViewState["PageSize"] != null) ? (int)base.ViewState["PageSize"] : 10;
            }
            set
            {
                base.ViewState["PageSize"] = value;
            }
        }
        public int CurrentPage
        {
            get
            {
                return (base.ViewState["CurrentPage"] != null) ? (int)base.ViewState["CurrentPage"] : 0;
            }
            set
            {
                base.ViewState["CurrentPage"] = value;
            }
        }
        public int RowCount
        {
            get { return (base.ViewState["RowCount"] != null) ? (int)base.ViewState["RowCount"] : 0; }
            set { base.ViewState["RowCount"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterRequiresControlState(ASPxPager1);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        //http://stackoverflow.com/questions/21537511/sql-server-query-with-pagination-and-count
        public void BindData()
        {
            int count = 0;
            //DataTable dtSource = DBUtils.GetDataPaging(PageSize, CurrentPage, ref count);
            var lstl = GetDataPaging(PageSize, CurrentPage);
            this.RowCount = lstl[0].ToltalRecords.Value;
            ASPxPager1.ItemCount = lstl[0].ToltalRecords.Value;
            ASPxPager1.ItemsPerPage = this.PageSize;
            grvCustomer.DataSource = lstl;
            grvCustomer.DataBind();
        }
        protected void ASPxPager1_PageIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = ASPxPager1.PageIndex;
            BindData();
        }

        public List<mucluchoso> GetDataPaging(int pagesize, int currentpage)
        {
            var lsthh = new List<mucluchoso>();
            try
            {
                using (var ctx = new ADDJContext())
                {
                    //var strSQl = "select * from HT_NODE_LUONG_HOTRO0 where nguoitao='" + loginInfo.Username + "' order by ngaytao desc";
                    var strSQl = string.Format(@";with query as
(
  select *,ROW_NUMBER() OVER(ORDER BY ID ASC) as line from dbo.mucluchoso
) 
--order by clause is required to use offset-fetch
select query.id,
       query.hopso ,
       query.hoso_so ,
       query.trichyeunoidung ,
       query.soto ,
       query.thoigian ,
       query.nam ,
       query.mlhs ,
       query.line 
	   ,tCountOrders.CountOrders ToltalRecords 
from query CROSS JOIN (SELECT Count(*) AS CountOrders FROM query) AS tCountOrders
order by query.ID 
offset (({0} - 1) * {1}) rows
fetch next {2} rows only", currentpage + 1, pagesize, pagesize);
                    var lstHoTro = ctx.Database.SqlQuery<mucluchoso>(strSQl);
                    lsthh = lstHoTro.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lsthh;
        }
    }
}