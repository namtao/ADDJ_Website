using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;

namespace Website.AppCode.Controller
{
	public class BuildLoaiPhongBan_ThoiGianXuLyKhieuNai
	{
		public static string BuildListLoaiPhongBan_ThoiGianXuLyKhieuNai(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListLoaiPhongBan_ThoiGianXuLyKhieuNai()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

	}
}

