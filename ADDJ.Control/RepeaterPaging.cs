using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VnptNet.Control
{
    [DefaultProperty("TotalItems")]
    [ToolboxData("<{0}:RepeaterPaging runat=server></{0}:RepeaterPaging>")]
    public class RepeaterPaging : Repeater
    {
        private int _TotalItems = 0;
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("0")]
        [Localizable(true)]
        [Description("Tổng số bản ghi")]
        public int TotalItems
        {
            get
            {
                return _TotalItems;
            }

            set
            {
                _TotalItems = value;
            }
        }
        private int _PageSize = 5;
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("0")]
        [Localizable(true)]
        [Description("Tổng số bản ghi")]
        public int PageSize
        {
            get
            {
                return _PageSize;
            }

            set
            {
                _PageSize = value;
            }
        }
    }
}
