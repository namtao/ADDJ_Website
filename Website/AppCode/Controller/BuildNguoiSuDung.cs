using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using ADDJ.Core;
using ADDJ.Entity;
using ADDJ.Admin;
using System;

namespace Website.AppCode.Controller
{
	public class BuildNguoiSuDung
	{
		public static string BuildListNguoiSuDung(string selectQuery, string whereClause, string orderBy, int pageIndex, int pageSize, ref int total)
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged(selectQuery, whereClause, orderBy, pageIndex, pageSize, ref total);
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

		public static string BuildListNguoiSuDung()
		{
			StringBuilder sb = new StringBuilder();
			var lst = ServiceFactory.GetInstanceNguoiSuDung().GetList();
			if (lst != null && lst.Count > 0)
			{
				foreach (var item in lst)
				{
					sb.Append("");
				}
			}
			return sb.ToString();
		}

        public static string BindData(int DoiTacId, string FillterType, string Keyword, int pageIndex)
        {
            var admin = LoginAdmin.AdminLogin();
            if (admin == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            string whereClause = string.Empty;
            List<NguoiSuDungInfo> lst = null;

            int pageSize = Config.RecordPerPage;
            int totalRecord = 0;

            whereClause = "TrangThai <> 2 ";

            string orderClause = "LDate DESC";
            if (DoiTacId == 0)
            {
                if (FillterType.Equals("0") || string.IsNullOrEmpty(Keyword))
                    lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged("", whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                else
                {
                    whereClause += " AND " + FillterType + " like '%" + Keyword + "%'";
                    lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged("", whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                }
            }
            else
            {
                whereClause += " and DoiTacId=" + DoiTacId;
                if (FillterType.Equals("0") || string.IsNullOrEmpty(Keyword))
                    lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged("", whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                else
                {
                    whereClause += " AND " + FillterType + " like '%" + Keyword + "%'";
                    lst = ServiceFactory.GetInstanceNguoiSuDung().GetPaged("", whereClause, orderClause, pageIndex, pageSize, ref totalRecord);
                }
            }
            if (lst == null)
                return string.Empty;

            sb.Append("<table class=\"nobor\"><tbody><tr><td>Có " + totalRecord + " người sử dụng được tìm thấy.</td><td style='text-align:right'><input id='btUpdateTrangThai' onclick='NguoiSuDung.UpdateTrangThai();' type='button' value='Cập nhật' class='login_btn' /></td></tr><tr><td>&nbsp;</td></tr></tbody></table>");
            sb.Append(@"<table class='myTbl'><tr>
                        <th>
                            STT
                        </th>                       
                        <th>
                            Tên truy cập
                        </th>
                        <th>
                            Tên đầy đủ
                        </th>
                        <th>
                            Nhóm người dùng
                        </th>
                        <th>
                            Đối tác
                        </th>
                        <th>
                            Di động
                        </th>
                        <th>
                            Trạng thái&nbsp;<input type='checkbox' id='selectall' />
                        </th>
                        <th colspan='2'>
                            Thao tác
                        </th>
                    </tr>");

            if (lst != null)
            {
                int i = (pageIndex - 1) * pageSize;
                if (i < 0)
                    i = 0;
                foreach (var item in lst)
                {

                    if (i % 2 == 0)
                        sb.Append("<tr height='23px' onmouseover=\"this.style.backgroundColor='#f5eeb8'\" onmouseout=\"this.style.backgroundColor='#ffffff'\" style='color: black; background-color: rgb(255, 255, 255);'>");
                    else
                        sb.Append("<tr height='23px' onmouseover=\"this.style.backgroundColor='#f5eeb8'\" onmouseout=\"this.style.backgroundColor='#DEEAF3'\" style='color: black; background-color: rgb(222, 234, 243);'>");

                    i++;
                    sb.AppendFormat("<td align='center'>{0}</td>", i.ToString());
                    sb.AppendFormat("<td>{0}</td>", item.TenTruyCap);
                    sb.AppendFormat("<td>{0}</td>", item.TenDayDu);
                    sb.AppendFormat("<td>{0}</td>", item.NhomNguoiDung);
                    sb.AppendFormat("<td>{0}</td>", item.TenDoiTac);
                    sb.AppendFormat("<td>{0}</td>", item.DiDong);
                    sb.AppendFormat("<td align='center'>{0}&nbsp;&nbsp;", item.TrangThai == 1 ? "Sử dụng" : "Không sử dụng");

                    if (item.Id == admin.Id || item.NhomNguoiDung == admin.NhomNguoiDung)
                    {
                        if (item.TrangThai == 1)
                            sb.AppendFormat("<span class=\"case\" userid='{0}'><input id=\"chkRead_{0}\" disabled=\"disabled\" checked=\"checked\" type=\"checkbox\" name=\"chkRead_{0}\"></span></td>", item.Id);
                        else
                            sb.AppendFormat("<span class=\"case\" userid='{0}'><input id=\"chkRead_{0}\" disabled=\"disabled\" type=\"checkbox\" name=\"chkRead_{0}\"></span></td>", item.Id);

                        sb.Append("<td align='center'></td>");
                        sb.Append("<td align='center'></td>");

                    }
                    else
                    {
                        if (item.TrangThai == 1)
                            sb.AppendFormat("<span class=\"case\" userid='{0}'><input id=\"chkRead_{0}\" checked=\"checked\" type=\"checkbox\" name=\"chkRead_{0}\"></span></td>", item.Id);
                        else
                            sb.AppendFormat("<span class=\"case\" userid='{0}'><input id=\"chkRead_{0}\" type=\"checkbox\" name=\"chkRead_{0}\"></span></td>", item.Id);

                        sb.AppendFormat("<td align='center'><a href='#' title='Sửa' onclick='NguoiSuDung.Edit({0});'><img src='/images/icon2.jpg' /></a></td>", item.Id);
                        sb.AppendFormat("<td align='center'><a href='#' title='Xóa' onclick='NguoiSuDung.Delete({0});'><img src='/images/del.png' /></a></td>", item.Id);
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");

            if (totalRecord > pageSize)
            {
                sb.Append("<div style='padding-top:10px; padding-bottom:30px;'>");
                sb.Append(HtmlUtility.BuildPagerNormal(totalRecord, pageSize, pageIndex, "&DoiTacId=" + DoiTacId + "&Filter=" + FillterType + "&Keyword=" + Keyword, "", "active", 5));
                sb.Append("</div>");
            }
            return sb.ToString();
        }

	}
}

