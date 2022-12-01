using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ADDJ.Entity;

namespace ADDJ.Core
{
    public class EnumDonViQuanLyHelper
    {
        public void Bind2Control(DropDownList ddl, string header = "", string selected = "")
        {
            ddl.Items.Clear();
            List<DonViQuanLy> lstObjType = Enum.GetValues(typeof(DonViQuanLy)).Cast<DonViQuanLy>().ToList();
            foreach (DonViQuanLy item in lstObjType.OrderBy(v => v.ThuTu()))
            {
                string name = item.Name();
                int value = (int)item;
                ddl.Items.Add(new ListItem(name, value.ToString()));
            }
            if (!string.IsNullOrEmpty(header)) ddl.Items.Insert(0, new ListItem(header, "0"));
            if (!string.IsNullOrEmpty(selected)) ddl.SelectedValue = selected;
        }
        public void Bind2Control(CheckBoxList cbl, params int[] checkValue)
        {
            cbl.Items.Clear();
            List<DonViQuanLy> lstObjType = Enum.GetValues(typeof(DonViQuanLy)).Cast<DonViQuanLy>().ToList();
            foreach (DonViQuanLy item in lstObjType.OrderBy(v => v.ThuTu()))
            {
                string name = item.Name();
                int value = (int)item;
                cbl.Items.Add(new ListItem(name, value.ToString()));
            }
            // Selected mặc định
            if (checkValue.Length > 0)
            {
                foreach (ListItem item in cbl.Items)
                {
                    if (checkValue.Count(v => v.ToString() == item.Value) > 0) item.Selected = true;
                }
            }

        }
        public string GetNameByObject(DonViQuanLy value)
        {
            try
            {
                List<DonViQuanLy> lst = Enum.GetValues(typeof(DonViQuanLy)).Cast<DonViQuanLy>().ToList();
                return lst.Single(v => (int)v == (int)value).Name();
            }
            catch
            {
                return "Không xác định";
            }
        }
        public string GetNameByObject(int value)
        {
            try
            {
                List<DonViQuanLy> lst = Enum.GetValues(typeof(DonViQuanLy)).Cast<DonViQuanLy>().ToList();
                return lst.Single(v => (int)v == value).Name();
            }
            catch
            {
                return "Không xác định";
            }
        }
    }
}