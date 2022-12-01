using ADDJ.Entity;
using System.Collections.Generic;
using System.Text;

namespace Website.AppCode.Controller
{
    public class BuildLoaiKhieuNai2PhongBan
    {
        public static string BuildListLoaiKhieuNai2PhongBan(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
        {
            StringBuilder sb = new StringBuilder();
            List<ADDJ.Entity.LoaiKhieuNai2PhongBanInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListLoaiKhieuNai2PhongBan()
        {
            StringBuilder sb = new StringBuilder();
            List<LoaiKhieuNai2PhongBanInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetList();
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListLoaiKhieuNai2PhongBanByLoaiKhieuNaiId(int _loaiKhieuNaiId)
        {
            StringBuilder sb = new StringBuilder();
            List<LoaiKhieuNai2PhongBanInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByLoaiKhieuNaiId(_loaiKhieuNaiId);
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        public static string BuildListLoaiKhieuNai2PhongBanByPhongBanId(int _phongBanId)
        {
            StringBuilder sb = new StringBuilder();
            List<LoaiKhieuNai2PhongBanInfo> lst = ServiceFactory.GetInstanceLoaiKhieuNai2PhongBan().GetListByPhongBanId(_phongBanId);
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

