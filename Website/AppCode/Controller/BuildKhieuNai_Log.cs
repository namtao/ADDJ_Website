using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.Admin;
using AIVietNam.GQKN.Entity;
using System;

namespace Website.AppCode.Controller
{
	public class BuildKhieuNai_Log
	{
		public static string BuildListKhieuNai_Log(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Log().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListKhieuNai_Log()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceKhieuNai_Log().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

        public static void LogKhieuNai(int khieuNaiId, string thaotac, string TruongThayDoi = "", string GiaTriCu = "", string GiaTriMoi = "", int archiveId = 0)
        {
            try
            {
                var userLogin = LoginAdmin.AdminLogin();
                var item = new KhieuNai_LogInfo();
                item.PhongBanId = LoginAdmin.AdminLogin().PhongBanId;
                item.KhieuNaiId = khieuNaiId;
                item.CUser = userLogin.Username;
                item.GiaTriCu = GiaTriCu;
                item.GiaTriMoi = GiaTriMoi;
                item.TruongThayDoi = TruongThayDoi;
                item.ThaoTac = thaotac;

                ServiceFactory.GetInstanceKhieuNai_Log(archiveId).Add(item);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
        }

	}
}

