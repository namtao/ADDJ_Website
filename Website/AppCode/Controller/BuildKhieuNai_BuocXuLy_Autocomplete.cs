using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.Admin;

namespace Website.AppCode.Controller
{
	public class BuildKhieuNai_BuocXuLy_Autocomplete
	{
		public static string BuildListKhieuNai_BuocXuLy_Autocomplete(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_BuocXuLy_Autocomplete()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

        public static string BuildWord()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            var lst = ServiceFactory.GetInstanceKhieuNai_BuocXuLy_Autocomplete().GetListDynamic("Name","PhongBanId=" + LoginAdmin.AdminLogin().PhongBanId,"");
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.AppendFormat("'{0}',", item.Name);
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

	}
}

