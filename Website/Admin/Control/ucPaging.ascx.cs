using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using VnptNet.Control;

namespace VTN_QLTS.AppCode
{
    public partial class ucPaging : System.Web.UI.UserControl
    {
        private RepeaterPaging _repeater;
        #region Các biến xử dụng
        private int _RowPerPage;
        private int _CurrentPage = 1;
        private string _LinkPage;
        private int _TotalPage = 0;
        private string _LinkPageExt;
        private int _TotalRecords = 0;
        #endregion

        #region Các thuộc tính
        public int RowPerPage
        {
            get { return _RowPerPage; }
            set { _RowPerPage = value; }
        }

        public int TotalPage
        {
            get { return _TotalPage; }
            set { _TotalPage = value; }
        }
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }

        public string LinkPage
        {
            get { return _LinkPage; }
            set { _LinkPage = value; }
        }
        public string LinkPageExt
        {
            get { return _LinkPageExt; }
            set { _LinkPageExt = value; }
        }
        public int TotalRecords
        {
            get { return _TotalRecords; }
            set { _TotalRecords = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Control c = Parent;
            while (c != null)
            {                
                if (c is RepeaterPaging)
                {
                    _repeater = (RepeaterPaging)c;
                    LoadPagingRepeaterInfo();
                    break;
                }

                c = c.Parent;
            }
        }

        private void LoadPagingRepeaterInfo()
        {
            if (_repeater != null)
            {
                if (!string.IsNullOrEmpty(Request["Page"]))
                {
                    Int32.TryParse(Request["Page"], out _CurrentPage);
                }
                _TotalRecords = _repeater.TotalItems;
                _RowPerPage = _repeater.PageSize;
                _TotalPage = (_TotalRecords % _RowPerPage == 0) ? _TotalRecords / _RowPerPage : ((_TotalRecords - (_TotalRecords % _RowPerPage)) / _RowPerPage) + 1;
            }
        }

        protected void TextBoxPage_TextChanged(object sender, EventArgs e)
        {
            if (_repeater == null)
            {
                return;
            }
            int page;
            if (int.TryParse(TextBoxPage.Text.Trim(), out page))
            {
                if (page <= 0)
                {
                    page = 1;
                }
                if (page > TotalPage)
                {
                    page = TotalPage;
                }
                TotalPage = page - 1;
            }
            TextBoxPage.Text = (CurrentPage + 1).ToString(CultureInfo.CurrentCulture);
        }

        protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_repeater == null)
            {
                return;
            }
            DropDownList dropdownlistpagersize = (DropDownList)sender;
            RowPerPage = Convert.ToInt32(dropdownlistpagersize.SelectedValue, CultureInfo.CurrentCulture);
            int pageindex = CurrentPage;            
        }
    }
}