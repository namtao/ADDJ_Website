using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;

namespace Website.AppCode.Controller
{
	public class BuildKhieuNai_Watchers
	{
		public static string BuildListKhieuNai_Watchers(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Watchers().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_Watchers()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Watchers().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_WatchersByKhieuNaiId(int _khieuNaiId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Watchers().GetListByKhieuNaiId(_khieuNaiId);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_WatchersByNguoiSuDungId(int _nguoiSuDungId)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Watchers().GetListByNguoiSuDungId(_nguoiSuDungId);
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

