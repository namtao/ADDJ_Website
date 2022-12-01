using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Website.AppCode;

namespace Website.Views.ChiTietKhieuNai.UC
{
    public partial class ucQuaTrinhKhieuNai : System.Web.UI.UserControl
    {
        public int KhieuNaiId { get; set; }

        private List<PhongBanInfo> _listPhongBan = null;

        private DateTime _nullDateTime = new DateTime(1900, 1, 1);
        
        private int ArchiveId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {           
            KhieuNaiId = ConvertUtility.ToInt32(Request.QueryString["MaKN"]);
            ArchiveId = ConvertUtility.ToInt32(Request.QueryString["archive"]);
            _listPhongBan = ServiceFactory.GetInstancePhongBan().GetList();
        }

        public void FillQuaTrinhKhieuNaiGrid()
        {
            string whereClause = "KhieuNaiId = " + KhieuNaiId;
            List<KhieuNai_ActivityInfo> listKhieuNaiActivity = new List<KhieuNai_ActivityInfo>();
            listKhieuNaiActivity = ServiceFactory.GetInstanceKhieuNai_Activity(ArchiveId).GetListDynamic("", whereClause, "CDate DESC");
            gvQuaTrinhKhieuNai.DataSource = listKhieuNaiActivity;
            gvQuaTrinhKhieuNai.DataBind();

            if(gvQuaTrinhKhieuNai.Rows.Count > 0)
            {
                //if (gvQuaTrinhKhieuNai.Rows[0].Cells[2].Text != Enum.GetName(typeof(KhieuNai_Actitivy_HanhDong), KhieuNai_Actitivy_HanhDong.Đóng_KN).Replace("_", " ")
                //    && DateTime.Now >= ConvertUtility.ToDateTime(gvQuaTrinhKhieuNai.Rows[0].Cells[8].Text, "dd/MM/yyyy HH:mm", _nullDateTime)
                //    && gvQuaTrinhKhieuNai.PageIndex == 0)
                //{
                //    gvQuaTrinhKhieuNai.Rows[0].Cells[9].Text = "Quá hạn";
                //}   

                for(int i=0;i<gvQuaTrinhKhieuNai.Rows.Count;i++)
                {
                    if (i == 0 && gvQuaTrinhKhieuNai.PageIndex == 0)
                    {
                        if (gvQuaTrinhKhieuNai.Rows[0].Cells[2].Text != Enum.GetName(typeof(KhieuNai_Actitivy_HanhDong), KhieuNai_Actitivy_HanhDong.Đóng_KN).Replace("_", " ")
                            && DateTime.Now >= ConvertUtility.ToDateTime(gvQuaTrinhKhieuNai.Rows[0].Cells[8].Text, "dd/MM/yyyy HH:mm", _nullDateTime))
                        {
                            gvQuaTrinhKhieuNai.Rows[0].Cells[9].Text = "Quá hạn";
                        }   
                    }
                    else
                    {
                        if (ConvertUtility.ToDateTime(gvQuaTrinhKhieuNai.Rows[i].Cells[7].Text, "dd/MM/yyyy HH:mm", _nullDateTime) >= ConvertUtility.ToDateTime(gvQuaTrinhKhieuNai.Rows[i].Cells[8].Text, "dd/MM/yyyy HH:mm", _nullDateTime))
                        {
                            gvQuaTrinhKhieuNai.Rows[i].Cells[9].Text = "Quá hạn";
                        }
                    }                    
                }
            }
        }

        private void PageIndexChanging(GridViewPageEventArgs e)
        {
            gvQuaTrinhKhieuNai.PageIndex = e.NewPageIndex;
            FillQuaTrinhKhieuNaiGrid();
        }

        protected void gvQuaTrinhKhieuNai_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PageIndexChanging(e);
        }

        protected void gvQuaTrinhKhieuNai_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string hanhDong = e.Row.Cells[2].Text;
                e.Row.Cells[0].Text = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(ConvertUtility.ToInt32(e.Row.Cells[0].Text));
                e.Row.Cells[2].Text = Enum.GetName(typeof(KhieuNai_Actitivy_HanhDong), ConvertUtility.ToInt32(e.Row.Cells[2].Text)).Replace("_", " ");
                e.Row.Cells[3].Text = ServiceFactory.GetInstancePhongBan().GetNamePhongBan(ConvertUtility.ToInt32(e.Row.Cells[3].Text));

                if(ConvertUtility.ToDateTime(e.Row.Cells[4].Text, "dd/MM/yyyy HH:mm", _nullDateTime) >= ConvertUtility.ToDateTime(e.Row.Cells[8].Text, "dd/MM/yyyy HH:mm", _nullDateTime))                    
                {
                    e.Row.Cells[9].Text = "Quá hạn";
                }

                if (ConvertUtility.ToDateTime(e.Row.Cells[6].Text, "dd/MM/yyyy HH:mm", _nullDateTime) <= _nullDateTime
                    || ConvertUtility.ToDateTime(e.Row.Cells[6].Text, "dd/MM/yyyy HH:mm", _nullDateTime).Year >= DateTime.MaxValue.Year)
                {
                    e.Row.Cells[6].Text = "";
                }  
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
            
            
        }
    }
}