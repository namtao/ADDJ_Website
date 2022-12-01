using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ADDJ.Core;
using ADDJ.Entity;

namespace Website.AppCode
{
    public class PhongBanHelper
    {
        #region EnumComponents
        public PhongBanHelper() { }

        public void BindPhongBanTrangThai(DropDownList ddl, string header, string selected)
        {
            ddl.Items.Clear();
            List<PhongBan_TrangThai> lstObjType = Enum.GetValues(typeof(PhongBan_TrangThai)).Cast<PhongBan_TrangThai>().ToList();
            foreach (PhongBan_TrangThai item in lstObjType)
            {
                string name = item.Name();
                int value = (int)item;
                ddl.Items.Add(new ListItem(name, value.ToString()));
            }
            if (!string.IsNullOrEmpty(header)) ddl.Items.Insert(0, new ListItem(header, string.Empty));
            if (!string.IsNullOrEmpty(selected)) ddl.SelectedValue = selected;
        }

        public string GetNameFromId(object id)
        {
            List<PhongBan_TrangThai> lstObjType = Enum.GetValues(typeof(PhongBan_TrangThai)).Cast<PhongBan_TrangThai>().ToList();
            foreach (PhongBan_TrangThai item in lstObjType)
            {
                if (Convert.ToInt32(id) == (int)item)
                {
                    return item.Name();
                }
            }
            return string.Empty;
        }
        #endregion
    }
}